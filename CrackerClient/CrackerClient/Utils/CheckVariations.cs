using CrackerClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CrackerClient.Utils {
	/// <summary>
	/// Checks variations of a dictionary word.
	/// </summary>
	public static class CheckVariations {
		private static readonly HashAlgorithm messageDigest = new SHA1CryptoServiceProvider();

		/// <summary>
		/// Encrypts a string in SHA1 and compares its bytes to a list of encrypted passwords.
		/// </summary>
		/// <param name="userInfos">The list of encrypted passwords (and usernames).</param>
		/// <param name="possiblePassword">The string to compare.</param>
		/// <returns>Returns a list of user info with successfully matched passwords.</returns>
		public static IEnumerable<UserInfoClearText> CheckSingleWord(IEnumerable<UserInfo> userInfos, string possiblePassword) {
			char[] charArray = possiblePassword.ToCharArray();
			byte[] passwordAsBytes = Array.ConvertAll(charArray, PasswordHandler.GetConverter());
			byte[] encryptedPassword = messageDigest.ComputeHash(passwordAsBytes);

			List<UserInfoClearText> results = new List<UserInfoClearText>();
			foreach(UserInfo userInfo in userInfos) {
				if(CompareBytes(userInfo.EncryptedPassword, encryptedPassword)) {
					results.Add(new UserInfoClearText(userInfo.Username, possiblePassword));
					Console.WriteLine($"{userInfo.Username}: {possiblePassword}");
				}
			}
			return results;
		}

		/// <summary>
		/// Checks the value equality of two byte lists.
		/// </summary>
		/// <param name="firstArray">The first byte array to compare.</param>
		/// <param name="secondArray">The second byte array to compare.</param>
		/// <returns>Returns false if arrays did not match, true otherwise.</returns>
		private static bool CompareBytes(IList<byte> firstArray, IList<byte> secondArray) {
			if(firstArray.Count != secondArray.Count) return false;

			for(int i = 0,L = firstArray.Count; i<L; i++) {
				if (firstArray[i] != secondArray[i]) return false;
			}
			return true;
		}
	}
}
