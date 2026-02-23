using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Dajunctic
{
    public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private ItemData _itemData;
        private Vector3 _originalPosition;
        private Transform _originalParent;
        private Camera _mainCamera;
        
        [SerializeField] private LayerMask heroLayer;
        [SerializeField] private Image iconImage;

        public void Initialize(ItemData data)
        {
            _itemData = data;
            
            if (iconImage == null)
            {
                TryGetComponent(out iconImage);
            }
            if (iconImage != null && data != null)
            {
                iconImage.sprite = data.icon;
                // Ensure image is visible in case the prefab has it transparent
                iconImage.color = Color.white; 
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _originalPosition = transform.position;
            _originalParent = transform.parent;
            
            // Move to the root of the canvas so it renders on top of everything and doesn't get clipped
            var canvas = GetComponentInParent<Canvas>();
            if (canvas != null)
            {
                transform.SetParent(canvas.transform, true);
                transform.SetAsLastSibling();
            }
            
            // Disable raycast block so the mouse raycast can go through it and hit the 3D world or heroes
            if (iconImage != null)
            {
                iconImage.raycastTarget = false;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (iconImage != null)
            {
                iconImage.raycastTarget = true;
            }

            // Check if dropped on a hero
            if (_mainCamera == null) _mainCamera = Camera.main;
            if (_mainCamera != null)
            {
                Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, 100f, heroLayer))
                {
                    HeroCombatActor hero = hit.collider.GetComponentInParent<HeroCombatActor>();
                    if (hero != null)
                    {
                        // Try to give item
                        var itemSystem = GameSystemManager.Instance.Items;
                        if (itemSystem != null)
                        {
                            if (itemSystem.TryGiveItemToHero(_itemData, hero))
                            {
                                // Item was given and the ItemSystem will destroy this GameObject.
                                return;
                            }
                        }
                    }
                }
            }

            // If not dropped on hero, return to bench
            ResetPosition();
        }

        public void ResetPosition()
        {
            transform.SetParent(_originalParent);
            transform.position = _originalPosition;
        }
    }
}
