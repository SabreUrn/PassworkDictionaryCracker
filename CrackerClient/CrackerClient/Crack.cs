using CrackerClient.Model;
using CrackerClient.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrackerClient {
	/// <summary>
	/// Responsible for managing password cracking.
	/// </summary>
	public static class Crack {
		private static int remainingPasswords; //no need to keep going through dic if we've gotten all pws
		private static Client client = Client.Instance;

		/// <summary>
		/// Starts the dictionary cracking process.
		/// </summary>
		public static void RunCracking() {
			List<string> usersRaw = ServerMessaging.SetupPasswords();
			List<UserInfo> userInfos = PasswordHandler.ReadPasswordList(usersRaw);
			remainingPasswords = userInfos.Count;
			List<UserInfoClearText> result = new List<UserInfoClearText>();

			while(ServerMessaging.SetupValidChunk()) {
				foreach (string line in Client.Instance.Chunk) {
					IEnumerable<UserInfoClearText> partialResult = CheckVariations.CheckWordWithVariations(line, userInfos);
					result.AddRange(partialResult);
					remainingPasswords -= partialResult.Count();
				}
				if(result.Count > 0) ServerMessaging.WriteResults(result);
				result = new List<UserInfoClearText>();
			}
		}

		/*
		/// <summary>
		/// Checks each dictionary word with possible variations.
		/// </summary>
		/// <param name="dictionaryEntry">The dictionary word to test with.</param>
		/// <param name="userInfos">The encrypted user-password info to test against.</param>
		/// <returns></returns>
		private static IEnumerable<UserInfoClearText> CheckWordWithVariations(string dictionaryEntry, List<UserInfo> userInfos) {
			List<UserInfoClearText> result = new List<UserInfoClearText>();

			//check word
			IEnumerable<UserInfoClearText> partialResult = CheckVariations.CheckSingleWord(userInfos, dictionaryEntry);
			result.AddRange(partialResult);

			//check word with capitalisation variations
			for(int i=1,L=dictionaryEntry.Length; i<=L; i++) {
				IEnumerable<UserInfoClearText> partialResultCapitalised = CheckVariations.CheckSingleWord(userInfos, StringUtils.CapitaliseToIndex(dictionaryEntry, i));
				result.AddRange(partialResultCapitalised);
			}

			//check word reversed
			IEnumerable<UserInfoClearText> partialResultReverse = CheckVariations.CheckSingleWord(userInfos, StringUtils.Reverse(dictionaryEntry));
			result.AddRange(partialResultReverse);

			//check word with number in front and at end
			for(int i=0,L=100; i<L; i++) {
				IEnumerable<UserInfoClearText> partialResultEndDigits = CheckVariations.CheckSingleWord(userInfos, dictionaryEntry + i);
				result.AddRange(partialResultEndDigits);

				IEnumerable<UserInfoClearText> partialResultStartDigits = CheckVariations.CheckSingleWord(userInfos, i + dictionaryEntry);
				result.AddRange(partialResultStartDigits);

				for (int j=0,K=100; j<K; j++) {
					IEnumerable<UserInfoClearText> partialResultStartEndDigits = CheckVariations.CheckSingleWord(userInfos, i + dictionaryEntry + j);
					result.AddRange(partialResultStartEndDigits);
				}
			}
			
			return result;
		}
		*/
	}
}
