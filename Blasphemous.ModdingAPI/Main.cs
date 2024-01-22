using BepInEx;

namespace Blasphemous.ModdingAPI
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Main : BaseUnityPlugin
    {
        public static ModdingAPI ModdingAPI { get; private set; }

        private void Start()
        {
            ModdingAPI = new ModdingAPI();
        }
    }
}
