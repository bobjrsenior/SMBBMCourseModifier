using System.IO;

namespace SMBBMCourseModifier.BMM
{
    public class BMMJsonLoader : JsonLoader
    {
        public T DeserializeJson<T>(string filepath)
        {
            // BMM had the dlls for System.Text.Json included
            // and has issues with extra dll libs so using it made
            // miore sense than Newtonsoft
            return System.Text.Json.JsonSerializer.Deserialize<T>(File.ReadAllText(filepath));
        }
    }
}
