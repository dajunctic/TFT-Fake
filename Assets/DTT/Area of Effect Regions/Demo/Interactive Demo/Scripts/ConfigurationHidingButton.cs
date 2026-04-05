using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace DTT.AreaOfEffectRegions.Demo
{
    [RequireComponent(typeof(Button))]
    /// <summary>
    /// Handle the dropdown arrow of the configuration.
    /// </summary>
    public class ConfigurationHidingButton : MonoBehaviour
    {
        /// <summary>
        /// The settings tab of the UI.
        /// </summary>
        [SerializeField]
        private GameObject _settingsTab;

        /// <summary>
        /// The complete configuration UI panel.
        /// </summary>
        [SerializeField]
        private RectTransform _configurationTab;

        /// <summary>
        /// Anchor value when configuraiton is open.
        /// </summary>
        private float _topAnchorValue;

        /// <summary>
        /// Whether the button will show the configuration or hide it on click.
        /// </summary>
        private bool _goingUp;

        /// <summary>
        /// The arrow Button.
        /// </summary>
        private Button _arrowButton;

        /// <summary>
        /// Initialize the button and original anchor value.
        /// </summary>
        private void Awake()
        {
            _arrowButton = GetComponent<Button>();
            _topAnchorValue = _configurationTab.anchorMax.y;
            _goingUp = false;
            AddListener();
        }

        /// <summary>
        /// Add listener for the arrow button.
        /// </summary>
        private void AddListener() => _arrowButton.onClick.AddListener(ChangeConfigState);

        /// <summary>
        /// Remove listener of the arrow button.
        /// </summary>
        private void RemoveListener() => _arrowButton.onClick.RemoveListener(ChangeConfigState);

        /// <summary>
        /// Handle openning or closing the configuration tab.
        /// </summary>
        private void ChangeConfigState() => StartCoroutine(HideConfigAnimation());

        /// <summary>
        /// Animate the closing of the configuration tab.
        /// </summary>
        /// <returns><see cref="IEnumerator{T}"/></returns>
        private IEnumerator HideConfigAnimation()
        {
            while (_configurationTab.anchorMax.y > 0)
            {
                _configurationTab.anchorMax += new Vector2(0, (_goingUp) ? 0.1f : -0.1f);
                yield return new WaitForSeconds(0.01f);
            }
            _configurationTab.anchorMax = new Vector2(_configurationTab.anchorMax.x, (_goingUp) ? _topAnchorValue : 0f);
            RectTransform trans = (RectTransform) gameObject.transform;
            trans.Rotate(Vector3.forward, 180);
            _settingsTab.SetActive(!_settingsTab.activeInHierarchy);
            _goingUp = !_goingUp;
        }
    }
}
