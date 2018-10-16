using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CrackerClient {
	/// <summary>
	/// Singleton client class for maintaining the client, its status, and its connection.
	/// </summary>
	public class Client {
		private static Client _instance;
		private TcpClient _clientSocket;
		private NetworkStream _ns;
		private StreamReader _sr;
		private StreamWriter _sw;
		private string _ip;
		private int _port;

		/// <summary>
		/// The current chunk of dictionary words to test with.
		/// </summary>
		public List<string> Chunk { get; set; }

		/// <summary>
		/// Instantiates the client.
		/// </summary>
		private Client() { }

		public void Initialise() {
			GetIpAndPort();
			_clientSocket = WaitForServer();
			_ns = _clientSocket.GetStream();
			_sr = new StreamReader(_ns);
			_sw = new StreamWriter(_ns) { AutoFlush = true };
			Console.WriteLine("Client ready.");
		}

		/// <summary>
		/// Singleton instance getter.
		/// </summary>
		public static Client Instance {
			get {
				if (_instance == null) {
					_instance = new Client();
				}
				return _instance;
			}
		}

		/// <summary>
		/// Reads a single-line message from the server.
		/// </summary>
		/// <returns>The read message.</returns>
		public string ReadLine() {
			return _sr.ReadLine();
		}

		/// <summary>
		/// Writes a single-line message to the server.
		/// </summary>
		/// <param name="line">The message to write.</param>
		public void WriteLine(string line) {
			_sw.WriteLine(line);
		}

		/// <summary>
		/// Traps the user in a loop until IP and proper port are provided.
		/// </summary>
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

		/// <summary>
		/// Holds client until server connection can be found.
		/// </summary>
		/// <returns>The server TCP socket.</returns>
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
