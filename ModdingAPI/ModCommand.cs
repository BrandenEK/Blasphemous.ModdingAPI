using Gameplay.UI.Widgets;

namespace ModdingAPI
{
    /// <summary>
    /// A command that can be used in the console for this mod/>
    /// </summary>
    public abstract class ModCommand
    {
        /// <summary>
        /// The prefix used to call this command
        /// </summary>
        protected internal abstract string CommandName { get; }

        /// <summary>
        /// Whether to keep uppercase letters in the command or convert everything to lowercase
        /// </summary>
        protected internal abstract bool AllowUppercase { get; }

        /// <summary>
        /// Perform an action based on the command and parameters passed in by the user
        /// </summary>
        /// <param name="command">The command entered</param>
        /// <param name="parameters">The list of parameters passed in</param>
        protected internal abstract void ProcessCommand(string command, string[] parameters);

        /// <summary>
        /// Validates the number of parameters passed in and displays an error if incorrect
        /// </summary>
        /// <param name="command">The name of the command</param>
        /// <param name="parameters">The list of paramaters</param>
        /// <param name="amount">The correct amount of parameters</param>
        /// <returns></returns>
        protected bool ValidateParameterList(string command, string[] parameters, int amount)
        {
            bool result = amount == parameters.Length;
            if (!result)
            {
                Write($"The {CommandName} command {command} takes {amount} parameters.  You passed {parameters.Length}");
            }
            return result;
        }

        /// <summary>
        /// Parses a parameter into an integer and validates its value
        /// </summary>
        /// <param name="parameter">The parameter passed in</param>
        /// <param name="minValue">The minimum acceptable value</param>
        /// <param name="maxValue">The maxmimum acceptable value</param>
        /// <param name="result">The resulting integer</param>
        /// <returns>Whether the parameter is valid or not</returns>
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
        /// <param name="parameter">The parameter passed in</param>
        /// <param name="minValue">The minimum acceptable value</param>
        /// <param name="maxValue">The maxmimum acceptable value</param>
        /// <param name="result">The resulting float</param>
        /// <returns>Whether the parameter is valid or not</returns>
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
        /// <param name="parameter">The parameter passed in</param>
        /// <param name="minLength">The minimum acceptable length</param>
        /// <param name="maxLength">The maxmimum acceptable length</param>
        /// <returns>Whether the parameter is valid or not</returns>
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
        /// <param name="text">The text to write</param>
        protected void Write(string text)
        {
            console.Write(text);
        }

        private ConsoleWidget console;
        internal void setConsole(ConsoleWidget console) { this.console = console; }
    }
}
