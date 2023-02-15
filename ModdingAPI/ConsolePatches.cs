using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Framework.Managers;
using Gameplay.UI.Widgets;
using Gameplay.UI.Others;

namespace ModdingAPI
{
    // Allow access to console
    [HarmonyPatch(typeof(ConsoleWidget), "Update")]
    public class ConsoleWidget_Patch
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
    public class KeepFocus_Patch
    {
        public static bool Prefix()
        {
            return ConsoleWidget.Instance == null || !ConsoleWidget.Instance.IsEnabled();
        }
    }
    [HarmonyPatch(typeof(ConsoleWidget), "SetEnabled")]
    public class ConsoleWidgetDisable_Patch
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
}
