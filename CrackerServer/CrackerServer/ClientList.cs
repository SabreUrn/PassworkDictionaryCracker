using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrackerServer {
	/// <summary>
	/// Keeps track of all clients.
	/// </summary>
	public static class ClientList {
		private static List<Client> _list = new List<Client>();

		/// <summary>
		/// Adds a client to the list.
		/// </summary>
		/// <param name="c">The client to add to the list.</param>
		public static void Add(Client c) {
			_list.Add(c);
		}

		/// <summary>
		/// Gets a client by its index in the list.
		/// </summary>
		/// <param name="index">The index to search with.</param>
		/// <returns>Returns the client corresponding to the index.</returns>
		public static Client GetByIndex(int index) {
			return _list[index];
		}

		/// <summary>
		/// Removes a client by object reference.
		/// </summary>
		/// <param name="c">The client to remove.</param>
		public static void RemoveByClient(Client c) {
			_list.Remove(c);
		}

		/// <summary>
		/// The number of clients in the client list.
		/// </summary>
		public static int Count {
			get { return _list.Count; }
		}
	}
}
