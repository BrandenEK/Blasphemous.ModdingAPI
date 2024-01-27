using Framework.Managers;
using Gameplay.GameControllers.Effects.Player.Recolor;
using Gameplay.UI.Others.Buttons;
using Gameplay.UI.Others.MenuLogic;
using Gameplay.UI.Others;
using HarmonyLib;
using I2.Loc;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Blasphemous.ModdingAPI.Skins;

// Load custom skins from folder
[HarmonyPatch(typeof(ColorPaletteManager), "Initialize")]
internal class ColorPaletteManagerInit_Patch
{
    public static void Postfix(ColorPaletteDictionary ___palettes, Dictionary<string, bool> ___palettesStates)
    {
        Main.ModdingAPI.SkinLoader.LoadCustomSkins();

        foreach (SkinInfo skin in Main.ModdingAPI.SkinLoader.GetAllSkinInfos())
        {
            PalettesById palette = new PalettesById();
            palette.id = skin.id;
            palette.paletteTex = skin.texture;
            palette.palettePreview = skin.texture;
            ___palettes.PalettesById.Add(palette);
            if (!___palettesStates.ContainsKey(skin.id))
                ___palettesStates.Add(skin.id, true);
        }
    }
}

// Create menu options for custom skins
[HarmonyPatch(typeof(ExtrasMenuWidget), "Awake")]
internal class ExtrasMenuWidget_Patch
{
    public static void Postfix(List<string> ___allSkins, List<ExtrasMenuWidget.SkinSelectorElement> ___skinSelectorDataElements, List<ExtrasMenuWidget.SkinSelectorElement> ___skinSelectorSelectionElements)
    {
        for (int i = 0; i < ___allSkins.Count; i++)
        {
            addMissingElements(___allSkins[i], ___skinSelectorDataElements);
            addMissingElements(___allSkins[i], ___skinSelectorSelectionElements);
        }

        for (int i = 0; i < ___skinSelectorSelectionElements.Count; i++)
        {
            EventsButton button = ___skinSelectorSelectionElements[i].element.GetComponentInChildren<EventsButton>();
            if (button != null)
            {
                addEvent(button, ___skinSelectorSelectionElements[i].element, i);
            }
        }

        void addEvent(EventsButton button, GameObject obj, int skinIdx)
        {
            button.onSelected.RemoveAllListeners();
            button.onSelected = new EventsButton.ButtonSelectedEvent();
            button.onSelected.AddListener(delegate
            {
                ExtrasMenuWidget widget = Object.FindObjectOfType<ExtrasMenuWidget>();
                widget.Option_OnSelect(obj);
                widget.Option_OnSelectSkin(skinIdx);
            });
        }

        void addMissingElements(string skinId, List<ExtrasMenuWidget.SkinSelectorElement> skinElements)
        {
            bool elementExists = false;
            foreach (ExtrasMenuWidget.SkinSelectorElement skinElement in skinElements)
            {
                if (skinElement.skinKey == skinId)
                {
                    elementExists = true;
                    break;
                }
            }
            if (!elementExists)
            {
                SkinInfo skin = Main.ModdingAPI.SkinLoader.GetSkinInfo(skinId);
                string objName = "Skin" + skin.name.Replace(" ", string.Empty);

                GameObject firstElement = skinElements[0].element;
                GameObject newElement = Object.Instantiate(firstElement, firstElement.transform.parent);
                newElement.name = objName;

                Text text = newElement.GetComponentInChildren<Text>();
                if (text != null)
                {
                    Object.Destroy(text.GetComponent<Localize>());
                    text.name = objName + "Text";
                    text.text = skin.name;
                }
                EventsButton button = newElement.GetComponentInChildren<EventsButton>();
                if (button != null)
                {
                    Main.ModdingAPI.SkinLoader.AddSkinButton(button.gameObject);
                }

                ExtrasMenuWidget.SkinSelectorElement newSkinElement = new ExtrasMenuWidget.SkinSelectorElement();
                newSkinElement.skinKey = skin.id;
                newSkinElement.element = newElement;
                skinElements.Add(newSkinElement);
            }
        }
    }
}

// Add custom skins button allowed focus objects
[HarmonyPatch(typeof(KeepFocus), "Update")]
internal class KeepFocusSkins_Patch
{
    public static void Prefix(KeepFocus __instance, List<GameObject> ___allowedObjects)
    {
        if (!Main.ModdingAPI.SkinLoader.AllowedSkinButtons && __instance.name == "Extras_SkinSelector")
        {
            foreach (GameObject obj in Main.ModdingAPI.SkinLoader.AllowSkinButtons())
                ___allowedObjects.Add(obj);
        }
    }
}

// Display creator of each skin
[HarmonyPatch(typeof(ExtrasMenuWidget), "Option_OnSelectSkin")]
internal class SelectSkin_Patch
{
    public static bool Prefix(ExtrasMenuWidget __instance, int idx, List<string> ___allSkins)
    {
        Text text = __instance.transform.Find("Options/Extras_SkinSelector/SubText").GetComponent<Text>();

        SkinInfo customSkin = Main.ModdingAPI.SkinLoader.GetSkinInfo(___allSkins[idx]);
        text.text = Main.ModdingAPI.LocalizationHandler.Localize("cr") + ": " + (customSkin == null ? "TGK" : customSkin.author);
        return true;
    }
}

// Use default skin if custom one is removed
[HarmonyPatch(typeof(ColorPaletteManager), "GetCurrentColorPaletteId")]
internal class ColorPaletteManagerGet_Patch
{
    public static void Postfix(ColorPaletteManager __instance, ColorPaletteDictionary ___palettes, ref string __result)
    {
        foreach (PalettesById palette in ___palettes.PalettesById)
        {
            if (palette.id == __result) return;
        }
        __instance.SetCurrentColorPaletteId("PENITENT_DEFAULT");
        __result = "PENITENT_DEFAULT";
    }
}
