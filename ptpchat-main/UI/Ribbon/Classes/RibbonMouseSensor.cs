namespace PtpChat.Main.Ribbon.Classes
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;

    using IContainsSelectableRibbonItems = PtpChat.Main.Ribbon.Classes.Interfaces.IContainsSelectableRibbonItems;
    using IScrollableRibbonItem = PtpChat.Main.Ribbon.Classes.Interfaces.IScrollableRibbonItem;
    using Ribbon = PtpChat.Main.Ribbon.Component_Classes.Ribbon;
    using RibbonItem = PtpChat.Main.Ribbon.Component_Classes.RibbonItem;
    using RibbonPanel = PtpChat.Main.Ribbon.Component_Classes.RibbonPanel;
    using RibbonPanelPopup = PtpChat.Main.Ribbon.Component_Classes.RibbonPanelPopup;
    using RibbonTab = PtpChat.Main.Ribbon.Component_Classes.RibbonTab;
    using RibbonTextBox = PtpChat.Main.Ribbon.Component_Classes.RibbonTextBox;

    /// <summary>
    /// Provides mouse functionality to RibbonTab, RibbonPanel and RibbonItem objects on a specified Control
    /// </summary>
    public class RibbonMouseSensor : IDisposable
    {
        #region Fields

        private RibbonItem _lastMouseDown;

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            this.Disposed = true;
            this.RemoveHandlers();
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes inner fields
        /// </summary>
        private RibbonMouseSensor()
        {
            this.Tabs = new List<RibbonTab>();
            this.Panels = new List<RibbonPanel>();
            this.Items = new List<RibbonItem>();
        }

        /// <summary>
        /// Creates a new Empty Sensor
        /// </summary>
        /// <param name="control">Control to listen mouse events</param>
        /// <param name="ribbon">Ribbon that will be affected</param>
        public RibbonMouseSensor(Control control, Ribbon ribbon)
            : this()
        {
            if (control == null)
            {
                throw new ArgumentNullException("control");
            }
            if (ribbon == null)
            {
                throw new ArgumentNullException("ribbon");
            }

            this.Control = control;
            this.Ribbon = ribbon;

            this.AddHandlers();
        }

        /// <summary>
        /// Creates a new Sensor for specified objects
        /// </summary>
        /// <param name="control">Control to listen mouse events</param>
        /// <param name="ribbon">Ribbon that will be affected</param>
        /// <param name="tabs">Tabs that will be sensed</param>
        /// <param name="panels">Panels that will be sensed</param>
        /// <param name="items">Items that will be sensed</param>
        public RibbonMouseSensor(Control control, Ribbon ribbon, IEnumerable<RibbonTab> tabs, IEnumerable<RibbonPanel> panels, IEnumerable<RibbonItem> items)
            : this(control, ribbon)
        {
            if (tabs != null)
            {
                this.Tabs.AddRange(tabs);
            }
            if (panels != null)
            {
                this.Panels.AddRange(panels);
            }
            if (items != null)
            {
                this.Items.AddRange(items);
            }
        }

        /// <summary>
        /// Creates a new Sensor for the specified RibbonTab
        /// </summary>
        /// <param name="control">Control to listen to mouse events</param>
        /// <param name="ribbon">Ribbon that will be affected</param>
        /// <param name="tab">Tab that will be sensed, from which all panels and items will be extracted to sensing also.</param>
        public RibbonMouseSensor(Control control, Ribbon ribbon, RibbonTab tab)
            : this(control, ribbon)
        {
            this.Tabs.Add(tab);
            this.Panels.AddRange(tab.Panels);

            foreach (var panel in tab.Panels)
            {
                this.Items.AddRange(panel.Items);
            }
        }

        /// <summary>
        /// Creates a new Sensor for only the specified items
        /// </summary>
        /// <param name="control">Control to listen to mouse events</param>
        /// <param name="ribbon">Ribbon that will be affected</param>
        /// <param name="items">Items that will be sensed</param>
        public RibbonMouseSensor(Control control, Ribbon ribbon, IEnumerable<RibbonItem> itemsSource)
            : this(control, ribbon)
        {
            this.ItemsSource = itemsSource;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the control where the sensor listens to mouse events
        /// </summary>
        public Control Control { get; }

        /// <summary>
        /// Gets if the sensor has already been 
        /// </summary>
        public bool Disposed { get; private set; }

        /// <summary>
        /// Gets the RibbonTab hitted by the last <see cref="HitTest"/>
        /// </summary>
        internal RibbonTab HittedTab { get; set; }

        /// <summary>
        /// Gets if the test hit resulted on some scroll button of the hitted tab
        /// </summary>
        internal bool HittedTabScroll => this.HittedTabScrollLeft || this.HittedTabScrollRight;

        /// <summary>
        /// Gets or sets if the last hit test resulted on the left scroll of the hitted tab
        /// </summary>
        internal bool HittedTabScrollLeft { get; set; }

        /// <summary>
        /// Gets or sets if the last hit test resulted on the right scroll of the hitted tab
        /// </summary>
        internal bool HittedTabScrollRight { get; set; }

        /// <summary>
        /// Gets the RibbonPanel hitted by the last <see cref="HitTest"/>
        /// </summary>
        internal RibbonPanel HittedPanel { get; set; }

        /// <summary>
        /// Gets the RibbonItem hitted by the last <see cref="HitTest"/>
        /// </summary>
        internal RibbonItem HittedItem { get; set; }

        /// <summary>
        /// Gets the RibbonItem (on other RibbonItem) hitted by the last <see cref="HitTest"/>
        /// </summary>
        internal RibbonItem HittedSubItem { get; set; }

        /// <summary>
        /// Gets if the sensor is currently suspended
        /// </summary>
        public bool IsSupsended { get; private set; }

        /// <summary>
        /// Gets or ests the source of items what limits the sensing.
        /// If collection is null, all items on the <see cref="Items"/> property will be sensed.
        /// </summary>
        public IEnumerable<RibbonItem> ItemsSource { get; set; }

        /// <summary>
        /// Gets the collection of items this sensor affects.
        /// Sensing can be limitated by the <see cref="ItemsLimit"/> property
        /// </summary>
        public List<RibbonItem> Items { get; }

        /// <summary>
        /// Gets or sets the Panel that will be the limit to be sensed.
        /// If set to null, all panels in the <see cref="Panels"/> property will be sensed.
        /// </summary>
        public RibbonPanel PanelLimit { get; set; }

        /// <summary>
        /// Gets the collection of panels this sensor affects.
        /// Sensing can be limitated by the <see cref="PanelLimit"/> property
        /// </summary>
        public List<RibbonPanel> Panels { get; }

        /// <summary>
        /// Gets the ribbon this sensor responds to
        /// </summary>
        public Ribbon Ribbon { get; }

        /// <summary>
        /// Gets or sets the last selected tab
        /// </summary>
        internal RibbonTab SelectedTab { get; set; }

        /// <summary>
        /// Gets or sets the last selected panel
        /// </summary>
        internal RibbonPanel SelectedPanel { get; set; }

        /// <summary>
        /// Gets or sets the last selected item
        /// </summary>
        internal RibbonItem SelectedItem { get; set; }

        /// <summary>
        /// Gets or sets the last selected sub-item
        /// </summary>
        internal RibbonItem SelectedSubItem { get; set; }

        /// <summary>
        /// Gets or sets the Tab that will be the only to be sensed. 
        /// If set to null, all tabs in the <see cref="Tabs"/> property will be sensed.
        /// </summary>
        public RibbonTab TabLimit { get; set; }

        /// <summary>
        /// Gets the collection of tabs this sensor affects. 
        /// Sensing can be limitated by the <see cref="TabLimit"/> property
        /// </summary>
        public List<RibbonTab> Tabs { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Adds the necessary handlers to the control
        /// </summary>
        private void AddHandlers()
        {
            if (this.Control == null)
            {
                throw new ApplicationException("Control is Null, cant Add RibbonMouseSensor Handles");
            }

            this.Control.MouseMove += this.Control_MouseMove;
            this.Control.MouseLeave += this.Control_MouseLeave;
            this.Control.MouseDown += this.Control_MouseDown;
            this.Control.MouseUp += this.Control_MouseUp;
            this.Control.MouseClick += this.Control_MouseClick;
            this.Control.MouseDoubleClick += this.Control_MouseDoubleClick;
            //Control.MouseEnter 
        }

        private void Control_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.IsSupsended || this.Disposed)
            {
                return;
            }

            #region Panel

            if (this.HittedPanel != null)
            {
                this.HittedPanel.OnDoubleClick(e);
            }

            #endregion

            #region Item

            if (this.HittedItem != null)
            {
                this.HittedItem.OnDoubleClick(e);
            }

            #endregion

            #region SubItem

            if (this.HittedSubItem != null)
            {
                this.HittedSubItem.OnDoubleClick(e);
            }

            #endregion
        }

        private void Control_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.IsSupsended || this.Disposed)
            {
                return;
            }

            #region Panel

            if (this.HittedPanel != null)
            {
                this.HittedPanel.OnClick(e);
            }

            #endregion

            #region Item

            //Kevin Carbis - Added _lastMouseDown variable to track if our click originated with this control.
            //Sometimes when scrolling your mouse moves off the thumb while dragging and your mouseup event
            //is not the same control that you started with.  This omits firing the click event on the control you
            //eventually release the mouse over if it wasn't the control you started with.
            if (this.HittedItem != null && this.HittedItem == this._lastMouseDown)
            {
                //Kevin Carbis - this fixes the focus problem with textboxes when a different item is clicked
                //and the edit box is still visible. This will now close the edit box for the textbox that previously
                //had the focus. Otherwise the first click on a button would close the textbox and you would have to click twice.
                //Control.Focus();
                if (this.Ribbon.ActiveTextBox != null)
                {
                    (this.Ribbon.ActiveTextBox as RibbonTextBox).EndEdit();
                }

                //foreach (RibbonPanel pnl in HittedItem.OwnerTab.Panels)
                //{
                //   foreach (RibbonItem itm in pnl.Items)
                //   {
                //      if (itm is RibbonTextBox && itm != HittedItem)
                //      {
                //         RibbonTextBox txt = (RibbonTextBox)itm;
                //         txt.EndEdit();
                //      }
                //   }
                //}

                this.HittedItem.OnClick(e);
            }

            #endregion

            #region SubItem

            if (this.HittedSubItem != null)
            {
                this.HittedSubItem.OnClick(e);
            }

            #endregion
        }

        /// <summary>
        /// Handles the MouseUp event on the control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Control_MouseUp(object sender, MouseEventArgs e)
        {
            if (this.IsSupsended || this.Disposed)
            {
                return;
            }

            #region Tab Scrolls

            if (this.HittedTab != null)
            {
                if (this.HittedTab.ScrollLeftVisible)
                {
                    this.HittedTab.SetScrollLeftPressed(false);
                    this.Control.Invalidate(this.HittedTab.ScrollLeftBounds);
                }
                if (this.HittedTab.ScrollRightVisible)
                {
                    this.HittedTab.SetScrollRightPressed(false);
                    this.Control.Invalidate(this.HittedTab.ScrollRightBounds);
                }
            }

            #endregion

            #region Panel

            if (this.HittedPanel != null)
            {
                this.HittedPanel.SetPressed(false);
                this.HittedPanel.OnMouseUp(e);
                this.Control.Invalidate(this.HittedPanel.Bounds);
            }

            #endregion

            #region Item

            if (this.HittedItem != null)
            {
                this.HittedItem.SetPressed(false);
                this.HittedItem.OnMouseUp(e);
                this.Control.Invalidate(this.HittedItem.Bounds);
            }

            #endregion

            #region SubItem

            if (this.HittedSubItem != null)
            {
                this.HittedSubItem.SetPressed(false);
                this.HittedSubItem.OnMouseUp(e);
                this.Control.Invalidate(Rectangle.Intersect(this.HittedItem.Bounds, this.HittedSubItem.Bounds));
            }

            #endregion
        }

        /// <summary>
        /// Handles the MouseDown on the control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Control_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.IsSupsended || this.Disposed)
            {
                return;
            }

            this.HitTest(e.Location);
            this._lastMouseDown = this.HittedItem;

            #region Tab Scrolls

            if (this.HittedTab != null)
            {
                if (this.HittedTabScrollLeft)
                {
                    this.HittedTab.SetScrollLeftPressed(true);
                    this.Control.Invalidate(this.HittedTab.ScrollLeftBounds);
                }

                if (this.HittedTabScrollRight)
                {
                    this.HittedTab.SetScrollRightPressed(true);
                    this.Control.Invalidate(this.HittedTab.ScrollRightBounds);
                }
            }

            #endregion

            #region Panel

            if (this.HittedPanel != null)
            {
                this.HittedPanel.SetPressed(true);
                this.HittedPanel.OnMouseDown(e);
                this.Control.Invalidate(this.HittedPanel.Bounds);
            }

            #endregion

            #region Item

            if (this.HittedItem != null)
            {
                this.HittedItem.SetPressed(true);
                this.HittedItem.OnMouseDown(e);
                this.Control.Invalidate(this.HittedItem.Bounds);
            }

            #endregion

            #region SubItem

            if (this.HittedSubItem != null)
            {
                this.HittedSubItem.SetPressed(true);
                this.HittedSubItem.OnMouseDown(e);
                this.Control.Invalidate(Rectangle.Intersect(this.HittedItem.Bounds, this.HittedSubItem.Bounds));
            }

            #endregion
        }

        /// <summary>
        /// Handles the MouseLeave on the control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Control_MouseLeave(object sender, System.EventArgs e)
        {
            if (this.IsSupsended || this.Disposed)
            {
            }
        }

        /// <summary>
        /// Handles the MouseMove on the control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Control_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.IsSupsended || this.Disposed)
            {
                return;
            }
            //Console.WriteLine("MouseMove " + Control.Name);
            this.HitTest(e.Location);

            #region Selected ones

            if (this.SelectedPanel != null && this.SelectedPanel != this.HittedPanel)
            {
                this.SelectedPanel.SetSelected(false);
                this.SelectedPanel.OnMouseLeave(e);
                this.Control.Invalidate(this.SelectedPanel.Bounds);
            }

            if (this.SelectedItem != null && this.SelectedItem != this.HittedItem)
            {
                this.SelectedItem.SetSelected(false);
                this.SelectedItem.OnMouseLeave(e);
                this.Control.Invalidate(this.SelectedItem.Bounds);
            }

            if (this.SelectedSubItem != null && this.SelectedSubItem != this.HittedSubItem)
            {
                this.SelectedSubItem.SetSelected(false);
                this.SelectedSubItem.OnMouseLeave(e);
                this.Control.Invalidate(Rectangle.Intersect(this.SelectedItem.Bounds, this.SelectedSubItem.Bounds));
            }

            #endregion

            #region Tab Scrolls

            if (this.HittedTab != null)
            {
                if (this.HittedTab.ScrollLeftVisible)
                {
                    this.HittedTab.SetScrollLeftSelected(this.HittedTabScrollLeft);
                    this.Control.Invalidate(this.HittedTab.ScrollLeftBounds);
                }
                if (this.HittedTab.ScrollRightVisible)
                {
                    this.HittedTab.SetScrollRightSelected(this.HittedTabScrollRight);
                    this.Control.Invalidate(this.HittedTab.ScrollRightBounds);
                }
            }

            #endregion

            #region Panel

            if (this.HittedPanel != null)
            {
                if (this.HittedPanel == this.SelectedPanel)
                {
                    this.HittedPanel.OnMouseMove(e);
                }
                else
                {
                    this.HittedPanel.SetSelected(true);
                    this.HittedPanel.OnMouseEnter(e);
                    this.Control.Invalidate(this.HittedPanel.Bounds);
                }
            }

            #endregion

            #region Item

            if (this.HittedItem != null)
            {
                if (this.HittedItem == this.SelectedItem)
                {
                    this.HittedItem.OnMouseMove(e);
                }
                else
                {
                    this.HittedItem.SetSelected(true);
                    this.HittedItem.OnMouseEnter(e);
                    this.Control.Invalidate(this.HittedItem.Bounds);
                }
            }

            #endregion

            #region SubItem

            if (this.HittedSubItem != null)
            {
                if (this.HittedSubItem == this.SelectedSubItem)
                {
                    this.HittedSubItem.OnMouseMove(e);
                }
                else
                {
                    this.HittedSubItem.SetSelected(true);
                    this.HittedSubItem.OnMouseEnter(e);
                    this.Control.Invalidate(Rectangle.Intersect(this.HittedItem.Bounds, this.HittedSubItem.Bounds));
                }
            }

            #endregion
        }

        /// <summary>
        /// Performs a hit-test and specifies hitted objects on properties: <see cref="HittedPanel"/>, 
        /// <see cref="HittedTab"/>, <see cref="HittedItem"/> and <see cref="HittedSubItem"/>
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        internal void HitTest(Point p)
        {
            this.SelectedTab = this.HittedTab;
            this.SelectedPanel = this.HittedPanel;
            this.SelectedItem = this.HittedItem;
            this.SelectedSubItem = this.HittedSubItem;

            this.HittedTab = null;
            this.HittedTabScrollLeft = false;
            this.HittedTabScrollRight = false;
            this.HittedPanel = null;
            this.HittedItem = null;
            this.HittedSubItem = null;

            #region Tabs

            if (this.TabLimit != null)
            {
                if (this.TabLimit.TabContentBounds.Contains(p))
                {
                    this.HittedTab = this.TabLimit;
                }
            }
            else
            {
                foreach (var tab in this.Tabs)
                {
                    if (tab.TabContentBounds.Contains(p))
                    {
                        this.HittedTab = tab;
                        break;
                    }
                }
            }

            #endregion

            #region TabScrolls

            if (this.HittedTab != null)
            {
                this.HittedTabScrollLeft = this.HittedTab.ScrollLeftVisible && this.HittedTab.ScrollLeftBounds.Contains(p);
                this.HittedTabScrollRight = this.HittedTab.ScrollRightVisible && this.HittedTab.ScrollRightBounds.Contains(p);
            }

            #endregion

            if (!this.HittedTabScroll)
            {
                #region Panels

                if (this.PanelLimit != null)
                {
                    if (this.PanelLimit.Bounds.Contains(p))
                    {
                        this.HittedPanel = this.PanelLimit;
                    }
                }
                else
                {
                    foreach (var pnl in this.Panels)
                    {
                        if (pnl.Bounds.Contains(p))
                        {
                            this.HittedPanel = pnl;
                            break;
                        }
                    }
                }

                #endregion

                #region Item

                IEnumerable<RibbonItem> items = this.Items;

                if (this.ItemsSource != null)
                {
                    items = this.ItemsSource;
                }

                foreach (var item in items)
                {
                    if (item.OwnerPanel != null && item.OwnerPanel.OverflowMode && !(this.Control is RibbonPanelPopup))
                    {
                        continue;
                    }

                    if (item.Bounds.Contains(p))
                    {
                        this.HittedItem = item;
                        break;
                    }
                }

                #endregion

                #region Subitem

                var container = this.HittedItem as IContainsSelectableRibbonItems;
                var scrollable = this.HittedItem as IScrollableRibbonItem;

                if (container != null)
                {
                    var sensibleBounds = scrollable != null ? scrollable.ContentBounds : this.HittedItem.Bounds;

                    foreach (var item in container.GetItems())
                    {
                        var actualBounds = item.Bounds;
                        actualBounds.Intersect(sensibleBounds);

                        if (actualBounds.Contains(p))
                        {
                            this.HittedSubItem = item;
                        }
                    }
                }

                #endregion
            }
        }

        /// <summary>
        /// Removes the added handlers to the Control
        /// </summary>
        private void RemoveHandlers()
        {
            //Do not Change State because if Text or Image of RibbonItem is Changed in Runtime RemoveHandlers() is called
            /*
             foreach (RibbonItem item in Items)
            {
               item.SetSelected(false);
               item.SetPressed(false);
            }
            */

            this.Control.MouseMove -= this.Control_MouseMove;
            this.Control.MouseLeave -= this.Control_MouseLeave;
            this.Control.MouseDown -= this.Control_MouseDown;
            this.Control.MouseUp -= this.Control_MouseUp;

            // ADDED
            this.Control.MouseClick -= this.Control_MouseClick;
            this.Control.MouseDoubleClick -= this.Control_MouseDoubleClick;
        }

        /// <summary>
        /// Resumes the sensing after being suspended by <see cref="Suspend"/>
        /// </summary>
        public void Resume()
        {
            this.IsSupsended = false;
        }

        /// <summary>
        /// Suspends sensing until <see cref="Resume"/> is called
        /// </summary>
        public void Suspend()
        {
            this.IsSupsended = true;
        }

        #endregion
    }
}