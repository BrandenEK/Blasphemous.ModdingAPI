using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Framework.Managers;
using Gameplay.UI.Widgets;
using Gameplay.UI.Others;
using Gameplay.GameControllers.Effects.Player.Recolor;
using System.Text;
using System.Collections.Generic;

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

            foreach (Mod mod in Main.moddingAPI.getMods())
            {
                sb.AppendFormat("{0} v{1}\n", mod.ModName, mod.ModVersion);
            }
            version.text = sb.ToString();
        }
    }

    // Allow access to console
    [HarmonyPatch(typeof(ConsoleWidget), "Update")]
    internal class ConsoleWidget_Patch
    {
        public static void Postfix(ConsoleWidget __instance, bool ___isEnabled)
        {
            if (Input.GetKeyDown(KeyCode.Backslash))
            {
                __instance.SetEnabled(!___isEnabled);
            }
        }
    }

    // Allow console commands on the main menu
    [HarmonyPatch(typeof(KeepFocus), "Update")]
    internal class KeepFocus_Patch
    {
        public static bool Prefix()
        {
            return ConsoleWidget.Instance == null || !ConsoleWidget.Instance.IsEnabled();
        }
    }
    [HarmonyPatch(typeof(ConsoleWidget), "SetEnabled")]
    internal class ConsoleWidgetDisable_Patch
    {
        public static void Postfix(bool enabled)
        {
            if (!enabled && Core.LevelManager.currentLevel.LevelName == "MainMenu")
            {
                Button[] buttons = Object.FindObjectsOfType<Button>();
                foreach (Button b in buttons)
                {
                    if (b.name == "Continue")
                    {
                        EventSystem.current.SetSelectedGameObject(b.gameObject);
                        return;
                    }
                }
            }
        }
    }

    // Load custom skins from folder
    [HarmonyPatch(typeof(ColorPaletteManager), "Initialize")]
    internal class ColorPaletteManager_Patch
    {
        public static void Postfix(ColorPaletteDictionary ___palettes)
        {
            Dictionary<string, Sprite> customSkins = new FileUtil().loadCustomSkins();

            foreach (string id in customSkins.Keys)
            {
                PalettesById palette = new PalettesById();
                palette.id = id;
                palette.paletteTex = customSkins[id];
                ___palettes.PalettesById.Add(palette);
                Main.LogMessage("Loading skin: " + id);
            }
        }
    }
}
