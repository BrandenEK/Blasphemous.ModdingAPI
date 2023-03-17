using Framework.Inventory;

namespace ModdingAPI.Items
{
    /// <summary>
    /// An abstract represention of an item effect
    /// </summary>
    public abstract class ModItemEffect
    {
        /// <summary>
        /// Called before startup
        /// </summary>
        protected internal virtual void Awake() { }
        
        /// <summary>
        /// Called after startup
        /// </summary>
        protected internal virtual void Start() { }

        /// <summary>
        /// Called every frame
        /// </summary>
        protected internal virtual void Update() { }

        /// <summary>
        /// Called when the item is disposed
        /// </summary>
        protected internal virtual void Dispose() { }

        /// <summary>
        /// Called when this effect should be applied
        /// </summary>
        protected internal abstract void ApplyEffect();

        /// <summary>
        /// Called when this effect should be removed
        /// </summary>
        protected internal abstract void RemoveEffect();

        /// <summary>
        /// The inventory object that this effect applies to
        /// </summary>
        protected internal BaseInventoryObject InventoryObject { get; internal set; }

        internal abstract void SetSystemProperties(ModItemEffectSystem system);

        private protected ModItemEffect() { }

        internal abstract ModItem.ModItemType ValidItemTypes { get; }
    }
}
