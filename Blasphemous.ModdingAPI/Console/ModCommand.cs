using Gameplay.UI.Widgets;
using System;
using System.Collections.Generic;

namespace Blasphemous.ModdingAPI.Console;

/// <summary>
/// A command that can be used in the console for this mod/>
/// </summary>
public abstract class ModCommand
{
    private ConsoleWidget console;

    private Dictionary<string, Action<string[]>> availableCommands;

    /// <summary>
    /// The prefix used to call this command
    /// </summary>
    protected internal abstract string CommandName { get; }

    /// <summary>
    /// Whether to keep uppercase letters in the command or convert everything to lowercase
    /// </summary>
    protected internal abstract bool AllowUppercase { get; }

    /// <summary>
    /// Validates the number of parameters passed in and displays an error if incorrect
    /// </summary>
    protected bool ValidateParameterList(string[] parameters, int amount)
    {
        bool result = amount == parameters.Length;
        if (!result)
        {
            Write($"This command takes {amount} parameters.  You passed {parameters.Length}");
        }
        return result;
    }

    /// <summary>
    /// Parses a parameter into an integer and validates its value
    /// </summary>
    protected bool ValidateIntParameter(string parameter, int minValue, int maxValue, out int result)
    {
        bool flag = int.TryParse(parameter, out result) && result >= minValue && result <= maxValue;
        if (!flag)
        {
            Write($"The parameter {parameter} must be an int between {minValue} and {maxValue}");
        }
        return flag;
    }

    /// <summary>
    /// Parses a parameter into a float and validates its value
    /// </summary>
    protected bool ValidateFloatParameter(string parameter, float minValue, float maxValue, out float result)
    {
        bool flag = float.TryParse(parameter, out result) && result >= minValue && result <= maxValue;
        if (!flag)
        {
            Write($"The parameter {parameter} must be a float between {minValue} and {maxValue}");
        }
        return flag;
    }

    /// <summary>
    /// Validates the length of a string parameter
    /// </summary>
    protected bool ValidateStringParameter(string parameter, int minLength, int maxLength)
    {
        bool flag = parameter.Length >= minLength && parameter.Length <= maxLength;
        if (!flag)
        {
            Write($"The parameter {parameter} must have a length between {minLength} and {maxLength}");
        }
        return flag;
    }

    /// <summary>
    /// Writes a line of text to the console
    /// </summary>
    protected void Write(string text)
    {
        console.Write(text);
    }

    /// <summary>
    /// Initializes a mapping of command names to functionality
    /// </summary>
    protected abstract Dictionary<string, Action<string[]>> AddSubCommands();

    internal void ProcessCommand(ConsoleWidget console, string command, string[] parameters)
    {
        this.console = console;
        availableCommands ??= AddSubCommands();

        if (command == null || !availableCommands.ContainsKey(command))
        {
            Write($"Command unknown, use {CommandName} help");
            return;
        }
        availableCommands[command](parameters);
    }
}
