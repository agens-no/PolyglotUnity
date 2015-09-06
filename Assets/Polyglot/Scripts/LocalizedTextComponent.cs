using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Polyglot
{
    public abstract class LocalizedTextComponent<T> : MonoBehaviour, ILocalize where T : Component
    {
        [Tooltip("The text component to localize")]
        [SerializeField]
        private T text;

        [Tooltip("The key to localize with")]
        [SerializeField]
        private string key;
        public string Key
        {
            get
            {
                return key;
            }
            set
            {
                key = value;
                OnLocalize();
            }
        }

        public List<object> Parameters { get { return parameters; } }

        private List<object> parameters = new List<object>();

#if UNITY_5
        [UsedImplicitly]
#endif
        public void Reset()
        {
            text = GetComponent<T>();
        }

#if UNITY_5
        [UsedImplicitly]
#endif
        public void Start()
        {
            LocalizationManager.Instance.AddOnLocalizeEvent(this);
        }

        protected abstract void SetText(T component, string value);

        protected abstract void UpdateAlignment(T component, LanguageDirection direction);

        public void OnLocalize()
        {
#if UNITY_EDITOR
            var flags = text.hideFlags;
            text.hideFlags = HideFlags.DontSave;
#endif
            if (parameters != null && parameters.Count > 0)
            {
                SetText(text, LocalizationManager.GetFormat(key, parameters.ToArray()));
            }
            else
            {
                SetText(text, LocalizationManager.Get(key));
            }
            var direction = LocalizationManager.Instance.SelectedLanguageDirection;

            UpdateAlignment(text, direction);
#if UNITY_EDITOR
            text.hideFlags = flags;
#endif
        }
        
        public void ClearParameters()
        {
            parameters.Clear();
        }

        public void AddParameter(object parameter)
        {
            parameters.Add(parameter);
            OnLocalize();
        }
        public void AddParameter(int parameter)
        {
            AddParameter((object)parameter);
        }
        public void AddParameter(float parameter)
        {
            AddParameter((object)parameter);
        }
        public void AddParameter(string parameter)
        {
            AddParameter((object)parameter);
        }
    }
}