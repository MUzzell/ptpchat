namespace PtpChat.Main.Ribbon.Classes.EventArgs
{
    using System;

    using PtpChat.Main.Ribbon.Component_Classes;

    public class RibbonItemEventArgs : EventArgs
    {
        public RibbonItemEventArgs(RibbonItem item)
        {
            this.Item = item;
        }

        public RibbonItem Item { get; set; }
    }
}