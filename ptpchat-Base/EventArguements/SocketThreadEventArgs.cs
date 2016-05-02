namespace PtpChat.Base.EventArguements
{
	using System;
	using System.Net;
	using System.Net.Sockets;

	public class SocketThreadEventArgs : EventArgs
	{
		public IPEndPoint Destination { get; set; }
		public int? ErrorCode { get; set; }
		public SocketException Exception { get; set; }
	}
}
