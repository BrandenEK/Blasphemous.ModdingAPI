using BepInEx;
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

    private Assembly LoadMissingAssemblies(object send, ResolveEventArgs args)
    {
        string assemblyPath = Path.GetFullPath($"Modding/data/{args.Name.Substring(0, args.Name.IndexOf(","))}.dll");

        if (File.Exists(assemblyPath))
        {
            ModdingAPI.LogWarning("Successfully loaded missing assembly: " + args.Name);
            return Assembly.LoadFrom(assemblyPath);
        }
        else
        {
            ModdingAPI.LogWarning("Failed to load missing assembly: " + args.Name);
            return null;
        }
    }
}
