using System.Collections.Generic;
using System.Collections.ObjectModel;
using Framework.Managers;
using Framework.FrameworkCore;
using ModdingAPI.Commands;
using ModdingAPI.Penitences;
using ModdingAPI.Skins;
using ModdingAPI.Items;

namespace ModdingAPI
{
    internal class ModdingAPI
    {
        private readonly List<Mod> mods;
        private readonly List<ModCommand> modCommands;
        private readonly List<ModPenitence> modPenitences;
        private readonly List<ModItem> modItems;

        public SkinLoader skinLoader { get; private set; }
        public PenitenceLoader penitenceLoader { get; private set; }
        public ItemLoader itemLoader { get; private set; }
        public FileUtil fileUtil { get; private set; }
        public Localizer localizer { get; private set; }
        public HitboxViewer HitboxViewer { get; private set; }

        private bool initialized;

        public ModdingAPI()
        {
            mods = new List<Mod>();
            modCommands = new List<ModCommand>();
            modPenitences = new List<ModPenitence>();
            modItems = new List<ModItem>();

            skinLoader = new SkinLoader();
            penitenceLoader = new PenitenceLoader();
            itemLoader = new ItemLoader();
            fileUtil = new FileUtil();
            localizer = new Localizer(fileUtil.loadLocalization());
            HitboxViewer = new HitboxViewer();
            initialized = false;
        }

        public void Update()
        {
            if (!initialized) return;

            for (int i = 0; i < mods.Count; i++)
            {
                mods[i].Update();
            }
            penitenceLoader.Update();
            HitboxViewer.Update();
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

            if (modPenitences.Count > 0)
                Core.PenitenceManager.ResetPersistence();
            HitboxViewer.LoadImage();
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

            HitboxViewer.AddHitboxes();
        }

        public void LevelUnloaded(Level oldLevel, Level newLevel)
        {
            string oLevel = oldLevel == null ? string.Empty : oldLevel.LevelName, nLevel = newLevel == null ? string.Empty : newLevel.LevelName;
            for (int i = 0; i < mods.Count; i++)
            {
                mods[i].LevelUnloaded(oLevel, nLevel);
            }

            HitboxViewer.RemoveHitboxes();
        }

        public void NewGame()
        {
            for (int i = 0; i < mods.Count; i++)
            {
                if (mods[i] is PersistentMod mod)
                    mod.NewGame();
            }

            foreach (ModItem item in modItems)
            {
                if (item.CarryOnStart)
                    item.GiveItem();
            }
        }

        public void registerMod(Mod mod)
        {
            foreach (Mod modMod in mods)
            {
                if (modMod.ModId == mod.ModId)
                    return;
            }
            Main.LogMessage(Main.MOD_NAME, $"Registering mod: {mod.ModName} ({mod.ModVersion})");
            Main.AddLogger(mod.ModName);
            mods.Add(mod);
        }

        public void registerCommand(ModCommand command)
        {
            foreach (ModCommand modCommand in modCommands)
            {
                if (modCommand.CommandName == command.CommandName)
                    return;
            }
            modCommands.Add(command);
        }

        public void registerPenitence(ModPenitence penitence)
        {
            foreach (ModPenitence modPenitence in modPenitences)
            {
                if (modPenitence.Id == penitence.Id)
                    return;
            }
            modPenitences.Add(penitence);
            Main.LogMessage(Main.MOD_NAME, $"Registering custom penitence: {penitence.Name} ({penitence.Id})");
        }

        public void registerItem(ModItem item)
        {
            foreach (ModItem modItem in modItems)
            {
                if (modItem.Id == item.Id)
                    return;
            }
            modItems.Add(item);
            itemLoader.AddItem(item);
            Main.LogMessage(Main.MOD_NAME, $"Registering custom item: {item.Name} ({item.Id})");
        }

        public ReadOnlyCollection<Mod> GetMods()
        {
            return mods.AsReadOnly();
        }

        public ReadOnlyCollection<ModCommand> GetModCommands()
        {
            return modCommands.AsReadOnly();
        }

        public ReadOnlyCollection<ModPenitence> GetModPenitences()
        {
            return modPenitences.AsReadOnly();
        }

        public ReadOnlyCollection<ModItem> GetModItems()
        {
            return modItems.AsReadOnly();
        }
    }
}
