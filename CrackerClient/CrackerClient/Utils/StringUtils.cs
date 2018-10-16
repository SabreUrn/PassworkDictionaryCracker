using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrackerClient.Utils {
	/// <summary>
	/// Holds methods for string transformations.
	/// </summary>
	public static class StringUtils {
		/// <summary>
		/// Capitalises the first letter of a string.
		/// </summary>
		/// <param name="str">The string to capitalise.</param>
		/// <returns>The string, capitalised.</returns>
		public static string Capitalise(string str) {
			if (str == null) throw new ArgumentNullException("str");
			if (str.Trim().Length == 0) return str;

			string strFirstLetterUppsecase = str.Substring(0, 1).ToUpper();
			string strRest = str.Substring(1);
			return strFirstLetterUppsecase + strRest;
		}


		public static string CapitaliseToIndex(string str, int index) {
			if (str == null) throw new ArgumentNullException("str");
			if (str.Trim().Length == 0) return str;

			string strFirstLetterUppsecase = str.Substring(0, index).ToUpper();
			string strRest = str.Substring(index);
			return strFirstLetterUppsecase + strRest;
		}

		/// <summary>
		/// Reverses a string.
		/// </summary>
		/// <param name="str">The string to reverse.</param>
		/// <returns>The string with each character in inverse order.</returns>
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
