using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Hcs.Platform
{
    public static class ObjectSerializerExtensions
    {
        public static byte[] SerializeObjectToBinary<T>(this T obj)
        {
            var binaryFormatter = new BinaryFormatter();
            using (var memoryStream = new MemoryStream())
            {
                binaryFormatter.Serialize(memoryStream, obj);
                return memoryStream.ToArray();
            }
        }
        public static T DeserializeBinaryToObject<T>(this byte[] bytes)
        {
            using (var memoryStream = new MemoryStream())
            {
                var binaryFormatter = new BinaryFormatter();
                memoryStream.Write(bytes, 0, bytes.Length);
                memoryStream.Seek(0, SeekOrigin.Begin);
                var obj = binaryFormatter.Deserialize(memoryStream);
                return (T)obj;
            }
        }
    }
}
