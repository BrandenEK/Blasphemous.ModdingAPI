using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ModdingAPI
{
    public class ModdingAPI
    {
        private List<Mod> mods;
        private bool initialized;

        public ModdingAPI()
        {
            mods = new List<Mod>();
            initialized = false;
        }

        public void Update()
        {
            if (!initialized) return;

            for (int i = 0; i < mods.Count; i++)
            {
                mods[i].Update();
            }
        }

        public void LateUpdate()
        {
            if (!initialized) return;

            for (int i = 0; i < mods.Count; i++)
            {
                mods[i].LateUpdate();
            }
        }

        public void Initialize()
        {
            initialized = true;
            for (int i = 0; i < mods.Count; i++)
            {
                mods[i].Initialize();
            }
        }

        public void Dispose()
        {
            for (int i = 0; i < mods.Count; i++)
            {
                mods[i].Dispose();
            }
            initialized = false;
        }

        public void LevelLoaded(string oldLevel, string newLevel)
        {
            for (int i = 0; i < mods.Count; i++)
            {
                mods[i].LevelLoaded(oldLevel, newLevel);
            }
        }

        public void registerMod(Mod mod)
        {
            if (!mods.Contains(mod))
            {
                Main.LogMessage($"Registering mod {mod.ModName} ({mod.ModVersion})");
                mods.Add(mod);
            }
        }

        public ReadOnlyCollection<Mod> getMods()
        {
            return mods.AsReadOnly();
        }
    }
}
