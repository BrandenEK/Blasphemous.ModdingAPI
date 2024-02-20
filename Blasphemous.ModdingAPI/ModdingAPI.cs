using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Blasphemous.ModdingAPI;

internal class ModdingAPI : BlasMod
{
    public ModdingAPI() : base(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_AUTHOR, ModInfo.MOD_VERSION) { }

    private GameObject _modList;

    public Font BlasFont { get; set; }
    public VersionNumber VersionNumber { get; set; }

    protected internal override void OnLevelLoaded(string oldLevel, string newLevel)
    {
        if (newLevel != "MainMenu" || _modList != null)
            return;

        // Find highres canvas
        Canvas canvas = Object.FindObjectsOfType<Canvas>().FirstOrDefault(x => x.name == "Game UI (No Pixel Perfect)");
        if (canvas == null)
            return;
        foreach (Transform child in canvas.transform)
        {
            LogError(child.name);
        }

        // Create text for mod list
        StringBuilder sb = new StringBuilder($"Loaded {Main.ModLoader.AllMods.Count()} mods:").AppendLine();
        foreach (var mod in Main.ModLoader.AllMods)
        {
            sb.AppendLine($"{mod.Name} v{mod.Version}");
        }

        // Create rect transform
        RectTransform r = new GameObject().AddComponent<RectTransform>();
        r.name = "Mod list";
        r.SetParent(canvas.transform, false);
        r.anchorMin = new Vector2(0, 0);
        r.anchorMax = new Vector2(1, 1);
        r.pivot = new Vector2(0, 1);
        r.anchoredPosition = new Vector2(20, -15);
        r.sizeDelta = new Vector2(400, 100);

        // Create text
        Text t = r.gameObject.AddComponent<Text>();
        t.text = sb.ToString();
        t.alignment = TextAnchor.UpperLeft;
        t.font = BlasFont;
        t.fontSize = 32;

        _modList = t.gameObject;
    }

    protected internal override void OnUpdate()
    {
        LogWarning("VN active: " + (VersionNumber != null && VersionNumber.isActiveAndEnabled));
    }
}
