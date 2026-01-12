using BepInEx;
using Blasphemous.ModdingAPI.Persistence;
using System;
using System.IO;
using System.Reflection;

namespace Blasphemous.ModdingAPI;

[BepInPlugin(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_VERSION)]
internal class Main : BaseUnityPlugin
{
    public static ModLoader ModLoader { get; private set; }
    public static ModdingAPI ModdingAPI { get; private set; }
    public static Main Instance { get; private set; }

    private void Awake()
    {
        AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(LoadMissingAssemblies);

        if (Instance == null)
            Instance = this;

        ModLoader = new ModLoader();
    }

    private void Start()
    {
        ModdingAPI = new ModdingAPI();
    }

    private void Update() => ModLoader.Update();

    private void LateUpdate() => ModLoader.LateUpdate();

    private void OnApplicationQuit() => GlobalSaveData.Save();

    private Assembly LoadMissingAssemblies(object send, ResolveEventArgs args)
    {
        string assemblyPath = Path.GetFullPath($"Modding/data/{args.Name.Substring(0, args.Name.IndexOf(","))}.dll");

        if (File.Exists(assemblyPath))
        {
            Logger.LogWarning("Successfully loaded missing assembly: " + args.Name);
            return Assembly.LoadFrom(assemblyPath);
        }
        else
        {
            Logger.LogWarning("Failed to load missing assembly: " + args.Name);
            return null;
        }
    }
}
