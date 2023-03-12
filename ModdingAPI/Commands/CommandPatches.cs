using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Framework.Managers;
using Gameplay.UI.Widgets;
using Gameplay.UI.Console;
using Gameplay.UI.Others;
using System.Collections.Generic;

namespace ModdingAPI.Patches
{
    // Allow access to console
    [HarmonyPatch(typeof(ConsoleWidget), "Update")]
    internal class ConsoleWidgetEnable_Patch
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
    internal class KeepFocusMain_Patch
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

    // Add custom mod commands to the console
    [HarmonyPatch(typeof(ConsoleWidget), "InitializeCommands")]
    internal class ConsoleWidgetInitialize_Patch
    {
        public static void Postfix(List<ConsoleCommand> ___commands)
        {
            foreach (ModCommand command in Main.moddingAPI.GetModCommands())
            {
                ___commands.Add(new ModCommandSystem(command));
            }
        }
    }
}
