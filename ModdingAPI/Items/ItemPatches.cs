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
        public static void Postfix(InventoryManager __instance, GameObject ___mainObject, Dictionary<string, RosaryBead> ___allBeads)
        {
            foreach (ModItem item in Main.moddingAPI.GetModItems())
            {
                if (item is ModRosaryBead bead)
                {
                    RosaryBead Bead = CreateRosaryBead(bead);

                    // Add object effects
                    foreach (ModItemEffect effect in bead.Effects)
                    {
                        Bead.gameObject.AddComponent<ModItemEffectSystem>().SetEffect(effect);
                    }

                    // Add bead to list
                    ___allBeads.Add(item.Id, Bead);
                    //if (bead.CarryOnStart && PersistentManager.GetAutomaticSlot() >= 0)
                    //{
                    //    __instance.AddRosaryBead(Bead);
                    //}
                }
            }

            RosaryBead CreateRosaryBead(ModRosaryBead bead)
            {
                // Create object
                GameObject obj = new GameObject(bead.Id);
                obj.transform.SetParent(___mainObject.transform.Find("RosaryBead"));

                // Set properties
                RosaryBead Bead = obj.AddComponent<RosaryBead>();
                Bead.id = bead.Id;
                Bead.caption = bead.Name;
                Bead.description = bead.Description;
                Bead.lore = bead.Lore;
                Bead.picture = bead.Picture;
                Bead.carryonstart = bead.CarryOnStart;
                Bead.preserveInNewGamePlus = bead.PreserveInNGPlus;
                Bead.UsePercentageCompletition = bead.AddToPercentageCompletion;

                return Bead;
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
