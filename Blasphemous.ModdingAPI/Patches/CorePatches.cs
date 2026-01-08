using Framework.Managers;
using HarmonyLib;

namespace Blasphemous.ModdingAPI.Patches;

[HarmonyPatch(typeof(Core), nameof(Core.PreInit))]
class Core_PreInit_Patch
{
    public static void Prefix()
    {
        ModLog.Error("PreInit prefix");
    }

    public static void Postfix()
    {
        ModLog.Error("PreInit postfix");
    }
}

[HarmonyPatch(typeof(Core), nameof(Core.Initialize))]
class Core_Initialize_Patch
{
    public static void Prefix()
    {
        ModLog.Error("Initialize prefix");
    }

    public static void Postfix()
    {
        ModLog.Error("Initialize postfix");
    }
}

[HarmonyPatch(typeof(Core), nameof(Core.OnDestroy))]
class Core_OnDestroy_Patch
{
    public static void Prefix()
    {
        ModLog.Error("OnDestroy prefix");
    }

    public static void Postfix()
    {
        ModLog.Error("OnDestroy postfix");
    }
}

[HarmonyPatch(typeof(AchievementsManager), nameof(AchievementsManager.Initialize))]
class t1
{
    public static void Postfix()
    {
        ModLog.Error("AchievementsManager Initialize postfix");
    }
}

[HarmonyPatch(typeof(AchievementsManager), nameof(AchievementsManager.AllInitialized))]
class t2
{
    public static void Postfix()
    {
        ModLog.Error("AchievementsManager AllInitialized postfix");
    }
}
