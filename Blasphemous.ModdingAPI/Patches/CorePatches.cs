using Framework.Managers;
using HarmonyLib;

namespace Blasphemous.ModdingAPI.Patches;

[HarmonyPatch(typeof(Core), nameof(Core.PreInit))]
class Core_PreInit_Patch
{
    public static void Prefix()
    {
        Main.ModLoader.PreInitialize();
    }

    public static void Postfix()
    {
        Main.ModLoader.Initialize();
    }
}

[HarmonyPatch(typeof(Core), nameof(Core.Initialize))]
class Core_Initialize_Patch
{
    public static void Postfix()
    {
        Main.ModLoader.PostInitialize();
    }
}

[HarmonyPatch(typeof(Core), nameof(Core.OnDestroy))]
class Core_OnDestroy_Patch
{
    public static void Prefix()
    {
        Main.ModLoader.Dispose();
    }
}
