using System.Collections.Generic;
using UnityEngine;

namespace ModdingAPI
{
    internal class SkinLoader
    {
        public bool allowedSkinButtons { get; private set; }
        
        private Dictionary<string, SkinInfo> customSkins;
        private List<GameObject> skinButtons;

        public SkinLoader()
        {
            customSkins = new Dictionary<string, SkinInfo>();
            skinButtons = new List<GameObject>();
        }

        public void addSkinButton(GameObject obj)
        {
            skinButtons.Add(obj);
        }

        public List<GameObject> allowSkinButtons()
        {
            allowedSkinButtons = true;
            return skinButtons;
        }

        public SkinInfo getSkinInfo(string id)
        {
            if (customSkins.ContainsKey(id))
                return customSkins[id];
            return null;
        }

        public IEnumerable<SkinInfo> getAllSkinInfos()
        {
            return customSkins.Values;
        }

        public void loadCustomSkins()
        {
            Dictionary<string, Sprite> skinData = Main.moddingAPI.fileUtil.loadCustomSkins();

            foreach (string skinText in skinData.Keys)
            {
                SkinInfo skinInfo = Main.moddingAPI.fileUtil.jsonObject<SkinInfo>(skinText);
                skinInfo.texture = skinData[skinText];
                customSkins.Add(skinInfo.id, skinInfo);
                Main.LogMessage(Main.MOD_NAME, $"Loading custom skin: {skinInfo.id} by {skinInfo.author}");
            }
        }
    }

    [System.Serializable]
    internal class SkinInfo
    {
        public string id;
        public string name;
        public string author;
        public Sprite texture;
    }
}
