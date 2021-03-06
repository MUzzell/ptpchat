﻿namespace PtpChat.Net
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading.Tasks;

    using PtpChat.Base.Interfaces;

    public class SocketHandler : ISocketHandler
    {
        private const string LogPortBound = "SocketHandler has bound to 0.0.0.0:{0}";
        private const string EndpointRefused = "Could not connect to {0} because the target machine refused";

        //private readonly UdpClient localClient;

        //im holding onto this until i figure out how to do the rest of the socket stuff.
        private readonly int localPort;

        private readonly ILogManager logger;
        private readonly INodeManager nodeManager;
        private readonly TcpListener localListener;

		public event EventHandler SocketConnected;
		public event EventHandler SocketDisconnected;
		public event EventHandler SocketReset;
		
        //threads are organised by their port.
        public IDictionary<string, SocketThread> InternalThreads { get; }

        public IPEndPoint LocalEndpoint { get; }

        public SocketHandler(ILogManager logger, IDataManager dataManager, IMessageHandler messageHandler)
        {
            this.logger = logger;
            this.logger.Info(string.Format(LogPortBound, this.localPort));

            this.InternalThreads = new Dictionary<string, SocketThread>();

            //What if it's in use??
            this.localPort = new Random().Next(10000, 65535);
            //this.localClient = new UdpClient(this.localPort, AddressFamily.InterNetwork);

            dataManager.NodeManager.LocalNode.Port = this.localPort;
            this.nodeManager = dataManager.NodeManager;
			
            //this.InternalThreads.Add(this.localPort, new SocketThread(this.localClient, this.localListener, messageHandler, this.logger));
        }

        public void AddSocketThread(IPEndPoint destination, IMessageHandler messageHandler)
        {
			var thread = new SocketThread(
					destination,
					messageHandler,
					this.logger);

			thread.SocketConnected += this.SocketConnected;
			thread.SocketDisconnected += this.SocketDisconnected;
			thread.SocketReset += this.SocketReset;

			this.InternalThreads.Add(destination.Address.ToString(), thread);
        }
		
        public bool SendMessage(Guid dstNodeId, byte[] message)
        {
            var node = this.nodeManager.GetNodes(a => a.Value.NodeId.Id == dstNodeId).FirstOrDefault();

            return node != null && this.SendMessage(node.IpEndPoint, null, message);
        }

        public bool SendMessage(IPEndPoint dst, IPEndPoint src, byte[] message)
        {
            try
            {
                this.InternalThreads[dst.Address.ToString()].Send(message);
            }
            catch (SocketException ex)
            {
                this.logger.Error(string.Format(EndpointRefused, dst));

                return false;
            }

            return true;
        }

        public void StartListening()
        {
            foreach (var thread in this.InternalThreads.Values)
            {
                Task.Run(() => thread.Listen());
            }
        }

        public void StopListening()
        {
            foreach (var thread in this.InternalThreads.Values)
            {
                //stop the listeners
                new Task(() => thread.Stop()).Start();
            }
        }
    }
}