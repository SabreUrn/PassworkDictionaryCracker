using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CrackerClient {
	class Program {
		static void Main(string[] args) {
			Console.Write("Enter IP: ");
			string ip = Console.ReadLine();
			Console.Write("Enter port: ");
			string port = Console.ReadLine();
			TcpClient clientSocket = WaitForServer(ip, port);
			Console.WriteLine("Client ready.");

			NetworkStream ns = clientSocket.GetStream();
			StreamReader sr = new StreamReader(ns);
			StreamWriter sw = new StreamWriter(ns) { AutoFlush = true };

			string message = "";
			string serverAnswer = "";

			while(true) {
				message = Console.ReadLine();
				try {
					sw.WriteLine(message);
					serverAnswer = sr.ReadLine();
				} catch(IOException) {
					Console.WriteLine("Server connection closed.");
					Console.WriteLine("Press any key to exit.");
					Console.ReadKey();
					return;
				}
				Console.WriteLine($"Server: {serverAnswer}");
			}
		}

		private static TcpClient WaitForServer(string ip, string port) {
			TcpClient clientSocket = new TcpClient();
			bool serverFound = false;

			bool portIsInt = Int32.TryParse(port, out int portNumber);
			while(!portIsInt) {
				Console.WriteLine("Unable to parse port.");
				Console.Write("Enter port: ");
				port = Console.ReadLine();
				portIsInt = Int32.TryParse(port, out portNumber);
			}

			while(!serverFound) {
				try {
					clientSocket = new TcpClient(ip, portNumber);
					serverFound = true;
				} catch(SocketException) {
					Console.WriteLine("Cannot find server. Check if server is running.");
					Console.WriteLine("Retrying in 5 seconds.");
					System.Threading.Thread.Sleep(5000);
				}
			}
			return clientSocket;
		}
	}
}
