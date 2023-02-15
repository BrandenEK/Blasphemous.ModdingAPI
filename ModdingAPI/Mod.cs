
namespace ModdingAPI
{
    public abstract class Mod
    {
        public string ModName { get; private set; }
        public string ModVersion { get; private set; }

        public Mod(string name, string version)
        {
            ModName = name;
            ModVersion = version;
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

        // Persistent interface

        public void Log(string message) { Main.LogMessage(message); }

        public void LogWarning(string message) { Main.LogWarning(message); }

        public void LogError(string message) { Main.LogError(message); }
    }
}
