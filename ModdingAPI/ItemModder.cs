using Framework.Managers;
using Framework.Inventory;
using Gameplay.UI;

namespace ModdingAPI
{
    /// <summary>
    /// A collection of methods used to work with items
    /// </summary>
    public static class ItemModder
    {
        /// <summary>
        /// Gets the item type based off of the item's prefix
        /// </summary>
        /// <param name="id">The id of the item</param>
        /// <returns>The type of the item</returns>
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
            Main.LogError(Main.MOD_NAME, "Could not determine item type for " + id);
            return InventoryManager.ItemType.Bead;
        }

        /// <summary>
        /// Adds an item to the inventory, or tears if it is already owned, and displays the item
        /// </summary>
        /// <param name="itemId">The id of the item to add</param>
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
        /// <param name="itemId">The id of the item to remove</param>
        public static void RemoveAndDisplayItem(string itemId)
        {
            InventoryManager.ItemType itemType = GetItemTypeFromId(itemId);
            BaseInventoryObject obj = Core.InventoryManager.GetBaseObject(itemId, itemType);
            if (obj == null) return;

            bool removed = Core.InventoryManager.RemoveBaseObject(obj);
            if (removed)
                UIController.instance.ShowObjectPopUp(UIController.PopupItemAction.GiveObject, obj.caption, obj.picture, itemType, 3f, true);
        }
    }
}
