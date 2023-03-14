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

            //if (bead.CarryOnStart && PersistentManager.GetAutomaticSlot() >= 0)
            //{
            //    __instance.AddRosaryBead(Bead);
            //}

            //displayItemComponents(___mainObject.transform.Find("Prayer"));
            //displayItemComponents(___mainObject.transform.Find("Relic"));
            //displayItemComponents(___mainObject.transform.Find("Sword"));
            //displayItemComponents(___mainObject.transform.Find("QuestItem"));
            //displayItemComponents(___mainObject.transform.Find("CollectibleItem"));

            //void displayItemComponents(Transform parent)
            //{
            //    foreach (Transform child in parent)
            //    {
            //        Main.LogError(Main.MOD_NAME, child.name);
            //        foreach (Component c in child.GetComponents<Component>())
            //            Main.LogWarning(Main.MOD_NAME, c.ToString());
            //    }
            //}
        }
    }

    // Add extra slots to inventory tabs based on how many custom items
    [HarmonyPatch(typeof(NewInventory_LayoutGrid), "Awake")]
    public class InventoryLayout_Patch
    {
        public static void Prefix(ref int ___numGridElements)
        {
            Main.LogWarning(Main.MOD_NAME, "Awake for adding slots");
            if (___numGridElements == 0) // ??
            {
                ___numGridElements += CountNumberOfType<ModRosaryBead>();
            }
            else if (___numGridElements == 11)
            {
                ___numGridElements += CountNumberOfType<ModPrayer>();
            }
            else if (___numGridElements == 7)
            {
                ___numGridElements += CountNumberOfType<ModRelic>();
            }
            else if (___numGridElements == 0) // ??
            {
                ___numGridElements += CountNumberOfType<ModSwordHeart>();
            }
            else if (___numGridElements == 44)
            {
                ___numGridElements += CountNumberOfType<ModCollectible>();
            }
            // Don't add any slots for quest items

            int CountNumberOfType<T>() where T : ModItem
            {
                int count = 0;
                foreach (ModItem item in Main.moddingAPI.GetModItems())
                {
                    if (item is T)
                        count++;
                }
                return count;
            }
        }
    }

    // Maybe methods for adding them ?
}
