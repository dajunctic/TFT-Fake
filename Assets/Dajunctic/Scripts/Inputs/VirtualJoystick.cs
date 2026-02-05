using UnityEngine;
using UnityEngine.EventSystems;

namespace Dajunctic{
    public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
    {
        [SerializeField] private RectTransform container;
        [SerializeField] private RectTransform handle;
        [SerializeField] private float handleRange = 100f;

        public static VirtualJoystick Instance;

        public Vector2 InputDirection { get; private set; }

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 position = Vector2.zero;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(container, eventData.position, eventData.pressEventCamera, out position))
            {
                position.x = position.x / container.sizeDelta.x;
                position.y = position.y / container.sizeDelta.y;

                InputDirection = new Vector2(position.x * 2, position.y * 2);
                InputDirection = (InputDirection.magnitude > 1.0f) ? InputDirection.normalized : InputDirection;

                handle.anchoredPosition = new Vector2(InputDirection.x * handleRange, InputDirection.y * handleRange);
            }
        }

        public void OnPointerDown(PointerEventData eventData) => OnDrag(eventData);

        public void OnPointerUp(PointerEventData eventData)
        {
            InputDirection = Vector2.zero;
            handle.anchoredPosition = Vector2.zero;
        }
    }
}