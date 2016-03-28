namespace PtpChat.Main.Ribbon.Classes.Collections
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    using PtpChat.Main.Ribbon.Classes.Enums;
    using PtpChat.Main.Ribbon.Component_Classes;

    public class RibbonButtonCollection : RibbonItemCollection
    {
        internal RibbonButtonCollection(RibbonButtonList list)
        {
            this.OwnerList = list;
        }

        /// <summary>
        /// Gets the list that owns the collection (If any)
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RibbonButtonList OwnerList { get; }

        /// <summary>
        /// Checks for the restrictions that buttons should have on the RibbonButton List
        /// </summary>
        /// <param name="button"></param>
        private void CheckRestrictions(RibbonButton button)
        {
            if (button == null)
            {
                throw new ApplicationException("The RibbonButtonList only accepts button in the Buttons collection");
            }

            //if (!string.IsNullOrEmpty(button.Text))
            //    throw new ApplicationException("The buttons on the RibbonButtonList should have no text");

            if (button.Style != RibbonButtonStyle.Normal)
            {
                throw new ApplicationException("The only style supported by the RibbonButtonList is Normal");
            }
        }

        /// <summary>
        /// Adds the specified item to the collection
        /// </summary>
        public override void Add(RibbonItem item)
        {
            this.CheckRestrictions(item as RibbonButton);

            item.SetOwner(this.Owner);
            item.SetOwnerPanel(this.OwnerPanel);
            item.SetOwnerTab(this.OwnerTab);
            item.SetOwnerItem(this.OwnerList);

            item.Click += this.OwnerList.item_Click;

            base.Add(item);
        }

        /// <summary>
        /// Adds the specified range of items
        /// </summary>
        /// <param name="items">Items to add</param>
        public override void AddRange(IEnumerable<RibbonItem> items)
        {
            foreach (var item in items)
            {
                this.CheckRestrictions(item as RibbonButton);

                item.SetOwner(this.Owner);
                item.SetOwnerPanel(this.OwnerPanel);
                item.SetOwnerTab(this.OwnerTab);
                item.SetOwnerItem(this.OwnerList);
            }

            base.AddRange(items);
        }

        /// <summary>
        /// Inserts the specified item at the desired index
        /// </summary>
        /// <param name="index">Desired index of the item</param>
        /// <param name="item">Item to insert</param>
        public override void Insert(int index, RibbonItem item)
        {
            this.CheckRestrictions(item as RibbonButton);

            item.SetOwner(this.Owner);
            item.SetOwnerPanel(this.OwnerPanel);
            item.SetOwnerTab(this.OwnerTab);
            item.SetOwnerItem(this.OwnerList);

            base.Insert(index, item);
        }
    }
}