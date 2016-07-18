using System;
using System.Net;
using System.Text;
using System.Security;
using System.Security.Cryptography;
using System.Runtime.InteropServices;

namespace TeamPomodoro.Util
{
	static class Crypto
	{
		/// <summary>
		/// Returns SHA256 hash as string of 64 hexadecimal digits for the specified SecureString
		/// </summary>
		internal static string GetHashString(this SecureString s)
		{
			StringBuilder sb = new StringBuilder(64);
			IntPtr ptr = IntPtr.Zero;
			try
			{
				ptr = Marshal.SecureStringToGlobalAllocUnicode(s);
				string str = Marshal.PtrToStringUni(ptr);

				using (var sha = new SHA256Managed())
				{
					byte[] hash = sha.ComputeHash(Encoding.Unicode.GetBytes(str));
					foreach (byte b in hash)
						sb.Append(b.ToString("X2"));
				}

				return sb.ToString();
			}
			finally
			{
				Marshal.ZeroFreeGlobalAllocUnicode(ptr);
			}
		}
	}
}
