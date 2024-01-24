using Framework.Managers;
using Framework.Penitences;
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
    private Transform _currentObjectHolder; // Only accessed when adding objects, so always set when loading scene with objects to add

    public bool InLoadProcess { get; private set; }

    public void LoadLevelEdits()
    {
        // Done near initialization:
        // Loads all json files and parses the disabled and added objects by scene
    }

    public void PreloadLevel(string level)
    {
        bool hasAdditions = _additions.TryGetValue(level, out var additions);
        bool hasDeletions = _deletions.TryGetValue(level, out var deletions);

        if (!hasAdditions && !hasDeletions) return;
        Main.ModdingAPI.Log("Applying level modifications for " + level);

        if (hasAdditions)
            AddObjects(additions);
        if (hasDeletions)
            DeleteObjects(deletions, level);
    }

    public void LoadLevel(string level)
    {
        if (!_loadedObjects && level == "MainMenu")
        {
            _loadedObjects = true;
            Main.Instance.StartCoroutine(LoadNecessaryObjects());
        }
    }

    private void AddObjects(IEnumerable<ObjectAddition> objects)
    {
        _currentObjectHolder = GameObject.Find("LOGIC").transform;

        foreach (var addition in objects.Where(x => CheckCondition(x.Condition)))
        {
            if (!_objects.TryGetValue(addition.Type, out GameObject storedObject))
                continue;

            if (!LevelRegister.TryGetModifier(addition.Type, out ObjectModifier modifier))
                continue;

            GameObject newObject = Object.Instantiate(storedObject, _currentObjectHolder);
            _baseModifier.Apply(newObject, addition);
            modifier.Modifier.Apply(newObject, addition);
        }
    }

    private void DeleteObjects(IEnumerable<ObjectDeletion> objects, string level)
    {
        foreach (var deletion in objects.Where(d => CheckCondition(d.Condition)).GroupBy(d => d.Scene))
        {
            Scene scene = SceneManager.GetSceneByName(level + deletion.Key switch
            {
                "decoration" => "_DECO",
                "layout" => "_LAYOUT",
                "logic" => "_LOGIC",
                _ => throw new System.Exception("Invalid scene type for object deletion: " + deletion.Key)
            });

            DisableObjectGroup(scene, deletion.Select(x => x.Path));
        }
    }

    /// <summary>
    /// Deactivates all objects in a scene that exist at a path
    /// </summary>
    private void DisableObjectGroup(Scene scene, IEnumerable<string> disabledObjects)
    {
        // Store dictionary of root objects
        Dictionary<string, Transform> rootObjects = FindRootObjectsInScene(scene, false);

        // Loop through disabled objects and locate & disable them
        foreach (GameObject obj in disabledObjects.Select(x => FindObjectInScene(rootObjects, x)))
        {
            if (obj != null)
                obj.SetActive(false);
        }
    }

    /// <summary>
    /// Checks whether a modification's condition is met
    /// </summary>
    private bool CheckCondition(string condition)
    {
        if (condition == null)
            return true;

        int colon = condition.IndexOf(':');
        string conditionType = condition.Substring(0, colon);
        string conditionValue = condition.Substring(colon + 1);

        if (conditionType == "flag")
        {
            return Core.Events.GetFlag(conditionValue);
        }

        if (conditionType == "penitence")
        {
            IPenitence penitence = Core.PenitenceManager.GetCurrentPenitence();
            return penitence != null && penitence.Id == conditionValue;
        }

        return true;
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
        _additions.Add("D01Z02S01", new List<ObjectAddition>()
        {
            new ObjectAddition("chest-iron", "RB501", new Vector(), new Vector(), new Vector(), null, null),
            new ObjectAddition("platform", "RB501", new Vector(), new Vector(), new Vector(), null, null),
            new ObjectAddition("chest-iron", "RB501", new Vector(), new Vector(), new Vector(), null, null),
        });
        _additions.Add("D01Z02S02", new List<ObjectAddition>()
        {
            new ObjectAddition("chest-iron", "RB501", new Vector(), new Vector(), new Vector(), null, null),
            new ObjectAddition("platform", "RB501", new Vector(), new Vector(), new Vector(), null, null),
            new ObjectAddition("chest-relic", "RB501", new Vector(), new Vector(), new Vector(), null, null),
        });
        _deletions.Add("D01Z02S01", new List<ObjectDeletion>()
        {
            new ObjectDeletion("logic", "fake/path", null),
            new ObjectDeletion("logic", "fake/path2", null),
            new ObjectDeletion("decoration", "fake/path3", null),
        });
    }
}
