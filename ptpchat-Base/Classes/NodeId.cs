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
			if (longId == null)
			{
				throw new ArgumentNullException("invalid longId");
			}

            if (!longId.Contains('@'))
            {
				throw new ArgumentException("Invalid longId");
            }

            var splitId = longId.Split('@');

			if(splitId.Length != 2)
			{
				throw new ArgumentException("Invalid longId");
			}

            this.Name = splitId[0];
            this.Id = Guid.Parse(splitId[1]);
        }

        public NodeId(string name, Guid Id)
        {
            this.Id = Id;
            this.Name = name;
        }

		public override bool Equals(object obj)
		{
			return obj is NodeId && ((NodeId)obj).Id == this.Id;
		}

		public bool Equals(NodeId node)
		{
			return this.Id == node.Id;
		}

		public override string ToString()
		{
			return GetWholeId();
		}

		public string GetWholeId() => this.Name + "@" + this.Id;
    }
}