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
		/// Checks each dictionary word with possible variations.
		/// </summary>
		/// <param name="dictionaryEntry">The dictionary word to test with.</param>
		/// <param name="userInfos">The encrypted user-password info to test against.</param>
		/// <returns></returns>
		public static IEnumerable<UserInfoClearText> CheckWordWithVariations(string dictionaryEntry, List<UserInfo> userInfos) {
			List<UserInfoClearText> result = new List<UserInfoClearText>();

			//check normal
			result.AddRange(CheckWord(userInfos, dictionaryEntry));

			//check with capitalisation variations
			result.AddRange(CheckWordCapitalised(userInfos, dictionaryEntry));

			//check reversed
			result.AddRange(CheckWordReversed(userInfos, dictionaryEntry));

			//check reversed with capitalisation variants
			result.AddRange(CheckWordCapitalisedReversed(userInfos, dictionaryEntry));

			//check with number in front OR at end
			for (int firstDigit = 0; firstDigit < 100; firstDigit++) {
				//check normal
				result.AddRange(CheckWord(userInfos, dictionaryEntry, firstDigit));

				//check with capitalisation variations
				result.AddRange(CheckWordCapitalised(userInfos, dictionaryEntry, firstDigit));

				//check reversed
				result.AddRange(CheckWordReversed(userInfos, dictionaryEntry, firstDigit));

				//check with number in front AND at end
				for (int secondDigit = 0; secondDigit < 100; secondDigit++) {
					//check normal
					result.AddRange(CheckWord(userInfos, dictionaryEntry, firstDigit, secondDigit));

					//check with capitalisation variations
					result.AddRange(CheckWordCapitalised(userInfos, dictionaryEntry, firstDigit, secondDigit));

					//check reversed
					result.AddRange(CheckWordReversed(userInfos, dictionaryEntry, firstDigit, secondDigit));

					//check reversed with capitalisation variations
					result.AddRange(CheckWordCapitalisedReversed(userInfos, dictionaryEntry, firstDigit, secondDigit));
				}
			}

			return result;
		}

		private static IEnumerable<UserInfoClearText> CheckWord(IEnumerable<UserInfo> userInfos, string dictionaryEntry, int firstDigit = -1, int secondDigit = -1) {
			List<UserInfoClearText> result = new List<UserInfoClearText>();
			string firstDigitString = firstDigit > -1 ? firstDigit.ToString() : "";
			string secondDigitString = secondDigit > -1 ? secondDigit.ToString() : "";

			result.AddRange(CheckSingleWord(userInfos, firstDigitString + dictionaryEntry + secondDigitString));
			return result;
		}

		private static IEnumerable<UserInfoClearText> CheckWordCapitalised(IEnumerable<UserInfo> userInfos, string dictionaryEntry, int firstDigit = -1, int secondDigit = -1) {
			List<UserInfoClearText> result = new List<UserInfoClearText>();
			string firstDigitString = firstDigit > -1 ? firstDigit.ToString() : "";
			string secondDigitString = secondDigit > -1 ? secondDigit.ToString() : "";

			for(int i=1,L=dictionaryEntry.Length; i<=L; i++) {
				string wordCapitalised = StringUtils.CapitaliseToIndex(dictionaryEntry, i);
				result.AddRange(CheckSingleWord(userInfos, firstDigitString + wordCapitalised + secondDigitString));
			}
			return result;
		}

		private static IEnumerable<UserInfoClearText> CheckWordReversed(IEnumerable<UserInfo> userInfos, string dictionaryEntry, int firstDigit = -1, int secondDigit = -1) {
			List<UserInfoClearText> result = new List<UserInfoClearText>();
			string firstDigitString = firstDigit > -1 ? firstDigit.ToString() : "";
			string secondDigitString = secondDigit > -1 ? secondDigit.ToString() : "";

			string wordReversed = StringUtils.Reverse(dictionaryEntry);
			result.AddRange(CheckSingleWord(userInfos, firstDigitString + wordReversed + secondDigitString));
			return result;
		}

		private static IEnumerable<UserInfoClearText> CheckWordCapitalisedReversed(IEnumerable<UserInfo> userInfos, string dictionaryEntry, int firstDigit = -1, int secondDigit = -1) {
			List<UserInfoClearText> result = new List<UserInfoClearText>();
			string firstDigitString = firstDigit > -1 ? firstDigit.ToString() : "";
			string secondDigitString = secondDigit > -1 ? secondDigit.ToString() : "";

			string wordReversed = StringUtils.Reverse(dictionaryEntry);
			for(int i=1,L=dictionaryEntry.Length; i<=L; i++) {
				string wordCapitalisedReversed = StringUtils.CapitaliseToIndex(wordReversed, i);
				result.AddRange(CheckSingleWord(userInfos, firstDigitString + wordCapitalisedReversed + secondDigitString));
			}
			return result;
		}

		/// <summary>
		/// Encrypts a string in SHA1 and compares its bytes to a list of encrypted passwords.
		/// </summary>
		/// <param name="userInfos">The list of encrypted passwords (and usernames).</param>
		/// <param name="possiblePassword">The string to compare.</param>
		/// <returns>Returns a list of user info with successfully matched passwords.</returns>
		private static IEnumerable<UserInfoClearText> CheckSingleWord(IEnumerable<UserInfo> userInfos, string possiblePassword) {
			char[] charArray = possiblePassword.ToCharArray();
			byte[] passwordAsBytes = Array.ConvertAll(charArray, PasswordHandler.GetConverter());
			byte[] encryptedPassword = messageDigest.ComputeHash(passwordAsBytes);

			List<UserInfoClearText> results = new List<UserInfoClearText>();
			foreach (UserInfo userInfo in userInfos) {
				if (CompareBytes(userInfo.EncryptedPassword, encryptedPassword)) {
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
