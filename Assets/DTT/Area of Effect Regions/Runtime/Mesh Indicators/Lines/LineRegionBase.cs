using System;
using UnityEngine;

namespace DTT.AreaOfEffectRegions
{
    /// <summary>
    /// A line region that can be used to display straight areas.
    /// </summary>
    public abstract class LineRegionBase : MonoBehaviour
    {
        /// <summary>
        /// The angle of the line.
        /// </summary>
        public float Angle
        {
            get => _angle;
            set => _angle = value;
        }
        
        /// <summary>
        /// The length of the line.
        /// </summary>
        public float Length
        {
            get => _length;
            set => _length = value;
        }

        /// <summary>
        /// The width of the line.
        /// </summary>
        public float Width
        {
            get => _width;
            set => _width = value;
        }

        /// <summary>
        /// The amount the line is filled.
        /// </summary>
        public float FillProgress
        {
            get => _fillProgress;
            set => _fillProgress = Mathf.Clamp01(value);
        }

        /// <summary>
        /// The centre end position of the line.
        /// </summary>
        public Vector3 EndPosition
        {
            get
            {
                Vector3 start = transform.position;
                float radians = (360 - (_angle - 90)) % 360 / 360 * Mathf.PI * 2 + -transform.eulerAngles.y * Mathf.Deg2Rad;
                return start + new Vector3(Mathf.Cos(radians) * _length, 0, Mathf.Sin(radians) * _length);
            }
        }

        /// <summary>
        /// The width offset used for determining where from the centre the corner points are.
        /// </summary>
        public Vector3 WidthOffset => Vector3.Cross(EndPosition - transform.position, Vector3.up).normalized * _width * 0.5f;

        /// <summary>
        /// The position of the left hand side corner in the beginning.
        /// </summary>
        public Vector3 LeftHandSideStart => transform.position + WidthOffset;
        
        /// <summary>
        /// The position of the right hand side corner in the beginning.
        /// </summary>
        public Vector3 RightHandSideStart => transform.position - WidthOffset;
        
        /// <summary>
        /// The position of the left hand side corner at the end.
        /// </summary>
        public Vector3 LeftHandSideEnd => EndPosition + WidthOffset;
        
        /// <summary>
        /// The position of the right hand side corner at the end.
        /// </summary>
        public Vector3 RightHandSideEnd => EndPosition - WidthOffset;
        
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
        [Range(0, 5)]
        private float _length;

        /// <summary>
        /// The width of the line.
        /// </summary>
        [SerializeField]
        [Range(0, 1)]
        private float _width;

        /// <summary>
        /// The progress of filling the line.
        /// </summary>
        [SerializeField]
        [Range(0, 1)]
        private float _fillProgress;

        /// <summary>
        /// Draws the lines representing the line region in gizmos.
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(LeftHandSideStart, LeftHandSideEnd);
            Gizmos.DrawLine(RightHandSideStart, RightHandSideEnd);
        }
    }
}