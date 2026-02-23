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
        private Vector3 _dragOffset; // To fix the perspective issue you mentioned

        private void Awake()
        {
            mainCamera = Camera.main;
            // Find all potential snap targets in the scene
            var targets = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
            foreach (var target in targets)
            {
                if (target is IDragTarget dragTarget)
                {
                    allTargets.Add(dragTarget);
                }
            }
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
                    
                    Debug.Log($"Started dragging: {hit.collider.name}");
                }
            }
        }

        private void UpdateDrag()
        {
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
            currentDragged = null;
        }
    }
}
