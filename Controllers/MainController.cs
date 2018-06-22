
using HGT6.Models;
using HGT6.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HGT6.Controllers
{
    public class MainController :Controller
    {
        string Default_Poster = "/Media/Got_Talent_logo.jpg";
        private IServiceProvider services { get; }
        public MainController(IServiceProvider services)
        {
            this.services = services;
        }

        public IActionResult GetVideoList(string type,int skip, int take = 15)
        {
            var result = new List<VideoViewModel>();
            var context = this.services.GetService(typeof(HGTDbContext)) as HGTDbContext;
            IQueryable<VideoInfo> videos;
            if (!String.IsNullOrEmpty(type))
            {
                videos = context.Videos.OrderBy(x=>x.UploadDateTime).Include(x => x.HGTUser).Skip(skip).Take(take);
            }
            else
            {
                videos = context.Videos.Include(x => x.HGTUser).Skip(skip).Take(take);
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

    }
}