using UnityEditor;
using UnityEngine;

namespace Polyglot
{
	[CustomPropertyDrawer(typeof(FontOverride), true)]
	public sealed class FontOverrideDrawer : PropertyDrawer
	{
#if UNITY_2017_3_OR_NEWER
		public override bool CanCacheInspectorGUI(SerializedProperty property)
		{
			// Cache lead to problems on the layout.
			return false;
		}
#endif
		
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);

			Rect left, right;
			left = right = position;
			left.width = EditorGUIUtility.labelWidth;
			right.xMin += left.width;
			left = EditorGUI.IndentedRect(left);

			int oldIndentLevel = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;
			
			var languageProperty = property.FindPropertyRelative("language");
			var fontProperty = property.FindPropertyRelative("font");
			EditorGUI.PropertyField(left, languageProperty, GUIContent.none, true);
			EditorGUI.PropertyField(right, fontProperty, GUIContent.none, true);

			EditorGUI.indentLevel = oldIndentLevel;

			EditorGUI.EndProperty();
		}
	}
}
