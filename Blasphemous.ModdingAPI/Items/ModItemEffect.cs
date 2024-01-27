using Framework.Inventory;

namespace Blasphemous.ModdingAPI.Items;

/// <summary>
/// An abstract represention of an item effect
/// </summary>
public abstract class ModItemEffect
{
    /// <summary>
    /// Called when the item is created
    /// </summary>
    protected internal virtual void Initialize() { }

    /// <summary>
    /// Called every frame
    /// </summary>
    protected internal virtual void Update() { }

    /// <summary>
    /// Called when the item is destroyed
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

    internal abstract ModItem.ModItemType ValidItemTypes { get; }
}
