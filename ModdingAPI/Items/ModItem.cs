using System.Collections.Generic;
using UnityEngine;

namespace ModdingAPI
{
    public abstract class ModItem
    {
        protected internal abstract string Id { get; }

        protected internal abstract string Name { get; }

        protected internal abstract string Description { get; }

        protected internal abstract string Lore { get; }

        protected internal abstract bool CarryOnStart { get; }

        protected internal abstract bool PreserveInNGPlus { get; }

        protected internal abstract bool AddToPercentageCompletion { get; }

        internal Sprite Picture { get; private set; }

        internal List<ModItemEffect> Effects { get; private set; }

        protected abstract Sprite GetImage();

        protected void AddEffect<T>() where T : ModItemEffect, new()
        {
            Effects.Add(new T());
        }

        public ModItem()
        {
            Effects = new List<ModItemEffect>();
            Picture = GetImage();
        }
    }
}
