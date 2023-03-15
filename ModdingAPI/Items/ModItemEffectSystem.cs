﻿using Framework.Inventory;

namespace ModdingAPI.Items
{
    internal class ModItemEffectSystem : ObjectEffect
    {
        private ModItemEffect modEffect;
        public void SetEffect(ModItemEffect effect)
        {
            modEffect = effect;
            modEffect.SetSystemProperties(this);
            modEffect.InventoryObject = InvObj;
            modEffect.Awake();
        }

        protected override void OnAwake()
        {
            if (modEffect != null)
                modEffect.Awake();
        }

        protected override void OnStart()
        {
            if (modEffect != null)
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
