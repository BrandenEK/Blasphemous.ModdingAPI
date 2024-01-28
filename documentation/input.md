# Input

Every mod has an 'InputHandler' property, which is used to handle in-game input and load keybindings from the keybinding folder

---

```cs
using Blasphemous.ModdingAPI;
using Blasphemous.ModdingAPI.Input;
using System.Collections.Generic;
using UnityEngine;

namespace Blasphemous.Example;

public class ExampleMod : BlasMod
{
    public ExampleMod() : base(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION) { }

    protected override void OnInitialize()
    {
        InputHandler.RegisterDefaultKeybindings(new Dictionary<string, KeyCode>()
        {
            { "Key1", KeyCode.F7 },
            { "Key2", KeyCode.F8 },
        });
    }

    protected override void OnUpdate()
    {
        if (InputHandler.GetKeyDown("Key1"))
        {
            LogWarning("Pressed custom keybinding");
        }

        if (InputHandler.GetButtonUp(ButtonCode.Attack))
        {
            LogWarning("Released attack button");
        }
    }
}
```