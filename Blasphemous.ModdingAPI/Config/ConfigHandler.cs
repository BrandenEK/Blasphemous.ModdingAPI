using Newtonsoft.Json;
using System.Collections.Generic;

namespace Blasphemous.ModdingAPI.Config;

/// <summary>
/// Provides access to saving and loading configuration properties
/// </summary>
public class ConfigHandler
{
    private readonly BlasMod _mod;

    private bool _registered = false;
    private readonly Dictionary<string, object> _properties = [];

    internal ConfigHandler(BlasMod mod) => _mod = mod;

    /// <summary>
    /// Loads the properties from the config file
    /// </summary>
    public T Load<T>() where T : new()
    {
        string contents = _mod.FileHandler.LoadConfig();

        if (contents == string.Empty)
        {
            T config = new();
            Save(config);
            return config;
        }

        return JsonConvert.DeserializeObject<T>(contents);
    }

    /// <summary>
    /// Saves the current properties to the config file
    /// </summary>
    public void Save<T>(T config)
    {
        _mod.FileHandler.SaveConfig(JsonConvert.SerializeObject(config, Formatting.Indented));
    }
}
