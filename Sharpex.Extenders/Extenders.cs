using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Xml.Serialization;

namespace Sharpex.Extenders
{
    public static class Extenders
    {
        public static string GetDescription<T>(this T enumValue) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
                return "";

            string description = enumValue.ToString();

            if (enumValue.GetType().GetField(enumValue.ToString()) is FieldInfo fieldInfo)
            {
                if (fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), true) is object[] attrs
                    && attrs.Length > 0)
                {
                    description = ((DescriptionAttribute)attrs[0]).Description;
                }
            }

            return description;
        }

    public static string ToJSON(this object instance)
        {
            return new JavaScriptSerializer().Serialize(instance);
        }

        public static T ToInstance<T>(this string value, SerializationType serializationType)
        {
            switch (serializationType)
            {
                case SerializationType.Xml:
                    using (StringReader reader = new StringReader(value))
                    {
                        Type tarType = typeof(T);

                        return (T)new XmlSerializer(tarType).
                            Deserialize(reader);
                    }
                case SerializationType.Json:
                    return (T)new JavaScriptSerializer().Deserialize(value, typeof(T));
                default:
                    return default;
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
