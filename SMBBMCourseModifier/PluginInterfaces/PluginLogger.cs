using System;
using System.Collections.Generic;
using System.Text;

namespace SMBBMCourseModifier
{
    public interface PluginLogger
    {
        void LogDebug(string message);
        void LogInfo(string message);
    }
}
