using UnityEngine;
using UnityEngine.Serialization;

namespace DTT.AreaOfEffectRegions
{
    /// <summary>
    /// An example implementation of <see cref="ArcRegionBase"/> with some custom textures.
    /// </summary>
    [ExecuteAlways]
    public class ArcRegion : ArcRegionBase
    {
        /// <summary>
        /// The amount the line is filled.
        /// </summary>
        public float FillProgress
        {
            get => _fillProgress;
            set => _fillProgress = Mathf.Clamp01(value);
        }
        
        /// <summary>
        /// The dot in the centre
        /// </summary>
        [SerializeField]
        private Transform _centreDot;
        
        /// <summary>
        /// The object on the left side of the arc.
        /// </summary>
        [SerializeField]
        private Transform _leftSide;
        
        /// <summary>
        /// The object on the right side of the arc.
        /// </summary>
        [SerializeField]
        private Transform _rightSide;
        
        /// <summary>
        /// The centre fill of the arc.
        /// </summary>
        [SerializeField]
        private Transform _centre;

        /// <summary>
        /// The progress of the fill.
        /// </summary>
        [SerializeField]
        [Range(0, 1)]
        [FormerlySerializedAs("_progress")]
        private float _fillProgress;

        /// <summary>
        /// All the centre areas that should be masked with the arc.
        /// These should make use of the ArcShader shader.
        /// </summary>
        [SerializeField]
        private MeshRenderer[] _maskedCentres;

        /// <summary>
        /// The ID of the <c>_Arc</c> property.
        /// </summary>
        private static readonly int ArcShaderID = Shader.PropertyToID("_Arc");
        
        /// <summary>
        /// The ID of the <c>_Angle</c> property.
        /// </summary>
        private static readonly int AngleShaderID = Shader.PropertyToID("_Angle");
        
        /// <summary>
        /// The ID of the <c>_Progress</c> property.
        /// </summary>
        private static readonly int ProgressShaderID = Shader.PropertyToID("_Progress");

        /// <summary>
        /// Updates the transforms of the textures that are placed on the arc.
        /// </summary>
        private void Update()
        {
            if (_centreDot == null || _leftSide == null || _rightSide == null || _centre == null)
                return;
            
            // Centre dot transform configure.
            _centreDot.localPosition = Vector3.up * 0.01f;
            _centreDot.localScale = Radius * Vector3.one * 0.3f;
            
            // Centre transform configure.
            _centre.localPosition = Vector3.zero;
            _centre.localScale = Radius * 2 * Vector3.one;
            
            // Left side transform configure.
            Vector3 original = _leftSide.localEulerAngles;
            _leftSide.localEulerAngles = new Vector3(original.x, LeftAngle - transform.eulerAngles.y,  original.z);
            _leftSide.localScale = Radius * (Vector3.one * 1.2f);

            // Right side transform configure.
            original = _rightSide.localEulerAngles;
            _rightSide.localEulerAngles = new Vector3(original.x, RightAngle - transform.eulerAngles.y,  original.z);
            _rightSide.localScale = Radius * (Vector3.one * 1.2f);

            // Apply normalized arc and angle to mask shader.
            for (int i = 0; i < _maskedCentres.Length; i++)
            {
                _maskedCentres[i].sharedMaterial.SetFloat(ArcShaderID, 1 - Arc / 360);
                _maskedCentres[i].sharedMaterial.SetFloat(AngleShaderID , Mathf.Repeat((Angle - 90) % 360, 360) / 360);
                _maskedCentres[i].sharedMaterial.SetFloat(ProgressShaderID , Mathf.Clamp01(_fillProgress));
            }
        }
    }
}