using Gameplay.UI.Console;
using System.Collections.Generic;

namespace Blasphemous.ModdingAPI.Console;

internal class ModCommandSystem(ModCommand command) : ConsoleCommand
{
    public override string GetName()
    {
        return command.CommandName;
    }

    public override bool ToLowerAll()
    {
        return !command.AllowUppercase;
    }

    public override bool HasLowerParameters()
    {
        return !command.AllowUppercase;
    }

    public override void Execute(string cmd, string[] parameters)
    {
        if (cmd == null || cmd != command.CommandName) return;

        string subcommand = GetSubcommand(parameters, out List<string> paramList);
        command.ProcessCommand(Console, subcommand, paramList.ToArray());
    }
}
