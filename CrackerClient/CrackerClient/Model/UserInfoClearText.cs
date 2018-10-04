using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrackerClient.Model {
	/// <summary>
	/// Unencrypted user info.
	/// </summary>
	public class UserInfoClearText {
		/// <summary>
		/// User info username.
		/// </summary>
		public string Username { get; set; }
		/// <summary>
		/// User info plaintext password.
		/// </summary>
		public string Password { get; set; }

		/// <summary>
		/// Instantiates a new plaintext user info object.
		/// </summary>
		/// <param name="username">The username of the user info.</param>
		/// <param name="password">The plaintext password of the user info.</param>
		public UserInfoClearText(string username, string password) {
			Username = username ?? throw new ArgumentNullException("username");
			Password = password ?? throw new ArgumentNullException("password");
		}

		/// <summary>
		/// Prints the user info in the format "{Username}: {Password}"
		/// </summary>
		/// <returns>User-friendly string of object username and password.</returns>
		public override string ToString() {
			return $"{Username}: {Password}";
		}
	}
}
