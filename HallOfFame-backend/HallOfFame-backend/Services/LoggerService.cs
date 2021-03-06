using HallOfFame_backend.Services.Interfaces;
using NLog;

namespace HallOfFame_backend.Services
{
    public class LoggerService : ILoggerService
    {
        private static NLog.Logger logger = LogManager.GetLogger("hallOfFameLoggerRule");

        public LoggerService() { }

        public void Debug(string message)
        {
            logger.Debug(message);
        }

        public void Error(string message)
        {
            logger.Error(message);
        }

        public void Information(string message)
        {
            logger.Info(message);
        }

        public void Warning(string message)
        {
            logger.Warn(message);
        }
    }
}
