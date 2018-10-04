using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrackerClient.Model {
	/// <summary>
	/// Encrypted user info.
	/// </summary>
	public class UserInfo {
		/// <summary>
		/// The unencrypted username.
		/// </summary>
		public string Username { get; set; }
		/// <summary>
		/// The password encrypted in Base64.
		/// </summary>
		public string EncryptedPasswordBase64 { get; set; }
		/// <summary>
		/// The encrypted password converted to bytes.
		/// </summary>
		public byte[] EncryptedPassword { get; set; }

		/// <summary>
		/// Instantiates a new encrypted user info object.
		/// </summary>
		/// <param name="username">Unencrypted username.</param>
		/// <param name="encryptedPasswordBase64">Password as Base64</param>
		public UserInfo(string username, string encryptedPasswordBase64) {
			Username = username ?? throw new ArgumentNullException("username");
			EncryptedPasswordBase64 = encryptedPasswordBase64 ?? throw new ArgumentNullException("encryptedPasswordBase64");
			EncryptedPassword = Convert.FromBase64String(encryptedPasswordBase64);
		}

		/// <summary>
		/// Prints the user info in the format "{Username}:{EncryptedPassword}"
		/// </summary>
		/// <returns>Single-string username and encrypted password.</returns>
		public override string ToString() {
			return Username + ":" + EncryptedPasswordBase64;
		}
	}
}
