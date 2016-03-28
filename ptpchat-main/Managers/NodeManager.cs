namespace PtpChat.Main.Managers
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;

    using PtpChat.Base.Classes;
    using PtpChat.Base.EventArguements;
    using PtpChat.Base.Interfaces;

    internal class NodeManager : INodeManager
    {
        public NodeManager(ILogManager logger)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger), @"Is Null");
            }

            this.logger = logger;

            this.LocalNode = new Node { NodeId = Guid.NewGuid() };
        }

        private const string LogAddedNode = "Added new node, Node ID: {0}";

        private const string LogDeletedNode = "Deleted node, Node ID: {0}";

        private const string LogUpdatedNode = "Updated node, Node ID: {0}";

        private readonly ILogManager logger;

        //Can set the 'concurrency level'? why does # of threads matter?
        private readonly ConcurrentDictionary<Guid, Node> nodes = new ConcurrentDictionary<Guid, Node>();

        public event EventHandler NodeAdd;

        public event EventHandler NodeDelete;

        public event EventHandler NodeUpdate;

        public Node LocalNode { get; }

        public void Add(Node node)
        {
            if (!this.nodes.TryAdd(node.NodeId, node))
            {
                throw new InvalidOperationException("Add, Node is already present");
            }

            this.logger.Info(string.Format(LogAddedNode, node.NodeId));

            this.NodeAdd?.Invoke(this, new NodeEventArgs { Node = node });
        }

        public Node Delete(Node node)
        {
            if (node?.NodeId == null)
            {
                throw new ArgumentNullException(nameof(node), @"node or its ID is null");
            }

            return this.Delete(node.NodeId);
        }

        public Node Delete(Guid nodeId)
        {
            if (nodeId == null || nodeId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(nodeId), @"Invalid nodeId");
            }

            Node outNode;
            if (!this.nodes.TryRemove(nodeId, out outNode))
            {
                throw new InvalidOperationException("Delete, NodeID not present");
            }

            this.logger.Info(string.Format(LogDeletedNode, outNode.NodeId));

            this.NodeDelete?.Invoke(this, new NodeEventArgs { Node = outNode });

            return outNode;
        }

        public void Update(Node node)
        {
            if (node?.NodeId == null)
            {
                throw new ArgumentNullException(nameof(node), @"node or its ID is null");
            }

            Node currentNode;

            if (!this.nodes.TryGetValue(node.NodeId, out currentNode))
            {
                throw new InvalidOperationException("Update, could not find Node");
            }

            if (!this.nodes.TryUpdate(node.NodeId, node, currentNode))
            {
                throw new InvalidOperationException("Update, unable to update node");
            }

            this.logger.Info(string.Format(LogUpdatedNode, node.NodeId));

            this.NodeUpdate?.Invoke(this, new NodeEventArgs { Node = currentNode });
        }

        public IEnumerable<Node> GetNodes(Func<KeyValuePair<Guid, Node>, bool> filter) => this.nodes.Where(filter).Select(n => n.Value);

        public IEnumerable<Node> GetNodes() => this.nodes.Select(n => n.Value);
    }
}