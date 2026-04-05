using System;
using DTT.AreaOfEffectRegions.Shaders;
using DTT.Utils.Extensions;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace DTT.AreaOfEffectRegions
{
    /// <summary>
    /// An arc region that uses projectors for effects.
    /// </summary>
    [ExecuteAlways]
    public class ArcRegionProjector : MonoBehaviour
    {
        [Header("Projectors")]
        /// <summary>
        /// The circle projector.
        /// </summary>
        [SerializeField]
        private Projector _circleProjector;

        /// <summary>
        /// The circle decorator projector.
        /// </summary>
        [SerializeField]
        private Projector _circleDecorationProjector;

        /// <summary>
        /// The center circle projector.
        /// </summary>
        [SerializeField]
        private Projector _centerDotProjector;

        /// <summary>
        /// The left border of the arc projector.
        /// </summary>
        [SerializeField]
        private Projector _leftBorderProjector;

        /// <summary>
        /// The right border of the arc projector.
        /// </summary>
        [SerializeField]
        private Projector _rightBorderProjector;

        /// <summary>
        /// The rotation pivot of the left border projector.
        /// </summary>
        [SerializeField]
        private Transform _leftBorderPivot;

        /// <summary>
        /// The rotation pivot of the right border projector.
        /// </summary>
        [SerializeField]
        private Transform _rightBorderPivot;

        /// <summary>
        /// The amount the line is filled.
        /// </summary>
        public float FillProgress
        {
            get => _fillProgress;
            set => _fillProgress = Mathf.Clamp01(value);
        }

        /// <summary>
        /// The length of the arc.
        /// </summary>
        public float Radius
        {
            get => _radius;
            set => _radius = Mathf.Max(value, 0f);
        }

        /// <summary>
        /// The arc in euler angles. This is the amount that it's opened, 0 being closed and 360 being fully open.
        /// </summary>
        public float Arc
        {
            get => _arc;
            set => _arc = Mathf.Clamp(value, 0f, 360f);
        }

        /// <summary>
        /// The angle offset for the cone in euler angles.
        /// </summary>
        public float Angle
        {
            get => _angle;
            set => _angle = value;
        }

        /// <summary>
        /// The arc angle normalized to a range of 0 to 1.
        /// </summary>
        public float ArcAngleNormalized => 1f - Arc / 360;

        /// <summary>
        /// Normalizes the inspector angle value to a range of 0 to 360.
        /// If value is higher than 360, the the angle repeats. 
        /// Example: 0 to 90 degrees is the same rotation angle as 0 to 450 degrees.
        /// </summary>
        public float NormalizedAngle => Mathf.Repeat((Angle - 90) % 360, 360) / 360;

        /// <summary>
        /// The angle in euler for the left side of the arc.
        /// </summary>
        public float LeftAngle => _angle - _arc / 2;

        /// <summary>
        /// The angle in euler for the right side of the arc.
        /// </summary>
        public float RightAngle => _angle + _arc / 2;

        [Header("Properties")]
        /// <summary>
        /// The length of the arc.
        /// </summary>
        [SerializeField]
        [Min(0f)]
        private float _radius = 1.0f;

        /// <summary>
        /// The angle offset for the cone in euler angles.
        /// </summary>
        [SerializeField]
        private float _angle;

        /// <summary>
        /// The arc in euler angles. This is the amount that it's opened, 0 being closed and 360 being fully open.
        /// </summary>
        [SerializeField]
        [Range(0, 360)]
        private float _arc = 30.0f;

        /// <summary>
        /// The progress of the fill.
        /// </summary>
        [SerializeField]
        [Range(0, 1)]
        private float _fillProgress;
        
        /// <summary>
        /// Origin of the fill color.
        /// </summary>
        [SerializeField]
        [Tooltip("The origin of the fill color.")]
        private Origin _fillOrigin;

        /// <summary>
        /// The projector's 'y' position.
        /// </summary>
        private const float Y_POSITION = 0.15f;

        /// <summary>
        /// The size of the left and right border projectors is 60% of that of the circle projector.
        /// </summary>
        private const float BORDER_TO_CIRCLE_SIZE_RATIO = 0.6f;

        /// <summary>
        /// The Z axis displacement of the borders within their pivot points, this value centers the
        /// border's round pivot based on its actual size. This value works for the currently used
        /// border textures and their projector size of 60% of that of the circle projector.
        /// </summary>
        private const float Z_AXIS_DISPLACEMENT = 0.5665f;

        /// <summary>
        /// The size ration between the center dot to that of the circle projector.
        /// </summary>
        private const float CENTER_DOT_TO_CIRCLE_SIZE_RATIO = 0.2f;

        [Header("Colors")]
        /// <summary>
        /// The base color for the main circle.
        /// </summary>
        [SerializeField]
        private Color _circleBaseColor = Color.white;
        
        /// <summary>
        /// The fill color for the main circle.
        /// </summary>
        [SerializeField]
        private Color _circleFillColor = Color.white;

        /// <summary>
        /// The base color for the decorator circle.
        /// </summary>
        [SerializeField]
        private Color _circleDecoratorBaseColor = Color.white;

        /// <summary>
        /// The fill color for the decorator circle.
        /// </summary>
        [SerializeField]
        private Color _circleDecoratorFillColor = Color.white;

        /// <summary>
        /// The color for the left border of the arc.
        /// </summary>
        [SerializeField]
        private Color _leftBorderColor = Color.white;

        /// <summary>
        /// The color for the right border of the arc.
        /// </summary>
        [SerializeField]
        private Color _rightBorderColor = Color.white;

        /// <summary>
        /// The color for the center dot.
        /// </summary>
        [SerializeField]
        private Color _centerDotColor = Color.white;

        /// <summary>
        /// The shader ID of the <c>_Color</c> property.
        /// </summary>
        private static readonly int ColorShaderID = Shader.PropertyToID("_Color");

        /// <summary>
        /// The shader ID of the <c>_FillColor</c> property.
        /// </summary>
        private static readonly int FillColorShaderID = Shader.PropertyToID("_FillColor");

        /// <summary>
        /// The shader ID of the <c>_FillProgress</c> property.
        /// </summary>
        private static readonly int FillProgressShaderID = Shader.PropertyToID("_FillProgress");

        /// <summary>
        /// The ID of the <c>_Arc</c> property.
        /// </summary>
        private static readonly int ArcShaderID = Shader.PropertyToID("_Arc");

        /// <summary>
        /// The ID of the <c>_Angle</c> property.
        /// </summary>
        private static readonly int AngleShaderID = Shader.PropertyToID("_Angle");
        
        /// <summary>
        /// The shader ID of the <c>_Origin</c> property.
        /// </summary>
        private static readonly int FillDirection = Shader.PropertyToID("_Origin");

        /// <summary>
        /// Reassigns all materials and updates all projectors and their rendering order.
        /// </summary>
        private void Start()
        {
            ReassignMaterials();
            UpdateProjectors();
            ResetProjectorsRenderingOrder();
        }

        /// <summary>
        /// Update projectors if there are any changes made in the inspector.
        /// </summary>
        private void OnValidate() => UpdateProjectors();

        /// <summary>
        /// Create a copy of the used material when runtime starts.
        /// This is done so changes in one projector's materials are not applied
        /// to projectors that use the same material.
        /// </summary>
        private void ReassignMaterials()
        {
            if (_circleProjector != null)
                _circleProjector.material = new Material(_circleProjector.material);

            if (_circleDecorationProjector != null)
                _circleDecorationProjector.material = new Material(_circleDecorationProjector.material);

            if (_centerDotProjector != null)
                _centerDotProjector.material = new Material(_centerDotProjector.material);

            if (_leftBorderProjector != null)
                _leftBorderProjector.material = new Material(_leftBorderProjector.material);

            if (_rightBorderProjector != null)
                _rightBorderProjector.material = new Material(_rightBorderProjector.material);
        }

        /// <summary>
        /// Updates all projectors of this region.
        /// </summary>
        public void UpdateProjectors()
        {
            UpdateCircleProjector();
            UpdateCircleDecorationProjector();
            UpdateCenterDotProjector();
            UpdateLeftBorderProjector();
            UpdateRightBroderProjector();
            UpdateLeftBorderPivot();
            UpdateRightBorderPivot();
        }

        /// <summary>
        /// Updates the circle projector properties.
        /// </summary>
        private void UpdateCircleProjector()
        {
            if (_circleProjector == null)
                return;

            _circleProjector.orthographicSize = _radius;

            if (_circleProjector.material == null)
                return;

            _circleProjector.material.SetColor(ColorShaderID, _circleBaseColor);
            _circleProjector.material.SetColor(FillColorShaderID, _circleFillColor);
            _circleProjector.material.SetInt(FillDirection, _fillOrigin.ToInt());
            _circleProjector.material.SetFloat(FillProgressShaderID, _fillProgress);
            _circleProjector.material.SetFloat(ArcShaderID, ArcAngleNormalized);
            _circleProjector.material.SetFloat(AngleShaderID, NormalizedAngle);
        }

        /// <summary>
        /// Updates the circle decoration projector properties.
        /// </summary>
        private void UpdateCircleDecorationProjector()
        {
            if (_circleDecorationProjector == null)
                return;

            _circleDecorationProjector.orthographicSize = _radius;

            if (_circleDecorationProjector.material == null)
                return;

            _circleDecorationProjector.material.SetColor(ColorShaderID, _circleDecoratorBaseColor);
            _circleDecorationProjector.material.SetColor(FillColorShaderID, _circleDecoratorFillColor);
            _circleDecorationProjector.material.SetInt(FillDirection, _fillOrigin.ToInt());
            _circleDecorationProjector.material.SetFloat(FillProgressShaderID, _fillProgress);
            _circleDecorationProjector.material.SetFloat(ArcShaderID, ArcAngleNormalized);
            _circleDecorationProjector.material.SetFloat(AngleShaderID, NormalizedAngle);
        }

        /// <summary>
        /// Updates the center dot projector properties.
        /// </summary>
        private void UpdateCenterDotProjector()
        {
            if (_centerDotProjector == null)
                return;

            _centerDotProjector.orthographicSize = _radius * CENTER_DOT_TO_CIRCLE_SIZE_RATIO;

            if (_centerDotProjector.material == null)
                return;

            _centerDotProjector.material.SetColor(ColorShaderID, _centerDotColor);
        }

        /// <summary>
        /// Updates the left border projector properties.
        /// </summary>
        private void UpdateLeftBorderProjector()
        {
            if (_leftBorderProjector == null)
                return;

            _leftBorderProjector.transform.localPosition = new Vector3(0, Y_POSITION, Z_AXIS_DISPLACEMENT * _radius);
            _leftBorderProjector.orthographicSize = _radius * BORDER_TO_CIRCLE_SIZE_RATIO;

            if (_leftBorderProjector.material == null)
                return;

            _leftBorderProjector.material.SetColor(ColorShaderID, _leftBorderColor);
        }

        /// <summary>
        /// Updates the right border projector properties.
        /// </summary>
        private void UpdateRightBroderProjector()
        {
            if (_rightBorderProjector == null)
                return;

            _rightBorderProjector.orthographicSize = _radius * BORDER_TO_CIRCLE_SIZE_RATIO;
            _rightBorderProjector.transform.localPosition = new Vector3(0, Y_POSITION, Z_AXIS_DISPLACEMENT * _radius);

            if (_rightBorderProjector.material == null)
                return;

            _rightBorderProjector.material.SetColor(ColorShaderID, _rightBorderColor);
        }

        /// <summary>
        /// Updates the left border projector's pivot point.
        /// </summary>
        private void UpdateLeftBorderPivot()
        {
            if (_leftBorderPivot != null)
                _leftBorderPivot.localEulerAngles = new Vector3(0, LeftAngle, 0);
        }

        /// <summary>
        /// Updates the right border projector's pivot point.
        /// </summary>
        private void UpdateRightBorderPivot()
        {
            if (_rightBorderPivot != null)
                _rightBorderPivot.localEulerAngles = new Vector3(0, RightAngle, 0);
        }

        /// <summary>
        /// Deactivates and activates the projectors so the borders and center dot show
        /// up on top of the circle and circle decorator projectors.
        /// </summary>
        private void ResetProjectorsRenderingOrder()
        {
            // Deactivate all.
            _circleProjector.gameObject.SetActive(false);
            _circleDecorationProjector.gameObject.SetActive(false);
            _centerDotProjector.gameObject.SetActive(false);
            _leftBorderProjector.gameObject.SetActive(false);
            _rightBorderProjector.gameObject.SetActive(false);

            // Reactivate in order.
            _circleDecorationProjector.gameObject.SetActive(true);
            _circleProjector.gameObject.SetActive(true);
            _leftBorderProjector.gameObject.SetActive(true);
            _rightBorderProjector.gameObject.SetActive(true);
            _centerDotProjector.gameObject.SetActive(true);
        }
    }
}