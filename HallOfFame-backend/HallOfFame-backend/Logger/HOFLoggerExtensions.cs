using Microsoft.Extensions.Logging;

namespace HallOfFame_backend.Logger
{
    public static class HOFLoggerExtension
    {
        public static ILoggerFactory AddFile(this ILoggerFactory factory, string filePath)
        {
            factory.AddProvider(new HOFLoggerProvider(filePath));
            return factory;
        }
    }
}
