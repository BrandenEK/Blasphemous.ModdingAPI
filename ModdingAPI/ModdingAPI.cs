using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ModdingAPI
{
    public class ModdingAPI
    {
        private List<Mod> mods;

        public void Awake()
        {
            mods = new List<Mod>();
        }

        public void Update()
        {
            for (int i = 0; i < mods.Count; i++)
            {
                mods[i].Update();
            }
        }

        public void LateUpdate()
        {
            for (int i = 0; i < mods.Count; i++)
            {
                mods[i].LateUpdate();
            }
        }

        public void Initialize()
        {
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
