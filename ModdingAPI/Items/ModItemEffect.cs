using Framework.Inventory;

namespace ModdingAPI.Items
{
    public abstract class ModItemEffect
    {
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

        internal abstract void SetSystemProperties(ModItemEffectSystem system);

        internal ModItemEffect() { }
    }
}
