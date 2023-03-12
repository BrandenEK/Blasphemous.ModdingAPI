using UnityEngine;
using Framework.Managers;

namespace ModdingAPI
{
    /// <summary>
    /// An abstract representation of a penitence
    /// </summary>
    public abstract class ModPenitence
    {
        /// <summary>
        /// The unique id of this penitence (PEXX...)
        /// </summary>
        protected internal abstract string Id { get; }

        /// <summary>
        /// The descriptive name of this penitence
        /// </summary>
        protected internal abstract string Name { get; }

        /// <summary>
        /// The full description of this penitence
        /// </summary>
        protected internal abstract string Description { get; }

        /// <summary>
        /// The ID of the item to give for completing the penitence
        /// </summary>
        protected internal abstract string ItemIdToGive { get; }

        /// <summary>
        /// The type of the item to give for completing the penitence
        /// </summary>
        protected internal abstract InventoryManager.ItemType ItemTypeToGive { get; }

        /// <summary>
        /// Should perform any necessary actions to activate this penitence's functionality
        /// </summary>
        protected internal abstract void Activate();

        /// <summary>
        /// Should perform any necessary actions to deactivate this penitence's functionality
        /// </summary>
        protected internal abstract void Deactivate();

        internal Sprite InProgressImage { get; private set; }
        internal Sprite CompletedImage { get; private set; }
        internal Sprite AbandonedImage { get; private set; }
        internal Sprite GameplayImage { get; private set; }
        internal Sprite ChooseSelectedImage { get; private set; }
        internal Sprite ChooseUnselectedImage { get; private set; }

        protected abstract void LoadImages(out Sprite inProgress, out Sprite completed, out Sprite abandoned, out Sprite gameplay, out Sprite chooseSelected, out Sprite chooseUnselected);

        public ModPenitence()
        {
            LoadImages(out Sprite inProgress, out Sprite completed, out Sprite abandoned, out Sprite gameplay, out Sprite chooseSelected, out Sprite chooseUnselected);
            InProgressImage = inProgress;
            CompletedImage = completed;
            AbandonedImage = abandoned;
            GameplayImage = gameplay;
            ChooseSelectedImage = chooseSelected;
            ChooseUnselectedImage = chooseUnselected;
        }
    }
}
