namespace PtpChat.Main.Ribbon.Component_Classes
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    using PtpChat.Main.Ribbon.Classes.Designers;

    using IContainsRibbonComponents = PtpChat.Main.Ribbon.Classes.Interfaces.IContainsRibbonComponents;
    using IDropDownRibbonItem = PtpChat.Main.Ribbon.Classes.Interfaces.IDropDownRibbonItem;
    using RibbonItemCollection = PtpChat.Main.Ribbon.Classes.Collections.RibbonItemCollection;
    using RibbonItemEventArgs = PtpChat.Main.Ribbon.Classes.EventArgs.RibbonItemEventArgs;

    [Designer(typeof(RibbonComboBoxDesigner))]
    public class RibbonComboBox : RibbonTextBox, IContainsRibbonComponents, IDropDownRibbonItem
    {
        #region IContainsRibbonComponents Members

        public IEnumerable<Component> GetAllChildComponents()
        {
            return this.DropDownItems.ToArray();
        }

        #endregion

        #region Fields

        // Steve
        private RibbonItem _selectedItem;

        private readonly Classes.Set<RibbonItem> _assignedHandlers = new Classes.Set<RibbonItem>();

        #endregion

        #region Events

        /// <summary>
        /// Raised when the DropDown is about to be displayed
        /// </summary>
        public event EventHandler DropDownShowing;

        public event RibbonItemEventHandler DropDownItemClicked;

        public delegate void RibbonItemEventHandler(object sender, RibbonItemEventArgs e);

        #endregion

        #region Ctor

        public RibbonComboBox()
        {
            this.DropDownItems = new RibbonItemCollection();
            this.DropDownButtonVisible = true;
            this.AllowTextEdit = true;
            this.DrawIconsBar = true;
            this.DropDownMaxHeight = 0;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.RemoveHandlers();
            }
            base.Dispose(disposing);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the maximum height for the dropdown window.  0 = autosize.  If the size is smaller than the contents then scrollbars will be shown.
        /// </summary>
        [DefaultValue(0)]
        [Description("Gets or sets the maximum height for the dropdown window.  0 = Autosize.  If the size is smaller than the contents then scrollbars will be shown.")]
        public int DropDownMaxHeight { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if the DropDown portion of the combo box is currently shown.
        /// </summary>
        [DefaultValue(false)]
        [Description("Indicates if the dropdown window is currently visible")]
        public bool DropDownVisible { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating if the DropDown should be resizable
        /// </summary>
        [DefaultValue(false)]
        [Description("Makes the DropDown resizable with a grip on the corner")]
        public bool DropDownResizable { get; set; }

        /// <summary>
        /// Overriden.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override Rectangle TextBoxTextBounds
        {
            get
            {
                var r = base.TextBoxTextBounds;

                r.Width -= this.DropDownButtonBounds.Width;

                return r;
            }
        }

        /// <summary>
        /// Gets the collection of items to be displayed on the dropdown
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public RibbonItemCollection DropDownItems { get; }

        // Steve
        /// <summary>
        /// Gets the selected of item on the dropdown
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
            //Steve
            set
            {
                if (value.GetType().BaseType == typeof(RibbonItem))
                {
                    if (this.DropDownItems.Contains(value))
                    {
                        this._selectedItem = value;
                        this.TextBoxText = this._selectedItem.Text;
                    }
                    else
                    {
                        //_dropDownItems.Add(value);
                        this._selectedItem = value;
                        this.TextBoxText = this._selectedItem.Text;
                    }
                }
            }
        }

        // Kevin
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
                        if (this._selectedItem != item)
                        {
                            this._selectedItem = item;
                            this.TextBoxText = this._selectedItem.Text;
                            var arg = new RibbonItemEventArgs(item);
                            this.OnDropDownItemClicked(ref arg);
                        }
                    }
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Raises the <see cref="DropDownShowing"/> event;
        /// </summary>
        /// <param name="e"></param>
        public void OnDropDownShowing(EventArgs e)
        {
            if (this.DropDownShowing != null)
            {
                this.DropDownShowing(this, e);
            }
        }

        /// <summary>
        /// Gets or sets if the icons bar should be drawn
        /// </summary>
        [DefaultValue(true)]
        public bool DrawIconsBar { get; set; }

        /// <summary>
        /// Shows the DropDown
        /// </summary>
        public virtual void ShowDropDown()
        {
            if (!this.DropDownVisible)
            {
                this.AssignHandlers();

                this.OnDropDownShowing(EventArgs.Empty);

                var dd = new RibbonDropDown(this, this.DropDownItems, this.Owner);
                dd.DropDownMaxHeight = this.DropDownMaxHeight;
                dd.ShowSizingGrip = this.DropDownResizable;
                dd.DrawIconsBar = this.DrawIconsBar;
                dd.Closed += this.DropDown_Closed;
                var location = this.OnGetDropDownMenuLocation();
                dd.Show(location);
                this.DropDownVisible = true;
            }
        }

        private void DropDown_Closed(object sender, EventArgs e)
        {
            this.DropDownVisible = false;

            //Steve - when popup closed, un-highlight the dropdown arrow and redraw
            this.DropDownButtonPressed = false;
            //Kevin - Unselect it as well
            this.DropDownButtonSelected = false;
            this.RedrawItem();
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
            foreach (var item in this._assignedHandlers)
            {
                item.Click -= this.DropDownItem_Click;
            }
            this._assignedHandlers.Clear();
        }

        private void DropDownItem_Click(object sender, EventArgs e)
        {
            // Steve
            this._selectedItem = sender as RibbonItem;

            this.TextBoxText = (sender as RibbonItem).Text;
            //Kevin Carbis
            var ev = new RibbonItemEventArgs(sender as RibbonItem);
            this.OnDropDownItemClicked(ref ev);
        }

        #endregion

        #region Overrides

        protected override bool ClosesDropDownAt(Point p)
        {
            return false;
        }

        protected override void InitTextBox(TextBox t)
        {
            base.InitTextBox(t);

            t.Width -= this.DropDownButtonBounds.Width;
        }

        public override void SetBounds(Rectangle bounds)
        {
            base.SetBounds(bounds);

            this.DropDownButtonBounds = Rectangle.FromLTRB(bounds.Right - 15, bounds.Top, bounds.Right + 1, bounds.Bottom + 1);
        }

        public virtual void OnDropDownItemClicked(ref RibbonItemEventArgs e)
        {
            if (this.DropDownItemClicked != null)
            {
                this.DropDownItemClicked(this, e);
            }
        }

        public override void OnMouseMove(MouseEventArgs e)
        {
            if (!this.Enabled)
            {
                return;
            }

            base.OnMouseMove(e);

            var mustRedraw = false;

            if (this.DropDownButtonBounds.Contains(e.X, e.Y))
            {
                this.Owner.Cursor = Cursors.Default;

                mustRedraw = !this.DropDownButtonSelected;

                this.DropDownButtonSelected = true;
            }
            else if (this.TextBoxBounds.Contains(e.X, e.Y))
            {
                this.Owner.Cursor = this.AllowTextEdit ? Cursors.IBeam : Cursors.Default;

                mustRedraw = this.DropDownButtonSelected;

                this.DropDownButtonSelected = false;
            }
            else
            {
                this.Owner.Cursor = Cursors.Default;
            }

            if (mustRedraw)
            {
                this.RedrawItem();
            }
        }

        public override void OnMouseDown(MouseEventArgs e)
        {
            if (!this.Enabled)
            {
                return;
            }
            // Steve - if allowtextedit is false, allow the textbox to bring up the popup
            if (this.DropDownButtonBounds.Contains(e.X, e.Y) || (this.TextBoxBounds.Contains(e.X, e.Y) != this.AllowTextEdit))
            {
                this.DropDownButtonPressed = true;

                this.ShowDropDown();
            }
            else if (this.TextBoxBounds.Contains(e.X, e.Y) && this.AllowTextEdit)
            {
                this.StartEdit();
            }
        }

        public override void OnMouseUp(MouseEventArgs e)
        {
            if (!this.Enabled)
            {
                return;
            }

            base.OnMouseUp(e);

            //Steve - pressed if set false when popup is closed
            //_dropDownPressed = false;
        }

        public override void OnMouseLeave(MouseEventArgs e)
        {
            if (!this.Enabled)
            {
                return;
            }

            base.OnMouseLeave(e);

            this.DropDownButtonSelected = false;
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
                location = this.Canvas.PointToScreen(new Point(this.TextBoxBounds.Left, this.Bounds.Bottom));
            }
            else
            {
                location = this.Owner.PointToScreen(new Point(this.TextBoxBounds.Left, this.Bounds.Bottom));
            }

            return location;
        }

        #endregion

        #region IDropDownRibbonItem Members

        /// <summary>
        /// Gets or sets the bounds of the DropDown button
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Rectangle DropDownButtonBounds { get; private set; }

        /// <summary>
        /// Gets a value indicating if the DropDown is currently visible
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DropDownButtonVisible { get; }

        /// <summary>
        /// Gets a value indicating if the DropDown button is currently selected
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DropDownButtonSelected { get; private set; }

        /// <summary>
        /// Gets a value indicating if the DropDown button is currently pressed
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DropDownButtonPressed { get; private set; }

        internal override void SetOwner(Ribbon owner)
        {
            base.SetOwner(owner);

            this.DropDownItems.SetOwner(owner);
        }

        internal override void SetOwnerPanel(RibbonPanel ownerPanel)
        {
            base.SetOwnerPanel(ownerPanel);

            this.DropDownItems.SetOwnerPanel(ownerPanel);
        }

        internal override void SetOwnerTab(RibbonTab ownerTab)
        {
            base.SetOwnerTab(ownerTab);

            this.DropDownItems.SetOwnerTab(this.OwnerTab);
        }

        #endregion
    }
}