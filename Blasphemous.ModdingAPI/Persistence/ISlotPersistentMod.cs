
namespace Blasphemous.ModdingAPI.Persistence;

/// <summary>
/// A mod that saves data with a slot's save file
/// </summary>
public interface ISlotPersistentMod<TData> where TData : SlotSaveData
{
    /// <summary>
    /// Saves the slot data to an object
    /// </summary>
    public TData SaveSlot();

    /// <summary>
    /// Loads the slot data from an object
    /// </summary>
    public void LoadSlot(TData data);

    /// <summary>
    /// Resets the slot data
    /// </summary>
    public void ResetSlot();
}
