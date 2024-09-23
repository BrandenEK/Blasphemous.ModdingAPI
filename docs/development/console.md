# Console

Every mod can define custom console commands that can be used in the in-game console

---

```cs
using Blasphemous.ModdingAPI;
using Blasphemous.ModdingAPI.Console;
using System;
using System.Collections.Generic;

namespace Blasphemous.Example;

public class ExampleMod : BlasMod
{
    public ExampleMod() : base(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION) { }

    protected override void OnRegisterServices(ModServiceProvider provider)
    {
        provider.RegisterCommand(new ExampleCommand());
    }
}

public class ExampleCommand : ModCommand
{
    protected override string CommandName => "example";

    protected override bool AllowUppercase => false;

    protected override Dictionary<string, Action<string[]>> AddSubCommands()
    {
        return new Dictionary<string, Action<string[]>>()
        {
            { "help", Help },
            { "action", PerformAction },
        };
    }

    private void Help(string[] args)
    {
        if (!ValidateParameterList(args, 0)) return;

        Write("Available EXAMPLE commands:");
        Write("example action: Performs an example action");
    }

    private void PerformAction(string[] args)
    {
        if (!ValidateParameterList(args, 1)) return;

        Write($"Performing action with {args[0]}");
    }
}
```