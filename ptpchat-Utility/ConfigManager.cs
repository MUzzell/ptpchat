namespace PtpChat.Utility
{
    using System.Net;

    using PtpChat.Utility.Properties;

    public class ConfigManager
    {
        public IPAddress InitialServerAddress => IPAddress.Parse(Settings.Default.DefaultServer_Host);

        public string DefaultLoggingFile => Settings.Default.DefaultLoggingFile;

        public string DefaultApplicationFolder => Settings.Default.DefaultApplicationFolder;

        public bool IsLoggingEnabled => Settings.Default.IsLoggingEnabled;
    }
}