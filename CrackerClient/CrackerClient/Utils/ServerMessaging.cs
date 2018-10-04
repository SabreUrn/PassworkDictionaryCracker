using CrackerClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrackerClient.Utils {
	/// <summary>
	/// Responsible for communicating with the server.
	/// Contains the protocols for communicating with the server.
	/// </summary>
	public static class ServerMessaging {
		private static Client client = Client.Instance;

		/// <summary>
		/// Requests the encrypted user-password pairs from the server.
		/// </summary>
		/// <returns>Returns a string list of the encrypted user-password pairs.</returns>
		public static List<string> SetupPasswords() {
			/* Get password list */
			//protocol:
			//1st msg: "PWLISTREQ"
			//2nd to nth msg: user-encrpw pair
			//n+1th msg: "PWLISTEND"
			List<string> passwords = new List<string>();

			client.WriteLine("PWLISTREQ");
			string response = client.ReadLine();
			if (response == "PWLISTRES") {
				string nextLine = client.ReadLine();
				while (nextLine != "PWLISTRESEND") {
					passwords.Add(nextLine);
					nextLine = client.ReadLine();
				}
			}
			return passwords;
		}

		/// <summary>
		/// Requests a chunk of dictionary words to test against the encrypted passwords.
		/// </summary>
		/// <returns>True if a valid chunk was gotten from server, false otherwise.</returns>
		public static bool SetupValidChunk() {
			/* Get chunk */
			//protocol:
			//1st msg: "CHUNKSIZE"
			//2nd to nth msg: each word in the chunk
			//n+1th msg: "CHUNKSIZERESEND"
			client.Chunk = new List<string>();
			client.WriteLine("CHUNKSIZEREQ");
			string response = client.ReadLine();
			if (response == "CHUNKSIZERES") {
				string nextLine = client.ReadLine();
				while (nextLine != "CHUNKSIZERESEND") {
					client.Chunk.Add(nextLine);
					nextLine = client.ReadLine();
				}
				return true;
			}
			if(response == "NOCHUNK") {
				return false;
			}
			return false;
		}

		/// <summary>
		/// Writes successfully decrypted user-password pairs to the server.
		/// </summary>
		/// <param name="result">The list of decrypted user-password pairs.</param>
		public static void WriteResults(List<UserInfoClearText> result) {
			//protocol:
			//1st msg: "CHUNKRES"
			//2nd msg: results in 1 string separated by \\
			client.WriteLine("CHUNKRES");
			client.WriteLine(String.Join("\\", result));
		}
	}
}
