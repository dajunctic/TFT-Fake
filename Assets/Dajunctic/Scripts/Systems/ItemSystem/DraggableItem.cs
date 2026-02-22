using UnityEngine;

namespace Dajunctic
{
    public class DraggableItem : MonoBehaviour, IDraggable
    {
        private ItemData _itemData;
        private Vector3 _originalPosition;
        private bool _isDragging;
        private Camera _mainCamera;
        
        [SerializeField] private LayerMask heroLayer;

        public void Initialize(ItemData data)
        {
            _itemData = data;
            _mainCamera = Camera.main;
            // Set visuals based on data.icon or a 3D model
        }

        public void OnDragStart()
        {
            _isDragging = true;
            _originalPosition = transform.position;
        }

        public void OnDragUpdate(Vector3 worldPos)
        {
            // Follow mouse with a slight height offset
            transform.position = worldPos + Vector3.up * 0.5f;
            
            // Optional: Raycast to find hero and show preview
            CheckForHeroUnderMouse();
        }

        public void OnDrop(Vector3 finalPos)
        {
            _isDragging = false;
            
            // Check if dropped on a hero
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
                        itemSystem.TryGiveItemToHero(_itemData, hero);
                        // If successfully given, the ItemSystem will handle the bench logic
                        // and this object should probably be destroyed or returned to pool
                        Destroy(gameObject);
                        return;
                    }
                }
            }

            // If not dropped on hero, return to bench
            ResetPosition();
        }

        private void CheckForHeroUnderMouse()
        {
            // Visual feedback when hovering over hero
        }

        public void ResetPosition()
        {
            transform.position = _originalPosition;
        }

        public Transform GetTransform() => transform;
    }
}
