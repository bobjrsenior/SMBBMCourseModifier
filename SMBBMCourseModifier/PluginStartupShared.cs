using HarmonyLib;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace SMBBMCourseModifier
{
    public class PluginStartupShared
    {
        public void Load()
        {
            //TODO PluginStartupShared.PluginResources.pluginLogger = base.PluginResources.pluginLogger;

            // Make sure the UserData and plugin data directories exist
            // The exists check isn't needed but is included for PluginResources.pluginLoggerging purposes
            if (!Directory.Exists(PluginResources.userDataDir))
            {
                Directory.CreateDirectory(PluginResources.userDataDir);
                PluginResources.pluginLogger.LogInfo("Created UserData folder since it didn't already exist");
            }
            if (!Directory.Exists(PluginResources.dataDir))
            {
                Directory.CreateDirectory(PluginResources.dataDir);
               PluginResources.pluginLogger.LogInfo($"Created {PluginResources.dataDir} folder since it didn't already exist");
            }

            // Find and load all the configuration JSON files
            PluginResources.courses = new();

            foreach (var file in Directory.EnumerateFiles(PluginResources.dataDir, "*.json", SearchOption.TopDirectoryOnly))
            {
                LoadJSONFile(file);
            }

            var dict = "";
            foreach (KeyValuePair<string, CourseDef> course in PluginResources.courses)
            {
                dict += $"\"{course.Key}\", \"{course.Value}\"\n";
            }
            PluginResources.pluginLogger.LogInfo($"Final Course List JSON is {{{dict}}}");

            // If we are patching something, make sure to disable the leaderboards
            if (PluginResources.courses.Count > 0)
            {
                PluginResources.LeaderboardDisabler.DisableLeaderboards(PluginResources.PLUGIN_NAME);
            }

            // Harmony Patching
            var harmony = new Harmony("com.bobjrsenior.SMBBMCourseModifier");
            harmony.PatchAll();

            //TODO AddComponent<DelayedCourseModifier>();
            PluginResources.pluginLogger.LogInfo($"Plugin {PluginResources.PLUGIN_NAME} is loaded!");
        }

        /// <summary>
        /// Loads the configuration settings from a given JSON file
        /// </summary>
        /// <param name="filepath">filepath of the JSON file to load</param>
        private void LoadJSONFile(string filepath)
        {
            PluginResources.pluginLogger.LogInfo($"Loading file {filepath}");

            using (StreamReader file = File.OpenText(filepath))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                // Serializethe JSON file into a C# one
                JObject obj = JToken.ReadFrom(reader) as JObject;
                CourseModDef courseModDef = obj.ToObject<CourseModDef>();
                MergeCourses(courseModDef.course_defs);
                PluginResources.pluginLogger.LogInfo($"Loaded: {courseModDef}");
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
                    PluginResources.courses[course.Key] = course.Value;
                }
            }
        }
    }
}
