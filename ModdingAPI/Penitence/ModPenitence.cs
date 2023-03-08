using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ModdingAPI
{
    public abstract class ModPenitence
    {
        public abstract string Id { get; }

        public abstract string Name { get; }
        public abstract string Description { get; }

        public abstract void Activate();

        public abstract void Deactivate();

        public ModPenitence(Sprite inProgress, Sprite completed, Sprite abandoned, Sprite gameplay, Sprite selection)
        {
            InProgressImage = inProgress;
            CompletedImage = completed;
            AbandonedImage = abandoned;
            GameplayImage = gameplay;
            SelectionImage = selection;
        }

        public Sprite InProgressImage { get; private set; }
        public Sprite CompletedImage { get; private set; }
        public Sprite AbandonedImage { get; private set; }
        public Sprite GameplayImage { get; private set; }
        public Sprite SelectionImage { get; private set; }
    }
}
