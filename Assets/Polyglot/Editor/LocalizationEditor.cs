#if UNITY_5_3_OR_NEWER
using JetBrains.Annotations;
#endif
using System;
using UnityEditor;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;

namespace Polyglot
{
#if UNITY_5
    [UsedImplicitly]
#endif
    [CustomEditor(typeof(Localization))]
    public class LocalizationInspector : Editor
    {

        private const string PathPrefs = "polyglotpath";
        private const string DocsIdPrefs = "polyglotdocsid";
        private const string SheetIdPrefs = "polyglotsheetid";
        private const string FormatIdPrefs = "polyglotformatid";

        private const string CustomPathPrefs = "polyglotcustompath";
        private const string CustomDocsIdPrefs = "polyglotcustomdocsid";
        private const string CustomSheetIdPrefs = "polyglotcustomsheetid";
        private const string CustomFormatIdPrefs = "polyglotcustomformatid";

        //https://docs.google.com/spreadsheets/d/17f0dQawb-s_Fd7DHgmVvJoEGDMH_yoSd8EYigrb0zmM/edit?usp=sharing
        private const string OfficialSheet = "17f0dQawb-s_Fd7DHgmVvJoEGDMH_yoSd8EYigrb0zmM";
        private const string OfficialGId = "296134756";

#if UNITY_5_3_OR_NEWER
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

            DisplayDocsAndSheetId("Official Polyglot Master", true, false, PathPrefs, 0, DocsIdPrefs, OfficialSheet, SheetIdPrefs, OfficialGId, FormatIdPrefs, LocalizationAssetFormat.CSV);

            EditorGUILayout.Space();

            DisplayDocsAndSheetId("Custom Sheet", false, !ValidateDownloadCustomSheet(), CustomPathPrefs, 1, CustomDocsIdPrefs, string.Empty, CustomSheetIdPrefs, string.Empty, FormatIdPrefs, LocalizationAssetFormat.CSV);


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

        private static void DisplayDocsAndSheetId(string title, bool disableId, bool disableOpen, string pathPrefs, int index, string docs, string defaultDocs, string sheet, string defaultSheet, string format, LocalizationAssetFormat defaultFormat)
        {
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.LabelField(title, (GUIStyle)"IN TitleText");
            EditorGUI.BeginDisabledGroup(disableId);
            EditorGUI.BeginChangeCheck();
            var docsId = EditorGUILayout.TextField("Docs Id", EditorPrefs.GetString(docs, defaultDocs));
            if (EditorGUI.EndChangeCheck())
            {
                EditorPrefs.SetString(docs, docsId);
            }
            EditorGUI.BeginChangeCheck();
            var sheetId = EditorGUILayout.TextField("Sheet Id", EditorPrefs.GetString(sheet, defaultSheet));
            if (EditorGUI.EndChangeCheck())
            {
                EditorPrefs.SetString(sheet, sheetId);
            }
            EditorGUI.EndDisabledGroup();

            EditorGUI.BeginDisabledGroup(disableOpen);
            EditorGUI.BeginChangeCheck();
            var formatId = (LocalizationAssetFormat)EditorGUILayout.EnumPopup("Format", (LocalizationAssetFormat)EditorPrefs.GetInt(format, (int)defaultFormat));
            if (EditorGUI.EndChangeCheck())
            {
                EditorPrefs.SetInt(format, (int)formatId);
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(string.Empty);
            if(GUILayout.Button("Open"))
            {
                var url = string.Format("https://docs.google.com/spreadsheets/d/{0}/edit#gid={1}", docsId, sheetId);
                Application.OpenURL(url);
            }
            if(GUILayout.Button("Download"))
            {
                DownloadGoogleSheet(docsId, sheetId, formatId, pathPrefs, string.Empty);

            }
            EditorGUILayout.EndHorizontal();
            EditorGUI.EndDisabledGroup();
            DeletePath(pathPrefs, index);
            EditorGUILayout.EndVertical();
        }

        [MenuItem("Assets/Import latest Polyglot Mastersheet", false, 30)]
        private static void DownloadMasterSheet()
        {
            var defaultPath = string.Empty;
            if (Localization.Instance.InputFiles.Count > 0)
            {
                defaultPath = AssetDatabase.GetAssetPath(Localization.Instance.InputFiles[0].TextAsset);
            }

            DownloadGoogleSheet(EditorPrefs.GetString(DocsIdPrefs, OfficialSheet), EditorPrefs.GetString(SheetIdPrefs, OfficialGId), (LocalizationAssetFormat)EditorPrefs.GetInt(FormatIdPrefs), PathPrefs, defaultPath);
        }

        [MenuItem("Assets/Import latest Custom Sheet", true, 30)]
        private static bool ValidateDownloadCustomSheet()
        {
            return !string.IsNullOrEmpty(EditorPrefs.GetString(CustomDocsIdPrefs)) && !string.IsNullOrEmpty(EditorPrefs.GetString(CustomSheetIdPrefs));
        }

        [MenuItem("Assets/Import latest Custom Sheet", false, 30)]
        private static void DownloadCustomSheet()
        {
            var defaultPath = string.Empty;
            if (Localization.Instance.InputFiles.Count > 1)
            {
                defaultPath = AssetDatabase.GetAssetPath(Localization.Instance.InputFiles[1].TextAsset);
            }

			DownloadGoogleSheet(EditorPrefs.GetString(CustomDocsIdPrefs), EditorPrefs.GetString(CustomSheetIdPrefs), (LocalizationAssetFormat)EditorPrefs.GetInt(CustomFormatIdPrefs), CustomPathPrefs, defaultPath);
        }

        private static void DownloadGoogleSheet(string docsId, string sheetId, LocalizationAssetFormat format, string prefs, string defaultPath)
        {
            EditorUtility.DisplayProgressBar("Download", "Downloading...", 0);
            Debug.Log("docsId: " + docsId);
            Debug.Log("sheetId: " + sheetId);
            Debug.Log("format: " + format);
            var url = string.Format("https://docs.google.com/spreadsheets/d/{0}/export?format={2}&gid={1}", docsId, sheetId, Enum.GetName(typeof(LocalizationAssetFormat), format).ToLower());
            Debug.Log(url);
#if UNITY_5_5_OR_NEWER
            var www = UnityWebRequest.Get(url);
            www.Send();
#else
            var www = new WWW(url);
#endif
            while (!www.isDone)
            {
#if UNITY_5_5_OR_NEWER
                var progress = www.downloadProgress;
#else
                var progress = www.progress;
#endif

                if (EditorUtility.DisplayCancelableProgressBar("Download", "Downloading...", progress))
                {
                    return;
                }
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

#if UNITY_5_5_OR_NEWER
            var text = www.downloadHandler.text;
#else
            var text = www.text;
#endif

            if (text.StartsWith("<!"))
            {
                Debug.LogError("Google sheet could not be downloaded\n" + text);
                return;
            }

            File.WriteAllText(path, text);

            Debug.Log("Importing " + path);
            AssetDatabase.ImportAsset(path);
        }
    }
}