namespace PtpChat.Main.Ribbon.Classes.Collections
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    using PtpChat.Main.Ribbon.Component_Classes;

    /// <summary>
    /// Represents a collection of RibbonPanel objects
    /// </summary>
    public sealed class RibbonPanelCollection : List<RibbonPanel>
    {
        /// <summary>
        /// Creates a new RibbonPanelCollection
        /// </summary>
        /// <param name="ownerTab">RibbonTab that contains this panel collection</param>
        /// <exception cref="ArgumentNullException">ownerTab is null</exception>
        public RibbonPanelCollection(RibbonTab ownerTab)
        {
            if (ownerTab == null)
            {
                throw new ArgumentNullException("ownerTab");
            }

            this.OwnerTab = ownerTab;
        }

        /// <summary>
        /// Gets the Ribbon that contains this panel collection
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Ribbon Owner => this.OwnerTab.Owner;

        /// <summary>
        /// Gets the RibbonTab that contains this panel collection
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RibbonTab OwnerTab { get; private set; }

        /// <summary>
        /// Adds the specified item to the collection
        /// </summary>
        public new void Add(RibbonPanel item)
        {
            item.SetOwner(this.Owner);
            item.SetOwnerTab(this.OwnerTab);
            base.Add(item);
        }

        /// <summary>
        /// Adds a range of panels to the collection
        /// </summary>
        /// <param name="items">Panels to add</param>
        public new void AddRange(IEnumerable<RibbonPanel> items)
        {
            foreach (var p in items)
            {
                p.SetOwner(this.Owner);
                p.SetOwnerTab(this.OwnerTab);
            }

            base.AddRange(items);
        }

        /// <summary>
        /// Inserts the specified panel at the desired index
        /// </summary>
        /// <param name="index">Desired index to insert the panel</param>
        /// <param name="item">Panel to insert</param>
        public new void Insert(int index, RibbonPanel item)
        {
            item.SetOwner(this.Owner);
            item.SetOwnerTab(this.OwnerTab);
            base.Insert(index, item);
        }

        /// <summary>
        /// Sets the value of the Owner Property
        /// </summary>
        internal void SetOwner(Ribbon owner)
        {
            foreach (var panel in this)
            {
                panel.SetOwner(owner);
            }
        }

        /// <summary>
        /// Sets the value of the OwnerTab Property
        /// </summary>
        /// <param name="onwerTab"></param>
        internal void SetOwnerTab(RibbonTab ownerTab)
        {
            this.OwnerTab = ownerTab;

            foreach (var panel in this)
            {
                panel.SetOwnerTab(this.OwnerTab);
            }
        }
    }
}