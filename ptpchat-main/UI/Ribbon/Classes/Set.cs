namespace PtpChat.Main.Ribbon.Classes
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// There is no HashSet&lt;T&gt; available in .net 2.0.
    /// </summary>
    /// <typeparam name="T">Der Typ des Sets</typeparam>
    [Serializable]
    public class Set<T> : ICollection<T>, IEnumerable<T>, IEnumerable
    {
        private readonly Dictionary<T, object> _items = new Dictionary<T, object>();

        public void AddRange(IEnumerable<T> items)
        {
            if (items == null)
            {
                return;
            }
            foreach (var item in items)
            {
                this.Add(item);
            }
        }

        public T[] ToArray()
        {
            var array = new T[this._items.Count];
            this.CopyTo(array, 0);
            return array;
        }

        #region ICollection<T>

        public void Add(T item)
        {
            if (item == null)
            {
                return;
            }
            this._items[item] = null;
        }

        public void Clear()
        {
            this._items.Clear();
        }

        public bool Contains(T item)
        {
            if (item == null)
            {
                return false;
            }
            return this._items.ContainsKey(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            this._items.Keys.CopyTo(array, arrayIndex);
        }

        public int Count => this._items.Count;

        public bool IsReadOnly => false;

        public bool Remove(T item)
        {
            if (item == null)
            {
                return false;
            }
            return this._items.Remove(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this._items.Keys.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this._items.Keys.GetEnumerator();
        }

        #endregion
    }
}