
using System;
#if UNITY_5
using JetBrains.Annotations;
#endif
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Polyglot
{
    public class LocalizationManager : MonoBehaviour
    { 
        [Tooltip("The comma separated text files to get localization strings from")]
        [SerializeField]
        private List<TextAsset> csvFiles;

        public List<TextAsset> CSVFiles { get { return csvFiles; } }

        private static LocalizationManager instance;

        /// <summary>
        /// The singleton instance of this manager.
        /// </summary>
        public static LocalizationManager Instance
        {
            get
            {
                if (!HasInstance)
                {
                    Instance = FindObjectOfType<LocalizationManager>();
                }
                return instance;
            }
            set { instance = value; }
        }

        public static bool HasInstance { get { return instance != null; }  }

        [Tooltip("The currently selected language of the game.\nThis will also be the default when you start the game for the first time.")]
        [SerializeField]
        private Language selectedLanguage = Language.English;

        [Tooltip("If we cant find the string for the selected language we fall back to this language.")]
        [SerializeField]
        private Language fallbackLanguage = Language.English;

        [Tooltip("This event is invoked every time the selected language is changed.")]
        public UnityEvent Localize = new UnityEvent();

        public LanguageDirection SelectedLanguageDirection
        {
            get { return GetLanguageDirection(SelectedLanguage); }
        }

        private LanguageDirection GetLanguageDirection(Language language)
        {
            switch (language)
            {
                case Language.Hebrew:
                    return LanguageDirection.RightToLeft;
                case Language.Arabic:
                    return LanguageDirection.RightToLeft;
                default:
                    return LanguageDirection.LeftToRight;
            }
        }

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
        }

        /// <summary>
        /// The english names of all available languages.
        /// </summary>
        public List<string> EnglishLanguageNames { get { return LocalizationImporter.GetLanguages("MENU_LANGUAGE_THIS_EN"); } }

        /// <summary>
        /// The localized names of all available languages.
        /// </summary>
        public List<string> LocalizedLanguageNames { get { return LocalizationImporter.GetLanguages("MENU_LANGUAGE_THIS"); } }

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

            var languages = LocalizationImporter.GetLanguages(key);
            var selected = (int) Instance.selectedLanguage;
            if (languages.Count > 0 && Instance.selectedLanguage >= 0 && selected < languages.Count)
            {
                var currentString = languages[selected];
                if (string.IsNullOrEmpty(currentString) || LocalizationImporter.IsLineBreak(currentString))
                {
                    Debug.LogWarning("Could not find key " + key + " for current language " + Instance.selectedLanguage + ". Falling back to " + Instance.fallbackLanguage + " with " + languages[(int)Instance.fallbackLanguage]);
                    currentString = languages[(int)Instance.fallbackLanguage];
                }
                return currentString;
            }

            return string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static string GetFormat(string key, params object[] arguments)
        {
            if (string.IsNullOrEmpty(key) || arguments == null || arguments.Length == 0)
            {
                return Get(key);
            }

            return string.Format(Get(key), arguments);
        }
    }
}