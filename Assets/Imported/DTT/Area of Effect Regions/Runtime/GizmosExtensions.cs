using UnityEngine;

namespace DTT.AreaOfEffectRegions
{
    /// <summary>
    /// Defines additional methods for the <see cref="Gizmos"/> library.
    /// </summary>
    internal static class GizmosExtensions
    {
        /// <summary>
        /// Draws a wire arc on the world y axis.
        /// </summary>
        /// <param name="position">Position of the arc.</param>
        /// <param name="radius">The half-length of the circle.</param>
        /// <param name="angle">The angle the arc should rotate to in degrees.</param>
        /// <param name="arc">The angle of the arc in degrees.</param>
        public static void DrawWireArc(Vector3 position, float radius, float angle, float arc)
        {
            angle = (angle - 90) % 360;
            angle = 360 - angle;
            float angleA = angle - arc / 2;
            float angleB = angle + arc / 2;
            Vector3 endA = position + PolarToCartesian(angleA, radius);
            Vector3 endB = position + PolarToCartesian(angleB, radius);
            
            Gizmos.DrawLine(position, endA);
            Gizmos.DrawLine(position, endB);

            Vector3 prevPoint = endA;
            int resolution = Mathf.RoundToInt(arc);
            for (int i = 0; i < resolution; i++)
            {
                float newAngle = Mathf.Lerp(angleA, angleB, i / (float)resolution);
                Vector3 newPoint = position + PolarToCartesian(newAngle, radius);
                Gizmos.DrawLine(prevPoint, newPoint);
                prevPoint = newPoint;
            }
            Gizmos.DrawLine(prevPoint, endB);
        }
    
        /// <summary>
        /// Draws a circle on the world y axis.
        /// </summary>
        /// <param name="position">The position of the circle.</param>
        /// <param name="radius">The half-length of the circle.</param>
        public static void DrawCircle(Vector3 position, float radius)
        {
            const int RESOLUTION = 360;
            for (int i = 1; i < RESOLUTION; i++)
            {
                Gizmos.DrawLine(position + PolarToCartesian(i - 1, radius), position + PolarToCartesian(i, radius));
            }
            Gizmos.DrawLine(position + PolarToCartesian(RESOLUTION - 1, radius), position + PolarToCartesian(RESOLUTION, radius));
        }
        
        /// <summary>
        /// Converts polar coordinates to the Cartesian coordinate system.
        /// </summary>
        /// <param name="angle">The angle of the coordinate.</param>
        /// <param name="length">The distance from the origin of the coordinate.</param>
        /// <returns>The Cartesian world position.</returns>
        private static Vector3 PolarToCartesian(float angle, float length)
        {
            return new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0, Mathf.Sin(angle * Mathf.Deg2Rad)) * length;
        }
    }
}