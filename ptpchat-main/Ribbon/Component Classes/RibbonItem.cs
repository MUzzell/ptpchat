namespace PtpChat.Main.Ribbon.Component_Classes
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Drawing;
    using System.Windows.Forms;

    using IContainsSelectableRibbonItems = PtpChat.Main.Ribbon.Classes.Interfaces.IContainsSelectableRibbonItems;
    using IRibbonElement = PtpChat.Main.Ribbon.Classes.Interfaces.IRibbonElement;
    using IRibbonToolTip = PtpChat.Main.Ribbon.Classes.Interfaces.IRibbonToolTip;
    using RibbonDesigner = PtpChat.Main.Ribbon.Classes.Designers.RibbonDesigner;
    using RibbonElementMeasureSizeEventArgs = PtpChat.Main.Ribbon.Classes.EventArgs.RibbonElementMeasureSizeEventArgs;
    using RibbonElementPaintEventArgs = PtpChat.Main.Ribbon.Classes.EventArgs.RibbonElementPaintEventArgs;
    using RibbonElementPopupEventArgs = PtpChat.Main.Ribbon.Classes.EventArgs.RibbonElementPopupEventArgs;
    using RibbonElementPopupEventHandler = PtpChat.Main.Ribbon.Classes.EventArgs.RibbonElementPopupEventHandler;
    using RibbonElementSizeMode = PtpChat.Main.Ribbon.Classes.Enums.RibbonElementSizeMode;
    using RibbonPopupManager = PtpChat.Main.Ribbon.Classes.RibbonPopupManager;

    [DesignTimeVisible(false)]
    public abstract class RibbonItem : Component, IRibbonElement, IRibbonToolTip
    {
        #region enums

        public enum RibbonItemTextAlignment
        {
            Left = StringAlignment.Near,

            Right = StringAlignment.Far,

            Center = StringAlignment.Center
        }

        #endregion

        #region Flashtimer

        private void _flashTimer_Tick(object sender, EventArgs e)
        {
            this._showFlashImage = !this._showFlashImage;
            this.NotifyOwnerRegionsChanged();
        }

        #endregion

        #region Fields

        private string _text;

        private Image _image;

        private bool _checked;

        private bool _selected;

        private bool _pressed;

        private bool _enabled;

        private RibbonElementSizeMode _maxSize;

        private RibbonElementSizeMode _minSize;

        private Control _canvas;

        private bool _visible;

        private RibbonItemTextAlignment _textAlignment;

        private bool _flashEnabled;

        private int _flashIntervall = 1000;

        private Image _flashImage;

        private readonly Timer _flashTimer = new Timer();

        protected bool _showFlashImage;

        private readonly RibbonToolTip _TT;

        private static RibbonToolTip _lastActiveToolTip;

        private string _checkedGroup;

        #endregion

        #region Events

        public virtual event EventHandler DoubleClick;

        public virtual event EventHandler Click;

        public virtual event MouseEventHandler MouseUp;

        public virtual event MouseEventHandler MouseMove;

        public virtual event MouseEventHandler MouseDown;

        public virtual event MouseEventHandler MouseEnter;

        public virtual event MouseEventHandler MouseLeave;

        public virtual event EventHandler CanvasChanged;

        public virtual event EventHandler OwnerChanged;

        /// <summary>
        /// Occurs before a ToolTip is initially displayed.
        /// <remarks>Use this event to change the ToolTip or Cancel it at all.</remarks>
        /// </summary>
        public virtual event RibbonElementPopupEventHandler ToolTipPopUp;

        #endregion

        #region Ctor

        public RibbonItem()
        {
            this._enabled = true;
            this._visible = true;
            this.Click += this.RibbonItem_Click;
            this._flashTimer.Tick += this._flashTimer_Tick;

            //Initialize the ToolTip for this Item
            this._TT = new RibbonToolTip(this);
            this._TT.InitialDelay = 100;
            this._TT.AutomaticDelay = 800;
            this._TT.AutoPopDelay = 8000;
            this._TT.UseAnimation = true;
            this._TT.Active = false;
            this._TT.Popup += this._TT_Popup;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && RibbonDesigner.Current == null)
            {
                this._flashTimer.Enabled = false;

                // ADDED
                this._TT.Popup -= this._TT_Popup;

                this._TT.Dispose();
                if (this.Image != null)
                {
                    this.Image.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Selects the item when in a dropdown, in design mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RibbonItem_Click(object sender, EventArgs e)
        {
            var dd = this.Canvas as RibbonDropDown;

            if (dd != null && dd.SelectionService != null)
            {
                dd.SelectionService.SetSelectedComponents(new Component[] { this }, SelectionTypes.Primary);
            }
        }

        #endregion

        #region Props

        /// <summary>
        /// Gets the bounds of the item's content. (It takes the Ribbon.ItemMargin)
        /// </summary>
        /// <remarks>
        /// Although this is the regular item content bounds, it depends on the logic of the item 
        /// and how each item handles its own content.
        /// </remarks>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual Rectangle ContentBounds
        {
            get
            {
                //Kevin - another point in the designer where an error is thrown when Owner is null
                if (this.Owner == null)
                {
                    return Rectangle.Empty;
                }

                return Rectangle.FromLTRB(
                    this.Bounds.Left + this.Owner.ItemMargin.Left,
                    this.Bounds.Top + this.Owner.ItemMargin.Top,
                    this.Bounds.Right - this.Owner.ItemMargin.Right,
                    this.Bounds.Bottom - this.Owner.ItemMargin.Bottom);
            }
        }

        /// <summary>
        /// Gets the control where the item is currently being dawn
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Control Canvas
        {
            get
            {
                if (this._canvas != null && !this._canvas.IsDisposed)
                {
                    return this._canvas;
                }

                return this.Owner;
            }
        }

        /// <summary>
        /// Gets the RibbonItemGroup that owns the item (If any)
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RibbonItem OwnerItem { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating if the Image should be Flashing
        /// </summary>
        [DefaultValue(false)]
        [Category("Flash")]
        public bool FlashEnabled
        {
            get { return this._flashEnabled; }
            set
            {
                if (this._flashEnabled != value)
                {
                    this._flashEnabled = value;

                    if (this._flashEnabled)
                    {
                        this._showFlashImage = false;
                        this._flashTimer.Interval = this._flashIntervall;
                        this._flashTimer.Enabled = true;
                    }
                    else
                    {
                        this._flashTimer.Enabled = false;
                        this._showFlashImage = false;
                        this.NotifyOwnerRegionsChanged();
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating the flashing frequency in Milliseconds
        /// </summary>
        [DefaultValue(1000)]
        [Category("Flash")]
        public int FlashIntervall
        {
            get { return this._flashIntervall; }
            set
            {
                if (this._flashIntervall != value)
                {
                    this._flashIntervall = value;
                }
            }
        }

        [DefaultValue(null)]
        [Category("Flash")]
        public Image FlashImage
        {
            get { return this._flashImage; }
            set
            {
                if (this._flashImage != value)
                {
                    this._flashImage = value;
                }
            }
        }

        [DefaultValue(false)]
        [Browsable(false)]
        public bool ShowFlashImage
        {
            get { return this._showFlashImage; }
            set
            {
                if (this._showFlashImage != value)
                {
                    this._showFlashImage = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the text that is to be displayed on the item
        /// </summary>
        [DefaultValue(null)]
        [Localizable(true)]
        public virtual string Text
        {
            get { return this._text; }
            set
            {
                this._text = value;

                this.NotifyOwnerRegionsChanged();
            }
        }

        /// <summary>
        /// Gets or sets the image to be displayed on the item
        /// </summary>
        [DefaultValue(null)]
        public virtual Image Image
        {
            get { return this._image; }
            set
            {
                this._image = value;

                this.NotifyOwnerRegionsChanged();
            }
        }

        /// <summary>
        /// Gets or sets the Visibility of this item
        /// </summary>
        [DefaultValue(true)]
        public virtual bool Visible
        {
            get { return this._visible; }
            set
            {
                this._visible = value;

                this.NotifyOwnerRegionsChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating if the item is currently checked
        /// </summary>
        [DefaultValue(false)]
        public virtual bool Checked
        {
            get { return this._checked; }
            set
            {
                this._checked = value;
                //Kevin Carbis - implementing the CheckGroup property logic.  This will uncheck all the other buttons in this group
                if (value)
                {
                    if (this.Canvas is RibbonDropDown)
                    {
                        foreach (var itm in ((RibbonDropDown)this.Canvas).Items)
                        {
                            if (itm.CheckedGroup == this._checkedGroup && itm.Checked && itm != this)
                            {
                                itm.Checked = false;
                                itm.RedrawItem();
                            }
                        }
                    }
                    else if ((this.OwnerPanel != null) && (this._checkedGroup != null))
                    {
                        foreach (var itm in this.OwnerPanel.Items)
                        {
                            if (itm.CheckedGroup == this._checkedGroup && itm.Checked && itm != this)
                            {
                                itm.Checked = false;
                                itm.RedrawItem();
                            }
                        }
                    }
                }

                this.NotifyOwnerRegionsChanged();
            }
        }

        /// <summary>
        /// Determins the other Ribbon Items that belong to this checked group.  When one button is checked the other items in this group will be unchecked automatically.  This only applies to Items that are within the same Ribbon Panel or Dropdown Window.
        /// </summary>
        /// <remarks></remarks>
        [DefaultValue(null)]
        [Description(
            "Determins the other Ribbon Items that belong to this checked group.  When one button is checked the other items in this group will be unchecked automatically.  This only applies to Items that are within the same Parent"
            )]
        public virtual string CheckedGroup { get { return this._checkedGroup; } set { this._checkedGroup = value; } }

        /// <summary>
        /// Gets the item's current SizeMode
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RibbonElementSizeMode SizeMode { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the item is selected
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual bool Selected => this._selected;

        /// <summary>
        /// Gets a value indicating whether the state of the item is pressed
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual bool Pressed => this._pressed;

        /// <summary>
        /// Gets the Ribbon owner of this item
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Ribbon Owner { get; private set; }

        /// <summary>
        /// Gets the bounds of the element relative to the Ribbon control
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Rectangle Bounds { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating if the item is currently enabled
        /// </summary>
        [DefaultValue(true)]
        public virtual bool Enabled
        {
            get { return this._enabled; }
            set
            {
                this._enabled = value;

                var container = this as IContainsSelectableRibbonItems;

                if (container != null)
                {
                    foreach (var item in container.GetItems())
                    {
                        item.Enabled = value;
                    }
                }

                this.NotifyOwnerRegionsChanged();
            }
        }

        /// <summary>
        /// Gets or sets the tool tip title
        /// </summary>
        [DefaultValue("")]
        public string ToolTipTitle { get { return this._TT.ToolTipTitle; } set { this._TT.ToolTipTitle = value; } }

        /// <summary>
        /// Gets or sets the image of the tool tip
        /// </summary>
        [DefaultValue(ToolTipIcon.None)]
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ToolTipIcon ToolTipIcon { get { return this._TT.ToolTipIcon; } set { this._TT.ToolTipIcon = value; } }

        /// <summary>
        /// Gets or sets the tool tip text
        /// </summary>
        [DefaultValue(null)]
        [Localizable(true)]
        public string ToolTip { get; set; }

        /// <summary>
        /// Gets or sets the tool tip image
        /// </summary>
        [DefaultValue(null)]
        [Localizable(true)]
        public Image ToolTipImage { get { return this._TT.ToolTipImage; } set { this._TT.ToolTipImage = value; } }

        /// <summary>
        /// Gets or sets the custom object data associated with this control
        /// </summary>
        [Description("An Object field for associating custom data for this control")]
        [DefaultValue(null)]
        [TypeConverter(typeof(StringConverter))]
        public object Tag { get; set; }

        /// <summary>
        /// Gets or sets the custom string data associated with this control
        /// </summary>
        [DefaultValue(null)]
        [Description("A string field for associating custom data for this control")]
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the key combination that activates this element when the Alt key was pressed
        /// </summary>
        [DefaultValue(null)]
        public string AltKey { get; set; }

        /// <summary>
        /// Gets the RibbonTab that contains this item
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RibbonTab OwnerTab { get; private set; }

        /// <summary>
        /// Gets the RibbonPanel where this item is located
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RibbonPanel OwnerPanel { get; private set; }

        /// <summary>
        /// Gets or sets the maximum size mode of the element
        /// </summary>
        [DefaultValue(RibbonElementSizeMode.None)]
        public RibbonElementSizeMode MaxSizeMode
        {
            get { return this._maxSize; }
            set
            {
                this._maxSize = value;

                this.NotifyOwnerRegionsChanged();
            }
        }

        /// <summary>
        /// Gets or sets the minimum size mode of the element
        /// </summary>
        [DefaultValue(RibbonElementSizeMode.None)]
        public RibbonElementSizeMode MinSizeMode
        {
            get { return this._minSize; }
            set
            {
                this._minSize = value;

                this.NotifyOwnerRegionsChanged();
            }
        }

        /// <summary>
        /// Gets the last result of  MeasureSize
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Size LastMeasuredSize { get; private set; }

        /// <summary>
        /// Sets the alignment of the label text if it exists
        /// </summary>
        [DefaultValue(RibbonItemTextAlignment.Left)]
        public RibbonItemTextAlignment TextAlignment
        {
            get { return this._textAlignment; }
            set
            {
                this._textAlignment = value;
                this.NotifyOwnerRegionsChanged();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets if owner dropdown must be closed when the item is clicked on the specified point
        /// </summary>
        /// <param name="p">Point to test.</param>
        /// <returns></returns>
        protected virtual bool ClosesDropDownAt(Point p)
        {
            return true;
        }

        /// <summary>
        /// Forces the owner Ribbon to update its regions
        /// </summary>
        protected void NotifyOwnerRegionsChanged()
        {
            if (this.Owner != null)
            {
                if (this.Owner == this.Canvas)
                {
                    this.Owner.OnRegionsChanged();
                }
                else if (this.Canvas != null)
                {
                    if (this.Canvas is RibbonOrbDropDown)
                    {
                        (this.Canvas as RibbonOrbDropDown).OnRegionsChanged();
                    }
                    else
                    {
                        this.Canvas.Invalidate(this.Bounds);
                    }
                }
            }
        }

        /// <summary>
        /// Sets the value of the <see cref="OwnerItem"/> property
        /// </summary>
        /// <param name="item"></param>
        internal virtual void SetOwnerItem(RibbonItem item)
        {
            this.OwnerItem = item;
        }

        /// <summary>
        /// Sets the Ribbon that owns this item
        /// </summary>
        /// <param name="owner">Ribbon that owns this item</param>
        internal virtual void SetOwner(Ribbon owner)
        {
            this.Owner = owner;
            this.OnOwnerChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Sets the value of the OwnerPanel property
        /// </summary>
        /// <param name="ownerPanel">RibbonPanel where this item is located</param>
        internal virtual void SetOwnerPanel(RibbonPanel ownerPanel)
        {
            this.OwnerPanel = ownerPanel;
        }

        /// <summary>
        /// Sets the value of the Selected property
        /// </summary>
        /// <param name="selected">Value that indicates if the element is selected</param>
        internal virtual void SetSelected(bool selected)
        {
            if (!this.Enabled)
            {
                return;
            }

            this._selected = selected;
        }

        /// <summary>
        /// Sets the value of the Pressed property
        /// </summary>
        /// <param name="pressed">Value that indicates if the element is pressed</param>
        internal virtual void SetPressed(bool pressed)
        {
            this._pressed = pressed;
        }

        /// <summary>
        /// Sets the value of the OwnerTab property
        /// </summary>
        /// <param name="ownerTab">RibbonTab where this item is located</param>
        internal virtual void SetOwnerTab(RibbonTab ownerTab)
        {
            this.OwnerTab = ownerTab;
        }

        /// <summary>
        /// Sets the value of the OwnerList property
        /// </summary>
        /// <param name="ownerList"></param>
        internal virtual void SetOwnerGroup(RibbonItemGroup ownerGroup)
        {
            this.OwnerItem = ownerGroup;
        }

        /// <summary>
        /// Gets the size applying the rules of MaxSizeMode and MinSizeMode properties
        /// </summary>
        /// <param name="sizeMode">Suggested sizeMode</param>
        /// <returns>The nearest size to the specified one</returns>
        protected RibbonElementSizeMode GetNearestSize(RibbonElementSizeMode sizeMode)
        {
            var size = (int)sizeMode;
            var max = (int)this.MaxSizeMode;
            var min = (int)this.MinSizeMode;
            var result = (int)sizeMode;

            if (max > 0 && size > max) //Max is specified and value exceeds max
            {
                result = max;
            }

            if (min > 0 && size < min) //Min is specified and value exceeds min
            {
                result = min;
            }

            return (RibbonElementSizeMode)result;
        }

        /// <summary>
        /// Sets the value of the LastMeasuredSize property
        /// </summary>
        /// <param name="size">Size to set to the property</param>
        protected void SetLastMeasuredSize(Size size)
        {
            this.LastMeasuredSize = size;
        }

        /// <summary>
        /// Sets the value of the SizeMode property
        /// </summary>
        /// <param name="sizeMode"></param>
        internal virtual void SetSizeMode(RibbonElementSizeMode sizeMode)
        {
            this.SizeMode = this.GetNearestSize(sizeMode);
        }

        /// <summary>
        /// Raises the <see cref="CanvasChanged"/> event
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnCanvasChanged(EventArgs e)
        {
            if (this.CanvasChanged != null)
            {
                this.CanvasChanged(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="OwnerChanged"/> event
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnOwnerChanged(EventArgs e)
        {
            if (this.OwnerChanged != null)
            {
                this.OwnerChanged(this, e);
            }
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

            //RibbonPopup pop = Canvas as RibbonPopup;

            //if (pop != null)
            //{
            //   if (ClosesDropDownAt(e.Location))
            //   {
            //      RibbonPopupManager.Dismiss(RibbonPopupManager.DismissReason.ItemClicked);
            //   }
            //OnClick(EventArgs.Empty);
            //}

            this.SetPressed(true);
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

            this.DeactivateToolTip(this._TT);

            if (this.MouseLeave != null)
            {
                this.MouseLeave(this, e);
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

            if (this.Pressed)
            {
                this.SetPressed(false);
                this.RedrawItem();
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

            //Kevin - found cases where mousing into buttons doesn't set the selection. This arose with the office 2010 style
            if (!this.Selected)
            {
                this.SetSelected(true);
                this.Owner.Invalidate(this.Bounds);
            }

            if (!this._TT.Active && !string.IsNullOrEmpty(this.ToolTip)) // ToolTip should be working without title as well - to get Office 2007 Look & Feel
            {
                this.DeactivateToolTip(_lastActiveToolTip);
                if (this.ToolTip != this._TT.GetToolTip(this.Canvas))
                {
                    this._TT.SetToolTip(this.Canvas, this.ToolTip);
                }
                this._TT.Active = true;
                _lastActiveToolTip = null;
                _lastActiveToolTip = this._TT;
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

            if (this.ClosesDropDownAt(this.Canvas.PointToClient(Cursor.Position)))
            {
                this.DeactivateToolTip(this._TT);
                RibbonPopupManager.Dismiss(RibbonPopupManager.DismissReason.ItemClicked);
            }

            this.SetSelected(false);

            if (this.Click != null)
            {
                this.Click(this, e);
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
        /// Redraws the item area on the Onwer Ribbon
        /// </summary>
        public virtual void RedrawItem()
        {
            if (this.Canvas != null)
            {
                this.Canvas.Invalidate(Rectangle.Inflate(this.Bounds, 1, 1));
            }
        }

        /// <summary>
        /// Sets the canvas of the item
        /// </summary>
        /// <param name="canvas"></param>
        internal void SetCanvas(Control canvas)
        {
            this._canvas = canvas;

            this.SetCanvas(this as IContainsSelectableRibbonItems, canvas);

            this.OnCanvasChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Recurse on setting the canvas
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="canvas"></param>
        private void SetCanvas(IContainsSelectableRibbonItems parent, Control canvas)
        {
            if (parent == null)
            {
                return;
            }

            foreach (var item in parent.GetItems())
            {
                item.SetCanvas(canvas);
            }
        }

        private void _TT_Popup(object sender, PopupEventArgs e)
        {
            if (this.ToolTipPopUp != null)
            {
                this.ToolTipPopUp(sender, new RibbonElementPopupEventArgs(this, e));
                if (this.ToolTip != this._TT.GetToolTip(this.Canvas))
                {
                    this._TT.SetToolTip(this.Canvas, this.ToolTip);
                }
            }
        }

        private void DeactivateToolTip(RibbonToolTip toolTip)
        {
            if (toolTip == null)
            {
                return;
            }

            toolTip.Active = false;
            toolTip.RemoveAll(); // this is needed otherwise a tooltip within a dropdown is not shown again if the item is clicked
        }

        #endregion

        #region IRibbonElement Members

        public abstract void OnPaint(object sender, RibbonElementPaintEventArgs e);

        public virtual void SetBounds(Rectangle bounds)
        {
            this.Bounds = bounds;
        }

        public abstract Size MeasureSize(object sender, RibbonElementMeasureSizeEventArgs e);

        #endregion
    }
}