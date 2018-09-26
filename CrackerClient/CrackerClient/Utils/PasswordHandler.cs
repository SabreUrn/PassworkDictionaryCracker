using CrackerClient.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrackerClient.Utils {
	public static class PasswordHandler {
		private static readonly Converter<char, byte> converter = CharToByte;

		public static List<UserInfo> ReadPasswordList(List<string> usersRaw) {
			List<UserInfo> result = new List<UserInfo>();

			Console.WriteLine(String.Join("\n", usersRaw));
			Console.WriteLine();
			foreach(string user in usersRaw) {
				Console.WriteLine(user);
				string[] parts = user.Split(':');
				UserInfo userInfo = new UserInfo(parts[0], parts[1]);
				result.Add(userInfo);
			}

			return result;
		}

		public static Converter<char, byte> GetConverter() {
			return converter;
		}

		private static byte CharToByte(char c) {
			return Convert.ToByte(c);
		}
	}
}
