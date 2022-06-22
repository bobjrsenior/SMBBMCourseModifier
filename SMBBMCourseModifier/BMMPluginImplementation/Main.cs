using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnhollowerRuntimeLib;

namespace SMBBMCourseModifier.BMM
{
    public class Main
    {
        private static DelayedCourseModifier delayedCourseModifier;

        public static List<Type> OnModLoad(Dictionary<string, object> settings)
        {
            PluginResources.InitializePluginResources(ModLoader.BEPINEX, "SMBBM Course Modifier", new BMMPluginLogger(), Directory.GetCurrentDirectory(), new BMMLeaderboardDisabler());
            PluginStartupShared plugin = new();
            plugin.Load();

            List<Type> listInjectedTypes = new(1);
            listInjectedTypes.Add(typeof(DelayedCourseModifier));
            Console.WriteLine("Loaded Plugin Modifier");
            return listInjectedTypes;
        }

        public static void OnModUpdate()
        {
            if (delayedCourseModifier == null)
            {
                //Console.WriteLine("Trying to inject DelayedCourseModifier Component");
                //UnhollowerRuntimeLib.ClassInjector.RegisterTypeInIl2Cpp<DelayedCourseModifier>(UnhollowerRuntimeLib.RegisterTypeOptions.Default);
                var obj = new GameObject { hideFlags = HideFlags.HideAndDontSave };
                UnityEngine.Object.DontDestroyOnLoad(obj);
                Console.WriteLine("Trying to add DelayedCourseModifier Component");
                delayedCourseModifier = new DelayedCourseModifier(obj.AddComponent(Il2CppType.Of<DelayedCourseModifier>()).Pointer);
            }
        }
    }
}
