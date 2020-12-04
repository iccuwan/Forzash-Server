using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Forzash_Server
{
	public struct UdpState
	{
		public UdpClient client;
		public IPEndPoint endPoint;
	}
}
