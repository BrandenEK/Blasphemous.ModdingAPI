using Framework.Managers;
using Gameplay.UI.Console;
using Gameplay.UI.Others;
using Gameplay.UI.Widgets;
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Blasphemous.ModdingAPI.Console;

// Allow access to console
[HarmonyPatch(typeof(ConsoleWidget), "Update")]
internal class ConsoleWidgetEnable_Patch
{
    public static void Postfix(ConsoleWidget __instance, bool ___isEnabled)
    {
        if (Main.ModdingAPI.InputHandler.GetKeyDown("Console"))
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
        foreach (ModCommand command in ConsoleModder.AllCommands)
        {
            ___commands.Add(new ModCommandSystem(command));
        }
    }
}
