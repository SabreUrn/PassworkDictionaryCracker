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

			//accept clients while accepting clients
			Task.Factory.StartNew(() => AcceptClients.AcceptClient(serverSocket));
			Console.WriteLine("Waiting for clients.");
			//stop accepting when all connected
			Console.WriteLine("Press any key to stop accepting clients and begin cracking password list.");
			Console.ReadKey();
			AcceptClients.Accepting = false;

			//get username-password list file location
			string[] lines = System.IO.File.ReadAllLines(@"passwords.txt");
			foreach(string line in lines) {
				Console.WriteLine(line);
			}
			Console.ReadKey();
		}
	}
}
