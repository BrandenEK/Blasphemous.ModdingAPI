using BepInEx.Logging;
using Gameplay.UI;
using System.Collections.Generic;
using System.Reflection;

namespace Blasphemous.ModdingAPI;

/// <summary>
/// Useful methods for logging data
/// </summary>
public static class ModLog
{
    private static readonly Dictionary<Assembly, ManualLogSource> _loggers = new();
    private static readonly ManualLogSource _unknownLogger = Logger.CreateLogSource("Unknown mod");

    /// <summary>
    /// Registers a new mod to be able to log, based on its assembly
    /// </summary>
    internal static void Register(BlasMod mod)
    {
        if (mod == null)
            return;

        _loggers.Add(mod.GetType().Assembly, Logger.CreateLogSource(mod.Name));
    }

    private static void LogInternal(object message, LogLevel level, Assembly assembly)
    {
        ManualLogSource source = _loggers.TryGetValue(assembly, out var logger) ? logger : _unknownLogger;
        source.Log(level, message);
    }

    private static void DisplayInternal(object message, Assembly assembly)
    {
        try
        {
            LogInternal(message, LogLevel.Message, assembly);
            UIController.instance.ShowPopUp(message?.ToString(), "", 0, false);
        }
        catch
        {
            LogInternal("Tried to call 'LogDisplay' before the UIController was initialized", LogLevel.Error, assembly);
        }
    }

    /// <summary>
    /// Logs an information message
    /// </summary>
    public static void Info(object message) => LogInternal(message, LogLevel.Message, Assembly.GetCallingAssembly());
    /// <summary>
    /// Logs an information message through the specified mod
    /// </summary>
    public static void Info(object message, BlasMod mod) => LogInternal(message, LogLevel.Message, mod.GetType().Assembly);

    /// <summary>
    /// Logs a warning message
    /// </summary>
    public static void Warn(object message) => LogInternal(message, LogLevel.Warning, Assembly.GetCallingAssembly());
    /// <summary>
    /// Logs a warning message through the specified mod
    /// </summary>
    public static void Warn(object message, BlasMod mod) => LogInternal(message, LogLevel.Warning, mod.GetType().Assembly);

    /// <summary>
    /// Logs an error message
    /// </summary>
    public static void Error(object message) => LogInternal(message, LogLevel.Error, Assembly.GetCallingAssembly());
    /// <summary>
    /// Logs an error message through the specified mod
    /// </summary>
    public static void Error(object message, BlasMod mod) => LogInternal(message, LogLevel.Error, mod.GetType().Assembly);

    /// <summary>
    /// Logs a fatal error message
    /// </summary>
    public static void Fatal(object message) => LogInternal(message, LogLevel.Fatal, Assembly.GetCallingAssembly());
    /// <summary>
    /// Logs a fatal error message through the specified mod
    /// </summary>
    public static void Fatal(object message, BlasMod mod) => LogInternal(message, LogLevel.Fatal, mod.GetType().Assembly);

    /// <summary>
    /// Logs a debug message
    /// </summary>
    public static void Debug(object message) => LogInternal(message, LogLevel.Info, Assembly.GetCallingAssembly());
    /// <summary>
    /// Logs a debug message through the specified mod
    /// </summary>
    public static void Debug(object message, BlasMod mod) => LogInternal(message, LogLevel.Info, mod.GetType().Assembly);

    /// <summary>
    /// Logs a message to the in-game UI
    /// </summary>
    public static void Display(object message) => DisplayInternal(message, Assembly.GetCallingAssembly());
    /// <summary>
    /// Logs a message to the in-game UI through the specified mod
    /// </summary>
    public static void Display(object message, BlasMod mod) => DisplayInternal(message, mod.GetType().Assembly);
}
