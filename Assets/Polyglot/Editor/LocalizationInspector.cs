#if UNITY_5
using JetBrains.Annotations;
#endif
using System;
using UnityEditor;
using UnityEngine;
using System.IO;

namespace Polyglot
{
#if UNITY_5
    [UsedImplicitly]
#endif
    [CustomEditor(typeof(Localization))]
    public class LocalizationInspector : Editor
    {

#if UNITY_5
        [UsedImplicitly]
#endif
        [MenuItem("Assets/Create Polyglot Settings")]
        public static void CreateLocalization()
        {
            var asset = ScriptableObject.CreateInstance<Localization>();
            ProjectWindowUtil.CreateAsset(asset, "Localization.asset");
        }


        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Download Polyglot mastersheet"))
            {
                DownloadMasterCSV();
            }

            if (DrawDefaultInspector())
            {
                var manager = target as Localization;
                if (manager != null)
                {
                    manager.InvokeOnLocalize();
                }
            }
        }

        [MenuItem("Assets/Import latest Polyglot mastersheet", false, 30)]
        private static void DownloadMasterCSV()
        {
            DownloadGoogleCSV("17f0dQawb-s_Fd7DHgmVvJoEGDMH_yoSd8EYigrb0zmM", "296134756");
        }

        private static void DownloadGoogleCSV(string docsId, string sheetId)
        {
            EditorUtility.DisplayProgressBar("Download CSV", "Downloading...", 0);
            var www = new WWW(String.Format("https://docs.google.com/spreadsheets/d/{0}/export?format=csv&gid={1}", docsId, sheetId));
            while (!www.isDone)
            {
                EditorUtility.DisplayProgressBar("Download CSV", "Downloading...", www.progress);
            }
            EditorUtility.ClearProgressBar();
            var path = EditorUtility.SaveFilePanelInProject("Save CSV", "", "csv", "Please enter a file name to save the csv to");
            if (String.IsNullOrEmpty(path))
            {
                return;
            }
            File.WriteAllText(path, www.text);

            Debug.Log("Importing " + path);
            AssetDatabase.ImportAsset(path);
        }
    }
}