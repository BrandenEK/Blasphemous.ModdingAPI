using Blasphemous.ModdingAPI.Input;
using UnityEngine;

namespace Blasphemous.ModdingAPI;

internal class ModdingAPI : BlasMod
{
    public ModdingAPI() : base(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION) { }

    protected internal override void OnInitialize()
    {
        LogWarning("Initialize");
        LocalizationHandler.RegisterDefaultLanguage("en");
        InputHandler.RegisterDefaultKeybindings(new System.Collections.Generic.Dictionary<string, KeyCode>()
        {
            { "test", KeyCode.P }
        });

        Log(LocalizationHandler.Localize("cr"));
    }

    protected internal override void OnAllInitialized()
    {
        LogWarning("All initialized");

        FileHandler.LoadDataAsSprite("test.png", out Sprite s, new Files.SpriteImportOptions()
        {
            PixelsPerUnit = 32,
            UsePointFilter = false
        });
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
        LogWarning("Load: " + newLevel);
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
