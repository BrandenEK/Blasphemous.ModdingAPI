using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ModdingAPI
{
    internal class HitboxViewer
    {
        private Sprite hitboxImage;

        public HitboxViewer()
        {
            hitboxImage = Main.moddingAPI.fileUtil.loadDataImages("hitbox.png", 32, 32, 32, 0, true, out Sprite[] images) ? images[0] : null;
        }

        public void AddHitboxes()
        {

            Main.LogMessage(Main.MOD_NAME, "Adding outlines to 0 hitboxes");
        }

        public void RemoveHitboxes()
        {

        }
    }
}
