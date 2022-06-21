using System;
using System.Collections.Generic;
using System.IO;

namespace SMBBMCourseModifier
{
    public class PluginResources
    {
        /// <summary>
        /// The directory that plugin user data is stored in
        /// </summary>
        public static string userDataDir;

        /// <summary>
        /// The name of this plugins data directory
        /// </summary>
        public static readonly string dataDirName = "CourseDefinitions";

        /// <summary>
        /// The directory that data for this Plugin is expected
        /// </summary>
        public static string dataDir;

        /// <summary>
        /// A Key/Value map of Course Names to patch
        /// Key: Course name
        /// Value: Course data to patch in
        /// </summary>
        internal static Dictionary<string, CourseDef> courses;

        public static ModLoader MOD_LOADER;

        public static PluginLogger pluginLogger;

        public static LeaderboardDisabler LeaderboardDisabler;

        public static string PLUGIN_NAME;

        public static void InitializePluginResources(ModLoader modLoader, string pluginName, PluginLogger pluginLogger, String gameRootPath, LeaderboardDisabler leaderboardDisabler = null)
        {
            PLUGIN_NAME = pluginName;
            MOD_LOADER = modLoader;
            PluginResources.pluginLogger = pluginLogger;
            userDataDir = $"{gameRootPath}{Path.DirectorySeparatorChar}UserData";
            dataDir = $"{userDataDir}{Path.DirectorySeparatorChar}{dataDirName}";
            LeaderboardDisabler = leaderboardDisabler;
        }
    }
}
