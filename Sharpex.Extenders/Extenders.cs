using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Sharpex.Extenders
{
    public static class Extenders
    {
        public static string ToRealString(this SecureString value)
        {
            IntPtr valuePtr = IntPtr.Zero;

            try
            {
                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(value);

                return Marshal.PtrToStringUni(valuePtr);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }
        }

        public static string GetHash(this string input)
        {
            using (SHA256 engine = SHA256.Create())
            {
                byte[] data = engine.ComputeHash(
                        Encoding.UTF8.GetBytes(input)
                    );

                return string.Join("",
                        data.Select(x => x.ToString("x2"))
                    );
            }
        }

        public static T ToEnum<T>(this string text, bool ignoreCase = true) where T : struct
        {
            return Enum.TryParse(text, ignoreCase, out T parsed) ? parsed : default;
        }
    }
}
