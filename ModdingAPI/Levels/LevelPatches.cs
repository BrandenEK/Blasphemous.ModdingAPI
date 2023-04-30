using HarmonyLib;
using Tools.Level.Layout;
using Tools.Level.Actionables;
using UnityEngine;

namespace ModdingAPI.Levels
{
    // Prevent init functions when loading temp level
    [HarmonyPatch(typeof(LevelInitializer), "Awake")]
    internal class LevelInit_Patch
    {
        public static bool Prefix()
        {
            return !Main.moddingAPI.LevelLoader.InLoadProcess;
        }
    }

    // Set blood platform data when creating the object
    [HarmonyPatch(typeof(FaithPlatform), "Use")]
    internal class FaithPlatform_Patch
    {
        public static bool Prefix(ref bool ___firstPlatform, ref GameObject[] ___target)
        {
            if (!LevelLoader.SettingBloodPlatforms)
                return true;

            Main.LogError(Main.MOD_NAME, "Setting fiath platofm prpos!");
            ___firstPlatform = LevelLoader.BloodFirst;
            ___target = LevelLoader.BloodObjects;
            return false;
        }
    }
}
