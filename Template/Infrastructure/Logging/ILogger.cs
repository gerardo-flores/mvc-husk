using System;

namespace Template.Infrastructure.Logging
{
    public interface ILogger
    {
        void LogDebug(string message);
        void LogError(Exception ex);
        void LogError(string message);
        void LogFatal(Exception ex);
        void LogFatal(string message);
        void LogInfo(string message);
        void LogWarning(string message);
    }
}
