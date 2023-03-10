using UnityEngine;
using System.Collections.Generic;

namespace ModdingAPI
{
    public class ModRosaryBead
    {
        protected internal string Id { get; }

        protected internal string Name { get; }

        protected internal string Description { get; }

        protected internal string Lore { get; }

        protected internal bool CarryOnStart { get; }

        protected internal bool PreserveInNGPlus { get; }

        protected internal bool AddToPercentageCompletion { get; }

        internal Sprite Picture { get; private set; }

        internal List<ModItemEffect> Effects { get; private set; }

        public ModRosaryBead SetImage(Sprite image)
        {
            Picture = image;
            return this;
        }


        public ModRosaryBead AddEffect<T>() where T : ModItemEffect, new()
        {
            Effects.Add(new T());
            return this;
        }
    }
}
