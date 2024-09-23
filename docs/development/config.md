# Config

Every mod has a 'ConfigHandler' property, which is used to save and load configuration settings from the config folder

---

```cs
using Blasphemous.ModdingAPI;

namespace Blasphemous.Example;

public class ExampleMod : BlasMod
{
    public ExampleMod() : base(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION) { }

    public ExampleConfig Settings { get; private set; }

    protected override void OnInitialize()
    {
        Settings = ConfigHandler.Load<ExampleConfig>();

        LogWarning($"Flag: {Settings.ExampleFlag}");
    }
}

public class ExampleConfig
{
    public bool ExampleFlag { get; set; }
    public int ExampleAmount { get; set; }

    public ExampleConfig()
    {
        ExampleFlag = true;
        ExampleAmount = 2;
    }
}
```