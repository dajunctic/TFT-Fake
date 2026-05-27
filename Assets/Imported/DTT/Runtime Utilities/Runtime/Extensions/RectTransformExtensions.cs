using System;
using System.Collections.Generic;
using UnityEngine;

namespace DTT.Utils.Extensions
{
    /// <summary>
    /// Provides extension methods for rect transform components.
    /// </summary>
    public static class RectTransformExtensions
    {
        /// <summary>
        /// Represents values for a rect anchor setting.
        /// </summary>
        private struct RectSetting
        {
            /// <summary>
            /// The anchor's max values.
            /// </summary>
            public Vector2 anchorMax;

            /// <summary>
            /// The anchor's min values.
            /// </summary>
            public Vector2 anchorMin;

            /// <summary>
            /// The pivot values.
            /// </summary>
            public Vector2 pivot;

            /// <summary>
            /// Initializes the rectangle setting.
            /// </summary>
            /// <param name="xMin">The minimum x value.</param>
            /// <param name="xMax">The maximum x value.</param>
            /// <param name="yMin">The minimum y value.</param>
            /// <param name="yMax">The maximum y value.</param>
            /// <param name="xPivot">The pivot value on the x axis.</param>
            /// <param name="yPivot">The pivot value on the y axis.</param>
            public RectSetting(float xMin, float xMax, float yMin, float yMax, float xPivot, float yPivot)
            {
                anchorMax = new Vector2(xMax, yMax);
                anchorMin = new Vector2(xMin, yMin);
                pivot = new Vector2(xPivot, yPivot);
            }
        }
        
        /// <summary>
        /// Holds the preset values used for each anchor setting.
        /// </summary>
        private static readonly Dictionary<RectAnchor, RectSetting> _anchorPresets = new Dictionary<RectAnchor, RectSetting>
        {
            { RectAnchor.TOP_LEFT, new RectSetting( 0f, 0f, 1f,1f, 0f,1f )},
            { RectAnchor.TOP_CENTER, new RectSetting( 0.5f, 0.5f, 1f,1f,0.5f,1f )},
            { RectAnchor.TOP_RIGHT, new RectSetting( 1f,1f,1f,1f, 1f,1f )},
            { RectAnchor.MIDDLE_LEFT, new RectSetting( 0f,0f, 0.5f, 0.5f,0f,0.5f )},
            { RectAnchor.MIDDLE_CENTER, new RectSetting( 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f )},
            { RectAnchor.MIDDLE_RIGHT, new RectSetting( 1f,1f,0.5f,0.5f, 1f, 0.5f )},
            { RectAnchor.BOTTOM_LEFT, new RectSetting( 0f,0f,0f,0f, 0f, 0f )},
            { RectAnchor.BOTTOM_CENTER, new RectSetting( 0.5f, 0.5f, 0f, 0f, 0.5f, 0f )},
            { RectAnchor.BOTTOM_RIGHT, new RectSetting( 1f,1f,0f,0f, 1f, 0f )},
            { RectAnchor.STRETCH_TOP, new RectSetting( 0f, 1f, 1f,1f,0.5f,1f )},
            { RectAnchor.STRETCH_MIDDLE, new RectSetting( 0f,1f,0.5f,0.5f, 0.5f, 0.5f )},
            { RectAnchor.STRETCH_BOTTOM, new RectSetting( 0f,1f,0f,0f, 0.5f, 0f )},
            { RectAnchor.STRETCH_LEFT, new RectSetting( 0f,0f,0f,1f, 0f,0.5f )},
            { RectAnchor.STRETCH_CENTER, new RectSetting( 0.5f, 0.5f, 0f, 1f, 0.5f, 0.5f )},
            { RectAnchor.STRETCH_RIGHT, new RectSetting( 1f,1f, 0f, 1f, 1f, 0.5f )},
            { RectAnchor.STRETCH_FULL, new RectSetting( 0f, 1f, 0f, 1f, 0.5f, 0.5f )},
        };

        /// <summary>
        /// Sets the anchor values.
        /// </summary>
        /// <param name="transform">The rectangle transform.</param>
        /// <param name="xMin">The minimum x value.</param>
        /// <param name="xMax">The maximum x value.</param>
        /// <param name="yMin">The minimum y value.</param>
        /// <param name="yMax">The maximum y value.</param>
        public static void SetAnchor(this RectTransform transform, float xMin, float xMax, float yMin, float yMax)
        {
            if (transform == null)
                throw new ArgumentNullException(nameof(transform));
            
            transform.anchorMin = new Vector2(xMin, yMin);
            transform.anchorMax = new Vector2(xMax, yMax);
        }

        /// <summary>
        /// Sets the anchor to a given setting.
        /// </summary>
        /// <param name="transform">The rectangle transform.</param>
        /// <param name="anchor">The anchor setting to use.</param>
        /// <param name="setPivot">Whether the pivot should also be set based on the new setting.</param>
        /// <param name="setPosition">Whether to set the position after the setting has been applied.</param>
        public static void SetAnchor(
            this RectTransform transform, 
            RectAnchor anchor, 
            bool setPivot = false, 
            bool setPosition = false)
        {
            if (transform == null)
                throw new ArgumentNullException(nameof(transform));
            
            RectSetting setting = _anchorPresets[anchor];
            SetAnchor(transform, setting.anchorMin.x, setting.anchorMax.x, setting.anchorMin.y,setting.anchorMax.y);

            if (setPivot)
                transform.pivot = setting.pivot;

            if (setPosition)
                transform.anchoredPosition = Vector2.zero;
        }
        
        /// <summary>
        /// Returns the world rectangle of a rectangle transform.
        /// </summary>
        /// <param name="transform">The rectangle transform.</param>
        /// <returns>The world rectangle.</returns>
        public static Rect GetWorldRect(this RectTransform transform)
        {
            if (transform == null)
                throw new ArgumentNullException(nameof(transform));
            
            Vector3[] corners = new Vector3[4];
            transform.GetWorldCorners(corners);
            
            Vector3 bottomLeft = corners[0];
     
            Vector2 size = new Vector2(
                transform.lossyScale.x * transform.rect.size.x,
                transform.lossyScale.y * transform.rect.size.y);

            return new Rect(bottomLeft, size);
        }
        
        /// <summary>
        /// Returns the rectangle transform. Will return null if a normal transform is used.
        /// </summary>
        /// <param name="component">The component of which to get the rectangle transform.</param>
        /// <returns>The rectangle transform instance.</returns>
        public static RectTransform GetRectTransform(this Component component)
        {
            if (component == null)
                throw new ArgumentNullException(nameof(component));
            
            return component.transform as RectTransform;
        }

        /// <summary>
        /// Returns the rectangle transform. Will return null if a normal transform is used.
        /// </summary>
        /// <param name="gameObject">The game object of which to get the rectangle transform.</param>
        /// <returns>The rectangle transform instance.</returns>
        public static RectTransform GetRectTransform(this GameObject gameObject)
        {
            if (gameObject == null)
                throw new ArgumentNullException(nameof(gameObject));
            
            return gameObject.transform as RectTransform;
        }
    }
}
