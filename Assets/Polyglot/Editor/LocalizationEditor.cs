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

        private const string docsIdPrefs = "polyglotdocsid";
        private const string sheetIdPrefs = "polyglotsheetid";

        private const string customDocsIdPrefs = "polyglotcustomdocsid";
        private const string customSheetIdPrefs = "polyglotcustomsheetid";

#if UNITY_5
        [UsedImplicitly]
#endif
        [MenuItem("Assets/Polyglot Localization")]
        public static void CreateLocalization()
        {
            var asset = Resources.Load<Localization>("Localization");
            if (asset == null)
            {
                asset = CreateInstance<Localization>();
                ProjectWindowUtil.CreateAsset(asset, "Localization.asset");
            }
            Selection.activeObject = asset;
        }

        private static void DeletePath(string key, int index)
        {
            var defaultPath = string.Empty;
            if (Localization.Instance.InputFiles.Count > index)
            {
                defaultPath = AssetDatabase.GetAssetPath(Localization.Instance.InputFiles[index].TextAsset);
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextField(EditorPrefs.GetString(key, defaultPath));
            EditorGUI.EndDisabledGroup();
            EditorGUI.BeginDisabledGroup(!EditorPrefs.HasKey(key));
            if (GUILayout.Button("Clear"))
            {
                EditorPrefs.DeleteKey(key);
            }
            EditorGUILayout.EndHorizontal();
            EditorGUI.EndDisabledGroup();
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Importer Settings", (GUIStyle)"IN TitleText");
            if (GUILayout.Button("Download Polyglot mastersheet"))
            {
                DownloadMasterCSV();
            }
            EditorGUI.BeginDisabledGroup(true);
            DisplayDocsAndSheetId(docsIdPrefs, "17f0dQawb-s_Fd7DHgmVvJoEGDMH_yoSd8EYigrb0zmM", sheetIdPrefs, "296134756");
            DeletePath("polyglotpath", 0);
            EditorGUI.EndDisabledGroup();


            EditorGUI.BeginDisabledGroup(!ValidateDownloadCustomCSV());
            if (GUILayout.Button("Download Custom Google sheet"))
            {
                DownloadCustomCSV();
            }
            EditorGUI.EndDisabledGroup();
            DisplayDocsAndSheetId(customDocsIdPrefs, string.Empty, customSheetIdPrefs, string.Empty);
            DeletePath("polyglotcustompath", 1);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Localization Settings", (GUIStyle)"IN TitleText");
            if (DrawDefaultInspector())
            {
                var manager = target as Localization;
                if (manager != null)
                {
                    manager.InvokeOnLocalize();
                }
            }
        }

        private static void DisplayDocsAndSheetId(string docs, string defaultDocs, string sheet, string defautlSheet)
        {
            EditorGUI.BeginChangeCheck();
            var customDocs = EditorGUILayout.TextField("Docs Id", EditorPrefs.GetString(docs, defaultDocs));
            if (EditorGUI.EndChangeCheck())
            {
                EditorPrefs.SetString(docs, customDocs);
            }
            EditorGUI.BeginChangeCheck();
            var customSheet = EditorGUILayout.TextField("Sheet Id", EditorPrefs.GetString(sheet, defautlSheet));
            if (EditorGUI.EndChangeCheck())
            {
                EditorPrefs.SetString(sheet, customSheet);
            }
        }

        [MenuItem("Assets/Import latest Polyglot Mastersheet", false, 30)]
        private static void DownloadMasterCSV()
        {
            var defaultPath = string.Empty;
            if (Localization.Instance.InputFiles.Count > 0)
            {
                defaultPath = AssetDatabase.GetAssetPath(Localization.Instance.InputFiles[0].TextAsset);
            }

            DownloadGoogleCSV(EditorPrefs.GetString(docsIdPrefs, "17f0dQawb-s_Fd7DHgmVvJoEGDMH_yoSd8EYigrb0zmM"), "csv", EditorPrefs.GetString(sheetIdPrefs, "296134756"), "polyglotpath", defaultPath);
        }

        [MenuItem("Assets/Import latest Custom Sheet", true, 30)]
        private static bool ValidateDownloadCustomCSV()
        {
            return !string.IsNullOrEmpty(EditorPrefs.GetString(customDocsIdPrefs)) && !string.IsNullOrEmpty(EditorPrefs.GetString(customSheetIdPrefs));
        }

        [MenuItem("Assets/Import latest Custom Sheet", false, 30)]
        private static void DownloadCustomCSV()
        {
            var defaultPath = string.Empty;
            if (Localization.Instance.InputFiles.Count > 1)
            {
                defaultPath = AssetDatabase.GetAssetPath(Localization.Instance.InputFiles[1].TextAsset);
            }

			DownloadGoogleCSV(EditorPrefs.GetString(customDocsIdPrefs), EditorPrefs.GetString(customSheetIdPrefs), "tsv", "polyglotcustompath", defaultPath);
        }

        private static void DownloadGoogleCSV(string docsId, string sheetId, string format, string prefs, string defaultPath)
        {
            EditorUtility.DisplayProgressBar("Download", "Downloading...", 0);
            var www = new WWW(String.Format("https://docs.google.com/spreadsheets/d/{0}/export?format={2}&gid={1}", docsId, sheetId, format));
            while (!www.isDone)
            {
                EditorUtility.DisplayProgressBar("Download", "Downloading...", www.progress);
            }
            EditorUtility.ClearProgressBar();
            var path = EditorPrefs.GetString(prefs, defaultPath);

            if (string.IsNullOrEmpty(path))
            {
                path = EditorUtility.SaveFilePanelInProject("Save Localization", "", "txt", "Please enter a file name to save the csv to", path);
                EditorPrefs.SetString(prefs, path);
            }
            if (string.IsNullOrEmpty(path))
            {
                return;
            }
            File.WriteAllText(path, www.text);

            Debug.Log("Importing " + path);
            AssetDatabase.ImportAsset(path);
        }
    }
}