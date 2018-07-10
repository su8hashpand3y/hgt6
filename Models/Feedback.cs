using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HGT6.Models
{
    public class Feedback
    {
        public long Id { get; set; }
        public string Message { get; set; }
        public string UserEmail { get; set; }
    }
}
