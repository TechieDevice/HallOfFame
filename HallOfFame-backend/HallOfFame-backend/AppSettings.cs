using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HallOfFame_backend.AppSettings
{
    public class AppSettings
    {
        public Logging Logging { get; set; }
        public ConnectionStrings ConnectionStrings { get; set; }
    }
    public class Logging
    {
        public LogPath LogPath { get; set; }
    }

    public class LogPath
    {
        public string LoggerName { get; set; }
        public bool CorrentDirectory { get; set; }
        public string PathToFile { get; set; }
    }

    public class ConnectionStrings
    {
        public string DefaultConnection { get; set; }
    }
}
