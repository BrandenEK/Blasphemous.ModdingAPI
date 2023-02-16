using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace ModdingAPI
{
    public class FileUtil
    {
        private Mod mod;

        public FileUtil(Mod mod)
        {
            this.mod = mod;
        }

        // Config files

        public bool loadConfig(out string text)
        {
            text = "";
            return true;
        }

        public void saveConfig(string text)
        {

        }

        // Log files

        public void appendLog(string line)
        {

        }

        public void clearLog()
        {

        }

        // Data files

        public bool loadData(string fileName, out string text)
        {
            text = "";
            return true;
        }

        public void saveData(string fileName, string text)
        {

        }

        public void deleteData(string fileName)
        {

        }

        // Specific data files

        public bool loadDictionary(string fileName, out Dictionary<string, string> output)
        {
            output = null;
            return true;
        }

        public bool loadArray(string fileName, out string[] output)
        {
            output = null;
            return true;
        }

        public bool loadImages(string fileName, out Sprite[] output)
        {
            output = null;
            return true;
        }

        // Json files

        public bool loadJson(string fileName, out string text)
        {
            text = "";
            return true;
        }

        public void saveJson(string fileName, string text)
        {

        }

        private T jsonObject<T>(string json) { return JsonConvert.DeserializeObject<T>(json); }

        private string jsonString<T>(T obj) { return JsonConvert.SerializeObject(obj, Formatting.Indented); }
    }
}
