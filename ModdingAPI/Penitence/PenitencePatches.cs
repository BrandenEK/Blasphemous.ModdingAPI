using System;
using System.Collections.Generic;
using HarmonyLib;
using Framework.Managers;
using Framework.Penitences;
using Gameplay.UI;
using Gameplay.UI.Others.MenuLogic;
using Gameplay.UI.Widgets;
using UnityEngine;
using UnityEngine.UI;
using I2.Loc;

namespace ModdingAPI
{
    internal class PenitencePatches
    {
        // Add custom penitences to list
        [HarmonyPatch(typeof(PenitenceManager), "ResetPenitencesList")]
        public class PenitenceManager_Patch
        {
            public static void Postfix(List<IPenitence> ___allPenitences)
            {
                foreach (ModPenitence penitence in Main.moddingAPI.GetModPenitences())
                {
                    ___allPenitences.Add(new ModPenitenceSystem(penitence.Id));
                }
            }
        }

        // Add config settings for custom penitences
        [HarmonyPatch(typeof(PenitenceSlot), "SetPenitenceConfig")]
        public class PenitenceSlot_Patch
        {
            public static void Postfix(Dictionary<string, SelectSaveSlots.PenitenceData> ___allPenitences)
            {
                foreach (SelectSaveSlots.PenitenceData data in Main.moddingAPI.penitenceLoader.GetPenitenceData(true))
                {
                    ___allPenitences.Add(data.id, data);
                }
            }
        }
        [HarmonyPatch(typeof(GameplayWidget), "Awake")]
        public class GameplayWidget_Patch
        {
            public static void Postfix(List<SelectSaveSlots.PenitenceData> ___PenitencesConfig)
            {
                foreach (SelectSaveSlots.PenitenceData data in Main.moddingAPI.penitenceLoader.GetPenitenceData(false))
                {
                    ___PenitencesConfig.Add(data);
                }
            }
        }

        // Set selecting status when changing options
        [HarmonyPatch(typeof(ChoosePenitenceWidget), "Option_SelectPE01")]
        public class ChoosePenitenceWidgetSelect1_Patch
        {
            public static void Postfix() { Main.moddingAPI.penitenceLoader.SelectingCustomPenitence = false; }
        }
        [HarmonyPatch(typeof(ChoosePenitenceWidget), "Option_SelectPE02")]
        public class ChoosePenitenceWidgetSelect2_Patch
        {
            public static void Postfix() { Main.moddingAPI.penitenceLoader.SelectingCustomPenitence = false; }
        }
        [HarmonyPatch(typeof(ChoosePenitenceWidget), "Option_SelectPE03")]
        public class ChoosePenitenceWidgetSelect3_Patch
        {
            public static void Postfix() { Main.moddingAPI.penitenceLoader.SelectingCustomPenitence = false; }
        }
        [HarmonyPatch(typeof(ChoosePenitenceWidget), "OnClose")]
        public class ChoosePenitenceWidgetClose_Patch
        {
            public static void Postfix() { Main.moddingAPI.penitenceLoader.SelectingCustomPenitence = false; }
        }

        // Update selection status when selecting normally or changing custom penitences
        [HarmonyPatch(typeof(ChoosePenitenceWidget), "Option_SelectNoPenitence")]
        public class ChoosePenitenceWidgetSelectNone_Patch
        {
            public static bool Prefix(Text ___penitenceTitle, Text ___penitenceInfoText, CustomScrollView ___penitenceScroll)
            {
                if (Main.moddingAPI.penitenceLoader.customStatus)
                {
                    // Pressed lb/rb to update custom penitence
                    int currPenitenceIdx = Main.moddingAPI.penitenceLoader.CurrentSelectedCustomPenitence;
                    if (currPenitenceIdx > 0)
                    {
                        ModPenitence currentPenitence = Main.moddingAPI.GetModPenitences()[currPenitenceIdx - 1];
                        ___penitenceTitle.text = currentPenitence.Name;
                        ___penitenceInfoText.text = currentPenitence.Description;
                        if (Main.moddingAPI.penitenceLoader.SelectPenitenceImage != null)
                            Main.moddingAPI.penitenceLoader.SelectPenitenceImage.sprite = currentPenitence.SelectionImage;
                    }
                    else
                    {
                        ___penitenceTitle.text = ScriptLocalization.UI_Penitences.NO_PENITENCE;
                        ___penitenceInfoText.text = ScriptLocalization.UI_Penitences.NO_PENITENCE_INFO;
                        if (Main.moddingAPI.penitenceLoader.SelectPenitenceImage != null)
                            Main.moddingAPI.penitenceLoader.SelectPenitenceImage.sprite = Main.moddingAPI.penitenceLoader.NoPenitenceImage;
                    }
                    ___penitenceScroll.NewContentSetted();
                    return false;
                }
                else
                {
                    // First time selecting this slot
                    Main.moddingAPI.penitenceLoader.SelectingCustomPenitence = true;
                    return true;
                }
            }
        }

        // Display choose penitence confirmation when choosing custom one
        [HarmonyPatch(typeof(UIController), "ShowConfirmationWidget", typeof(string), typeof(Action), typeof(Action))]
        public class UIController_Patch
        {
            public static void Prefix(ref string infoMessage)
            {
                if (Main.moddingAPI.penitenceLoader.CurrentSelectedCustomPenitence > 0)
                    infoMessage = ScriptLocalization.UI_Penitences.CHOOSE_PENITENCE_CONFIRMATION;
            }
        }

        // When confirming a custom penitence, activate it
        [HarmonyPatch(typeof(ChoosePenitenceWidget), "ContinueWithNoPenitenceAndClose")]
        public class ChoosePenitenceWidgetActivate_Patch
        {
            public static bool Prefix(ChoosePenitenceWidget __instance)
            {
                if (Main.moddingAPI.penitenceLoader.CurrentSelectedCustomPenitence > 0)
                {
                    Main.moddingAPI.penitenceLoader.ConfirmCustomPenitence();
                    __instance.Close();
                    return false;
                }
                return true;
            }
        }

        // Display buttons and store action when opening widget
        [HarmonyPatch(typeof(ChoosePenitenceWidget), "Open", typeof(Action), typeof(Action))]
        public class ChoosePenitenceWidgetOpen_Patch
        {
            public static void Postfix(ChoosePenitenceWidget __instance, Action onChoosingPenitence)
            {
                Main.moddingAPI.penitenceLoader.chooseAction = onChoosingPenitence;

                // Add lb/rb buttons
                Transform buttonHolder = UnityEngine.Object.FindObjectOfType<NewInventoryWidget>().transform.Find("External/Background/Headers/Inventory/Caption/Selector_Help/");
                Image selectionImage = Main.moddingAPI.penitenceLoader.SelectPenitenceImage;
                if (buttonHolder == null || selectionImage == null) return;

                Transform parent = selectionImage.transform.parent.parent;
                if (parent.childCount == 1)
                {
                    GameObject left = UnityEngine.Object.Instantiate(buttonHolder.GetChild(0).gameObject, parent);
                    GameObject right = UnityEngine.Object.Instantiate(buttonHolder.GetChild(1).gameObject, parent);
                    left.GetComponent<RectTransform>().anchoredPosition = new Vector2(-5, 10);
                    right.GetComponent<RectTransform>().anchoredPosition = new Vector2(5, 10);
                }
            }
        }

        // Show custom penitence when abandoning
        [HarmonyPatch(typeof(AbandonPenitenceWidget), "UpdatePenitenceTextsAndDisplayedMedal")]
        public class AbandonPenitenceWidgetOpen_Patch
        {
            public static bool Prefix(Text ___penitenceTitle, Text ___penitenceInfoText, GameObject ___PE01Medal, GameObject ___PE02Medal, GameObject ___PE03Medal)
            {
                if (Core.PenitenceManager.GetCurrentPenitence() is ModPenitenceSystem modPenitence)
                {
                    ModPenitence currPenitence = Main.moddingAPI.penitenceLoader.GetPenitence(modPenitence.Id);
                    if (currPenitence == null) return false;

                    Image medalImage = ___PE02Medal.GetComponentInChildren<Image>();
                    Main.moddingAPI.penitenceLoader.Penitence2Image = medalImage.sprite;
                    medalImage.sprite = currPenitence.SelectionImage;
                    ___penitenceTitle.text = currPenitence.Name;
                    ___penitenceInfoText.text = currPenitence.Description;

                    ___PE01Medal.SetActive(false);
                    ___PE02Medal.SetActive(true);
                    ___PE03Medal.SetActive(false);
                    return false;
                }
                return true;
            }
        }
        [HarmonyPatch(typeof(AbandonPenitenceWidget), "OnClose")]
        public class AbandonPenitenceWidgetClose_Patch
        {
            public static void Postfix(GameObject ___PE02Medal)
            {
                ___PE02Medal.GetComponentInChildren<Image>().sprite = Main.moddingAPI.penitenceLoader.Penitence2Image;
            }
        }
    }
}
