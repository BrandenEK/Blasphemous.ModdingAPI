using System.Collections.Generic;
using Gameplay.UI.Console;

namespace ModdingAPI.Commands
{
    internal class ModCommandSystem : ConsoleCommand
    {
        private ModCommand command;

        public ModCommandSystem(ModCommand command)
        {
            this.command = command;
        }

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

        public override void Execute(string command, string[] parameters)
        {
            if (command == null || command != this.command.CommandName) return;

            string subcommand = GetSubcommand(parameters, out List<string> paramList);
            this.command.ProcessCommand(Console, subcommand, paramList.ToArray());
        }
    }
}
