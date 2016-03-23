
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PtpChat.Utility
{
	using PtpChat.Base.Interfaces;

	using NLog;
	using NLog.Config;
	using NLog.Targets;
	using System.IO;
	internal class Logger : ILogManager
	{
		private NLog.Logger internalLogger;

		public Logger(ConfigManager config, string logName)
		{
			var loggerConfig = new LoggingConfiguration();
			
			var consoleTarget = new ColoredConsoleTarget();

			consoleTarget.Layout = @"${date:format=HH\:mm\:ss} ${logger} ${level} ${message}";
			
			var fileTarget = new FileTarget();

			fileTarget.Layout = @"${date:format=yyyy-MM-dd HH\:mm\:ss} ${logger} ${level} ${message}";
			
			fileTarget.FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), config.DefaultLoggingFile);

			loggerConfig.AddTarget("console", consoleTarget);
			loggerConfig.AddTarget("file", fileTarget);

			loggerConfig.LoggingRules.Add(new LoggingRule("*", config.IsLoggingEnabled ? LogLevel.Debug : LogLevel.Off, consoleTarget));
			loggerConfig.LoggingRules.Add(new LoggingRule("*", config.IsLoggingEnabled ? LogLevel.Debug : LogLevel.Off, fileTarget));

			LogManager.Configuration = loggerConfig;

			internalLogger = LogManager.GetLogger(logName);
			
		}

		public void Fatal(string message, Exception exception = null)
		{
			if (exception != null)
				internalLogger.Fatal(message);
			else
				internalLogger.Fatal(exception, message);
		}

		public void Debug(string message)
		{
			internalLogger.Debug(message);
		}

		public void Error(string message, Exception exception = null)
		{
			if (exception != null)
				internalLogger.Error(message);
			else
				internalLogger.Error(exception, message);
		}

		public void Info(string message)
		{
			internalLogger.Info(message);
		}

		public void Warning(string message)
		{
			internalLogger.Warn(message);
		}
	}
}
