#if UNITY_5
using JetBrains.Annotations;
#endif
using UnityEditor;

namespace Polyglot
{
#if UNITY_5
    [UsedImplicitly]
#endif
    [CustomEditor(typeof(LocalizationManager))]
    public class LocalizationManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var manager = target as LocalizationManager;

            var language = manager.SelectedLanguage;

            base.OnInspectorGUI();

            if (language != manager.SelectedLanguage)
            {
                manager.InvokeOnLocalize();
            }
        }
    }
}