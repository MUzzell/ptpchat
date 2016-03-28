namespace PtpChat.Main.Ribbon.Classes.Designers
{
    using PtpChat.Main.Ribbon.Classes.Collections;
    using PtpChat.Main.Ribbon.Component_Classes;

    internal class RibbonButtonListDesigner : RibbonElementWithItemCollectionDesigner
    {
        public override Ribbon Ribbon
        {
            get
            {
                if (this.Component is RibbonButtonList)
                {
                    return (this.Component as RibbonButtonList).Owner;
                }
                return null;
            }
        }

        public override RibbonItemCollection Collection
        {
            get
            {
                if (this.Component is RibbonButtonList)
                {
                    return (this.Component as RibbonButtonList).Buttons;
                }
                return null;
            }
        }
    }
}