using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Dajunctic
{
    public class BaseSO : AssetId
    {
        [HorizontalGroup("IDGroup")] 
        [SerializeField, ReadOnly] 
        string id;

        public override string Id => id;

#if UNITY_EDITOR
        [HorizontalGroup("IDGroup", Width = 30)]
        [Button("R")]
        public void SyncAndRename()
        {
            id = name;
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        protected virtual void OnValidate()
        {
            if (id != name)
            {
                id = name;
            }
        }
#endif
    }
}
