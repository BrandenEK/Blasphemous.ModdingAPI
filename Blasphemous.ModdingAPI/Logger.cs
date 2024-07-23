using BepInEx.Logging;
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
    /// Logs a message using the caller's assembly to determine the mod name
    /// </summary>
    private static void Log(object message, LogLevel level)
    {
        ManualLogSource source = _loggers.TryGetValue(Assembly.GetCallingAssembly(), out var logger) ? logger : _unknownLogger;
        source.Log(level, message);
    }

    /// <summary>
    /// Logs an information message
    /// </summary>
    public static void Info(object message)
    {
        Log(message, LogLevel.Message);
    }

    /// <summary>
    /// Logs a warning message
    /// </summary>
    public static void Warn(object message)
    {
        Log(message, LogLevel.Warning);
    }

    /// <summary>
    /// Logs an error message
    /// </summary>
    public static void Error(object message)
    {
        Log(message, LogLevel.Error);
    }

    /// <summary>
    /// Logs a fatal error message
    /// </summary>
    public static void Fatal(object message)
    {
        Log(message, LogLevel.Fatal);
    }

    /// <summary>
    /// Logs a debug message
    /// </summary>
    public static void Debug(object message)
    {
        Log(message, LogLevel.Debug);
    }

    /// <summary>
    /// Logs a message when given an assembly.  This is only used for backwards compatability
    /// </summary>
    internal static void LogArchive(object message, LogLevel level, Assembly assembly)
    {
        ManualLogSource source = _loggers.TryGetValue(assembly, out var logger) ? logger : _unknownLogger;
        source.Log(level, message);
    }
}
