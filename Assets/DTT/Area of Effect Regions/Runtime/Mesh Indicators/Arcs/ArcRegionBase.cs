using System;
using UnityEngine;

namespace DTT.AreaOfEffectRegions
{
    /// <summary>
    /// Can be used as base class for creating regions that are arc/cone based.
    /// </summary>
    public abstract class ArcRegionBase : MonoBehaviour
    {
        /// <summary>
        /// The arc in euler angles. This is the amount that it's opened, 0 being closed and 360 being fully open.
        /// </summary>
        public float Arc
        {
            get => _arc;
            set => _arc = value;
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
        /// The length of the arc.
        /// </summary>
        public float Radius
        { 
            get => _radius;
            set => _radius = value;
        }
        
        /// <summary>
        /// The angle in euler for the left side of the arc.
        /// </summary>
        public float LeftAngle => _angle + transform.eulerAngles.y - _arc / 2;
        
        /// <summary>
        /// The angle in euler for the right side of the arc.
        /// </summary>
        public float RightAngle => _angle + transform.eulerAngles.y + _arc / 2;

        /// <summary>
        /// The end position of the left side of the arc. 
        /// </summary>
        public Vector3 LeftEndPosition => new Vector3(
            Mathf.Sin((LeftAngle) * Mathf.Deg2Rad), 
            0, 
            Mathf.Cos((LeftAngle) * Mathf.Deg2Rad)
        ) * _radius;

        /// <summary>
        /// The end position of the right side of the arc. 
        /// </summary>
        public Vector3 RightEndPosition => new Vector3(
            Mathf.Sin((RightAngle) * Mathf.Deg2Rad), 
            0, 
            Mathf.Cos((RightAngle) * Mathf.Deg2Rad)
        ) * _radius;

        /// <summary>
        /// The arc in euler angles. This is the amount that it's opened, 0 being closed and 360 being fully open.
        /// </summary>
        [SerializeField]
        [Range(0, 360)]
        private float _arc = 30.0f;

        /// <summary>
        /// The angle offset for the cone in euler angles.
        /// </summary>
        [SerializeField]
        private float _angle;

        /// <summary>
        /// The length of the arc.
        /// </summary>
        [SerializeField]
        private float _radius = 5.0f;

        /// <summary>
        /// Gizmo representation of the arc.
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            GizmosExtensions.DrawWireArc(transform.position, _radius, _angle + transform.eulerAngles.y, _arc);
        }
    }
}