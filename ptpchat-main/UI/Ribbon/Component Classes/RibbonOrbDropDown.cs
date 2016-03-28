namespace PtpChat.Main.Ribbon.Component_Classes
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    using RibbonDesigner = PtpChat.Main.Ribbon.Classes.Designers.RibbonDesigner;
    using RibbonElementMeasureSizeEventArgs = PtpChat.Main.Ribbon.Classes.EventArgs.RibbonElementMeasureSizeEventArgs;
    using RibbonElementPaintEventArgs = PtpChat.Main.Ribbon.Classes.EventArgs.RibbonElementPaintEventArgs;
    using RibbonElementSizeMode = PtpChat.Main.Ribbon.Classes.Enums.RibbonElementSizeMode;
    using RibbonItemCollection = PtpChat.Main.Ribbon.Classes.Collections.RibbonItemCollection;
    using RibbonMouseSensor = PtpChat.Main.Ribbon.Classes.RibbonMouseSensor;
    using RibbonOrbDropDownEventArgs = PtpChat.Main.Ribbon.Classes.EventArgs.RibbonOrbDropDownEventArgs;

    public class RibbonOrbDropDown : RibbonPopup
    {
        #region const

        private const bool DefaultAutoSizeContentButtons = true;

        private const int DefaultContentButtonsMinWidth = 150;

        private const int DefaultContentRecentItemsMinWidth = 150;

        #endregion

        #region Fields

        internal RibbonOrbMenuItem LastPoppedMenuItem;

        private Rectangle designerSelectedBounds;

        private readonly int glyphGap = 3;

        private Padding _contentMargin;

        private DateTime OpenedTime; //Steve - capture time popup was shown

        private string _recentItemsCaption;

        //private GlobalHook _keyboardHook;
        private int _contentButtonsWidth = DefaultContentButtonsMinWidth;

        #endregion

        #region Ctor

        internal RibbonOrbDropDown(Ribbon ribbon)
        {
            this.DoubleBuffered = true;
            this.Ribbon = ribbon;
            this.MenuItems = new RibbonItemCollection();
            this.RecentItems = new RibbonItemCollection();
            this.OptionItems = new RibbonItemCollection();

            this.MenuItems.SetOwner(this.Ribbon);
            this.RecentItems.SetOwner(this.Ribbon);
            this.OptionItems.SetOwner(this.Ribbon);

            this.OptionItemsPadding = 6;
            this.Size = new Size(527, 447);
            this.BorderRoundness = 8;

            //if (!(Site != null && Site.DesignMode))
            //{
            //   _keyboardHook = new GlobalHook(GlobalHook.HookTypes.Keyboard);
            //   _keyboardHook.KeyUp += new KeyEventHandler(_keyboardHook_KeyUp);
            //}
        }

        ~RibbonOrbDropDown()
        {
            if (this.Sensor != null)
            {
                this.Sensor.Dispose();
            }
            //if (_keyboardHook != null)
            //{
            //   _keyboardHook.Dispose();
            //}
        }

        #endregion

        #region Props

        /// <summary>
        /// Gets all items involved in the dropdown
        /// </summary>
        internal List<RibbonItem> AllItems
        {
            get
            {
                var lst = new List<RibbonItem>();
                lst.AddRange(this.MenuItems);
                lst.AddRange(this.RecentItems);
                lst.AddRange(this.OptionItems);
                return lst;
            }
        }

        /// <summary>
        /// Gets the margin of the content bounds
        /// </summary>
        [Browsable(false)]
        public Padding ContentMargin
        {
            get
            {
                if (this._contentMargin.Size.IsEmpty)
                {
                    this._contentMargin = new Padding(6, 17, 6, 29);
                }

                return this._contentMargin;
            }
        }

        /// <summary>
        /// Gets the bounds of the content (where menu buttons are)
        /// </summary>
        [Browsable(false)]
        public Rectangle ContentBounds
            => Rectangle.FromLTRB(this.ContentMargin.Left, this.ContentMargin.Top, this.ClientRectangle.Right - this.ContentMargin.Right, this.ClientRectangle.Bottom - this.ContentMargin.Bottom);

        /// <summary>
        /// Gets the bounds of the content part that contains the buttons on the left
        /// </summary>
        [Browsable(false)]
        public Rectangle ContentButtonsBounds
        {
            get
            {
                var r = this.ContentBounds;
                r.Width = this._contentButtonsWidth;
                if (this.Ribbon.RightToLeft == RightToLeft.Yes)
                {
                    r.X = this.ContentBounds.Right - this._contentButtonsWidth;
                }
                return r;
            }
        }

        /// <summary>
        /// Gets or sets the minimum width for the content buttons.
        /// </summary>
        [DefaultValue(DefaultContentButtonsMinWidth)]
        public int ContentButtonsMinWidth { get; set; } = DefaultContentButtonsMinWidth;

        /// <summary>
        /// Gets the bounds fo the content part that contains the recent-item list
        /// </summary>
        [Browsable(false)]
        public Rectangle ContentRecentItemsBounds
        {
            get
            {
                var r = this.ContentBounds;
                r.Width -= this._contentButtonsWidth;

                //Steve - Recent Items Caption
                r.Height -= this.ContentRecentItemsCaptionBounds.Height;
                r.Y += this.ContentRecentItemsCaptionBounds.Height;

                if (this.Ribbon.RightToLeft == RightToLeft.No)
                {
                    r.X += this._contentButtonsWidth;
                }

                return r;
            }
        }

        /// <summary>
        /// Gets the bounds of the caption area on the content part of the recent-item list
        /// </summary>
        [Browsable(false)]
        public Rectangle ContentRecentItemsCaptionBounds
        {
            get
            {
                if (this.RecentItemsCaption != null)
                {
                    //Lets measure the height of the text so we take into account the font and its size
                    SizeF cs;
                    using (var g = this.CreateGraphics())
                    {
                        cs = g.MeasureString(this.RecentItemsCaption, this.Ribbon.RibbonTabFont);
                    }
                    var r = this.ContentBounds;
                    r.Width -= this._contentButtonsWidth;
                    r.Height = Convert.ToInt32(cs.Height) + this.Ribbon.ItemMargin.Top + this.Ribbon.ItemMargin.Bottom; //padding
                    r.Height += this.RecentItemsCaptionLineSpacing; //Spacing for the divider line

                    if (this.Ribbon.RightToLeft == RightToLeft.No)
                    {
                        r.X += this._contentButtonsWidth;
                    }
                    return r;
                }
                return Rectangle.Empty;
            }
        }

        /// <summary>
        /// Gets the bounds of the caption area on the content part of the recent-item list
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int RecentItemsCaptionLineSpacing { get; } = 8;

        /// <summary>
        /// Gets or sets the minimum width for the recent items.
        /// </summary>
        [DefaultValue(DefaultContentRecentItemsMinWidth)]
        public int ContentRecentItemsMinWidth { get; set; } = DefaultContentRecentItemsMinWidth;

        /// <summary>
        /// Gets if currently on design mode
        /// </summary>
        private bool RibbonInDesignMode => RibbonDesigner.Current != null;

        /// <summary>
        /// Gets the collection of items shown in the menu area
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public RibbonItemCollection MenuItems { get; }

        /// <summary>
        /// Gets the collection of items shown in the options area (bottom)
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public RibbonItemCollection OptionItems { get; }

        [DefaultValue(6)]
        [Description("Spacing between option buttons (those on the bottom)")]
        public int OptionItemsPadding { get; set; }

        /// <summary>
        /// Gets the collection of items shown in the recent items area
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public RibbonItemCollection RecentItems { get; }

        /// <summary>
        /// Gets or Sets the caption for the Recent Items area
        /// </summary>
        [DefaultValue(null)]
        public string RecentItemsCaption
        {
            get { return this._recentItemsCaption; }
            set
            {
                this._recentItemsCaption = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Gets the ribbon that owns this dropdown
        /// </summary>
        [Browsable(false)]
        public Ribbon Ribbon { get; }

        /// <summary>
        /// Gets the sensor of the dropdown
        /// </summary>
        [Browsable(false)]
        public RibbonMouseSensor Sensor { get; private set; }

        /// <summary>
        /// Gets the bounds of the glyph
        /// </summary>
        internal Rectangle ButtonsGlyphBounds
        {
            get
            {
                var s = new Size(50, 18);
                var rf = this.ContentButtonsBounds;
                var r = new Rectangle(rf.Left + (rf.Width - s.Width * 2) / 2, rf.Top + this.glyphGap, s.Width, s.Height);

                if (this.MenuItems.Count > 0)
                {
                    r.Y = this.MenuItems[this.MenuItems.Count - 1].Bounds.Bottom + this.glyphGap;
                }

                return r;
            }
        }

        /// <summary>
        /// Gets the bounds of the glyph
        /// </summary>
        internal Rectangle ButtonsSeparatorGlyphBounds
        {
            get
            {
                var s = new Size(18, 18);

                var r = this.ButtonsGlyphBounds;

                r.X = r.Right + this.glyphGap;

                return r;
            }
        }

        /// <summary>
        /// Gets the bounds of the recent items add glyph
        /// </summary>
        internal Rectangle RecentGlyphBounds
        {
            get
            {
                var s = new Size(50, 18);
                var rf = this.ContentRecentItemsBounds;
                var r = new Rectangle(rf.Left + this.glyphGap, rf.Top + this.glyphGap, s.Width, s.Height);

                if (this.RecentItems.Count > 0)
                {
                    r.Y = this.RecentItems[this.RecentItems.Count - 1].Bounds.Bottom + this.glyphGap;
                }

                return r;
            }
        }

        /// <summary>
        /// Gets the bounds of the option items add glyph
        /// </summary>
        internal Rectangle OptionGlyphBounds
        {
            get
            {
                var s = new Size(50, 18);
                var rf = this.ContentBounds;
                var r = new Rectangle(rf.Right - s.Width, rf.Bottom + this.glyphGap, s.Width, s.Height);

                if (this.OptionItems.Count > 0)
                {
                    r.X = this.OptionItems[this.OptionItems.Count - 1].Bounds.Left - s.Width - this.glyphGap;
                }

                return r;
            }
        }

        [DefaultValue(DefaultAutoSizeContentButtons)]
        public bool AutoSizeContentButtons { get; set; } = DefaultAutoSizeContentButtons;

        #endregion

        #region Methods

        internal void HandleDesignerItemRemoved(RibbonItem item)
        {
            if (this.MenuItems.Contains(item))
            {
                this.MenuItems.Remove(item);
            }
            else if (this.RecentItems.Contains(item))
            {
                this.RecentItems.Remove(item);
            }
            else if (this.OptionItems.Contains(item))
            {
                this.OptionItems.Remove(item);
            }

            this.OnRegionsChanged();
        }

        /// <summary>
        /// Gets the height that a separator should be on the DropDown
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private int SeparatorHeight(RibbonSeparator s)
        {
            if (!string.IsNullOrEmpty(s.Text))
            {
                return 20;
            }
            return 3;
        }

        /// <summary>
        /// Updates the regions and bounds of items
        /// </summary>
        private void UpdateRegions()
        {
            var curtop = 0;
            var curright = 0;
            var menuItemHeight = 44;
            var recentHeight = 22;
            var mbuttons = 1; //margin
            var mrecent = 1; //margin
            var buttonsHeight = 0;
            var recentsHeight = 0;

            if (this.AutoSizeContentButtons)
            {
                #region important to do the item max width check before the ContentBounds and other stuff is used (internal Property stuff)

                var itemMaxWidth = 0;
                using (var g = this.CreateGraphics())
                {
                    foreach (var item in this.MenuItems)
                    {
                        var width = item.MeasureSize(this, new RibbonElementMeasureSizeEventArgs(g, RibbonElementSizeMode.DropDown)).Width;
                        if (width > itemMaxWidth)
                        {
                            itemMaxWidth = width;
                        }
                    }
                }
                itemMaxWidth = Math.Min(itemMaxWidth, this.ContentBounds.Width - this.ContentRecentItemsMinWidth);
                itemMaxWidth = Math.Max(itemMaxWidth, this.ContentButtonsMinWidth);
                this._contentButtonsWidth = itemMaxWidth;

                #endregion
            }

            var rcontent = this.ContentBounds;
            var rbuttons = this.ContentButtonsBounds;
            var rrecent = this.ContentRecentItemsBounds;

            foreach (var item in this.AllItems)
            {
                item.SetSizeMode(RibbonElementSizeMode.DropDown);
                item.SetCanvas(this);
            }

            #region Menu Items

            curtop = rcontent.Top + 1;

            foreach (var item in this.MenuItems)
            {
                var ritem = new Rectangle(rbuttons.Left + mbuttons, curtop, rbuttons.Width - mbuttons * 2, menuItemHeight);

                if (item is RibbonSeparator)
                {
                    ritem.Height = this.SeparatorHeight(item as RibbonSeparator);
                }

                item.SetBounds(ritem);

                curtop += ritem.Height;
            }

            buttonsHeight = curtop - rcontent.Top + 1;

            #endregion

            #region Recent List

            //curtop = rbuttons.Top; //Steve - for recent documents
            curtop = rrecent.Top; //Steve - for recent documents

            foreach (var item in this.RecentItems)
            {
                var ritem = new Rectangle(rrecent.Left + mrecent, curtop, rrecent.Width - mrecent * 2, recentHeight);

                if (item is RibbonSeparator)
                {
                    ritem.Height = this.SeparatorHeight(item as RibbonSeparator);
                }

                item.SetBounds(ritem);

                curtop += ritem.Height;
            }

            recentsHeight = curtop - rbuttons.Top;

            #endregion

            #region Set size

            var actualHeight = Math.Max(buttonsHeight, recentsHeight);

            if (RibbonDesigner.Current != null)
            {
                actualHeight += this.ButtonsGlyphBounds.Height + this.glyphGap * 2;
            }

            this.Height = actualHeight + this.ContentMargin.Vertical;
            rcontent = this.ContentBounds;

            #endregion

            #region Option buttons

            curright = this.ClientSize.Width - this.ContentMargin.Right;

            using (var g = this.CreateGraphics())
            {
                foreach (var item in this.OptionItems)
                {
                    var s = item.MeasureSize(this, new RibbonElementMeasureSizeEventArgs(g, RibbonElementSizeMode.DropDown));
                    curtop = rcontent.Bottom + (this.ContentMargin.Bottom - s.Height) / 2;
                    item.SetBounds(new Rectangle(new Point(curright - s.Width, curtop), s));
                    curright = item.Bounds.Left - this.OptionItemsPadding;
                }
            }

            #endregion
        }

        /// <summary>
        /// Refreshes the sensor
        /// </summary>
        private void UpdateSensor()
        {
            if (this.Sensor != null)
            {
                this.Sensor.Dispose();
            }

            this.Sensor = new RibbonMouseSensor(this, this.Ribbon, this.AllItems);
        }

        /// <summary>
        /// Updates all areas and bounds of items
        /// </summary>
        internal void OnRegionsChanged()
        {
            this.UpdateRegions();
            this.UpdateSensor();
            this.UpdateDesignerSelectedBounds();
            this.Invalidate();
        }

        /// <summary>
        /// Selects the specified item on the designer
        /// </summary>
        /// <param name="item"></param>
        internal void SelectOnDesigner(RibbonItem item)
        {
            if (RibbonDesigner.Current != null)
            {
                RibbonDesigner.Current.SelectedElement = item;
                this.UpdateDesignerSelectedBounds();
                this.Invalidate();
            }
        }

        /// <summary>
        /// Updates the selection bounds on the designer
        /// </summary>
        internal void UpdateDesignerSelectedBounds()
        {
            this.designerSelectedBounds = Rectangle.Empty;

            if (this.RibbonInDesignMode)
            {
                var item = RibbonDesigner.Current.SelectedElement as RibbonItem;

                if (item != null && this.AllItems.Contains(item))
                {
                    this.designerSelectedBounds = item.Bounds;
                }
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (this.RibbonInDesignMode)
            {
                #region DesignMode clicks

                if (this.ContentBounds.Contains(e.Location))
                {
                    if (this.ContentButtonsBounds.Contains(e.Location))
                    {
                        foreach (var item in this.MenuItems)
                        {
                            if (item.Bounds.Contains(e.Location))
                            {
                                this.SelectOnDesigner(item);
                                break;
                            }
                        }
                    }
                    else if (this.ContentRecentItemsBounds.Contains(e.Location))
                    {
                        foreach (var item in this.RecentItems)
                        {
                            if (item.Bounds.Contains(e.Location))
                            {
                                this.SelectOnDesigner(item);
                                break;
                            }
                        }
                    }
                }
                if (this.ButtonsGlyphBounds.Contains(e.Location))
                {
                    RibbonDesigner.Current.CreteOrbMenuItem(typeof(RibbonOrbMenuItem));
                }
                else if (this.ButtonsSeparatorGlyphBounds.Contains(e.Location))
                {
                    RibbonDesigner.Current.CreteOrbMenuItem(typeof(RibbonSeparator));
                }
                else if (this.RecentGlyphBounds.Contains(e.Location))
                {
                    RibbonDesigner.Current.CreteOrbRecentItem(typeof(RibbonOrbRecentItem));
                }
                else if (this.OptionGlyphBounds.Contains(e.Location))
                {
                    RibbonDesigner.Current.CreteOrbOptionItem(typeof(RibbonOrbOptionButton));
                }
                else
                {
                    foreach (var item in this.OptionItems)
                    {
                        if (item.Bounds.Contains(e.Location))
                        {
                            this.SelectOnDesigner(item);
                            break;
                        }
                    }
                }

                #endregion
            }
        }

        protected override void OnOpening(CancelEventArgs e)
        {
            base.OnOpening(e);

            this.UpdateRegions();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            this.Ribbon.Renderer.OnRenderOrbDropDownBackground(new RibbonOrbDropDownEventArgs(this.Ribbon, this, e.Graphics, e.ClipRectangle));

            foreach (var item in this.AllItems)
            {
                item.OnPaint(this, new RibbonElementPaintEventArgs(e.ClipRectangle, e.Graphics, RibbonElementSizeMode.DropDown));
            }

            if (this.RibbonInDesignMode)
            {
                using (var b = new SolidBrush(Color.FromArgb(50, Color.Blue)))
                {
                    e.Graphics.FillRectangle(b, this.ButtonsGlyphBounds);
                    e.Graphics.FillRectangle(b, this.RecentGlyphBounds);
                    e.Graphics.FillRectangle(b, this.OptionGlyphBounds);
                    e.Graphics.FillRectangle(b, this.ButtonsSeparatorGlyphBounds);
                }

                using (var sf = new StringFormat())
                {
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Center;
                    sf.Trimming = StringTrimming.None;
                    e.Graphics.DrawString("+", this.Font, Brushes.White, this.ButtonsGlyphBounds, sf);
                    e.Graphics.DrawString("+", this.Font, Brushes.White, this.RecentGlyphBounds, sf);
                    e.Graphics.DrawString("+", this.Font, Brushes.White, this.OptionGlyphBounds, sf);
                    e.Graphics.DrawString("---", this.Font, Brushes.White, this.ButtonsSeparatorGlyphBounds, sf);
                }

                using (var p = new Pen(Color.Black))
                {
                    p.DashStyle = DashStyle.Dot;
                    e.Graphics.DrawRectangle(p, this.designerSelectedBounds);
                }

                //e.Graphics.DrawString("Press ESC to Hide", Font, Brushes.Black, Width - 100f, 2f);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            this.Ribbon.OrbPressed = false;
            this.Ribbon.OrbSelected = false;
            this.LastPoppedMenuItem = null;
            foreach (var item in this.AllItems)
            {
                item.SetSelected(false);
                item.SetPressed(false);
            }
            base.OnClosed(e);
        }

        protected override void OnShowed(EventArgs e)
        {
            base.OnShowed(e);
            this.OpenedTime = DateTime.Now;
            this.UpdateSensor();
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (this.Ribbon.RectangleToScreen(this.Ribbon.OrbBounds).Contains(this.PointToScreen(e.Location)))
            {
                this.Ribbon.OnOrbClicked(EventArgs.Empty);
                //Steve - if click time is within the double click time after the drop down was shown, then this is a double click
                if (DateTime.Compare(DateTime.Now, this.OpenedTime.AddMilliseconds(SystemInformation.DoubleClickTime)) < 0)
                {
                    this.Ribbon.OnOrbDoubleClicked(EventArgs.Empty);
                }
            }

            base.OnMouseClick(e);
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);

            if (this.Ribbon.RectangleToScreen(this.Ribbon.OrbBounds).Contains(this.PointToScreen(e.Location)))
            {
                this.Ribbon.OnOrbDoubleClicked(EventArgs.Empty);
            }
        }

        private void _keyboardHook_KeyUp(object sender, KeyEventArgs e)
        {
            //base.OnKeyUp(e);
            if (e.KeyCode == Keys.Down)
            {
                RibbonItem NextItem = null;
                RibbonItem SelectedItem = null;
                foreach (var itm in this.MenuItems)
                {
                    if (itm.Selected)
                    {
                        SelectedItem = itm;
                        break;
                    }
                }
                if (SelectedItem != null)
                {
                    //get the next item in the chain
                    var Index = this.MenuItems.IndexOf(SelectedItem);
                    NextItem = this.GetNextSelectableMenuItem(Index + 1);
                }
                else
                {
                    //nothing found so lets search through the recent buttons
                    foreach (var itm in this.RecentItems)
                    {
                        if (itm.Selected)
                        {
                            SelectedItem = itm;
                            itm.SetSelected(false);
                            itm.RedrawItem();
                            break;
                        }
                    }
                    if (SelectedItem != null)
                    {
                        //get the next item in the chain
                        var Index = this.RecentItems.IndexOf(SelectedItem);
                        NextItem = this.GetNextSelectableRecentItem(Index + 1);
                    }
                    else
                    {
                        //nothing found so lets search through the option buttons
                        foreach (var itm in this.OptionItems)
                        {
                            if (itm.Selected)
                            {
                                SelectedItem = itm;
                                itm.SetSelected(false);
                                itm.RedrawItem();
                                break;
                            }
                        }
                        if (SelectedItem != null)
                        {
                            //get the next item in the chain
                            var Index = this.OptionItems.IndexOf(SelectedItem);
                            NextItem = this.GetNextSelectableOptionItem(Index + 1);
                        }
                    }
                }
                //last check to make sure we found a selected item
                if (SelectedItem == null)
                {
                    //we should have the right item by now so lets select it
                    NextItem = this.GetNextSelectableMenuItem(0);
                    if (NextItem != null)
                    {
                        NextItem.SetSelected(true);
                        NextItem.RedrawItem();
                    }
                }
                else
                {
                    SelectedItem.SetSelected(false);
                    SelectedItem.RedrawItem();

                    NextItem.SetSelected(true);
                    NextItem.RedrawItem();
                }
                //_sensor.SelectedItem = NextItem;
                //_sensor.HittedItem = NextItem;
            }
            else if (e.KeyCode == Keys.Up)
            {
            }
        }

        private RibbonItem GetNextSelectableMenuItem(int StartIndex)
        {
            for (var idx = StartIndex; idx < this.MenuItems.Count; idx++)
            {
                var btn = this.MenuItems[idx] as RibbonButton;
                if (btn != null)
                {
                    return btn;
                }
            }
            //nothing found so lets move on to the recent items
            var NextItem = this.GetNextSelectableRecentItem(0);
            if (NextItem == null)
            {
                //nothing found so lets try the option items
                NextItem = this.GetNextSelectableOptionItem(0);
                if (NextItem == null)
                {
                    //nothing again so go back to the top of the menu items
                    NextItem = this.GetNextSelectableMenuItem(0);
                }
            }
            return NextItem;
        }

        private RibbonItem GetNextSelectableRecentItem(int StartIndex)
        {
            for (var idx = StartIndex; idx < this.RecentItems.Count; idx++)
            {
                var btn = this.RecentItems[idx] as RibbonButton;
                if (btn != null)
                {
                    return btn;
                }
            }
            //nothing found so lets move on to the option items
            var NextItem = this.GetNextSelectableOptionItem(0);
            if (NextItem == null)
            {
                //nothing found so lets try the menu items
                NextItem = this.GetNextSelectableMenuItem(0);
                if (NextItem == null)
                {
                    //nothing again so go back to the top of the recent items
                    NextItem = this.GetNextSelectableRecentItem(0);
                }
            }
            return NextItem;
        }

        private RibbonItem GetNextSelectableOptionItem(int StartIndex)
        {
            for (var idx = StartIndex; idx < this.OptionItems.Count; idx++)
            {
                var btn = this.OptionItems[idx] as RibbonButton;
                if (btn != null)
                {
                    return btn;
                }
            }
            //nothing found so lets move on to the menu items
            var NextItem = this.GetNextSelectableMenuItem(0);
            if (NextItem == null)
            {
                //nothing found so lets try the recent items
                NextItem = this.GetNextSelectableRecentItem(0);
                if (NextItem == null)
                {
                    //nothing again so go back to the top of the option items
                    NextItem = this.GetNextSelectableOptionItem(0);
                }
            }
            return NextItem;
        }

        #endregion
    }
}