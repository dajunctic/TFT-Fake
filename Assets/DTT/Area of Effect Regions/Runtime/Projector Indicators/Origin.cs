using UnityEngine;
namespace DTT.AreaOfEffectRegions.Shaders
{
    /// <summary>
    /// Origin of the fill color.
    /// </summary>
    public enum Origin
    {
        /// <summary>
        /// Color start from the bottom/center.
        /// </summary>
        [InspectorName("Bottom")]
        BOTTOM = 0,
        
        /// <summary>
        /// Color start from the top/edge.
        /// </summary>
        [InspectorName("Top")]
        TOP = 1,
        
        /// <summary>
        /// Color start from the left.
        /// </summary>
        [InspectorName("Left")]
        LEFT = 2,
        
        /// <summary>
        /// Color start from the right.
        /// </summary>
        [InspectorName("Right")]
        RIGHT = 3,
        
        /// <summary>
        /// Color start from the center.
        /// </summary>
        [InspectorName("Center")]
        CENTER = 4,
    }
}