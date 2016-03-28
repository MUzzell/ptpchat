namespace PtpChat.Utility
{
    using System;
    using System.Net;

    using PtpChat.Utility.Properties;

    public class ConfigManager
    {
        public IPAddress InitialServerAddress => IPAddress.Parse(Settings.Default.DefaultServer_Host);

        public Guid InitialServerGuid => Guid.Parse(Settings.Default.DefaultServer_Guid);

        public string DefaultLoggingFile => Settings.Default.DefaultLoggingFile;

        public string DefaultApplicationFolder => Settings.Default.DefaultApplicationFolder;

        public bool IsLoggingEnabled => Settings.Default.IsLoggingEnabled;

        public string LocalNodeVersion => Settings.Default.LocalNodeVersion;

        public Guid LocalNodeId => Guid.NewGuid();
    }
}