using NLog;

namespace com.tweetapp.api.Log
{
    public class LogNLog : ILog
    {
        private static NLog.ILogger logger = LogManager.GetCurrentClassLogger();

        public LogNLog()
        {
        }

        public void Debug(string message)
        {            
            //var logEventInfo = new LogEventInfo(NLog.LogLevel.Debug, "RmqLogMessage", $"{message}, generated at {DateTime.UtcNow}.");
            logger.Debug(message);             
        }

        public void Error(string message)
        {
            //var logEventInfo = new LogEventInfo(NLog.LogLevel.Error, "RmqLogMessage", $"{message}, generated at {DateTime.UtcNow}.");
            logger.Error(message);
        }

        public void Information(string message)
        {
            //var logEventInfo = new LogEventInfo(NLog.LogLevel.Debug, "RmqLogMessage", $"{message}, generated at {DateTime.UtcNow}.");
            logger.Info(message);
        }

        public void Warning(string message)
        {
            //var logEventInfo = new LogEventInfo(NLog.LogLevel.Debug, "RmqLogMessage", $"{message}, generated at {DateTime.UtcNow}.");
            logger.Warn(message);
        }
    }
}
