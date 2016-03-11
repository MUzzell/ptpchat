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
        public SocketManager(IPEndPoint localEndpoint, UdpClient updClient)
        {
            this.LocalEndpoint = localEndpoint;
            this.UdpClient = updClient;
        }

        private IPEndPoint destinationEndpoint;
        private IPEndPoint localEndpoint;

        private bool isSocketListening;

        private DateTime lastHelloRecieved;

        //the node id of the guy we're connected to
        private Guid nodeId;

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

        public UdpClient UdpClient { get; set; }

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

        public Guid NodeId
        {
            get { return this.nodeId; }
            set
            {
                if (this.nodeId == value)
                {
                    return;
                }

                this.nodeId = value;
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