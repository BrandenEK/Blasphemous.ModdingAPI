using Framework.Managers;
using Gameplay.UI.Others.MenuLogic;
using HarmonyLib;
using System.Linq;
using System.Text;
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

[HarmonyPatch(typeof(VersionNumber), "Start")]
internal class VersionNumber_Patch
{
    public static void Postfix(VersionNumber __instance)
    {
        Text version = __instance.GetComponent<Text>();

        StringBuilder sb = new StringBuilder($"Loaded {Main.ModLoader.AllMods.Count()} mods:").AppendLine();
        foreach (var mod in Main.ModLoader.AllMods)
        {
            sb.AppendLine($"{mod.Name} v{mod.Version}");
        }

        //version.alignment = TextAnchor.UpperRight;
        //version.text = sb.ToString();
        //Canvas c = Object.FindObjectOfType<Canvas>();
        Canvas c = Object.FindObjectsOfType<Canvas>().First(x => x.name == "Game UI (No Pixel Perfect)");
        foreach (Canvas canvas in Object.FindObjectsOfType<Canvas>())
        {
            Main.ModdingAPI.LogError(canvas.name);
            Main.ModdingAPI.LogWarning(canvas.referencePixelsPerUnit);
            Main.ModdingAPI.LogWarning(canvas.GetComponent<CanvasScaler>()?.referenceResolution);
        }
        //foreach (Transform child in c.transform)
        //{
        //    Main.ModdingAPI.LogError(child.name);
        //    foreach (Transform nchild in child)
        //    {
        //        Main.ModdingAPI.LogWarning(nchild.name);
        //    }
        //}

        RectTransform r = new GameObject().AddComponent<RectTransform>();
        r.name = "Mod list";
        r.SetParent(c.transform, false);
        r.anchorMin = new Vector2(0, 0);
        r.anchorMax = new Vector2(1, 1);
        r.pivot = new Vector2(0, 1);
        r.anchoredPosition = new Vector2(20, -15);
        r.sizeDelta = new Vector2(400, 100);
        Text t = r.gameObject.AddComponent<Text>();
        t.text = sb.ToString();
        t.alignment = TextAnchor.UpperLeft;
        t.font = version.font;
        t.fontSize = 32;
    }
}
