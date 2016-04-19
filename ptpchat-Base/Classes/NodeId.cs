namespace PtpChat.Base.Classes
{
    using System;
    using System.Linq;

    public class NodeId
    {
        public Guid Id { get; }

        public string Name { get; }

        public NodeId(string longId)
        {
            if (!longId.Contains('@'))
            {
                return;
            }

            var splitid = longId.Split('@');

            this.Name = splitid[0];
            this.Id = Guid.Parse(splitid[1]);
        }

        public NodeId(string name, Guid Id)
        {
            this.Id = Id;
            this.Name = name;
        }

        public string GetWholeId() => this.Name + "@" + this.Id;
    }
}