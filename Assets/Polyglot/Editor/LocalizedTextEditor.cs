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
    [CanEditMultipleObjects]
    public class LocalizedTextEditor : LocalizedEditor<LocalizedText>
    {
        public override void OnInspectorGUI()
        {
            OnInspectorGUI("key");
        }
    }
}