using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HGT6.Models
{
    public class ErrorLog
    {
        public long Id { get; set; }
        public string Exception { get; set; }
        public string InnerException { get; set; }
        public string Message { get; set; }

        public DateTime Time { get; set; } = DateTime.Now;
    }
}
