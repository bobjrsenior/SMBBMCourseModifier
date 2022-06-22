using System;
using System.Collections.Generic;
using System.Text;

namespace SMBBMCourseModifier
{
    public interface LeaderboardDisabler
    {
        void DisableLeaderboards();
        void DisableLeaderboards(string disabler);
    }
}
