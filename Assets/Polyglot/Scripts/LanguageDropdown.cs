#if UNITY_5
using JetBrains.Annotations;
#endif
using UnityEngine;
using UnityEngine.UI;

namespace Polyglot
{
#if UNITY_5_2
    [RequireComponent(typeof(Dropdown))]
#endif
    [AddComponentMenu("UI/Language Dropdown", 36)]
    public class LanguageDropdown : MonoBehaviour, ILocalize
    {
#if UNITY_5_2 || UNITY_5_3
        [Tooltip("The dropdown to populate with all the available languages")]

        [SerializeField]
        private Dropdown dropdown;

#if UNITY_5
        [UsedImplicitly]
#endif
        public void Reset()
        {
            dropdown = GetComponent<Dropdown>();
        }

#if UNITY_5
        [UsedImplicitly]
#endif
        public void Start()
        {
            if (!LocalizationManager.HasInstance)
            {
                Debug.LogWarning("LocalizationManager missing", this);
                return;
            }

            CreateDropdown();

            LocalizationManager.Instance.AddOnLocalizeEvent(this);
        }

        private void CreateDropdown()
        {
            var flags = dropdown.hideFlags;
            dropdown.hideFlags = HideFlags.DontSaveInEditor;

            dropdown.options.Clear();

            var languageNames = LocalizationManager.Instance.EnglishLanguageNames;

            for (int index = 0; index < languageNames.Count; index++)
            {
                var languageName = languageNames[index];
                dropdown.options.Add(new Dropdown.OptionData(languageName));
            }

            dropdown.value = -1;
            dropdown.value = (int)LocalizationManager.Instance.SelectedLanguage;

            dropdown.hideFlags = flags;
        }

#endif
        public void OnLocalize()
        {
#if UNITY_5_2
            dropdown.onValueChanged.RemoveListener(LocalizationManager.Instance.SelectLanguage);
            dropdown.value = (int)LocalizationManager.Instance.SelectedLanguage;
            dropdown.onValueChanged.AddListener(LocalizationManager.Instance.SelectLanguage);
#endif
        }
    }
}
