#if BIE
namespace SMBBMCourseModifier.BepInEx
{
    public class BepInExLeaderboardDisabler : LeaderboardDisabler
    {
        public void DisableLeaderboards()
        {
            SMBBMLeaderboardDisabler.Plugin.DisableLeaderboards();
        }

        public void DisableLeaderboards(string disabler)
        {
            SMBBMLeaderboardDisabler.Plugin.DisableLeaderboards(disabler);
        }
    }
}
#endif