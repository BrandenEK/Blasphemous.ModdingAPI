using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace Blasphemous.ModdingAPI.Levels;

/// <summary>
/// Easy methods of locating objects by path in a scene
/// </summary>
public static class LevelExtensions
{
    /// <summary>
    /// Finds an object at the given scene and path, if it exists
    /// </summary>
    public static GameObject FindObject(this Scene scene, string path, bool shouldDisableRoots)
    {
        return scene.FindRoots(shouldDisableRoots).FindObject(path);
    }

    /// <summary>
    /// Finds an object in the given root objects, if it exists
    /// </summary>
    public static GameObject FindObject(this Dictionary<string, Transform> roots, string path)
    {
        string[] transformPath = path.Split('/');

        Transform currTransform = null;
        for (int i = 0; i < transformPath.Length; i++)
        {
            string finder = transformPath[i];
            if (i == 0)
            {
                currTransform = roots.ContainsKey(finder) ? roots[finder] : null;
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
    public static Dictionary<string, Transform> FindRoots(this Scene scene, bool shouldDisableRoots)
    {
        Dictionary<string, Transform> rootObjects = new();
        foreach (GameObject obj in scene.GetRootGameObjects())
        {
            if (obj.name[0] != '=' && !rootObjects.ContainsKey(obj.name))
            {
                rootObjects.Add(obj.name, obj.transform);
                if (shouldDisableRoots)
                    obj.SetActive(false);
            }
        }
        return rootObjects;
    }
}
