using System;
using UnityEngine;

namespace Polyglot
{
    [Serializable]
    public class LocalizationAsset
    {
        [SerializeField]
        private TextAsset textAsset;

        [SerializeField]
        private LocalizationAssetFormat format = LocalizationAssetFormat.CSV;

        public TextAsset TextAsset
        {
            get { return textAsset; }
            set { textAsset = value; }
        }

        public LocalizationAssetFormat Format
        {
            get { return format; }
            set { format = value; }
        }
    }
}