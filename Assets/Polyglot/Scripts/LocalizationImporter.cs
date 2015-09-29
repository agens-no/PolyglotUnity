using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Polyglot
{
    public static class LocalizationImporter
    {
        /// <summary>
        /// A dictionary with the key of the text item you want and a list of all the languages.
        /// </summary>
        private static Dictionary<string, List<string>> languageStrings = new Dictionary<string, List<string>>();

        private static List<string> EmptyList = new List<string>();

        private static List<TextAsset> CSVFiles = new List<TextAsset>();

        static LocalizationImporter()
        {
            var settings = Localization.Instance;
            if (settings == null)
            {
                Debug.LogError("Could not find a Localization Settings file in Resources.");
                return;
            }
            Init(settings.CSVFiles);
        }

        private static void Init(List<TextAsset> csvFiles)
        {
            CSVFiles.Clear();
            CSVFiles.AddRange(csvFiles);
        }

        private static void PopulateLanguageStrings()
        {
            for (var index = 0; index < CSVFiles.Count; index++)
            {
                var textAsset = CSVFiles[index];

                if (textAsset == null)
                {
                    Debug.LogError("CSVFiles[" + index + "] is null");
                    continue;
                }

                var text = textAsset.text;
                var lines = text.Split('\n');
                var canBegin = false;
                for (int lineIndex = 0; lineIndex < lines.Length; lineIndex++)
                {
                    var line = lines[lineIndex];

                    if (!canBegin)
                    {
                        if (line.StartsWith("Polyglot") || line.StartsWith("PolyMaster") || line.StartsWith("BEGIN"))
                        {
                            canBegin = true;
                            continue;
                        }
                    }

                    if (!canBegin)
                    {
                        continue;
                    }

                    if (line.StartsWith("END"))
                    {
                        break;
                    }

                    var keys = line.Split(',').ToList();
                    var key = keys[0];

                    if (string.IsNullOrEmpty(key) || IsLineBreak(key) || keys.Count <= 1)
                    {
                        //Ignore empty lines in the sheet
                        continue;
                    }

                    //Remove key
                    keys.RemoveAt(0);
                    //Remove description
                    keys.RemoveAt(0);

                    if (languageStrings.ContainsKey(key))
                    {
                        Debug.Log("The key '" + key + "' already exist, but is now overwritten by a csv (" + textAsset.name + ") with higher priority (" + index + ")");
                        languageStrings[key] = keys;
                        continue;
                    }
                    languageStrings.Add(key, keys);
                }
            }
        }

        /// <summary>
        /// Checks if the current string is \r or \n
        /// </summary>
        /// <param name="currentString"></param>
        /// <returns></returns>
        public static bool IsLineBreak(string currentString)
        {
            return currentString.Length == 1 && (currentString[0] == '\r' || currentString[0] == '\n')
                || currentString.Length == 2 && currentString.Equals(Environment.NewLine);
        }

        /// <summary>
        /// Get all language strings on a given key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static List<string> GetLanguages(string key, List<Language> supportedLanguages = null)
        {
            if (languageStrings == null || languageStrings.Count == 0)
            {
                PopulateLanguageStrings();
            }

            if (string.IsNullOrEmpty(key) || !languageStrings.ContainsKey(key))
            {
                return EmptyList;
            }

            if (supportedLanguages == null || supportedLanguages.Count == 0)
            {
                return languageStrings[key];
            }

            // Filter the supported languages down to the supported ones
            var supportedLanguageStrings = new List<string>(supportedLanguages.Count);
            for (int index = 0; index < supportedLanguages.Count; index++)
            {
                var language = supportedLanguages[index];
                var supportedLanguage = (int) language;
                supportedLanguageStrings.Add(languageStrings[key][supportedLanguage]);
            }
            return supportedLanguageStrings;
        }

        public static Dictionary<string, List<string>> GetLanguagesStartsWith(string key)
        {
            if (languageStrings == null || languageStrings.Count == 0)
            {
                PopulateLanguageStrings();
            }

            var multipleLanguageStrings = new Dictionary<string, List<string>>();
            foreach (var languageString in languageStrings)
            {
                if (languageString.Key.ToLower().StartsWith(key.ToLower()))
                {
                    multipleLanguageStrings.Add(languageString.Key, languageString.Value);
                }
            }

            return multipleLanguageStrings;
        }

        public static Dictionary<string, List<string>> GetLanguagesContains(string key)
        {
            if (languageStrings == null || languageStrings.Count == 0)
            {
                PopulateLanguageStrings();
            }

            var multipleLanguageStrings = new Dictionary<string, List<string>>();
            foreach (var languageString in languageStrings)
            {
                if (languageString.Key.ToLower().Contains(key.ToLower()))
                {
                    multipleLanguageStrings.Add(languageString.Key, languageString.Value);
                }
            }

            return multipleLanguageStrings;
        }

        public static void Refresh()
        {
            languageStrings.Clear();
            PopulateLanguageStrings();
        }
    }
}