namespace PtpChat.Base.Interfaces
{
    using System;
    using System.Collections.Generic;

    using PtpChat.Base.Classes;

    /// <summary>
    /// Handles the management of Nodes inside this program
    /// </summary>
    public interface INodeManager
    {
        /// <summary>
        /// This attribute represents this node.
        /// </summary>
        Node LocalNode { get; }

        /// <summary>
        /// Attempt to add the given Node.
        /// Throws an ArgumentException if unable to do so, such as if the nodeId already exists.
        /// <param name="node">The new node to be added, will be compared against its NodeId.</param>
        /// </summary>
        void Add(Node node);

        /// <summary>
        /// Attempt to delete the given Node.
        /// Throws an ArgumentException if unable to do so, such as if the nodeId does not exists.
        /// <param name="node">The node to be deleted, will be compared against its NodeId.</param>
        /// </summary>
        Node Delete(Node node);

        /// <summary>
        /// Attempt to delete the given Node, using the given nodeId.
        /// Throws an ArgumentException if unable to do so, such as if the nodeId does not exists.
        /// <param name="nodeId">The nodeId of the Node to be deleted.</param>
        /// </summary>
        Node Delete(Guid nodeId);

        /// <summary>
        /// Update the given node, replacing the internal node with this one.
        /// Throws an ArgumentException if unable to do so, such as if the Node does not exists.
        /// <param name="node">The node to be updates, will be compared against its NodeId.</param>
        /// </summary>
        void Update(Node node);
		
        IEnumerable<Node> GetNodes();
        IEnumerable<Node> GetNodes(Func<KeyValuePair<Guid, Node>, bool> filter);
    }

    public enum NodeFilterType
    {
        NodeId
    }
}