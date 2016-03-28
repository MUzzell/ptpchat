namespace PtpChat.Main.Ribbon.Classes.EventArgs
{
    using System.Drawing;
    using System.Windows.Forms;

    using Ribbon = PtpChat.Main.Ribbon.Component_Classes.Ribbon;
    using RibbonPanel = PtpChat.Main.Ribbon.Component_Classes.RibbonPanel;

    public sealed class RibbonPanelRenderEventArgs : RibbonRenderEventArgs
    {
        public RibbonPanelRenderEventArgs(Ribbon owner, Graphics g, Rectangle clip, RibbonPanel panel, Control canvas)
            : base(owner, g, clip)
        {
            this.Panel = panel;
            this.Canvas = canvas;
        }

        /// <summary>
        /// Gets or sets the panel related to the events
        /// </summary>
        public RibbonPanel Panel { get; set; }

        /// <summary>
        /// Gets or sets the control where the panel is being rendered
        /// </summary>
        public Control Canvas { get; set; }
    }
}