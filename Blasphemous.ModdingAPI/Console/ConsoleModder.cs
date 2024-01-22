using System.Collections.Generic;
using System.Linq;

namespace Blasphemous.ModdingAPI.Console;

/// <summary>
/// Allows registering custom commands that can be run in the console
/// </summary>
public static class ConsoleModder
{
    private static readonly List<ModCommand> _commands = new();
    internal static IEnumerable<ModCommand> AllCommands => _commands;

    /// <summary>
    /// Registers a new console command
    /// </summary>
    public static void RegisterCommand(ModCommand command)
    {
        if (_commands.Any(cmd => cmd.CommandName == command.CommandName))
            return;

        _commands.Add(command);
    }
}
