namespace PtpChat.Main.Ribbon.Component_Classes
{
    using System.Windows.Forms;

    internal class RibbonWrappedDropDown : ToolStripDropDown
    {
        public RibbonWrappedDropDown()
        {
            this.DoubleBuffered = false;
            this.SetStyle(ControlStyles.Opaque, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.Selectable, false);
            this.SetStyle(ControlStyles.ResizeRedraw, false);
            this.AutoSize = false;
        }
    }
}