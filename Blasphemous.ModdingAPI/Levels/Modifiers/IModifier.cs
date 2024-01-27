using UnityEngine;

namespace Blasphemous.ModdingAPI.Levels.Modifiers;

/// <summary>
/// Interface for modifying objects for the level editor
/// </summary>
public interface IModifier
{
    /// <summary>
    /// Applies the properties in the data to the loaded object
    /// </summary>
    public void Apply(GameObject obj, ObjectData data);
}
