namespace PtpChat.Main.Ribbon.Classes.EventArgs
{
    using System;
    using System.Drawing;

    using PtpChat.Main.Ribbon.Component_Classes;

    /// <remarks>Ribbon rendering event data</remarks>
    public class RibbonRenderEventArgs : EventArgs
    {
        public RibbonRenderEventArgs(Ribbon owner, Graphics g, Rectangle clip)
        {
            this.Ribbon = owner;
            this.Graphics = g;
            this.ClipRectangle = clip;
        }

        /// <summary>
        /// Gets the Ribbon related to the render
        /// </summary>
        public Ribbon Ribbon { get; set; }

        /// <summary>
        /// Gets the Device to draw into
        /// </summary>
        public Graphics Graphics { get; set; }

        /// <summary>
        /// Gets the Rectangle area where to draw into
        /// </summary>
        public Rectangle ClipRectangle { get; set; }
    }
}