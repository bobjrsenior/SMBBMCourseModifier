using System.Collections.Generic;

namespace SMBBMCourseModifier
{
    internal class CourseDef
    {
        public string next_course { get; set; } = null;
        public string start_movie_id { get; set; }
        public string end_movie_id { get; set; }
        public IList<CourseElementDef> course_stages { get; set; }
    }
}
