using BepInEx;
using BepInEx.IL2CPP;
using BepInEx.Logging;
using Flash2;
using HarmonyLib;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace SMBBMCourseModifier
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    [BepInDependency(SMBBMLeaderboardDisabler.PluginInfo.PLUGIN_GUID, BepInDependency.DependencyFlags.HardDependency)]
    public class Plugin : BasePlugin
    {
        internal static new ManualLogSource Log;

        /// <summary>
        /// The directory that plugin user data is stored in
        /// </summary>
        public static readonly string userDataDir = $"{Paths.GameRootPath}{Path.DirectorySeparatorChar}UserData";

        /// <summary>
        /// The name of this plugins data directory
        /// </summary>
        public static readonly string dataDirName = "CourseDefinitions";

        /// <summary>
        /// The directory that data for this Plugin is expected
        /// </summary>
        public static readonly string dataDir = $"{userDataDir}{Path.DirectorySeparatorChar}{dataDirName}";

        /// <summary>
        /// A Key/Value map of Course Names to patch
        /// Key: Course name
        /// Value: Course data to patch in
        /// </summary>
        internal static Dictionary<string, CourseDef> courses;

        public override void Load()
        {
            Plugin.Log = base.Log;

            // Make sure the UserData and plugin data directories exist
            // The exists check isn't needed but is included for logging purposes
            if (!Directory.Exists(userDataDir))
            {
                Directory.CreateDirectory(userDataDir);
                Log.LogInfo("Created UserData folder since it didn't already exist");
            }
            if (!Directory.Exists(dataDir))
            {
                Directory.CreateDirectory(dataDir);
                Log.LogInfo($"Created {dataDir} folder since it didn't already exist");
            }

            // Find and load all the configuration JSON files
            courses = new();

            foreach (var file in Directory.EnumerateFiles(dataDir, "*.json", SearchOption.TopDirectoryOnly))
            {
                LoadJSONFile(file);
            }

            var dict = "";
            foreach (KeyValuePair<string, CourseDef> course in courses)
            {
                dict += $"\"{course.Key}\", \"{course.Value}\"\n";
            }
            Log.LogInfo($"Final Course List JSON is {{{dict}}}");

            // If we are patching something, make sure to disable the leaderboards
            if (courses.Count > 0)
            {
                SMBBMLeaderboardDisabler.Plugin.DisableLeaderboards(PluginInfo.PLUGIN_NAME);
            }

            // Harmony Patching
            var harmony = new Harmony("com.bobjrsenior.SMBBMCourseModifier");
            harmony.PatchAll();

            AddComponent<CourseModifier>();
            Log.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
        }

        /// <summary>
        /// Loads the configuration settings from a given JSON file
        /// </summary>
        /// <param name="filepath">filepath of the JSON file to load</param>
        private void LoadJSONFile(string filepath)
        {
            Log.LogInfo($"Loading file {filepath}");

            using (StreamReader file = File.OpenText(filepath))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                // Serializethe JSON file into a C# one
                JObject obj = JToken.ReadFrom(reader) as JObject;
                CourseModDef courseModDef = obj.ToObject<CourseModDef>();
                MergeCourses(courseModDef.course_defs);
                Log.LogInfo($"Loaded: {courseModDef}");
            }
        }

        /// <summary>
        /// Merges a dictionary of Course patches with the current Course mappings
        /// </summary>
        /// <param name="newCourses">Course->Course Definition mappings to merge</param>
        internal void MergeCourses(Dictionary<string, CourseDef> newCourses)
        {
            if (newCourses != null)
            {
                foreach (KeyValuePair<string, CourseDef> course in newCourses)
                {
                    courses[course.Key] = course.Value;
                }
            }
        }
    }
}
