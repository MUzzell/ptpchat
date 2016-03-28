namespace PtpChat.Main.Ribbon.Classes
{
    using System;
    using System.ComponentModel.Design;

    using PtpChat.Main.Ribbon.Classes.Collections;
    using PtpChat.Main.Ribbon.Component_Classes;

    public class RibbonItemCollectionEditor : CollectionEditor
    {
        public RibbonItemCollectionEditor()
            : base(typeof(RibbonItemCollection))
        {
        }

        protected override Type CreateCollectionItemType()
        {
            return typeof(RibbonButton);
        }

        protected override Type[] CreateNewItemTypes()
        {
            return new[]
                       {
                           typeof(RibbonButton), typeof(RibbonButtonList), typeof(RibbonItemGroup), typeof(RibbonComboBox), typeof(RibbonSeparator), typeof(RibbonTextBox), typeof(RibbonColorChooser),
                           typeof(RibbonCheckBox), typeof(RibbonUpDown), typeof(RibbonLabel), typeof(RibbonHost)
                       };
        }
    }
}