namespace PtpChat.Main.Ribbon.Classes.Collections
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    using PtpChat.Main.Ribbon.Component_Classes;

    /// <summary>
    /// Represents a collection of RibbonTab objects
    /// </summary>
    public sealed class RibbonTabCollection : List<RibbonTab>
    {
        /// <summary>
        /// Creates a new RibbonTabCollection
        /// </summary>
        /// <param name="owner">|</param>
        /// <exception cref="AgrumentNullException">owner is null</exception>
        internal RibbonTabCollection(Ribbon owner)
        {
            if (owner == null)
            {
                throw new ArgumentNullException("null");
            }

            this.Owner = owner;
        }

        /// <summary>
        /// Gets the Ribbon that owns this tab
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Ribbon Owner { get; private set; }

        /// <summary>
        /// Adds the specified item to the collection
        /// </summary>
        /// <param name="item">Item to add to the collection</param>
        public new void Add(RibbonTab item)
        {
            item.SetOwner(this.Owner);
            base.Add(item);

            this.Owner.OnRegionsChanged();
        }

        /// <summary>
        /// Adds the specified items to the collection
        /// </summary>
        /// <param name="items">Items to add to the collection</param>
        public new void AddRange(IEnumerable<RibbonTab> items)
        {
            foreach (var tab in items)
            {
                tab.SetOwner(this.Owner);
            }

            base.AddRange(items);

            this.Owner.OnRegionsChanged();
        }

        /// <summary>
        /// Inserts the specified item into the specified index
        /// </summary>
        /// <param name="index">Desired index of the item into the collection</param>
        /// <param name="item">Tab to be inserted</param>
        public new void Insert(int index, RibbonTab item)
        {
            item.SetOwner(this.Owner);

            base.Insert(index, item);

            this.Owner.OnRegionsChanged();
        }

        public new void Remove(RibbonTab context)
        {
            base.Remove(context);
            this.Owner.OnRegionsChanged();
        }

        public new int RemoveAll(Predicate<RibbonTab> predicate)
        {
            throw new ApplicationException("RibbonTabCollection.RemoveAll function is not supported");
        }

        public new void RemoveAt(int index)
        {
            base.RemoveAt(index);
            this.Owner.OnRegionsChanged();
        }

        public new void RemoveRange(int index, int count)
        {
            base.RemoveRange(index, count);
            this.Owner.OnRegionsChanged();
        }

        /// <summary>
        /// Sets the value of the Owner Property
        /// </summary>
        internal void SetOwner(Ribbon owner)
        {
            this.Owner = owner;
        }
    }
}