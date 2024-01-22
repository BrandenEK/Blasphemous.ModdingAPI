using ModdingAPI;

namespace Blasphemous.ModdingAPI
{
    public class ModdingAPI : Mod
    {
        public ModdingAPI() : base(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION) { }

        protected override void Initialize()
        {
            Log($"{PluginInfo.PLUGIN_NAME} has been initialized");
        }
    }
}
