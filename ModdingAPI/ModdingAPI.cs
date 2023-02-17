using System.Collections.Generic;
using System.Collections.ObjectModel;
using Framework.Managers;
using Framework.FrameworkCore;
using UnityEngine;

namespace ModdingAPI
{
    internal class ModdingAPI
    {
        private List<Mod> mods;
        private bool initialized;

        public List<GameObject> skinButtons = new List<GameObject>();
        public bool createdSkinButtons { get; set; }

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
            LevelManager.OnLevelLoaded += LevelLoaded;
            LevelManager.OnBeforeLevelLoad += LevelUnloaded;
            
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

            LevelManager.OnLevelLoaded -= LevelLoaded;
            LevelManager.OnBeforeLevelLoad -= LevelUnloaded;
            initialized = false;
        }

        public void LevelLoaded(Level oldLevel, Level newLevel)
        {
            for (int i = 0; i < mods.Count; i++)
            {
                mods[i].LevelLoaded(oldLevel == null ? string.Empty : oldLevel.LevelName, newLevel == null ? string.Empty : newLevel.LevelName);
            }
        }

        public void LevelUnloaded(Level oldLevel, Level newLevel)
        {
            for (int i = 0; i < mods.Count; i++)
            {
                mods[i].LevelUnloaded(oldLevel == null ? string.Empty : oldLevel.LevelName, newLevel == null ? string.Empty : newLevel.LevelName);
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
