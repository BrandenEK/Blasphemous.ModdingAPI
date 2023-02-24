using HarmonyLib;
using System;

namespace ModdingAPI
{
    /// <summary>
    /// An abstract representation of a mod
    /// </summary>
    public abstract class Mod
    {
        private Localizer localizer;

        /// <summary>
        /// The unique id of this mod used for harmony patching
        /// </summary>
        public string ModId { get; private set; }
        /// <summary>
        /// The name of the mod to be displayed on the main menu
        /// </summary>
        public string ModName { get; private set; }
        /// <summary>
        /// The version of the mod
        /// </summary>
        public string ModVersion { get; private set; }
        /// <summary>
        /// The file utility for performing various IO operations
        /// </summary>
        public FileUtil FileUtil { get; private set; }

        /// <summary>
        /// Creates, registers, and patches a new mod
        /// </summary>
        /// <param name="modId">The unique id of this mod used for harmony patching</param>
        /// <param name="modName">The name of the mod to be displayed on the main menu</param>
        /// <param name="modVersion">The version of the mod</param>
        public Mod(string modId, string modName, string modVersion)
        {
            ModId = modId;
            ModName = modName;
            ModVersion = modVersion;
            FileUtil = new FileUtil(this);

            // Set up localization
            localizer = new Localizer(FileUtil.loadLocalization());

            // Set up logging
            FileUtil.clearLog();
            FileUtil.appendLog(DateTime.Now.ToString() + "\n");

            // Register and patch this mod
            Main.moddingAPI.registerMod(this);
            Harmony harmony = new Harmony(ModId);
            harmony.PatchAll(GetType().Assembly);
        }

        /// <summary>
        /// Called every frame by the modding API
        /// </summary>
        protected internal virtual void Update()
        {

        }

        /// <summary>
        /// Called at the end of every frame by the modding API
        /// </summary>
        protected internal virtual void LateUpdate()
        {

        }

        /// <summary>
        /// Called at the beginning of the game by the modding API
        /// </summary>
        protected internal virtual void Initialize()
        {

        }

        /// <summary>
        /// Called at the end of the game by the modding API
        /// </summary>
        protected internal virtual void Dispose()
        {

        }

        /// <summary>
        /// Called when a scene is loaded by the modding API
        /// </summary>
        /// <param name="oldLevel">The name of the old level</param>
        /// <param name="newLevel">The name of the new level</param>
        protected internal virtual void LevelLoaded(string oldLevel, string newLevel)
        {

        }

        /// <summary>
        /// Called when a scene is unloaded by the modding API
        /// </summary>
        /// <param name="oldLevel">The name of the old level</param>
        /// <param name="newLevel">The name of the new level</param>
        protected internal virtual void LevelUnloaded(string oldLevel, string newLevel)
        {

        }

        /// <summary>
        /// Registers a command to be used in the debug console for this mod
        /// </summary>
        /// <param name="command">The new command to be added</param>
        protected void RegisterCommand(ModCommand command)
        {
            Main.moddingAPI.registerCommand(command);
        }

        /// <summary>
        /// Checks whether a specific mod has been loaded or not
        /// </summary>
        /// <param name="modId">The unique id of the mod</param>
        /// <returns>Whether or not the mod has been loaded</returns>
        protected bool IsModLoaded(string modId)
        {
            foreach (Mod mod in Main.moddingAPI.getMods())
            {
                if (mod.ModId == ModId)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Localizes text based on a key into the game's current language
        /// </summary>
        /// <param name="key">The key of the text to localize</param>
        /// <returns>The localized text</returns>
        public string Localize(string key)
        {
            return localizer.Localize(key);
        }

        /// <summary>
        /// Logs a message to the console
        /// </summary>
        /// <param name="message">The message to display</param>
        public void Log(string message) { Main.LogMessage(ModName, message); }

        /// <summary>
        /// Logs a warning to the console
        /// </summary>
        /// <param name="warning">The warning to display</param>
        public void LogWarning(string warning) { Main.LogWarning(ModName, warning); }

        /// <summary>
        /// Logs an error to the console
        /// </summary>
        /// <param name="error">The error to display</param>
        public void LogError(string error) { Main.LogError(ModName, error); }
    }
}
