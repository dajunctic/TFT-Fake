using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace DTT.AreaOfEffectRegions.Demo
{
    /// <summary>
    /// Different type of projector.
    /// </summary>
    public enum ProjectorType
    {
        ARC = 0,
        CIRCLE = 1,
        LINE = 2,
        SCATTERLINE =3,
    }

    /// <summary>
    /// Handle the selection menu.
    /// </summary>
    public class SelectionMenuHandler : MonoBehaviour
    {
        /// <summary>
        /// Object that hold the projector.
        /// </summary>
        [SerializeField]
        private MoveObject _indicator;
        
        /// <summary>
        /// Handle settings of the configuration time.
        /// </summary>
        [SerializeField]
        private SettingsHandler _settingsHandler;

        /// <summary>
        /// List of projectors.
        /// </summary>
        private List<GameObject> _projectors;

        /// <summary>
        /// Current selected type of projectors.
        /// </summary>
        private ProjectorType _currentType;
        
        /// <summary>
        /// Currently selected texture for the projector.
        /// </summary>
        private int _currentTexture;

        /// <summary>
        /// Initialize the projectors.
        /// </summary>
        private void Awake()
        {
            _projectors = new List<GameObject>();
            foreach (Transform child in _indicator.transform)
                _projectors.Add(child.gameObject);
            UpdateIndicator(0);
        }
        
        /// <summary>
        /// Update the displayed indicator.
        /// </summary>
        /// <param name="index">Index of the indicator to be displayed.</param>
        public void UpdateIndicator(int index)
        {
            foreach (GameObject projector in _projectors)
                projector.SetActive(false);
            _projectors[index].SetActive(true);
            _indicator.ChangeIndicator(_projectors[index]);
            _settingsHandler.UpdateSettings(_projectors[index], _currentType);
        }

        /// <summary>
        /// Update the selected projector type.
        /// </summary>
        /// <param name="type">New projector type.</param>
        public void UpdateProjectorType(ProjectorType type)
        {
            _currentType = type;
            UpdateIndicator(((int)_currentType * 3) + _currentTexture);
        }

        /// <summary>
        /// Update the selected projector texture.
        /// </summary>
        /// <param name="texture">New projector texture index.</param>
        public void UpdateProjectorTexture(int texture)
        {
            _currentTexture = texture;
            UpdateIndicator(((int)_currentType * 3) + _currentTexture);
        }
    }
}