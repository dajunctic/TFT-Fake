using UnityEngine;
using UnityEngine.UI;

namespace DTT.AreaOfEffectRegions.Demo
{
    /// <summary>
    /// Handle circle settings ui.
    /// </summary>
    public class CircleSettings : MonoBehaviour
    {
        /// <summary>
        /// Slider that handle the radius value of the circle projector.
        /// </summary>
        [SerializeField]
        private Slider _radius;
        
        /// <summary>
        /// Slider that handle the fill progress value of the circle projector.
        /// </summary>
        [SerializeField]
        private Slider _fillProgress;

        /// <summary>
        /// Current circle projector that is being altered.
        /// </summary>
        private CircleRegionProjector _projector;

        /// <summary>
        /// Initialize the slider max and min value.
        /// </summary>
        private void InitializeSettings(){
            _radius.minValue = 50;
            _radius.maxValue = 200;
            
            _fillProgress.minValue = 0;
            _fillProgress.maxValue = 1;
        }

        /// <summary>
        /// Initialize the slider value to match the projector value.
        /// </summary>
        private void InitializeValues()
        {
            _radius.value = _projector.Radius ;
            _fillProgress.value = _projector.FillProgress;
        }

        /// <summary>
        /// Add listener for the slider changes.
        /// </summary>
        private void AddListener()
        {
            _fillProgress.onValueChanged.AddListener(UpdateFill);
            _radius.onValueChanged.AddListener(UpdateRadius);
        }

        /// <summary>
        /// Remove listener for the slider changes.
        /// </summary>
        private void RemoveListerner()
        {
            _fillProgress.onValueChanged.RemoveListener(UpdateFill);
            _radius.onValueChanged.RemoveListener(UpdateRadius);
        }
        
        /// <summary>
        /// Update the radius value of the projector.
        /// </summary>
        /// <param name="radius">New radius radius value.</param>
        private void UpdateRadius(float radius)
        {
            _projector.Radius = radius;
            _projector.UpdateProjectors();
        }
        
        /// <summary>
        /// Update the fill value of the projector.
        /// </summary>
        /// <param name="fill">New fill amount value.</param>
        private void UpdateFill(float fill)
        {
            _projector.FillProgress = fill;
            _projector.UpdateProjectors();
        }

        /// <summary>
        /// Change the current projector.
        /// </summary>
        /// <param name="indic">New indicator.</param>
        public void ChangeIndicator(CircleRegionProjector indic)
        {
            RemoveListerner();
            _projector = indic;
            InitializeSettings();
            InitializeIndicator();
            InitializeValues();
            _radius.value = _projector.Radius ;
            _fillProgress.value = _projector.FillProgress;
            AddListener();
        }

        /// <summary>
        /// Initialize the projector.
        /// </summary>
        private void InitializeIndicator() => _projector.Radius = 50;
    }
}