using UnityEngine;

namespace DTT.Utils.Extensions.Demo
{
    public class RectTransformBehaviour : MonoBehaviour
    {
        private void Awake()
        {
            // Retrieve the RectTransform component.
            RectTransform rectTransform = this.GetRectTransform();

            // Set the anchor values with one method.
            rectTransform.SetAnchor(0.5f, 0.5f, 0.5f, 0.5f);

            // Or set the anchor values using one of unity's predefined anchor settings.
            rectTransform.SetAnchor(RectAnchor.STRETCH_FULL, true, true);
        }
    }
}
