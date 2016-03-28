namespace PtpChat.Main.Ribbon.Component_Classes
{
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    using RibbonElementMeasureSizeEventArgs = PtpChat.Main.Ribbon.Classes.EventArgs.RibbonElementMeasureSizeEventArgs;
    using RibbonElementPaintEventArgs = PtpChat.Main.Ribbon.Classes.EventArgs.RibbonElementPaintEventArgs;
    using RibbonElementSizeMode = PtpChat.Main.Ribbon.Classes.Enums.RibbonElementSizeMode;
    using RibbonItemBoundsEventArgs = PtpChat.Main.Ribbon.Classes.EventArgs.RibbonItemBoundsEventArgs;
    using RibbonItemRenderEventArgs = PtpChat.Main.Ribbon.Classes.EventArgs.RibbonItemRenderEventArgs;
    using RibbonTextEventArgs = PtpChat.Main.Ribbon.Classes.EventArgs.RibbonTextEventArgs;

    //[Designer(typeof(RibbonUpDown))]
    public class RibbonUpDown : RibbonTextBox
    {
        #region Ctor

        public RibbonUpDown()
        {
            this._textboxWidth = 50;
            this._UpDownSize = 16;
        }

        #endregion

        #region Fields

        private const int spacing = 3;

        //private Ribbon _ownerRibbon;
        private Rectangle _UpButtonBounds;

        private Rectangle _DownButtonBounds;

        private readonly int _UpDownSize = 16;

        #endregion

        #region Events

        public event MouseEventHandler UpButtonClicked;

        public event MouseEventHandler DownButtonClicked;

        #endregion

        #region Props

        #endregion

        #region Methods

        /// <summary>
        /// Measures the suposed height of the textbox
        /// </summary>
        /// <returns></returns>
        public override int MeasureHeight()
        {
            return 16 + this.Owner.ItemMargin.Vertical;
        }

        public override void OnPaint(object sender, RibbonElementPaintEventArgs e)
        {
            if (this.Owner == null)
            {
                return;
            }

            this.Owner.Renderer.OnRenderRibbonItem(new RibbonItemRenderEventArgs(this.Owner, e.Graphics, this.Bounds, this));

            if (this.ImageVisible)
            {
                this.Owner.Renderer.OnRenderRibbonItemImage(new RibbonItemBoundsEventArgs(this.Owner, e.Graphics, e.Clip, this, this._imageBounds));
            }

            var f = new StringFormat();

            f.Alignment = StringAlignment.Near;
            f.LineAlignment = StringAlignment.Center;
            f.Trimming = StringTrimming.None;
            f.FormatFlags |= StringFormatFlags.NoWrap;

            this.Owner.Renderer.OnRenderRibbonItemText(new RibbonTextEventArgs(this.Owner, e.Graphics, this.Bounds, this, this.TextBoxTextBounds, this.TextBoxText, f));

            if (this.LabelVisible)
            {
                f.Alignment = (StringAlignment)this.TextAlignment;
                this.Owner.Renderer.OnRenderRibbonItemText(new RibbonTextEventArgs(this.Owner, e.Graphics, this.Bounds, this, this.LabelBounds, this.Text, f));
            }
        }

        public override void SetBounds(Rectangle bounds)
        {
            base.SetBounds(bounds);

            this._textBoxBounds = Rectangle.FromLTRB(bounds.Right - this.TextBoxWidth - this._UpDownSize, bounds.Top, bounds.Right - this._UpDownSize, bounds.Bottom);

            if (this.Image != null)
            {
                this._imageBounds = new Rectangle(bounds.Left + this.Owner.ItemMargin.Left, bounds.Top + this.Owner.ItemMargin.Top, this.Image.Width, this.Image.Height);
            }
            else
            {
                this._imageBounds = new Rectangle(this.ContentBounds.Location, Size.Empty);
            }

            this._labelBounds = Rectangle.FromLTRB(
                this._imageBounds.Right + (this._imageBounds.Width > 0 ? spacing : 0),
                bounds.Top,
                this._textBoxBounds.Left - spacing,
                bounds.Bottom - this.Owner.ItemMargin.Bottom);

            this._UpButtonBounds = new Rectangle(bounds.Right - this._UpDownSize, bounds.Top, this._UpDownSize, bounds.Height / 2);
            this._DownButtonBounds = new Rectangle(this._UpButtonBounds.X, this._UpButtonBounds.Bottom + 1, this._UpButtonBounds.Width, bounds.Height - this._UpButtonBounds.Height);

            if (this.SizeMode == RibbonElementSizeMode.Large)
            {
                this._imageVisible = true;
                this._labelVisible = true;
            }
            else if (this.SizeMode == RibbonElementSizeMode.Medium)
            {
                this._imageVisible = true;
                this._labelVisible = false;
                this._labelBounds = Rectangle.Empty;
            }
            else if (this.SizeMode == RibbonElementSizeMode.Compact)
            {
                this._imageBounds = Rectangle.Empty;
                this._imageVisible = false;
                this._labelBounds = Rectangle.Empty;
                this._labelVisible = false;
            }
        }

        public override Size MeasureSize(object sender, RibbonElementMeasureSizeEventArgs e)
        {
            if (!this.Visible && !this.Owner.IsDesignMode())
            {
                this.SetLastMeasuredSize(new Size(0, 0));
                return this.LastMeasuredSize;
            }

            var size = Size.Empty;

            var w = 0;
            var iwidth = this.Image != null ? this.Image.Width + spacing : 0;
            var lwidth = string.IsNullOrEmpty(this.Text) ? 0 : this._labelWidth > 0 ? this._labelWidth : e.Graphics.MeasureString(this.Text, this.Owner.Font).ToSize().Width + spacing;
            var twidth = this.TextBoxWidth;

            w += this.TextBoxWidth + this._UpDownSize;

            switch (e.SizeMode)
            {
                case RibbonElementSizeMode.Large:
                    w += iwidth + lwidth;
                    break;
                case RibbonElementSizeMode.Medium:
                    w += iwidth;
                    break;
            }

            this.SetLastMeasuredSize(new Size(w, this.MeasureHeight()));

            return this.LastMeasuredSize;
        }

        public override void OnMouseEnter(MouseEventArgs e)
        {
            if (!this.Enabled)
            {
                return;
            }

            base.OnMouseEnter(e);
            if (this.TextBoxBounds.Contains(e.Location))
            {
                this.Canvas.Cursor = Cursors.IBeam;
            }
        }

        public override void OnMouseLeave(MouseEventArgs e)
        {
            if (!this.Enabled)
            {
                return;
            }

            base.OnMouseLeave(e);

            this.UpButtonSelected = false;
            this.DownButtonSelected = false;

            this.Canvas.Cursor = Cursors.Default;
        }

        public override void OnMouseUp(MouseEventArgs e)
        {
            if (!this.Enabled)
            {
                return;
            }

            base.OnMouseUp(e);
            var mustRedraw = false;

            if (this.UpButtonPressed || this.DownButtonPressed)
            {
                mustRedraw = true;
            }

            this.UpButtonPressed = false;
            this.DownButtonPressed = false;

            if (mustRedraw)
            {
                this.RedrawItem();
            }
        }

        public override void OnMouseDown(MouseEventArgs e)
        {
            if (!this.Enabled)
            {
                return;
            }

            if (this._UpButtonBounds.Contains(e.Location))
            {
                this.UpButtonPressed = true;
                this.DownButtonPressed = false;
                this.DownButtonSelected = false;
                if (this.UpButtonClicked != null)
                {
                    this.UpButtonClicked(this, e);
                }
            }
            else if (this._DownButtonBounds.Contains(e.Location))
            {
                this.DownButtonPressed = true;
                this.UpButtonPressed = false;
                this.UpButtonSelected = false;
                if (this.DownButtonClicked != null)
                {
                    this.DownButtonClicked(this, e);
                }
            }
            else if (this.TextBoxBounds.Contains(e.X, e.Y) && this.AllowTextEdit)
            {
                this.StartEdit();
            }
        }

        public override void OnMouseMove(MouseEventArgs e)
        {
            if (!this.Enabled)
            {
                return;
            }

            base.OnMouseMove(e);

            var mustRedraw = false;

            if (this._UpButtonBounds.Contains(e.Location))
            {
                this.Owner.Cursor = Cursors.Default;
                mustRedraw = !this.UpButtonSelected || this.DownButtonSelected || this.DownButtonPressed;
                this.UpButtonSelected = true;
                this.DownButtonSelected = false;
                this.DownButtonPressed = false;
            }
            else if (this._DownButtonBounds.Contains(e.Location))
            {
                this.Owner.Cursor = Cursors.Default;
                mustRedraw = !this.DownButtonSelected || this.UpButtonSelected || this.UpButtonPressed;
                this.DownButtonSelected = true;
                this.UpButtonSelected = false;
                this.UpButtonPressed = false;
            }
            else if (this.TextBoxBounds.Contains(e.X, e.Y))
            {
                this.Owner.Cursor = Cursors.IBeam;
                mustRedraw = this.DownButtonSelected || this.DownButtonPressed || this.UpButtonSelected || this.UpButtonPressed;
                this.UpButtonSelected = false;
                this.UpButtonPressed = false;
                this.DownButtonSelected = false;
                this.DownButtonPressed = false;
            }
            else
            {
                this.Owner.Cursor = Cursors.Default;
            }

            if (mustRedraw)
            {
                this.RedrawItem();
            }
        }

        #endregion

        #region Properties

        ///// <summary>
        ///// Gets the Ribbon this DropDown belongs to
        ///// </summary>
        //public Ribbon OwnerRibbon
        //{
        //   get { return _ownerRibbon; }
        //}

        /// <summary>
        /// Gets a value indicating if the Up button is currently pressed
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool UpButtonPressed { get; private set; }

        /// <summary>
        /// Gets a value indicating if the Down button is currently pressed
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DownButtonPressed { get; private set; }

        /// <summary>
        /// Gets a value indicating if the Up button is currently selected
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool UpButtonSelected { get; private set; }

        /// <summary>
        /// Gets a value indicating if the Down button is currently selected
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DownButtonSelected { get; private set; }

        /// <summary>
        /// Gets or sets the bounds of the DropDown button
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Rectangle UpButtonBounds => this._UpButtonBounds;

        /// <summary>
        /// Gets or sets the bounds of the DropDown button
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Rectangle DownButtonBounds => this._DownButtonBounds;

        /// <summary>
        /// Overriden.
        /// </summary>
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        //public override Rectangle TextBoxTextBounds
        //{
        //   get
        //   {
        //      Rectangle r = base.TextBoxTextBounds;

        //      r.Width -= _UpButtonBounds.Width;

        //      return r;
        //   }
        //}

        #endregion
    }
}