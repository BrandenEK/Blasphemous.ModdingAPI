using Framework.Inventory;

namespace ModdingAPI.Items
{
    /// <summary>
    /// An item effect that is activated when equipping the item
    /// </summary>
    public abstract class ModItemEffectOnEquip : ModItemEffect
    {
        internal override ModItem.ModItemType ValidItemTypes => ModItem.ModItemType.Equippables;

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
        /// <summary>
        /// The name of the ability to activate this effect
        /// </summary>
        protected abstract string AbilityName { get; }

        /// <summary>
        /// How long this effect should last, or 0 for only ability duration
        /// </summary>
        protected abstract float EffectTime { get; }

        internal override ModItem.ModItemType ValidItemTypes => ModItem.ModItemType.Equippables;

        internal override void SetSystemProperties(ModItemEffectSystem system)
        {
            system.effectType = ObjectEffect.EffectType.OnAbilityCast;
            system.abilityName = AbilityName;
            system.LimitTime = EffectTime > 0;
            system.EffectTime = EffectTime;
        }
    }

    /// <summary>
    /// An item effect that is activated when using a prayer
    /// </summary>
    public abstract class ModItemEffectOnPrayerUse : ModItemEffect
    {
        /// <summary>
        /// How long the prayer should last
        /// </summary>
        protected abstract float EffectTime { get; }

        /// <summary>
        /// Whether or not the effect duration should be scaled by stat modifiers
        /// </summary>
        protected abstract bool UsePrayerDurationModifier { get; }

        internal override ModItem.ModItemType ValidItemTypes => ModItem.ModItemType.Prayer;

        internal override void SetSystemProperties(ModItemEffectSystem system)
        {
            system.effectType = ObjectEffect.EffectType.OnUse;
            system.LimitTime = true;
            system.EffectTime = EffectTime;
            system.OnlyWhenUsingPrayer = true;
            system.UsePrayerDurationAddition = UsePrayerDurationModifier;
            system.UseWhenCastingPrayer = true;
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

        internal override ModItem.ModItemType ValidItemTypes => ModItem.ModItemType.All;

        internal override void SetSystemProperties(ModItemEffectSystem system)
        {
            system.effectType = ActivateOnce ? ObjectEffect.EffectType.OnAdquisition : ObjectEffect.EffectType.OnInitialization;
        }
    }
}