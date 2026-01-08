using Framework.Managers;
using HarmonyLib;

namespace Blasphemous.ModdingAPI.Patches;

[HarmonyPatch(typeof(Core), nameof(Core.PreInit))]
class Core_PreInit_Patch
{
    public static void Prefix()
    {
        ModLog.Warn("PreInit prefix");
    }

    public static void Postfix()
    {
        ModLog.Warn("PreInit postfix");
    }
}

[HarmonyPatch(typeof(Core), nameof(Core.Initialize))]
class Core_Initialize_Patch
{
    public static void Prefix()
    {
        ModLog.Warn("Initialize prefix");
    }

    public static void Postfix()
    {
        ModLog.Warn("Initialize postfix");
    }
}

[HarmonyPatch(typeof(Core), nameof(Core.OnDestroy))]
class Core_OnDestroy_Patch
{
    public static void Prefix()
    {
        ModLog.Warn("OnDestroy prefix");
    }

    public static void Postfix()
    {
        ModLog.Warn("OnDestroy postfix");
    }
}
