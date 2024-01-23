using Blasphemous.ModdingAPI.Items;
using Framework.Inventory;
using Framework.Util;
using UnityEngine;

namespace Blasphemous.ModdingAPI.Levels;

public interface IModifier
{
    public void Apply(GameObject obj, ObjectAddition data);
}

public class BaseCreator : IModifier
{
    public void Apply(GameObject obj, ObjectAddition data)
    {
        obj.transform.position = data.Position;
        obj.transform.localEulerAngles = data.Rotation;
        obj.transform.localScale = data.Scale;
        obj.SetActive(true);
    }
}

public class CollectibleItemCreator : IModifier
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

public class ChestCreator : IModifier
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

public class ObjectModifier
{
    public string Scene { get; }
    public string Path { get; }
    public IModifier Modifier { get; }

    public ObjectModifier(string scene, string path, IModifier modifier)
    {
        Scene = scene;
        Path = path;
        Modifier = modifier;
    }
}
