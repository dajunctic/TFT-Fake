using System;
using UnityEngine;

namespace DTT.AreaOfEffectRegions.Demo
{
    /// <summary>
    /// Handle the settings section.
    /// </summary>
    public class SettingsHandler : MonoBehaviour
    {
        /// <summary>
        /// Arc settings field.
        /// </summary>
        [SerializeField]
        private ArcSettings _arcSettings;
        
        /// <summary>
        /// Circle settings field.
        /// </summary>
        [SerializeField]
        private CircleSettings _circleSettings;
        
        /// <summary>
        /// Line settings field.
        /// </summary>
        [SerializeField]
        private lineSettings _lineSettings;
        
        /// <summary>
        /// Scatter settings field.
        /// </summary>
        [SerializeField]
        private ScatterLineSettings _scatterSettings;
        
        /// <summary>
        /// Update the currently displayed settings.
        /// </summary>
        /// <param name="newindicator">Selected projector indicator.</param>
        /// <param name="type">Type of the selected projector.</param>
        /// <exception cref="ArgumentOutOfRangeException">Not recognized type of projector.</exception>
        public void UpdateSettings(GameObject newindicator, ProjectorType type)
        {
            switch (type)
            {
                case ProjectorType.ARC:
                    _arcSettings.ChangeIndicator(newindicator.GetComponent<ArcRegionProjector>());
                    ShowSettings(_arcSettings.gameObject);
                    break;
                case ProjectorType.CIRCLE:
                    _circleSettings.ChangeIndicator(newindicator.GetComponent<CircleRegionProjector>());
                    ShowSettings(_circleSettings.gameObject);
                    break;
                case ProjectorType.LINE:
                    _lineSettings.ChangeIndicator(newindicator.GetComponent<LineRegionProjector>());
                    ShowSettings(_lineSettings.gameObject);
                    break;
                case ProjectorType.SCATTERLINE:
                    _scatterSettings.ChangeIndicator(newindicator.GetComponent<ScatterLineRegionProjector>());
                    ShowSettings(_scatterSettings.gameObject);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
        
        /// <summary>
        /// Show a settings window.
        /// </summary>
        /// <param name="settings">Which settings window to show.</param>
        private void ShowSettings(GameObject settings)
        {
            _scatterSettings.gameObject.SetActive(false);
            _lineSettings.gameObject.SetActive(false);
            _circleSettings.gameObject.SetActive(false);
            _arcSettings.gameObject.SetActive(false);

            settings.SetActive(true);
        }
    }
}