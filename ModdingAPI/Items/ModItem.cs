using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ModdingAPI
{
    public class ModItem
    {
        protected internal string Id { get; }

        protected internal string Name { get; }

        protected internal string Description { get; }

        protected internal string Lore { get; }

        protected internal bool CarryOnStart { get; }

        protected internal bool PreserveInNGPlus { get; }

        internal Sprite Picture { get; private set; }

        protected void SetPicture(Sprite picture)
        {
            Picture = picture;
        }
    }
}
