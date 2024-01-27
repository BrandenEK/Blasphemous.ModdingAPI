using UnityEngine;

namespace Blasphemous.ModdingAPI.Levels.Modifiers;

internal class BaseModifier : IModifier
{
    public void Apply(GameObject obj, ObjectData data)
    {
        obj.transform.position = data.position;
        obj.transform.eulerAngles = data.rotation;
        obj.transform.localScale = data.scale;
        obj.SetActive(true);
    }
}
