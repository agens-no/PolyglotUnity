using UnityEditor;
using UnityEngine;

namespace Polyglot
{
	[CustomPropertyDrawer(typeof(LocalizedString), true)]
	public sealed class LocalizedStringDrawer : PropertyDrawer
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
			EditorGUI.BeginChangeCheck();

			var keyProperty = property.FindPropertyRelative("key");
			EditorGUI.PropertyField(position, keyProperty, label, true);

			EditorGUI.EndProperty();
		}
	}
}
