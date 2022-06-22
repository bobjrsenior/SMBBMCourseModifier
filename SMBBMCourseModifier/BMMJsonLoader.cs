using System.IO;

namespace SMBBMCourseModifier.BMM
{
    public class BMMJsonLoader : JsonLoader
    {
        public T DeserializeJson<T>(string filepath)
        {
            return System.Text.Json.JsonSerializer.Deserialize<T>(File.ReadAllText(filepath));
            //using (StreamReader file = File.OpenText(filepath))
            //{
            //    // Serialize the JSON file into a C# one
            //    return System.Text.Json.JsonSerializer.Deserialize<T>(file.BaseStream);
            //}
        }
    }
}
