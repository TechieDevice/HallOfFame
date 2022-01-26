using System;
using System.Collections.Generic;
using System.Text;
using HallOfFame_backend.Services.Interfaces;

namespace HallOfFame_Tests
{
    public class EmptyLoggingService : ILoggerService
    {
        public void Information(string message) { }

        public void Warning(string message) { }

        public void Debug(string message) { }

        public void Error(string message) { }
    }
}
