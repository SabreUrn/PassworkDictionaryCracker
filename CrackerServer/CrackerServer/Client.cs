using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CrackerServer {
	/// <summary>
	/// Class for handling n number of clients concurrently.
	/// </summary>
	public class Client {
		private TcpClient _client;
		private NetworkStream _ns;
		private StreamReader _sr;
		private StreamWriter _sw;

		private string _name;
		private int _lastAccessedChunkIndex;

		/// <summary>
		/// Instantiates a new client at any point in the server's life cycle.
		/// </summary>
		/// <param name="client">The accepted TCP connection through the server socket.</param>
		/// <param name="name">The name of the client.</param>
		public Client(TcpClient client, string name) {
			_client = client;
			_ns = _client.GetStream();
			_sr = new StreamReader(_ns);
			_sw = new StreamWriter(_ns) { AutoFlush = true };
			_name = name;
			ClientList.Add(this);

			Console.WriteLine($"Client {_name} connected.");
			Task.Factory.StartNew(() => RunClient());
		}

		/// <summary>
		/// Gets the TcpClient inside object.
		/// </summary>
		/// <returns>TcpClient of the client.</returns>
		public TcpClient GetClient() {
			return _client;
		}

		/// <summary>
		/// TcpClient's Connected status.
		/// </summary>
		/// <returns>True if client is still connected, false otherwise.</returns>
		public bool IsRunning() {
			return _client.Connected;
		}

		/// <summary>
		/// Main run loop.
		/// Reads message through StreamReader and calls client methods according to protocol.
		/// </summary>
		public void RunClient() {
			while(true) {
				string message = ReadMessage();
				if(message == null) { //necessary to break properly if client closes
					return;
				}
				switch(message) {
					case "PWLISTREQ":
						WriteEncryptedPasswords(SharedStatus.UserInfos);
						break;
					case "CHUNKSIZEREQ":
						WriteChunk();
						break;
					case "CHUNKRES":
						ReadCrackedPasswords();
						break;
					default:
						break;
				}
			}
		}

		/// <summary>
		/// Reads a line from the client through the StreamReader.
		/// Catches IOException on client closing and returns null.
		/// </summary>
		/// <returns>Returns the read line.</returns>
		public string ReadMessage() {
			try {
				return _sr.ReadLine();
			} catch(Exception ex) when(ex is IOException) {
				Console.WriteLine($"Client {_name} disconnected.");
				Close();
				return null;
			}
		}

		/// <summary>
		/// Writes a single-line message to the client through the StreamWriter.
		/// </summary>
		/// <param name="message">Message to write to the client.</param>
		public void WriteMessage(string message) {
			_sw.WriteLine(message);
		}

		/// <summary>
		/// Reads a single-line message from client and prints out user-friendly username-passwords pairs.
		/// </summary>
		public void ReadCrackedPasswords() {
			//protocol:
			//1st msg: "CHUNKRES"
			//2nd msg: results in 1 string separated by \
			string message = ReadMessage();
			string[] messageSplit = message.Split('\\');
			for (int i = 0, L = messageSplit.Length; i < L; i++) {
				Console.WriteLine(messageSplit[i]);
			}
		}

		/// <summary>
		/// Writes all encrypted username-password pairs to the client.
		/// </summary>
		/// <param name="message">Array of username + SHA1-encrypted password pairs.</param>
		public void WriteEncryptedPasswords(string[] message) {
			//protocol:
			//1st msg: "PWLISTRES"
			//2nd to nth msg: user-encrpw pair
			//n+1th msg: "PWLISTRESEND"
			WriteMessage("PWLISTRES");
			for(int i=0,L=message.Length; i<L; i++) {
				WriteMessage(message[i]);
			}
			WriteMessage("PWLISTRESEND");
		}

		/// <summary>
		/// Writes a previously cancelled chunk of dictionary words to the client if possible, otherwise the current chunk index.
		/// </summary>
		public void WriteChunk() {
			List<string> chunk = new List<string>();
			int currentChunkIndex;
			bool overrideIndex = false;
			//protocol if chunks left:
			//1st msg: "CHUNKSIZEREQ"
			//2nd to nth msg: each word in the chunk
			//n+1th msg: "CHUNKSIZERESEND"

			//protocol if no chunks left:
			//1st msg: "NOCHUNK"

			if (SharedStatus.ChunkIndexOverrides.Count > 0) { //if we have chunks we need overridden (because a client closed before sending its response),
				currentChunkIndex = SharedStatus.ChunkIndexOverrides[0]; //set our current chunk index to the first element in our overrides list
				SharedStatus.ChunkIndexOverrides.RemoveAt(0); //remove it so we don't assign a client to the chunk twice at once
				overrideIndex = true;
			} else {
				currentChunkIndex = SharedStatus.CurrentChunkIndex; //otherwise, we're just using the regular index
			}
			if (currentChunkIndex < SharedStatus.DictionaryChunks.Count) { //if true, we haven't sent all chunks yet
				chunk = SharedStatus.DictionaryChunks[currentChunkIndex]; //grab chunk at current index
				_lastAccessedChunkIndex = currentChunkIndex;
				WriteMessage("CHUNKSIZERES");
				foreach (string line in chunk) {
					WriteMessage(line);
				}
				WriteMessage("CHUNKSIZERESEND");
				if(!overrideIndex) SharedStatus.CurrentChunkIndex++;
			} else {
				WriteMessage("NOCHUNK");
			}
		}

		/// <summary>
		/// Closes, disposes, and removes client and client properties.
		/// Index of last accessed chunk is stored in SharedStatus.
		/// Client is removed from ClientList.
		/// </summary>
		private void Close() {
			_sw.Close();
			_sr.Close();
			_ns.Close();
			_ns.Dispose();
			_client.Close();
			_client.Dispose();
			if (!SharedStatus.ChunkIndexOverrides.Contains(_lastAccessedChunkIndex)) {
				SharedStatus.ChunkIndexOverrides.Add(_lastAccessedChunkIndex); //add the chunk this was working on but didn't return to our overrides so next client can do it
			}
			ClientList.RemoveByClient(this);
		}
	}
}
