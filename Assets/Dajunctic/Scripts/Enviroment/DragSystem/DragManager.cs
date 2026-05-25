using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Dajunctic
{
    public class DragManager : MonoBehaviour
    {
        [SerializeField] private LayerMask draggableLayer;
        [SerializeField] private LayerMask groundLayer;
        
        private IDraggable currentDragged;
        private Camera mainCamera;
        private List<IDragTarget> allTargets = new List<IDragTarget>();
        private Vector3 _dragOffset;

        public static DragManager Instance { get; private set; }

        public static event System.Action<IDraggable> OnGlobalDragStart;
        public static event System.Action<IDraggable> OnGlobalDragEnd;

        private void Awake()
        {
            Instance = this;

            // Fallback: scan targets already in scene at startup
            var targets = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
            foreach (var target in targets)
            {
                if (target is IDragTarget dragTarget)
                    allTargets.Add(dragTarget);
            }
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        /// <summary>Arena areas gọi Register khi Initialize() để đảm bảo luôn có trong list.</summary>
        public static void Register(IDragTarget target)
        {
            if (Instance != null && !Instance.allTargets.Contains(target))
                Instance.allTargets.Add(target);
        }

        /// <summary>Gọi khi area bị destroy.</summary>
        public static void Unregister(IDragTarget target)
        {
            Instance?.allTargets.Remove(target);
        }

        private void Update()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                TryStartDrag();
            }

            if (isDragging && Mouse.current.leftButton.isPressed)
            {
                UpdateDrag();
            }

            if (isDragging && Mouse.current.leftButton.wasReleasedThisFrame)
            {
                StopDrag();
            }
        }

        private bool isDragging => currentDragged != null;

        private void TryStartDrag()
        {
            // Không cho phép kéo thả tướng trong pha combat
            if (Gameplay.Instance != null && Gameplay.Instance.CurrentPhase == GameplayPhase.Combat)
                return;

            if (mainCamera == null) mainCamera = Camera.main;
            if (mainCamera == null) return;

            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, draggableLayer))
            {
                // Try to get IDraggable from the object or its parents
                var draggable = hit.collider.GetComponentInParent<IDraggable>();
                if (draggable != null)
                {
                    // Calculate the offset once when we start dragging
                    // We check where the mouse is on the ground to find the relative offset to the unit's pivot
                    if (Physics.Raycast(ray, out RaycastHit groundHit, 100f, groundLayer))
                    {
                        _dragOffset = draggable.GetTransform().position - groundHit.point;
                    }
                    else
                    {
                        _dragOffset = Vector3.zero;
                    }

                    currentDragged = draggable;
                    currentDragged.OnDragStart();
                    
                    foreach (var target in allTargets) target.OnDragStart();
                    OnGlobalDragStart?.Invoke(currentDragged);
                    
                    Debug.Log($"Started dragging: {hit.collider.name}");
                }
            }
        }

        private void UpdateDrag()
        {
            if (mainCamera == null) mainCamera = Camera.main;
            if (mainCamera == null) return;

            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundLayer))
            {
                Vector3 worldPos = hit.point + _dragOffset;
                Vector3 targetPos = worldPos;

                foreach (var target in allTargets)
                {
                    target.TryGetSnapPosition(worldPos, out _); 
                }

                currentDragged.OnDragUpdate(targetPos);
            }
        }

        private void StopDrag()
        {
            if (mainCamera == null) mainCamera = Camera.main;
            if (mainCamera == null) return;

            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            Vector3 finalPos = currentDragged.GetTransform().position;
            
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundLayer))
            {
                Vector3 worldPos = hit.point + _dragOffset;
                foreach (var target in allTargets)
                {
                    if (target.TryGetSnapPosition(worldPos, out Vector3 snappedPos))
                    {
                        finalPos = snappedPos;
                        break;
                    }
                }
            }
            
            currentDragged.OnDrop(finalPos);
            foreach (var target in allTargets) target.OnDragEnd();
            OnGlobalDragEnd?.Invoke(currentDragged);
            currentDragged = null;
        }
    }
}
