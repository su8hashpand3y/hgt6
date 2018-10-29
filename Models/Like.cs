
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HGT6.Models
{ 
    public class Like
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual long ID { get; set; }
        public virtual long VideoId { get; set; }
        public virtual string UserId { get; set; }
    }
}