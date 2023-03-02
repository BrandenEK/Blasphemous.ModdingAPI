using System.Collections.Generic;
using System.Collections.ObjectModel;
using Framework.Managers;
using Framework.FrameworkCore;

namespace ModdingAPI
{
    internal class ModdingAPI
    {
        private List<Mod> mods;
        private List<ModCommand> modCommands;

        public SkinLoader skinLoader { get; private set; }
        public FileUtil fileUtil { get; private set; }
        public Localizer localizer { get; private set; }

        private bool initialized;

        public ModdingAPI()
        {
            mods = new List<Mod>();
            modCommands = new List<ModCommand>();
            skinLoader = new SkinLoader();
            fileUtil = new FileUtil();
            localizer = new Localizer(fileUtil.loadLocalization());
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

            Main.LogSpecial("Initialization");
            for (int i = 0; i < mods.Count; i++)
            {
                mods[i].Initialize();
                if (mods[i] is PersistentMod mod)
                    Core.Persistence.AddPersistentManager(new ModPersistentSystem(mod));
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
            string oLevel = oldLevel == null ? string.Empty : oldLevel.LevelName, nLevel = newLevel == null ? string.Empty : newLevel.LevelName;

            Main.LogSpecial("Loaded level " + nLevel);
            for (int i = 0; i < mods.Count; i++)
            {
                mods[i].LevelLoaded(oLevel, nLevel);
            }
        }

        public void LevelUnloaded(Level oldLevel, Level newLevel)
        {
            string oLevel = oldLevel == null ? string.Empty : oldLevel.LevelName, nLevel = newLevel == null ? string.Empty : newLevel.LevelName;
            for (int i = 0; i < mods.Count; i++)
            {
                mods[i].LevelUnloaded(oLevel, nLevel);
            }
        }

        public void NewGame()
        {
            for (int i = 0; i < mods.Count; i++)
            {
                if (mods[i] is PersistentMod mod)
                    mod.NewGame();
            }
        }

        public void registerMod(Mod mod)
        {
            if (!mods.Contains(mod))
            {
                Main.LogMessage(Main.MOD_NAME, $"Registering mod: {mod.ModName} ({mod.ModVersion})");
                Main.AddLogger(mod.ModName);
                mods.Add(mod);
            }
        }

        public void registerCommand(ModCommand command)
        {
            if (!modCommands.Contains(command))
            {
                modCommands.Add(command);
            }
        }

        public ReadOnlyCollection<Mod> getMods()
        {
            return mods.AsReadOnly();
        }

        public ReadOnlyCollection<ModCommand> getModCommnds()
        {
            return modCommands.AsReadOnly();
        }
    }
}
