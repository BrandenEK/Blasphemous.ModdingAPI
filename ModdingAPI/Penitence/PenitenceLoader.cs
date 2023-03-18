using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Gameplay.UI.Others.MenuLogic;
using Framework.Managers;
using Rewired;

namespace ModdingAPI.Penitences
{
    internal class PenitenceLoader
    {
        public enum Selection { Normal, Bottom, Custom }
        public Selection CurrentSelection { get; set; }

        public System.Action chooseAction { get; set; }
        private Player rewired;

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

        public ModPenitence GetPenitence(string id)
        {
            foreach (ModPenitence penitence in Main.moddingAPI.GetModPenitences())
            {
                if (penitence.Id == id)
                    return penitence;
            }
            return null;
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

        public void ConfirmCustomPenitence()
        {
            ModPenitence newPenitence = Main.moddingAPI.GetModPenitences()[CurrentSelectedCustomPenitence - 1];
            Main.LogMessage(Main.MOD_NAME, "Activating custom penitence: " + newPenitence.Id);
            Core.PenitenceManager.ActivatePenitence(newPenitence.Id);
            chooseAction();
        }

        public void Update()
        {
            if (CurrentSelection != Selection.Normal)
            {
                if (rewired == null)
                {
                    rewired = ReInput.players.GetPlayer(0);
                }
                if (rewired.GetButtonDown(28))
                {
                    CurrentSelectedCustomPenitence--;
                    Object.FindObjectOfType<ChoosePenitenceWidget>().Option_SelectNoPenitence();
                }
                else if (rewired.GetButtonDown(29))
                {
                    CurrentSelectedCustomPenitence++;
                    Object.FindObjectOfType<ChoosePenitenceWidget>().Option_SelectNoPenitence();
                }
            }
        }

        private int m_CurrentSelectedCustomPenitence = 0;
        public int CurrentSelectedCustomPenitence
        {
            get { return m_CurrentSelectedCustomPenitence; }
            set
            {
                if (value > Main.moddingAPI.GetModPenitences().Count)
                    m_CurrentSelectedCustomPenitence = 0;
                else if (value < 0)
                    m_CurrentSelectedCustomPenitence = Main.moddingAPI.GetModPenitences().Count;
                else
                    m_CurrentSelectedCustomPenitence = value;
            }
        }

        private Image m_UnselectedButtonImage;
        public Image UnselectedButtonImage
        {
            get
            {
                if (m_UnselectedButtonImage == null)
                {
                    m_UnselectedButtonImage = Object.FindObjectOfType<ChoosePenitenceWidget>().transform.Find("Options/NoPenitence/Image").GetComponent<Image>();
                }
                return m_UnselectedButtonImage;
            }
        }

        private Image m_SelectedButtonImage;
        public Image SelectedButtonImage
        {
            get
            {
                if (m_SelectedButtonImage == null)
                {
                    m_SelectedButtonImage = UnselectedButtonImage.transform.Find("Selected/SelectedIconWithBorder").GetComponent<Image>();
                }
                return m_SelectedButtonImage;
            }
        }

        private Sprite m_NoPenitenceSelectedImage;
        public Sprite NoPenitenceSelectedImage
        {
            get { return m_NoPenitenceSelectedImage; }
            set
            {
                if (m_NoPenitenceSelectedImage == null)
                    m_NoPenitenceSelectedImage = value;
            }
        }

        private Sprite m_NoPenitenceUnselectedImage;
        public Sprite NoPenitenceUnselectedImage
        {
            get { return m_NoPenitenceUnselectedImage; }
            set
            {
                if (m_NoPenitenceUnselectedImage == null)
                    m_NoPenitenceUnselectedImage = value;
            }
        }

        private Sprite m_Penitence2Image;
        public Sprite Penitence2Image
        {
            get { return m_Penitence2Image; }
            set
            {
                if (m_Penitence2Image == null)
                    m_Penitence2Image = value;
            }
        }
    }
}
