namespace PtpChat.Utility
{
    using System;
    using System.Collections.Generic;

    public class PtpList<T> : List<T>
    {
        public event EventHandler<T> OnAdd;

        public event EventHandler<T> OnRemove;

        public new void Add(T item)
        {
            this.OnAdd?.Invoke(this, item);
            base.Add(item);
        }

        public new void Remove(T item)
        {
            this.OnRemove?.Invoke(this, item);
            base.Remove(item);
        }
    }
}