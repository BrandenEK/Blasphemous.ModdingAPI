using Blasphemous.ModdingAPI.Input;
using Blasphemous.ModdingAPI.Items;
using Blasphemous.ModdingAPI.Levels;
using Blasphemous.ModdingAPI.Penitence;
using Blasphemous.ModdingAPI.Skins;
using System.Collections.Generic;
using UnityEngine;

namespace Blasphemous.ModdingAPI;

internal class ModdingAPI : BlasMod
{
    public ModdingAPI() : base(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION) { }

    public ItemHandler ItemHandler { get; } = new();
    public LevelHandler LevelHandler { get; } = new();
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

    protected internal override void OnLevelPreloaded(string oldLevel, string newLevel)
    {
        LevelHandler.PreloadLevel(newLevel);
    }

    protected internal override void OnLevelLoaded(string oldLevel, string newLevel)
    {
        LevelHandler.LoadLevel(newLevel);
    }

    protected internal override void OnUpdate()
    {
        PenitenceHandler.Update();
    }

    protected internal override void OnRegisterServices(ModServiceProvider provider)
    {
        provider.RegisterObjectModifier("item", new ObjectModifier("D02Z02S14_LOGIC", "LOGIC/INTERACTABLES/ACT_Collectible",
            new CollectibleItemCreator()));

        IModifier chestModifier = new ChestCreator();
        provider.RegisterObjectModifier("chest-iron", new ObjectModifier("D01Z05S11_LOGIC", "LOGIC/INTERACTABLES/ACT_Iron Chest",
            chestModifier));
        provider.RegisterObjectModifier("chest-gold", new ObjectModifier("D20Z02S02_LOGIC", "ACT_Wooden Chest",
            chestModifier));
        provider.RegisterObjectModifier("chest-relic", new ObjectModifier("D17BZ01S01_LOGIC", "LOGIC/INTERACTABLES/ACT_Relicarium",
            chestModifier));
    }
}
