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
        
        public ItemData ItemData => _itemData;
        
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
                
                iconImage.color = Color.white; 
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _originalPosition = transform.position;
            _originalParent = transform.parent;

            var canvas = GetComponentInParent<Canvas>();
            if (canvas != null)
            {
                transform.SetParent(canvas.transform, true);
                transform.SetAsLastSibling();
            }

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

            if (_mainCamera == null) _mainCamera = Camera.main;
            if (_mainCamera != null)
            {
                Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, 100f, heroLayer))
                {
                    ChampionActor hero = hit.collider.GetComponentInParent<ChampionActor>();
                    if (hero != null)
                    {
                        
                        var itemSystem = GameSystemManager.Instance.Items;
                        if (itemSystem != null)
                        {
                            if (itemSystem.TryGiveItemToHero(this, hero))
                            {
                                
                                return;
                            }
                        }
                    }
                }
            }

            ResetPosition();
        }

        public void ResetPosition()
        {
            transform.SetParent(_originalParent);
            transform.localPosition = Vector3.zero;
        }
    }
}
