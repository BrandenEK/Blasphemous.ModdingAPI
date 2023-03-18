using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using Framework.Managers;
using Gameplay.UI.Others.MenuLogic;
using System.Text;

namespace ModdingAPI
{
    // Initialize mods
    [HarmonyPatch(typeof(AchievementsManager), "AllInitialized")]
    internal class AchievementsManager_InitializePatch
    {
        public static void Postfix()
        {
            Main.moddingAPI.Initialize();
        }
    }
    // Dispose mods
    [HarmonyPatch(typeof(AchievementsManager), "Dispose")]
    internal class AchievementsManager_DisposePatch
    {
        public static void Postfix()
        {
            Main.moddingAPI.Dispose();
        }
    }

    // Add mod names to main menu
    [HarmonyPatch(typeof(VersionNumber), "Start")]
    internal class VersionNumber_Patch
    {
        public static void Postfix(VersionNumber __instance)
        {
            Text version = __instance.GetComponent<Text>();
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0} v{1}\n", Main.MOD_NAME, Main.MOD_VERSION);

            foreach (Mod mod in Main.moddingAPI.GetMods())
            {
                sb.AppendFormat("{0} v{1}\n", mod.ModName, mod.ModVersion);
            }

            version.alignment = TextAnchor.UpperRight;
            version.text = sb.ToString();
        }
    }

    // Call new game when game is first started
    [HarmonyPatch(typeof(NewMainMenu), "InternalPlay")]
    internal class NewMainMenu_Patch
    {
        public static void Postfix(bool ___isContinue)
        {
            if (!___isContinue)
                Main.moddingAPI.NewGame();
        }
    }
}
