namespace PtpChat.Main.Ribbon.Classes
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    using Ribbon = PtpChat.Main.Ribbon.Component_Classes.Ribbon;
    using RibbonProfessionalRenderer = PtpChat.Main.Ribbon.Classes.Renderers.RibbonProfessionalRenderer;
    using RibbonWindowMode = PtpChat.Main.Ribbon.Classes.Enums.RibbonWindowMode;

    /// <summary>
    /// This class is used to make a form able to contain a ribbon on the non-client area.
    /// For further instrucions search "ribbon non-client" on www.menendezpoo.com
    /// </summary>
    public class RibbonFormHelper
    {
        #region Subclasses

        /// <summary>
        /// Possible results of a hit test on the non client area of a form
        /// </summary>
        public enum NonClientHitTestResult
        {
            Nowhere = 0,

            Client = 1,

            Caption = 2,

            GrowBox = 4,

            MinimizeButton = 8,

            MaximizeButton = 9,

            Left = 10,

            Right = 11,

            Top = 12,

            TopLeft = 13,

            TopRight = 14,

            Bottom = 15,

            BottomLeft = 16,

            BottomRight = 17
        }

        #endregion

        #region Fields

        private FormWindowState _lastState;

        private bool _frameExtended;

        private Ribbon _ribbon;

        private Size _storeSize;

        #endregion

        #region Ctor

        /// <summary>
        /// Creates a new helper for the specified form
        /// </summary>
        /// <param name="f"></param>
        public RibbonFormHelper(Form f)
        {
            this.Form = f;
            this.Form.Load += this.Form_Activated;
            this.Form.ResizeEnd += this._form_ResizeEnd;
            this.Form.Layout += this._form_Layout;
        }

        private void _form_Layout(object sender, LayoutEventArgs e)
        {
            if (this._lastState == this.Form.WindowState)
            {
                return;
            }

            if (WinApi.IsGlassEnabled)
            {
                this.Form.Invalidate();
            }
            else // on XP systems Invalidate is not sufficient in case the Form contains a control with DockStyle.Fill
            {
                this.Form.Refresh();
            }

            this._lastState = this.Form.WindowState;
        }

        private void _form_ResizeEnd(object sender, System.EventArgs e)
        {
            this.UpdateRibbonConditions();
            this.Form.Refresh();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Ribbon related with the form
        /// </summary>
        public Ribbon Ribbon
        {
            get { return this._ribbon; }
            set
            {
                this._ribbon = value;
                this.UpdateRibbonConditions();
            }
        }

        /// <summary>
        /// Gets or sets the height of the caption bar relative to the form
        /// </summary>
        public int CaptionHeight { get; set; }

        /// <summary>
        /// Gets the form this class is helping
        /// </summary>   
        public Form Form { get; }

        /// <summary>
        /// Gets the margins of the non-client area
        /// </summary>
        public Padding Margins { get; private set; }

        /// <summary>
        /// Gets or sets if the margins are already checked by WndProc
        /// </summary>
        private bool MarginsChecked { get; set; }

        /// <summary>
        /// Gets if the <see cref="Form"/> is currently in Designer mode
        /// </summary>
        private bool DesignMode => this.Form != null && this.Form.Site != null && this.Form.Site.DesignMode;

        #endregion

        #region Methods

        /// <summary>
        /// Checks if ribbon should be docked or floating and updates its size
        /// </summary>
        private void UpdateRibbonConditions()
        {
            if (this.Ribbon == null)
            {
                return;
            }

            if (this.Ribbon.Dock != DockStyle.Top)
            {
                this.Ribbon.Dock = DockStyle.Top;
            }
        }

        /// <summary>
        /// Called when helped form is activated
        /// </summary>
        /// <param name="sender">Object that raised the event</param>
        /// <param name="e">Event data</param>
        public void Form_Paint(object sender, PaintEventArgs e)
        {
            if (this.DesignMode)
            {
                return;
            }

            if (WinApi.IsGlassEnabled)
            {
                WinApi.FillForGlass(e.Graphics, new Rectangle(0, 0, this.Form.Width, this.Form.Height));

                using (Brush b = new SolidBrush(this.Form.BackColor))
                {
                    e.Graphics.FillRectangle(b, Rectangle.FromLTRB(this.Margins.Left - 0, this.Margins.Top + 0, this.Form.Width - this.Margins.Right - 0, this.Form.Height - this.Margins.Bottom - 0));
                }
            }
            else
            {
                this.PaintTitleBar(e);
            }
        }

        /// <summary>
        /// Draws the title bar of the form when not in glass
        /// </summary>
        /// <param name="e"></param>
        private void PaintTitleBar(PaintEventArgs e)
        {
            int radius1 = 4, radius2 = radius1 - 0;
            var rPath = new Rectangle(Point.Empty, this.Form.Size);
            var rInner = new Rectangle(Point.Empty, new Size(rPath.Width - 1, rPath.Height - 1));

            using (var path = RibbonProfessionalRenderer.RoundRectangle(rPath, radius1))
            {
                using (var innerPath = RibbonProfessionalRenderer.RoundRectangle(rInner, radius2))
                {
                    if (this.Ribbon != null && this.Ribbon.ActualBorderMode == RibbonWindowMode.NonClientAreaCustomDrawn)
                    {
                        var renderer = this.Ribbon.Renderer as RibbonProfessionalRenderer;

                        if (renderer != null)
                        {
                            e.Graphics.Clear(Theme.ColorTable.RibbonBackground); // draw the Form border explicitly, otherwise problems as MDI parent occur
                            using (var p = new SolidBrush(Theme.ColorTable.Caption1))
                            {
                                e.Graphics.FillRectangle(p, new Rectangle(0, 0, this.Form.Width, this.Ribbon.CaptionBarSize));
                            }
                            renderer.DrawCaptionBarBackground(new Rectangle(0, this.Margins.Bottom - 1, this.Form.Width, this.Ribbon.CaptionBarSize), e.Graphics);

                            using (var rgn = new Region(path))
                            {
                                //Set Window Region
                                this.Form.Region = rgn;
                                var smbuf = e.Graphics.SmoothingMode;
                                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                                using (var p = new Pen(Theme.ColorTable.FormBorder, 1))
                                {
                                    e.Graphics.DrawPath(p, innerPath);
                                }
                                e.Graphics.SmoothingMode = smbuf;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Called when helped form is activated
        /// </summary>
        /// <param name="sender">Object that raised the event</param>
        /// <param name="e">Event data</param>
        protected virtual void Form_Activated(object sender, System.EventArgs e)
        {
            if (this.DesignMode)
            {
                return;
            }
            var dwmMargins = new WinApi.MARGINS(this.Margins.Left, this.Margins.Right, this.Margins.Bottom + Ribbon.CaptionBarHeight, this.Margins.Bottom);

            if (WinApi.IsVista && !this._frameExtended)
            {
                WinApi.DwmExtendFrameIntoClientArea(this.Form.Handle, ref dwmMargins);
                this._frameExtended = true;
            }
        }

        /// <summary>
        /// Processes the WndProc for a form with a Ribbbon. Returns true if message has been handled
        /// </summary>
        /// <param name="m">Message to process</param>
        /// <returns><c>true</c> if message has been handled. <c>false</c> otherwise</returns>
        public virtual bool WndProc(ref Message m)
        {
            if (this.DesignMode)
            {
                return false;
            }

            var handled = false;

            if (WinApi.IsVista)
            {
                #region Checks if DWM processes the message

                IntPtr result;
                var dwmHandled = WinApi.DwmDefWindowProc(m.HWnd, m.Msg, m.WParam, m.LParam, out result);

                if (dwmHandled == 1)
                {
                    m.Result = result;
                    handled = true;
                }

                #endregion
            }

            //if (m.Msg == WinApi.WM_NCLBUTTONUP)
            //{
            //    UpdateRibbonConditions();
            //}

            if (!handled)
            {
                if (m.Msg == WinApi.WM_NCCALCSIZE && (int)m.WParam == 1) //0x83
                {
                    #region Catch the margins of what the client area would be

                    var nccsp = (WinApi.NCCALCSIZE_PARAMS)Marshal.PtrToStructure(m.LParam, typeof(WinApi.NCCALCSIZE_PARAMS));

                    if (!this.MarginsChecked)
                    {
                        //Set what client area would be for passing to DwmExtendIntoClientArea
                        this.SetMargins(
                            new Padding(nccsp.rect2.Left - nccsp.rect1.Left, nccsp.rect2.Top - nccsp.rect1.Top, nccsp.rect1.Right - nccsp.rect2.Right, nccsp.rect1.Bottom - nccsp.rect2.Bottom));

                        this.MarginsChecked = true;
                    }

                    #region Hack to get rid of the black caption bar when form is maximized on multi-monitor setups with DWM enabled

                    // toATwork: on multi-monitor setups the caption bar when the form is maximized the caption bar is black instead of glass
                    //             * set handled to false and let the base implementation handle it, will work but then the default caption bar
                    //               is also visible - not desired
                    //             * setting the client area to some other value, e.g. descrease the size of the client area by one pixel will
                    //               cause windows to render the caption bar a glass - not correct but the lesser of the two evils
                    if (Screen.AllScreens.Length > 1 && WinApi.IsGlassEnabled)
                    {
                        nccsp.rect0.Bottom -= 1;
                    }

                    #endregion

                    Marshal.StructureToPtr(nccsp, m.LParam, false);

                    m.Result = IntPtr.Zero;
                    handled = true;

                    #endregion
                }
                else if (m.Msg == WinApi.WM_NCACTIVATE && this.Ribbon != null && this.Ribbon.ActualBorderMode == RibbonWindowMode.NonClientAreaCustomDrawn)
                {
                    this.Ribbon.Invalidate();
                    handled = true;
                    if (m.WParam == IntPtr.Zero) // if could be removed because result is ignored if WParam is TRUE
                    {
                        m.Result = (IntPtr)1;
                    }
                }
                else if ((m.Msg == WinApi.WM_ACTIVATE || m.Msg == WinApi.WM_PAINT) && WinApi.IsVista) //0x06 - 0x000F
                {
                    m.Result = (IntPtr)1;
                    handled = false;
                }
                else if (m.Msg == WinApi.WM_NCHITTEST && (int)m.Result == 0) //0x84
                {
                    m.Result = new IntPtr(Convert.ToInt32(this.NonClientHitTest(new Point(WinApi.LoWord((int)m.LParam), WinApi.HiWord((int)m.LParam)))));
                    handled = true;
                }
                else if (m.Msg == WinApi.WM_SYSCOMMAND)
                {
                    var param = IntPtr.Size == 4 ? (uint)m.WParam.ToInt32() : (uint)m.WParam.ToInt64();
                    if ((param & 0xFFF0) == WinApi.SC_RESTORE)
                    {
                        this.Form.Size = this._storeSize;
                    }
                    else if (this.Form.WindowState == FormWindowState.Normal)
                    {
                        this._storeSize = this.Form.Size;
                    }
                }
                else if (m.Msg == WinApi.WM_WINDOWPOSCHANGING || m.Msg == WinApi.WM_WINDOWPOSCHANGED) // needed to update the title of MDI parent (at least)
                {
                    this.Ribbon.Invalidate();
                }
            }
            return handled;
        }

        /// <summary>
        /// Performs hit test for mouse on the non client area of the form
        /// </summary>
        /// <param name="form">Form to check bounds</param>
        /// <param name="dwmMargins">Margins of non client area</param>
        /// <param name="lparam">Lparam of</param>
        /// <returns></returns>
        public virtual NonClientHitTestResult NonClientHitTest(Point hitPoint)
        {
            var topleft = this.Form.RectangleToScreen(new Rectangle(0, 0, this.Margins.Left, this.Margins.Left));

            if (topleft.Contains(hitPoint))
            {
                return NonClientHitTestResult.TopLeft;
            }

            var topright = this.Form.RectangleToScreen(new Rectangle(this.Form.Width - this.Margins.Right, 0, this.Margins.Right, this.Margins.Right));

            if (topright.Contains(hitPoint))
            {
                return NonClientHitTestResult.TopRight;
            }

            var botleft = this.Form.RectangleToScreen(new Rectangle(0, this.Form.Height - this.Margins.Bottom, this.Margins.Left, this.Margins.Bottom));

            if (botleft.Contains(hitPoint))
            {
                return NonClientHitTestResult.BottomLeft;
            }

            var botright = this.Form.RectangleToScreen(new Rectangle(this.Form.Width - this.Margins.Right, this.Form.Height - this.Margins.Bottom, this.Margins.Right, this.Margins.Bottom));

            if (botright.Contains(hitPoint))
            {
                return NonClientHitTestResult.BottomRight;
            }

            var top = this.Form.RectangleToScreen(new Rectangle(0, 0, this.Form.Width, this.Margins.Left));

            if (top.Contains(hitPoint))
            {
                return NonClientHitTestResult.Top;
            }

            var cap = this.Form.RectangleToScreen(new Rectangle(0, this.Margins.Left, this.Form.Width, this.Margins.Top - this.Margins.Left));

            if (cap.Contains(hitPoint))
            {
                return NonClientHitTestResult.Caption;
            }

            var left = this.Form.RectangleToScreen(new Rectangle(0, 0, this.Margins.Left, this.Form.Height));

            if (left.Contains(hitPoint))
            {
                return NonClientHitTestResult.Left;
            }

            var right = this.Form.RectangleToScreen(new Rectangle(this.Form.Width - this.Margins.Right, 0, this.Margins.Right, this.Form.Height));

            if (right.Contains(hitPoint))
            {
                return NonClientHitTestResult.Right;
            }

            var bottom = this.Form.RectangleToScreen(new Rectangle(0, this.Form.Height - this.Margins.Bottom, this.Form.Width, this.Margins.Bottom));

            if (bottom.Contains(hitPoint))
            {
                return NonClientHitTestResult.Bottom;
            }

            return NonClientHitTestResult.Client;
        }

        /// <summary>
        /// Sets the value of the <see cref="Margins"/> property;
        /// </summary>
        /// <param name="p"></param>
        private void SetMargins(Padding p)
        {
            this.Margins = p;

            var formPadding = p;
            formPadding.Top = p.Bottom - 1;

            if (!this.DesignMode)
            {
                this.Form.Padding = formPadding;
            }
        }

        #endregion
    }
}