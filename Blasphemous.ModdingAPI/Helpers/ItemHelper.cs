using Framework.Inventory;
using Framework.Managers;
using Gameplay.UI;

namespace Blasphemous.ModdingAPI.Helpers;

/// <summary>
/// Provides utility methods for dealing with items
/// </summary>
public static class ItemHelper
{
    /// <summary>
    /// Gets the item type based off of the item's prefix
    /// </summary>
    public static InventoryManager.ItemType GetItemTypeFromId(string id)
    {
        if (id == null || id.Length < 2)
            throw new System.ArgumentException("Invalid item id");

        return id.Substring(0, 2) switch
        {
            "RB" => InventoryManager.ItemType.Bead,
            "PR" => InventoryManager.ItemType.Prayer,
            "RE" => InventoryManager.ItemType.Relic,
            "HE" => InventoryManager.ItemType.Sword,
            "QI" => InventoryManager.ItemType.Quest,
            "CO" => InventoryManager.ItemType.Collectible,
            _ => throw new System.ArgumentException("Invalid item id"),
        };
    }

    /// <summary>
    /// Adds an item to the inventory, or tears if it is already owned, and displays the item
    /// </summary>
    public static void AddAndDisplayItem(string itemId)
    {
        InventoryManager.ItemType itemType = GetItemTypeFromId(itemId);
        BaseInventoryObject obj = Core.InventoryManager.GetBaseObject(itemId, itemType);
        if (obj == null)
            return;

        obj = Core.InventoryManager.AddBaseObjectOrTears(obj);
        UIController.instance.ShowObjectPopUp(UIController.PopupItemAction.GetObejct, obj.caption, obj.picture, itemType, 3f, true);
    }

    /// <summary>
    /// Removes an item from the inventory, and displays the item if it was previously owned
    /// </summary>
    public static void RemoveAndDisplayItem(string itemId)
    {
        InventoryManager.ItemType itemType = GetItemTypeFromId(itemId);
        BaseInventoryObject obj = Core.InventoryManager.GetBaseObject(itemId, itemType);
        if (obj == null)
            return;

        bool removed = Core.InventoryManager.RemoveBaseObject(obj);
        if (removed)
            UIController.instance.ShowObjectPopUp(UIController.PopupItemAction.GiveObject, obj.caption, obj.picture, itemType, 3f, true);
    }
}
