using System.Collections.Generic;
using System.Linq;

namespace Blasphemous.ModdingAPI.Items;

/// <summary> Registers a new item </summary>
public static class ItemRegister
{
    private static readonly List<ModItem> _items = new List<ModItem>();
    internal static IEnumerable<ModItem> Items => _items;

    /// <summary> Registers a new console command </summary>
    public static void RegisterItem(this ModServiceProvider provider, ModItem item)
    {
        if (provider == null)
            return;

        if (_items.Any(i => i.Id == item.Id))
            return;

        _items.Add(item);
        Main.ModdingAPI.Log($"Registered custom item: {item.Id}");
    }
}
