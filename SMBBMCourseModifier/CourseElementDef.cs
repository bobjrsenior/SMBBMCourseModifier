using System.Collections.Generic;

namespace SMBBMCourseModifier
{
    internal class CourseElementDef
    {
        public bool is_check_point { get; set; }
        public bool is_half_time { get; set; }
        public int stage_id { get; set; }
        public IList<CourseGoalDef> goals { get; set; }
    }
}
