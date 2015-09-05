using UnityEditor;
using UnityEngine;

namespace Polyglot
{
    public abstract class LocalizedEditor<T> : Editor
    {
        private static int MaxAutoComplete = 6;

        public void OnInspectorGUI(string propertyPath)
        {
            base.OnInspectorGUI();


            var property = serializedObject.FindProperty(propertyPath);

            var key = property.stringValue;
            if (!string.IsNullOrEmpty(key))
            {
                var localizedString = LocalizationManager.Get(key);

                if (string.IsNullOrEmpty(localizedString))
                {
                    DrawAutoComplete(property);
                }
                else
                {
                    EditorGUILayout.LabelField("Localized", localizedString);
                }
            }
        }

        private void DrawAutoComplete(SerializedProperty property)
        {
            var localizedStrings = LocalizationManager.GetLanguagesStartsWith(property.stringValue);
            var selectedLanguage = (int)LocalizationManager.Instance.SelectedLanguage;

            EditorGUI.indentLevel++;
            var index = 0;
            foreach (var local in localizedStrings)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel(local.Key);
                if (GUILayout.Button(local.Value[selectedLanguage], "CN CountBadge"))
                {
                    property.stringValue = local.Value[selectedLanguage];
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