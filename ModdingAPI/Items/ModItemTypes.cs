using UnityEngine;
using Framework.Inventory;

namespace ModdingAPI.Items
{
    /// <summary>
    /// A custom rosary bead
    /// </summary>
    public abstract class ModRosaryBead : ModItem
    {
        internal RosaryBead CreateRosaryBead(GameObject itemHolder)
        {
            RosaryBead bead = CreateBaseObject<RosaryBead>(itemHolder);
            bead.UsePercentageCompletition = AddToPercentCompletion;
            return bead;
        }
    }

    /// <summary>
    /// A custom prayer
    /// </summary>
    public abstract class ModPrayer : ModItem
    {
        /// <summary>
        /// The fervour cost of using this prayer
        /// </summary>
        protected internal abstract int FervourCost { get; }

        internal Prayer CreatePrayer(GameObject itemHolder)
        {
            Prayer prayer = CreateBaseObject<Prayer>(itemHolder);
            prayer.UsePercentageCompletition = AddToPercentCompletion;
            prayer.fervourNeeded = FervourCost;
            return prayer;
        }
    }

    /// <summary>
    /// A custom relic
    /// </summary>
    public abstract class ModRelic : ModItem
    {
        internal Relic CreateRelic(GameObject itemHolder)
        {
            Relic relic = CreateBaseObject<Relic>(itemHolder);
            relic.UsePercentageCompletition = AddToPercentCompletion;
            return relic;
        }
    }

    /// <summary>
    /// A custom sword heart
    /// </summary>
    public abstract class ModSwordHeart : ModItem
    {
        internal Sword CreateSwordHeart(GameObject itemHolder)
        {
            Sword swordHeart = CreateBaseObject<Sword>(itemHolder);
            swordHeart.UsePercentageCompletition = AddToPercentCompletion;
            return swordHeart;
        }
    }

    /// <summary>
    /// A custom quest item
    /// </summary>
    public abstract class ModQuestItem : ModItem
    {
        internal QuestItem CreateQuestItem(GameObject itemHolder)
        {
            QuestItem questItem = CreateBaseObject<QuestItem>(itemHolder);
            return questItem;
        }
    }

    /// <summary>
    /// A custom collectible
    /// </summary>
    public abstract class ModCollectible : ModItem
    {
        internal Framework.Inventory.CollectibleItem CreateCollectible (GameObject itemHolder)
        {
            Framework.Inventory.CollectibleItem collectible = CreateBaseObject<Framework.Inventory.CollectibleItem>(itemHolder);
            collectible.UsePercentageCompletition = AddToPercentCompletion;
            return collectible;
        }
    }
}
