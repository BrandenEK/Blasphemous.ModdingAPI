using System.Collections.Generic;
using ModdingAPI;
using UnityEngine;
using UnityEngine.UI;
using Gameplay.UI.Others.MenuLogic;
using Framework.Managers;
using Rewired;

namespace ModdingAPI
{
    internal class PenitenceLoader
    {
        public void ActivatePenitence(string id)
        {
            foreach (ModPenitence penitence in Main.moddingAPI.GetModPenitences())
            {
                if (penitence.Id == id)
                    penitence.Activate();
            }
        }

        public void DeactivatePenitence(string id)
        {
            foreach (ModPenitence penitence in Main.moddingAPI.GetModPenitences())
            {
                if (penitence.Id == id)
                    penitence.Deactivate();
            }
        }

        public List<SelectSaveSlots.PenitenceData> GetPenitenceData(bool mainMenu)
        {
            List<SelectSaveSlots.PenitenceData> dataList = new List<SelectSaveSlots.PenitenceData>();
            foreach (ModPenitence penitence in Main.moddingAPI.GetModPenitences())
            {
                SelectSaveSlots.PenitenceData data = new SelectSaveSlots.PenitenceData()
                {
                    id = penitence.Id,
                    InProgress = mainMenu ? penitence.InProgressImage : penitence.GameplayImage,
                    Completed = penitence.CompletedImage,
                    Missing = penitence.AbandonedImage
                };
                dataList.Add(data);
            }
            return dataList;
        }
    }
}
