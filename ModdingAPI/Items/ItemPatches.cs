using HarmonyLib;
using UnityEngine;
using Framework.Managers;
using Framework.Inventory;
using Gameplay.UI.Others.MenuLogic;
using System.Collections.Generic;

namespace ModdingAPI.Items
{
    // Initialize any custom items
    [HarmonyPatch(typeof(InventoryManager), "InitializeObjects")]
    internal class InventoryInitialize_Patch
    {
        public static void Postfix(InventoryManager __instance, GameObject ___mainObject, Dictionary<string, RosaryBead> ___allBeads, Dictionary<string, Relic> ___allRellics,
            Dictionary<string, Prayer> ___allPrayers, Dictionary<string, Sword> ___allSwords, Dictionary<string, QuestItem> ___allQuestItems, Dictionary<string, Framework.Inventory.CollectibleItem> ___allCollectibleItems)
        {
            foreach (ModItem item in Main.moddingAPI.GetModItems())
            {
                if (item is ModRosaryBead bead)
                {
                    RosaryBead Bead = bead.CreateRosaryBead(___mainObject);
                    ___allBeads.Add(bead.Id, Bead);
                }
                else if (item is ModRelic relic)
                {
                    Relic Relic = relic.CreateRelic(___mainObject);
                    ___allRellics.Add(relic.Id, Relic);
                }
                else if (item is ModPrayer prayer)
                {
                    Prayer Prayer = prayer.CreatePrayer(___mainObject);
                    ___allPrayers.Add(prayer.Id, Prayer);
                }
                else if (item is ModSwordHeart swordHeart)
                {
                    Sword SwordHeart = swordHeart.CreateSwordHeart(___mainObject);
                    ___allSwords.Add(swordHeart.Id, SwordHeart);
                }
                else if (item is ModQuestItem questItem)
                {
                    QuestItem QuestItem = questItem.CreateQuestItem(___mainObject);
                    ___allQuestItems.Add(questItem.Id, QuestItem);
                }
                else if (item is ModCollectible collectible)
                {
                    Framework.Inventory.CollectibleItem Collectible = collectible.CreateCollectible(___mainObject);
                    ___allCollectibleItems.Add(collectible.Id, Collectible);
                }
            }
        }
    }

    // Add extra slots to inventory tabs based on how many custom items
    [HarmonyPatch(typeof(NewInventory_LayoutGrid), "ShowMaxSlotsForCurrentTabType")]
    internal class InventoryLayout_Patch
    {
        public static void Postfix(List<NewInventory_GridItem> ___cachedGridElements, InventoryManager.ItemType ___currentItemType)
        {
            Vector2 itemCounts = Main.moddingAPI.itemLoader.GetItemCountOfType(___currentItemType);
            if (itemCounts.x == 0) return;

            for (int i = (int)itemCounts.x; i < (int)itemCounts.y && i < 72; i++)
            {
                ___cachedGridElements[i].gameObject.SetActive(true);
            }
        }
    }

    // Maybe methods for adding them ?
}
