using Blasphemous.ModdingAPI.Helpers;
using Framework.Managers;

namespace Blasphemous.ModdingAPI;

/// <summary>
/// A collection of methods used to work with items
/// </summary>
public static class ItemModder
{
    /// <summary>
    /// Gets the item type based off of the item's prefix
    /// </summary>
    [System.Obsolete("Use new ItemHelper instead")]
    public static InventoryManager.ItemType GetItemTypeFromId(string id)
    {
        return ItemHelper.GetItemTypeFromId(id);
    }

    /// <summary>
    /// Adds an item to the inventory, or tears if it is already owned, and displays the item
    /// </summary>
    [System.Obsolete("Use new ItemHelper instead")]
    public static void AddAndDisplayItem(string itemId)
    {
        ItemHelper.AddAndDisplayItem(itemId);
    }

    /// <summary>
    /// Removes an item from the inventory, and displays the item if it was previously owned
    /// </summary>
    [System.Obsolete("Use new ItemHelper instead")]
    public static void RemoveAndDisplayItem(string itemId)
    {
        ItemHelper.RemoveAndDisplayItem(itemId);
    }
}
