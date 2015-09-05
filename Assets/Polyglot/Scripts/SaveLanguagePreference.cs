#if UNITY_5
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
        public IEnumerator Start()
        {
            while (!LocalizationManager.HasInstance)
            {
                yield return null;
            }
            LocalizationManager.Instance.SelectedLanguage = (Language) PlayerPrefs.GetInt(preferenceKey);
            LocalizationManager.Instance.AddOnLocalizeEvent(this);
        }

        public void OnLocalize()
        {
            PlayerPrefs.SetInt(preferenceKey, (int) LocalizationManager.Instance.SelectedLanguage);
        }
    }
}