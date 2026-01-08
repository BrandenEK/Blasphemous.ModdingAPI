using BepInEx;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;

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

    private void Update()
    {
        ModLoader.Update();

        if (UnityEngine.Input.GetKeyUp(KeyCode.Alpha9))
        {
            ModLog.Warn("Toggle");
            open = !open;
        }
    }

    private void LateUpdate() => ModLoader.LateUpdate();

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

    private Rect window;
    private bool open = false;

    private bool tizona = false;
    private bool cutscenes = false;
    private bool pathnotes = false;
    private bool warp = false;

    private void OnGUI()
    {
        Cursor.visible = true;
        Event e = Event.current;
        if (e.type == EventType.MouseDown && e.button == 0)
            open = window.Contains(e.mousePosition);

        int ypos = Screen.height - (open ? HEIGHT : 17);
        window = GUI.Window(0, new Rect(20, ypos, 200, HEIGHT + 20), SettingsWindow, "Quality of Life");

        GUI.Window(1, new Rect(240, Screen.height - 17, 200, HEIGHT + 20), SettingsWindow, "Glitch Reviver");
        GUI.Window(2, new Rect(460, Screen.height - 17, 200, HEIGHT + 20), SettingsWindow, "Hitbox Viewer");
    }

    private void SettingsWindow(int windowID)
    {
        GUI.Label(new Rect(10, 20, 180, 40), "Checkboxes");

        int ypos = 20;
        tizona = GUI.Toggle(new Rect(10, ypos += LINE_HEIGHT, 180, LINE_HEIGHT), tizona, "Tizona fix");
        cutscenes = GUI.Toggle(new Rect(10, ypos += LINE_HEIGHT, 180, LINE_HEIGHT), cutscenes, "Skip cutscenes");
        pathnotes = GUI.Toggle(new Rect(10, ypos += LINE_HEIGHT, 180, LINE_HEIGHT), pathnotes, "Patch notes fix");
        warp = GUI.Toggle(new Rect(10, ypos += LINE_HEIGHT, 180, LINE_HEIGHT), warp, "Always fast travel");
    }

    private const int HEIGHT = 300;
    private const int LINE_HEIGHT = 30;
}
