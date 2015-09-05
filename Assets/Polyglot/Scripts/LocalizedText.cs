
using System;
#if UNITY_5
using JetBrains.Annotations;
#endif
using UnityEngine;
using UnityEngine.UI;

namespace Polyglot
{
    [AddComponentMenu("UI/Localized Text", 11)]
    [RequireComponent(typeof(Text))]
    public class LocalizedText : MonoBehaviour, ILocalize
    {
        [Tooltip("The text component to localize")]
        [SerializeField]
        private Text text;

        [Tooltip("The key to localize with")]
        [SerializeField]
        private string key;

        public string Key
        {
            get
            {
                return key;
            }
            set
            {
                key = value;
                OnLocalize();
            }
        }

#if UNITY_5
        [UsedImplicitly]
#endif
        public void Reset()
        {
            text = GetComponent<Text>();
        }

#if UNITY_5
        [UsedImplicitly]
#endif
        public void Start()
        {
            LocalizationManager.Instance.AddOnLocalizeEvent(this);
        }

        public void OnLocalize()
        {
            var flags = text.hideFlags;
            text.hideFlags = HideFlags.DontSave;
            text.text = LocalizationManager.Get(key);
            var direction = LocalizationManager.Instance.SelectedLanguageDirection;

            if (IsOppositeDirection(text.alignment, direction))
            {
                switch (text.alignment)
                {
                    case TextAnchor.UpperLeft:
                        text.alignment = TextAnchor.UpperLeft;
                        break;
                    case TextAnchor.UpperRight:
                        text.alignment = TextAnchor.UpperRight;
                        break;
                    case TextAnchor.MiddleLeft:
                        text.alignment = TextAnchor.MiddleRight;
                        break;
                    case TextAnchor.MiddleRight:
                        text.alignment = TextAnchor.MiddleLeft;
                        break;
                    case TextAnchor.LowerLeft:
                        text.alignment = TextAnchor.LowerRight;
                        break;
                    case TextAnchor.LowerRight:
                        text.alignment = TextAnchor.LowerLeft;
                        break;
                }
            }
            
            text.hideFlags = flags;
        }

        private bool IsOppositeDirection(TextAnchor alignment, LanguageDirection direction)
        {
            return (direction == LanguageDirection.LeftToRight && IsAlignmentRight(alignment)) || (direction == LanguageDirection.RightToLeft && IsAlignmentLeft(alignment));
        }

        private bool IsAlignmentRight(TextAnchor alignment)
        {
            return alignment == TextAnchor.LowerRight || alignment == TextAnchor.MiddleRight || alignment == TextAnchor.UpperRight;
        }
        private bool IsAlignmentLeft(TextAnchor alignment)
        {
            return alignment == TextAnchor.LowerLeft || alignment == TextAnchor.MiddleLeft || alignment == TextAnchor.UpperLeft;
        }
    }
}