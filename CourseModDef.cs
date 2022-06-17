using System;
using System.Collections.Generic;
using System.Text;

namespace SMBBMCourseModifier
{
    internal class CourseModDef
    {
        public string name;
        public string description;
        public string author;
        public int file_format_version;
        public Dictionary<String, CourseDef> course_defs;

    }
}
