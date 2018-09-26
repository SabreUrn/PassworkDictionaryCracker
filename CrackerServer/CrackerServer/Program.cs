using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CrackerServer {
	class Program {
		static void Main(string[] args) {
			TcpListener serverSocket = new TcpListener(IPAddress.Any, 21279);
			serverSocket.Start();
			Console.WriteLine("Server started.");

			while(true) {
				//accept new Client and add to static ClientList
				Client client = new Client(serverSocket.AcceptTcpClient(), ClientList.Count().ToString());
				ClientList.Add(client);
			}
		}
	}
}
