using UnityEngine;
using Framework.Inventory;
using System.Collections.Generic;

namespace ModdingAPI
{
    public abstract class ModRosaryBead : ModItem
    {
        internal RosaryBead CreateRosaryBead(GameObject itemHolder)
        {
            // Create object
            GameObject obj = new GameObject(Id);
            obj.transform.SetParent(itemHolder.transform.Find("RosaryBead"));

            // Set properties
            RosaryBead Bead = obj.AddComponent<RosaryBead>();
            Bead.id = Id;
            Bead.caption = Name;
            Bead.description = Description;
            Bead.lore = Lore;
            Bead.picture = Picture;
            Bead.carryonstart = CarryOnStart;
            Bead.preserveInNewGamePlus = PreserveInNGPlus;
            Bead.UsePercentageCompletition = AddToPercentageCompletion;

            // Add object effects
            foreach (ModItemEffect effect in Effects)
            {
                Bead.gameObject.AddComponent<ModItemEffectSystem>().SetEffect(effect);
            }

            return Bead;
        }
    }

    public abstract class ModPrayer : ModItem
    {
        protected internal abstract int FervourCost { get; }
    }

    public abstract class ModRelic : ModItem
    {
        internal Relic CreateRelic(GameObject itemHolder)
        {
            // Create object
            GameObject obj = new GameObject(Id);
            obj.transform.SetParent(itemHolder.transform.Find("Relic"));

            // Set properties
            Relic Relic = obj.AddComponent<Relic>();
            Relic.id = Id;
            Relic.caption = Name;
            Relic.description = Description;
            Relic.lore = Lore;
            Relic.picture = Picture;
            Relic.carryonstart = CarryOnStart;
            Relic.preserveInNewGamePlus = PreserveInNGPlus;
            Relic.UsePercentageCompletition = AddToPercentageCompletion;

            // Add object effects
            foreach (ModItemEffect effect in Effects)
            {
                Relic.gameObject.AddComponent<ModItemEffectSystem>().SetEffect(effect);
            }

            return Relic;
        }
    }

    public abstract class ModSwordHeart : ModItem
    {

    }

    public abstract class ModQuestItem : ModItem
    {

    }

    public abstract class ModCollectible : ModItem
    {

    }
}
