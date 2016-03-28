namespace PtpChat.Main.Ribbon.Classes.Collections
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    using PtpChat.Main.Ribbon.Component_Classes;

    /// <summary>
    /// Represents a collection of RibbonTabContext
    /// </summary>
    public sealed class RibbonContextCollection : List<RibbonContext>
    {
        /// <summary>
        /// Creates a new RibbonTabContext Collection
        /// </summary>
        /// <param name="owner">Ribbon that owns this collection</param>
        /// <exception cref="ArgumentNullException">owner is null</exception>
        internal RibbonContextCollection(Ribbon owner)
        {
            if (owner == null)
            {
                throw new ArgumentNullException("owner");
            }
            this.Owner = owner;
        }

        /// <summary>
        /// Gets the Ribbon that owns this collection
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Ribbon Owner { get; private set; }

        /// <summary>
        /// Adds the specified context to the collection
        /// </summary>
        /// <param name="item">Item to add to the collection</param>
        public new void Add(RibbonContext item)
        {
            item.SetOwner(this.Owner);
            this.Owner.Tabs.AddRange(item.Tabs);
            base.Add(item);
        }

        /// <summary>
        /// Adds the specified contexts to the collection
        /// </summary>
        /// <param name="items">Items to add to the collection</param>
        public new void AddRange(IEnumerable<RibbonContext> items)
        {
            foreach (var c in items)
            {
                c.SetOwner(this.Owner);

                this.Owner.Tabs.AddRange(c.Tabs);
            }

            base.AddRange(items);
        }

        /// <summary>
        /// Inserts the specified context into the specified index
        /// </summary>
        /// <param name="index">Desired index of the item into the collection</param>
        /// <param name="item">Tab to be inserted</param>
        public new void Insert(int index, RibbonContext item)
        {
            item.SetOwner(this.Owner);

            this.Owner.Tabs.InsertRange(index, item.Tabs);

            base.Insert(index, item);
        }

        public new void Remove(RibbonContext context)
        {
            base.Remove(context);

            foreach (var tab in context.Tabs)
            {
                this.Owner.Tabs.Remove(tab);
            }
        }

        public new int RemoveAll(Predicate<RibbonContext> predicate)
        {
            throw new ApplicationException("RibbonContextCollectin.RemoveAll function is not supported");
        }

        public new void RemoveAt(int index)
        {
            base.RemoveAt(index);

            var ctx = this[index];

            foreach (var tab in ctx.Tabs)
            {
                this.Owner.Tabs.Remove(tab);
            }
        }

        public new void RemoveRange(int index, int count)
        {
            throw new ApplicationException("RibbonContextCollection.RemoveRange function is not supported");
        }

        /// <summary>
        /// Sets the value of the Owner Property
        /// </summary>
        internal void SetOwner(Ribbon owner)
        {
            if (owner == null)
            {
                throw new ArgumentNullException("owner");
            }

            this.Owner = owner;
        }
    }
}