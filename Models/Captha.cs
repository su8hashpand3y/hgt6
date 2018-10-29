using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HGT6.Models
{ 
    public class Captha
    {
     [DatabaseGenerated(DatabaseGeneratedOption.Identity )]
     public virtual int Id { get; set; }
     public string CapthaText { get; set; }
     public string CapthaAnswer { get; set; }
    }
}