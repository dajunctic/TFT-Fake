using DTT.AreaOfEffectRegions;
using UnityEngine;
using UnityEngine.UI;

namespace DTT.AreaOfEffectRegions.Demo
{
    /// <summary>
    /// Handle arc settings ui.
    /// </summary>
    public class ArcSettings : MonoBehaviour
    { 
        /// <summary>
        /// Slider that handle the radius.
        /// </summary>
        [SerializeField]
        private Slider _radius;
        
        /// <summary>
        /// Slider that handle the angle.
        /// </summary>
        [SerializeField]
        private Slider _angle;
        
        /// <summary>
        /// Slider that handler the arc angle.
        /// </summary>
        [SerializeField]
        private Slider _arc;
        
        /// <summary>
        /// Slider that handle the fill amount of the arc.
        /// </summary>
        [SerializeField]
        private Slider _fillProgress;
        
        /// <summary>
        /// Arc projector that can be modified with this UI.
        /// </summary>
        private ArcRegionProjector _projector;

        /// <summary>
        /// Initialize the sliders min and max value.
        /// </summary>
        private void InitializeSlider()
        {
            _radius.maxValue = 200;
            _radius.minValue = 50;
            
            _angle.maxValue = 360;
            _angle.minValue = 0;
            
            _arc.maxValue = 360;
            _arc.minValue = 0;
            
            _fillProgress.minValue = 0;
            _fillProgress.maxValue = 1;
        }
        
        /// <summary>
        /// Initialize the slider values to match the projector Value.
        /// </summary>
        private void InitializeValues()
        {
            _angle.value = _projector.Angle ;
            _radius.value = _projector.Radius ;
            _arc.value = _projector.Arc ;
            _fillProgress.value = _projector.FillProgress;
        }

        /// <summary>
        /// Add listeners for sliders.
        /// </summary>
        private void AddListener()
        {
            _arc.onValueChanged.AddListener(UpdateArc);
            _fillProgress.onValueChanged.AddListener(UpdateFill);
            _angle.onValueChanged.AddListener(UpdateAngle);
            _radius.onValueChanged.AddListener(UpdateRadius);
        }

        /// <summary>
        /// Remove listeners for sliders.
        /// </summary>
        private void RemoveListerner()
        {
            _arc.onValueChanged.RemoveListener(UpdateArc);
            _fillProgress.onValueChanged.RemoveListener(UpdateFill);
            _angle.onValueChanged.RemoveListener(UpdateAngle);
            _radius.onValueChanged.RemoveListener(UpdateRadius);
        }
        
        /// <summary>
        /// Update the arc value of the projector.
        /// </summary>
        /// <param name="arc">New arc value.</param>
        private void UpdateArc(float arc)
        {
            _projector.Arc = arc;
            _projector.UpdateProjectors();
        }
        
        /// <summary>
        /// Update the radius value of the projector.
        /// </summary>
        /// <param name="arc">New radius value.</param>
        private void UpdateRadius(float arc)
        {
            _projector.Radius = arc;
            _projector.UpdateProjectors();
        }
        
        /// <summary>
        /// Update the fill value of the projector.
        /// </summary>
        /// <param name="arc">New fill value.</param>
        private void UpdateFill(float arc)
        {
            _projector.FillProgress = arc;
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
        /// Change the projector type.
        /// </summary>
        /// <param name="indic">The new projector.</param>
        public void ChangeIndicator(ArcRegionProjector indic)
        {
            RemoveListerner();
            _projector = indic;
            InitializeSlider();
            InitializeIndicator();
            InitializeValues();
            _angle.value = _projector.Angle ;
            _radius.value = _projector.Radius ;
            _arc.value = _projector.Arc ;
            _fillProgress.value = _projector.FillProgress;
            AddListener();
        }
        
        /// <summary>
        /// Initialize the projector so it can be seen in the scene.
        /// </summary>
        private void InitializeIndicator() => _projector.Radius = 50;
    }
}