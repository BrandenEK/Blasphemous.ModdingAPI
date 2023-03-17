using Framework.Inventory;

namespace ModdingAPI.Items
{
    /// <summary>
    /// An item effect that is activated when equipping the item
    /// </summary>
    public abstract class ModItemEffectOnEquip : ModItemEffect
    {
        internal override ModItem.ModItemType ValidItemTypes => ModItem.EquippableTypes;

        internal override void SetSystemProperties(ModItemEffectSystem system)
        {
            system.effectType = ObjectEffect.EffectType.OnEquip;
        }
    }

    /// <summary>
    /// An item effect that is activated when using an ability
    /// </summary>
    public abstract class ModItemEffectOnAbility : ModItemEffect
    {
        // Change this to enum

        /// <summary>
        /// The name of the ability to activate this effect
        /// </summary>
        protected abstract string AbilityName { get; }

        /// <summary>
        /// How long this effect should last, or 0 for no time limit
        /// </summary>
        protected abstract float EffectTime { get; } // ??

        internal override ModItem.ModItemType ValidItemTypes => ModItem.EquippableTypes;

        internal override void SetSystemProperties(ModItemEffectSystem system)
        {
            system.effectType = ObjectEffect.EffectType.OnAbilityCast;
            system.abilityName = AbilityName;
            system.LimitTime = EffectTime > 0; // necessary ?
            system.EffectTime = EffectTime;
        }
    }

    /// <summary>
    /// An item effect that is activated when using a prayer
    /// </summary>
    public abstract class ModItemEffectOnPrayerUse : ModItemEffect
    {
        /// <summary>
        /// How long this effect should last, or 0 for no time limit
        /// </summary>
        protected abstract float EffectTime { get; }

        /// <summary>
        /// Whether or not this effect should only last while the prayer is active
        /// </summary>
        protected abstract bool OnlyWhenPrayerActive { get; }

        /// <summary>
        /// Whether or not the effect duration should be scaled by stat modifiers
        /// </summary>
        protected abstract bool UsePrayerDurationModifier { get; }

        internal override ModItem.ModItemType ValidItemTypes => ModItem.ModItemType.Prayer;

        internal override void SetSystemProperties(ModItemEffectSystem system)
        {
            system.effectType = ObjectEffect.EffectType.OnUse;
            system.LimitTime = EffectTime > 0; // necessary ?
            system.EffectTime = EffectTime;
            system.OnlyWhenUsingPrayer = OnlyWhenPrayerActive;
            system.UsePrayerDurationAddition = UsePrayerDurationModifier;
            system.UseWhenCastingPrayer = true; // ??
        }
    }

    /// <summary>
    /// An item effect that is activated when first obtaining the item
    /// </summary>
    public abstract class ModItemEffectOnAcquire : ModItemEffect
    {
        /// <summary>
        /// Whether to activate only once, or remain active as long as the item is owned
        /// </summary>
        protected abstract bool ActivateOnce { get; }

        internal override ModItem.ModItemType ValidItemTypes => ModItem.AllTypes;

        internal override void SetSystemProperties(ModItemEffectSystem system)
        {
            system.effectType = ActivateOnce ? ObjectEffect.EffectType.OnAdquisition : ObjectEffect.EffectType.OnInitialization;
        }
    }
}