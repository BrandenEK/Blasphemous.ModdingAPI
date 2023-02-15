
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
            Log("Update");
        }

        public virtual void LateUpdate()
        {
            Log("LateUpdate");
        }

        // Persistence stuff

        public virtual void Initialize()
        {
            Log("Initialize");
        }

        public virtual void Dispose()
        {
            Log("Dispose");
        }

        public virtual void LevelLoaded(string oldLevel, string newLevel)
        {
            Log("Level loaded: " + newLevel);
        }

        public void Log(string message) { Main.LogMessage(message); }

        public void LogWarning(string message) { Main.LogWarning(message); }

        public void LogError(string message) { Main.LogError(message); }
    }
}
