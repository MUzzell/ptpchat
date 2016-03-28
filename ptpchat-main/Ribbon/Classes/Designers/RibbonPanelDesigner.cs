namespace PtpChat.Main.Ribbon.Classes.Designers
{
    using PtpChat.Main.Ribbon.Classes.Collections;
    using PtpChat.Main.Ribbon.Component_Classes;

    internal class RibbonPanelDesigner : RibbonElementWithItemCollectionDesigner
    {
        public override Ribbon Ribbon
        {
            get
            {
                if (this.Component is RibbonPanel)
                {
                    return (this.Component as RibbonPanel).Owner;
                }
                return null;
            }
        }

        public override RibbonItemCollection Collection
        {
            get
            {
                if (this.Component is RibbonPanel)
                {
                    return (this.Component as RibbonPanel).Items;
                }
                return null;
            }
        }
    }
}