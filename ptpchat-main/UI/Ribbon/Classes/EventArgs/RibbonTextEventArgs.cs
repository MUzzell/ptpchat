namespace PtpChat.Main.Ribbon.Classes.EventArgs
{
    using System.Drawing;

    using PtpChat.Main.Ribbon.Component_Classes;

    public class RibbonTextEventArgs : RibbonItemBoundsEventArgs
    {
        #region Fields

        #endregion

        #region Ctor

        public RibbonTextEventArgs(Ribbon owner, Graphics g, Rectangle clip, RibbonItem item, Rectangle bounds, string text)
            : base(owner, g, clip, item, bounds)
        {
            this.Text = text;
            this.Style = FontStyle.Regular;
            this.Format = new StringFormat();
            this.Color = Color.Empty;
        }

        public RibbonTextEventArgs(Ribbon owner, Graphics g, Rectangle clip, RibbonItem item, Rectangle bounds, string text, FontStyle style)
            : base(owner, g, clip, item, bounds)
        {
            this.Text = text;
            this.Style = style;
            this.Format = new StringFormat();
            this.Color = Color.Empty;
        }

        public RibbonTextEventArgs(Ribbon owner, Graphics g, Rectangle clip, RibbonItem item, Rectangle bounds, string text, StringFormat format)
            : base(owner, g, clip, item, bounds)
        {
            this.Text = text;
            this.Style = FontStyle.Regular;
            this.Format = format;
            this.Color = Color.Empty;
        }

        public RibbonTextEventArgs(Ribbon owner, Graphics g, Rectangle clip, RibbonItem item, Rectangle bounds, string text, Color color, FontStyle style, StringFormat format)
            : base(owner, g, clip, item, bounds)
        {
            this.Text = text;
            this.Style = style;
            this.Format = format;
            this.Color = color;
        }

        #endregion

        #region Props

        /// <summary>
        /// Gets or sets the color of the text to render
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Gets or sets the text to render
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the format of the text
        /// </summary>
        public StringFormat Format { get; set; }

        /// <summary>
        /// Gets or sets the font style of the text
        /// </summary>
        public FontStyle Style { get; set; }

        #endregion
    }
}