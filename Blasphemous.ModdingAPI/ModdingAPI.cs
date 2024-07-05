using Framework.Managers;
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

    /// <summary>
    /// Stores the Blasphemous pixel font to use for the mod list
    /// </summary>
    public Font BlasFont { get; set; }

    /// <summary>
    /// Stores the amount of fervour that should be held after loading the game
    /// </summary>
    public float UnsavedFervourAmount { get; set; }

    protected internal override void OnLoadGame()
    {
        Core.Logic.Penitent.Stats.Fervour.Current = UnsavedFervourAmount;
    }

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
        StringBuilder fullText = new();
        StringBuilder shadowText = new();
        foreach (var mod in Main.ModLoader.AllMods.OrderBy(GetModPriority).ThenBy(x => x.Name))
        {
            fullText.AppendLine(GetModText(mod, true));
            shadowText.AppendLine(GetModText(mod, false));
        }

        // Create rect transform for shadow
        RectTransform r = new GameObject().AddComponent<RectTransform>();
        r.name = "Mod list";
        r.SetParent(canvas.transform, false);
        r.anchorMin = new Vector2(0, 0);
        r.anchorMax = new Vector2(1, 1);
        r.pivot = new Vector2(0, 1);
        r.anchoredPosition = new Vector2(20, -15);
        r.sizeDelta = new Vector2(400, 100);

        // Create text for shadow
        Text t = r.gameObject.AddComponent<Text>();
        t.text = shadowText.ToString();
        t.alignment = TextAnchor.UpperLeft;
        t.color = Color.black;
        t.font = BlasFont;
        t.fontSize = 32;

        // Store game object
        _modList = t.gameObject;

        // Duplicate shadow for real text
        GameObject real = Object.Instantiate(_modList, _modList.transform);
        Text st = real.GetComponent<Text>();
        st.supportRichText = true;
        st.text = fullText.ToString();
        st.rectTransform.anchoredPosition = new Vector2(-1, 2);
    }

    private int GetModPriority(BlasMod mod)
    {
        if (mod == this)
            return -1;

        if (mod.Name.EndsWith("Framework"))
            return 0;

        return 1;
    }

    private string GetModText(BlasMod mod, bool addColor)
    {
        string line = $"{mod.Name} v{mod.Version}";

        if (!addColor)
            return line;

        string color = mod == this || mod.Name.EndsWith("Framework") ? "7CA7BF" : "D3D3D3";
        return $"<color=#{color}>{line}</color>";
    }
}
