using HarmonyLib;
using Framework.Managers;
using Framework.Penitences;
using Gameplay.UI.Others.MenuLogic;
using Gameplay.UI.Widgets;
using Gameplay.GameControllers.Entities;
using System.Collections.Generic;

namespace ModdingAPI
{
    internal class PenitencePatches
    {
        // Add custom penitences to list
        [HarmonyPatch(typeof(PenitenceManager), "ResetPenitencesList")]
        public class PenitenceManager_Patch
        {
            public static void Postfix(List<IPenitence> ___allPenitences)
            {
                foreach (ModPenitence penitence in Main.moddingAPI.GetModPenitences())
                {
                    ___allPenitences.Add(new ModPenitenceSystem(penitence.Id));
                }
            }
        }

        // Add config settings for custom penitences
        [HarmonyPatch(typeof(PenitenceSlot), "SetPenitenceConfig")]
        public class PenitenceSlot_Patch
        {
            public static void Postfix(Dictionary<string, SelectSaveSlots.PenitenceData> ___allPenitences)
            {
                foreach (SelectSaveSlots.PenitenceData data in Main.moddingAPI.penitenceLoader.GetPenitenceData(true))
                {
                    ___allPenitences.Add(data.id, data);
                }
            }
        }
        [HarmonyPatch(typeof(GameplayWidget), "Awake")]
        public class GameplayWidget_Patch
        {
            public static void Postfix(List<SelectSaveSlots.PenitenceData> ___PenitencesConfig)
            {
                foreach (SelectSaveSlots.PenitenceData data in Main.moddingAPI.penitenceLoader.GetPenitenceData(false))
                {
                    ___PenitencesConfig.Add(data);
                }
            }
        }
    }
}
