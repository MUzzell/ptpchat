using System;
using System.Collections.Generic;
using System.Linq;

namespace PtpChat.Main
{
	using Base.Classes;
	using PtpChat.Base.Interfaces;
	using System.Collections.Concurrent;

	internal class NodeManager : INodeManager
	{
		private static readonly string LogAddedNode = "Added new node, Node ID: {0}";
		private static readonly string LogDeletedNode = "Deleted node, Node ID: {0}";
		private static readonly string LogUpdatedNode = "Updated node, Node ID: {0}";

		ILogManager logger;
		
		//Can set the 'concurrency level'? why does # of threads matter?
		ConcurrentDictionary<Guid, Node> nodes = new ConcurrentDictionary<Guid, Node>();

		public Node LocalNode
		{
			get
			{
				return new Node();
			}
		}

		public NodeManager(ILogManager logger)
		{
			if (logger == null)
			{
				throw new ArgumentNullException("logger", "Is Null");
			}

			this.logger = logger;
		}

		public void Add(Node node)
		{

			if (!this.nodes.TryAdd(node.NodeId, node))
			{
				throw new InvalidOperationException("Add, Node is already present");
			}
			this.logger.Info(string.Format(NodeManager.LogAddedNode, node.NodeId));
		}

		public Node Delete(Node node)
		{
			if (node == null || node.NodeId == null)
			{
				throw new ArgumentNullException("node", "node or its ID is null");
			}
			
			return this.Delete(node.NodeId);
			
		}

		public Node Delete(Guid nodeId)
		{

			if (nodeId == null || nodeId == Guid.Empty)
			{
				throw new ArgumentNullException("nodeId", "Invalid nodeId");
			}

			Node outNode;
			if (!this.nodes.TryRemove(nodeId, out outNode))
			{
				throw new InvalidOperationException("Delete, NodeID not present");
			}

			this.logger.Info(string.Format(NodeManager.LogDeletedNode, outNode.NodeId));
			return outNode;
		}

		public void Update(Node node)
		{
			if (node == null || node.NodeId == null)
			{
				throw new ArgumentNullException("node", "node or its ID is null");
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

			this.logger.Info(string.Format(NodeManager.LogUpdatedNode, node.NodeId));

		}
		
		public IList<Node> GetNodes(Dictionary<NodeFilterType, object> filter = null)
		{

			if (filter == null)
			{
				return (IList<Node>)this.nodes.ToList();
			}

			IList<Node> nodeList = new List<Node>();

			foreach (KeyValuePair<Guid, Node> kv in this.nodes.TakeWhile(kv => this.matches(kv.Value, filter)))
			{
				nodeList.Add(kv.Value);
			}

			return nodeList;
			
		}
		
		private bool matches(Node node, Dictionary<NodeFilterType, object> filter)
		{
			Node outNode = node;

			if (filter.ContainsKey(NodeFilterType.NodeId))
			{
				Guid nodeId = (Guid)filter[NodeFilterType.NodeId];

				if (node.NodeId != nodeId)
					outNode = null;
			}

			return outNode != null;
		}

		
	}
}
