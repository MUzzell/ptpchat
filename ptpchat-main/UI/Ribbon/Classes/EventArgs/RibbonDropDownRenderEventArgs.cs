namespace PtpChat.Main.Ribbon.Classes.EventArgs
{
    using System;
    using System.Drawing;

    using PtpChat.Main.Ribbon.Component_Classes;

    public class RibbonDropDownRenderEventArgs : EventArgs
    {
        #region ctor

        public RibbonDropDownRenderEventArgs(Graphics g, RibbonDropDown dropDown)
        {
            this.Graphics = g;
            this.DropDown = dropDown;
        }

        #endregion

        #region Props

        /// <summary>
        /// Gets or sets the graphics to paint
        /// </summary>
        public Graphics Graphics { get; set; }

        /// <summary>
        /// Gets or sets the Ribbon DropDown
        /// </summary>
        public RibbonDropDown DropDown { get; set; }

        #endregion
    }
}