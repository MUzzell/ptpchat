namespace PtpChat.Main.Managers
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    using PtpChat.Base.Classes;
    using PtpChat.Base.EventArguements;
    using PtpChat.Base.Interfaces;
    using PtpChat.Utility;

    public class NodeManager : INodeManager
    {
        private const string LogAddedNode = "Added new node, Node ID: {0}";

        private const string LogDeletedNode = "Deleted node, Node ID: {0}";

        private const string LogUpdatedNode = "Updated node, Node ID: {0}";

        public static object updateLock = new object();

        private readonly ILogManager logger;

        private readonly TimeSpan NodeCutoff;

        //Can set the 'concurrency level'? why does # of threads matter?
        private readonly ConcurrentDictionary<Guid, Node> nodes = new ConcurrentDictionary<Guid, Node>();

        private readonly Timer ProcessTimer;

        public NodeManager(ILogManager logger, ConfigManager config)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger), @"Is Null");
            }

            this.logger = logger;

            this.LocalNode = new Node(new NodeId("LocalNode", config.LocalNodeId)){ Version = config.LocalNodeVersion };

            this.ProcessTimer = new Timer(this.ProcessNodes, null, 10000, 5000);

            this.NodeCutoff = config.NodeCutoff;
        }

        public event EventHandler NodeAdd;

        public event EventHandler NodeDelete;

        public event EventHandler NodeUpdate;

        public Node LocalNode { get; }

        public void Add(Node node)
        {
            if (!this.nodes.TryAdd(node.NodeId.Id, node))
            {
                throw new InvalidOperationException("Add, Node is already present");
            }

            this.NodeAdd?.Invoke(this, new NodeEventArgs { Node = node });

            this.logger.Info(string.Format(LogAddedNode, node.NodeId));
        }

        public Node Delete(Node node)
        {
            if (node?.NodeId == null)
            {
                throw new ArgumentNullException(nameof(node), @"node or its ID is null");
            }

            return this.Delete(node.NodeId.Id);
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

        public void Update(Guid nodeId, Action<Node> updateFunc)
        {
            if (nodeId == Guid.Empty || updateFunc == null)
            {
                throw new ArgumentNullException(@"nodeId or updateFunc is null");
            }

            Node currentNode, node;

            if (!this.nodes.TryGetValue(nodeId, out currentNode))
            {
                throw new InvalidOperationException("Update, could not find Node");
            }

            lock (updateLock)
            {
                node = this.nodes[nodeId];
                updateFunc(node);

                if (!this.nodes.TryUpdate(nodeId, node, currentNode))
                {
                    throw new InvalidOperationException("Update, unable to update node");
                }
            }

            this.NodeUpdate?.Invoke(this, new NodeEventArgs { Node = node });

            this.logger.Info(string.Format(LogUpdatedNode, nodeId));
        }

        public Node GetNodeForConnect(Guid nodeId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Node> GetNodes(Func<KeyValuePair<Guid, Node>, bool> filter) => this.nodes.Where(filter).Select(n => n.Value);

        public IEnumerable<Node> GetNodes() => this.nodes.Select(n => n.Value);

        private void ProcessNodes(object state)
        {
            foreach (var node in this.GetNodes(n => n.Value.IsConnected && n.Value.LastRecieve < DateTime.Now.Subtract(this.NodeCutoff)))
            {
                node.IsConnected = false;
            }
        }
    }
}