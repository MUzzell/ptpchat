namespace PtpChat.Main.Ribbon.Component_Classes
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    using PtpChat.Main.Ribbon.Classes;

    using RibbonDesigner = PtpChat.Main.Ribbon.Classes.Designers.RibbonDesigner;
    using RibbonPopupManager = PtpChat.Main.Ribbon.Classes.RibbonPopupManager;
    using RibbonProfessionalRenderer = PtpChat.Main.Ribbon.Classes.Renderers.RibbonProfessionalRenderer;

    [ToolboxItem(false)]
    public class RibbonPopup : Control
    {
        #region Ctor

        public RibbonPopup()
        {
            this.SetStyle(ControlStyles.Opaque, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.Selectable, false);
            this.BorderRoundness = 3;
        }

        #endregion

        #region Fields

        #endregion

        #region Events

        public event EventHandler Showed;

        /// <summary>
        /// Raised when the popup is closed
        /// </summary>
        public event EventHandler Closed;

        /// <summary>
        /// Raised when the popup is about to be closed
        /// </summary>
        public event ToolStripDropDownClosingEventHandler Closing;

        /// <summary>
        /// Raised when the Popup is about to be opened
        /// </summary>
        public event CancelEventHandler Opening;

        #endregion

        #region Props

        /// <summary>
        /// Gets or sets the roundness of the border
        /// </summary>
        [Browsable(false)]
        public int BorderRoundness { get; set; }

        /// <summary>
        /// Gets the related ToolStripDropDown
        /// </summary>
        internal RibbonWrappedDropDown WrappedDropDown { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Shows this Popup on the specified location of the screen
        /// </summary>
        /// <param name="screenLocation"></param>
        public void Show(Point screenLocation)
        {
            if (this.WrappedDropDown == null)
            {
                var host = new ToolStripControlHost(this);
                this.WrappedDropDown = new RibbonWrappedDropDown();
                this.WrappedDropDown.AutoClose = RibbonDesigner.Current != null;
                this.WrappedDropDown.Items.Add(host);

                this.WrappedDropDown.Padding = Padding.Empty;
                this.WrappedDropDown.Margin = Padding.Empty;
                host.Padding = Padding.Empty;
                host.Margin = Padding.Empty;

                this.WrappedDropDown.Opening += this.ToolStripDropDown_Opening;
                this.WrappedDropDown.Closing += this.ToolStripDropDown_Closing;
                this.WrappedDropDown.Closed += this.ToolStripDropDown_Closed;
                this.WrappedDropDown.Size = this.Size;
            }
            this.WrappedDropDown.Show(screenLocation);
            RibbonPopupManager.Register(this);

            this.OnShowed(EventArgs.Empty);
        }

        /// <summary>
        /// Handles the Opening event of the ToolStripDropDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripDropDown_Opening(object sender, CancelEventArgs e)
        {
            this.OnOpening(e);
        }

        /// <summary>
        /// Called when pop-up is being opened
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnOpening(CancelEventArgs e)
        {
            if (this.Opening != null)
            {
                this.Opening(this, e);
            }
        }

        /// <summary>
        /// Handles the Closing event of the ToolStripDropDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripDropDown_Closing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            this.OnClosing(e);
        }

        /// <summary>
        /// Handles the closed event of the ToolStripDropDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripDropDown_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            this.OnClosed(EventArgs.Empty);
        }

        /// <summary>
        /// Closes this popup.
        /// </summary>
        public void Close()
        {
            if (this.WrappedDropDown != null)
            {
                this.WrappedDropDown.Close();
            }
        }

        /// <summary>
        /// Raises the <see cref="Closing"/> event
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnClosing(ToolStripDropDownClosingEventArgs e)
        {
            if (this.Closing != null)
            {
                this.Closing(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="Closed"/> event.
        /// <remarks>If you override this event don't forget to call base! Otherwise the popup will not be unregistered and hook will not work!</remarks>
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnClosed(EventArgs e)
        {
            RibbonPopupManager.Unregister(this);

            if (this.Closed != null)
            {
                this.Closed(this, e);
            }

            //if (NextPopup != null)
            //{
            //    NextPopup.CloseForward();
            //    NextPopup = null;
            //}

            //if (PreviousPopup != null && PreviousPopup.NextPopup.Equals(this))
            //{
            //    PreviousPopup.NextPopup = null;
            //}
        }

        /// <summary>
        /// Raises the Showed event
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnShowed(EventArgs e)
        {
            if (this.Showed != null)
            {
                this.Showed(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="Paint"/> event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            using (var p = RibbonProfessionalRenderer.RoundRectangle(new Rectangle(Point.Empty, this.Size), this.BorderRoundness))
            {
                using (var r = new Region(p))
                {
                    this.WrappedDropDown.Region = r;
                }
            }
        }

        /// <summary>
        /// Overriden. Used to drop a shadow on the popup
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;

                if (WinApi.IsXP)
                {
                    cp.ClassStyle |= WinApi.CS_DROPSHADOW;
                }

                return cp;
            }
        }

        #endregion
    }
}