using Framework.Managers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Blasphemous.ModdingAPI.Persistence;

/// <summary>
/// Used to store data with the global save file
/// </summary>
public class GlobalSaveData
{
    /// <summary>
    /// Saves the global save file for all persistent mods
    /// </summary>
    internal static void Save()
    {
        if (!Main.ModLoader.IsInitialized)
            return;

        ModLog.Debug($"Saving global data");

        var datas = LoadFile();
        var settings = new JsonSerializerSettings()
        {
            ContractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy(false, true)
            },
        };

        Main.ModLoader.ProcessModFunction(mod =>
        {
            Type modType = GetInterfaceType(mod);

            if (modType == null)
                return;

            var save = modType.GetMethod(nameof(IGlobalPersistentMod<GlobalSaveData>.SaveGlobal), BindingFlags.Instance | BindingFlags.Public);
            object data = save.Invoke(mod, []);

            datas[mod.Id] = JsonConvert.SerializeObject(data, settings);
        });

        SaveFile(datas);
    }

    /// <summary>
    /// Saves the json to the global save file
    /// </summary>
    private static void SaveFile(Dictionary<string, string> datas)
    {
        var sb = new StringBuilder();

        foreach (var kvp in datas)
        {
            sb.AppendLine(kvp.Key);
            sb.AppendLine(kvp.Value);
        }

        try
        {
            File.WriteAllText(GetGlobalDataPath(), sb.ToString());
        }
        catch (Exception e)
        {
            ModLog.Error($"Failed to save global data: {e.Message} ({e.GetType()})");
        }
    }

    /// <summary>
    /// Loads the global save file for all persistent mods
    /// </summary>
    internal static void Load()
    {
        if (!Main.ModLoader.IsInitialized)
            return;

        ModLog.Debug($"Loading global data");

        var datas = LoadFile();

        Main.ModLoader.ProcessModFunction(mod =>
        {
            Type modType = GetInterfaceType(mod);

            if (modType == null)
                return;

            Type dataType = modType.GetGenericArguments()[0];

            if (!datas.TryGetValue(mod.Id, out string json))
            {
                ModLog.Warn($"No global data could be found for mod {mod.Id}");
                return;
            }

            GlobalSaveData data = JsonConvert.DeserializeObject(json, dataType) as GlobalSaveData;

            var load = modType.GetMethod(nameof(IGlobalPersistentMod<GlobalSaveData>.LoadGlobal), BindingFlags.Instance | BindingFlags.Public);
            load.Invoke(mod, [data]);
        });
    }

    /// <summary>
    /// Loads the json from the global save file
    /// </summary>
    private static Dictionary<string, string> LoadFile()
    {
        var datas = new Dictionary<string, string>();

        try
        {
            string[] lines = File.ReadAllLines(GetGlobalDataPath());
            for (int i = 0; i < lines.Length - 1; i += 2)
            {
                datas.Add(lines[i], lines[i + 1]);
            }
        }
        catch (Exception e)
        {
            ModLog.Error($"Failed to load global data: {e.Message} ({e.GetType()})");
        }

        return datas;
    }

    /// <summary>
    /// Deletes the global save file
    /// </summary>
    internal static void Delete()
    {
        ModLog.Debug($"Deleting global data");

        try
        {
            string path = GetGlobalDataPath();
            File.Delete(path);
        }
        catch (Exception e)
        {
            ModLog.Error($"Failed to delete global data: {e.Message} ({e.GetType()})");
        }
    }

    /// <summary>
    /// Returns the interface type if the mod implements it
    /// </summary>
    private static Type GetInterfaceType(BlasMod mod)
    {
        return mod.GetType().GetInterfaces()
            .Where(i => i.IsGenericType)
            .FirstOrDefault(i => i.GetGenericTypeDefinition().IsAssignableFrom(typeof(IGlobalPersistentMod<>)));
    }

    /// <summary>
    /// Returns the file path of the global save file
    /// </summary>
    private static string GetGlobalDataPath()
    {
        return PersistentManager.GetPathAppSettings("app_settings_modded");
    }
}
