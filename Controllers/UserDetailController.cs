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
    public class UserDetailController : Controller
    {
        private IServiceProvider services { get; }
        public UserDetailController(IServiceProvider services)
        {
            this.services = services;
        }
        public IActionResult GetUser(string id)
        {
            var context = this.services.GetService(typeof(HGTDbContext)) as HGTDbContext;
            var user = context.Users.FirstOrDefault(x => x.Id == id);
            if (user != null)
            {
                var result = new RegisterViewModel { Age = user.Age, AvatarImage = user.AvatarImage,  District = user.District, Email = user.Email, FirstName = user.FirstName, LastName = user.LastName, Gender = user.Gender, Town = user.Town };
                return Ok(new ServiceTypedResponse<RegisterViewModel> { Status = "good", Message = result });
            }

            return Ok(new ServiceTypedResponse<RegisterViewModel> { Status = "bad", Message = new RegisterViewModel() });
        }

        public IActionResult GetUserVideo(string id)
        {
           var result = new List<VideoViewModel>();
            var context = this.services.GetService(typeof(HGTDbContext)) as HGTDbContext;
            var user = context.Users.Include(x=>x.Videos).FirstOrDefault(x => x.Id == id);
            if (user != null)
            {
                foreach (var video in user.Videos)
                {
                    result.Add(new VideoViewModel
                    {
                        VideoUrl = video.VideoUrl,
                        PosterUrl = video.PosterUrl,
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

            return Ok(new ServiceTypedResponse<List<VideoViewModel>> { Status = "bad", Message = result });   
        }
    }
}