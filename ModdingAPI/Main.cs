using BepInEx;
using HarmonyLib;

namespace ModdingAPI
{
    [BepInPlugin(MOD_ID, MOD_NAME, MOD_VERSION)]
    [BepInProcess("Blasphemous.exe")]
    internal class Main : BaseUnityPlugin
    {
        public const string MOD_ID = "com.damocles.blasphemous.modding-api";
        public const string MOD_NAME = "Modding API";
        public const string MOD_VERSION = "1.0.0";

        internal static ModdingAPI moddingAPI;
        private static Main instance;

        private void Awake()
        {
            moddingAPI = new ModdingAPI();
            instance = this;
            Harmony harmony = new Harmony(MOD_ID);
            harmony.PatchAll();
        }

        private void Update() { moddingAPI.Update(); }

        private void LateUpdate() { moddingAPI.LateUpdate(); }

        // Log messages to unity console
        internal static void LogMessage(string message)
        {
            instance.logMessage(message);
        }
        private void logMessage(string message)
        {
            Logger.LogMessage(message);
        }

        // Log warnings to unity console
        internal static void LogWarning(string message)
        {
            instance.logWarning(message);
        }
        private void logWarning(string message)
        {
            Logger.LogWarning(message);
        }

        // Log errors to unity console
        internal static void LogError(string message)
        {
            instance.logError(message);
        }
        private void logError(string message)
        {
            Logger.LogError(message);
        }
    }
}
