using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrackerServer {
	public static class ClientList {
		private static List<Client> _list = new List<Client>();

		public static void Add(Client c) {
			_list.Add(c);
		}

		public static Client GetByIndex(int index) {
			return _list[index];
		}

		public static void RemoveByClient(Client c) {
			_list.Remove(c);
		}

		public static int Count() {
			return _list.Count;
		}
	}
}
