
using HGT6.Models;
using HGT6.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using HGT6.Helpers;

namespace HGT6.Controllers
{
    public class MainController :Controller
    {
        string Default_Poster = "/Media/Got_Talent_logo.jpg";
        private IServiceProvider services { get; }
        private IConfiguration Configuration { get;  }
        private ILogger logger { get; }
        public MainController(IServiceProvider services, IConfiguration configuration,ILogger logger)
        {
            this.services = services;
            this.logger = logger;
        }

        public IActionResult GetVideoList(string type,int skip, int take = 15)
        {
            var result = new List<VideoViewModel>();
            var context = this.services.GetService(typeof(HGTDbContext)) as HGTDbContext;
            IQueryable<VideoInfo> videos;
            if (!String.IsNullOrEmpty(type))
            {
                videos = context.Videos.OrderBy(x=>x.UploadDateTime).Include(x => x.HGTUser).Where(x=> x.IsReviewed != false).Skip(skip).Take(take);
            }
            else
            {
                videos = context.Videos.Include(x => x.HGTUser).Where(x => x.IsReviewed != false).Skip(skip).Take(take);
            }

            foreach (var video in videos)
            {
                HGTUser user = video.HGTUser;
                result.Add(new VideoViewModel
                {
                    VideoUrl = video.VideoUrl,
                    PosterUrl = video.PosterUrl ?? Default_Poster,
                    Description = video.Description,
                    Title = video.Title,
                    UserFirstName = user?.FirstName,
                    UserDistrict = user?.District,
                    UserId = user?.Id,
                    NumberOfLikes = video.Likes,
                    VideoId = video.ID
                });
            }

            return Ok(new ServiceTypedResponse<List<VideoViewModel>> { Status = "good", Message = result });
        }

        public IActionResult SearchVideo(string searchTerm, int skip, int take = 50)
        {
            var result = new List<VideoViewModel>();
            var context = this.services.GetService(typeof(HGTDbContext)) as HGTDbContext;
            IQueryable<VideoInfo> videos;
            videos = context.Videos.Include(x => x.HGTUser).Where(x=>x.IsReviewed !=false  && (x.HGTUser.FirstName.Contains(searchTerm) ||
             x.Description.Contains(searchTerm) || x.Title.Contains(searchTerm))).Skip(skip).Take(take);

            foreach (var video in videos)
            {
                HGTUser user = video.HGTUser;
                result.Add(new VideoViewModel
                {
                    VideoUrl = video.VideoUrl,
                    PosterUrl = video.PosterUrl ?? Default_Poster,
                    Description = video.Description,
                    Title = video.Title,
                    UserFirstName = user?.FirstName,
                    UserDistrict = user?.District,
                    UserId = user?.Id,
                    NumberOfLikes = video.Likes,
                    VideoId = video.ID
                });
            }

            return Ok(new ServiceTypedResponse<List<VideoViewModel>> { Status = "good", Message = result });
        }

        public IActionResult Feedback(string message)
        {
            try
            {
                var context = this.services.GetService(typeof(HGTDbContext)) as HGTDbContext;
                Feedback f = new Feedback { Message = message, UserEmail = HttpContext.GetUserEmail() };
                context.Feedbacks.Add(f);
            }
            catch(Exception e)
            {
                this.logger.Log("GetVideo has Problem", e.Message, e.InnerException?.Message);
            }

            return Ok(new ServiceResponse { Status = "good", Message = "Thank You" });
        }
    }
}