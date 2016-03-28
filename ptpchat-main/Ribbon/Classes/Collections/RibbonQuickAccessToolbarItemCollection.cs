namespace PtpChat.Main.Ribbon.Classes.Collections
{
    using System.Collections.Generic;

    using PtpChat.Main.Ribbon.Classes.Enums;
    using PtpChat.Main.Ribbon.Component_Classes;

    public class RibbonQuickAccessToolbarItemCollection : RibbonItemCollection
    {
        /// <summary>
        /// Creates a new collection
        /// </summary>
        /// <param name="ownerGroup"></param>
        internal RibbonQuickAccessToolbarItemCollection(RibbonQuickAccessToolbar toolbar)
        {
            this.OwnerToolbar = toolbar;
            this.SetOwner(toolbar.Owner);
        }

        #region Fields

        #endregion

        /// <summary>
        /// Gets the group that owns this item collection
        /// </summary>
        public RibbonQuickAccessToolbar OwnerToolbar { get; }

        /// <summary>
        /// Adds the specified item to the collection
        /// </summary>
        public override void Add(RibbonItem item)
        {
            item.MaxSizeMode = RibbonElementSizeMode.Compact;
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
                item.MaxSizeMode = RibbonElementSizeMode.Compact;
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
            item.MaxSizeMode = RibbonElementSizeMode.Compact;
            base.Insert(index, item);
        }
    }
}