using System.Collections.Generic;
using System.Linq;

namespace Blasphemous.ModdingAPI.Penitence;

/// <summary> Registers a new penitence </summary>
public static class PenitenceRegister
{
    private static readonly List<ModPenitence> _penitences = new();
    internal static IEnumerable<ModPenitence> Penitences => _penitences;
    internal static ModPenitence AtIndex(int index) => _penitences[index];
    internal static int Total => _penitences.Count;

    /// <summary> Registers a new penitence </summary>
    public static void RegisterPenitence(this ModServiceProvider provider, ModPenitence penitence)
    {
        if (provider == null)
            return;

        if (_penitences.Any(pen => pen.Id == penitence.Id))
            return;

        _penitences.Add(penitence);
        Main.ModdingAPI.Log($"Registered custom penitence: {penitence.Id}");
    }
}
