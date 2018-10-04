using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrackerServer {
	/// <summary>
	/// Keeps track of information that must be accessed by any number of client objects.
	/// </summary>
	static class SharedStatus {
		/// <summary>
		/// Array over encrypted user data.
		/// </summary>
		public static string[] UserInfos;
		/// <summary>
		/// The current index of the dictionary chunks list.
		/// </summary>
		public static volatile int CurrentChunkIndex = 0;
		/// <summary>
		/// Used in case a client disconnects before returning its current chunk.
		/// </summary>
		public static List<int> ChunkIndexOverrides = new List<int>();
		/// <summary>
		/// The dictionary file split into smaller chunks.
		/// </summary>
		public static List<List<string>> DictionaryChunks = new List<List<string>>();
	}
}
