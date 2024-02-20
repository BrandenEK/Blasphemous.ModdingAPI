using Framework.Managers;
using Gameplay.UI.Others.MenuLogic;
using HarmonyLib;
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
    }
}

//[HarmonyPatch(typeof(NewMainMenu), "Update")]
//class Menu_Update_Patch
//{
//    public static void Postfix(NewMainMenu.MenuState menuState)
//}
