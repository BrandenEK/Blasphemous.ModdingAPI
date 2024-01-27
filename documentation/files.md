# Files

Every mod has a 'FileHandler' property, which is used to load text, images, etc. from the data folder

---

```cs
using Blasphemous.ModdingAPI;
using Blasphemous.ModdingAPI.Files;
using UnityEngine;

namespace Blasphemous.Example;

public class ExampleMod : BlasMod
{
    public ExampleMod() : base(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION) { }

    private Sprite _exampleImage;

    protected override void OnInitialize()
    {
        FileHandler.LoadDataAsSprite("example.png", out _exampleImage, new SpriteImportOptions()
        {
            PixelsPerUnit = 16,
            Pivot = Vector2.zero
        });
    }
}
```