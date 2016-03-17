namespace PtpChat.Main.Util
{
    using System.Net;

    using PtpChat.Main.Properties;

    internal class ConfigManager
    {
        IPAddress InitialServerAddress { get { return IPAddress.Parse(Settings.Default.InitialServerAddress_Host); } }
    }
}