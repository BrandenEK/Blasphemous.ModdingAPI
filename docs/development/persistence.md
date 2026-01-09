# Persistence

Every mod can define custom save data that can be saved globally or with a single save slot

---

Slot data is stored at ```APPDATA_LOCAL_LOW\TheGameKitchen\Blasphemous\Savegames\STEAM_ID\savegame_x_modded.save```

```cs
using Blasphemous.ModdingAPI;
using Blasphemous.ModdingAPI.Persistence;

namespace Blasphemous.Example;

public class ExampleMod : BlasMod, ISlotPersistentMod<ExampleSlotData>
{
    public ExampleMod() : base(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION) { }

    public int Amount { get; private set; }
    public string[] Values { get; private set; }

    public void LoadSlot(ExampleSlotData data)
    {
        Amount = data.saveAmount;
        Values = data.saveValues;
    }

    public ExampleSaveData SaveSlot()
    {
        return new ExampleSaveData()
        {
            saveAmount = Amount,
            saveValues = Values
        };
    }

    public void ResetSlot()
    {
        Amount = 0;
        Values = [];
    }
}

public class ExampleSlotData : SlotSaveData
{
    public int saveAmount;
    public string[] saveValues;
}
```

Global data is stored at ```APPDATA_LOCAL_LOW\TheGameKitchen\Blasphemous\Savegames\app_settings_modded```

```cs
using Blasphemous.ModdingAPI;
using Blasphemous.ModdingAPI.Persistence;

namespace Blasphemous.Example;

public class ExampleMod : BlasMod, IGlobalPersistentMod<ExampleGlobalData>
{
    public ExampleMod() : base(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION) { }

    public int Amount { get; private set; }
    public string[] Values { get; private set; }

    public void LoadGlobal(ExampleGlobalData data)
    {
        Amount = data.saveAmount;
        Values = data.saveValues;
    }

    public ExampleGlobalData SaveGlobal()
    {
        return new ExampleGlobalData()
        {
            saveAmount = Amount,
            saveValues = Values
        };
    }
}

public class ExampleGlobalData : GlobalSaveData
{
    public int saveAmount;
    public string[] saveValues;
}
```