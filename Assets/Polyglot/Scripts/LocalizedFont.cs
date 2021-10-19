#if UNITY_5 || UNITY_2017_1_OR_NEWER
using JetBrains.Annotations;
#endif
using UnityEngine;
using UnityEngine.UI;

namespace Polyglot
{
    [AddComponentMenu("UI/Localized Font")]
    [RequireComponent(typeof(Text))]
    public class LocalizedFont : MonoBehaviour, ILocalize
    {
        [Tooltip ("The text component to localize")]
        [SerializeField]
        private Text text;

        private bool initializedReferenceFont;
        private Font referenceFont;

#if UNITY_5 || UNITY_2017_1_OR_NEWER
        [UsedImplicitly]
#endif
        public void Reset ()
        {
            text = GetComponent<Text> ();
        }

#if UNITY_5 || UNITY_2017_1_OR_NEWER
        [UsedImplicitly]
#endif
        public void OnEnable ()
        {
            Localization.Instance.AddOnLocalizeEvent(this);
        }

        public void OnDisable ()
        {
            Localization.Instance.RemoveOnLocalizeEvent(this);
        }

        public void OnLocalize ()
        {
            if (!Application.isPlaying)
                return;

            if (text == null)
            {
                Debug.LogWarning ("Missing Text Component on " + gameObject, gameObject);
                return;
            }

            if (!initializedReferenceFont)
            {
                referenceFont = text.font;
                initializedReferenceFont = true;
            }

            text.font = Localization.GetFont (referenceFont);
        }

        public void SetLanguage (Language language)
        {
            text.font = Localization.GetFont (referenceFont, language);
        }
    }
}
