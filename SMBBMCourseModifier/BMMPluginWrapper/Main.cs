﻿#if BMM
using System;
using System.Collections.Generic;
using System.IO;
using UnhollowerRuntimeLib;
using UnityEngine;

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
            return listInjectedTypes;
        }

        public static void OnModUpdate()
        {
            if (delayedCourseModifier == null)
            {
                // Create Detours
                MgCourseDatumElement_tPatch.CreateDetour();

                // Register our MonoBehaviour
                // For some strange reason, it won't be injected automatically when returned
                // by OnModLoad if the MgCourseDatumElement_tPatch.GetNextStepDelegate is defined
                // I couldn't figure out why because it makes no sense but this gets around the issue.
                ClassInjector.RegisterTypeInIl2Cpp(typeof(DelayedCourseModifier));

                // Create an Object to run our MonoBehaviour
                var obj = new GameObject { hideFlags = HideFlags.HideAndDontSave };
                UnityEngine.Object.DontDestroyOnLoad(obj);
                delayedCourseModifier = new DelayedCourseModifier(obj.AddComponent(Il2CppType.Of<DelayedCourseModifier>()).Pointer);
            }
        }
    }
}
#endif