using UnityEngine;
using UnityEngine.EventSystems;

namespace Dajunctic
{

    public class FloatingJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        public static FloatingJoystick Instance;

        [Header("UI References")]
        [SerializeField] private RectTransform background;
        [SerializeField] private RectTransform handle;

        [Header("Settings")]
        [SerializeField] private float handleRange = 100f;

        public Vector2 InputDirection { get; private set; }

        private Vector2 _startPos;

        private void Awake()
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

        private void Start()
        {
            if (background != null)
            {
                background.gameObject.SetActive(false);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            background.position = eventData.position;
            background.gameObject.SetActive(true);

            _startPos = eventData.position;
            
            OnDrag(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 dragVector = eventData.position - _startPos;
            Vector2 clampedVector = Vector2.ClampMagnitude(dragVector, handleRange);

            handle.position = _startPos + clampedVector;
            InputDirection = clampedVector / handleRange;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            InputDirection = Vector2.zero;
            background.gameObject.SetActive(false);
        }
    }
}