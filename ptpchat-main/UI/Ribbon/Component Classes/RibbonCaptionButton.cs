namespace PtpChat.Main.Ribbon.Component_Classes
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    using PtpChat.Main.Ribbon.Classes;

    using RibbonElementSizeMode = PtpChat.Main.Ribbon.Classes.Enums.RibbonElementSizeMode;

    public class RibbonCaptionButton : RibbonButton
    {
        #region Subclasses

        /// <summary>
        /// Defines the possible caption buttons
        /// </summary>
        public enum CaptionButton
        {
            Minimize,

            Maximize,

            Restore,

            Close
        }

        #endregion

        #region Ctor

        /// <summary>
        /// Creates a new CaptionButton
        /// </summary>
        /// <param name="buttonType"></param>
        public RibbonCaptionButton(CaptionButton buttonType)
        {
            this.SetCaptionButtonType(buttonType);
        }

        #endregion

        #region Prop

        /// <summary>
        /// Gets the type of caption button this is
        /// </summary>
        public CaptionButton CaptionButtonType { get; private set; }

        #endregion

        #region Fields

        #endregion

        #region Static

        public const string WindowsIconsFont = "Marlett";

        /// <summary>
        /// Gets the character to render the specified button type
        /// </summary>
        /// <param name="type">type of button</param>
        /// <returns>Character to use with "Marlett" font in Windows, some other representative characters when in other O.S.</returns>
        public static string GetCharFor(CaptionButton type)
        {
            if (WinApi.IsWindows)
            {
                switch (type)
                {
                    case CaptionButton.Minimize:
                        return "0";
                    case CaptionButton.Maximize:
                        return "1";
                    case CaptionButton.Restore:
                        return "2";
                    case CaptionButton.Close:
                        return "r";
                    default:
                        return "?";
                }
            }
            switch (type)
            {
                case CaptionButton.Minimize:
                    return "_";
                case CaptionButton.Maximize:
                    return "+";
                case CaptionButton.Restore:
                    return "^";
                case CaptionButton.Close:
                    return "X";
                default:
                    return "?";
            }
        }

        #endregion

        #region Methods

        public override void OnClick(EventArgs e)
        {
            base.OnClick(e);

            var f = this.Owner.FindForm();

            if (f == null)
            {
                return;
            }

            switch (this.CaptionButtonType)
            {
                case CaptionButton.Minimize:
                    f.WindowState = FormWindowState.Minimized;
                    break;
                case CaptionButton.Maximize:
                    if (f.WindowState == FormWindowState.Normal)
                    {
                        f.WindowState = FormWindowState.Maximized;
                        f.Refresh();
                    }
                    else
                    {
                        f.WindowState = FormWindowState.Normal;
                        f.Refresh();
                    }
                    break;
                case CaptionButton.Restore:
                    f.WindowState = FormWindowState.Normal;
                    break;
                case CaptionButton.Close:
                    f.Close();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Sets value to the type of caption button
        /// </summary>
        /// <param name="buttonType"></param>
        internal void SetCaptionButtonType(CaptionButton buttonType)
        {
            this.Text = GetCharFor(buttonType);
            this.CaptionButtonType = buttonType;
        }

        internal override Rectangle OnGetTextBounds(RibbonElementSizeMode sMode, Rectangle bounds)
        {
            var r = bounds;
            r.X = bounds.Left + 3;
            return r;
        }

        #endregion
    }
}