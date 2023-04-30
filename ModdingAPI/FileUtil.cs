using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using ModdingAPI.Levels;

namespace ModdingAPI
{
    /// <summary>
    /// A class for handling file IO related operations
    /// </summary>
    public class FileUtil
    {
        private readonly string rootPath = "";
        private readonly string configPath = "";
        private readonly string dataPath = "";
        private readonly string localizationPath = "";
        private readonly string logPath = "";

        // Constructor called by mods to set up their paths
        internal FileUtil(Mod mod)
        {
            rootPath = Directory.GetCurrentDirectory() + "\\";
            configPath = Path.GetFullPath("Modding\\config\\" + mod.ModName + ".cfg");
            dataPath = Path.GetFullPath("Modding\\data\\" + mod.ModName + "\\");
            logPath = Path.GetFullPath("Modding\\logs\\" + mod.ModName + ".log");
            localizationPath = Path.GetFullPath("Modding\\localization\\" + mod.ModName + ".txt");
        }

        // Constructor called by mod api to create directories
        internal FileUtil()
        {
            Directory.CreateDirectory(Path.GetFullPath("Modding\\config\\"));
            Directory.CreateDirectory(Path.GetFullPath("Modding\\data\\"));
            Directory.CreateDirectory(Path.GetFullPath("Modding\\levels\\"));
            Directory.CreateDirectory(Path.GetFullPath("Modding\\localization\\"));
            Directory.CreateDirectory(Path.GetFullPath("Modding\\logs\\"));
            Directory.CreateDirectory(Path.GetFullPath("Modding\\skins\\"));
            localizationPath = Path.GetFullPath("Modding\\localization\\Modding API.txt");
            dataPath = Path.GetFullPath("Modding\\data\\Modding API\\");
        }

        internal Dictionary<string, Sprite> loadCustomSkins()
        {
            string skinsPath = Path.GetFullPath("Modding\\skins\\");
            Dictionary<string, Sprite> customSkins = new Dictionary<string, Sprite>();
            string[] skinFolders = Directory.GetDirectories(skinsPath);

            for (int i = 0; i < skinFolders.Length; i++)
            {
                if (getSkinFiles(skinFolders[i], out string skinInfo, out Sprite skinTexture))
                {
                    if (!customSkins.ContainsKey(skinInfo))
                        customSkins.Add(skinInfo, skinTexture);
                }
            }

            return customSkins;

            bool getSkinFiles(string path, out string skinInfo, out Sprite skinTexture)
            {
                skinInfo = null; skinTexture = null;
                if (!File.Exists(path + "\\info.txt") || !File.Exists(path + "\\texture.png")) return false;

                skinInfo = File.ReadAllText(path + "\\info.txt");
                byte[] bytes = File.ReadAllBytes(path + "\\texture.png");
                Texture2D tex = new Texture2D(256, 1, TextureFormat.RGB24, false);
                tex.LoadImage(bytes);
                tex.filterMode = FilterMode.Point;
                skinTexture = Sprite.Create(tex, new Rect(0, 0, 256, 1), new Vector2(0.5f, 0.5f));

                return true;
            }
        }

        internal Dictionary<string, LevelStructure> loadLevels()
        {
            string levelsPath = Path.GetFullPath("Modding\\levels\\");
            Dictionary<string, LevelStructure> allLevels = new Dictionary<string, LevelStructure>();

            foreach (string folder in Directory.GetDirectories(levelsPath))
            {
                // Check if the mod associated with these level edits is active
                string modName = folder.Substring(folder.LastIndexOf('\\') + 1);
                if (!isModEnabled(modName))
                    continue;
                Main.LogMessage(Main.MOD_NAME, "Loading level modifications for " + modName);

                // For each individual level file, load it
                foreach (string file in Directory.GetFiles(folder))
                {
                    string levelName = file.Substring(file.LastIndexOf('\\') + 1);
                    levelName = levelName.Substring(0, levelName.LastIndexOf('.'));
                    LevelStructure levelStructure = jsonObject<LevelStructure>(File.ReadAllText(file));

                    if (allLevels.ContainsKey(levelName))
                        allLevels[levelName].CombineLevel(levelStructure);
                    else
                        allLevels.Add(levelName, levelStructure);
                }
            }

            return allLevels;

            bool isModEnabled(string modName)
            {
                foreach (Mod mod in Main.moddingAPI.GetMods())
                {
                    if (mod.ModName == modName)
                        return true;
                }
                return false;
            }
        }

        internal string[] loadLocalization()
        {
            if (File.Exists(localizationPath))
            {
                return File.ReadAllLines(localizationPath);
            }
            return null;
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

        internal void appendLog(string line)
        {
            File.AppendAllText(logPath, line + "\n");
        }

        internal void clearLog()
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
        /// <param name="spriteWidth">The pixel width of each sprite in the image</param>
        /// <param name="spriteHeight">The pixel height of each sprite in the image</param>
        /// <param name="pixelsPerUnit">The pixels per unit of each sprite in the image</param>
        /// <param name="border">The border size of each sprite in the image</param>
        /// <param name="pointFilter">Whether a point filter should be applied to the image</param>
        /// <param name="output">The data images, or null if the file deosn't exist</param>
        /// <returns>Whether the data was loaded successfully or not</returns>
        public bool loadDataImages(string fileName, int spriteWidth, int spriteHeight, int pixelsPerUnit, int border, bool pointFilter, out Sprite[] output)
        {
            string path = dataPath + fileName;
            if (!File.Exists(path))
            {
                output = null;
                return false;
            }

            byte[] bytes = File.ReadAllBytes(path);
            Texture2D tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            tex.LoadImage(bytes);
            if (pointFilter) tex.filterMode = FilterMode.Point;
            int w = tex.width, h = tex.height;
            output = new Sprite[w * h / spriteWidth / spriteHeight];

            int count = 0;
            for (int i = h - spriteHeight; i >= 0; i -= spriteHeight)
            {
                for (int j = 0; j < w; j += spriteWidth)
                {
                    Sprite sprite = Sprite.Create(tex, new Rect(j, i, spriteWidth, spriteHeight), new Vector2(0.5f, 0.5f), pixelsPerUnit, 0, SpriteMeshType.Tight, new Vector4(border, border, border, border));
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
            File.WriteAllText(rootPath + fileName, text);
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
