# Localization

Every mod has a 'LocalizationHandler' property, which is used to translate text and load translations from the localization folder

---

```cs
using Blasphemous.ModdingAPI;

namespace Blasphemous.Example;

public class ExampleMod : BlasMod
{
    public ExampleMod() : base(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION) { }

    protected override void OnInitialize()
    {
        LocalizationHandler.RegisterDefaultLanguage("en");
    }

    protected override void OnLevelLoaded(string oldLevel, string newLevel)
    {
        if (newLevel == "D08Z01S01")
        {
            LogDisplay(LocalizationHandler.Localize("brname"));
        }
    }
}
```