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
                    NumberOfViews = video.Views
                };

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
            var comments = context.Comments.Include(x=>x.HGTUser).Where(x => x.VideoId == id).ToList();
            comments.ForEach(x => result.Add(new CommentViewModel { CommentText= x.CommentText,UserFirstName= x.HGTUser?.FirstName }));
            return Ok(new ServiceTypedResponse<List<CommentViewModel>> { Status = "good", Message = result });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Like(long videoId)
        {
            var context = this.services.GetService(typeof(HGTDbContext)) as HGTDbContext;
            var likedVideo = context.Videos.FirstOrDefault(x => x.ID == videoId);
            if (likedVideo != null)
            {
                likedVideo.Likes++;
                var like = new Like { VideoId = videoId, UserId = HttpContext.GetUserID() };
                context.Likes.Add(like);
                context.SaveChanges();
                return Ok(like);
            }

            return BadRequest();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Comment(long videoId,string commentText)
        {
            var context = this.services.GetService(typeof(HGTDbContext)) as HGTDbContext;
            var likedVideo = context.Videos.FirstOrDefault(x => x.ID == videoId);
            if (likedVideo != null)
            {
                likedVideo.Comments++;
                var comment = new Comment { VideoId = videoId, HGTUserID = HttpContext.GetUserID(), CommentText = commentText };
                context.Comments.Add(comment);
                context.SaveChanges();
                return Ok(comment);
            }

            return BadRequest();
        }
    }
}