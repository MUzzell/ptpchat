namespace PtpChat.Main.Ribbon.Component_Classes
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    using RibbonElementMeasureSizeEventArgs = PtpChat.Main.Ribbon.Classes.EventArgs.RibbonElementMeasureSizeEventArgs;
    using RibbonElementPaintEventArgs = PtpChat.Main.Ribbon.Classes.EventArgs.RibbonElementPaintEventArgs;
    using RibbonElementSizeMode = PtpChat.Main.Ribbon.Classes.Enums.RibbonElementSizeMode;
    using RibbonItemBoundsEventArgs = PtpChat.Main.Ribbon.Classes.EventArgs.RibbonItemBoundsEventArgs;
    using RibbonItemRenderEventArgs = PtpChat.Main.Ribbon.Classes.EventArgs.RibbonItemRenderEventArgs;
    using RibbonTextEventArgs = PtpChat.Main.Ribbon.Classes.EventArgs.RibbonTextEventArgs;

    public class RibbonTextBox : RibbonItem
    {
        #region Ctor

        public RibbonTextBox()
        {
            this._textboxWidth = 100;
            this._textBoxText = "";
        }

        #endregion

        #region Fields

        private const int spacing = 3;

        internal TextBox _actualTextBox;

        internal bool _removingTxt;

        internal bool _labelVisible;

        internal bool _imageVisible;

        internal Rectangle _labelBounds;

        internal Rectangle _imageBounds;

        internal int _textboxWidth;

        internal int _labelWidth;

        internal Rectangle _textBoxBounds;

        internal string _textBoxText;

        internal bool _AllowTextEdit = true;

        #endregion

        #region Events

        /// <summary>
        /// Raised when the <see cref="TextBoxText"/> property value has changed
        /// </summary>
        public event EventHandler TextBoxTextChanged;

        public event KeyPressEventHandler TextBoxKeyPress;

        public event KeyEventHandler TextBoxKeyDown;

        public event EventHandler TextBoxValidating;

        public event EventHandler TextBoxValidated;

        #endregion

        #region Props

        /// <summary>
        /// Gets or sets if the textbox allows editing
        /// </summary>
        [Description("Allow Test Edit")]
        [DefaultValue(true)]
        public bool AllowTextEdit { get { return this._AllowTextEdit; } set { this._AllowTextEdit = value; } }

        /// <summary>
        /// Gets or sets the text on the textbox
        /// </summary>
        [Description("Text on the textbox")]
        public string TextBoxText
        {
            get { return this._textBoxText; }
            set
            {
                this._textBoxText = value;
                if (this._actualTextBox != null)
                {
                    this._actualTextBox.Text = this._textBoxText;
                }
                this.OnTextChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets the bounds of the text on the textbox
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual Rectangle TextBoxTextBounds => this.TextBoxBounds;

        /// <summary>
        /// Gets the bounds of the image
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Rectangle ImageBounds => this._imageBounds;

        /// <summary>
        /// Gets the bounds of the label that is shown next to the textbox
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual Rectangle LabelBounds => this._labelBounds;

        /// <summary>
        /// Gets a value indicating if the image is currenlty visible
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ImageVisible => this._imageVisible;

        /// <summary>
        /// Gets a value indicating if the label is currently visible
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool LabelVisible => this._labelVisible;

        /// <summary>
        /// Gets the bounds of the text
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual Rectangle TextBoxBounds => this._textBoxBounds;

        /// <summary>
        /// Gets a value indicating if user is currently editing the text of the textbox
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Editing => this._actualTextBox != null;

        /// <summary>
        /// Gets or sets the width of the textbox
        /// </summary>
        [DefaultValue(100)]
        public int TextBoxWidth
        {
            get { return this._textboxWidth; }
            set
            {
                this._textboxWidth = value;
                this.NotifyOwnerRegionsChanged();
            }
        }

        /// <summary>
        /// Gets or sets the width of the Label. Enter zero to auto size based on contents.
        /// </summary>
        [DefaultValue(0)]
        public int LabelWidth
        {
            get { return this._labelWidth; }
            set
            {
                this._labelWidth = value;
                this.NotifyOwnerRegionsChanged();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Starts editing the text and focuses the TextBox
        /// </summary>
        public void StartEdit()
        {
            //if (!Enabled) return;

            this.PlaceActualTextBox();

            this._actualTextBox.SelectAll();
            this._actualTextBox.Focus();
        }

        /// <summary>
        /// Ends the editing of the textbox
        /// </summary>
        public void EndEdit()
        {
            this.RemoveActualTextBox();
        }

        /// <summary>
        /// Places the Actual TextBox on the owner so user can edit the text
        /// </summary>
        protected void PlaceActualTextBox()
        {
            this._actualTextBox = new TextBox();

            this.InitTextBox(this._actualTextBox);

            this._actualTextBox.TextChanged += this._actualTextbox_TextChanged;
            this._actualTextBox.KeyDown += this._actualTextbox_KeyDown;
            this._actualTextBox.KeyPress += this._actualTextbox_KeyPress;
            this._actualTextBox.LostFocus += this._actualTextbox_LostFocus;
            this._actualTextBox.VisibleChanged += this._actualTextBox_VisibleChanged;
            this._actualTextBox.Validating += this._actualTextbox_Validating;
            this._actualTextBox.Validated += this._actualTextbox_Validated;

            this._actualTextBox.Visible = true;
            //_actualTextBox.AcceptsTab = true;
            this.Canvas.Controls.Add(this._actualTextBox);
            this.Owner.ActiveTextBox = this;
        }

        public void _actualTextBox_VisibleChanged(object sender, EventArgs e)
        {
            if (!(sender as TextBox).Visible && !this._removingTxt)
            {
                this.RemoveActualTextBox();
            }
        }

        /// <summary>
        /// Removes the actual TextBox that edits the text
        /// </summary>
        protected void RemoveActualTextBox()
        {
            if (this._actualTextBox == null || this._removingTxt)
            {
                return;
            }
            this._removingTxt = true;

            this.TextBoxText = this._actualTextBox.Text;
            this._actualTextBox.Visible = false;
            if (this._actualTextBox.Parent != null)
            {
                this._actualTextBox.Parent.Controls.Remove(this._actualTextBox);
            }
            this._actualTextBox.Dispose();
            this._actualTextBox = null;

            this.RedrawItem();
            this._removingTxt = false;
            this.Owner.ActiveTextBox = null;
        }

        /// <summary>
        /// Initializes the texbox that edits the text
        /// </summary>
        /// <param name="t"></param>
        protected virtual void InitTextBox(TextBox t)
        {
            t.Text = this.TextBoxText;
            t.BorderStyle = BorderStyle.None;
            t.Width = this.TextBoxBounds.Width - 2;

            t.Location = new Point(this.TextBoxBounds.Left + 2, this.Bounds.Top + (this.Bounds.Height - t.Height) / 2);
        }

        /// <summary>
        /// Handles the LostFocus event of the actual TextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void _actualTextbox_LostFocus(object sender, EventArgs e)
        {
            this.RemoveActualTextBox();
        }

        /// <summary>
        /// Handles the KeyDown event of the actual TextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void _actualTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.TextBoxKeyDown != null)
            {
                this.TextBoxKeyDown(this, e);
            }

            if (e.KeyCode == Keys.Return || e.KeyCode == Keys.Enter || e.KeyCode == Keys.Escape)
            {
                this.RemoveActualTextBox();
            }
        }

        /// <summary>
        /// Handles the KeyPress event of the actual TextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void _actualTextbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!this.Enabled)
            {
                return;
            }

            if (this.TextBoxKeyPress != null)
            {
                this.TextBoxKeyPress(this, e);
            }
        }

        /// <summary>
        /// Handles the Validating event of the actual TextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void _actualTextbox_Validating(object sender, CancelEventArgs e)
        {
            if (!this.Enabled)
            {
                return;
            }

            if (this.TextBoxValidating != null)
            {
                this.TextBoxValidating(this, e);
            }
        }

        /// <summary>
        /// Handles the Validated event of the actual TextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void _actualTextbox_Validated(object sender, EventArgs e)
        {
            if (!this.Enabled)
            {
                return;
            }

            if (this.TextBoxValidated != null)
            {
                this.TextBoxValidated(this, e);
            }
        }

        /// <summary>
        /// Handles the TextChanged event of the actual TextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void _actualTextbox_TextChanged(object sender, EventArgs e)
        {
            //Text = (sender as TextBox).Text;
            {
                this.TextBoxText = (sender as TextBox).Text;
            }
        }

        /// <summary>
        /// Measures the suposed height of the textobx
        /// </summary>
        /// <returns></returns>
        public virtual int MeasureHeight()
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

            this._textBoxBounds = Rectangle.FromLTRB(bounds.Right - this.TextBoxWidth, bounds.Top, bounds.Right, bounds.Bottom);

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

            w += this.TextBoxWidth;

            switch (e.SizeMode)
            {
                case RibbonElementSizeMode.Large:
                    w += iwidth + lwidth + spacing;
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
                this.Canvas.Cursor = this.AllowTextEdit ? Cursors.IBeam : Cursors.Default;
            }
        }

        public override void OnMouseLeave(MouseEventArgs e)
        {
            if (!this.Enabled)
            {
                return;
            }

            base.OnMouseLeave(e);

            this.Canvas.Cursor = Cursors.Default;
        }

        public override void OnMouseDown(MouseEventArgs e)
        {
            if (!this.Enabled)
            {
                return;
            }

            base.OnMouseDown(e);

            if (this.TextBoxBounds.Contains(e.X, e.Y) && this._AllowTextEdit)
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

            if (this.TextBoxBounds.Contains(e.X, e.Y) && this.AllowTextEdit)
            {
                this.Owner.Cursor = Cursors.IBeam;
            }
            else
            {
                this.Owner.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// Raises the <see cref="TextBoxTextChanged"/> event
        /// </summary>
        /// <param name="e"></param>
        public void OnTextChanged(EventArgs e)
        {
            if (!this.Enabled)
            {
                return;
            }

            this.NotifyOwnerRegionsChanged();

            if (this.TextBoxTextChanged != null)
            {
                this.TextBoxTextChanged(this, e);
            }
        }

        #endregion
    }
}