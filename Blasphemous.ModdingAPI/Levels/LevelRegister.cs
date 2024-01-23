using System.Collections.Generic;

namespace Blasphemous.ModdingAPI.Levels;

/// <summary> Registers a new level modification </summary>
public static class LevelRegister
{
    private static readonly Dictionary<string, ObjectModifier> _modifiers = new();
    internal static IEnumerable<KeyValuePair<string, ObjectModifier>> Modifiers => _modifiers;
    
    internal static bool TryGetModifier(string type, out ObjectModifier modifier) =>
        _modifiers.TryGetValue(type, out modifier);

    /// <summary> Registers a new level modification </summary>
    public static void RegisterObjectModifier(this ModServiceProvider provider, string type, ObjectModifier modifier)
    {
        if (provider == null)
            return;

        if (_modifiers.ContainsKey(type))
            return;

        _modifiers.Add(type, modifier);
        Main.ModdingAPI.Log($"Registered custom object modifier: {type}");
    }
}
