namespace PtpChat.Main.Ribbon.Classes.EventArgs
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    using RibbonElementSizeMode = PtpChat.Main.Ribbon.Classes.Enums.RibbonElementSizeMode;

    /// <summary>
    /// Holds data and tools to draw the element
    /// </summary>
    public class RibbonElementPaintEventArgs : EventArgs
    {
        /// <param name="clip">Rectangle clip</param>
        /// <param name="graphics">Device to draw</param>
        /// <param name="mode">Size mode to draw</param>
        internal RibbonElementPaintEventArgs(Rectangle clip, Graphics graphics, RibbonElementSizeMode mode)
        {
            this.Clip = clip;
            this.Graphics = graphics;
            this.Mode = mode;
        }

        internal RibbonElementPaintEventArgs(Rectangle clip, Graphics graphics, RibbonElementSizeMode mode, Control control)
            : this(clip, graphics, mode)
        {
            this.Control = control;
        }

        /// <summary>
        /// Area that element should occupy
        /// </summary>
        public Rectangle Clip { get; }

        /// <summary>
        /// Gets the Device where to draw
        /// </summary>
        public Graphics Graphics { get; }

        /// <summary>
        /// Gets the mode to draw the element
        /// </summary>
        public RibbonElementSizeMode Mode { get; }

        /// <summary>
        /// Gets the control where element is being painted
        /// </summary>
        public Control Control { get; }
    }
}