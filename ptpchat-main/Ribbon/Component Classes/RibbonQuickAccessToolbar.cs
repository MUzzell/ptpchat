namespace PtpChat.Main.Ribbon.Component_Classes
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    using IContainsRibbonComponents = PtpChat.Main.Ribbon.Classes.Interfaces.IContainsRibbonComponents;
    using IContainsSelectableRibbonItems = PtpChat.Main.Ribbon.Classes.Interfaces.IContainsSelectableRibbonItems;
    using RibbonButtonStyle = PtpChat.Main.Ribbon.Classes.Enums.RibbonButtonStyle;
    using RibbonDesigner = PtpChat.Main.Ribbon.Classes.Designers.RibbonDesigner;
    using RibbonElementMeasureSizeEventArgs = PtpChat.Main.Ribbon.Classes.EventArgs.RibbonElementMeasureSizeEventArgs;
    using RibbonElementPaintEventArgs = PtpChat.Main.Ribbon.Classes.EventArgs.RibbonElementPaintEventArgs;
    using RibbonElementSizeMode = PtpChat.Main.Ribbon.Classes.Enums.RibbonElementSizeMode;
    using RibbonItemCollection = PtpChat.Main.Ribbon.Classes.Collections.RibbonItemCollection;
    using RibbonMouseSensor = PtpChat.Main.Ribbon.Classes.RibbonMouseSensor;
    using RibbonOrbStyle = PtpChat.Main.Ribbon.Classes.Enums.RibbonOrbStyle;
    using RibbonProfessionalRenderer = PtpChat.Main.Ribbon.Classes.Renderers.RibbonProfessionalRenderer;
    using RibbonQuickAccessToolbarItemCollection = PtpChat.Main.Ribbon.Classes.Collections.RibbonQuickAccessToolbarItemCollection;
    using RibbonRenderEventArgs = PtpChat.Main.Ribbon.Classes.EventArgs.RibbonRenderEventArgs;
    using Theme = PtpChat.Main.Ribbon.Classes.Theme;

    /// <summary>
    /// Represents a quick access toolbar hosted on the Ribbon
    /// </summary>
    public class RibbonQuickAccessToolbar : RibbonItem, IContainsSelectableRibbonItems, IContainsRibbonComponents
    {
        #region IContainsRibbonComponents Members

        public IEnumerable<Component> GetAllChildComponents()
        {
            return this.Items.ToArray();
        }

        #endregion

        #region Fields

        private readonly RibbonQuickAccessToolbarItemCollection _items;

        private bool _DropDownButtonVisible;

        #endregion

        #region Ctor

        internal RibbonQuickAccessToolbar(Ribbon ownerRibbon)
        {
            if (ownerRibbon == null)
            {
                throw new ArgumentNullException("ownerRibbon");
            }

            this.SetOwner(ownerRibbon);

            this.DropDownButton = new RibbonButton();
            this.DropDownButton.SetOwner(ownerRibbon);
            this.DropDownButton.SmallImage = this.CreateDropDownButtonImage();
            this.DropDownButton.Style = RibbonButtonStyle.DropDown;

            this.Margin = new Padding(9);
            this.Padding = new Padding(3, 0, 0, 0);
            this._items = new RibbonQuickAccessToolbarItemCollection(this);
            this.Sensor = new RibbonMouseSensor(ownerRibbon, ownerRibbon, this.Items);
            this._DropDownButtonVisible = true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && RibbonDesigner.Current == null)
            {
                foreach (var item in this._items)
                {
                    item.Dispose();
                }
                this.DropDownButton.Dispose();
                this.Sensor.Dispose();
            }
            base.Dispose(disposing);
        }

        private Image CreateDropDownButtonImage()
        {
            var bmp = new Bitmap(7, 7);
            var renderer = this.Owner.Renderer as RibbonProfessionalRenderer;

            var dk = Color.Navy;
            var lt = Color.White;

            if (renderer != null)
            {
                dk = Theme.ColorTable.Arrow;
                lt = Theme.ColorTable.ArrowLight;
            }

            using (var g = Graphics.FromImage(bmp))
            {
                this.DrawDropDownButtonArrow(g, lt, 0, 1);
                this.DrawDropDownButtonArrow(g, dk, 0, 0);
            }

            return bmp;
        }

        private void DrawDropDownButtonArrow(Graphics g, Color c, int x, int y)
        {
            using (var p = new Pen(c))
            {
                using (var b = new SolidBrush(c))
                {
                    g.DrawLine(p, x, y, x + 4, y);
                    g.FillPolygon(b, new[] { new Point(x, y + 3), new Point(x + 5, y + 3), new Point(x + 2, y + 6) });
                }
            }
        }

        #endregion

        #region Properties

        [Description("Shows or hides the dropdown button of the toolbar")]
        [DefaultValue(true)]
        public bool DropDownButtonVisible
        {
            get { return this._DropDownButtonVisible; }
            set
            {
                this._DropDownButtonVisible = value;
                this.Owner.OnRegionsChanged();
            }
        }

        /// <summary>
        /// Gets the bounds of the toolbar including the graphic adornments
        /// </summary>
        [Browsable(false)]
        internal Rectangle SuperBounds => Rectangle.FromLTRB(this.Bounds.Left - this.Padding.Horizontal, this.Bounds.Top, this.DropDownButton.Bounds.Right, this.Bounds.Bottom);

        /// <summary>
        /// Gets the dropdown button
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RibbonButton DropDownButton { get; }

        [Description("The drop down items of the dropdown button of the toolbar")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public RibbonItemCollection DropDownButtonItems => this.DropDownButton.DropDownItems;

        /// <summary>
        /// Gets or sets the padding (internal) of the toolbar
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Padding Padding { get; }

        /// <summary>
        /// Gets or sets the margin (external) of the toolbar
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Padding Margin { get; }

        /// <summary>
        /// Gets or sets a value indicating if the button that shows the menu of the 
        /// QuickAccess toolbar should be visible
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool MenuButtonVisible { get; set; }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RibbonMouseSensor Sensor { get; }

        /// <summary>
        /// Gets the Items of the QuickAccess toolbar.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public RibbonQuickAccessToolbarItemCollection Items
        {
            get
            {
                if (this.DropDownButtonVisible)
                {
                    if (!this._items.Contains(this.DropDownButton))
                    {
                        this._items.Add(this.DropDownButton);
                    }
                }
                else
                {
                    if (this._items.Contains(this.DropDownButton))
                    {
                        this._items.Remove(this.DropDownButton);
                    }
                }
                return this._items;
            }
        }

        #endregion

        #region Methods

        public override void OnPaint(object sender, RibbonElementPaintEventArgs e)
        {
            if (this.Visible && this.Owner.CaptionBarVisible)
            {
                if (this.Owner.OrbStyle == RibbonOrbStyle.Office_2007)
                {
                    this.Owner.Renderer.OnRenderRibbonQuickAccessToolbarBackground(new RibbonRenderEventArgs(this.Owner, e.Graphics, e.Clip));
                }

                foreach (var item in this.Items)
                {
                    item.OnPaint(this, new RibbonElementPaintEventArgs(item.Bounds, e.Graphics, RibbonElementSizeMode.Compact));
                }
            }
        }

        public override Size MeasureSize(object sender, RibbonElementMeasureSizeEventArgs e)
        {
            ///For RibbonItemGroup, size is always compact, and it's designed to be on an horizontal flow
            ///tab panel.
            ///
            if (!this.Visible || !this.Owner.CaptionBarVisible)
            {
                this.SetLastMeasuredSize(new Size(0, 0));
                return this.LastMeasuredSize;
            }

            var widthSum = this.Padding.Horizontal;
            var maxHeight = 16;

            foreach (var item in this.Items)
            {
                if (item.Equals(this.DropDownButton))
                {
                    continue;
                }
                item.SetSizeMode(RibbonElementSizeMode.Compact);
                var s = item.MeasureSize(this, new RibbonElementMeasureSizeEventArgs(e.Graphics, RibbonElementSizeMode.Compact));
                widthSum += s.Width + 1;
                maxHeight = Math.Max(maxHeight, s.Height);
            }

            widthSum -= 1;

            if (this.Site != null && this.Site.DesignMode)
            {
                widthSum += 16;
            }

            var result = new Size(widthSum, maxHeight);
            this.SetLastMeasuredSize(result);
            return result;
        }

        public override void SetBounds(Rectangle bounds)
        {
            base.SetBounds(bounds);

            if (this.Owner.RightToLeft == RightToLeft.No)
            {
                var curLeft = bounds.Left + this.Padding.Left;

                foreach (var item in this.Items)
                {
                    item.SetBounds(new Rectangle(new Point(curLeft, bounds.Top), item.LastMeasuredSize));

                    curLeft = item.Bounds.Right + 1;
                }

                this.DropDownButton.SetBounds(new Rectangle(bounds.Right + bounds.Height / 2 + 2, bounds.Top, 12, bounds.Height));
            }
            else
            {
                var curLeft = bounds.Left + this.Padding.Left;

                for (var i = this.Items.Count - 1; i >= 0; i--)
                {
                    this.Items[i].SetBounds(new Rectangle(new Point(curLeft, bounds.Top), this.Items[i].LastMeasuredSize));

                    curLeft = this.Items[i].Bounds.Right + 1;
                }

                this.DropDownButton.SetBounds(new Rectangle(bounds.Left - bounds.Height / 2 - 14, bounds.Top, 12, bounds.Height));
            }
        }

        #endregion

        #region IContainsSelectableRibbonItems Members

        public IEnumerable<RibbonItem> GetItems()
        {
            return this.Items;
        }

        public Rectangle GetContentBounds()
        {
            return this.Bounds;
        }

        #endregion
    }
}