namespace PtpChat.Main.Ribbon.Component_Classes
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Drawing;
    using System.Windows.Forms;

    using PtpChat.Main.Ribbon.Classes.Designers;

    using IContainsRibbonComponents = PtpChat.Main.Ribbon.Classes.Interfaces.IContainsRibbonComponents;
    using RibbonArrowDirection = PtpChat.Main.Ribbon.Classes.Enums.RibbonArrowDirection;
    using RibbonButtonStyle = PtpChat.Main.Ribbon.Classes.Enums.RibbonButtonStyle;
    using RibbonDesigner = PtpChat.Main.Ribbon.Classes.Designers.RibbonDesigner;
    using RibbonElementMeasureSizeEventArgs = PtpChat.Main.Ribbon.Classes.EventArgs.RibbonElementMeasureSizeEventArgs;
    using RibbonElementPaintEventArgs = PtpChat.Main.Ribbon.Classes.EventArgs.RibbonElementPaintEventArgs;
    using RibbonElementSizeMode = PtpChat.Main.Ribbon.Classes.Enums.RibbonElementSizeMode;
    using RibbonItemBoundsEventArgs = PtpChat.Main.Ribbon.Classes.EventArgs.RibbonItemBoundsEventArgs;
    using RibbonItemCollection = PtpChat.Main.Ribbon.Classes.Collections.RibbonItemCollection;
    using RibbonItemEventArgs = PtpChat.Main.Ribbon.Classes.EventArgs.RibbonItemEventArgs;
    using RibbonItemRenderEventArgs = PtpChat.Main.Ribbon.Classes.EventArgs.RibbonItemRenderEventArgs;
    using RibbonPopupManager = PtpChat.Main.Ribbon.Classes.RibbonPopupManager;
    using RibbonTextEventArgs = PtpChat.Main.Ribbon.Classes.EventArgs.RibbonTextEventArgs;

    [Designer(typeof(RibbonButtonDesigner))]
    public class RibbonButton : RibbonItem, IContainsRibbonComponents
    {
        #region IContainsRibbonComponents Members

        public IEnumerable<Component> GetAllChildComponents()
        {
            return this.DropDownItems.ToArray();
        }

        #endregion

        #region IContainsRibbonItems Members

        public IEnumerable<RibbonItem> GetItems()
        {
            return this.DropDownItems;
        }

        #endregion

        #region Fields

        private const int arrowWidth = 5;

        private RibbonButtonStyle _style = RibbonButtonStyle.Normal;

        private Image _smallImage;

        private Image _flashSmallImage;

        private Size _dropDownArrowSize;

        private Padding _dropDownMargin;

        private Point _lastMousePos;

        private RibbonArrowDirection _DropDownArrowDirection = RibbonArrowDirection.Down;

        //Kevin - Tracks the selected item when it has dropdownitems assigned
        private RibbonItem _selectedItem;

        private readonly Classes.Set<RibbonItem> _assignedHandlers = new Classes.Set<RibbonItem>();

        private Size _MinimumSize;

        private Size _MaximumSize;

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the dropdown is about to be displayed
        /// </summary>
        public event EventHandler DropDownShowing;

        public event RibbonItemEventHandler DropDownItemClicked;

        public delegate void RibbonItemEventHandler(object sender, RibbonItemEventArgs e);

        public virtual void OnDropDownItemClicked(ref RibbonItemEventArgs e)
        {
            if (this.DropDownItemClicked != null)
            {
                this.DropDownItemClicked(this, e);
            }
        }

        #endregion

        #region Ctors

        /// <summary>
        /// Creates a new button
        /// </summary>
        /// <param name="image">Image of the button (32 x 32 suggested)</param>
        /// <param name="smallImage">Image of the button when in medium of compact mode (16 x 16 suggested)</param>
        /// <param name="style">Style of the button</param>
        /// <param name="text">Text of the button</param>
        public RibbonButton()
        {
            this.DropDownItems = new RibbonItemCollection();
            this._dropDownArrowSize = new Size(5, 3);
            this._dropDownMargin = new Padding(6);
            this.Image = this.CreateImage(32);
            this.SmallImage = this.CreateImage(16);
            this.DrawIconsBar = true;
        }

        public RibbonButton(string text)
            : this()
        {
            this.Text = text;
        }

        public RibbonButton(Image smallImage)
            : this()
        {
            this.SmallImage = smallImage;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && RibbonDesigner.Current == null)
            {
                this.RemoveHandlers();
                if (this.SmallImage != null)
                {
                    this.SmallImage.Dispose();
                }
                if (this.FlashSmallImage != null)
                {
                    this.FlashSmallImage.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #endregion

        #region Props

        /// <summary>
        /// Gets the last DropDown Item that was clicked
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RibbonItem SelectedItem
        {
            get
            {
                if (this._selectedItem == null)
                {
                    return null;
                }
                if (this.DropDownItems.Contains(this._selectedItem))
                {
                    return this._selectedItem;
                }
                this._selectedItem = null;
                return null;
            }

            set
            {
                if (value.GetType().BaseType == typeof(RibbonItem))
                {
                    if (this.DropDownItems.Contains(value))
                    {
                        this._selectedItem = value;
                    }
                    else
                    {
                        this.DropDownItems.Add(value);
                        this._selectedItem = value;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the value of selected item on the dropdown.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string SelectedValue
        {
            get
            {
                if (this._selectedItem == null)
                {
                    return null;
                }
                return this._selectedItem.Value;
            }
            set
            {
                foreach (var item in this.DropDownItems)
                {
                    if (item.Value == value)
                    {
                        this._selectedItem = item;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the DropDown of the button
        /// </summary>
        internal RibbonDropDown DropDown { get; private set; }

        /// <summary>
        /// Gets or sets if the icons bar should be drawn
        /// </summary>
        [DefaultValue(true)]
        public bool DrawIconsBar { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if the <see cref="Checked"/> property should be toggled
        /// when button is clicked
        /// </summary>
        [DefaultValue(false)]
        [Description("Toggles the Checked property of the button when clicked")]
        public bool CheckOnClick { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if the DropDown should be resizable
        /// </summary>
        [DefaultValue(false)]
        [Description("Makes the DropDown resizable with a grip on the corner")]
        public bool DropDownResizable { get; set; }

        /// <summary>
        /// Gets the bounds where the <see cref="Image"/> or <see cref="SmallImage"/> will be drawn.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Rectangle ImageBounds { get; private set; }

        /// <summary>
        /// Gets the bounds where the <see cref="System.Text"/> of the button will be drawn
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Rectangle TextBounds { get; private set; }

        /// <summary>
        /// Gets if the DropDown is currently visible
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DropDownVisible { get; private set; }

        /// <summary>
        /// Gets or sets the size of the dropdown arrow
        /// </summary>
        [DefaultValue(typeof(Size), "5, 3")]
        public Size DropDownArrowSize
        {
            get { return this._dropDownArrowSize; }
            set
            {
                this._dropDownArrowSize = value;
                this.NotifyOwnerRegionsChanged();
            }
        }

        /// <summary>
        /// Gets or sets the direction where drop down's arrow should be pointing to
        /// </summary>
        [DefaultValue(RibbonArrowDirection.Down)]
        public RibbonArrowDirection DropDownArrowDirection
        {
            get { return this._DropDownArrowDirection; }
            set
            {
                this._DropDownArrowDirection = value;
                this.NotifyOwnerRegionsChanged();
            }
        }

        /// <summary>
        /// Gets the style of the button
        /// </summary>
        [DefaultValue(RibbonButtonStyle.Normal)]
        public RibbonButtonStyle Style
        {
            get { return this._style; }
            set
            {
                this._style = value;

                if (this.Canvas is RibbonPopup || (this.OwnerItem != null && this.OwnerItem.Canvas is RibbonPopup))
                {
                    this.DropDownArrowDirection = RibbonArrowDirection.Left;
                }

                this.NotifyOwnerRegionsChanged();
            }
        }

        /// <summary>
        /// Gets the collection of items shown on the dropdown pop-up when Style allows it
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public RibbonItemCollection DropDownItems { get; }

        /// <summary>
        /// Gets the bounds of the button face
        /// </summary>
        /// <remarks>When Style is different from SplitDropDown and SplitBottomDropDown, ButtonFaceBounds is the same area than DropDownBounds</remarks>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Rectangle ButtonFaceBounds { get; private set; }

        /// <summary>
        /// Gets the bounds of the dropdown button
        /// </summary>
        /// <remarks>When Style is different from SplitDropDown and SplitBottomDropDown, ButtonFaceBounds is the same area than DropDownBounds</remarks>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Rectangle DropDownBounds { get; private set; }

        /// <summary>
        /// Gets if the dropdown part of the button is selected
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DropDownSelected { get; private set; }

        /// <summary>
        /// Gets if the dropdown part of the button is pressed
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DropDownPressed { get; private set; }

        /// <summary>
        /// Gets or sets the image of the button when in compact of medium size mode
        /// </summary>
        [DefaultValue(null)]
        public virtual Image SmallImage
        {
            get { return this._smallImage; }
            set
            {
                if (this._smallImage != value)
                {
                    this._smallImage = value;

                    this.NotifyOwnerRegionsChanged();
                }
            }
        }

        [Category("Flash")]
        [DefaultValue(null)]
        public virtual Image FlashSmallImage
        {
            get { return this._flashSmallImage; }
            set
            {
                if (this._flashSmallImage != value)
                {
                    this._flashSmallImage = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the minimum size for this Item.  Only applies when in Large Size Mode.
        /// </summary>
        [DefaultValue(typeof(Size), "0, 0")]
        public Size MinimumSize
        {
            get { return this._MinimumSize; }
            set
            {
                this._MinimumSize = value;
                this.NotifyOwnerRegionsChanged();
            }
        }

        /// <summary>
        /// Gets or sets the maximum size for this Item.  Only applies when in Large Size Mode.
        /// </summary>
        [DefaultValue(typeof(Size), "0, 0")]
        public Size MaximumSize
        {
            get { return this._MaximumSize; }
            set
            {
                this._MaximumSize = value;
                this.NotifyOwnerRegionsChanged();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sets the value of the <see cref="DropDownMargin"/> property
        /// </summary>
        /// <param name="p"></param>
        protected void SetDropDownMargin(Padding p)
        {
            this._dropDownMargin = p;
        }

        /// <summary>
        /// Performs a click on the button
        /// </summary>
        public void PerformClick()
        {
            this.OnClick(EventArgs.Empty);
        }

        /// <summary>
        /// Creates a new Transparent, empty image
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        private Image CreateImage(int size)
        {
            var bmp = new Bitmap(size, size);

            //using (Graphics g = Graphics.FromImage(bmp))
            //{
            //    g.Clear(Color.FromArgb(50, Color.Navy));
            //}

            return bmp;
        }

        /// <summary>
        /// Creates the DropDown menu
        /// </summary>
        protected virtual void CreateDropDown()
        {
            this.DropDown = new RibbonDropDown(this, this.DropDownItems, this.Owner);
        }

        internal override void SetPressed(bool pressed)
        {
            base.SetPressed(pressed);
        }

        internal override void SetOwner(Ribbon owner)
        {
            base.SetOwner(owner);

            if (this.DropDownItems != null)
            {
                this.DropDownItems.SetOwner(owner);
            }
        }

        internal override void SetOwnerPanel(RibbonPanel ownerPanel)
        {
            base.SetOwnerPanel(ownerPanel);

            if (this.DropDownItems != null)
            {
                this.DropDownItems.SetOwnerPanel(ownerPanel);
            }
        }

        internal override void SetOwnerTab(RibbonTab ownerTab)
        {
            base.SetOwnerTab(ownerTab);

            if (this.DropDownItems != null)
            {
                this.DropDownItems.SetOwnerTab(ownerTab);
            }
        }

        /// <summary>
        /// Raises the Paint event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void OnPaint(object sender, RibbonElementPaintEventArgs e)
        {
            if (this.Owner == null)
            {
                return;
            }

            this.OnPaintBackground(e);

            this.OnPaintImage(e);

            this.OnPaintText(e);
        }

        /// <summary>
        /// Renders text of the button
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnPaintText(RibbonElementPaintEventArgs e)
        {
            if (this.SizeMode != RibbonElementSizeMode.Compact)
            {
                var sf = new StringFormat();
                sf.LineAlignment = StringAlignment.Center;
                sf.Alignment = StringAlignment.Near;

                if (this.SizeMode == RibbonElementSizeMode.Large)
                {
                    sf.Alignment = StringAlignment.Center;

                    if (!string.IsNullOrEmpty(this.Text) && !this.Text.Contains(" "))
                    {
                        sf.LineAlignment = StringAlignment.Near;
                    }
                }

                if (this.Style == RibbonButtonStyle.DropDownListItem)
                {
                    sf.LineAlignment = StringAlignment.Near;
                    this.Owner.Renderer.OnRenderRibbonItemText(new RibbonTextEventArgs(this.Owner, e.Graphics, e.Clip, this, this.Bounds, this.Text, sf));
                }
                else
                {
                    this.Owner.Renderer.OnRenderRibbonItemText(new RibbonTextEventArgs(this.Owner, e.Graphics, e.Clip, this, this.TextBounds, this.Text, sf));
                }
            }
        }

        /// <summary>
        /// Renders the image of the button
        /// </summary>
        /// <param name="e"></param>
        private void OnPaintImage(RibbonElementPaintEventArgs e)
        {
            var theSize = this.GetNearestSize(e.Mode);

            if (this._showFlashImage)
            {
                if ((theSize == RibbonElementSizeMode.Large && this.FlashImage != null) || this.SmallImage != null)
                {
                    this.Owner.Renderer.OnRenderRibbonItemImage(new RibbonItemBoundsEventArgs(this.Owner, e.Graphics, e.Clip, this, this.OnGetImageBounds(theSize, this.Bounds)));
                }
            }
            else
            {
                if ((theSize == RibbonElementSizeMode.Large && this.Image != null) || this.SmallImage != null)
                {
                    this.Owner.Renderer.OnRenderRibbonItemImage(new RibbonItemBoundsEventArgs(this.Owner, e.Graphics, e.Clip, this, this.OnGetImageBounds(theSize, this.Bounds)));
                }
            }
        }

        /// <summary>
        /// Renders the background of the buton
        /// </summary>
        /// <param name="e"></param>
        private void OnPaintBackground(RibbonElementPaintEventArgs e)
        {
            this.Owner.Renderer.OnRenderRibbonItem(new RibbonItemRenderEventArgs(this.Owner, e.Graphics, e.Clip, this));
        }

        /// <summary>
        /// Sets the bounds of the button
        /// </summary>
        /// <param name="bounds"></param>
        public override void SetBounds(Rectangle bounds)
        {
            base.SetBounds(bounds);

            var sMode = this.GetNearestSize(this.SizeMode);

            this.ImageBounds = this.OnGetImageBounds(sMode, bounds);

            this.TextBounds = this.OnGetTextBounds(sMode, bounds);

            if (this.Style == RibbonButtonStyle.SplitDropDown)
            {
                this.DropDownBounds = this.OnGetDropDownBounds(sMode, bounds);
                this.ButtonFaceBounds = this.OnGetButtonFaceBounds(sMode, bounds);
            }
        }

        /// <summary>
        /// Sets the bounds of the image of the button when SetBounds is called.
        /// Override this method to change image bounds
        /// </summary>
        /// <param name="sMode">Mode which is being measured</param>
        /// <param name="bounds">Bounds of the button</param>
        /// <remarks>
        /// The measuring occours in the following order:
        /// <list type="">
        /// <item>OnSetImageBounds</item>
        /// <item>OnSetTextBounds</item>
        /// <item>OnSetDropDownBounds</item>
        /// <item>OnSetButtonFaceBounds</item>
        /// </list>
        /// </remarks>
        internal virtual Rectangle OnGetImageBounds(RibbonElementSizeMode sMode, Rectangle bounds)
        {
            if (sMode == RibbonElementSizeMode.Large) // || this is RibbonOrbMenuItem)
            {
                if (this.Image != null)
                {
                    return new Rectangle(this.Bounds.Left + (this.Bounds.Width - this.Image.Width) / 2, this.Bounds.Top + this.Owner.ItemMargin.Top, this.Image.Width, this.Image.Height);
                }
                return new Rectangle(this.ContentBounds.Location, new Size(32, 32));
            }
            if (this.SmallImage != null)
            {
                return new Rectangle(this.Bounds.Left + this.Owner.ItemMargin.Left, this.Bounds.Top + (this.Bounds.Height - this.SmallImage.Height) / 2, this.SmallImage.Width, this.SmallImage.Height);
            }
            return new Rectangle(this.ContentBounds.Location, new Size(0, 0));
        }

        /// <summary>
        /// Sets the bounds of the text of the button when SetBounds is called.
        /// Override this method to change image bounds
        /// </summary>
        /// <param name="sMode">Mode which is being measured</param>
        /// <param name="bounds">Bounds of the button</param>
        /// <remarks>
        /// The measuring occours in the following order:
        /// <list type="">
        /// <item>OnSetImageBounds</item>
        /// <item>OnSetTextBounds</item>
        /// <item>OnSetDropDownBounds</item>
        /// <item>OnSetButtonFaceBounds</item>
        /// </list>
        /// </remarks>
        internal virtual Rectangle OnGetTextBounds(RibbonElementSizeMode sMode, Rectangle bounds)
        {
            var imgw = this.ImageBounds.Width;
            var imgh = this.ImageBounds.Height;

            if (sMode == RibbonElementSizeMode.Large)
            {
                return Rectangle.FromLTRB(
                    this.Bounds.Left + this.Owner.ItemMargin.Left,
                    this.Bounds.Top + this.Owner.ItemMargin.Top + imgh,
                    this.Bounds.Right - this.Owner.ItemMargin.Right,
                    this.Bounds.Bottom - this.Owner.ItemMargin.Bottom);
            }
            var ddw = this.Style != RibbonButtonStyle.Normal ? this._dropDownMargin.Horizontal : 0;
            return Rectangle.FromLTRB(
                this.Bounds.Left + imgw + this.Owner.ItemMargin.Horizontal + this.Owner.ItemMargin.Left,
                this.Bounds.Top + this.Owner.ItemMargin.Top,
                this.Bounds.Right - ddw,
                this.Bounds.Bottom - this.Owner.ItemMargin.Bottom);
        }

        /// <summary>
        /// Sets the bounds of the dropdown part of the button when SetBounds is called.
        /// Override this method to change image bounds
        /// </summary>
        /// <param name="sMode">Mode which is being measured</param>
        /// <param name="bounds">Bounds of the button</param>
        /// <remarks>
        /// The measuring occours in the following order:
        /// <list type="">
        /// <item>OnSetImageBounds</item>
        /// <item>OnSetTextBounds</item>
        /// <item>OnSetDropDownBounds</item>
        /// <item>OnSetButtonFaceBounds</item>
        /// </list>
        /// </remarks>
        internal virtual Rectangle OnGetDropDownBounds(RibbonElementSizeMode sMode, Rectangle bounds)
        {
            var sideBounds = Rectangle.FromLTRB(bounds.Right - this._dropDownMargin.Horizontal - 2, bounds.Top, bounds.Right, bounds.Bottom);

            switch (sMode)
            {
                case RibbonElementSizeMode.Large:
                case RibbonElementSizeMode.Overflow:
                    return Rectangle.FromLTRB(bounds.Left, bounds.Top + this.Image.Height + this.Owner.ItemMargin.Vertical, bounds.Right, bounds.Bottom);

                case RibbonElementSizeMode.DropDown:
                case RibbonElementSizeMode.Medium:
                case RibbonElementSizeMode.Compact:
                    return sideBounds;
            }

            return Rectangle.Empty;
        }

        /// <summary>
        /// Sets the bounds of the button face part of the button when SetBounds is called.
        /// Override this method to change image bounds
        /// </summary>
        /// <param name="sMode">Mode which is being measured</param>
        /// <param name="bounds">Bounds of the button</param>
        /// <remarks>
        /// The measuring occours in the following order:
        /// <list type="">
        /// <item>OnSetImageBounds</item>
        /// <item>OnSetTextBounds</item>
        /// <item>OnSetDropDownBounds</item>
        /// <item>OnSetButtonFaceBounds</item>
        /// </list>
        /// </remarks>
        internal virtual Rectangle OnGetButtonFaceBounds(RibbonElementSizeMode sMode, Rectangle bounds)
        {
            var sideBounds = Rectangle.FromLTRB(bounds.Right - this._dropDownMargin.Horizontal - 2, bounds.Top, bounds.Right, bounds.Bottom);

            switch (sMode)
            {
                case RibbonElementSizeMode.Large:
                case RibbonElementSizeMode.Overflow:
                    return Rectangle.FromLTRB(bounds.Left, bounds.Top, bounds.Right, this.DropDownBounds.Top);

                case RibbonElementSizeMode.DropDown:
                case RibbonElementSizeMode.Medium:
                case RibbonElementSizeMode.Compact:
                    return Rectangle.FromLTRB(bounds.Left, bounds.Top, this.DropDownBounds.Left, bounds.Bottom);
            }

            return Rectangle.Empty;
        }

        /// <summary>
        /// Measures the string for the large size
        /// </summary>
        /// <param name="g"></param>
        /// <param name="text"></param>
        /// <param name="font"></param>
        /// <returns></returns>
        public static Size MeasureStringLargeSize(Graphics g, string text, Font font)
        {
            if (string.IsNullOrEmpty(text))
            {
                return Size.Empty;
            }

            var sz = g.MeasureString(text, font).ToSize();
            var words = text.Split(' ');
            var longestWord = string.Empty;
            var width = sz.Width;

            for (var i = 0; i < words.Length; i++)
            {
                if (words[i].Length > longestWord.Length)
                {
                    longestWord = words[i];
                }
            }

            if (words.Length > 1)
            {
                width = Math.Max(sz.Width / 2, g.MeasureString(longestWord, font).ToSize().Width) + 1;
            }
            else
            {
                return g.MeasureString(text, font).ToSize();
            }

            var rs = g.MeasureString(text, font, width).ToSize();

            return new Size(rs.Width, rs.Height);
        }

        /// <summary>
        /// Measures the size of the button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public override Size MeasureSize(object sender, RibbonElementMeasureSizeEventArgs e)
        {
            if (!this.Visible && !this.Owner.IsDesignMode())
            {
                this.SetLastMeasuredSize(new Size(0, 0));
                return this.LastMeasuredSize;
            }

            var theSize = this.GetNearestSize(e.SizeMode);
            var widthSum = this.Owner.ItemMargin.Horizontal;
            var heightSum = this.Owner.ItemMargin.Vertical;
            var largeHeight = this.OwnerPanel == null ? 0 : this.OwnerPanel.ContentBounds.Height - this.Owner.ItemPadding.Vertical; // -Owner.ItemMargin.Vertical; //58;

            var simg = this.SmallImage != null ? this.SmallImage.Size : Size.Empty;
            var img = this.Image != null ? this.Image.Size : Size.Empty;
            var sz = Size.Empty;

            switch (theSize)
            {
                case RibbonElementSizeMode.Large:
                case RibbonElementSizeMode.Overflow:
                    sz = MeasureStringLargeSize(e.Graphics, this.Text, this.Owner.Font);
                    if (!string.IsNullOrEmpty(this.Text))
                    {
                        widthSum += Math.Max(sz.Width + 1, img.Width);
                        //Got off the patch site from logicalerror
                        //heightSum = largeHeight;
                        heightSum = Math.Max(0, largeHeight);
                    }
                    else
                    {
                        widthSum += img.Width;
                        heightSum += img.Height;
                    }

                    break;
                case RibbonElementSizeMode.DropDown:
                    sz = TextRenderer.MeasureText(this.Text, this.Owner.Font);
                    if (!string.IsNullOrEmpty(this.Text))
                    {
                        widthSum += sz.Width + 1;
                    }
                    widthSum += simg.Width + this.Owner.ItemMargin.Horizontal;
                    heightSum += Math.Max(sz.Height, simg.Height);
                    heightSum += 2;
                    break;
                case RibbonElementSizeMode.Medium:
                    sz = TextRenderer.MeasureText(this.Text, this.Owner.Font);
                    if (!string.IsNullOrEmpty(this.Text))
                    {
                        widthSum += sz.Width + 1;
                    }
                    widthSum += simg.Width + this.Owner.ItemMargin.Horizontal;
                    heightSum += Math.Max(sz.Height, simg.Height);
                    break;
                case RibbonElementSizeMode.Compact:
                    widthSum += simg.Width;
                    heightSum += simg.Height;
                    break;
                default:
                    throw new ApplicationException("SizeMode not supported: " + e.SizeMode);
            }

            //if (theSize == RibbonElementSizeMode.DropDown)
            //{
            //   heightSum += 2;
            //}
            switch (this.Style)
            {
                case RibbonButtonStyle.DropDown:
                case RibbonButtonStyle.SplitDropDown: // drawing size calculation for DropDown and SplitDropDown is identical
                    widthSum += arrowWidth + this._dropDownMargin.Horizontal;
                    break;
                case RibbonButtonStyle.DropDownListItem:
                    break;
            }

            //check the minimum and mazimum size properties but only in large mode
            if (theSize == RibbonElementSizeMode.Large)
            {
                //Minimum Size
                if (this.MinimumSize.Height > 0 && heightSum < this.MinimumSize.Height)
                {
                    heightSum = this.MinimumSize.Height;
                }
                if (this.MinimumSize.Width > 0 && widthSum < this.MinimumSize.Width)
                {
                    widthSum = this.MinimumSize.Width;
                }

                //Maximum Size
                if (this.MaximumSize.Height > 0 && heightSum > this.MaximumSize.Height)
                {
                    heightSum = this.MaximumSize.Height;
                }
                if (this.MaximumSize.Width > 0 && widthSum > this.MaximumSize.Width)
                {
                    widthSum = this.MaximumSize.Width;
                }
            }

            this.SetLastMeasuredSize(new Size(widthSum, heightSum));

            return this.LastMeasuredSize;
        }

        /// <summary>
        /// Sets the value of the DropDownPressed property
        /// </summary>
        /// <param name="pressed">Value that indicates if the dropdown button is pressed</param>
        internal void SetDropDownPressed(bool pressed)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets the value of the DropDownSelected property
        /// </summary>
        /// <param name="selected">Value that indicates if the dropdown part of the button is selected</param>
        internal void SetDropDownSelected(bool selected)
        {
            //Dont use, an overflow occours
            throw new Exception();
        }

        /// <summary>
        /// Shows the drop down items of the button, as if the dropdown part has been clicked
        /// </summary>
        public void ShowDropDown()
        {
            if (this.Style == RibbonButtonStyle.Normal || this.DropDownItems.Count == 0)
            {
                if (this.DropDown != null)
                {
                    RibbonPopupManager.DismissChildren(this.DropDown, RibbonPopupManager.DismissReason.NewPopup);
                }
                return;
            }

            if (this.Style == RibbonButtonStyle.DropDown)
            {
                this.SetPressed(true);
            }
            else
            {
                this.DropDownPressed = true;
            }
            this.AssignHandlers();

            this.CreateDropDown();
            this.DropDown.MouseEnter += this.DropDown_MouseEnter;
            this.DropDown.Closed += this.DropDown_Closed;
            this.DropDown.ShowSizingGrip = this.DropDownResizable;
            this.DropDown.DrawIconsBar = this.DrawIconsBar;

            var canvasdd = this.Canvas as RibbonPopup;
            var location = this.OnGetDropDownMenuLocation();
            var minsize = this.OnGetDropDownMenuSize();

            if (!minsize.IsEmpty)
            {
                this.DropDown.MinimumSize = minsize;
            }

            this.OnDropDownShowing(EventArgs.Empty);
            this.SetDropDownVisible(true);
            this.DropDown.SelectionService = this.GetService(typeof(ISelectionService)) as ISelectionService;
            this.DropDown.Show(location);
        }

        private void DropDownItem_Click(object sender, EventArgs e)
        {
            this._selectedItem = sender as RibbonItem;

            var ev = new RibbonItemEventArgs(sender as RibbonItem);
            this.OnDropDownItemClicked(ref ev);
        }

        private void AssignHandlers()
        {
            foreach (var item in this.DropDownItems)
            {
                if (this._assignedHandlers.Contains(item) == false)
                {
                    item.Click += this.DropDownItem_Click;
                    this._assignedHandlers.Add(item);
                }
            }
        }

        private void RemoveHandlers()
        {
            // ADDED
            if (this.DropDown != null)
            {
                this.DropDown.MouseEnter -= this.DropDown_MouseEnter;
                this.DropDown.Closed -= this.DropDown_Closed;
            }

            foreach (var item in this._assignedHandlers)
            {
                item.Click -= this.DropDownItem_Click;
            }
            this._assignedHandlers.Clear();
        }

        private void DropDown_MouseEnter(object sender, EventArgs e)
        {
            this.SetSelected(true);
            this.RedrawItem();
        }

        /// <summary>
        /// Gets the location where the dropdown menu will be shown
        /// </summary>
        /// <returns></returns>
        internal virtual Point OnGetDropDownMenuLocation()
        {
            var location = Point.Empty;

            if (this.Canvas is RibbonDropDown)
            {
                location = this.Canvas.PointToScreen(new Point(this.Bounds.Right, this.Bounds.Top));
            }
            else
            {
                location = this.Canvas.PointToScreen(new Point(this.Bounds.Left, this.Bounds.Bottom));
            }

            return location;
        }

        /// <summary>
        /// Gets the size of the dropdown. If Size.Empty is returned, 
        /// size will be measured automatically
        /// </summary>
        /// <returns></returns>
        internal virtual Size OnGetDropDownMenuSize()
        {
            return Size.Empty;
        }

        /// <summary>
        /// Handles the closing of the dropdown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DropDown_Closed(object sender, EventArgs e)
        {
            this.SetPressed(false);

            this.DropDownPressed = false;

            this.SetDropDownVisible(false);

            this.SetSelected(false);
            this.DropDownSelected = false;

            this.RedrawItem();
        }

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
        /// Closes the DropDown if opened
        /// </summary>
        public void CloseDropDown()
        {
            if (this.DropDown != null)
            {
                RibbonPopupManager.Dismiss(this.DropDown, RibbonPopupManager.DismissReason.NewPopup);
            }

            this.SetDropDownVisible(false);
        }

        /// <summary>
        /// Overriden. Informs the button text
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{1}: {0}", this.Text, this.GetType().Name);
        }

        /// <summary>
        /// Sets the value of DropDownVisible
        /// </summary>
        /// <param name="visible"></param>
        internal void SetDropDownVisible(bool visible)
        {
            this.DropDownVisible = visible;
        }

        /// <summary>
        /// Raises the DropDownShowing event
        /// </summary>
        /// <param name="e"></param>
        public void OnDropDownShowing(EventArgs e)
        {
            if (this.DropDownShowing != null)
            {
                this.DropDownShowing(this, e);
            }
        }

        #endregion

        #region Overrides

        public override void OnCanvasChanged(EventArgs e)
        {
            base.OnCanvasChanged(e);

            if (this.Canvas is RibbonDropDown)
            {
                this.DropDownArrowDirection = RibbonArrowDirection.Left;
            }
        }

        protected override bool ClosesDropDownAt(Point p)
        {
            if (this.Style == RibbonButtonStyle.DropDown)
            {
                return false;
            }
            if (this.Style == RibbonButtonStyle.SplitDropDown)
            {
                return this.ButtonFaceBounds.Contains(p);
            }
            return true;
        }

        internal override void SetSizeMode(RibbonElementSizeMode sizeMode)
        {
            if (sizeMode == RibbonElementSizeMode.Overflow)
            {
                base.SetSizeMode(RibbonElementSizeMode.Large);
            }
            else
            {
                base.SetSizeMode(sizeMode);
            }
        }

        internal override void SetSelected(bool selected)
        {
            base.SetSelected(selected);

            this.SetPressed(false);
        }

        public override void OnMouseDown(MouseEventArgs e)
        {
            if (!this.Enabled)
            {
                return;
            }

            if ((this.DropDownSelected || this.Style == RibbonButtonStyle.DropDown) && this.DropDownItems.Count > 0)
            {
                if (this.DropDownVisible == false)
                {
                    this.DropDownPressed = true;
                    this.ShowDropDown();
                }
                else
                {
                    //Hack: Workaround to close DropDown if it stays open....
                    this.DropDownPressed = false;
                    this.CloseDropDown();
                }
            }

            base.OnMouseDown(e);
        }

        public override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
        }

        public override void OnMouseMove(MouseEventArgs e)
        {
            if (!this.Enabled)
            {
                return;
            }

            //Detect mouse on dropdwon
            if (this.Style == RibbonButtonStyle.SplitDropDown)
            {
                var lastState = this.DropDownSelected;

                if (this.DropDownBounds.Contains(e.X, e.Y))
                {
                    this.DropDownSelected = true;
                }
                else
                {
                    this.DropDownSelected = false;
                }

                if (lastState != this.DropDownSelected)
                {
                    this.RedrawItem();
                }

                lastState = this.DropDownSelected;
            }

            this._lastMousePos = new Point(e.X, e.Y);

            base.OnMouseMove(e);
        }

        public override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);

            this.DropDownSelected = false;
        }

        public override void OnClick(EventArgs e)
        {
            if (this.Style != RibbonButtonStyle.Normal && this.Style != RibbonButtonStyle.DropDownListItem && !this.ButtonFaceBounds.Contains(this._lastMousePos))
            {
                //Clicked the dropdown area, don't raise Click()
                return;
            }

            if (this.CheckOnClick)
            {
                this.Checked = !this.Checked;
            }

            base.OnClick(e);
        }

        #endregion
    }
}