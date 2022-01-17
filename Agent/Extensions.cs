
using System.IO;
using System.Runtime.Serialization.Json;

namespace Agent
{
    public static class Extensions
    {
        public static byte[] Serialize<T>(this T data)
        {
            var serializer = new DataContractJsonSerializer(typeof(T));

            using (var ms = new MemoryStream())
            {
                serializer.WriteObject(ms, data);
                return ms.ToArray();
            }
        }

        public static T Deserialize<T>(this byte[] data)
        {
            var serializer = new DataContractJsonSerializer(typeof(T));

            using (var ms = new MemoryStream())
            {
                return (T)serializer.ReadObject(ms);
            }
        }
    }
}
