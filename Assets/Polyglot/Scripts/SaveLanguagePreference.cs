﻿#if UNITY_5
using JetBrains.Annotations;
#endif
using UnityEngine;
using System.Collections;

namespace Polyglot
{
    public class SaveLanguagePreference : MonoBehaviour, ILocalize
    {
        [SerializeField]
        private string preferenceKey = "Polyglot.SelectedLanguage";

#if UNITY_5
        [UsedImplicitly]
#endif
        private void Awake()
        {
            LocalizationImporter.ImportFromGoogle(Localization.Instance, this);
            DontDestroyOnLoad(gameObject);
        }
        public void Start()
        {
            Localization.Instance.SelectedLanguage = (Language) PlayerPrefs.GetInt(preferenceKey);
            Localization.Instance.AddOnLocalizeEvent(this);
        }

        public void OnLocalize()
        {
            PlayerPrefs.SetInt(preferenceKey, (int) Localization.Instance.SelectedLanguage);
        }
    }
}