using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace ViewModel.Util
{
    public static class Crypto
    {
        /// <summary>
        /// Returns SHA256 hash as string of 64 hexadecimal digits for the specified SecureString
        /// </summary>
        public static string GetHashString(this SecureString value)
        {
            return GetHashString(value.GetString());
        }

        /// <summary>
        /// Returns SHA256 hash as string of 64 hexadecimal digits for the specified string
        /// </summary>
        public static string GetHashString(this string value)
        {
            StringBuilder sb = new StringBuilder(64);
            using (var sha = new SHA256Managed())
            {
                byte[] hash = sha.ComputeHash(Encoding.Unicode.GetBytes(value));
                foreach (byte b in hash)
                {
                    sb.Append(b.ToString("X2", CultureInfo.InvariantCulture));
                }
            }

            return sb.ToString();
        }

        public static string GetString(this SecureString value)
        {
            IntPtr ptr = IntPtr.Zero;
            try
            {
                ptr = Marshal.SecureStringToGlobalAllocUnicode(value);
                return Marshal.PtrToStringUni(ptr);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(ptr);
            }
        }
    }
}
