using Sirenix.OdinInspector;
using UnityEngine;

namespace Dajunctic
{
    public abstract class AssetId : ScriptableObject, IAssetId
    {
        public virtual string Id => "";
    }
}