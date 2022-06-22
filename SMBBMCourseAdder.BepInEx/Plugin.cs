using BepInEx;
using BepInEx.IL2CPP;
using BepInEx.Logging;

namespace SMBBMCourseModifier.BepInEx
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    [BepInDependency(SMBBMLeaderboardDisabler.PluginInfo.PLUGIN_GUID, BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency(JsonLibs.PluginInfo.PLUGIN_GUID, BepInDependency.DependencyFlags.HardDependency)]
    public class Plugin : BasePlugin
    {
        /// <summary>
        /// For logging convienence
        /// </summary>
        internal static new ManualLogSource Log;

        public override void Load()
        {
            Plugin.Log = base.Log;
            PluginResources.InitializePluginResources(ModLoader.BEPINEX, PluginInfo.PLUGIN_NAME, new BepInExPluginLogger(), Paths.GameRootPath, new BepInExLeaderboardDisabler());
            PluginStartupShared plugin = new();
            plugin.Load();
            AddComponent<DelayedCourseModifier>();
        }
    }
}
