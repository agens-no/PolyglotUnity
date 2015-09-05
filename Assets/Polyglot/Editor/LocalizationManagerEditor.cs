
using System.IO;
#if UNITY_5
using JetBrains.Annotations;
#endif
using UnityEditor;
using UnityEngine;

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

            if (GUILayout.Button("Download latest"))
            {
                ImportMasterCSV();
            }
        }

        [MenuItem("Assets/Import latest Polyglot mastersheet", false, 30)]
        private static void ImportMasterCSV()
        {
            EditorUtility.DisplayProgressBar("Download Polyglot mastersheet CSV", "Downloading...", 0);
            WWW www = new WWW("https://docs.google.com/spreadsheets/d/17f0dQawb-s_Fd7DHgmVvJoEGDMH_yoSd8EYigrb0zmM/export?format=csv&gid=296134756");
            while (!www.isDone)
            {
                EditorUtility.DisplayProgressBar("Download Polyglot mastersheet CSV", "Downloading...", www.progress);
            }
            EditorUtility.ClearProgressBar();
            var path = EditorUtility.SaveFilePanelInProject("Save Polyglot mastersheet CSV", "PolyglotGamedev Master Sheet - Master", "csv", "Please enter a file name to save the polyglot csv to");
            if (string.IsNullOrEmpty(path))
            {
                return;
            }
            File.WriteAllText(path, www.text);
            AssetDatabase.ImportAsset(path);
        }
    }
}