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
                
            }

            void setBaseProperties(BaseInventoryObject obj, ModItem item)
            {
                obj.id = item.Id;
                obj.caption = item.Name;
                obj.description = item.Description;
                obj.lore = item.Lore;
                obj.picture = item.Picture;
                obj.carryonstart = item.CarryOnStart;
                obj.preserveInNewGamePlus = item.PreserveInNGPlus;
            }
        }

        void TempBead()
        {
            //// Create object
            //Transform parent = ___mainObject.transform.Find("RosaryBead");
            //GameObject obj = new GameObject(item.Id);
            //obj.transform.SetParent(parent);

            //// Set bead properties
            //RosaryBead bead = obj.AddComponent<RosaryBead>();
            //setBaseProperties(bead, item);
            ////bead.UsePercentageCompletition = item.percent;

            //// Add object effects
            ////foreach (ModItemEffect effect in modBead.effects)
            ////{
            ////    obj.AddComponent<ModItemEffectSystem>().SetEffect(effect);
            ////}

            //// Add bead to list
            //___allBeads.Add(item.Id, bead);
            //if (bead.carryonstart && PersistentManager.GetAutomaticSlot() >= 0)
            //{
            //    __instance.AddRosaryBead(bead);
            //}
        }
    }
}
