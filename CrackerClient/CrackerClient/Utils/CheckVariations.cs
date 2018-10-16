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
			IEnumerable<UserInfoClearText> partialResultNormal = CheckWord(userInfos, dictionaryEntry);
			result.AddRange(partialResultNormal);
			userInfos = RemoveMatches(partialResultNormal, userInfos);

			//check with capitalisation variations
			IEnumerable<UserInfoClearText> partialResultCapitalised = CheckWordCapitalised(userInfos, dictionaryEntry);
			result.AddRange(partialResultCapitalised);
			userInfos = RemoveMatches(partialResultCapitalised, userInfos);

			//check reversed
			IEnumerable<UserInfoClearText> partialResultReversed = CheckWordReversed(userInfos, dictionaryEntry);
			result.AddRange(partialResultReversed);
			userInfos = RemoveMatches(partialResultReversed, userInfos);

			//check reversed with capitalisation variants
			IEnumerable<UserInfoClearText> partialResultCapitalisedReversed = CheckWordCapitalisedReversed(userInfos, dictionaryEntry);
			result.AddRange(partialResultCapitalisedReversed);
			userInfos = RemoveMatches(partialResultNormal, userInfos);

			//check with number in front OR at end
			for (int firstDigit = 0; firstDigit < 100; firstDigit++) {
				//check normal
				IEnumerable<UserInfoClearText> partialResultNormalNumbersAtEither = CheckWord(userInfos, dictionaryEntry, firstDigit);
				result.AddRange(partialResultNormalNumbersAtEither);
				userInfos = RemoveMatches(partialResultNormalNumbersAtEither, userInfos);

				//check with capitalisation variations
				IEnumerable<UserInfoClearText> partialResultCapitalisedNumbersAtEither = CheckWordCapitalised(userInfos, dictionaryEntry, firstDigit);
				result.AddRange(partialResultCapitalisedNumbersAtEither);
				userInfos = RemoveMatches(partialResultCapitalisedNumbersAtEither, userInfos);

				//check reversed
				IEnumerable<UserInfoClearText> partialResultReversedNumbersAtEither = CheckWordReversed(userInfos, dictionaryEntry, firstDigit);
				result.AddRange(partialResultReversedNumbersAtEither);
				userInfos = RemoveMatches(partialResultReversedNumbersAtEither, userInfos);

				//check reversed with capitalisation variations
				IEnumerable<UserInfoClearText> partialResultCapitalisedReversedNumbersAtEither = CheckWordCapitalisedReversed(userInfos, dictionaryEntry, firstDigit);
				result.AddRange(partialResultCapitalisedReversedNumbersAtEither);
				userInfos = RemoveMatches(partialResultCapitalisedReversedNumbersAtEither, userInfos);

				//check with number in front AND at end
				for (int secondDigit = 0; secondDigit < 100; secondDigit++) {
					//check normal
					IEnumerable<UserInfoClearText> partialResultNormalNumbersAtBoth = CheckWord(userInfos, dictionaryEntry, firstDigit, secondDigit);
					result.AddRange(partialResultNormalNumbersAtBoth);
					userInfos = RemoveMatches(partialResultNormalNumbersAtBoth, userInfos);

					//check with capitalisation variations
					IEnumerable<UserInfoClearText> partialResultCapitalisedNumbersAtBoth = CheckWordCapitalised(userInfos, dictionaryEntry, firstDigit, secondDigit);
					result.AddRange(partialResultCapitalisedNumbersAtBoth);
					userInfos = RemoveMatches(partialResultCapitalisedNumbersAtBoth, userInfos);

					//check reversed
					IEnumerable<UserInfoClearText> partialResultReversedNumbersAtBoth = CheckWordReversed(userInfos, dictionaryEntry, firstDigit, secondDigit);
					result.AddRange(partialResultReversedNumbersAtBoth);
					userInfos = RemoveMatches(partialResultReversedNumbersAtBoth, userInfos);

					//check reversed with capitalisation variations
					IEnumerable<UserInfoClearText> partialResultCapitalisedReversedNumbersAtBoth = CheckWordCapitalisedReversed(userInfos, dictionaryEntry, firstDigit, secondDigit);
					result.AddRange(partialResultCapitalisedReversedNumbersAtBoth);
					userInfos = RemoveMatches(partialResultCapitalisedReversedNumbersAtBoth, userInfos);
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

		private static List<UserInfo> RemoveMatches(IEnumerable<UserInfoClearText> partialResult, List<UserInfo> userInfos) {
			foreach (UserInfoClearText matchedUserInfo in partialResult) {
				userInfos.Remove(userInfos.Where(userInfo => userInfo.Username == matchedUserInfo.Username).First());
			}
			return userInfos;
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
