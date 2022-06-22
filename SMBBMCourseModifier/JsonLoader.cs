using System;
using System.Collections.Generic;
using System.Text;

namespace SMBBMCourseModifier
{
    public interface JsonLoader
    {
        T DeserializeJson<T>(string filepath);
    }
}
