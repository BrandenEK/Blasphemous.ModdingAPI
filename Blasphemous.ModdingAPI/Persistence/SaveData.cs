using Framework.Managers;

namespace Blasphemous.ModdingAPI.Persistence;

/// <summary>
/// Used to save and load persistent data for a mod
/// </summary>
[System.Obsolete("Use the new ISlotPersistentMod instead")]
public abstract class SaveData(string persistentId) : PersistentManager.PersistentData(persistentId) { }
