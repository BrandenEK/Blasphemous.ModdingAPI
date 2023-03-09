using UnityEngine;

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
        /// Should perform any necessary actions to activate this penitence's functionality
        /// </summary>
        protected internal abstract void Activate();

        /// <summary>
        /// Should perform any necessary actions to deactivate this penitence's functionality
        /// </summary>
        protected internal abstract void Deactivate();

        /// <summary>
        /// Called when this penitence is completed
        /// </summary>
        protected internal abstract void OnCompletion();

        /// <summary>
        /// Creates a new custom penitence and stores all of its images
        /// </summary>
        /// <param name="inProgress">The menu icon for an activated penitence</param>
        /// <param name="completed">The menu icon for a completed penitence</param>
        /// <param name="abandoned">The menu icon for an abandoned penitence</param>
        /// <param name="gameplay">The gameplay icon for an activated penitence</param>
        /// <param name="chooseSelected">The selected icon when choosing a penitence</param>
        /// <param name="chooseUnselected">The unselected icon when chooseing a penitence</param>
        public ModPenitence(Sprite inProgress, Sprite completed, Sprite abandoned, Sprite gameplay, Sprite chooseSelected, Sprite chooseUnselected)
        {
            InProgressImage = inProgress;
            CompletedImage = completed;
            AbandonedImage = abandoned;
            GameplayImage = gameplay;
            ChooseSelectedImage = chooseSelected;
            ChooseUnselectedImage = chooseUnselected;
        }

        internal Sprite InProgressImage { get; private set; }
        internal Sprite CompletedImage { get; private set; }
        internal Sprite AbandonedImage { get; private set; }
        internal Sprite GameplayImage { get; private set; }
        internal Sprite ChooseSelectedImage { get; private set; }
        internal Sprite ChooseUnselectedImage { get; private set; }
    }
}
