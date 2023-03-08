using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public abstract string Id { get; }

        /// <summary>
        /// The descriptive name of this penitence
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// The full description of this penitence
        /// </summary>
        public abstract string Description { get; }

        /// <summary>
        /// Should perform any necessary actions to activate this penitence's functionality
        /// </summary>
        public abstract void Activate();

        /// <summary>
        /// Should perform any necessary actions to deactivate this penitence's functionality
        /// </summary>
        public abstract void Deactivate();

        /// <summary>
        /// Called when the penitence is completed
        /// </summary>
        public abstract void OnCompletion();

        /// <summary>
        /// Creates a new custom penitence and stores all of its images
        /// </summary>
        /// <param name="inProgress">The menu icon for an activated penitence</param>
        /// <param name="completed">The menu icon for a completed penitence</param>
        /// <param name="abandoned">The menu icon for an abandoned penitence</param>
        /// <param name="gameplay">The gameplay icon for an activated penitence</param>
        /// <param name="selection">The large penitence selection icon</param>
        public ModPenitence(Sprite inProgress, Sprite completed, Sprite abandoned, Sprite gameplay, Sprite selection)
        {
            InProgressImage = inProgress;
            CompletedImage = completed;
            AbandonedImage = abandoned;
            GameplayImage = gameplay;
            SelectionImage = selection;
        }

        internal Sprite InProgressImage { get; private set; }
        internal Sprite CompletedImage { get; private set; }
        internal Sprite AbandonedImage { get; private set; }
        internal Sprite GameplayImage { get; private set; }
        internal Sprite SelectionImage { get; private set; }
    }
}
