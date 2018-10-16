using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CrackerServer {
	/// <summary>
	/// Solely responsible for accepting new clients separate from the main thread.
	/// </summary>
	public static class AcceptClients {
		/// <summary>
		/// Accepts a new client through the server socket.
		/// </summary>
		/// <param name="serverSocket">The socket to listen to.</param>
		public static void AcceptClient(TcpListener serverSocket) {
			//accept new client
			while(true) {
				Client client = new Client(serverSocket.AcceptTcpClient(), ClientList.Count.ToString());
			}
		}
	}
}
