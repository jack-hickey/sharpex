using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Sharpex.Extenders
{
    public static class Extenders
    {
        public static T ToInstance<T>(this string xml)
        {
            using (StringReader reader = new StringReader(xml))
            {
                Type tarType = typeof(T);

                return (T)new XmlSerializer(tarType).
                    Deserialize(reader);
            }
        }

        public static string ToXML(this object instance)
        {
            using (StringWriter writer = new StringWriter())
            {
                new XmlSerializer(instance.GetType()).
                    Serialize(writer, instance);

                return writer.ToString();
            }
        }

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
