using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HallOfFame_backend
{
    public class AppSettings
    {
        public Logging Logging { get; set; }
        public ConnectionStrings ConnectionStrings { get; set; }
        public string AllowedHosts { get; set; }
    }

    public class ConnectionStrings
    {
        public string DefaultConnection { get; set; }
    }

    public class Logging
    {
        public LogLevel LogLevel { get; set; }
    }

    public class LogLevel
    {
        public string Default { get; set; }
        public string Microsoft { get; set; }
        public string Lifetime { get; set; }
    }
}
