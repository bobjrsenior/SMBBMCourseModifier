using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SMBBMCourseModifier.BepInEx
{
    public class BepInExJsonLoader : JsonLoader
    {
        public T DeserializeJson<T>(string filepath)
        {
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
