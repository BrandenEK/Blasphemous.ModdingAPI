using Blasphemous.ModdingAPI.Levels.Loaders;
using Blasphemous.ModdingAPI.Levels.Modifiers;
using System.Collections.Generic;

namespace Blasphemous.ModdingAPI.Levels;

/// <summary> Registers a new level modification </summary>
public static class LevelRegister
{
    private static readonly Dictionary<string, ObjectCreator> _creators = new();
    internal static IEnumerable<KeyValuePair<string, ObjectCreator>> Creators => _creators;
    
    internal static bool TryGetLoader(string type, out ILoader loader)
    {
        bool success = _creators.TryGetValue(type, out var creator);
        loader = creator?.Loader;
        return success;
    }

    internal static bool TryGetModifier(string type, out IModifier modifier)
    {
        bool success = _creators.TryGetValue(type, out var creator);
        modifier = creator?.Modifier;
        return success;
    }

    /// <summary> Registers a new level modification </summary>
    public static void RegisterObjectCreator(this ModServiceProvider provider, string type, ObjectCreator creator)
    {
        if (provider == null)
            return;

        if (_creators.ContainsKey(type))
            return;

        _creators.Add(type, creator);
        Main.ModdingAPI.Log($"Registered custom object creator: {type}");
    }
}
