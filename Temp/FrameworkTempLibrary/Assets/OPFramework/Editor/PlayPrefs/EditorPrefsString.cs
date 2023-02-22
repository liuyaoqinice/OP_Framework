using UnityEditor;

namespace OPFramework.Editor
{
    public class EditorPrefsString : EditorPrefsValueT<string>
    {
        public EditorPrefsString(string key, string @default) : base(key, @default)
        {
        }

        protected override string DoGet(string @default) => EditorPrefs.GetString(this.key, @default);

        protected override void DoSet() => EditorPrefs.SetString(this.key, this.value);
        
        public override string ToString() => this.value;
    }
}