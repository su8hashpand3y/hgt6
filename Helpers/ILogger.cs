using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HGT6.Helpers
{
    public interface ILogger
    {
        void Log(string Message, string exception = "", string innerException = "");
    }
}
