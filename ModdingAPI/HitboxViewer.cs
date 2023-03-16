using System.Collections.Generic;
using UnityEngine;

namespace ModdingAPI
{
    internal class HitboxViewer
    {
        private Sprite hitboxImage;
        private List<GameObject> sceneHitboxes = new List<GameObject>();

        public void LoadImage()
        {
            hitboxImage = Main.moddingAPI.fileUtil.loadDataImages("hitbox.png", 32, 32, 32, 0, true, out Sprite[] images) ? images[0] : null;
        }

        public void AddHitboxes()
        {
            GameObject baseHitbox = new GameObject("Hitbox");
            baseHitbox.AddComponent<SpriteRenderer>().sprite = hitboxImage;

            sceneHitboxes.Clear();
            foreach (BoxCollider2D collider in Object.FindObjectsOfType<BoxCollider2D>())
            {
                GameObject hitbox = Object.Instantiate(baseHitbox, collider.transform);
                hitbox.transform.localPosition = new Vector3(collider.offset.x, collider.offset.y, 0);
                hitbox.transform.localScale = new Vector3(collider.size.x, collider.size.y, 0);
                sceneHitboxes.Add(hitbox);
            }
            Object.Destroy(baseHitbox);

            Main.LogMessage(Main.MOD_NAME, $"Adding outlines to {sceneHitboxes.Count} hitboxes");
        }

        public void RemoveHitboxes()
        {
            for (int i = 0; i < sceneHitboxes.Count; i++)
            {
                if (sceneHitboxes[i] != null)
                    Object.Destroy(sceneHitboxes[i]);
            }
            sceneHitboxes.Clear();
        }
    }
}
