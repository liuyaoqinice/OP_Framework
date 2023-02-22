using System;
using UnityEditor;

namespace OPFramework.Editor
{
    public abstract partial class EditorPrefsValueT<T>  
    {
        public string key;
        private T m_Value;

        public T value
        {
            get => this.m_Value;
            set
            {
                if (object.Equals((object) this.value, (object) value))
                    return;
                this.m_Value = value;
                this.DoSet();
            }
        }

        protected EditorPrefsValueT(string key, T @default)
        {
            this.key = key;
            Func<T,T> doGet = DoGet;
            this.value = doGet(@default);
        }

        public static implicit operator T(EditorPrefsValueT<T> prefsValue) => prefsValue.value;

        public void Set(T v) => this.value = v;

        public void Reset(T @default)
        {
            EditorPrefs.DeleteKey(this.key);
            this.m_Value = @default;
        }

        protected abstract T DoGet(T @default);

        protected abstract void DoSet();
    }
}