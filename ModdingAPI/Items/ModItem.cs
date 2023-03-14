using UnityEngine;
using Framework.Inventory;
using System.Collections.Generic;

namespace ModdingAPI.Items
{
    /// <summary>
    /// An abstract representation of a custom item
    /// </summary>
    public abstract class ModItem
    {
        /// <summary>
        /// The unique id of the item (Must start with certain prefix)
        /// </summary>
        protected internal abstract string Id { get; }

        /// <summary>
        /// The name of the item
        /// </summary>
        protected internal abstract string Name { get; }
        
        /// <summary>
        /// The description of the item
        /// </summary>
        protected internal abstract string Description { get; }

        /// <summary>
        /// The lore for the item
        /// </summary>
        protected internal abstract string Lore { get; }

        /// <summary>
        /// Whether or not the item should be given upon starting a new game
        /// </summary>
        protected internal abstract bool CarryOnStart { get; }

        /// <summary>
        /// Whether or not the item should be carried over into NG+
        /// </summary>
        protected internal abstract bool PreserveInNGPlus { get; }

        /// <summary>
        /// Whether or not the item will add to the percent completion of the save file
        /// </summary>
        protected internal abstract bool AddToPercentCompletion { get; }

        internal Sprite Picture { get; private set; }

        internal List<ModItemEffect> Effects { get; private set; }

        /// <summary>
        /// Stores the associated images for the item - only executed on startup
        /// </summary>
        /// <param name="picture">The icon for the custom item</param>
        protected abstract void LoadImages(out Sprite picture);

        /// <summary>
        /// Adds an item effect to the custom item
        /// </summary>
        /// <typeparam name="T">The type of item effect to add</typeparam>
        /// <returns>The custom item</returns>
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
