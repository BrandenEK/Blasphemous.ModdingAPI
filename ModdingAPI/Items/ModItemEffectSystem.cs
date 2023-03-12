using Framework.Inventory;

namespace ModdingAPI
{
    internal class ModItemEffectSystem : ObjectEffect
    {
        private ModItemEffect modEffect;
        public void SetEffect(ModItemEffect effect)
        {
            modEffect = effect;
        }

        protected override void OnAwake()
        {
            modEffect.Awake();
        }

        protected override void OnStart()
        {
            modEffect.Start();
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
            modEffect.ApplyEffect();
            return true; // Should this return false ???
        }

        protected override void OnRemoveEffect()
        {
            modEffect.RemoveEffect();
        }
    }
}
