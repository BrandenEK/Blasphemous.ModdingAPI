using UnityEngine;

namespace Blasphemous.ModdingAPI.Levels.Modifiers;

public class SpikeModifier : IModifier
{
    public void Apply(GameObject obj, ObjectData data)
    {
        obj.name = "Spikes";

        obj.tag = "SpikeTrap";
        obj.layer = LayerMask.NameToLayer("Trap");

        BoxCollider2D collider = obj.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;
        collider.size = new Vector2(1.8f, 0.8f);
    }
}
