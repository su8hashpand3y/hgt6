using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HGT6.Models
{ 
    public class Comment
    {
       [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual long ID { get; set; }
        public virtual long VideoId { get; set; }
        public string CommentText { get; set; }
        public bool IsDeleted { get; set; }
        public long SpamCount { get; set; }
        public virtual string HGTUserID { get; set; }
        public virtual HGTUser HGTUser { get; set; }
    }
}