using CrackerClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CrackerClient.Utils {
	public static class CheckVariations {
		private static readonly HashAlgorithm messageDigest = new SHA1CryptoServiceProvider();

		//public static void CheckSingleWord(IEnumerable<UserInfo> userInfos, string possiblePassword) {

		//}

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

		private static bool CompareBytes(IList<byte> firstArray, IList<byte> secondArray) {
			if(firstArray.Count != secondArray.Count) return false;

			for(int i = 0,L = firstArray.Count; i<L; i++) {
				if (firstArray[i] != secondArray[i]) return false;
			}
			return true;
		}
	}
}
