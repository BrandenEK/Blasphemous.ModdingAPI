using Framework.Inventory;

namespace ModdingAPI.Items
{
    public abstract class ModItemEffect
    {
        private ObjectEffect.EffectType effectType;

        // Only for AbilityCast
        protected abstract string AbilityName { get; }

        protected abstract bool UseOnPrayerCast { get; }

        // Have to do with if the ability is timed
        protected abstract float EffectTime { get; }
        protected abstract bool UsePrayerDurationModifier { get; }

        protected internal virtual void Awake()
        {

        }

        protected internal virtual void Start()
        {

        }

        protected internal virtual void Update()
        {

        }

        protected internal virtual void Dispose()
        {

        }

        protected internal abstract void ApplyEffect();

        protected internal abstract void RemoveEffect();

        internal ModItemEffect()
        {
            if (AbilityName != null && AbilityName != "")
            {
                effectType = ObjectEffect.EffectType.OnAbilityCast;
            }
            else if (UseOnPrayerCast)
            {
                effectType = ObjectEffect.EffectType.OnUse;
            }
            else
            {
                effectType = ObjectEffect.EffectType.OnEquip;
            }
        }

        internal virtual void SetSystemProperties(ModItemEffectSystem system)
        {
            system.effectType = effectType;
            system.abilityName = AbilityName;
            system.UseWhenCastingPrayer = UseOnPrayerCast;
            system.EffectTime = EffectTime;
            system.UsePrayerDurationAddition = UsePrayerDurationModifier;
        }
    }
}
