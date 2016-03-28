namespace PtpChat.Main.Ribbon.Component_Classes
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    using RibbonCanvasEventArgs = PtpChat.Main.Ribbon.Classes.EventArgs.RibbonCanvasEventArgs;
    using RibbonElementPaintEventArgs = PtpChat.Main.Ribbon.Classes.EventArgs.RibbonElementPaintEventArgs;
    using RibbonElementSizeMode = PtpChat.Main.Ribbon.Classes.Enums.RibbonElementSizeMode;
    using RibbonMouseSensor = PtpChat.Main.Ribbon.Classes.RibbonMouseSensor;
    using RibbonPanelFlowDirection = PtpChat.Main.Ribbon.Classes.Enums.RibbonPanelFlowDirection;
    using RibbonPanelRenderEventArgs = PtpChat.Main.Ribbon.Classes.EventArgs.RibbonPanelRenderEventArgs;

    [ToolboxItem(false)]
    public class RibbonPanelPopup : RibbonPopup
    {
        #region Ctor

        internal RibbonPanelPopup(RibbonPanel panel)
        {
            this.DoubleBuffered = true;

            this.Sensor = new RibbonMouseSensor(this, panel.Owner, panel.Items);
            this.Sensor.PanelLimit = panel;
            this.Panel = panel;
            this.Panel.PopUp = this;
            panel.Owner.SuspendSensor();

            using (var g = this.CreateGraphics())
            {
                panel.overflowBoundsBuffer = panel.Bounds;
                var s = panel.SwitchToSize(this, g, this.GetSizeMode(panel));
                s.Width += 100;
                s.Height += 100;
                this.Size = s;
            }

            foreach (var item in panel.Items)
            {
                item.SetCanvas(this);
            }
        }

        #endregion

        #region Fields

        private bool _ignoreNext;

        #endregion

        #region Props

        public RibbonMouseSensor Sensor { get; }

        /// <summary>
        /// Gets the panel related to the form
        /// </summary>
        public RibbonPanel Panel { get; }

        #endregion

        #region Methods

        public RibbonElementSizeMode GetSizeMode(RibbonPanel pnl)
        {
            if (pnl.FlowsTo == RibbonPanelFlowDirection.Right)
            {
                return RibbonElementSizeMode.Medium;
            }
            return RibbonElementSizeMode.Large;
        }

        /// <summary>
        /// Prevents the form from being hidden the next time the mouse clicks on the form.
        /// It is useful for reacting to clicks of items inside items.
        /// </summary>
        public void IgnoreNextClickDeactivation()
        {
            this._ignoreNext = true;
        }

        #endregion

        #region Overrides

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (this._ignoreNext)
            {
                this._ignoreNext = false;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            this.Panel.Owner.Renderer.OnRenderPanelPopupBackground(new RibbonCanvasEventArgs(this.Panel.Owner, e.Graphics, new Rectangle(Point.Empty, this.ClientSize), this, this.Panel));

            foreach (var item in this.Panel.Items)
            {
                item.OnPaint(this, new RibbonElementPaintEventArgs(e.ClipRectangle, e.Graphics, RibbonElementSizeMode.Large));
            }

            this.Panel.Owner.Renderer.OnRenderRibbonPanelText(new RibbonPanelRenderEventArgs(this.Panel.Owner, e.Graphics, e.ClipRectangle, this.Panel, this));
        }

        protected override void OnClosed(EventArgs e)
        {
            foreach (var item in this.Panel.Items)
            {
                item.SetCanvas(null);
            }

            this.Panel.SetPressed(false);
            this.Panel.SetSelected(false);
            this.Panel.Owner.UpdateRegions();
            this.Panel.Owner.Refresh();
            this.Panel.PopUp = null;
            this.Panel.Owner.ResumeSensor();

            this.Panel.PopupShowed = false;

            this.Panel.Owner.RedrawArea(this.Panel.Bounds);
            base.OnClosed(e);
        }

        #endregion

        #region Shadow

        #endregion
    }
}