
namespace Blasphemous.ModdingAPI;

public class ModdingAPI : BlasMod
{
    public ModdingAPI() : base(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION) { }

    protected internal override void OnInitialize()
    {
        Log($"{PluginInfo.PLUGIN_NAME} has been initialized");
    }
}
