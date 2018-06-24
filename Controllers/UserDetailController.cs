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
        public IActionResult GetUser(int id)
        {
            var result = new RegisterViewModel();
            return Ok(new ServiceTypedResponse<RegisterViewModel> { Status = "good", Message = result }); 
        }

        public IActionResult GetUserVideo(int id)
        {
           var result = new List<VideoViewModel>();
           return Ok(new ServiceTypedResponse<List<VideoViewModel>> { Status = "good", Message = result });   
        }
    }
}