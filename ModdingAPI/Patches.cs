﻿using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Framework.Managers;
using Gameplay.UI.Widgets;
using Gameplay.UI.Others;
using Gameplay.UI.Others.MenuLogic;
using Gameplay.GameControllers.Effects.Player.Recolor;
using Gameplay.UI.Others.Buttons;
using System.Text;
using System.Collections.Generic;

namespace ModdingAPI
{
    // Initialize mods
    [HarmonyPatch(typeof(AchievementsManager), "AllInitialized")]
    internal class AchievementsManager_InitializePatch
    {
        public static void Postfix()
        {
            Main.moddingAPI.Initialize();
        }
    }
    // Dispose mods
    [HarmonyPatch(typeof(AchievementsManager), "Dispose")]
    internal class AchievementsManager_DisposePatch
    {
        public static void Postfix()
        {
            Main.moddingAPI.Dispose();
        }
    }

    // Add mod names to main menu
    [HarmonyPatch(typeof(VersionNumber), "Start")]
    internal class VersionNumber_Patch
    {
        public static void Postfix(VersionNumber __instance)
        {
            Text version = __instance.GetComponent<Text>();
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0} v{1}\n", Main.MOD_NAME, Main.MOD_VERSION);

            foreach (Mod mod in Main.moddingAPI.getMods())
            {
                sb.AppendFormat("{0} v{1}\n", mod.ModName, mod.ModVersion);
            }
            version.text = sb.ToString();
        }
    }

    // Allow access to console
    [HarmonyPatch(typeof(ConsoleWidget), "Update")]
    internal class ConsoleWidget_Patch
    {
        public static void Postfix(ConsoleWidget __instance, bool ___isEnabled)
        {
            if (Input.GetKeyDown(KeyCode.Backslash))
            {
                __instance.SetEnabled(!___isEnabled);
            }
        }
    }

    // Allow console commands on the main menu
    [HarmonyPatch(typeof(KeepFocus), "Update")]
    internal class KeepFocusMain_Patch
    {
        public static bool Prefix()
        {
            return ConsoleWidget.Instance == null || !ConsoleWidget.Instance.IsEnabled();
        }
    }
    [HarmonyPatch(typeof(ConsoleWidget), "SetEnabled")]
    internal class ConsoleWidgetDisable_Patch
    {
        public static void Postfix(bool enabled)
        {
            if (!enabled && Core.LevelManager.currentLevel.LevelName == "MainMenu")
            {
                Button[] buttons = Object.FindObjectsOfType<Button>();
                foreach (Button b in buttons)
                {
                    if (b.name == "Continue")
                    {
                        EventSystem.current.SetSelectedGameObject(b.gameObject);
                        return;
                    }
                }
            }
        }
    }

    // Load custom skins from folder
    [HarmonyPatch(typeof(ColorPaletteManager), "Initialize")]
    internal class ColorPaletteManager_Patch
    {
        public static void Postfix(ColorPaletteDictionary ___palettes, Dictionary<string, bool> ___palettesStates)
        {
            // Change to load skins into the skin laoder - Then loop through there
            Dictionary<string, Sprite> customSkins = new FileUtil().loadCustomSkins();

            foreach (string id in customSkins.Keys)
            {
                PalettesById palette = new PalettesById();
                palette.id = id;
                palette.paletteTex = customSkins[id];
                palette.palettePreview = customSkins[id];
                ___palettes.PalettesById.Add(palette);
                if (!___palettesStates.ContainsKey(id))
                    ___palettesStates.Add(id, true);
                Main.LogMessage("Loading custom skin: " + id);
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
                addMissingElements(___allSkins[i], i, ___skinSelectorDataElements);
                addMissingElements(___allSkins[i], i, ___skinSelectorSelectionElements);
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

            void addMissingElements(string skinId, int skinIndex, List<ExtrasMenuWidget.SkinSelectorElement> skinElements)
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
                    GameObject firstElement = skinElements[0].element;
                    GameObject newElement = Object.Instantiate(firstElement, firstElement.transform.parent);
                    newElement.name = skinId;

                    Text text = newElement.GetComponentInChildren<Text>();
                    if (text != null)
                    {
                        text.name = skinId + "Text";
                        text.text = skinId;
                    }
                    EventsButton button = newElement.GetComponentInChildren<EventsButton>();
                    if (button != null)
                    {
                        Main.moddingAPI.skinLoader.addSkinButton(button.gameObject);
                    }

                    ExtrasMenuWidget.SkinSelectorElement newSkinElement = new ExtrasMenuWidget.SkinSelectorElement();
                    newSkinElement.skinKey = skinId;
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
            if (!Main.moddingAPI.skinLoader.allowedSkinButtons && __instance.name == "Extras_SkinSelector")
            {
                foreach (GameObject obj in Main.moddingAPI.skinLoader.allowSkinButtons())
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

            SkinInfo customSkin = Main.moddingAPI.skinLoader.getSkinInfo(___allSkins[idx]);
            text.text = "Created by: " + (customSkin == null ? "TGK" : customSkin.author);
            return true;
        }
    }
}
