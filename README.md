# Blasphemous Modding API

To create a new project, open the command line and run: <br>
```dotnet new bepinex5plugin -n NewModName -T net35 -U 2017.4.40```

---

Template project file: <br>
```C#
using BepInEx;
using HarmonyLib;

namespace ModName
{
    [BepInPlugin("mod-guid", "mod-name", "1.0.0")]
    [BepInProcess("Blasphemous.exe")]
    public class Main : BaseUnityPlugin
    {
        public static ModName modName;
        private static Main instance;

        private void Awake()
        {
            modName = new ModName();
            instance = this;
            Patch();
        }
        
        private void Update()
        {
            modName.update()
        }

        private void Patch()
        {
            Harmony harmony = new Harmony("mod-guid");
            harmony.PatchAll();
        }

        private void UnityLog(string message)
        {
            Logger.LogMessage(message);
        }

        public static void Log(string message)
        {
            instance.Log(message);
        }
    }
}
```

### To-do:
- Add fileutil class
- Add mod interface (Awake, Init, Dispose, Update, etc.)
