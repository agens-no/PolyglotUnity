using UnityEditor;
using UnityEngine;

namespace Polyglot
{
    public abstract class LocalizedEditor<T> : Editor where T : class, ILocalize
    {
        private static int MaxAutoComplete = 6;

        public void OnInspectorGUI(string propertyPath)
        {
            var changed = DrawDefaultInspector();
            EditorGUI.BeginChangeCheck();
            serializedObject.Update();
            var property = serializedObject.FindProperty(propertyPath);

            var key = property.stringValue;
            if (!string.IsNullOrEmpty(key))
            {
                var localizedString = Localization.Get(key);

                if (string.IsNullOrEmpty(localizedString))
                {
                    DrawAutoComplete(property);
                }
                else
                {
                    EditorGUILayout.LabelField("Localized", localizedString);
                }
            }
            serializedObject.ApplyModifiedProperties();

            if (EditorGUI.EndChangeCheck() || changed)
            {
                var text = target as T;
                if (text != null)
                {
                    text.OnLocalize();
                }
            }
        }

        private void DrawAutoComplete(SerializedProperty property)
        {
            var localizedStrings = LocalizationImporter.GetLanguagesStartsWith(property.stringValue);

            if (localizedStrings.Count == 0)
            {
                localizedStrings = LocalizationImporter.GetLanguagesContains(property.stringValue);
            }

            var selectedLanguage = (int)Localization.Instance.SelectedLanguage;

            EditorGUI.indentLevel++;
            var index = 0;
            EditorGUILayout.LabelField("Auto-Complete");
            foreach (var local in localizedStrings)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel(local.Key);
                if (GUILayout.Button(local.Value[selectedLanguage], "CN CountBadge"))
                {
                    property.stringValue = local.Key;
                    GUIUtility.hotControl = 0;
                    GUIUtility.keyboardControl = 0;
                }

                index++;
                EditorGUILayout.EndHorizontal();

                if (index > MaxAutoComplete)
                {
                    EditorGUILayout.LabelField(string.Empty, "..And " + (localizedStrings.Count - MaxAutoComplete) + " more");
                    break;
                }
            }
            EditorGUI.indentLevel--;
        }
    }
}