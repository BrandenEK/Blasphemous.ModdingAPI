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
    }

    [System.Serializable]
    internal class SkinInfo
    {
        public string id;
        public string name;
        public string author;
        public Sprite texture;

        //public SkinInfo(string id, string name, string author)
        //{
        //    this.id = id;
        //    this.name = name;
        //    this.author = author;
        //}
    }
}
