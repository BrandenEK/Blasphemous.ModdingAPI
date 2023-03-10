using System;
using System.Collections.Generic;
using HarmonyLib;
using Framework.Managers;
using Framework.Penitences;
using Framework.Inventory;
using Gameplay.UI;
using Gameplay.UI.Others.MenuLogic;
using Gameplay.UI.Widgets;
using UnityEngine;
using UnityEngine.UI;
using Tools.Playmaker2.Action;
using I2.Loc;

namespace ModdingAPI
{
    // Add custom penitences to list
    [HarmonyPatch(typeof(PenitenceManager), "ResetPenitencesList")]
    internal class PenitenceManager_Patch
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
    internal class PenitenceSlot_Patch
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
    internal class GameplayWidget_Patch
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
    internal class ChoosePenitenceWidgetSelect1_Patch
    {
        public static void Postfix() { Main.moddingAPI.penitenceLoader.CurrentSelection = PenitenceLoader.Selection.Normal; }
    }
    [HarmonyPatch(typeof(ChoosePenitenceWidget), "Option_SelectPE02")]
    internal class ChoosePenitenceWidgetSelect2_Patch
    {
        public static void Postfix() { Main.moddingAPI.penitenceLoader.CurrentSelection = PenitenceLoader.Selection.Normal; }
    }
    [HarmonyPatch(typeof(ChoosePenitenceWidget), "Option_SelectPE03")]
    internal class ChoosePenitenceWidgetSelect3_Patch
    {
        public static void Postfix() { Main.moddingAPI.penitenceLoader.CurrentSelection = PenitenceLoader.Selection.Normal; }
    }
    [HarmonyPatch(typeof(ChoosePenitenceWidget), "OnClose")]
    internal class ChoosePenitenceWidgetClose_Patch
    {
        public static void Postfix() { Main.moddingAPI.penitenceLoader.CurrentSelection = PenitenceLoader.Selection.Normal; }
    }

    // Display buttons and store action when opening widget
    [HarmonyPatch(typeof(ChoosePenitenceWidget), "Open", typeof(Action), typeof(Action))]
    internal class ChoosePenitenceWidgetOpen_Patch
    {
        public static void Postfix(Action onChoosingPenitence)
        {
            Main.moddingAPI.penitenceLoader.chooseAction = onChoosingPenitence;
            Main.moddingAPI.penitenceLoader.NoPenitenceUnselectedImage = Main.moddingAPI.penitenceLoader.UnselectedButtonImage.sprite;
            Main.moddingAPI.penitenceLoader.NoPenitenceSelectedImage = Main.moddingAPI.penitenceLoader.SelectedButtonImage.sprite;
            Main.moddingAPI.penitenceLoader.CurrentSelectedCustomPenitence = 0;

            // Add lb/rb buttons
            Transform buttonHolder = UnityEngine.Object.FindObjectOfType<NewInventoryWidget>().transform.Find("External/Background/Headers/Inventory/Caption/Selector_Help/");

            Transform parent = Main.moddingAPI.penitenceLoader.UnselectedButtonImage.transform;
            if (parent.childCount == 1)
            {
                GameObject left = UnityEngine.Object.Instantiate(buttonHolder.GetChild(0).gameObject, parent);
                GameObject right = UnityEngine.Object.Instantiate(buttonHolder.GetChild(1).gameObject, parent);
                left.GetComponent<RectTransform>().anchoredPosition = new Vector2(-5, 10);
                right.GetComponent<RectTransform>().anchoredPosition = new Vector2(5, 10);
            }
        }
    }

    // Update selection status when selecting normally or changing custom penitences
    [HarmonyPatch(typeof(ChoosePenitenceWidget), "Option_SelectNoPenitence")]
    internal class ChoosePenitenceWidgetSelectNone_Patch
    {
        public static bool Prefix(Text ___penitenceTitle, Text ___penitenceInfoText, CustomScrollView ___penitenceScroll)
        {

            int currPenitenceIdx = Main.moddingAPI.penitenceLoader.CurrentSelectedCustomPenitence;
            if (currPenitenceIdx > 0)
            {
                Main.moddingAPI.penitenceLoader.CurrentSelection = PenitenceLoader.Selection.Custom;
                ModPenitence currentPenitence = Main.moddingAPI.GetModPenitences()[currPenitenceIdx - 1];
                ___penitenceTitle.text = currentPenitence.Name;
                ___penitenceInfoText.text = currentPenitence.Description;
                Main.moddingAPI.penitenceLoader.SelectedButtonImage.sprite = currentPenitence.ChooseSelectedImage;
                Main.moddingAPI.penitenceLoader.UnselectedButtonImage.sprite = currentPenitence.ChooseUnselectedImage;
            }
            else
            {
                ___penitenceTitle.text = ScriptLocalization.UI_Penitences.NO_PENITENCE;
                Main.moddingAPI.penitenceLoader.CurrentSelection = PenitenceLoader.Selection.Bottom;
                ___penitenceInfoText.text = ScriptLocalization.UI_Penitences.NO_PENITENCE_INFO;
                Main.moddingAPI.penitenceLoader.SelectedButtonImage.sprite = Main.moddingAPI.penitenceLoader.NoPenitenceSelectedImage;
                Main.moddingAPI.penitenceLoader.UnselectedButtonImage.sprite = Main.moddingAPI.penitenceLoader.NoPenitenceUnselectedImage;
            }
            ___penitenceScroll.NewContentSetted();
            return false;
        }
    }

    // Display choose penitence confirmation when choosing custom one
    [HarmonyPatch(typeof(UIController), "ShowConfirmationWidget", typeof(string), typeof(Action), typeof(Action))]
    internal class UIController_Patch
    {
        public static void Prefix(ref string infoMessage)
        {
            if (Main.moddingAPI.penitenceLoader.CurrentSelection == PenitenceLoader.Selection.Custom)
                infoMessage = ScriptLocalization.UI_Penitences.CHOOSE_PENITENCE_CONFIRMATION;
        }
    }

    // When confirming a custom penitence, activate it
    [HarmonyPatch(typeof(ChoosePenitenceWidget), "ContinueWithNoPenitenceAndClose")]
    internal class ChoosePenitenceWidgetActivate_Patch
    {
        public static bool Prefix(ChoosePenitenceWidget __instance)
        {
            if (Main.moddingAPI.penitenceLoader.CurrentSelection == PenitenceLoader.Selection.Custom)
            {
                Main.moddingAPI.penitenceLoader.ConfirmCustomPenitence();
                __instance.Close();
                return false;
            }
            return true;
        }
    }

    // Show custom penitence when abandoning
    [HarmonyPatch(typeof(AbandonPenitenceWidget), "UpdatePenitenceTextsAndDisplayedMedal")]
    internal class AbandonPenitenceWidgetOpen_Patch
    {
        public static bool Prefix(Text ___penitenceTitle, Text ___penitenceInfoText, GameObject ___PE01Medal, GameObject ___PE02Medal, GameObject ___PE03Medal)
        {
            if (Core.PenitenceManager.GetCurrentPenitence() is ModPenitenceSystem modPenitence)
            {
                ModPenitence currPenitence = Main.moddingAPI.penitenceLoader.GetPenitence(modPenitence.Id);
                if (currPenitence == null) return false;

                Image medalImage = ___PE02Medal.GetComponentInChildren<Image>();
                Main.moddingAPI.penitenceLoader.Penitence2Image = medalImage.sprite;
                medalImage.sprite = currPenitence.ChooseSelectedImage;
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
    internal class AbandonPenitenceWidgetClose_Patch
    {
        public static void Postfix(GameObject ___PE02Medal)
        {
            ___PE02Medal.GetComponentInChildren<Image>().sprite = Main.moddingAPI.penitenceLoader.Penitence2Image;
        }
    }

    // Complete penitence and give item on completion
    [HarmonyPatch(typeof(PenitenceCheckCurrent), "OnEnter")]
    internal class CurrentPenitence_Patch
    {
        public static bool Prefix(PenitenceCheckCurrent __instance)
        {
            if (Core.PenitenceManager.GetCurrentPenitence() is ModPenitenceSystem modPenitence)
            {
                // I am assuming that this method is only used when the game is over to complete the penitence
                Main.LogMessage(Main.MOD_NAME, "Completing custom penitence: " + modPenitence.Id);
                Core.PenitenceManager.MarkCurrentPenitenceAsCompleted();
                ModPenitence currPenitence = Main.moddingAPI.penitenceLoader.GetPenitence(modPenitence.Id);

                // If there is not item to give, save and finish
                if (currPenitence.ItemIdToGive == null || currPenitence.ItemIdToGive == string.Empty)
                {
                    FinishAction();
                    return false;
                }

                // If the item is not valid or already owned, save and finish
                BaseInventoryObject obj = Core.InventoryManager.GetBaseObject(currPenitence.ItemIdToGive, currPenitence.ItemTypeToGive);
                if (obj == null || isItemOwned(currPenitence.ItemIdToGive, currPenitence.ItemTypeToGive))
                {
                    FinishAction();
                    return false;
                }

                // Give the item, and once finished, save and finish
                PopUpWidget.OnDialogClose += FinishAction;
                obj = Core.InventoryManager.AddBaseObjectOrTears(obj);
                UIController.instance.ShowObjectPopUp(UIController.PopupItemAction.GetObejct, obj.caption, obj.picture, obj.GetItemType(), 3f, true);
                return false;
            }
            return true;

            void FinishAction()
            {
                PopUpWidget.OnDialogClose -= FinishAction;
                Core.Persistence.SaveGame(true);
                __instance.Fsm.Event(__instance.noPenitenceActive);
                __instance.Finish();
            }

            bool isItemOwned(string itemId, InventoryManager.ItemType itemType)
            {
                switch (itemType)
                {
                    case InventoryManager.ItemType.Bead: return Core.InventoryManager.IsRosaryBeadOwned(itemId);
                    case InventoryManager.ItemType.Prayer: return Core.InventoryManager.IsPrayerOwned(itemId);
                    case InventoryManager.ItemType.Relic: return Core.InventoryManager.IsRelicOwned(itemId);
                    case InventoryManager.ItemType.Sword: return Core.InventoryManager.IsSwordOwned(itemId);
                    case InventoryManager.ItemType.Quest: return Core.InventoryManager.IsQuestItemOwned(itemId);
                    case InventoryManager.ItemType.Collectible: return Core.InventoryManager.IsCollectibleItemOwned(itemId);
                }
                return false;
            }
        }
    }
}
