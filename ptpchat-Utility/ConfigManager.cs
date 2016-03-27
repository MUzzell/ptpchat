namespace PtpChat.Utility
{
	using System.Net;

	using PtpChat.Utility.Properties;
	using System;
	public class ConfigManager
    {
        public IPAddress InitialServerAddress => IPAddress.Parse(Settings.Default.DefaultServer_Host);

		public Guid InitialServerGuid => Guid.Parse(Settings.Default.DefaultServer_Guid);

        public string DefaultLoggingFile => Settings.Default.DefaultLoggingFile;

        public string DefaultApplicationFolder => Settings.Default.DefaultApplicationFolder;

        public bool IsLoggingEnabled => Settings.Default.IsLoggingEnabled;

		public string LocalNodeVersion => Settings.Default.LocalNodeVersion;

		public Guid LocalNodeId
		{
			get
			{
				return Guid.NewGuid();
				/*
				if (String.IsNullOrWhiteSpace(Settings.Default.LocalNodeId))
				{
					var localNodeId = Guid.NewGuid();
					Settings.Default.LocalNodeId = localNodeId.ToString();
					Settings.Default.Save();
					return localNodeId;
				}
				return Guid.Parse(Settings.Default.LocalNodeId);
				*/
			}
		}
    }
}