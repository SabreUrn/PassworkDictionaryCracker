using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CrackerServer {
	public static class AcceptClients {
		public static bool Accepting = true;

		public static void AcceptClient(TcpListener serverSocket) {
			while(Accepting) {
				//accept new Client and add to static ClientList
				Client client = new Client(serverSocket.AcceptTcpClient(), ClientList.Count.ToString());
				ClientList.Add(client);
			}
		}
	}
}
