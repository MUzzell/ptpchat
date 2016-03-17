namespace ptpchat.Class_Definitions
{
    using System;
    using System.ComponentModel;
    using System.Net;
    using System.Net.Sockets;

    public class SocketManager : INotifyPropertyChanged
    {
        public SocketManager()
        {
        }

        public SocketManager(IPEndPoint localEndpoint)
        {
            this.LocalEndpoint = localEndpoint;
        }

        //the node id of the guy we're connected to
        private Guid destinationNodeId;
        private IPEndPoint destinationEndpoint;

        //our node id
        private Guid localNodeId;
        private IPEndPoint localEndpoint;

        private bool isSocketListening;

        public bool IsServerConnection { get; set; }

        private DateTime lastHelloRecieved;

        public bool IsSocketListening
        {
            get { return this.isSocketListening; }
            set
            {
                if (this.isSocketListening == value)
                {
                    return;
                }

                this.isSocketListening = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("IsSocketListening"));
            }
        }
        public IPEndPoint DestinationEndpoint
        {
            get { return this.destinationEndpoint; }
            set
            {
                if (Equals(this.destinationEndpoint, value))
                {
                    return;
                }

                this.destinationEndpoint = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("DestinationEndpoint"));
            }
        }
        public IPEndPoint LocalEndpoint
        {
            get { return this.localEndpoint; }
            set
            {
                if (Equals(this.localEndpoint, value))
                {
                    return;
                }

                this.localEndpoint = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("LocalEndpoint"));
            }
        }

        public DateTime LastHelloRecieved
        {
            get { return this.lastHelloRecieved; }
            set
            {
                if (this.lastHelloRecieved == value)
                {
                    return;
                }

                this.lastHelloRecieved = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("LastHelloRecieved"));
            }
        }

        public Guid DestinationNodeId
        {
            get { return this.destinationNodeId; }
            set
            {
                if (this.destinationNodeId == value)
                {
                    return;
                }

                this.destinationNodeId = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("DestinationNodeId"));
            }
        }

        public Guid LocalNodeId
        {
            get { return this.localNodeId; }
            set
            {
                if (this.localNodeId == value)
                {
                    return;
                }

                this.localNodeId = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("NodeId"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            var handler = this.PropertyChanged;

            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}