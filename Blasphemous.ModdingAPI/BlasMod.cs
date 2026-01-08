using Blasphemous.ModdingAPI.Config;
using Blasphemous.ModdingAPI.Files;
using Blasphemous.ModdingAPI.Input;
using Blasphemous.ModdingAPI.Localization;
using HarmonyLib;
using System.Diagnostics;
using System.Linq;

namespace Blasphemous.ModdingAPI;

/// <summary>
/// The main class for the mod, handles access to the API
/// </summary>
public abstract class BlasMod
{
    // Mod info

    /// <summary>
    /// The unique id of the mod
    /// </summary>
    public string Id { get; }

    /// <summary>
    /// The display name of the mod
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// The developer of the mod
    /// </summary>
    public string Author { get; }

    /// <summary>
    /// The file version of the mod
    /// </summary>
    public string Version { get; }

    /// <summary>
    /// Whether the mod is a developer build
    /// </summary>
    public bool IsDebug { get; }

    // Handlers

    /// <summary>
    /// Handles storing and retrieving config properties
    /// </summary>
    public ConfigHandler ConfigHandler => _configHandler;
    private readonly ConfigHandler _configHandler;

    /// <summary>
    /// Handles file IO, such as such loading data or writing to a file
    /// </summary>
    public FileHandler FileHandler => _fileHandler;
    private readonly FileHandler _fileHandler;

    /// <summary>
    /// Handles player input, such as custom keybindings
    /// </summary>
    public InputHandler InputHandler => _inputHandler;
    private readonly InputHandler _inputHandler;

    /// <summary>
    /// Handles translations, such as automatic localization on language change
    /// </summary>
    public LocalizationHandler LocalizationHandler => _localizationHandler;
    private readonly LocalizationHandler _localizationHandler;

    // Events

    /// <summary>
    /// Called when starting the game, before managers have initialized
    /// </summary>
    protected internal virtual void OnPreInitialize() { }

    /// <summary>
    /// Called when starting the game, after managers have initialized
    /// </summary>
    protected internal virtual void OnInitialize() { }

    /// <summary>
    /// Called when starting the game, after all other mods have been initialized
    /// </summary>
    protected internal virtual void OnAllInitialized() { }

    /// <summary>
    /// Called when exiting the game, before managers have disposed
    /// </summary>
    protected internal virtual void OnDispose() { }

    /// <summary>
    /// Called every frame after initialization
    /// </summary>
    protected internal virtual void OnUpdate() { }

    /// <summary>
    /// Called at the end of every frame after initialization
    /// </summary>
    protected internal virtual void OnLateUpdate() { }

    /// <summary>
    /// Called when a new level is about to be loaded, including the main menu
    /// </summary>
    protected internal virtual void OnLevelPreloaded(string oldLevel, string newLevel) { }

    /// <summary>
    /// Called when a new level is loaded, including the main menu
    /// </summary>
    protected internal virtual void OnLevelLoaded(string oldLevel, string newLevel) { }

    /// <summary>
    /// Called when an old level is unloaded, including the main menu
    /// </summary>
    protected internal virtual void OnLevelUnloaded(string oldLevel, string newLevel) { }

    /// <summary>
    /// Called when starting a new game on the main menu, after data is reset
    /// </summary>
    protected internal virtual void OnNewGame() { }

    /// <summary>
    /// Called when loading an existing game on the main menu, after data is reset
    /// </summary>
    protected internal virtual void OnLoadGame() { }

    /// <summary>
    /// Called when quiting a game, after returning to the main menu
    /// </summary>
    protected internal virtual void OnExitGame() { }

    /// <summary>
    /// Called when mods are able to register services
    /// </summary>
    protected internal virtual void OnRegisterServices(ModServiceProvider provider) { }

    // Constructor

    /// <summary>
    /// Initializes and registers a new Blasphemous mod
    /// </summary>
    public BlasMod(string id, string name, string author, string version)
    {
        // Set data
        Id = id;
        Name = name;
        Author = author;
        Version = version;
        IsDebug = GetType().Assembly.GetCustomAttributes(false).OfType<DebuggableAttribute>().Any(x => x.IsJITTrackingEnabled);

        // Set handlers
        _configHandler = new ConfigHandler(this);
        _fileHandler = new FileHandler(this);
        _inputHandler = new InputHandler(this);
        _localizationHandler = new LocalizationHandler(this);

        // Register and patch mod
        if (Main.ModLoader.RegisterMod(this))
        {
            new Harmony(id).PatchAll(GetType().Assembly);
            ModLog.Register(this);
        }
    }
}
