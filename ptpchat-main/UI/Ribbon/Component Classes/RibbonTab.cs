namespace PtpChat.Main.Ribbon.Component_Classes
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    using IContainsRibbonComponents = PtpChat.Main.Ribbon.Classes.Interfaces.IContainsRibbonComponents;
    using IRibbonElement = PtpChat.Main.Ribbon.Classes.Interfaces.IRibbonElement;
    using IRibbonToolTip = PtpChat.Main.Ribbon.Classes.Interfaces.IRibbonToolTip;
    using RibbonDesigner = PtpChat.Main.Ribbon.Classes.Designers.RibbonDesigner;
    using RibbonElementMeasureSizeEventArgs = PtpChat.Main.Ribbon.Classes.EventArgs.RibbonElementMeasureSizeEventArgs;
    using RibbonElementPaintEventArgs = PtpChat.Main.Ribbon.Classes.EventArgs.RibbonElementPaintEventArgs;
    using RibbonElementPopupEventArgs = PtpChat.Main.Ribbon.Classes.EventArgs.RibbonElementPopupEventArgs;
    using RibbonElementPopupEventHandler = PtpChat.Main.Ribbon.Classes.EventArgs.RibbonElementPopupEventHandler;
    using RibbonElementSizeMode = PtpChat.Main.Ribbon.Classes.Enums.RibbonElementSizeMode;
    using RibbonPanelCollection = PtpChat.Main.Ribbon.Classes.Collections.RibbonPanelCollection;
    using RibbonPanelFlowDirection = PtpChat.Main.Ribbon.Classes.Enums.RibbonPanelFlowDirection;
    using RibbonTabDesigner = PtpChat.Main.Ribbon.Classes.Designers.RibbonTabDesigner;
    using RibbonTabRenderEventArgs = PtpChat.Main.Ribbon.Classes.EventArgs.RibbonTabRenderEventArgs;

    /// <summary>
    /// Represents a tab that can contain RibbonPanel objects
    /// </summary>
    [DesignTimeVisible(false)]
    [Designer(typeof(RibbonTabDesigner))]
    public class RibbonTab : Component, IRibbonElement, IRibbonToolTip, IContainsRibbonComponents
    {
        #region IContainsRibbonComponents Members

        public IEnumerable<Component> GetAllChildComponents()
        {
            return this.Panels.ToArray();
        }

        #endregion

        #region Fields

        private bool _pressed;

        private bool _selected;

        private bool _active;

        private string _text;

        private int _offset;

        private bool _visible = true;

        private readonly RibbonToolTip _TT;

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

        #endregion

        #region Ctor

        public RibbonTab()
        {
            this.Panels = new RibbonPanelCollection(this);

            //Initialize the ToolTip for this Item
            this._TT = new RibbonToolTip(this);
            this._TT.InitialDelay = 100;
            this._TT.AutomaticDelay = 800;
            this._TT.AutoPopDelay = 8000;
            this._TT.UseAnimation = true;
            this._TT.Active = false;
            this._TT.Popup += this._TT_Popup;
        }

        public RibbonTab(string text)
            : this()
        {
            this._text = text;
        }

        /// <summary>
        /// Creates a new RibbonTab
        /// </summary>
        [Obsolete("Use 'public RibbonTab(string text)' instead!")]
        public RibbonTab(Ribbon owner, string text)
            : this(text)
        {
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && RibbonDesigner.Current == null)
            {
                // ADDED
                this._TT.Popup -= this._TT_Popup;

                this._TT.Dispose();
                foreach (var p in this.Panels)
                {
                    p.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        #endregion

        #region Events

        public event EventHandler ScrollRightVisibleChanged;

        public event EventHandler ScrollRightPressedChanged;

        public event EventHandler ScrollRightBoundsChanged;

        public event EventHandler ScrollRightSelectedChanged;

        public event EventHandler ScrollLeftVisibleChanged;

        public event EventHandler ScrollLeftPressedChanged;

        public event EventHandler ScrollLeftSelectedChanged;

        public event EventHandler ScrollLeftBoundsChanged;

        public event EventHandler TabBoundsChanged;

        public event EventHandler TabContentBoundsChanged;

        public event EventHandler OwnerChanged;

        public event EventHandler PressedChanged;

        public event EventHandler ActiveChanged;

        public event EventHandler TextChanged;

        public event EventHandler ContextChanged;

        public virtual event RibbonElementPopupEventHandler ToolTipPopUp;

        #endregion

        #region Props

        /// <summary>
        /// Gets if the right-side scroll button is currently visible
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public bool ScrollRightVisible { get; private set; }

        /// <summary>
        /// Gets if the right-side scroll button is currently selected
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public bool ScrollRightSelected { get; private set; }

        /// <summary>
        /// Gets if the right-side scroll button is currently pressed
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public bool ScrollRightPressed { get; private set; }

        /// <summary>
        /// Gets if the right-side scroll button bounds
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Rectangle ScrollRightBounds { get; private set; }

        /// <summary>
        /// Gets if the left scroll button is currently visible
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public bool ScrollLeftVisible { get; private set; }

        /// <summary>
        /// Gets if the left scroll button bounds
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Rectangle ScrollLeftBounds { get; private set; }

        /// <summary>
        /// Gets if the left scroll button is currently selected
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public bool ScrollLeftSelected { get; private set; }

        /// <summary>
        /// Gets if the left scroll button is currently pressed
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public bool ScrollLeftPressed { get; private set; }

        /// <summary>
        /// Gets the <see cref="TabBounds"/> property value
        /// </summary>
        public Rectangle Bounds => this.TabBounds;

        /// <summary>
        /// Gets the collection of panels that belong to this tab
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public RibbonPanelCollection Panels { get; }

        /// <summary>
        /// Gets the bounds of the little tab showing the text
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Rectangle TabBounds { get; private set; }

        /// <summary>
        /// Gets the bounds of the tab content on the Ribbon
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Rectangle TabContentBounds { get; private set; }

        /// <summary>
        /// Gets the Ribbon that contains this tab
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Ribbon Owner { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the state of the tab is being pressed by the mouse or a key
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual bool Pressed => this._pressed;

        /// <summary>
        /// Gets a value indicating whether the tab is selected
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual bool Selected => this._selected;

        /// <summary>
        /// Gets a value indicating if the tab is currently the active tab
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual bool Active => this._active;

        /// <summary>
        /// Gets or sets the object that contains data about the control
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
        /// Gets or sets the text that is to be displayed on the tab
        /// </summary>
        [Localizable(true)]
        public string Text
        {
            get { return this._text; }
            set
            {
                this._text = value;

                this.OnTextChanged(EventArgs.Empty);

                if (this.Owner != null)
                {
                    this.Owner.OnRegionsChanged();
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the tab is attached to a  Context
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual bool Contextual => this.Context != null;

        /// <summary>
        /// Gets or sets the context this tab belongs to
        /// </summary>
        /// <remarks>Tabs on a context are highlighted with a special glow color</remarks>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RibbonContext Context { get; private set; }

        /// <summary>
        /// Gets or sets the visibility of this tab
        /// </summary>
        /// <remarks>Tabs on a context are highlighted with a special glow color</remarks>
        [Localizable(true), DefaultValue(true)]
        public bool Visible
        {
            get { return this._visible; }
            set
            {
                this._visible = value;
                this.Owner.UpdateRegions();
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

        #endregion

        #region IRibbonElement Members

        public void OnPaint(object sender, RibbonElementPaintEventArgs e)
        {
            if (this.Owner == null)
            {
                return;
            }

            this.Owner.Renderer.OnRenderRibbonTab(new RibbonTabRenderEventArgs(this.Owner, e.Graphics, e.Clip, this));
            this.Owner.Renderer.OnRenderRibbonTabText(new RibbonTabRenderEventArgs(this.Owner, e.Graphics, e.Clip, this));

            if (this.Active && (!this.Owner.Minimized || (this.Owner.Minimized && this.Owner.Expanded)))
            {
                foreach (var panel in this.Panels)
                {
                    if (panel.Visible)
                    {
                        panel.OnPaint(this, new RibbonElementPaintEventArgs(e.Clip, e.Graphics, panel.SizeMode, e.Control));
                    }
                }
            }

            this.Owner.Renderer.OnRenderTabScrollButtons(new RibbonTabRenderEventArgs(this.Owner, e.Graphics, e.Clip, this));
        }

        /// <summary>
        /// This method is not relevant for this class
        /// </summary>
        /// <exception cref="NotSupportedException">Always</exception>
        public void SetBounds(Rectangle bounds)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Sets the context of the tab
        /// </summary>
        /// <param name="context"></param>
        public void SetContext(RibbonContext context)
        {
            var trigger = !context.Equals(context);

            if (trigger)
            {
                this.OnContextChanged(EventArgs.Empty);
            }

            this.Context = context;

            throw new NotImplementedException();
        }

        /// <summary>
        /// Measures the size of the tab. The tab content bounds is measured by the Ribbon control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public Size MeasureSize(object sender, RibbonElementMeasureSizeEventArgs e)
        {
            if (!this.Visible && !this.Owner.IsDesignMode())
            {
                return new Size(0, 0);
            }

            var textSize = TextRenderer.MeasureText(this.Text, this.Owner.RibbonTabFont);
            return textSize;
        }

        /// <summary>
        /// Sets the value of the Owner Property
        /// </summary>
        internal void SetOwner(Ribbon owner)
        {
            this.Owner = owner;

            this.Panels.SetOwner(owner);

            this.OnOwnerChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Sets the value of the Pressed property
        /// </summary>
        /// <param name="pressed">Value that indicates if the element is pressed</param>
        internal void SetPressed(bool pressed)
        {
            this._pressed = pressed;

            this.OnPressedChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Sets the value of the Selected property
        /// </summary>
        /// <param name="selected">Value that indicates if the element is selected</param>
        internal void SetSelected(bool selected)
        {
            this._selected = selected;

            if (selected)
            {
                this.OnMouseEnter(new MouseEventArgs(MouseButtons.None, 0, 0, 0, 0));
            }
            else
            {
                this.OnMouseLeave(new MouseEventArgs(MouseButtons.None, 0, 0, 0, 0));
            }
        }

        #endregion

        #region Method Triggers

        public void OnContextChanged(EventArgs e)
        {
            if (this.ContextChanged != null)
            {
                this.ContextChanged(this, e);
            }
        }

        public void OnTextChanged(EventArgs e)
        {
            if (this.TextChanged != null)
            {
                this.TextChanged(this, e);
            }
        }

        public void OnActiveChanged(EventArgs e)
        {
            if (this.ActiveChanged != null)
            {
                this.ActiveChanged(this, e);
            }
        }

        public void OnPressedChanged(EventArgs e)
        {
            if (this.PressedChanged != null)
            {
                this.PressedChanged(this, e);
            }
        }

        public void OnOwnerChanged(EventArgs e)
        {
            if (this.OwnerChanged != null)
            {
                this.OwnerChanged(this, e);
            }
        }

        public void OnTabContentBoundsChanged(EventArgs e)
        {
            if (this.TabContentBoundsChanged != null)
            {
                this.TabContentBoundsChanged(this, e);
            }
        }

        public void OnTabBoundsChanged(EventArgs e)
        {
            if (this.TabBoundsChanged != null)
            {
                this.TabBoundsChanged(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="ScrollRightVisibleChanged"/> event
        /// </summary>
        /// <param name="e">Event data</param>
        public void OnScrollRightVisibleChanged(EventArgs e)
        {
            if (this.ScrollRightVisibleChanged != null)
            {
                this.ScrollRightVisibleChanged(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="ScrollRightPressedChanged"/> event
        /// </summary>
        /// <param name="e">Event data</param>
        public void OnScrollRightPressedChanged(EventArgs e)
        {
            if (this.ScrollRightPressedChanged != null)
            {
                this.ScrollRightPressedChanged(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="ScrollRightBoundsChanged"/> event
        /// </summary>
        /// <param name="e">Event data</param>
        public void OnScrollRightBoundsChanged(EventArgs e)
        {
            if (this.ScrollRightBoundsChanged != null)
            {
                this.ScrollRightBoundsChanged(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="ScrollRightSelectedChanged"/> event
        /// </summary>
        /// <param name="e">Event data</param>
        public void OnScrollRightSelectedChanged(EventArgs e)
        {
            if (this.ScrollRightSelectedChanged != null)
            {
                this.ScrollRightSelectedChanged(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="ScrollLeftVisibleChanged"/> event
        /// </summary>
        /// <param name="e">Event data</param>
        public void OnScrollLeftVisibleChanged(EventArgs e)
        {
            if (this.ScrollLeftVisibleChanged != null)
            {
                this.ScrollLeftVisibleChanged(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="ScrollLeftPressedChanged"/> event
        /// </summary>
        /// <param name="e">Event data</param>
        public void OnScrollLeftPressedChanged(EventArgs e)
        {
            if (this.ScrollLeftPressedChanged != null)
            {
                this.ScrollLeftPressedChanged(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="ScrollLeftBoundsChanged"/> event
        /// </summary>
        /// <param name="e">Event data</param>
        public void OnScrollLeftBoundsChanged(EventArgs e)
        {
            if (this.ScrollLeftBoundsChanged != null)
            {
                this.ScrollLeftBoundsChanged(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="ScrollLeftSelectedChanged"/> event
        /// </summary>
        /// <param name="e">Event data</param>
        public void OnScrollLeftSelectedChanged(EventArgs e)
        {
            if (this.ScrollLeftSelectedChanged != null)
            {
                this.ScrollLeftSelectedChanged(this, e);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sets the tab as active without sending the message to the Ribbon
        /// </summary>
        internal void SetActive(bool active)
        {
            var trigger = this._active != active;

            this._active = active;

            if (trigger)
            {
                this.OnActiveChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Sets the value of the TabBounds property
        /// </summary>
        /// <param name="tabBounds">Rectangle representing the bounds of the tab</param>
        internal void SetTabBounds(Rectangle tabBounds)
        {
            var tigger = this.TabBounds != tabBounds;

            this.TabBounds = tabBounds;

            this.OnTabBoundsChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Sets the value of the TabContentBounds
        /// </summary>
        /// <param name="tabContentBounds">Rectangle representing the bounds of the tab's content</param>
        internal void SetTabContentBounds(Rectangle tabContentBounds)
        {
            var trigger = this.TabContentBounds != tabContentBounds;

            this.TabContentBounds = tabContentBounds;

            this.OnTabContentBoundsChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Gets the panel with the larger width and the specified size mode
        /// </summary>
        /// <param name="size">Size mode of panel to search</param>
        /// <returns>Larger panel. Null if none of the specified size mode</returns>
        private RibbonPanel GetLargerPanel(RibbonElementSizeMode size)
        {
            RibbonPanel result = null;

            foreach (var panel in this.Panels)
            {
                if (panel.SizeMode != size)
                {
                    continue;
                }

                if (result == null)
                {
                    result = panel;
                }

                if (panel.Bounds.Width > result.Bounds.Width)
                {
                    result = panel;
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the panel with a larger size
        /// </summary>
        /// <returns></returns>
        private RibbonPanel GetLargerPanel()
        {
            var largeLarger = this.GetLargerPanel(RibbonElementSizeMode.Large);

            if (largeLarger != null)
            {
                return largeLarger;
            }

            var mediumLarger = this.GetLargerPanel(RibbonElementSizeMode.Medium);

            if (mediumLarger != null)
            {
                return mediumLarger;
            }

            var compactLarger = this.GetLargerPanel(RibbonElementSizeMode.Compact);

            if (compactLarger != null)
            {
                return compactLarger;
            }

            var overflowLarger = this.GetLargerPanel(RibbonElementSizeMode.Overflow);

            if (overflowLarger != null)
            {
                return overflowLarger;
            }

            return null;
        }

        private bool AllPanelsOverflow()
        {
            foreach (var panel in this.Panels)
            {
                if (panel.SizeMode != RibbonElementSizeMode.Overflow)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Updates the regions of the panels and its contents
        /// </summary>
        internal void UpdatePanelsRegions()
        {
            if (this.Panels.Count == 0)
            {
                return;
            }

            if (!this.Owner.IsDesignMode())
            {
                this._offset = 0;
            }

            var curRight = this.TabContentBounds.Left + this.Owner.PanelPadding.Left + this._offset;
            var curLeft = this.TabContentBounds.Right - this.Owner.PanelPadding.Right;
            var panelsTop = this.TabContentBounds.Top + this.Owner.PanelPadding.Top;

            using (var g = this.Owner.CreateGraphics())
            {
                //Check all at full size
                foreach (var panel in this.Panels)
                {
                    if (panel.Visible && this.Owner.RightToLeft == RightToLeft.No)
                    {
                        var sMode = panel.FlowsTo == RibbonPanelFlowDirection.Right ? RibbonElementSizeMode.Medium : RibbonElementSizeMode.Large;
                        //Set the bounds of the panel to let it know it's height
                        panel.SetBounds(new Rectangle(0, 0, 1, this.TabContentBounds.Height - this.Owner.PanelPadding.Vertical));

                        ///Size of the panel
                        var size = panel.MeasureSize(this, new RibbonElementMeasureSizeEventArgs(g, sMode));

                        ///Creates the bounds of the panel
                        var bounds = new Rectangle(curRight, panelsTop, size.Width, size.Height);

                        ///Set the bounds of the panel
                        panel.SetBounds(bounds);

                        ///Let the panel know what size we have decided for it
                        panel.SetSizeMode(sMode);

                        ///Update curLeft
                        curRight = bounds.Right + 1 + this.Owner.PanelSpacing;
                    }
                    else if (panel.Visible && this.Owner.RightToLeft == RightToLeft.Yes)
                    {
                        var sMode = panel.FlowsTo == RibbonPanelFlowDirection.Right ? RibbonElementSizeMode.Medium : RibbonElementSizeMode.Large;

                        //Set the bounds of the panel to let it know it's height
                        panel.SetBounds(new Rectangle(0, 0, 1, this.TabContentBounds.Height - this.Owner.PanelPadding.Vertical));

                        ///Size of the panel
                        var size = panel.MeasureSize(this, new RibbonElementMeasureSizeEventArgs(g, sMode));

                        curLeft -= size.Width + this.Owner.PanelSpacing;

                        ///Creates the bounds of the panel
                        var bounds = new Rectangle(curLeft, panelsTop, size.Width, size.Height);

                        ///Set the bounds of the panel
                        panel.SetBounds(bounds);

                        ///Let the panel know what size we have decided for it
                        panel.SetSizeMode(sMode);

                        ///Update curLeft
                        curLeft = bounds.Left - 1 - this.Owner.PanelSpacing;
                    }
                    else
                    {
                        panel.SetBounds(Rectangle.Empty);
                    }
                }

                if (!this.Owner.IsDesignMode())
                {
                    while (curRight > this.TabContentBounds.Right && !this.AllPanelsOverflow())
                    {
                        #region Down grade the larger panel one position

                        var larger = this.GetLargerPanel();

                        if (larger.SizeMode == RibbonElementSizeMode.Large)
                        {
                            larger.SetSizeMode(RibbonElementSizeMode.Medium);
                        }
                        else if (larger.SizeMode == RibbonElementSizeMode.Medium)
                        {
                            larger.SetSizeMode(RibbonElementSizeMode.Compact);
                        }
                        else if (larger.SizeMode == RibbonElementSizeMode.Compact)
                        {
                            larger.SetSizeMode(RibbonElementSizeMode.Overflow);
                        }

                        var size = larger.MeasureSize(this, new RibbonElementMeasureSizeEventArgs(g, larger.SizeMode));

                        larger.SetBounds(new Rectangle(larger.Bounds.Location, new Size(size.Width + this.Owner.PanelMargin.Horizontal, size.Height)));

                        #endregion

                        ///Reset x-axis reminder
                        curRight = this.TabContentBounds.Left + this.Owner.PanelPadding.Left;

                        ///Re-arrange location because of the new bounds
                        foreach (var panel in this.Panels)
                        {
                            var s = panel.Bounds.Size;
                            panel.SetBounds(new Rectangle(new Point(curRight, panelsTop), s));
                            curRight += panel.Bounds.Width + 1 + this.Owner.PanelSpacing;
                        }
                    }
                }

                ///Update regions of all panels
                foreach (var panel in this.Panels)
                {
                    panel.UpdateItemsRegions(g, panel.SizeMode);
                }
            }

            this.UpdateScrollBounds();
        }

        /// <summary>
        /// Updates the bounds of the scroll bounds
        /// </summary>
        private void UpdateScrollBounds()
        {
            var w = 13;
            var scrBuffer = this.ScrollRightVisible;
            var sclBuffer = this.ScrollLeftVisible;
            var rrBuffer = this.ScrollRightBounds;
            var rlBuffer = this.ScrollLeftBounds;

            if (this.Panels.Count == 0)
            {
                return;
            }

            if (this.Panels[this.Panels.Count - 1].Bounds.Right > this.TabContentBounds.Right)
            {
                this.ScrollRightVisible = true;
            }
            else
            {
                this.ScrollRightVisible = false;
            }

            if (this.ScrollRightVisible != scrBuffer)
            {
                this.OnScrollRightVisibleChanged(EventArgs.Empty);
            }

            if (this._offset < 0)
            {
                this.ScrollLeftVisible = true;
            }
            else
            {
                this.ScrollLeftVisible = false;
            }

            if (this.ScrollRightVisible != scrBuffer)
            {
                this.OnScrollLeftVisibleChanged(EventArgs.Empty);
            }

            if (this.ScrollLeftVisible || this.ScrollRightVisible)
            {
                this.ScrollRightBounds = Rectangle.FromLTRB(this.Owner.ClientRectangle.Right - w, this.TabContentBounds.Top, this.Owner.ClientRectangle.Right, this.TabContentBounds.Bottom);

                this.ScrollLeftBounds = Rectangle.FromLTRB(0, this.TabContentBounds.Top, w, this.TabContentBounds.Bottom);

                if (this.ScrollRightBounds != rrBuffer)
                {
                    this.OnScrollRightBoundsChanged(EventArgs.Empty);
                }

                if (this.ScrollLeftBounds != rlBuffer)
                {
                    this.OnScrollLeftBoundsChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Overriden. Returns a string representation of the tab
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("Tab: {0}", this.Text);
        }

        /// <summary>
        /// Raises the MouseEnter event
        /// </summary>
        /// <param name="e">Event data</param>
        public virtual void OnMouseEnter(MouseEventArgs e)
        {
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
            this._TT.Active = false;

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
            if (this.MouseMove != null)
            {
                this.MouseMove(this, e);
            }
            if (!this._TT.Active && !string.IsNullOrEmpty(this.ToolTip)) // ToolTip should be working without title as well - to get Office 2007 Look & Feel
            {
                if (this.ToolTip != this._TT.GetToolTip(this.Owner))
                {
                    this._TT.SetToolTip(this.Owner, this.ToolTip);
                }
                this._TT.Active = true;
            }
        }

        /// <summary>
        /// Sets the value of the <see cref="ScrollLeftPressed"/>
        /// </summary>
        /// <param name="pressed"></param>
        internal void SetScrollLeftPressed(bool pressed)
        {
            this.ScrollLeftPressed = pressed;

            if (pressed)
            {
                this.ScrollLeft();
            }

            this.OnScrollLeftPressedChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Sets the value of the <see cref="ScrollLeftSelected"/>
        /// </summary>
        /// <param name="selected"></param>
        internal void SetScrollLeftSelected(bool selected)
        {
            this.ScrollLeftSelected = selected;

            this.OnScrollLeftSelectedChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Sets the value of the <see cref="ScrollRightPressed"/>
        /// </summary>
        /// <param name="pressed"></param>
        internal void SetScrollRightPressed(bool pressed)
        {
            this.ScrollRightPressed = pressed;

            if (pressed)
            {
                this.ScrollRight();
            }

            this.OnScrollRightPressedChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Sets the value of the <see cref="ScrollRightSelected"/>
        /// </summary>
        /// <param name="selected"></param>
        internal void SetScrollRightSelected(bool selected)
        {
            this.ScrollRightSelected = selected;

            this.OnScrollRightSelectedChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Presses the lef-scroll button
        /// </summary>
        public void ScrollLeft()
        {
            this.ScrollOffset(50);
        }

        /// <summary>
        /// Presses the left-scroll button
        /// </summary>
        public void ScrollRight()
        {
            this.ScrollOffset(-50);
        }

        public void ScrollOffset(int amount)
        {
            this._offset += amount;

            foreach (var p in this.Panels)
            {
                p.SetBounds(new Rectangle(p.Bounds.Left + amount, p.Bounds.Top, p.Bounds.Width, p.Bounds.Height));
            }

            if (this.Site != null && this.Site.DesignMode)
            {
                this.UpdatePanelsRegions();
            }

            this.UpdateScrollBounds();

            this.Owner.Invalidate();
        }

        private void _TT_Popup(object sender, PopupEventArgs e)
        {
            if (this.ToolTipPopUp != null)
            {
                this.ToolTipPopUp(sender, new RibbonElementPopupEventArgs(this, e));
                if (this.ToolTip != this._TT.GetToolTip(this.Owner))
                {
                    this._TT.SetToolTip(this.Owner, this.ToolTip);
                }
            }
        }

        #endregion
    }
}