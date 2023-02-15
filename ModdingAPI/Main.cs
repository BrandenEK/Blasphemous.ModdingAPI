using BepInEx;
using HarmonyLib;

namespace ModdingAPI
{
    [BepInPlugin("com.damocles.blasphemous.modding-api", "Modding API", "1.0.0")]
    [BepInProcess("Blasphemous.exe")]
    public class Main : BaseUnityPlugin
    {
        public static ModdingAPI moddingAPI;
        private static Main instance;

        private void Awake()
        {
            moddingAPI = new ModdingAPI();
            instance = this;
            Patch();
        }

        private void Update() { moddingAPI.Update(); }

        private void LateUpdate() { moddingAPI.LateUpdate(); }

        private void Patch()
        {
            Harmony harmony = new Harmony("com.damocles.blasphemous.modding-api");
            harmony.PatchAll();
        }

        // Log messages to unity console
        public static void LogMessage(string message)
        {
            instance.logMessage(message);
        }
        private void logMessage(string message)
        {
            Logger.LogMessage(message);
        }

        // Log warnings to unity console
        public static void LogWarning(string message)
        {
            instance.logWarning(message);
        }
        private void logWarning(string message)
        {
            Logger.LogWarning(message);
        }

        // Log errors to unity console
        public static void LogError(string message)
        {
            instance.logError(message);
        }
        private void logError(string message)
        {
            Logger.LogError(message);
        }
    }
}
