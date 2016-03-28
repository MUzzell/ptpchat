namespace PtpChat.Main.Ribbon.Classes.Designers
{
    using PtpChat.Main.Ribbon.Classes.Collections;
    using PtpChat.Main.Ribbon.Component_Classes;

    internal class RibbonComboBoxDesigner : RibbonElementWithItemCollectionDesigner
    {
        public override Ribbon Ribbon
        {
            get
            {
                if (this.Component is RibbonComboBox)
                {
                    return (this.Component as RibbonComboBox).Owner;
                }
                return null;
            }
        }

        public override RibbonItemCollection Collection
        {
            get
            {
                if (this.Component is RibbonComboBox)
                {
                    return (this.Component as RibbonComboBox).DropDownItems;
                }
                return null;
            }
        }
    }
}