using System.Collections.Generic;
using Framework.Managers;
using UnityEngine;

namespace ModdingAPI.Items
{
    internal class ItemLoader
    {
        private Dictionary<InventoryManager.ItemType, Vector2> itemCountsByType;

        public ItemLoader()
        {
            itemCountsByType = new Dictionary<InventoryManager.ItemType, Vector2>()
            {
                { InventoryManager.ItemType.Bead, new Vector2(44, 44) },
                { InventoryManager.ItemType.Prayer, new Vector2(17, 17) },
                { InventoryManager.ItemType.Relic, new Vector2(7, 7) },
                { InventoryManager.ItemType.Sword, new Vector2(11, 11) },
                { InventoryManager.ItemType.Collectible, new Vector2(44, 44) },
            };
        }

        public void AddItem(ModItem item)
        {
            if (!item.AddInventorySlot) return;

            if (item is ModRosaryBead)
            {
                itemCountsByType[InventoryManager.ItemType.Bead] = new Vector2(44, itemCountsByType[InventoryManager.ItemType.Bead].y + 1);
            }
            else if (item is ModPrayer)
            {
                itemCountsByType[InventoryManager.ItemType.Prayer] = new Vector2(17, itemCountsByType[InventoryManager.ItemType.Prayer].y + 1);
            }
            else if (item is ModRelic)
            {
                itemCountsByType[InventoryManager.ItemType.Relic] = new Vector2(7, itemCountsByType[InventoryManager.ItemType.Relic].y + 1);
            }
            else if (item is ModSwordHeart)
            {
                itemCountsByType[InventoryManager.ItemType.Sword] = new Vector2(11, itemCountsByType[InventoryManager.ItemType.Sword].y + 1);
            }
            else if (item is ModCollectible)
            {
                itemCountsByType[InventoryManager.ItemType.Collectible] = new Vector2(44, itemCountsByType[InventoryManager.ItemType.Collectible].y + 1);
            }
        }

        public Vector2 GetItemCountOfType(InventoryManager.ItemType itemType)
        {
            if (itemCountsByType.ContainsKey(itemType))
                return itemCountsByType[itemType];
            return new Vector2();
        }
    }
}
