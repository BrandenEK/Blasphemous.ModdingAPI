using Blasphemous.ModdingAPI.Items;
using Framework.Inventory;
using Framework.Util;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public void Apply(GameObject obj, ObjectAddition data);
}

public class BaseModifier : IModifier
{
    public void Apply(GameObject obj, ObjectAddition data)
    {
        obj.transform.position = data.Position;
        obj.transform.localEulerAngles = data.Rotation;
        obj.transform.localScale = data.Scale;
        obj.SetActive(true);
    }
}

public class CollectibleItemModifier : IModifier
{
    public void Apply(GameObject obj, ObjectAddition data)
    {
        UniqueId idComp = obj.GetComponent<UniqueId>();
        idComp.uniqueId = "ITEM-PICKUP-" + data.Id;

        InteractableInvAdd addComp = obj.GetComponent<InteractableInvAdd>();
        addComp.item = data.Id;
        addComp.itemType = ItemModder.GetItemTypeFromId(data.Id);
    }
}

public class ChestModifier : IModifier
{
    public void Apply(GameObject obj, ObjectAddition data)
    {
        UniqueId idComp = obj.GetComponent<UniqueId>();
        idComp.uniqueId = "CHEST-" + data.Id;

        InteractableInvAdd addComp = obj.GetComponent<InteractableInvAdd>();
        addComp.item = data.Id;
        addComp.itemType = ItemModder.GetItemTypeFromId(data.Id);
    }
}

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
