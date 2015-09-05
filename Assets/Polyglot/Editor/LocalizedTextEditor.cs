#if UNITY_5
using JetBrains.Annotations;
#endif
using UnityEditor;

namespace Polyglot
{
#if UNITY_5
    [UsedImplicitly]
#endif
    [CustomEditor(typeof(LocalizedText))]
    public class LocalizedTextEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var localized = target as LocalizedText;
            if (!string.IsNullOrEmpty(localized.Key))
            {
                EditorGUILayout.LabelField("Localized", LocalizationManager.Get(localized.Key));
                localized.OnLocalize();
            }
        }
    }
}