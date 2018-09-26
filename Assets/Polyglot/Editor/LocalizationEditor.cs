using System;
using UnityEditor;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;

namespace Polyglot
{
    [CustomEditor(typeof(Localization))]
    public class LocalizationInspector : Editor
    {

        private const string PathPrefs = "polyglotpath";
        private const string DefaultPolyglotPath = "Assets/Polyglot/Localization/PolyglotGameDev - Master.txt";

        private const string CustomPathPrefs = "polyglotcustompath";
        private const string CustomDocsIdPrefs = "polyglotcustomdocsid";
        private const string CustomSheetIdPrefs = "polyglotcustomsheetid";

        //https://docs.google.com/spreadsheets/d/17f0dQawb-s_Fd7DHgmVvJoEGDMH_yoSd8EYigrb0zmM/edit?usp=sharing
        private const string OfficialSheet = "17f0dQawb-s_Fd7DHgmVvJoEGDMH_yoSd8EYigrb0zmM";
        private const string OfficialGId = "296134756";

        private const string MenuItemPath = "Window/Polyglot Localization/";

        private const string LocalizationAssetName = "Localization";
        private const string LocalizationAssetPath = "Assets/Polyglot/Resources/"+LocalizationAssetName+".asset";

        
        [SerializeField]
        private string myField;
        
        public string MyField
        {
            get { return myField; }
            set { myField = value; }
        }


        [MenuItem(MenuItemPath + "Configurate", false, 0)]
        public static void Configurate()
        {
            var asset = Resources.Load<Localization>(LocalizationAssetName);
            if (asset == null)
            {
                asset = CreateInstance<Localization>();
                var assetPathAndName = AssetDatabase.GenerateUniqueAssetPath (LocalizationAssetPath);
                AssetDatabase.CreateAsset(asset, assetPathAndName);
            }

            Selection.activeObject = asset;
        }
        
        #region Prefs

        private static string GetPrefsString(string key, string defaultString = null)
        {
            return EditorPrefs.GetString(Application.productName + "." + key, defaultString);
        }
        private static void SetPrefsString(string key, string value)
        {
            EditorPrefs.SetString(Application.productName + "." + key, value);
        }
        private static int GetPrefsInt(string key, int defaultInt = 0)
        {
            return EditorPrefs.GetInt(Application.productName + "." + key, defaultInt);
        }
        private static void SetPrefsInt(string key, int value)
        {
            EditorPrefs.SetInt(Application.productName + "." + key, value);
        }
        private static bool HasPrefsKey(string key)
        {
            return EditorPrefs.HasKey(Application.productName + "." + key);
        }
        private static void DeletePrefsKey(string key)
        {
            EditorPrefs.DeleteKey(Application.productName + "." + key);
        }
        #endregion

        private static void DeletePath(string key, int index)
        {
            var defaultPath = string.Empty;
            if (Localization.Instance.InputFiles.Count > index)
            {
                defaultPath = AssetDatabase.GetAssetPath(Localization.Instance.InputFiles[index].TextAsset);
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextField(GetPrefsString(key, defaultPath));
            EditorGUI.EndDisabledGroup();
            EditorGUI.BeginDisabledGroup(!HasPrefsKey(key));
            if (GUILayout.Button("Clear"))
            {
                DeletePrefsKey(key);
            }
            EditorGUILayout.EndHorizontal();
            EditorGUI.EndDisabledGroup();
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            serializedObject.Update();
            
            EditorGUILayout.LabelField("Polyglot Localization Settings", (GUIStyle)"IN TitleText");

            var polyglotPath = GetPrefsString(PathPrefs);
            if (string.IsNullOrEmpty(polyglotPath))
            {
                polyglotPath = DefaultPolyglotPath;
            }
            
            DisplayDocsAndSheetId("Official Polyglot Master", true, false, serializedObject.FindProperty("polyglotDocument"), OfficialSheet, OfficialGId, polyglotPath);

            EditorGUILayout.Space();

            DisplayDocsAndSheetId("Custom Sheet", false, !ValidateDownloadCustomSheet(), serializedObject.FindProperty("customDocument"), GetPrefsString(CustomDocsIdPrefs), GetPrefsString(CustomSheetIdPrefs), GetPrefsString(CustomPathPrefs));

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Localization Settings", (GUIStyle)"IN TitleText");
            var iterator = serializedObject.GetIterator();
            for (bool enterChildren = true; iterator.NextVisible(enterChildren); enterChildren = false)
            {
                if(iterator.propertyPath.Contains("Document")) continue;
                
#if !ARABSUPPORT_ENABLED
                if (iterator.propertyPath == "Localize")
                {
                    using (new EditorGUI.DisabledGroupScope(true))
                    {
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Arabic Support", (GUIStyle)"BoldLabel");
                        EditorGUILayout.HelpBox("Enable Arabic Support with ARABSUPPORT_ENABLED post processor flag", MessageType.Info);
                        EditorGUILayout.Toggle(new GUIContent("Show Tashkeel", "Enable Arabic Support with ARABSUPPORT_ENABLED post processor flag"), true);
                        EditorGUILayout.Toggle(new GUIContent("Use Hindu Numbers", "Enable Arabic Support with ARABSUPPORT_ENABLED post processor flag"), false);
                    }
                }
#endif
                
                using (new EditorGUI.DisabledScope("m_Script" == iterator.propertyPath))
                    EditorGUILayout.PropertyField(iterator, true, new GUILayoutOption[0]);
            }
#if !ARABSUPPORT_ENABLED
#endif

            serializedObject.ApplyModifiedProperties();
            if (EditorGUI.EndChangeCheck())
            {
                var manager = target as Localization;
                if (manager != null)
                {
                    manager.InvokeOnLocalize();
                }
            }
        }

        private static void DisplayDocsAndSheetId(string title, bool disableId, bool disableOpen, SerializedProperty document, string defaultDocs, string defaultSheet, string defaultTextAssetPath)
        {
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.LabelField(title, (GUIStyle)"IN TitleText");
            EditorGUI.BeginDisabledGroup(disableId);
            var docsIdProp = document.FindPropertyRelative("docsId");
            if (string.IsNullOrEmpty(docsIdProp.stringValue))
            {
                docsIdProp.stringValue = defaultDocs;
            }
            EditorGUILayout.PropertyField(docsIdProp);
            var sheetIdProps = document.FindPropertyRelative("sheetId");
            if (string.IsNullOrEmpty(sheetIdProps.stringValue))
            {
                sheetIdProps.stringValue = defaultSheet;
            }
            EditorGUILayout.PropertyField(sheetIdProps);
            var textAssetProps = document.FindPropertyRelative("textAsset");
            if (textAssetProps.objectReferenceValue == null && !string.IsNullOrEmpty(defaultTextAssetPath))
            {
                textAssetProps.objectReferenceValue = AssetDatabase.LoadAssetAtPath<TextAsset>(defaultTextAssetPath);
            }
            EditorGUILayout.PropertyField(textAssetProps);
            EditorGUI.EndDisabledGroup();

            EditorGUI.BeginDisabledGroup(disableOpen);
            var downloadOnstartProps = document.FindPropertyRelative("downloadOnStart");
            EditorGUILayout.PropertyField(downloadOnstartProps);
            var formatProps = document.FindPropertyRelative("format");
            EditorGUILayout.PropertyField(formatProps);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(string.Empty);
            if(GUILayout.Button("Open"))
            {
                var url = string.Format("https://docs.google.com/spreadsheets/d/{0}/edit#gid={1}", docsIdProp.stringValue, sheetIdProps.stringValue);
                Application.OpenURL(url);
            }
            if(GUILayout.Button("Download"))
            {
                DownloadGoogleSheet(docsIdProp.stringValue, sheetIdProps.stringValue, (GoogleDriveDownloadFormat)formatProps.enumValueIndex, (TextAsset)document.FindPropertyRelative("textAsset").objectReferenceValue);
            }
            EditorGUILayout.EndHorizontal();
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndVertical();
        }

        [MenuItem(MenuItemPath + "Download Polyglot Mastersheet", false, 30)]
        private static void DownloadMasterSheet()
        {
            var doc = Localization.Instance.PolyglotDocument;

            DownloadGoogleSheet(doc.DocsId, doc.SheetId, doc.Format, doc.TextAsset);
        }

        [MenuItem(MenuItemPath + "Download Custom Sheet", true, 30)]
        private static bool ValidateDownloadCustomSheet()
        {
            var doc = Localization.Instance.CustomDocument;
            return !string.IsNullOrEmpty(doc.DocsId) && !string.IsNullOrEmpty(doc.SheetId);
        }

        [MenuItem(MenuItemPath + "Download Custom Sheet", false, 30)]
        private static void DownloadCustomSheet()
        {
            var doc = Localization.Instance.CustomDocument;

            DownloadGoogleSheet(doc.DocsId, doc.SheetId, doc.Format, doc.TextAsset);
        }

        private static void DownloadGoogleSheet(string docsId, string sheetId, GoogleDriveDownloadFormat format, TextAsset textAsset)
        {
            EditorUtility.DisplayCancelableProgressBar("Download", "Downloading...", 0);

            var iterator = GoogleDownload.DownloadSheet(docsId, sheetId, t => DownloadComplete(t, textAsset), format, DisplayDownloadProgressbar);
            while(iterator.MoveNext())
            {}
        }

        private static void DownloadComplete(string text, TextAsset textAsset)
        {
            if (string.IsNullOrEmpty(text))
            {
                Debug.LogError("Could not download google sheet");
                return;
            }
            
            var path = textAsset != null ? AssetDatabase.GetAssetPath(textAsset) : null;

            if (string.IsNullOrEmpty(path))
            {
                path = EditorUtility.SaveFilePanelInProject("Save Localization", "", "txt", "Please enter a file name to save the csv to", path);
            }
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            File.WriteAllText(path, text);

            Debug.Log("Importing " + path);
            AssetDatabase.ImportAsset(path);

            LocalizationImporter.Refresh();
        }

        private static bool DisplayDownloadProgressbar(float progress)
        {
            if(progress < 1)
            {
                return EditorUtility.DisplayCancelableProgressBar("Download Localization", "Downloading...", progress);
            }
        
            EditorUtility.ClearProgressBar();
            return false;
        }
    }
}