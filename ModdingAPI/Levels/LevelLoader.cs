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
        private const string SPIKE_SCENE = "";

        public bool InLoadProcess { get; private set; }
        private bool LoadedObjects { get; set; }

        private Dictionary<string, LevelStructure> LevelModifications { get; set; }

        private GameObject itemObject;
        private GameObject spikesObject;

        public void LoadLevelEdits()
        {
            LevelModifications = Main.moddingAPI.fileUtil.loadLevels();
        }

        public void LevelPreLoaded(string level)
        {
            // Apply all level edits from other mods
            if (!LevelModifications.ContainsKey(level))
                return;

            LevelStructure levelModification = LevelModifications[level];
            Main.LogMessage(Main.MOD_NAME, "Applying modifications for " + level);

            if (levelModification.AddedObjects != null)
            {
                // Add any objects
                foreach (AddedObject obj in levelModification.AddedObjects)
                {
                    if (obj.Type == "item")
                        CreateCollectibleItem(obj.Id, new Vector3(obj.XPos, obj.YPos));
                }
            }
            if (levelModification.DisabledObjects != null)
            {
                // Potentially disable decoration objects
                List<string> decoration = levelModification.DisabledObjects.Decoration;
                if (decoration != null && decoration.Count > 0)
                    DisableObjectGroup(SceneManager.GetSceneByName(level + "_DECO"), levelModification.DisabledObjects.Decoration);
                // Potentially disable layout objects
                List<string> layout = levelModification.DisabledObjects.Layout;
                if (layout != null && layout.Count > 0)
                    DisableObjectGroup(SceneManager.GetSceneByName(level + "_LAYOUT"), levelModification.DisabledObjects.Layout);
                // Potentially disable logic objects
                List<string> logic = levelModification.DisabledObjects.Logic;
                if (logic != null && logic.Count > 0)
                    DisableObjectGroup(SceneManager.GetSceneByName(level + "_LOGIC"), levelModification.DisabledObjects.Logic);
            }
        }

        public void LevelLoaded(string level)
        {
            // When game is first started, load objects
            if (level == "MainMenu" && !LoadedObjects)
            {
                itemObject = null;
                Main.Instance.StartCoroutine(LoadRequiredItems());
            }
        }

        #region Loading Objects

        private IEnumerator LoadRequiredItems()
        {
            yield return Main.Instance.StartCoroutine(LoadSceneForObject(ITEM_SCENE, "LOGIC/INTERACTABLES/???", itemObject));
            //yield return Main.Instance.StartCoroutine(LoadSceneForObject(SPIKE_SCENE, "???", spikesObject));

            Camera.main.transform.position = new Vector3(0, 0, -10);
            Camera.main.backgroundColor = new Color(0, 0, 0, 1);
            LoadedObjects = true;
        }

        private IEnumerator LoadSceneForObject(string sceneName, string objectPath, GameObject obj)
        {
            InLoadProcess = true;

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            // Load the item from this scene
            Scene tempScene = SceneManager.GetSceneByName(sceneName);
            GameObject sceneObject = FindObjectInScene(tempScene, objectPath, true);
            if (sceneObject == null)
            {
                Main.LogError(Main.MOD_NAME, $"Failed to load {objectPath} from {sceneName}");
            }
            else
            {
                obj = Object.Instantiate(sceneObject, Main.Instance.transform);
                obj.SetActive(false);
                Main.LogMessage(Main.MOD_NAME, $"Loaded {objectPath} from {sceneName}");
            }

            yield return null;

            asyncLoad = SceneManager.UnloadSceneAsync(tempScene);
            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            InLoadProcess = false;
        }

        private void DisableObjectGroup(Scene scene, List<string> disabledObjects)
        {
            // Store dictionary of root objects
            Dictionary<string, Transform> rootObjects = FindRootObjectsInScene(scene, false);

            // Loop through disabled objects and locate & disable them
            foreach (string disabledObject in disabledObjects)
            {
                GameObject obj = FindObjectInScene(rootObjects, disabledObject);
                if (obj != null)
                    obj.SetActive(false);
            }
        }

        #endregion Loading Objects

        #region Finding Objects

        private GameObject FindObjectInScene(Scene scene, string objectPath, bool disableRoots)
        {
            return FindObjectInScene(FindRootObjectsInScene(scene, disableRoots), objectPath);
        }

        private GameObject FindObjectInScene(Dictionary<string, Transform> rootObjects, string objectPath)
        {
            string[] transformPath = objectPath.Split('/');

            Transform currTransform = null;
            for (int i = 0; i < transformPath.Length; i++)
            {
                string finder = transformPath[i];
                if (i == 0)
                {
                    currTransform = rootObjects.ContainsKey(finder) ? rootObjects[finder] : null;
                }
                else if (finder.Length >= 3 && finder[0] == '{' && finder[finder.Length - 1] == '}')
                {
                    int childIdx = int.Parse(finder.Substring(1, finder.Length - 2));
                    currTransform = currTransform.childCount > childIdx ? currTransform.GetChild(childIdx) : null;
                }
                else
                {
                    currTransform = currTransform.Find(finder);
                }

                if (finder == null) break;
            }

            return currTransform?.gameObject;
        }

        private Dictionary<string, Transform> FindRootObjectsInScene(Scene scene, bool disableRoots)
        {
            Dictionary<string, Transform> rootObjects = new Dictionary<string, Transform>();
            foreach (GameObject obj in scene.GetRootGameObjects())
            {
                if (obj.name[0] != '=' && !rootObjects.ContainsKey(obj.name))
                {
                    rootObjects.Add(obj.name, obj.transform);
                    if (disableRoots)
                        obj.SetActive(false);
                }
            }
            return rootObjects;
        }

        #endregion Finding Objects

        #region Creating Objects

        private void CreateCollectibleItem(string itemId, Vector3 position)
        {
            if (itemObject == null) return;

            GameObject newItem = Object.Instantiate(itemObject, GameObject.Find("INTERACTABLES").transform);
            newItem.name = "Item Pickup " + itemId;
            newItem.SetActive(true);
            newItem.transform.position = position;
            newItem.GetComponent<UniqueId>().uniqueId = "ITEM-PICKUP-" + itemId;

            InteractableInvAdd addComponent = newItem.GetComponent<InteractableInvAdd>();
            addComponent.item = itemId;
            addComponent.itemType = GetItemType(itemId);
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

        #endregion Creating Objects
    }
}
