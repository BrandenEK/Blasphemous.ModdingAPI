using System.Collections;
using UnityEngine;

namespace Blasphemous.ModdingAPI.Levels.Loaders;

public interface ILoader
{
    public IEnumerator Apply();

    public GameObject Result { get; }
}
