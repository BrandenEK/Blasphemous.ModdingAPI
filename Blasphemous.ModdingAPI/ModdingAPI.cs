using Blasphemous.ModdingAPI.Input;
using Blasphemous.ModdingAPI.Items;
using Blasphemous.ModdingAPI.Penitence;
using Blasphemous.ModdingAPI.Skins;
using System.Collections.Generic;
using UnityEngine;

namespace Blasphemous.ModdingAPI;

internal class ModdingAPI : BlasMod
{
    public ModdingAPI() : base(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION) { }

    public ItemHandler ItemHandler { get; } = new();
    public SkinLoader SkinLoader { get; } = new();
    public PenitenceHandler PenitenceHandler { get; } = new();

    protected internal override void OnInitialize()
    {
        LocalizationHandler.RegisterDefaultLanguage("en");
        InputHandler.RegisterDefaultKeybindings(new Dictionary<string, KeyCode>()
        {
            { "Console", KeyCode.Backslash }
        });
    }

    protected internal override void OnAllInitialized()
    {
        ItemHandler.Initialize();
        PenitenceHandler.Initialize();
    }

    protected internal override void OnNewGame()
    {
        ItemHandler.NewGame();
    }

    protected internal override void OnUpdate()
    {
        PenitenceHandler.Update();
    }
}
