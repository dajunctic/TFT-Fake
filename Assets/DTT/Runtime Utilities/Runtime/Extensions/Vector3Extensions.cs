using UnityEngine;

namespace DTT.Utils.Extensions
{
    /// <summary>
    /// Provides extension methods for Vector3 values.
    /// </summary>
    public static class Vector3Extensions
    {
        /// <summary>
        /// Flattens a vector by setting its axis components to 0.
        /// </summary>
        /// <param name="vector">The vector to flatten.</param>
        /// <param name="axis">The axis to flatten (Uses enum flags).</param>
        /// <returns>The flattened vector.</returns>
        public static Vector3 Flatten(this Vector3 vector, Vector3Axis axis)
        {
            if (axis.HasFlag(Vector3Axis.X))
                vector.x = 0.0f;

            if (axis.HasFlag(Vector3Axis.Y))
                vector.y = 0.0f;

            if (axis.HasFlag(Vector3Axis.Z))
                vector.z = 0.0f;

            return vector;
        }
    }
}