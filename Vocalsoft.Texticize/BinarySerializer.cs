using System;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;

namespace Vocalsoft.Serialization
{
    public static class BinarySerializer
    {
        #region Binary Serialization Methods

        /// <summary>
        /// Converts a serializable object to its base64 string representation.
        /// </summary>
        /// <param name="source">The serializable object to convert.</param>
        /// <returns>Base64 string representation of the source.</returns>
        public static string ObjectToBase64String(object source)
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, source);
            return Convert.ToBase64String(stream.ToArray());
        }

        public static void ObjectToFile(Uri path, object source)
        {
            WriteToFile(path, ObjectToBase64String(source));
        }

        /// <summary>
        /// Converts a Base64 string to an object.
        /// </summary>
        /// <param name="source">The source Base64 string.</param>
        /// <returns>Original object that this Base64 string was generated from.</returns>
        public static object Base64StringToObject(string source)
        {
            try
            {
                MemoryStream stream = new MemoryStream(Convert.FromBase64String(source));
                BinaryFormatter formatter = new BinaryFormatter();
                return formatter.Deserialize(stream);
            }
            catch
            {
                return source;
            }
        }

        public static object FileToObject(Uri path)
        {
            return Base64StringToObject(ReadFromFile(path));
        }

        public static T Base64StringToObject<T>(string source)
        {
            return (T)Base64StringToObject(source);
        }

        public static T FileToObject<T>(Uri path)
        {
            return Base64StringToObject<T>(ReadFromFile(path));
        }


        #endregion

        private static string ReadFromFile(Uri path)
        {
            return File.ReadAllText(path.LocalPath, Encoding.ASCII);
        }

        private static void WriteToFile(Uri path, string content)
        {
            File.WriteAllText(path.LocalPath, content);
        }


    }
}
