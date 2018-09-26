using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrackerClient.Model {
	public class UserInfo {
		public string Username { get; set; }
		public string EncryptedPasswordBase64 { get; set; }
		public byte[] EncryptedPassword { get; set; }

		public UserInfo(string username, string encryptedPasswordBase64) {
			Username = username ?? throw new ArgumentNullException("username");
			EncryptedPasswordBase64 = encryptedPasswordBase64 ?? throw new ArgumentNullException("encryptedPasswordBase64");
			EncryptedPassword = Convert.FromBase64String(encryptedPasswordBase64);
		}

		public override string ToString() {
			return Username + ":" + EncryptedPasswordBase64;
		}
	}
}
