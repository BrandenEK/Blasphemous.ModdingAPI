using Framework.Inventory;
using Framework.Managers;

namespace Blasphemous.ModdingAPI.Items;

internal class ModItemEffectSystem : ObjectEffect
{
    private ModItemEffect modEffect;

    public void SetEffect(ModItemEffect effect)
    {
        modEffect = effect;
        modEffect.SetSystemProperties(this);
        modEffect.InventoryObject = InvObj;
        modEffect.Initialize();
    }

    protected override void OnUpdate()
    {
        modEffect.Update();
    }

    protected override void OnDispose()
    {
        modEffect.Dispose();
    }

    protected override bool OnApplyEffect()
    {
        if (effectType == EffectType.OnAdquisition)
        {
            // Set flag to only apply effect once
            string effectFlag = InvObj.id.ToUpper() + "_EFFECT";
            if (Core.Events.GetFlag(effectFlag))
                return true;
            Core.Events.SetFlag(effectFlag, true, InvObj.preserveInNewGamePlus);
        }

        modEffect.ApplyEffect();
        return true;
    }

    protected override void OnRemoveEffect()
    {
        modEffect.RemoveEffect();
    }
}
