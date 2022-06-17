using System;
using System.Collections.Generic;
using System.Text;

namespace SMBBMCourseModifier
{
    internal class CourseDef
    {
        public string next_course = null;
        public bool add_a_default_blue_goal = true;
        public string start_movie_id;
        public string end_movie_id;
        public List<CourseElementDef> course_stages;
    }
}
