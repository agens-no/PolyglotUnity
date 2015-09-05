#if UNITY_5
using JetBrains.Annotations;
#endif
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

namespace Polyglot
{
    public enum Language
    {
        English,
        French,
        Spanish,
        German,
        Italian,
        Portuguese_Brazil,
        Portuguese,
        Russian,
        Greek,
        Turkish,
        Danish,
        Norwegian,
        Swedish,
        Dutch,
        Polish,
        Finnish,
        Japanese,
        Simplified_Chinese,
        Traditional_Chinese,
        Korean,
        Czech,
        Hungarian,
        Romanian,
        Thai,
        Bulgarian,
        Hebrew,
        Malay,
        Indonesian,
        Arabic

    }

    public class LocalizationManager : MonoBehaviour
    { 
        [Tooltip("The comma separated text files to get localization strings from")]
        [SerializeField]
        private List<TextAsset> csvFiles;

        /// <summary>
        /// A dictionary with the key of the text item you want and a list of all the languages.
        /// </summary>
        private Dictionary<string, List<string>> languageStrings = new Dictionary<string, List<string>>(); 

        private static LocalizationManager instance;

        /// <summary>
        /// The singleton instance of this manager.
        /// </summary>
        public static LocalizationManager Instance {  get { return instance; } set { instance = value; } }

        public static bool HasInstance { get { return Instance != null; }  }

        [Tooltip("The currently selected language of the game.\nThis will also be the default when you start the game for the first time.")]
        [SerializeField]
        private Language selectedLanguage = Language.English;

        [Tooltip("If we cant find the string for the selected language we fall back to this language.")]
        [SerializeField]
        private Language fallbackLanguage = Language.English;

        private static List<string> EmptyList = new List<string>();

        private static Dictionary<string, List<string>> EmptyDictionary = new Dictionary<string, List<string>>();

        [Tooltip("This event is invoked every time the selected language is changed.")]
        public UnityEvent Localize = new UnityEvent();

        public Language SelectedLanguage
        {
            get
            {
                return selectedLanguage;
            }
            set
            {
                selectedLanguage = value;
                InvokeOnLocalize();
            }
        }

        public void InvokeOnLocalize()
        {
            if (HasInstance && Instance.Localize != null)
            {
                Instance.Localize.Invoke();
            }
        }

#if UNITY_5
        [UsedImplicitly]
#endif
        public void Awake()
        {
            Instance = this;

            PopulateLanguageStrings();
        }

        /// <summary>
        /// The english names of all available languages.
        /// </summary>
        public List<string> EnglishLanguageNames { get { return GetLanguages("MENU_LANGUAGE_THIS_EN"); } }

        /// <summary>
        /// The localized names of all available languages.
        /// </summary>
        public List<string> LocalizedLanguageNames { get { return GetLanguages("MENU_LANGUAGE_THIS"); } }

        /// <summary>
        /// The english name of the selected language.
        /// </summary>
        public string EnglishLanguageName { get { return Get("MENU_LANGUAGE_THIS_EN"); } }

        /// <summary>
        /// The Localized name of the selected language.
        /// </summary>
        public string LocalizedLanguageName { get { return Get("MENU_LANGUAGE_THIS"); } }

        /// <summary>
        /// Select a language, used by dropdowns and the like.
        /// </summary>
        /// <param name="selected"></param>
        public void SelectLanguage(int selected)
        {
            SelectedLanguage = (Language)selected;
        }

        /// <summary>
        /// Add a Localization listener to catch the event that is invoked when the selected language is changed.
        /// </summary>
        /// <param name="localize"></param>
        public void AddOnLocalizeEvent(ILocalize localize)
        {
            Localize.RemoveListener(localize.OnLocalize);
            Localize.AddListener(localize.OnLocalize);
            localize.OnLocalize();
        }

        /// <summary>
        /// Retreives the correct language string by key.
        /// </summary>
        /// <param name="key">The key string</param>
        /// <returns>A localized string</returns>
        public static string Get(string key)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                Instance = FindObjectOfType<LocalizationManager>();
            }
#endif

            if (!HasInstance)
            {
                Debug.LogWarning("LocalizationManager is missing");
                return string.Empty;
            }

            var languages = GetLanguages(key);
            var selected = (int) Instance.selectedLanguage;
            if (languages.Count > 0 && Instance.selectedLanguage >= 0 && selected < languages.Count)
            {
                var currentString = languages[selected];
                if (string.IsNullOrEmpty(currentString) || IsLineBreak(currentString))
                {
                    Debug.LogWarning("Could not find key " + key + " for current language " + Instance.selectedLanguage + ". Falling back to " + Instance.fallbackLanguage + " with " + languages[(int)Instance.fallbackLanguage]);
                    currentString = languages[(int)Instance.fallbackLanguage];
                }
                return currentString;
            }

            return string.Empty;
        }

        private static bool IsLineBreak(string currentString)
        {
            return currentString.Length == 1 && (currentString[0] == '\r' || currentString[0] == '\n');
        }

        public static Dictionary<string, List<string>> GetLanguagesStartsWith(string key)
        {
            if (!HasInstance)
            {
                return EmptyDictionary;
            }

            if (Instance.languageStrings == null || Instance.languageStrings.Count == 0)
            {
                Instance.PopulateLanguageStrings();
            }

            var multipleLanguageStrings = new Dictionary<string, List<string>>();
            foreach (var languageString in Instance.languageStrings)
            {
                if (languageString.Key.ToLower().StartsWith(key.ToLower()))
                {
                    multipleLanguageStrings.Add(languageString.Key, languageString.Value);
                }
            }

            return multipleLanguageStrings;
        }

        private static List<string> GetLanguages(string key)
        {
            if (!HasInstance)
            {
                return EmptyList;
            }

            if (Instance.languageStrings == null || Instance.languageStrings.Count == 0)
            {
                Instance.PopulateLanguageStrings();
            }

            if (!Instance.languageStrings.ContainsKey(key))
            {
                return EmptyList;
            }

            return Instance.languageStrings[key];
        }

        private void PopulateLanguageStrings()
        {
            for (int i = 0; i < csvFiles.Count; i++)
            {
                var textAsset = csvFiles[i];
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

    }
}