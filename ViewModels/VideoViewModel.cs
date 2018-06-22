using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


namespace HGT6.ViewModels
{
    public class VideoViewModel 
    {
        public long VideoId { get; set; }

        public string VideoUrl { get; set; }
        public string PosterUrl { get; set; }
        public string Format { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public string UserFirstName { get; set; }
        public string UserDistrict { get; set; }
        public long NumberOfLikes { get; set; }
        public long NumberOfComments { get; set; }
    }
}
