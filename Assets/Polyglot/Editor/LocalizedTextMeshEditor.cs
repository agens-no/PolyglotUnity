using JetBrains.Annotations;
using UnityEditor;

namespace Polyglot
{
    [UsedImplicitly]
    [CustomEditor(typeof(LocalizedTextMesh))]
    public class LocalizedTextMeshEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var localized = target as LocalizedTextMesh;
            if (!string.IsNullOrEmpty(localized.Key))
            {
                EditorGUILayout.LabelField("Localized", LocalizationManager.Get(localized.Key));
                localized.OnLocalize();
            }
        }
    }
}