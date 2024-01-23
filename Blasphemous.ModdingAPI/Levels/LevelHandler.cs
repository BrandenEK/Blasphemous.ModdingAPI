using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Blasphemous.ModdingAPI.Levels;

internal class LevelHandler
{
    private readonly Dictionary<string, GameObject> _objects = new();
    private readonly IModifier _baseModifier = new BaseCreator();

    private readonly Dictionary<string, IEnumerable<ObjectAddition>> _additions = new();
    private readonly Dictionary<string, IEnumerable<ObjectDeletion>> _deletions = new();

    private bool _loadedObjects = false;

    public bool InLoadProcess { get; private set; }

    private GameObject GetObjectOfType(string type)
    {
        return _objects.TryGetValue(type, out var obj) ? obj : throw new System.Exception($"Object {type} has not been loaded");
    }

    public void LoadLevelEdits()
    {

    }

    public void LoadLevel(string level)
    {
        if (!_loadedObjects && level == "MainMenu")
        {
            _loadedObjects = true;
            Main.Instance.StartCoroutine(LoadNecessaryObjects());
        }
    }

    /// <summary>
    /// When loading the main menu for the first time, async load all necessary objects from their scenes
    /// </summary>
    private IEnumerator LoadNecessaryObjects()
    {
        foreach (var type in GetNecessaryObjects())
        {
            if (!LevelRegister.TryGetModifier(type, out var modifier))
            {
                Main.ModdingAPI.LogError($"There is no modifier to handle {type} objects");
                continue;
            }

            yield return Main.Instance.StartCoroutine(LoadSceneForObject(type, modifier.Scene, modifier.Path));
        }

        // Fix camera after scene loads
        Camera.main.transform.position = new Vector3(0, 0, -10);
        Camera.main.backgroundColor = new Color(0, 0, 0, 1);
        yield return null;
    }

    /// <summary>
    /// Gets a list of all object types required by mods
    /// </summary>
    private IEnumerable<string> GetNecessaryObjects()
    {
        return _additions.Values.SelectMany(x => x).Select(x => x.Type).Distinct();
    }

    /// <summary>
    /// Loads a scene async and adds the object to the dictionary if it is found
    /// </summary>
    private IEnumerator LoadSceneForObject(string type, string scene, string objectPath)
    {
        InLoadProcess = true;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Load the item from this scene
        Scene tempScene = SceneManager.GetSceneByName(scene);
        GameObject sceneObject = FindObjectInScene(tempScene, objectPath, true);

        if (sceneObject == null)
        {
            Main.ModdingAPI.LogError($"Failed to load {objectPath} from {scene}");
        }
        else
        {
            GameObject obj = Object.Instantiate(sceneObject, Main.Instance.transform);
            obj.name = type;
            obj.transform.position = Vector3.zero;
            obj.SetActive(false);
            _objects.Add(type, obj);
            Main.ModdingAPI.Log($"Loaded {objectPath} from {scene}");
        }

        yield return null;

        asyncLoad = SceneManager.UnloadSceneAsync(tempScene);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        InLoadProcess = false;
    }

    /// <summary>
    /// Finds an object at the given scene and path, if it exists
    /// </summary>
    private GameObject FindObjectInScene(Scene scene, string objectPath, bool disableRoots)
    {
        return FindObjectInScene(FindRootObjectsInScene(scene, disableRoots), objectPath);
    }

    /// <summary>
    /// Finds an object in the given root objects, if it exists
    /// </summary>
    private GameObject FindObjectInScene(Dictionary<string, Transform> rootObjects, string objectPath)
    {
        string[] transformPath = objectPath.Split('/');

        Transform currTransform = null;
        for (int i = 0; i < transformPath.Length; i++)
        {
            string finder = transformPath[i];
            if (i == 0)
            {
                currTransform = rootObjects.ContainsKey(finder) ? rootObjects[finder] : null;
            }
            else if (finder.Length >= 3 && finder[0] == '{' && finder[finder.Length - 1] == '}')
            {
                int childIdx = int.Parse(finder.Substring(1, finder.Length - 2));
                currTransform = currTransform.childCount > childIdx ? currTransform.GetChild(childIdx) : null;
            }
            else
            {
                currTransform = currTransform.Find(finder);
            }

            if (currTransform == null)
                break;
        }

        return currTransform?.gameObject;
    }

    /// <summary>
    /// Finds all root objects in the given scene
    /// </summary>
    private Dictionary<string, Transform> FindRootObjectsInScene(Scene scene, bool disableRoots)
    {
        Dictionary<string, Transform> rootObjects = new();
        foreach (GameObject obj in scene.GetRootGameObjects())
        {
            if (obj.name[0] != '=' && !rootObjects.ContainsKey(obj.name))
            {
                rootObjects.Add(obj.name, obj.transform);
                if (disableRoots)
                    obj.SetActive(false);
            }
        }
        return rootObjects;
    }

    public LevelHandler()
    {
        _additions.Add("1", new List<ObjectAddition>()
        {
            new ObjectAddition("chest-iron", "RB501", new Vector(), new Vector(), new Vector(), null, null),
            new ObjectAddition("platform", "RB501", new Vector(), new Vector(), new Vector(), null, null),
            new ObjectAddition("chest-iron", "RB501", new Vector(), new Vector(), new Vector(), null, null),
        });
        _additions.Add("2", new List<ObjectAddition>()
        {
            new ObjectAddition("chest-iron", "RB501", new Vector(), new Vector(), new Vector(), null, null),
            new ObjectAddition("platform", "RB501", new Vector(), new Vector(), new Vector(), null, null),
            new ObjectAddition("chest-relic", "RB501", new Vector(), new Vector(), new Vector(), null, null),
        });
    }
}
