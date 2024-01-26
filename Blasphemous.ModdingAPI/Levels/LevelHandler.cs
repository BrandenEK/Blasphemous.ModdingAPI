using Blasphemous.ModdingAPI.Levels.Modifiers;
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
    private readonly IModifier _baseModifier = new BaseModifier();

    private readonly Dictionary<string, IEnumerable<ObjectAddition>> _additions = new();
    private readonly Dictionary<string, IEnumerable<ObjectDeletion>> _deletions = new();

    private bool _loadedObjects = false;
    private Transform _currentObjectHolder; // Only accessed when adding objects, so always set when loading scene with objects to add

    public void Initialize()
    {
        Main.ModLoader.ProcessModFunction(mod => ProcessModifications(mod.FileHandler.LoadLevels()));
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

    private void ProcessModifications(Dictionary<string, LevelModification> modifications)
    {
        foreach (var modification in modifications)
        {
            if (modification.Value.additions.Length > 0)
            {
                _additions[modification.Key] = _additions.TryGetValue(modification.Key, out var addition)
                    ? addition.Concat(modification.Value.additions)
                    : modification.Value.additions;
            }

            if (modification.Value.deletions.Length > 0)
            {
                _deletions[modification.Key] = _deletions.TryGetValue(modification.Key, out var deletion)
                    ? deletion.Concat(modification.Value.deletions)
                    : modification.Value.deletions;
            }
        }
    }

    private void AddObjects(IEnumerable<ObjectAddition> objects)
    {
        _currentObjectHolder = GameObject.Find("LOGIC").transform;

        foreach (var addition in objects.Where(x => CheckCondition(x.condition)))
        {
            if (!_objects.TryGetValue(addition.type, out GameObject storedObject))
                continue;

            if (!LevelRegister.TryGetModifier(addition.type, out IModifier modifier))
                continue;

            GameObject newObject = Object.Instantiate(storedObject, _currentObjectHolder);
            _baseModifier.Apply(newObject, addition);
            modifier.Apply(newObject, addition);
        }
    }

    private void DeleteObjects(IEnumerable<ObjectDeletion> objects, string level)
    {
        foreach (var deletion in objects.Where(d => CheckCondition(d.condition)).GroupBy(d => d.scene))
        {
            Scene scene = SceneManager.GetSceneByName(level + deletion.Key switch
            {
                "decoration" => "_DECO",
                "layout" => "_LAYOUT",
                "logic" => "_LOGIC",
                _ => throw new System.Exception("Invalid scene type for object deletion: " + deletion.Key)
            });

            DisableObjectGroup(scene, deletion.Select(x => x.path));
        }
    }

    /// <summary>
    /// Deactivates all objects in a scene that exist at a path
    /// </summary>
    private void DisableObjectGroup(Scene scene, IEnumerable<string> disabledObjects)
    {
        // Store dictionary of root objects
        Dictionary<string, Transform> rootObjects = scene.FindRoots(false);

        // Loop through disabled objects and locate & disable them
        foreach (GameObject obj in disabledObjects.Select(rootObjects.FindObject))
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
            // Ensure a loader exists for this type of object
            if (!LevelRegister.TryGetLoader(type, out ILoader loader))
            {
                Main.ModdingAPI.LogError($"There is no creator to handle {type} objects");
                continue;
            }

            // Tell it to load the object somehow
            yield return loader.Apply();
            GameObject loadedObject = loader.Result;

            if (loadedObject == null)
            {
                Main.ModdingAPI.LogError("Failed to load object of type " + type);
                continue;
            }

            // Store it in the dictionary
            Main.ModdingAPI.Log("Successfully loaded object of type " + type);
            loadedObject.name = type;
            loadedObject.transform.position = Vector3.zero;
            loadedObject.SetActive(false);
            _objects.Add(type, loadedObject);
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
        return _additions.Values.SelectMany(x => x).Select(x => x.type).Distinct();
    }
}
