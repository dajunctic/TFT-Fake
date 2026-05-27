using DTT.AreaOfEffectRegions.Shaders;
using DTT.Utils.Extensions;
using UnityEngine;
using UnityEngine.Serialization;

namespace DTT.AreaOfEffectRegions
{
    /// <summary>
    /// A circle region that uses projectors for effects.
    /// </summary>
    [ExecuteAlways]
    public class CircleRegionProjector : MonoBehaviour
    {
        [Header("Projectors")]
        /// <summary>
        /// The circle projector.
        /// </summary>
        [SerializeField]
        private Projector _circleProjector;

        /// <summary>
        /// The center circle projector.
        /// </summary>
        [SerializeField]
        private Projector _centerDotProjector;

        /// <summary>
        /// The transform of the game object that holds the the projectors.
        /// </summary>
        [SerializeField]
        private Transform _regionTransform;

        /// <summary>
        /// The radius of the circle.
        /// </summary>
        public float Radius
        {
            get => _radius;
            set => _radius = Mathf.Max(value, 0f);
        }

        /// <summary>
        /// The offset of the circle from it's object.
        /// </summary>
        public Vector2 Offset
        {
            get => _offset;
            set => _offset = value;
        }

        /// <summary>
        /// The amount the circle is filled.
        /// </summary>
        public float FillProgress
        {
            get => _fillProgress;
            set => _fillProgress = Mathf.Clamp01(value);
        }

        [Header("Properties")]
        /// <summary>
        /// The offset of the circle from it's object.
        /// </summary>
        [SerializeField]
        private Vector2 _offset;

        /// <summary>
        /// The radius of the circle.
        /// </summary>
        [SerializeField]
        [Min(0f)]
        private float _radius;

        /// <summary>
        /// The amount the circle is filled.
        /// </summary>
        [Range(0f, 1f)]
        [SerializeField]
        private float _fillProgress;
        
        /// <summary>
        /// Origin of the fill color.
        /// </summary>
        [SerializeField]
        [Tooltip("The origin of the fill color.")]
        private Origin _fillOrigin;

        /// <summary>
        /// The base color for the circle.
        /// </summary>
        [Header("Colors")]
        [SerializeField]
        private Color _circleBaseColor = Color.white;

        /// <summary>
        /// The fill color for the circle.
        /// </summary>
        [SerializeField]
        private Color _circleFillColor = Color.white;

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
        public void OnValidate() => UpdateProjectors();

        /// <summary>
        /// Create a copy of the used material when runtime starts.
        /// This is done so changes in one projector's materials are not applied
        /// to projectors that use the same material.
        /// </summary>
        private void ReassignMaterials()
        {
            if (_circleProjector != null)
                _circleProjector.material = new Material(_circleProjector.material);

            if (_centerDotProjector != null)
                _centerDotProjector.material = new Material(_centerDotProjector.material);
        }

        /// <summary>
        /// Updates all projectors of this region.
        /// </summary>
        public void UpdateProjectors()
        {
            UpdateCircleProjector();
            UpdateCenterDotProjector();
            UpdateProjectorPosition();
        }

        /// <summary>
        /// Updates the Circle projector properties.
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
        }

        /// <summary>
        /// Updates the Center Dot projector properties.
        /// </summary>
        private void UpdateCenterDotProjector()
        {
            if (_centerDotProjector == null)
                return;

            _centerDotProjector.orthographicSize = _radius * 0.25f;

            if (_centerDotProjector.material == null)
                return;

            _centerDotProjector.material.SetColor(ColorShaderID, _centerDotColor);
        }

        /// <summary>
        /// Updates the position of the game object that holds the game objects with projector components.
        /// </summary>
        private void UpdateProjectorPosition()
        {
            if (_regionTransform != null)
                _regionTransform.localPosition = new Vector3(Offset.x, 0, Offset.y);
        }

        /// <summary>
        /// Deactivates and activates the center dot projector so it always shows
        /// up on top of the circle projector.
        /// </summary>
        private void ResetProjectorsRenderingOrder()
        {
            _centerDotProjector.gameObject.SetActive(false);
            _centerDotProjector.gameObject.SetActive(true);
        }
    }
}