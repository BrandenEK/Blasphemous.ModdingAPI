using System.Collections.Generic;
using UnityEngine;

namespace ModdingAPI
{
    internal class HitboxViewer
    {
        public bool HitboxesEnabled { get; private set; }
        private const float SCALE_AMOUNT = 0.05f;

        private Sprite hitboxImage;
        private List<GameObject> sceneHitboxes = new List<GameObject>();

        public void LoadImage()
        {
            hitboxImage = Main.moddingAPI.fileUtil.loadDataImages("hitbox.png", 1, 1, 1, 0, true, out Sprite[] images) ? images[0] : null;
        }

        public void AddHitboxes()
        {
            if (!HitboxesEnabled) return;
            GameObject baseHitbox = CreateBaseHitbox();
            sceneHitboxes.Clear();

            foreach (BoxCollider2D collider in Object.FindObjectsOfType<BoxCollider2D>())
            {
                Main.LogWarning(Main.MOD_NAME, collider.gameObject.name);
                if (collider.name.StartsWith("GEO_Block")) continue;

                GameObject hitbox = Object.Instantiate(baseHitbox, collider.transform);
                hitbox.transform.localPosition = Vector3.zero;

                Transform side = hitbox.transform.GetChild(0);
                side.localPosition = new Vector3(collider.offset.x, collider.size.y / 2 + collider.offset.y, 0);
                side.localScale = new Vector3(collider.size.x, SCALE_AMOUNT / collider.transform.localScale.y, 0);

                side = hitbox.transform.GetChild(1);
                side.localPosition = new Vector3(-collider.size.x / 2 + collider.offset.x, collider.offset.y, 0);
                side.localScale = new Vector3(SCALE_AMOUNT / collider.transform.localScale.x, collider.size.y, 0);

                side = hitbox.transform.GetChild(2);
                side.localPosition = new Vector3(collider.size.x / 2 + collider.offset.x, collider.offset.y, 0);
                side.localScale = new Vector3(SCALE_AMOUNT / collider.transform.localScale.x, collider.size.y, 0);

                side = hitbox.transform.GetChild(3);
                side.localPosition = new Vector3(collider.offset.x, -collider.size.y / 2 + collider.offset.y, 0);
                side.localScale = new Vector3(collider.size.x, SCALE_AMOUNT / collider.transform.localScale.y, 0);

                sceneHitboxes.Add(hitbox);
            }
            Object.Destroy(baseHitbox);

            Main.LogMessage(Main.MOD_NAME, $"Adding outlines to {sceneHitboxes.Count} hitboxes");
        }

        private GameObject CreateBaseHitbox()
        {
            GameObject baseHitbox = new GameObject("Hitbox");
            GameObject side = new GameObject("TOP");
            side.AddComponent<SpriteRenderer>().sprite = hitboxImage;
            side.transform.parent = baseHitbox.transform;
            side = new GameObject("LEFT");
            side.AddComponent<SpriteRenderer>().sprite = hitboxImage;
            side.transform.parent = baseHitbox.transform;
            side = new GameObject("RIGHT");
            side.AddComponent<SpriteRenderer>().sprite = hitboxImage;
            side.transform.parent = baseHitbox.transform;
            side = new GameObject("BOTTOM");
            side.AddComponent<SpriteRenderer>().sprite = hitboxImage;
            side.transform.parent = baseHitbox.transform;
            return baseHitbox;
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

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Equals))
            {
                HitboxesEnabled = !HitboxesEnabled;
                if (HitboxesEnabled)
                    AddHitboxes();
                else
                    RemoveHitboxes();
            }
        }
    }
}
