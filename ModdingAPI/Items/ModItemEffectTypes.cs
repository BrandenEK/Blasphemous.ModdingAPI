using Framework.Inventory;

namespace ModdingAPI
{
    public abstract class ModItemEffectAbility : ModItemEffect
    {
        internal override ObjectEffect.EffectType EffectType => ObjectEffect.EffectType.OnAbilityCast;

        protected internal abstract string AbilityName { get; }
        // Change this to enum
    }

    public abstract class ModItemEffectEquip : ModItemEffect
    {
        internal override ObjectEffect.EffectType EffectType => ObjectEffect.EffectType.OnEquip;
    }

    // Class for enemy interactions ?
}
