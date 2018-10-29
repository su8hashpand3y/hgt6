using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HGT6.Models
{
    public class ErrorLog
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual long Id { get; set; }
        public string Exception { get; set; }
        public string InnerException { get; set; }
        public string Message { get; set; }

        public DateTime Time { get; set; } = DateTime.Now;
    }
}
