
namespace HallOfFame_backend.Services.Interfaces
{
    public interface ILoggerService 
    {
        void Information(string message);
        void Warning(string message);
        void Debug(string message);
        void Error(string message);
    }
}
