using UnityEngine;

namespace Dajunctic
{
    public class CarouselBarrier : MonoBehaviour
    {
        [SerializeField] private GameObject visual;
        [SerializeField] private Collider col;

        public void SetActive(bool active)
        {
            if (visual != null) visual.SetActive(active);
            if (col != null) col.enabled = active;
        }
    }
}
