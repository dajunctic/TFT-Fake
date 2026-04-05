using UnityEngine;

namespace DTT.AreaOfEffectRegions
{
    /// <summary>
    /// An implementation of <see cref="LineRegionBase"/> with added textures.
    /// </summary>
    [ExecuteAlways]
    public class LineRegion : LineRegionBase
    {
        /// <summary>
        /// The body of the line.
        /// </summary>
        [SerializeField]
        private MeshRenderer _bodyRenderer;
        
        /// <summary>
        /// The end point, head, of the line.
        /// </summary>
        [SerializeField]
        private MeshRenderer _headRenderer;
        
        /// <summary>
        /// The body of the line.
        /// </summary>
        [SerializeField]
        private Transform _bodyTransform;
        
        /// <summary>
        /// The end point, head, of the line.
        /// </summary>
        [SerializeField]
        private Transform _headTransform;
        
        /// <summary>
        /// The shader ID of the _FillProgress property.
        /// </summary>
        private static readonly int ProgressShaderID = Shader.PropertyToID("_FillProgress");

        /// <summary>
        /// Updates the position of the head and the body according to the line data.
        /// </summary>
        private void Update()
        {
            if (_bodyTransform == null || _headTransform == null)
                return;
            
            // Sets body angle and scale.
            _bodyTransform.localEulerAngles = new Vector3(_bodyTransform.eulerAngles.x, Angle, _bodyTransform.eulerAngles.z);
            _bodyTransform.localScale = new Vector3(Width, _bodyTransform.localScale.y, Mathf.Max(Length - 2, 0));
            
            // Sets head angle, scale and position.
            _headTransform.localScale = new Vector3(Width, _headTransform.localScale.y, Mathf.Min((Length / 2),  1 ));
            _headTransform.position = _bodyTransform.position + _bodyTransform.forward * _bodyTransform.localScale.z;
            _headTransform.eulerAngles = _bodyTransform.eulerAngles;

            float bodyPart = _bodyTransform.localScale.z / Length;
            _bodyRenderer.sharedMaterial.SetFloat(ProgressShaderID, Mathf.InverseLerp(0, bodyPart, FillProgress));
            _headRenderer.sharedMaterial.SetFloat(ProgressShaderID, Mathf.InverseLerp(bodyPart, 1, FillProgress));
        }
    }
}