namespace PtpChat_Main
{
    using System;
    using System.Windows.Forms;

    using PtpChat_UtilityClasses;

    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new UDPChatForm(new ConfigManager()));
        }
    }
}