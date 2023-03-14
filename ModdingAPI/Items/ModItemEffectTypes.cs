using Framework.Inventory;

namespace ModdingAPI.Items
{
    public abstract class ModItemEffectOnEquip : ModItemEffect
    {
        internal override void SetSystemProperties(ModItemEffectSystem system)
        {
            system.effectType = ObjectEffect.EffectType.OnEquip;
        }
    }

    public abstract class ModItemEffectOnAbility : ModItemEffect
    {
        // Change this to enum
        protected abstract string AbilityName { get; }

        protected abstract float EffectTime { get; }

        internal override void SetSystemProperties(ModItemEffectSystem system)
        {
            system.effectType = ObjectEffect.EffectType.OnAbilityCast;
            system.abilityName = AbilityName;
            system.LimitTime = EffectTime > 0; // necessary ?
            system.EffectTime = EffectTime;
        }
    }

    public abstract class ModItemEffectOnPrayerUse : ModItemEffect
    {
        protected abstract float EffectTime { get; }

        protected abstract bool OnlyWhenPrayerActive { get; }

        protected abstract bool UsePrayerDurationModifier { get; }

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

    public abstract class ModItemEffectOnAcquire : ModItemEffect
    {
        protected abstract bool ActivateOnce { get; }

        internal override void SetSystemProperties(ModItemEffectSystem system)
        {
            system.effectType = ActivateOnce ? ObjectEffect.EffectType.OnAdquisition : ObjectEffect.EffectType.OnInitialization;
        }
    }
}