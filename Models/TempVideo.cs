using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HGT6.Models
{
    public class TempVideo
    {
        public long Id { get; set; }
        public string Path { get; set; }
        public string  UserEmail { get; set; }
        public DateTime Time { get; set; } = DateTime.Now;
    }
}
