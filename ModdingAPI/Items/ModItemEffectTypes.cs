using Framework.Inventory;

namespace ModdingAPI
{
    public abstract class ModItemEffectAbility : ModItemEffect
    {
        protected internal override ObjectEffect.EffectType EffectType => ObjectEffect.EffectType.OnAbilityCast;

        protected internal abstract string AbilityName { get; }
    }

    public abstract class ModItemEffectEquip : ModItemEffect
    {
        protected internal override ObjectEffect.EffectType EffectType => ObjectEffect.EffectType.OnEquip;
    }
}
