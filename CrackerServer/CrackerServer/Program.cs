using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CrackerServer {
	class Program {
		static void Main(string[] args) {
			int chunkSize = 10000;

			TcpListener serverSocket = new TcpListener(IPAddress.Any, 21279);
			serverSocket.Start();
			Console.WriteLine("Server started.");

			/* Get username-password list file contents */
			string[] userInfos = File.ReadAllLines("passwords.txt");
			SharedStatus.UserInfos = userInfos;

			/* Fill dictionary chunks */
			using (FileStream fs = new FileStream("webster-dictionary.txt", FileMode.Open, FileAccess.Read))
			using (StreamReader dictionaryStream = new StreamReader(fs)) {
				while(!dictionaryStream.EndOfStream) {
					List<string> chunk = new List<string>();
					for(int i=0,L=chunkSize; i<L; i++) {
						if(dictionaryStream.EndOfStream) break;
						chunk.Add(dictionaryStream.ReadLine());
					}
					//string dictionaryLine = dictionaryStream.ReadLine();
					SharedStatus.DictionaryChunks.Add(chunk);
				}
			}

			/* Accept clients */
			Task.Factory.StartNew(() => AcceptClients.AcceptClient(serverSocket));
			Console.WriteLine("Waiting for clients.");


			/* Divide work to clients */
			int clientCount = ClientList.Count;
			int currentIndex = 0;
			List<Task> clientReadTasks = new List<Task>();
			Task[] taskArray = clientReadTasks.ToArray();
			//Task.Factory.ContinueWhenAll(taskArray, FinishWork);
			while(true) { } //don't exit until told to in FinishWork()
		}

		static void FinishWork(Task[] tasks) {
			if(tasks.All(t => t.Status == TaskStatus.RanToCompletion)) {
				Console.WriteLine("Passwords read.");
				Console.ReadKey();
				Environment.Exit(0);
			}
		}
	}
}
