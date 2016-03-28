namespace PtpChat.Main.Ribbon.Component_Classes
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    using PtpChat.Main.Ribbon.Classes.Designers;

    using IContainsRibbonComponents = PtpChat.Main.Ribbon.Classes.Interfaces.IContainsRibbonComponents;
    using IContainsSelectableRibbonItems = PtpChat.Main.Ribbon.Classes.Interfaces.IContainsSelectableRibbonItems;
    using IRibbonElement = PtpChat.Main.Ribbon.Classes.Interfaces.IRibbonElement;
    using RibbonDesigner = PtpChat.Main.Ribbon.Classes.Designers.RibbonDesigner;
    using RibbonElementMeasureSizeEventArgs = PtpChat.Main.Ribbon.Classes.EventArgs.RibbonElementMeasureSizeEventArgs;
    using RibbonElementPaintEventArgs = PtpChat.Main.Ribbon.Classes.EventArgs.RibbonElementPaintEventArgs;
    using RibbonElementSizeMode = PtpChat.Main.Ribbon.Classes.Enums.RibbonElementSizeMode;
    using RibbonItemCollection = PtpChat.Main.Ribbon.Classes.Collections.RibbonItemCollection;
    using RibbonPanelFlowDirection = PtpChat.Main.Ribbon.Classes.Enums.RibbonPanelFlowDirection;
    using RibbonPanelRenderEventArgs = PtpChat.Main.Ribbon.Classes.EventArgs.RibbonPanelRenderEventArgs;

    [DesignTimeVisible(false)]
    [Designer(typeof(RibbonPanelDesigner))]
    public class RibbonPanel : Component, IRibbonElement, IContainsSelectableRibbonItems, IContainsRibbonComponents
    {
        #region IContainsRibbonComponents Members

        public IEnumerable<Component> GetAllChildComponents()
        {
            return this.Items.ToArray();
        }

        #endregion

        #region Fields

        private bool _enabled;

        private Image _image;

        private string _text;

        private bool _selected;

        private RibbonPanelFlowDirection _flowsTo;

        private bool _buttonMoreVisible;

        private bool _buttonMoreEnabled;

        internal Rectangle overflowBoundsBuffer;

        private bool _visible = true;

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the mouse pointer enters the panel
        /// </summary>
        public event MouseEventHandler MouseEnter;

        /// <summary>
        /// Occurs when the mouse pointer leaves the panel
        /// </summary>
        public event MouseEventHandler MouseLeave;

        /// <summary>
        /// Occurs when the mouse pointer is moved inside the panel
        /// </summary>
        public event MouseEventHandler MouseMove;

        /// <summary>
        /// Occurs when the panel is redrawn
        /// </summary>
        public event PaintEventHandler Paint;

        /// <summary>
        /// Occurs when the panel is resized
        /// </summary>
        public event EventHandler Resize;

        public event EventHandler ButtonMoreClick;

        public virtual event EventHandler Click;

        public virtual event EventHandler DoubleClick;

        public virtual event MouseEventHandler MouseDown;

        public virtual event MouseEventHandler MouseUp;

        #endregion

        #region Ctor

        /// <summary>
        /// Creates a new RibbonPanel
        /// </summary>
        public RibbonPanel()
        {
            this.Items = new RibbonItemCollection();
            this.SizeMode = RibbonElementSizeMode.None;
            this._flowsTo = RibbonPanelFlowDirection.Bottom;
            this._buttonMoreEnabled = true;
            this._buttonMoreVisible = true;
            this.Items.SetOwnerPanel(this);
            this._enabled = true;
        }

        /// <summary>
        /// Creates a new RibbonPanel with the specified text
        /// </summary>
        /// <param name="text">Text of the panel</param>
        public RibbonPanel(string text)
            : this(text, RibbonPanelFlowDirection.Bottom)
        {
        }

        /// <summary>
        /// Creates a new RibbonPanel with the specified text and panel flow direction
        /// </summary>
        /// <param name="text">Text of the panel</param>
        /// <param name="flowsTo">Flow direction of the content items</param>
        public RibbonPanel(string text, RibbonPanelFlowDirection flowsTo)
            : this(text, flowsTo, new RibbonItem[] { })
        {
        }

        /// <summary>
        /// Creates a new RibbonPanel with the specified text and panel flow direction
        /// </summary>
        /// <param name="text">Text of the panel</param>
        /// <param name="flowsTo">Flow direction of the content items</param>
        public RibbonPanel(string text, RibbonPanelFlowDirection flowsTo, IEnumerable<RibbonItem> items)
            : this()
        {
            this._text = text;
            this._flowsTo = flowsTo;
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

        [Description("Sets if the panel should be enabled")]
        [DefaultValue(true)]
        public bool Enabled
        {
            get { return this._enabled; }
            set
            {
                this._enabled = value;

                foreach (var item in this.Items)
                {
                    item.Enabled = value;
                }
            }
        }

        [Description("Sets if the panel should be Visible")]
        [DefaultValue(true)]
        public virtual bool Visible
        {
            get { return this._visible; }
            set
            {
                this._visible = value;
                //this.OwnerTab.UpdatePanelsRegions();
                this.OwnerTab.Owner.PerformLayout();
                this.Owner.Invalidate();
            }
        }

        /// <summary>
        /// Gets if this panel is currenlty collapsed
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Collapsed => this.SizeMode == RibbonElementSizeMode.Overflow;

        /// <summary>
        /// Gets or sets the visibility of the "More" button
        /// </summary>
        [Description("Sets the visibility of the \"More...\" button")]
        [DefaultValue(true)]
        public bool ButtonMoreVisible
        {
            get { return this._buttonMoreVisible; }
            set
            {
                this._buttonMoreVisible = value;
                if (this.Owner != null)
                {
                    this.Owner.OnRegionsChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating if the "More" button should be enabled
        /// </summary>
        [Description(@"Enables/Disables the ""More..."" button")]
        [DefaultValue(true)]
        public bool ButtonMoreEnabled
        {
            get { return this._buttonMoreEnabled; }
            set
            {
                this._buttonMoreEnabled = value;
                if (this.Owner != null)
                {
                    this.Owner.OnRegionsChanged();
                }
            }
        }

        /// <summary>
        /// Gets if the "More" button is currently selected
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ButtonMoreSelected { get; private set; }

        /// <summary>
        /// Gets if the "More" button is currently pressed
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ButtonMorePressed { get; private set; }

        /// <summary>
        /// Gets the bounds of the "More" button
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Rectangle ButtonMoreBounds { get; private set; }

        /// <summary>
        /// Gets if the panel is currently on overflow and pressed
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Pressed { get; private set; }

        /// <summary>
        /// Gets or sets the pop up where the panel is being drawn (if any)
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal Control PopUp { get; set; }

        /// <summary>
        /// Gets the current size mode of the panel
        /// </summary>
        public RibbonElementSizeMode SizeMode { get; private set; }

        /// <summary>
        /// Gets the collection of RibbonItem elements of this panel
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public RibbonItemCollection Items { get; }

        /// <summary>
        /// Gets or sets the text that is to be displayed on the bottom of the panel
        /// </summary>
        [Localizable(true)]
        public string Text
        {
            get { return this._text; }
            set
            {
                this._text = value;

                if (this.Owner != null)
                {
                    this.Owner.OnRegionsChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the image that is to be displayed on the panel when shown as an overflow button
        /// </summary>
        [DefaultValue(null)]
        public Image Image
        {
            get { return this._image; }
            set
            {
                this._image = value;

                if (this.Owner != null)
                {
                    this.Owner.OnRegionsChanged();
                }
            }
        }

        /// <summary>
        /// Gets if the panel is in overflow mode
        /// </summary>
        /// <remarks>Overflow mode is when the available space to draw the panel is not enough to draw components, so panel is drawn as a button that shows the full content of the panel in a pop-up window when clicked</remarks>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool OverflowMode => this.SizeMode == RibbonElementSizeMode.Overflow;

        /// <summary>
        /// Gets the Ribbon that contains this panel
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Ribbon Owner { get; private set; }

        /// <summary>
        /// Gets the bounds of the panel relative to the Ribbon control
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Rectangle Bounds { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the panel is selected
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual bool Selected { get { return this._selected; } set { this._selected = value; } }

        /// <summary>
        /// Gets a value indicating whether the panel is the first panel on the tab
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual bool IsFirstPanel { get; set; } = false;

        /// <summary>
        /// Gets a value indicating whether the panel is the last panel on the tab
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual bool IsLastPanel { get; set; } = false;

        /// <summary>
        /// Gets a value indicating what the index of the panel is in the Tabs panel collection
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual int Index { get; set; } = -1;

        /// <summary>
        /// Gets or sets the object that contains data about the control
        /// </summary>
        [Description("An Object field for associating custom data for this control")]
        [DefaultValue(null)]
        [TypeConverter(typeof(StringConverter))]
        public object Tag { get; set; }

        /// <summary>
        /// Gets the bounds of the content of the panel
        /// </summary>
        public Rectangle ContentBounds { get; private set; }

        /// <summary>
        /// Gets the RibbonTab that contains this panel
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RibbonTab OwnerTab { get; private set; }

        /// <summary>
        /// Gets or sets the flow direction to layout items
        /// </summary>
        [DefaultValue(RibbonPanelFlowDirection.Bottom)]
        public RibbonPanelFlowDirection FlowsTo
        {
            get { return this._flowsTo; }
            set
            {
                this._flowsTo = value;

                if (this.Owner != null)
                {
                    this.Owner.OnRegionsChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets if the popup is currently showing
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal bool PopupShowed { get; set; }

        #endregion

        #region IRibbonElement Members

        public Size SwitchToSize(Control ctl, Graphics g, RibbonElementSizeMode size)
        {
            var s = this.MeasureSize(this, new RibbonElementMeasureSizeEventArgs(g, size));
            var r = new Rectangle(0, 0, s.Width, s.Height);

            //if (!(ctl is Ribbon))
            //    r = boundsBuffer;
            //else
            //    r = new Rectangle(0, 0, 0, 0);

            this.SetBounds(r);
            this.UpdateItemsRegions(g, size);
            return s;
        }

        /// <summary>
        /// Raises the paint event and draws the
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void OnPaint(object sender, RibbonElementPaintEventArgs e)
        {
            if (this.Paint != null)
            {
                this.Paint(this, new PaintEventArgs(e.Graphics, e.Clip));
            }

            if (this.PopupShowed && e.Control == this.Owner)
            {
                //Draw a fake collapsed and pressed panel

                #region Create fake panel

                var fakePanel = new RibbonPanel(this.Text);
                fakePanel.Image = this.Image;
                fakePanel.SetSizeMode(RibbonElementSizeMode.Overflow);
                fakePanel.SetBounds(this.overflowBoundsBuffer);
                fakePanel.SetPressed(true);
                fakePanel.SetOwner(this.Owner);

                #endregion

                this.Owner.Renderer.OnRenderRibbonPanelBackground(new RibbonPanelRenderEventArgs(this.Owner, e.Graphics, e.Clip, fakePanel, e.Control));
                this.Owner.Renderer.OnRenderRibbonPanelText(new RibbonPanelRenderEventArgs(this.Owner, e.Graphics, e.Clip, fakePanel, e.Control));
            }
            else
            {
                //Draw normal
                this.Owner.Renderer.OnRenderRibbonPanelBackground(new RibbonPanelRenderEventArgs(this.Owner, e.Graphics, e.Clip, this, e.Control));
                this.Owner.Renderer.OnRenderRibbonPanelText(new RibbonPanelRenderEventArgs(this.Owner, e.Graphics, e.Clip, this, e.Control));
            }

            if (e.Mode != RibbonElementSizeMode.Overflow || (e.Control != null && e.Control == this.PopUp))
            {
                foreach (var item in this.Items)
                {
                    if (item.Visible || this.Owner.IsDesignMode())
                    {
                        item.OnPaint(this, new RibbonElementPaintEventArgs(item.Bounds, e.Graphics, item.SizeMode));
                    }
                }
            }
        }

        /// <summary>
        /// Sets the bounds of the panel
        /// </summary>
        /// <param name="bounds"></param>
        public void SetBounds(Rectangle bounds)
        {
            var trigger = this.Bounds != bounds;

            this.Bounds = bounds;

            this.OnResize(EventArgs.Empty);

            if (this.Owner != null)
            {
                //Update contentBounds
                this.ContentBounds = Rectangle.FromLTRB(
                    bounds.X + this.Owner.PanelMargin.Left + 0,
                    bounds.Y + this.Owner.PanelMargin.Top + 0,
                    bounds.Right - this.Owner.PanelMargin.Right,
                    bounds.Bottom - this.Owner.PanelMargin.Bottom);
            }

            //"More" bounds
            if (this.ButtonMoreVisible)
            {
                this.SetMoreBounds(Rectangle.FromLTRB(bounds.Right - 15, this.ContentBounds.Bottom + 1, bounds.Right, bounds.Bottom));
            }
            else
            {
                this.SetMoreBounds(Rectangle.Empty);
            }
        }

        /// <summary>
        /// Measures the size of the panel on the mode specified by the event object
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public Size MeasureSize(object sender, RibbonElementMeasureSizeEventArgs e)
        {
            var result = Size.Empty;
            var minSize = Size.Empty;

            if (!this.Visible && !this.Owner.IsDesignMode())
            {
                return new Size(0, 0);
            }

            var panelHeight = this.OwnerTab.TabContentBounds.Height - this.Owner.PanelPadding.Vertical;

            #region Measure width of minSize

            minSize.Width = e.Graphics.MeasureString(this.Text, this.Owner.Font).ToSize().Width + this.Owner.PanelMargin.Horizontal + 1;

            if (this.ButtonMoreVisible)
            {
                minSize.Width += this.ButtonMoreBounds.Width + 3;
            }

            #endregion

            if (e.SizeMode == RibbonElementSizeMode.Overflow)
            {
                var textSize = RibbonButton.MeasureStringLargeSize(e.Graphics, this.Text, this.Owner.Font);

                return new Size(textSize.Width + this.Owner.PanelMargin.Horizontal, panelHeight);
            }

            switch (this.FlowsTo)
            {
                case RibbonPanelFlowDirection.Left:
                    result = this.MeasureSizeFlowsToBottom(sender, e);
                    break;
                case RibbonPanelFlowDirection.Right:
                    result = this.MeasureSizeFlowsToRight(sender, e);
                    break;
                case RibbonPanelFlowDirection.Bottom:
                    result = this.MeasureSizeFlowsToBottom(sender, e);
                    break;
                default:
                    result = Size.Empty;
                    break;
            }

            return new Size(Math.Max(result.Width, minSize.Width), panelHeight);
        }

        /// <summary>
        /// Sets the value of the Owner Property
        /// </summary>
        internal void SetOwner(Ribbon owner)
        {
            this.Owner = owner;

            this.Items.SetOwner(owner);
        }

        /// <summary>
        /// Sets the value of the Selected property
        /// </summary>
        /// <param name="selected">Value that indicates if the element is selected</param>
        internal void SetSelected(bool selected)
        {
            this._selected = selected;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Raises the <see cref="Resize"/> method
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnResize(EventArgs e)
        {
            if (this.Resize != null)
            {
                this.Resize(this, e);
            }
        }

        /// <summary>
        /// Shows the panel in a popup
        /// </summary>
        private void ShowOverflowPopup()
        {
            var b = this.Bounds;
            var f = new RibbonPanelPopup(this);
            var p = this.Owner.PointToScreen(new Point(b.Left, b.Bottom));
            this.PopupShowed = true;
            f.Show(p);
        }

        /// <summary>
        /// Measures the size when flow direction is to right
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private Size MeasureSizeFlowsToRight(object sender, RibbonElementMeasureSizeEventArgs e)
        {
            var widthSum = this.Owner.PanelMargin.Horizontal;
            var maxWidth = 0;
            var maxHeight = 0;
            var dividedWidth = 0;

            foreach (var item in this.Items)
            {
                if (item.Visible || this.Owner.IsDesignMode())
                {
                    var itemSize = item.MeasureSize(this, e);

                    widthSum += itemSize.Width + this.Owner.ItemPadding.Horizontal + 1;

                    maxWidth = Math.Max(maxWidth, itemSize.Width);
                    maxHeight = Math.Max(maxHeight, itemSize.Height);
                }
            }

            switch (e.SizeMode)
            {
                case RibbonElementSizeMode.Large:
                    dividedWidth = widthSum / 1; //Show items on one row
                    break;
                case RibbonElementSizeMode.Medium:
                    dividedWidth = widthSum / 2; //Show items on two rows
                    break;
                case RibbonElementSizeMode.Compact:
                    dividedWidth = widthSum / 3; //Show items on three rows
                    break;
                default:
                    break;
            }

            //Add padding
            dividedWidth += this.Owner.PanelMargin.Horizontal;

            return new Size(Math.Max(maxWidth, dividedWidth) + this.Owner.PanelMargin.Horizontal, 0); //Height is provided by MeasureSize
        }

        /// <summary>
        /// Measures the size when flow direction is to bottom
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private Size MeasureSizeFlowsToBottom(object sender, RibbonElementMeasureSizeEventArgs e)
        {
            var curRight = this.Owner.PanelMargin.Left + this.Owner.ItemPadding.Horizontal;
            var curBottom = this.ContentBounds.Top + this.Owner.ItemPadding.Vertical;
            var lastRight = 0;
            var lastBottom = 0;
            var availableHeight = this.OwnerTab.TabContentBounds.Height - this.Owner.TabContentMargin.Vertical - this.Owner.PanelPadding.Vertical - this.Owner.PanelMargin.Vertical;
            var maxRight = 0;
            var maxBottom = 0;

            foreach (var item in this.Items)
            {
                if (item.Visible || this.Owner.IsDesignMode() || item.GetType() == typeof(RibbonSeparator))
                {
                    var itemSize = item.MeasureSize(this, new RibbonElementMeasureSizeEventArgs(e.Graphics, e.SizeMode));

                    if (curBottom + itemSize.Height > this.ContentBounds.Bottom)
                    {
                        curBottom = this.ContentBounds.Top + this.Owner.ItemPadding.Vertical + 0;
                        curRight = maxRight + this.Owner.ItemPadding.Horizontal + 0;
                    }

                    var bounds = new Rectangle(curRight, curBottom, itemSize.Width, itemSize.Height);

                    lastRight = bounds.Right;
                    lastBottom = bounds.Bottom;

                    curBottom = bounds.Bottom + this.Owner.ItemPadding.Vertical + 1;

                    maxRight = Math.Max(maxRight, lastRight);
                    maxBottom = Math.Max(maxBottom, lastBottom);
                }
            }

            return new Size(maxRight + this.Owner.ItemPadding.Right + this.Owner.PanelMargin.Right + 1, 0); //Height is provided by MeasureSize
        }

        /// <summary>
        /// Sets the value of the SizeMode property
        /// </summary>
        /// <param name="sizeMode"></param>
        internal void SetSizeMode(RibbonElementSizeMode sizeMode)
        {
            this.SizeMode = sizeMode;

            foreach (var item in this.Items)
            {
                item.SetSizeMode(sizeMode);
            }
        }

        /// <summary>
        /// Sets the value of the ContentBounds property
        /// </summary>
        /// <param name="contentBounds">Bounds of the content on the panel</param>
        internal void SetContentBounds(Rectangle contentBounds)
        {
            this.ContentBounds = contentBounds;
        }

        /// <summary>
        /// Sets the value of the OwnerTab property
        /// </summary>
        /// <param name="ownerTab">RibbonTab where this item is located</param>
        internal void SetOwnerTab(RibbonTab ownerTab)
        {
            this.OwnerTab = ownerTab;

            this.Items.SetOwnerTab(this.OwnerTab);
        }

        /// <summary>
        /// Updates the bounds of child elements
        /// </summary>
        internal void UpdateItemsRegions(Graphics g, RibbonElementSizeMode mode)
        {
            switch (this.FlowsTo)
            {
                case RibbonPanelFlowDirection.Right:
                    this.UpdateRegionsFlowsToRight(g, mode);
                    break;
                case RibbonPanelFlowDirection.Bottom:
                    this.UpdateRegionsFlowsToBottom(g, mode);
                    break;
                case RibbonPanelFlowDirection.Left:
                    this.UpdateRegionsFlowsToLeft(g, mode);
                    break;
            }

            ///Center items on the panel
            this.CenterItems();
        }

        /// <summary>
        /// Updates the bounds of child elements when flow is to bottom
        /// </summary>
        private void UpdateRegionsFlowsToBottom(Graphics g, RibbonElementSizeMode mode)
        {
            var curRight = this.ContentBounds.Left + this.Owner.ItemPadding.Horizontal + 0;
            var curBottom = this.ContentBounds.Top + this.Owner.ItemPadding.Vertical + 0;
            var lastRight = curRight;
            var lastBottom = 0;
            var lastColumn = new List<RibbonItem>();

            ///Iterate thru items on panel
            foreach (var item in this.Items)
            {
                ///Gets the last measured size (to avoid re-measuring calculations)
                Size itemSize;
                if (item.Visible || this.Owner.IsDesignMode())
                {
                    itemSize = item.LastMeasuredSize;
                }
                else
                {
                    itemSize = new Size(0, 0);
                }

                ///If not enough space available, reset curBottom and advance curRight
                if (curBottom + itemSize.Height > this.ContentBounds.Bottom)
                {
                    curBottom = this.ContentBounds.Top + this.Owner.ItemPadding.Vertical + 0;
                    curRight = lastRight + this.Owner.ItemPadding.Horizontal + 0;
                    this.Items.CenterItemsVerticallyInto(lastColumn, this.ContentBounds);
                    lastColumn.Clear();
                }

                ///Set the item's bounds
                item.SetBounds(new Rectangle(curRight, curBottom, itemSize.Width, itemSize.Height));

                ///save last right and bottom
                lastRight = Math.Max(item.Bounds.Right, lastRight);
                lastBottom = item.Bounds.Bottom;

                ///update current bottom
                curBottom = item.Bounds.Bottom + this.Owner.ItemPadding.Vertical + 1;

                ///Add to the collection of items of the last column
                lastColumn.Add(item);
            }

            ///Center the items vertically on the last column 
            this.Items.CenterItemsVerticallyInto(lastColumn, this.ContentBounds);
        }

        /// <summary>
        /// Updates the bounds of child elements when flow is to Left.
        /// </summary>
        private void UpdateRegionsFlowsToLeft(Graphics g, RibbonElementSizeMode mode)
        {
            var curRight = this.ContentBounds.Left + this.Owner.ItemPadding.Horizontal + 0;
            var curBottom = this.ContentBounds.Top + this.Owner.ItemPadding.Vertical + 0;
            var lastRight = curRight;
            var lastBottom = 0;
            var lastColumn = new List<RibbonItem>();

            ///Iterate thru items on panel
            for (var i = this.Items.Count - 1; i >= 0; i--)
            {
                var item = this.Items[i];

                ///Gets the last measured size (to avoid re-measuring calculations)
                Size itemSize;
                if (item.Visible)
                {
                    itemSize = item.LastMeasuredSize;
                }
                else
                {
                    itemSize = new Size(0, 0);
                }

                ///If not enough space available, reset curBottom and advance curRight
                if (curBottom + itemSize.Height > this.ContentBounds.Bottom)
                {
                    curBottom = this.ContentBounds.Top + this.Owner.ItemPadding.Vertical + 0;
                    curRight = lastRight + this.Owner.ItemPadding.Horizontal + 0;
                    this.Items.CenterItemsVerticallyInto(lastColumn, this.ContentBounds);
                    lastColumn.Clear();
                }

                ///Set the item's bounds
                item.SetBounds(new Rectangle(curRight, curBottom, itemSize.Width, itemSize.Height));

                ///save last right and bottom
                lastRight = Math.Max(item.Bounds.Right, lastRight);
                lastBottom = item.Bounds.Bottom;

                ///update current bottom
                curBottom = item.Bounds.Bottom + this.Owner.ItemPadding.Vertical + 1;

                ///Add to the collection of items of the last column
                lastColumn.Add(item);
            }

            ///Center the items vertically on the last column 
            this.Items.CenterItemsVerticallyInto(lastColumn, this.Items.GetItemsBounds());
        }

        /// <summary>
        /// Updates the bounds of child elements when flow is to bottom
        /// </summary>
        private void UpdateRegionsFlowsToRight(Graphics g, RibbonElementSizeMode mode)
        {
            var curLeft = this.ContentBounds.Left;
            var curTop = this.ContentBounds.Top;
            var padding = mode == RibbonElementSizeMode.Medium ? 7 : 0;
            var maxBottom = 0;

            #region Sorts from larger to smaller

            var array = this.Items.ToArray();

            for (var i = array.Length - 1; i >= 0; i--)
            {
                for (var j = 1; j <= i; j++)
                {
                    if (array[j - 1].LastMeasuredSize.Width < array[j].LastMeasuredSize.Width)
                    {
                        var temp = array[j - 1];
                        array[j - 1] = array[j];
                        array[j] = temp;
                    }
                }
            }

            #endregion

            var list = new List<RibbonItem>(array);

            ///Attend elements, deleting every attended element from the list
            while (list.Count > 0)
            {
                ///Extract item and delete it
                var item = list[0];
                list.Remove(item);

                ///If not enough space left, reset left and advance top
                if (curLeft + item.LastMeasuredSize.Width > this.ContentBounds.Right)
                {
                    curLeft = this.ContentBounds.Left;
                    curTop = maxBottom + this.Owner.ItemPadding.Vertical + 1 + padding;
                }

                ///Set item's bounds
                item.SetBounds(new Rectangle(new Point(curLeft, curTop), item.LastMeasuredSize));

                ///Increment reminders
                curLeft += item.Bounds.Width + this.Owner.ItemPadding.Horizontal;
                maxBottom = Math.Max(maxBottom, item.Bounds.Bottom);

                ///Check available space after placing item
                var spaceAvailable = this.ContentBounds.Right - curLeft;

                ///Check for elements that fit on available space
                for (var i = 0; i < list.Count; i++)
                {
                    ///If item fits on the available space
                    if (list[i].LastMeasuredSize.Width < spaceAvailable)
                    {
                        ///Place the item there and reset the counter to check for further items
                        list[i].SetBounds(new Rectangle(new Point(curLeft, curTop), list[i].LastMeasuredSize));
                        curLeft += list[i].Bounds.Width + this.Owner.ItemPadding.Horizontal;
                        maxBottom = Math.Max(maxBottom, list[i].Bounds.Bottom);
                        spaceAvailable = this.ContentBounds.Right - curLeft;
                        list.RemoveAt(i);
                        i = 0;
                    }
                }
            }
        }

        /// <summary>
        /// Centers the items on the tab conent
        /// </summary>
        private void CenterItems()
        {
            this.Items.CenterItemsInto(this.ContentBounds);
        }

        /// <summary>
        /// Overriden. Gives info about the panel as a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("Panel: {0} ({1})", this.Text, this.SizeMode);
        }

        /// <summary>
        /// Sets the value of the Pressed property
        /// </summary>
        /// <param name="pressed"></param>
        public void SetPressed(bool pressed)
        {
            this.Pressed = pressed;
        }

        /// <summary>
        /// Sets the value of the ButtonMorePressed property
        /// </summary>
        /// <param name="bounds">property value</param>
        internal void SetMorePressed(bool pressed)
        {
            this.ButtonMorePressed = pressed;
        }

        /// <summary>
        /// Sets the value of the ButtonMoreSelected property
        /// </summary>
        /// <param name="bounds">property value</param>
        internal void SetMoreSelected(bool selected)
        {
            this.ButtonMoreSelected = selected;
        }

        /// <summary>
        /// Sets the value of the ButtonMoreBounds property
        /// </summary>
        /// <param name="bounds">property value</param>
        internal void SetMoreBounds(Rectangle bounds)
        {
            this.ButtonMoreBounds = bounds;
        }

        /// <summary>
        /// Raised the <see cref="ButtonMoreClick"/> event
        /// </summary>
        /// <param name="e"></param>
        protected void OnButtonMoreClick(EventArgs e)
        {
            if (this.ButtonMoreClick != null)
            {
                this.ButtonMoreClick(this, e);
            }
        }

        #endregion

        #region IContainsRibbonItems Members

        public IEnumerable<RibbonItem> GetItems()
        {
            return this.Items;
        }

        public Rectangle GetContentBounds()
        {
            return this.ContentBounds;
        }

        /// <summary>
        /// Raises the MouseEnter event
        /// </summary>
        /// <param name="e">Event data</param>
        public virtual void OnMouseEnter(MouseEventArgs e)
        {
            if (!this.Enabled)
            {
                return;
            }

            if (this.MouseEnter != null)
            {
                this.MouseEnter(this, e);
            }
        }

        /// <summary>
        /// Raises the MouseLeave event
        /// </summary>
        /// <param name="e">Event data</param>
        public virtual void OnMouseLeave(MouseEventArgs e)
        {
            if (!this.Enabled)
            {
                return;
            }

            if (this.MouseLeave != null)
            {
                this.MouseLeave(this, e);
            }
        }

        /// <summary>
        /// Raises the MouseMove event
        /// </summary>
        /// <param name="e">Event data</param>
        public virtual void OnMouseMove(MouseEventArgs e)
        {
            if (!this.Enabled)
            {
                return;
            }

            if (this.MouseMove != null)
            {
                this.MouseMove(this, e);
            }

            var redraw = false;

            if (this.ButtonMoreEnabled && this.ButtonMoreVisible && this.ButtonMoreBounds.Contains(e.X, e.Y) && !this.Collapsed)
            {
                this.SetMoreSelected(true);
                redraw = true;
            }
            else
            {
                redraw = this.ButtonMoreSelected;
                this.SetMoreSelected(false);
            }

            if (redraw)
            {
                this.Owner.Invalidate(this.Bounds);
            }
        }

        /// <summary>
        /// Raises the Click event
        /// </summary>
        /// <param name="e">Event data</param>
        public virtual void OnClick(EventArgs e)
        {
            if (!this.Enabled)
            {
                return;
            }

            if (this.Click != null)
            {
                this.Click(this, e);
            }

            if (this.Collapsed && this.PopUp == null)
            {
                this.ShowOverflowPopup();
            }
        }

        /// <summary>
        /// Raises the DoubleClick event
        /// </summary>
        /// <param name="e">Event data</param>
        public virtual void OnDoubleClick(EventArgs e)
        {
            if (!this.Enabled)
            {
                return;
            }

            if (this.DoubleClick != null)
            {
                this.DoubleClick(this, e);
            }
        }

        /// <summary>
        /// Raises the MouseDown event
        /// </summary>
        /// <param name="e">Event data</param>
        public virtual void OnMouseDown(MouseEventArgs e)
        {
            if (!this.Enabled)
            {
                return;
            }

            if (this.MouseDown != null)
            {
                this.MouseDown(this, e);
            }

            this.SetPressed(true);

            var redraw = false;

            if (this.ButtonMoreEnabled && this.ButtonMoreVisible && this.ButtonMoreBounds.Contains(e.X, e.Y) && !this.Collapsed)
            {
                this.SetMorePressed(true);
                redraw = true;
            }
            else
            {
                redraw = this.ButtonMoreSelected;
                this.SetMorePressed(false);
            }

            if (redraw)
            {
                this.Owner.Invalidate(this.Bounds);
            }
        }

        /// <summary>
        /// Raises the MouseUp event
        /// </summary>
        /// <param name="e">Event data</param>
        public virtual void OnMouseUp(MouseEventArgs e)
        {
            if (!this.Enabled)
            {
                return;
            }

            if (this.MouseUp != null)
            {
                this.MouseUp(this, e);
            }

            if (this.ButtonMoreEnabled && this.ButtonMoreVisible && this.ButtonMorePressed && !this.Collapsed)
            {
                this.OnButtonMoreClick(EventArgs.Empty);
            }

            this.SetPressed(false);
            this.SetMorePressed(false);
        }

        #endregion
    }
}