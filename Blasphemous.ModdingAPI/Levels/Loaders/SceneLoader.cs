using HarmonyLib;
using System.Collections;
using Tools.Level.Layout;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace Blasphemous.ModdingAPI.Levels.Loaders;

public class SceneLoader : ILoader
{
    private readonly string _scene;
    private readonly string _path;

    public GameObject Result { get; private set; }

    public static bool InLoadProcess { get; private set; }

    public SceneLoader(string scene, string path)
    {
        _scene = scene;
        _path = path;
    }

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
}

// Prevent init functions when loading temp level
[HarmonyPatch(typeof(LevelInitializer), "Awake")]
internal class LevelInit_Patch
{
    public static bool Prefix() => !SceneLoader.InLoadProcess;
}
