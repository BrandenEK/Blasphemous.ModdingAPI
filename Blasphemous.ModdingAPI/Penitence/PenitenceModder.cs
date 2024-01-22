using Framework.Managers;
using System.Collections.Generic;
using System.Linq;

namespace Blasphemous.ModdingAPI.Penitence;

/// <summary>
/// Allows registering custom penitences
/// </summary>
public static class PenitenceModder
{
    private static readonly List<ModPenitence> _penitences = new();
    internal static IEnumerable<ModPenitence> AllPenitences => _penitences;

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

    /// <summary>
    /// When game starts, if there are any custom penitences, reset data
    /// </summary>
    internal static void ResetCustomPenitences()
    {
        if (_penitences.Count > 0)
            Core.PenitenceManager.ResetPersistence();
    }

    internal static void UpdateSelection()
    {

    }
}
