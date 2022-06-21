using System;
using System.Collections.Generic;
using System.Text;

namespace SMBBMCourseModifier
{
    internal class CourseElementDef
    {
        public bool is_check_point;
        public bool is_half_time;
        public int stage_id;
        public List<CourseGoalDef> goals;
    }
}
