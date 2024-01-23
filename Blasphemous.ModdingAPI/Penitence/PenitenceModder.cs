using System.Collections.Generic;
using System.Linq;

namespace Blasphemous.ModdingAPI.Penitence;

/// <summary>
/// Allows registering custom penitences
/// </summary>
public static class PenitenceModder
{
    private static readonly List<ModPenitence> _penitences = new();

    /// <summary>
    /// Registers a new penitence
    /// </summary>
    public static void RegisterPenitence(ModPenitence penitence)
    {
        if (_penitences.Any(p => p.Id == penitence.Id))
            return;

        _penitences.Add(penitence);
        Main.ModdingAPI.Log($"Registering custom penitence: {penitence.Name} ({penitence.Id})");
    }

    internal static IEnumerable<ModPenitence> All => _penitences;

    internal static ModPenitence AtIndex(int index) => _penitences[index];
}
