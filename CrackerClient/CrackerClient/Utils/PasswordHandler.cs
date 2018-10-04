using CrackerClient.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrackerClient.Utils {
	/// <summary>
	/// Manages password encryption and user info encrypted password extraction.
	/// </summary>
	public static class PasswordHandler {
		private static readonly Converter<char, byte> converter = CharToByte;

		/// <summary>
		/// Converts a list of one-string and colon-separated user+password info into just the passwords.
		/// </summary>
		/// <param name="usersRaw">The unseparated user info.</param>
		/// <returns>User info list of the usernames+separated password pairs.</returns>
		public static List<UserInfo> ReadPasswordList(List<string> usersRaw) {
			List<UserInfo> result = new List<UserInfo>();

			Console.WriteLine(String.Join("\n", usersRaw));
			Console.WriteLine();
			foreach(string user in usersRaw) {
				string[] parts = user.Split(':');
				UserInfo userInfo = new UserInfo(parts[0], parts[1]);
				result.Add(userInfo);
			}

			return result;
		}

		/// <summary>
		/// Converts a char to a byte.
		/// </summary>
		/// <returns></returns>
		public static Converter<char, byte> GetConverter() {
			return converter;
		}

		private static byte CharToByte(char c) {
			return Convert.ToByte(c);
		}
	}
}
