using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CrackerServer {
	public class Client {
		private TcpClient _client;
		private NetworkStream _ns;
		private StreamReader _sr;
		private StreamWriter _sw;

		private string _name;

		public Client(TcpClient client, string name) {
			_client = client;
			_ns = _client.GetStream();
			_sr = new StreamReader(_ns);
			_sw = new StreamWriter(_ns) { AutoFlush = true };
			_name = name;

			Console.WriteLine($"Client {_name} connected.");
			Task.Factory.StartNew(() => RunClient());
		}

		public TcpClient GetClient() {
			return _client;
		}

		public bool IsRunning() {
			return _client.Connected;
		}

		public void RunClient() {
			while(true) {
			}
		}

		public string ReadMessage() {
			try {
				return _sr.ReadLine();
			} catch(IOException) {
				Console.WriteLine($"Client {_name} disconnected.");
				Close();
				return null;
			}
		}

		public bool ReadCrackedPasswords() {
			string[] returnMessage = this.ReadMessage().Split('\\');
			for (int i=0,L=returnMessage.Length; i<L; i++) {
				Console.WriteLine(returnMessage[i]);
			}
			return true;
		}

		public void WriteMessage(string message) {
			_sw.WriteLine(message);
		}

		public void WriteEncryptedPasswords(string[] message) {
			//protocol:
			//1st line of server message is cracking portion size
			//2nd line is cracking portion with each pair separated by space
			//last line is "PWLISTEND" to tell the client to start decoding
			WriteMessage(message.Length.ToString());
			for(int i=0,L=message.Length; i<L; i++) {
				WriteMessage(message[i]);
			}
			WriteMessage("PWLISTEND");
		}

		private void Close() {
			_sw.Close();
			_sr.Close();
			_ns.Close();
			_ns.Dispose();
			_client.Close();
			_client.Dispose();
			ClientList.RemoveByClient(this);
		}
	}
}
