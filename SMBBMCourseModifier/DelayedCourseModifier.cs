using Flash2;
using System;
using System.Collections.Generic;
using UnhollowerBaseLib;
using UnhollowerRuntimeLib;
using UnityEngine;

namespace SMBBMCourseModifier
{
    public class DelayedCourseModifier : MonoBehaviour
    {
        private readonly float startupDelay = 2.0f;
        private float curDelay = 0.0f;
        private bool initializedCourseModifying = false;
        private bool initializedCourseAdding = true; // Disabled since it doesn't work

        private MgCourseDataManager dataManager;

        public DelayedCourseModifier(IntPtr value) : base(value) { }

        public DelayedCourseModifier() { }

        void Update()
        {
            if (!initializedCourseModifying || !initializedCourseAdding)
            {
                curDelay += Time.deltaTime;
                if (curDelay > startupDelay)
                {
                    if (!initializedCourseModifying)
                        InitializeCourseModifying();
                    if (!initializedCourseAdding)
                        InitializeCourseAdding();
                }
            }
        }

        private void InitializeCourseModifying()
        {
            PluginResources.PluginLogger.LogDebug("Initializing DelayedCourseModifier");
            initializedCourseModifying = true;

            // Check if the data manager is available
            dataManager = MgCourseDataManager.Instance;
            if (dataManager == null)
            {
                initializedCourseModifying = false;
                curDelay = 0.0f;
                PluginResources.PluginLogger.LogDebug("Data Manager is still null");
            }
            else
            {
                foreach (KeyValuePair<string, CourseDef> course in PluginResources.courses)
                {
                    // Convert the user provided course name to an internal Enum
                    MainGameDef.eCourse courseEnum;
                    if (!Enum.TryParse<MainGameDef.eCourse>(course.Key, out courseEnum))
                    {
                        PluginResources.PluginLogger.LogDebug($"Invalid Course Name: {course.Key}");
                        continue;
                    }
                    // See if it matches an existing course
                    if (dataManager.m_CourseDataDict.ContainsKey(courseEnum))
                    {
                        PluginResources.PluginLogger.LogDebug($"Patching Course: {courseEnum}");

                        // Get the existing course data
                        MgCourseDatum courseData = dataManager.m_CourseDataDict[courseEnum];

                        // Set course data
                        MainGameDef.eCourse nextCourse;
                        if (course.Value.next_course != null && Enum.TryParse<MainGameDef.eCourse>(course.Value.next_course, out nextCourse))
                        {
                            courseData.m_NextCourse = nextCourse;
                            courseData.m_NextCourseStr = course.Value.next_course;
                        }
                        MovieDef.eID movieId;
                        if (course.Value.start_movie_id != null && Enum.TryParse<MovieDef.eID>(course.Value.start_movie_id, out movieId))
                        {
                            courseData.m_StartMovieID = movieId;
                            courseData.m_StartMovieIDStr = course.Value.start_movie_id;
                        }
                        if (course.Value.end_movie_id != null && Enum.TryParse<MovieDef.eID>(course.Value.end_movie_id, out movieId))
                        {
                            courseData.m_EndMovieID = movieId;
                            courseData.m_EndMovieIDStr = course.Value.end_movie_id;
                        }

                        // Make a List for the Stage information
                        Il2CppSystem.Collections.Generic.List<MgCourseDatum.element_t> stages = new();
                        if (course.Value.course_stages != null)
                        {
                            foreach (CourseElementDef courseElement in course.Value.course_stages)
                            {
                                MgCourseDatum.element_t stage = new();
                                stage.m_IsCheckPoint = courseElement.is_check_point;
                                stage.m_IsHalfTime = courseElement.is_half_time;
                                stage.m_stageId = courseElement.stage_id;

                                PluginResources.PluginLogger.LogDebug($"Adding stage with id {courseElement.stage_id}");

                                // Collect Goal information
                                Il2CppSystem.Collections.Generic.List<MgCourseDatum.goal_t> goals = new();
                                if (courseElement.goals != null)
                                {
                                    foreach (CourseGoalDef goalDef in courseElement.goals)
                                    {
                                        MgCourseDatum.goal_t goal = new();
                                        MainGameDef.eGoalKind goalKind;
                                        if (!Enum.TryParse<MainGameDef.eGoalKind>(goalDef.goal_kind, out goalKind))
                                        {
                                            PluginResources.PluginLogger.LogDebug($"Invalid Goal Kind: {goalDef.goal_kind}");
                                            continue;
                                        }

                                        PluginResources.PluginLogger.LogDebug($"Adding goal {goalKind} for stage with id {courseElement.stage_id}");
                                        goal.m_goalKind = goalKind;
                                        goal.m_goalKindStr = goalDef.goal_kind;
                                        goal.m_nextStep = goalDef.next_step;
                                        goal.Initialize();
                                        goals.Add(goal);
                                    }
                                }
                                stage.m_goalList = goals;

                                stages.Add(stage);
                            }
                        }
                        courseData.m_elementList = stages;
                    }
                    else
                    {
                        PluginResources.PluginLogger.LogDebug($"Didn't find dictionary entry for {courseEnum}");
                    }
                }

            }
        }

        /// <summary>
        /// A helper method to log every base game course in a JSON format (+1 extra comma)
        /// </summary>
        /// <param name="dataManager">Data Manager from the game to pull from</param>
        /*private void LogCourses(MgCourseDataManager dataManager)
        {
            StringBuilder sb = new();
            sb
                .Append($"\n")
                .Append($"{{\n")
                .Append($"  \"course_defs\": {{\n");

            // Every course
            foreach (Il2CppSystem.Collections.Generic.KeyValuePair<MainGameDef.eCourse, MgCourseDatum> course in dataManager.m_CourseDataDict)
            {

                sb
                    .Append($"    \"{course.Key}\": {{\n")
                    .Append($"      \"next_course\": \"{course.Value.m_NextCourse}\",\n")
                    .Append($"      \"start_movie_id\": \"{course.Value.m_StartMovieID}\",\n")
                    .Append($"      \"end_movie_id\": \"{course.Value.m_EndMovieID}\",\n")
                    .Append($"      \"course_stages\": [\n");

                // Every stage in every course
                for (int j = 0; j < course.Value.m_elementList.Count; j++)
                {
                    MgCourseDatum.element_t stage = course.Value.m_elementList[j];
                    sb
                        .Append($"        {{\n")
                        .Append($"          \"is_check_point\": {JsonBool(stage.m_IsCheckPoint)},\n")
                        .Append($"          \"is_half_time\": {JsonBool(stage.m_IsHalfTime)},\n")
                        .Append($"          \"stage_id\": {stage.m_stageId},\n")
                        .Append($"          \"goals\": [\n");

                    // Every goal in every stage
                    for (int k = 0; k < stage.m_goalList.Count; k++)
                    {
                        MgCourseDatum.goal_t goal = stage.m_goalList[k];
                        sb
                            .Append($"            {{\n")
                            .Append($"              \"goal_kind\": \"{goal.m_goalKind}\",\n")
                            .Append($"              \"next_step\": {goal.m_nextStep}\n");
                        if (k == stage.m_goalList.Count - 1)
                            sb.Append($"            }}\n");
                        else
                            sb.Append($"            }},\n");
                    }
                    sb
                        .Append($"          ]\n");
                    if (j == course.Value.m_elementList.Count - 1)
                        sb.Append($"        }}\n");
                    else
                        sb.Append($"        }},\n");
                }
                sb
                    .Append($"      ]\n")
                    .Append($"    }},\n");
            }
            sb
                .Append($"  }}\n")
                .Append($"}}\n");
           PluginResources.PluginLogger.LogInfo(sb.ToString());

        }

        public string JsonBool(bool test)
        {
            return test ? "true" : "false";
        }*/

        // Eperimental, doesn't work right now
        private void InitializeCourseAdding()
        {
            initializedCourseAdding = true;

            dataManager = MgCourseDataManager.Instance;
            if (dataManager == null)
            {
                initializedCourseAdding = false;
                curDelay = 0.0f;
            }
            else
            {
                MainGameDef.eCourse neweCourse = (MainGameDef.eCourse)0x2C;

                MgCourseDatum oldCourseDatum = dataManager.m_CourseDataDict[MainGameDef.eCourse.Smb1_Casual];

                EnumInjector.InjectEnumValues(typeof(MainGameDef.eCourse), new()
                {
                    ["Smb1_Custom"] = 0x2C
                });

                // Proceed to make our own
                MgCourseDatum myCourseDatum = (MgCourseDatum)MgCourseDatum.CreateInstance<MgCourseDatum>();//UnhollowerRuntimeLib.Il2CppType.Of<MgCourseDatum>());
                myCourseDatum.m_Course = neweCourse;
                myCourseDatum.m_courseNameTextReference = oldCourseDatum.m_courseNameTextReference;
                myCourseDatum.m_CourseStr = "Smb1_Custom";
                myCourseDatum.name = "MgCourseDatum_Smb1_Custom";


                // TODO m_elementList
                Il2CppSystem.Collections.Generic.List<MgCourseDatum.element_t> elements = new(oldCourseDatum.m_elementList.Count);
                if (oldCourseDatum.m_elementList.Count > 0)
                {
                    foreach (MgCourseDatum.element_t element in oldCourseDatum.m_elementList)
                    {
                        elements.Add(element);
                    }
                }
                myCourseDatum.m_elementList = elements;
                myCourseDatum.m_EndMovieID = oldCourseDatum.m_EndMovieID;
                myCourseDatum.m_EndMovieIDStr = oldCourseDatum.m_EndMovieIDStr;
                myCourseDatum.m_GameKind = oldCourseDatum.m_GameKind;
                myCourseDatum.m_GameKindStr = oldCourseDatum.m_GameKindStr;
                myCourseDatum.m_NextCourse = oldCourseDatum.m_NextCourse;
                myCourseDatum.m_NextCourseStr = oldCourseDatum.m_NextCourseStr;
                myCourseDatum.m_StartMovieID = oldCourseDatum.m_StartMovieID;
                myCourseDatum.m_StartMovieIDStr = oldCourseDatum.m_StartMovieIDStr;
                myCourseDatum.m_ThumbnailSpritePath = oldCourseDatum.m_ThumbnailSpritePath;
                dataManager.m_CourseDataDict[myCourseDatum.m_Course] = myCourseDatum;
                Il2CppReferenceArray<MgCourseDatum> courseDatumArray = new(dataManager.m_MgCourseData.m_Entity.Count + 1);
                if (dataManager.m_MgCourseData.m_Entity.Count > 0)
                {
                    for (int i = 0; i < dataManager.m_MgCourseData.m_Entity.Count; i++)
                    {
                        courseDatumArray[i] = dataManager.m_MgCourseData.m_Entity[i];
                    }
                }
                //dataManager.m_MgCourseData.m_Entity.CopyTo(courseDatumArray, 0);
                courseDatumArray[courseDatumArray.Count - 1] = myCourseDatum;
                dataManager.m_MgCourseData.m_Entity = courseDatumArray;
                PluginResources.PluginLogger.LogDebug($"eCourse Id 0x2C, MgCourseData entry should be at index {courseDatumArray.Count - 1}");

                // Deal with the Main Game Def
                Il2CppStructArray<MainGameDef.eCourse> mainGameCourses = new(MainGameDef.s_DefaultUnlockedCourseArray.Count + 1);
                if (MainGameDef.s_DefaultUnlockedCourseArray.Count > 0)
                {
                    for (int i = 0; i < MainGameDef.s_DefaultUnlockedCourseArray.Count; i++)
                    {
                        mainGameCourses[i] = MainGameDef.s_DefaultUnlockedCourseArray[i];
                        if (MainGameDef.s_DefaultUnlockedCourseArray[i] == MainGameDef.eCourse.Invalid)
                        {
                            PluginResources.PluginLogger.LogDebug($"Value is null at index {i}");
                        }
                    }
                }
                //MainGameDef.s_DefaultUnlockedCourseArray.CopyTo(mainGameCourses, 0);
                mainGameCourses[mainGameCourses.Count - 1] = (MainGameDef.eCourse)0x2C;
                MainGameDef.s_DefaultUnlockedCourseArray = mainGameCourses;
                myCourseDatum.Initialize();

                // Deal with the UI
                Il2CppStructArray<MainGameDef.eCourse> mainMenuSequence = new(SelMainMenuSequence.s_MainGameCourses.Count + 1);
                if (SelMainMenuSequence.s_MainGameCourses.Count > 0)
                {
                    for (int i = 0; i < SelMainMenuSequence.s_MainGameCourses.Count; i++)
                    {
                        mainMenuSequence[i] = SelMainMenuSequence.s_MainGameCourses[i];
                    }
                }
                mainMenuSequence[mainMenuSequence.Count - 1] = neweCourse;


            }
        }
    }
}
