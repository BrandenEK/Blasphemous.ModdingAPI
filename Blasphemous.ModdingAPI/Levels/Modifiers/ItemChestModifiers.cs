using Blasphemous.ModdingAPI.Items;
using Framework.Inventory;
using Framework.Util;
using UnityEngine;

namespace Blasphemous.ModdingAPI.Levels.Modifiers;

public class GroundItemModifier : IModifier
{
    public void Apply(GameObject obj, ObjectData data)
    {
        obj.name = $"Ground item {data.id}";

        UniqueId idComp = obj.GetComponent<UniqueId>();
        idComp.uniqueId = "ITEM-GROUND-" + data.id;

        InteractableInvAdd addComp = obj.GetComponent<InteractableInvAdd>();
        addComp.item = data.id;
        addComp.itemType = ItemModder.GetItemTypeFromId(data.id);
    }
}

public class ShrineItemModifier : IModifier
{
    public void Apply(GameObject obj, ObjectData data)
    {
        obj.name = $"Shrine item {data.id}";

        GameObject item = obj.transform.GetChild(0).gameObject;
        item.transform.localPosition = new Vector3(1, 1, 0);

        UniqueId idComp = item.GetComponent<UniqueId>();
        idComp.uniqueId = "ITEM-SHRINE-" + data.id;

        InteractableInvAdd addComp = item.GetComponent<InteractableInvAdd>();
        addComp.item = data.id;
        addComp.itemType = Framework.Managers.InventoryManager.ItemType.Sword;

        GameObject shrine = obj.transform.GetChild(1).gameObject;
        shrine.transform.localPosition = Vector3.zero;
    }
}

public class ChestModifier : IModifier
{
    public void Apply(GameObject obj, ObjectData data)
    {
        obj.name = $"Chest {data.id}";

        UniqueId idComp = obj.GetComponent<UniqueId>();
        idComp.uniqueId = "CHEST-" + data.id;

        InteractableInvAdd addComp = obj.GetComponent<InteractableInvAdd>();
        addComp.item = data.id;
        addComp.itemType = ItemModder.GetItemTypeFromId(data.id);
    }
}
