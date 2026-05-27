using UnityEngine;

namespace KBCore.Refs
{
    
    internal class PrefabUtil
    {
        internal static bool IsUninstantiatedPrefab(GameObject obj)
            => obj.scene.rootCount == 0;
    }
}
