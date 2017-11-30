using System;
using UnityEngine;

namespace Polyglot
{
    [Serializable]
    public class LocalizationDocument
    {
        [SerializeField]
        private string docsId;
        [SerializeField]
        private string sheetId;
        [SerializeField]
        private LocalizationAssetFormat format;
        [SerializeField]
        private TextAsset textAsset;

        public TextAsset TextAsset
        {
            get { return textAsset; }
            set { textAsset = value; }
        }

        public string DocsId
        {
            get { return docsId; }
            set { docsId = value; }
        }

        public string SheetId
        {
            get { return sheetId; }
            set { sheetId = value; }
        }

        public LocalizationAssetFormat Format
        {
            get { return format; }
            set { format = value; }
        }
    }
}