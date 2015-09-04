using JetBrains.Annotations;
using UnityEditor;

namespace Polyglot
{
    [UsedImplicitly]
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