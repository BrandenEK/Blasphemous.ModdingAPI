using Framework.Managers;
using System.Collections.Generic;

namespace Blasphemous.ModdingAPI.Localization;

/// <summary>
/// Provides access to automatic localization based on selected language
/// </summary>
public class LocalizationHandler
{
    private readonly BlasMod _mod;

    private bool _registered = false;
    private string _defaultLanguage = string.Empty;
    private readonly Dictionary<string, Dictionary<string, string>> _textByLanguage = [];

    internal LocalizationHandler(BlasMod mod) => _mod = mod;

    /// <summary>
    /// Localizes the key into its term in the current language
    /// </summary>
    public string Localize(string key)
    {
        string currentLanguage = Core.Localization.GetCurrentLanguageCode();

        // The language exists and contains the specified key
        if (_textByLanguage.ContainsKey(currentLanguage) && _textByLanguage[currentLanguage].ContainsKey(key))
        {
            return _textByLanguage[currentLanguage][key];
        }

        // The language doesn't exist - use default language
        if (_textByLanguage.ContainsKey(_defaultLanguage) && _textByLanguage[_defaultLanguage].ContainsKey(key))
        {
            return _textByLanguage[_defaultLanguage][key];
        }

        _mod.LogError($"Failed to localize '{key}' to any language.");
        return ERROR_TEXT;
    }

    /// <summary>
    /// Specifies which language to default to and loads the translations
    /// </summary>
    public void RegisterDefaultLanguage(string languageKey)
    {
        if (_registered)
        {
            _mod.LogWarning("LocalizationHandler has already been registered!");
            return;
        }
        _registered = true;

        _defaultLanguage = languageKey;
        DeserializeLocalization(_mod.FileHandler.LoadLocalization());
    }

    /// <summary>
    /// Takes in the lines from the localization file and fills the text dictionary
    /// </summary>
    private void DeserializeLocalization(string[] localization)
    {
        string currLanguage = null;
        foreach (string line in localization)
        {
            // Skip lines without a colon
            int colon = line.IndexOf(':');
            if (colon < 0)
                continue;

            // Get key and term for each pair
            string key = line.Substring(0, colon).Trim();
            string term = line.Substring(colon + 1).Trim();

            // Possibly set new language
            if (key == "lang")
            {
                currLanguage = term;
                _textByLanguage.Add(term, []);
                continue;
            }

            // Make sure the current language has been set
            if (currLanguage == null)
                continue;

            _textByLanguage[currLanguage].Add(key, term.Replace("\\n", "\n"));
        }
    }

    private const string ERROR_TEXT = "LOC_ERROR";
}
