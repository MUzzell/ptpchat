namespace PtpChat.Main.Ribbon.Component_Classes
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;

    using PtpChat.Main.Ribbon.Classes.Collections;
    using PtpChat.Main.Ribbon.Classes.Designers;
    using PtpChat.Main.Ribbon.Classes.Enums;
    using PtpChat.Main.Ribbon.Classes.EventArgs;
    using PtpChat.Main.Ribbon.Classes.Interfaces;

    //[Designer("System.Windows.Forms.RibbonQuickAccessToolbarDesigner")]
    [Designer(typeof(RibbonItemGroupDesigner))]
    public class RibbonItemGroup : RibbonItem, IContainsSelectableRibbonItems, IContainsRibbonComponents
    {
        #region IContainsRibbonComponents Members

        public IEnumerable<Component> GetAllChildComponents()
        {
            return this.Items.ToArray();
        }

        #endregion

        #region Fields

        #endregion

        #region Ctor

        public RibbonItemGroup()
        {
            this.Items = new RibbonItemGroupItemCollection(this);
            this.DrawBackground = true;
        }

        public RibbonItemGroup(IEnumerable<RibbonItem> items)
            : this()
        {
            this.Items.AddRange(items);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && RibbonDesigner.Current == null)
            {
                foreach (var ri in this.Items)
                {
                    ri.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        #endregion

        #region Props

        /// <summary>
        /// This property is not relevant for this class
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override bool Checked { get { return base.Checked; } set { base.Checked = value; } }

        /// <summary>
        /// Gets or sets a value indicating if the group should
        /// </summary>
        [DefaultValue(true)]
        [Description("Background drawing should be avoided when group contains only TextBoxes and ComboBoxes")]
        public bool DrawBackground { get; set; }

        /// <summary>
        /// Gets the first item of the group
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RibbonItem FirstItem
        {
            get
            {
                if (this.Items.Count > 0)
                {
                    return this.Items[0];
                }

                return null;
            }
        }

        /// <summary>
        /// Gets the last item of the group
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RibbonItem LastItem
        {
            get
            {
                if (this.Items.Count > 0)
                {
                    return this.Items[this.Items.Count - 1];
                }

                return null;
            }
        }

        /// <summary>
        /// Gets the collection of items of this group
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public RibbonItemGroupItemCollection Items { get; }

        #endregion

        #region Methods

        protected override bool ClosesDropDownAt(Point p)
        {
            return false;
        }

        public override void SetBounds(Rectangle bounds)
        {
            base.SetBounds(bounds);

            var curLeft = bounds.Left;

            foreach (var item in this.Items)
            {
                item.SetBounds(new Rectangle(new Point(curLeft, bounds.Top), item.LastMeasuredSize));

                curLeft = item.Bounds.Right + 1;
            }
        }

        public override void OnPaint(object sender, RibbonElementPaintEventArgs e)
        {
            if (this.DrawBackground)
            {
                this.Owner.Renderer.OnRenderRibbonItem(new RibbonItemRenderEventArgs(this.Owner, e.Graphics, e.Clip, this));
            }

            foreach (var item in this.Items)
            {
                item.OnPaint(this, new RibbonElementPaintEventArgs(item.Bounds, e.Graphics, RibbonElementSizeMode.Compact));
            }

            if (this.DrawBackground)
            {
                this.Owner.Renderer.OnRenderRibbonItemBorder(new RibbonItemRenderEventArgs(this.Owner, e.Graphics, e.Clip, this));
            }
        }

        public override Size MeasureSize(object sender, RibbonElementMeasureSizeEventArgs e)
        {
            if (!this.Visible && !this.Owner.IsDesignMode())
            {
                this.SetLastMeasuredSize(new Size(0, 0));
                return this.LastMeasuredSize;
            }

            ///For RibbonItemGroup, size is always compact, and it's designed to be on an horizontal flow
            ///tab panel.
            ///
            var minWidth = 16;
            var widthSum = 0;
            var maxHeight = 16;

            foreach (var item in this.Items)
            {
                var s = item.MeasureSize(this, new RibbonElementMeasureSizeEventArgs(e.Graphics, RibbonElementSizeMode.Compact));
                widthSum += s.Width + 1;
                maxHeight = Math.Max(maxHeight, s.Height);
            }

            widthSum -= 1;

            widthSum = Math.Max(widthSum, minWidth);

            if (this.Site != null && this.Site.DesignMode)
            {
                widthSum += 10;
            }

            var result = new Size(widthSum, maxHeight);
            this.SetLastMeasuredSize(result);
            return result;
        }

        /// <param name="ownerPanel">RibbonPanel where this item is located</param>
        internal override void SetOwnerPanel(RibbonPanel ownerPanel)
        {
            base.SetOwnerPanel(ownerPanel);

            this.Items.SetOwnerPanel(ownerPanel);
        }

        /// <param name="owner">Ribbon that owns this item</param>
        internal override void SetOwner(Ribbon owner)
        {
            base.SetOwner(owner);

            this.Items.SetOwner(owner);
        }

        /// <param name="ownerTab">RibbonTab where this item is located</param>
        internal override void SetOwnerTab(RibbonTab ownerTab)
        {
            base.SetOwnerTab(ownerTab);

            this.Items.SetOwnerTab(ownerTab);
        }

        internal override void SetSizeMode(RibbonElementSizeMode sizeMode)
        {
            base.SetSizeMode(sizeMode);

            foreach (var item in this.Items)
            {
                item.SetSizeMode(RibbonElementSizeMode.Compact);
            }
        }

        public override string ToString()
        {
            return "Group: " + this.Items.Count + " item(s)";
        }

        #endregion

        #region IContainsRibbonItems Members

        public IEnumerable<RibbonItem> GetItems()
        {
            return this.Items;
        }

        public Rectangle GetContentBounds()
        {
            return Rectangle.FromLTRB(this.Bounds.Left + 1, this.Bounds.Top + 1, this.Bounds.Right - 1, this.Bounds.Bottom);
        }

        #endregion
    }
}