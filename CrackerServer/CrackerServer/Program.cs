using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CrackerServer {
	class Program {
		static void Main(string[] args) {
			TcpListener serverSocket = new TcpListener(IPAddress.Any, 21279);
			serverSocket.Start();
			Console.WriteLine("Server started.");

			//accept clients while accepting clients
			Task.Factory.StartNew(() => AcceptClients.AcceptClient(serverSocket));
			Console.WriteLine("Waiting for clients.");
			//stop accepting when all connected
			Console.WriteLine("Press any key to stop accepting clients and begin cracking password list.");
			Console.ReadKey(true);
			AcceptClients.Accepting = false;

			//get username-password list file contents
			string[] userInfos = System.IO.File.ReadAllLines("passwords.txt");
			//Console.WriteLine(String.Join(" ", lines));

			//divide work to clients
			int clientCount = ClientList.Count;
			int currentIndex = 0;
			List<Task> clientReadTasks = new List<Task>();
			for (int currentClientIndex = 1; currentClientIndex <= clientCount; currentClientIndex++) {
				int newSize = userInfos.Length / clientCount;
				string[] smallerArray = new string[newSize];
				for (int i = 0; i < newSize; i++) {
					Console.WriteLine(userInfos[currentIndex]);
					smallerArray[i] = userInfos[currentIndex];
					currentIndex++;
				}

				Client client = ClientList.GetByIndex(currentClientIndex - 1);
				client.WriteEncryptedPasswords(smallerArray);

				Task task = Task.Factory.StartNew(() => client.ReadCrackedPasswords());
				clientReadTasks.Add(task);
				Console.WriteLine("did it");
			}
			Task[] taskArray = clientReadTasks.ToArray();
			Task.Factory.ContinueWhenAll(taskArray, FinishWork);

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
