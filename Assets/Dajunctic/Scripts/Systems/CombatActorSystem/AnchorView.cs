using UnityEngine;

namespace Dajunctic
{
    public class AnchorView: BaseView
    {
        [SerializeField] public AnchorType anchorType;

        public Vector3 Position => CachedTransform.position;
    }



  

}