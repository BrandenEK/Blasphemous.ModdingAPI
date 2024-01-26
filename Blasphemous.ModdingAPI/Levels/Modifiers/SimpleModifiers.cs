using Blasphemous.ModdingAPI.Items;
using Framework.Inventory;
using Framework.Util;
using UnityEngine;

namespace Blasphemous.ModdingAPI.Levels.Modifiers;

public class BaseModifier : IModifier
{
    public void Apply(GameObject obj, ObjectData data)
    {
        obj.transform.position = data.position;
        obj.transform.eulerAngles = data.rotation;
        obj.transform.localScale = data.scale;
        obj.SetActive(true);
    }
}

public class CollectibleItemModifier : IModifier
{
    public void Apply(GameObject obj, ObjectData data)
    {
        UniqueId idComp = obj.GetComponent<UniqueId>();
        idComp.uniqueId = "ITEM-PICKUP-" + data.id;

        InteractableInvAdd addComp = obj.GetComponent<InteractableInvAdd>();
        addComp.item = data.id;
        addComp.itemType = ItemModder.GetItemTypeFromId(data.id);
    }
}

public class ChestModifier : IModifier
{
    public void Apply(GameObject obj, ObjectData data)
    {
        UniqueId idComp = obj.GetComponent<UniqueId>();
        idComp.uniqueId = "CHEST-" + data.id;

        InteractableInvAdd addComp = obj.GetComponent<InteractableInvAdd>();
        addComp.item = data.id;
        addComp.itemType = ItemModder.GetItemTypeFromId(data.id);
    }
}
