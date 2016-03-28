namespace PtpChat.Main.Ribbon.Classes.Designers
{
    using PtpChat.Main.Ribbon.Classes.Collections;
    using PtpChat.Main.Ribbon.Component_Classes;

    internal class RibbonItemGroupDesigner : RibbonElementWithItemCollectionDesigner
    {
        public override Ribbon Ribbon
        {
            get
            {
                if (this.Component is RibbonItemGroup)
                {
                    return (this.Component as RibbonItemGroup).Owner;
                }
                return null;
            }
        }

        public override RibbonItemCollection Collection
        {
            get
            {
                if (this.Component is RibbonItemGroup)
                {
                    return (this.Component as RibbonItemGroup).Items;
                }
                return null;
            }
        }
    }
}