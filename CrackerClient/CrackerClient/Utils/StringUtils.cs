using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrackerClient.Utils {
	public static class StringUtils {
		public static string Capitalise(string str) {
			if (str == null) throw new ArgumentNullException("str");
			if (str.Trim().Length == 0) return str;

			string strFirstLetterUppsecase = str.Substring(0, 1).ToUpper();
			string strRest = str.Substring(1);
			return strFirstLetterUppsecase + strRest;
		}

		public static string Reverse(string str) {
			if (str == null) throw new ArgumentNullException("str");
			if (str.Trim().Length == 0) return str;

			StringBuilder reverseStr = new StringBuilder();
			for (int i = 0, L = str.Length; i < L; i++) {
				reverseStr.Append(str.ElementAt(str.Length - 1 - i));
			}
			return reverseStr.ToString();
		}
	}
}
