using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Blasphemous.ModdingAPI;

internal class ModdingAPI : BlasMod
{
    public ModdingAPI() : base(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_AUTHOR, ModInfo.MOD_VERSION) { }

    private GameObject _modList;
    private bool _loadedMenu;

    public Font BlasFont { get; set; }

    protected internal override void OnLevelLoaded(string oldLevel, string newLevel)
    {
        if (newLevel == "MainMenu")
            _loadedMenu = true;
    }

    protected internal override void OnLevelUnloaded(string oldLevel, string newLevel)
    {
        ShowMenu = false;
    }

    public bool ShowMenu
    {
        set
        {
            if (value && _loadedMenu && LoadStatus.CurrentScene == "MainMenu")
            {
                if (_modList == null)
                    CreateModList();

                _modList?.SetActive(true);
            }
            else
            {
                _modList?.SetActive(false);
            }
        }
    }

    private void CreateModList()
    {
        // Find highres canvas
        Canvas canvas = Object.FindObjectsOfType<Canvas>().FirstOrDefault(x => x.name == "Game UI (No Pixel Perfect)");
        if (canvas == null)
            return;

        // Create text for mod list
        //StringBuilder sb = new StringBuilder($"Loaded {Main.ModLoader.AllMods.Count()} mods:").AppendLine().AppendLine();
        StringBuilder sb = new();
        foreach (var mod in Main.ModLoader.AllMods.OrderBy(GetModPriority).ThenBy(x => x.Name))
        {
            sb.AppendLine(GetModText(mod));
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
        t.supportRichText = true;

        _modList = t.gameObject;
    }

    private int GetModPriority(BlasMod mod)
    {
        if (mod == this)
            return -1;

        if (mod.Name.EndsWith("Framework"))
            return 0;

        return 1;
    }

    private string GetModText(BlasMod mod)
    {
        bool dependency = mod == this || mod.Name.EndsWith("Framework");
        string line = $"{mod.Name} v{mod.Version}";

        string color = dependency ? "7CA7BF" : "D3D3D3";
        return $"<color=#{color}>{line}</color>";
    }
}
