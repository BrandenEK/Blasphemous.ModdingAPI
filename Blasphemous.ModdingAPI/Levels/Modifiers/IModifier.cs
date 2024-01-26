using UnityEngine;

namespace Blasphemous.ModdingAPI.Levels.Modifiers;

public interface IModifier
{
    public void Apply(GameObject obj, ObjectData data);
}
