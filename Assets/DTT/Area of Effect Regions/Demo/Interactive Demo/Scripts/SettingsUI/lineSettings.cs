using UnityEngine;
using UnityEngine.UI;

namespace DTT.AreaOfEffectRegions.Demo
{
    /// <summary>
    /// Handle line settings UI.
    /// </summary>
    public class lineSettings : MonoBehaviour
    { 
        /// <summary>
        /// Slider that handle the fill value of the circle projector.
        /// </summary>
        [SerializeField]
        private Slider _fillProgress;
        
        /// <summary>
        /// Slider that handle the fade value of the circle projector.
        /// </summary>
        [SerializeField]
        private Slider _fadeAmount;
        
        /// <summary>
        /// Slider that handle the angle value of the circle projector.
        /// </summary>
        [SerializeField]
        private Slider _angle;
        
        /// <summary>
        /// Slider that handle the length value of the circle projector.
        /// </summary>
        [SerializeField]
        private Slider _length;
        
        /// <summary>
        /// Slider that handle the width value of the circle projector.
        /// </summary>
        [SerializeField]
        private Slider _width;

        /// <summary>
        /// Current line projector.
        /// </summary>
        private LineRegionProjector _projector;

        /// <summary>
        /// Initialize slider value to match the projector value.
        /// </summary>
        private void InitializeValues()
        {
            _fillProgress.value = _projector.FillProgress;
            _fadeAmount.value = _projector.FadeAmount;
            _angle.value = _projector.Angle;
            
            _length.value = _projector.Length;
            _width.value = _projector.Width;
        }
        
        /// <summary>
        /// Initialize the slider min and max value.
        /// </summary>
        private void InitializeSlider()
        {
            _fillProgress.maxValue = 1;
            _fillProgress.minValue = 0;
            _fadeAmount.maxValue = 1;
            _fadeAmount.minValue = 0;
            _angle.maxValue = 360;
            _angle.minValue = 0;
            _length.maxValue = 100;
            _length.minValue = 25;
            _width.maxValue = 25;
            _width.minValue = 1;
        }
        
        /// <summary>
        /// Add listener for the slider value change.
        /// </summary>
        private void AddListener()
        {
            _fillProgress.onValueChanged.AddListener(UpdateFill);
            _fadeAmount.onValueChanged.AddListener(UpdateFade);
            _angle.onValueChanged.AddListener(UpdateAngle);
            
            _length.onValueChanged.AddListener(UpdateLength);
            _width.onValueChanged.AddListener(UpdateWidth);
        }

        /// <summary>
        /// Remove listener for the slider value change.
        /// </summary>
        private void RemoveListerner()
        {
            _fillProgress.onValueChanged.RemoveListener(UpdateFill);
            _fadeAmount.onValueChanged.RemoveListener(UpdateFade);
            _angle.onValueChanged.RemoveListener(UpdateAngle);
            
            _length.onValueChanged.RemoveListener(UpdateLength);
            _width.onValueChanged.RemoveListener(UpdateWidth);
        }
        
        /// <summary>
        /// Update the fade value of the projector.
        /// </summary>
        /// <param name="fade">New fade value.</param>
        private void UpdateFade(float fade)
        {
            _projector.FadeAmount = fade;
            _projector.UpdateProjectors();
        }
        
        /// <summary>
        /// Update the angle value of the projector.
        /// </summary>
        /// <param name="angle">New angle value.</param>
        private void UpdateAngle(float angle)
        {
            _projector.Angle = angle;
            _projector.UpdateProjectors();
        }
        
        /// <summary>
        /// Update the length value of the projector.
        /// </summary>
        /// <param name="length">New length value.</param>
        private void UpdateLength(float length)
        {
            _projector.Length = length;
            _projector.UpdateProjectors();
        }
        
        /// <summary>
        /// Update the width value of the projector.
        /// </summary>
        /// <param name="width">New width value.</param>
        private void UpdateWidth(float width)
        {
            _projector.Width = width;
            _projector.UpdateProjectors();
        }
        
        /// <summary>
        /// Update the fill value of the projector.
        /// </summary>
        /// <param name="arc">New arc value.</param>
        private void UpdateFill(float arc)
        {
            _projector.FillProgress = arc;
            _projector.UpdateProjectors();
        }

        /// <summary>
        /// Change the current projector.
        /// </summary>
        /// <param name="indic">New Line projector Value.</param>
        public void ChangeIndicator(LineRegionProjector indic)
        {
            RemoveListerner();
            InitializeSlider();
            _projector = indic;
            InitializeIndicator();
            InitializeValues();
            AddListener();
        }

        /// <summary>
        /// Initialize the projector value.
        /// </summary>
        private void InitializeIndicator() => _projector.Length = 15;
    }
}