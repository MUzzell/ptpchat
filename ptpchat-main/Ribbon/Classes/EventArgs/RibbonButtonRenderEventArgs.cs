namespace PtpChat.Main.Ribbon.Classes.EventArgs
{
    using System.Drawing;

    using PtpChat.Main.Ribbon.Component_Classes;

    public sealed class RibbonButtonRenderEventArgs : RibbonRenderEventArgs
    {
        public RibbonButtonRenderEventArgs(Ribbon owner, Graphics g, Rectangle clip, RibbonButton button)
            : base(owner, g, clip)
        {
            this.Button = button;
        }

        /// <summary>
        /// Gets or sets the RibbonButton related to the evennt
        /// </summary>
        public RibbonButton Button { get; set; }
    }
}