using UnityEngine;
using System.Collections.Generic;

namespace ModdingAPI
{
    public class ModRosaryBead : ModItem
    {
        protected internal bool AddToPercentageCompletion { get; }

        internal List<ModItemEffect> Effects { get; private set; }

        public ModRosaryBead AddEffect<T>() where T : ModItemEffect, new()
        {
            Effects.Add(new T());
            return this;
        }
    }

    // Prayers have fervourNeeded
    // Questitems dont add to percentage
}
