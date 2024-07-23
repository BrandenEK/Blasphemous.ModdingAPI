using BepInEx.Logging;
using Blasphemous.ModdingAPI.Persistence;
using Framework.FrameworkCore;
using Framework.Managers;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blasphemous.ModdingAPI;

internal class ModLoader
{
    private readonly List<BlasMod> _mods;
    private readonly ManualLogSource _logger;

    private bool _initialized = false;
    private bool _loadedMenu = false;
    private string _currentScene = string.Empty;

    public IEnumerable<BlasMod> AllMods => _mods;
    public string CurrentScene => _currentScene;

    public ModLoader()
    {
        _mods = [];
        _logger = BepInEx.Logging.Logger.CreateLogSource("Mod Loader");
    }

    /// <summary>
    /// Loops over the list of registered mods and performs an action on each one
    /// </summary>
    public void ProcessModFunction(System.Action<BlasMod> action)
    {
        foreach (var mod in _mods)
        {
            try
            {
                action(mod);
            }
            catch (System.Exception e)
            {
                mod.LogError($"Encountered error: {e.Message}\n{e.CleanStackTrace()}");
            }
        }
    }

    /// <summary>
    /// Initializes all mods
    /// </summary>
    public void Initialize()
    {
        if (_initialized) return;

        LogSpecial("Initialization");
        LevelManager.OnLevelPreLoaded += LevelPreLoaded;
        LevelManager.OnLevelLoaded += LevelLoaded;
        LevelManager.OnBeforeLevelLoad += LevelUnloaded;

        Main.ModdingAPI.Log("Initializing mods...");
        ProcessModFunction(mod => mod.OnInitialize());

        ProcessModFunction(mod => mod.OnRegisterServices(new ModServiceProvider(mod)));

        Main.ModdingAPI.Log("All mods initialized!");
        ProcessModFunction(mod => mod.OnAllInitialized());

        ProcessModFunction(mod =>
        {
            if (mod is IPersistentMod pmod)
                Core.Persistence.AddPersistentManager(new ModPersistentSystem(pmod));
        });
        _initialized = true;
    }

    /// <summary>
    /// Disposes all mods
    /// </summary>
    public void Dispose()
    {
        ProcessModFunction(mod => mod.OnDispose());

        LevelManager.OnLevelPreLoaded -= LevelPreLoaded;
        LevelManager.OnLevelLoaded -= LevelLoaded;
        LevelManager.OnBeforeLevelLoad -= LevelUnloaded;
        Main.ModdingAPI.Log("All mods disposed!");
    }

    /// <summary>
    /// Updates all mods
    /// </summary>
    public void Update()
    {
        if (!_initialized) return;

        ProcessModFunction(mod => mod.OnUpdate());
    }

    /// <summary>
    /// Late updates all mods
    /// </summary>
    public void LateUpdate()
    {
        if (!_initialized) return;

        ProcessModFunction(mod => mod.OnLateUpdate());
    }

    /// <summary>
    /// Processes a PreloadScene event for all mods
    /// </summary>
    public void LevelPreLoaded(Level oldLevel, Level newLevel)
    {
        string oLevel = oldLevel?.LevelName ?? string.Empty;
        string nLevel = newLevel?.LevelName ?? string.Empty;

        ProcessModFunction(mod => mod.OnLevelPreloaded(oLevel, nLevel));
    }

    /// <summary>
    /// Processes a LoadScene event for all mods
    /// </summary>
    public void LevelLoaded(Level oldLevel, Level newLevel)
    {
        string oLevel = oldLevel?.LevelName ?? string.Empty;
        string nLevel = newLevel?.LevelName ?? string.Empty;

        if (nLevel == "MainMenu")
        {
            if (_loadedMenu)
                ProcessModFunction(mod => mod.OnExitGame());
            _loadedMenu = true;
        }

        LogSpecial("Loaded level " + nLevel);

        _currentScene = nLevel;
        ProcessModFunction(mod => mod.OnLevelLoaded(oLevel, nLevel));
    }

    /// <summary>
    /// Processes an UnloadScene event for all mods
    /// </summary>
    public void LevelUnloaded(Level oldLevel, Level newLevel)
    {
        string oLevel = oldLevel?.LevelName ?? string.Empty;
        string nLevel = newLevel?.LevelName ?? string.Empty;

        _currentScene = string.Empty;
        ProcessModFunction(mod => mod.OnLevelUnloaded(oLevel, nLevel));
    }

    /// <summary>
    /// Registers a new mod whenever it is first created
    /// </summary>
    public bool RegisterMod(BlasMod mod)
    {
        if (_mods.Any(m => m.Id == mod.Id))
        {
            _logger.LogError($"Mod with id '{mod.Id}' already exists!");
            return false;
        }

        _logger.LogMessage($"Registering mod: {mod.Id} ({mod.Version})");
        _mods.Add(mod);
        return true;
    }

    /// <summary>
    /// Checks whether a mod is already loaded
    /// </summary>
    public bool IsModLoadedId(string Id, out BlasMod mod)
    {
        return (mod = _mods.FirstOrDefault(m => m.Id == Id)) != null;
    }

    /// <summary>
    /// Checks whether a mod is already loaded
    /// </summary>
    public bool IsModLoadedName(string name, out BlasMod mod)
    {
        return (mod = _mods.FirstOrDefault(m => m.Name == name)) != null;
    }

    /// <summary>
    /// Formats the message for scene loading
    /// </summary>
    internal void LogSpecial(string message)
    {
        StringBuilder sb = new();
        int length = message.Length;
        while (length > 0)
        {
            sb.Append('=');
            length--;
        }
        string line = sb.ToString();

        Logger.Info("");
        Logger.Info(line);
        Logger.Info(message);
        Logger.Info(line);
        Logger.Info("");
    }
}
