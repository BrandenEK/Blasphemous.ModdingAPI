﻿using HarmonyLib;
using UnityEngine;
using Framework.Managers;
using Framework.Inventory;
using System.Collections.Generic;

namespace ModdingAPI.Items
{
    // Register items on init
    // Add slots to inventory
    // Maybe methods for adding them ?

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
}