using HarmonyLib;
using System;

namespace ModdingAPI
{
    public abstract class Mod
    {
        public string ModId { get; private set; }
        public string ModName { get; private set; }
        public string ModVersion { get; private set; }
        public FileUtil FileUtil { get; private set; }

        public Mod(string modId, string modName, string modVersion)
        {
            ModId = modId;
            ModName = modName;
            ModVersion = modVersion;
            FileUtil = new FileUtil(this);

            // Set up logging
            FileUtil.clearLog();
            FileUtil.appendLog(DateTime.Now.ToString());

            // Register and patch this mod
            Main.moddingAPI.registerMod(this);
            Harmony harmony = new Harmony(ModId);
            harmony.PatchAll(GetType().Assembly);
        }

        public virtual void Update()
        {

        }

        public virtual void LateUpdate()
        {

        }

        public virtual void Initialize()
        {

        }

        public virtual void Dispose()
        {

        }

        public virtual void LevelLoaded(string oldLevel, string newLevel)
        {

        }

        public virtual void LevelUnloaded(string oldLevel, string newLevel)
        {

        }

        public void Log(string message) { Main.LogMessage(message); }

        public void LogWarning(string message) { Main.LogWarning(message); }

        public void LogError(string message) { Main.LogError(message); }
    }
}
