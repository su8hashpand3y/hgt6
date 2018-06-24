using System;

namespace HGT6.Models
{ 
    public class Comment
    {
        public long ID { get; set; }
        public long VideoId { get; set; }
        public string CommentText { get; set; }
        public bool IsDeleted { get; set; }
        public long SpamCount { get; set; }
        public string HGTUserID { get; set; }
        public HGTUser HGTUser { get; set; }
    }
}