using System;
using System.Collections.Generic;

namespace SMBBMCourseModifier
{
    internal class CourseModDef
    {
        public string name { get; set; }
        public string description { get; set; }
        public string author { get; set; }
        public int file_format_version { get; set; }
        public Dictionary<String, CourseDef> course_defs { get; set; }
    }
}
