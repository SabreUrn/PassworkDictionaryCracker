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
		}
	}
}
