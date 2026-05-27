using UnityEngine;

namespace Dajunctic
{
    public class Billboard : MonoBehaviour
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
                Debug.LogError("Billboard: No main camera found. Billboard will not function.");
                enabled = false; 
            }
        }

        void LateUpdate()
        {
            if (mainCameraTransform == null) return;
            transform.LookAt(transform.position + mainCameraTransform.forward);
        }
    }
}
