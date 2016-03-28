namespace PtpChat.Main.Ribbon.Classes.EventArgs
{
    using System.Drawing;
    using System.Windows.Forms;

    using IRibbonElement = PtpChat.Main.Ribbon.Classes.Interfaces.IRibbonElement;

    public delegate void RibbonElementPopupEventHandler(object sender, RibbonElementPopupEventArgs e);

    public class RibbonElementPopupEventArgs : PopupEventArgs
    {
        public RibbonElementPopupEventArgs(IRibbonElement item)
            : base(item.Owner, item.Owner, false, new Size(-1, -1))
        {
            this.AssociatedRibbonElement = item;
        }

        public RibbonElementPopupEventArgs(IRibbonElement item, PopupEventArgs args)
            : base(args.AssociatedWindow, args.AssociatedControl, args.IsBalloon, args.ToolTipSize)
        {
            this.AssociatedRibbonElement = item;
            this._args = args;
        }

        private readonly PopupEventArgs _args;

        public IRibbonElement AssociatedRibbonElement { get; }

        public new bool Cancel
        {
            get { return this._args == null ? base.Cancel : this._args.Cancel; }
            set
            {
                if (this._args != null)
                {
                    this._args.Cancel = value;
                }
                base.Cancel = value;
            }
        }
    }
}