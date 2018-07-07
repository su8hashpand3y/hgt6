using System;
using System.Collections.Generic;
using HGT6.Models;
using HGT6.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HGT6.Helpers;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace HGT6.Controllers
{
public class VideoController : Controller
    {
        private IServiceProvider services { get; }
        public VideoController(IServiceProvider services)
        {
            this.services = services;
        }
        // GET: /<controller>/ 
        [HttpGet()]
        public IActionResult GetVideo(long id)
        {
            VideoViewModel result = null; ;
            var context = this.services.GetService(typeof(HGTDbContext)) as HGTDbContext;
           
            var video = context.Videos.Include(x=>x.HGTUser).FirstOrDefault(x=>x.ID == id);
            if(video != null)
            {
                HGTUser user = video.HGTUser;
                result = new VideoViewModel
                {
                    VideoUrl = video.VideoUrl,
                    PosterUrl = video.PosterUrl,
                    Format = video.Format,
                    Description = video.Description,
                    Title = video.Title,
                    UserFirstName = user?.FirstName,
                    UserLastName = user?.LastName,
                    UserDistrict = user?.District,
                    UserTown = user?.Town,
                    UserId = user?.Id,
                    NumberOfLikes = video.Likes,
                    VideoId = video.ID,
                    NumberOfViews = video.Views,
                };

                try
                {
                    var userId = HttpContext.GetUserID();
                    if(context.Likes.Any(x=>x.UserId == userId && x.VideoId == video.ID))
                    {
                        result.IsLikedByMe = true;
                    }
                }
                catch
                {
                }

                try
                {
                    video.Views++;
                    context.SaveChangesAsync();
                }
                catch 
                {
                }

                return Ok(new ServiceTypedResponse<VideoViewModel> { Status = "good", Message = result });
            }

            return Ok(new ServiceResponse { Status = "bad", Message = "Cant get this video" });
        }

        [HttpGet()]
        public IActionResult GetComments(long id)
        {
            var result = new List<CommentViewModel>();
            var context = this.services.GetService(typeof(HGTDbContext)) as HGTDbContext;
            var comments = context.Comments.Include(x=>x.HGTUser).Where(x => x.VideoId == id).OrderByDescending(x=>x.ID).ToList();
            comments.ForEach(x => result.Add(new CommentViewModel { CommentText= x.CommentText,UserFirstName= x.HGTUser?.FirstName }));
            return Ok(new ServiceTypedResponse<List<CommentViewModel>> { Status = "good", Message = result });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Like([FromBody]long videoId)
        {
            var context = this.services.GetService(typeof(HGTDbContext)) as HGTDbContext;
            var likedVideo = context.Videos.FirstOrDefault(x => x.ID == videoId);
            if (likedVideo != null)
            {
                likedVideo.Likes++;
                var userId = HttpContext.GetUserID();
                var liked = context.Likes.FirstOrDefault(x => x.VideoId == videoId && x.UserId == userId);
                if (liked != null)
                {
                    var like = new Like { VideoId = videoId, UserId = HttpContext.GetUserID() };
                    context.Likes.Add(like);
                    context.SaveChanges();
                    return Ok(like);
                }
            }

            return BadRequest();
        }


        [HttpPost]
        [Authorize]
        public IActionResult Comment(long VideoId, string CommentText)
        {
            var context = this.services.GetService(typeof(HGTDbContext)) as HGTDbContext;
            var likedVideo = context.Videos.FirstOrDefault(x => x.ID == VideoId);
            if (likedVideo != null)
            {
                likedVideo.Comments++;
                var comment = new Comment { VideoId = VideoId, HGTUserID = HttpContext.GetUserID(), CommentText = CommentText };
                context.Comments.Add(comment);
                context.SaveChanges();
                return Ok(comment);
            }

            return BadRequest();
        }

        [HttpPost]
        public IActionResult ReportVideo(long videoId)
        {
            var context = this.services.GetService(typeof(HGTDbContext)) as HGTDbContext;
            var video = context.Videos.FirstOrDefault(x => x.ID == videoId);
            if (video != null)
            {
                video.SpamCount++;
                if(video.SpamCount > 100 && video.SpamCount<999999999)
                {
                    video.IsReviewed = false;
                }
                context.SaveChanges();
            }

            return Ok();
        }
    }
}