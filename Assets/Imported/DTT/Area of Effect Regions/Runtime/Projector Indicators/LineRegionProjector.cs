using System;
using System.IO;
using DTT.AreaOfEffectRegions.Shaders;
using DTT.Utils.Extensions;
using UnityEngine;
using UnityEngine.Serialization;

namespace DTT.AreaOfEffectRegions
{
    /// <summary>
    /// A line region that uses projectors for effects.
    /// </summary>
    [ExecuteAlways]
    public class LineRegionProjector : MonoBehaviour
    {
        [Header("Projectors")]
        /// <summary>
        /// The line head projector.
        /// </summary>
        [SerializeField]
        private Projector _headProjector;

        /// <summary>
        /// The line body projector.
        /// </summary>
        [SerializeField]
        private Projector _bodyProjector;

        /// <summary>
        /// The amount the line is filled.
        /// </summary>
        public float FillProgress
        {
            get => _fillProgress;
            set => _fillProgress = Mathf.Clamp01(value);
        }
        
        /// <summary>
        /// The origin of the fill color.
        /// </summary>
        public Origin FillProgressDirection
        {
            get => _fillOrigin;
            set => _fillOrigin = value;
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

        [Header("Properties")]
        /// <summary>
        /// The progress of filling the line.
        /// </summary>
        [SerializeField]
        [Range(0, 1)]
        private float _fillProgress;
        
        /// <summary>
        /// The origin of the fill color.
        /// </summary>
        [SerializeField]
        [Tooltip("The origin of the fill color.")]
        private Origin _fillOrigin;

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
        /// The angle of the line.
        /// </summary>
        public float Angle
        {
            get => _angle;
            set => _angle = Mathf.Repeat(value, 360);
        }

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
        [SerializeField]
        private Color _fillColor;

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
        /// The shader ID of the <c>_Origin</c> property.
        /// </summary>
        private static readonly int FillDirection = Shader.PropertyToID("_Origin");

        /// <summary>
        /// Reassigns all materials and updates all projectors.
        /// </summary>
        private void Start()
        {
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
            transform.localRotation = Quaternion.Euler(0, Angle, 0);
        }

        /// <summary>
        /// Updates the head projector properties.
        /// </summary>
        private void UpdateHeadProjector()
        {
            if (_headProjector == null)
                return;

            _headProjector.orthographicSize = _length;
            _headProjector.aspectRatio = _width;
            _headProjector.transform.localPosition = new Vector3(0, Y_POSITION, _length * ARROW_Z_DISPLACEMENT);

            if (_headProjector.material == null)
                return;

            _headProjector.material.SetColor(ColorShaderID, _color);
            _headProjector.material.SetColor(FillColorShaderID, _fillColor);
            _headProjector.material.SetFloat(FadeAmountShaderID, _fadeAmount * 2 - 0.5f);
            _headProjector.material.SetInt(FillDirection, _fillOrigin.ToInt());
            switch (_fillOrigin)
            {
                case (Origin.BOTTOM):
                    _headProjector.material.SetFloat(FillProgressShaderID, Mathf.Clamp01(-1f + _fillProgress * 2));
                    break;
                case Origin.TOP:
                    _headProjector.material.SetFloat(FillProgressShaderID, Mathf.Clamp01( 1 - _fillProgress * 2));
                    break;
                case Origin.LEFT:
                    _headProjector.material.SetFloat(FillProgressShaderID, Mathf.Clamp01( _fillProgress ));
                    break;
                case Origin.RIGHT :
                    _headProjector.material.SetFloat(FillProgressShaderID, Mathf.Clamp01( 1-_fillProgress ));
                    break;
                case Origin.CENTER:
                    _bodyProjector.material.SetInt(FillDirection, Origin.BOTTOM.ToInt());
                    _headProjector.material.SetFloat(FillProgressShaderID, Mathf.Clamp01( _fillProgress ));
                    break;
                default:
                    _headProjector.material.SetFloat(FillProgressShaderID, Mathf.Clamp01(-1f + _fillProgress * 2));
                    break;
            }
        }

        /// <summary>
        /// Updates the body projector properties.
        /// </summary>
        private void UpdateBodyProjector()
        {
            if (_bodyProjector == null)
                return;

            _bodyProjector.orthographicSize = _length;
            _bodyProjector.aspectRatio = _width;
            _bodyProjector.transform.localPosition = new Vector3(0, Y_POSITION, _length);

            if (_bodyProjector.material == null)
                return;

            _bodyProjector.material.SetColor(ColorShaderID, _color);
            _bodyProjector.material.SetColor(FillColorShaderID, _fillColor);
            _bodyProjector.material.SetFloat(FadeAmountShaderID, _fadeAmount * 2);
            _bodyProjector.material.SetInt(FillDirection, _fillOrigin.ToInt());
            switch (_fillOrigin)
            {
                case (Origin.BOTTOM):
                    _bodyProjector.material.SetFloat(FillProgressShaderID, Mathf.Clamp01( _fillProgress * 2));
                    break;
                case Origin.TOP:
                    _bodyProjector.material.SetFloat(FillProgressShaderID, Mathf.Clamp01( 2- _fillProgress * 2));
                    break;
                case Origin.LEFT:
                    _bodyProjector.material.SetFloat(FillProgressShaderID, Mathf.Clamp01( _fillProgress ));
                    break;
                case Origin.RIGHT :
                    _bodyProjector.material.SetFloat(FillProgressShaderID, Mathf.Clamp01( 1-_fillProgress ));
                    break;
                case Origin.CENTER:
                    _bodyProjector.material.SetInt(FillDirection, Origin.TOP.ToInt());
                    _bodyProjector.material.SetFloat(FillProgressShaderID, Mathf.Clamp01( 1-_fillProgress ));
                    break;
                default:
                    _bodyProjector.material.SetFloat(FillProgressShaderID, Mathf.Clamp01( _fillProgress * 2));
                    break;
            }
        }
    }
}