namespace ptpchat.Client_Class
{
    using System;
    using System.Collections.Generic;

    public class PtpList<T> : List<T>
    {
        public event EventHandler<T> OnAdd;

        public event EventHandler<T> OnRemove;

        public new void Add(T item)
        {
            if (this.OnAdd != null)
            {
                this.OnAdd(this, item);
            }

            base.Add(item);
        }

        public new void Remove(T item)
        {
            if (this.OnRemove != null)
            {
                this.OnRemove(this, item);
            }

            base.Remove(item);
        }
    }
}