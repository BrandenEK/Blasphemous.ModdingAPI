using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Gameplay.UI.Others.MenuLogic;
using Framework.Managers;
using Rewired;

namespace ModdingAPI
{
    internal class PenitenceLoader
    {
        public bool customStatus { get; private set; }
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

        public void CompleteCurrentPenitence()
        {
            if (Core.PenitenceManager.GetCurrentPenitence() is ModPenitenceSystem modPenitence)
            {
                foreach (ModPenitence penitence in Main.moddingAPI.GetModPenitences())
                {
                    if (penitence.Id == modPenitence.Id)
                        penitence.OnCompletion();
                }
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
            if (SelectingCustomPenitence)
            {
                if (rewired == null)
                {
                    rewired = ReInput.players.GetPlayer(0);
                }
                if (rewired.GetButtonDown(23))
                {
                    CurrentSelectedCustomPenitence--;
                    selectCustomPenitence();
                }
                else if (rewired.GetButtonDown(38))
                {
                    CurrentSelectedCustomPenitence++;
                    selectCustomPenitence();
                }
            }

            void selectCustomPenitence()
            {
                customStatus = true;
                Object.FindObjectOfType<ChoosePenitenceWidget>().Option_SelectNoPenitence();
                customStatus = false;
            }
        }

        private bool m_SelectingCustomPenitence;
        public bool SelectingCustomPenitence
        {
            get { return m_SelectingCustomPenitence; }
            set
            {
                m_SelectingCustomPenitence = value;
                if (value)
                {
                    NoPenitenceImage = SelectedButtonImage.sprite;
                }
                else
                {
                    if (NoPenitenceImage != null)
                        SelectedButtonImage.sprite = NoPenitenceImage;
                    CurrentSelectedCustomPenitence = 0;
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

        private Sprite m_NoPenitenceImage;
        public Sprite NoPenitenceImage
        {
            get { return m_NoPenitenceImage; }
            set
            {
                if (m_NoPenitenceImage == null)
                    m_NoPenitenceImage = value;
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
