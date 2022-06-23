using Flash2;
using HarmonyLib;
using System;
using UnhollowerBaseLib;
using UnhollowerBaseLib.Runtime;
using UnhollowerRuntimeLib;

namespace SMBBMCourseModifier
{
    public class MgCourseDatumElement_tPatch
    {
        // BMM doesn't support IL2CPP Harmony Patches
        // So we a Detour instead
        private delegate int GetNextStepDelegate(IntPtr _thisPtr, MainGameDef.eGoalKind in_goalKind);
        private static GetNextStepDelegate GetNextStepDelegateInstance;

        public static unsafe void CreateDetour()
        {
            GetNextStepDelegateInstance = MgCourseDatumElement_tPatch.GetNextStepDetour;

            var original = typeof(MgCourseDatum.element_t).GetMethod(nameof(MgCourseDatum.element_t.GetNextStep), AccessTools.all);
            var methodInfo = UnityVersionHandler.Wrap((Il2CppMethodInfo*)(IntPtr)UnhollowerUtils.GetIl2CppMethodInfoPointerFieldForGeneratedMethod(original).GetValue(null));

            ClassInjector.Detour.Detour(methodInfo.MethodPointer,
                               GetNextStepDelegateInstance);
        }

        public static int GetNextStepDetour(IntPtr _thisPtr, MainGameDef.eGoalKind in_goalKind)
        {
            PluginResources.PluginLogger.LogDebug("GetNextStep Detour Method");

            MgCourseDatum.element_t self = new(_thisPtr);
            // The base game converts negative and zero nextStep values to 1. The code below
            // is to add support for custom courses with zero or negative values (ex: to go backwards in a course)
            // That being said, some base game stages in a course have zero values for some reason... so we need to
            // default to base game behaviour for base game courses instead of our custom support
            if (PluginResources.courses.ContainsKey(MgCourseDataManager.GetCurrentCourseDatum().m_CourseStr))
            {
                foreach (MgCourseDatum.goal_t goal in self.m_goalList)
                {
                    if (goal.m_goalKind == in_goalKind)
                    {
                        return goal.m_nextStep;
                    }
                }
                // Default values if the goal color isn't defined
                switch (in_goalKind)
                {
                    case MainGameDef.eGoalKind.Blue:
                        return 1;
                    case MainGameDef.eGoalKind.Green:
                        return 2;
                    case MainGameDef.eGoalKind.Red:
                        return 3; ;
                }
            }

            // Recreate the built-in method
            // 0 and negative values default to 1
            // If a goal kind isn't defined, it defaults to 1
            foreach (MgCourseDatum.goal_t goal in self.m_goalList)
            {
                if (goal.m_goalKind == in_goalKind)
                {
                    if (goal.m_nextStep <= 0)
                        return 1;
                    return goal.m_nextStep;
                }
            }
            return 1;
        }
    }
}
