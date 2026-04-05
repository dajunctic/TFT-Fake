using UnityEditor;
using UnityEngine;

namespace DTT.EditorUtilities
{
    /// <summary>
    /// Provides additional methods for drawing using the Handles api.
    /// </summary>
    public static class HandlesUtility
    {
        /// <summary>
        /// Draws a line with arrow caps.
        /// </summary>
        /// <param name="start">Starting point.</param>
        /// <param name="end">End point.</param>
        /// <param name="capLength">The length of the caps.</param>
        /// <param name="enableStartArrowCap">Whether to use the cap for starting point.</param>
        /// <param name="enableEndArrowCap">Whether to use the cap for ending point.</param>
        public static void DrawArrowLine(Vector3 start, Vector3 end, float capLength, bool enableStartArrowCap = true, bool enableEndArrowCap = true)
        {
            Vector3 top;
            Vector3 bottom;
            if (start.y > end.y)
                (top, bottom) = (start, end);
            else
                (bottom, top) = (start, end);

            Vector3 topRight = new Vector3(1, 1, 0);
            
            Handles.DrawLine(top, bottom);
            if (enableStartArrowCap)
            {
                Handles.DrawLine(top, top - topRight * capLength);
                Handles.DrawLine(top, top - (Vector3.left + Vector3.up) * capLength);
            }

            if (enableEndArrowCap)
            {
                Handles.DrawLine(bottom, bottom - (Vector3.right + Vector3.down) * capLength);
                Handles.DrawLine(bottom, bottom + topRight * capLength);
            }
        }
    }
}


