using UnityEngine;
using System.Collections.Generic;

namespace ModdingAPI
{
    public abstract class ModRosaryBead : ModItem
    {
        
    }

    public abstract class ModPrayer : ModItem
    {
        protected internal abstract int FervourCost { get; }
    }

    public abstract class ModRelic : ModItem
    {

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
