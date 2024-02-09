using Blasphemous.ModdingAPI.Levels;
using Blasphemous.ModdingAPI.Levels.Loaders;
using Blasphemous.ModdingAPI.Levels.Modifiers;
using Blasphemous.ModdingAPI.Skins;

namespace Blasphemous.ModdingAPI;

internal class ModdingAPI : BlasMod
{
    public ModdingAPI() : base(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_AUTHOR, ModInfo.MOD_VERSION) { }

    public LevelHandler LevelHandler { get; } = new();
    public SkinLoader SkinLoader { get; } = new();

    protected internal override void OnInitialize()
    {
        LocalizationHandler.RegisterDefaultLanguage("en");
    }

    protected internal override void OnAllInitialized()
    {
        LevelHandler.Initialize();
    }

    protected internal override void OnLevelPreloaded(string oldLevel, string newLevel)
    {
        LevelHandler.PreloadLevel(newLevel);
    }

    protected internal override void OnLevelLoaded(string oldLevel, string newLevel)
    {
        LevelHandler.LoadLevel(newLevel);
    }

    protected internal override void OnRegisterServices(ModServiceProvider provider)
    {
        // Items
        provider.RegisterObjectCreator("item-ground", new ObjectCreator(
            new SceneLoader("D02Z02S14_LOGIC", "LOGIC/INTERACTABLES/ACT_Collectible"),
            new GroundItemModifier()));
        provider.RegisterObjectCreator("item-shrine", new ObjectCreator(
            new SceneLoader("D06Z01S22_LOGIC", "LOGIC/INTERACTABLES"),
            new ShrineItemModifier()));

        // Chests
        IModifier chestModifier = new ChestModifier();
        provider.RegisterObjectCreator("chest-iron", new ObjectCreator(
            new SceneLoader("D01Z05S11_LOGIC", "LOGIC/INTERACTABLES/ACT_Iron Chest"), chestModifier));
        provider.RegisterObjectCreator("chest-gold", new ObjectCreator(
            new SceneLoader("D20Z02S02_LOGIC", "ACT_Wooden Chest"), chestModifier));
        provider.RegisterObjectCreator("chest-relic", new ObjectCreator(
            new SceneLoader("D17BZ01S01_LOGIC", "LOGIC/INTERACTABLES/ACT_Relicarium"), chestModifier));

        // Platforms
        provider.RegisterObjectCreator("platform-wood", new ObjectCreator(
            new SceneLoader("D05Z02S12_LOGIC", "LOGIC/INTERACTABLES/{0}"),
            new NoModifier("Platform")));
        provider.RegisterObjectCreator("platform-blood", new ObjectCreator(
            new SceneLoader("D17Z01S10_LOGIC", "LOGIC/INTERACTABLES/{0}"),
            new BloodModifier()));

        // Traps
        provider.RegisterObjectCreator("spikes", new ObjectCreator(
            new SceneLoader("D01Z03S01_DECO", "MIDDLEGROUND/AfterPlayer/Spikes/{0}"),
            new SpikeModifier()));

        // Misc.
        provider.RegisterObjectCreator("lantern", new ObjectCreator(
            new SceneLoader("D20Z01S02_LOGIC", "LOGIC/INTERACTABLES/Chain Hook"),
            new NoModifier("Lantern")));
    }
}
