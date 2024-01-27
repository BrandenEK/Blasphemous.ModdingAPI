using Framework.Managers;

namespace Blasphemous.ModdingAPI.Persistence;

/// <summary>
/// Used to save and load persistent data for a mod
/// </summary>
public abstract class SaveData(string persistentId) : PersistentManager.PersistentData(persistentId) { }
