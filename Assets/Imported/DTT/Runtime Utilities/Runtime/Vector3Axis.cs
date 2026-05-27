using System;

namespace DTT.Utils
{
    /// <summary>
    /// The axis of a Vector3 in Unity. Can be combined using bit shifting.
    /// </summary>
    [Flags]
    public enum Vector3Axis 
    {
        /// <summary>
        /// The x axis.
        /// </summary>
        X = 1 << 0, 
        
        /// <summary>
        /// The y axis.
        /// </summary>
        Y = 1 << 1,
        
        /// <summary>
        /// The z axis.
        /// </summary>
        Z = 1 << 2
    }
}

