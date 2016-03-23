using PtpChat.Utility.Properties;
using System.Net;

namespace PtpChat.Utility
{
    public class ConfigManager
    {
        public IPAddress InitialServerAddress { get { return IPAddress.Parse(Settings.Default.DefaultServer_Host); } }

		public string DefaultLoggingFile {  get { return Settings.Default.DefaultLoggingFile; } }

		public string DefaultApplicationFolder { get { return Settings.Default.DefaultApplicationFolder; } }

		public bool IsLoggingEnabled { get { return Settings.Default.IsLoggingEnabled; } }

    }
}