using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Framework.Managers;
using Framework.Inventory;
using Framework.Util;

namespace ModdingAPI.Levels
{
    internal class LevelLoader
    {
        private const string ITEM_SCENE = "D02Z02S14_LOGIC";

        public bool InLoadProcess { get; private set; }
        private bool LoadedObjects { get; set; }

        private Dictionary<string, LevelStructure> LevelModifications { get; set; }

        private GameObject itemObject;


        public string ItemPersId => "RE402-ITEM";
        public string ItemFlag => "RE402_COLLECTED";

        public void LoadObjects()
        {
            itemObject = null;
            Main.Instance.StartCoroutine(LoadCollectibleItem(ITEM_SCENE));
            LoadedObjects = true;
        }

        public void LoadLevelEdits()
        {
            LevelModifications = Main.moddingAPI.fileUtil.loadLevels();
        }

        public void LevelLoaded(string level)
        {
            // When game is first started, load objects
            if (level == "MainMenu")
            {
                if (!LoadedObjects)
                    LoadObjects();
                return;
            }

            // Apply all level edits from other mods

            // Change
            // When loading level MoM, remove wall climb and add the collectible item
            if (level != "D04Z02S01") return;

            // Remove wall climb
            //foreach (GameObject obj in SceneManager.GetSceneByName("D04Z02S01_DECO").GetRootGameObjects())
            //{
            //    if (obj.name == "MIDDLEGROUND")
            //    {
            //        Transform holder = obj.transform.Find("AfterPlayer/WallClimb");
            //        holder.GetChild(0).gameObject.SetActive(false);
            //        holder.GetChild(1).gameObject.SetActive(false);
            //    }
            //}
            //foreach (GameObject obj in SceneManager.GetSceneByName("D04Z02S01_LAYOUT").GetRootGameObjects())
            //{
            //    if (obj.name == "NAVIGATION")
            //    {
            //        obj.transform.Find("NAV_Wall Climb (1x3) (2)").gameObject.SetActive(false);
            //    }
            //}

            CreateCollectibleItem(ItemPersId, "RE402", new Vector3(233, 29, 0));
        }

        private IEnumerator LoadCollectibleItem(string sceneName)
        {
            InLoadProcess = true;

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            Scene tempScene = SceneManager.GetSceneByName(sceneName);
            foreach (GameObject obj in tempScene.GetRootGameObjects())
            {
                if (obj.name == "LOGIC")
                {
                    // This is the logic object
                    CollectibleItem item = obj.GetComponentInChildren<CollectibleItem>();
                    itemObject = Object.Instantiate(item.gameObject, Main.Instance.transform);
                    itemObject.SetActive(false);
                }

                obj.SetActive(false);
                //Object.DestroyImmediate(obj);
            }

            yield return null;

            asyncLoad = SceneManager.UnloadSceneAsync(tempScene);
            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            Camera.main.transform.position = new Vector3(0, 0, -10);
            Camera.main.backgroundColor = new Color(0, 0, 0, 1);

            InLoadProcess = false;
            if (itemObject == null)
                Main.LogError(Main.MOD_NAME, "Failed to load CollectibleItem object");
            else
                Main.LogMessage(Main.MOD_NAME, "Loaded CollectibleItem object");
        }

        // Change to use pers. id instead of flag
        private void CreateCollectibleItem(string persistentId, string itemId, Vector3 position)
        {
            if (itemObject == null) return;

            GameObject newItem = Object.Instantiate(itemObject, GameObject.Find("INTERACTABLES").transform);
            newItem.SetActive(true);
            newItem.transform.position = position;
            newItem.GetComponent<UniqueId>().uniqueId = persistentId;

            InteractableInvAdd addComponent = newItem.GetComponent<InteractableInvAdd>();
            addComponent.item = itemId;
            addComponent.itemType = GetItemType(itemId);

            // Hopefully can remove this once the actual pers id is used
            CollectibleItem collectComponent = newItem.GetComponent<CollectibleItem>();
            bool collected = Core.Events.GetFlag(ItemFlag);
            collectComponent.Consumed = collected;
            collectComponent.transform.GetChild(2).gameObject.SetActive(!collected);
        }

        private InventoryManager.ItemType GetItemType(string id)
        {
            if (id != null && id.Length >= 2)
            {
                switch (id.Substring(0, 2))
                {
                    case "RB": return InventoryManager.ItemType.Bead;
                    case "PR": return InventoryManager.ItemType.Prayer;
                    case "RE": return InventoryManager.ItemType.Relic;
                    case "HE": return InventoryManager.ItemType.Sword;
                    case "QI": return InventoryManager.ItemType.Quest;
                    case "CO": return InventoryManager.ItemType.Collectible;
                }
            }
            Main.LogError(Main.MOD_NAME, "Could not determine item type for " + id);
            return InventoryManager.ItemType.Bead;
        }
    }
}
