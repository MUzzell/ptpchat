﻿namespace PtpChat.Base.Interfaces
{
	using PtpChat.Base.Classes;
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// Handles the management of Nodes inside this program
	/// </summary>
	public interface INodeManager
    {
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

		/// <summary>
		/// Get a list of Nodes that match the provided filter.
		/// <param name="filter">A Dictionary that matches </param>
		/// </summary>
		IList<Node> GetNodes(Dictionary<NodeFilterType, object> filter);
    }

	public enum NodeFilterType
	{
		NodeId
	}
}