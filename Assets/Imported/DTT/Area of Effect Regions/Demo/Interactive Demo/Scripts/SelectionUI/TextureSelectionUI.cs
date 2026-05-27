using System;
using System.Collections;
using System.Collections.Generic;
using DTT.AreaOfEffectRegions.Demo;
using UnityEngine;
using UnityEngine.UI;

namespace DTT.AreaOfEffectRegions.Demo
{
    /// <summary>
    /// Handle the selection of the texture type.
    /// </summary>
    public class TextureSelectionUI : MonoBehaviour
    {
        /// <summary>
        /// First texture button.
        /// </summary>
        [SerializeField]
        private Button _color1;

        /// <summary>
        /// Second texture button.
        /// </summary>
        [SerializeField]
        private Button _color2;

        /// <summary>
        /// Third texture button.
        /// </summary>
        [SerializeField]
        private Button _color3;

        /// <summary>
        /// Demo scene projector handling.
        /// </summary>
        [SerializeField]
        private SelectionMenuHandler _selectionMenu;

        /// <summary>
        /// Add the listeners for texture buttons.
        /// </summary>
        private void Awake() => AddListener();

        /// <summary>
        /// Add listeners.
        /// </summary>
        private void AddListener()
        {
            _color1.onClick.AddListener(() => _selectionMenu.UpdateProjectorTexture(0));
            _color2.onClick.AddListener(() => _selectionMenu.UpdateProjectorTexture(1));
            _color3.onClick.AddListener(() => _selectionMenu.UpdateProjectorTexture(2));
        }

        /// <summary>
        /// Remove Listeners.
        /// </summary>
        private void RemoveListener()
        {
            _color1.onClick.RemoveListener(() => _selectionMenu.UpdateProjectorTexture(0));
            _color2.onClick.RemoveListener(() => _selectionMenu.UpdateProjectorTexture(1));
            _color3.onClick.RemoveListener(() => _selectionMenu.UpdateProjectorTexture(2));
        }
    }
}
