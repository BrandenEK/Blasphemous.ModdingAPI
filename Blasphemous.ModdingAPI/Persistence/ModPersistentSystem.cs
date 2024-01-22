using Framework.FrameworkCore;
using Framework.Managers;

namespace Blasphemous.ModdingAPI.Persistence;

internal class ModPersistentSystem(IPersistentMod mod) : PersistentInterface
{
    public PersistentManager.PersistentData GetCurrentPersistentState(string dataPath, bool fullSave) =>
        mod.SaveGame();

    public void SetCurrentPersistentState(PersistentManager.PersistentData data, bool isloading, string dataPath) =>
        mod.LoadGame((SaveData)data);

    public void ResetPersistence() =>
        mod.ResetGame();

    public int GetOrder() => 0;

    public string GetPersistenID() =>
        mod.PersistentID;
}
