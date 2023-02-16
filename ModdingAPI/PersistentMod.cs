using Framework.Managers;

namespace ModdingAPI
{
    /// <summary>
    /// An abstract representation of a mod that can save and load game data
    /// </summary>
    public abstract class PersistentMod : Mod
    {
        /// <summary>
        /// Creates, registers, and patches a new persistent mod
        /// </summary>
        /// <param name="modId">The unique id of this mod used for harmony patching</param>
        /// <param name="modName">The name of the mod to be displayed on the main menu</param>
        /// <param name="modVersion">The version of the mod</param>
        public PersistentMod(string modId, string modName, string modVersion) : base(modId, modName, modVersion) { }

        /// <summary>
        /// Called by the modding API whenever the game is saved
        /// </summary>
        /// <returns>The persistent data to save</returns>
        public abstract PersistentManager.PersistentData SaveGame();

        /// <summary>
        /// Called by the modding API whenever the game is loaded
        /// </summary>
        /// <param name="data">The persistent data that was loaded</param>
        public abstract void LoadGame(PersistentManager.PersistentData data);

        /// <summary>
        /// Called by the modding API whenever the game is reset
        /// </summary>
        public abstract void ResetGame();

        /// <summary>
        /// The unique id of this persistent system
        /// </summary>
        public abstract string PersistentID { get; }

        /// <summary>
        /// Called at the beginning of the game by the modding API
        /// </summary>
        protected internal override void Initialize()
        {
            base.Initialize();
            Core.Persistence.AddPersistentManager(new ModPersistentSystem(this));
        }
    }
}
