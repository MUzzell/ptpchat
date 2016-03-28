namespace PtpChat.Main.Ribbon.Component_Classes
{
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    using RibbonArrowDirection = PtpChat.Main.Ribbon.Classes.Enums.RibbonArrowDirection;
    using RibbonElementMeasureSizeEventArgs = PtpChat.Main.Ribbon.Classes.EventArgs.RibbonElementMeasureSizeEventArgs;
    using RibbonElementPaintEventArgs = PtpChat.Main.Ribbon.Classes.EventArgs.RibbonElementPaintEventArgs;
    using RibbonElementSizeMode = PtpChat.Main.Ribbon.Classes.Enums.RibbonElementSizeMode;
    using RibbonTextEventArgs = PtpChat.Main.Ribbon.Classes.EventArgs.RibbonTextEventArgs;

    /// <summary>
    /// Large menu item with a description bellow the text
    /// </summary>
    public class RibbonDescriptionMenuItem : RibbonButton
    {
        #region Fields

        #endregion

        #region Ctor

        public RibbonDescriptionMenuItem()
        {
            this.DropDownArrowDirection = RibbonArrowDirection.Left;
            this.SetDropDownMargin(new Padding(10));
        }

        /// <summary>
        /// Creates a new menu item with description
        /// </summary>
        /// <param name="text">Text of the menuitem</param>
        public RibbonDescriptionMenuItem(string text)
            : this(null, text, null)
        {
        }

        /// <summary>
        /// Creates a new menu item with description
        /// </summary>
        /// <param name="text">Text of the menuitem</param>
        /// <param name="description">Descripion of the menuitem</param>
        public RibbonDescriptionMenuItem(string text, string description)
            : this(null, text, description)
        {
        }

        /// <summary>
        /// Creates a new menu item with description
        /// </summary>
        /// <param name="image">Image for the menuitem</param>
        /// <param name="text">Text for the menuitem</param>
        /// <param name="description">Description for the menuitem</param>
        public RibbonDescriptionMenuItem(Image image, string text, string description)
        {
            this.Image = image;
            this.Text = text;
            this.Description = description;
        }

        #endregion

        #region Props

        /// <summary>
        /// Gets or sets the bounds of the description
        /// </summary>
        public Rectangle DescriptionBounds { get; set; }

        /// <summary>
        /// Gets or sets the image of the menu item
        /// </summary>
        public override Image Image
        {
            get { return base.Image; }
            set
            {
                base.Image = value;

                this.SmallImage = value;
            }
        }

        /// <summary>
        /// This property is not relevant for this class
        /// </summary>
        [Browsable(false)]
        public override Image SmallImage { get { return base.SmallImage; } set { base.SmallImage = value; } }

        /// <summary>
        /// Gets or sets the description of the button
        /// </summary>
        [DefaultValue(null)]
        public string Description { get; set; }

        #endregion

        #region Methods

        protected override void OnPaintText(RibbonElementPaintEventArgs e)
        {
            if (e.Mode == RibbonElementSizeMode.DropDown)
            {
                var sf = new StringFormat();
                sf.LineAlignment = StringAlignment.Center;
                sf.Alignment = StringAlignment.Near;

                this.Owner.Renderer.OnRenderRibbonItemText(new RibbonTextEventArgs(this.Owner, e.Graphics, e.Clip, this, this.TextBounds, this.Text, Color.Empty, FontStyle.Bold, sf));

                sf.Alignment = StringAlignment.Near;

                this.Owner.Renderer.OnRenderRibbonItemText(new RibbonTextEventArgs(this.Owner, e.Graphics, e.Clip, this, this.DescriptionBounds, this.Description, sf));
            }
            else
            {
                base.OnPaintText(e);
            }
        }

        public override Size MeasureSize(object sender, RibbonElementMeasureSizeEventArgs e)
        {
            if (!this.Visible && !this.Owner.IsDesignMode())
            {
                this.SetLastMeasuredSize(new Size(0, 0));
                return this.LastMeasuredSize;
            }

            var s = base.MeasureSize(sender, e);

            s.Height = 52;

            this.SetLastMeasuredSize(s);

            return s;
        }

        internal override Rectangle OnGetTextBounds(RibbonElementSizeMode sMode, Rectangle bounds)
        {
            var r = base.OnGetTextBounds(sMode, bounds);
            this.DescriptionBounds = r;

            r.Height = 20;

            this.DescriptionBounds = Rectangle.FromLTRB(this.DescriptionBounds.Left, r.Bottom, this.DescriptionBounds.Right, this.DescriptionBounds.Bottom);

            return r;
        }

        #endregion
    }
}