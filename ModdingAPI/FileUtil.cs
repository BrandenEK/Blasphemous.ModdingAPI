using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace ModdingAPI
{
    public class FileUtil
    {
        private readonly string configPath = "";
        private readonly string logPath = "";
        private readonly string dataPath = "";

        public FileUtil(Mod mod)
        {
            configPath = Path.GetFullPath("Modding\\config\\" + mod.ModName + ".cfg");
            logPath = Path.GetFullPath("Modding\\logs\\" + mod.ModName + ".log");
            dataPath = Path.GetFullPath("Modding\\data\\" + mod.ModName + "\\");
        }

        private bool read(string path, out string text)
        {
            if (File.Exists(path))
            {
                text = File.ReadAllText(path);
                return true;
            }

            text = null;
            return false;
        }

        // Config files

        public T loadConfig<T>() where T : new()
        {
            if (read(configPath, out string json))
            {
                return jsonObject<T>(json);
            }

            T config = new T();
            saveConfig(config);
            return config;
        }

        public void saveConfig<T>(T config)
        {
            File.WriteAllText(configPath, jsonString(config));
        }

        // Log files

        public void appendLog(string line)
        {
            File.AppendAllText(logPath, line);
        }

        public void clearLog()
        {
            File.WriteAllText(logPath, string.Empty);
        }

        // Data files

        public bool loadData(string fileName, out string text)
        {
            return read(dataPath + fileName, out text);
        }

        public void saveData(string fileName, string text)
        {
            File.WriteAllText(dataPath + fileName, text);
        }

        public bool deleteData(string fileName)
        {
            string path = dataPath + fileName;
            if (File.Exists(path))
            {
                try
                {
                    File.Delete(path);
                    return true;
                }
                catch (Exception) { }
            }
            return false;
        }

        // Specific data files

        public bool loadArray(string fileName, out string[] output)
        {
            if (loadData(fileName, out string text))
            {
                text = text.Replace("\r", string.Empty);
                output = text.Split('\n');
                return true;
            }

            output = null;
            return false;
        }

        public bool loadDictionary(string fileName, out Dictionary<string, string> output)
        {
            if (loadArray(fileName, out string[] array))
            {
                output = new Dictionary<string, string>();
                for (int i = 0; i < array.Length; i++)
                {
                    int num = array[i].IndexOf(',');
                    output.Add(array[i].Substring(0, num), array[i].Substring(num + 1));
                }
                return true;
            }

            output = null;
            return false;
        }

        public bool loadImages(string fileName, int spriteSize, int pixelsPerUnit, int border, bool pointFilter, out Sprite[] output)
        {
            string path = dataPath + fileName;
            if (!File.Exists(path))
            {
                output = null;
                return false;
            }

            byte[] bytes = File.ReadAllBytes(path);
            Texture2D tex = new Texture2D(spriteSize, spriteSize, TextureFormat.RGBA32, false);
            tex.LoadImage(bytes);
            if (pointFilter) tex.filterMode = FilterMode.Point;
            int w = tex.width, h = tex.height;
            output = new Sprite[w * h / (spriteSize * spriteSize)];

            int count = 0;
            for (int i = h - spriteSize; i >= 0; i -= spriteSize)
            {
                for (int j = 0; j < w; j += spriteSize)
                {
                    Sprite sprite = Sprite.Create(tex, new Rect(j, i, spriteSize, spriteSize), new Vector2(0.5f, 0.5f), pixelsPerUnit, 0, SpriteMeshType.Tight, new Vector4(border, border, border, border));
                    output[count] = sprite;
                    count++;
                }
            }
            return true;
        }

        // Json files

        public bool loadJson<T>(string fileName, out T obj)
        {
            if (loadData(fileName, out string json))
            {
                obj = jsonObject<T>(json);
                return true;
            }

            obj = default(T);
            return false;
        }

        public void saveJson<T>(string fileName, T obj)
        {
            saveData(fileName, jsonString(obj));
        }

        public T jsonObject<T>(string json) { return JsonConvert.DeserializeObject<T>(json); }

        public string jsonString<T>(T obj) { return JsonConvert.SerializeObject(obj, Formatting.Indented); }
    }
}
