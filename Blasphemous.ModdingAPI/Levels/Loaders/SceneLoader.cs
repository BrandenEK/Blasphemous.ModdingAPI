using HarmonyLib;
using System.Collections;
using Tools.Level.Layout;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace Blasphemous.ModdingAPI.Levels.Loaders;

/// <summary>
/// Loads an object by finding it in a scene
/// </summary>
public class SceneLoader : ILoader
{
    private readonly string _scene;
    private readonly string _path;

    /// <summary>
    /// The loaded object
    /// </summary>
    public GameObject Result { get; private set; }

    /// <summary>
    /// Creates a new scene loader
    /// </summary>
    public SceneLoader(string scene, string path)
    {
        _scene = scene;
        _path = path;
    }

    /// <summary>
    /// Coroutine to load the object
    /// </summary>
    public IEnumerator Apply()
    {
        InLoadProcess = true;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(_scene, LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Load the item from this scene
        Scene tempScene = SceneManager.GetSceneByName(_scene);
        GameObject sceneObject = tempScene.FindObject(_path, true);

        if (sceneObject != null)
        {
            Result = Object.Instantiate(sceneObject, Main.Instance.transform);
        }

        yield return null;

        asyncLoad = SceneManager.UnloadSceneAsync(tempScene);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        InLoadProcess = false;
    }

    internal static bool InLoadProcess { get; private set; }
}

// Prevent init functions when loading temp level
[HarmonyPatch(typeof(LevelInitializer), "Awake")]
internal class LevelInit_Patch
{
    public static bool Prefix() => !SceneLoader.InLoadProcess;
}
