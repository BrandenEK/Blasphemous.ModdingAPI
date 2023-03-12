using Framework.Inventory;

namespace ModdingAPI
{
    public abstract class ModItemEffect
    {
        protected internal abstract ObjectEffect.EffectType effectType { get; }

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
    }
}
