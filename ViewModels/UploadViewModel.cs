
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HGT6.ViewModels
{
    public class UploadInfoViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string VideoUrl { get; set; }
        public string PosterUrl { get; set; }
    }
}