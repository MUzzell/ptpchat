namespace PtpChat.Main.Ribbon.Component_Classes
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Windows.Forms;
    using System.Windows.Forms.VisualStyles;

    using RibbonElementMeasureSizeEventArgs = PtpChat.Main.Ribbon.Classes.EventArgs.RibbonElementMeasureSizeEventArgs;
    using RibbonElementPaintEventArgs = PtpChat.Main.Ribbon.Classes.EventArgs.RibbonElementPaintEventArgs;
    using RibbonElementSizeMode = PtpChat.Main.Ribbon.Classes.Enums.RibbonElementSizeMode;
    using RibbonItemRenderEventArgs = PtpChat.Main.Ribbon.Classes.EventArgs.RibbonItemRenderEventArgs;
    using RibbonTextEventArgs = PtpChat.Main.Ribbon.Classes.EventArgs.RibbonTextEventArgs;

    public class RibbonCheckBox : RibbonItem
    {
        public enum CheckBoxOrientationEnum
        {
            Left = 0,

            Right = 1
        }

        public enum CheckBoxStyle
        {
            CheckBox,

            RadioButton
        }

        #region Ctor

        public RibbonCheckBox()
        {
            this._checkboxSize = 16;
        }

        #endregion

        #region Fields

        private const int spacing = 3;

        private Rectangle _labelBounds;

        private Rectangle _checkboxBounds;

        private int _labelWidth;

        private int _checkboxSize;

        private CheckBoxOrientationEnum _CheckBoxOrientation;

        private CheckBoxStyle _style;

        #endregion

        #region Events

        /// <summary>
        /// Raised when the <see cref="CheckBox Checked"/> property value has changed
        /// </summary>
        public event EventHandler CheckBoxCheckChanged;

        /// <summary>
        /// Raised when the <see cref="CheckBox Checked"/> property value is changing
        /// </summary>
        public event CancelEventHandler CheckBoxCheckChanging;

        #endregion

        #region Props

        /// <summary>
        /// Gets or sets the style of the checkbox item
        /// </summary>
        [DefaultValue(CheckBoxStyle.CheckBox)]
        public CheckBoxStyle Style
        {
            get { return this._style; }
            set
            {
                this._style = value;
                this.NotifyOwnerRegionsChanged();
            }
        }

        /// <summary>
        /// Gets or sets the width of the Label
        /// </summary>
        [DefaultValue(CheckBoxOrientationEnum.Left)]
        public CheckBoxOrientationEnum CheckBoxOrientation
        {
            get { return this._CheckBoxOrientation; }
            set
            {
                this._CheckBoxOrientation = value;
                this.NotifyOwnerRegionsChanged();
            }
        }

        /// <summary>
        /// Gets the bounds of the label that is shown next to the textbox
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual Rectangle LabelBounds => this._labelBounds;

        /// <summary>
        /// Gets a value indicating if the label is currently visible
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool LabelVisible { get; private set; } = true;

        /// <summary>
        /// Gets the bounds of the text
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual Rectangle CheckBoxBounds => this._checkboxBounds;

        /// <summary>
        /// Gets or sets the width of the Label
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
        /// Measures the suposed height of the boxbox
        /// </summary>
        /// <returns></returns>
        protected virtual int MeasureHeight()
        {
            return this._checkboxSize + this.Owner.ItemMargin.Vertical;
        }

        public override void OnPaint(object sender, RibbonElementPaintEventArgs e)
        {
            if (this.Owner != null)
            {
                this.Owner.Renderer.OnRenderRibbonItem(new RibbonItemRenderEventArgs(this.Owner, e.Graphics, this.Bounds, this));

                if (this.Style == CheckBoxStyle.CheckBox)
                {
                    var CheckState = this.Checked ? CheckBoxState.CheckedNormal : CheckBoxState.UncheckedNormal;
                    if (this.Selected)
                    {
                        CheckState += 1;
                    }

                    if (this.CheckBoxOrientation == CheckBoxOrientationEnum.Left)
                    {
                        CheckBoxRenderer.DrawCheckBox(e.Graphics, new Point(this._checkboxBounds.Left, this._checkboxBounds.Top), CheckState);
                    }
                    else
                    {
                        CheckBoxRenderer.DrawCheckBox(e.Graphics, new Point(this._checkboxBounds.Left + spacing, this._checkboxBounds.Top), CheckState);
                    }
                }
                else
                {
                    var RadioState = this.Checked ? RadioButtonState.CheckedNormal : RadioButtonState.UncheckedNormal;
                    if (this.Selected)
                    {
                        RadioState += 1;
                    }
                    if (this.CheckBoxOrientation == CheckBoxOrientationEnum.Left)
                    {
                        RadioButtonRenderer.DrawRadioButton(e.Graphics, new Point(this._checkboxBounds.Left, this._checkboxBounds.Top), RadioState);
                    }
                    else
                    {
                        RadioButtonRenderer.DrawRadioButton(e.Graphics, new Point(this._checkboxBounds.Left + spacing, this._checkboxBounds.Top), RadioState);
                    }
                }

                if (this.LabelVisible)
                {
                    var f = new StringFormat();
                    if (this._CheckBoxOrientation == CheckBoxOrientationEnum.Left)
                    {
                        f.Alignment = StringAlignment.Near;
                    }
                    else
                    {
                        f.Alignment = StringAlignment.Far;
                    }

                    f.LineAlignment = StringAlignment.Far; //Top
                    f.Trimming = StringTrimming.None;
                    f.FormatFlags |= StringFormatFlags.NoWrap;
                    this.Owner.Renderer.OnRenderRibbonItemText(new RibbonTextEventArgs(this.Owner, e.Graphics, this._labelBounds, this, this.LabelBounds, this.Text, f));
                }
            }
        }

        public override void SetBounds(Rectangle bounds)
        {
            base.SetBounds(bounds);
            if (this.CheckBoxOrientation == CheckBoxOrientationEnum.Left)
            {
                this._checkboxBounds = new Rectangle(
                    bounds.Left + this.Owner.ItemMargin.Left,
                    bounds.Top + this.Owner.ItemMargin.Top + (bounds.Height - this._checkboxSize) / 2,
                    this._checkboxSize,
                    this._checkboxSize);

                this._labelBounds = Rectangle.FromLTRB(
                    this._checkboxBounds.Right,
                    bounds.Top + this.Owner.ItemMargin.Top,
                    bounds.Right - this.Owner.ItemMargin.Right,
                    bounds.Bottom - this.Owner.ItemMargin.Bottom);
            }
            else
            {
                this._checkboxBounds = new Rectangle(
                    bounds.Right - this.Owner.ItemMargin.Right - this._checkboxSize,
                    bounds.Top + this.Owner.ItemMargin.Top + (bounds.Height - this._checkboxSize) / 2,
                    this._checkboxSize,
                    this._checkboxSize);

                this._labelBounds = Rectangle.FromLTRB(
                    bounds.Left + this.Owner.ItemMargin.Left,
                    bounds.Top + this.Owner.ItemMargin.Top,
                    this._checkboxBounds.Left,
                    bounds.Bottom - this.Owner.ItemMargin.Bottom);
            }

            if (this.SizeMode == RibbonElementSizeMode.Large)
            {
                this.LabelVisible = true;
            }
            else if (this.SizeMode == RibbonElementSizeMode.Medium)
            {
                this.LabelVisible = true;
                this._labelBounds = Rectangle.Empty;
            }
            else if (this.SizeMode == RibbonElementSizeMode.Compact)
            {
                this._labelBounds = Rectangle.Empty;
                this.LabelVisible = false;
            }
        }

        private bool checkedGlyphSize;

        public override Size MeasureSize(object sender, RibbonElementMeasureSizeEventArgs e)
        {
            if (!this.checkedGlyphSize)
            {
                try
                {
                    if (this.Style == CheckBoxStyle.CheckBox)
                    {
                        this._checkboxSize = CheckBoxRenderer.GetGlyphSize(e.Graphics, this.Checked ? CheckBoxState.CheckedNormal : CheckBoxState.UncheckedNormal).Height + spacing;
                    }
                    else
                    {
                        this._checkboxSize = CheckBoxRenderer.GetGlyphSize(e.Graphics, this.Checked ? CheckBoxState.CheckedNormal : CheckBoxState.UncheckedNormal).Height + spacing;
                    }
                }
                catch
                {
                    /* I don't mind at all */
                }
                this.checkedGlyphSize = true;
            }

            if (!this.Visible && !this.Owner.IsDesignMode())
            {
                this.SetLastMeasuredSize(new Size(0, 0));
                return this.LastMeasuredSize;
            }

            var size = Size.Empty;

            var w = this.Owner.ItemMargin.Horizontal;
            var iwidth = this.Image != null ? this.Image.Width + spacing : 0;
            var lwidth = string.IsNullOrEmpty(this.Text) ? 0 : this._labelWidth > 0 ? this._labelWidth : e.Graphics.MeasureString(this.Text, this.Owner.Font).ToSize().Width;

            w += this._checkboxSize;

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

            //Canvas.Cursor = Cursors.Default;
            //SetSelected(true);
        }

        public override void OnMouseLeave(MouseEventArgs e)
        {
            if (!this.Enabled)
            {
                return;
            }

            base.OnMouseLeave(e);

            //Canvas.Cursor = Cursors.Default;
            //SetSelected(false);
        }

        public override void OnMouseDown(MouseEventArgs e)
        {
            if (!this.Enabled)
            {
                return;
            }

            base.OnMouseDown(e);

            if (this.CheckBoxBounds.Contains(e.X, e.Y))
            {
                var cev = new CancelEventArgs();
                this.OnCheckChanging(cev);
                if (!cev.Cancel)
                {
                    this.Checked = !this.Checked;
                    this.OnCheckChanged(e);
                }
            }
        }

        public override void OnMouseMove(MouseEventArgs e)
        {
            if (!this.Enabled)
            {
                return;
            }

            base.OnMouseMove(e);

            if (this.CheckBoxBounds.Contains(e.X, e.Y))
            {
                Debug.WriteLine("Owner.Cursor = Cursors.Hand e.X=" + e.X + " e.Y=" + e.Y + " CheckBoxBounds (" + this.CheckBoxBounds + ")");
                this.Owner.Cursor = Cursors.Hand;

                if (!this.Selected)
                {
                    this.SetSelected(true);
                }
            }
            else
            {
                Debug.WriteLine("Owner.Cursor = Cursors.Default e.X=" + e.X + " e.Y=" + e.Y + " CheckBoxBounds (" + this.CheckBoxBounds + ")");
                this.Owner.Cursor = Cursors.Default;

                if (this.Selected)
                {
                    this.SetSelected(false);
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="CheckBox Check Changed"/> event
        /// </summary>
        /// <param name="e"></param>
        public void OnCheckChanged(EventArgs e)
        {
            if (!this.Enabled)
            {
                return;
            }

            this.NotifyOwnerRegionsChanged();

            if (this.CheckBoxCheckChanged != null)
            {
                this.CheckBoxCheckChanged(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="CheckBox Check Changing"/> event
        /// </summary>
        /// <param name="e"></param>
        public void OnCheckChanging(CancelEventArgs e)
        {
            if (!this.Enabled)
            {
                return;
            }

            if (this.CheckBoxCheckChanging != null)
            {
                this.CheckBoxCheckChanging(this, e);
            }
        }

        #endregion
    }
}