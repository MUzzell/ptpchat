namespace PtpChat.Main.Ribbon.Component_Classes
{
    using System.ComponentModel;
    using System.Drawing;

    public class RibbonOrbOptionButton : RibbonButton
    {
        #region Ctors

        public RibbonOrbOptionButton()
        {
        }

        public RibbonOrbOptionButton(string text)
            : this()
        {
            this.Text = text;
        }

        #endregion

        #region Props

        public override Image Image
        {
            get { return base.Image; }
            set
            {
                base.Image = value;

                this.SmallImage = value;
            }
        }

        [Browsable(false)]
        public override Image SmallImage { get { return base.SmallImage; } set { base.SmallImage = value; } }

        #endregion
    }
}