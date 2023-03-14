using UnityEngine;
using Framework.Inventory;
using System.Collections.Generic;

namespace ModdingAPI.Items
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

        protected abstract void LoadImages(out Sprite picture);

        public ModItem AddEffect<T>() where T : ModItemEffect, new()
        {
            Effects.Add(new T());
            return this;
        }

        internal ModItem()
        {
            Effects = new List<ModItemEffect>();
            LoadImages(out Sprite picture);
            Picture = picture;
        }

        private protected T CreateBaseObject<T>(GameObject itemHolder) where T : BaseInventoryObject
        {
            // Create object
            GameObject obj = new GameObject(Id);
            obj.transform.SetParent(itemHolder.transform.Find(typeof(T).Name));

            // Set properties
            T item = obj.AddComponent<T>();
            item.id = Id;
            item.caption = Name;
            item.description = Description;
            item.lore = Lore;
            item.picture = Picture;
            item.carryonstart = CarryOnStart;
            item.preserveInNewGamePlus = PreserveInNGPlus;

            // Add effects
            foreach (ModItemEffect effect in Effects)
            {
                item.gameObject.AddComponent<ModItemEffectSystem>().SetEffect(effect);
            }

            return item;
        }
    }
}
