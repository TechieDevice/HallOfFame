using Microsoft.Extensions.Logging;

namespace HallOfFame_backend.Logger
{
    public class HOFLoggerProvider : ILoggerProvider
    {
        private string _path;
        public HOFLoggerProvider(string path)
        {
            _path = path;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new HOFLogger(_path, categoryName);
        }

        public void Dispose()
        {
        }
    }
}
