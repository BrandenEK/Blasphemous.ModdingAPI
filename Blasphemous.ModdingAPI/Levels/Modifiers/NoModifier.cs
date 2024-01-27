using UnityEngine;

namespace Blasphemous.ModdingAPI.Levels.Modifiers;

/// <summary>
/// Only sets the name of an object
/// </summary>
public class NoModifier : IModifier
{
    private readonly string _name;

    /// <summary>
    /// Creates a new modifier with the specified name
    /// </summary>
    /// <param name="name"></param>
    public NoModifier(string name)
    {
        _name = name;
    }

    /// <summary>
    /// Only sets the name of an object
    /// </summary>
    public void Apply(GameObject obj, ObjectData data)
    {
        obj.name = _name;
    }
}
