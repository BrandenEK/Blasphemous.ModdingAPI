
namespace ModdingAPI
{
    public abstract class Mod
    {
        public string ModName { get; private set; }
        public string ModVersion { get; private set; }

        public Mod(string modName, string modVersion)
        {
            ModName = modName;
            ModVersion = modVersion;
            Main.moddingAPI.registerMod(this);
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
