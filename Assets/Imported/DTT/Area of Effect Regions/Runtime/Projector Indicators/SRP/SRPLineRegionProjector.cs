using UnityEngine;
#if UNIVERSAL_PACKAGE
using UnityEngine.Rendering.Universal;
#endif

namespace DTT.AreaOfEffectRegions
{
    /// <summary>
    /// A line region that uses projectors for effects.
    /// </summary>
    [ExecuteAlways]
    public class SRPLineRegionProjector : MonoBehaviour
    {
#if UNIVERSAL_PACKAGE
        [Header("Projectors")]
        /// <summary>
        /// The line head object.
        /// </summary>
        [SerializeField]
        private GameObject _head;

        /// <summary>
        /// The line body object.
        /// </summary>
        [SerializeField] private GameObject _body;
        
        [Header("Materials")]
        /// <summary>
        /// The line head material.
        /// </summary>
        [SerializeField]
        private Material _headMat;

        /// <summary>
        /// The line body material.
        /// </summary>
        [SerializeField] 
        private Material _bodyMat;
        
        /// <summary>
        /// The line head projector.
        /// </summary>
        private DecalProjector _headProjector;

        /// <summary>
        /// The line body projector.
        /// </summary>
        private DecalProjector _bodyProjector;

        /// <summary>
        /// The amount the line is filled.
        /// </summary>
        public float FillProgress
        {
            get => _fillProgress;
            set => _fillProgress = Mathf.Clamp01(value);
        }

        /// <summary>
        /// The amount the line is faded.
        /// </summary>
        public float FadeAmount
        {
            get => _fadeAmount;
            set => _fadeAmount = Mathf.Clamp01(value);
        }

        /// <summary>
        /// The angle of the line.
        /// </summary>
        public float Angle
        {
            get => _angle;
            set => _angle = Mathf.Clamp(value, 0f, 360f);
        }

        /// <summary>
        /// The length of the line.
        /// </summary>
        public float Length
        {
            get => _length;
            set => _length = Mathf.Max(value, 0);
        }

        /// <summary>
        /// The width of the line.
        /// </summary>
        public float Width
        {
            get => _width;
            set => _width = Mathf.Max(value, 0);
        }

        public float Depth
        {
            get => _depth;
            set => _depth = value;
        }

        /// <summary>
        /// The progress of filling the line.
        /// </summary>
        [Header("Properties")]
        [SerializeField]
        [Range(0, 1)]
        private float _fillProgress;

        /// <summary>
        /// The fade amount of the line.
        /// </summary>
        [SerializeField] 
        [Range(0, 1)] 
        private float _fadeAmount;

        /// <summary>
        /// The angle of the line.
        /// </summary>
        [SerializeField] 
        [Range(0, 360)] 
        private float _angle;

        /// <summary>
        /// The length of the line.
        /// </summary>
        [SerializeField] 
        [Min(0)] 
        private float _length = 1f;

        /// <summary>
        /// The width of the line.
        /// </summary>
        [SerializeField] 
        [Min(0)] 
        private float _width = 1f;
        
        /// <summary>
        /// Depth to project the object.
        /// </summary>
        [SerializeField]
        [Min(0)]
        private float _depth = 10;

        /// <summary>
        /// The line head projector's Z offset, this lines up the head and body of the line. 
        /// </summary>
        private const float ARROW_Z_DISPLACEMENT = 2.9835f;

        /// <summary>
        /// The projector's 'y' position.
        /// </summary>
        private const float Y_POSITION = 0.15f;

        [Header("Colors")]
        /// <summary>
        /// The color for the line.
        /// </summary>
        [SerializeField]
        private Color _color;

        /// <summary>
        /// The fill color for the line.
        /// </summary>
        [SerializeField] private Color _fillColor;

        /// <summary>
        /// The color for the line.
        /// </summary>
        public Color Color
        {
            get => _color;
            set => _color = value;
        }

        /// <summary>
        /// The fill color for the line.
        /// </summary>
        public Color FillColor
        {
            get => _fillColor;
            set => _fillColor = value;
        }

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
        /// The shader ID of the <c>_FadeAmount</c> property.
        /// </summary>
        private static readonly int FadeAmountShaderID = Shader.PropertyToID("_FadeAmount");

        /// <summary>
        /// Generate the SRP decal projector.
        /// </summary>
        public void GenerateProjector()
        {
            _headProjector = _head.GetComponent<DecalProjector>();
            if(_headProjector == null)
                _headProjector = _head.AddComponent<DecalProjector>();
            _headProjector.material = _headMat;

            _bodyProjector = _body.GetComponent<DecalProjector>();
            if(_bodyProjector == null)
                _bodyProjector = _body.AddComponent<DecalProjector>();
            _bodyProjector.material = _bodyMat;
            Start();
        }
        
        /// <summary>
        /// Reassigns all materials and updates all projectors.
        /// </summary>
        private void Start()
        {
            if (_headProjector == null || _bodyProjector == null)
                return;
            ReassignMaterials();
            UpdateProjectors();
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
            if (_headProjector != null)
                _headProjector.material = new Material(_headProjector.material);

            if (_bodyProjector != null)
                _bodyProjector.material = new Material(_bodyProjector.material);
        }

        /// <summary>
        /// Updates all projectors of this region.
        /// </summary>
        public void UpdateProjectors()
        {
            UpdateHeadProjector();
            UpdateBodyProjector();
            transform.localRotation = Quaternion.Euler(0, _angle, 0);
        }

        /// <summary>
        /// Updates the head projector properties.
        /// </summary>
        private void UpdateHeadProjector()
        {
            if (_headProjector == null)
                return;

            Vector3 currentSize = _headProjector.size;
            currentSize.x = _length + 1 + (_width - 1);
            currentSize.y = _length + 1;
            currentSize.z = _depth;
            _headProjector.pivot = new Vector3(0, 0,_depth / 2);
            _headProjector.size = currentSize;
            _headProjector.transform.localPosition =
                new Vector3(0, Y_POSITION, (_length + 1) * 0.5f * ARROW_Z_DISPLACEMENT);

            if (_headProjector.material == null)
                return;

            _headProjector.material.SetColor(ColorShaderID, _color);
            _headProjector.material.SetColor(FillColorShaderID, _fillColor);
            _headProjector.material.SetFloat(FillProgressShaderID, Mathf.Clamp01(-1f + _fillProgress * 2));
            _headProjector.material.SetFloat(FadeAmountShaderID, _fadeAmount * 2 - 0.5f);
        }

        /// <summary>
        /// Updates the body projector properties.
        /// </summary>
        private void UpdateBodyProjector()
        {
            if (_bodyProjector == null)
                return;

            Vector3 currentSize = _bodyProjector.size;
            currentSize.x = _length + 1 + (_width - 1);
            currentSize.y = _length + 1;
            currentSize.z = _depth;
            _bodyProjector.pivot = new Vector3(0, 0,_depth / 2);
            _bodyProjector.size = currentSize;
            _bodyProjector.transform.localPosition = new Vector3(0, Y_POSITION, (_length + 1) * 0.5f);

            if (_bodyProjector.material == null)
                return;

            _bodyProjector.material.SetColor(ColorShaderID, _color);
            _bodyProjector.material.SetColor(FillColorShaderID, _fillColor);
            _bodyProjector.material.SetFloat(FillProgressShaderID, Mathf.Clamp01(_fillProgress * 2));
            _bodyProjector.material.SetFloat(FadeAmountShaderID, _fadeAmount * 2);
        }
#endif
    }
}