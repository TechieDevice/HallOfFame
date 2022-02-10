using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text;

namespace HallOfFame_backend.Logger
{
    public class HOFLogger : ILogger
    {
        private string filePath;
        private string _name;
        private static object _lock = new object();

        public HOFLogger(string path, string name)
        {
            _name = name;
            filePath = path;
        }
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (formatter != null)
            {
                var message = formatter(state, exception);
                WriteMessageToFile(logLevel, eventId, exception, message);
            }
        }

        private void WriteMessageToFile(LogLevel logLevel, EventId eventId, Exception exception, string message)
        {
            var date = DateTime.Now;
            var messageString = $"{date}|{logLevel}|{_name}|{eventId}";
            messageString += Environment.NewLine;
            messageString += $"message: {message}";

            if (exception != null)
            {
                messageString += Environment.NewLine;
                messageString += $"exception: {exception}";
            }

            lock (_lock)
            {
                File.AppendAllText(filePath, messageString + Environment.NewLine);
            }
        }
    }
}
