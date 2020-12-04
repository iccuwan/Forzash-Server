using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Numerics;
using System.Text;

namespace Forzash_Server
{
	class Player
	{
		public UdpState udpState;
		public string name;
		public Vector3 position;
		public bool connectedToServer = false;

		public void SendMessage(string message)
		{
			if (!udpState.client.Client.Connected)
			{
				udpState.client.Connect(udpState.endPoint);
				connectedToServer = true;
			}
			byte[] sendBytes = Encoding.ASCII.GetBytes(message);
			try
			{
				udpState.client.Send(sendBytes, sendBytes.Length);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}
	};
}
