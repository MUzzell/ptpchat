namespace PtpChat.Main.Ribbon.Classes.EventArgs
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    using Ribbon = PtpChat.Main.Ribbon.Component_Classes.Ribbon;

    public class RibbonCanvasEventArgs : EventArgs
    {
        #region ctor

        public RibbonCanvasEventArgs(Ribbon owner, Graphics g, Rectangle bounds, Control canvas, object relatedObject)
        {
            this.Owner = owner;
            this.Graphics = g;
            this.Bounds = bounds;
            this.Canvas = canvas;
            this.RelatedObject = relatedObject;
        }

        #endregion

        #region Props

        public object RelatedObject { get; set; }

        /// <summary>
        /// Gets or sets the Ribbon that raised the event
        /// </summary>
        public Ribbon Owner { get; set; }

        /// <summary>
        /// Gets or sets the graphics to paint
        /// </summary>
        public Graphics Graphics { get; set; }

        /// <summary>
        /// Gets or sets the bounds that should be painted
        /// </summary>
        public Rectangle Bounds { get; set; }

        /// <summary>
        /// Gets or sets the control where to be painted
        /// </summary>
        public Control Canvas { get; set; }

        #endregion
    }
}