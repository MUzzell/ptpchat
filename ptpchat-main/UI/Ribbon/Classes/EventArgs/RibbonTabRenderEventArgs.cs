namespace PtpChat.Main.Ribbon.Classes.EventArgs
{
    using System.Drawing;

    using PtpChat.Main.Ribbon.Component_Classes;

    public sealed class RibbonTabRenderEventArgs : RibbonRenderEventArgs
    {
        public RibbonTabRenderEventArgs(Ribbon owner, Graphics g, Rectangle clip, RibbonTab tab)
            : base(owner, g, clip)
        {
            this.Tab = tab;
        }

        /// <summary>
        /// Gets or sets the RibbonTab related to the evennt
        /// </summary>
        public RibbonTab Tab { get; set; }
    }
}