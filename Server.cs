using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Forzash_Server
{
	class Server
	{
		private Dictionary<Guid, Player> players;
		private Settings settings;

		private UdpState listener;
		private bool stopServer = false;
		
		public Server()
		{
			settings = new Settings();
			players = new Dictionary<Guid, Player>(settings.MaxSlots);

			IPEndPoint e = new IPEndPoint(IPAddress.Any, settings.Port);
			UdpClient c = new UdpClient(e);
			listener = new UdpState
			{
				endPoint = e,
				client = c

			};
			StartServer();
		}

		private void StartServer()
		{
			Console.WriteLine("Waiting for connections...");
			listener.client.BeginReceive(new AsyncCallback(ReceiveCallback), listener);
			while (!stopServer)
			{

				Thread.Sleep(100);
			}
		}

		private void ReceiveCallback(IAsyncResult result)
		{
			UdpClient c = ((UdpState)(result.AsyncState)).client;
			IPEndPoint e = ((UdpState)(result.AsyncState)).endPoint;
			Player p;

			byte[] receiveBytes = c.EndReceive(result, ref e);
			string receiveString = Encoding.ASCII.GetString(receiveBytes);

			Console.WriteLine($"Received: {receiveString}");

			string[] args = receiveString.Split(':');
			string cmd = args[0].ToUpperInvariant();

			switch (cmd)
			{
				case "CONNECT":
					ConnectPlayer(c, e, args[1]);
					break;
				default:
					Console.WriteLine($"Get incorrect command {receiveString} from client {e}");
					break;
			}

			listener.client.BeginReceive(new AsyncCallback(ReceiveCallback), listener);
		}

		private void ConnectPlayer(UdpClient c, IPEndPoint e, string name)
		{
			Player p = new Player
			{
				udpState = new UdpState
				{
					client = c,
					endPoint = e
				},
				name = name
			};
			Guid guid = Guid.NewGuid();
			players.Add(guid, p);

			Console.WriteLine($"Player {name} with connected and get id {guid}");

			string response = $"CONNECTGOOD:{guid}";
			p.SendMessage(response);
		}

	}
}
