namespace PtpChat.Utility
{
    using System;
    using System.Net;

    using PtpChat.Utility.Properties;

    public class ConfigManager
    {
        public TimeSpan ChannelCutoff => new TimeSpan(0, 0, 0, Settings.Default.ChannelCutoff);

        public string DefaultApplicationFolder => Settings.Default.DefaultApplicationFolder;

        public string DefaultLoggingFile => Settings.Default.DefaultLoggingFile;

        public IPAddress InitialServerAddress => IPAddress.Parse(Settings.Default.DefaultServer_Host);

        public Guid InitialServerGuid => Guid.Parse(Settings.Default.DefaultServer_Guid);

        public int InitialServerPort => Settings.Default.DefaultServer_Port;

        public bool IsLoggingEnabled => Settings.Default.IsLoggingEnabled;

		public Guid LocalNodeId
		{
			get
			{
				if (String.IsNullOrWhiteSpace(Settings.Default.LocalNodeId))
				{
					var localNodeId = Guid.NewGuid();
					Settings.Default.LocalNodeId = localNodeId.ToString();
					Settings.Default.Save();
					return localNodeId;
				}
				return Guid.Parse(Settings.Default.LocalNodeId);
				
			}
		}

public string LocalNodeVersion => Settings.Default.LocalNodeVersion;

        public int MaxMessageResendAttempts => Settings.Default.MaxMessageResendAttempts;

        public TimeSpan MessageCutoff => new TimeSpan(0, 0, 0, Settings.Default.MessageCutoff);

        public TimeSpan NodeCutoff => new TimeSpan(0, 0, 0, Settings.Default.NodeCutoff);
    }
}