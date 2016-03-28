namespace PtpChat.Main.Ribbon.Component_Classes
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;

    using PtpChat.Main.Ribbon.Classes.Enums;
    using PtpChat.Main.Ribbon.Classes.EventArgs;

    /// <summary>
    /// A RibbonButton that incorporates a <see cref="Color"/> property and
    /// draws this color below the displaying <see cref="Image"/> or <see cref="SmallImage"/>
    /// </summary>
    public class RibbonColorChooser : RibbonButton
    {
        #region Ctor

        public RibbonColorChooser()
        {
            this._color = Color.Transparent;
            this.ImageColorHeight = 8;
            this.SmallImageColorHeight = 4;
        }

        #endregion

        #region Events

        /// <summary>
        /// Raised when the <see cref="Color"/> property has been changed
        /// </summary>
        public event EventHandler ColorChanged;

        #endregion

        #region Overrides

        public override void OnPaint(object sender, RibbonElementPaintEventArgs e)
        {
            base.OnPaint(sender, e);

            var c = this.Color.Equals(Color.Transparent) ? Color.White : this.Color;

            var h = e.Mode == RibbonElementSizeMode.Large ? this.ImageColorHeight : this.SmallImageColorHeight;

            var colorFill = Rectangle.FromLTRB(this.ImageBounds.Left, this.ImageBounds.Bottom - h, this.ImageBounds.Right, this.ImageBounds.Bottom);
            var sm = e.Graphics.SmoothingMode;
            e.Graphics.SmoothingMode = SmoothingMode.None;
            using (var b = new SolidBrush(c))
            {
                e.Graphics.FillRectangle(b, colorFill);
            }

            if (this.Color.Equals(Color.Transparent))
            {
                e.Graphics.DrawRectangle(Pens.DimGray, colorFill);
            }

            e.Graphics.SmoothingMode = sm;
        }

        #endregion

        #region Fields

        private Color _color;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the height of the color preview on the <see cref="Image"/>
        /// </summary>
        [Description("Height of the color preview on the large image")]
        [DefaultValue(8)]
        public int ImageColorHeight { get; set; }

        /// <summary>
        /// Gets or sets the height of the color preview on the <see cref="SmallImage"/>
        /// </summary>
        [Description("Height of the color preview on the small image")]
        [DefaultValue(4)]
        public int SmallImageColorHeight { get; set; }

        /// <summary>
        /// Gets or sets the currently chosen color
        /// </summary>
        public Color Color
        {
            get { return this._color; }
            set
            {
                this._color = value;
                this.RedrawItem();
                this.OnColorChanged(EventArgs.Empty);
            }
        }

        #endregion

        #region Methods

        private Image CreateColorBmp(Color c)
        {
            var b = new Bitmap(16, 16);

            using (var g = Graphics.FromImage(b))
            {
                using (var br = new SolidBrush(c))
                {
                    g.FillRectangle(br, new Rectangle(0, 0, 15, 15));
                }

                g.DrawRectangle(Pens.DimGray, new Rectangle(0, 0, 15, 15));
            }

            return b;
        }

        /// <summary>
        /// Raises the <see cref="ColorChanged"/>
        /// </summary>
        /// <param name="e"></param>
        protected void OnColorChanged(EventArgs e)
        {
            if (this.ColorChanged != null)
            {
                this.ColorChanged(this, e);
            }
        }

        #endregion
    }
}