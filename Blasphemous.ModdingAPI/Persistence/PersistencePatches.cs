using Framework.Managers;
using HarmonyLib;

namespace Blasphemous.ModdingAPI.Persistence;

[HarmonyPatch(typeof(PersistentManager), nameof(PersistentManager.ResetPersistence))]
class PersistentManager_ResetPersistence_Patch
{
    public static void Postfix() => SlotSaveData.Reset();
}

[HarmonyPatch(typeof(PersistentManager), nameof(PersistentManager.SaveGame_Internal))]
class PersistentManager_SaveGame_Internal_Patch
{
    public static void Postfix(int slot) => SlotSaveData.Save(slot);
}

[HarmonyPatch(typeof(PersistentManager), nameof(PersistentManager.LoadGameWithOutRespawn))]
class PersistentManager_LoadGameWithOutRespawn_Patch
{
    public static void Postfix(int slot) => SlotSaveData.Load(slot);
}

[HarmonyPatch(typeof(PersistentManager), nameof(PersistentManager.DeleteSaveGame))]
class PersistentManager_DeleteSaveGame_Patch
{
    public static void Postfix(int slot) => SlotSaveData.Delete(slot);
}
