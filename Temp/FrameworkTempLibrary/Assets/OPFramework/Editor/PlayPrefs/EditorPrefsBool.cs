using UnityEditor;

namespace OPFramework.Editor
{
    public class EditorPrefsBool : EditorPrefsValueT<bool>
    {
        public EditorPrefsBool(string key, bool @default) : base(key, @default)
        {
        }

        protected override bool DoGet(bool @default) => EditorPrefs.GetInt(this.key, @default ? 1 : 0) != 0;

        protected override void DoSet() => EditorPrefs.SetInt(this.key, this.value ? 1 : 0);
    }
}