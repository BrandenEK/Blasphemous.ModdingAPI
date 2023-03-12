using HarmonyLib;
using UnityEngine;
using Framework.Managers;
using Framework.Inventory;
using System.Collections.Generic;

namespace ModdingAPI.Patches
{
    // Register items on init
    // Add slots to inventory
    // Maybe methods for adding them ?

    [HarmonyPatch(typeof(InventoryManager), "InitializeObjects")]
    internal class InventoryInitialize_Patch
    {
        public static void Postfix(InventoryManager __instance, GameObject ___mainObject, Dictionary<string, RosaryBead> ___allBeads, Dictionary<string, Relic> ___allRellics)
        {
            foreach (ModItem item in Main.moddingAPI.GetModItems())
            {
                if (item is ModRosaryBead bead)
                {
                    RosaryBead Bead = bead.CreateRosaryBead(___mainObject);
                    ___allBeads.Add(bead.Id, Bead);

                    //if (bead.CarryOnStart && PersistentManager.GetAutomaticSlot() >= 0)
                    //{
                    //    __instance.AddRosaryBead(Bead);
                    //}
                }
                else if (item is ModRelic relic)
                {
                    Relic Relic = relic.CreateRelic(___mainObject);
                    ___allRellics.Add(relic.Id, Relic);
                }
            }

            

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
