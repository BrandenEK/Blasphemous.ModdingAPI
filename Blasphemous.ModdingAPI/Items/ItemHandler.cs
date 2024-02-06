using Framework.Managers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Blasphemous.ModdingAPI.Items;

internal class ItemHandler
{
    private readonly Dictionary<InventoryManager.ItemType, Vector2> itemCountsByType = new()
        {
            { InventoryManager.ItemType.Bead, new Vector2(44, 44) },
            { InventoryManager.ItemType.Prayer, new Vector2(17, 17) },
            { InventoryManager.ItemType.Relic, new Vector2(7, 7) },
            { InventoryManager.ItemType.Sword, new Vector2(11, 11) },
            { InventoryManager.ItemType.Collectible, new Vector2(44, 44) },
        };

    public void NewGame()
    {
        foreach (var item in ItemRegister.Items.Where(i => i.CarryOnStart))
            item.GiveItem();
    }

    public void Initialize()
    {
        foreach (var item in ItemRegister.Items.Where(i => i.AddInventorySlot))
            AddItemCount(item);
    }

    private void AddItemCount(ModItem item)
    {
        switch (item.ItemType)
        {
            case ModItem.ModItemType.RosaryBead:
                itemCountsByType[InventoryManager.ItemType.Bead] = new Vector2(44, itemCountsByType[InventoryManager.ItemType.Bead].y + 1);
                return;
            case ModItem.ModItemType.Prayer:
                itemCountsByType[InventoryManager.ItemType.Prayer] = new Vector2(17, itemCountsByType[InventoryManager.ItemType.Prayer].y + 1);
                return;
            case ModItem.ModItemType.Relic:
                itemCountsByType[InventoryManager.ItemType.Relic] = new Vector2(7, itemCountsByType[InventoryManager.ItemType.Relic].y + 1);
                return;
            case ModItem.ModItemType.SwordHeart:
                itemCountsByType[InventoryManager.ItemType.Sword] = new Vector2(11, itemCountsByType[InventoryManager.ItemType.Sword].y + 1);
                return;
            case ModItem.ModItemType.QuestItem:
                Main.ModdingAPI.LogWarning("Can not add an inventory slot for quest items!");
                return;
            case ModItem.ModItemType.Collectible:
                itemCountsByType[InventoryManager.ItemType.Collectible] = new Vector2(44, itemCountsByType[InventoryManager.ItemType.Collectible].y + 1);
                return;
        }
    }

    public Vector2 GetItemCountOfType(InventoryManager.ItemType itemType)
    {
        return itemCountsByType.TryGetValue(itemType, out Vector2 amount) ? amount : new Vector2();
    }
}
