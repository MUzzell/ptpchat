namespace PtpChat.Utility
{
    using System;
    using System.IO;

    using NLog;
    using NLog.Config;
    using NLog.Targets;
    using NLog.Windows.Forms;

    using PtpChat.Base.Interfaces;

    public class Logger : ILogManager
    {
        public readonly ColoredConsoleTarget ConsoleTarget;

        public readonly FileTarget FileTarget;

        public readonly RichTextBoxTarget FormTarget;

        private readonly NLog.Logger internalLogger;

        public Logger(ConfigManager config, string logName)
        {
            this.ConsoleTarget = new ColoredConsoleTarget { Layout = @"${date:format=HH\:mm\:ss} ${level:uppercase=true} ${message}" };

            this.FileTarget = new FileTarget
                                  {
                                      Layout = @"${date:format=yyyy-MM-dd HH\:mm\:ss} ${level:uppercase=true} ${message}",
                                      FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), config.DefaultLoggingFile)
                                  };

            this.FormTarget = new RichTextBoxTarget { Layout = @"${date:format=HH\:mm\:ss} ${level:uppercase=true} ${message}", SupportLinks = true };

            var loggerConfig = new LoggingConfiguration();
            loggerConfig.AddTarget("console", this.ConsoleTarget);
            loggerConfig.AddTarget("file", this.FileTarget);
            loggerConfig.AddTarget("form", this.FormTarget);

            loggerConfig.LoggingRules.Add(new LoggingRule("*", config.IsLoggingEnabled ? LogLevel.Debug : LogLevel.Off, this.ConsoleTarget));
            loggerConfig.LoggingRules.Add(new LoggingRule("*", config.IsLoggingEnabled ? LogLevel.Debug : LogLevel.Off, this.FileTarget));
            loggerConfig.LoggingRules.Add(new LoggingRule("*", config.IsLoggingEnabled ? LogLevel.Debug : LogLevel.Off, this.FormTarget));

            LogManager.Configuration = loggerConfig;

            this.internalLogger = LogManager.GetLogger(logName);
        }

        public void Fatal(string message, Exception exception = null)
        {
            if (exception != null)
            {
                this.internalLogger.Fatal(exception, message);
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