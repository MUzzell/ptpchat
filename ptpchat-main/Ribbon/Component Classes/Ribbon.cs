namespace PtpChat.Main.Ribbon.Component_Classes
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Text;
    using System.Windows.Forms;

    using PtpChat.Main.Ribbon.Classes;

    using IRibbonForm = PtpChat.Main.Ribbon.Classes.Interfaces.IRibbonForm;
    using RibbonContextCollection = PtpChat.Main.Ribbon.Classes.Collections.RibbonContextCollection;
    using RibbonDesigner = PtpChat.Main.Ribbon.Classes.Designers.RibbonDesigner;
    using RibbonElementMeasureSizeEventArgs = PtpChat.Main.Ribbon.Classes.EventArgs.RibbonElementMeasureSizeEventArgs;
    using RibbonElementPaintEventArgs = PtpChat.Main.Ribbon.Classes.EventArgs.RibbonElementPaintEventArgs;
    using RibbonElementSizeMode = PtpChat.Main.Ribbon.Classes.Enums.RibbonElementSizeMode;
    using RibbonFormHelper = PtpChat.Main.Ribbon.Classes.RibbonFormHelper;
    using RibbonMouseSensor = PtpChat.Main.Ribbon.Classes.RibbonMouseSensor;
    using RibbonOrbStyle = PtpChat.Main.Ribbon.Classes.Enums.RibbonOrbStyle;
    using RibbonPopupManager = PtpChat.Main.Ribbon.Classes.RibbonPopupManager;
    using RibbonProfessionalRenderer = PtpChat.Main.Ribbon.Classes.Renderers.RibbonProfessionalRenderer;
    using RibbonRenderer = PtpChat.Main.Ribbon.Classes.Renderers.RibbonRenderer;
    using RibbonRenderEventArgs = PtpChat.Main.Ribbon.Classes.EventArgs.RibbonRenderEventArgs;
    using RibbonTabCollection = PtpChat.Main.Ribbon.Classes.Collections.RibbonTabCollection;
    using RibbonTheme = PtpChat.Main.Ribbon.Classes.Enums.RibbonTheme;
    using RibbonWindowMode = PtpChat.Main.Ribbon.Classes.Enums.RibbonWindowMode;
    using Theme = PtpChat.Main.Ribbon.Classes.Theme;

    /// <summary>
    /// Provides a Ribbon toolbar
    /// </summary>
    [Designer(typeof(RibbonDesigner))]
    public class Ribbon : Control
    {
        #region Static

        public static int CaptionBarHeight = 24;

        #endregion

        private delegate void HandlerCallbackMethode();

        #region Const

        private const int DefaultTabSpacing = 6;

        private const int DefaultPanelSpacing = 3;

        #endregion

        #region Fields

        internal bool ForceOrbMenu;

        private Size _lastSizeMeasured;

        private Padding _tabsMargin;

        internal bool _minimized = true; //is the ribbon minimized?

        internal bool _expanded; //is the ribbon currently expanded when in minimize mode

        internal bool _expanding; //is the ribbon expanding. Flag used to prevent calling methods multiple time while the size changes

        //private int _minimizedHeight;//height when minimized
        private int _expandedHeight; //height when expanded

        private RibbonRenderer _renderer;

        private Padding _panelMargin;

        private RibbonTab _activeTab;

        private RibbonTab _lastSelectedTab;

        //private float _tabSum;
        private bool _updatingSuspended;

        private bool _orbSelected;

        private bool _orbPressed;

        private bool _orbVisible;

        private Image _orbImage;

        private string _orbText;

        //private bool _quickAcessVisible;

        private RibbonWindowMode _borderMode;

        private GlobalHook _mouseHook;

        private GlobalHook _keyboardHook;

        private Font _RibbonItemFont = new Font("Trebuchet MS", 9);

        internal RibbonItem ActiveTextBox; //tracks the current active textbox so we can hide it when you click off it

        #endregion

        #region Events

        /// <summary>
        /// Occours when the Orb is clicked
        /// </summary>
        public event EventHandler OrbClicked;

        /// <summary>
        /// Occours when the Orb is double-clicked
        /// </summary>
        public event EventHandler OrbDoubleClick;

        /// <summary>
        /// Occours when the <see cref="ActiveTab"/> property value has changed
        /// </summary>
        public event EventHandler ActiveTabChanged;

        /// <summary>
        /// Occours when the <see cref="ActualBorderMode"/> property has changed
        /// </summary>
        public event EventHandler ActualBorderModeChanged;

        /// <summary>
        /// Occours when the <see cref="CaptionButtonsVisible"/> property value has changed
        /// </summary>
        public event EventHandler CaptionButtonsVisibleChanged;

        ///// <summary>
        ///// Occours when the Ribbon changes its miminized state
        ///// </summary>
        public event EventHandler ExpandedChanged;

        #endregion

        #region Ctor

        /// <summary>
        /// Creates a new Ribbon control
        /// </summary>
        public Ribbon()
        {
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.Selectable, false);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            this.Dock = DockStyle.Top;

            this.Tabs = new RibbonTabCollection(this);
            this.Contexts = new RibbonContextCollection(this);

            this.OrbsPadding = new Padding(8, 5, 8, 3);
            this.TabsPadding = new Padding(8, 5, 8, 3);
            this._tabsMargin = new Padding(12, 24 + 2, 20, 0);
            this.TabTextMargin = new Padding(4, 2, 4, 2);

            this.TabContentMargin = new Padding(1, 0, 1, 2);
            this.PanelPadding = new Padding(3);
            this._panelMargin = new Padding(3, 2, 3, 15);
            this.PanelSpacing = DefaultPanelSpacing;
            this.ItemPadding = new Padding(1, 0, 1, 0);
            this.ItemMargin = new Padding(4, 2, 4, 2);
            this.TabSpacing = DefaultTabSpacing;
            this.DropDownMargin = new Padding(2);
            this._renderer = new RibbonProfessionalRenderer();
            this._orbVisible = true;
            this.OrbDropDown = new RibbonOrbDropDown(this);
            this.QuickAcessToolbar = new RibbonQuickAccessToolbar(this);
            //_quickAcessVisible = true;
            this.MinimizeButton = new RibbonCaptionButton(RibbonCaptionButton.CaptionButton.Minimize);
            this.MaximizeRestoreButton = new RibbonCaptionButton(RibbonCaptionButton.CaptionButton.Maximize);
            this.CloseButton = new RibbonCaptionButton(RibbonCaptionButton.CaptionButton.Close);

            this.MinimizeButton.SetOwner(this);
            this.MaximizeRestoreButton.SetOwner(this);
            this.CloseButton.SetOwner(this);
            this._CaptionBarVisible = true;

            this.Font = SystemFonts.CaptionFont;

            this.BorderMode = RibbonWindowMode.NonClientAreaGlass;

            this._minimized = false;
            this._expanded = true;

            RibbonPopupManager.PopupRegistered += this.OnPopupRegistered;
            RibbonPopupManager.PopupUnRegistered += this.OnPopupUnregistered;
            //Theme.MainRibbon = this;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && RibbonDesigner.Current == null)
            {
                foreach (var tab in this.Tabs)
                {
                    tab.Dispose();
                }
                this.OrbDropDown.Dispose();
                this.QuickAcessToolbar.Dispose();
                this.MinimizeButton.Dispose();
                this.MaximizeRestoreButton.Dispose();
                this.CloseButton.Dispose();

                RibbonPopupManager.PopupRegistered -= this.OnPopupRegistered;
                RibbonPopupManager.PopupUnRegistered -= this.OnPopupUnregistered;

                GC.SuppressFinalize(this);
            }

            this.DisposeHooks();

            base.Dispose(disposing);
        }

        ~Ribbon()
        {
            this.Dispose(false);
        }

        private void DisposeHooks()
        {
            if (this._mouseHook != null)
            {
                this._mouseHook.MouseWheel -= this._mouseHook_MouseWheel;
                this._mouseHook.MouseDown -= this._mouseHook_MouseDown;
                this._mouseHook.Dispose();
                this._mouseHook = null;
            }
            if (this._keyboardHook != null)
            {
                this._keyboardHook.KeyDown -= this._keyboardHook_KeyDown;
                this._keyboardHook.Dispose();
                this._keyboardHook = null;
            }
        }

        #endregion

        #region Props

        /// <summary>
        /// Gets or sets the tabs expanded state when in minimize mode
        /// </summary>
        [DefaultValue(true)]
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Expanded
        {
            get { return this._expanded; }
            set
            {
                this._expanded = value;
                if (!this.IsDesignMode() && this.Minimized)
                {
                    this._expanding = true;
                    if (this._expanded)
                    {
                        this.Height = this._expandedHeight;
                    }
                    else
                    {
                        this.Height = this.MinimizedHeight;
                    }

                    this.OnExpandedChanged(EventArgs.Empty);
                    if (this._expanded)
                    {
                        this.SetUpHooks();
                    }
                    else if (!this._expanded && RibbonPopupManager.PopupCount == 0)
                    {
                        this.DisposeHooks();
                    }
                    //UpdateRegions();
                    this.Invalidate();
                    this._expanding = false;
                }
            }
        }

        /// <summary>
        /// Gets the height of the ribbon when collapsed <see cref="MinimizedHeight"/>
        /// </summary>
        //[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Gets the height of the ribbon when collapsed")]
        public int MinimizedHeight
        {
            get
            {
                var tabBottom = this.Tabs.Count > 0 ? this.Tabs[0].Bounds.Bottom : 0;
                return Math.Max(this.OrbBounds.Bottom, tabBottom) + 1;
            }
        }

        //[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Size Size
        {
            get { return base.Size; }
            set
            {
                base.Size = value;
                this.Height = value.Height;
                if (!this.Minimized || (!this._expanding && this.Expanded))
                {
                    this._expandedHeight = this.Height;
                }
            }
        }

        internal Rectangle CaptionTextBounds
        {
            get
            {
                if (this.RightToLeft == RightToLeft.No)
                {
                    var left = 0;
                    if (this.OrbVisible)
                    {
                        left = this.OrbBounds.Right;
                    }
                    if (this.QuickAcessToolbar.Visible)
                    {
                        left = this.QuickAcessToolbar.Bounds.Right + 20;
                    }
                    if (this.QuickAcessToolbar.Visible && this.QuickAcessToolbar.DropDownButtonVisible)
                    {
                        left = this.QuickAcessToolbar.DropDownButton.Bounds.Right;
                    }
                    var r = Rectangle.FromLTRB(left, 0, this.Width - 100, this.CaptionBarSize);
                    return r;
                }
                else
                {
                    var right = this.ClientRectangle.Right;
                    if (this.OrbVisible)
                    {
                        right = this.OrbBounds.Left;
                    }
                    if (this.QuickAcessToolbar.Visible)
                    {
                        right = this.QuickAcessToolbar.Bounds.Left - 20;
                    }
                    if (this.QuickAcessToolbar.Visible && this.QuickAcessToolbar.DropDownButtonVisible)
                    {
                        right = this.QuickAcessToolbar.DropDownButton.Bounds.Left;
                    }
                    var r = Rectangle.FromLTRB(100, 0, right, this.CaptionBarSize);
                    return r;
                }
            }
        }

        /// <summary>
        /// Gets if the caption buttons are currently visible, according to the value specified in <see cref="BorderMode"/>
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool CaptionButtonsVisible { get; private set; }

        /// <summary>
        /// Gets the Ribbon's close button
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RibbonCaptionButton CloseButton { get; }

        /// <summary>
        /// Gets the Ribbon's maximize-restore button
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RibbonCaptionButton MaximizeRestoreButton { get; }

        /// <summary>
        /// Gets the Ribbon's minimize button
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RibbonCaptionButton MinimizeButton { get; }

        /// <summary>
        /// Gets or sets the RibbonFormHelper object if the parent form is IRibbonForm
        /// </summary>
        [Browsable(false)]
        public RibbonFormHelper FormHelper
        {
            get
            {
                var irf = this.Parent as IRibbonForm;

                if (irf != null)
                {
                    return irf.Helper;
                }

                return null;
            }
        }

        /// <summary>
        /// Gets the actual <see cref="RibbonWindowMode"/> that the ribbon has. 
        /// It's value may vary from <see cref="BorderMode"/>
        /// because of computer and operative system capabilities.
        /// </summary>
        [Browsable(false)]
        public RibbonWindowMode ActualBorderMode { get; private set; }

        /// <summary>
        /// Gets or sets the border mode of the ribbon relative to the window where it is contained
        /// </summary>
        [DefaultValue(RibbonWindowMode.NonClientAreaGlass)]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Description("Specifies how the Ribbon is placed on the window border and the non-client area")]
        public RibbonWindowMode BorderMode
        {
            get { return this._borderMode; }
            set
            {
                this._borderMode = value;

                var actual = value;

                if (value == RibbonWindowMode.NonClientAreaGlass && !WinApi.IsGlassEnabled)
                {
                    actual = RibbonWindowMode.NonClientAreaCustomDrawn;
                }

                if (this.FormHelper == null || (value == RibbonWindowMode.NonClientAreaCustomDrawn && Environment.OSVersion.Platform != PlatformID.Win32NT))
                {
                    actual = RibbonWindowMode.InsideWindow;
                }

                this.SetActualBorderMode(actual);
            }
        }

        /// <summary>
        /// Gets the Orb's DropDown
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Browsable(true)]
        public RibbonOrbDropDown OrbDropDown { get; }

        /// <summary>
        /// Gets or sets the height of the Panel Caption area.
        /// </summary>
        [DefaultValue(15)]
        [Description("Gets or sets the height of the Panel Caption area")]
        public int PanelCaptionHeight
        {
            get { return this._panelMargin.Bottom; }
            set
            {
                this._panelMargin.Bottom = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Gets  the QuickAcessToolbar
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public RibbonQuickAccessToolbar QuickAcessToolbar { get; }

        /// <summary>
        /// Gets or sets the Style of the orb
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [DefaultValue(RibbonOrbStyle.Office_2007)]
        public RibbonOrbStyle OrbStyle
        {
            get { return Theme.ThemeStyle; }
            set
            {
                Theme.blnSetOnly = true;
                Theme.ThemeStyle = value;
                Theme.blnSetOnly = false;

                if (value == RibbonOrbStyle.Office_2007)
                {
                    this.TabsPadding = new Padding(8, 4, 8, 4);
                }
                else if (value == RibbonOrbStyle.Office_2010)
                {
                    this.TabsPadding = new Padding(8, 4, 8, 4);
                }
                else if (value == RibbonOrbStyle.Office_2013)
                {
                    this.TabsPadding = new Padding(8, 3, 8, 3);
                }

                if (value == RibbonOrbStyle.Office_2007)
                {
                    this.OrbsPadding = new Padding(8, 4, 8, 4);
                }
                else if (value == RibbonOrbStyle.Office_2010)
                {
                    this.OrbsPadding = new Padding(8, 4, 8, 4);
                }
                else if (value == RibbonOrbStyle.Office_2013)
                {
                    this.OrbsPadding = new Padding(15, 3, 15, 3);
                }

                this.UpdateRegions();
                this.Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the theme of the ribbon control
        /// </summary>
        //Michael Spradlin 07/05/2013
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [DefaultValue(RibbonTheme.Normal)]
        public RibbonTheme ThemeColor
        {
            get { return Theme.ThemeColor; }
            set
            {
                Theme.blnSetOnly = true;
                Theme.ThemeColor = value;
                Theme.blnSetOnly = false;

                this.OnRegionsChanged();
                this.Invalidate();

                //if (Theme.ThemeColor == RibbonTheme.Blue | Theme.ThemeColor == RibbonTheme.Normal)
                //    (_renderer as RibbonProfessionalRenderer).ColorTable = new RibbonProfesionalRendererColorTable();
                //else if (Theme.ThemeColor == RibbonTheme.Black)
                //    (_renderer as RibbonProfessionalRenderer).ColorTable = new RibbonProfesionalRendererColorTableBlack();
                //else if (Theme.ThemeColor == RibbonTheme.Green)
                //    (_renderer as RibbonProfessionalRenderer).ColorTable = new RibbonProfesionalRendererColorTableGreen();
                //else if (Theme.ThemeColor == RibbonTheme.Purple)
                //    (_renderer as RibbonProfessionalRenderer).ColorTable = new RibbonProfesionalRendererColorTablePurple();
                //else if (Theme.ThemeColor == RibbonTheme.JellyBelly)
                //    (_renderer as RibbonProfessionalRenderer).ColorTable = new RibbonProfesionalRendererColorTableJellyBelly();
                //else if (Theme.ThemeColor == RibbonTheme.Halloween)
                //    (_renderer as RibbonProfessionalRenderer).ColorTable = new RibbonProfesionalRendererColorTableHalloween();
            }
        }

        /// <summary>
        /// Gets or sets the Text in the orb. Only available when the OrbStyle is set to Office2010
        /// </summary>
        [DefaultValue(null)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string OrbText
        {
            get { return this._orbText; }
            set
            {
                this._orbText = value;
                this.OnRegionsChanged();
                this.Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the Image of the orb
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Image OrbImage
        {
            get { return this._orbImage; }
            set
            {
                this._orbImage = value;
                this.OnRegionsChanged();
                this.Invalidate(this.OrbBounds);
            }
        }

        /// <summary>
        /// Gets or sets if the Ribbon should show an orb on the corner
        /// </summary>
        [DefaultValue(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool OrbVisible
        {
            get { return this._orbVisible; }
            set
            {
                this._orbVisible = value;
                this.OnRegionsChanged();
            }
        }

        /// <summary>
        /// Gets or sets if the Orb is currently selected
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool OrbSelected
        {
            get { return this._orbSelected; }
            set
            {
                this._orbSelected = value;
                this.Invalidate(this.OrbBounds);
            }
        }

        /// <summary>
        /// Gets or sets if the Orb is currently pressed
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool OrbPressed
        {
            get { return this._orbPressed; }
            set
            {
                this._orbPressed = value;
                this.Invalidate(this.OrbBounds);
            }
        }

        /// <summary>
        /// Gets the Height of the caption bar
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int CaptionBarSize => CaptionBarHeight;

        /// <summary>
        /// Gets the bounds of the orb
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Rectangle OrbBounds
        {
            get
            {
                if (this.OrbStyle == RibbonOrbStyle.Office_2007)
                {
                    if (this.OrbVisible && this.RightToLeft == RightToLeft.No && this.CaptionBarVisible)
                    {
                        return new Rectangle(4, 4, 36, 36);
                    }
                    if (this.OrbVisible && this.RightToLeft == RightToLeft.Yes && this.CaptionBarVisible)
                    {
                        return new Rectangle(this.Width - 36 - 4, 4, 36, 36);
                    }
                    if (this.RightToLeft == RightToLeft.No)
                    {
                        return new Rectangle(4, 4, 0, 0);
                    }
                    return new Rectangle(this.Width - 4, 4, 0, 0);
                }
                if (this.OrbStyle == RibbonOrbStyle.Office_2010) //Kevin Carbis - office 2010 style orb
                {
                    //Measure the string size of the button text so we know how big to make the button
                    var contentSize = TextRenderer.MeasureText(this.OrbText, this.RibbonTabFont);
                    //If we are using an image adjust the size
                    if (this.OrbImage != null)
                    {
                        contentSize.Width = Math.Max(contentSize.Width, this.OrbImage.Size.Width);
                        contentSize.Height = Math.Max(contentSize.Height, this.OrbImage.Size.Height);
                    }

                    if (this.OrbVisible && this.RightToLeft == RightToLeft.No)
                    {
                        return new Rectangle(
                            4,
                            this.TabsMargin.Top,
                            contentSize.Width + this.OrbsPadding.Left + this.OrbsPadding.Right,
                            this.OrbsPadding.Top + contentSize.Height + this.OrbsPadding.Bottom);
                    }
                    if (this.OrbVisible && this.RightToLeft == RightToLeft.Yes && this.CaptionBarVisible)
                    {
                        return new Rectangle(
                            this.Width - contentSize.Width - this.OrbsPadding.Left - this.OrbsPadding.Right - 4,
                            this.TabsMargin.Top,
                            contentSize.Width + this.OrbsPadding.Left + this.OrbsPadding.Right,
                            this.OrbsPadding.Top + contentSize.Height + this.OrbsPadding.Bottom);
                    }
                    if (this.RightToLeft == RightToLeft.No)
                    {
                        return new Rectangle(4, 4, 0, 0);
                    }
                    return new Rectangle(this.Width - 4, 4, 0, 0);
                }
                else //Michael Spradlin - 05/03/2013 Office 2013 Style Changes
                {
                    //Measure the string size of the button text so we know how big to make the button
                    var contentSize = TextRenderer.MeasureText(this.OrbText, this.RibbonTabFont);
                    //If we are using an image adjust the size
                    if (this.OrbImage != null)
                    {
                        contentSize.Width = Math.Max(contentSize.Width, this.OrbImage.Size.Width);
                        contentSize.Height = Math.Max(contentSize.Height, this.OrbImage.Size.Height);
                    }

                    if (this.OrbVisible && this.RightToLeft == RightToLeft.No)
                    {
                        return new Rectangle(
                            0,
                            this.TabsMargin.Top,
                            contentSize.Width + this.OrbsPadding.Left + this.OrbsPadding.Right,
                            this.OrbsPadding.Top + contentSize.Height + this.OrbsPadding.Bottom);
                    }
                    if (this.OrbVisible && this.RightToLeft == RightToLeft.Yes && this.CaptionBarVisible)
                    {
                        return new Rectangle(
                            this.Width - contentSize.Width - this.OrbsPadding.Left - this.OrbsPadding.Right - 4,
                            this.TabsMargin.Top,
                            contentSize.Width + this.OrbsPadding.Left + this.OrbsPadding.Right,
                            this.OrbsPadding.Top + contentSize.Height + this.OrbsPadding.Bottom);
                    }
                    if (this.RightToLeft == RightToLeft.No)
                    {
                        return new Rectangle(4, 4, 0, 0);
                    }
                    return new Rectangle(this.Width - 4, 4, 0, 0);
                }
            }
        }

        /// <summary>
        /// Gets the next tab to be activated
        /// </summary>
        /// <returns></returns>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RibbonTab NextTab
        {
            get
            {
                if (this.ActiveTab == null || this.Tabs.Count == 0)
                {
                    if (this.Tabs.Count == 0)
                    {
                        return null;
                    }

                    return this.Tabs[0];
                }

                var index = this.Tabs.IndexOf(this.ActiveTab);

                if (index == this.Tabs.Count - 1)
                {
                    return this.ActiveTab;
                }
                return this.Tabs[index + 1];
            }
        }

        /// <summary>
        /// Gets the next tab to be activated
        /// </summary>
        /// <returns></returns>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RibbonTab PreviousTab
        {
            get
            {
                if (this.ActiveTab == null || this.Tabs.Count == 0)
                {
                    if (this.Tabs.Count == 0)
                    {
                        return null;
                    }

                    return this.Tabs[0];
                }

                var index = this.Tabs.IndexOf(this.ActiveTab);

                if (index == 0)
                {
                    return this.ActiveTab;
                }
                return this.Tabs[index - 1];
            }
        }

        /// <summary>
        /// Gets or sets the internal spacing between the tab and its text
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Padding TabTextMargin { get; set; }

        /// <summary> 
        /// Gets or sets the margis of the DropDowns shown by the Ribbon
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Padding DropDownMargin { get; set; }

        /// <summary>
        /// Gets or sets the external spacing of items on panels
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Padding ItemPadding { get; set; }

        /// <summary>
        /// Gets or sets the internal spacing of items
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Padding ItemMargin { get; set; }

        /// <summary>
        /// Gets or sets the tab that is currently active
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RibbonTab ActiveTab
        {
            get { return this._activeTab; }
            set
            {
                var NewTab = this._activeTab;
                foreach (var tab in this.Tabs)
                {
                    if (tab != value)
                    {
                        tab.SetActive(false);
                    }
                    else
                    {
                        NewTab = tab;
                    }
                }
                NewTab.SetActive(true);

                this._activeTab = value;

                this.RemoveHelperControls();

                value.UpdatePanelsRegions();

                this.Invalidate();

                this.RenewSensor();

                this.OnActiveTabChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the spacing leaded between panels
        /// </summary>
        [DefaultValue(DefaultPanelSpacing)]
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int PanelSpacing { get; set; }

        /// <summary>
        /// Gets or sets the external spacing of panels inside of tabs
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Padding PanelPadding { get; set; }

        /// <summary>
        /// Gets or sets the internal spacing of panels inside of tabs
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Padding PanelMargin { get { return this._panelMargin; } set { this._panelMargin = value; } }

        /// <summary>
        /// Gets or sets the spacing between tabs
        /// </summary>
        [DefaultValue(DefaultTabSpacing)]
        public int TabSpacing { get; set; }

        /// <summary>
        /// Gets the collection of RibbonTab tabs
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public RibbonTabCollection Tabs { get; }

        /// <summary>
        /// Gets or sets a value indicating if the Ribbon supports being minimized
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool Minimized
        {
            get { return this._minimized; }
            set
            {
                this._minimized = value;
                if (!this.IsDesignMode())
                {
                    if (this._minimized)
                    {
                        this.Height = this.MinimizedHeight;
                    }
                    else
                    {
                        this.Height = this._expandedHeight;
                    }
                    this.Expanded = !this.Minimized;
                    this.UpdateRegions();
                    this.Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets the collection of Contexts of this Ribbon
        /// </summary>
        public RibbonContextCollection Contexts { get; }

        /// <summary>
        /// Gets or sets the Renderer for this Ribbon control
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RibbonRenderer Renderer
        {
            get { return this._renderer; }
            set
            {
                if (value == null)
                {
                    throw new ApplicationException("Null renderer!");
                }
                this._renderer = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the internal spacing of the tab content pane
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Padding TabContentMargin { get; set; }

        /// <summary>
        /// Gets or sets the external spacing of the tabs content pane
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Padding TabContentPadding { get; set; }

        /// <summary>
        /// Gets a value indicating the external spacing of tabs
        /// </summary>
        //[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Padding TabsMargin
        {
            get { return this._tabsMargin; }
            set
            {
                this._tabsMargin = value;
                this.UpdateRegions();
                this.Invalidate();
            }
        }

        /// <summary>
        /// Gets a value indicating the internal spacing of tabs
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Padding TabsPadding { get; set; }

        /// <summary>
        /// Gets a value indicating the internal spacing of the orb
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Padding OrbsPadding { get; set; }

        /// <summary>
        /// Overriden. The maximum size is fixed
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Size MaximumSize
        {
            get
            {
                return new Size(0, 200); //115 was the old one
            }
            set
            {
                //not supported
            }
        }

        /// <summary>
        /// Overriden. The minimum size is fixed
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Size MinimumSize
        {
            get
            {
                return new Size(0, 27); //115);
            }
            set
            {
                //not supported
            }
        }

        /// <summary>
        /// Overriden. The default dock of the ribbon is top
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [DefaultValue(DockStyle.Top)]
        public override DockStyle Dock { get { return base.Dock; } set { base.Dock = value; } }

        /// <summary>
        /// Gets or sets the current panel sensor for this ribbon
        /// </summary>
        [Browsable(false)]
        public RibbonMouseSensor Sensor { get; private set; }

        [DefaultValue(RightToLeft.No)]
        public override RightToLeft RightToLeft
        {
            get { return base.RightToLeft; }
            set
            {
                base.RightToLeft = value;
                this.OnRegionsChanged();
            }
        }

        private bool _CaptionBarVisible;

        /// <summary>
        /// sets or gets the visibility of the caption bar
        /// </summary>
        [DefaultValue(true)]
        public bool CaptionBarVisible
        {
            get { return this._CaptionBarVisible; }
            set
            {
                this._CaptionBarVisible = value;

                if (this.CaptionBarVisible)
                {
                    var tm = this.TabsMargin;
                    tm.Top = CaptionBarHeight + 2;
                    this.TabsMargin = tm;
                }
                else
                {
                    var tm = this.TabsMargin;
                    tm.Top = 2;
                    this.TabsMargin = tm;
                }

                this.UpdateRegions();
                this.Invalidate();
            }
        }

        public override void Refresh()
        {
            try
            {
                if (this.IsDisposed == false)
                {
                    if (this.InvokeRequired)
                    {
                        HandlerCallbackMethode del = this.Refresh;
                        this.Invoke(del);
                    }
                    else
                    {
                        base.Refresh();
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        #region cr

        private string cr => "Professional Ribbon\n\n2009 Jos?Manuel Menéndez Poo\nwww.menendezpoo.com";

        #endregion

        ///// <summary>
        ///// Gets or sets the Font associated with Ribbon Items.
        ///// </summary>
        //[DefaultValue(null)]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        //public Font RibbonItemFont
        //{
        //    get { return _RibbonItemFont; }
        //    set { _RibbonItemFont = value;}
        //}

        /// <summary>
        /// Gets or sets the Font associated with Ribbon tabs and the ORB.
        /// </summary>
        [DefaultValue(null)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Font RibbonTabFont { get; set; } = new Font("Trebuchet MS", 9);

        #endregion

        #region Handler Methods

        /// <summary>
        /// Resends the mousedown to PopupManager
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _mouseHook_MouseDown(object sender, MouseEventArgs e)
        {
            var handled = false;
            if (!this.RectangleToScreen(this.OrbBounds).Contains(e.Location))
            {
                handled = RibbonPopupManager.FeedHookClick(e);
            }
            if (this.RectangleToScreen(this.Bounds).Contains(e.Location))
            {
                //they clicked inside the ribbon
                handled = true;
            }
            if (this.Minimized && !handled)
            {
                this.Expanded = false;
            }
        }

        /// <summary>
        /// Checks if MouseWheel should be raised
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _mouseHook_MouseWheel(object sender, MouseEventArgs e)
        {
            if (!RibbonPopupManager.FeedMouseWheel(e))
            {
                if (this.RectangleToScreen(new Rectangle(Point.Empty, this.Size)).Contains(e.Location))
                {
                    this.OnMouseWheel(e);
                }
            }
        }

        /// <summary>
        /// Raises the OrbClicked event
        /// </summary>
        /// <param name="e">event data</param>
        internal virtual void OnOrbClicked(EventArgs e)
        {
            if (this.OrbPressed)
            {
                RibbonPopupManager.Dismiss(RibbonPopupManager.DismissReason.ItemClicked);
            }
            else
            {
                this.ShowOrbDropDown();
            }

            if (this.OrbClicked != null)
            {
                this.OrbClicked(this, e);
            }
        }

        /// <summary>
        /// Raises the OrbDoubleClicked
        /// </summary>
        /// <param name="e"></param>
        internal virtual void OnOrbDoubleClicked(EventArgs e)
        {
            if (this.OrbDoubleClick != null)
            {
                this.OrbDoubleClick(this, e);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes hooks
        /// </summary>
        private void SetUpHooks()
        {
            if (RibbonDesigner.Current == null)
            {
                if (this._mouseHook == null)
                {
                    this._mouseHook = new GlobalHook(GlobalHook.HookTypes.Mouse);
                    this._mouseHook.MouseWheel += this._mouseHook_MouseWheel;
                    this._mouseHook.MouseDown += this._mouseHook_MouseDown;
                }

                if (this._keyboardHook == null)
                {
                    this._keyboardHook = new GlobalHook(GlobalHook.HookTypes.Keyboard);
                    this._keyboardHook.KeyDown += this._keyboardHook_KeyDown;
                }
            }
        }

        private void _keyboardHook_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                RibbonPopupManager.Dismiss(RibbonPopupManager.DismissReason.EscapePressed);
            }
        }

        /// <summary>
        /// Shows the Orb's dropdown
        /// </summary>
        public void ShowOrbDropDown()
        {
            this.OrbPressed = true;
            if (this.RightToLeft == RightToLeft.No)
            {
                if (this.OrbStyle == RibbonOrbStyle.Office_2007)
                {
                    this.OrbDropDown.Show(this.PointToScreen(new Point(this.OrbBounds.X - 4, this.OrbBounds.Bottom - this.OrbDropDown.ContentMargin.Top + 2)));
                }
                else if (this.OrbStyle == RibbonOrbStyle.Office_2010 | this.OrbStyle == RibbonOrbStyle.Office_2013) //Michael Spradlin - 05/03/2013 Office 2013 Style Changes
                {
                    this.OrbDropDown.Show(this.PointToScreen(new Point(this.OrbBounds.X - 4, this.OrbBounds.Bottom)));
                }
                else if (this.OrbStyle == RibbonOrbStyle.Office_2007)
                {
                    this.OrbDropDown.Show(this.PointToScreen(new Point(this.OrbBounds.Right + 4 - this.OrbDropDown.Width, this.OrbBounds.Bottom - this.OrbDropDown.ContentMargin.Top + 2)));
                }
                else if (this.OrbStyle == RibbonOrbStyle.Office_2010 | this.OrbStyle == RibbonOrbStyle.Office_2013) //Michael Spradlin - 05/03/2013 Office 2013 Style Changes
                {
                    this.OrbDropDown.Show(this.PointToScreen(new Point(this.OrbBounds.Right + 4 - this.OrbDropDown.Width, this.OrbBounds.Bottom)));
                }
            }
        }

        /// <summary>
        /// Shows the Orb's dropdown at the specified point.
        /// </summary>
        public void ShowOrbDropDown(Point pt)
        {
            this.OrbPressed = true;
            this.OrbDropDown.Show(this.PointToScreen(pt));
        }

        /// <summary>
        /// Drops out the old sensor and creates a new one
        /// </summary>
        private void RenewSensor()
        {
            if (this.ActiveTab == null)
            {
                return;
            }

            if (this.Sensor != null)
            {
                this.Sensor.Dispose();
            }

            this.Sensor = new RibbonMouseSensor(this, this, this.ActiveTab);

            if (this.CaptionButtonsVisible)
            {
                this.Sensor.Items.AddRange(new RibbonItem[] { this.CloseButton, this.MaximizeRestoreButton, this.MinimizeButton });
            }
        }

        /// <summary>
        /// Sets the value of the <see cref="BorderMode"/> property
        /// </summary>
        /// <param name="borderMode">Actual border mode accquired</param>
        private void SetActualBorderMode(RibbonWindowMode borderMode)
        {
            var trigger = this.ActualBorderMode != borderMode;

            this.ActualBorderMode = borderMode;

            if (trigger)
            {
                this.OnActualBorderModeChanged(EventArgs.Empty);
            }

            this.SetCaptionButtonsVisible(borderMode == RibbonWindowMode.NonClientAreaCustomDrawn);
        }

        /// <summary>
        /// Sets the value of the <see cref="CaptionButtonsVisible"/> property
        /// </summary>
        /// <param name="visible">Value to set to the caption buttons</param>
        private void SetCaptionButtonsVisible(bool visible)
        {
            var trigger = this.CaptionButtonsVisible != visible;

            this.CaptionButtonsVisible = visible;

            if (trigger)
            {
                this.OnCaptionButtonsVisibleChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Suspends any drawing/regions update operation
        /// </summary>
        public void SuspendUpdating()
        {
            this._updatingSuspended = true;
        }

        /// <summary>
        /// Resumes any drawing/regions update operation
        /// </summary>
        /// <param name="update"></param>
        public void ResumeUpdating()
        {
            this.ResumeUpdating(true);
        }

        /// <summary>
        /// Resumes any drawing/regions update operation
        /// </summary>
        /// <param name="update"></param>
        public void ResumeUpdating(bool update)
        {
            this._updatingSuspended = false;

            if (update)
            {
                this.OnRegionsChanged();
            }
        }

        /// <summary>
        /// Removes all helper controls placed by any reason.
        /// Contol's visibility is set to false before removed.
        /// </summary>
        private void RemoveHelperControls()
        {
            RibbonPopupManager.Dismiss(RibbonPopupManager.DismissReason.AppClicked);

            while (this.Controls.Count > 0)
            {
                var ctl = this.Controls[0];

                ctl.Visible = false;

                this.Controls.Remove(ctl);
            }
        }

        /// <summary>
        /// Hittest on tab
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>true if a tab has been clicked</returns>
        internal bool TabHitTest(int x, int y)
        {
            //if (Rectangle.FromLTRB(Right - 10, Bottom - 10, Right, Bottom).Contains(x, y))
            //{
            //   MessageBox.Show(cr);
            //}

            //look for mouse on tabs
            foreach (var tab in this.Tabs)
            {
                if (tab.TabBounds.Contains(x, y))
                {
                    this.ActiveTab = tab;
                    this.Expanded = true;

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Updates the regions of the tabs and the tab content bounds of the active tab
        /// </summary>
        internal void UpdateRegions()
        {
            this.UpdateRegions(null);
        }

        /// <summary>
        /// Updates the regions of the tabs and the tab content bounds of the active tab
        /// </summary>
        internal void UpdateRegions(Graphics g)
        {
            var graphicsCreated = false;

            if (this.IsDisposed || this._updatingSuspended)
            {
                return;
            }

            ///Graphics for measurement
            if (g == null)
            {
                g = this.CreateGraphics();
                graphicsCreated = true;
            }

            ///Saves the bottom of the tabs
            var tabsBottom = 0;

            if (this.RightToLeft == RightToLeft.No)
            {
                ///X coordinate reminder
                var curLeft = this.TabsMargin.Left + this.OrbBounds.Width;

                ///Saves the width of the larger tab
                var maxWidth = 0;

                #region Assign default tab bounds (best case)

                foreach (var tab in this.Tabs)
                {
                    if (tab.Visible || this.IsDesignMode())
                    {
                        var tabSize = tab.MeasureSize(this, new RibbonElementMeasureSizeEventArgs(g, RibbonElementSizeMode.None));
                        var bounds = new Rectangle(
                            curLeft,
                            this.TabsMargin.Top,
                            this.TabsPadding.Left + tabSize.Width + this.TabsPadding.Right,
                            this.TabsPadding.Top + tabSize.Height + this.TabsPadding.Bottom);

                        tab.SetTabBounds(bounds);

                        curLeft = bounds.Right + this.TabSpacing;

                        maxWidth = Math.Max(bounds.Width, maxWidth);
                        tabsBottom = Math.Max(bounds.Bottom, tabsBottom);

                        tab.SetTabContentBounds(
                            Rectangle.FromLTRB(
                                this.TabContentMargin.Left,
                                tabsBottom + this.TabContentMargin.Top,
                                this.ClientSize.Width - 1 - this.TabContentMargin.Right,
                                this.ClientSize.Height - 1 - this.TabContentMargin.Bottom));

                        if (tab.Active)
                        {
                            tab.UpdatePanelsRegions();
                        }
                    }
                    else
                    {
                        tab.SetTabBounds(Rectangle.Empty);
                        tab.SetTabContentBounds(Rectangle.Empty);
                    }
                }

                #endregion

                #region Reduce bounds of tabs if needed

                while (curLeft > this.ClientRectangle.Right && maxWidth > 0)
                {
                    curLeft = this.TabsMargin.Left + this.OrbBounds.Width;
                    maxWidth--;

                    foreach (var tab in this.Tabs)
                    {
                        if (tab.Visible)
                        {
                            if (tab.TabBounds.Width >= maxWidth)
                            {
                                tab.SetTabBounds(new Rectangle(curLeft, this.TabsMargin.Top, maxWidth, tab.TabBounds.Height));
                            }
                            else
                            {
                                tab.SetTabBounds(new Rectangle(new Point(curLeft, this.TabsMargin.Top), tab.TabBounds.Size));
                            }

                            curLeft = tab.TabBounds.Right + this.TabSpacing;
                        }
                    }
                }

                #endregion

                #region Update QuickAccess bounds

                this.QuickAcessToolbar.MeasureSize(this, new RibbonElementMeasureSizeEventArgs(g, RibbonElementSizeMode.Compact));
                if (this.OrbStyle == RibbonOrbStyle.Office_2007)
                {
                    this.QuickAcessToolbar.SetBounds(
                        new Rectangle(new Point(this.OrbBounds.Right + this.QuickAcessToolbar.Margin.Left, this.OrbBounds.Top - 2), this.QuickAcessToolbar.LastMeasuredSize));
                }
                else if (this.OrbStyle == RibbonOrbStyle.Office_2010) //2010 - no need to offset for the orb
                {
                    this.QuickAcessToolbar.SetBounds(new Rectangle(new Point(this.QuickAcessToolbar.Margin.Left, 0), this.QuickAcessToolbar.LastMeasuredSize));
                }
                else if (this.OrbStyle == RibbonOrbStyle.Office_2013) //Michael Spradlin - 05/03/2013 Office 2013 Style Changes : no need to offset for the orb
                {
                    this.QuickAcessToolbar.SetBounds(new Rectangle(new Point(this.QuickAcessToolbar.Margin.Left, 0), this.QuickAcessToolbar.LastMeasuredSize));
                }

                #endregion

                #region Update Caption Buttons bounds

                if (this.CaptionButtonsVisible)
                {
                    var cbs = new Size(20, 20);
                    var cbg = 2;
                    this.CloseButton.SetBounds(new Rectangle(new Point(this.ClientRectangle.Right - cbs.Width - cbg, cbg), cbs));
                    this.MaximizeRestoreButton.SetBounds(new Rectangle(new Point(this.CloseButton.Bounds.Left - cbs.Width, cbg), cbs));
                    this.MinimizeButton.SetBounds(new Rectangle(new Point(this.MaximizeRestoreButton.Bounds.Left - cbs.Width, cbg), cbs));
                }

                #endregion
            }
            else
            {
                ///X coordinate reminder
                var curRight = this.OrbBounds.Left - this.TabsMargin.Left + 4;

                ///Saves the width of the larger tab
                var maxWidth = 0;

                #region Assign default tab bounds (best case)

                foreach (var tab in this.Tabs)
                {
                    if (tab.Visible || this.IsDesignMode())
                    {
                        var tabSize = tab.MeasureSize(this, new RibbonElementMeasureSizeEventArgs(g, RibbonElementSizeMode.None));
                        curRight -= tabSize.Width + this.TabsPadding.Right + this.TabsPadding.Left;

                        var bounds = new Rectangle(
                            curRight,
                            this.TabsMargin.Top,
                            this.TabsPadding.Left + tabSize.Width + this.TabsPadding.Right,
                            this.TabsPadding.Top + tabSize.Height + this.TabsPadding.Bottom);

                        tab.SetTabBounds(bounds);

                        maxWidth = Math.Max(bounds.Width, maxWidth);
                        tabsBottom = Math.Max(bounds.Bottom, tabsBottom);

                        tab.SetTabContentBounds(
                            Rectangle.FromLTRB(
                                this.TabContentMargin.Left,
                                tabsBottom + this.TabContentMargin.Top,
                                this.ClientSize.Width - 1 - this.TabContentMargin.Right,
                                this.ClientSize.Height - 1 - this.TabContentMargin.Bottom));

                        if (tab.Active)
                        {
                            tab.UpdatePanelsRegions();
                        }
                    }
                    else
                    {
                        tab.SetTabBounds(Rectangle.Empty);
                        tab.SetTabContentBounds(Rectangle.Empty);
                    }
                }

                #endregion

                #region Reduce bounds of tabs if needed

                while (curRight < this.ClientRectangle.Left && maxWidth > 0)
                {
                    curRight = this.TabsMargin.Left + this.OrbBounds.Width;
                    maxWidth--;

                    foreach (var tab in this.Tabs)
                    {
                        if (tab.Visible)
                        {
                            if (tab.TabBounds.Width >= maxWidth)
                            {
                                tab.SetTabBounds(new Rectangle(curRight, this.TabsMargin.Top, maxWidth, tab.TabBounds.Height));
                            }
                            else
                            {
                                tab.SetTabBounds(new Rectangle(new Point(curRight, this.TabsMargin.Top), tab.TabBounds.Size));
                            }
                            curRight = tab.TabBounds.Right + this.TabSpacing;
                        }
                    }
                }

                #endregion

                #region Update QuickAccess bounds

                this.QuickAcessToolbar.MeasureSize(this, new RibbonElementMeasureSizeEventArgs(g, RibbonElementSizeMode.Compact));
                if (this.OrbStyle == RibbonOrbStyle.Office_2007)
                {
                    this.QuickAcessToolbar.SetBounds(
                        new Rectangle(
                            new Point(this.OrbBounds.Left - this.QuickAcessToolbar.Margin.Right - this.QuickAcessToolbar.LastMeasuredSize.Width, this.OrbBounds.Top - 2),
                            this.QuickAcessToolbar.LastMeasuredSize));
                }
                else if (this.OrbStyle == RibbonOrbStyle.Office_2010) //2010 - no need to offset for the orb
                {
                    this.QuickAcessToolbar.SetBounds(
                        new Rectangle(
                            new Point(this.ClientRectangle.Right - this.QuickAcessToolbar.Margin.Right - this.QuickAcessToolbar.LastMeasuredSize.Width, 0),
                            this.QuickAcessToolbar.LastMeasuredSize));
                }
                else if (this.OrbStyle == RibbonOrbStyle.Office_2013) //Michael Spradlin - 05/03/2013 Office 2013 Style Changes: no need to offset for the orb
                {
                    this.QuickAcessToolbar.SetBounds(
                        new Rectangle(
                            new Point(this.ClientRectangle.Right - this.QuickAcessToolbar.Margin.Right - this.QuickAcessToolbar.LastMeasuredSize.Width, 0),
                            this.QuickAcessToolbar.LastMeasuredSize));
                }

                #endregion

                #region Update Caption Buttons bounds

                if (this.CaptionButtonsVisible)
                {
                    var cbs = new Size(20, 20);
                    var cbg = 2;
                    this.CloseButton.SetBounds(new Rectangle(new Point(this.ClientRectangle.Left, cbg), cbs));
                    this.MaximizeRestoreButton.SetBounds(new Rectangle(new Point(this.CloseButton.Bounds.Right, cbg), cbs));
                    this.MinimizeButton.SetBounds(new Rectangle(new Point(this.MaximizeRestoreButton.Bounds.Right, cbg), cbs));
                }

                #endregion
            }

            //Update the minimize settings
            //_minimizedHeight = tabsBottom;

            if (graphicsCreated)
            {
                g.Dispose();
            }

            this._lastSizeMeasured = this.Size;

            this.RenewSensor();
        }

        /// <summary>
        /// Forces a size recalculation on the entire control
        /// </summary>
        internal void OnRegionsChanged()
        {
            if (this._updatingSuspended)
            {
                return;
            }

            //Kevin - Fix when only one tab present and there is a textbox on it. It will loose focus after each char is entered
            //if (Tabs.Count == 1)
            if (this.Tabs.Count == 1 && this.ActiveTab != this.Tabs[0])
            {
                this.ActiveTab = this.Tabs[0];
            }

            this._lastSizeMeasured = Size.Empty;

            this.Refresh();
        }

        /// <summary>
        /// Redraws the specified tab
        /// </summary>
        /// <param name="tab"></param>
        internal void RedrawTab(RibbonTab tab)
        {
            using (var g = this.CreateGraphics())
            {
                var clip = Rectangle.FromLTRB(tab.TabBounds.Left, tab.TabBounds.Top, tab.TabBounds.Right, tab.TabBounds.Bottom);

                g.SetClip(clip);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.TextRenderingHint = TextRenderingHint.AntiAlias;
                tab.OnPaint(this, new RibbonElementPaintEventArgs(tab.TabBounds, g, RibbonElementSizeMode.None));
            }
        }

        /// <summary>
        /// Sets the currently selected tab
        /// </summary>
        /// <param name="tab"></param>
        private void SetSelectedTab(RibbonTab tab)
        {
            if (tab == this._lastSelectedTab)
            {
                return;
            }

            if (this._lastSelectedTab != null)
            {
                this._lastSelectedTab.SetSelected(false);
                this.RedrawTab(this._lastSelectedTab);
            }

            if (tab != null)
            {
                tab.SetSelected(true);
                this.RedrawTab(tab);
            }

            this._lastSelectedTab = tab;
        }

        /// <summary>
        /// Suspends the sensor activity
        /// </summary>
        internal void SuspendSensor()
        {
            if (this.Sensor != null)
            {
                this.Sensor.Suspend();
            }
        }

        /// <summary>
        /// Resumes the sensor activity
        /// </summary>
        internal void ResumeSensor()
        {
            this.Sensor.Resume();
        }

        /// <summary>
        /// Redraws the specified area on the sensor's control
        /// </summary>
        /// <param name="area"></param>
        public void RedrawArea(Rectangle area)
        {
            this.Sensor.Control.Invalidate(area);
        }

        /// <summary>
        /// Activates the next tab available
        /// </summary>
        public void ActivateNextTab()
        {
            var tab = this.NextTab;

            if (tab != null)
            {
                this.ActiveTab = tab;
            }
        }

        /// <summary>
        /// Activates the previous tab available
        /// </summary>
        public void ActivatePreviousTab()
        {
            var tab = this.PreviousTab;

            if (tab != null)
            {
                this.ActiveTab = tab;
            }
        }

        /// <summary>
        /// Handles the mouse down on the orb area
        /// </summary>
        internal void OrbMouseDown()
        {
            this.OnOrbClicked(EventArgs.Empty);
        }

        protected override void WndProc(ref Message m)
        {
            var bypassed = false;

            if (WinApi.IsWindows && (this.ActualBorderMode == RibbonWindowMode.NonClientAreaGlass || this.ActualBorderMode == RibbonWindowMode.NonClientAreaCustomDrawn))
            {
                if (m.Msg == WinApi.WM_NCHITTEST) //0x84
                {
                    var f = this.FindForm();
                    Rectangle caption;

                    if (this.RightToLeft == RightToLeft.No)
                    {
                        var captionLeft = this.QuickAcessToolbar.Visible ? this.QuickAcessToolbar.Bounds.Right : this.OrbBounds.Right;
                        if (this.QuickAcessToolbar.Visible && this.QuickAcessToolbar.DropDownButtonVisible)
                        {
                            captionLeft = this.QuickAcessToolbar.DropDownButton.Bounds.Right;
                        }
                        caption = Rectangle.FromLTRB(captionLeft, 0, this.Width, this.CaptionBarSize);
                    }
                    else
                    {
                        var captionRight = this.QuickAcessToolbar.Visible ? this.QuickAcessToolbar.Bounds.Left : this.OrbBounds.Left;
                        if (this.QuickAcessToolbar.Visible && this.QuickAcessToolbar.DropDownButtonVisible)
                        {
                            captionRight = this.QuickAcessToolbar.DropDownButton.Bounds.Left;
                        }
                        caption = Rectangle.FromLTRB(0, 0, captionRight, this.CaptionBarSize);
                    }

                    var screenPoint = new Point(WinApi.LoWord((int)m.LParam), WinApi.HiWord((int)m.LParam));
                    var ribbonPoint = this.PointToClient(screenPoint);
                    var onCaptionButtons = false;

                    if (this.CaptionButtonsVisible)
                    {
                        onCaptionButtons = this.CloseButton.Bounds.Contains(ribbonPoint) || this.MinimizeButton.Bounds.Contains(ribbonPoint) || this.MaximizeRestoreButton.Bounds.Contains(ribbonPoint);
                    }

                    if (this.RectangleToScreen(caption).Contains(screenPoint) && !onCaptionButtons)
                    {
                        //on the caption bar area
                        var p = this.PointToScreen(screenPoint);
                        WinApi.SendMessage(f.Handle, WinApi.WM_NCHITTEST, m.WParam, WinApi.MakeLParam(p.X, p.Y));
                        m.Result = new IntPtr(-1);
                        bypassed = true;
                        //Kevin - fix so when you mouse off the caption buttons onto the caption area
                        //the buttons will clear the selection. same with the QAT buttons
                        this.CloseButton.SetSelected(false);
                        this.MinimizeButton.SetSelected(false);
                        this.MaximizeRestoreButton.SetSelected(false);
                        this.OrbSelected = false;
                        this.QuickAcessToolbar.DropDownButton.SetSelected(false);
                    }
                }
            }

            if (!bypassed)
            {
                base.WndProc(ref m);
            }
        }

        /// <summary>
        /// Paints the Ribbon on the specified device
        /// </summary>
        /// <param name="g">Device where to paint on</param>
        /// <param name="clip">Clip rectangle</param>
        private void PaintOn(Graphics g, Rectangle clip)
        {
            try
            {
                if (WinApi.IsWindows && Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                }

                //Caption Background
                this.Renderer.OnRenderRibbonBackground(new RibbonRenderEventArgs(this, g, clip));

                //Caption Bar
                this.Renderer.OnRenderRibbonCaptionBar(new RibbonRenderEventArgs(this, g, clip));

                //Caption Buttons
                if (this.CaptionButtonsVisible)
                {
                    this.MinimizeButton.OnPaint(this, new RibbonElementPaintEventArgs(clip, g, RibbonElementSizeMode.Medium));
                    this.MaximizeRestoreButton.OnPaint(this, new RibbonElementPaintEventArgs(clip, g, RibbonElementSizeMode.Medium));
                    this.CloseButton.OnPaint(this, new RibbonElementPaintEventArgs(clip, g, RibbonElementSizeMode.Medium));
                }

                //Orb
                this.Renderer.OnRenderRibbonOrb(new RibbonRenderEventArgs(this, g, clip));

                //QuickAcess toolbar
                this.QuickAcessToolbar.OnPaint(this, new RibbonElementPaintEventArgs(clip, g, RibbonElementSizeMode.Compact));

                //Render Tabs
                foreach (var tab in this.Tabs)
                {
                    if (tab.Visible || this.IsDesignMode())
                    {
                        tab.OnPaint(this, new RibbonElementPaintEventArgs(tab.TabBounds, g, RibbonElementSizeMode.None, this));
                    }
                }

                if ((this.OrbStyle == RibbonOrbStyle.Office_2010) && this._expanded == false)
                {
                    //draw the divider line at the bottom of the ribbon
                    var p = new Pen(Theme.ColorTable.TabBorder);
                    g.DrawLine(p, this.OrbBounds.Left, this.OrbBounds.Bottom, this.Bounds.Right, this.OrbBounds.Bottom);
                }
                else if ((this.OrbStyle == RibbonOrbStyle.Office_2013) && this._expanded == false)
                {
                    //draw the divider line at the bottom of the ribbon
                    var p = new Pen(Theme.ColorTable.TabBorder_2013);
                    g.DrawLine(p, this.OrbBounds.Left, this.OrbBounds.Bottom, this.Bounds.Right, this.OrbBounds.Bottom);
                }
            }
            catch (Exception e)
            {
            }
        }

        private void PaintDoubleBuffered(Graphics wndGraphics, Rectangle clip)
        {
            using (var bmp = new Bitmap(this.Width, this.Height))
            {
                using (var g = Graphics.FromImage(bmp))
                {
                    g.Clear(Color.Black);
                    this.PaintOn(g, clip);
                    g.Flush();

                    WinApi.BitBlt(wndGraphics.GetHdc(), clip.X, clip.Y, clip.Width, clip.Height, g.GetHdc(), clip.X, clip.Y, WinApi.SRCCOPY);
                    //WinApi.BitBlt(wndGraphics.GetHdc(), 0, 0, Width, Height, g.GetHdc(), 0, 0, WinApi.SRCCOPY);
                }

                //wndGraphics.DrawImage(bmp, Point.Empty);
            }
        }

        internal bool IsDesignMode()
        {
            return this.Site != null && this.Site.DesignMode;
        }

        #endregion

        #region Event Overrides

        /// <summary>
        /// Raises the <see cref="ActiveTabChanged"/> event
        /// </summary>
        /// <param name="e">Event data</param>
        protected virtual void OnActiveTabChanged(EventArgs e)
        {
            if (this.ActiveTabChanged != null)
            {
                this.ActiveTabChanged(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="ActualBorderMode"/> event
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnActualBorderModeChanged(EventArgs e)
        {
            if (this.ActualBorderModeChanged != null)
            {
                this.ActualBorderModeChanged(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="CaptionButtonsVisibleChanged"/> event
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnCaptionButtonsVisibleChanged(EventArgs e)
        {
            if (this.CaptionButtonsVisibleChanged != null)
            {
                this.CaptionButtonsVisibleChanged(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="ExpandedChanged"/> event
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnExpandedChanged(EventArgs e)
        {
            if (this.ExpandedChanged != null)
            {
                this.ExpandedChanged(this, e);
            }
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);

            if (this.OrbBounds.Contains(e.Location))
            {
                this.OnOrbDoubleClicked(EventArgs.Empty);
            }

            foreach (var tab in this.Tabs)
            {
                if (tab.Bounds.Contains(e.Location))
                {
                    this.Minimized = !this.Minimized;
                    break;
                }
            }
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            //base.OnPaintBackground(pevent);
        }

        /// <summary>
        /// Overriden. Raises the Paint event and draws all the Ribbon content
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"></see> that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (this._updatingSuspended)
            {
                return;
            }

            if (this.Size != this._lastSizeMeasured)
            {
                this.UpdateRegions(e.Graphics);
            }

            this.PaintOn(e.Graphics, e.ClipRectangle);
        }

        /// <summary>
        /// Overriden. Raises the Click event and tunnels the message to child elements
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
        }

        /// <summary>
        /// Overriden. Riases the MouseEnter event and tunnels the message to child elements
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
        }

        /// <summary>
        /// Overriden. Raises the MouseLeave  event and tunnels the message to child elements
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            //Console.WriteLine("Ribbon Mouse Leave");
            this.SetSelectedTab(null);
            if (!this.Expanded)
            {
                foreach (var tab in this.Tabs)
                {
                    tab.SetSelected(false);
                }
            }
            this.Invalidate();
        }

        /// <summary>
        /// Overriden. Raises the MouseMove event and tunnels the message to child elements
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"></see> that contains the event data.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            // Kevin Carbis's new code, edited by adriancs, on behave of Carbis
            // The below fix some minor bug. The cursor is not displayed properly
            // when cursor is entering CheckBound of CheckBox and TextBox.
            // The cursor keep changing from Cursors.Default to Cursors.Hand a few times
            // within a second.
            // The below code is obtain from Kcarbis's website

            #region Kevin Carbis's new code, edited by adriancs

            base.OnMouseMove(e);

            if (this.ActiveTab == null)
            {
                return;
            }

            var someTabHitted = false;

            //Check if mouse on tab
            if (this.ActiveTab.TabContentBounds.Contains(e.X, e.Y))
            {
                //Do nothing, everything is on the sensor
            }
            //Check if mouse on orb
            else if (this.OrbVisible && this.OrbBounds.Contains(e.Location) && !this.OrbSelected)
            {
                this.OrbSelected = true;
                this.Invalidate(this.OrbBounds);
            }
            //Check if mouse on QuickAccess toolbar
            else if (this.QuickAcessToolbar.Visible && this.QuickAcessToolbar.Bounds.Contains(e.Location))
            {
            }
            else
            {
                //look for mouse on tabs
                foreach (var tab in this.Tabs)
                {
                    if (tab.TabBounds.Contains(e.X, e.Y))
                    {
                        this.SetSelectedTab(tab);
                        someTabHitted = true;
                        tab.OnMouseMove(e);
                    }
                }
            }

            if (!someTabHitted)
            {
                this.SetSelectedTab(null);
            }

            if (this.OrbSelected && !this.OrbBounds.Contains(e.Location))
            {
                this.OrbSelected = false;
                this.Invalidate(this.OrbBounds);
            }

            #endregion

            #region Kevin Carbis's old code, commented out by adriancs

            //base.OnMouseMove(e);

            ////Kevin Carbis - Need to reset the curor here so we can pickup the cursor when it moves off of an active textbox.  If we don't we will
            ////have the IBeam cursor over the entire ribbon.
            //Cursor.Current = Cursors.Default;

            //if (ActiveTab == null) return;

            //bool someTabHitted = false;

            ////Check if mouse on tab
            //if (ActiveTab.TabContentBounds.Contains(e.X, e.Y))
            //{
            //    //Do nothing, everything is on the sensor
            //}
            ////Check if mouse on orb
            //else if (OrbVisible && OrbBounds.Contains(e.Location) && !OrbSelected)
            //{
            //    OrbSelected = true;
            //    Invalidate(OrbBounds);
            //}
            ////Check if mouse on QuickAccess toolbar
            //else if (QuickAcessToolbar.Visible && QuickAcessToolbar.Bounds.Contains(e.Location))
            //{

            //}
            //else
            //{
            //    //look for mouse on tabs
            //    foreach (RibbonTab tab in Tabs)
            //    {
            //        if (tab.TabBounds.Contains(e.X, e.Y))
            //        {
            //            SetSelectedTab(tab);
            //            someTabHitted = true;
            //            tab.OnMouseMove(e);
            //        }
            //    }
            //}

            //if (!someTabHitted)
            //    SetSelectedTab(null);

            ////Clear the orb highlight
            //if (OrbSelected && !OrbBounds.Contains(e.Location))
            //{
            //    OrbSelected = false;
            //    Invalidate(OrbBounds);
            //}

            #endregion
        }

        /// <summary>
        /// Overriden. Raises the MouseUp event and tunnels the message to child elements
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"></see> that contains the event data.</param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
        }

        /// <summary>
        /// Overriden. Raises the MouseDown event and tunnels the message to child elements
        /// </summary>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (this.OrbBounds.Contains(e.Location))
            {
                this.OrbMouseDown();
            }
            else
            {
                this.TabHitTest(e.X, e.Y);
            }
        }

        /// <summary>
        /// Handles the mouse wheel
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            //if (Tabs.Count == 0 || ActiveTab == null) return;

            //int index = Tabs.IndexOf(ActiveTab);

            //if (e.Delta < 0)
            //{
            //   _tabSum += 0.4f;
            //}
            //else
            //{
            //   _tabSum -= 0.4f;
            //}

            //int tabRounded = Convert.ToInt16(Math.Round(_tabSum));

            //if (tabRounded != 0)
            //{
            //   index += tabRounded;

            //   if (index < 0)
            //   {
            //      index = 0;
            //   }
            //   else if (index >= Tabs.Count - 1)
            //   {
            //      index = Tabs.Count - 1;
            //   }

            //   ActiveTab = Tabs[index];
            //   _tabSum = 0f;
            //}
        }

        /// <summary>
        /// Overriden. Raises the OnSizeChanged event and performs layout calculations
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
        protected override void OnSizeChanged(EventArgs e)
        {
            this.UpdateRegions();

            this.RemoveHelperControls();

            base.OnSizeChanged(e);
        }

        /// <summary>
        /// Handles when its parent has changed
        /// </summary>
        /// <param name="e"></param>
        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);

            if (!(this.Site != null && this.Site.DesignMode))
            {
                this.BorderMode = this.BorderMode;

                if (this.Parent is IRibbonForm)
                {
                    this.FormHelper.Ribbon = this;
                }
            }

            if (this.Parent != null)
            {
                var p = this.Parent;
                while (p.Parent != null)
                {
                    p = p.Parent;
                }
                var parentForm = p as Form;
                if (parentForm != null)
                {
                    parentForm.Deactivate += this.parentForm_Deactivate;
                }
            }
        }

        private void parentForm_Deactivate(object sender, EventArgs e)
        {
            if (Form.ActiveForm == null) // check for ActiveForm, because Click in Orb Menu causes the Form as well to fire the Deactivate Event
            {
                RibbonPopupManager.Dismiss(RibbonPopupManager.DismissReason.AppFocusChanged);
            }
        }

        private void OnPopupRegistered(object sender, EventArgs args)
        {
            if (RibbonPopupManager.PopupCount == 1)
            {
                this.SetUpHooks();
            }
        }

        private void OnPopupUnregistered(object sender, EventArgs args)
        {
            if (RibbonPopupManager.PopupCount == 0 && (this.Minimized == false || (this.Minimized && this.Expanded == false)))
            {
                this.DisposeHooks();
            }
        }

        #endregion
    }
}