using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Template.Infrastructure.Logging
{
    public class MyLogger : ILogger
    {
        public void LogDebug(string message)
        {
            throw new NotImplementedException();
        }

        public void LogError(Exception ex)
        {
            throw new NotImplementedException();
        }

        public void LogError(string message)
        {
            throw new NotImplementedException();
        }

        public void LogFatal(Exception ex)
        {
            throw new NotImplementedException();
        }

        public void LogFatal(string message)
        {
            throw new NotImplementedException();
        }

        public void LogInfo(string message)
        {
            throw new NotImplementedException();
        }

        public void LogWarning(string message)
        {
            throw new NotImplementedException();
        }
    }
}