namespace PtpChat.Main.Ribbon.Classes.Designers
{
    using System.ComponentModel.Design;

    using PtpChat.Main.Ribbon.Classes.Collections;
    using PtpChat.Main.Ribbon.Component_Classes;

    internal class RibbonOrbMenuItemDesigner : RibbonElementWithItemCollectionDesigner
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

        protected override DesignerVerbCollection OnGetVerbs()
        {
            return new DesignerVerbCollection(new[] { new DesignerVerb("Add DescriptionMenuItem", this.AddDescriptionMenuItem), new DesignerVerb("Add Separator", this.AddSeparator) });
        }
    }
}