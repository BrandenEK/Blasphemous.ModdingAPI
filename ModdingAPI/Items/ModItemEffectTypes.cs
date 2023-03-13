using Framework.Inventory;

namespace ModdingAPI
{
    public abstract class ModItemEffectAbility : ModItemEffect
    {
        private protected override ObjectEffect.EffectType EffectType => ObjectEffect.EffectType.OnAbilityCast;

        protected internal abstract string AbilityName { get; }

        internal override void SetSystemProperties(ModItemEffectSystem system)
        {
            base.SetSystemProperties(system);
            system.abilityName = AbilityName;
        }
        // Change this to enum
    }

    public abstract class ModItemEffectEquip : ModItemEffect
    {
        private protected override ObjectEffect.EffectType EffectType => ObjectEffect.EffectType.OnEquip;
    }

    // Class for enemy interactions ?

    // Set properties of ModItemEffectSystem based on these
}

// Sound fx
// Conditions & stopping conditions
