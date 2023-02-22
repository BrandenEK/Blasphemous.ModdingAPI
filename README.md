# Blasphemous Modding API

## Features

- Enables the console for all registered mods
- Allows loading custom skins
- Ensures compatibility between all mods
- Very simple process of installing new mods

---

## Table of Contents

[How to use]()
- [Installation](https://github.com/BrandenEK/Blasphemous-Modding-API#installation)
- [Usage](https://github.com/BrandenEK/Blasphemous-Modding-API#usage)
- [Custom skins](https://github.com/BrandenEK/Blasphemous-Modding-API#custom-skins)

[Creating a mod](https://github.com/BrandenEK/Blasphemous-Modding-API#creating-an-example-mod)

[Documentation](https://github.com/BrandenEK/Blasphemous-Modding-API#documentation)
- [The Mod Class](https://github.com/BrandenEK/Blasphemous-Modding-API#the-mod-class)
- [Logging](https://github.com/BrandenEK/Blasphemous-Modding-API#logging)
- [File Utility](https://github.com/BrandenEK/Blasphemous-Modding-API#file-utility)
- [Persistent Data](https://github.com/BrandenEK/Blasphemous-Modding-API#persistent-data)
- [Commands](https://github.com/BrandenEK/Blasphemous-Modding-API#commands)
- [Harmony Patching](https://github.com/BrandenEK/Blasphemous-Modding-API#harmony-patching)

[Examples](https://github.com/BrandenEK/Blasphemous-Modding-API#examples)

---

## How to use

### Installation

1. Navigate to the game's root directory, which should be in 
```C:\Program Files (x86)\Steam\steamapps\common\Blasphemous```
2. Copy everything in this folder (There should be five items) to a new location, to be used specifically for modding
3. Name this folder something unique, such as "Blasphemous Modded"
4. Download the latest release of the Modding API from the releases page
5. Extract the contents of the zip file into the new folder that you just created
6. Verify that a new folder called "Modding" now exists same folder as "Blasphemous.exe"

### Usage

- Press 'backslash' at any time to open the debug console and enter commands
- All registered mods should be displayed in the top right corner of the main menu

### Custom skins

- Extract the contents of each skin's zip file into the Modding/skins folder.  There should be one folder for each skin in the Modding/skins folder

---

## Creating an example mod

1. Create a folder called "ExampleMod"
2. Open the command prompt in this folder and run the command:
```dotnet new bepinex5plugin -n ExampleMod -T net35 -U 2017.4.40```
3. Open the newly created "ExampleMod.csproj" in Visual Studio
4. Go to "Project/Add Assembly Reference/Browse" and add a reference to any required assemblies.  This will include "ModdingAPI.dll", "BepInEx.dll", "Assembly-CSharp.dll", "UnityEngine.dll", and likely many more
5. Rename the "Plugin.cs" file to "Main.cs"
6. Create a new file called "Example.cs" that contains a class that derives from Mod

---

## Documentation

### The Mod Class

Every mod should derive from this class, as it has many useful features and handles most of the repitive parts of the code.  When calling the the constructor for this class, the modding api automatically registers the mod and patches all harmony functions in its assembly.

```cs
public class Example : Mod
{
    public Example(string modId, string modName, string modVersion) : base(modId, modName, modVersion) { }

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

### Logging

Every mod has public methods for logging messages to the unity console.  There is a separate one for logging messages, warnings, and errors.

```cs
public class Example : Mod
{
    public Example(string modId, string modName, string modVersion) : base(modId, modName, modVersion) { }
    
    protected override void LevelLoaded(string oldLevel, string newLevel)
    {
        if (newLevel == "D17Z01S01")
        {
            Log("This is a message");
        }
        else if (newLevel == "D01Z02S01")
        {
            LogWarning("This is a warning");
        }
        else
        {
            LogError("This is an error");
        }
    }
}
```

### File Utility

Every mod has a public property called FileUtil, which allows the mod to perform various IO related activities, such as loading data or configuration from a file, saving a text document, or converting to/from JSON.

```cs
public class Example : Mod
{
    public Example(string modId, string modName, string modVersion) : base(modId, modName, modVersion) { }
    
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
        
        // Write to the log file
        FileUtil.appendLog("Initialization complete");
    }
}

[System.Serializable]
public class ExampleConfig
{
    public int numberSetting;
    public string textSetting;
}
```

### Persistent Data

Some mods need to save data with the save file, and that can be accomplished by deriving from the PersistentMod class.  This class is dervied from the base Mod class and contains additional methods for saving and loading through a ModPersistentData object.

```cs
public class Example : PersistentMod
{
    public Example(string modId, string modName, string modVersion) : base(modId, modName, modVersion) { }
    
    public abstract string PersistentID => "ID_EXAMPLE";
    
    public override ModPersistentData SaveGame()
    {
        // Called whenever game data is saved
    }
    
    public override void LoadGame(ModPersistentData data)
    {
        // Called whenever game data is loaded
    }
    
    public abstract void ResetGame()
    {
        // Called whenever game data is reset
    }

    public abstract void NewGame()
    {
        // Called whenever a new save game is started
    }
}

[System.Serializable]
public class ExamplePersistentData : ModPersistentData
{
    public ExamplePersistentData() : base("ID_EXAMPLE") { }
    
    public int numberToSave;
    public string textToSave;
}
```

### Commands

Some mods may want to add a command that can be executed through the debug console.  This is done by creating a class that derives from ModCommand and registering the command in the main Mod class.

```cs
public class Example : Mod
{
    public Example(string modId, string modName, string modVersion) : base(modId, modName, modVersion) { }
    
    protected override void Initialize()
    {
        RegisterCommand(new ExampleCommand());
    }
}

public class ExampleCommand : ModCommand
{
    protected override string CommandName => "example";

    protected override bool AllowUppercase => false;

    protected override Dictionary<string, Action<string[]>> AddSubCommands()
    {
        return new Dictionary<string, Action<string[]>>()
        {
            { "help", Help },
            { "status", Status },
            { "test", Test },
            { "use", Use }
        };
    }

    private void Help(string[] parameters)
    {
        if (!ValidateParameterList(parameters, 0)) return;

        Write("Available EXAMPLE commands:");
        Write("example status: Display status");
        Write("example test: Test something");
        Write("example use ID: Use something with the given id");
    }
    
    private void Status(string[] parameters)
    {
        if (!ValidateParameterList(parameters, 0)) return;
        
        // Display status
    }
    
    private void Test(string[] parameters)
    {
        if (!ValidateParameterList(parameters, 0)) return;
        
        // Test something
    }
    
    private void Use(string[] parameters)
    {
        if (!ValidateParameterList(parameters, 1) || !ValidateIntParameter(parameters[0], 1, 100, out int objectId)) return;
        
        // Use object with objectId
    }
}
```

### Harmony Patching

Most mods will want to patch game functions with harmony.  The actual patching is already handled by the modding api, so all the mod has to do is create the patch functions.  Refer to the harmony docs on how to implement patches: ***Link to harmony***.

## Examples

- Main class
- Mod class
- Persistent mod
- Command
- File util functions
- Harmony patches

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
    public class Example : PersistentMod
    {
        public Example(string modId, string modName, string modVersion) : base(modId, modName, modVersion) { }
		
	public abstract string PersistentID => "ID_EXAMPLE";

        protected override void Initialize()
        {
            base.Initialize();
        }
		
	public override ModPersistentData SaveGame()
	{
		return new ExamplePersistentData()
		{
			numberToSave = 5,
			textToSave = "some text"
		};
	}
		
	public override void LoadGame(ModPersistentData data)
	{
		ExamplePersistentData exampleData = (ExamplePersistentData)data;
		*** Do something with values ***
	}
		
	public abstract void ResetGame()
	{
		numberToSave = 0;
		textToSave = "";
	}

	public abstract void NewGame() { }
    }
	
    [System.Serializable]
    public class ExamplePersistentData : ModPersistentData
    {
        public ExamplePersistentData() : base("ID_EXAMPLE") { }
	
	public int numberToSave;
	public string textToSave;
    }
}
```
