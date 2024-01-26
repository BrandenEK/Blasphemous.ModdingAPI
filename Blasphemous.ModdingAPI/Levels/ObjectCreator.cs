using System.Collections;
using UnityEngine;

namespace Blasphemous.ModdingAPI.Levels;

public class ObjectCreator
{
    public ILoader Loader { get; }
    public IModifier Modifier { get; }

    public ObjectCreator(ILoader loader, IModifier modifier)
    {
        Loader = loader;
        Modifier = modifier;
    }
}

public interface ILoader
{
    public IEnumerator Apply();

    public GameObject Result { get; }
}

public interface IModifier
{
    public void Apply(GameObject obj, ObjectData data);
}
