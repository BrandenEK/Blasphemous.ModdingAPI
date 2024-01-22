using UnityEngine;

namespace Blasphemous.ModdingAPI;

internal class ModdingAPI : BlasMod
{
    public ModdingAPI() : base(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION) { }

    protected internal override void OnInitialize()
    {
        LogWarning("Initialize");
    }

    protected internal override void OnAllInitialized()
    {
        LogWarning("All initialized");
    }

    protected internal override void OnDispose()
    {
        LogWarning("Dispose");
    }

    protected internal override void OnExitGame()
    {
        LogWarning("Exit game");
    }

    protected internal override void OnLateUpdate()
    {
        if (Time.frameCount % 120 == 0)
            LogWarning("Late update");
    }

    protected internal override void OnLevelLoaded(string oldLevel, string newLevel)
    {
        LogWarning("load: " + newLevel);
        throw new System.Exception("Test");
    }

    protected internal override void OnLevelPreloaded(string oldLevel, string newLevel)
    {
        LogWarning("Pre load: " + newLevel);
    }

    protected internal override void OnLevelUnloaded(string oldLevel, string newLevel)
    {
        LogWarning("Unload: " + oldLevel);
    }

    protected internal override void OnLoadGame()
    {
        LogWarning("Load game");
    }

    protected internal override void OnNewGame()
    {
        LogWarning("New game");
    }

    protected internal override void OnUpdate()
    {
        if (Time.frameCount % 120 == 0)
            LogWarning("Update");
    }
}
