namespace PtpChat.Main.Ribbon.Component_Classes
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using System.Windows.Forms.VisualStyles;

    using PtpChat.Main.Ribbon.Classes.Designers;

    using IContainsRibbonComponents = PtpChat.Main.Ribbon.Classes.Interfaces.IContainsRibbonComponents;
    using IContainsSelectableRibbonItems = PtpChat.Main.Ribbon.Classes.Interfaces.IContainsSelectableRibbonItems;
    using IScrollableRibbonItem = PtpChat.Main.Ribbon.Classes.Interfaces.IScrollableRibbonItem;
    using RibbonButtonCollection = PtpChat.Main.Ribbon.Classes.Collections.RibbonButtonCollection;
    using RibbonElementMeasureSizeEventArgs = PtpChat.Main.Ribbon.Classes.EventArgs.RibbonElementMeasureSizeEventArgs;
    using RibbonElementPaintEventArgs = PtpChat.Main.Ribbon.Classes.EventArgs.RibbonElementPaintEventArgs;
    using RibbonElementSizeMode = PtpChat.Main.Ribbon.Classes.Enums.RibbonElementSizeMode;
    using RibbonItemCollection = PtpChat.Main.Ribbon.Classes.Collections.RibbonItemCollection;
    using RibbonItemEventArgs = PtpChat.Main.Ribbon.Classes.EventArgs.RibbonItemEventArgs;
    using RibbonItemRenderEventArgs = PtpChat.Main.Ribbon.Classes.EventArgs.RibbonItemRenderEventArgs;

    /// <summary>
    /// Represents a list of buttons that can be navigated
    /// </summary>
    [Designer(typeof(RibbonButtonListDesigner))]
    public sealed class RibbonButtonList : RibbonItem, IContainsSelectableRibbonItems, IScrollableRibbonItem, IContainsRibbonComponents
    {
        #region Subtypes

        public enum ListScrollType
        {
            UpDownButtons,

            Scrollbar
        }

        #endregion

        #region IContainsRibbonComponents Members

        public IEnumerable<Component> GetAllChildComponents()
        {
            var result = new List<Component>(this.Buttons.ToArray());

            result.AddRange(this.DropDownItems.ToArray());

            return result;
        }

        #endregion

        internal void item_Click(object sender, EventArgs e)
        {
            // Steve
            this._selectedItem = sender as RibbonItem;

            //Kevin Carbis
            var ev = new RibbonItemEventArgs(this._selectedItem);
            if (this.DropDownItems.Contains(this._selectedItem))
            {
                this.OnDropDownItemClicked(ref ev);
            }
            else
            {
                this.OnButtonItemClicked(ref ev);
            }
        }

        #region Fields

        private int _itemsInLargeMode;

        private int _itemsInMediumMode;

        private Size _ItemsInDropwDownMode;

        private Rectangle _buttonUpBounds;

        private Rectangle _buttonDownBounds;

        private Rectangle _buttonDropDownBounds;

        private Rectangle _contentBounds;

        private int _controlButtonsWidth;

        private RibbonElementSizeMode _buttonsSizeMode;

        private int _jumpDownSize;

        private int _jumpUpSize;

        private int _offset;

        private RibbonDropDown _dropDown;

        private bool _dropDownVisible;

        private Rectangle _thumbBounds;

        private int _scrollValue;

        private Rectangle fullContentBounds;

        private int _thumbOffset;

        private bool _avoidNextThumbMeasure;

        private RibbonItem _selectedItem;

        public event RibbonItemEventHandler ButtonItemClicked;

        public event RibbonItemEventHandler DropDownItemClicked;

        public delegate void RibbonItemEventHandler(object sender, RibbonItemEventArgs e);

        #endregion

        #region Ctor

        public RibbonButtonList()
        {
            this.Buttons = new RibbonButtonCollection(this);
            this.DropDownItems = new RibbonItemCollection();

            this._controlButtonsWidth = 16;
            this._itemsInLargeMode = 7;
            this._itemsInMediumMode = 3;
            this._ItemsInDropwDownMode = new Size(7, 5);
            this._buttonsSizeMode = RibbonElementSizeMode.Large;
            this.ScrollType = ListScrollType.UpDownButtons;
        }

        public RibbonButtonList(IEnumerable<RibbonButton> buttons)
            : this(buttons, null)
        {
        }

        public RibbonButtonList(IEnumerable<RibbonButton> buttons, IEnumerable<RibbonItem> dropDownItems)
            : this()
        {
            if (buttons != null)
            {
                var items = new List<RibbonButton>(buttons);

                this.Buttons.AddRange(items.ToArray());

                //add the handlers
                foreach (RibbonItem item in buttons)
                {
                    item.Click += this.item_Click;
                }
            }

            if (dropDownItems != null)
            {
                this.DropDownItems.AddRange(dropDownItems);

                //add the handlers
                foreach (var item in dropDownItems)
                {
                    item.Click += this.item_Click;
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var item in this.Buttons)
                {
                    item.Click -= this.item_Click;
                }
                foreach (var item in this.DropDownItems)
                {
                    item.Click -= this.item_Click;
                }
            }
            base.Dispose(disposing);
        }

        #endregion

        #region Props

        [Description("If activated, buttons will flow to bottom inside the list")]
        public bool FlowToBottom { get; set; }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Rectangle ScrollBarBounds => Rectangle.FromLTRB(this.ButtonUpBounds.Left, this.ButtonUpBounds.Top, this.ButtonDownBounds.Right, this.ButtonDownBounds.Bottom);

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ScrollBarEnabled { get; private set; }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ListScrollType ScrollType { get; private set; }

        /// <summary>
        /// Gets the percent of scrolled content
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double ScrolledPercent
        {
            get { return (this.ContentBounds.Top - (double)this.fullContentBounds.Top) / (this.fullContentBounds.Height - (double)this.ContentBounds.Height); }
            set
            {
                this._avoidNextThumbMeasure = true;
                this.ScrollTo(-Convert.ToInt32((this.fullContentBounds.Height - this.ContentBounds.Height) * value));
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int ScrollMinimum
        {
            get
            {
                if (this.ScrollType == ListScrollType.Scrollbar)
                {
                    return this.ButtonUpBounds.Bottom;
                }
                return 0;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int ScrollMaximum
        {
            get
            {
                if (this.ScrollType == ListScrollType.Scrollbar)
                {
                    //return ButtonDownBounds.Top - ThumbBounds.Height;
                    return this.ButtonDownBounds.Top - this.ThumbBounds.Height;
                }
                return 0;
            }
        }

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
        /// Gets if the DropDown button is present on thelist
        /// </summary>
        public bool ButtonDropDownPresent => this.ButtonDropDownBounds.Height > 0;

        /// <summary>
        /// Gets the collection of items shown on the dropdown pop-up when Style allows it
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public RibbonItemCollection DropDownItems { get; }

        /// <summary>
        /// Gets or sets the size that the buttons on the list should be
        /// </summary>
        public RibbonElementSizeMode ButtonsSizeMode
        {
            get { return this._buttonsSizeMode; }
            set
            {
                this._buttonsSizeMode = value;
                if (this.Owner != null)
                {
                    this.Owner.OnRegionsChanged();
                }
            }
        }

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
        /// Gets a value indicating if the DropDown button is currently selected
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ButtonDropDownSelected { get; private set; }

        /// <summary>
        /// Gets a value indicating if the DropDown button is currently pressed
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ButtonDropDownPressed { get; private set; }

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
        /// Gets a vaule indicating if the button that scrolls up the content is currently pressed
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ButtonUpPressed { get; private set; }

        /// <summary>
        /// Gets the bounds of the content where items are shown
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Rectangle ContentBounds => this._contentBounds;

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
        /// Gets the bounds of the button that scrolls
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Rectangle ButtonDropDownBounds => this._buttonDropDownBounds;

        /// <summary>
        /// Gets or sets the with of the buttons that allow to navigate thru the list
        /// </summary>
        [DefaultValue(16)]
        public int ControlButtonsWidth
        {
            get { return this._controlButtonsWidth; }
            set
            {
                this._controlButtonsWidth = value;
                if (this.Owner != null)
                {
                    this.Owner.OnRegionsChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating the amount of items to show
        /// (wide) when SizeMode is Large 
        /// </summary>
        [DefaultValue(7)]
        public int ItemsWideInLargeMode
        {
            get { return this._itemsInLargeMode; }
            set
            {
                this._itemsInLargeMode = value;
                if (this.Owner != null)
                {
                    this.Owner.OnRegionsChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating the amount of items to show
        /// (wide) when SizeMode is Medium
        /// </summary>
        [DefaultValue(3)]
        public int ItemsWideInMediumMode
        {
            get { return this._itemsInMediumMode; }
            set
            {
                this._itemsInMediumMode = value;
                if (this.Owner != null)
                {
                    this.Owner.OnRegionsChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating the amount of items to show
        /// (wide) when SizeMode is Medium
        /// </summary>
        public Size ItemsSizeInDropwDownMode
        {
            get { return this._ItemsInDropwDownMode; }
            set
            {
                this._ItemsInDropwDownMode = value;
                if (this.Owner != null)
                {
                    this.Owner.OnRegionsChanged();
                }
            }
        }

        /// <summary>
        /// Gets the collection of buttons of the list
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public RibbonButtonCollection Buttons { get; }

        #endregion

        #region Methods

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

        /// <summary>
        /// Redraws the control buttons: up, down and dropdown
        /// </summary>
        private void RedrawControlButtons()
        {
            if (this.Canvas != null)
            {
                if (this.ScrollType == ListScrollType.Scrollbar)
                {
                    this.Canvas.Invalidate(this.ScrollBarBounds);
                }
                else
                {
                    this.Canvas.Invalidate(Rectangle.FromLTRB(this.ButtonUpBounds.Left, this.ButtonUpBounds.Top, this.ButtonDropDownBounds.Right, this.ButtonDropDownBounds.Bottom));
                }
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
            var minOffset = this.ContentBounds.Height - this.fullContentBounds.Height;
            if (offset < minOffset)
            {
                offset = minOffset;
            }

            this._offset = offset;
            this.SetBounds(this.Bounds);
            this.RedrawItem();
        }

        /// <summary>
        /// Scrolls the list down
        /// </summary>
        public void ScrollDown()
        {
            this.ScrollOffset(-(this._jumpDownSize + 1));
        }

        /// <summary>
        /// Scrolls the list up
        /// </summary>
        public void ScrollUp()
        {
            this.ScrollOffset(this._jumpDownSize + 1);
        }

        /// <summary>
        /// Shows the drop down items of the button, as if the dropdown part has been clicked
        /// </summary>
        public void ShowDropDown()
        {
            if (this.DropDownItems.Count == 0)
            {
                this.SetPressed(false);
                return;
            }

            this.IgnoreDeactivation();

            this._dropDown = new RibbonDropDown(this, this.DropDownItems, this.Owner);
            //_dropDown.FormClosed += new FormClosedEventHandler(dropDown_FormClosed);
            //_dropDown.StartPosition = FormStartPosition.Manual;
            this._dropDown.ShowSizingGrip = true;
            var location = this.Canvas.PointToScreen(new Point(this.Bounds.Left, this.Bounds.Top));

            this.SetDropDownVisible(true);
            this._dropDown.Show(location);
        }

        private void dropDown_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.SetDropDownVisible(false);
        }

        /// <summary>
        /// Closes the DropDown if opened
        /// </summary>
        public void CloseDropDown()
        {
            if (this._dropDown != null)
            {
                //RibbonDropDown.DismissTo(_dropDown);
            }

            this.SetDropDownVisible(false);
        }

        /// <summary>
        /// Sets the value of DropDownVisible
        /// </summary>
        /// <param name="visible"></param>
        internal void SetDropDownVisible(bool visible)
        {
            this._dropDownVisible = visible;
        }

        #endregion

        #region Overrides

        public override void OnCanvasChanged(EventArgs e)
        {
            base.OnCanvasChanged(e);

            if (this.Canvas is RibbonDropDown)
            {
                this.ScrollType = ListScrollType.Scrollbar;
            }
            else
            {
                this.ScrollType = ListScrollType.UpDownButtons;
            }
        }

        protected override bool ClosesDropDownAt(Point p)
        {
            return
                !(this.ButtonDropDownBounds.Contains(p) || this.ButtonDownBounds.Contains(p) || this.ButtonUpBounds.Contains(p)
                  || (this.ScrollType == ListScrollType.Scrollbar && this.ScrollBarBounds.Contains(p)));
        }

        internal override void SetOwner(Ribbon owner)
        {
            base.SetOwner(owner);

            this.Buttons.SetOwner(owner);
            this.DropDownItems.SetOwner(owner);
        }

        internal override void SetOwnerPanel(RibbonPanel ownerPanel)
        {
            base.SetOwnerPanel(ownerPanel);

            this.Buttons.SetOwnerPanel(ownerPanel);
            this.DropDownItems.SetOwnerPanel(ownerPanel);
        }

        internal override void SetOwnerTab(RibbonTab ownerTab)
        {
            base.SetOwnerTab(ownerTab);

            this.Buttons.SetOwnerTab(ownerTab);
            this.DropDownItems.SetOwnerTab(this.OwnerTab);
        }

        public override void OnPaint(object sender, RibbonElementPaintEventArgs e)
        {
            this.Owner.Renderer.OnRenderRibbonItem(new RibbonItemRenderEventArgs(this.Owner, e.Graphics, e.Clip, this));

            if (e.Mode != RibbonElementSizeMode.Compact)
            {
                var lastClip = e.Graphics.Clip;
                var newClip = new Region(lastClip.GetBounds(e.Graphics));
                newClip.Intersect(this.ContentBounds);
                e.Graphics.SetClip(newClip.GetBounds(e.Graphics));

                foreach (RibbonButton button in this.Buttons)
                {
                    if (!button.Bounds.IsEmpty)
                    {
                        button.OnPaint(this, new RibbonElementPaintEventArgs(button.Bounds, e.Graphics, this.ButtonsSizeMode));
                    }
                }
                e.Graphics.SetClip(lastClip.GetBounds(e.Graphics));
            }
        }

        public override void SetBounds(Rectangle bounds)
        {
            base.SetBounds(bounds);

            #region Assign control buttons bounds

            if (this.ScrollType != ListScrollType.Scrollbar)
            {
                #region Custom Buttons

                var cbtns = 3; // Canvas is RibbonDropDown ? 2 : 3;
                var buttonHeight = bounds.Height / cbtns;
                var buttonWidth = this._controlButtonsWidth;

                this._buttonUpBounds = Rectangle.FromLTRB(bounds.Right - buttonWidth, bounds.Top, bounds.Right, bounds.Top + buttonHeight);

                this._buttonDownBounds = Rectangle.FromLTRB(this._buttonUpBounds.Left, this._buttonUpBounds.Bottom, bounds.Right, this._buttonUpBounds.Bottom + buttonHeight);

                if (cbtns == 2)
                {
                    this._buttonDropDownBounds = Rectangle.Empty;
                }
                else
                {
                    this._buttonDropDownBounds = Rectangle.FromLTRB(this._buttonDownBounds.Left, this._buttonDownBounds.Bottom, bounds.Right, bounds.Bottom + 1);
                }

                this._thumbBounds.Location = Point.Empty;

                #endregion
            }
            else
            {
                #region Scrollbar

                var bwidth = this.ThumbBounds.Width;
                var bheight = this.ThumbBounds.Width;

                this._buttonUpBounds = Rectangle.FromLTRB(bounds.Right - bwidth, bounds.Top + 1, bounds.Right, bounds.Top + bheight + 1);

                this._buttonDownBounds = Rectangle.FromLTRB(this._buttonUpBounds.Left, bounds.Bottom - bheight, bounds.Right, bounds.Bottom);

                this._buttonDropDownBounds = Rectangle.Empty;

                this._thumbBounds.X = this._buttonUpBounds.Left;

                #endregion
            }

            this._contentBounds = Rectangle.FromLTRB(bounds.Left + 1, bounds.Top + 1, this._buttonUpBounds.Left - 1, bounds.Bottom - 1);

            #endregion

            #region Assign buttons regions

            this.ButtonUpEnabled = this._offset < 0;
            if (!this.ButtonUpEnabled)
            {
                this._offset = 0;
            }
            this.ButtonDownEnabled = false;

            var curLeft = this.ContentBounds.Left + 1;
            var curTop = this.ContentBounds.Top + 1 + this._offset;
            var maxBottom = curTop; // int.MinValue;
            var iniTop = curTop;

            foreach (var item in this.Buttons)
            {
                item.SetBounds(Rectangle.Empty);
            }

            for (var i = 0; i < this.Buttons.Count; i++)
            {
                var button = this.Buttons[i] as RibbonButton;
                if (button == null)
                {
                    break;
                }

                if (curLeft + button.LastMeasuredSize.Width > this.ContentBounds.Right)
                {
                    curLeft = this.ContentBounds.Left + 1;
                    curTop = maxBottom + 1;
                }
                button.SetBounds(new Rectangle(curLeft, curTop, button.LastMeasuredSize.Width, button.LastMeasuredSize.Height));

                curLeft = button.Bounds.Right + 1;
                maxBottom = Math.Max(maxBottom, button.Bounds.Bottom);

                if (button.Bounds.Bottom > this.ContentBounds.Bottom)
                {
                    this.ButtonDownEnabled = true;
                }

                this._jumpDownSize = button.Bounds.Height;
                this._jumpUpSize = button.Bounds.Height;
            }
            //Kevin - The bottom row of buttons were always getting cropped off a tiny bit
            maxBottom += 1;

            #endregion

            #region Adjust thumb size

            double contentHeight = maxBottom - iniTop;
            double viewHeight = this.ContentBounds.Height;

            if (contentHeight > viewHeight && contentHeight != 0)
            {
                var viewPercent = viewHeight / contentHeight;
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

                this._thumbBounds.Height = Convert.ToInt32(thumbHeight);

                this.fullContentBounds = Rectangle.FromLTRB(this.ContentBounds.Left, iniTop, this.ContentBounds.Right, maxBottom);

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

        public override Size MeasureSize(object sender, RibbonElementMeasureSizeEventArgs e)
        {
            if (!this.Visible && !this.Owner.IsDesignMode())
            {
                this.SetLastMeasuredSize(new Size(0, 0));
                return this.LastMeasuredSize;
            }

            #region Determine items

            var itemsWide = 0;

            switch (e.SizeMode)
            {
                case RibbonElementSizeMode.DropDown:
                    itemsWide = this.ItemsSizeInDropwDownMode.Width;
                    break;
                case RibbonElementSizeMode.Large:
                    itemsWide = this.ItemsWideInLargeMode;
                    break;
                case RibbonElementSizeMode.Medium:
                    itemsWide = this.ItemsWideInMediumMode;
                    break;
                case RibbonElementSizeMode.Compact:
                    itemsWide = 0;
                    break;
            }

            #endregion

            var height = this.OwnerPanel.ContentBounds.Height - this.Owner.ItemPadding.Vertical - 4;
            var scannedItems = 0;
            var widthSum = 1;
            var buttonHeight = 0;
            var heightSum = 0;
            var sumWidth = true;

            foreach (RibbonButton button in this.Buttons)
            {
                var s = button.MeasureSize(this, new RibbonElementMeasureSizeEventArgs(e.Graphics, this.ButtonsSizeMode));

                if (sumWidth)
                {
                    widthSum += s.Width + 1;
                }

                buttonHeight = button.LastMeasuredSize.Height;
                heightSum += buttonHeight;

                if (++scannedItems == itemsWide)
                {
                    sumWidth = false;
                }
            }

            if (e.SizeMode == RibbonElementSizeMode.DropDown)
            {
                height = buttonHeight * this.ItemsSizeInDropwDownMode.Height;
            }

            if (ScrollBarRenderer.IsSupported)
            {
                this._thumbBounds = new Rectangle(Point.Empty, ScrollBarRenderer.GetSizeBoxSize(e.Graphics, ScrollBarState.Normal));
            }
            else
            {
                this._thumbBounds = new Rectangle(Point.Empty, new Size(16, 16));
            }

            //if (height < 0)
            //{
            //    throw new Exception("???");
            //}

            //Got off the patch site from logicalerror
            //SetLastMeasuredSize(new Size(widthSum + ControlButtonsWidth, height));
            this.SetLastMeasuredSize(new Size(Math.Max(0, widthSum + this.ControlButtonsWidth), Math.Max(0, height)));

            return this.LastMeasuredSize;
        }

        internal override void SetSizeMode(RibbonElementSizeMode sizeMode)
        {
            base.SetSizeMode(sizeMode);

            foreach (var item in this.Buttons)
            {
                item.SetSizeMode(this.ButtonsSizeMode);
            }
        }

        public override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

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
            var dropCache = this.ButtonDropDownSelected;
            var thumbCache = this.ThumbSelected;

            this.ButtonUpSelected = this._buttonUpBounds.Contains(e.Location);
            this.ButtonDownSelected = this._buttonDownBounds.Contains(e.Location);
            this.ButtonDropDownSelected = this._buttonDropDownBounds.Contains(e.Location);
            this.ThumbSelected = this._thumbBounds.Contains(e.Location) && this.ScrollType == ListScrollType.Scrollbar && this.ScrollBarEnabled;

            if ((upCache != this.ButtonUpSelected) || (downCache != this.ButtonDownSelected) || (dropCache != this.ButtonDropDownSelected) || (thumbCache != this.ThumbSelected))
            {
                this.RedrawControlButtons();
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
                this.RedrawScroll();
            }
        }

        public override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);

            var mustRedraw = this.ButtonUpSelected || this.ButtonDownSelected || this.ButtonDropDownSelected;

            this.ButtonUpSelected = false;
            this.ButtonDownSelected = false;
            this.ButtonDropDownSelected = false;

            if (mustRedraw)
            {
                this.RedrawControlButtons();
            }
        }

        public override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (this.ButtonDownSelected || this.ButtonUpSelected || this.ButtonDropDownSelected)
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

            if (this.ButtonDropDownSelected)
            {
                this.ButtonDropDownPressed = true;
                this.ShowDropDown();
            }

            if (this.ThumbSelected)
            {
                this.ThumbPressed = true;
                this._thumbOffset = e.Y - this._thumbBounds.Y;
            }

            if (this.ScrollType == ListScrollType.Scrollbar && this.ScrollBarBounds.Contains(e.Location) && e.Y >= this.ButtonUpBounds.Bottom && e.Y <= this.ButtonDownBounds.Y
                && !this.ThumbBounds.Contains(e.Location) && !this.ButtonDownBounds.Contains(e.Location) && !this.ButtonUpBounds.Contains(e.Location))
            {
                //clicked the scroll area above or below the thumb
                if (e.Y < this.ThumbBounds.Y)
                {
                    this.ScrollOffset(this.ContentBounds.Height);
                }
                else
                {
                    this.ScrollOffset(-this.ContentBounds.Height);
                }
            }
        }

        public override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            this.ButtonDownPressed = false;
            this.ButtonUpPressed = false;
            this.ButtonDropDownPressed = false;
            this.ThumbPressed = false;
        }

        public override void OnClick(EventArgs e)
        {
            //we need to override the onclick otherwise clicking on the scrollbar will close the popup window
            var pop = this.Canvas as RibbonPopup;
            if (pop == null)
            {
                base.OnClick(e);
            }
        }

        public void OnDropDownItemClicked(ref RibbonItemEventArgs e)
        {
            if (this.DropDownItemClicked != null)
            {
                this.DropDownItemClicked(e.Item, e);
            }
        }

        public void OnButtonItemClicked(ref RibbonItemEventArgs e)
        {
            if (this.ButtonItemClicked != null)
            {
                this.ButtonItemClicked(e.Item, e);
            }
        }

        #endregion

        #region IContainsRibbonItems Members

        public IEnumerable<RibbonItem> GetItems()
        {
            return this.Buttons;
        }

        public Rectangle GetContentBounds()
        {
            return this.ContentBounds;
        }

        #endregion
    }
}