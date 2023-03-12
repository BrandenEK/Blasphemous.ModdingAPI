using UnityEngine;
using Framework.Inventory;

namespace ModdingAPI
{
    public abstract class ModRosaryBead : ModItem
    {
        internal RosaryBead CreateRosaryBead(GameObject itemHolder)
        {
            RosaryBead bead = CreateBaseObject<RosaryBead>(itemHolder);
            bead.UsePercentageCompletition = AddToPercentageCompletion;
            return bead;
        }
    }

    public abstract class ModPrayer : ModItem
    {
        protected internal abstract int FervourCost { get; }

        internal Prayer CreatePrayer(GameObject itemHolder)
        {
            Prayer prayer = CreateBaseObject<Prayer>(itemHolder);
            prayer.UsePercentageCompletition = AddToPercentageCompletion;
            prayer.fervourNeeded = FervourCost;
            return prayer;
        }
    }

    public abstract class ModRelic : ModItem
    {
        internal Relic CreateRelic(GameObject itemHolder)
        {
            Relic relic = CreateBaseObject<Relic>(itemHolder);
            relic.UsePercentageCompletition = AddToPercentageCompletion;
            return relic;
        }
    }

    public abstract class ModSwordHeart : ModItem
    {
        internal Sword CreateSwordHeart(GameObject itemHolder)
        {
            Sword swordHeart = CreateBaseObject<Sword>(itemHolder);
            swordHeart.UsePercentageCompletition = AddToPercentageCompletion;
            return swordHeart;
        }
    }

    public abstract class ModQuestItem : ModItem
    {
        internal QuestItem CreateQuestItem(GameObject itemHolder)
        {
            QuestItem questItem = CreateBaseObject<QuestItem>(itemHolder);
            return questItem;
        }
    }

    public abstract class ModCollectible : ModItem
    {
        internal Framework.Inventory.CollectibleItem CreateCollectible (GameObject itemHolder)
        {
            Framework.Inventory.CollectibleItem collectible = CreateBaseObject<Framework.Inventory.CollectibleItem>(itemHolder);
            collectible.UsePercentageCompletition = AddToPercentageCompletion;
            return collectible;
        }
    }
}
