 
using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HGT6.Models
{ 
    public class HGTDbContext : IdentityDbContext<HGTUser>
    {
        public HGTDbContext(DbContextOptions<HGTDbContext> options) : base(options) { }

        public DbSet<HGTUser> HGTUsers { get; set; }
        public DbSet<VideoInfo> Videos { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Captha> Capthas { get; set; }
        public DbSet<TempVideo> TempVideo { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }

        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    this.Capthas.Add(new Captha { CapthaText = "2+2", CapthaAnswer = "4" });
        //    this.Capthas.Add(new Captha { CapthaText = "yes", CapthaAnswer = "yes" });
        //    this.Capthas.Add(new Captha { CapthaText = "2*3", CapthaAnswer = "6" });
        //    this.Capthas.Add(new Captha { CapthaText = "2+3", CapthaAnswer = "5" });
        //    this.Capthas.Add(new Captha { CapthaText = "go", CapthaAnswer = "go" });
        //    this.Capthas.Add(new Captha { CapthaText = "start", CapthaAnswer = "start" });
        //    this.Capthas.Add(new Captha { CapthaText = "8+1", CapthaAnswer = "9" });
        //    this.Capthas.Add(new Captha { CapthaText = "5+1", CapthaAnswer = "6" });
        //    this.SaveChanges();
        //    base.OnModelCreating(builder);
        //}
    }
}