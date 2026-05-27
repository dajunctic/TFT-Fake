using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

namespace DTT.AreaOfEffectRegions.Demo
{
    /// <summary>
    /// Handle Moving the indicator on mouse pointer.
    /// </summary>
    public class MoveObject : MonoBehaviour
    {
        /// <summary>
        /// Layer of the terrain to render the indicator on.
        /// </summary>
        [SerializeField] private LayerMask _terrainLayer;
        
        /// <summary>
        /// Projector of the indicator.
        /// </summary>
        [SerializeField] private GameObject _projector;
        
        /// <summary>
        /// Anchor point of the indicator.
        /// </summary>
        [SerializeField]
        private GameObject _anchorPoint;

        /// <summary>
        /// Is the mouse currently over the game window.
        /// </summary>
        private bool _isMouseOverGameWindow => !(0 > Input.mousePosition.x || 
                                                0 > Input.mousePosition.y || 
                                                Screen.width < Input.mousePosition.x || 
                                                Screen.height < Input.mousePosition.y);
        
        /// <summary>
        /// Current object position.
        /// </summary>
        private Vector3 _currentPosition = new Vector3(0,0,0);
        
        /// <summary>
        /// Main camera of the scene.
        /// </summary>
        private Camera _camera;

        /// <summary>
        /// Instantiate the camera.
        /// </summary>
        private void Awake() => _camera = Camera.main;

        /// <summary>
        /// Get the mouse position on the terrain.
        /// </summary>
        /// <returns>Position of the mouse on the terrain.</returns>
        private Vector3 GetPosition()
        {
            if (EventSystem.current.IsPointerOverGameObject() || !_isMouseOverGameWindow || IsPointerOverUIElement(GetEventSystemRaycastResults()))
                return _anchorPoint.transform.position;
            RaycastHit hit;
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            _currentPosition = Physics.Raycast(ray, out hit,Mathf.Infinity,_terrainLayer) ? hit.point+new Vector3(0,50,0) : new Vector3(0, 50, 0);
            return _currentPosition;
        }

        /// <summary>
        /// Update the current projector position.
        /// </summary>
        private void Update()
        {
            if (!_projector)
                return;
            _projector.transform.position = GetPosition();
        }

        /// <summary>
        /// Change the displayed projector.
        /// </summary>
        /// <param name="projector">The new projector.</param>
        public void ChangeIndicator(GameObject projector) => _projector = projector;
        
        /// <summary>
        /// Look if the input is over an UI element.
        /// </summary>
        /// <param name="eventSystemRaycastResults">Result of the raycast.</param>
        /// <returns>True if we touched or hovering on Unity UI element.</returns>
        private bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaycastResults) => eventSystemRaycastResults.Any(curRaycastResult => curRaycastResult.gameObject.layer == LayerMask.NameToLayer("UI"));
        
        /// <summary>
        /// Gets all event system raycast results of current mouse or touch position.
        /// </summary>
        /// <returns>List of raycast result from input.</returns>
        private List<RaycastResult> GetEventSystemRaycastResults()
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;
            List<RaycastResult> raysastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, raysastResults);
            return raysastResults;
        }
    }
}
