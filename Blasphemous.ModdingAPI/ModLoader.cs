using BepInEx.Logging;
using Blasphemous.ModdingAPI.Extensions;
using Blasphemous.ModdingAPI.Helpers;
using Framework.FrameworkCore;
using Framework.Managers;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blasphemous.ModdingAPI;

internal class ModLoader
{
    private readonly List<BlasMod> _mods = new();
    private readonly ManualLogSource _logger = Logger.CreateLogSource("Mod Loader");

    private bool _initialized = false;
    private bool _loadedMenu = false;

    public ModLoader()
    {
        ModHelper.LoadedMods = _mods;
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
                ModLog.Error($"Encountered error: {e.Message}\n{e.CleanStackTrace()}", mod);
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

        ModLog.Info("Initializing mods...");
        ProcessModFunction(mod => mod.OnInitialize());

        ModLog.Info("All mods initialized!");
        ProcessModFunction(mod => mod.OnAllInitialized());

        _initialized = true;
    }

    /// <summary>
    /// Disposes all mods
    /// </summary>
    public void Dispose()
    {
        ProcessModFunction(mod => mod.OnDispose());

        ModLog.Info("All mods disposed!");
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

        ModLog.Info("");
        ModLog.Info(line);
        ModLog.Info(message);
        ModLog.Info(line);
        ModLog.Info("");
    }
}
