using HarmonyLib;
using System.Linq;
using Tools.Level.Actionables;
using UnityEngine;

namespace Blasphemous.ModdingAPI.Levels.Modifiers;

public class BloodModifier : IModifier
{
    public void Apply(GameObject obj, ObjectData data)
    {
        // Must be added in reverse order so that previous platforms can reference their new ones
        obj.name = data.id;

        Transform holder = Main.ModdingAPI.LevelHandler.CurrentObjectHolder;
        BloodFirst = bool.Parse(data.properties[0]);
        BloodObjects = data.properties.Skip(1).Select(faithId => holder.Find(faithId).gameObject).ToArray();

        SettingBloodPlatforms = true;
        obj.GetComponent<FaithPlatform>().Use();
        SettingBloodPlatforms = false;
    }

    internal static bool SettingBloodPlatforms { get; private set; }
    internal static GameObject[] BloodObjects { get; private set; }
    internal static bool BloodFirst { get; private set; }
}

// Set blood platform data when creating the object
[HarmonyPatch(typeof(FaithPlatform), "Use")]
internal class FaithPlatform_Patch
{
    public static bool Prefix(ref bool ___firstPlatform, ref GameObject[] ___target)
    {
        if (!BloodModifier.SettingBloodPlatforms)
            return true;

        ___firstPlatform = BloodModifier.BloodFirst;
        ___target = BloodModifier.BloodObjects;
        return false;
    }
}
