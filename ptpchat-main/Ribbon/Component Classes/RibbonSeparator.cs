namespace PtpChat.Main.Ribbon.Component_Classes
{
    using System.ComponentModel;
    using System.Drawing;

    using PtpChat.Main.Ribbon.Classes.Enums;
    using PtpChat.Main.Ribbon.Classes.EventArgs;

    public sealed class RibbonSeparator : RibbonItem
    {
        public RibbonSeparator()
        {
        }

        public RibbonSeparator(string text)
        {
            this.Text = text;
        }

        /// <summary>
        /// Gets or sets a value indicating if the separator should draw the divider lines
        /// </summary>
        [DefaultValue(true)]
        [Description("Background drawing should be avoided when group contains only TextBoxes and ComboBoxes")]
        public bool DrawBackground { get; set; } = true;

        public override void SetBounds(Rectangle bounds)
        {
            base.SetBounds(bounds);
        }

        public override void OnPaint(object sender, RibbonElementPaintEventArgs e)
        {
            if ((this.Owner == null || !this.DrawBackground) && !this.Owner.IsDesignMode())
            {
                return;
            }

            this.Owner.Renderer.OnRenderRibbonItem(new RibbonItemRenderEventArgs(this.Owner, e.Graphics, e.Clip, this));

            if (!string.IsNullOrEmpty(this.Text))
            {
                this.Owner.Renderer.OnRenderRibbonItemText(
                    new RibbonTextEventArgs(
                        this.Owner,
                        e.Graphics,
                        e.Clip,
                        this,
                        Rectangle.FromLTRB(
                            this.Bounds.Left + this.Owner.ItemMargin.Left,
                            this.Bounds.Top + this.Owner.ItemMargin.Top,
                            this.Bounds.Right - this.Owner.ItemMargin.Right,
                            this.Bounds.Bottom - this.Owner.ItemMargin.Bottom),
                        this.Text,
                        FontStyle.Bold));
            }
        }

        public override Size MeasureSize(object sender, RibbonElementMeasureSizeEventArgs e)
        {
            if (e.SizeMode == RibbonElementSizeMode.DropDown)
            {
                if (string.IsNullOrEmpty(this.Text))
                {
                    this.SetLastMeasuredSize(new Size(1, 3));
                }
                else
                {
                    var sz = e.Graphics.MeasureString(this.Text, new Font(this.Owner.Font, FontStyle.Bold)).ToSize();
                    this.SetLastMeasuredSize(new Size(sz.Width + this.Owner.ItemMargin.Horizontal, sz.Height + this.Owner.ItemMargin.Vertical));
                }
            }
            else
            {
                this.SetLastMeasuredSize(new Size(2, this.OwnerPanel.ContentBounds.Height - this.Owner.ItemPadding.Vertical - this.Owner.ItemMargin.Vertical));
            }

            return this.LastMeasuredSize;
        }
    }
}