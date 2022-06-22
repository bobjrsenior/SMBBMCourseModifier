using System;
using System.Collections.Generic;
using System.Text;

namespace SMBBMCourseModifier
{
    public interface JsonLoader
    {
        // I had issues using the same JSON loader across BMM and BepInEx...
        // This is so that they can each provide their own
        T DeserializeJson<T>(string filepath);
    }
}
