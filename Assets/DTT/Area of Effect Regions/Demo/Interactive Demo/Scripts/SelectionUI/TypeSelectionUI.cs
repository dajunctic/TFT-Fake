using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DTT.AreaOfEffectRegions.Demo
{
    /// <summary>
    /// Handle the selection of a projector type.
    /// </summary>
    public class TypeSelectionUI : MonoBehaviour
    {
        /// <summary>
        /// Button for the arc projectors.
        /// </summary>
        [SerializeField]
        private Button _arcButton;
        
        /// <summary>
        /// Button for the circle projectors.
        /// </summary>
        [SerializeField]
        private Button _circleButton;
        
        /// <summary>
        /// Button for the line projectors.
        /// </summary>
        [SerializeField]
        private Button _lineButton;
        
        /// <summary>
        /// Button for the scatter line projectors.
        /// </summary>
        [SerializeField]
        private Button _scatterLineButton;

        /// <summary>
        /// Selection menu manager.
        /// </summary>
        [SerializeField]
        private SelectionMenuHandler _selectionMenu;

        /// <summary>
        /// Add listeners for UI and update the text.
        /// </summary>
        private void Awake() => AddListener();

        /// <summary>
        /// Add listeners.
        /// </summary>
        private void AddListener()
        {
            _arcButton.onClick.AddListener(() => _selectionMenu.UpdateProjectorType(ProjectorType.ARC));
            _circleButton.onClick.AddListener(() => _selectionMenu.UpdateProjectorType(ProjectorType.CIRCLE));
            _lineButton.onClick.AddListener(() => _selectionMenu.UpdateProjectorType(ProjectorType.LINE));
            _scatterLineButton.onClick.AddListener(() => _selectionMenu.UpdateProjectorType(ProjectorType.SCATTERLINE));
        }

        /// <summary>
        /// Remove listeners.
        /// </summary>
        private void RemoveListener()
        {
            _arcButton.onClick.RemoveListener(() => _selectionMenu.UpdateProjectorType(ProjectorType.ARC));
            _circleButton.onClick.RemoveListener(() => _selectionMenu.UpdateProjectorType(ProjectorType.CIRCLE));
            _lineButton.onClick.RemoveListener(() => _selectionMenu.UpdateProjectorType(ProjectorType.LINE));
            _scatterLineButton.onClick.RemoveListener(() => _selectionMenu.UpdateProjectorType(ProjectorType.SCATTERLINE));
        }
        
    }   
}