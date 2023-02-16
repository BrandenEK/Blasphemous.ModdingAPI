using System.Reflection;
using HarmonyLib;

namespace ModdingAPI
{
    public abstract class Mod
    {
        public string ModId { get; private set; }
        public string ModName { get; private set; }
        public string ModVersion { get; private set; }

        public Mod(string modId, string modName, string modVersion)
        {
            ModId = modId;
            ModName = modName;
            ModVersion = modVersion;

            // Register and patch this mod
            Main.moddingAPI.registerMod(this);
            Log("Current: " + Assembly.GetExecutingAssembly().GetName());
            Log("This mod: " + GetType().Assembly.GetName());

            Harmony harmony = new Harmony(ModId);
            harmony.PatchAll(Assembly.GetExecutingAssembly());
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
