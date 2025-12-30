
namespace Blasphemous.ModdingAPI.Persistence;

/// <summary>
/// A mod that saves data with the global save file
/// </summary>
public interface IGlobalPersistentMod<TData> where TData : GlobalSaveData
{
    /// <summary>
    /// Saves the global data to an object
    /// </summary>
    public TData SaveGlobal();

    /// <summary>
    /// Loads the global data from an object
    /// </summary>
    public void LoadGlobal(TData data);
}
