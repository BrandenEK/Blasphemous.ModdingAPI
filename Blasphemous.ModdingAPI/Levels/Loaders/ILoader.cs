using System.Collections;
using UnityEngine;

namespace Blasphemous.ModdingAPI.Levels.Loaders;

/// <summary>
/// Interface for loading objects for the level editor
/// </summary>
public interface ILoader
{
    /// <summary>
    /// Coroutine to load the object
    /// </summary>
    public IEnumerator Apply();

    /// <summary>
    /// The loaded object
    /// </summary>
    public GameObject Result { get; }
}
