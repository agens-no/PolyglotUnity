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

        public static void Init(List<TextAsset> csvFiles)
        {
            CSVFiles.Clear();
            CSVFiles.AddRange(csvFiles);
        }

        private static void PopulateLanguageStrings()
        {
            for (int i = 0; i < CSVFiles.Count; i++)
            {
                var textAsset = CSVFiles[i];
                var text = textAsset.text;
                var lines = text.Split('\n');
                var canBegin = false;
                for (int index = 0; index < lines.Length; index++)
                {
                    var line = lines[index];

                    if (!canBegin)
                    {
                        if (line.StartsWith("PolyMaster") || line.StartsWith("BEGIN"))
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
                        Debug.LogWarning("Duplicate key: " + key);
                        continue;
                    }
                    languageStrings.Add(key, keys);
                }
            }
        }

        public static bool IsLineBreak(string currentString)
        {
            return currentString.Length == 1 && (currentString[0] == '\r' || currentString[0] == '\n');
        }

        public static List<string> GetLanguages(string key)
        {
            if (languageStrings == null || languageStrings.Count == 0)
            {
                PopulateLanguageStrings();
            }

            if (!languageStrings.ContainsKey(key))
            {
                return EmptyList;
            }

            return languageStrings[key];
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
    }
}