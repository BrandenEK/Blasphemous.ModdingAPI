using Blasphemous.ModdingAPI.Input;
using Blasphemous.ModdingAPI.Items;
using Blasphemous.ModdingAPI.Levels;
using Blasphemous.ModdingAPI.Levels.Loaders;
using Blasphemous.ModdingAPI.Levels.Modifiers;
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
        LevelHandler.Initialize();
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
        provider.RegisterObjectCreator("item", new ObjectCreator(
            new SceneLoader("D02Z02S14_LOGIC", "LOGIC/INTERACTABLES/ACT_Collectible"),
            new CollectibleItemModifier()));

        IModifier chestModifier = new ChestModifier();
        provider.RegisterObjectCreator("chest-iron", new ObjectCreator(
            new SceneLoader("D01Z05S11_LOGIC", "LOGIC/INTERACTABLES/ACT_Iron Chest"), chestModifier));
        provider.RegisterObjectCreator("chest-gold", new ObjectCreator(
            new SceneLoader("D20Z02S02_LOGIC", "ACT_Wooden Chest"), chestModifier));
        provider.RegisterObjectCreator("chest-relic", new ObjectCreator(
            new SceneLoader("D17BZ01S01_LOGIC", "LOGIC/INTERACTABLES/ACT_Relicarium"), chestModifier));
    }
}
