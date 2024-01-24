using Blasphemous.ModdingAPI.Items;
using Framework.Inventory;
using Framework.Util;
using UnityEngine;

namespace Blasphemous.ModdingAPI.Levels.Modifiers;

public class BaseModifier : IModifier
{
    public void Apply(GameObject obj, ObjectAddition data)
    {
        obj.transform.position = data.Position;
        obj.transform.eulerAngles = data.Rotation;
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
