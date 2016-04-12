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

        event EventHandler NodeAdd;

        event EventHandler NodeDelete;

        event EventHandler NodeUpdate;

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

		/*
        /// <summary>
        /// Get a single node that would be suitable for sending a CONNECT Message 
        /// </summary>
        /// <param name="nodeId">The ID of the node we wish to send a CONNECT to.</param>
        /// <returns>The Node that is considered siutable.</returns>
        Node GetNodeForConnect(Guid nodeId);
		*/

        /// <summary>
        /// Updates the node which has the given nodeId, using the funtion provided to edit its attributs.
        /// Throws an ArgumentException if unable to do so, such as if the Node does not exists.
        /// <param name="nodeId">The id of the node to be updated.</param>
        /// <param name="updateFunc">The function that will edit the parameters of the target node.</param>
        /// </summary>
        void Update(Guid nodeId, Action<Node> updateFunc);

        IEnumerable<Node> GetNodes();

        IEnumerable<Node> GetNodes(Func<KeyValuePair<Guid, Node>, bool> filter);
    }
}