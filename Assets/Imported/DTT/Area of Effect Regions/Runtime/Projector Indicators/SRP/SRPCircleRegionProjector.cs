using UnityEngine;
#if UNIVERSAL_PACKAGE
using UnityEngine.Rendering.Universal;
#endif

namespace DTT.AreaOfEffectRegions
{
    /// <summary>
    /// A circle region that uses projectors for effects.
    /// </summary>
    [ExecuteAlways]
    public class SRPCircleRegionProjector : MonoBehaviour
    {
#if UNIVERSAL_PACKAGE
        [Header("Projectors")]
        /// <summary>
        /// The circle object.
        /// </summary>
        [SerializeField]
        private GameObject _circle;

        /// <summary>
        /// The center circle object.
        /// </summary>
        [SerializeField]
        private GameObject _centerDot;
        
        [Header("Materials")]
        /// <summary>
        /// The circle material.
        /// </summary>
        [SerializeField]
        private Material _circleMat;

        /// <summary>
        /// The center circle material.
        /// </summary>
        [SerializeField]
        private Material _centerDotMat;
        
        /// <summary>
        /// The transform of the game object that holds the the projectors.
        /// </summary>
        [SerializeField]
        private Transform _regionTransform;
        
        /// <summary>
        /// The circle projector.
        /// </summary>
        private DecalProjector _circleProjector;

        /// <summary>
        /// The center circle projector.
        /// </summary>
        private DecalProjector _centerDotProjector;

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
        /// Depth to project the object.
        /// </summary>
        [SerializeField]
        [Min(0)]
        private float _depth = 10;

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
        /// Generate the SRP decal projector.
        /// </summary>
        public void GenerateProjector()
        {
            _circleProjector = _circle.GetComponent<DecalProjector>();
            if(_circleProjector == null)
                _circleProjector = _circle.AddComponent<DecalProjector>();
            _circleProjector.material = _circleMat;

            _centerDotProjector = _centerDot.GetComponent<DecalProjector>();
            if(_centerDotProjector == null)
                _centerDotProjector = _centerDot.AddComponent<DecalProjector>();
            _centerDotProjector.material = _centerDotMat;
            Start();
        }
        
        /// <summary>
        /// Reassigns all materials and updates all projectors and their rendering order.
        /// </summary>
        private void Start()
        {
            if (_centerDotProjector == null || _circleProjector == null)
            {
                return;
            }
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

            Vector3 currentSize = _circleProjector.size;
            currentSize.x = _radius;
            currentSize.y = _radius;
            currentSize.z = _depth;
            _circleProjector.pivot = new Vector3(0, 0,_depth / 2);
            _circleProjector.size = currentSize;

            if (_circleProjector.material == null)
                return;

            _circleProjector.material.SetColor(ColorShaderID, _circleBaseColor);
            _circleProjector.material.SetColor(FillColorShaderID, _circleFillColor);
            _circleProjector.material.SetFloat(FillProgressShaderID, _fillProgress);
        }

        /// <summary>
        /// Updates the Center Dot projector properties.
        /// </summary>
        private void UpdateCenterDotProjector()
        {
            if (_centerDotProjector == null)
                return;
            
            Vector3 currentSize = _centerDotProjector.size;
            currentSize.x = _radius * 0.25f;
            currentSize.y = _radius * 0.25f;
            currentSize.z = _depth;
            _centerDotProjector.pivot = new Vector3(0, 0,_depth / 2);
            _centerDotProjector.size = currentSize;
            
            
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
#endif
    }
}