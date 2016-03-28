namespace PtpChat.Main.Ribbon.Classes.Collections
{
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Design;

    using PtpChat.Main.Ribbon.Component_Classes;

    [Editor("System.Windows.Forms.RibbonItemCollectionEditor", typeof(UITypeEditor))]
    public class RibbonItemCollection : List<RibbonItem>, IList
    {
        #region Ctor

        /// <summary>
        /// Creates a new ribbon item collection
        /// </summary>
        internal RibbonItemCollection()
        {
        }

        #endregion

        #region Fields

        #endregion

        #region Properties

        /// <summary>
        /// Gets the Ribbon owner of this collection
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Ribbon Owner { get; private set; }

        /// <summary>
        /// Gets the RibbonPanel where this item is located
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RibbonPanel OwnerPanel { get; private set; }

        /// <summary>
        /// Gets the RibbonTab that contains this item
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RibbonTab OwnerTab { get; private set; }

        #endregion

        #region Overrides

        /// <summary>
        /// Adds the specified item to the collection
        /// </summary>
        public new virtual void Add(RibbonItem item)
        {
            item.SetOwner(this.Owner);
            item.SetOwnerPanel(this.OwnerPanel);
            item.SetOwnerTab(this.OwnerTab);

            base.Add(item);
        }

        /// <summary>
        /// Adds the specified range of items
        /// </summary>
        /// <param name="items">Items to add</param>
        public new virtual void AddRange(IEnumerable<RibbonItem> items)
        {
            foreach (var item in items)
            {
                item.SetOwner(this.Owner);
                item.SetOwnerPanel(this.OwnerPanel);
                item.SetOwnerTab(this.OwnerTab);
            }

            base.AddRange(items);
        }

        /// <summary>
        /// Inserts the specified item at the desired index
        /// </summary>
        /// <param name="index">Desired index of the item</param>
        /// <param name="item">Item to insert</param>
        public new virtual void Insert(int index, RibbonItem item)
        {
            item.SetOwner(this.Owner);
            item.SetOwnerPanel(this.OwnerPanel);
            item.SetOwnerTab(this.OwnerTab);

            base.Insert(index, item);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the left of items as a group of shapes
        /// </summary>
        /// <returns></returns>
        internal int GetItemsLeft(IEnumerable<RibbonItem> items)
        {
            if (this.Count == 0)
            {
                return 0;
            }

            var min = int.MaxValue;

            foreach (var item in items)
            {
                if (item.Bounds.X < min)
                {
                    min = item.Bounds.X;
                }
            }

            return min;
        }

        /// <summary>
        /// Gets the right of items as a group of shapes
        /// </summary>
        /// <returns></returns>
        internal int GetItemsRight(IEnumerable<RibbonItem> items)
        {
            if (this.Count == 0)
            {
                return 0;
            }

            var max = int.MinValue;
            ;

            foreach (var item in items)
            {
                if (item.Bounds.Right > max)
                {
                    max = item.Bounds.Right;
                }
            }

            return max;
        }

        /// <summary>
        /// Gets the top of items as a group of shapes
        /// </summary>
        /// <returns></returns>
        internal int GetItemsTop(IEnumerable<RibbonItem> items)
        {
            if (this.Count == 0)
            {
                return 0;
            }

            var min = int.MaxValue;

            foreach (var item in items)
            {
                if (item.Bounds.Y < min)
                {
                    min = item.Bounds.Y;
                }
            }

            return min;
        }

        /// <summary>
        /// Gets the bottom of items as a group of shapes
        /// </summary>
        /// <returns></returns>
        internal int GetItemsBottom(IEnumerable<RibbonItem> items)
        {
            if (this.Count == 0)
            {
                return 0;
            }

            var max = int.MinValue;

            foreach (var item in items)
            {
                if (item.Bounds.Bottom > max)
                {
                    max = item.Bounds.Bottom;
                }
            }

            return max;
        }

        /// <summary>
        /// Gets the width of items as a group of shapes
        /// </summary>
        /// <returns></returns>
        internal int GetItemsWidth(IEnumerable<RibbonItem> items)
        {
            return this.GetItemsRight(items) - this.GetItemsLeft(items);
        }

        /// <summary>
        /// Gets the height of items as a group of shapes
        /// </summary>
        /// <returns></returns>
        internal int GetItemsHeight(IEnumerable<RibbonItem> items)
        {
            return this.GetItemsBottom(items) - this.GetItemsTop(items);
        }

        /// <summary>
        /// Gets the bounds of items as a group of shapes
        /// </summary>
        /// <returns></returns>
        internal Rectangle GetItemsBounds(IEnumerable<RibbonItem> items)
        {
            return Rectangle.FromLTRB(this.GetItemsLeft(items), this.GetItemsTop(items), this.GetItemsRight(items), this.GetItemsBottom(items));
        }

        /// <summary>
        /// Gets the left of items as a group of shapes
        /// </summary>
        /// <returns></returns>
        internal int GetItemsLeft()
        {
            if (this.Count == 0)
            {
                return 0;
            }

            var min = int.MaxValue;

            foreach (var item in this)
            {
                if (item.Bounds.X < min)
                {
                    min = item.Bounds.X;
                }
            }

            return min;
        }

        /// <summary>
        /// Gets the right of items as a group of shapes
        /// </summary>
        /// <returns></returns>
        internal int GetItemsRight()
        {
            if (this.Count == 0)
            {
                return 0;
            }

            var max = int.MinValue;
            ;

            foreach (var item in this)
            {
                if (item.Visible && item.Bounds.Right > max)
                {
                    max = item.Bounds.Right;
                }
            }
            if (max == int.MinValue)
            {
                max = 0;
            }

            return max;
        }

        /// <summary>
        /// Gets the top of items as a group of shapes
        /// </summary>
        /// <returns></returns>
        internal int GetItemsTop()
        {
            if (this.Count == 0)
            {
                return 0;
            }

            var min = int.MaxValue;

            foreach (var item in this)
            {
                if (item.Bounds.Y < min)
                {
                    min = item.Bounds.Y;
                }
            }

            return min;
        }

        /// <summary>
        /// Gets the bottom of items as a group of shapes
        /// </summary>
        /// <returns></returns>
        internal int GetItemsBottom()
        {
            if (this.Count == 0)
            {
                return 0;
            }

            var max = int.MinValue;

            foreach (var item in this)
            {
                if (item.Visible && item.Bounds.Bottom > max)
                {
                    max = item.Bounds.Bottom;
                }
            }
            if (max == int.MinValue)
            {
                max = 0;
            }

            return max;
        }

        /// <summary>
        /// Gets the width of items as a group of shapes
        /// </summary>
        /// <returns></returns>
        internal int GetItemsWidth()
        {
            return this.GetItemsRight() - this.GetItemsLeft();
        }

        /// <summary>
        /// Gets the height of items as a group of shapes
        /// </summary>
        /// <returns></returns>
        internal int GetItemsHeight()
        {
            return this.GetItemsBottom() - this.GetItemsTop();
        }

        /// <summary>
        /// Gets the bounds of items as a group of shapes
        /// </summary>
        /// <returns></returns>
        internal Rectangle GetItemsBounds()
        {
            return Rectangle.FromLTRB(this.GetItemsLeft(), this.GetItemsTop(), this.GetItemsRight(), this.GetItemsBottom());
        }

        /// <summary>
        /// Moves the bounds of items as a group of shapes
        /// </summary>
        /// <param name="p"></param>
        internal void MoveTo(Point p)
        {
            this.MoveTo(this, p);
        }

        /// <summary>
        /// Moves the bounds of items as a group of shapes
        /// </summary>
        /// <param name="p"></param>
        internal void MoveTo(IEnumerable<RibbonItem> items, Point p)
        {
            var oldBounds = this.GetItemsBounds(items);

            foreach (var item in items)
            {
                var dx = item.Bounds.X - oldBounds.Left;
                var dy = item.Bounds.Y - oldBounds.Top;

                item.SetBounds(new Rectangle(new Point(p.X + dx, p.Y + dy), item.Bounds.Size));
            }
        }

        /// <summary>
        /// Centers the items on the specified rectangle
        /// </summary>
        /// <param name="rectangle"></param>
        internal void CenterItemsInto(Rectangle rectangle)
        {
            this.CenterItemsInto(this, rectangle);
        }

        /// <summary>
        /// Centers the items vertically on the specified rectangle
        /// </summary>
        /// <param name="rectangle"></param>
        internal void CenterItemsVerticallyInto(Rectangle rectangle)
        {
            this.CenterItemsVerticallyInto(this, rectangle);
        }

        /// <summary>
        /// Centers the items horizontally on the specified rectangle
        /// </summary>
        /// <param name="rectangle"></param>
        internal void CenterItemsHorizontallyInto(Rectangle rectangle)
        {
            this.CenterItemsHorizontallyInto(this, rectangle);
        }

        /// <summary>
        /// Centers the items on the specified rectangle
        /// </summary>
        /// <param name="rectangle"></param>
        internal void CenterItemsInto(IEnumerable<RibbonItem> items, Rectangle rectangle)
        {
            var x = rectangle.Left + (rectangle.Width - this.GetItemsWidth()) / 2;
            var y = rectangle.Top + (rectangle.Height - this.GetItemsHeight()) / 2;

            this.MoveTo(items, new Point(x, y));
        }

        /// <summary>
        /// Centers the items vertically on the specified rectangle
        /// </summary>
        /// <param name="rectangle"></param>
        internal void CenterItemsVerticallyInto(IEnumerable<RibbonItem> items, Rectangle rectangle)
        {
            var x = this.GetItemsLeft(items);
            var y = rectangle.Top + (rectangle.Height - this.GetItemsHeight(items)) / 2;

            this.MoveTo(items, new Point(x, y));
        }

        /// <summary>
        /// Centers the items horizontally on the specified rectangle
        /// </summary>
        /// <param name="rectangle"></param>
        internal void CenterItemsHorizontallyInto(IEnumerable<RibbonItem> items, Rectangle rectangle)
        {
            var x = rectangle.Left + (rectangle.Width - this.GetItemsWidth(items)) / 2;
            var y = this.GetItemsTop(items);

            this.MoveTo(items, new Point(x, y));
        }

        /// <summary>
        /// Sets the owner Ribbon of the collection
        /// </summary>
        /// <param name="owner"></param>
        internal void SetOwner(Ribbon owner)
        {
            this.Owner = owner;

            foreach (var item in this)
            {
                item.SetOwner(owner);
            }
        }

        /// <summary>
        /// Sets the owner Tab of the collection
        /// </summary>
        /// <param name="tab"></param>
        internal void SetOwnerTab(RibbonTab tab)
        {
            this.OwnerTab = tab;

            foreach (var item in this)
            {
                item.SetOwnerTab(tab);
            }
        }

        /// <summary>
        /// Sets the owner panel of the collection
        /// </summary>
        /// <param name="panel"></param>
        internal void SetOwnerPanel(RibbonPanel panel)
        {
            this.OwnerPanel = panel;

            foreach (var item in this)
            {
                item.SetOwnerPanel(panel);
            }
        }

        #endregion

        #region IList

        int IList.Add(object item)
        {
            var ri = item as RibbonItem;
            if (ri == null)
            {
                return -1;
            }

            this.Add(ri);
            return this.Count - 1;
        }

        void IList.Insert(int index, object item)
        {
            var ri = item as RibbonItem;
            if (ri == null)
            {
                return;
            }

            this.Insert(index, ri);
        }

        #endregion
    }
}