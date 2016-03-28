namespace PtpChat.Main.Ribbon.Classes.EventArgs
{
    using System.Drawing;

    using PtpChat.Main.Ribbon.Component_Classes;

    public class RibbonItemBoundsEventArgs : RibbonItemRenderEventArgs
    {
        public RibbonItemBoundsEventArgs(Ribbon owner, Graphics g, Rectangle clip, RibbonItem item, Rectangle bounds)
            : base(owner, g, clip, item)
        {
            this.Bounds = bounds;
        }

        #region Properties

        /// <summary>
        /// Gets or sets the suggested bounds
        /// </summary>
        public Rectangle Bounds { get; set; }

        #endregion
    }
}