using System;
using System.Collections.Generic;
using System.Text;

namespace SMBBMCourseModifier.BMM
{
    public class BMMPluginLogger : PluginLogger
    {
        public void LogDebug(string message)
        {
            Console.WriteLine($"DEBUG: {message}");
        }

        public void LogInfo(string message)
        {
            Console.WriteLine($"INFO: {message}");
        }
    }
}
