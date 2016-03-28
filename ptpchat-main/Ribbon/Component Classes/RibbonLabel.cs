namespace PtpChat.Main.Ribbon.Component_Classes
{
    using System.ComponentModel;
    using System.Drawing;

    using PtpChat.Main.Ribbon.Classes.EventArgs;

    public class RibbonLabel : RibbonItem
    {
        #region Properties

        [Description("Sets the width of the label portion of the control")]
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

        #region Fields

        private int _labelWidth;

        private const int spacing = 3;

        #endregion

        #region Methods

        protected virtual int MeasureHeight()
        {
            if (this.Owner != null)
            {
                return 16 + this.Owner.ItemMargin.Vertical;
            }
            return 16 + 4;
        }

        /// <summary>
        /// Measures the size of the panel on the mode specified by the event object
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public override Size MeasureSize(object sender, RibbonElementMeasureSizeEventArgs e)
        {
            if (!this.Visible && ((this.Site == null) || !this.Site.DesignMode))
            {
                return new Size(0, 0);
            }
            var f = new Font("Microsoft Sans Serif", 8);
            if (this.Owner != null)
            {
                f = this.Owner.Font;
            }

            var w = string.IsNullOrEmpty(this.Text) ? 0 : (this._labelWidth > 0 ? this._labelWidth : e.Graphics.MeasureString(this.Text, f).ToSize().Width + 6);
            this.SetLastMeasuredSize(new Size(w, this.MeasureHeight()));
            return this.LastMeasuredSize;
        }

        /// <summary>
        /// Raises the paint event and draws the
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void OnPaint(object sender, RibbonElementPaintEventArgs e)
        {
            if (this.Owner != null)
            {
                this.Owner.Renderer.OnRenderRibbonItem(new RibbonItemRenderEventArgs(this.Owner, e.Graphics, this.Bounds, this));
                var f = new StringFormat();
                f.Alignment = (StringAlignment)this.TextAlignment;
                f.LineAlignment = StringAlignment.Center;
                f.Trimming = StringTrimming.None;
                f.FormatFlags |= StringFormatFlags.NoWrap;
                var clipBounds = Rectangle.FromLTRB(this.Bounds.Left + 3, this.Bounds.Top + this.Owner.ItemMargin.Top, this.Bounds.Right - 3, this.Bounds.Bottom - this.Owner.ItemMargin.Bottom);
                this.Owner.Renderer.OnRenderRibbonItemText(new RibbonTextEventArgs(this.Owner, e.Graphics, this.Bounds, this, clipBounds, this.Text, f));
            }
        }

        /// <summary>
        /// Sets the bounds of the panel
        /// </summary>
        /// <param name="bounds"></param>
        public override void SetBounds(Rectangle bounds)
        {
            base.SetBounds(bounds);
        }

        #endregion
    }
}