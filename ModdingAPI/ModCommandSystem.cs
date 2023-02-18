using System.Collections.Generic;
using Gameplay.UI.Console;

namespace ModdingAPI
{
    internal class ModCommandSystem : ConsoleCommand
    {
        private ModCommand command;

        public ModCommandSystem(ModCommand command)
        {
            this.command = command;
            command.setConsole(Console);
        }

        public override string GetName()
        {
            return command.CommandName;
        }

        public override bool ToLowerAll()
        {
            return !command.AllowUppercase;
        }

        public override void Execute(string command, string[] parameters)
        {
            if (command == null || command != this.command.CommandName) return;

            string subcommand = GetSubcommand(parameters, out List<string> paramList);
            if (subcommand == null)
            {
                Console.Write($"Command unknown, use {this.command.CommandName} help");
                return;
            }

            this.command.ProcessCommand(subcommand, paramList.ToArray());
        }
    }
}
