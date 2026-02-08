using UnityEngine;

namespace Dajunctic
{
    public class BillboardEmote : MonoBehaviour
    {
        private Transform mainCameraTransform;

        void Start()
        {
            if (Camera.main != null)
            {
                mainCameraTransform = Camera.main.transform;
            }
            else
            {
                Debug.LogError("BillboardEmote: Không tìm thấy Main Camera. Hãy đảm bảo camera của bạn có tag 'MainCamera'.");
                enabled = false; 
            }
        }

        void LateUpdate()
        {
            if (mainCameraTransform == null) return;
            transform.LookAt(transform.position + mainCameraTransform.forward);
        }

        public void SetEmoteSprite(Sprite newSprite)
        {
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.sprite = newSprite;
            }
        }
    }
}