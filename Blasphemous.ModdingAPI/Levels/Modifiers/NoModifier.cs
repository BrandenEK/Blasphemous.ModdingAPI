using UnityEngine;

namespace Blasphemous.ModdingAPI.Levels.Modifiers;

public class NoModifier : IModifier
{
    private readonly string _name;

    public NoModifier(string name)
    {
        _name = name;
    }

    public void Apply(GameObject obj, ObjectData data)
    {
        obj.name = _name;
    }
}
