namespace PtpChat.Main.Ribbon.Component_Classes
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    using RibbonElementMeasureSizeEventArgs = PtpChat.Main.Ribbon.Classes.EventArgs.RibbonElementMeasureSizeEventArgs;
    using RibbonElementPaintEventArgs = PtpChat.Main.Ribbon.Classes.EventArgs.RibbonElementPaintEventArgs;
    using RibbonElementSizeMode = PtpChat.Main.Ribbon.Classes.Enums.RibbonElementSizeMode;
    using RibbonHostSizeModeHandledEventArgs = PtpChat.Main.Ribbon.Classes.EventArgs.RibbonHostSizeModeHandledEventArgs;
    using RibbonTextEventArgs = PtpChat.Main.Ribbon.Classes.EventArgs.RibbonTextEventArgs;

    public class RibbonHost : RibbonItem
    {
        public delegate void RibbonHostSizeModeHandledEventHandler(object sender, RibbonHostSizeModeHandledEventArgs e);

        private RibbonElementSizeMode _lastSizeMode;

        private Control ctl;

        private Font ctlFont;

        private Size ctlSize;

        /// <summary>
        /// Gets or sets the control that this item willl host
        /// </summary>
        public Control HostedControl
        {
            get { return this.ctl; }
            set
            {
                this.ctl = value;
                this.NotifyOwnerRegionsChanged();

                //_mouseHook = new RibbonHelpers.GlobalHook(RibbonHelpers.GlobalHook.HookTypes.Mouse);
                //_mouseHook.MouseDown += new MouseEventHandler(_mouseHook_MouseDown);
                //_mouseHook.MouseUp += new MouseEventHandler(_mouseHook_MouseUp);

                if (this.ctl != null && this.Site == null)
                {
                    //Initially set the control to be hidden. If it needs to be displayed immediately it will
                    //get set in the placecontrol function
                    this.ctl.Visible = false;

                    //changing the owner changes the font so let save it for future use
                    this.ctlFont = this.ctl.Font;
                    this.ctlSize = this.ctl.Size;

                    //hook into some needed events
                    this.ctl.MouseMove += this.ctl_MouseMove;
                    this.CanvasChanged += this.RibbonHost_CanvasChanged;
                    //we must know when our tab changes so we can hide the control
                    if (this.OwnerTab != null)
                    {
                        this.Owner.ActiveTabChanged += this.Owner_ActiveTabChanged;
                    }

                    //the control must always have the same parent as the host item so set it here.
                    if (this.Owner != null)
                    {
                        this.Owner.Controls.Add(this.ctl);
                    }

                    this.ctl.Font = this.ctlFont;
                }
            }
        }

        /// <summary>
        /// Occurs when the SizeMode of the controls container is changing. if you manually set the size of the control you need to set the Handled flag to true.
        /// </summary>
        [Description("Occurs when the SizeMode of the Controls container is changing. if you manually set the size of the control you need to set the Handled flag to true.")]
        public event RibbonHostSizeModeHandledEventHandler SizeModeChanging;

        /// <summary>
        /// Sets the bounds of the panel
        /// </summary>
        /// <param name="bounds"></param>
        public override void SetBounds(Rectangle bounds)
        {
            base.SetBounds(bounds);
        }

        /// <summary>
        /// Call this method when you need to close a popup that the control is contained in
        /// </summary>
        public void HostCompleted()
        {
            //Kevin Carbis - Clear everything by simulating the click event on the parent item
            //just in case we are in a popup window
            this.OnClick(new MouseEventArgs(MouseButtons.Left, 1, Cursor.Position.X, Cursor.Position.Y, 0));
        }

        /// <summary>
        /// Raises the <see cref="SizeModeChanged"/> event
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnSizeModeChanging(ref RibbonHostSizeModeHandledEventArgs e)
        {
            if (this.SizeModeChanging != null)
            {
                this.SizeModeChanging(this, e);
            }
        }

        private void PlaceControls()
        {
            if (this.ctl != null && this.Site == null)
            {
                this.ctl.Location = new Point(this.Bounds.Left + 1, this.Bounds.Top + 1);
                //if we are located directly in a panel then we need to make sure the panel is not in overflow
                //mode or we will look bad showing on the panel when we shouldn't be
                if (this.Canvas is Ribbon && this.OwnerPanel != null && this.OwnerPanel.SizeMode == RibbonElementSizeMode.Overflow)
                {
                    this.ctl.Visible = false;
                }
            }
        }

        internal override void SetSizeMode(RibbonElementSizeMode sizeMode)
        {
            base.SetSizeMode(sizeMode);
            if (this.OwnerPanel != null && this.OwnerPanel.SizeMode == RibbonElementSizeMode.Overflow)
            {
                this.ctl.Visible = false;
            }
        }

        /// <summary>
        /// Raises the paint event and draws the
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void OnPaint(object sender, RibbonElementPaintEventArgs e)
        {
            if (this.Owner != null)
            {
                var f = new StringFormat();
                f.Alignment = StringAlignment.Center;
                f.LineAlignment = StringAlignment.Center;
                f.Trimming = StringTrimming.None;
                f.FormatFlags |= StringFormatFlags.NoWrap;
                if (this.Site != null && this.Site.DesignMode)
                {
                    this.Owner.Renderer.OnRenderRibbonItemText(new RibbonTextEventArgs(this.Owner, e.Graphics, this.Bounds, this, this.Bounds, this.Site.Name, f));
                }
                else
                {
                    this.Owner.Renderer.OnRenderRibbonItemText(new RibbonTextEventArgs(this.Owner, e.Graphics, this.Bounds, this, this.Bounds, this.Text, f));
                    if (this.ctl != null)
                    {
                        if (this.ctl.Parent == null)
                        {
                            this.Owner.Controls.Add(this.ctl);
                        }

                        //time to show our control
                        this.ctl.Location = new Point(this.Bounds.Left + 1, this.Bounds.Top + 1);
                        this.ctl.Visible = true;
                        this.ctl.BringToFront();
                    }
                }
            }
        }

        /// <summary>
        /// Measures the size of the panel on the mode specified by the event object
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public override Size MeasureSize(object sender, RibbonElementMeasureSizeEventArgs e)
        {
            if (this.Site != null && this.Site.DesignMode && this.Owner != null)
            {
                //when in design mode just paint the name of this control
                var Width = Convert.ToInt32(e.Graphics.MeasureString(this.Site.Name, this.Owner.Font).Width);
                var Height = 20;
                this.SetLastMeasuredSize(new Size(Width, Height));
            }
            else if (this.ctl == null || !this.Visible)
            {
                this.SetLastMeasuredSize(new Size(0, 0));
            }
            else
            {
                this.ctl.Visible = false;
                if (this._lastSizeMode != e.SizeMode)
                {
                    this._lastSizeMode = e.SizeMode;
                    var hev = new RibbonHostSizeModeHandledEventArgs(e.Graphics, e.SizeMode);
                    this.OnSizeModeChanging(ref hev);
                }
                this.SetLastMeasuredSize(new Size(this.ctl.Size.Width + 2, this.ctl.Size.Height + 2));
            }
            return this.LastMeasuredSize;
        }

        private void ctl_MouseMove(object sender, MouseEventArgs e)
        {
            //convert the controls mousemove to the items mousemove to keep the highlighting on the panels in sync
            this.OnMouseMove(e);
            //Console.WriteLine(e.Location.ToString());
        }

        private void Owner_ActiveTabChanged(object sender, EventArgs e)
        {
            //hide this control if our tab is not the active tab
            if (this.OwnerTab != null && this.Owner.ActiveTab != this.OwnerTab)
            {
                this.ctl.Visible = false;
            }
        }

        private void RibbonHost_CanvasChanged(object sender, EventArgs e)
        {
            if (this.ctl != null)
            {
                this.Canvas.Controls.Add(this.ctl);
                this.ctl.Font = this.ctlFont;
                //ctl.Location = new System.Drawing.Point(Bounds.Left + 1, Bounds.Top + 1);
            }
        }
    }
}