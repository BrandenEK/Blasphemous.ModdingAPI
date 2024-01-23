using Framework.Inventory;
using Framework.Managers;
using Gameplay.UI;
using System.Collections.Generic;
using System.Linq;

namespace Blasphemous.ModdingAPI.Items;

/// <summary>
/// Allows registering custom items and acquiring them
/// </summary>
public static class ItemModder
{
    private static readonly List<ModItem> _items = new();
    internal static IEnumerable<ModItem> AllItems => _items;

    /// <summary>
    /// Registers a new item
    /// </summary>
    public static void RegisterItem(ModItem item)
    {
        if (_items.Any(i => i.Id == item.Id))
            return;

        _items.Add(item);
        //itemLoader.AddItem(item);
        Main.ModdingAPI.Log($"Registering custom item: {item.Name} ({item.Id})");
    }

    /// <summary>
    /// Gets the item type based off of the item's prefix
    /// </summary>
    public static InventoryManager.ItemType GetItemTypeFromId(string id)
    {
        if (id != null && id.Length >= 2)
        {
            switch (id.Substring(0, 2))
            {
                case "RB": return InventoryManager.ItemType.Bead;
                case "PR": return InventoryManager.ItemType.Prayer;
                case "RE": return InventoryManager.ItemType.Relic;
                case "HE": return InventoryManager.ItemType.Sword;
                case "QI": return InventoryManager.ItemType.Quest;
                case "CO": return InventoryManager.ItemType.Collectible;
            }
        }
        Main.ModdingAPI.LogError("Could not determine item type for " + id);
        return InventoryManager.ItemType.Bead;
    }

    /// <summary>
    /// Adds an item to the inventory, or tears if it is already owned, and displays the item
    /// </summary>
    public static void AddAndDisplayItem(string itemId)
    {
        InventoryManager.ItemType itemType = GetItemTypeFromId(itemId);
        BaseInventoryObject obj = Core.InventoryManager.GetBaseObject(itemId, itemType);
        if (obj == null) return;

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
        if (obj == null) return;

        bool removed = Core.InventoryManager.RemoveBaseObject(obj);
        if (removed)
            UIController.instance.ShowObjectPopUp(UIController.PopupItemAction.GiveObject, obj.caption, obj.picture, itemType, 3f, true);
    }

    /// <summary>
    /// Give any custom items marked as 'CarryOnStart'
    /// </summary>
    internal static void GrantStartingItems()
    {
        //foreach (var item in _items.Where(i => i.CarryOnStart))
        //    item.GiveItem();

        Core.Persistence.SaveGame();
    }
}
