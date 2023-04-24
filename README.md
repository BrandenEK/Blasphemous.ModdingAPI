# Blasphemous Modding API
![Release version](https://img.shields.io/github/v/release/BrandenEK/Blasphemous-Modding-API)
![Last commit](https://img.shields.io/github/last-commit/BrandenEK/Blasphemous-Modding-API?color=important)
![Downloads](https://img.shields.io/github/downloads/BrandenEK/Blasphemous-Modding-API/total?color=success)

## Table of Contents

- [Features](https://github.com/BrandenEK/Blasphemous-Modding-API#features)
- [How to use](https://github.com/BrandenEK/Blasphemous-Modding-API#how-to-use)
  - [Installation](https://github.com/BrandenEK/Blasphemous-Modding-API#installation)
  - [Usage](https://github.com/BrandenEK/Blasphemous-Modding-API#usage)
  - [Custom skins](https://github.com/BrandenEK/Blasphemous-Modding-API#custom-skins)
  - [Save Compatibility](https://github.com/BrandenEK/Blasphemous-Modding-API#save-compatibility)
- [Translations](https://github.com/BrandenEK/Blasphemous-Modding-API#translations)

<br>

- [Creating a mod](https://github.com/BrandenEK/Blasphemous-Modding-API#creating-an-example-mod)
  - [Project Setup](https://github.com/BrandenEK/Blasphemous-Modding-API#project-setup)
  - [Exporting](https://github.com/BrandenEK/Blasphemous-Modding-API#exporting)
- [Documentation](https://github.com/BrandenEK/Blasphemous-Modding-API#documentation)
  - [The Mod Class](https://github.com/BrandenEK/Blasphemous-Modding-API#the-mod-class)
  - [Logging](https://github.com/BrandenEK/Blasphemous-Modding-API#logging)
  - [File Utility](https://github.com/BrandenEK/Blasphemous-Modding-API#file-utility)
  - [Localization](https://github.com/BrandenEK/Blasphemous-Modding-API#localization)
  - [Persistent Data](https://github.com/BrandenEK/Blasphemous-Modding-API#persistent-data)
  - [Commands](https://github.com/BrandenEK/Blasphemous-Modding-API#commands)
  - [Custom Penitences](https://github.com/BrandenEK/Blasphemous-Modding-API#custom-penitences)
  - [Custom Items](https://github.com/BrandenEK/Blasphemous-Modding-API#custom-items)
  - [Level Modifications](https://github.com/BrandenEK/Blasphemous-Modding-API#level-modifications)
  - [Harmony Patching](https://github.com/BrandenEK/Blasphemous-Modding-API#harmony-patching)
- [Examples](https://github.com/BrandenEK/Blasphemous-Modding-API#examples)

---

## Features

- Enables the console for all registered mods
- Allows loading custom skins
- Ensures compatibility between all mods
- Adds support for custom penitences and items
- Very simple process of installing new mods

---

## How to use

### Installation
**Note:** *If you have used other mods before without the API, make sure to delete the BepInEx folder and restore any modified files before installing the modding api!*

1. Navigate to the game's root directory, which should be in 
```C:\Program Files (x86)\Steam\steamapps\common\Blasphemous```
    - (Optional) Make a duplicate of this folder for playing Blasphemous mods, so that the original game files remain unchanged
4. Download the latest release of the Modding API from the [Releases](https://github.com/BrandenEK/Blasphemous-Modding-API/releases) page
5. Extract the contents of the ModdingAPI.zip file into the blasphemous root folder
6. Verify that a new folder called "Modding" now exists in the same folder as "Blasphemous.exe"

### Usage

- Press 'backslash' at any time to open the debug console and enter commands
- All registered mods should be displayed in the top right corner of the main menu
- Mods can be disabled by simply moving them out of the "Modding/plugins" folder

### Custom skins

- Skins can be downloaded from https://github.com/BrandenEK/Blasphemous-Custom-Skins
- Extract the contents of each skin's zip file into the "Modding/skins" folder.  There should be one folder for each skin in the "Modding/skins" folder

### Save Compatibility

- Make sure to back up any save files you care about before installing mods
- To prevent save corruption, don't load a save file with different mods installed than were present when the save file was created
- If corruption does happen, strange effects may take place, such as rosary beads being randomly equipped.  Removing all existing save files should fix this problem

---

## Translations

The Modding API is available in these other languages in addition to English:
- Spanish (Thanks to Rodol J. "ConanCimmerio" Pérez (Ro))
- Chinese (Thanks to NewbieElton)
- French  (Thanks to Rocher)

---

<br>
<br>

## Creating an example mod

### Project Setup

1. Create a folder called "ExampleMod"
2. Open the command prompt in this folder and run these two commands: <br>
```dotnet new -i BepInEx.Templates --nuget-source https://nuget.bepinex.dev/v3/index.json``` <br>
```dotnet new bepinex5plugin -n ExampleMod -T net35 -U 2017.4.40```
3. Open the newly created "ExampleMod.csproj" in Visual Studio
4. Go to "Project > Add Assembly Reference > Browse" and add a reference to any required assemblies.  This will include "ModdingAPI.dll", "BepInEx.dll", "Assembly-CSharp.dll", "UnityEngine.dll", and likely many more
5. Download the "ModdingAPI.xml file from the source code and place it in the same folder as the required assemblies
6. Rename the "Plugin.cs" file to "Main.cs"
7. Create a new file called "Example.cs" that contains a class that derives from Mod
8. Follow the [Examples](https://github.com/BrandenEK/Blasphemous-Modding-API#examples) section on how to program the mod

### Exporting

Each mod must export the ExampleMod.dll, and it can also export any config files, doc files, data files, localization files, and required dll files.  The exported zip file should follow this file format so that it can be extracted into the "Modding" folder.

```
ExampleMod.zip
├── config
│   └── ExampleMod.cfg
├── data
│   ├── ExampleMod
│   │   ├── dataFileOne.dat
│   │   └── dataFileTwo.dat
│   └── RequiredDLL.dll
├── docs
│   └── ExampleMod.txt
├── levels
│   ├── D04Z02S01.json
│   └── D17Z01S02.json
├── localization
│   └── ExampleMod.txt
└── plugins
    └── ExampleMod.dll
```

---

## Documentation

### The Mod Class

Every mod should derive from this class, as it has many useful features and handles most of the repitive parts of the code.  When calling the constructor for this class, the modding api automatically registers the mod and patches all harmony functions in its assembly.

Mod class structure:
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

The mod class also has a public property for checking player input.
```cs
protected override void Update()
{
    if (Input.GetButtonDown(InputHandler.ButtonCode.Jump))
    {
        // Executes whenever the jump button is pressed	
    }
}
```

The mod class also has a public method for checking whether another mod is installed or not.
```cs
protected override void Initialize()
{
    if (IsModLoaded("com.damocles.blasphemous.randomizer"))
        LogWarning("The randomizer mod is active!");
}
```

### Logging

Every mod has public methods for logging messages to the unity console.  There is a separate one for logging messages, warnings, and errors.  You can also display text directly to the user with the LogDisplay method.

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
		
        LogDisplay("You have entered a new scene: " + newLevel);
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
    }
}

[System.Serializable]
public class ExampleConfig
{
    public int numberSetting;
    public string textSetting;
}
```

### Localization

To translate your mod into different languages, you just have to include a text file in the localization directory.  In the code for the mod, instead of directly referencing a string, you can instead call the main mod's Localize method to convert a key term into whatever the game's current language is.

Mod class:
```cs
protected override void LevelLoaded(string oldLevel, string newLevel)
{
    if (newLevel == "D08Z01S01")
    {
        LogDisplay(Localize("bridge"));
    }
}
```

Localization/ModName.txt:
```
lang: en
bridge: English text

lang: es
bridge: Spanish text

lang: de
bridge: German text
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

    public abstract void NewGame(bool NGPlus)
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

### Custom Penitences

If you want to add a custom penitence that can be chosen from the penitence selector, all you have to do is fill out its data and register it in the mod's Initialize function.  Generally you will just want to set a flag in its Activate/Deactivate methods and handle the functionality elsewhere.

Penitence class:
```cs
using ModdingAPI.Penitences

public class PenitenceExample : ModPenitence
{
    // The unique id of the penitence - Should start with "PE_"
    protected override string Id => "PE_EXAMPLE";

    // The display name of the penitence
    protected override string Name => "Example Name";

    // The description of the penitence
    protected override string Description => "Example Description";

    // The item to give when completing the penitence, or null if no item should be given
    protected override string ItemIdToGive => "RB999";

    // The type of the item to give for completing the penitence
    protected override InventoryManager.ItemType ItemTypeToGive => InventoryManager.ItemType.Bead;

    protected override void Activate()
    {
        // Set flag the penitence is active
    }

    protected override void Deactivate()
    {
        // Unset flag that the penitence is active
    }

    protected override void LoadImages(out Sprite inProgress, out Sprite completed, out Sprite abandoned, out Sprite gameplay, out Sprite chooseSelected, out Sprite chooseUnselected)
    {
        // Load all of the images using the FileUtil and set them here
    }
}
```

Mod class:
```cs
protected override void Initialize()
{
    RegisterPenitence(new PenitenceExample());
}
```

### Custom Items

To add a custom item into the game, all you have to do is fill out its data and register it in the mod's Initialize function.  However, to make the item actually do something when equipped or used, you will also want to add an item effect to it.

There are a few different methods of giving the player the custom item, you can either set the CarryOnStart property to true, add an interactable item pickup to a scene using the level editor, or use the ItemModder class to give and display the item.

Item class:
```cs
using ModdingAPI.Items

public class BeadExample : ModRosaryBead
{
    // The unique id of the item.  Must start with the appropriate prefix followed by a number
    protected override string Id => "RB999";

    // The name of the item
    protected override string Name => "Example Bead";

    // The description of the item
    protected override string Description => "Example description";

    // The lore of the item
    protected override string Lore => "Example lore";

    // Whether or not the item should be kept when moving to NG+
    protected override bool PreserveInNGPlus => true;

    // Whether or not the item will add percent completion to the save file
    protected override bool AddToPercentCompletion => false;

    // Whether or not an extra item slot should be added to the inventory for this item
    protected override bool AddInventorySlot => true;

    // Whether or not the item should be given when starting a new save file
    protected override bool CarryOnStart => false;

    protected override void LoadImages(out Sprite picture)
    {
        // Load the item image using the FileUtil and set it here
    }
}
```

Item Effect class:
```cs
using ModdingAPI.Items;

public class ExampleEffect : ModItemEffectOnEquip
{
    protected override void ApplyEffect()
    {
        // Set the active flag to true
    }

    protected override void RemoveEffect()
    {
        // Set the active flag to false
    }
}
```

Mod class:
```cs
protected override void Initialize()
{
    RegisterItem(new BeadExample().AddEffect<ExampleEffect>());
}
```

### Level Modifications

Level modifications, including removing, changing, or adding objects to/from a scene can be accomplished by creating a level file and placing it in the levels directory.  Currently, the only objects that can be added are CollectibleItems, Chests, and Spikes; however, any object already in a scene can be easily disabled.

levels/D17Z01S02.json:
```
{
    "AddedObjects":
    [
        {
            "Type": "Chest",
            "Id": "PR999",
            "XPos": -923.5,
            "YPos": 6
        }
    ]
}
```

### Harmony Patching

Most mods will want to patch game functions with harmony.  The actual patching is already handled by the modding api, so all the mod has to do is create the patch functions.  Refer to the harmony docs on how to implement patches: https://harmony.pardeike.net/articles/patching.html

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
    [BepInDependency("com.damocles.blasphemous.modding-api", "1.3.0")]
    [BepInProcess("Blasphemous.exe")]
    public class Main : BaseUnityPlugin
    {
        public const string MOD_ID = "com.author.blasphemous.example-mod";
        public const string MOD_NAME = "Example";
        public const string MOD_VERSION = "1.0.0";

        public static Example Example { get; private set; }

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
    
        protected override void Initialize()
        {
            Log("Example Mod has been initialized!");
        }
    }
}
```
