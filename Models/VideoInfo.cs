
using System;

namespace HGT6.Models
{ 
     public class VideoInfo
    {
        public long ID { get; set; }
        public string Title { get; set; }
        public string Format { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string VideoUrl { get; set; }
        public string PosterUrl { get; set; }
        public bool IsReviewed { get; set; }
        public DateTime UploadDateTime { get; set; }
        public long Likes { get; set; }
        public long Comments { get; set; }
        public long Views { get; set; }
        public bool  IsFeatured { get; set; }
        public int Donation { get; set; }
        public bool SuperVideo { get; set; }
        public int SpamCount { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsUploaded { get; set; }
        public bool SponseredVideo { get; set; }

        public string HGTUserID { get; set; }
        public HGTUser HGTUser { get; set; }
    }
}