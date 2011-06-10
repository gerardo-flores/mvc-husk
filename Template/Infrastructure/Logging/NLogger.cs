using System;
using NLog;

namespace Template.Infrastructure.Logging
{
    public class NLogger : ILogger
    {
        Logger _logger;

        public NLogger()
        {
            _logger = LogManager.GetCurrentClassLogger();
        }

        public void LogInfo(string message)
        {
            _logger.Info(message);
        }

        public void LogWarning(string message)
        {
            _logger.Warn(message);
        }

        public void LogDebug(string message)
        {
            _logger.Debug(message);
        }

        public void LogError(string message)
        {
            _logger.Error(message);
        }

        public void LogError(Exception ex)
        {
            _logger.Error(BuildExceptionMessage(ex));
        }

        public void LogFatal(string message)
        {
            _logger.Fatal(message);
        }

        public virtual void LogFatal(Exception ex)
        {
            _logger.Fatal(BuildExceptionMessage(ex));
        }

        protected virtual string BuildExceptionMessage(Exception ex)
        {
            Exception logException = ex;

            if (ex.InnerException != null)
            {
                logException = ex.InnerException;
            }

            string errorMessage = Environment.NewLine + "Error: ";

            errorMessage += Environment.NewLine + "Message: " + logException.Message;
            errorMessage += Environment.NewLine + "Stack Trace: " + logException.StackTrace;
            errorMessage += Environment.NewLine + "Source: " + logException.Source;
            errorMessage += Environment.NewLine + "Target Site: " + logException.TargetSite;

            return errorMessage;
        }
    }
}