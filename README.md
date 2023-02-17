# Blasphemous Modding API

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
```C#
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
```C#
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
