#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace Dajunctic
{
    [CustomEditor(typeof(IdDatabase), true)]
    public class IdDatabaseEditor: OdinEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Sort"))
            {
                Sort();
                EditorUtility.SetDirty(target);
            }
            if (GUILayout.Button("Update GUID"))
            {
                GuidReferenceHelper.Refresh();
            }
        }

        protected virtual void Sort()
        {
        }
    }
}
#endif