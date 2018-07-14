using HGT6.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HGT6.Helpers
{
    public class Logger : ILogger
    {

        HGTDbContext context;
        public Logger(IServiceProvider services)
        {
            this.context = services.GetService(typeof(HGTDbContext)) as HGTDbContext;
        }
        public void Log(string message,string exception= "",string innerException = "")
        {
            try
            {
                ErrorLog el = new ErrorLog { Message = message, Exception = exception, InnerException = innerException };
                this.context.Errors.Add(el);
                this.context.SaveChanges();
            }
            catch(Exception e)
            {
                Console.WriteLine("Some Problem Writing the log");
                Console.WriteLine(e.Message);
                Console.WriteLine(e.InnerException);
            }
        }
    }
}
