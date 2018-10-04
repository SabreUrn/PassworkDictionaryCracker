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

			/* Crack list */
			Crack.RunCracking();

			Console.WriteLine("Done with list. Press any key to exit.");
			Console.ReadKey();
		}
	}
}
