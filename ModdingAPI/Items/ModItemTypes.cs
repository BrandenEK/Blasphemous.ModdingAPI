using UnityEngine;
using Framework.Managers;
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

        internal override void GiveItem()
        {
            Core.InventoryManager.AddRosaryBead(Id);
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

        internal override void GiveItem()
        {
            Core.InventoryManager.AddPrayer(Id);
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

        internal override void GiveItem()
        {
            Core.InventoryManager.AddRelic(Id);
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

        internal override void GiveItem()
        {
            Core.InventoryManager.AddSword(Id);
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

        internal override void GiveItem()
        {
            Core.InventoryManager.AddQuestItem(Id);
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

        internal override void GiveItem()
        {
            Core.InventoryManager.AddCollectibleItem(Id);
        }
    }
}
