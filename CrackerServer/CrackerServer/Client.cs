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
		private string _readMessage;

		public Client(TcpClient client, string name) {
			_client = client;
			_ns = _client.GetStream();
			_sr = new StreamReader(_ns);
			_sw = new StreamWriter(_ns) { AutoFlush = true };
			_name = name;
			_readMessage = "";

			Task.Factory.StartNew(() => RunClient());
		}

		public TcpClient GetClient() {
			return _client;
		}

		public bool IsRunning() {
			return _client.Connected;
		}

		public void RunClient() {
			Console.WriteLine($"Client {_name} connected.");
			while(true) {
				//if(_readMessage == null) {
				//	return;
				//}
				//_readMessage = ReadMessage();
				//while(_readMessage != null) {
				//	Console.WriteLine($"Client {_name}: {_readMessage}");
				//	WriteMessage(_readMessage);
				//	_readMessage = ReadMessage();
				//}
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

		public void WriteMessage(string message) {
			//string writeMessage = "";
			//writeMessage = readMessage.ToUpper();
			_sw.WriteLine(message);
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
