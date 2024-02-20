using Framework.Managers;
using Gameplay.UI;
using Gameplay.UI.Others;
using Gameplay.UI.Others.MenuLogic;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Blasphemous.ModdingAPI;

[HarmonyPatch(typeof(AchievementsManager), nameof(AchievementsManager.AllInitialized))]
class Mod_AllInitialized_Patch
{
    public static void Postfix() => Main.ModLoader.Initialize();
}

[HarmonyPatch(typeof(AchievementsManager), nameof(AchievementsManager.Dispose))]
class Mod_Dispose_Patch
{
    public static void Postfix() => Main.ModLoader.Dispose();
}

[HarmonyPatch(typeof(NewMainMenu), "InternalPlay")]
class Mod_NewLoad_Patch
{
    public static void Postfix(bool ___isContinue, bool ___mustConvertToNewgamePlus)
    {
        if (!___isContinue || ___mustConvertToNewgamePlus)
            Main.ModLoader.ProcessModFunction(mod => mod.OnNewGame());
        else
            Main.ModLoader.ProcessModFunction(mod => mod.OnLoadGame());

        Core.Persistence.SaveGame();
    }
}

/// <summary>
/// Store the blasphemous font when game is started
/// </summary>
[HarmonyPatch(typeof(VersionNumber), "Start")]
internal class VersionNumber_Patch
{
    public static void Postfix(VersionNumber __instance)
    {
        Text version = __instance.GetComponent<Text>();
        Main.ModdingAPI.BlasFont = version.font;
        Main.ModdingAPI.VersionNumber = __instance;
        Main.ModdingAPI.MenuUI = __instance.transform.parent.parent.gameObject;

        Main.ModdingAPI.LogError(__instance.transform.parent.parent.parent.DisplayHierarchy(10, true));
    }
}

[HarmonyPatch(typeof(NewMainMenu), "SetState")]
class Menu_Update_Patch
{
    public static void Postfix(Animator ___animator)
    {
        Main.ModdingAPI.LogError("State: " + ___animator.GetInteger("STATUS"));
    }
}

[HarmonyPatch(typeof(NewMainMenu), nameof(NewMainMenu.OptionBossRush))]
class t1
{
    public static void Postfix() => Main.ModdingAPI.ShowMenu = false;
}

[HarmonyPatch(typeof(NewMainMenu), nameof(NewMainMenu.OptionCampain))]
class t2
{
    public static void Postfix() => Main.ModdingAPI.ShowMenu = false;
}

[HarmonyPatch(typeof(NewMainMenu), nameof(NewMainMenu.OptionExtras))]
class t3
{
    public static void Postfix() => Main.ModdingAPI.ShowMenu = false;
}

[HarmonyPatch(typeof(NewMainMenu), nameof(NewMainMenu.OptionOptions))]
class t4
{
    public static void Postfix() => Main.ModdingAPI.ShowMenu = false;
}

[HarmonyPatch(typeof(NewMainMenu), nameof(NewMainMenu.ShowMenu))]//"ShowMainMenuOptions")]
class t5
{
    public static void Postfix(KeepFocus ___mainMenuKeepFocus)
    {
        Main.ModdingAPI.Focuser = ___mainMenuKeepFocus;
    }
}

[HarmonyPatch(typeof(NewMainMenu), nameof(NewMainMenu.ShowChooseBackground))]
class t6
{
    public static void Postfix() => Main.ModdingAPI.ShowMenu = false;
}

[HarmonyPatch(typeof(NewMainMenu), "Update")]
class t7
{
    public static void Postfix(bool ___menuButtonsNavEnabled, List<GameObject> ___mainMenuOptions, List<Button> ___AllButtons)
    {
        //Main.ModdingAPI.Nav = ___menuButtonsNavEnabled;

        //Main.ModdingAPI.Log(___mainMenuOptions.Where(x => x.activeInHierarchy).Count());

        foreach (var button in ___AllButtons)
        {
            Text text = button.transform.GetChild(1).GetComponent<Text>();
            Main.ModdingAPI.LogError(text.name + " " + text.color);
        }

        Main.ModdingAPI.Nav = (___AllButtons.Any(x => x.transform.GetChild(1).GetComponent<Text>().color != Color.white));
    }
}

//[HarmonyPatch(typeof(KeepFocus), "Update")]
//class Focus_Update_Patch
//{
//    public static void Prefix(List<GameObject> ___allowedObjects)
//    {
//        Main.ModdingAPI.Nav = ___allowedObjects.Any(x => EventSystem.current.currentSelectedGameObject == x);
//    }
//}
