using System.Collections.Generic;
using Framework.Managers;

namespace ModdingAPI
{
    internal class Localizer
    {
        private Dictionary<string, string[]> localizationByLanguage;

        public Localizer(string[] localizationText)
        {
            localizationByLanguage = new Dictionary<string, string[]>();
            if (localizationText != null)
                setupLocalization(localizationText);
            
            if (!localizationByLanguage.ContainsKey("keys"))
                localizationByLanguage.Add("keys", new string[0]);
        }

        private void setupLocalization(string[] localizationText)
        {
            for (int i = 0; i < localizationText.Length; i++)
            {
                int colonIdx = localizationText[i].IndexOf(':');
                string langKey = localizationText[i].Substring(0, colonIdx);
                string[] langTerms = localizationText[i].Substring(colonIdx + 1).Trim().Split('~');
                localizationByLanguage.Add(langKey, langTerms);
            }
        }

        public string Localize(string key)
        {
            string langKey = Core.Localization.GetCurrentLanguageCode();
            if (!localizationByLanguage.ContainsKey(langKey))
            {
                if (!localizationByLanguage.ContainsKey("en"))
                    return "LOC_ERROR";
                langKey = "en";
            }

            string[] keyTerms = localizationByLanguage["keys"];
            string[] langTerms = localizationByLanguage[langKey];

            for (int i = 0; i < keyTerms.Length; i++)
            {
                if (keyTerms[i] == key)
                {
                    return (i < langTerms.Length && langTerms[i] != null && langTerms[i] != "") ? langTerms[i] : "LOC_ERROR";
                }
            }

            return "LOC_ERROR";
        }
    }
}
