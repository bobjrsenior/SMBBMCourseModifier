using System;
using System.Collections.Generic;
using System.Text;
using Flash2;
using HarmonyLib;

namespace SMBBMCourseModifier
{
    [HarmonyPatch(typeof(MgCourseDatum.element_t))]
    public class MgCourseDatumElement_tPatch
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(MgCourseDatum.element_t.GetNextStep),
            new[] { typeof(MainGameDef.eGoalKind) })]
        static bool GetNextStep(MgCourseDatum.element_t __instance, ref int __result, MainGameDef.eGoalKind in_goalKind)
        {
            // The base game converts negative and zero nextStep values to 1. The code below
            // is to add support for custom courses with zero or negative values (ex: to go backwards in a course)
            // That being said, some base game stages in a course have zero values for some reason... so we need to
            // default to base game behaviour for base game courses instead of our custom support
            if (PluginResources.courses.ContainsKey(MgCourseDataManager.GetCurrentCourseDatum().m_CourseStr))
            {
                foreach (MgCourseDatum.goal_t goal in __instance.m_goalList)
                {
                    if (goal.m_goalKind == in_goalKind)
                    {
                        __result = goal.m_nextStep;
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
