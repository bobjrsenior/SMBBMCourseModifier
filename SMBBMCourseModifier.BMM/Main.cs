using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SMBBMCourseModifier.BMM
{
    public class Main
    {
        private static DelayedCourseModifier delayedCourseModifier;

        public static List<Type> OnModLoad(Dictionary<string, object> settings)
        {
            PluginResources.InitializePluginResources(ModLoader.BEPINEX, "SMBBM Course Modifier", new BMMPluginLogger(), Directory.GetCurrentDirectory(), new BMMJsonLoader(), new BMMLeaderboardDisabler());
            PluginStartupShared plugin = new();
            plugin.Load();

            List<Type> listInjectedTypes = new(1);
            listInjectedTypes.Add(typeof(DelayedCourseModifier));
            return listInjectedTypes;
        }

        public static void OnModUpdate()
        {
            if (delayedCourseModifier == null)
            {
                var obj = new GameObject("SMBBMDelayedCourseModifier");
                UnityEngine.Object.DontDestroyOnLoad(obj);
                delayedCourseModifier = obj.AddComponent<DelayedCourseModifier>();
            }
        }
    }
}
