using Framework.Managers;
using Gameplay.GameControllers.Entities;
using Gameplay.UI.Others.MenuLogic;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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
        Text text = __instance.GetComponent<Text>();
        if (text == null || text.font == null)
            return;

        Main.ModdingAPI.BlasFont = text.font;
    }
}

/// <summary>
/// Show the mod list if any of the menu buttons are selected
/// </summary>
[HarmonyPatch(typeof(NewMainMenu), "Update")]
class Menu_Update_Patch
{
    public static void Postfix(List<Button> ___AllButtons)
    {
        Main.ModdingAPI.ShowMenu = ___AllButtons.Any(x => x.transform.GetChild(1).GetComponent<Text>().color != Color.white);
    }
}

/// <summary>
/// Save the actual fervour amount when the game is first loaded
/// </summary>
[HarmonyPatch(typeof(EntityStats), nameof(EntityStats.SetCurrentPersistentState))]
class Stats_Load_Patch
{
    public static void Postfix(PersistentManager.PersistentData data, bool isloading)
    {
        if (!isloading)
            return;

        EntityStats.StatsPersistenceData statsData = data as EntityStats.StatsPersistenceData;
        float fervourAmount = statsData.currentValues[EntityStats.StatsTypes.Fervour];

        Main.ModdingAPI.Log($"Storing {fervourAmount} fervour to be restored after loading the game");
        Main.ModdingAPI.UnsavedFervourAmount = fervourAmount;
    }
}
