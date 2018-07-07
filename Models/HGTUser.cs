using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace HGT6.Models
{ 
    public class HGTUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string District { get; set; }
        public string Town { get; set; }
        public bool IsVerified { get; set; }
        public string VerificationCode { get; set; } = "Not Available";
        public string Salt { get; set; }
        public string AvatarImage { get; set; }
        public int AllowedSpaceMB { get; set; }
        public long Credits { get; set; }
        public int ReportedSpamCount { get; set; }
        public bool IsDeleted { get; set; }
        public bool SuperUser { get; set; }
        public DateTime LastPassowrdResetTime { get; set; }
        public virtual ICollection<VideoInfo> Videos {get; set;}
        public virtual ICollection<Comment> Comments {get; set; }
    }
}