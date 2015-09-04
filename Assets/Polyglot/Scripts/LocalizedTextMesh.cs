using JetBrains.Annotations;
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

        [UsedImplicitly]
        public void Reset()
        {
            text = GetComponent<TextMesh>();
        }

        [UsedImplicitly]
        public void Start()
        {
            LocalizationManager.Instance.AddOnLocalizeEvent(this);
        }

        public void OnLocalize()
        {
            var flags = text.hideFlags;
            text.hideFlags = HideFlags.DontSaveInEditor;
            text.text = LocalizationManager.Get(key);
            text.hideFlags = flags;
        }
    }
}