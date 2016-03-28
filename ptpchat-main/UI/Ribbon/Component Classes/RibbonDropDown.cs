namespace PtpChat.Main.Ribbon.Component_Classes
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Drawing;
    using System.Windows.Forms;

    using IScrollableRibbonItem = PtpChat.Main.Ribbon.Classes.Interfaces.IScrollableRibbonItem;
    using RibbonCanvasEventArgs = PtpChat.Main.Ribbon.Classes.EventArgs.RibbonCanvasEventArgs;
    using RibbonDesigner = PtpChat.Main.Ribbon.Classes.Designers.RibbonDesigner;
    using RibbonElementMeasureSizeEventArgs = PtpChat.Main.Ribbon.Classes.EventArgs.RibbonElementMeasureSizeEventArgs;
    using RibbonElementPaintEventArgs = PtpChat.Main.Ribbon.Classes.EventArgs.RibbonElementPaintEventArgs;
    using RibbonElementSizeMode = PtpChat.Main.Ribbon.Classes.Enums.RibbonElementSizeMode;
    using RibbonMouseSensor = PtpChat.Main.Ribbon.Classes.RibbonMouseSensor;

    [ToolboxItem(false)]
    public class RibbonDropDown : RibbonPopup, IScrollableRibbonItem
    {
        #region Static

        //private static List<RibbonDropDown> registeredDds = new List<RibbonDropDown>();

        //private static void RegisterDropDown(RibbonDropDown dropDown)
        //{
        //    registeredDds.Add(dropDown);
        //}

        //private static void UnregisterDropDown(RibbonDropDown dropDown)
        //{
        //    registeredDds.Remove(dropDown);
        //}

        //internal static void DismissAll()
        //{
        //    for (int i = 0; i < registeredDds.Count; i++)
        //    {

        //        registeredDds[i].Close();
        //    }

        //    registeredDds.Clear();
        //}

        ///// <summary>
        ///// Closes all the dropdowns before the specified dropDown
        ///// </summary>
        ///// <param name="dropDown"></param>
        //internal static void DismissTo(RibbonDropDown dropDown)
        //{
        //    if (dropDown == null) throw new ArgumentNullException("dropDown");

        //    for (int i = registeredDds.Count - 1; i >= 0; i--)
        //    {
        //        if (i >= registeredDds.Count)
        //        {
        //            break;
        //        }

        //        if (registeredDds[i].Equals(dropDown))
        //        {
        //            break;
        //        }
        //        else
        //        {
        //            registeredDds[i].Close();
        //        }
        //    }
        //}

        #endregion

        #region Fields

        private bool _showSizingGrip;

        private bool _ignoreNext;

        private bool _resizing;

        private Point _resizeOrigin;

        private Size _resizeSize;

        //scroll properties
        private Rectangle _thumbBounds;

        private Rectangle _buttonUpBounds;

        private Rectangle _buttonDownBounds;

        private Rectangle _fullContentBounds;

        private int _scrollValue;

        private bool _avoidNextThumbMeasure;

        private int _jumpDownSize;

        private int _jumpUpSize;

        private int _offset;

        private int _thumbOffset;

        #endregion

        #region Ctor

        private RibbonDropDown()
        {
            //RegisterDropDown(this);
            this.DoubleBuffered = true;
            this.DrawIconsBar = true;
        }

        internal RibbonDropDown(RibbonItem parentItem, IEnumerable<RibbonItem> items, Ribbon ownerRibbon)
            : this(parentItem, items, ownerRibbon, RibbonElementSizeMode.DropDown)
        {
        }

        internal RibbonDropDown(RibbonItem parentItem, IEnumerable<RibbonItem> items, Ribbon ownerRibbon, RibbonElementSizeMode measuringSize)
            : this()
        {
            this.Items = items;
            this.OwnerRibbon = ownerRibbon;
            this.SizingGripHeight = 12;
            this.ParentItem = parentItem;
            this.Sensor = new RibbonMouseSensor(this, this.OwnerRibbon, items);
            this.MeasuringSize = measuringSize;
            this.ScrollBarSize = 16;

            if (this.Items != null)
            {
                foreach (var item in this.Items)
                {
                    item.SetSizeMode(RibbonElementSizeMode.DropDown);
                    item.SetCanvas(this);
                }
            }

            this.UpdateSize();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Sets the maximum height in pixels for the dropdown window. Enter 0 for autosize. If the contents is larger than the window scrollbars will be shown.
        /// </summary>
        public int DropDownMaxHeight { get; set; } = 0;

        /// <summary>
        /// Gets or sets the width of the scrollbar
        /// </summary>
        public int ScrollBarSize { get; set; }

        /// <summary>
        /// Gets the control where the item is currently being drawn
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Control Canvas => this;

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Rectangle ScrollBarBounds => Rectangle.FromLTRB(this.ButtonUpBounds.Left, this.ButtonUpBounds.Top, this.ButtonDownBounds.Right, this.ButtonDownBounds.Bottom);

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ScrollBarEnabled { get; private set; }

        /// <summary>
        /// Gets the percent of scrolled content
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double ScrolledPercent
        {
            get
            {
                if (this._fullContentBounds.Height > (double)this.ContentBounds.Height)
                {
                    return (this.ContentBounds.Top - (double)this._fullContentBounds.Top) / (this._fullContentBounds.Height - (double)this.ContentBounds.Height);
                }
                return 0.0;
            }
            set
            {
                this._avoidNextThumbMeasure = true;
                this.ScrollTo(-Convert.ToInt32((this._fullContentBounds.Height - this.ContentBounds.Height) * value));
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int ScrollMinimum => this.ButtonUpBounds.Bottom;

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int ScrollMaximum => this.ButtonDownBounds.Top - this.ThumbBounds.Height;

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int ScrollValue
        {
            get { return this._scrollValue; }
            set
            {
                if (value > this.ScrollMaximum || value < this.ScrollMinimum)
                {
                    throw new IndexOutOfRangeException("Scroll value must exist between ScrollMinimum and Scroll Maximum");
                }

                this._thumbBounds.Y = value;

                double scrolledPixels = value - this.ScrollMinimum;
                double pixelsAvailable = this.ScrollMaximum - this.ScrollMinimum;

                this.ScrolledPercent = scrolledPixels / pixelsAvailable;

                this._scrollValue = value;
            }
        }

        /// <summary>
        /// Redraws the scroll part of the list
        /// </summary>
        private void RedrawScroll()
        {
            if (this.Canvas != null)
            {
                this.Canvas.Invalidate(Rectangle.FromLTRB(this.ButtonDownBounds.X, this.ButtonUpBounds.Y, this.ButtonDownBounds.Right, this.ButtonDownBounds.Bottom));
            }
        }

        /// <summary>
        /// Gets if the scrollbar thumb is currently selected
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ThumbSelected { get; private set; }

        /// <summary>
        /// Gets if the scrollbar thumb is currently pressed
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ThumbPressed { get; private set; }

        /// <summary>
        /// Gets the bounds of the scrollbar thumb
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Rectangle ThumbBounds => this._thumbBounds;

        /// <summary>
        /// Gets a value indicating if the button that scrolls up the content is currently enabled
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ButtonUpEnabled { get; private set; }

        /// <summary>
        /// Gets a value indicating if the button that scrolls down the content is currently enabled
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ButtonDownEnabled { get; private set; }

        /// <summary>
        /// Gets a vaule indicating if the button that scrolls down the content is currently selected
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ButtonDownSelected { get; private set; }

        /// <summary>
        /// Gets a vaule indicating if the button that scrolls down the content is currently pressed
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ButtonDownPressed { get; private set; }

        /// <summary>
        /// Gets a vaule indicating if the button that scrolls up the content is currently selected
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ButtonUpSelected { get; private set; }

        /// <summary>
        /// Gets the bounds of the content where items are shown
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Rectangle ContentBounds { get; private set; }

        /// <summary>
        /// Gets a vaule indicating if the button that scrolls up the content is currently pressed
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ButtonUpPressed { get; private set; }

        /// <summary>
        /// Gets the bounds of the button that scrolls the items up
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Rectangle ButtonUpBounds => this._buttonUpBounds;

        /// <summary>
        /// Gets the bounds of the button that scrolls the items down
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Rectangle ButtonDownBounds => this._buttonDownBounds;

        /// <summary>
        /// Gets or sets if the icons bar should be drawn
        /// </summary>
        public bool DrawIconsBar { get; set; }

        /// <summary>
        /// Gets or sets the selection service for the dropdown
        /// </summary>
        internal ISelectionService SelectionService { get; set; }

        /// <summary>
        /// Gets the bounds of the sizing grip
        /// </summary>
        public Rectangle SizingGripBounds { get; private set; }

        /// <summary>
        /// Gets or sets the size for measuring items (by default is DropDown)
        /// </summary>
        public RibbonElementSizeMode MeasuringSize { get; set; }

        /// <summary>
        /// Gets the parent item of this dropdown
        /// </summary>
        public RibbonItem ParentItem { get; }

        /// <summary>
        /// Gets the sennsor of this dropdown
        /// </summary>
        public RibbonMouseSensor Sensor { get; }

        /// <summary>
        /// Gets the Ribbon this DropDown belongs to
        /// </summary>
        public Ribbon OwnerRibbon { get; }

        /// <summary>
        /// Gets the RibbonItem this dropdown belongs to
        /// </summary>
        public IEnumerable<RibbonItem> Items { get; }

        /// <summary>
        /// Gets or sets a value indicating if the sizing grip should be visible
        /// </summary>
        public bool ShowSizingGrip
        {
            get { return this._showSizingGrip; }
            set
            {
                this._showSizingGrip = value;
                this.UpdateSize();
            }
        }

        /// <summary>
        /// Gets or sets the height of the sizing grip area
        /// </summary>
        [DefaultValue(12)]
        public int SizingGripHeight { get; set; }

        #endregion

        #region Methods

        public void SetBounds()
        {
            #region Assign grip regions

            if (this.ShowSizingGrip)
            {
                this.SizingGripBounds = Rectangle.FromLTRB(this.ClientSize.Width - this.SizingGripHeight, this.ClientSize.Height - this.SizingGripHeight, this.ClientSize.Width, this.ClientSize.Height);
            }
            else
            {
                this.SizingGripBounds = Rectangle.Empty;
            }

            #endregion

            #region Assign buttons regions

            if (this.ScrollBarEnabled)
            {
                var bwidth = this.ScrollBarSize;
                var bheight = this.ScrollBarSize;
                this._thumbBounds.Width = this.ScrollBarSize;

                this._buttonUpBounds = new Rectangle(this.Bounds.Right - bwidth - 1, this.Bounds.Top + this.OwnerRibbon.DropDownMargin.Top, bwidth, bheight);

                this._buttonDownBounds = new Rectangle(
                    this._buttonUpBounds.Left,
                    this.Bounds.Height - bheight - this.SizingGripBounds.Height - this.OwnerRibbon.DropDownMargin.Bottom - 1,
                    bwidth,
                    bheight);

                this._thumbBounds.X = this._buttonUpBounds.Left;

                this.ButtonUpEnabled = this._offset < 0;
                if (!this.ButtonUpEnabled)
                {
                    this._offset = 0;
                }
                this.ButtonDownEnabled = false;
            }

            #endregion

            var scrollWidth = this.ScrollBarEnabled ? this.ScrollBarSize : 0;
            var itemsWidth = Math.Max(0, this.ClientSize.Width - this.OwnerRibbon.DropDownMargin.Horizontal - scrollWidth);

            this.ContentBounds = Rectangle.FromLTRB(
                this.OwnerRibbon.DropDownMargin.Left,
                this.OwnerRibbon.DropDownMargin.Top,
                this.Bounds.Right - scrollWidth - this.OwnerRibbon.DropDownMargin.Right,
                this.Bounds.Bottom - this.OwnerRibbon.DropDownMargin.Bottom - this.SizingGripBounds.Height);

            var curTop = this.OwnerRibbon.DropDownMargin.Top + this._offset;
            var curLeft = this.OwnerRibbon.DropDownMargin.Left;
            var maxBottom = curTop; // int.MinValue;
            var iniTop = curTop;

            foreach (var item in this.Items)
            {
                item.SetBounds(Rectangle.Empty);
            }

            foreach (var item in this.Items)
            {
                curTop = maxBottom + 1;

                item.SetBounds(new Rectangle(curLeft, curTop, itemsWidth, item.LastMeasuredSize.Height));

                //maxBottom = Math.Max(maxBottom, item.Bounds.Bottom);
                maxBottom = curTop + item.LastMeasuredSize.Height;

                //if (item.Bounds.Bottom > ContentBounds.Bottom) _buttonDownEnabled = true;

                this._jumpDownSize = item.Bounds.Height;
                this._jumpUpSize = item.Bounds.Height;
            }

            this._fullContentBounds = Rectangle.FromLTRB(this.ContentBounds.Left, iniTop, this.ContentBounds.Right, maxBottom);

            #region Adjust thumb size

            double contentHeight = maxBottom - iniTop - 1;
            double viewHeight = this.Bounds.Height;

            //scrollbars?
            if (this.ContentBounds.Height < this._fullContentBounds.Height)
            {
                var viewPercent = this._fullContentBounds.Height > this.ContentBounds.Height ? (double)this.ContentBounds.Height / this._fullContentBounds.Height : 0.0;
                double availHeight = this.ButtonDownBounds.Top - this.ButtonUpBounds.Bottom;
                var thumbHeight = Math.Ceiling(viewPercent * availHeight);

                if (thumbHeight < 30)
                {
                    if (availHeight >= 30)
                    {
                        thumbHeight = 30;
                    }
                    else
                    {
                        thumbHeight = availHeight;
                    }
                }
                this.ButtonUpEnabled = this._offset < 0;
                this.ButtonDownEnabled = this.ScrollMaximum > -this._offset;

                this._thumbBounds.Height = Convert.ToInt32(thumbHeight);

                this.ScrollBarEnabled = true;

                this.UpdateThumbPos();
            }
            else
            {
                this.ScrollBarEnabled = false;
            }

            #endregion
        }

        /// <summary>
        /// Updates the position of the scroll thumb depending on the current offset
        /// </summary>
        private void UpdateThumbPos()
        {
            if (this._avoidNextThumbMeasure)
            {
                this._avoidNextThumbMeasure = false;
                return;
            }

            var scrolledp = this.ScrolledPercent;

            if (!double.IsInfinity(scrolledp))
            {
                double availSpace = this.ScrollMaximum - this.ScrollMinimum;
                var scrolledSpace = Math.Ceiling(availSpace * this.ScrolledPercent);

                this._thumbBounds.Y = this.ScrollMinimum + Convert.ToInt32(scrolledSpace);
            }
            else
            {
                this._thumbBounds.Y = this.ScrollMinimum;
            }

            if (this._thumbBounds.Y > this.ScrollMaximum)
            {
                this._thumbBounds.Y = this.ScrollMaximum;
            }
        }

        /// <summary>
        /// Scrolls the list down
        /// </summary>
        public void ScrollDown()
        {
            if (this.ScrollBarEnabled)
            {
                this.ScrollOffset(-(this._jumpDownSize + 1));
            }
        }

        /// <summary>
        /// Scrolls the list up
        /// </summary>
        public void ScrollUp()
        {
            if (this.ScrollBarEnabled)
            {
                this.ScrollOffset(this._jumpUpSize + 1);
            }
        }

        /// <summary>
        /// Pushes the amount of _offset of the top of items
        /// </summary>
        /// <param name="amount"></param>
        private void ScrollOffset(int amount)
        {
            this.ScrollTo(this._offset + amount);
        }

        /// <summary>
        /// Scrolls the content to the specified offset
        /// </summary>
        /// <param name="offset"></param>
        private void ScrollTo(int offset)
        {
            if (this.ScrollBarEnabled)
            {
                var minOffset = this.ContentBounds.Height - this._fullContentBounds.Height;

                if (offset < minOffset)
                {
                    offset = minOffset;
                }

                this._offset = offset;
                this.SetBounds();
                this.Invalidate();
            }
        }

        /// <summary>
        /// Prevents the form from being hidden the next time the mouse clicks on the form.
        /// It is useful for reacting to clicks of items inside items.
        /// </summary>
        public void IgnoreNextClickDeactivation()
        {
            this._ignoreNext = true;
        }

        /// <summary>
        /// Updates the size of the dropdown
        /// </summary>
        private void UpdateSize()
        {
            var heightSum = this.OwnerRibbon.DropDownMargin.Vertical;
            var maxWidth = 0;
            var scrollableHeight = 0;
            using (var g = this.CreateGraphics())
            {
                foreach (var item in this.Items)
                {
                    var s = item.MeasureSize(this, new RibbonElementMeasureSizeEventArgs(g, this.MeasuringSize));

                    heightSum += s.Height + 1;
                    maxWidth = Math.Max(maxWidth, s.Width);

                    if (item is IScrollableRibbonItem)
                    {
                        scrollableHeight += s.Height;
                    }
                }
            }

            //This is the initial sizing of the popup window so
            //we need to add the width of the scrollbar if its needed.
            if ((this.DropDownMaxHeight > 0 && this.DropDownMaxHeight < heightSum && !this._resizing)
                || heightSum + (this.ShowSizingGrip ? this.SizingGripHeight + 2 : 0) + 1 > Screen.PrimaryScreen.WorkingArea.Height)
            {
                if (this.DropDownMaxHeight > 0)
                {
                    heightSum = this.DropDownMaxHeight;
                }
                else
                {
                    heightSum = Screen.PrimaryScreen.WorkingArea.Height - ((this.ShowSizingGrip ? this.SizingGripHeight + 2 : 0) + 1);
                }

                maxWidth += this.ScrollBarSize;
                this._thumbBounds.Width = this.ScrollBarSize;
                this.ScrollBarEnabled = true;
            }

            if (!this._resizing)
            {
                var sz = new Size(maxWidth + this.OwnerRibbon.DropDownMargin.Horizontal, heightSum + (this.ShowSizingGrip ? this.SizingGripHeight + 2 : 0) + 1);
                this.Size = sz;
            }

            if (this.WrappedDropDown != null)
            {
                this.WrappedDropDown.Size = this.Size;
            }

            this.SetBounds();
        }

        ///// <summary>
        ///// Updates the bounds of the items
        ///// </summary>
        //private void UpdateItemsBounds()
        //{
        //   SetBounds();
        //   return;
        //   int curTop = OwnerRibbon.DropDownMargin.Top;
        //   int curLeft = OwnerRibbon.DropDownMargin.Left;
        //   //Got off the patch site from logicalerror
        //   //int itemsWidth = ClientSize.Width - OwnerRibbon.DropDownMargin.Horizontal;
        //   int itemsWidth = Math.Max(0, ClientSize.Width - OwnerRibbon.DropDownMargin.Horizontal);

        //   if (ScrollBarEnabled) itemsWidth -= ScrollBarSize;

        //   int scrollableItemsHeight = 0;
        //   int nonScrollableItemsHeight = 0;
        //   int scrollableItems = 0;
        //   int scrollableItemHeight = 0;

        //   #region Measure scrollable content
        //   foreach (RibbonItem item in Items)
        //   {
        //      if (item is IScrollableRibbonItem)
        //      {
        //         scrollableItemsHeight += item.LastMeasuredSize.Height;
        //         scrollableItems++;
        //      }
        //      else
        //      {
        //         nonScrollableItemsHeight += item.LastMeasuredSize.Height;
        //      }
        //   }

        //   if (scrollableItems > 0)
        //   {
        //      //Got off the patch site from logicalerror
        //      //scrollableItemHeight = (Height - nonScrollableItemsHeight - (ShowSizingGrip ? SizingGripHeight : 0)) / scrollableItems;
        //      scrollableItemHeight = Math.Max(0, (Height - nonScrollableItemsHeight - (ShowSizingGrip ? SizingGripHeight : 0)) / scrollableItems);
        //   }

        //   #endregion

        //   foreach (RibbonItem item in Items)
        //   {
        //      if (item is IScrollableRibbonItem)
        //      {
        //         item.SetBounds(new Rectangle(curLeft, curTop, itemsWidth, scrollableItemHeight - 1));
        //      }
        //      else
        //      {
        //         item.SetBounds(new Rectangle(curLeft, curTop, itemsWidth, item.LastMeasuredSize.Height));
        //      }

        //      curTop += item.Bounds.Height;
        //   }

        //   if (ShowSizingGrip)
        //   {
        //      _sizingGripBounds = Rectangle.FromLTRB(
        //          ClientSize.Width - SizingGripHeight, ClientSize.Height - SizingGripHeight,
        //          ClientSize.Width, ClientSize.Height);
        //   }
        //   else
        //   {
        //      _sizingGripBounds = Rectangle.Empty;
        //   }
        //}

        /// <summary>
        /// Ignores deactivation of canvas if it is a volatile window
        /// </summary>
        private void IgnoreDeactivation()
        {
            if (this.Canvas is RibbonPanelPopup)
            {
                (this.Canvas as RibbonPanelPopup).IgnoreNextClickDeactivation();
            }

            if (this.Canvas is RibbonDropDown)
            {
                (this.Canvas as RibbonDropDown).IgnoreNextClickDeactivation();
            }
        }

        #region Overrides

        protected override void OnOpening(CancelEventArgs e)
        {
            base.OnOpening(e);

            this.SetBounds();
        }

        protected override void OnShowed(EventArgs e)
        {
            base.OnShowed(e);

            foreach (var item in this.Items)
            {
                item.SetSelected(false);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (this.Cursor == Cursors.SizeNWSE)
            {
                this._resizeOrigin = new Point(e.X, e.Y);
                this._resizeSize = this.Size;
                this._resizing = true;
            }
            if (this.ButtonDownSelected || this.ButtonUpSelected)
            {
                this.IgnoreDeactivation();
            }

            if (this.ButtonDownSelected && this.ButtonDownEnabled)
            {
                this.ButtonDownPressed = true;
                this.ScrollDown();
            }

            if (this.ButtonUpSelected && this.ButtonUpEnabled)
            {
                this.ButtonUpPressed = true;
                this.ScrollUp();
            }

            if (this.ThumbSelected)
            {
                this.ThumbPressed = true;
                this._thumbOffset = e.Y - this._thumbBounds.Y;
            }

            if (this.ScrollBarBounds.Contains(e.Location) && e.Y >= this.ButtonUpBounds.Bottom && e.Y <= this.ButtonDownBounds.Y && !this.ThumbBounds.Contains(e.Location)
                && !this.ButtonDownBounds.Contains(e.Location) && !this.ButtonUpBounds.Contains(e.Location))
            {
                if (e.Y < this.ThumbBounds.Y)
                {
                    this.ScrollOffset(this.Bounds.Height);
                }
                else
                {
                    this.ScrollOffset(-this.Bounds.Height);
                }
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            //Close();

            base.OnMouseClick(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (this.ShowSizingGrip && this.SizingGripBounds.Contains(e.X, e.Y))
            {
                this.Cursor = Cursors.SizeNWSE;
            }
            else if (this.Cursor == Cursors.SizeNWSE)
            {
                this.Cursor = Cursors.Default;
            }

            if (this._resizing)
            {
                var dx = e.X - this._resizeOrigin.X;
                var dy = e.Y - this._resizeOrigin.Y;

                var w = this._resizeSize.Width + dx;
                var h = this._resizeSize.Height + dy;

                if (w != this.Width || h != this.Height)
                {
                    this.Size = new Size(w, h);
                    if (this.WrappedDropDown != null)
                    {
                        this.WrappedDropDown.Size = this.Size;
                    }
                    var contentHeight = this.Bounds.Height - this.OwnerRibbon.DropDownMargin.Vertical - this.SizingGripBounds.Height;
                    if (contentHeight < this._fullContentBounds.Height)
                    {
                        this.ScrollBarEnabled = true;
                        if (-this._offset + contentHeight > this._fullContentBounds.Height)
                        {
                            this._offset = contentHeight - this._fullContentBounds.Height;
                        }
                    }
                    else
                    {
                        this.ScrollBarEnabled = false;
                    }

                    this.SetBounds();
                    this.Invalidate();
                }
            }

            if (this.ButtonDownPressed && this.ButtonDownSelected && this.ButtonDownEnabled)
            {
                this.ScrollOffset(-1);
            }

            if (this.ButtonUpPressed && this.ButtonUpSelected && this.ButtonUpEnabled)
            {
                this.ScrollOffset(1);
            }

            var upCache = this.ButtonUpSelected;
            var downCache = this.ButtonDownSelected;
            var thumbCache = this.ThumbSelected;

            this.ButtonUpSelected = this._buttonUpBounds.Contains(e.Location);
            this.ButtonDownSelected = this._buttonDownBounds.Contains(e.Location);
            this.ThumbSelected = this._thumbBounds.Contains(e.Location) && this.ScrollBarEnabled;

            if ((upCache != this.ButtonUpSelected) || (downCache != this.ButtonDownSelected) || (thumbCache != this.ThumbSelected))
            {
                this.Invalidate();
            }

            if (this.ThumbPressed)
            {
                var newval = e.Y - this._thumbOffset;

                if (newval < this.ScrollMinimum)
                {
                    newval = this.ScrollMinimum;
                }
                else if (newval > this.ScrollMaximum)
                {
                    newval = this.ScrollMaximum;
                }

                this.ScrollValue = newval;
                this.Invalidate();
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            this.ButtonDownPressed = false;
            this.ButtonUpPressed = false;
            this.ThumbPressed = false;

            if (this._resizing)
            {
                this._resizing = false;
                return;
            }

            if (this._ignoreNext)
            {
                this._ignoreNext = false;
                return;
            }

            if (RibbonDesigner.Current != null)
            {
                this.Close();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            this.OwnerRibbon.Renderer.OnRenderDropDownBackground(new RibbonCanvasEventArgs(this.OwnerRibbon, e.Graphics, new Rectangle(Point.Empty, this.ClientSize), this, this.ParentItem));

            var lastClip = e.Graphics.ClipBounds;
            //if (e.ClipRectangle.Top < OwnerRibbon.DropDownMargin.Top)
            //{
            var newClip = lastClip;
            newClip.Y = this.OwnerRibbon.DropDownMargin.Top;
            newClip.Height = this.Bounds.Bottom - this.SizingGripBounds.Height - this.OwnerRibbon.DropDownMargin.Vertical;
            e.Graphics.SetClip(newClip);
            //}

            foreach (var item in this.Items)
            {
                if (item.Bounds.IntersectsWith(this.ContentBounds))
                {
                    item.OnPaint(this, new RibbonElementPaintEventArgs(item.Bounds, e.Graphics, RibbonElementSizeMode.DropDown));
                }
            }

            if (this.ScrollBarEnabled)
            {
                this.OwnerRibbon.Renderer.OnRenderScrollbar(e.Graphics, this, this.OwnerRibbon);
            }

            e.Graphics.SetClip(lastClip);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            foreach (var item in this.Items)
            {
                item.SetSelected(false);
            }
        }

        #endregion

        #endregion
    }
}