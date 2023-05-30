using System.Collections.Generic;
using Framework.Managers;

namespace ModdingAPI
{
    internal class Localizer
    {
        private readonly Dictionary<string, Dictionary<string, string>> localizationByLanguage;

        public Localizer(string[] localizationText)
        {
            localizationByLanguage = new Dictionary<string, Dictionary<string, string>>();
            if (localizationText != null)
                setupLocalization(localizationText);
        }

        private void setupLocalization(string[] localizationText)
        {
            string currLangKey = null;
            for (int i = 0; i < localizationText.Length; i++)
            {
                // Skip lines without colon
                int colonIdx = localizationText[i].IndexOf(':');
                if (colonIdx < 0)
                    continue;

                // Get key and term of each pair
                string key = localizationText[i].Substring(0, colonIdx);
                string term = localizationText[i].Substring(colonIdx + 1).Trim();

                // Set new language
                if (key == "lang")
                {
                    currLangKey = term;
                    localizationByLanguage.Add(term, new Dictionary<string, string>());
                    continue;
                }

                // If currently on a language, add the key term pair
                if (currLangKey != null)
                {
                    localizationByLanguage[currLangKey].Add(key, term.Replace("\\n", "\n"));
                }
            }
        }

        public string Localize(string key)
        {
            string langKey = Core.Localization.GetCurrentLanguageCode();

            // The language exists and contains the specified key
            if (localizationByLanguage.ContainsKey(langKey) && localizationByLanguage[langKey].ContainsKey(key))
            {
                return localizationByLanguage[langKey][key];
            }

            // The language doesn't exist - default to english
            if (localizationByLanguage.ContainsKey("en") && localizationByLanguage["en"].ContainsKey(key))
            {
                return localizationByLanguage["en"][key];
            }

            return "LOC_ERROR";
        }
    }
}
