namespace PtpChat.Main.Ribbon.Classes.Designers
{
    using System;

    using PtpChat.Main.Ribbon.Classes.Collections;
    using PtpChat.Main.Ribbon.Component_Classes;

    internal class RibbonButtonDesigner : RibbonElementWithItemCollectionDesigner
    {
        public override Ribbon Ribbon
        {
            get
            {
                if (this.Component is RibbonButton)
                {
                    return (this.Component as RibbonButton).Owner;
                }
                return null;
            }
        }

        public override RibbonItemCollection Collection
        {
            get
            {
                if (this.Component is RibbonButton)
                {
                    return (this.Component as RibbonButton).DropDownItems;
                }
                return null;
            }
        }

        protected override void AddButton(object sender, EventArgs e)
        {
            base.AddButton(sender, e);
        }
    }
}