using HarmonyLib;
using Tools.Level.Layout;

namespace Blasphemous.ModdingAPI.Levels;

// Prevent init functions when loading temp level
[HarmonyPatch(typeof(LevelInitializer), "Awake")]
internal class LevelInit_Patch
{
    public static bool Prefix() => !Main.ModdingAPI.LevelHandler.InLoadProcess;
}
