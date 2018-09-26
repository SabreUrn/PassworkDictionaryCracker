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
			Client client = Client.Instance;

			//protocol:
			//1st line of server message is cracking portion size
			//2nd line is cracking portion with each pair separated by space
			//last line is "PWLISTEND" to tell the client to start decoding

			// Get portion size
			string portionSize = client.ReadLine();
			bool portionSizeIsNum = Int32.TryParse(portionSize, out int portionSizeInt);
			if(!portionSizeIsNum) {
				throw new Exception($"Portion size is not int. Portion size: {portionSize}");
			}
			Console.WriteLine($"Portion size: {portionSize}");

			// Get portions until "PWLISTEND" received
			List<string> portion = new List<string>();
			string nextLine = client.ReadLine();
			while(nextLine != "PWLISTEND") {
				portion.Add(nextLine);
				nextLine = client.ReadLine();
			}

			// Crack list
			client.WriteLine(Crack.RunCracking(portion));

			Console.WriteLine("Done with list. Press any key to exit.");
			Console.ReadKey();

			//string message = "";
			//string serverMessage = "";
			//while(true) {
			//	message = Console.ReadLine();
			//	try {
			//		serverMessage = sr.ReadLine();

			//	} catch(IOException) {
			//		Console.WriteLine("Server connection closed.");
			//		Console.WriteLine("Press any key to exit.");
			//		Console.ReadKey();
			//		return;
			//	}
			//	Console.WriteLine($"Server: {serverMessage}");
			//}
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
