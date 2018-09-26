using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CrackerClient {
	public class Client {
		private static Client _instance;
		private TcpClient _clientSocket;
		private NetworkStream _ns;
		private StreamReader _sr;
		private StreamWriter _sw;

		private string _ip;
		private int _port;

		private Client() {
			GetIpAndPort();
			_clientSocket = WaitForServer();
			NetworkStream ns = _clientSocket.GetStream();
			_sr = new StreamReader(ns);
			_sw = new StreamWriter(ns) { AutoFlush = true };
			Console.WriteLine("Client ready.");
		}

		public static Client Instance {
			get {
				if (_instance == null) {
					_instance = new Client();
				}
				return _instance;
			}
		}

		public string ReadLine() {
			return _sr.ReadLine();
		}

		public void WriteLine(string line) {
			_sw.WriteLine(line);
		}


		private void GetIpAndPort() {
			Console.Write("Enter IP: ");
			_ip = Console.ReadLine();
			Console.Write("Enter port: ");
			string portStr = Console.ReadLine();
			bool portIsInt = Int32.TryParse(portStr, out _port);
			while (!portIsInt) {
				Console.WriteLine("Unable to parse port.");
				Console.Write("Enter port: ");
				portStr = Console.ReadLine();
				portIsInt = Int32.TryParse(portStr, out _port);
			}
		}

		private TcpClient WaitForServer() {
			TcpClient clientSocket = new TcpClient();
			bool serverFound = false;

			while (!serverFound) {
				try {
					clientSocket = new TcpClient(_ip, _port);
					serverFound = true;
				} catch (SocketException) {
					Console.WriteLine("Cannot find server. Check if server is running.");
					Console.WriteLine("Retrying in 5 seconds.");
					System.Threading.Thread.Sleep(5000);
				}
			}
			return clientSocket;
		}
	}
}
