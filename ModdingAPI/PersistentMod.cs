using Framework.Managers;

namespace ModdingAPI
{
    public abstract class PersistentMod : Mod
    {
        public PersistentMod(string modName, string modVersion) : base(modName, modVersion) { }

        public abstract PersistentManager.PersistentData SaveGame();

        public abstract void LoadGame(PersistentManager.PersistentData data);

        public abstract void ResetGame();

        public abstract string Id { get; }

        public override void Initialize()
        {
            base.Initialize();
            Core.Persistence.AddPersistentManager(new ModPersistentSystem(this));
        }
    }
}
