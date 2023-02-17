using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace ModdingAPI
{
    /// <summary>
    /// A class for handling file IO related operations
    /// </summary>
    public class FileUtil
    {
        private readonly string configPath = "";
        private readonly string logPath = "";
        private readonly string dataPath = "";

        internal FileUtil(Mod mod)
        {
            configPath = Path.GetFullPath("Modding\\config\\" + mod.ModName + ".cfg");
            logPath = Path.GetFullPath("Modding\\logs\\" + mod.ModName + ".log");
            dataPath = Path.GetFullPath("Modding\\data\\" + mod.ModName + "\\");
        }

        internal FileUtil() { }

        internal Dictionary<string, Sprite> loadCustomSkins()
        {
            string skinsPath = Path.GetFullPath("Modding\\skins\\");
            Dictionary<string, Sprite> customSkins = new Dictionary<string, Sprite>();
            string[] skinFiles = Directory.GetFiles(skinsPath);

            for (int i = 0; i < skinFiles.Length; i++)
            {
                Main.LogWarning(skinFiles[i]);
                byte[] bytes = File.ReadAllBytes(skinFiles[i]);
                Texture2D tex = new Texture2D(256, 1, TextureFormat.RGBA32, false);
                tex.LoadImage(bytes);
                Sprite skinTexture = Sprite.Create(tex, new Rect(0, 0, 256, 1), new Vector2(0.5f, 0.5f));
                string skinId = skinFiles[i].Substring(skinFiles[i].LastIndexOf("\\") + 1);
                Main.LogWarning(skinId.Substring(0, skinId.LastIndexOf(".")));
                customSkins.Add(skinId.Substring(0, skinId.LastIndexOf(".")), skinTexture);
            }

            return customSkins;
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

        /// <summary>
        /// Loads the configuration file for this mod from the configs folder
        /// </summary>
        /// <typeparam name="T">The type of the config object</typeparam>
        /// <returns>The configuration object</returns>
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

        /// <summary>
        /// Saves a configuration file for this mod to the configs folder
        /// </summary>
        /// <typeparam name="T">The type of the config object</typeparam>
        /// <param name="config">The configuration object</param>
        public void saveConfig<T>(T config)
        {
            File.WriteAllText(configPath, jsonString(config));
        }

        // Log files

        /// <summary>
        /// Adds a message to the log file for this mod
        /// </summary>
        /// <param name="line">The message to add to the log file</param>
        public void appendLog(string line)
        {
            File.AppendAllText(logPath, line);
        }

        /// <summary>
        /// Clears the log file for this mod
        /// </summary>
        public void clearLog()
        {
            File.WriteAllText(logPath, string.Empty);
        }

        // Data files

        /// <summary>
        /// Loads a string from a file in the data folder
        /// </summary>
        /// <param name="fileName">The name of the data file</param>
        /// <param name="output">The data string, or null if the file deosn't exist</param>
        /// <returns>Whether the data was loaded successfully or not</returns>
        public bool loadDataText(string fileName, out string output)
        {
            return read(dataPath + fileName, out output);
        }

        /// <summary>
        /// Loads an array from a file in the data folder
        /// </summary>
        /// <param name="fileName">The name of the data file</param>
        /// <param name="output">The data array, or null if the file deosn't exist</param>
        /// <returns>Whether the data was loaded successfully or not</returns>
        public bool loadDataArray(string fileName, out string[] output)
        {
            if (loadDataText(fileName, out string text))
            {
                text = text.Replace("\r", string.Empty);
                output = text.Split('\n');
                return true;
            }

            output = null;
            return false;
        }

        /// <summary>
        /// Loads a dictionary from a file in the data folder
        /// </summary>
        /// <param name="fileName">The name of the data file</param>
        /// <param name="output">The data dictionary, or null if the file deosn't exist</param>
        /// <returns>Whether the data was loaded successfully or not</returns>
        public bool loadDataDictionary(string fileName, out Dictionary<string, string> output)
        {
            if (loadDataArray(fileName, out string[] array))
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

        /// <summary>
        /// Loads an array of images from an image file in the data folder
        /// </summary>
        /// <param name="fileName">The name of the data file</param>
        /// <param name="spriteSize">The pixel size of each square sprite in the image</param>
        /// <param name="pixelsPerUnit">The pixels per unit of each square sprite in the image</param>
        /// <param name="border">The border size of each square sprite in the image</param>
        /// <param name="pointFilter">Whether a point filter should be applied to the image</param>
        /// <param name="output">The data images, or null if the file deosn't exist</param>
        /// <returns>Whether the data was loaded successfully or not</returns>
        public bool loadDataImages(string fileName, int spriteSize, int pixelsPerUnit, int border, bool pointFilter, out Sprite[] output)
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

        // Misc. methods

        /// <summary>
        /// Writes text to a file in the root directory
        /// </summary>
        /// <param name="fileName">The name of text file to create</param>
        /// <param name="text">The text to write to the file</param>
        public void saveTextFile(string fileName, string text)
        {
            File.WriteAllText(dataPath + fileName, text);
        }

        /// <summary>
        /// Converts a json string into a json object
        /// </summary>
        /// <typeparam name="T">The type of object to convert to</typeparam>
        /// <param name="json">The json string to convert into an object</param>
        /// <returns>The object created from the json string</returns>
        public T jsonObject<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// Converts an object into a json string
        /// </summary>
        /// <typeparam name="T">The type of object to convert from</typeparam>
        /// <param name="obj">The object to convert into a json string</param>
        /// <returns>The json string created from the object</returns>
        public string jsonString<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }
    }
}
