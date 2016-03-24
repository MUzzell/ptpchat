namespace PtpChat.Utility
{
    using System;
    using System.IO;

    using NLog;
    using NLog.Config;
    using NLog.Targets;

    using PtpChat.Base.Interfaces;

    public class Logger : ILogManager
    {
        public Logger(ConfigManager config, string logName)
        {
            var consoleTarget = new ColoredConsoleTarget { Layout = @"${date:format=HH\:mm\:ss} ${logger} ${level} ${message}" };

            var fileTarget = new FileTarget
                                 {
                                     Layout = @"${date:format=yyyy-MM-dd HH\:mm\:ss} ${logger} ${level} ${message}",
                                     FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), config.DefaultLoggingFile)
                                 };

            var loggerConfig = new LoggingConfiguration();
            loggerConfig.AddTarget("console", consoleTarget);
            loggerConfig.AddTarget("file", fileTarget);

            loggerConfig.LoggingRules.Add(new LoggingRule("*", config.IsLoggingEnabled ? LogLevel.Debug : LogLevel.Off, consoleTarget));
            loggerConfig.LoggingRules.Add(new LoggingRule("*", config.IsLoggingEnabled ? LogLevel.Debug : LogLevel.Off, fileTarget));

            LogManager.Configuration = loggerConfig;

            this.internalLogger = LogManager.GetLogger(logName);
        }

        private readonly NLog.Logger internalLogger;

        public void Fatal(string message, Exception exception = null)
        {
            if (exception != null)
            {
                this.internalLogger.Fatal(exception,message);
            }
            else
            {
                this.internalLogger.Fatal(message);
            }
        }

        public void Debug(string message) => this.internalLogger.Debug(message);

        public void Error(string message, Exception exception = null)
        {
            if (exception != null)
            {
                this.internalLogger.Error(exception, message);
            }
            else
            {
                this.internalLogger.Error(message);
            }
        }

        public void Info(string message) => this.internalLogger.Info(message);

        public void Warning(string message) => this.internalLogger.Warn(message);
    }
}