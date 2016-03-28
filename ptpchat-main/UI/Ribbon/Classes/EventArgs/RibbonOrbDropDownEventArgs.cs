namespace PtpChat.Main.Ribbon.Classes.EventArgs
{
    using System.Drawing;

    using PtpChat.Main.Ribbon.Component_Classes;

    public class RibbonOrbDropDownEventArgs : RibbonRenderEventArgs
    {
        #region Ctor

        public RibbonOrbDropDownEventArgs(Ribbon ribbon, RibbonOrbDropDown dropDown, Graphics g, Rectangle clip)
            : base(ribbon, g, clip)
        {
            this.RibbonOrbDropDown = dropDown;
        }

        #endregion

        #region Props

        /// <summary>
        /// Gets the RibbonOrbDropDown related to the event
        /// </summary>
        public RibbonOrbDropDown RibbonOrbDropDown { get; }

        #endregion

        #region Fields

        #endregion
    }
}