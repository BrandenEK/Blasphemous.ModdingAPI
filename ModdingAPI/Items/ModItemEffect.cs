using Framework.Inventory;

namespace ModdingAPI
{
    public abstract class ModItemEffect
    {
        private protected abstract ObjectEffect.EffectType EffectType { get; }

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

        internal ModItemEffect() { }

        internal virtual void SetSystemProperties(ModItemEffectSystem system)
        {
            system.effectType = EffectType;
        }
    }
}
