namespace PtpChat.Net
{
    using System;
    using System.ComponentModel;
    using System.Net;

    public class SocketManager : INotifyPropertyChanged
    {
        private IPEndPoint destinationEndpoint;

        //the node id of the guy we're connected to
        private Guid destinationNodeId;

        private bool isSocketListening;

        private DateTime lastHelloRecieved;

        private IPEndPoint localEndpoint;

        //our node id
        private Guid localNodeId;

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

        public bool IsServerConnection { get; set; }

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

        public SocketManager()
        {
        }

        public SocketManager(IPEndPoint localEndpoint)
        {
            this.LocalEndpoint = localEndpoint;
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