# Blasphemous Modding API

## Features

- Enables the console for all registered mods
- Allows loading custom skins
- Ensures compatibility between all mods
- Very simple process of installing new mods

---

## Creating an example mod

1. Create a folder called "ExampleMod"
2. Open the command prompt in this folder and run the command:
```dotnet new bepinex5plugin -n ExampleMod -T net35 -U 2017.4.40```
3. Open the newly created "ExampleMod.csproj" in Visual Studio
4. Go to "Project/Add Assembly Reference/Browse" and add a reference to any required assemblies.  This will include "ModdingAPI.dll", "Assembly-CSharp.dll", "UnityEngine.dll", and likely many more
5. Rename the "Plugin.cs" file to "Main.cs"
6. Create a new class called "Example.cs"
7. Copy the template code into these new files

---

Template "Main.cs" <br>
```cs
using BepInEx;

namespace ExampleMod
{
    [BepInPlugin(MOD_ID, MOD_NAME, MOD_VERSION)]
    [BepInDependency("com.damocles.blasphemous.modding-api", BepInDependency.DependencyFlags.HardDependency)]
    [BepInProcess("Blasphemous.exe")]
    public class Main : BaseUnityPlugin
    {
        public const string MOD_ID = "com.author.blasphemous.example-mod";
        public const string MOD_NAME = "Example";
        public const string MOD_VERSION = "1.0.0";

        public static Example Example;

        private void Start()
        {
            Example = new Example(MOD_ID, MOD_NAME, MOD_VERSION);
        }
    }
}
```

Template "Example.cs" <br>
```cs
using ModdingAPI;

namespace ExampleMod
{
    public class Example : Mod
    {
        public Example(string modId, string modName, string modVersion) : base(modId, modName, modVersion) { }

        protected override void Initialize()
        {
            base.Initialize();
        }
    }
}
```

## Documentation

### Mod

Every mod should derive from this class, as it has many useful features and handles most of the repitive parts of the code.  When calling the the constructor for this class, the modding api automatically registers the mod and patches all harmony functions in its assembly.

#### Virtual functions

```cs
public class Example : Mod
{
    protected override void Initialize()
    {
        // Called when the game is first started up
    }
    
    protected override void Dispose()
    {
        // Called when the game is ended
    }
    
    protected override void Update()
    {
        // Called every frame
    }
    
    protected override void LateUpdate()
    {
        // Called at the end of every frame
    }
    
    protected override void LevelLoaded(string oldLevel, string newLevel)
    {
        // Called right after a scene is loaded
    }
    
    protected override void LevelUnloaded(string oldLevel, string newLevel)
    {
        // Called right before a scene is unloaded
    }
}
```

#### Logging


#### File Utility

Every mod has a public property called FileUtil, which allows the mod to perform various IO related activities, such as loading data or configuration from a file, saving a text document, or converting to/from JSON.

```cs
public class Example : Mod
{
    protected override void Initialize()
    {
        // Load configuration from file
        ExampleConfig config = FileUtil.loadConfig<ExampleConfig>();
        
        // Load array of enemy names from data folder
        if (FileUtil.loadDataArray("enemyNames", out string[] enemyNames))
        {
            Log("Enemy names successfully loaded!");
        }
        else
        {
            LogError("Enemy names could not be loaded!");
        }
    }
}

[System.Serializable]
public class ExampleConfig
{
    public int number;
    public string text;
}
```


