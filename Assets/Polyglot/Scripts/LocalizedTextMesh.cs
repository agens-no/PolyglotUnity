#if UNITY_5
using JetBrains.Annotations;
#endif
using UnityEngine;

namespace Polyglot
{
    [AddComponentMenu("Mesh/Localized TextMesh")]
    [RequireComponent(typeof(TextMesh))]
    public class LocalizedTextMesh : MonoBehaviour, ILocalize
    {

        [Tooltip("The TextMesh component to localize")]
        [SerializeField]
        private TextMesh text;

        [Tooltip("The key to localize with")]
        [SerializeField]
        private string key;

        public string Key { get { return key; } }

#if UNITY_5
        [UsedImplicitly]
#endif
        public void Reset()
        {
            text = GetComponent<TextMesh>();
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
            text.hideFlags = flags;
        }
    }
}