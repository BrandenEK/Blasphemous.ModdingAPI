using BepInEx;
using HarmonyLib;

namespace ModdingAPI
{
    [BepInPlugin("com.damocles.blasphemous.modding-api", "Modding API", "1.0.0")]
    public class Main : BaseUnityPlugin
    {
        private void Awake()
        {
            Harmony harmony = new Harmony("com.damocles.blasphemous.modding-api");
            harmony.PatchAll();
        }
    }
}
