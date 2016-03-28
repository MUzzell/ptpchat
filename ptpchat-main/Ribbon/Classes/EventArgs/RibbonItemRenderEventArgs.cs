namespace PtpChat.Main.Ribbon.Classes.EventArgs
{
    using System.Drawing;

    using PtpChat.Main.Ribbon.Component_Classes;

    public class RibbonItemRenderEventArgs : RibbonRenderEventArgs
    {
        public RibbonItemRenderEventArgs(Ribbon owner, Graphics g, Rectangle clip, RibbonItem item)
            : base(owner, g, clip)
        {
            this.Item = item;
        }

        public RibbonItem Item { get; set; }
    }
}