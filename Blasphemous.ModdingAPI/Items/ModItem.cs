using Framework.Inventory;
using System.Collections.Generic;
using UnityEngine;

namespace Blasphemous.ModdingAPI.Items;

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
    /// The icon for the item
    /// </summary>
    protected internal abstract Sprite Picture { get; }

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

    /// <summary>
    /// Whether or not to add one additional item slot to the inventory screen
    /// </summary>
    protected internal abstract bool AddInventorySlot { get; }

    [System.Flags]
    internal enum ModItemType
    {
        None = 0b_0000_0000,
        RosaryBead = 0b_0000_0001,
        Prayer = 0b_0000_0010,
        Relic = 0b_0000_0100,
        SwordHeart = 0b_0000_1000,
        QuestItem = 0b_0001_0000,
        Collectible = 0b_0010_0000,
        Equippables = RosaryBead | Prayer | Relic | SwordHeart,
        All = Equippables | QuestItem | Collectible
    }

    internal abstract ModItemType ItemType { get; }

    internal List<ModItemEffect> Effects { get; } = new();

    private Sprite _picture;

    /// <summary>
    /// Adds an item effect to the custom item
    /// </summary>
    public ModItem AddEffect(ModItemEffect effect)
    {
        if ((effect.ValidItemTypes & ItemType) > 0)
            Effects.Add(effect);
        else
            Main.ModdingAPI.LogWarning($"Can not add effect '{effect.GetType().Name}' to an item of type {ItemType}!");

        return this;
    }

    /// <summary>
    /// Adds multiple item effects to the custom item
    /// </summary>
    public ModItem AddEffects(params ModItemEffect[] effects)
    {
        foreach (var effect in effects)
            AddEffect(effect);

        return this;
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
        item.picture = _picture ??= Picture;
        item.carryonstart = CarryOnStart;
        item.preserveInNewGamePlus = PreserveInNGPlus;

        // Add effects
        foreach (ModItemEffect effect in Effects)
        {
            item.gameObject.AddComponent<ModItemEffectSystem>().SetEffect(effect);
        }

        return item;
    }

    internal abstract void GiveItem();
}
