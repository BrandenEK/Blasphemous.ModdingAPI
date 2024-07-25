# Persistence

Every mod can define custom save data that will be stored with the game's save file

---

```cs
using Blasphemous.ModdingAPI;
using Blasphemous.ModdingAPI.Persistence;

namespace Blasphemous.Example;

public class ExampleMod : BlasMod, IPersistentMod
{
    public ExampleMod() : base(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION) { }

    public string PersistentID => "ID_EXAMPLE";

    public int Amount { get; private set; }
    public string[] Values { get; private set; }

    public void LoadGame(SaveData data)
    {
        var exampleData = data as ExampleSaveData;

        Amount = exampleData.saveAmount;
        Values = exampleData.saveValues;
    }

    public SaveData SaveGame()
    {
        return new ExampleSaveData()
        {
            saveAmount = Amount,
            saveValues = Values
        };
    }

    public void ResetGame()
    {
        Amount = 0;
        Values = [];
    }
}

public class ExampleSaveData : SaveData
{
    public ExampleSaveData() : base("ID_EXAMPLE") { }

    public int saveAmount;
    public string[] saveValues;
}
```