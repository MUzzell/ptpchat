namespace PtpChat.Main.Ribbon.Component_Classes
{
    using System.Drawing;

    using PtpChat.Main.Ribbon.Classes.Enums;

    public class RibbonOrbRecentItem : RibbonButton
    {
        #region Ctor

        public RibbonOrbRecentItem()
        {
        }

        public RibbonOrbRecentItem(string text)
            : this()
        {
            this.Text = text;
        }

        #endregion

        #region Methods

        internal override Rectangle OnGetImageBounds(RibbonElementSizeMode sMode, Rectangle bounds)
        {
            return Rectangle.Empty;
        }

        internal override Rectangle OnGetTextBounds(RibbonElementSizeMode sMode, Rectangle bounds)
        {
            var r = base.OnGetTextBounds(sMode, bounds);

            r.X = this.Bounds.Left + 3;

            return r;
        }

        #endregion
    }
}