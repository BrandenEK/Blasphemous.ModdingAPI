using BepInEx.Logging;
using System.Collections.Generic;
using System.Reflection;

namespace Blasphemous.ModdingAPI;

public static class Logger
{
    private static readonly Dictionary<Assembly, ManualLogSource> _loggers = new();
    private static readonly ManualLogSource _unknownLogger = BepInEx.Logging.Logger.CreateLogSource("Unknown mod");

    private static void Log(object message, LogLevel level)
    {
        ManualLogSource source = _loggers.TryGetValue(Assembly.GetCallingAssembly(), out var logger) ? logger : _unknownLogger;
        source.Log(level, message);
    }

    public static void Register(BlasMod mod)
    {
        if (mod == null)
            return;

        _loggers.Add(mod.GetType().Assembly, BepInEx.Logging.Logger.CreateLogSource(mod.Name));
    }

    public static void Info(object message)
    {
        Log(message, LogLevel.Message);
    }

    public static void Warn(object message)
    {
        Log(message, LogLevel.Warning);
    }
}
