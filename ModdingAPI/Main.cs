using BepInEx;
using HarmonyLib;
using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Collections.Generic;

namespace ModdingAPI
{
    [BepInPlugin(MOD_ID, MOD_NAME, MOD_VERSION)]
    [BepInProcess("Blasphemous.exe")]
    internal class Main : BaseUnityPlugin
    {
        public const string MOD_ID = "com.damocles.blasphemous.modding-api";
        public const string MOD_NAME = "Modding API";
        public const string MOD_VERSION = "1.2.0";

        internal static ModdingAPI moddingAPI;
        private static Dictionary<string, BepInEx.Logging.ManualLogSource> loggers;

        private void Awake()
        {
            moddingAPI = new ModdingAPI();
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(loadMissingAssemblies);
            Harmony harmony = new Harmony(MOD_ID);
            harmony.PatchAll();

            loggers = new Dictionary<string, BepInEx.Logging.ManualLogSource>();
            AddLogger(MOD_NAME);
        }

        private Assembly loadMissingAssemblies(object send, ResolveEventArgs args)
        {
            string assemblyPath = Path.GetFullPath($"Modding\\data\\{args.Name.Substring(0, args.Name.IndexOf(","))}.dll");
            LogMessage(MOD_NAME, "Loading assembly from " + assemblyPath);
            return File.Exists(assemblyPath) ? Assembly.LoadFrom(assemblyPath) : null;
        }

        private void Update() { moddingAPI.Update(); }

        private void LateUpdate() { moddingAPI.LateUpdate(); }

        public static void AddLogger(string name)
        {
            if (!loggers.ContainsKey(name))
                loggers.Add(name, BepInEx.Logging.Logger.CreateLogSource(name));
        }

        // Log messages to unity console
        internal static void LogMessage(string mod, string message)
        {
            if (loggers.ContainsKey(mod))
                loggers[mod].LogMessage(message);
        }

        // Log warnings to unity console
        internal static void LogWarning(string mod, string message)
        {
            if (loggers.ContainsKey(mod))
                loggers[mod].LogWarning(message);
        }

        // Log errors to unity console
        internal static void LogError(string mod, string message)
        {
            if (loggers.ContainsKey(mod))
                loggers[mod].LogError(message);
        }

        // Logs special message to unity console
        internal static void LogSpecial(string message)
        {
            // Create line text
            StringBuilder sb = new StringBuilder();
            int length = message.Length;
            while (length > 0)
            {
                sb.Append('=');
                length--;
            }
            string line = sb.ToString();

            // Create final message
            BepInEx.Logging.ManualLogSource logger = loggers[MOD_NAME];
            logger.LogMessage("");
            logger.LogMessage(line);
            logger.LogMessage(message);
            logger.LogMessage(line);
            logger.LogMessage("");
        }
    }
}
