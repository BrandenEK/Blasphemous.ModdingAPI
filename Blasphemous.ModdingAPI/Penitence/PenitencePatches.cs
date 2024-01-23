using Framework.Inventory;
using Framework.Managers;
using Framework.Penitences;
using Gameplay.UI.Others.MenuLogic;
using Gameplay.UI.Widgets;
using Gameplay.UI;
using HarmonyLib;
using I2.Loc;
using System;
using System.Collections.Generic;
using Tools.Playmaker2.Action;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace Blasphemous.ModdingAPI.Penitence;

// Add custom penitences to list
[HarmonyPatch(typeof(PenitenceManager), "ResetPenitencesList")]
internal class PenitenceManager_Patch
{
    public static void Postfix(List<IPenitence> ___allPenitences)
    {
        ___allPenitences.AddRange(PenitenceModder.All.Select(p => new ModPenitenceSystem(p.Id) as IPenitence));
        //foreach (ModPenitence penitence in PenitenceModder.All)
        //{
        //    ___allPenitences.Add(new ModPenitenceSystem(penitence.Id));
        //}
    }
}

// Add config settings for custom penitences
[HarmonyPatch(typeof(PenitenceSlot), "SetPenitenceConfig")]
internal class PenitenceSlot_Patch
{
    public static void Postfix(Dictionary<string, SelectSaveSlots.PenitenceData> ___allPenitences)
    {
        foreach (SelectSaveSlots.PenitenceData data in Main.ModdingAPI.PenitenceHandler.GetPenitenceData(true))
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
        ___PenitencesConfig.AddRange(Main.ModdingAPI.PenitenceHandler.GetPenitenceData(false));
        //foreach (SelectSaveSlots.PenitenceData data in Main.ModdingAPI.PenitenceHandler.GetPenitenceData(false))
        //{
        //    ___PenitencesConfig.Add(data);
        //}
    }
}

// Set selecting status when changing options
[HarmonyPatch(typeof(ChoosePenitenceWidget), "Option_SelectPE01")]
internal class ChoosePenitenceWidgetSelect1_Patch
{
    public static void Postfix() => Main.ModdingAPI.PenitenceHandler.CurrentSelection = PenitenceHandler.Selection.Normal;
}
[HarmonyPatch(typeof(ChoosePenitenceWidget), "Option_SelectPE02")]
internal class ChoosePenitenceWidgetSelect2_Patch
{
    public static void Postfix() => Main.ModdingAPI.PenitenceHandler.CurrentSelection = PenitenceHandler.Selection.Normal;
}
[HarmonyPatch(typeof(ChoosePenitenceWidget), "Option_SelectPE03")]
internal class ChoosePenitenceWidgetSelect3_Patch
{
    public static void Postfix() => Main.ModdingAPI.PenitenceHandler.CurrentSelection = PenitenceHandler.Selection.Normal;
}
[HarmonyPatch(typeof(ChoosePenitenceWidget), "OnClose")]
internal class ChoosePenitenceWidgetClose_Patch
{
    public static void Postfix()
    {
        Main.ModdingAPI.PenitenceHandler.CurrentSelection = PenitenceHandler.Selection.Normal;
        Main.ModdingAPI.PenitenceHandler.SelectedButtonImage.sprite = Main.ModdingAPI.PenitenceHandler.NoPenitenceSelectedImage;
        Main.ModdingAPI.PenitenceHandler.UnselectedButtonImage.sprite = Main.ModdingAPI.PenitenceHandler.NoPenitenceUnselectedImage;
        Main.ModdingAPI.PenitenceHandler.CurrentSelectedCustomPenitence = 0;
    }
}

// Display buttons and store action when opening widget
[HarmonyPatch(typeof(ChoosePenitenceWidget), "Open", typeof(Action), typeof(Action))]
internal class ChoosePenitenceWidgetOpen_Patch
{
    public static void Postfix(Action onChoosingPenitence)
    {
        Main.ModdingAPI.PenitenceHandler.ChooseAction = onChoosingPenitence;
        Main.ModdingAPI.PenitenceHandler.NoPenitenceUnselectedImage = Main.ModdingAPI.PenitenceHandler.UnselectedButtonImage.sprite;
        Main.ModdingAPI.PenitenceHandler.NoPenitenceSelectedImage = Main.ModdingAPI.PenitenceHandler.SelectedButtonImage.sprite;
        Main.ModdingAPI.PenitenceHandler.CurrentSelectedCustomPenitence = 0;

        // Add lb/rb buttons
        Transform buttonHolder = UnityEngine.Object.FindObjectOfType<NewInventoryWidget>().transform.Find("External/Background/Headers/Inventory/Caption/Selector_Help/");

        Transform parent = Main.ModdingAPI.PenitenceHandler.UnselectedButtonImage.transform;
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

        int currPenitenceIdx = Main.ModdingAPI.PenitenceHandler.CurrentSelectedCustomPenitence;
        if (currPenitenceIdx > 0)
        {
            Main.ModdingAPI.PenitenceHandler.CurrentSelection = PenitenceHandler.Selection.Custom;
            ModPenitence currentPenitence = PenitenceModder.AtIndex(currPenitenceIdx - 1);
            ___penitenceTitle.text = currentPenitence.Name;
            ___penitenceInfoText.text = currentPenitence.Description;
            Main.ModdingAPI.PenitenceHandler.SelectedButtonImage.sprite = currentPenitence.ChooseSelectedImage;
            Main.ModdingAPI.PenitenceHandler.UnselectedButtonImage.sprite = currentPenitence.ChooseUnselectedImage;
        }
        else
        {
            ___penitenceTitle.text = ScriptLocalization.UI_Penitences.NO_PENITENCE;
            Main.ModdingAPI.PenitenceHandler.CurrentSelection = PenitenceHandler.Selection.Bottom;
            ___penitenceInfoText.text = ScriptLocalization.UI_Penitences.NO_PENITENCE_INFO;
            Main.ModdingAPI.PenitenceHandler.SelectedButtonImage.sprite = Main.ModdingAPI.PenitenceHandler.NoPenitenceSelectedImage;
            Main.ModdingAPI.PenitenceHandler.UnselectedButtonImage.sprite = Main.ModdingAPI.PenitenceHandler.NoPenitenceUnselectedImage;
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
        if (Main.ModdingAPI.PenitenceHandler.CurrentSelection == PenitenceHandler.Selection.Custom)
            infoMessage = ScriptLocalization.UI_Penitences.CHOOSE_PENITENCE_CONFIRMATION;
    }
}

// When confirming a custom penitence, activate it
[HarmonyPatch(typeof(ChoosePenitenceWidget), "ContinueWithNoPenitenceAndClose")]
internal class ChoosePenitenceWidgetActivate_Patch
{
    public static bool Prefix(ChoosePenitenceWidget __instance)
    {
        if (Main.ModdingAPI.PenitenceHandler.CurrentSelection == PenitenceHandler.Selection.Custom)
        {
            Main.ModdingAPI.PenitenceHandler.ConfirmCustomPenitence();
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
        if (Core.PenitenceManager.GetCurrentPenitence() is not ModPenitenceSystem modPenitence)
            return true;

        ModPenitence currPenitence = Main.ModdingAPI.PenitenceHandler.GetPenitence(modPenitence.Id);
        if (currPenitence == null)
            return false;

        Image medalImage = ___PE02Medal.GetComponentInChildren<Image>();
        Main.ModdingAPI.PenitenceHandler.Penitence2Image = medalImage.sprite;
        medalImage.sprite = currPenitence.ChooseSelectedImage;
        ___penitenceTitle.text = currPenitence.Name;
        ___penitenceInfoText.text = currPenitence.Description;

        ___PE01Medal.SetActive(false);
        ___PE02Medal.SetActive(true);
        ___PE03Medal.SetActive(false);
        return false;
    }
}
[HarmonyPatch(typeof(AbandonPenitenceWidget), "OnClose")]
internal class AbandonPenitenceWidgetClose_Patch
{
    public static void Postfix(GameObject ___PE02Medal)
    {
        ___PE02Medal.GetComponentInChildren<Image>().sprite = Main.ModdingAPI.PenitenceHandler.Penitence2Image;
    }
}

// Complete penitence and give item on completion
[HarmonyPatch(typeof(PenitenceCheckCurrent), "OnEnter")]
internal class CurrentPenitence_Patch
{
    public static bool Prefix(PenitenceCheckCurrent __instance)
    {
        if (Core.PenitenceManager.GetCurrentPenitence() is not ModPenitenceSystem modPenitence)
            return true;

        // I am assuming that this method is only used when the game is over to complete the penitence
        Main.ModdingAPI.Log("Completing custom penitence: " + modPenitence.Id);
        Core.PenitenceManager.MarkCurrentPenitenceAsCompleted();
        ModPenitence currPenitence = Main.ModdingAPI.PenitenceHandler.GetPenitence(modPenitence.Id);

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


        void FinishAction()
        {
            PopUpWidget.OnDialogClose -= FinishAction;
            Core.Persistence.SaveGame(true);
            __instance.Fsm.Event(__instance.noPenitenceActive);
            __instance.Finish();
        }

        bool isItemOwned(string itemId, InventoryManager.ItemType itemType)
        {
            return itemType switch
            {
                InventoryManager.ItemType.Bead => Core.InventoryManager.IsRosaryBeadOwned(itemId),
                InventoryManager.ItemType.Prayer => Core.InventoryManager.IsPrayerOwned(itemId),
                InventoryManager.ItemType.Relic => Core.InventoryManager.IsRelicOwned(itemId),
                InventoryManager.ItemType.Sword => Core.InventoryManager.IsSwordOwned(itemId),
                InventoryManager.ItemType.Quest => Core.InventoryManager.IsQuestItemOwned(itemId),
                InventoryManager.ItemType.Collectible => Core.InventoryManager.IsCollectibleItemOwned(itemId),
                _ => false,
            };
        }
    }
}
