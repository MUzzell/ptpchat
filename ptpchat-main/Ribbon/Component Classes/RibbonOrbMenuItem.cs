namespace PtpChat.Main.Ribbon.Component_Classes
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    using PtpChat.Main.Ribbon.Classes.Designers;

    using RibbonArrowDirection = PtpChat.Main.Ribbon.Classes.Enums.RibbonArrowDirection;
    using RibbonButtonStyle = PtpChat.Main.Ribbon.Classes.Enums.RibbonButtonStyle;
    using RibbonDesigner = PtpChat.Main.Ribbon.Classes.Designers.RibbonDesigner;

    [Designer(typeof(RibbonOrbMenuItemDesigner))]
    public class RibbonOrbMenuItem : RibbonButton
    {
        #region Fields

        #endregion

        #region Ctor

        public RibbonOrbMenuItem()
        {
            this.DropDownArrowDirection = RibbonArrowDirection.Left;
            this.SetDropDownMargin(new Padding(10));
            this.DropDownShowing += this.RibbonOrbMenuItem_DropDownShowing;
        }

        public RibbonOrbMenuItem(string text)
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

        #region Methods

        private void RibbonOrbMenuItem_DropDownShowing(object sender, EventArgs e)
        {
            if (this.DropDown != null)
            {
                this.DropDown.DrawIconsBar = false;
            }
        }

        public override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);

            if (RibbonDesigner.Current == null)
            {
                if (this.Owner.OrbDropDown.LastPoppedMenuItem != null)
                {
                    this.Owner.OrbDropDown.LastPoppedMenuItem.CloseDropDown();
                }

                if (this.Style == RibbonButtonStyle.DropDown || this.Style == RibbonButtonStyle.SplitDropDown)
                {
                    this.ShowDropDown();

                    this.Owner.OrbDropDown.LastPoppedMenuItem = this;
                }
            }
        }

        public override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
        }

        internal override Point OnGetDropDownMenuLocation()
        {
            if (this.Owner == null)
            {
                return base.OnGetDropDownMenuLocation();
            }

            var b = this.Owner.RectangleToScreen(this.Bounds);
            var c = this.Owner.OrbDropDown.RectangleToScreen(this.Owner.OrbDropDown.ContentRecentItemsBounds);

            return new Point(b.Right, c.Top);
        }

        internal override Size OnGetDropDownMenuSize()
        {
            var r = this.Owner.OrbDropDown.ContentRecentItemsBounds;
            r.Inflate(-1, -1);
            return r.Size;
        }

        public override void OnClick(EventArgs e)
        {
            base.OnClick(e);
        }

        #endregion
    }
}