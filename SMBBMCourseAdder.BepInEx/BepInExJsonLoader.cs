using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace SMBBMCourseModifier.BepInEx
{
    public class BepInExJsonLoader : JsonLoader
    {
        public T DeserializeJson<T>(string filepath)
        {
            // Using Newtonsoft limits the extra dlls to 1. I also had
            // issues when I initially tried using System.Text.Json with BepInEx
            using (StreamReader file = File.OpenText(filepath))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                // Serializethe JSON file into a C# one
                JObject obj = JToken.ReadFrom(reader) as JObject;
                return obj.ToObject<T>();
            }
        }
    }
}
