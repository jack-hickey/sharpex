using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.CompilerServices;
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
        /// <summary>
        /// Check that a string is in valid email format
        /// </summary>
        /// <param name="address">The email address to validate</param>
        /// <returns>True if the input was a valid email address, otherwise false</returns>
        public static bool IsValidEmail(this string address)
        {
            try
            {
                System.Net.Mail.MailAddress addressAttempt = new System.Net.Mail.MailAddress(address);
                return addressAttempt.Address == address;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Get the description of an enum instance
        /// </summary>
        /// <typeparam name="T">The type of enum being dealt with</typeparam>
        /// <param name="enumValue">The instance to grab the description from</param>
        /// <returns>A string representing the description of the enum</returns>
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

        /// <summary>
        /// Serialize an object into JSON format
        /// </summary>
        /// <param name="instance">The object to serialize</param>
        /// <returns>A string representing the JSON data</returns>
        public static string ToJSON(this object instance)
        {
            return new JavaScriptSerializer().Serialize(instance);
        }

        /// <summary>
        /// Deserializes from either XML or JSON into a constructed object
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize the data into</typeparam>
        /// <param name="value">The JSON or XML data to deserialize</param>
        /// <param name="serializationType">Specified whether the input data is XML or JSON</param>
        /// <returns>A fully constructed object based on the input data and deserialization type</returns>
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

        /// <summary>
        /// Serialize an object into XML format
        /// </summary>
        /// <param name="instance">The object to serialize</param>
        /// <returns>A string representing the XML data</returns>
        public static string ToXML(this object instance)
        {
            using (StringWriter writer = new StringWriter())
            {
                new XmlSerializer(instance.GetType()).
                    Serialize(writer, instance);

                return writer.ToString();
            }
        }

        /// <summary>
        /// Converts a SecureString value to a normal string
        /// </summary>
        /// <param name="value">The SecureString instance to convert</param>
        /// <returns>A string representing the raw data stored in the SecureString</returns>
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

        /// <summary>
        /// Hash a string value using the SHA256 algorithm
        /// </summary>
        /// <param name="input">The input string to hash</param>
        /// <returns>A string representing the hashed value of the input data</returns>
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

        /// <summary>
        /// Directly cast a string into an enum value
        /// </summary>
        /// <typeparam name="T">The type of enum to convert into</typeparam>
        /// <param name="text">The string to convert from</param>
        /// <param name="ignoreCase">Determines whether or not to ignore case sensitivity</param>
        /// <returns>An instance of the enum, determined by the type parameter</returns>
        public static T ToEnum<T>(this string text, bool ignoreCase = true) where T : struct
        {
            return Enum.TryParse(text, ignoreCase, out T parsed) ? parsed : default;
        }
    }
}
