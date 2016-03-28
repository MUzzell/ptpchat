namespace PtpChat.Main.Ribbon.Classes.EventArgs
{
    using System;
    using System.Drawing;

    using PtpChat.Main.Ribbon.Classes.Enums;

    /// <summary>
    /// Holds data and tools to measure the size
    /// </summary>
    public class RibbonElementMeasureSizeEventArgs : EventArgs
    {
        /// <summary>
        /// Creates a new RibbonElementMeasureSizeEventArgs object
        /// </summary>
        /// <param name="graphics">Device info to draw and measure</param>
        /// <param name="sizeMode">Size mode to measure</param>
        internal RibbonElementMeasureSizeEventArgs(Graphics graphics, RibbonElementSizeMode sizeMode)
        {
            this.Graphics = graphics;
            this.SizeMode = sizeMode;
        }

        /// <summary>
        /// Gets the size mode to measure
        /// </summary>
        public RibbonElementSizeMode SizeMode { get; }

        /// <summary>
        /// Gets the device to measure objects
        /// </summary>
        public Graphics Graphics { get; }
    }
}