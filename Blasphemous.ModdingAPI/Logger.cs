using BepInEx.Logging;
using Gameplay.UI;
using System.Collections.Generic;
using System.Reflection;

namespace Blasphemous.ModdingAPI;

/// <summary>
/// Useful methods for logging data
/// </summary>
public static class Logger
{
    private static readonly Dictionary<Assembly, ManualLogSource> _loggers = new();
    private static readonly ManualLogSource _unknownLogger = BepInEx.Logging.Logger.CreateLogSource("Unknown mod");

    /// <summary>
    /// Registers a new mod to be able to log, based on its assembly
    /// </summary>
    internal static void Register(BlasMod mod)
    {
        if (mod == null)
            return;

        _loggers.Add(mod.GetType().Assembly, BepInEx.Logging.Logger.CreateLogSource(mod.Name));
    }

    /// <summary>
    /// Logs a message using an assembly to determine the mod name
    /// </summary>
    internal static void LogInternal(object message, LogLevel level, Assembly assembly)
    {
        ManualLogSource source = _loggers.TryGetValue(assembly, out var logger) ? logger : _unknownLogger;
        source.Log(level, message);
    }

    /// <summary>
    /// Logs an information message
    /// </summary>
    public static void Info(object message)
    {
        LogInternal(message, LogLevel.Message, Assembly.GetCallingAssembly());
    }

    /// <summary>
    /// Logs a warning message
    /// </summary>
    public static void Warn(object message)
    {
        LogInternal(message, LogLevel.Warning, Assembly.GetCallingAssembly());
    }

    /// <summary>
    /// Logs an error message
    /// </summary>
    public static void Error(object message)
    {
        LogInternal(message, LogLevel.Error, Assembly.GetCallingAssembly());
    }

    /// <summary>
    /// Logs a fatal error message
    /// </summary>
    public static void Fatal(object message)
    {
        LogInternal(message, LogLevel.Fatal, Assembly.GetCallingAssembly());
    }

    /// <summary>
    /// Logs a debug message
    /// </summary>
    public static void Debug(object message)
    {
        LogInternal(message, LogLevel.Debug, Assembly.GetCallingAssembly());
    }

    /// <summary>
    /// Logs a message to the in-game UI
    /// </summary>
    public static void Display(object message)
    {
        try
        {
            LogInternal(message, LogLevel.Message, Assembly.GetCallingAssembly());
            UIController.instance.ShowPopUp(message?.ToString(), "", 0, false);
        }
        catch
        {
            LogInternal("Tried to call 'LogDisplay' before the UIController was initialized", LogLevel.Error, Assembly.GetCallingAssembly());
        }
    }
}
