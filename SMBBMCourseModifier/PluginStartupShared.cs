using HarmonyLib;
using System.Collections.Generic;
using System.IO;

namespace SMBBMCourseModifier
{
    public class PluginStartupShared
    {
        public void Load()
        {
            //TODO PluginStartupShared.PluginResources.PluginLogger = base.PluginResources.PluginLogger;

            // Make sure the UserData and plugin data directories exist
            // The exists check isn't needed but is included for PluginResources.pluginLoggerging purposes
            if (!Directory.Exists(PluginResources.userDataDir))
            {
                Directory.CreateDirectory(PluginResources.userDataDir);
                PluginResources.PluginLogger.LogInfo("Created UserData folder since it didn't already exist");
            }
            if (!Directory.Exists(PluginResources.dataDir))
            {
                Directory.CreateDirectory(PluginResources.dataDir);
                PluginResources.PluginLogger.LogInfo($"Created {PluginResources.dataDir} folder since it didn't already exist");
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
            PluginResources.PluginLogger.LogInfo($"Final Course List JSON is {{{dict}}}");

            // If we are patching something, make sure to disable the leaderboards
            if (PluginResources.courses.Count > 0)
            {
                PluginResources.LeaderboardDisabler.DisableLeaderboards(PluginResources.PLUGIN_NAME);
            }

            // Harmony Patching
            var harmony = new Harmony("com.bobjrsenior.SMBBMCourseModifier");
            harmony.PatchAll();

            //TODO AddComponent<DelayedCourseModifier>();
            PluginResources.PluginLogger.LogInfo($"Plugin {PluginResources.PLUGIN_NAME} is loaded!");
        }

        /// <summary>
        /// Loads the configuration settings from a given JSON file
        /// </summary>
        /// <param name="filepath">filepath of the JSON file to load</param>
        private void LoadJSONFile(string filepath)
        {
            PluginResources.PluginLogger.LogInfo($"Loading file {filepath}");


            // Serializethe JSON file into a C# one
            var courseModDef = PluginResources.JsonLoader.DeserializeJson<CourseModDef>(filepath);

            MergeCourses(courseModDef.course_defs);
            PluginResources.PluginLogger.LogInfo($"Loaded: {courseModDef}");
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
