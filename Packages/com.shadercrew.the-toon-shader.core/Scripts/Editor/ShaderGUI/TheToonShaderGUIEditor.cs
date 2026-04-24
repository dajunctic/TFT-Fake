//#if USING_URP
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.AnimatedValues;

using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Rendering;
using static ShaderCrew.TheToonShader.ShaderUtils;

#if USING_URP
using UnityEditor.Rendering.Universal.ShaderGUI;
using static UnityEditor.Rendering.Universal.ShaderGUI.SimpleLitGUI;
#endif

#if USING_SEE_THROUGH_SHADER
    using ShaderCrew.SeeThroughShader;
using UnityEditor.Rendering;
using System.Linq;
#endif
#if USING_URP
using UnityEngine.Rendering.Universal;
#endif


namespace ShaderCrew.TheToonShader
{
    public class TheToonShaderGUIEditor : ShaderGUI
    {

        public static string[] stylingOptions = new string[] {"Hatching","Halftones", };
        //public static string[] stylingOptions = new string[] {"Hatching","Halftones", "Texture",};


        #region Enums

        public enum SurfaceType
        {
            Opaque,
            Transparent
        }
        public enum BlendMode
        {
            Alpha,   // Old school alpha-blending mode, fresnel does not affect amount of transparency
            Premultiply, // Physically plausible transparency mode, implemented as alpha pre-multiply
            Additive,
            Multiply
        }
        public enum RenderFace
        {
            Front = 2,
            Back = 1,
            Both = 0
        }


        public enum GeneralShadingMode
        {
            SolidColor = 0,
            LightBased = 1
        }       
        
        public enum LightFunction
        {
            BlinnPhong = 0,
            PBR = 1,
            //Mixed = 2,
        }     
        
        public enum GeneralShadingFunction
        {
            Cells = 0,
            Gradient = 1
        }
        
        public enum CastShadowColorMode
        {
            CoreShadowColor = 0,
            FormShadowColor = 1
        }


        public enum RimAffectedRegion
        {
            OnlyLights = 0,
            OnlyShadows = 1,
            Both = 2
        }

        public enum Styling
        {
            Hatching = 0,
            HalftonePatterns = 1,
            TextureBased = 2,
        }

        public enum HatchingShadingMode
        {
            LineSpacing = 0,
            CrossHatching = 1
        }    


        public enum HatchingDirection
        {
            Random = 0,
            Cycling = 1,
            //Manual = 2
        }

        public enum RandomMode
        {
            SingleLine = 0,
            AllLines = 1,
            Mixed = 2
        }
        public enum RandomModeHalftones
        {
            //WhiteNoise = 0,
            //PerlinNoise = 1,
            SingleHalftone = 0,
            AllHalftones = 1,
            Mixed = 2
        }


        public enum DrawSpace
        {
            ObjectUVSpace = 0,
            ScreenSpace = 1,
            //Triplanar = 2 
        }
        public enum CoordinateSystem
        {
            Cartesian = 0,
            Polar = 1,
        }
        public enum SizeControl
        {
            Manual = 0,
            LightBased = 1,
        }

        public enum PolarCenterMode
        {
            ScreenSpace = 0,
            WorldSpace = 1,
        }

        public enum BlendingMode
        {
            Lerp = 0,
            Additive = 1,
            Multiply = 2,
            Subtract = 3,
            //Lerp = 4,
        }

        public enum StylePosition
        {
            Top = 0,
            Mid = 1,
            Bottom = 2
        }

        enum StyleAreaMode
        {
            Shading,
            CastShadows,
            Specular,
            Rim
        }

        enum GradientMode
        {
            Combined,
            Separated,
        }

        enum TextureQuality
        {
            High,
            Mid,
            Low,
            UltraLow
        }

        enum StylingSyncMode
        {
            None,
            Shading,
            CastShadows,
            Specular,
            Rim
        }

        public enum CellMethod
        {
            //LightIntensityOnly = 0,
            //IntensityAndColor = 1,
            //IntensityColorAndMainTex = 2
            LightOnly = 0,
            LightAndMaintex = 1,
            Luminance = 2,
        }

        public enum RoundingMethod
        {
            Ceil = 0,
            Floor = 1,
        }

        #endregion


        public class Styles
        {
            // Catergories
            public static readonly GUIContent SurfaceOptions =
                new GUIContent("Surface Options", "Controls how Universal RP renders the Material on a screen.");

            public static readonly GUIContent SurfaceInputs = new GUIContent("Surface Inputs",
                "These settings describe the look and feel of the surface itself.");

            public static readonly GUIContent AdvancedLabel = new GUIContent("Advanced",
                "These settings affect behind-the-scenes rendering and underlying calculations.");

            public static readonly GUIContent surfaceType = new GUIContent("Surface Type",
                "Select a surface type for your texture. Choose between Opaque or Transparent.");

            public static readonly GUIContent blendingMode = new GUIContent("Blending Mode",
                "Controls how the color of the Transparent surface blends with the Material color in the background.");

            public static readonly GUIContent cullingText = new GUIContent("Render Face",
                "Specifies which faces to cull from your geometry. Front culls front faces. Back culls backfaces. None means that both sides are rendered.");

            public static readonly GUIContent alphaClipText = new GUIContent("Alpha Clipping",
                "Makes your Material act like a Cutout shader. Use this to create a transparent effect with hard edges between opaque and transparent areas.");

            public static readonly GUIContent alphaClipThresholdText = new GUIContent("Threshold",
                "Sets where the Alpha Clipping starts. The higher the value is, the brighter the  effect is when clipping starts.");

            public static readonly GUIContent receiveShadowText = new GUIContent("Receive Shadows",
                "When enabled, other GameObjects can cast shadows onto this GameObject.");

            public static readonly GUIContent baseMap = new GUIContent("Base Map",
                "Specifies the base Material and/or Color of the surface. If you’ve selected Transparent or Alpha Clipping under Surface Options, your Material uses the Texture’s alpha channel or color.");

            public static readonly GUIContent emissionMap = new GUIContent("Emission Map",
                "Sets a Texture map to use for emission. You can also select a color with the color picker. Colors are multiplied over the Texture.");

            public static readonly GUIContent normalMapText =
                new GUIContent("Normal Map", "Assigns a tangent-space normal map.");

            public static readonly GUIContent normalMapScaleText =
            new GUIContent("Normal Strength", "Scales the intensity of the normal map effect. 1 = normal strength. Lower = softer, higher = exaggerated.");


            public static readonly GUIContent bumpScaleNotSupported =
                new GUIContent("Bump scale is not supported on mobile platforms");

            public static readonly GUIContent fixNormalNow = new GUIContent("Fix now",
                "Converts the assigned texture to be a normal map format.");

            //public static readonly GUIContent queueSlider = new GUIContent("Priority",
            //    "Determines the chronological rendering order for a Material. High values are rendered first.");


            public static GUIContent occlusionText = new GUIContent("Occlusion Map", "Sets an occlusion map to simulate shadowing from ambient lighting.");


            //SimpleLit:
            public static GUIContent specularMapText =
                new GUIContent("Specular Map", "Sets and configures a Specular map and color for your Material.");

            public static GUIContent smoothnessText = new GUIContent("Smoothness",
                "Controls the spread of highlights and reflections on the surface.");

            public static GUIContent smoothnessMapChannelText =
                new GUIContent("Source",
                    "Specifies where to sample a smoothness map from. By default, uses the alpha channel for your map.");

            public static GUIContent highlightsText = new GUIContent("Specular Highlights",
                "When enabled, the Material reflects the shine from direct lighting.");
            


            public static readonly GUIContent GeneralShadingLightPartitioning = new GUIContent("Light Partitioning", "TODO");
            public static readonly GUIContent GeneralShadingLightGradient= new GUIContent("Light Gradient", "TODO");
            public static readonly GUIContent GeneralShadingShadowGradient= new GUIContent("Shadow Gradient", "TODO");
            public static readonly GUIContent GeneralShadingLightAndShadowGradient = new GUIContent("Light and Shadow Gradient", "TODO");
            public static readonly GUIContent GeneralShadingLighting = new GUIContent("Lighting", "TODO");
            public static readonly GUIContent GeneralShadingShading = new GUIContent("Shading", "TODO");
            public static readonly GUIContent GeneralShadingCastShadows = new GUIContent("Cast Shadows", "TODO");
            public static readonly GUIContent GeneralShadingSpecular= new GUIContent("Specular", "TODO");
            public static readonly GUIContent GeneralShadingRim = new GUIContent("Rim", "TODO");
            public static readonly GUIContent GeneralShadingAdvanced = new GUIContent("Advanced", "TODO");


            public static readonly GUIContent ToonShadingHeader = new GUIContent("Toon Shading", "TODO");
            public static readonly GUIContent OutlineHeader = new GUIContent("Outline", "TODO");
            public static readonly GUIContent StylingAreaHeader = new GUIContent("Styling", "TODO");


            public static readonly GUIContent GeneralStylingDistanceFade = new GUIContent("Distance Fade", "TODO");

            public static readonly GUIContent Styling = new GUIContent("Styling", "TODO");
            public static readonly GUIContent StylingSettings = new GUIContent(" Style Settings", "TODO");

            public static readonly GUIContent StylingRandomizeHatching = new GUIContent(" Randomize", "TODO");
            public static readonly GUIContent StylingRandomizeHalftones = new GUIContent(" Randomize Halftones", "TODO");
            public static readonly GUIContent StylingBlending = new GUIContent("Blending", "TODO");
            public static readonly GUIContent StylingPositionAndBlending = new GUIContent(" Blending", "TODO");
            public static readonly GUIContent StylingDrawSpace = new GUIContent(" Draw Space", "TODO");
            public static readonly GUIContent StylingHalftonesDensityRotationOffset = new GUIContent(" Density, Rotation and Offset", "TODO");
            public static readonly GUIContent StylingHatchingDensityDirection = new GUIContent(" Density and Direction", "TODO");
            public static readonly GUIContent StylingPartitioning = new GUIContent(" Partitioning", "TODO");


            public static readonly GUIContent StylingShading = new GUIContent("Shading", "TODO");
            public static readonly GUIContent StylingCastShadows = new GUIContent(" Cast Shadows", "TODO");
            public static readonly GUIContent StylingSpecular = new GUIContent("Specular", "TODO");
            public static readonly GUIContent StylingRim = new GUIContent("Rim", "TODO");

            public static readonly GUIContent StylingCastShadowsSettings = new GUIContent(" Partitioning", "TODO");
            public static readonly GUIContent StylingSpecularSettings = new GUIContent(" Specular Settings", "TODO");
            public static readonly GUIContent StylingRimSettings = new GUIContent(" Rim Settings", "TODO");

        }

        Color oriCol;
        Color textColor;

        public bool m_FirstTimeApply = true;

        private const string k_KeyPrefix = "TheToonShader:Material:UI_State:";

        private string m_HeaderStateKey = null;
        protected string headerStateKey { get { return m_HeaderStateKey; } }


        protected MaterialEditor materialEditor { get; set; }
        protected MaterialProperty surfaceTypeProp { get; set; }
        protected MaterialProperty isOptimized { get; set; }
        protected MaterialProperty blendModeProp { get; set; }
        protected MaterialProperty cullingProp { get; set; }
        protected MaterialProperty alphaClipProp { get; set; }
        protected MaterialProperty alphaCutoffProp { get; set; }
        protected MaterialProperty receiveShadowsProp { get; set; }
        protected MaterialProperty baseMapProp { get; set; }
        protected MaterialProperty baseColorProp { get; set; }
        protected MaterialProperty useAlphaOnlyFromBaseMap { get; set; }
        protected MaterialProperty enableEmission{ get; set; }
        protected MaterialProperty emissionMapProp { get; set; }
        protected MaterialProperty emissionColorProp { get; set; }

        protected MaterialProperty enableNormalMap { get; set; }
        protected MaterialProperty bumpMapProp { get; set; }
        protected MaterialProperty bumpScaleProp { get; set; }
        protected MaterialProperty occlusionStrength { get; set; }
        protected MaterialProperty occlusionMap { get; set; }

        // URP 2020 SimpleLit
        //protected MaterialProperty specColor { get; set; }
        //protected MaterialProperty specGlossMap { get; set; }
        //protected MaterialProperty specHighlights { get; set; }
        //protected MaterialProperty smoothnessMapChannel { get; set; }
        //protected MaterialProperty smoothness { get; set; }

#if USING_URP
        private SimpleLitGUI.SimpleLitProperties shadingModelProperties;
#endif


        protected MaterialProperty shadingMode { get; set; }
        protected MaterialProperty lightFunction { get; set; }

        
        protected MaterialProperty useMainTex { get; set; } // 2d TOON ONLY
        protected MaterialProperty convertNormalToAlbedo { get; set; } // 2d TOON ONLY
        protected MaterialProperty normalToAlbedoConversionVector { get; set; } // 2d TOON ONLY

        #region MaterialPropertiesToonShading

        protected MaterialProperty enableToonShading { get; set; }
        protected MaterialProperty shadingFunction { get; set; }

        // currently 2d only
        protected MaterialProperty enableMainTexPosterization { get; set; } // 2d TOON ONLY
        protected MaterialProperty cellMethod { get; set; }// 2d TOON ONLY
        protected MaterialProperty roundingMethod { get; set; }// 2d TOON ONLY
        protected MaterialProperty useCoreShadowColor { get; set; }// 2d TOON ONLY
        protected MaterialProperty enableLightPartitioning { get; set; }// 2d TOON ONLY

        //protected MaterialProperty fakeLightPosition { get; set; } // TOON ONLY


        protected MaterialProperty gradientMode { get; set; }
        protected MaterialProperty gradientBlending { get; set; }
        protected MaterialProperty gradientBlendFactor { get; set; }
        //Gradient/Ramp
        protected MaterialProperty gradientTex { get; set; }
        //Cells
        protected MaterialProperty numberOfCells { get; set; }
        protected MaterialProperty cellTransitionSmoothness { get; set; }
        protected MaterialProperty sumLightsBeforePosterization { get; set; }
        protected MaterialProperty shadingUseLightColors { get; set; }

        protected MaterialProperty enableShadows { get; set; }
        protected MaterialProperty coreShadowColor { get; set; }
        protected MaterialProperty terminatorPosition { get; set; }
        protected MaterialProperty terminatorWidth { get; set; }
        protected MaterialProperty terminatorSmoothness { get; set; }
        protected MaterialProperty formShadowColor { get; set; }

        protected MaterialProperty shadingAffectByNormalMap { get; set; }
        protected MaterialProperty enableCastShadows { get; set; }
        protected MaterialProperty castShadowsStrength { get; set; }
        protected MaterialProperty castShadowsSmoothness { get; set; }
        //protected MaterialProperty customizeCastShadowsColor { get; set; }
        protected MaterialProperty castShadowColorMode { get; set; }
        protected MaterialProperty castShadowColor { get; set; }


        protected MaterialProperty enableSpecular { get; set; }
        protected MaterialProperty specularColor { get; set; }
        protected MaterialProperty specularSize { get; set; }
        protected MaterialProperty specularSmoothness { get; set; }
        protected MaterialProperty specularOpacity { get; set; }
        protected MaterialProperty specularAffectedByNormalMap { get; set; }
        protected MaterialProperty specularUseLightColors { get; set; }


        protected MaterialProperty enableRim { get; set; }
        protected MaterialProperty rimColor { get; set; }
        protected MaterialProperty rimSize { get; set; }
        protected MaterialProperty rimSmoothness { get; set; }
        protected MaterialProperty rimOpacity { get; set; }
        protected MaterialProperty rimAffectedArea { get; set; }
        protected MaterialProperty rimAffectedByNormalMap { get; set; }


        protected MaterialProperty specularBlending { get; set; }
        protected MaterialProperty rimBlending { get; set; }

        #endregion

        ///////////////////
        // ENABLE STYLE: //
        ///////////////////
        protected MaterialProperty enableStyling { get; set; }
        protected MaterialProperty enableShadingStyling { get; set; }
        protected MaterialProperty enableCastShadowsStyling { get; set; }
        protected MaterialProperty enableSpecularStyling { get; set; }
        protected MaterialProperty enableRimStyling { get; set; }

// STYLES
        protected MaterialProperty shadingStyle { get; set; }
        protected MaterialProperty castShadowsStyle { get; set; }
        protected MaterialProperty specularStyle { get; set; }
        protected MaterialProperty rimStyle { get; set; }   
        
// STYLING BLENDING
        protected MaterialProperty stylingShadingBlending { get; set; }
        protected MaterialProperty stylingCastShadowsBlending { get; set; }
        protected MaterialProperty stylingSpecularBlending { get; set; }
        protected MaterialProperty stylingRimBlending { get; set; }

        protected MaterialProperty stylingShadingIsInverted { get; set; }
        protected MaterialProperty stylingCastShadowsIsInverted { get; set; }
        protected MaterialProperty stylingSpecularIsInverted { get; set; }
        protected MaterialProperty stylingRimIsInverted { get; set; }


        #region MaterialProperties - Draw Spaces

        #region MaterialProperties - Draw Spaces - Shading
        protected MaterialProperty drawSpace { get; set; }
        protected MaterialProperty coordinateSystem { get; set; }
        protected MaterialProperty polarCenterMode { get; set; }
        protected MaterialProperty polarCenter { get; set; }
        protected MaterialProperty sSCameraDistanceScaled { get; set; }
        protected MaterialProperty anchorSSToObjectsOrigin { get; set; }
        #endregion

        #region MaterialProperties - Draw Spaces - Cast Shadows
        protected MaterialProperty castShadowsDrawSpace { get; set; }
        protected MaterialProperty castShadowsCoordinateSystem { get; set; }
        protected MaterialProperty castShadowsPolarCenterMode { get; set; }
        protected MaterialProperty castShadowsPolarCenter { get; set; }
        protected MaterialProperty castShadowsSSCameraDistanceScaled { get; set; }
        protected MaterialProperty castShadowsAnchorSSToObjectsOrigin { get; set; }
        #endregion

        #region MaterialProperties - Draw Spaces - Specular
        protected MaterialProperty specularDrawSpace { get; set; }
        protected MaterialProperty specularCoordinateSystem { get; set; }
        protected MaterialProperty specularPolarCenterMode { get; set; }
        protected MaterialProperty specularPolarCenter { get; set; }
        protected MaterialProperty specularSSCameraDistanceScaled { get; set; }
        protected MaterialProperty specularAnchorSSToObjectsOrigin { get; set; }
        #endregion

        #region MaterialProperties - Draw Spaces - Rim
        protected MaterialProperty rimDrawSpace { get; set; }
        protected MaterialProperty rimCoordinateSystem { get; set; }
        protected MaterialProperty rimPolarCenterMode { get; set; }
        protected MaterialProperty rimPolarCenter { get; set; }
        protected MaterialProperty rimSSCameraDistanceScaled { get; set; }
        protected MaterialProperty rimAnchorSSToObjectsOrigin { get; set; }
        #endregion

        #endregion


        protected MaterialProperty stylingTerminatorPosition { get; set; }
        protected MaterialProperty stylingOvermodelingFactor { get; set; }
        protected MaterialProperty hatchingAffectedByNormalMap { get; set; }
        protected MaterialProperty enableAntiAliasing { get; set; }


        protected MaterialProperty noiseMap1 { get; set; }
        protected MaterialProperty noiseMap2 { get; set; }

        protected MaterialProperty maxLightCount { get; set; } //URP2D
        protected MaterialProperty noiseTextureQuality { get; set; }


        protected MaterialProperty enableStylingDistanceFade { get; set; }
        protected MaterialProperty stylingDFStartingDistance { get; set; }
        protected MaterialProperty stylingDFFalloff { get; set; }
        protected MaterialProperty stylingAdjustDistanceFadeValue { get; set; }
        protected MaterialProperty stylingDistanceFadeValue { get; set; }


        #region MaterialProperties - Randomizer

        #region MaterialProperties - Randomizer - Shading
        protected MaterialProperty enableShadingRandomizer { get; set; }
        protected MaterialProperty shadingNoise1Size { get; set; }
        protected MaterialProperty shadingNoise1Seed { get; set; }
        protected MaterialProperty shadingNoise2Seed { get; set; }
        protected MaterialProperty noiseIntensity { get; set; }
        protected MaterialProperty hardnessRandomMode { get; set; }
        protected MaterialProperty hardnessRandomIntensity { get; set; }
        protected MaterialProperty spacingRandomMode { get; set; }
        protected MaterialProperty spacingRandomIntensity { get; set; }
        protected MaterialProperty opacityRandomMode { get; set; }
        protected MaterialProperty opacityRandomIntensity { get; set; }
        protected MaterialProperty lengthRandomMode { get; set; }
        protected MaterialProperty lengthRandomIntensity { get; set; }
        protected MaterialProperty thicknessRandomMode { get; set; }
        protected MaterialProperty thicknessRandomIntensity { get; set; }
        #endregion

        #region MaterialProperties - Randomizer - Cast Shadows
        protected MaterialProperty enableCastShadowsRandomizer { get; set; }
        protected MaterialProperty castShadowsNoise1Size { get; set; }
        protected MaterialProperty castShadowsNoise1Seed { get; set; }
        protected MaterialProperty castShadowsNoise2Seed { get; set; }
        protected MaterialProperty noiseIntensityCastShadows { get; set; }
        protected MaterialProperty hardnessRandomModeCastShadows { get; set; }
        protected MaterialProperty hardnessRandomIntensityCastShadows { get; set; }
        protected MaterialProperty spacingRandomModeCastShadows { get; set; }
        protected MaterialProperty spacingRandomIntensityCastShadows { get; set; }
        protected MaterialProperty opacityRandomModeCastShadows { get; set; }
        protected MaterialProperty opacityRandomIntensityCastShadows { get; set; }
        protected MaterialProperty lengthRandomModeCastShadows { get; set; }
        protected MaterialProperty lengthRandomIntensityCastShadows { get; set; }
        protected MaterialProperty thicknessRandomModeCastShadows { get; set; }
        protected MaterialProperty thicknessRandomIntensityCastShadows { get; set; }
        #endregion

        #region MaterialProperties - Randomizer - Specular
        protected MaterialProperty enableSpecularRandomizer { get; set; }
        protected MaterialProperty noise1SizeSpecular { get; set; }
        protected MaterialProperty noise1SeedSpecular { get; set; }
        protected MaterialProperty noise2SeedSpecular { get; set; }
        protected MaterialProperty noiseIntensitySpecular { get; set; }
        protected MaterialProperty hardnessRandomModeSpecular { get; set; }
        protected MaterialProperty hardnessRandomIntensitySpecular { get; set; }
        protected MaterialProperty spacingRandomModeSpecular { get; set; }
        protected MaterialProperty spacingRandomIntensitySpecular { get; set; }
        protected MaterialProperty opacityRandomModeSpecular { get; set; }
        protected MaterialProperty opacityRandomIntensitySpecular { get; set; }
        protected MaterialProperty lengthRandomModeSpecular { get; set; }
        protected MaterialProperty lengthRandomIntensitySpecular { get; set; }
        protected MaterialProperty thicknessRandomModeSpecular { get; set; }
        protected MaterialProperty thicknessRandomIntensitySpecular { get; set; }
        #endregion

        #region MaterialProperties - Randomizer - Rim
        protected MaterialProperty enableRimRandomizer { get; set; }
        protected MaterialProperty rimNoise1Size { get; set; }
        protected MaterialProperty rimNoise1Seed { get; set; }
        protected MaterialProperty rimNoise2Seed { get; set; }
        protected MaterialProperty noiseIntensityRim { get; set; }
        protected MaterialProperty hardnessRandomModeRim { get; set; }
        protected MaterialProperty hardnessRandomIntensityRim { get; set; }
        protected MaterialProperty spacingRandomModeRim { get; set; }
        protected MaterialProperty spacingRandomIntensityRim { get; set; }
        protected MaterialProperty opacityRandomModeRim { get; set; }
        protected MaterialProperty opacityRandomIntensityRim { get; set; }
        protected MaterialProperty lengthRandomModeRim { get; set; }
        protected MaterialProperty lengthRandomIntensityRim { get; set; }
        protected MaterialProperty thicknessRandomModeRim { get; set; }
        protected MaterialProperty thicknessRandomIntensityRim { get; set; }
        #endregion

        #endregion

        
        protected MaterialProperty stylingShadingSyncWithOtherStyling { get; set; }

        protected MaterialProperty syncWithLightPartitioning { get; set; }
        protected MaterialProperty numberOfCellsHatching { get; set; }

        protected MaterialProperty stylingShadingColor { get; set; }
        //protected MaterialProperty hatchingMap { get; set; } //tmp
        protected MaterialProperty stylingShadingDensity { get; set; }
        protected MaterialProperty stylingShadingThicknessControl { get; set; }
        protected MaterialProperty stylingShadingThickness { get; set; }
        protected MaterialProperty hatchingMinThickness { get; set; }
        protected MaterialProperty stylingShadingOpacity { get; set; }
        protected MaterialProperty stylingShadingOpacityFalloff { get; set; }
        protected MaterialProperty stylingShadingThicknessFalloff { get; set; }
        protected MaterialProperty stylingShadingHardness { get; set; }



        protected MaterialProperty hatchingShadingMode { get; set; }
        protected MaterialProperty hatchingDirection { get; set; }
        protected MaterialProperty hatchingDirectionSeed { get; set; }

        protected MaterialProperty stylingShadingInitialDirection { get; set; }
        protected MaterialProperty stylingShadingRotationBetweenCells { get; set; }


        protected MaterialProperty hatchingCameraDistanceFade { get; set; }
        protected MaterialProperty halftonePatternCameraDistanceFade { get; set; }


        protected MaterialProperty stylingShadingHalftonesRoundness { get; set; }
        protected MaterialProperty stylingShadingHalftonesRoundnessFalloff { get; set; }
        protected MaterialProperty stylingShadingHalftonesOffset { get; set; }



        protected MaterialProperty stylingCastShadowsSyncWithOtherStyling { get; set; }
        protected MaterialProperty stylingCastShadowsNumberOfCellsHatching { get; set; }
        protected MaterialProperty stylingCastShadowsSmoothness { get; set; }
        protected MaterialProperty stylingCastShadowsColor { get; set; }
        protected MaterialProperty stylingCastShadowsDensity { get; set; }
        protected MaterialProperty stylingCastShadowsHalftonesOffset { get; set; }
        protected MaterialProperty stylingCastShadowsThicknessControl { get; set; }
        protected MaterialProperty stylingCastShadowsThickness { get; set; }
        //protected MaterialProperty stylingSpecularMinThickness { get; set; }
        protected MaterialProperty stylingCastShadowsThicknessFalloff { get; set; }
        protected MaterialProperty stylingCastShadowsOpacity { get; set; }
        protected MaterialProperty stylingCastShadowsOpacityFalloff { get; set; }
        protected MaterialProperty stylingCastShadowsHardness { get; set; }
        protected MaterialProperty stylingCastShadowsHalftonesRoundness { get; set; }
        protected MaterialProperty stylingCastShadowsHalftonesRoundnessFalloff { get; set; }
        protected MaterialProperty stylingCastShadowsInitialDirection { get; set; }
        protected MaterialProperty stylingCastShadowsRotationBetweenCells { get; set; }



        protected MaterialProperty stylingSpecularSyncWithOtherStyling { get; set; }

        protected MaterialProperty syncWithSpecular { get; set; }
        protected MaterialProperty stylingSpecularSize { get; set; }
        protected MaterialProperty stylingSpecularSmoothness { get; set; }
        protected MaterialProperty stylingSpecularCutOutShading { get; set; }
        protected MaterialProperty stylingSpecularUseLightColors { get; set; }
        protected MaterialProperty stylingSpecularColorBoost { get; set; }
        
        protected MaterialProperty stylingSpecularColor { get; set; }
        protected MaterialProperty stylingSpecularDensity { get; set; }
        protected MaterialProperty stylingSpecularHalftonesOffset { get; set; }
        protected MaterialProperty stylingSpecularThicknessControl { get; set; }
        protected MaterialProperty stylingSpecularThickness { get; set; }
        //protected MaterialProperty stylingSpecularMinThickness { get; set; }
        protected MaterialProperty stylingSpecularThicknessFalloff { get; set; }
        protected MaterialProperty stylingSpecularOpacity { get; set; }
        protected MaterialProperty stylingSpecularOpacityFalloff { get; set; }
        protected MaterialProperty stylingSpecularHardness { get; set; }
        protected MaterialProperty stylingSpecularHalftonesRoundness { get; set; }
        protected MaterialProperty stylingSpecularHalftonesRoundnessFalloff { get; set; }
        protected MaterialProperty stylingSpecularRotation { get; set; }



        protected MaterialProperty stylingRimSyncWithOtherStyling { get; set; }

        protected MaterialProperty syncWithRim { get; set; }
        protected MaterialProperty stylingRimSize { get; set; }
        protected MaterialProperty stylingRimSmoothness { get; set; }
        protected MaterialProperty stylingRimAffectedArea { get; set; }

        protected MaterialProperty stylingRimColor { get; set; }
        protected MaterialProperty stylingRimDensity { get; set; }
        protected MaterialProperty stylingRimHalftonesOffset { get; set; }
        protected MaterialProperty stylingRimThicknessControl { get; set; }
        protected MaterialProperty stylingRimThickness { get; set; }
      //  protected MaterialProperty stylingRimMinThickness { get; set; }
        protected MaterialProperty stylingRimThicknessFalloff { get; set; }
        protected MaterialProperty stylingRimOpacity { get; set; }
        protected MaterialProperty stylingRimOpacityFalloff { get; set; }
        protected MaterialProperty stylingRimHardness { get; set; }
        protected MaterialProperty stylingRimHalftonesRoundness { get; set; }
        protected MaterialProperty stylingRimHalftonesRoundnessFalloff { get; set; }
        protected MaterialProperty stylingRimRotation { get; set; }



        protected MaterialProperty enableOutline { get; set; }
        protected MaterialProperty outlineColor { get; set; }
        protected MaterialProperty outlineWidth { get; set; }
        protected MaterialProperty outlineSoftness { get; set; }
        protected MaterialProperty outlineMode { get; set; }


        protected MaterialProperty outlineDepthOffset { get; set; }
        protected MaterialProperty outlineCameraDistanceImpact { get; set; }

        SavedString originalShaderName;

        SavedBool m_PerformanceAreaFoldout;
        SavedBool m_SurfaceInputsAndOptionsAreaFoldout;

        SavedBool m_SurfaceOptionsFoldout;
        SavedBool m_SurfaceInputsFoldout;

        SavedBool m_GeneralShadingLightPartitioningFoldout;
        SavedBool m_GeneralShadingLightingFoldout;
        SavedBool m_GeneralShadingShadowFoldout;
        SavedBool m_GeneralShadingCastShadowsFoldout;
        SavedBool m_GeneralShadingSpecularFoldout;
        SavedBool m_GeneralShadingRimFoldout;
        SavedBool m_GeneralShadingAdvancedFoldout;

        SavedBool m_ToonShadingAreaFoldout;
        SavedBool m_StylingAreaFoldout;
        SavedBool m_OutlineAreaFoldout;

        SavedBool m_GeneralStylingDistanceFadeFoldout;


        SavedBool m_StylingShadingFoldout;
        SavedBool m_ShadingStylingBlendingFoldout;
        SavedBool m_ShadingStylingPartitioningFoldout;
        SavedBool m_ShadingStylingDrawSpaceFoldout;
        SavedBool m_ShadingStylingRotationFoldout;
        SavedBool m_ShadingStylingStylingFoldout;
        SavedBool m_ShadingStylingRandomizeFoldout;

        SavedBool m_StylingCastShadowsFoldout;
        SavedBool m_StylingCastShadowsSettingsFoldout;
        SavedBool m_CastShadowsStylingBlendingFoldout;
        SavedBool m_CastShadowsStylingPartitioningFoldout;
        SavedBool m_CastShadowsStylingDrawSpaceFoldout;
        SavedBool m_CastShadowsStylingRotationFoldout;
        SavedBool m_CastShadowsStylingStylingFoldout;
        SavedBool m_CastShadowsStylingRandomizeFoldout;

        SavedBool m_StylingSpecularFoldout;
        SavedBool m_StylingSpecularSettingsFoldout;
        SavedBool m_SpecularStylingBlendingFoldout;
        SavedBool m_SpecularStylingPartitioningFoldout;
        SavedBool m_SpecularStylingDrawSpaceFoldout;
        SavedBool m_SpecularStylingRotationFoldout;
        SavedBool m_SpecularStylingStylingFoldout;
        SavedBool m_SpecularStylingRandomizeFoldout;

        SavedBool m_StylingRimFoldout;
        SavedBool m_StylingRimSettingsFoldout;
        SavedBool m_RimStylingBlendingFoldout;
        SavedBool m_RimStylingPartitioningFoldout;
        SavedBool m_RimStylingDrawSpaceFoldout;
        SavedBool m_RimStylingRotationFoldout;
        SavedBool m_RimStylingStylingFoldout;
        SavedBool m_RimStylingRandomizeFoldout;



        AnimBool presetsAnimBool;
        AnimBool performanceAnimBool;
        AnimBool surfaceOptionsAndInputsAnimBool;
        AnimBool generalShadingAnimBool;
        AnimBool toonShadingAnimBool;



        AnimBool stylingAnimBool;
        AnimBool enableStylingAnimBool;

        AnimBool animationAnimBool;

        AnimBool outlineAnimBool;
        AnimBool enableOutlineAnimBool;



        float oriLabelWidth = EditorGUIUtility.labelWidth;


        //AnimBool enableCastShadowsAnimBool;
        //AnimBool enableShadingStylingAnimBool;
        //AnimBool enableSpecularAnimBool;
        //AnimBool enableRimAnimBool;
        //AnimBool enableOutlineAnimBool;


        Color lightGreyForLines = new Color(0.35f, 0.35f, 0.35f, 1);



        TheToonShaderGenerator toonShaderGenerator = new TheToonShaderGenerator();


        public virtual void FindProperties2D(MaterialProperty[] properties, Material material)
        {
            if (material.HasProperty("_IsOptimized"))
            {
                isOptimized = FindProperty("_IsOptimized", properties);
            }

            useMainTex = FindProperty("_UseMainTex", properties, false);

            convertNormalToAlbedo = FindProperty("_ConvertNormalToAlbedo", properties, false);
            normalToAlbedoConversionVector = FindProperty("_NormalToAlbedoConversionVector", properties, false);

            
            //#if USING_URP
            //            surfaceTypeProp = FindProperty("_Surface", properties);
            //            blendModeProp = FindProperty("_Blend", properties);
            //            cullingProp = FindProperty("_Cull", properties);
            //            alphaClipProp = FindProperty("_AlphaClip", properties);
            //            alphaCutoffProp = FindProperty("_Cutoff", properties);
            //            receiveShadowsProp = FindProperty("_ReceiveShadows", properties, false);
            baseMapProp = FindProperty("_MainTex", properties, false);
            //baseColorProp = FindProperty("_Color", properties, false);
            baseColorProp = FindProperty("_BaseColor", properties, false);

            //            useAlphaOnlyFromBaseMap = FindProperty("_UseAlphaOnlyFromBaseMap", properties, false);

            enableEmission = FindProperty("_EnableEmission", properties, false);
            if (material.HasProperty("_EmissionMap"))
            {
                emissionMapProp = FindProperty("_EmissionMap", properties, false);
            }
            emissionColorProp = FindProperty("_EmissionColor", properties, false);
            //            //queueOffsetProp = FindProperty("_QueueOffset", properties, false);

            enableNormalMap = FindProperty("_EnableNormalMap", properties, propertyIsMandatory: false);
            if (material.HasProperty("_NormalMap"))
            {
                bumpMapProp = FindProperty("_NormalMap", properties, propertyIsMandatory: false);
            }
            bumpScaleProp = FindProperty("_NormalMapScale", properties, propertyIsMandatory: false);
            //            occlusionStrength = FindProperty("_OcclusionStrength", properties, propertyIsMandatory: false);
            //            occlusionMap = FindProperty("_OcclusionMap", properties, propertyIsMandatory: false);

            //            //URP 2020 SimpleLIT:
            //            //https://github.com/Unity-Technologies/Graphics/blob/v10.10.2/com.unity.render-pipelines.universal/Editor/ShaderGUI/ShadingModels/SimpleLitGUI.cs
            //            //specColor = FindProperty("_SpecColor", properties);
            //            //specGlossMap = FindProperty("_SpecGlossMap", properties, false);
            //            //specHighlights = FindProperty("_SpecularHighlights", properties, false);
            //            //smoothnessMapChannel = FindProperty("_SmoothnessSource", properties, false);
            //            //smoothness = FindProperty("_Smoothness", properties, false);


            //            shadingModelProperties = new SimpleLitGUI.SimpleLitProperties(properties);
            //#endif

            shadingMode = FindProperty("_ShadingMode", properties);
            //lightFunction = FindProperty("_LightFunction", properties);

            #region FindProperties - Toon Shading

            enableToonShading = FindProperty("_EnableToonShading", properties);

            //fakeLightPosition = FindProperty("_FakeLightPosition", properties);


            shadingFunction = FindProperty("_ShadingFunction", properties);


            enableMainTexPosterization = FindProperty("_EnableMainTexPosterization", properties);
            cellMethod = FindProperty("_CellMethod", properties);
            roundingMethod = FindProperty("_RoundingMethod", properties);
            useCoreShadowColor = FindProperty("_UseCoreShadowColor", properties);
            enableLightPartitioning = FindProperty("_EnableLightPartitioning", properties);
            //gradientMode = FindProperty("_GradientMode", properties);

            gradientBlending = FindProperty("_GradientBlending", properties);
            gradientBlendFactor = FindProperty("_GradientBlendFactor", properties);

            gradientTex = FindProperty("_GradientTex", properties);

            numberOfCells = FindProperty("_NumberOfCells", properties);
            cellTransitionSmoothness = FindProperty("_CellTransitionSmoothness", properties);
            //sumLightsBeforePosterization = FindProperty("_SumLightsBeforePosterization", properties);
            shadingUseLightColors = FindProperty("_ShadingUseLightColors", properties);

            enableShadows = FindProperty("_EnableShadows", properties);
            //shadowMode = FindProperty("_ShadowMode", properties);
            coreShadowColor = FindProperty("_CoreShadowColor", properties);
            //terminatorWidth = FindProperty("_TerminatorWidth", properties);
            //terminatorSmoothness = FindProperty("_TerminatorSmoothness", properties);
            //formShadowColor = FindProperty("_FormShadowColor", properties);
            //shadingAffectByNormalMap = FindProperty("_ShadingAffectedByNormalMap", properties);
            //enableCastShadows = FindProperty("_EnableCastShadows", properties);
            //castShadowsStrength = FindProperty("_CastShadowsStrength", properties);
            //castShadowsSmoothness = FindProperty("_CastShadowsSmoothness", properties);
            ////customizeCastShadowsColor = FindProperty("_CustomizeCastShadowsColor", properties);
            //castShadowColorMode = FindProperty("_CastShadowColorMode", properties);
            //castShadowColor = FindProperty("_CastShadowColor", properties);

            enableSpecular = FindProperty("_EnableSpecular", properties);
            specularColor = FindProperty("_SpecularColor", properties);
            specularSize = FindProperty("_SpecularSize", properties);
            specularSmoothness = FindProperty("_SpecularSmoothness", properties);
            specularOpacity = FindProperty("_SpecularOpacity", properties);
            specularAffectedByNormalMap = FindProperty("_SpecularAffectedByNormalMap", properties);
            specularUseLightColors = FindProperty("_SpecularUseLightColors", properties);

            enableRim = FindProperty("_EnableRim", properties);
            rimColor = FindProperty("_RimColor", properties);
            rimSize = FindProperty("_RimSize", properties);
            rimSmoothness = FindProperty("_RimSmoothness", properties);
            rimOpacity = FindProperty("_RimOpacity", properties);
            rimAffectedArea = FindProperty("_RimAffectedArea", properties);
            rimAffectedByNormalMap = FindProperty("_RimAffectedByNormalMap", properties);

            specularBlending = FindProperty("_SpecularBlending", properties);
            rimBlending = FindProperty("_RimBlending", properties);

            #endregion

            //////////////////////////////////////////////////////////////////////////////
            //                              STYLING GENERAL                             //
            //////////////////////////////////////////////////////////////////////////////
            //#region FindProperties - Styling - Distance Fade
            //enableStylingDistanceFade = FindProperty("_EnableStylingDistanceFade", properties);
            //stylingDFStartingDistance = FindProperty("_StylingDFStartingDistance", properties);
            //stylingDFFalloff = FindProperty("_StylingDFFalloff", properties);
            //stylingAdjustDistanceFadeValue = FindProperty("_StylingAdjustDistanceFadeValue", properties);
            //stylingDistanceFadeValue = FindProperty("_StylingDistanceFadeValue", properties);
            //#endregion

            enableStyling = FindProperty("_EnableStyling", properties);

            enableAntiAliasing = FindProperty("_EnableAntiAliasing", properties);



            #region FindProperties - Styling - Shading
            ////////////////////////////////////////////////////////////////////////////////////////////////////
            //                                      SHADING STYLING:                                          //
            ////////////////////////////////////////////////////////////////////////////////////////////////////
            enableShadingStyling = FindProperty("_EnableShadingStyling", properties);
            stylingShadingSyncWithOtherStyling = FindProperty("_StylingShadingSyncWithOtherStyling", properties);


            shadingStyle = FindProperty("_ShadingStyle", properties);

            //PARTITIONING - SHADING STYLING: 
            syncWithLightPartitioning = FindProperty("_SyncWithLightPartitioning", properties);
            numberOfCellsHatching = FindProperty("_NumberOfCellsHatching", properties);
            stylingOvermodelingFactor = FindProperty("_StylingOvermodelingFactor", properties);

            //BLENDING - SHADING STYLING: 
            stylingShadingBlending = FindProperty("_StylingShadingBlending", properties);
            stylingShadingIsInverted = FindProperty("_StylingShadingIsInverted", properties);

            //DRAW SPACE - SHADING STYLING: 
            drawSpace = FindProperty("_DrawSpace", properties);
            coordinateSystem = FindProperty("_CoordinateSystem", properties);
            polarCenterMode = FindProperty("_PolarCenterMode", properties);
            polarCenter = FindProperty("_PolarCenter", properties);
            sSCameraDistanceScaled = FindProperty("_SSCameraDistanceScaled", properties);
            anchorSSToObjectsOrigin = FindProperty("_AnchorSSToObjectsOrigin", properties);

            //DENSITY AND DIRECTION  - SHADING STYLING: 
            stylingShadingDensity = FindProperty("_StylingShadingDensity", properties);
            stylingShadingInitialDirection = FindProperty("_StylingShadingInitialDirection", properties);
            stylingShadingRotationBetweenCells = FindProperty("_StylingShadingRotationBetweenCells", properties);
            stylingShadingHalftonesOffset = FindProperty("_StylingShadingHalftonesOffset", properties);

            //STYLE SETTINGS - SHADING STYLING:
            stylingShadingColor = FindProperty("_StylingColor", properties);
            stylingShadingOpacity = FindProperty("_StylingShadingOpacity", properties);
            stylingShadingOpacityFalloff = FindProperty("_StylingShadingOpacityFalloff", properties);
            stylingShadingThicknessControl = FindProperty("_StylingShadingThicknessControl", properties);
            stylingShadingThickness = FindProperty("_StylingShadingThickness", properties);
            stylingShadingThicknessFalloff = FindProperty("_StylingShadingThicknessFalloff", properties);
            stylingShadingHardness = FindProperty("_StylingShadingHardness", properties);
            stylingShadingHalftonesRoundness = FindProperty("_StylingShadingHalftonesRoundness", properties);
            stylingShadingHalftonesRoundnessFalloff = FindProperty("_StylingShadingHalftonesRoundnessFalloff", properties);

            //RANDOMIZER - SHADING STYLING:
            enableShadingRandomizer = FindProperty("_EnableShadingRandomizer", properties);
            shadingNoise1Size = FindProperty("_ShadingNoise1Size", properties);
            shadingNoise1Seed = FindProperty("_ShadingNoise1Seed", properties);
            shadingNoise2Seed = FindProperty("_ShadingNoise2Seed", properties);
            noiseIntensity = FindProperty("_NoiseIntensity", properties);
            spacingRandomMode = FindProperty("_SpacingRandomMode", properties);
            spacingRandomIntensity = FindProperty("_SpacingRandomIntensity", properties);
            opacityRandomMode = FindProperty("_OpacityRandomMode", properties);
            opacityRandomIntensity = FindProperty("_OpacityRandomIntensity", properties);
            hardnessRandomMode = FindProperty("_HardnessRandomMode", properties);
            hardnessRandomIntensity = FindProperty("_HardnessRandomIntensity", properties);
            lengthRandomMode = FindProperty("_LengthRandomMode", properties);
            lengthRandomIntensity = FindProperty("_LengthRandomIntensity", properties);
            thicknessRandomMode = FindProperty("_ThicknessRandomMode", properties);
            thicknessRandomIntensity = FindProperty("_ThicknesshRandomIntensity", properties);
            #endregion

            #region FindProperties - Styling - Cast Shadows
            //////////////////////////////////////////////////////////////////////////////////////////////////////
            ////                                 CAST SHADOWS STYLING:                                          //
            //////////////////////////////////////////////////////////////////////////////////////////////////////

            //enableCastShadowsStyling = FindProperty("_EnableCastShadowsStyling", properties);
            //stylingCastShadowsSyncWithOtherStyling = FindProperty("_EnableCastShadowsStyling", properties);

            //castShadowsStyle = FindProperty("_CastShadowsStyle", properties);

            ////STYLE - CAST SHADOWS STYLING: 
            //stylingCastShadowsSyncWithOtherStyling = FindProperty("_StylingCastShadowsSyncWithOtherStyling", properties);

            ////PARTITIONING - CAST SHADOWS STYLING: 
            //stylingCastShadowsNumberOfCellsHatching = FindProperty("_CastShadowsNumberOfCellsHatching", properties);
            //stylingCastShadowsSmoothness = FindProperty("_StylingCastShadowsSmoothness", properties);

            ////BLENDING - CAST SHADOWS STYLING: 
            //stylingCastShadowsBlending = FindProperty("_StylingCastShadowsBlending", properties);
            //stylingCastShadowsIsInverted = FindProperty("_StylingCastShadowsIsInverted", properties);

            ////DRAW SPACE - CAST SHADOWS STYLING: 
            //castShadowsDrawSpace = FindProperty("_CastShadowsDrawSpace", properties);
            //castShadowsSSCameraDistanceScaled = FindProperty("_CastShadowsSSCameraDistanceScaled", properties);
            //castShadowsAnchorSSToObjectsOrigin = FindProperty("_CastShadowsAnchorSSToObjectsOrigin", properties);
            //castShadowsCoordinateSystem = FindProperty("_CastShadowsCoordinateSystem", properties);
            //castShadowsPolarCenterMode = FindProperty("_CastShadowsPolarCenterMode", properties);
            //castShadowsPolarCenter = FindProperty("_CastShadowsPolarCenter", properties);

            ////DENSITY AND DIRECTION - CAST SHADOWS STYLING: 
            //stylingCastShadowsDensity = FindProperty("_StylingCastShadowsDensity", properties);
            //stylingCastShadowsInitialDirection = FindProperty("_StylingCastShadowsInitialDirection", properties);
            //stylingCastShadowsRotationBetweenCells = FindProperty("_StylingCastShadowsRotationBetweenCells", properties);
            //stylingCastShadowsHalftonesOffset = FindProperty("_StylingCastShadowsHalftonesOffset", properties);

            ////STYLE SETTINGS - CAST SHADOWS STYLING: 
            //stylingCastShadowsColor = FindProperty("_StylingCastShadowsColor", properties);
            //stylingCastShadowsOpacity = FindProperty("_StylingCastShadowsOpacity", properties);
            //stylingCastShadowsOpacityFalloff = FindProperty("_StylingCastShadowsOpacityFalloff", properties);
            //stylingCastShadowsThicknessControl = FindProperty("_StylingCastShadowsThicknessControl", properties);
            //stylingCastShadowsThickness = FindProperty("_StylingCastShadowsThickness", properties);
            //stylingCastShadowsThicknessFalloff = FindProperty("_StylingCastShadowsThicknessFalloff", properties);
            //stylingCastShadowsHardness = FindProperty("_StylingCastShadowsHardness", properties);
            //stylingCastShadowsHalftonesRoundness = FindProperty("_StylingCastShadowsHalftonesRoundness", properties);
            //stylingCastShadowsHalftonesRoundnessFalloff = FindProperty("_StylingCastShadowsHalftonesRoundnessFalloff", properties);

            ////RANDOMIZER - CAST SHADOWS STYLING:
            //enableCastShadowsRandomizer = FindProperty("_EnableCastShadowsRandomizer", properties);
            //castShadowsNoise1Size = FindProperty("_CastShadowsNoise1Size", properties);
            //castShadowsNoise1Seed = FindProperty("_CastShadowsNoise1Seed", properties);
            //castShadowsNoise2Seed = FindProperty("_CastShadowsNoise2Seed", properties);
            //noiseIntensityCastShadows = FindProperty("_CastShadowsNoiseIntensity", properties);
            //spacingRandomModeCastShadows = FindProperty("_CastShadowsSpacingRandomMode", properties);
            //spacingRandomIntensityCastShadows = FindProperty("_CastShadowsSpacingRandomIntensity", properties);
            //opacityRandomModeCastShadows = FindProperty("_CastShadowsOpacityRandomMode", properties);
            //opacityRandomIntensityCastShadows = FindProperty("_CastShadowsOpacityRandomIntensity", properties);
            //hardnessRandomModeCastShadows = FindProperty("_CastShadowsHardnessRandomMode", properties);
            //hardnessRandomIntensityCastShadows = FindProperty("_CastShadowsHardnessRandomIntensity", properties);
            //lengthRandomModeCastShadows = FindProperty("_CastShadowsLengthRandomMode", properties);
            //lengthRandomIntensityCastShadows = FindProperty("_CastShadowsLengthRandomIntensity", properties);
            //thicknessRandomModeCastShadows = FindProperty("_CastShadowsThicknessRandomMode", properties);
            //thicknessRandomIntensityCastShadows = FindProperty("_CastShadowsThicknesshRandomIntensity", properties);
            #endregion

            #region FindProperties - Styling - Specular
            ////////////////////////////////////////////////////////////////////////////////////////////////////
            //                                 SPECULAR STYLING:                                              //
            ////////////////////////////////////////////////////////////////////////////////////////////////////

            enableSpecularStyling = FindProperty("_EnableSpecularStyling", properties);
            stylingSpecularSyncWithOtherStyling = FindProperty("_StylingSpecularSyncWithOtherStyling", properties);

            //SPECULAR SETTINGS - SPECULAR STYLING: 
            syncWithSpecular = FindProperty("_SyncWithSpecular", properties);
            stylingSpecularSize = FindProperty("_StylingSpecularSize", properties);
            stylingSpecularSmoothness = FindProperty("_StylingSpecularSmoothness", properties);
            stylingSpecularCutOutShading = FindProperty("_StylingSpecularCutOutShading", properties);
            stylingSpecularUseLightColors = FindProperty("_StylingSpecularUseLightColors", properties);

            stylingSpecularColorBoost = FindProperty("_StylingSpecularColorBoost", properties);

            //STYLE - SPECULAR STYLING: 
            specularStyle = FindProperty("_SpecularStyle", properties);

            //BLENDING - SPECULAR STYLING: 
            stylingSpecularBlending = FindProperty("_StylingSpecularBlending", properties);
            stylingSpecularIsInverted = FindProperty("_StylingSpecularIsInverted", properties);

            //DRAW SPACE - SPECULAR STYLING: 
            specularDrawSpace = FindProperty("_SpecularDrawSpace", properties);
            specularCoordinateSystem = FindProperty("_SpecularCoordinateSystem", properties);
            specularPolarCenterMode = FindProperty("_SpecularPolarCenterMode", properties);
            specularPolarCenter = FindProperty("_SpecularPolarCenter", properties);
            specularSSCameraDistanceScaled = FindProperty("_SpecularSSCameraDistanceScaled", properties);
            specularAnchorSSToObjectsOrigin = FindProperty("_SpecularAnchorSSToObjectsOrigin", properties);

            //DENSITY AND DIRECTION - SPECULAR STYLING:
            stylingSpecularDensity = FindProperty("_StylingSpecularDensity", properties);
            stylingSpecularRotation = FindProperty("_StylingSpecularRotation", properties);
            stylingSpecularHalftonesOffset = FindProperty("_StylingSpecularHalftonesOffset", properties);

            //STYLE SETTINGS - SPECULAR STYLING:
            stylingSpecularColor = FindProperty("_StylingSpecularColor", properties);
            stylingSpecularOpacity = FindProperty("_StylingSpecularOpacity", properties);
            stylingSpecularOpacityFalloff = FindProperty("_StylingSpecularOpacityFalloff", properties);
            stylingSpecularThicknessControl = FindProperty("_StylingSpecularThicknessControl", properties);
            stylingSpecularThickness = FindProperty("_StylingSpecularThickness", properties);
            stylingSpecularThicknessFalloff = FindProperty("_StylingSpecularThicknessFalloff", properties);
            stylingSpecularHardness = FindProperty("_StylingSpecularHardness", properties);
            stylingSpecularHalftonesRoundness = FindProperty("_StylingSpecularHalftonesRoundness", properties);
            stylingSpecularHalftonesRoundnessFalloff = FindProperty("_StylingSpecularHalftonesRoundnessFalloff", properties);

            //RANDOMIZER - SPECULAR STYLING:
            enableSpecularRandomizer = FindProperty("_EnableSpecularRandomizer", properties);
            noise1SizeSpecular = FindProperty("_SpecularNoise1Size", properties);
            noise1SeedSpecular = FindProperty("_SpecularNoise1Seed", properties);
            noise2SeedSpecular = FindProperty("_SpecularNoise2Seed", properties);
            noiseIntensitySpecular = FindProperty("_SpecularNoiseIntensity", properties);
            spacingRandomModeSpecular = FindProperty("_SpecularSpacingRandomMode", properties);
            spacingRandomIntensitySpecular = FindProperty("_SpecularSpacingRandomIntensity", properties);
            opacityRandomModeSpecular = FindProperty("_SpecularOpacityRandomMode", properties);
            opacityRandomIntensitySpecular = FindProperty("_SpecularOpacityRandomIntensity", properties);
            hardnessRandomModeSpecular = FindProperty("_SpecularHardnessRandomMode", properties);
            hardnessRandomIntensitySpecular = FindProperty("_SpecularHardnessRandomIntensity", properties);
            lengthRandomModeSpecular = FindProperty("_SpecularLengthRandomMode", properties);
            lengthRandomIntensitySpecular = FindProperty("_SpecularLengthRandomIntensity", properties);
            thicknessRandomModeSpecular = FindProperty("_SpecularThicknessRandomMode", properties);
            thicknessRandomIntensitySpecular = FindProperty("_SpecularThicknesshRandomIntensity", properties);
            #endregion

            #region FindProperties - Styling - Rim
            ////////////////////////////////////////////////////////////////////////////////////////////////////
            //                                      RIM STYLING:                                              //
            ////////////////////////////////////////////////////////////////////////////////////////////////////

            enableRimStyling = FindProperty("_EnableRimStyling", properties);
            stylingRimSyncWithOtherStyling = FindProperty("_StylingRimSyncWithOtherStyling", properties);

            //RIM SETTINGS - RIM STYLING:
            syncWithRim = FindProperty("_SyncWithRim", properties);
            stylingRimSize = FindProperty("_StylingRimSize", properties);
            stylingRimSmoothness = FindProperty("_StylingRimSmoothness", properties);
            stylingRimAffectedArea = FindProperty("_StylingRimAffectedArea", properties);

            //STYLE - RIM STYLING: 
            rimStyle = FindProperty("_RimStyle", properties);

            //BLENDING - RIM STYLING: 
            stylingRimBlending = FindProperty("_StylingRimBlending", properties);
            stylingRimIsInverted = FindProperty("_StylingRimIsInverted", properties);

            //DRAW SPACE - RIM STYLING: 
            rimDrawSpace = FindProperty("_RimDrawSpace", properties);
            rimCoordinateSystem = FindProperty("_RimCoordinateSystem", properties);
            rimPolarCenterMode = FindProperty("_RimPolarCenterMode", properties);
            rimPolarCenter = FindProperty("_RimPolarCenter", properties);
            rimSSCameraDistanceScaled = FindProperty("_RimSSCameraDistanceScaled", properties);
            rimAnchorSSToObjectsOrigin = FindProperty("_RimAnchorSSToObjectsOrigin", properties);

            //DENSITY AND DIRECTION - RIM STYLING:
            stylingRimDensity = FindProperty("_StylingRimDensity", properties);
            stylingRimRotation = FindProperty("_StylingRimRotation", properties);
            stylingRimHalftonesOffset = FindProperty("_StylingRimHalftonesOffset", properties);

            //STYLING SETTINGS - RIM STYLING: 
            stylingRimColor = FindProperty("_StylingRimColor", properties);
            stylingRimThicknessControl = FindProperty("_StylingRimThicknessControl", properties);
            stylingRimThickness = FindProperty("_StylingRimThickness", properties);
            stylingRimThicknessFalloff = FindProperty("_StylingRimThicknessFalloff", properties);
            stylingRimOpacity = FindProperty("_StylingRimOpacity", properties);
            stylingRimOpacityFalloff = FindProperty("_StylingRimOpacityFalloff", properties);
            stylingRimHardness = FindProperty("_StylingRimHardness", properties);
            stylingRimHalftonesRoundness = FindProperty("_StylingRimHalftonesRoundness", properties);
            stylingRimHalftonesRoundnessFalloff = FindProperty("_StylingRimHalftonesRoundnessFalloff", properties);

            //RANDOMIZER - RIM STYLING:
            enableRimRandomizer = FindProperty("_EnableRimRandomizer", properties);
            rimNoise1Size = FindProperty("_RimNoise1Size", properties);
            rimNoise1Seed = FindProperty("_RimNoise1Seed", properties);
            rimNoise2Seed = FindProperty("_RimNoise2Seed", properties);
            noiseIntensityRim = FindProperty("_RimNoiseIntensity", properties);
            spacingRandomModeRim = FindProperty("_RimSpacingRandomMode", properties);
            spacingRandomIntensityRim = FindProperty("_RimSpacingRandomIntensity", properties);
            opacityRandomModeRim = FindProperty("_RimOpacityRandomMode", properties);
            opacityRandomIntensityRim = FindProperty("_RimOpacityRandomIntensity", properties);
            hardnessRandomModeRim = FindProperty("_RimHardnessRandomMode", properties);
            hardnessRandomIntensityRim = FindProperty("_RimHardnessRandomIntensity", properties);
            lengthRandomModeRim = FindProperty("_RimLengthRandomMode", properties);
            lengthRandomIntensityRim = FindProperty("_RimLengthRandomIntensity", properties);
            thicknessRandomModeRim = FindProperty("_RimThicknessRandomMode", properties);
            thicknessRandomIntensityRim = FindProperty("_RimThicknesshRandomIntensity", properties);
            #endregion


            hatchingAffectedByNormalMap = FindProperty("_HatchingAffectedByNormalMap", properties);

            noiseMap1 = FindProperty("_NoiseMap1", properties);
            noiseMap2 = FindProperty("_NoiseMap2", properties);


            maxLightCount = FindProperty("_MaxLightCount", properties);
            noiseTextureQuality = FindProperty("_NoiseTextureQuality", properties);

            //hatchingCameraDistanceFade = FindProperty("_HatchingCameraDistanceFade", properties);
            //halftonePatternCameraDistanceFade = FindProperty("_HalftonePatternCameraDistanceFade", properties);

            enableOutline = FindProperty("_EnableOutline", properties);
            outlineColor = FindProperty("_OutlineColor", properties);
            outlineWidth = FindProperty("_OutlineWidth", properties);
            outlineSoftness = FindProperty("_OutlineSoftness", properties);
            outlineMode = FindProperty("_OutlineMode", properties);
            //outlineDepthOffset = FindProperty("_OutlineDepthOffset", properties);
            outlineCameraDistanceImpact = FindProperty("_OutlineCameraDistanceImpact", properties);
        }




        public virtual void FindProperties(MaterialProperty[] properties, Material material)
        {
            if(material.HasProperty("_IsOptimized"))
            {
                isOptimized = FindProperty("_IsOptimized", properties);
            }


#if USING_URP
            surfaceTypeProp = FindProperty("_Surface", properties);
            blendModeProp = FindProperty("_Blend", properties);
            cullingProp = FindProperty("_Cull", properties);
            alphaClipProp = FindProperty("_AlphaClip", properties);
            alphaCutoffProp = FindProperty("_Cutoff", properties);
            receiveShadowsProp = FindProperty("_ReceiveShadows", properties, false);
            baseMapProp = FindProperty("_BaseMap", properties, false);
            baseColorProp = FindProperty("_BaseColor", properties, false);

            useAlphaOnlyFromBaseMap = FindProperty("_UseAlphaOnlyFromBaseMap", properties, false);

            emissionMapProp = FindProperty("_EmissionMap", properties, false);
            emissionColorProp = FindProperty("_EmissionColor", properties, false);
            //queueOffsetProp = FindProperty("_QueueOffset", properties, false);

            bumpMapProp = FindProperty("_BumpMap", properties, propertyIsMandatory: false);
            bumpScaleProp = FindProperty("_BumpScale", properties, propertyIsMandatory: false);
            occlusionStrength = FindProperty("_OcclusionStrength", properties, propertyIsMandatory: false);
            occlusionMap = FindProperty("_OcclusionMap", properties, propertyIsMandatory: false);

            //URP 2020 SimpleLIT:
            //https://github.com/Unity-Technologies/Graphics/blob/v10.10.2/com.unity.render-pipelines.universal/Editor/ShaderGUI/ShadingModels/SimpleLitGUI.cs
            //specColor = FindProperty("_SpecColor", properties);
            //specGlossMap = FindProperty("_SpecGlossMap", properties, false);
            //specHighlights = FindProperty("_SpecularHighlights", properties, false);
            //smoothnessMapChannel = FindProperty("_SmoothnessSource", properties, false);
            //smoothness = FindProperty("_Smoothness", properties, false);


            shadingModelProperties = new SimpleLitGUI.SimpleLitProperties(properties);
#endif

            shadingMode = FindProperty("_ShadingMode", properties);
            lightFunction = FindProperty("_LightFunction", properties);

            #region FindProperties - Toon Shading

            enableToonShading = FindProperty("_EnableToonShading", properties);

            shadingFunction = FindProperty("_ShadingFunction", properties);
            gradientMode = FindProperty("_GradientMode", properties);

            gradientBlending = FindProperty("_GradientBlending", properties);
            gradientBlendFactor = FindProperty("_GradientBlendFactor", properties);

            gradientTex = FindProperty("_GradientTex", properties);

            numberOfCells = FindProperty("_NumberOfCells", properties);
            cellTransitionSmoothness = FindProperty("_CellTransitionSmoothness", properties);
            sumLightsBeforePosterization = FindProperty("_SumLightsBeforePosterization", properties);
            shadingUseLightColors = FindProperty("_ShadingUseLightColors", properties);

            enableShadows = FindProperty("_EnableShadows", properties);
            //shadowMode = FindProperty("_ShadowMode", properties);
            coreShadowColor = FindProperty("_CoreShadowColor", properties);
            terminatorPosition = FindProperty("_TerminatorPosition", properties);
            terminatorWidth = FindProperty("_TerminatorWidth", properties);
            terminatorSmoothness = FindProperty("_TerminatorSmoothness", properties);
            formShadowColor = FindProperty("_FormShadowColor", properties);
            shadingAffectByNormalMap = FindProperty("_ShadingAffectedByNormalMap", properties);
            enableCastShadows = FindProperty("_EnableCastShadows", properties);
            castShadowsStrength = FindProperty("_CastShadowsStrength", properties);
            castShadowsSmoothness = FindProperty("_CastShadowsSmoothness", properties);
            //customizeCastShadowsColor = FindProperty("_CustomizeCastShadowsColor", properties);
            castShadowColorMode = FindProperty("_CastShadowColorMode", properties);
            castShadowColor = FindProperty("_CastShadowColor", properties);

            enableSpecular = FindProperty("_EnableSpecular", properties);
            specularColor = FindProperty("_SpecularColor", properties);
            specularSize = FindProperty("_SpecularSize", properties);
            specularSmoothness = FindProperty("_SpecularSmoothness", properties);
            specularOpacity = FindProperty("_SpecularOpacity", properties);
            specularAffectedByNormalMap = FindProperty("_SpecularAffectedByNormalMap", properties);
            specularUseLightColors = FindProperty("_SpecularUseLightColors", properties);

            enableRim = FindProperty("_EnableRim", properties);
            rimColor = FindProperty("_RimColor", properties);
            rimSize = FindProperty("_RimSize", properties);
            rimSmoothness = FindProperty("_RimSmoothness", properties);
            rimOpacity = FindProperty("_RimOpacity", properties);
            rimAffectedArea = FindProperty("_RimAffectedArea", properties);
            rimAffectedByNormalMap = FindProperty("_RimAffectedByNormalMap", properties);

            specularBlending = FindProperty("_SpecularBlending", properties);
            rimBlending = FindProperty("_RimBlending", properties);

            #endregion

            //////////////////////////////////////////////////////////////////////////////
            //                              STYLING GENERAL                             //
            //////////////////////////////////////////////////////////////////////////////
#region FindProperties - Styling - Distance Fade
            enableStylingDistanceFade = FindProperty("_EnableStylingDistanceFade", properties);
            stylingDFStartingDistance = FindProperty("_StylingDFStartingDistance", properties);
            stylingDFFalloff = FindProperty("_StylingDFFalloff", properties);
            stylingAdjustDistanceFadeValue = FindProperty("_StylingAdjustDistanceFadeValue", properties);
            stylingDistanceFadeValue = FindProperty("_StylingDistanceFadeValue", properties);
#endregion

            enableStyling = FindProperty("_EnableStyling", properties);

            enableAntiAliasing = FindProperty("_EnableAntiAliasing", properties);



#region FindProperties - Styling - Shading
////////////////////////////////////////////////////////////////////////////////////////////////////
//                                      SHADING STYLING:                                          //
////////////////////////////////////////////////////////////////////////////////////////////////////
            enableShadingStyling = FindProperty("_EnableShadingStyling", properties);
            stylingShadingSyncWithOtherStyling = FindProperty("_StylingShadingSyncWithOtherStyling", properties);


            shadingStyle = FindProperty("_ShadingStyle", properties);

    //PARTITIONING - SHADING STYLING: 
            syncWithLightPartitioning = FindProperty("_SyncWithLightPartitioning", properties);
            numberOfCellsHatching = FindProperty("_NumberOfCellsHatching", properties);
            stylingTerminatorPosition = FindProperty("_StylingTerminatorPosition", properties);
            stylingOvermodelingFactor = FindProperty("_StylingOvermodelingFactor", properties);

    //BLENDING - SHADING STYLING: 
            stylingShadingBlending = FindProperty("_StylingShadingBlending", properties);
            stylingShadingIsInverted = FindProperty("_StylingShadingIsInverted", properties);

    //DRAW SPACE - SHADING STYLING: 
            drawSpace = FindProperty("_DrawSpace", properties);
            coordinateSystem = FindProperty("_CoordinateSystem", properties);
            polarCenterMode = FindProperty("_PolarCenterMode", properties);
            polarCenter = FindProperty("_PolarCenter", properties);
            sSCameraDistanceScaled = FindProperty("_SSCameraDistanceScaled", properties);
            anchorSSToObjectsOrigin = FindProperty("_AnchorSSToObjectsOrigin", properties);

    //DENSITY AND DIRECTION  - SHADING STYLING: 
            stylingShadingDensity = FindProperty("_StylingShadingDensity", properties);
            stylingShadingInitialDirection = FindProperty("_StylingShadingInitialDirection", properties);
            stylingShadingRotationBetweenCells = FindProperty("_StylingShadingRotationBetweenCells", properties);
            stylingShadingHalftonesOffset = FindProperty("_StylingShadingHalftonesOffset", properties);

    //STYLE SETTINGS - SHADING STYLING:
            stylingShadingColor = FindProperty("_StylingColor", properties);
            stylingShadingOpacity = FindProperty("_StylingShadingOpacity", properties);
            stylingShadingOpacityFalloff = FindProperty("_StylingShadingOpacityFalloff", properties);
            stylingShadingThicknessControl = FindProperty("_StylingShadingThicknessControl", properties);
            stylingShadingThickness = FindProperty("_StylingShadingThickness", properties);
            stylingShadingThicknessFalloff = FindProperty("_StylingShadingThicknessFalloff", properties);
            stylingShadingHardness = FindProperty("_StylingShadingHardness", properties);
            stylingShadingHalftonesRoundness = FindProperty("_StylingShadingHalftonesRoundness", properties);
            stylingShadingHalftonesRoundnessFalloff = FindProperty("_StylingShadingHalftonesRoundnessFalloff", properties);

    //RANDOMIZER - SHADING STYLING:
            enableShadingRandomizer = FindProperty("_EnableShadingRandomizer", properties);
            shadingNoise1Size = FindProperty("_ShadingNoise1Size", properties);
            shadingNoise1Seed = FindProperty("_ShadingNoise1Seed", properties);
            shadingNoise2Seed = FindProperty("_ShadingNoise2Seed", properties);
            noiseIntensity = FindProperty("_NoiseIntensity", properties);
            spacingRandomMode = FindProperty("_SpacingRandomMode", properties);
            spacingRandomIntensity = FindProperty("_SpacingRandomIntensity", properties);
            opacityRandomMode = FindProperty("_OpacityRandomMode", properties);
            opacityRandomIntensity = FindProperty("_OpacityRandomIntensity", properties);
            hardnessRandomMode = FindProperty("_HardnessRandomMode", properties);
            hardnessRandomIntensity = FindProperty("_HardnessRandomIntensity", properties);
            lengthRandomMode = FindProperty("_LengthRandomMode", properties);
            lengthRandomIntensity = FindProperty("_LengthRandomIntensity", properties);
            thicknessRandomMode = FindProperty("_ThicknessRandomMode", properties);
            thicknessRandomIntensity = FindProperty("_ThicknesshRandomIntensity", properties);
#endregion

#region FindProperties - Styling - Cast Shadows
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    //                                 CAST SHADOWS STYLING:                                          //
    ////////////////////////////////////////////////////////////////////////////////////////////////////

            enableCastShadowsStyling = FindProperty("_EnableCastShadowsStyling", properties);
            stylingCastShadowsSyncWithOtherStyling = FindProperty("_EnableCastShadowsStyling", properties);

            castShadowsStyle = FindProperty("_CastShadowsStyle", properties);

    //STYLE - CAST SHADOWS STYLING: 
            stylingCastShadowsSyncWithOtherStyling = FindProperty("_StylingCastShadowsSyncWithOtherStyling", properties);

    //PARTITIONING - CAST SHADOWS STYLING: 
            stylingCastShadowsNumberOfCellsHatching = FindProperty("_CastShadowsNumberOfCellsHatching", properties);
            stylingCastShadowsSmoothness = FindProperty("_StylingCastShadowsSmoothness", properties);

    //BLENDING - CAST SHADOWS STYLING: 
            stylingCastShadowsBlending = FindProperty("_StylingCastShadowsBlending", properties);
            stylingCastShadowsIsInverted = FindProperty("_StylingCastShadowsIsInverted", properties);

    //DRAW SPACE - CAST SHADOWS STYLING: 
            castShadowsDrawSpace = FindProperty("_CastShadowsDrawSpace", properties);
            castShadowsSSCameraDistanceScaled = FindProperty("_CastShadowsSSCameraDistanceScaled", properties);
            castShadowsAnchorSSToObjectsOrigin = FindProperty("_CastShadowsAnchorSSToObjectsOrigin", properties);
            castShadowsCoordinateSystem = FindProperty("_CastShadowsCoordinateSystem", properties);
            castShadowsPolarCenterMode = FindProperty("_CastShadowsPolarCenterMode", properties);
            castShadowsPolarCenter = FindProperty("_CastShadowsPolarCenter", properties);

    //DENSITY AND DIRECTION - CAST SHADOWS STYLING: 
            stylingCastShadowsDensity = FindProperty("_StylingCastShadowsDensity", properties);
            stylingCastShadowsInitialDirection = FindProperty("_StylingCastShadowsInitialDirection", properties);
            stylingCastShadowsRotationBetweenCells = FindProperty("_StylingCastShadowsRotationBetweenCells", properties);
            stylingCastShadowsHalftonesOffset = FindProperty("_StylingCastShadowsHalftonesOffset", properties);

    //STYLE SETTINGS - CAST SHADOWS STYLING: 
            stylingCastShadowsColor = FindProperty("_StylingCastShadowsColor", properties);
            stylingCastShadowsOpacity = FindProperty("_StylingCastShadowsOpacity", properties);
            stylingCastShadowsOpacityFalloff = FindProperty("_StylingCastShadowsOpacityFalloff", properties);
            stylingCastShadowsThicknessControl = FindProperty("_StylingCastShadowsThicknessControl", properties);
            stylingCastShadowsThickness = FindProperty("_StylingCastShadowsThickness", properties);
            stylingCastShadowsThicknessFalloff = FindProperty("_StylingCastShadowsThicknessFalloff", properties);
            stylingCastShadowsHardness = FindProperty("_StylingCastShadowsHardness", properties);
            stylingCastShadowsHalftonesRoundness = FindProperty("_StylingCastShadowsHalftonesRoundness", properties);
            stylingCastShadowsHalftonesRoundnessFalloff = FindProperty("_StylingCastShadowsHalftonesRoundnessFalloff", properties);

    //RANDOMIZER - CAST SHADOWS STYLING:
            enableCastShadowsRandomizer = FindProperty("_EnableCastShadowsRandomizer", properties);
            castShadowsNoise1Size = FindProperty("_CastShadowsNoise1Size", properties);
            castShadowsNoise1Seed = FindProperty("_CastShadowsNoise1Seed", properties);
            castShadowsNoise2Seed = FindProperty("_CastShadowsNoise2Seed", properties);
            noiseIntensityCastShadows = FindProperty("_CastShadowsNoiseIntensity", properties);
            spacingRandomModeCastShadows = FindProperty("_CastShadowsSpacingRandomMode", properties);
            spacingRandomIntensityCastShadows = FindProperty("_CastShadowsSpacingRandomIntensity", properties);
            opacityRandomModeCastShadows = FindProperty("_CastShadowsOpacityRandomMode", properties);
            opacityRandomIntensityCastShadows = FindProperty("_CastShadowsOpacityRandomIntensity", properties);
            hardnessRandomModeCastShadows = FindProperty("_CastShadowsHardnessRandomMode", properties);
            hardnessRandomIntensityCastShadows = FindProperty("_CastShadowsHardnessRandomIntensity", properties);
            lengthRandomModeCastShadows = FindProperty("_CastShadowsLengthRandomMode", properties);
            lengthRandomIntensityCastShadows = FindProperty("_CastShadowsLengthRandomIntensity", properties);
            thicknessRandomModeCastShadows = FindProperty("_CastShadowsThicknessRandomMode", properties);
            thicknessRandomIntensityCastShadows = FindProperty("_CastShadowsThicknesshRandomIntensity", properties);
#endregion

#region FindProperties - Styling - Specular
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    //                                 SPECULAR STYLING:                                              //
    ////////////////////////////////////////////////////////////////////////////////////////////////////

            enableSpecularStyling = FindProperty("_EnableSpecularStyling", properties);
            stylingSpecularSyncWithOtherStyling = FindProperty("_StylingSpecularSyncWithOtherStyling", properties);

    //SPECULAR SETTINGS - SPECULAR STYLING: 
            syncWithSpecular = FindProperty("_SyncWithSpecular", properties);
            stylingSpecularSize = FindProperty("_StylingSpecularSize", properties);
            stylingSpecularSmoothness = FindProperty("_StylingSpecularSmoothness", properties);
            stylingSpecularCutOutShading = FindProperty("_StylingSpecularCutOutShading", properties);
            stylingSpecularUseLightColors = FindProperty("_StylingSpecularUseLightColors", properties);

    //STYLE - SPECULAR STYLING: 
            specularStyle = FindProperty("_SpecularStyle", properties);

    //BLENDING - SPECULAR STYLING: 
            stylingSpecularBlending = FindProperty("_StylingSpecularBlending", properties);
            stylingSpecularIsInverted = FindProperty("_StylingSpecularIsInverted", properties);

    //DRAW SPACE - SPECULAR STYLING: 
            specularDrawSpace = FindProperty("_SpecularDrawSpace", properties);
            specularCoordinateSystem = FindProperty("_SpecularCoordinateSystem", properties);
            specularPolarCenterMode = FindProperty("_SpecularPolarCenterMode", properties);
            specularPolarCenter = FindProperty("_SpecularPolarCenter", properties);
            specularSSCameraDistanceScaled = FindProperty("_SpecularSSCameraDistanceScaled", properties);
            specularAnchorSSToObjectsOrigin = FindProperty("_SpecularAnchorSSToObjectsOrigin", properties);

    //DENSITY AND DIRECTION - SPECULAR STYLING:
            stylingSpecularDensity = FindProperty("_StylingSpecularDensity", properties);
            stylingSpecularRotation = FindProperty("_StylingSpecularRotation", properties);
            stylingSpecularHalftonesOffset = FindProperty("_StylingSpecularHalftonesOffset", properties);

    //STYLE SETTINGS - SPECULAR STYLING:
            stylingSpecularColor = FindProperty("_StylingSpecularColor", properties);
            stylingSpecularOpacity = FindProperty("_StylingSpecularOpacity", properties);
            stylingSpecularOpacityFalloff = FindProperty("_StylingSpecularOpacityFalloff", properties);
            stylingSpecularThicknessControl = FindProperty("_StylingSpecularThicknessControl", properties);
            stylingSpecularThickness = FindProperty("_StylingSpecularThickness", properties);
            stylingSpecularThicknessFalloff = FindProperty("_StylingSpecularThicknessFalloff", properties);
            stylingSpecularHardness = FindProperty("_StylingSpecularHardness", properties);
            stylingSpecularHalftonesRoundness = FindProperty("_StylingSpecularHalftonesRoundness", properties);
            stylingSpecularHalftonesRoundnessFalloff = FindProperty("_StylingSpecularHalftonesRoundnessFalloff", properties);

    //RANDOMIZER - SPECULAR STYLING:
            enableSpecularRandomizer = FindProperty("_EnableSpecularRandomizer", properties);
            noise1SizeSpecular = FindProperty("_SpecularNoise1Size", properties);
            noise1SeedSpecular = FindProperty("_SpecularNoise1Seed", properties);
            noise2SeedSpecular = FindProperty("_SpecularNoise2Seed", properties);
            noiseIntensitySpecular = FindProperty("_SpecularNoiseIntensity", properties);
            spacingRandomModeSpecular = FindProperty("_SpecularSpacingRandomMode", properties);
            spacingRandomIntensitySpecular = FindProperty("_SpecularSpacingRandomIntensity", properties);
            opacityRandomModeSpecular = FindProperty("_SpecularOpacityRandomMode", properties);
            opacityRandomIntensitySpecular = FindProperty("_SpecularOpacityRandomIntensity", properties);
            hardnessRandomModeSpecular = FindProperty("_SpecularHardnessRandomMode", properties);
            hardnessRandomIntensitySpecular = FindProperty("_SpecularHardnessRandomIntensity", properties);
            lengthRandomModeSpecular = FindProperty("_SpecularLengthRandomMode", properties);
            lengthRandomIntensitySpecular = FindProperty("_SpecularLengthRandomIntensity", properties);
            thicknessRandomModeSpecular = FindProperty("_SpecularThicknessRandomMode", properties);
            thicknessRandomIntensitySpecular = FindProperty("_SpecularThicknesshRandomIntensity", properties);
#endregion

#region FindProperties - Styling - Rim
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    //                                      RIM STYLING:                                              //
    ////////////////////////////////////////////////////////////////////////////////////////////////////

            enableRimStyling = FindProperty("_EnableRimStyling", properties);
            stylingRimSyncWithOtherStyling = FindProperty("_StylingRimSyncWithOtherStyling", properties);

    //RIM SETTINGS - RIM STYLING:
            syncWithRim = FindProperty("_SyncWithRim", properties);
            stylingRimSize = FindProperty("_StylingRimSize", properties);
            stylingRimSmoothness = FindProperty("_StylingRimSmoothness", properties);
            stylingRimAffectedArea = FindProperty("_StylingRimAffectedArea", properties);

    //STYLE - RIM STYLING: 
            rimStyle = FindProperty("_RimStyle", properties);

    //BLENDING - RIM STYLING: 
            stylingRimBlending = FindProperty("_StylingRimBlending", properties);
            stylingRimIsInverted = FindProperty("_StylingRimIsInverted", properties);

    //DRAW SPACE - RIM STYLING: 
            rimDrawSpace = FindProperty("_RimDrawSpace", properties);
            rimCoordinateSystem = FindProperty("_RimCoordinateSystem", properties);
            rimPolarCenterMode = FindProperty("_RimPolarCenterMode", properties);
            rimPolarCenter = FindProperty("_RimPolarCenter", properties);
            rimSSCameraDistanceScaled = FindProperty("_RimSSCameraDistanceScaled", properties);
            rimAnchorSSToObjectsOrigin = FindProperty("_RimAnchorSSToObjectsOrigin", properties);

    //DENSITY AND DIRECTION - RIM STYLING:
            stylingRimDensity = FindProperty("_StylingRimDensity", properties);
            stylingRimRotation = FindProperty("_StylingRimRotation", properties);
            stylingRimHalftonesOffset = FindProperty("_StylingRimHalftonesOffset", properties);

    //STYLING SETTINGS - RIM STYLING: 
            stylingRimColor = FindProperty("_StylingRimColor", properties);
            stylingRimThicknessControl = FindProperty("_StylingRimThicknessControl", properties);
            stylingRimThickness = FindProperty("_StylingRimThickness", properties);
            stylingRimThicknessFalloff = FindProperty("_StylingRimThicknessFalloff", properties);
            stylingRimOpacity = FindProperty("_StylingRimOpacity", properties);
            stylingRimOpacityFalloff = FindProperty("_StylingRimOpacityFalloff", properties);
            stylingRimHardness = FindProperty("_StylingRimHardness", properties);
            stylingRimHalftonesRoundness = FindProperty("_StylingRimHalftonesRoundness", properties);
            stylingRimHalftonesRoundnessFalloff = FindProperty("_StylingRimHalftonesRoundnessFalloff", properties);

    //RANDOMIZER - RIM STYLING:
            enableRimRandomizer = FindProperty("_EnableRimRandomizer", properties);
            rimNoise1Size = FindProperty("_RimNoise1Size", properties);
            rimNoise1Seed = FindProperty("_RimNoise1Seed", properties);
            rimNoise2Seed = FindProperty("_RimNoise2Seed", properties);
            noiseIntensityRim = FindProperty("_RimNoiseIntensity", properties);
            spacingRandomModeRim = FindProperty("_RimSpacingRandomMode", properties);
            spacingRandomIntensityRim = FindProperty("_RimSpacingRandomIntensity", properties);
            opacityRandomModeRim = FindProperty("_RimOpacityRandomMode", properties);
            opacityRandomIntensityRim = FindProperty("_RimOpacityRandomIntensity", properties);
            hardnessRandomModeRim = FindProperty("_RimHardnessRandomMode", properties);
            hardnessRandomIntensityRim = FindProperty("_RimHardnessRandomIntensity", properties);
            lengthRandomModeRim = FindProperty("_RimLengthRandomMode", properties);
            lengthRandomIntensityRim = FindProperty("_RimLengthRandomIntensity", properties);
            thicknessRandomModeRim = FindProperty("_RimThicknessRandomMode", properties);
            thicknessRandomIntensityRim = FindProperty("_RimThicknesshRandomIntensity", properties);
#endregion


            hatchingAffectedByNormalMap = FindProperty("_HatchingAffectedByNormalMap", properties);

            noiseMap1 = FindProperty("_NoiseMap1", properties);
            noiseMap2 = FindProperty("_NoiseMap2", properties);


            noiseTextureQuality = FindProperty("_NoiseTextureQuality", properties);

            hatchingCameraDistanceFade = FindProperty("_HatchingCameraDistanceFade", properties);
            halftonePatternCameraDistanceFade = FindProperty("_HalftonePatternCameraDistanceFade", properties);

            enableOutline = FindProperty("_EnableOutline", properties);
            outlineColor = FindProperty("_OutlineColor", properties);
            outlineWidth = FindProperty("_OutlineWidth", properties);
            outlineDepthOffset = FindProperty("_OutlineDepthOffset", properties);
            outlineCameraDistanceImpact = FindProperty("_OutlineCameraDistanceImpact", properties);
        }


        public void FindPropertiesBiRPLighting(MaterialProperty[] props)
        {

            // Standard
            biRPLightingGUI.blendMode = FindProperty("_Mode", props);
            biRPLightingGUI.cullMode = FindProperty("_Cull", props);
            //alphaMode = FindProperty("_AlphaMode", props);

            biRPLightingGUI.albedoMap = FindProperty("_MainTex", props);
            biRPLightingGUI.albedoColor = FindProperty("_Color", props);
            biRPLightingGUI.alphaCutoff = FindProperty("_Cutoff", props);

            biRPLightingGUI.metallicMap = FindProperty("_MetallicGlossMap", props);
            biRPLightingGUI.metallic = FindProperty("_Metallic", props);


            biRPLightingGUI.smoothness = FindProperty("_Glossiness", props);
            biRPLightingGUI.smoothnessScale = FindProperty("_GlossMapScale", props, false);
            biRPLightingGUI.smoothnessMapChannel = FindProperty("_SmoothnessTextureChannel", props, false);

            biRPLightingGUI.bumpScale = FindProperty("_BumpScale", props);
            biRPLightingGUI.bumpMap = FindProperty("_BumpMap", props);

            biRPLightingGUI.heigtMapScale = FindProperty("_Parallax", props);
            biRPLightingGUI.heightMap = FindProperty("_ParallaxMap", props);

            biRPLightingGUI.occlusionStrength = FindProperty("_OcclusionStrength", props);
            biRPLightingGUI.occlusionMap = FindProperty("_OcclusionMap", props);

            biRPLightingGUI.emissionMap = FindProperty("_EmissionMap", props);
            biRPLightingGUI.emissionColor = FindProperty("_EmissionColor", props);

            biRPLightingGUI.detailMask = FindProperty("_DetailMask", props);
            biRPLightingGUI.detailAlbedoMap = FindProperty("_DetailAlbedoMap", props);
            biRPLightingGUI.detailNormalMap = FindProperty("_DetailNormalMap", props);
            biRPLightingGUI.detailNormalMapScale = FindProperty("_DetailNormalMapScale", props);
            biRPLightingGUI.uvSetSecondary = FindProperty("_UVSec", props);

        }


        private Shader FindCorrectShader(Material material)
        {
            Shader shader;
            //string ShaderString = ShaderUtils.getUnityVersionAndRenderPipelineCorrectedShaderString().versionAndRPCorrectedShader;
            //string outlineString = "_WithOutline";

            //if (enableOutline.floatValue == 1)
            //{
            //    shader = Shader.Find(ShaderString + outlineString);
            //} 
            //else 
            //{
            //    shader = Shader.Find(ShaderString);
            //}

            Dictionary<string, string> UnityToTTSShaderNameMapping = getUnityToTTSShaderMapping();
            if (!material.HasProperty(TheToonShaderConstants.THETOONSHADER2D_IDENTIFIER_PROPERTY))
            {
#if USING_SEE_THROUGH_SHADER
                if (material.HasProperty(SeeThroughShaderConstants.STS_SHADER_IDENTIFIER_PROPERTY))
                {
                    if (enableOutline.floatValue == 1)
                    {
                        shader = Shader.Find(UnityToTTSShaderNameMapping[TheToonShaderConstants.TTS_SHADER_STS_WITH_OUTLINE_KEY]);
                    }
                    else
                    {
                        shader = Shader.Find(UnityToTTSShaderNameMapping[TheToonShaderConstants.TTS_SHADER_STS_KEY]);
                    }
                }
                else
#endif
                {

                    if (enableOutline.floatValue == 1)
                    {
                        shader = Shader.Find(UnityToTTSShaderNameMapping[TheToonShaderConstants.TTS_SHADER_STANDARD_WITH_OUTLINE_KEY]);
                    }
                    else
                    {
                        shader = Shader.Find(UnityToTTSShaderNameMapping[TheToonShaderConstants.TTS_SHADER_STANDARD_KEY]);
                    }
                }
            }
            else
            {
                shader = Shader.Find(UnityToTTSShaderNameMapping[TheToonShaderConstants.TTS_URP2D_SHADER_STANDARD_KEY]);
            }


            if (shader == null)
            {
                Debug.LogWarning("Correct Toon Shader couldn't be found!");
            }

            //shader = null; // REMOVE

            return shader;
        }

        public void DoSetup(MaterialEditor materialEditor)
        {

            if (EditorGUIUtility.isProSkin)
            {
                textColor = Color.white;
                oriCol = EditorStyles.label.normal.textColor;
            }
            else
            {
                //textColor = EditorStyles.label.normal.textColor;
                textColor = Color.black;
                oriCol = new Color(0.9f, 0.9f, 0.9f, 1);
            }

            presetsAnimBool = new AnimBool(false);
            presetsAnimBool.valueChanged.AddListener(materialEditor.Repaint);

            performanceAnimBool = new AnimBool(false);
            performanceAnimBool.valueChanged.AddListener(materialEditor.Repaint);

            surfaceOptionsAndInputsAnimBool = new AnimBool(false);
            surfaceOptionsAndInputsAnimBool.valueChanged.AddListener(materialEditor.Repaint);

            generalShadingAnimBool = new AnimBool(false);
            generalShadingAnimBool.valueChanged.AddListener(materialEditor.Repaint);

            toonShadingAnimBool = new AnimBool(false);
            toonShadingAnimBool.valueChanged.AddListener(materialEditor.Repaint);

            stylingAnimBool = new AnimBool(false);
            stylingAnimBool.valueChanged.AddListener(materialEditor.Repaint);

            enableStylingAnimBool = new AnimBool(false);
            enableStylingAnimBool.valueChanged.AddListener(materialEditor.Repaint);          

            animationAnimBool = new AnimBool(false);
            animationAnimBool.valueChanged.AddListener(materialEditor.Repaint);

            enableOutlineAnimBool = new AnimBool(false);
            enableOutlineAnimBool.valueChanged.AddListener(materialEditor.Repaint);


            outlineAnimBool = new AnimBool(false);
            outlineAnimBool.valueChanged.AddListener(materialEditor.Repaint);

            GradientSetup();



        }


        BiRPLightingGUI biRPLightingGUI;
#if USING_URP
        LitShader LitShader = new LitShader();
#endif
        protected RenderPipelineOptions rp = RenderPipelineOptions.NONE;

        static string currentShaderName = "";

        bool hasAdditionalLights;

        private bool IsToon2DShader()
        {
            if(((Material)materialEditor.target).HasProperty(TheToonShaderConstants.THETOONSHADER2D_IDENTIFIER_PROPERTY))
            {
                return true;
            }
            return false;
        }
        override public void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
        {
            Material material = materialEditor.target as Material;
            this.materialEditor = materialEditor;


            if (materialEditor.targets.Length > 1)
            {
                EditorGUILayout.HelpBox(
                    "Multi-edit is disabled for this shader. Select a single Material.",
                    MessageType.Info
                );

                return; // URP2D breaks materials when multiple are selected
            }



            if (rp == RenderPipelineOptions.NONE)
            {
                rp = getCurrentRenderPipeline();
            }



            if (rp == RenderPipelineOptions.URP2D && IsToon2DShader())
            {
                EditorUtils.LogoOnlyStart("The Toon Shader 2D - " + TheToonShaderConstants.THETOONSHADER_VERSION_2D);
            }
            else
            {
                EditorUtils.LogoOnlyStart("The Toon Shader 3D - " + TheToonShaderConstants.THETOONSHADER_VERSION_3D);
            }

            if (rp == RenderPipelineOptions.URP2D && !IsToon2DShader())
            {
                EditorGUILayout.Space(10);
                EditorGUILayout.HelpBox("You are trying to use The Toon Shader 3D with URP2D. Please use the URP2D Toon Shader" +
                    " for it to work!", MessageType.Warning);
                EditorGUILayout.Space(30);
                EditorUtils.LogoOnlyEnd();
                return;

            }
            else if (rp != RenderPipelineOptions.URP2D && IsToon2DShader())
            {
                EditorGUILayout.Space(10);
                EditorGUILayout.HelpBox("You are trying to use The Toon Shader 2D in a 3D RenderPipeline. Please use the correct 3D Toon Shader" +
                    " for it to work!", MessageType.Warning);
                EditorGUILayout.Space(30);
                EditorUtils.LogoOnlyEnd();
                return;
            }





            if (rp != RenderPipelineOptions.URP2D && !IsToon2DShader() )
            {
#if USING_URP

                hasAdditionalLights = false;
                foreach (Light addLight in UnityEngine.Object.FindObjectsOfType<Light>())
                {
                    if (!addLight.enabled) continue;
                    if (addLight.type == LightType.Directional) continue;
                    //if (addLight.lightmapBakeType == LightmapBakeType.Baked) continue;

                    hasAdditionalLights = true;
                    break;
                }
#endif
            }
            


            //Debug.Log(rp.ToString());

            if(rp == RenderPipelineOptions.URP2D && IsToon2DShader())
            {              
                FindProperties2D(properties, material);
            }
            else
            {
                FindProperties(properties, material);
            }



            if(currentShaderName != null && currentShaderName != material.shader.name)
            {
                if (material.HasProperty("_IsOptimized"))
                {
                    isOptimized = MaterialEditor.GetMaterialProperty(new UnityEngine.Object[] { material }, "_IsOptimized");
                    currentShaderName = material.shader.name;
                } else
                {
                    isOptimized = null;
                    currentShaderName = material.shader.name;
                }
            } 

            if (m_FirstTimeApply)
            {
                //if (ShaderUtils.IsNativeTTSShader(material.shader))
                //{
                //    isOptimized.floatValue = 0;
                //}
                //else
                //{
                //    if (TheToonShaderGenerator.CheckIfShaderIsAlreadyOptimizedPropertyBased(material))
                //    {
                //        isOptimized.floatValue = 1;
                //    }
                //}

                //if (isOptimized.floatValue == 0)
                //{
                //     originalShaderName.value = material.shader.name;
                //}
                //if(isOptimized.floatValue == 0)

                //if(rp != RenderPipelineOptions.URP2D)
                //{
                    if(!TheToonShaderGenerator.CheckIfShaderIsAlreadyOptimizedPropertyBased(material))
                    {
                        Shader shader = FindCorrectShader(material);
                        if (shader != null)
                        {
                            material.shader = shader;
                        }

                    } else
                    {
                        if (material.HasProperty("_IsOptimized"))
                        {
                            isOptimized = MaterialEditor.GetMaterialProperty(new UnityEngine.Object[] { material }, "_IsOptimized");
                        }
                    }
                //}

                OnOpenGUI(material, materialEditor);
                DoSetup(materialEditor);


                if (rp == RenderPipelineOptions.BiRP)
                {
                    if (biRPLightingGUI == null)
                    {
                        biRPLightingGUI = new BiRPLightingGUI();
                    }
                    FindPropertiesBiRPLighting(properties);
                    biRPLightingGUI.m_MaterialEditor = materialEditor;
                    biRPLightingGUI.DoSetup(materialEditor);

                }



  

                m_FirstTimeApply = false;
            }
            //if (rp != RenderPipelineOptions.URP2D)
            //{
                InitializeToonShaderGeneratorData(rp); // move first time and then do change check
            //}
            if (noiseMap1 != null && noiseMap1.textureValue == null)
            {
                string noiseTexName = "PerlinNoise";
                Texture2D texture = Resources.Load("NoiseTextures/" + noiseTexName, typeof(Texture2D)) as Texture2D;
                SetTextureImporterFormat(texture, true);
                noiseMap1.textureValue = texture;
            }


            if (noiseMap2 != null && noiseMap2.textureValue == null)
            {

                string noiseTexName = "WhiteNoise";
                noiseTexName = "BlueNoise128";
                Texture2D texture = Resources.Load("NoiseTextures/" + noiseTexName, typeof(Texture2D)) as Texture2D;
                SetTextureImporterFormat(texture, true);
                noiseMap2.textureValue = texture;
            }


            DoPerformanceArea(material, properties);

            DoSurfaceInputsAndOptionsArea(material, properties);

            DoToonShadingArea(material);
            DoStylingArea(material);
            //if (rp != RenderPipelineOptions.URP2D)
            //{
            DoOutlineArea(material);
            //}
            //DoAnimationArea(material); // TODO

            EditorUtils.LogoOnlyEnd();

#if USING_SEE_THROUGH_SHADER
            if(material.HasProperty(SeeThroughShaderConstants.STS_SHADER_IDENTIFIER_PROPERTY))
            {
                DrawSeeThroughShaderGUI(properties, material);
            }
#endif
        }

#if USING_SEE_THROUGH_SHADER
        protected MaterialEditor m_MaterialEditorSTS;
        bool m_FirstTimeApplySTS = true;
        protected SeeThroughShaderGUI seeThroughShaderGUI;
        protected Color textColorSTS;
        protected Color originalColorSTS;

        private void DrawSeeThroughShaderGUI(MaterialProperty[] properties, Material material)
        {
            EditorStyles.label.normal.textColor = Color.white;
            m_MaterialEditorSTS = materialEditor;

            if (seeThroughShaderGUI == null)
            {
                seeThroughShaderGUI = new SeeThroughShaderGUI();
            }

            FindPropertiesSeeThroughShader(properties, material);
            seeThroughShaderGUI.m_MaterialEditor = materialEditor;

            if (m_FirstTimeApplySTS)
            {
                DoSetup(materialEditor);
                seeThroughShaderGUI.DoSetup(materialEditor);
                originalColorSTS = EditorStyles.label.normal.textColor;

                if (EditorGUIUtility.isProSkin)
                {
                    textColorSTS = Color.white;
                }
                else
                {
                    textColorSTS = Color.black;
                }
                m_FirstTimeApplySTS = false;
            }

            ShaderCrew.SeeThroughShader.EditorUtils.LogoOnlyStart("The Shader");

            EditorStyles.label.normal.textColor = textColorSTS;

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            GUIStyle STSAreaStyle = new GUIStyle(EditorStyles.helpBox);
            Rect rect = EditorGUILayout.BeginVertical(STSAreaStyle);
            GUI.Box(rect, GUIContent.none);

            seeThroughShaderGUI.STSShaderPropertiesGUI(material);

            EditorGUILayout.EndVertical();

            EditorUtils.LogoOnlyEnd();
            EditorStyles.label.normal.textColor = originalColorSTS;


        }



        public void FindPropertiesSeeThroughShader(MaterialProperty[] props, Material material)
        {
            seeThroughShaderGUI.isReferenceMaterialMat = FindProperty("_isReferenceMaterial", props);


            // See-through Shader
            seeThroughShaderGUI.dissolveMap = FindProperty("_DissolveTex", props);
            seeThroughShaderGUI.dissolveColor = FindProperty("_DissolveColor", props);
            seeThroughShaderGUI.dissolveSize = FindProperty("_UVs", props);
            seeThroughShaderGUI.dissolveColorSaturation = FindProperty("_DissolveColorSaturation", props);

            seeThroughShaderGUI.dissolveEmmission = FindProperty("_DissolveEmission", props);
            seeThroughShaderGUI.dissolveEmmissionBooster = FindProperty("_DissolveEmissionBooster", props);
            seeThroughShaderGUI.dissolveTexturedEmissionEdge = FindProperty("_TexturedEmissionEdge", props);
            seeThroughShaderGUI.dissolveTexturedEmissionEdgeStrength = FindProperty("_TexturedEmissionEdgeStrength", props);

            seeThroughShaderGUI.dissolveClippedShadowsEnabled = FindProperty("_hasClippedShadows", props);


            seeThroughShaderGUI.dissolveTextureAnimationEnabled = FindProperty("_AnimationEnabled", props);
            seeThroughShaderGUI.dissolveTextureAnimationSpeed = FindProperty("_AnimationSpeed", props);
            seeThroughShaderGUI.dissolveTransitionDuration = FindProperty("_TransitionDuration", props);




            seeThroughShaderGUI.interactionMode = FindProperty("_InteractionMode", props);
            //centerPosition = FindProperty("_CenterPosition", props);

            seeThroughShaderGUI.obstructionMode = FindProperty("_Obstruction", props);
            seeThroughShaderGUI.obstructionPlayerOffset = FindProperty("_ObstructionPlayerOffset", props);
            seeThroughShaderGUI.angleStrength = FindProperty("_AngleStrength", props);
            seeThroughShaderGUI.coneStrength = FindProperty("_ConeStrength", props);
            seeThroughShaderGUI.coneObstructionDestroyRadius = FindProperty("_ConeObstructionDestroyRadius", props);

            seeThroughShaderGUI.cylinderStrength = FindProperty("_CylinderStrength", props);
            seeThroughShaderGUI.cylinderObstructionDestroyRadius = FindProperty("_CylinderObstructionDestroyRadius", props);

            seeThroughShaderGUI.circleStrength = FindProperty("_CircleStrength", props);
            seeThroughShaderGUI.circleObstructionDestroyRadius = FindProperty("_CircleObstructionDestroyRadius", props);

            seeThroughShaderGUI.curveStrength = FindProperty("_CurveStrength", props);
            seeThroughShaderGUI.curveObstructionDestroyRadius = FindProperty("_CurveObstructionDestroyRadius", props);

            seeThroughShaderGUI.dissolveObstructionCurve = FindProperty("_ObstructionCurve", props);

            seeThroughShaderGUI.dissolveFallOff = FindProperty("_DissolveFallOff", props);
            seeThroughShaderGUI.dissolveMask = FindProperty("_DissolveMask", props);
            seeThroughShaderGUI.dissolveMaskEnabled = FindProperty("_DissolveMaskEnabled", props);

            seeThroughShaderGUI.affectedAreaPlayerBasedObstruction = FindProperty("_AffectedAreaPlayerBasedObstruction", props);

            seeThroughShaderGUI.intrinsicDissolveStrength = FindProperty("_IntrinsicDissolveStrength", props);


            seeThroughShaderGUI.ceilingEnabled = FindProperty("_Ceiling", props);
            seeThroughShaderGUI.ceilingMode = FindProperty("_CeilingMode", props);
            seeThroughShaderGUI.ceilingBlendMode = FindProperty("_CeilingBlendMode", props);
            seeThroughShaderGUI.ceilingY = FindProperty("_CeilingY", props);
            seeThroughShaderGUI.ceilingPlayerYOffset = FindProperty("_CeilingPlayerYOffset", props);
            seeThroughShaderGUI.ceilingYGradientLength = FindProperty("_CeilingYGradientLength", props);


            seeThroughShaderGUI.isometricExlusionEnabled = FindProperty("_IsometricExclusion", props);
            seeThroughShaderGUI.isometricExclusionDistance = FindProperty("_IsometricExclusionDistance", props);
            seeThroughShaderGUI.isometricExclusionGradientLength = FindProperty("_IsometricExclusionGradientLength", props);

            seeThroughShaderGUI.floorEnabled = FindProperty("_Floor", props);
            seeThroughShaderGUI.floorMode = FindProperty("_FloorMode", props);
            seeThroughShaderGUI.floorY = FindProperty("_FloorY", props);
            seeThroughShaderGUI.playerPosYOffset = FindProperty("_PlayerPosYOffset", props);
            seeThroughShaderGUI.floorYTextureGradientLength = FindProperty("_FloorYTextureGradientLength", props);
            seeThroughShaderGUI.affectedAreaFloor = FindProperty("_AffectedAreaFloor", props);


            seeThroughShaderGUI.zoningEnabled = FindProperty("_Zoning", props);
            seeThroughShaderGUI.zoningMode = FindProperty("_ZoningMode", props);
            seeThroughShaderGUI.zoningEdgeGradientLength = FindProperty("_ZoningEdgeGradientLength", props);
            seeThroughShaderGUI.zoningIsRevealable = FindProperty("_IsZoningRevealable", props);

            seeThroughShaderGUI.zoningSyncZonesWithFloorY = FindProperty("_SyncZonesWithFloorY", props);
            seeThroughShaderGUI.zoningSyncZonesFloorYOffset = FindProperty("_SyncZonesFloorYOffset", props);


            seeThroughShaderGUI.debugModeEnabled = FindProperty("_PreviewMode", props);
            seeThroughShaderGUI.debugModeIndicatorLineThickness = FindProperty("_PreviewIndicatorLineThickness", props);

            seeThroughShaderGUI.isReplacementShader = FindProperty("_IsReplacementShader", props);

            seeThroughShaderGUI.defaultEffectRadius = FindProperty("_DefaultEffectRadius", props);
            seeThroughShaderGUI.enableDefaultEffectRadius = FindProperty("_EnableDefaultEffectRadius", props);


            seeThroughShaderGUI.showContentDissolveArea = FindProperty("_ShowContentDissolveArea", props);
            seeThroughShaderGUI.showContentInteractionOptionsArea = FindProperty("_ShowContentInteractionOptionsArea", props);
            seeThroughShaderGUI.showContentObstructionOptionsArea = FindProperty("_ShowContentObstructionOptionsArea", props);
            seeThroughShaderGUI.showContentAnimationArea = FindProperty("_ShowContentAnimationArea", props);
            seeThroughShaderGUI.showContentZoningArea = FindProperty("_ShowContentZoningArea", props);
            seeThroughShaderGUI.showContentReplacementOptionsArea = FindProperty("_ShowContentReplacementOptionsArea", props);
            seeThroughShaderGUI.showContentDebugArea = FindProperty("_ShowContentDebugArea", props);

            seeThroughShaderGUI.syncCullMode = FindProperty("_SyncCullMode", props);


            seeThroughShaderGUI.useCustomTime = FindProperty("_UseCustomTime", props);


            if (material.HasProperty("_CrossSectionEnabled") && material.HasProperty("_CrossSectionColor") &&
                material.HasProperty("_CrossSectionTextureEnabled") && material.HasProperty("_CrossSectionTexture") &&
                material.HasProperty("_CrossSectionTextureScale") && material.HasProperty("_CrossSectionUVScaledByDistance"))
            {
                seeThroughShaderGUI.crossSectionEnabled = FindProperty("_CrossSectionEnabled", props);
                seeThroughShaderGUI.crossSectionColor = FindProperty("_CrossSectionColor", props);

                seeThroughShaderGUI.crossSectionTextureEnabled = FindProperty("_CrossSectionTextureEnabled", props);
                seeThroughShaderGUI.crossSectionTexture = FindProperty("_CrossSectionTexture", props);
                seeThroughShaderGUI.crossSectionTextureScale = FindProperty("_CrossSectionTextureScale", props);
                seeThroughShaderGUI.crossSectionUVScaledByDistance = FindProperty("_CrossSectionUVScaledByDistance", props);
            }

            seeThroughShaderGUI.dissolveMethod = FindProperty("_DissolveMethod", props);
            seeThroughShaderGUI.dissolveTexSpace = FindProperty("_DissolveTexSpace", props);

#if USING_HDRP
            if (material.HasProperty("_CullMode"))
            {
                seeThroughShaderGUI.cull = FindProperty("_CullMode", props);
            }
#else
            if (material.HasProperty("_Cull"))
            {
                seeThroughShaderGUI.cull = FindProperty("_Cull", props);
            }
#endif

        }
#endif
        public virtual void OnOpenGUI(Material material, MaterialEditor materialEditor)
        {
            // Foldout states
            m_HeaderStateKey = k_KeyPrefix + Application.productName + material.name; // + material.shader.name; // Create key string for editor prefs

            originalShaderName = new SavedString($"{m_HeaderStateKey}.originalShaderName", "");

            m_PerformanceAreaFoldout = new SavedBool($"{m_HeaderStateKey}.m_PerformanceAreaFoldout", true);
            m_SurfaceInputsAndOptionsAreaFoldout = new SavedBool($"{m_HeaderStateKey}.m_SurfaceInputsAndOptionsAreaFoldout", true);

            m_SurfaceOptionsFoldout = new SavedBool($"{m_HeaderStateKey}.SurfaceOptionsFoldout", true);
            m_SurfaceInputsFoldout = new SavedBool($"{m_HeaderStateKey}.SurfaceInputsFoldout", true);

            m_GeneralShadingLightPartitioningFoldout = new SavedBool($"{m_HeaderStateKey}.GeneralShadingLightPartitioningFoldout ", true);
            m_GeneralShadingLightingFoldout = new SavedBool($"{m_HeaderStateKey}.GeneralShadingLightingFoldout ", true);
            m_GeneralShadingShadowFoldout = new SavedBool($"{m_HeaderStateKey}.GeneralShadingShadowFoldout", true);
            m_GeneralShadingCastShadowsFoldout = new SavedBool($"{m_HeaderStateKey}.GeneralShadingCastShadowsFoldout", true);
            m_GeneralShadingSpecularFoldout = new SavedBool($"{m_HeaderStateKey}.GeneralShadingSpecularFoldout", true);
            m_GeneralShadingRimFoldout = new SavedBool($"{m_HeaderStateKey}.GeneralShadingRimFoldout", true);

            m_ToonShadingAreaFoldout = new SavedBool($"{m_HeaderStateKey}.ToonShadingFoldout", true);
            m_StylingAreaFoldout = new SavedBool($"{m_HeaderStateKey}.StylingAreaFoldout", true);
            m_OutlineAreaFoldout = new SavedBool($"{m_HeaderStateKey}.OutlineFoldout", true);

            m_GeneralStylingDistanceFadeFoldout = new SavedBool($"{m_HeaderStateKey}.GeneralStylingDistanceFadeFoldout", true);

            m_StylingShadingFoldout = new SavedBool($"{m_HeaderStateKey}.ShadingStylingShadowFoldout ", true);
            m_ShadingStylingBlendingFoldout = new SavedBool($"{m_HeaderStateKey}.ShadingStylingBlendingFoldout", true);
            m_ShadingStylingDrawSpaceFoldout = new SavedBool($"{m_HeaderStateKey}.ShadingStylingUVSpaceFoldout", true);
            m_ShadingStylingRotationFoldout = new SavedBool($"{m_HeaderStateKey}.ShadingStylingRotationFoldout", true);
            m_ShadingStylingStylingFoldout = new SavedBool($"{m_HeaderStateKey}.ShadingStylingStylingFoldout ", true);
            m_ShadingStylingPartitioningFoldout = new SavedBool($"{m_HeaderStateKey}.ShadingStylingPartitioningFoldout ", true);
            m_ShadingStylingRandomizeFoldout = new SavedBool($"{m_HeaderStateKey}.ShadingStylingRandomizeFoldout ", true);

            m_StylingCastShadowsFoldout = new SavedBool($"{m_HeaderStateKey}.ShadingStylingCastShadowsFoldout", true);
            m_StylingCastShadowsSettingsFoldout = new SavedBool($"{m_HeaderStateKey}.StylingCastShadowsSettingsFoldout ", true);
            m_CastShadowsStylingBlendingFoldout = new SavedBool($"{m_HeaderStateKey}.CastShadowsStylingBlendingFoldout", true);
            m_CastShadowsStylingDrawSpaceFoldout = new SavedBool($"{m_HeaderStateKey}.CastShadowsStylingUVSpaceFoldout", true);
            m_CastShadowsStylingRotationFoldout = new SavedBool($"{m_HeaderStateKey}.CastShadowsStylingRotationFoldout", true);
            m_CastShadowsStylingStylingFoldout = new SavedBool($"{m_HeaderStateKey}.CastShadowsStylingStylingFoldout ", true);
            m_CastShadowsStylingPartitioningFoldout = new SavedBool($"{m_HeaderStateKey}.CastShadowsStylingPartitioningFoldout ", true);
            m_CastShadowsStylingRandomizeFoldout = new SavedBool($"{m_HeaderStateKey}.CastShadowsStylingRandomizeFoldout ", true);

            m_StylingSpecularFoldout = new SavedBool($"{m_HeaderStateKey}.ShadingStylingSpecularFoldout ", true);
            m_StylingSpecularSettingsFoldout = new SavedBool($"{m_HeaderStateKey}.StylingSpecularSettingsFoldout ", true);
            m_SpecularStylingBlendingFoldout = new SavedBool($"{m_HeaderStateKey}.SpecularStylingBlendingFoldout", true);
            m_SpecularStylingDrawSpaceFoldout = new SavedBool($"{m_HeaderStateKey}.SpecularStylingUVSpaceFoldout", true);
            m_SpecularStylingRotationFoldout = new SavedBool($"{m_HeaderStateKey}.SpecularStylingRotationFoldout", true);
            m_SpecularStylingStylingFoldout = new SavedBool($"{m_HeaderStateKey}.SpecularStylingStylingFoldout ", true);
            m_SpecularStylingPartitioningFoldout = new SavedBool($"{m_HeaderStateKey}.SpecularStylingPartitioningFoldout ", true);
            m_SpecularStylingRandomizeFoldout = new SavedBool($"{m_HeaderStateKey}.SpecularStylingRandomizeFoldout ", true);

            m_StylingRimFoldout = new SavedBool($"{m_HeaderStateKey}.ShadingStylingRimFoldout ", true);
            m_StylingRimSettingsFoldout = new SavedBool($"{m_HeaderStateKey}.StylingRimSettingsFoldout ", true);
            m_RimStylingBlendingFoldout = new SavedBool($"{m_HeaderStateKey}.RimStylingBlendingFoldout", true);
            m_RimStylingDrawSpaceFoldout = new SavedBool($"{m_HeaderStateKey}.RimStylingUVSpaceFoldout", true);
            m_RimStylingRotationFoldout = new SavedBool($"{m_HeaderStateKey}.RimStylingRotationFoldout", true);
            m_RimStylingStylingFoldout = new SavedBool($"{m_HeaderStateKey}.RimStylingStylingFoldout ", true);
            m_RimStylingPartitioningFoldout = new SavedBool($"{m_HeaderStateKey}.RimStylingPartitioningFoldout ", true);
            m_RimStylingRandomizeFoldout = new SavedBool($"{m_HeaderStateKey}.RimStylingRandomizeFoldout ", true);


            //m_ShadingStylingStylingToolbar = new SavedInt($"{m_HeaderStateKey}.ShadingStylingStylingToolbar ", 0);

            foreach (var obj in materialEditor.targets)
                MaterialChanged((Material)obj);
        }

        //doesnt work
        //override public void OnClosed(Material material)
        //{
        //    Debug.Log("OnClosed");

        //    //OnClosed(materialEditor.target as Material);
        //}

        public void MaterialChanged(Material material)
        {
            if (material == null)
                throw new ArgumentNullException("material");

            // Clear all keywords for fresh start
            material.shaderKeywords = null;
#if USING_URP
            if (rp == RenderPipelineOptions.URP) 
            {
                if (rp != RenderPipelineOptions.URP2D && !IsToon2DShader())
                {
                    if ((GeneralShadingMode)shadingMode.floatValue != GeneralShadingMode.SolidColor)
                    {
                        if ((LightFunction)lightFunction.floatValue == LightFunction.BlinnPhong)
                        {
                            SimpleLitGUI.SetMaterialKeywords(material);//, SimpleLitGUI.SetMaterialKeywords);

                        }
                        else
                        {
                            LitShader.SetMaterialKeywords(material, LitGUI.SetMaterialKeywords, LitDetailGUI.SetMaterialKeywords);

                        }
                    }
                    SetMaterialKeywordsToon(material);
                    SetMaterialKeywords(material);
                }

            }           

#endif
            if (rp == RenderPipelineOptions.BiRP)
            {
                SetMaterialKeywordsToon(material);

            }



        }

        public void SetMaterialKeywordsToon(Material material)
        {
            SetKeyword(material, "_SHADING_COLOR", false);
            SetKeyword(material, "_SHADING_BLINNPHONG", false);
            SetKeyword(material, "_SHADING_PBR", false);

            if ((GeneralShadingMode)shadingMode.floatValue == GeneralShadingMode.SolidColor)
            {
                SetKeyword(material, "_SHADING_COLOR", true);
            }
            else if ((LightFunction)lightFunction.floatValue == LightFunction.BlinnPhong)
            {
                SetKeyword(material, "_SHADING_BLINNPHONG", true);
            }
            else
            {
                SetKeyword(material, "_SHADING_PBR", true);

            }
        }


        void SetKeyword(Material material, string keyword, bool state)
        {
            if (state)
            {
                material.EnableKeyword(keyword);
            }
            else
            {
                material.DisableKeyword(keyword);
            }

            //Work around for ShaderGraph as ShaderGraph also needs to set floats of associated keywords
            if (material.HasProperty(keyword))
            {
                material.SetFloat(keyword, Convert.ToSingle(state));
            }
        }

#if USING_URP
        //https://github.com/Unity-Technologies/Graphics/blob/v10.10.2/com.unity.render-pipelines.universal/Editor/ShaderGUI/BaseShaderGUI.cs#L392
        public static void SetMaterialKeywords(Material material)
        {
            // Clear all keywords for fresh start
            //material.shaderKeywords = null;

            // Setup blending - consistent across all Universal RP shaders
            SetupMaterialBlendMode(material);  //TODO

            // Receive Shadows
            if (material.HasProperty("_ReceiveShadows"))
                CoreUtils.SetKeyword(material, "_RECEIVE_SHADOWS_OFF", material.GetFloat("_ReceiveShadows") == 0.0f);

            // Emission
            if (material.HasProperty("_EmissionColor"))
                MaterialEditor.FixupEmissiveFlag(material);
            bool shouldEmissionBeEnabled =
                (material.globalIlluminationFlags & MaterialGlobalIlluminationFlags.EmissiveIsBlack) == 0;
            if (material.HasProperty("_EmissionEnabled") && !shouldEmissionBeEnabled)
                shouldEmissionBeEnabled = material.GetFloat("_EmissionEnabled") >= 0.5f;
            CoreUtils.SetKeyword(material, "_EMISSION", shouldEmissionBeEnabled);

            // Normal Map
            if (material.HasProperty("_BumpMap"))
                CoreUtils.SetKeyword(material, "_NORMALMAP", material.GetTexture("_BumpMap"));

            UpdateMaterialSpecularSource(material);
        }

        private static void UpdateMaterialSpecularSource(Material material)
        {
            var opaque = ((UnityEditor.BaseShaderGUI.SurfaceType)material.GetFloat("_Surface") ==
                          UnityEditor.BaseShaderGUI.SurfaceType.Opaque);
            SpecularSource specSource = (SpecularSource)material.GetFloat("_SpecularHighlights");
            if (specSource == SpecularSource.NoSpecular)
            {
                CoreUtils.SetKeyword(material, "_SPECGLOSSMAP", false);
                CoreUtils.SetKeyword(material, "_SPECULAR_COLOR", false);
                CoreUtils.SetKeyword(material, "_GLOSSINESS_FROM_BASE_ALPHA", false);
            }
            else
            {

                var smoothnessSource = (SmoothnessMapChannel)material.GetFloat("_SmoothnessSource");
                bool hasMap = material.GetTexture("_SpecGlossMap");
                CoreUtils.SetKeyword(material, "_SPECGLOSSMAP", hasMap);
                CoreUtils.SetKeyword(material, "_SPECULAR_COLOR", !hasMap);
                if (opaque)
                    CoreUtils.SetKeyword(material, "_GLOSSINESS_FROM_BASE_ALPHA", smoothnessSource == SmoothnessMapChannel.AlbedoAlpha);
                else
                    CoreUtils.SetKeyword(material, "_GLOSSINESS_FROM_BASE_ALPHA", false);

                string color;
                if (smoothnessSource != SmoothnessMapChannel.AlbedoAlpha || !opaque)
                    color = "_SpecColor";
                else
                    color = "_BaseColor";

                var col = material.GetColor(color);
                col.a = material.GetFloat("_Smoothness");
                material.SetColor(color, col);
            }
        }

        public static void SetupMaterialBlendMode(Material material)
        {
            if (material == null)
                throw new ArgumentNullException("material");

            bool alphaClip = false;
            if (material.HasProperty("_AlphaClip"))
                alphaClip = material.GetFloat("_AlphaClip") >= 0.5;

            if (alphaClip)
            {
                material.EnableKeyword("_ALPHATEST_ON");
            }
            else
            {
                material.DisableKeyword("_ALPHATEST_ON");
            }

            if (material.HasProperty("_Surface"))
            {
                SurfaceType surfaceType = (SurfaceType)material.GetFloat("_Surface");
                if (surfaceType == SurfaceType.Opaque)
                {
                    if (alphaClip)
                    {
                        material.renderQueue = (int)RenderQueue.AlphaTest;
                        material.SetOverrideTag("RenderType", "TransparentCutout");
                    }
                    else
                    {
                        material.renderQueue = (int)RenderQueue.Geometry;
                        material.SetOverrideTag("RenderType", "Opaque");
                    }

                    material.renderQueue += material.HasProperty("_QueueOffset") ? (int)material.GetFloat("_QueueOffset") : 0;
                    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                    material.SetInt("_ZWrite", 1);
                    material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.SetShaderPassEnabled("ShadowCaster", true);
                }
                else
                {
                    BlendMode blendMode = (BlendMode)material.GetFloat("_Blend");

                    // Specific Transparent Mode Settings
                    switch (blendMode)
                    {
                        case BlendMode.Alpha:
                            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                            break;
                        case BlendMode.Premultiply:
                            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                            material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                            break;
                        case BlendMode.Additive:
                            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.One);
                            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                            break;
                        case BlendMode.Multiply:
                            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.DstColor);
                            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                            material.EnableKeyword("_ALPHAMODULATE_ON");
                            break;
                    }

                    // General Transparent Material Settings
                    material.SetOverrideTag("RenderType", "Transparent");
                    material.SetInt("_ZWrite", 0);
                    material.renderQueue = (int)RenderQueue.Transparent;
                    material.renderQueue += material.HasProperty("_QueueOffset") ? (int)material.GetFloat("_QueueOffset") : 0;
                    material.SetShaderPassEnabled("ShadowCaster", false);
                }
            }
        }
#endif

        private void DoBasicProModeToggleArea(Material material)
        {
            Rect rect2 = EditorGUILayout.BeginVertical();
            EditorGUILayout.Space(2);
            rect2.width = rect2.width + rect2.x;
            rect2.x = 0;
            if (EditorGUIUtility.isProSkin)
            {
                Color lightBlue = new Color(0.0f, 0.0f, 0.0f, 1);
                EditorUtils.DrawUILine(rect2, lightBlue, 1, 0);
            }
            else
            {
                Color lightBlue = new Color(0.6f, 0.8f, 2f, 1);
                EditorUtils.DrawUILine(rect2, new Color(0.1f, 0.1f, 0.1f, 1f), 2, -8);
                EditorUtils.DrawUILine(rect2, lightBlue, 1, -5);
            }

            if (EditorGUIUtility.isProSkin)
            {
                ToonEditorUtils.DrawBox(rect2, new Color(1.5f, 1.5f,1.5f, 2f));
            }

            shadingMode.floatValue = GUILayout.Toolbar((int)shadingMode.floatValue, new string[] { "Basic Mode", "Pro Mode" });
            EditorGUILayout.Space(1);
            Color lightBlue2 = new Color(0.5f, 0.6f, 0.7f, 1);
            EditorUtils.DrawUILineBottom(rect2, lightBlue2);
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
        }

        //int selectedPreset = 0;
        //Gradient gradient = new Gradient();


        //TODO
        private void DoPresetsArea(Material material)
        {
            //Uncomment for TODO
            ////EditorGUI.indentLevel += 1;
            //showContentAreaPresets.floatValue = Convert.ToSingle(MakeTheToonShaderSectionHeaderWithFoldout("Presets", Convert.ToBoolean(showContentAreaPresets.floatValue)));

            ////EditorGUI.indentLevel -= 1;

            //presetsAnimBool.target = showContentAreaPresets.floatValue == 1;
            //if (EditorGUILayout.BeginFadeGroup(presetsAnimBool.faded))
            //{
            //    EditorGUILayout.LabelField("Coming Soon");
            //    //Rect rect2 = EditorGUILayout.BeginHorizontal();

            //    //EditorGUILayout.LabelField("Select Preset");
            //    //selectedPreset = EditorGUI.Popup(new Rect(rect2.x + rect2.width / 2, rect2.y, rect2.width / 2, rect2.height), selectedPreset, new[] { "a", "b", "c" });
            //    //EditorGUILayout.EndHorizontal();

            //    //GUILayout.Button("Load");

            //    //gradient = EditorGUILayout.GradientField("Gradient", gradient);

            //}
            //EditorGUILayout.EndFadeGroup();
        }



        private void InitializeToonShaderGeneratorData(RenderPipelineOptions rp)
        {
            toonShaderGenerator = new TheToonShaderGenerator();

            toonShaderGenerator._ENABLE_TOON_SHADING = Convert.ToBoolean(enableToonShading.floatValue);

            //if (shadingFunction.floatValue == 0)// test if with int same perf
            //{
            //    toonShaderGenerator._SHADING_FUNCTION_CELLS = true;
            //}
            //else
            //{
            //    toonShaderGenerator._SHADING_FUNCTION_GRADIENT = true;
            //}
            if (enableToonShading.floatValue == 1)
            {
                toonShaderGenerator._SHADING_FUNCTION = (int)shadingFunction.floatValue;

                bool terminatorPositionBool = terminatorPosition.floatValue == 0.0f ? false : true;
                toonShaderGenerator._SHADING_TERMINATORPOSITION = terminatorPositionBool;

                toonShaderGenerator._ENABLE_SHADOWS = Convert.ToBoolean(enableShadows.floatValue);
                if (rp != RenderPipelineOptions.URP2D &&  !IsToon2DShader())
                {
                    toonShaderGenerator._ENABLE_CAST_SHADOWS = Convert.ToBoolean(enableCastShadows.floatValue);
                }
                toonShaderGenerator._ENABLE_SPECULAR = Convert.ToBoolean(enableSpecular.floatValue);
                toonShaderGenerator._SPECULAR_BLENDING = (int)specularBlending.floatValue;
                toonShaderGenerator._ENABLE_RIM = Convert.ToBoolean(enableRim.floatValue);
                toonShaderGenerator._RIM_BLENDING = (int)rimBlending.floatValue;

            }

            if (rp != RenderPipelineOptions.URP2D && !IsToon2DShader())
            { 
                toonShaderGenerator._SUM_LIGHTS_BEFORE_POSTERIZATION = Convert.ToBoolean(sumLightsBeforePosterization.floatValue);
            }

            toonShaderGenerator._SHADING_USE_LIGHT_COLORS = Convert.ToBoolean(shadingUseLightColors.floatValue);
            toonShaderGenerator._SPECULAR_USE_LIGHT_COLORS = Convert.ToBoolean(specularUseLightColors.floatValue);
            toonShaderGenerator._STYLING_SPECULAR_USE_LIGHT_COLORS = Convert.ToBoolean(stylingSpecularUseLightColors.floatValue);


            if (rp != RenderPipelineOptions.URP2D && !IsToon2DShader())
            {
                toonShaderGenerator._ENABLE_STYLING_DISTANCEFADE = Convert.ToBoolean(enableStylingDistanceFade.floatValue);
            }



            toonShaderGenerator._ENABLE_STYLING = Convert.ToBoolean(enableStyling.floatValue);

            toonShaderGenerator._STYLING_CASTSHADOWS_SYNC_WITH_OTHER_STYLING = (int)stylingCastShadowsSyncWithOtherStyling.floatValue;


            toonShaderGenerator._ENABLE_SHADING_STYLING = Convert.ToBoolean(enableShadingStyling.floatValue);
            if (enableShadingStyling.floatValue == 1)
            {
                toonShaderGenerator._SHADING_STYLING_BLENDING = (int)stylingShadingBlending.floatValue;
                toonShaderGenerator._SHADING_STYLING_DRAWSPACE = (int)drawSpace.floatValue;
                toonShaderGenerator._SHADING_STYLING_COORDINATESYSTEM = (int)coordinateSystem.floatValue;
                toonShaderGenerator._SHADING_STYLE = (int)shadingStyle.floatValue;

                bool terminatorPositionBool = stylingTerminatorPosition.floatValue == 0.0f ? false : true;
                toonShaderGenerator._SHADING_STYLING_TERMINATORPOSITION = terminatorPositionBool;

                if (syncWithLightPartitioning.floatValue == 1)
                {
                    toonShaderGenerator._SHADING_STYLING_NUMBER_OF_CELLS_HATCHING = (int)numberOfCells.floatValue;
                }
                else
                {
                    toonShaderGenerator._SHADING_STYLING_NUMBER_OF_CELLS_HATCHING = (int)numberOfCellsHatching.floatValue;
                }

                toonShaderGenerator._SHADING_STYLING_RANDOMIZER = Convert.ToBoolean(enableShadingRandomizer.floatValue);

                if (enableShadingRandomizer.floatValue == 1)
                {
                    if (noiseIntensity.floatValue != 0)
                    {
                        toonShaderGenerator._SHADING_STYLING_RANDOMIZER_PERLIN = true;
                    }

                    if ((thicknessRandomIntensity.floatValue != 0 && thicknessRandomMode.floatValue == 1)
                        || (spacingRandomIntensity.floatValue != 0 && spacingRandomMode.floatValue == 1)
                        || (lengthRandomIntensity.floatValue != 0 && lengthRandomMode.floatValue == 1)
                        || (hardnessRandomIntensity.floatValue != 0 && hardnessRandomMode.floatValue == 1)
                        || (opacityRandomIntensity.floatValue != 0 && opacityRandomMode.floatValue == 1)
                        )
                    {
                        toonShaderGenerator._SHADING_STYLING_RANDOMIZER_PERLIN_FLOORED = true;
                    }

                    if (shadingStyle.floatValue == 0)
                    {
                        if ((thicknessRandomIntensity.floatValue != 0 && (thicknessRandomMode.floatValue == 0 || thicknessRandomMode.floatValue == 2))
                            || (spacingRandomIntensity.floatValue != 0 && (spacingRandomMode.floatValue == 0 || spacingRandomMode.floatValue == 2))
                            || (lengthRandomIntensity.floatValue != 0 && (lengthRandomMode.floatValue == 0 || lengthRandomMode.floatValue == 2))
                            || (hardnessRandomIntensity.floatValue != 0 && (hardnessRandomMode.floatValue == 0 || hardnessRandomMode.floatValue == 2))
                            || (opacityRandomIntensity.floatValue != 0 && (opacityRandomMode.floatValue == 0 || opacityRandomMode.floatValue == 2)))
                        {
                            toonShaderGenerator._SHADING_STYLING_RANDOMIZER_WHITE = true;
                        }
                    }
                    else if (shadingStyle.floatValue == 1)
                    {
                        if (lengthRandomMode.floatValue == 0 || lengthRandomMode.floatValue == 2)
                        {
                            toonShaderGenerator._SHADING_STYLING_RANDOMIZER_WHITE_FLOORED = true;
                        }

                        if ((thicknessRandomIntensity.floatValue != 0 && (thicknessRandomMode.floatValue == 0 || thicknessRandomMode.floatValue == 2))
                            || (spacingRandomIntensity.floatValue != 0 && (spacingRandomMode.floatValue == 0 || spacingRandomMode.floatValue == 2))
                            || (hardnessRandomIntensity.floatValue != 0 && (hardnessRandomMode.floatValue == 0 || hardnessRandomMode.floatValue == 2))
                            || (opacityRandomIntensity.floatValue != 0 && (opacityRandomMode.floatValue == 0 || opacityRandomMode.floatValue == 2)))
                        {
                            toonShaderGenerator._SHADING_STYLING_RANDOMIZER_WHITE = true;
                        }
                    }
                }
            }
            if (rp != RenderPipelineOptions.URP2D && !IsToon2DShader())
            {
                toonShaderGenerator._ENABLE_CASTSHADOWS_STYLING = Convert.ToBoolean(enableCastShadowsStyling.floatValue);
                if (enableCastShadowsStyling.floatValue == 1)
                {
                    toonShaderGenerator._CASTSHADOWS_STYLING_BLENDING = (int)stylingCastShadowsBlending.floatValue;
                    toonShaderGenerator._CASTSHADOWS_STYLING_DRAWSPACE = (int)castShadowsDrawSpace.floatValue;
                    toonShaderGenerator._CASTSHADOWS_STYLING_COORDINATESYSTEM = (int)castShadowsCoordinateSystem.floatValue;
                    toonShaderGenerator._CASTSHADOWS_STYLE = (int)castShadowsStyle.floatValue;

                    toonShaderGenerator._CASTSHADOWS_STYLING_NUMBER_OF_CELLS_HATCHING = (int)stylingCastShadowsNumberOfCellsHatching.floatValue;


                    toonShaderGenerator._CASTSHADOWS_STYLING_RANDOMIZER = Convert.ToBoolean(enableCastShadowsRandomizer.floatValue);


                    if (enableCastShadowsRandomizer.floatValue == 1)
                    {
                        if (noiseIntensityCastShadows.floatValue != 0)
                        {
                            toonShaderGenerator._CASTSHADOWS_STYLING_RANDOMIZER_PERLIN = true;
                        }

                        if ((thicknessRandomIntensityCastShadows.floatValue != 0 && thicknessRandomModeCastShadows.floatValue == 1)
                            || (spacingRandomIntensityCastShadows.floatValue != 0 && spacingRandomModeCastShadows.floatValue == 1)
                            || (lengthRandomIntensityCastShadows.floatValue != 0 && lengthRandomModeCastShadows.floatValue == 1)
                            || (hardnessRandomIntensityCastShadows.floatValue != 0 && hardnessRandomModeCastShadows.floatValue == 1)
                            || (opacityRandomIntensityCastShadows.floatValue != 0 && opacityRandomModeCastShadows.floatValue == 1)
                            )
                        {
                            toonShaderGenerator._CASTSHADOWS_STYLING_RANDOMIZER_PERLIN_FLOORED = true;
                        }

                        if (castShadowsStyle.floatValue == 0)
                        {
                            if ((thicknessRandomIntensityCastShadows.floatValue != 0 && (thicknessRandomModeCastShadows.floatValue == 0 || thicknessRandomModeCastShadows.floatValue == 2))
                                || (spacingRandomIntensityCastShadows.floatValue != 0 && (spacingRandomModeCastShadows.floatValue == 0 || spacingRandomModeCastShadows.floatValue == 2))
                                || (lengthRandomIntensityCastShadows.floatValue != 0 && (lengthRandomModeCastShadows.floatValue == 0 || lengthRandomModeCastShadows.floatValue == 2))
                                || (hardnessRandomIntensityCastShadows.floatValue != 0 && (hardnessRandomModeCastShadows.floatValue == 0 || hardnessRandomModeCastShadows.floatValue == 2))
                                || (opacityRandomIntensityCastShadows.floatValue != 0 && (opacityRandomModeCastShadows.floatValue == 0 || opacityRandomModeCastShadows.floatValue == 2)))
                            {
                                toonShaderGenerator._CASTSHADOWS_STYLING_RANDOMIZER_WHITE = true;
                            }
                        }
                        else if (castShadowsStyle.floatValue == 1)
                        {
                            if (lengthRandomModeCastShadows.floatValue == 0 || lengthRandomModeCastShadows.floatValue == 2)
                            {
                                toonShaderGenerator._CASTSHADOWS_STYLING_RANDOMIZER_WHITE_FLOORED = true;
                            }

                            if ((thicknessRandomIntensityCastShadows.floatValue != 0 && (thicknessRandomModeCastShadows.floatValue == 0 || thicknessRandomModeCastShadows.floatValue == 2))
                                || (spacingRandomIntensityCastShadows.floatValue != 0 && (spacingRandomModeCastShadows.floatValue == 0 || spacingRandomModeCastShadows.floatValue == 2))
                                || (hardnessRandomIntensityCastShadows.floatValue != 0 && (hardnessRandomModeCastShadows.floatValue == 0 || hardnessRandomModeCastShadows.floatValue == 2))
                                || (opacityRandomIntensityCastShadows.floatValue != 0 && (opacityRandomModeCastShadows.floatValue == 0 || opacityRandomModeCastShadows.floatValue == 2)))
                            {
                                toonShaderGenerator._CASTSHADOWS_STYLING_RANDOMIZER_WHITE = true;
                            }
                        }
                    }
                }

            }

            toonShaderGenerator._ENABLE_SPECULAR_STYLING = Convert.ToBoolean(enableSpecularStyling.floatValue);
            if (enableSpecularStyling.floatValue == 1)
            {
                toonShaderGenerator._SPECULAR_STYLING_BLENDING = (int)stylingSpecularBlending.floatValue;
                toonShaderGenerator._SPECULAR_STYLING_DRAWSPACE = (int)specularDrawSpace.floatValue;
                toonShaderGenerator._SPECULAR_STYLING_COORDINATESYSTEM = (int)specularCoordinateSystem.floatValue;
                toonShaderGenerator._SPECULAR_STYLE = (int)specularStyle.floatValue;

                toonShaderGenerator._SPECULAR_STYLING_RANDOMIZER = Convert.ToBoolean(enableSpecularRandomizer.floatValue);


                if (enableSpecularRandomizer.floatValue == 1)
                {
                    if (noiseIntensitySpecular.floatValue != 0)
                    {
                        toonShaderGenerator._SPECULAR_STYLING_RANDOMIZER_PERLIN = true;
                    }

                    if ((thicknessRandomIntensitySpecular.floatValue != 0 && thicknessRandomModeSpecular.floatValue == 1)
                        || (spacingRandomIntensitySpecular.floatValue != 0 && spacingRandomModeSpecular.floatValue == 1)
                        || (lengthRandomIntensitySpecular.floatValue != 0 && lengthRandomModeSpecular.floatValue == 1)
                        || (hardnessRandomIntensitySpecular.floatValue != 0 && hardnessRandomModeSpecular.floatValue == 1)
                        || (opacityRandomIntensitySpecular.floatValue != 0 && opacityRandomModeSpecular.floatValue == 1)
                        )
                    {
                        toonShaderGenerator._SPECULAR_STYLING_RANDOMIZER_PERLIN_FLOORED = true;
                    }

                    if (specularStyle.floatValue == 0)
                    {
                        if ((thicknessRandomIntensitySpecular.floatValue != 0 && (thicknessRandomModeSpecular.floatValue == 0 || thicknessRandomModeSpecular.floatValue == 2))
                            || (spacingRandomIntensitySpecular.floatValue != 0 && (spacingRandomModeSpecular.floatValue == 0 || spacingRandomModeSpecular.floatValue == 2))
                            || (lengthRandomIntensitySpecular.floatValue != 0 && (lengthRandomModeSpecular.floatValue == 0 || lengthRandomModeSpecular.floatValue == 2))
                            || (hardnessRandomIntensitySpecular.floatValue != 0 && (hardnessRandomModeSpecular.floatValue == 0 || hardnessRandomModeSpecular.floatValue == 2))
                            || (opacityRandomIntensitySpecular.floatValue != 0 && (opacityRandomModeSpecular.floatValue == 0 || opacityRandomModeSpecular.floatValue == 2)))
                        {
                            toonShaderGenerator._SPECULAR_STYLING_RANDOMIZER_WHITE = true;
                        }
                    }
                    else if (specularStyle.floatValue == 1)
                    {
                        if (lengthRandomModeSpecular.floatValue == 0 || lengthRandomModeSpecular.floatValue == 2)
                        {
                            toonShaderGenerator._SPECULAR_STYLING_RANDOMIZER_WHITE_FLOORED = true;
                        }

                        if ((thicknessRandomIntensitySpecular.floatValue != 0 && (thicknessRandomModeSpecular.floatValue == 0 || thicknessRandomModeSpecular.floatValue == 2))
                            || (spacingRandomIntensitySpecular.floatValue != 0 && (spacingRandomModeSpecular.floatValue == 0 || spacingRandomModeSpecular.floatValue == 2))
                            || (hardnessRandomIntensitySpecular.floatValue != 0 && (hardnessRandomModeSpecular.floatValue == 0 || hardnessRandomModeSpecular.floatValue == 2))
                            || (opacityRandomIntensitySpecular.floatValue != 0 && (opacityRandomModeSpecular.floatValue == 0 || opacityRandomModeSpecular.floatValue == 2)))
                        {
                            toonShaderGenerator._SPECULAR_STYLING_RANDOMIZER_WHITE = true;
                        }
                    }
                }
            }



            toonShaderGenerator._ENABLE_RIM_STYLING = Convert.ToBoolean(enableRimStyling.floatValue);

            if (enableRimStyling.floatValue == 1)
            {
                toonShaderGenerator._RIM_STYLING_BLENDING = (int)stylingRimBlending.floatValue;
                toonShaderGenerator._RIM_STYLING_DRAWSPACE = (int)rimDrawSpace.floatValue;
                toonShaderGenerator._RIM_STYLING_COORDINATESYSTEM = (int)rimCoordinateSystem.floatValue;
                toonShaderGenerator._RIM_STYLE = (int)rimStyle.floatValue;

                toonShaderGenerator._RIM_STYLING_RANDOMIZER = Convert.ToBoolean(enableRimRandomizer.floatValue);


                if (enableRimRandomizer.floatValue == 1)
                {
                    if (noiseIntensityRim.floatValue != 0)
                    {
                        toonShaderGenerator._RIM_STYLING_RANDOMIZER_PERLIN = true;
                    }

                    if ((thicknessRandomIntensityRim.floatValue != 0 && thicknessRandomModeRim.floatValue == 1)
                        || (spacingRandomIntensityRim.floatValue != 0 && spacingRandomModeRim.floatValue == 1)
                        || (lengthRandomIntensityRim.floatValue != 0 && lengthRandomModeRim.floatValue == 1)
                        || (hardnessRandomIntensityRim.floatValue != 0 && hardnessRandomModeRim.floatValue == 1)
                        || (opacityRandomIntensityRim.floatValue != 0 && opacityRandomModeRim.floatValue == 1)
                        )
                    {
                        toonShaderGenerator._RIM_STYLING_RANDOMIZER_PERLIN_FLOORED = true;
                    }

                    if (rimStyle.floatValue == 0)
                    {
                        if ((thicknessRandomIntensityRim.floatValue != 0 && (thicknessRandomModeRim.floatValue == 0 || thicknessRandomModeRim.floatValue == 2))
                            || (spacingRandomIntensityRim.floatValue != 0 && (spacingRandomModeRim.floatValue == 0 || spacingRandomModeRim.floatValue == 2))
                            || (lengthRandomIntensityRim.floatValue != 0 && (lengthRandomModeRim.floatValue == 0 || lengthRandomModeRim.floatValue == 2))
                            || (hardnessRandomIntensityRim.floatValue != 0 && (hardnessRandomModeRim.floatValue == 0 || hardnessRandomModeRim.floatValue == 2))
                            || (opacityRandomIntensityRim.floatValue != 0 && (opacityRandomModeRim.floatValue == 0 || opacityRandomModeRim.floatValue == 2)))
                        {
                            toonShaderGenerator._RIM_STYLING_RANDOMIZER_WHITE = true;
                        }
                    }
                    else if (rimStyle.floatValue == 1)
                    {
                        if (lengthRandomModeRim.floatValue == 0 || lengthRandomModeRim.floatValue == 2)
                        {
                            toonShaderGenerator._RIM_STYLING_RANDOMIZER_WHITE_FLOORED = true;
                        }

                        if ((thicknessRandomIntensityRim.floatValue != 0 && (thicknessRandomModeRim.floatValue == 0 || thicknessRandomModeRim.floatValue == 2))
                            || (spacingRandomIntensityRim.floatValue != 0 && (spacingRandomModeRim.floatValue == 0 || spacingRandomModeRim.floatValue == 2))
                            || (hardnessRandomIntensityRim.floatValue != 0 && (hardnessRandomModeRim.floatValue == 0 || hardnessRandomModeRim.floatValue == 2))
                            || (opacityRandomIntensityRim.floatValue != 0 && (opacityRandomModeRim.floatValue == 0 || opacityRandomModeRim.floatValue == 2)))
                        {
                            toonShaderGenerator._RIM_STYLING_RANDOMIZER_WHITE = true;
                        }
                    }
                }

            }


            toonShaderGenerator._ENABLE_OUTLINE = Convert.ToBoolean(enableOutline.floatValue);

#if USING_URP2D
            if(IsToon2DShader())
            {
                toonShaderGenerator._MAX_LIGHT_COUNT = (int)maxLightCount.floatValue;
                toonShaderGenerator._CELL_METHOD = (int)cellMethod.floatValue;
                toonShaderGenerator._ENABLE_MAINTEX_POSTERIZATION = Convert.ToBoolean(enableMainTexPosterization.floatValue);

                toonShaderGenerator._EMISSION = Convert.ToBoolean(enableEmission.floatValue);
                toonShaderGenerator._NORMALMAP = Convert.ToBoolean(enableNormalMap.floatValue);

                toonShaderGenerator._USE_MAINTEX = Convert.ToBoolean(useMainTex.floatValue);

                toonShaderGenerator._CONVERT_NORMAL_TO_ALBEDO = Convert.ToBoolean(convertNormalToAlbedo.floatValue) && Convert.ToBoolean(normalToAlbedoConversionVector.vectorValue.w != 0);

                toonShaderGenerator._USE_CORE_SHADOW_COLOR = Convert.ToBoolean(useCoreShadowColor.floatValue);
                toonShaderGenerator._ENABLE_LIGHT_PARTITIONING = Convert.ToBoolean(enableLightPartitioning.floatValue);
                toonShaderGenerator._ROUND_METHOD = (int)roundingMethod.floatValue;
            }
#endif


        }


        private void DoPerformanceArea(Material material, MaterialProperty[] properties)
        {


            m_PerformanceAreaFoldout.value = ToonEditorUtils.MakeTheToonShaderSectionHeaderWithFoldout("Performance", m_PerformanceAreaFoldout.value);

            performanceAnimBool.target = m_PerformanceAreaFoldout.value == true;
            if (EditorGUILayout.BeginFadeGroup(performanceAnimBool.faded))
            {


                bool is2022OrNewer = false;

#if UNITY_2022_1_OR_NEWER
                is2022OrNewer = true;
#endif
                if (is2022OrNewer || !IsToon2DShader())
                {

                    //if (isOptimized.floatValue == 0)
                    if (!TheToonShaderGenerator.CheckIfShaderIsAlreadyOptimizedPropertyBased(material))
                    {
                        GUIStyle labelStyle = new GUIStyle(EditorStyles.wordWrappedLabel);
                        labelStyle.richText = true;
                        string label = "The shader of this material is currently <b>not optimized</b>. To get the most performance," +
                            " you can press \"Optimize\". This will generate a shader fitting this material's property settings.";
                        EditorGUILayout.LabelField(label, labelStyle);
                        EditorGUILayout.Space(2);

                        if (GUILayout.Button("Optimize"))
                        {
                            //toonShaderGenerator = new TheToonShaderGenerator();
                            if (toonShaderGenerator != null && ShaderUtils.IsNativeTTSShader(material.shader))
                            {
                                Shader shader = toonShaderGenerator.GenerateOptimizedShaderFile(material);
                                if (shader != null)
                                {
                                    material.shader = shader;

                                    //isOptimized.floatValue = 1;
                                    if (material.HasProperty("_IsOptimized"))
                                    {
                                        isOptimized = MaterialEditor.GetMaterialProperty(new UnityEngine.Object[] { material }, "_IsOptimized");
                                    }

                                }
                            }
                            else
                            {
                                string logString = "";
                                logString = "Material \"" + material.name + "\" with shader \"" + material.shader + "\" couldn't be optimized. Reason: ";
                                if (toonShaderGenerator == null)
                                {
                                    logString += "ToonShaderGenerator is null. ";
                                }
                                if (!ShaderUtils.IsNativeTTSShader(material.shader))
                                {
                                    logString += material.shader + " is not a native shader";
                                }
                                Debug.Log(logString);
                            }


                        }


                    }
                    else
                    {
                        GUIStyle labelStyle = new GUIStyle(EditorStyles.wordWrappedLabel);
                        labelStyle.richText = true;
                        EditorGUILayout.LabelField("The shader " + material.shader.name + " is optimized for the selected property values." +
                            "\n <b>To be able to change all the settings you have to change back to the unoptimized shader!</b>", labelStyle);
                        EditorGUILayout.Space(2);
                        if (GUILayout.Button("Change back to original shader"))
                        {
                            isOptimized = null;


                            // doesnt work if shader is optimized from the beginning
                            //if (originalShaderName != null && originalShaderName.value != null && !originalShaderName.value.Equals(""))
                            //{
                            //    Shader originalShader = Shader.Find(originalShaderName.value);
                            //    if (originalShader != null)
                            //    {
                            //        material.shader = originalShader;
                            //    }
                            //}
                            // safer?


                            Shader originalShader = FindCorrectShader(material);
                            if (originalShader != null)
                            {
                                material.shader = originalShader;
                            }

                        }
                    }
                    DisableGUIIfOptimizedShader();


                    if (IsToon2DShader())
                    {
                        maxLightCount.floatValue = maxLightCount.floatValue = EditorGUILayout.IntSlider("Maximum Light Count", (int)maxLightCount.floatValue, 1, 32);
                    }
                    //if (EditorGUI.EndChangeCheck())
                    //{
                    //}


                    EditorGUI.BeginChangeCheck();
                    noiseTextureQuality.floatValue = (int)(TextureQuality)EditorGUILayout.EnumPopup("Noise Texture Quality", (TextureQuality)noiseTextureQuality.floatValue);
                    if (EditorGUI.EndChangeCheck())
                    {
                        string quality = "";
                        if ((TextureQuality)noiseTextureQuality.floatValue == TextureQuality.High)
                        {
                            quality = "_High";
                        }
                        else if ((TextureQuality)noiseTextureQuality.floatValue == TextureQuality.Mid)
                        {
                            quality = "_Medium";
                        }
                        else if ((TextureQuality)noiseTextureQuality.floatValue == TextureQuality.Low)
                        {
                            quality = "_Low";
                        }
                        else
                        {
                            quality = "_UltraLow";
                        }

                        string perlinNoiseTexName = "PerlinNoise" + quality;
                        Texture2D texturePerlin = Resources.Load("NoiseTextures/" + perlinNoiseTexName, typeof(Texture2D)) as Texture2D;
                        SetTextureImporterFormat(texturePerlin, true);
                        noiseMap1.textureValue = texturePerlin;


                        //string whiteNoiseTexName = "WhiteNoise" + quality;
                        //Texture2D texture = Resources.Load("NoiseTextures/" + whiteNoiseTexName, typeof(Texture2D)) as Texture2D;
                        //SetTextureImporterFormat(texture, true);
                        //noiseMap2.textureValue = texture;
                    }


                }
                else
                {
                    GUIStyle labelStyle = new GUIStyle(EditorStyles.wordWrappedLabel);
                    labelStyle.richText = true;
                    EditorGUILayout.LabelField("The Optimizer Feature currently only works with Unity 2022 and above!", labelStyle);
                }


            }
            GUI.enabled = true;


                EditorGUILayout.EndFadeGroup();
        }



        #region SurfaceOptionsAndInputFunctions
        //////////////////////////////////////////
        // SurfaceOptionsAndInputFunctions Main //
        //////////////////////////////////////////

#if USING_URP
        // SurfaceOptionsAndInputFunctions Helper //
        #region SurfaceOptionsAndInputFunctions Helper
        public void DoPopup(GUIContent label, MaterialProperty property, string[] options)
        {
            DoPopup(label, property, options, materialEditor);
        }

        public static void DoPopup(GUIContent label, MaterialProperty property, string[] options, MaterialEditor materialEditor)
        {
            if (property == null)
                throw new ArgumentNullException("property");

            EditorGUI.showMixedValue = property.hasMixedValue;

            var mode = property.floatValue;
            EditorGUI.BeginChangeCheck();
            mode = EditorGUILayout.Popup(label, (int)mode, options);
            if (EditorGUI.EndChangeCheck())
            {
                materialEditor.RegisterPropertyChangeUndo(label.text);
                property.floatValue = mode;
            }

            EditorGUI.showMixedValue = false;
        }

        public virtual void DrawSurfaceOptions(Material material)
        {
            // Detect any changes to the material
            EditorGUI.BeginChangeCheck();
            {
                GUI.enabled = false; // TODO    
                DoPopup(Styles.surfaceType, surfaceTypeProp, Enum.GetNames(typeof(SurfaceType)));
                if ((SurfaceType)material.GetFloat("_Surface") == SurfaceType.Transparent)
                    DoPopup(Styles.blendingMode, blendModeProp, Enum.GetNames(typeof(BlendMode)));


                //In case the material was transparent beforehand
                if ((SurfaceType)material.GetFloat("_Surface") == SurfaceType.Transparent)
                {
                    material.SetFloat("_Surface", (float)SurfaceType.Opaque);
                    material.DisableKeyword("_SURFACE_TYPE_TRANSPARENT");
                    surfaceTypeProp.floatValue = 0.0f;

                    SetupMaterialBlendMode(material);

                }
                GUI.enabled = true;

                EditorGUI.BeginChangeCheck();
                EditorGUI.showMixedValue = cullingProp.hasMixedValue;
                var culling = (RenderFace)cullingProp.floatValue;
                culling = (RenderFace)EditorGUILayout.EnumPopup(Styles.cullingText, culling);
                if (EditorGUI.EndChangeCheck())
                {
                    materialEditor.RegisterPropertyChangeUndo(Styles.cullingText.text);
                    cullingProp.floatValue = (float)culling;
                    material.doubleSidedGI = (RenderFace)cullingProp.floatValue != RenderFace.Front;
                }

                EditorGUI.showMixedValue = false;

                EditorGUI.BeginChangeCheck();
                EditorGUI.showMixedValue = alphaClipProp.hasMixedValue;
                var alphaClipEnabled = EditorGUILayout.Toggle(Styles.alphaClipText, alphaClipProp.floatValue == 1);
                if (EditorGUI.EndChangeCheck())
                    alphaClipProp.floatValue = alphaClipEnabled ? 1 : 0;
                EditorGUI.showMixedValue = false;

                if (alphaClipProp.floatValue == 1)
                    materialEditor.ShaderProperty(alphaCutoffProp, Styles.alphaClipThresholdText, 1);

                if (receiveShadowsProp != null)
                {
                    EditorGUI.BeginChangeCheck();
                    EditorGUI.showMixedValue = receiveShadowsProp.hasMixedValue;
                    var receiveShadows =
                        EditorGUILayout.Toggle(Styles.receiveShadowText, receiveShadowsProp.floatValue == 1.0f);
                    if (EditorGUI.EndChangeCheck())
                        receiveShadowsProp.floatValue = receiveShadows ? 1.0f : 0.0f;
                    EditorGUI.showMixedValue = false;
                }
            }
            if (EditorGUI.EndChangeCheck())
            {
                foreach (var obj in blendModeProp.targets)
                    MaterialChanged((Material)obj);
            }
         }


        public virtual void DrawBaseProperties(Material material)
        {
            if (baseMapProp != null && baseColorProp != null) // Draw the baseMap, most shader will have at least a baseMap
            {
                materialEditor.TexturePropertySingleLine(Styles.baseMap, baseMapProp, baseColorProp);
                //// TODO Temporary fix for lightmapping, to be replaced with attribute tag.
                //if (material.HasProperty("_MainTex"))
                //{
                //    material.SetTexture("_MainTex", baseMapProp.textureValue);
                //    var baseMapTiling = baseMapProp.textureScaleAndOffset;
                //    material.SetTextureScale("_MainTex", new Vector2(baseMapTiling.x, baseMapTiling.y));
                //    material.SetTextureOffset("_MainTex", new Vector2(baseMapTiling.z, baseMapTiling.w));
                //}
            }
        }

        private void DrawNormalArea()
        {
            if (bumpScaleProp != null)
            {
                materialEditor.TexturePropertySingleLine(Styles.normalMapText, bumpMapProp, (bumpMapProp.textureValue != null) ? bumpScaleProp : null);
                if (bumpScaleProp.floatValue != 1f && InternalEditorUtility.IsMobilePlatform(EditorUserBuildSettings.activeBuildTarget) && materialEditor.HelpBoxWithButton(Styles.bumpScaleNotSupported, Styles.fixNormalNow))
                {
                    bumpScaleProp.floatValue = 1f;
                }
            }
            else
            {
                materialEditor.TexturePropertySingleLine(Styles.normalMapText, bumpMapProp);
            }
        }

        protected virtual void DrawEmissionProperties(Material material, bool keyword)
        {
            var emissive = true;
            var hadEmissionTexture = emissionMapProp.textureValue != null;

            if (!keyword)
            {
                materialEditor.TexturePropertyWithHDRColor(Styles.emissionMap, emissionMapProp, emissionColorProp,
                    false);
            }
            else
            {
                // Emission for GI?
                emissive = materialEditor.EmissionEnabledProperty();

                EditorGUI.BeginDisabledGroup(!emissive);
                {
                    // Texture and HDR color controls
                    materialEditor.TexturePropertyWithHDRColor(Styles.emissionMap, emissionMapProp,
                        emissionColorProp,
                        false);
                }
                EditorGUI.EndDisabledGroup();
            }

            // If texture was assigned and color was black set color to white
            var brightness = emissionColorProp.colorValue.maxColorComponent;
            if (emissionMapProp.textureValue != null && !hadEmissionTexture && brightness <= 0f)
                emissionColorProp.colorValue = Color.white;

            // UniversalRP does not support RealtimeEmissive. We set it to bake emissive and handle the emissive is black right.
            if (emissive)
            {
                var oldFlags = material.globalIlluminationFlags;
                var newFlags = MaterialGlobalIlluminationFlags.BakedEmissive;

                if (brightness <= 0f)
                    newFlags |= MaterialGlobalIlluminationFlags.EmissiveIsBlack;

                if (newFlags != oldFlags)
                    material.globalIlluminationFlags = newFlags;
            }
        }
        protected static void DrawTileOffset(MaterialEditor materialEditor, MaterialProperty textureProp)
        {
            materialEditor.TextureScaleOffsetProperty(textureProp);
        }


        public static void DoSpecularAreaBlinnPhong(SimpleLitProperties properties, MaterialEditor materialEditor, Material material)
        {
            SpecularSource specSource = (SpecularSource)properties.specHighlights.floatValue;
            EditorGUI.BeginDisabledGroup(specSource == SpecularSource.NoSpecular);
            UnityEditor.BaseShaderGUI.TextureColorProps(materialEditor, Styles.specularMapText, properties.specGlossMap, properties.specColor, true);
#if !UNITY_2021_1_OR_NEWER
            DoSmoothness(properties, material);
#else
            LitGUI.DoSmoothness(materialEditor, material, properties.smoothness, properties.smoothnessMapChannel, LitGUI.Styles.specularSmoothnessChannelNames);
#endif
            //DoSmoothness(properties, material);
            EditorGUI.EndDisabledGroup();
        }

#if !UNITY_2021_1_OR_NEWER

        public static void DoSmoothness(SimpleLitProperties properties, Material material)
        {
            var opaque = ((BaseShaderGUI.SurfaceType)material.GetFloat("_Surface") ==
                          BaseShaderGUI.SurfaceType.Opaque);
            EditorGUI.indentLevel += 2;

            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = properties.smoothness.hasMixedValue;
            var smoothnessSource = (int)properties.smoothnessMapChannel.floatValue;
            var smoothness = properties.smoothness.floatValue;
            smoothness = EditorGUILayout.Slider(Styles.smoothnessText, smoothness, 0f, 1f);
            if (EditorGUI.EndChangeCheck())
            {
                properties.smoothness.floatValue = smoothness;
            }
            EditorGUI.showMixedValue = false;

            EditorGUI.indentLevel++;
            EditorGUI.BeginDisabledGroup(!opaque);
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = properties.smoothnessMapChannel.hasMixedValue;
            if (opaque)
                smoothnessSource = EditorGUILayout.Popup(Styles.smoothnessMapChannelText, smoothnessSource, Enum.GetNames(typeof(SmoothnessMapChannel)));
            else
                EditorGUILayout.Popup(Styles.smoothnessMapChannelText, 0, Enum.GetNames(typeof(SmoothnessMapChannel)));
            if (EditorGUI.EndChangeCheck())
                properties.smoothnessMapChannel.floatValue = smoothnessSource;
            EditorGUI.showMixedValue = false;
            EditorGUI.indentLevel -= 3;
            EditorGUI.EndDisabledGroup();
        }
#endif


        public virtual void DrawSurfaceInputs2D(Material material)
        {
            //EditorGUI.BeginChangeCheck();
            {
                //GUI.enabled = false;
                //materialEditor.TexturePropertySingleLine(Styles.baseMap, baseMapProp, baseColorProp);
                ////materialEditor.TexturePropertySingleLine(Styles.normalMapText, bumpMapProp);
                //GUI.enabled = true;
                ////DrawNormalArea();

                //GUI.enabled = false;
                //materialEditor.TexturePropertySingleLine(Styles.normalMapText, bumpMapProp);
                //GUI.enabled = true;

                EditorGUILayout.HelpBox("In URP2D, when using the Sprite Renderer, you have to set the textures via the Sprite Editor. This shader has support for _NormalMap and _EmissionMap." +
                    "\nIn case your NormalMap is called something else you can change it in the ShaderGraph itself(unoptimized shader). ", MessageType.Info);
                DisableGUIIfOptimizedShader();

                
                materialEditor.ShaderProperty(useMainTex, "Use MainTex");

                if(useMainTex.floatValue == 1)
                {
                    materialEditor.ShaderProperty(baseColorProp, "Tint");
                } else
                {
                    materialEditor.ShaderProperty(baseColorProp, "Color");
                }

                EditorGUILayout.Space();
                EditorUtils.DrawSubMenuSeparation2();
                EditorGUILayout.Space();

                materialEditor.ShaderProperty(enableNormalMap, "Enable NormalMap");
                GUI.enabled = true;
                if (bumpScaleProp != null && enableNormalMap.floatValue == 1) // && bumpMapProp.textureValue != null)               
                {
                    materialEditor.ShaderProperty(bumpScaleProp, Styles.normalMapScaleText);

                    if (bumpScaleProp.floatValue != 1f &&
                        InternalEditorUtility.IsMobilePlatform(EditorUserBuildSettings.activeBuildTarget) &&
                        materialEditor.HelpBoxWithButton(Styles.bumpScaleNotSupported, Styles.fixNormalNow))
                    {
                        bumpScaleProp.floatValue = 1f;
                    }

                    EditorGUILayout.Space();
                    EditorUtils.DrawSubMenuSeparation();
                    EditorGUILayout.Space();

                    //materialEditor.ShaderProperty(convertNormalToAlbedo, "Add Grayscale NormalMap to Albedo");
                    string AlbedoLight = "Light";
                    if(enableToonShading.floatValue == 0)
                    {
                        AlbedoLight = "Albedo";
                    }
                    convertNormalToAlbedo.floatValue = Convert.ToSingle(EditorGUILayout.ToggleLeft("Add Grayscale NormalMap to " + AlbedoLight , Convert.ToBoolean(convertNormalToAlbedo.floatValue)));
                    if (convertNormalToAlbedo.floatValue == 1)
                    {
                        Vector3 vector3 = Vector3.zero;
                        EditorGUILayout.LabelField("Levels");
                        float xC = EditorGUILayout.Slider("X - Red(Left/Right)", normalToAlbedoConversionVector.vectorValue.x, 0, 2);
                        float yC = EditorGUILayout.Slider("Y - Green(Up/Down)", normalToAlbedoConversionVector.vectorValue.y, 0, 2);
                        float zC = EditorGUILayout.Slider("Z - Blue(Forward/Backward)", normalToAlbedoConversionVector.vectorValue.z, 0, 2);
                        float wC = EditorGUILayout.Slider("Contrast", normalToAlbedoConversionVector.vectorValue.w, 0, 2);
                        normalToAlbedoConversionVector.vectorValue = new Vector4(xC, yC, zC, wC);

                        //materialEditor.RangeProperty(normalToAlbedoConversionVector.vectorValue.x, "Levels");                        
                    }
                }
                EditorGUILayout.Space();
                EditorUtils.DrawSubMenuSeparation2();
                EditorGUILayout.Space();

                DisableGUIIfOptimizedShader();
                materialEditor.ShaderProperty(enableEmission, "Enable Emission");
                GUI.enabled = true;
                if (enableEmission.floatValue == 1) 
                {
                    //GUI.enabled = false;
                    //materialEditor.TexturePropertySingleLine(new GUIContent("Emission Map"), emissionMapProp);
                    //GUI.enabled = true;

                    materialEditor.ShaderProperty(emissionColorProp, new GUIContent("Emission Color"));
                }


            }
            //if (EditorGUI.EndChangeCheck())
            //{
            //    //foreach (var obj in blendModeProp.targets)
            //        MaterialChanged(material);
            //}
}       

        public virtual void DrawSurfaceInputs(Material material)
        {
            EditorGUI.BeginChangeCheck();
            {
                DrawBaseProperties(material);

                if(shadingMode.floatValue == 1 && lightFunction.floatValue == 0 || lightFunction.floatValue == 2)//URP 2020 BlinnPhong / SimpleLit:
                {
                    Advanced(shadingModelProperties);
                    DoSpecularAreaBlinnPhong(shadingModelProperties, materialEditor, material);
                } 
                else
                {
                    if(baseMapProp != null && baseMapProp.textureValue != null)
                    {
                        EditorGUI.indentLevel += 2;
                        materialEditor.ShaderProperty(useAlphaOnlyFromBaseMap, "Use Alpha only");
                        EditorGUI.indentLevel -= 2;
                    }
                }

                DrawNormalArea();

                //if (occlusionMap != null)
                //{
                //    materialEditor.TexturePropertySingleLine(Styles.occlusionText, occlusionMap, (occlusionMap.textureValue != null) ? occlusionStrength : null);
                //}

                DrawEmissionProperties(material, true);
                DrawTileOffset(materialEditor, baseMapProp);
            }
            if (EditorGUI.EndChangeCheck())
            {
                foreach (var obj in blendModeProp.targets)
                    MaterialChanged((Material)obj);
            }
        }

#endregion
#endif


        private void DoSurfaceInputsAndOptionsArea(Material material, MaterialProperty[] properties)
        {



            //showContentAreaSurfaceInputsAndOptions.floatValue = Convert.ToSingle(MakeTheToonShaderSectionHeaderWithFoldout("Surface Options and Inputs", Convert.ToBoolean(showContentAreaSurfaceInputsAndOptions.floatValue)));
            m_SurfaceInputsAndOptionsAreaFoldout.value = ToonEditorUtils.MakeTheToonShaderSectionHeaderWithFoldout("Surface Options and Inputs", m_SurfaceInputsAndOptionsAreaFoldout.value);

            surfaceOptionsAndInputsAnimBool.target = m_SurfaceInputsAndOptionsAreaFoldout.value == true;
            if (EditorGUILayout.BeginFadeGroup(surfaceOptionsAndInputsAnimBool.faded))
            {

                if (rp != RenderPipelineOptions.URP2D && !IsToon2DShader())
                {
                    EditorGUILayout.Space();
                    EditorGUI.BeginChangeCheck();
                    shadingMode.floatValue = GUILayout.Toolbar((int)shadingMode.floatValue, new string[] { "Color Based", "Light Based" });
                    if (EditorGUI.EndChangeCheck())
                    {
                        SetMaterialKeywordsToon(material);
                    }
  

                    if (shadingMode.floatValue == 1)
                    {
                        EditorGUI.indentLevel += 1;
                        if (rp == RenderPipelineOptions.URP)
                        {
                            EditorGUILayout.Space();
                            EditorGUI.BeginChangeCheck();
#if USING_URP
                            lightFunction.floatValue = (int)(LightFunction)EditorGUILayout.EnumPopup("Light Function", (LightFunction)lightFunction.floatValue);
#endif
                            if (EditorGUI.EndChangeCheck())
                            {
                                SetMaterialKeywordsToon(material);
                            }
                            EditorGUILayout.Space();

                        }
                        else if (rp == RenderPipelineOptions.BiRP)
                        {
                            lightFunction.floatValue = (float)LightFunction.PBR;
                        }

                        //if(lightFunction.floatValue == 2)
                        //{
                        //    materialEditor.ShaderProperty(lightFunctionLerpValue, "Mix Ratio");

                        //}

                        EditorGUI.indentLevel -= 1;
                    }
                }

                EditorGUILayout.Space();
                if (rp == RenderPipelineOptions.BiRP)
                {
                    biRPLightingGUI.BiRPShaderPropertiesGUI(material, (GeneralShadingMode)shadingMode.floatValue, (LightFunction)lightFunction.floatValue);
                } 
                else if (rp == RenderPipelineOptions.URP)
                {
#if USING_URP
                    if (shadingMode.floatValue == 1 && lightFunction.floatValue == 1) // PBR
                    {
                        // Detect any changes to the material
                        EditorGUI.BeginChangeCheck();
                        {
                            LitShader.OnGUI(materialEditor, properties);
                        }
                        if (EditorGUI.EndChangeCheck())
                        {
                            foreach (var obj in blendModeProp.targets)
                                MaterialChanged((Material)obj);
                        }



                    }
                    else
                    {
                        m_SurfaceOptionsFoldout.value = ToonEditorUtils.MakeTheToonShaderSubSectionHeaderWithFoldout(Styles.SurfaceOptions, m_SurfaceOptionsFoldout.value);

                        if (m_SurfaceOptionsFoldout.value)
                        {
                            DrawSurfaceOptions(material);
                            EditorGUILayout.Space();
                        }


                        m_SurfaceInputsFoldout.value = ToonEditorUtils.MakeTheToonShaderSubSectionHeaderWithFoldout(Styles.SurfaceInputs, m_SurfaceInputsFoldout.value);

                        if (m_SurfaceInputsFoldout.value)
                        {
                            DrawSurfaceInputs(material);
                            EditorGUILayout.Space();
                        }
                    }

                } else if (rp == RenderPipelineOptions.URP2D && IsToon2DShader())
                {
                    DrawSurfaceInputs2D(material);
#endif
                }


            }

            EditorGUILayout.EndFadeGroup();

        }
#endregion


        public GradientSO gradientSO;
        public GradientSO gradientSOShadow;
        Texture2D gradientTexture;
        //Gradient gradient = new Gradient();

        readonly int gradientTextureResolution = 512; //make optional?


        private void GradientSetup()
        {
            if (enableToonShading.floatValue == 1 && (GeneralShadingFunction)shadingFunction.floatValue == GeneralShadingFunction.Gradient)
            {
                if (gradientTexture == null)
                {
                    //gradientTexture = new Texture2D(gradientTextureResolution, 1, TextureFormat.RGBA32, true, true);
                    gradientTexture = new Texture2D(gradientTextureResolution, 1, TextureFormat.RGBA32, true, false);
                }

                CreateInitialTexture(materialEditor.target as Material);

                RefreshGradientSO(ref gradientSO, GradientTyp.Light);
                if (rp != RenderPipelineOptions.URP2D && !IsToon2DShader())
                {
                    if ((GradientMode)gradientMode.floatValue == GradientMode.Separated)
                    {
                        RefreshGradientSO(ref gradientSOShadow, GradientTyp.Shadow);
                    }
                }
                UpdateGradientTexture(materialEditor.target as Material);
            }
        }

        enum GradientTyp
        {
            Light = 0,
            Shadow = 1,
        }
        private void RefreshGradientSO(ref GradientSO gradientSOTMP, GradientTyp gradientTyp)
        {
            if (gradientSOTMP == null)
            {
                string materialName = (materialEditor.target as Material).name;
                materialName = materialName.Replace("/", "_");
                string nameSO = materialName.Replace("/", "_");
                nameSO += gradientTyp.ToString();
                gradientSOTMP = Resources.Load<GradientSO>("GradientScriptableObjects/" + nameSO);

                if (gradientSOTMP == null)
                {
                    gradientSOTMP = ScriptableObject.CreateInstance<GradientSO>();
                    gradientSOTMP.gradient = new Gradient();

                    //string dataPathWithoutAssets = Application.dataPath;
                    //if (dataPathWithoutAssets.EndsWith("/Assets"))
                    //{
                    //    dataPathWithoutAssets = dataPathWithoutAssets.Substring(0, dataPathWithoutAssets.LastIndexOf("/Assets"));
                    //}
                    //string dirPath = dataPathWithoutAssets + "/Packages/com.shadercrew.the-toon-shader.core/Scripts/Editor/Resources/GradientScriptableObjects/";

                    string dirPath;
                    if (IsToon2DShader())
                    {
                        dirPath = "Packages/com.shadercrew.the-toon-shader.2d/Scripts/Editor/Resources/GradientScriptableObjects/"; //relative path needed for AssetDatabase.CreateAsset()
                    }
                    else
                    {
                        dirPath = "Packages/com.shadercrew.the-toon-shader.3d/Scripts/Editor/Resources/GradientScriptableObjects/"; //relative path needed for AssetDatabase.CreateAsset()
                    }

                    //string dirPath = "Assets/TheToonShader/Resources/GradientScriptableObjects/";

                    if (!Directory.Exists(dirPath))
                    {
                        Directory.CreateDirectory(dirPath);
                    }
                    AssetDatabase.CreateAsset(gradientSOTMP, dirPath + nameSO + ".asset");
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                    Debug.Log("ScriptableObject for material " + materialName + " doesn't exist. ScriptableObject " + nameSO + ".asset was created.");
                    //gradientSOTMP = Resources.Load<GradientSO>("GradientScriptableObjects/" + nameSO);
                }
                else
                {
                    if (!gradientSOTMP.isBakedToTexture)
                    {
                        SaveTexture(materialEditor.target as Material);
                        Texture2D texture = Resources.Load("Gradients/" + materialName, typeof(Texture2D)) as Texture2D;
                        SetTextureImporterFormat(texture, true);
                        gradientTex.textureValue = texture;
                        gradientSOTMP.isBakedToTexture = true;
                        EditorUtility.SetDirty(gradientSOTMP);
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                    }
                    else
                    {
                        Texture2D texture = Resources.Load("Gradients/" + materialName, typeof(Texture2D)) as Texture2D;
                        SetTextureImporterFormat(texture, true);
                        gradientTex.textureValue = texture;
                        EditorUtility.SetDirty(gradientSOTMP);
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                    }
                }
            }

            //return gradientSO;
        }

        public void CreateInitialTexture(Material material)
        {
            if (gradientTexture != null)
            {
                string dataPathWithoutAssets = Application.dataPath;
                if (dataPathWithoutAssets.EndsWith("/Assets"))
                {
                    dataPathWithoutAssets = dataPathWithoutAssets.Substring(0, dataPathWithoutAssets.LastIndexOf("/Assets"));
                }

                string dirPath;
                if (IsToon2DShader())
                {
                    dirPath = dataPathWithoutAssets + "/Packages/com.shadercrew.the-toon-shader.2d/Scripts/Resources/Gradients/";
                }
                else
                { 
                    dirPath = dataPathWithoutAssets + "/Packages/com.shadercrew.the-toon-shader.3d/Scripts/Resources/Gradients/";
                }

                //string dirPath = dataPathWithoutAssets + "/Assets/TheToonShader/Resources/Gradients/";
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }

                string name = material.name;
                if (!File.Exists(dirPath + name + ".png"))
                {
                    Debug.Log(name + ".png doesn't exist. Gradient Texture for material " + name + " was created.");
                    SaveTexture(material);
                    Texture2D texture = Resources.Load("Gradients/" + name, typeof(Texture2D)) as Texture2D;
                    SetTextureImporterFormat(texture, true);
                    gradientTex.textureValue = texture;
                }                
            }
        }
        public static void SetTextureImporterFormat(Texture2D texture, bool isReadable)
        {
            if (null == texture) return;

            string assetPath = AssetDatabase.GetAssetPath(texture);
            var tImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            if (tImporter != null)
            {
                tImporter.isReadable = isReadable;

                AssetDatabase.ImportAsset(assetPath);
                AssetDatabase.Refresh();
            }
        }


        public void GradientToTexture()
        {
            if (gradientTexture != null)
            {
                if (gradientTexture.width != gradientTextureResolution && gradientTexture.height != 1)
                {
#if !UNITY_2021_1_OR_NEWER
                    gradientTexture.Resize(gradientTextureResolution, 1);
#else
                    gradientTexture.Reinitialize(gradientTextureResolution, 1);
#endif
                }

                gradientTexture.wrapMode = TextureWrapMode.Clamp;
                gradientTexture.filterMode = FilterMode.Bilinear;


                bool isToonShader = IsToon2DShader();

                Color[] colors = new Color[gradientTextureResolution];
                for (int i = 0; i < gradientTextureResolution; ++i)
                {
                    Color col;
                    if (rp != RenderPipelineOptions.URP2D && !isToonShader && (GradientMode)gradientMode.floatValue == GradientMode.Separated)
                    {
                        if (i < gradientTextureResolution / 2)
                        {
                            if (enableShadows.floatValue == 1)
                            {
                                if (gradientSOShadow == null)
                                {
                                    RefreshGradientSO(ref gradientSOShadow, GradientTyp.Shadow);
                                }
                                var t = (float)(i / (gradientTextureResolution / 2.0));
                                col = gradientSOShadow.gradient.Evaluate(t);
                            }
                            else
                            {
                                col = gradientSO.gradient.Evaluate(0);
                            }
                        }
                        else
                        {
                            float t = (float)((i - gradientTextureResolution / 2.0) / (gradientTextureResolution / 2.0));
                            col = gradientSO.gradient.Evaluate(t);
                        }
                    }
                    else
                    {
                        var t = (float)i / gradientTextureResolution;
                        col = gradientSO.gradient.Evaluate(t);
                    }
                    colors[i] = col;
                }
                gradientTexture.SetPixels(colors);
                gradientTexture.Apply(false);
            } else
            {
                Debug.Log("gradientTexture is null");
            }
        }



        public void SaveTexture(Material material)
        {
            if (gradientTexture != null)
            {
                byte[] bytes = gradientTexture.EncodeToPNG();

                string dataPathWithoutAssets = Application.dataPath;
                if (dataPathWithoutAssets.EndsWith("/Assets"))
                {
                    dataPathWithoutAssets = dataPathWithoutAssets.Substring(0, dataPathWithoutAssets.LastIndexOf("/Assets"));
                }

                string dirPath;
                if (IsToon2DShader())
                {
                    dirPath = dataPathWithoutAssets + "/Packages/com.shadercrew.the-toon-shader.2d/Scripts/Resources/Gradients/";
                }
                else
                {
                    dirPath = dataPathWithoutAssets + "/Packages/com.shadercrew.the-toon-shader.3d/Scripts/Resources/Gradients/";
                }



                //string dirPath = dataPathWithoutAssets + "/Assets/TheToonShader/Resources/Gradients/";

                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }
                string name = material.name;
                //name = name.Replace(" (Instance)", "");
                File.WriteAllBytes(dirPath + name + ".png", bytes);
                //Debug.Log(name + " saved");
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }


        public void UpdateGradientTexture(Material material)
        {
            GradientToTexture();
            if (gradientTexture != null)
            {
                gradientTexture.Apply(true);
                SaveTexture(material);
                if(gradientSO != null)
                {
                    gradientSO.isBakedToTexture = true;
                }
                else
                {
                    Debug.Log("gradientSO is null");
                }
                if (gradientSOShadow != null)
                {
                    gradientSOShadow.isBakedToTexture = true;
                }
            }
        }

        public void UpdateGradientTextureMemory(Material material)
        {
            GradientToTexture();
            if (gradientTexture != null)
            {
                gradientTexture.Apply(true);
                gradientTex.textureValue = gradientTexture;

                if (gradientSO != null)
                {
                    gradientSO.isBakedToTexture = false;
                }
                else
                {
                    Debug.Log("gradientSO is null");

                }
                if (gradientSOShadow != null)
                {
                    gradientSOShadow.isBakedToTexture = false;
                }
            }
        }

        public void DisableGUIIfOptimizedShader()
        {
            if (isOptimized != null)
            {
                GUI.enabled = false;
            }
        }

        // Store the last selected transform for editor convenience
        private Transform selectedTransform;

        private void DoToonShadingArea(Material material)
        {


            Color lightGreyForLines = new Color(0.35f, 0.35f, 0.35f, 1);


            Color toonAreaColor = Color.gray;

            DisableGUIIfOptimizedShader();
            bool showToonShading = ToonEditorUtils.MakeTheToonShaderSectionHeaderWithFoldoutWithToggle(Styles.ToonShadingHeader, m_ToonShadingAreaFoldout, enableToonShading);
            GUI.enabled = true;

            toonShadingAnimBool.target = showToonShading == true;

            if (EditorGUILayout.BeginFadeGroup(toonShadingAnimBool.faded))
            {

                EditorGUILayout.Space();

                if ((GeneralShadingMode)shadingMode.floatValue == GeneralShadingMode.LightBased)
                {
                    shadingFunction.floatValue = 0;
                } else
                {
                    DisableGUIIfOptimizedShader();

                    //if (rp == RenderPipelineOptions.URP2D)
                    //{
                    //    EditorGUI.BeginChangeCheck();
                    //    selectedTransform = (Transform)EditorGUILayout.ObjectField("Transform", selectedTransform, typeof(Transform), true);
                    //    if (EditorGUI.EndChangeCheck() && selectedTransform != null)
                    //    {
                    //        Vector3 pos = selectedTransform.position;
                    //        fakeLightPosition.vectorValue = new Vector4(pos.x, pos.y, pos.z, 0);
                    //    }
                    //}

                    EditorGUI.BeginChangeCheck();
                    shadingFunction.floatValue = (int)(GeneralShadingFunction)EditorGUILayout.EnumPopup("Shading Function", (GeneralShadingFunction)shadingFunction.floatValue);
                    if (EditorGUI.EndChangeCheck())
                    {
                        if ((GeneralShadingFunction)shadingFunction.floatValue == GeneralShadingFunction.Gradient)
                        {
                            GradientSetup();
                        }
                    }
                    GUI.enabled = true;
                    EditorGUILayout.Space();
                }


                if((GeneralShadingFunction) shadingFunction.floatValue == GeneralShadingFunction.Gradient)
                {
                    EditorGUILayout.HelpBox("Sometimes initially you have to open the gradient field twice and wiggle it, for it to take effect, or press bake! Sorry about that :)", MessageType.Info);

                    if(rp != RenderPipelineOptions.URP2D && !IsToon2DShader())
                    {
                        EditorGUI.BeginChangeCheck();
                        gradientMode.floatValue = (int)(GradientMode)EditorGUILayout.EnumPopup("Gradient Mode", (GradientMode)gradientMode.floatValue);
                        if (EditorGUI.EndChangeCheck())
                        {
                            UpdateGradientTexture(material);
                        }
                    }


                    EditorGUILayout.Space();
                    gradientBlending.floatValue = (int)(BlendingMode)EditorGUILayout.EnumPopup("Blending", (BlendingMode)gradientBlending.floatValue);
                    DarkerSliderProperty(gradientBlendFactor, "Bend Factor");
                    EditorGUILayout.Space();
                    if (rp != RenderPipelineOptions.URP2D && !IsToon2DShader())
                    {
                        EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth - 94;
                        materialEditor.ShaderProperty(shadingAffectByNormalMap, "Shadows Affect By NormalMap");
                        EditorGUIUtility.labelWidth = oriLabelWidth;
                        EditorGUILayout.Space();
                    }

                }

                //materialEditor.TexturePropertySingleLine(Styles.normalMapText, gradientTex);


                if ((GeneralShadingFunction)shadingFunction.floatValue == GeneralShadingFunction.Cells || (rp != RenderPipelineOptions.URP2D && !IsToon2DShader() && (GradientMode)gradientMode.floatValue == GradientMode.Separated))
                {
                    GUIContent gUIContent;

                    if((GeneralShadingFunction)shadingFunction.floatValue == GeneralShadingFunction.Cells)
                    {
                        gUIContent = Styles.GeneralShadingLightPartitioning;
                    } else
                    {

                        gUIContent = Styles.GeneralShadingLightGradient;
                    }

                    bool showLightPartitioningOptions = false;
                    if (rp != RenderPipelineOptions.URP2D && !IsToon2DShader())
                    {
                        m_GeneralShadingLightPartitioningFoldout.value = ToonEditorUtils.MakeTheToonShaderSubSectionHeaderWithFoldout(gUIContent, m_GeneralShadingLightPartitioningFoldout.value, toonAreaColor);
                        showLightPartitioningOptions = m_GeneralShadingLightPartitioningFoldout.value;
                    }
                    else
                    {
                        showLightPartitioningOptions = ToonEditorUtils.MakeTheToonShaderSubSectionHeaderWithFoldoutWithToggle(gUIContent, m_GeneralShadingLightPartitioningFoldout, enableLightPartitioning, toonAreaColor);
                        GUI.enabled = true;
                    }


                    //    m_GeneralShadingLightPartitioningFoldout.value = ToonEditorUtils.MakeTheToonShaderSubSectionHeaderWithFoldout(gUIContent, m_GeneralShadingLightPartitioningFoldout.value, toonAreaColor);
                    ////float oriLabelWidth = EditorGUIUtility.labelWidth;

                    ////m_SurfaceOptionsFoldout.value = EditorGUILayout.BeginFoldoutHeaderGroup(m_SurfaceOptionsFoldout.value, Styles.SurfaceOptions);
                    //if (m_GeneralShadingLightPartitioningFoldout.value)

                   if (showLightPartitioningOptions)
                   {
                        //EditorUtils.DrawUILine(lightGreyForLines);
                        EditorGUILayout.Space();


                        EditorGUI.indentLevel += 1;
                        if ((GeneralShadingFunction)shadingFunction.floatValue == GeneralShadingFunction.Cells)
                        {
                            if (enableShadows.floatValue == 1 || (GeneralShadingMode)shadingMode.floatValue == GeneralShadingMode.LightBased)
                            {
                                if (numberOfCells.floatValue <= 0)
                                {
                                    numberOfCells.floatValue = 0;
                                }
                                numberOfCells.floatValue = EditorGUILayout.IntSlider("Number Of Cells", (int)numberOfCells.floatValue, 1, 15);

                                //EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth - 94;
                                //EditorGUIUtility.labelWidth = oriLabelWidth;

                                //materialEditor.ShaderProperty(numberOfCells, "Number Of Cells");
                                //materialEditor.ShaderProperty(cellTransitionSmoothness, "Cell Transition Smoothness");
                                DarkerSliderProperty(cellTransitionSmoothness, "Cell Transition Smoothness");

#if USING_URP
                                if(rp != RenderPipelineOptions.URP2D && !IsToon2DShader())
                                {
                                    if ((GeneralShadingMode)shadingMode.floatValue == GeneralShadingMode.SolidColor)
                                    {
                                        materialEditor.ShaderProperty(sumLightsBeforePosterization, "Sum Lights Before Posterization");
                                    }
                                }
#endif


                                if (rp == RenderPipelineOptions.URP2D && IsToon2DShader())
                                {
                                    EditorGUILayout.Space();
                                    DisableGUIIfOptimizedShader();
                                    EditorGUIUtility.labelWidth = oriLabelWidth;
                                    cellMethod.floatValue = (int)(CellMethod)EditorGUILayout.EnumPopup("Cell Method", (CellMethod)cellMethod.floatValue);
                                    roundingMethod.floatValue = (int)(RoundingMethod)EditorGUILayout.EnumPopup("Rounding Method", (RoundingMethod)roundingMethod.floatValue);
                                    EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth - 94;
                                    if (cellMethod.floatValue == 0)
                                    {
                                        materialEditor.ShaderProperty(enableMainTexPosterization, "Posterize MainTex");   //Add later          
                                    }
                                    GUI.enabled = true;
                                }

                            }
                            else
                            {
                                EditorGUILayout.HelpBox("You need to enable Shading for this feature!", MessageType.Info);

                            }

                        }
                        else
                        {


                            if (gradientSO == null)
                            {
                                RefreshGradientSO(ref gradientSO, GradientTyp.Light);
                            }
                            //materialEditor.TexturePropertySingleLine(Styles.normalMapText, gradientTex);
                            //Gradient gradient = new Gradient();
                            EditorGUI.BeginChangeCheck();
                            gradientSO.gradient = EditorGUILayout.GradientField(gradientSO.gradient);
                            if (EditorGUI.EndChangeCheck())
                            {
                                //UpdateGradientTexture(material);
                                UpdateGradientTextureMemory(material);
                                //Debug.Log("Gradient Change Detected");
                            }


                            string name = material.name;
                            //name += GradientTyp.Light.ToString();

                            //if (!name.Contains(SeeThroughShaderConstants.STS_INSTANCE_PREFIX) &&
                            //!material.name.Contains(SeeThroughShaderConstants.STS_TRIGGER_PREFIX))
                            //{
                            if (!gradientSO.isBakedToTexture)
                            {
                                EditorGUILayout.HelpBox("THIS IS ONLY A PREVIEW! TO USE THE GRADIENT IN BUILDS, YOU HAVE TO BAKE THE GRADIENT!", MessageType.Warning);
                                var rect = EditorGUI.IndentedRect(EditorGUILayout.GetControlRect(new GUILayoutOption[] { }));
                                if (GUI.Button(rect, "Bake Gradient"))
                                {
                                    SaveTexture(material);
                                    Texture2D texture = Resources.Load("Gradients/" + name, typeof(Texture2D)) as Texture2D;

                                    //Debug.Log(texture.name);
                                    SetTextureImporterFormat(texture, true);
                                    //Debug.Log("D");
                                    gradientTex.textureValue = texture;
                                    //material.SetTexture("_ObstructionCurve", texture);
                                    gradientSO.isBakedToTexture = true;
                                    EditorUtility.SetDirty(gradientSO);
                                    AssetDatabase.SaveAssets();
                                }
                            }
                            //}
                        }
                        EditorGUI.indentLevel -= 1;





                        EditorGUILayout.Space();
                    }
                } 
                else
                {
                    m_GeneralShadingLightPartitioningFoldout.value = ToonEditorUtils.MakeTheToonShaderSubSectionHeaderWithFoldout(Styles.GeneralShadingLightAndShadowGradient, m_GeneralShadingLightPartitioningFoldout.value, toonAreaColor);
                    //float oriLabelWidth = EditorGUIUtility.labelWidth;

                    //m_SurfaceOptionsFoldout.value = EditorGUILayout.BeginFoldoutHeaderGroup(m_SurfaceOptionsFoldout.value, Styles.SurfaceOptions);
                    if (m_GeneralShadingLightPartitioningFoldout.value)
                    { 

                    }
                }

                //if ((GradientMode)gradientMode.floatValue == GradientMode.Combined)
                //{

                //} else
                //{
                    
                //}
                //EditorUtils.DrawUILine(lightGreyForLines);
                EditorGUILayout.Space();

                if (shadingMode.floatValue == 0) 
                {


                    //if((GeneralShadingFunction)shadingFunction.floatValue == GeneralShadingFunction.Cells || (GradientMode)gradientMode.floatValue != GradientMode.Combined)
                    if((GeneralShadingFunction)shadingFunction.floatValue == GeneralShadingFunction.Cells)
                    {
                        //if (enableShadows.floatValue == 0)
                        //{
                        //    m_GeneralShadingShadowFoldout.value = false;
                        //}
                        EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth - 94;
#if USING_URP
                        DisableGUIIfOptimizedShader();
                        if (rp != RenderPipelineOptions.URP2D && !IsToon2DShader()) //TODO: add later for toon2d
                        {
                            materialEditor.ShaderProperty(shadingUseLightColors, "Use Light Colors");
                            GUI.enabled = true;
                        }
                        EditorGUILayout.Space();
#endif
                        if (rp != RenderPipelineOptions.URP2D && !IsToon2DShader())
                        {
                            materialEditor.ShaderProperty(shadingAffectByNormalMap, "Shading/Shadows Affect By NormalMap");
                        }
                        else
                        {

                        }

                        EditorGUIUtility.labelWidth = oriLabelWidth;
                        EditorGUILayout.Space();

                        DisableGUIIfOptimizedShader();
                        bool showShadowOptions = ToonEditorUtils.MakeTheToonShaderSubSectionHeaderWithFoldoutWithToggle(Styles.GeneralShadingShading, m_GeneralShadingShadowFoldout, enableShadows, toonAreaColor);
                        GUI.enabled = true;
                        if (showShadowOptions)
                        {


                            EditorGUILayout.Space();

                            EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth - 94;





                            if(rp != RenderPipelineOptions.URP2D && !IsToon2DShader())
                            {
                                materialEditor.ShaderProperty(coreShadowColor, "Core Shadow Color");

                                //materialEditor.ShaderProperty(terminatorWidth, "Terminator Width");
                                DisableGUIIfOptimizedShader();
                                DarkerSliderProperty(terminatorPosition, "Terminator Position"); // TODO
                                GUI.enabled = true;
                                DarkerSliderProperty(terminatorWidth, "Terminator Width");
                                //materialEditor.ShaderProperty(terminatorSmoothness, "Terminator Smoothness");
                                DarkerSliderProperty(terminatorSmoothness, "Terminator Smoothness");
                                materialEditor.ShaderProperty(formShadowColor, "Form Shadow Color");
                            }
                            else
                            {
                                materialEditor.ShaderProperty(useCoreShadowColor, "Use Core Shadow Color");
                                if(useCoreShadowColor.floatValue == 1)
                                {
                                    materialEditor.ShaderProperty(coreShadowColor, "Core Shadow Color");
                                }

                            }

                            EditorGUIUtility.labelWidth = oriLabelWidth;

                        }
                    }
                    else
                    {
                        if (rp == RenderPipelineOptions.URP2D && IsToon2DShader() || (GradientMode)gradientMode.floatValue == GradientMode.Combined)
                        {
                            if (gradientSO == null)
                            {
                                RefreshGradientSO(ref gradientSO, GradientTyp.Light);
                            }

                            EditorGUI.BeginChangeCheck();
                            gradientSO.gradient = EditorGUILayout.GradientField(gradientSO.gradient);
                            if (EditorGUI.EndChangeCheck())
                            {
                                UpdateGradientTextureMemory(material);
                            }
                            string name = material.name;
                            if (!gradientSO.isBakedToTexture)
                            {
                                EditorGUILayout.HelpBox("THIS IS ONLY A PREVIEW! TO USE THE GRADIENT IN BUILDS, YOU HAVE TO BAKE THE GRADIENT!", MessageType.Warning);
                                var rect = EditorGUI.IndentedRect(EditorGUILayout.GetControlRect(new GUILayoutOption[] { }));
                                if (GUI.Button(rect, "Bake Gradient"))
                                {
                                    SaveTexture(material);
                                    Texture2D texture = Resources.Load("Gradients/" + name, typeof(Texture2D)) as Texture2D;
                                    if(texture != null)
                                    {
                                        SetTextureImporterFormat(texture, true);
                                        gradientTex.textureValue = texture;
                                        //material.SetTexture("_ObstructionCurve", texture);
                                        gradientSO.isBakedToTexture = true;
                                        EditorUtility.SetDirty(gradientSO);
                                        AssetDatabase.SaveAssets();
                                    }
                                }
                            }


                            EditorGUILayout.Space();

                        }
                        if (rp != RenderPipelineOptions.URP2D && !IsToon2DShader())
                        {
                            if ((GradientMode)gradientMode.floatValue == GradientMode.Separated)
                            {
                                bool showShadowOptions = ToonEditorUtils.MakeTheToonShaderSubSectionHeaderWithFoldoutWithToggle(Styles.GeneralShadingShadowGradient, m_GeneralShadingShadowFoldout, enableShadows, toonAreaColor);
                                if (showShadowOptions)
                                {


                                    //EditorUtils.DrawUILine(lightGreyForLines);
                                    EditorGUILayout.Space();

                                    if (gradientSOShadow == null)
                                    {
                                        RefreshGradientSO(ref gradientSOShadow, GradientTyp.Shadow);
                                    }
                                    if (gradientSO == null)
                                    {
                                        RefreshGradientSO(ref gradientSO, GradientTyp.Light);

                                    }

                                    GradientColorKey[] gradientColorKeys = gradientSOShadow.gradient.colorKeys;

                                    GradientColorKey key = gradientColorKeys[gradientColorKeys.Length - 1];
                                    if (gradientColorKeys.Length < 9 && key.time < 0.9)
                                    {
                                        Array.Resize<GradientColorKey>(ref gradientColorKeys, gradientColorKeys.Length + 1);
                                    }

                                    gradientColorKeys[gradientColorKeys.Length - 1] = new GradientColorKey(gradientSO.gradient.Evaluate(0), 1);

                                    gradientSOShadow.gradient.colorKeys = gradientColorKeys;


                                    EditorGUI.BeginChangeCheck();
                                    gradientSOShadow.gradient = EditorGUILayout.GradientField(gradientSOShadow.gradient);
                                    if (EditorGUI.EndChangeCheck())
                                    {
                                        UpdateGradientTextureMemory(material);
                                    }
                                    string name = material.name;

                                    if (!gradientSOShadow.isBakedToTexture)
                                    {
                                        EditorGUILayout.HelpBox("THIS IS ONLY A PREVIEW! TO USE THE GRADIENT IN BUILDS, YOU HAVE TO BAKE THE GRADIENT!", MessageType.Warning);
                                        var rect = EditorGUI.IndentedRect(EditorGUILayout.GetControlRect(new GUILayoutOption[] { }));
                                        if (GUI.Button(rect, "Bake Gradient"))
                                        {
                                            SaveTexture(material);
                                            Texture2D texture = Resources.Load("Gradients/" + name, typeof(Texture2D)) as Texture2D;
                                            SetTextureImporterFormat(texture, true);
                                            Debug.Log("F");
                                            gradientTex.textureValue = texture;
                                            //material.SetTexture("_ObstructionCurve", texture);
                                            gradientSOShadow.isBakedToTexture = true;
                                            EditorUtility.SetDirty(gradientSOShadow);
                                            AssetDatabase.SaveAssets();
                                        }
                                    }






                                    //enableCastShadows.floatValue = Convert.ToSingle(EditorGUILayout.ToggleLeft("Enable Cast Shadows", Convert.ToBoolean(enableCastShadows.floatValue)));
                                    //if (enableCastShadows.floatValue == 1)
                                    //{
                                    //    EditorGUI.indentLevel += 1;




                                    //    EditorGUI.indentLevel -= 1;

                                    //}

                                }
                            }
                        }
                    }
                    EditorGUILayout.Space();


                    if (rp != RenderPipelineOptions.URP2D && !IsToon2DShader())
                    {
                        DisableGUIIfOptimizedShader();
                        bool showShadowCastOptions = ToonEditorUtils.MakeTheToonShaderSubSectionHeaderWithFoldoutWithToggle(Styles.GeneralShadingCastShadows, m_GeneralShadingCastShadowsFoldout, enableCastShadows, toonAreaColor);
                        GUI.enabled = true;
                        if (showShadowCastOptions)
                        {

                            if (enableShadows.floatValue == 0)
                            {
                                EditorGUILayout.Space();
                                EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth - 94;
                                materialEditor.ShaderProperty(coreShadowColor, "Cast Shadows Color");
                                EditorGUIUtility.labelWidth = oriLabelWidth;
                                EditorGUILayout.Space();
                            }


                            //EditorGUI.indentLevel += 1;




                            //if ((GeneralShadingFunction)shadingFunction.floatValue == GeneralShadingFunction.Cells)
                            //{
                            //    if (enableShadows.floatValue == 1)
                            //    {
                            //        EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth / 2f;
                            //        castShadowColorMode.floatValue = (int)(CastShadowColorMode)EditorGUILayout.EnumPopup("Cast Shadow Color Mode", (CastShadowColorMode)castShadowColorMode.floatValue);
                            //        EditorGUIUtility.labelWidth = oriLabelWidth;
                            //    }
                            //    else
                            //    {
                            //        EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth - 94;
                            //        materialEditor.ShaderProperty(castShadowColor, "Cast Shadow Color");
                            //        EditorGUIUtility.labelWidth = oriLabelWidth;
                            //    }
                            //}
                            //else
                            //{
                            //    EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth - 94;
                            //    materialEditor.ShaderProperty(castShadowColor, "Cast Shadow Color");
                            //    EditorGUIUtility.labelWidth = oriLabelWidth;
                            //}

                            EditorGUILayout.HelpBox("You can adjust the Shadow Smoothness and Strength in the Light Settings/Pipeline Settings.", MessageType.Info);
                            if (rp == RenderPipelineOptions.URP)
                            {
                                EditorGUILayout.HelpBox("Cast Shadows can cause jagged cell edges, if the Depth Bias of your Shadows isn't set correctly. If they do look odd, please check your Universal Render Pipeline Asset -> Shadows -> Depth Bias!", MessageType.Info);
                            }




                            //EditorGUI.indentLevel -= 1;
                            //}
                        }
                    }
                    //EditorUtils.DrawUILine(lightGreyForLines);
                    EditorGUILayout.Space();

                    //SPECULAR
                    //m_GeneralShadingSpecularFoldout.value = MakeTheToonShaderSubSectionHeaderWithFoldout(Styles.GeneralShadingSpecular, m_GeneralShadingSpecularFoldout.value);

                    ////m_SurfaceOptionsFoldout.value = EditorGUILayout.BeginFoldoutHeaderGroup(m_SurfaceOptionsFoldout.value, Styles.SurfaceOptions);
                    //if (m_GeneralShadingSpecularFoldout.value)
                    //{
                    DisableGUIIfOptimizedShader();
                    bool showSpecularOptions = ToonEditorUtils.MakeTheToonShaderSubSectionHeaderWithFoldoutWithToggle(Styles.GeneralShadingSpecular, m_GeneralShadingSpecularFoldout, enableSpecular, toonAreaColor);
                    GUI.enabled = true;
                    if (showSpecularOptions)
                    {
                        //EditorUtils.DrawUILine(lightGreyForLines);

                        //enableSpecular.floatValue = Convert.ToSingle(EditorGUILayout.ToggleLeft("Enable Specular", Convert.ToBoolean(enableSpecular.floatValue)));

                        //if (enableSpecular.floatValue == 1)
                        //{
                        EditorGUILayout.Space();
                        EditorGUI.indentLevel += 1;


#if UNITY_EDITOR && USING_URP && USING_TOON2D

                        if (rp == RenderPipelineOptions.URP2D && IsToon2DShader())
                        {
                            Toon2DLightManager toon2DLightManager = UnityEngine.Object.FindFirstObjectByType<Toon2DLightManager>(FindObjectsInactive.Include);
                            if (toon2DLightManager == null)
                            {
                                EditorGUILayout.HelpBox("You haven't added the Toon2DLightManager Component to your scene. Without it Specular won't work!", MessageType.Warning);

                                if (GUILayout.Button("Add Toon2DLightManager component"))
                                {
                                    GameObject go = new GameObject(typeof(Toon2DLightManager).Name); // Name = "MyCustomScript"
                                    go.AddComponent<Toon2DLightManager>();
                                }


                            }
                            else if (toon2DLightManager.enabled == false)
                            {
                                EditorGUILayout.HelpBox("Your Toon2DLightManager Component on the GameObject \"" + toon2DLightManager.gameObject.name + "\" is not active, please enable it for Specular to work!", MessageType.Warning);
                                if (GUILayout.Button("Enable Toon2DLightManager component"))
                                {
                                    toon2DLightManager.enabled = true;
                                }
                            }
                            else if (toon2DLightManager.gameObject.activeInHierarchy == false)
                            {
                                EditorGUILayout.HelpBox("Your GameObject \"" + toon2DLightManager.gameObject.name + "\" with the Toon2DLightManager Component on is not active, please enable it for Specular to work!", MessageType.Warning);
                                if (GUILayout.Button("Enable GameObject with Toon2DLightManager"))
                                {
                                    toon2DLightManager.gameObject.SetActive(true);
                                }
                            }
                            //SortingLayer[] layers = SortingLayer.layers; 
                            //string[] names = layers.Select(l => l.name).ToArray();
                            //EditorGUILayout.MaskField(new GUIContent("Affected Sorting Layers"), 0, names);
                            EditorGUILayout.Space();

                        }

#endif


                        specularBlending.floatValue = (int)(BlendingMode)EditorGUILayout.EnumPopup("Blending", (BlendingMode)specularBlending.floatValue);
                        EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth - 94;
                        materialEditor.ShaderProperty(specularSize, "Size");
                        EditorGUIUtility.labelWidth = oriLabelWidth;
                        materialEditor.ShaderProperty(specularSmoothness, "Smoothness");
                        materialEditor.ShaderProperty(specularOpacity, "Opacity");

                        EditorGUILayout.Space();
#if USING_URP
                        specularUseLightColors.floatValue = Convert.ToSingle(EditorGUILayout.ToggleLeft("Use Light Colors", Convert.ToBoolean(specularUseLightColors.floatValue)));

                        if (specularUseLightColors.floatValue == 0)
#endif
                        {
                            EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth - 94;
                            materialEditor.ShaderProperty(specularColor, "Color");
                            EditorGUIUtility.labelWidth = oriLabelWidth;
                        }

                        EditorGUILayout.Space();

                        if (rp != RenderPipelineOptions.URP2D && !IsToon2DShader())
                        {
                            EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth - 94;
                            materialEditor.ShaderProperty(specularAffectedByNormalMap, "Affected By NormalMap");
                            EditorGUIUtility.labelWidth = oriLabelWidth;
                        }
                        EditorGUI.indentLevel -= 1;
                        //}


                        EditorGUILayout.Space();
                    }
                    //EditorUtils.DrawUILine(lightGreyForLines);
                    EditorGUILayout.Space();
                }
                else
                {
                    if (rp != RenderPipelineOptions.URP2D && !IsToon2DShader())
                    {
                        m_GeneralShadingLightingFoldout.value = ToonEditorUtils.MakeTheToonShaderSubSectionHeaderWithFoldout(Styles.GeneralShadingLighting, m_GeneralShadingLightingFoldout.value, toonAreaColor);
                        //float oriLabelWidth = EditorGUIUtility.labelWidth;

                        //m_SurfaceOptionsFoldout.value = EditorGUILayout.BeginFoldoutHeaderGroup(m_SurfaceOptionsFoldout.value, Styles.SurfaceOptions);
                        if (m_GeneralShadingLightingFoldout.value)
                        {
                            //EditorUtils.DrawUILine(lightGreyForLines);
                            EditorGUILayout.Space();


                            //shadingMode.floatValue = (int)(GeneralShadingFunction)EditorGUILayout.EnumPopup("Shading Mode", (GeneralShadingMode)shadingMode.floatValue);
                            materialEditor.ShaderProperty(shadingAffectByNormalMap, "Shading Affected By NormalMap");
                            materialEditor.ShaderProperty(specularAffectedByNormalMap, "Specular Affected By NormalMap");
                            //materialEditor.ShaderProperty(specularSmoothness, "Specular Edge Smoothness");


                            EditorGUILayout.Space();
                        }
                    }

                }



                DisableGUIIfOptimizedShader();
                bool showRimOptions = ToonEditorUtils.MakeTheToonShaderSubSectionHeaderWithFoldoutWithToggle(Styles.GeneralShadingRim, m_GeneralShadingRimFoldout, enableRim, toonAreaColor);
                GUI.enabled = true;
                if (showRimOptions)
                {


                    //EditorUtils.DrawUILine(lightGreyForLines);

                    //enableRim.floatValue = Convert.ToSingle(EditorGUILayout.ToggleLeft("Enable Rim", Convert.ToBoolean(enableRim.floatValue)));

                    //if (enableRim.floatValue == 1)
                    //{
                    EditorGUI.indentLevel += 1;
                    EditorGUILayout.Space();
                    rimBlending.floatValue = (int)(BlendingMode)EditorGUILayout.EnumPopup("Blending", (BlendingMode)rimBlending.floatValue);

                    EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth - 94;
                    materialEditor.ShaderProperty(rimColor, "Color");
                    materialEditor.ShaderProperty(rimSize, "Size");
                    EditorGUIUtility.labelWidth = oriLabelWidth;
                    materialEditor.ShaderProperty(rimSmoothness, "Smoothness");
                    materialEditor.ShaderProperty(rimOpacity, "Opacity");



                    EditorGUILayout.Space();
#if USING_URP
                    if (hasAdditionalLights)
                    {
                        rimAffectedArea.floatValue = 2;
                    }
                    else
#endif
                    {
                        rimAffectedArea.floatValue = (int)(RimAffectedRegion)EditorGUILayout.EnumPopup("Rim Affected Region", (RimAffectedRegion)rimAffectedArea.floatValue);
                    }

                    if (rp != RenderPipelineOptions.URP2D && !IsToon2DShader())
                    {
                        EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth - 94;
                        materialEditor.ShaderProperty(rimAffectedByNormalMap, "Affected By NormalMap");
                        EditorGUIUtility.labelWidth = oriLabelWidth;
                    }
                    EditorGUI.indentLevel -= 1;
                    //}

                    EditorGUILayout.Space();

                }

                //EditorUtils.DrawUILine(lightGreyForLines);
                EditorGUILayout.Space();


                //m_GeneralShadingAdvancedFoldout.value = MakeTheToonShaderSubSectionHeaderWithFoldout(Styles.GeneralShadingAdvanced, m_GeneralShadingAdvancedFoldout.value);

                ////m_SurfaceOptionsFoldout.value = EditorGUILayout.BeginFoldoutHeaderGroup(m_SurfaceOptionsFoldout.value, Styles.SurfaceOptions);
                //if (m_GeneralShadingAdvancedFoldout.value)
                //{
                //    EditorUtils.DrawUILine(lightGreyForLines);

                //    materialEditor.ShaderProperty(usePBRLighting, "Use PBR Lighting");
                //    materialEditor.ShaderProperty(lightColorContribution, "Light Color Contribution");
                //    materialEditor.ShaderProperty(ambientColorContribution, "Ambient Color Contribution");

                //    EditorGUILayout.Space();
                //}


            }
            EditorGUILayout.EndFadeGroup();

        }
        GameObject source;



        private StylingSyncMode FindFirstEnabledStyle(StylingSyncMode current)
        {
            if (enableShadingStyling.floatValue == 1 && current != StylingSyncMode.Shading &&
                (StylingSyncMode)stylingShadingSyncWithOtherStyling.floatValue != current)
            {
                return StylingSyncMode.Shading;
            }
            else if (rp != RenderPipelineOptions.URP2D && !IsToon2DShader() && enableCastShadowsStyling.floatValue == 1 && current != StylingSyncMode.CastShadows &&
                (StylingSyncMode)stylingCastShadowsSyncWithOtherStyling.floatValue != current)
            {
                return StylingSyncMode.CastShadows;
            }
            else if (enableSpecularStyling.floatValue == 1 && current != StylingSyncMode.Specular &&
                (StylingSyncMode)stylingSpecularSyncWithOtherStyling.floatValue != current)
            {
                return StylingSyncMode.Specular;
            }
            else if (enableRimStyling.floatValue == 1 && current != StylingSyncMode.Rim &&
                (StylingSyncMode)stylingRimSyncWithOtherStyling.floatValue != current)
            {
                return StylingSyncMode.Rim;
            }
            return StylingSyncMode.None;
        }

        private string[] GetArrayOfAvailableStylingOptions(StylingSyncMode current)
        {
            List<string> styles = new List<string>();
            styles.Add(StylingSyncMode.None.ToString());
            if (enableShadingStyling.floatValue == 1 && current != StylingSyncMode.Shading &&
                (StylingSyncMode)stylingShadingSyncWithOtherStyling.floatValue != current)
            {
                styles.Add(StylingSyncMode.Shading.ToString());
            }

            if (rp != RenderPipelineOptions.URP2D && !IsToon2DShader())
            {
                if (enableCastShadowsStyling.floatValue == 1 && current != StylingSyncMode.CastShadows &&
                (StylingSyncMode)stylingCastShadowsSyncWithOtherStyling.floatValue != current)
                {
                    styles.Add(StylingSyncMode.CastShadows.ToString());
                }
            }

            if (enableSpecularStyling.floatValue == 1 && current != StylingSyncMode.Specular &&
                (StylingSyncMode)stylingSpecularSyncWithOtherStyling.floatValue != current)
            {
                styles.Add(StylingSyncMode.Specular.ToString());
            }

            if (enableRimStyling.floatValue == 1 && current != StylingSyncMode.Rim &&
                (StylingSyncMode)stylingRimSyncWithOtherStyling.floatValue != current)
            {
                styles.Add(StylingSyncMode.Rim.ToString());
            }

            return styles.ToArray();
        }

        private int FromFloatValueToPopUpInt(float floatValue, string[] availableOptions)
        {
            string styleName = ((StylingSyncMode)floatValue).ToString();
            int popUpInt = 0;
            for (int i = 0; i < availableOptions.Length; i++)
            {
                if (availableOptions[i] == styleName)
                {
                    popUpInt = i;
                }
            }
            return popUpInt;
        }

        private float FromPopUpIntToFloatValue(int popUpInt, string[] availableOptions)
        {
            string styleName = availableOptions[popUpInt];
            StylingSyncMode someStylingSyncMode;
            Enum.TryParse(styleName, out someStylingSyncMode);
            float floatValue = (float)someStylingSyncMode;
            return floatValue;

        }





        private void DoShadingStylingArea()
        {
            Color shadingColor = Color.green;


            if (enableShadingStyling.floatValue == 0)
            {
                m_StylingShadingFoldout.value = false;
            }
            DisableGUIIfOptimizedShader(); 
            Tuple<bool,bool> tupleValues = ToonEditorUtils.MakeTheToonShaderSubSectionHeaderWithFoldoutWithToggle(Styles.StylingShading, m_StylingShadingFoldout.value, Convert.ToBoolean(enableShadingStyling.floatValue), shadingColor, 4);
            GUI.enabled = true;
            m_StylingShadingFoldout.value = tupleValues.Item1;
            //enableShadingStyling.floatValue = Convert.ToSingle(tupleValues.Item2);

            float enableShadingStylingFloat = Convert.ToSingle(tupleValues.Item2);
            if (enableShadingStyling.floatValue != enableShadingStylingFloat && enableShadingStylingFloat == 1)
            {
                m_StylingShadingFoldout.value = true;
                //if(rp != RenderPipelineOptions.URP2D)
                //{
                    stylingShadingSyncWithOtherStyling.floatValue = (float)FindFirstEnabledStyle(StylingSyncMode.Shading);

                //}
            }
            enableShadingStyling.floatValue = enableShadingStylingFloat;


            EditorGUILayout.Space();


            if (m_StylingShadingFoldout.value)
            {
                if (enableShadingStyling.floatValue == 1)
                {
                    EditorGUILayout.Space();

                    EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth - 94;
                    materialEditor.ShaderProperty(stylingShadingColor, "Color");
                    EditorGUIUtility.labelWidth = oriLabelWidth;

                    EditorGUILayout.Space();
                    EditorUtils.DrawSubMenuSeparation2();
                    EditorGUILayout.Space();
                    //if (rp != RenderPipelineOptions.URP2D)
                    //{
                    string[] availableOptions = GetArrayOfAvailableStylingOptions(StylingSyncMode.Shading);
                    if (availableOptions.Length > 1)
                    {
                        DisableGUIIfOptimizedShader();
                        stylingShadingSyncWithOtherStyling.floatValue = FromPopUpIntToFloatValue(EditorGUILayout.Popup("Sync with ", FromFloatValueToPopUpInt(stylingShadingSyncWithOtherStyling.floatValue, availableOptions), availableOptions), availableOptions);
                        GUI.enabled = true;
                        EditorGUILayout.Space();

                        if (stylingShadingSyncWithOtherStyling.floatValue != 0)
                        {
                            EditorGUILayout.HelpBox("Shading styling now syncs continuously with "
                            + ((StylingSyncMode)stylingShadingSyncWithOtherStyling.floatValue).ToString()
                            + " Styling, updating automatically.", MessageType.Info);

                            SyncStylingValues((StylingSyncMode)stylingShadingSyncWithOtherStyling.floatValue, StyleAreaMode.Shading);
                            EditorGUILayout.Space();
                        }


                        EditorUtils.DrawSubMenuSeparation2();
                        EditorGUILayout.Space();
                    }
                    else
                    {
                        stylingShadingSyncWithOtherStyling.floatValue = 0;
                    }
                    //}

                    GUI.enabled = stylingShadingSyncWithOtherStyling.floatValue == 0;
                    //shadingStyle.floatValue = GUILayout.Toolbar((int)shadingStyle.floatValue, new string[] {"Hatching", "Halftones", "Texture" });
                    DisableGUIIfOptimizedShader();
                    shadingStyle.floatValue = GUILayout.Toolbar((int)shadingStyle.floatValue, stylingOptions);
                    GUI.enabled = true;
                    EditorGUILayout.Space();

                    //if (shadingStyle.floatValue != 1)
                    {
                        DoStylingPartitioningArea(shadingColor);
                    }
                    if (stylingShadingSyncWithOtherStyling.floatValue == 0)
                    {

                        EditorGUILayout.Space();
                        if (shadingStyle.floatValue != 2)
                        {
                            //EditorUtils.DrawUILine(lightGreyForLines);



                            DoStylingPositionAndBlendingSubArea(StyleAreaMode.Shading, m_ShadingStylingBlendingFoldout, shadingColor);

                            DoStylingDrawSpaceArea(StyleAreaMode.Shading, m_ShadingStylingDrawSpaceFoldout, shadingColor);

                            DoStylingStyleDensityRotationArea((Styling)shadingStyle.floatValue, m_ShadingStylingRotationFoldout, stylingShadingDensity, stylingShadingInitialDirection, stylingShadingRotationBetweenCells, stylingShadingHalftonesOffset, shadingColor);

                            DoStylingShadingStylingArea(shadingColor);

                        }
                        else
                        {
                            EditorGUILayout.LabelField("TODO: Texture Stuff");

                        }

                    }
                    //else
                    //{
                    //    EditorGUILayout.HelpBox("Shading styling now syncs continuously with "
                    //        + ((StylingSyncMode)stylingShadingSyncWithOtherStyling.floatValue).ToString()
                    //        + " Styling, updating automatically.", MessageType.Info);

                    //    SyncStylingValues((StylingSyncMode)stylingShadingSyncWithOtherStyling.floatValue, StyleAreaMode.Shading);
                    //}


                    //EditorUtils.DrawUILine(new Color(0.3f, 0.3f, 0.3f, 1), 16,0);
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();

                }


            }

            
        }


        private void DoCastShadowsStylingArea()
        {
            Color black = Color.black;

            if (enableCastShadowsStyling.floatValue == 0)
            {
                m_StylingCastShadowsFoldout.value = false;
            }

            DisableGUIIfOptimizedShader();
            Tuple<bool, bool> tupleValues = ToonEditorUtils.MakeTheToonShaderSubSectionHeaderWithFoldoutWithToggle(Styles.StylingCastShadows, m_StylingCastShadowsFoldout.value, Convert.ToBoolean(enableCastShadowsStyling.floatValue), black, 4);
            GUI.enabled = true;
            m_StylingCastShadowsFoldout.value = tupleValues.Item1;  

            float enableStylingCastShadowsFloat = Convert.ToSingle(tupleValues.Item2);
            if (enableCastShadowsStyling.floatValue != enableStylingCastShadowsFloat && enableStylingCastShadowsFloat == 1)
            {
                m_StylingCastShadowsFoldout.value = true;
                stylingCastShadowsSyncWithOtherStyling.floatValue = (float)FindFirstEnabledStyle(StylingSyncMode.CastShadows);
            }
            enableCastShadowsStyling.floatValue = enableStylingCastShadowsFloat;



            //EditorUtils.DrawUILine(Color.white);

            EditorGUILayout.Space();

            if (m_StylingCastShadowsFoldout.value)
            {
                if (enableCastShadowsStyling.floatValue == 1)
                {
                    if(rp == RenderPipelineOptions.URP)
                    {
                        EditorGUILayout.HelpBox("If you see weird artifacts, please check your Universal Render Pipeline Asset -> Shadows -> Depth Bias! You can completely get rid of them by adjusting the bias correctly!", MessageType.Info);
                    }


                    if (stylingCastShadowsSyncWithOtherStyling.floatValue != 1)
                    {
                        EditorGUILayout.Space();
                        EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth - 94;
                        materialEditor.ShaderProperty(stylingCastShadowsColor, "Color");
                        EditorGUIUtility.labelWidth = oriLabelWidth;
                    }
                    EditorGUILayout.Space();
                    EditorUtils.DrawSubMenuSeparation2();
                    EditorGUILayout.Space();

                    string[] availableOptions = GetArrayOfAvailableStylingOptions(StylingSyncMode.CastShadows);
                    if (availableOptions.Length > 1)
                    {
                        DisableGUIIfOptimizedShader();
                        stylingCastShadowsSyncWithOtherStyling.floatValue = FromPopUpIntToFloatValue(EditorGUILayout.Popup("Sync with ", FromFloatValueToPopUpInt(stylingCastShadowsSyncWithOtherStyling.floatValue, availableOptions), availableOptions), availableOptions);
                        GUI.enabled = true;

                        EditorGUILayout.Space();

                        if (stylingCastShadowsSyncWithOtherStyling.floatValue != 0)
                        {

                            EditorGUILayout.HelpBox("Cast shadows styling now syncs continuously with "
                            + ((StylingSyncMode)stylingCastShadowsSyncWithOtherStyling.floatValue).ToString()
                            + " Styling, updating automatically.", MessageType.Info);

                            SyncStylingValues((StylingSyncMode)stylingCastShadowsSyncWithOtherStyling.floatValue, StyleAreaMode.CastShadows);

                            EditorGUILayout.Space();

                        }

                        EditorUtils.DrawSubMenuSeparation2();
                        EditorGUILayout.Space();
                    }
                    else
                    {
                        stylingCastShadowsSyncWithOtherStyling.floatValue = 0;
                    }

                    GUI.enabled = stylingCastShadowsSyncWithOtherStyling.floatValue == 0;
                    DisableGUIIfOptimizedShader();
                    castShadowsStyle.floatValue = GUILayout.Toolbar((int)castShadowsStyle.floatValue, stylingOptions);
                    GUI.enabled = true;
                    EditorGUILayout.Space();

                    if (castShadowsStyle.floatValue != 1)
                    {
                        DoStylingCastShadowsPartitioningAndSettingsArea(black);
                    }

                    //EditorGUILayout.Space();
                    if (stylingCastShadowsSyncWithOtherStyling.floatValue == 0)
                    {

                        //EditorGUILayout.Space();
                        if (castShadowsStyle.floatValue != 2)
                        {

                            DoStylingPositionAndBlendingSubArea(StyleAreaMode.CastShadows, m_CastShadowsStylingBlendingFoldout, black);

                            DoStylingDrawSpaceArea(StyleAreaMode.CastShadows, m_CastShadowsStylingDrawSpaceFoldout, black);

                            //DoStylingStyleDensityRotationArea((Styling)specularStyle.floatValue, stylingSpecularDensity);
                            DoStylingStyleDensityRotationArea((Styling)castShadowsStyle.floatValue, m_CastShadowsStylingRotationFoldout, stylingCastShadowsDensity, stylingCastShadowsInitialDirection, stylingCastShadowsRotationBetweenCells, stylingCastShadowsHalftonesOffset, black);

                            DoStylingCastShadowsStylingArea(black);

                        }
                        else
                        {
                            EditorGUILayout.LabelField("TODO: Texture Stuff");

                        }
                    } 
                    //else
                    //{
                    //    EditorGUILayout.HelpBox("Cast shadows styling now syncs continuously with " 
                    //        + ((StylingSyncMode)stylingCastShadowsSyncWithOtherStyling.floatValue).ToString() 
                    //        + " Styling, updating automatically.", MessageType.Info);

                    //    SyncStylingValues((StylingSyncMode)stylingCastShadowsSyncWithOtherStyling.floatValue, StyleAreaMode.CastShadows);
                    //}


                    EditorGUILayout.Space();
                    EditorGUILayout.Space();
                }


            }

        }

        
        private void SyncStylingValues(StylingSyncMode from, StyleAreaMode to)
        {
            if(from != StylingSyncMode.None)
            {
                float numberOfCellsHatchingTmp = 1;
                float terminatorPositionTmp = 0;

                // STYLE
                float styleTmp = 0;

                // BLENDING
                float blendingTmp = 0;
                float isInvertedTmp = 0;

                // DRAW SPACE
                float drawSpaceTmp = 0;
                float coordinateSystemTmp = 0;
                float polarCenterModeTmp = 0;
                Vector4 polarCenterTmp = Vector4.zero;
                float sSCameraDistanceScaledTmp = 0;
                float anchorSSToObjectsOriginTmp = 0;

                // DENSITY AND DIRECTION
                float densityTmp = 0;
                float initialDirectionTmp = 0;
                float rotationBetweenCellsTmp = 0;
                float halftonesOffsetTmp = 0;

                // STYLE SETTINGS
                //float colorTmp = 0;
                float opacityTmp = 0;
                float opacityFalloffTmp = 0;
                float thicknessControlTmp = 0;
                float thicknessTmp = 0;
                float thicknessFalloffTmp = 0;
                float hardnessTmp = 0;
                float halftonesRoundnessTmp = 0;
                float halftonesRoundnessFalloffTmp = 0;

                // RANDOMIZER
                float enableRandomizerTmp = 0;
                float perlinNoiseSizeTmp = 0;
                float perlinNoiseSeedTmp = 0;
                float whiteNoiseSeedTmp = 0;
                float noiseIntensityTmp = 0;
                float spacingRandomModeTmp = 0;
                float spacingRandomIntensityTmp = 0;
                float opacityRandomModeTmp = 0;
                float opacityRandomIntensityTmp = 0;
                float hardnessRandomModeTmp = 0;
                float hardnessRandomIntensityTmp = 0;
                float lengthRandomModeTmp = 0;
                float lengthRandomIntensityTmp = 0;
                float thicknessRandomModeTmp = 0;
                float thicknessRandomIntensityTmp = 0;


                if (from == StylingSyncMode.Shading)
                {
                    // PARTITIONING
                    numberOfCellsHatchingTmp = numberOfCellsHatching.floatValue;
                    terminatorPositionTmp = stylingTerminatorPosition.floatValue;

                    // STYLE
                    styleTmp = shadingStyle.floatValue;

                    // BLENDING
                    blendingTmp = stylingShadingBlending.floatValue;
                    isInvertedTmp = stylingShadingIsInverted.floatValue;

                    // DRAW SPACE
                    drawSpaceTmp = drawSpace.floatValue;
                    coordinateSystemTmp = coordinateSystem.floatValue;
                    polarCenterModeTmp = polarCenterMode.floatValue;
                    polarCenterTmp = polarCenter.vectorValue;
                    sSCameraDistanceScaledTmp = sSCameraDistanceScaled.floatValue;
                    anchorSSToObjectsOriginTmp = anchorSSToObjectsOrigin.floatValue;

                    // DENSITY AND DIRECTION
                    densityTmp = stylingShadingDensity.floatValue;
                    initialDirectionTmp = stylingShadingInitialDirection.floatValue;
                    rotationBetweenCellsTmp = stylingShadingRotationBetweenCells.floatValue;
                    halftonesOffsetTmp = stylingShadingHalftonesOffset.floatValue;

                    // STYLE SETTINGS
                    //colorTmp = stylingShadingColor.floatValue;
                    opacityTmp = stylingShadingOpacity.floatValue;
                    opacityFalloffTmp = stylingShadingOpacityFalloff.floatValue;
                    thicknessControlTmp = stylingShadingThicknessControl.floatValue;
                    thicknessTmp = stylingShadingThickness.floatValue;
                    thicknessFalloffTmp = stylingShadingThicknessFalloff.floatValue;
                    hardnessTmp = stylingShadingHardness.floatValue;
                    halftonesRoundnessTmp = stylingShadingHalftonesRoundness.floatValue;
                    halftonesRoundnessFalloffTmp = stylingShadingHalftonesRoundnessFalloff.floatValue;

                    // RANDOMIZER
                    enableRandomizerTmp = enableShadingRandomizer.floatValue;
                    perlinNoiseSizeTmp = shadingNoise1Size.floatValue;
                    perlinNoiseSeedTmp = shadingNoise1Seed.floatValue;
                    whiteNoiseSeedTmp = shadingNoise2Seed.floatValue;
                    noiseIntensityTmp = noiseIntensity.floatValue;
                    spacingRandomModeTmp = spacingRandomMode.floatValue;
                    spacingRandomIntensityTmp = spacingRandomIntensity.floatValue;
                    opacityRandomModeTmp = opacityRandomMode.floatValue;
                    opacityRandomIntensityTmp = opacityRandomIntensity.floatValue;
                    hardnessRandomModeTmp = hardnessRandomMode.floatValue;
                    hardnessRandomIntensityTmp = hardnessRandomIntensity.floatValue;
                    lengthRandomModeTmp = lengthRandomMode.floatValue;
                    lengthRandomIntensityTmp = lengthRandomIntensity.floatValue;
                    thicknessRandomModeTmp = thicknessRandomMode.floatValue;
                    thicknessRandomIntensityTmp = thicknessRandomIntensity.floatValue;
                }
                else if (rp != RenderPipelineOptions.URP2D && !IsToon2DShader() && from == StylingSyncMode.CastShadows)
                {
                    // PARTITIONING
                    numberOfCellsHatchingTmp = stylingCastShadowsNumberOfCellsHatching.floatValue;

                    // STYLE
                    styleTmp = castShadowsStyle.floatValue;

                    // BLENDING 
                    blendingTmp = stylingCastShadowsBlending.floatValue;
                    isInvertedTmp = stylingCastShadowsIsInverted.floatValue;

                    // DRAW SPACE 
                    drawSpaceTmp = castShadowsDrawSpace.floatValue;
                    coordinateSystemTmp = castShadowsCoordinateSystem.floatValue;
                    polarCenterModeTmp = castShadowsPolarCenterMode.floatValue;
                    polarCenterTmp = castShadowsPolarCenter.vectorValue;
                    sSCameraDistanceScaledTmp = castShadowsSSCameraDistanceScaled.floatValue;
                    anchorSSToObjectsOriginTmp = castShadowsAnchorSSToObjectsOrigin.floatValue;

                    // DENSITY AND DIRECTION 
                    densityTmp = stylingCastShadowsDensity.floatValue;
                    initialDirectionTmp = stylingCastShadowsInitialDirection.floatValue;
                    rotationBetweenCellsTmp = stylingCastShadowsRotationBetweenCells.floatValue;
                    halftonesOffsetTmp = stylingCastShadowsHalftonesOffset.floatValue;

                    // STYLE SETTINGS 
                    //colorTmp = stylingCastShadowsColor.floatValue;
                    opacityTmp = stylingCastShadowsOpacity.floatValue;
                    opacityFalloffTmp = stylingCastShadowsOpacityFalloff.floatValue;
                    thicknessControlTmp = stylingCastShadowsThicknessControl.floatValue;
                    thicknessTmp = stylingCastShadowsThickness.floatValue;
                    thicknessFalloffTmp = stylingCastShadowsThicknessFalloff.floatValue;
                    hardnessTmp = stylingCastShadowsHardness.floatValue;
                    halftonesRoundnessTmp = stylingCastShadowsHalftonesRoundness.floatValue;
                    halftonesRoundnessFalloffTmp = stylingCastShadowsHalftonesRoundnessFalloff.floatValue;

                    // RANDOMIZER
                    enableRandomizerTmp = enableCastShadowsRandomizer.floatValue;
                    perlinNoiseSizeTmp = castShadowsNoise1Size.floatValue;
                    perlinNoiseSeedTmp = castShadowsNoise1Seed.floatValue;
                    whiteNoiseSeedTmp = castShadowsNoise2Seed.floatValue;
                    noiseIntensityTmp = noiseIntensityCastShadows.floatValue;
                    spacingRandomModeTmp = spacingRandomModeCastShadows.floatValue;
                    spacingRandomIntensityTmp = spacingRandomIntensityCastShadows.floatValue;
                    opacityRandomModeTmp = opacityRandomModeCastShadows.floatValue;
                    opacityRandomIntensityTmp = opacityRandomIntensityCastShadows.floatValue;
                    hardnessRandomModeTmp = hardnessRandomModeCastShadows.floatValue;
                    hardnessRandomIntensityTmp = hardnessRandomIntensityCastShadows.floatValue;
                    lengthRandomModeTmp = lengthRandomModeCastShadows.floatValue;
                    lengthRandomIntensityTmp = lengthRandomIntensityCastShadows.floatValue;
                    thicknessRandomModeTmp = thicknessRandomModeCastShadows.floatValue;
                    thicknessRandomIntensityTmp = thicknessRandomIntensityCastShadows.floatValue;
                }
                else if (from == StylingSyncMode.Specular)
                {
                    // PARTITIONING
                    numberOfCellsHatchingTmp = 1;

                    // STYLE
                    styleTmp = specularStyle.floatValue;

                    // BLENDING 
                    blendingTmp = stylingSpecularBlending.floatValue;
                    isInvertedTmp = stylingSpecularIsInverted.floatValue;

                    // DRAW SPACE 
                    drawSpaceTmp = specularDrawSpace.floatValue;
                    coordinateSystemTmp = specularCoordinateSystem.floatValue;
                    polarCenterModeTmp = specularPolarCenterMode.floatValue;
                    polarCenterTmp = specularPolarCenter.vectorValue;
                    sSCameraDistanceScaledTmp = specularSSCameraDistanceScaled.floatValue;
                    anchorSSToObjectsOriginTmp = specularAnchorSSToObjectsOrigin.floatValue;

                    // DENSITY AND DIRECTION 
                    densityTmp = stylingSpecularDensity.floatValue;
                    initialDirectionTmp = stylingSpecularRotation.floatValue;
                    rotationBetweenCellsTmp = initialDirectionTmp + 40;
                    halftonesOffsetTmp = stylingSpecularHalftonesOffset.floatValue;

                    // STYLE SETTINGS 
                    //colorTmp = stylingSpecularColor.floatValue;
                    opacityTmp = stylingSpecularOpacity.floatValue;
                    opacityFalloffTmp = stylingSpecularOpacityFalloff.floatValue;
                    thicknessControlTmp = stylingSpecularThicknessControl.floatValue;
                    thicknessTmp = stylingSpecularThickness.floatValue;
                    thicknessFalloffTmp = stylingSpecularThicknessFalloff.floatValue;
                    hardnessTmp = stylingSpecularHardness.floatValue;
                    halftonesRoundnessTmp = stylingSpecularHalftonesRoundness.floatValue;
                    halftonesRoundnessFalloffTmp = stylingSpecularHalftonesRoundnessFalloff.floatValue;

                    // RANDOMIZER
                    enableRandomizerTmp = enableSpecularRandomizer.floatValue;
                    perlinNoiseSizeTmp = noise1SizeSpecular.floatValue;
                    perlinNoiseSeedTmp = noise1SeedSpecular.floatValue;
                    whiteNoiseSeedTmp = noise2SeedSpecular.floatValue;
                    noiseIntensityTmp = noiseIntensitySpecular.floatValue;
                    spacingRandomModeTmp = spacingRandomModeSpecular.floatValue;
                    spacingRandomIntensityTmp = spacingRandomIntensitySpecular.floatValue;
                    opacityRandomModeTmp = opacityRandomModeSpecular.floatValue;
                    opacityRandomIntensityTmp = opacityRandomIntensitySpecular.floatValue;
                    hardnessRandomModeTmp = hardnessRandomModeSpecular.floatValue;
                    hardnessRandomIntensityTmp = hardnessRandomIntensitySpecular.floatValue;
                    lengthRandomModeTmp = lengthRandomModeSpecular.floatValue;
                    lengthRandomIntensityTmp = lengthRandomIntensitySpecular.floatValue;
                    thicknessRandomModeTmp = thicknessRandomModeSpecular.floatValue;
                    thicknessRandomIntensityTmp = thicknessRandomIntensitySpecular.floatValue;
                }
                else if (from == StylingSyncMode.Rim)
                {
                    // PARTITIONING
                    numberOfCellsHatchingTmp = 1;

                    // STYLE
                    styleTmp = rimStyle.floatValue;

                    // BLENDING 
                    blendingTmp = stylingRimBlending.floatValue;
                    isInvertedTmp = stylingRimIsInverted.floatValue;

                    // DRAW SPACE 
                    drawSpaceTmp = rimDrawSpace.floatValue;
                    coordinateSystemTmp = rimCoordinateSystem.floatValue;
                    polarCenterModeTmp = rimPolarCenterMode.floatValue;
                    polarCenterTmp = rimPolarCenter.vectorValue;
                    sSCameraDistanceScaledTmp = rimSSCameraDistanceScaled.floatValue;
                    anchorSSToObjectsOriginTmp = rimAnchorSSToObjectsOrigin.floatValue;

                    // DENSITY AND DIRECTION 
                    densityTmp = stylingRimDensity.floatValue;
                    initialDirectionTmp = stylingRimRotation.floatValue;
                    rotationBetweenCellsTmp = initialDirectionTmp + 40;
                    halftonesOffsetTmp = stylingRimHalftonesOffset.floatValue;

                    // STYLE SETTINGS 
                    //colorTmp = stylingRimColor.floatValue;
                    opacityTmp = stylingRimOpacity.floatValue;
                    opacityFalloffTmp = stylingRimOpacityFalloff.floatValue;
                    thicknessControlTmp = stylingRimThicknessControl.floatValue;
                    thicknessTmp = stylingRimThickness.floatValue;
                    thicknessFalloffTmp = stylingRimThicknessFalloff.floatValue;
                    hardnessTmp = stylingRimHardness.floatValue;
                    halftonesRoundnessTmp = stylingRimHalftonesRoundness.floatValue;
                    halftonesRoundnessFalloffTmp = stylingRimHalftonesRoundnessFalloff.floatValue;

                    // RANDOMIZER
                    enableRandomizerTmp = enableRimRandomizer.floatValue;
                    perlinNoiseSizeTmp = rimNoise1Size.floatValue;
                    perlinNoiseSeedTmp = rimNoise1Seed.floatValue;
                    whiteNoiseSeedTmp = rimNoise2Seed.floatValue;
                    noiseIntensityTmp = noiseIntensityRim.floatValue;
                    spacingRandomModeTmp = spacingRandomModeRim.floatValue;
                    spacingRandomIntensityTmp = spacingRandomIntensityRim.floatValue;
                    opacityRandomModeTmp = opacityRandomModeRim.floatValue;
                    opacityRandomIntensityTmp = opacityRandomIntensityRim.floatValue;
                    hardnessRandomModeTmp = hardnessRandomModeRim.floatValue;
                    hardnessRandomIntensityTmp = hardnessRandomIntensityRim.floatValue;
                    lengthRandomModeTmp = lengthRandomModeRim.floatValue;
                    lengthRandomIntensityTmp = lengthRandomIntensityRim.floatValue;
                    thicknessRandomModeTmp = thicknessRandomModeRim.floatValue;
                    thicknessRandomIntensityTmp = thicknessRandomIntensityRim.floatValue;
                }


                if (to == StyleAreaMode.Shading)
                {
                    // PARTITIONING
                    numberOfCellsHatching.floatValue = numberOfCellsHatchingTmp;

                    // STYLE
                    shadingStyle.floatValue = styleTmp;

                    // BLENDING
                    stylingShadingBlending.floatValue = blendingTmp;
                    stylingShadingIsInverted.floatValue = isInvertedTmp;

                    // DRAW SPACE
                    drawSpace.floatValue = drawSpaceTmp;
                    coordinateSystem.floatValue = coordinateSystemTmp;
                    polarCenterMode.floatValue = polarCenterModeTmp;
                    polarCenter.vectorValue = polarCenterTmp;
                    sSCameraDistanceScaled.floatValue = sSCameraDistanceScaledTmp;
                    anchorSSToObjectsOrigin.floatValue = anchorSSToObjectsOriginTmp;

                    // DENSITY AND DIRECTION
                    stylingShadingDensity.floatValue = densityTmp;
                    stylingShadingInitialDirection.floatValue = initialDirectionTmp;
                    stylingShadingRotationBetweenCells.floatValue = rotationBetweenCellsTmp;
                    stylingShadingHalftonesOffset.floatValue = halftonesOffsetTmp;

                    // STYLE SETTINGS
                    //stylingShadingColor.floatValue = colorTmp;
                    stylingShadingOpacity.floatValue = opacityTmp;
                    stylingShadingOpacityFalloff.floatValue = opacityFalloffTmp;
                    stylingShadingThicknessControl.floatValue = thicknessControlTmp;
                    stylingShadingThickness.floatValue = thicknessTmp;
                    stylingShadingThicknessFalloff.floatValue = thicknessFalloffTmp;
                    stylingShadingHardness.floatValue = hardnessTmp;
                    stylingShadingHalftonesRoundness.floatValue = halftonesRoundnessTmp;
                    stylingShadingHalftonesRoundnessFalloff.floatValue = halftonesRoundnessFalloffTmp;

                    // RANDOMIZER
                    enableShadingRandomizer.floatValue = enableRandomizerTmp;
                    shadingNoise1Size.floatValue = perlinNoiseSizeTmp;
                    shadingNoise1Seed.floatValue = perlinNoiseSeedTmp;
                    shadingNoise2Seed.floatValue = whiteNoiseSeedTmp;
                    noiseIntensity.floatValue = noiseIntensityTmp;
                    spacingRandomMode.floatValue = spacingRandomModeTmp;
                    spacingRandomIntensity.floatValue = spacingRandomIntensityTmp;
                    opacityRandomMode.floatValue = opacityRandomModeTmp;
                    opacityRandomIntensity.floatValue = opacityRandomIntensityTmp;
                    hardnessRandomMode.floatValue = hardnessRandomModeTmp;
                    hardnessRandomIntensity.floatValue = hardnessRandomIntensityTmp;
                    lengthRandomMode.floatValue = lengthRandomModeTmp;
                    lengthRandomIntensity.floatValue = lengthRandomIntensityTmp;
                    thicknessRandomMode.floatValue = thicknessRandomModeTmp;
                    thicknessRandomIntensity.floatValue = thicknessRandomIntensityTmp;
                }
                else if (rp != RenderPipelineOptions.URP2D && !IsToon2DShader() && to == StyleAreaMode.CastShadows)
                {
                    // PARTITIONING 
                    stylingCastShadowsNumberOfCellsHatching.floatValue = numberOfCellsHatchingTmp;

                    // STYLE
                    castShadowsStyle.floatValue = styleTmp;

                    // BLENDING 
                    stylingCastShadowsBlending.floatValue = blendingTmp;
                    stylingCastShadowsIsInverted.floatValue = isInvertedTmp;

                    // DRAW SPACE 
                    castShadowsDrawSpace.floatValue = drawSpaceTmp;
                    castShadowsCoordinateSystem.floatValue = coordinateSystemTmp;
                    castShadowsPolarCenterMode.floatValue = polarCenterModeTmp;
                    castShadowsPolarCenter.vectorValue = polarCenterTmp;
                    castShadowsSSCameraDistanceScaled.floatValue = sSCameraDistanceScaledTmp;
                    castShadowsAnchorSSToObjectsOrigin.floatValue = anchorSSToObjectsOriginTmp;

                    // DENSITY AND DIRECTION 
                    stylingCastShadowsDensity.floatValue = densityTmp;
                    stylingCastShadowsInitialDirection.floatValue = initialDirectionTmp;
                    stylingCastShadowsRotationBetweenCells.floatValue = rotationBetweenCellsTmp;
                    stylingCastShadowsHalftonesOffset.floatValue = halftonesOffsetTmp;

                    // STYLE SETTINGS 
                    //stylingCastShadowsColor.floatValue = colorTmp;
                    stylingCastShadowsOpacity.floatValue = opacityTmp;
                    stylingCastShadowsOpacityFalloff.floatValue = opacityFalloffTmp;
                    stylingCastShadowsThicknessControl.floatValue = thicknessControlTmp;
                    stylingCastShadowsThickness.floatValue = thicknessTmp;
                    stylingCastShadowsThicknessFalloff.floatValue = thicknessFalloffTmp;
                    stylingCastShadowsHardness.floatValue = hardnessTmp;
                    stylingCastShadowsHalftonesRoundness.floatValue = halftonesRoundnessTmp;
                    stylingCastShadowsHalftonesRoundnessFalloff.floatValue = halftonesRoundnessFalloffTmp;

                    // RANDOMIZER
                    enableCastShadowsRandomizer.floatValue = enableRandomizerTmp;
                    castShadowsNoise1Size.floatValue = perlinNoiseSizeTmp;
                    castShadowsNoise1Seed.floatValue = perlinNoiseSeedTmp;
                    castShadowsNoise2Seed.floatValue = whiteNoiseSeedTmp;
                    noiseIntensityCastShadows.floatValue = noiseIntensityTmp;
                    spacingRandomModeCastShadows.floatValue = spacingRandomModeTmp;
                    spacingRandomIntensityCastShadows.floatValue = spacingRandomIntensityTmp;
                    opacityRandomModeCastShadows.floatValue = opacityRandomModeTmp;
                    opacityRandomIntensityCastShadows.floatValue = opacityRandomIntensityTmp;
                    hardnessRandomModeCastShadows.floatValue = hardnessRandomModeTmp;
                    hardnessRandomIntensityCastShadows.floatValue = hardnessRandomIntensityTmp;
                    lengthRandomModeCastShadows.floatValue = lengthRandomModeTmp;
                    lengthRandomIntensityCastShadows.floatValue = lengthRandomIntensityTmp;
                    thicknessRandomModeCastShadows.floatValue = thicknessRandomModeTmp;
                    thicknessRandomIntensityCastShadows.floatValue = thicknessRandomIntensityTmp;

                }
                else if (to == StyleAreaMode.Specular)
                {
                    // STYLE
                    specularStyle.floatValue = styleTmp;

                    // BLENDING 
                    stylingSpecularBlending.floatValue = blendingTmp;
                    stylingSpecularIsInverted.floatValue = isInvertedTmp;

                    // DRAW SPACE 
                    specularDrawSpace.floatValue = drawSpaceTmp;
                    specularCoordinateSystem.floatValue = coordinateSystemTmp;
                    specularPolarCenterMode.floatValue = polarCenterModeTmp;
                    specularPolarCenter.vectorValue = polarCenterTmp;
                    //if (rp != RenderPipelineOptions.URP2D && !IsToon2DShader())
                    //{
                        specularSSCameraDistanceScaled.floatValue = sSCameraDistanceScaledTmp;
                    //}
                    specularAnchorSSToObjectsOrigin.floatValue = anchorSSToObjectsOriginTmp;

                    // DENSITY AND DIRECTION 
                    stylingSpecularDensity.floatValue = densityTmp;
                    stylingSpecularRotation.floatValue = initialDirectionTmp;
                    stylingSpecularHalftonesOffset.floatValue = halftonesOffsetTmp;

                    // STYLE SETTINGS 
                    //stylingSpecularColor.floatValue = colorTmp;
                    stylingSpecularOpacity.floatValue = opacityTmp;
                    stylingSpecularOpacityFalloff.floatValue = opacityFalloffTmp;
                    stylingSpecularThicknessControl.floatValue = thicknessControlTmp;
                    stylingSpecularThickness.floatValue = thicknessTmp;
                    stylingSpecularThicknessFalloff.floatValue = thicknessFalloffTmp;
                    stylingSpecularHardness.floatValue = hardnessTmp;
                    stylingSpecularHalftonesRoundness.floatValue = halftonesRoundnessTmp;
                    stylingSpecularHalftonesRoundnessFalloff.floatValue = halftonesRoundnessFalloffTmp;

                    // RANDOMIZER
                    enableSpecularRandomizer.floatValue = enableRandomizerTmp;
                    noise1SizeSpecular.floatValue = perlinNoiseSizeTmp;
                    noise1SeedSpecular.floatValue = perlinNoiseSeedTmp;
                    noise2SeedSpecular.floatValue = whiteNoiseSeedTmp;
                    noiseIntensitySpecular.floatValue = noiseIntensityTmp;
                    spacingRandomModeSpecular.floatValue = spacingRandomModeTmp;
                    spacingRandomIntensitySpecular.floatValue = spacingRandomIntensityTmp;
                    opacityRandomModeSpecular.floatValue = opacityRandomModeTmp;
                    opacityRandomIntensitySpecular.floatValue = opacityRandomIntensityTmp;
                    hardnessRandomModeSpecular.floatValue = hardnessRandomModeTmp;
                    hardnessRandomIntensitySpecular.floatValue = hardnessRandomIntensityTmp;
                    lengthRandomModeSpecular.floatValue = lengthRandomModeTmp;
                    lengthRandomIntensitySpecular.floatValue = lengthRandomIntensityTmp;
                    thicknessRandomModeSpecular.floatValue = thicknessRandomModeTmp;
                    thicknessRandomIntensitySpecular.floatValue = thicknessRandomIntensityTmp;

                }
                else if (to == StyleAreaMode.Rim)
                {
                    // STYLE
                    rimStyle.floatValue = styleTmp;

                    // BLENDING 
                    stylingRimBlending.floatValue = blendingTmp;
                    stylingRimIsInverted.floatValue = isInvertedTmp;

                    // DRAW SPACE 
                    rimDrawSpace.floatValue = drawSpaceTmp;
                    rimCoordinateSystem.floatValue = coordinateSystemTmp;
                    rimPolarCenterMode.floatValue = polarCenterModeTmp;
                    rimPolarCenter.vectorValue = polarCenterTmp;
                    rimSSCameraDistanceScaled.floatValue = sSCameraDistanceScaledTmp;
                    rimAnchorSSToObjectsOrigin.floatValue = anchorSSToObjectsOriginTmp;

                    // DENSITY AND DIRECTION 
                    stylingRimDensity.floatValue = densityTmp;
                    stylingRimRotation.floatValue = initialDirectionTmp;
                    stylingRimHalftonesOffset.floatValue = halftonesOffsetTmp;

                    // STYLE SETTINGS 
                    //stylingRimColor.floatValue = colorTmp;
                    stylingRimOpacity.floatValue = opacityTmp;
                    stylingRimOpacityFalloff.floatValue = opacityFalloffTmp;
                    stylingRimThicknessControl.floatValue = thicknessControlTmp;
                    stylingRimThickness.floatValue = thicknessTmp;
                    stylingRimThicknessFalloff.floatValue = thicknessFalloffTmp;
                    stylingRimHardness.floatValue = hardnessTmp;
                    stylingRimHalftonesRoundness.floatValue = halftonesRoundnessTmp;
                    stylingRimHalftonesRoundnessFalloff.floatValue = halftonesRoundnessFalloffTmp;

                    // RANDOMIZER
                    enableRimRandomizer.floatValue = enableRandomizerTmp;
                    rimNoise1Size.floatValue = perlinNoiseSizeTmp;
                    rimNoise1Seed.floatValue = perlinNoiseSeedTmp;
                    rimNoise2Seed.floatValue = whiteNoiseSeedTmp;
                    noiseIntensityRim.floatValue = noiseIntensityTmp;
                    spacingRandomModeRim.floatValue = spacingRandomModeTmp;
                    spacingRandomIntensityRim.floatValue = spacingRandomIntensityTmp;
                    opacityRandomModeRim.floatValue = opacityRandomModeTmp;
                    opacityRandomIntensityRim.floatValue = opacityRandomIntensityTmp;
                    hardnessRandomModeRim.floatValue = hardnessRandomModeTmp;
                    hardnessRandomIntensityRim.floatValue = hardnessRandomIntensityTmp;
                    lengthRandomModeRim.floatValue = lengthRandomModeTmp;
                    lengthRandomIntensityRim.floatValue = lengthRandomIntensityTmp;
                    thicknessRandomModeRim.floatValue = thicknessRandomModeTmp;
                    thicknessRandomIntensityRim.floatValue = thicknessRandomIntensityTmp;

                }
            }
        }



        private void DoSpecularStylingArea()
        {
            Color green = Color.blue;

            if (enableSpecularStyling.floatValue == 0)
            {
                m_StylingSpecularFoldout.value = false;
            }
            DisableGUIIfOptimizedShader();
            Tuple<bool, bool> tupleValues = ToonEditorUtils.MakeTheToonShaderSubSectionHeaderWithFoldoutWithToggle(Styles.StylingSpecular, m_StylingSpecularFoldout.value, Convert.ToBoolean(enableSpecularStyling.floatValue), green, 4);
            GUI.enabled = true;

            m_StylingSpecularFoldout.value = tupleValues.Item1;

            float enableSpecularStylingFloat = Convert.ToSingle(tupleValues.Item2);
            if (enableSpecularStyling.floatValue != enableSpecularStylingFloat && enableSpecularStylingFloat == 1)
            {
                m_StylingSpecularFoldout.value = true;
                stylingSpecularSyncWithOtherStyling.floatValue = (float)FindFirstEnabledStyle(StylingSyncMode.Specular);

            }
            enableSpecularStyling.floatValue = enableSpecularStylingFloat;

            EditorGUILayout.Space();

            if (m_StylingSpecularFoldout.value)
            {
                if (enableSpecularStyling.floatValue != 0)
                {

#if UNITY_EDITOR && USING_URP && USING_TOON2D
                    if (rp == RenderPipelineOptions.URP2D)
                    {
                        //var go = Selection.activeGameObject;
                        //if (go != null && go.GetComponent<TheToonShaderSprite>() == null)
                        //{
                        //    EditorGUILayout.HelpBox("This Sprite doesn't have a TheToonShaderSprite component attached to it. Without it Specular won't work!", MessageType.Warning);
                        //}
                        Toon2DLightManager toon2DLightManager = UnityEngine.Object.FindFirstObjectByType<Toon2DLightManager>(FindObjectsInactive.Include);
                        if (toon2DLightManager == null)
                        {
                            EditorGUILayout.HelpBox("You haven't added the Toon2DLightManager Component to your scene. Without it Specular won't work!", MessageType.Warning);
                        }
                        else if (toon2DLightManager.enabled == false)
                        {
                            EditorGUILayout.HelpBox("Your Toon2DLightManager Component on the GameObject \"" + toon2DLightManager.gameObject.name + "\" is not active, please enable it for Specular to work!", MessageType.Warning);
                        }
                        else if (toon2DLightManager.gameObject.activeInHierarchy == false)
                        {
                            EditorGUILayout.HelpBox("Your GameObject \"" + toon2DLightManager.gameObject.name + "\" with the Toon2DLightManager Component on is not active, please enable it for Specular to work!", MessageType.Warning);
                        }

                        EditorGUILayout.Space();
                    }
#endif

                    EditorGUILayout.Space();
                    DoStylingSpecularSettingsArea(green);
                    EditorGUILayout.Space();
                    EditorUtils.DrawSubMenuSeparation();

                    materialEditor.ShaderProperty(stylingSpecularCutOutShading, "Cut out shading");


#if USING_URP
                    stylingSpecularUseLightColors.floatValue = Convert.ToSingle(EditorGUILayout.ToggleLeft("Use Light Colors", Convert.ToBoolean(stylingSpecularUseLightColors.floatValue)));
                    if(stylingSpecularUseLightColors.floatValue == 0)
#endif
                    {
                        EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth - 94;
                        materialEditor.ShaderProperty(stylingSpecularColor, "Color");
                        EditorGUIUtility.labelWidth = oriLabelWidth;
                    }
#if USING_URP
                    else
                    {
                        //materialEditor.ShaderProperty(stylingSpecularColorBoost, "Color Boost");                        
                    }
#endif

                    EditorGUILayout.Space();
                    EditorUtils.DrawSubMenuSeparation2();
                    EditorGUILayout.Space();
                    //if (rp != RenderPipelineOptions.URP2D)
                    //{
                    string[] availableOptions = GetArrayOfAvailableStylingOptions(StylingSyncMode.Specular);
                    if (availableOptions.Length > 1)
                    {
                        DisableGUIIfOptimizedShader();
                        stylingSpecularSyncWithOtherStyling.floatValue = FromPopUpIntToFloatValue(EditorGUILayout.Popup("Sync with ", FromFloatValueToPopUpInt(stylingSpecularSyncWithOtherStyling.floatValue, availableOptions), availableOptions), availableOptions);
                        GUI.enabled = true;
                        EditorGUILayout.Space();


                        if (stylingSpecularSyncWithOtherStyling.floatValue != 0)
                        {
                            EditorGUILayout.HelpBox("Specular styling now syncs continuously with "
                              + ((StylingSyncMode)stylingSpecularSyncWithOtherStyling.floatValue).ToString()
                              + " Styling, updating automatically.", MessageType.Info);

                            SyncStylingValues((StylingSyncMode)stylingSpecularSyncWithOtherStyling.floatValue, StyleAreaMode.Specular);
                            EditorGUILayout.Space();
                        }

                        EditorUtils.DrawSubMenuSeparation2();
                        EditorGUILayout.Space();
                    }
                    else
                    {
                        stylingSpecularSyncWithOtherStyling.floatValue = 0;
                    }

                    GUI.enabled = stylingSpecularSyncWithOtherStyling.floatValue == 0;
                    //}
                    DisableGUIIfOptimizedShader();
                    specularStyle.floatValue = GUILayout.Toolbar((int)specularStyle.floatValue, stylingOptions);
                    GUI.enabled = true;
                    EditorGUILayout.Space();


           

                    if (stylingSpecularSyncWithOtherStyling.floatValue == 0)
                    {
                        if (specularStyle.floatValue != 2)
                        {                           
                            DoStylingPositionAndBlendingSubArea(StyleAreaMode.Specular, m_SpecularStylingBlendingFoldout, green);
                            DoStylingDrawSpaceArea(StyleAreaMode.Specular, m_SpecularStylingDrawSpaceFoldout, green);
                            DoStylingStyleDensityRotationArea((Styling)specularStyle.floatValue, m_ShadingStylingRotationFoldout, stylingSpecularDensity, stylingSpecularRotation, null, stylingSpecularHalftonesOffset, green);
                            DoStylingSpecularStylingArea(green);
                        }
                        else
                        {
                            EditorGUILayout.LabelField("TODO: Texture Stuff");
                        }
                    } 
                    //else
                    //{
                    //    EditorGUILayout.HelpBox("Specular styling now syncs continuously with "
                    //          + ((StylingSyncMode)stylingSpecularSyncWithOtherStyling.floatValue).ToString()
                    //          + " Styling, updating automatically.", MessageType.Info);

                    //    SyncStylingValues((StylingSyncMode)stylingSpecularSyncWithOtherStyling.floatValue, StyleAreaMode.Specular);
                    //}


                    //EditorGUI.indentLevel -= 1;
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();
                }
            }        
        }        

        
        
        private void DoRimStylingArea()
        {
            Color blue = Color.cyan;

            if (enableRimStyling.floatValue == 0)
            {
                m_StylingRimFoldout.value = false;
            }

            DisableGUIIfOptimizedShader();
            Tuple<bool, bool> tupleValues = ToonEditorUtils.MakeTheToonShaderSubSectionHeaderWithFoldoutWithToggle(Styles.StylingRim, m_StylingRimFoldout.value, Convert.ToBoolean(enableRimStyling.floatValue),blue, 4);
            GUI.enabled = true;

            m_StylingRimFoldout.value = tupleValues.Item1;

            float enableRimStylingFloat = Convert.ToSingle(tupleValues.Item2);
            if (enableRimStyling.floatValue != enableRimStylingFloat && enableRimStylingFloat == 1)
            {
                m_StylingRimFoldout.value = true;
                stylingRimSyncWithOtherStyling.floatValue = (float)FindFirstEnabledStyle(StylingSyncMode.Rim);
            }
            enableRimStyling.floatValue = enableRimStylingFloat;
            EditorGUILayout.Space();
            if (m_StylingRimFoldout.value)
            {
                if (enableRimStyling.floatValue != 0)
                {

                    EditorGUILayout.Space();
                    DoStylingRimSettingsArea(blue);
                    EditorGUILayout.Space();
                    EditorUtils.DrawSubMenuSeparation();

                    EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth - 94;
                    materialEditor.ShaderProperty(stylingRimColor, "Color");
                    EditorGUIUtility.labelWidth = oriLabelWidth;

                    EditorGUILayout.Space();
                    EditorUtils.DrawSubMenuSeparation2();
                    EditorGUILayout.Space();
                    //if (rp != RenderPipelineOptions.URP2D)
                    //{
                    string[] availableOptions = GetArrayOfAvailableStylingOptions(StylingSyncMode.Rim);
                    if (availableOptions.Length > 1)
                    {
                        DisableGUIIfOptimizedShader();
                        stylingRimSyncWithOtherStyling.floatValue = FromPopUpIntToFloatValue(EditorGUILayout.Popup("Sync with ", FromFloatValueToPopUpInt(stylingRimSyncWithOtherStyling.floatValue, availableOptions), availableOptions), availableOptions);
                        GUI.enabled = true;

                        EditorGUILayout.Space();

                        if (stylingRimSyncWithOtherStyling.floatValue != 0)
                        {
                            EditorGUILayout.HelpBox("Specular styling now syncs continuously with "
                          + ((StylingSyncMode)stylingRimSyncWithOtherStyling.floatValue).ToString()
                          + " Styling, updating automatically.", MessageType.Info);

                            SyncStylingValues((StylingSyncMode)stylingRimSyncWithOtherStyling.floatValue, StyleAreaMode.Rim);

                            EditorGUILayout.Space();
                        }

                        EditorUtils.DrawSubMenuSeparation2();
                        EditorGUILayout.Space();
                    }
                    else
                    {
                        stylingRimSyncWithOtherStyling.floatValue = 0;
                    }

                    GUI.enabled = stylingRimSyncWithOtherStyling.floatValue == 0;
                    //}
                    DisableGUIIfOptimizedShader();
                    rimStyle.floatValue = GUILayout.Toolbar((int)rimStyle.floatValue, stylingOptions);
                    GUI.enabled = true;
                    EditorGUILayout.Space();


                    if (stylingRimSyncWithOtherStyling.floatValue == 0)
                    {
                        if (rimStyle.floatValue != 2)
                        {
                            DoStylingPositionAndBlendingSubArea(StyleAreaMode.Rim, m_RimStylingBlendingFoldout, blue);
                            DoStylingDrawSpaceArea(StyleAreaMode.Rim, m_RimStylingDrawSpaceFoldout, blue);
                            DoStylingStyleDensityRotationArea((Styling)rimStyle.floatValue, m_ShadingStylingRotationFoldout, stylingRimDensity, stylingRimRotation, null, stylingRimHalftonesOffset, blue);
                            DoStylingRimStylingArea(blue);
                        }
                        else
                        {
                            EditorGUILayout.LabelField("TODO: Texture Stuff");
                        }
                    }
                    //else
                    //{
                    //    EditorGUILayout.HelpBox("Specular styling now syncs continuously with "
                    //          + ((StylingSyncMode)stylingRimSyncWithOtherStyling.floatValue).ToString()
                    //          + " Styling, updating automatically.", MessageType.Info);

                    //    SyncStylingValues((StylingSyncMode)stylingRimSyncWithOtherStyling.floatValue, StyleAreaMode.Rim);
                    //}
                }
            }
        }

        private void DoStylingArea(Material material)
        {
            DisableGUIIfOptimizedShader();
            bool showStylingArea = ToonEditorUtils.MakeTheToonShaderSectionHeaderWithFoldoutWithToggle(Styles.StylingAreaHeader, m_StylingAreaFoldout, enableStyling);
            GUI.enabled = true;

            stylingAnimBool.target = showStylingArea == true;
            if (EditorGUILayout.BeginFadeGroup(stylingAnimBool.faded))
            {
                EditorGUILayout.Space();

                if (rp == RenderPipelineOptions.BiRP)
                {
                    EditorGUILayout.HelpBox("In the Built-in RenderPipeline(BiRP), you can use Styling only with the main directional light! Any other light source will be ignored! If you want to use Spotlights etc with BiRP, let us know. The URP version does support any light type!", MessageType.Info);
                }

                if (rp != RenderPipelineOptions.URP2D && !IsToon2DShader())
                {
                    if (enableShadingStyling.floatValue == 1) //|| enableCastShadowsStyling.floatValue == 1 || enableSpecularStyling.floatValue == 1 || enableRimStyling.floatValue == 1)
                    {
                        EditorGUILayout.Space();

                        DisableGUIIfOptimizedShader();
                        bool showDistanceFadeOptions = ToonEditorUtils.MakeTheToonShaderSubSectionHeaderWithFoldoutWithToggle(Styles.GeneralStylingDistanceFade, m_GeneralStylingDistanceFadeFoldout, enableStylingDistanceFade, Color.black);
                        GUI.enabled = true;
                        if (showDistanceFadeOptions)
                        {
                            EditorGUI.indentLevel += 1;
                            EditorGUIUtility.labelWidth = oriLabelWidth;

                            EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth - 104;

                            materialEditor.ShaderProperty(stylingDFStartingDistance, "Starting Distance");
                            makeAlwaysPositiv(stylingDFStartingDistance);
                            materialEditor.ShaderProperty(stylingDFFalloff, "Falloff");
                            makeAlwaysPositiv(stylingDFFalloff);
                            EditorGUIUtility.labelWidth = oriLabelWidth;

                            stylingAdjustDistanceFadeValue.floatValue = Convert.ToSingle(EditorGUILayout.ToggleLeft("Adjust Fade Value", Convert.ToBoolean(stylingAdjustDistanceFadeValue.floatValue)));
                            if (stylingAdjustDistanceFadeValue.floatValue == 1)
                            {
                                materialEditor.ShaderProperty(stylingDistanceFadeValue, "Fade Value");

                            }
                            EditorGUI.indentLevel -= 1;

                            EditorGUILayout.Space();
                        }
                        //enableStylingDistanceFade.floatValue = Convert.ToSingle(EditorGUILayout.ToggleLeft("Distance Fade", Convert.ToBoolean(enableStylingDistanceFade.floatValue)));
                        //if (enableStylingDistanceFade.floatValue == 1)
                        //{
                        //    EditorGUI.indentLevel += 1;
                        //    EditorGUIUtility.labelWidth = oriLabelWidth;

                        //    EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth - 104;

                        //    materialEditor.ShaderProperty(stylingDFStartingDistance, "Starting Distance");
                        //    makeAlwaysPositiv(stylingDFStartingDistance);
                        //    materialEditor.ShaderProperty(stylingDFFalloff, "Falloff");
                        //    makeAlwaysPositiv(stylingDFFalloff);
                        //    EditorGUIUtility.labelWidth = oriLabelWidth;

                        //    stylingAdjustDistanceFadeValue.floatValue = Convert.ToSingle(EditorGUILayout.ToggleLeft("Adjust Fade Value", Convert.ToBoolean(stylingAdjustDistanceFadeValue.floatValue)));
                        //    if (stylingAdjustDistanceFadeValue.floatValue == 1)
                        //    {
                        //        materialEditor.ShaderProperty(stylingDistanceFadeValue, "Fade Value");

                        //    }
                        //    EditorGUI.indentLevel -= 1;

                        //    EditorGUILayout.Space();
                        //}

                        EditorGUILayout.Space();

                    }
                }
                EditorUtils.DrawSubMenuSeparation();

                EditorGUILayout.Space();

                EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth - 94;
                if (rp != RenderPipelineOptions.URP2D && !IsToon2DShader())
                {
                    materialEditor.ShaderProperty(hatchingAffectedByNormalMap, "Affected By NormalMap");
                }
                materialEditor.ShaderProperty(enableAntiAliasing, "Enable Anti-Aliasing");
                EditorGUIUtility.labelWidth = oriLabelWidth;

                EditorGUILayout.Space();

                EditorUtils.DrawSubMenuSeparation();

                EditorGUILayout.Space();
                DoShadingStylingArea();
                if (rp != RenderPipelineOptions.URP2D && !IsToon2DShader())
                {
                    EditorUtils.DrawSubMenuSeparation();
                    DoCastShadowsStylingArea();
                }
                EditorUtils.DrawSubMenuSeparation();
                DoSpecularStylingArea();
                EditorUtils.DrawSubMenuSeparation();


                DoRimStylingArea();
                EditorGUILayout.Space();
                

                EditorGUILayout.Space();
                //}
                //EditorGUILayout.EndFadeGroup();

            }
            EditorGUILayout.EndFadeGroup();





        }

        //TODO
        private void DoAnimationArea(Material material)
        {

            //showContentAreaAnimation.floatValue = Convert.ToSingle(MakeTheToonShaderSectionHeaderWithFoldout("Animation", Convert.ToBoolean(showContentAreaAnimation.floatValue)));


            //animationAnimBool.target = showContentAreaAnimation.floatValue == 1;
            //if (EditorGUILayout.BeginFadeGroup(animationAnimBool.faded))
            //{

            //        EditorGUILayout.LabelField("TODO: Introduce Noise to patterns and hatching with FPS control");

            //}
            //EditorGUILayout.EndFadeGroup();

        }




        private void DoStylingSpecularSettingsArea(Color color)
        {
            m_StylingSpecularSettingsFoldout.value = ToonEditorUtils.MakeTheToonShaderSubSectionHeaderWithFoldout(Styles.StylingSpecularSettings, m_StylingSpecularSettingsFoldout.value, color);
            if (m_StylingSpecularSettingsFoldout.value)
            {
                EditorGUILayout.Space();
                if(enableToonShading.floatValue == 1 &&  enableSpecular.floatValue == 1)
                {
                    syncWithSpecular.floatValue = Convert.ToSingle(EditorGUILayout.ToggleLeft("Sync With Regular Specular", Convert.ToBoolean(syncWithSpecular.floatValue)));
                }
                else
                {
                    syncWithSpecular.floatValue = 0;
                }

                if (syncWithSpecular.floatValue == 0)
                {
                    materialEditor.ShaderProperty(stylingSpecularSize, "Size");
                    materialEditor.ShaderProperty(stylingSpecularSmoothness, "Smoothness");
                }
                EditorGUILayout.Space();
            }
        }

        private void DoStylingRimSettingsArea(Color color)
        {
            m_StylingRimSettingsFoldout.value = ToonEditorUtils.MakeTheToonShaderSubSectionHeaderWithFoldout(Styles.StylingRimSettings, m_StylingRimSettingsFoldout.value, color);
            //float oriLabelWidth = EditorGUIUtility.labelWidth;

            //m_SurfaceOptionsFoldout.value = EditorGUILayout.BeginFoldoutHeaderGroup(m_SurfaceOptionsFoldout.value, Styles.SurfaceOptions);
            if (m_StylingRimSettingsFoldout.value)
            {
                //EditorUtils.DrawUILine(lightGreyForLines);
                EditorGUILayout.Space();


                //EditorGUI.indentLevel += 1;

                //EditorGUILayout.Space();

                if (enableToonShading.floatValue == 1 && enableRim.floatValue == 1)
                {
                    syncWithRim.floatValue = Convert.ToSingle(EditorGUILayout.ToggleLeft("Sync With Regular Rim", Convert.ToBoolean(syncWithRim.floatValue)));
                }
                else
                {
                    syncWithRim.floatValue = 0;
                }

                if (syncWithRim.floatValue == 0)
                {
                    materialEditor.ShaderProperty(stylingRimSize, "Size");
                    materialEditor.ShaderProperty(stylingRimSmoothness, "Smoothness");

                    EditorGUILayout.Space();
#if USING_URP
                    if (hasAdditionalLights)
                    {
                        stylingRimAffectedArea.floatValue = 2;
                    }
                    else
#endif
                    {
                        stylingRimAffectedArea.floatValue = (int)(RimAffectedRegion)EditorGUILayout.EnumPopup("Rim Affected Region", (RimAffectedRegion)stylingRimAffectedArea.floatValue);
                    }
                }
                //stylingOvermodelingFactor.floatValue = EditorGUILayout.Slider("Overmodeling Factor", stylingOvermodelingFactor.floatValue, 0, 1);




                //EditorGUI.indentLevel -= 1;

                EditorGUILayout.Space();
            }
        }



        private void DoStylingCastShadowsPartitioningAndSettingsArea(Color color)
        {
            m_StylingCastShadowsSettingsFoldout.value = ToonEditorUtils.MakeTheToonShaderSubSectionHeaderWithFoldout(Styles.StylingCastShadowsSettings, m_StylingCastShadowsSettingsFoldout.value, color);
            if (m_StylingCastShadowsSettingsFoldout.value)
            {
                EditorGUILayout.Space();
                if ((StylingSyncMode) stylingCastShadowsSyncWithOtherStyling.floatValue == StylingSyncMode.Shading)
                {
                    GUI.enabled = false;
                }
                DisableGUIIfOptimizedShader();
                stylingCastShadowsNumberOfCellsHatching.floatValue = EditorGUILayout.IntSlider("Number Of Cells", (int)stylingCastShadowsNumberOfCellsHatching.floatValue, 1, 15);

                if(enableShadingStyling.floatValue == 1 && (StylingSyncMode)stylingShadingSyncWithOtherStyling.floatValue != StylingSyncMode.CastShadows )
                {
                    GUI.enabled = false;
                }
                DarkerSliderProperty(stylingTerminatorPosition, "Terminator Position");
                GUI.enabled = true;

                if (enableShadingStyling.floatValue == 1 && (StylingSyncMode)stylingShadingSyncWithOtherStyling.floatValue != StylingSyncMode.CastShadows)
                {
                    EditorGUILayout.HelpBox("Terminator Position is shared with Styling Shading and can be set there!", MessageType.Info);
                }

                if (!(enableShadingStyling.floatValue == 1 && (StylingSyncMode)stylingCastShadowsSyncWithOtherStyling.floatValue == StylingSyncMode.Shading) )
                {
                    EditorGUILayout.Space();
                    materialEditor.ShaderProperty(stylingCastShadowsSmoothness, "Smoothness");
                    EditorGUILayout.Space();
                }
            }
        }

        private void DoStylingPartitioningArea(Color color)
        {
            m_ShadingStylingPartitioningFoldout.value = ToonEditorUtils.MakeTheToonShaderSubSectionHeaderWithFoldout(Styles.StylingPartitioning, m_ShadingStylingPartitioningFoldout.value, color);
            //float oriLabelWidth = EditorGUIUtility.labelWidth;

            //m_SurfaceOptionsFoldout.value = EditorGUILayout.BeginFoldoutHeaderGroup(m_SurfaceOptionsFoldout.value, Styles.SurfaceOptions);
            if (m_ShadingStylingPartitioningFoldout.value)
            {
                //EditorUtils.DrawUILine(lightGreyForLines);
                EditorGUILayout.Space();


                //EditorGUI.indentLevel += 1;

                //EditorGUILayout.Space();
                DisableGUIIfOptimizedShader();
                if ((StylingSyncMode) stylingShadingSyncWithOtherStyling.floatValue != StylingSyncMode.CastShadows)
                {
                    if (enableToonShading.floatValue == 1)
                    {
                        syncWithLightPartitioning.floatValue = Convert.ToSingle(EditorGUILayout.ToggleLeft("Sync With Light Partitioning", Convert.ToBoolean(syncWithLightPartitioning.floatValue)));
                    }
                    else
                    {
                        syncWithLightPartitioning.floatValue = 0;
                    }

                    if (syncWithLightPartitioning.floatValue == 0)
                    {
                        if ((Styling) shadingStyle.floatValue == Styling.Hatching)
                        {
                            numberOfCellsHatching.floatValue = EditorGUILayout.IntSlider("Number Of Cells", (int)numberOfCellsHatching.floatValue, 1, 15);

                        }
                        DarkerSliderProperty(stylingTerminatorPosition, "Terminator Position");
                    }
                }
                else
                {
                    syncWithLightPartitioning.floatValue = 0;

                    if ((Styling)shadingStyle.floatValue == Styling.Hatching)
                    {
                        GUI.enabled = false;
                        numberOfCellsHatching.floatValue = EditorGUILayout.IntSlider("Number Of Cells", (int)numberOfCellsHatching.floatValue, 1, 15);
                        DarkerSliderProperty(stylingTerminatorPosition, "Terminator Position");
                        GUI.enabled = true;
                    }

                }
                GUI.enabled = true;


                //stylingOvermodelingFactor.floatValue = EditorGUILayout.Slider("Overmodeling Factor", stylingOvermodelingFactor.floatValue, 0, 1);


                EditorGUILayout.Space();
                //materialEditor.ShaderProperty(stylingOvermodelingFactor, "Overmodeling Factor");

                if ((Styling)shadingStyle.floatValue == Styling.Hatching)
                {
                    //GUI.enabled = false;
                    DarkerSliderProperty(stylingOvermodelingFactor, "Overmodeling Factor");
                    stylingOvermodelingFactor.floatValue = Math.Min(stylingOvermodelingFactor.floatValue, 0.999f);
                    GUI.enabled = true;
                }





                //EditorGUI.indentLevel -= 1;

                EditorGUILayout.Space();
            }
        }



        private void DoStylingHalftoneArea( MaterialProperty color,
                                            MaterialProperty thickness,
                                            MaterialProperty thicknessControl,
                                            MaterialProperty thicknessFalloff,
                                            MaterialProperty opacity,
                                            MaterialProperty opacityFallof,
                                            MaterialProperty hardness,
                                            MaterialProperty roundness,
                                            MaterialProperty roundnessFalloff)
        {
            EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth - 94;
            EditorGUILayout.Space();

            //EditorGUI.indentLevel -= 1;
            //EditorStyles.label.normal.textColor = textColor;
            //EditorGUILayout.LabelField("Color");
            //EditorStyles.label.normal.textColor = oriCol;
            //EditorGUI.indentLevel += 1;

            // materialEditor.ShaderProperty(color, "Color");


            EditorUtils.DrawSubMenuSeparation();

            EditorGUI.indentLevel -= 1;
            EditorStyles.label.normal.textColor = textColor;
            EditorGUILayout.LabelField("Opacity");
            EditorStyles.label.normal.textColor = oriCol;
            EditorGUI.indentLevel += 1;

            //materialEditor.ShaderProperty(opacity, "Opacity");
            DarkerSliderProperty(opacity, "Opacity");

            //materialEditor.ShaderProperty(opacityFallof, "Opacity Falloff");
            DarkerSliderProperty(opacityFallof, "Opacity Falloff");

            EditorUtils.DrawSubMenuSeparation();
            EditorGUI.indentLevel -= 1;
            EditorStyles.label.normal.textColor = textColor;
            EditorGUILayout.LabelField("Size");
            EditorStyles.label.normal.textColor = oriCol;
            EditorGUI.indentLevel += 1;

            EditorGUIUtility.labelWidth = oriLabelWidth;

            EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth / 2.5f;
            thicknessControl.floatValue = (int)(SizeControl)EditorGUILayout.EnumPopup("Size Control", (SizeControl)thicknessControl.floatValue);
            EditorGUIUtility.labelWidth = oriLabelWidth;


            if (thicknessControl.floatValue == (int)SizeControl.Manual)
            {
                //materialEditor.ShaderProperty(thickness, "Size");
                DarkerSliderProperty(thickness, "Size");
            }
            else
            {
                //materialEditor.ShaderProperty(thickness, "Max Size");
                DarkerSliderProperty(thickness, "Max Size");
                //hatchingMinThickness.floatValue = Mathf.Min(hatchingMinThickness.floatValue, hatchingThickness.floatValue);
                //materialEditor.ShaderProperty(hatchingMinThickness, "Min Thickness");
            }
            if (thicknessControl.floatValue == (int)SizeControl.Manual)
            {
                //materialEditor.ShaderProperty(thicknessFalloff, "Size Falloff");
                DarkerSliderProperty(thicknessFalloff, "Size Falloff");
                thicknessFalloff.floatValue = Math.Max(thicknessFalloff.floatValue, 0.001f);

            }
            EditorUtils.DrawSubMenuSeparation();

            EditorGUI.indentLevel -= 1;
            EditorStyles.label.normal.textColor = textColor;
            EditorGUILayout.LabelField("Roundness");
            EditorStyles.label.normal.textColor = oriCol;
            EditorGUI.indentLevel += 1;

            //materialEditor.ShaderProperty(roundness, "Roundness");
            DarkerSliderProperty(roundness, "Roundness");
            //materialEditor.ShaderProperty(roundnessFalloff, "Roundness Falloff");
            EditorGUI.BeginDisabledGroup(roundness.floatValue == 1);
            DarkerSliderProperty(roundnessFalloff, "Roundness Falloff");
            EditorGUI.EndDisabledGroup();



            EditorUtils.DrawSubMenuSeparation();


            EditorGUI.indentLevel -= 1;
            EditorStyles.label.normal.textColor = textColor;
            EditorGUILayout.LabelField("Hardness");
            EditorStyles.label.normal.textColor = oriCol;
            EditorGUI.indentLevel += 1;

            //materialEditor.ShaderProperty(hardness, "Hardness");
            DarkerSliderProperty(hardness, "Hardness");
        }

            private void DoStylingRandomSubArea(Color color, SavedBool foldout,
                                            MaterialProperty styling,

                                            MaterialProperty enableRandomizer,
                                            MaterialProperty perlinNoiseSize,
                                            MaterialProperty perlinNoiseSeed,
                                            MaterialProperty whiteNoiseSeed,
                                            MaterialProperty spacingRandomMode,
                                            MaterialProperty spacingRandomIntensity,
                                            MaterialProperty opacityRandomMode,
                                            MaterialProperty opacityRandomIntensity,
                                            MaterialProperty hardnessRandomMode,
                                            MaterialProperty hardnessRandomIntensity,
                                            MaterialProperty lengthRandomMode,
                                            MaterialProperty lengthRandomIntensity,
                                            MaterialProperty thicknessRandomMode,
                                            MaterialProperty thicknesshRandomIntensity,
                                            MaterialProperty noiseIntensity)
        {

            if (enableRandomizer.floatValue == 0)
            {
                foldout.value = false;
            }
            bool showRandomizerOptions = ToonEditorUtils.MakeTheToonShaderSubSectionHeaderWithFoldoutWithToggle(Styles.StylingRandomizeHatching, foldout, enableRandomizer, color);
            if (showRandomizerOptions)
            {

            //    //m_ShadingStylingRandomizeFoldout.value = false;
            //    Tuple<bool, bool> tupleValues = ToonEditorUtils.MakeTheToonShaderSubSectionHeaderWithFoldoutWithToggle(Styles.StylingRandomizeHatching, foldout.value, Convert.ToBoolean(enableRandomizer.floatValue), color);
            //foldout.value = tupleValues.Item1;
            ////enableSpecularStyling.floatValue = Convert.ToSingle(tupleValues.Item2);

            //float enableRandomizerFloat = Convert.ToSingle(tupleValues.Item2);
            //if (enableRandomizer.floatValue != enableRandomizerFloat && enableRandomizerFloat == 1)
            //{
            //    foldout.value = true;
            //}
            //enableRandomizer.floatValue = enableRandomizerFloat;




            ////m_ShadingStylingRandomizeFoldout.value = MakeTheToonShaderSubSectionHeaderWithFoldout(Styles.ShadingStylingRandomizeHatching, m_ShadingStylingRandomizeFoldout.value, color);




            ////float oriLabelWidth = EditorGUIUtility.labelWidth;

            ////m_SurfaceOptionsFoldout.value = EditorGUILayout.BeginFoldoutHeaderGroup(m_SurfaceOptionsFoldout.value, Styles.SurfaceOptions);
            //if (foldout.value && enableRandomizer.floatValue == 1)
            //{


                //EditorUtils.DrawUILine(lightGreyForLines);
                //EditorGUILayout.Space();

                EditorGUI.indentLevel += 1;

                EditorGUILayout.Space();
                //materialEditor.TexturePropertySingleLine(new GUIContent("Perlin Noise"), noiseMap1);
                //materialEditor.TexturePropertySingleLine(new GUIContent("White Noise"), noiseMap2);

                //materialEditor.ShaderProperty(perlinNoiseSize, "Random perlinNoiseSize");


                if (styling.floatValue == 0)
                {

                    EditorGUI.indentLevel -= 1;
                    EditorStyles.label.normal.textColor = textColor;
                    EditorGUILayout.LabelField("Noise Size");
                    EditorStyles.label.normal.textColor = oriCol;
                    EditorGUI.indentLevel += 1;

                    DarkerSliderProperty(perlinNoiseSize, "Random perlinNoiseSize");
                    makeAlwaysPositiv(perlinNoiseSize);



                    EditorUtils.DrawSubMenuSeparation();

                    EditorGUI.indentLevel -= 1;
                    EditorStyles.label.normal.textColor = textColor;
                    EditorGUILayout.LabelField("Opacity");
                    EditorStyles.label.normal.textColor = oriCol;
                    EditorGUI.indentLevel += 1;

                    EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth / 2f;
                    opacityRandomMode.floatValue = (int)(RandomMode)EditorGUILayout.EnumPopup("Random Mode", (RandomMode)opacityRandomMode.floatValue);
                    EditorGUIUtility.labelWidth = oriLabelWidth;
                    //materialEditor.ShaderProperty(opacityRandomIntensity, "Random Intensity");
                    DarkerSliderProperty(opacityRandomIntensity, "Random Intensity");

                    EditorUtils.DrawSubMenuSeparation();

                    EditorGUI.indentLevel -= 1;
                    EditorStyles.label.normal.textColor = textColor;
                    EditorGUILayout.LabelField("Hardness");
                    EditorStyles.label.normal.textColor = oriCol;
                    EditorGUI.indentLevel += 1;

                    EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth / 2f;
                    hardnessRandomMode.floatValue = (int)(RandomMode)EditorGUILayout.EnumPopup("Random Mode", (RandomMode)hardnessRandomMode
                        .floatValue);
                    EditorGUIUtility.labelWidth = oriLabelWidth;
                    //materialEditor.ShaderProperty(hardnessRandomIntensity, "Random Intensity");
                    DarkerSliderProperty(hardnessRandomIntensity, "Random Intensity");

                    EditorUtils.DrawSubMenuSeparation();

                    EditorGUI.indentLevel -= 1;
                    EditorStyles.label.normal.textColor = textColor;
                    EditorGUILayout.LabelField("Spacing");
                    EditorStyles.label.normal.textColor = oriCol;
                    EditorGUI.indentLevel += 1;

                    EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth / 2f;
                    spacingRandomMode.floatValue = (int)(RandomMode)EditorGUILayout.EnumPopup("Random Mode", (RandomMode)spacingRandomMode.floatValue);
                    EditorGUIUtility.labelWidth = oriLabelWidth;


                    //materialEditor.ShaderProperty(spacingRandomIntensity, "Random Intensity");
                    DarkerSliderProperty(spacingRandomIntensity, "Random Intensity");


                    EditorUtils.DrawSubMenuSeparation();

                    EditorGUI.indentLevel -= 1;
                    EditorStyles.label.normal.textColor = textColor;
                    EditorGUILayout.LabelField("Length");
                    EditorStyles.label.normal.textColor = oriCol;
                    EditorGUI.indentLevel += 1;

                    EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth / 2f;
                    lengthRandomMode.floatValue = (int)(RandomMode)EditorGUILayout.EnumPopup("Random Mode", (RandomMode)lengthRandomMode.floatValue);
                    EditorGUIUtility.labelWidth = oriLabelWidth;


                    //materialEditor.ShaderProperty(lengthRandomIntensity, "Random Intensity");
                    DarkerSliderProperty(lengthRandomIntensity, "Random Intensity");

                    EditorUtils.DrawSubMenuSeparation();

                    EditorGUI.indentLevel -= 1;
                    EditorStyles.label.normal.textColor = textColor;
                    EditorGUILayout.LabelField("Thickness");
                    EditorStyles.label.normal.textColor = oriCol;
                    EditorGUI.indentLevel += 1;

                    EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth / 2f;
                    thicknessRandomMode.floatValue = (int)(RandomMode)EditorGUILayout.EnumPopup("Random Mode", (RandomMode)thicknessRandomMode.floatValue);
                    EditorGUIUtility.labelWidth = oriLabelWidth;
                    //materialEditor.ShaderProperty(thicknesshRandomIntensity, "Random Intensity");
                    DarkerSliderProperty(thicknesshRandomIntensity, "Random Intensity");

                    EditorUtils.DrawSubMenuSeparation();

                    EditorGUI.indentLevel -= 1;
                    EditorStyles.label.normal.textColor = textColor;
                    EditorGUILayout.LabelField("Shape / General Noise");
                    EditorStyles.label.normal.textColor = oriCol;
                    EditorGUI.indentLevel += 1;

                    //materialEditor.ShaderProperty(noiseIntensity, "Random Intensity");
                    DarkerSliderProperty(noiseIntensity, "Random Intensity");

                }
                else
                {
                    EditorGUI.indentLevel -= 1;
                    EditorStyles.label.normal.textColor = textColor;
                    EditorGUILayout.LabelField("Noise Size");
                    EditorStyles.label.normal.textColor = oriCol;
                    EditorGUI.indentLevel += 1;

                    DarkerSliderProperty(perlinNoiseSize, "Random perlinNoiseSize");
                    makeAlwaysPositiv(perlinNoiseSize);



                    EditorUtils.DrawSubMenuSeparation();

                    EditorGUI.indentLevel -= 1;
                    EditorStyles.label.normal.textColor = textColor;
                    EditorGUILayout.LabelField("Opacity");
                    EditorStyles.label.normal.textColor = oriCol;
                    EditorGUI.indentLevel += 1;

                    EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth / 2f;
                    opacityRandomMode.floatValue = (int)(RandomMode)EditorGUILayout.EnumPopup("Random Mode", (RandomModeHalftones)opacityRandomMode.floatValue);
                    EditorGUIUtility.labelWidth = oriLabelWidth;
                    //materialEditor.ShaderProperty(opacityRandomIntensity, "Random Intensity");
                    DarkerSliderProperty(opacityRandomIntensity, "Random Intensity");

                    EditorUtils.DrawSubMenuSeparation();

                    EditorGUI.indentLevel -= 1;
                    EditorStyles.label.normal.textColor = textColor;
                    EditorGUILayout.LabelField("Hardness");
                    EditorStyles.label.normal.textColor = oriCol;
                    EditorGUI.indentLevel += 1;

                    EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth / 2f;
                    hardnessRandomMode.floatValue = (int)(RandomMode)EditorGUILayout.EnumPopup("Random Mode", (RandomModeHalftones)hardnessRandomMode
                        .floatValue);
                    EditorGUIUtility.labelWidth = oriLabelWidth;
                    //materialEditor.ShaderProperty(hardnessRandomIntensity, "Random Intensity");
                    DarkerSliderProperty(hardnessRandomIntensity, "Random Intensity");

                    EditorUtils.DrawSubMenuSeparation();

                    EditorGUI.indentLevel -= 1;
                    EditorStyles.label.normal.textColor = textColor;
                    EditorGUILayout.LabelField("Spacing");
                    EditorStyles.label.normal.textColor = oriCol;
                    EditorGUI.indentLevel += 1;

                    EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth / 2f;
                    spacingRandomMode.floatValue = (int)(RandomMode)EditorGUILayout.EnumPopup("Random Mode", (RandomModeHalftones)spacingRandomMode.floatValue);
                    EditorGUIUtility.labelWidth = oriLabelWidth;


                    //materialEditor.ShaderProperty(spacingRandomIntensity, "Random Intensity");
                    DarkerSliderProperty(spacingRandomIntensity, "Random Intensity");


                    EditorUtils.DrawSubMenuSeparation();

                    EditorGUI.indentLevel -= 1;
                    EditorStyles.label.normal.textColor = textColor;
                    EditorGUILayout.LabelField("Length");
                    EditorStyles.label.normal.textColor = oriCol;
                    EditorGUI.indentLevel += 1;

                    EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth / 2f;
                    lengthRandomMode.floatValue = (int)(RandomMode)EditorGUILayout.EnumPopup("Random Mode", (RandomModeHalftones)lengthRandomMode.floatValue);
                    EditorGUIUtility.labelWidth = oriLabelWidth;


                    //materialEditor.ShaderProperty(lengthRandomIntensity, "Random Intensity");
                    DarkerSliderProperty(lengthRandomIntensity, "Random Intensity");

                    EditorUtils.DrawSubMenuSeparation();

                    EditorGUI.indentLevel -= 1;
                    EditorStyles.label.normal.textColor = textColor;
                    EditorGUILayout.LabelField("Size");
                    EditorStyles.label.normal.textColor = oriCol;
                    EditorGUI.indentLevel += 1;

                    EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth / 2f;
                    thicknessRandomMode.floatValue = (int)(RandomMode)EditorGUILayout.EnumPopup("Random Mode", (RandomModeHalftones)thicknessRandomMode.floatValue);
                    EditorGUIUtility.labelWidth = oriLabelWidth;
                    //materialEditor.ShaderProperty(thicknesshRandomIntensity, "Random Intensity");
                    DarkerSliderProperty(thicknesshRandomIntensity, "Random Intensity");

                    EditorUtils.DrawSubMenuSeparation();

                    EditorGUI.indentLevel -= 1;
                    EditorStyles.label.normal.textColor = textColor;
                    EditorGUILayout.LabelField("Shape / General Noise");
                    EditorStyles.label.normal.textColor = oriCol;
                    EditorGUI.indentLevel += 1;

                    //materialEditor.ShaderProperty(noiseIntensity, "Random Intensity");
                    DarkerSliderProperty(noiseIntensity, "Random Intensity");

                }


    

                EditorGUILayout.Space();

                EditorGUI.indentLevel -= 1;

                EditorGUILayout.Space();
            }
        }
        public static float Remap(float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }

        private void DarkerSliderProperty(MaterialProperty materialProperty, string label)
        {
 
            Color originalColor = GUI.backgroundColor;

            Vector2 minMax = materialProperty.rangeLimits;
            float value = Remap(materialProperty.floatValue*3, minMax.x, minMax.y, 0.2f, 1.0f);
            float value2 = Remap(materialProperty.floatValue * 3, minMax.x, minMax.y, 0.5f, 1);

            float valueR = Remap(materialProperty.floatValue * 3, minMax.x, minMax.y, 0.2f, 0.5f);
            float valueG = Remap(materialProperty.floatValue * 3, minMax.x, minMax.y, 0.2f, 0.7f);

            GUI.backgroundColor = originalColor * new Color(valueR, valueG, value, value2); 
            materialEditor.ShaderProperty(materialProperty, label);
            GUI.backgroundColor = originalColor;
        }

        
        private void DoStylingPositionAndBlendingSubArea(StyleAreaMode styleAreaMode, SavedBool savedBoolFoldout, Color color)
        {
            savedBoolFoldout.value = ToonEditorUtils.MakeTheToonShaderSubSectionHeaderWithFoldout(Styles.StylingPositionAndBlending, savedBoolFoldout.value, color);
            if (savedBoolFoldout.value)
            {
                //EditorUtils.DrawUILine(lightGreyForLines);
                EditorGUILayout.Space();



//Position selection.. maybe add in later update

                ////In case something gets messed up
                //if(stylingShadingPosition == stylingSpecularPosition || stylingShadingPosition == stylingRimPosition || stylingSpecularPosition == stylingRimPosition)
                //{
                //    stylingShadingPosition.floatValue = 0;
                //    stylingSpecularPosition.floatValue = 1;
                //    stylingRimPosition.floatValue = 2;
                //}

                //if(styleAreaMode == StyleAreaMode.Shading) { 


                //    //shadingStyle.floatValue = (int)(StyleMapping)EditorGUILayout.EnumPopup("Mapping", (StyleMapping)shadingStyle.floatValue);

                //    float tempShading = (int)(StylePosition)EditorGUILayout.EnumPopup("Position", (StylePosition)stylingShadingPosition.floatValue);
                //    if(stylingSpecularPosition.floatValue == tempShading)
                //    {
                //        stylingSpecularPosition.floatValue = stylingShadingPosition.floatValue;
                //        stylingShadingPosition.floatValue = tempShading;
                //    } else if(stylingRimPosition.floatValue == tempShading)
                //    {
                //        stylingRimPosition.floatValue = stylingShadingPosition.floatValue;
                //        stylingShadingPosition.floatValue = tempShading;
                //    }

                //} else if (styleAreaMode == StyleAreaMode.Specular)
                //{


                //    //specularStyle.floatValue = (int)(StyleMapping)EditorGUILayout.EnumPopup("Mapping", (StyleMapping)specularStyle.floatValue);
                //    //specularStylePosition.floatValue = (int)(StylePosition)EditorGUILayout.EnumPopup("Position", (StylePosition)specularStylePosition.floatValue);
                //    float tempSpecular = (int)(StylePosition)EditorGUILayout.EnumPopup("Position", (StylePosition)stylingSpecularPosition.floatValue);
                //    if (stylingShadingPosition.floatValue == tempSpecular)
                //    {
                //        stylingShadingPosition.floatValue = stylingSpecularPosition.floatValue;
                //        stylingSpecularPosition.floatValue = tempSpecular;
                //    }
                //    else if (stylingRimPosition.floatValue == tempSpecular)
                //    {
                //        stylingRimPosition.floatValue = stylingSpecularPosition.floatValue;
                //        stylingSpecularPosition.floatValue = tempSpecular;
                //    }

                //}
                //else
                //{


                //    //rimStyle.floatValue = (int)(StyleMapping)EditorGUILayout.EnumPopup("Mapping", (StyleMapping)rimStyle.floatValue);
                //    //rimStylePosition.floatValue = (int)(StylePosition)EditorGUILayout.EnumPopup("Position", (StylePosition)rimStylePosition.floatValue);

                //    float tempRim = (int)(StylePosition)EditorGUILayout.EnumPopup("Position", (StylePosition)stylingRimPosition.floatValue);
                //    if (stylingShadingPosition.floatValue == tempRim)
                //    {
                //        stylingShadingPosition.floatValue = stylingRimPosition.floatValue;
                //        stylingRimPosition.floatValue = tempRim;
                //    }
                //    else if (stylingSpecularPosition.floatValue == tempRim)
                //    {
                //        stylingSpecularPosition.floatValue = stylingRimPosition.floatValue;
                //        stylingRimPosition.floatValue = tempRim;
                //    }


                //}


                DoStylingBlendingSubArea(styleAreaMode);                


                EditorGUILayout.Space();
            }
        }

        private void DoStylingBlendingSubArea(StyleAreaMode styleAreaMode)
        {
            DisableGUIIfOptimizedShader();
            if (styleAreaMode == StyleAreaMode.Shading)
            {
                stylingShadingBlending.floatValue = (int)(BlendingMode)EditorGUILayout.EnumPopup("Blending", (BlendingMode)stylingShadingBlending.floatValue);
                //materialEditor.ShaderProperty(stylingShadingIsInverted, "Is Inverted");

            }
            else if (styleAreaMode == StyleAreaMode.CastShadows)
            {
                stylingCastShadowsBlending.floatValue = (int)(BlendingMode)EditorGUILayout.EnumPopup("Blending", (BlendingMode)stylingCastShadowsBlending.floatValue);
                //materialEditor.ShaderProperty(stylingSpecularIsInverted, "Is Inverted");

            }
            else if(styleAreaMode == StyleAreaMode.Specular)
            {
                stylingSpecularBlending.floatValue = (int)(BlendingMode)EditorGUILayout.EnumPopup("Blending", (BlendingMode)stylingSpecularBlending.floatValue);
                //materialEditor.ShaderProperty(stylingSpecularIsInverted, "Is Inverted");

            }
            else
            {
                stylingRimBlending.floatValue = (int)(BlendingMode)EditorGUILayout.EnumPopup("Blending", (BlendingMode)stylingRimBlending.floatValue);
                //materialEditor.ShaderProperty(stylingRimIsInverted, "Is Inverted");
            }
            GUI.enabled = true;


            if (styleAreaMode == StyleAreaMode.Shading)
            {
                materialEditor.ShaderProperty(stylingShadingIsInverted, "Is Inverted");
            }

        }




        private void DoStylingStylingArea(  Styling styling,
                                            StyleAreaMode styleAreaMode,
                                            MaterialProperty styleColor,
                                            MaterialProperty thickness,
                                            MaterialProperty thicknessControl,
                                            MaterialProperty thicknessFalloff,
                                            MaterialProperty opacity,
                                            MaterialProperty opacityFallof,
                                            MaterialProperty hardness,
                                            MaterialProperty roundness,
                                            MaterialProperty roundnessFalloff,
                                            Color color)
        {
            if (styling == Styling.Hatching)
            {



                EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth - 94;
                EditorGUILayout.Space();
                //EditorGUI.indentLevel -= 1;
                //EditorStyles.label.normal.textColor = textColor;
                //EditorGUILayout.LabelField("Color");
                //EditorStyles.label.normal.textColor = oriCol;
                //EditorGUI.indentLevel += 1;
                //materialEditor.ShaderProperty(styleColor, "Color");
                ////EditorGUILayout.Space();
                //EditorUtils.DrawSubMenuSeparation();

                //EditorGUILayout.Space();

                EditorGUI.indentLevel -= 1;
                EditorStyles.label.normal.textColor = textColor;
                EditorGUILayout.LabelField("Opacity");
                EditorStyles.label.normal.textColor = oriCol;
                EditorGUI.indentLevel += 1;

                //materialEditor.ShaderProperty(opacity, "Opacity");
                DarkerSliderProperty(opacity, "Opacity");

                //materialEditor.ShaderProperty(opacityFallof, "Opacity Falloff");
                DarkerSliderProperty(opacityFallof, "Opacity Falloff");


                EditorUtils.DrawSubMenuSeparation();

                EditorGUI.indentLevel -= 1;
                EditorStyles.label.normal.textColor = textColor;
                EditorGUILayout.LabelField("Thickness");
                EditorStyles.label.normal.textColor = oriCol;
                EditorGUI.indentLevel += 1;

                EditorGUIUtility.labelWidth = oriLabelWidth;
                thicknessControl.floatValue = (int)(SizeControl)EditorGUILayout.EnumPopup("Thickness Control", (SizeControl)thicknessControl.floatValue);

                EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth - 94;

                if (thicknessControl.floatValue == (int)SizeControl.Manual)
                {
                    //materialEditor.ShaderProperty(thickness, "Thickness");
                    DarkerSliderProperty(thickness, "Thickness");
                }
                else
                {
                    //materialEditor.ShaderProperty(thickness, "Max Thickness");
                    DarkerSliderProperty(thickness, "Max Thickness");
                    //hatchingMinThickness.floatValue = Mathf.Min(hatchingMinThickness.floatValue, hatchingThickness.floatValue);
                    //materialEditor.ShaderProperty(hatchingMinThickness, "Min Thickness");
                }
                if (stylingShadingThicknessControl.floatValue == (int)SizeControl.Manual)
                {
                    //materialEditor.ShaderProperty(thicknessFalloff, "Thickness Falloff");
                    DarkerSliderProperty(thicknessFalloff, "Thickness Falloff");

                }

                EditorUtils.DrawSubMenuSeparation();

                EditorGUIUtility.labelWidth = oriLabelWidth;


                EditorGUI.indentLevel -= 1;
                EditorStyles.label.normal.textColor = textColor;
                EditorGUILayout.LabelField("Hardness");
                EditorStyles.label.normal.textColor = oriCol;
                EditorGUI.indentLevel += 1;

                //materialEditor.ShaderProperty(hardness, "Hardness");
                DarkerSliderProperty(hardness, "Hardness");







            }
            else if (styling == Styling.HalftonePatterns)
            {
                DoStylingHalftoneArea(styleColor, thickness, thicknessControl, thicknessFalloff, opacity, opacityFallof, hardness, roundness, roundnessFalloff);
            }
            else
            {

                EditorGUILayout.LabelField("TODO: Texture Stuff");

            }
            EditorGUI.indentLevel -= 1;
            //EditorGUILayout.Space();
            //if (styleAreaMode == StyleAreaMode.Shading)
            //{
            //    DoStylingCastShadowsSubArea();
            //}

            EditorGUILayout.Space();

            DisableGUIIfOptimizedShader();
            if (styleAreaMode == StyleAreaMode.Shading)
            {
                DoStylingRandomSubArea(color, m_ShadingStylingRandomizeFoldout, shadingStyle,
                enableShadingRandomizer,
                shadingNoise1Size,
                shadingNoise1Seed,
                shadingNoise2Seed,
                spacingRandomMode,
                spacingRandomIntensity,
                opacityRandomMode,
                opacityRandomIntensity,
                hardnessRandomMode,
                hardnessRandomIntensity,
                lengthRandomMode,
                lengthRandomIntensity,
                thicknessRandomMode,
                thicknessRandomIntensity,
                noiseIntensity);
            }
            else if (styleAreaMode == StyleAreaMode.CastShadows)
            {
                DoStylingRandomSubArea(color, m_CastShadowsStylingRandomizeFoldout, castShadowsStyle,
                enableCastShadowsRandomizer,
                castShadowsNoise1Size,
                castShadowsNoise1Seed,
                castShadowsNoise2Seed,
                spacingRandomModeCastShadows,
                spacingRandomIntensityCastShadows,
                opacityRandomModeCastShadows,
                opacityRandomIntensityCastShadows,
                hardnessRandomModeCastShadows,
                hardnessRandomIntensityCastShadows,
                lengthRandomModeCastShadows,
                lengthRandomIntensityCastShadows,
                thicknessRandomModeCastShadows,
                thicknessRandomIntensityCastShadows,
                noiseIntensityCastShadows);
            }
            else if (styleAreaMode == StyleAreaMode.Specular)
            {
                DoStylingRandomSubArea(color, m_SpecularStylingRandomizeFoldout, specularStyle,
                enableSpecularRandomizer,
                noise1SizeSpecular,
                noise1SeedSpecular,
                noise2SeedSpecular,
                spacingRandomModeSpecular,
                spacingRandomIntensitySpecular,
                opacityRandomModeSpecular,
                opacityRandomIntensitySpecular,
                hardnessRandomModeSpecular,
                hardnessRandomIntensitySpecular,
                lengthRandomModeSpecular,
                lengthRandomIntensitySpecular,
                thicknessRandomModeSpecular,
                thicknessRandomIntensitySpecular,
                noiseIntensitySpecular);
            }
            else // RIM
            {
                DoStylingRandomSubArea(color, m_RimStylingRandomizeFoldout, rimStyle,
                enableRimRandomizer,
                rimNoise1Size,
                rimNoise1Seed,
                rimNoise2Seed,
                spacingRandomModeRim,
                spacingRandomIntensityRim,
                opacityRandomModeRim,
                opacityRandomIntensityRim,
                hardnessRandomModeRim,
                hardnessRandomIntensityRim,
                lengthRandomModeRim,
                lengthRandomIntensityRim,
                thicknessRandomModeRim,
                thicknessRandomIntensityRim,
                noiseIntensityRim);
            }
            GUI.enabled = true;

            EditorGUI.indentLevel += 1;
        }
        private void DoStylingShadingStylingArea(Color color)
        {
            m_ShadingStylingStylingFoldout.value = ToonEditorUtils.MakeTheToonShaderSubSectionHeaderWithFoldout(Styles.StylingSettings, m_ShadingStylingStylingFoldout.value, color);
            if (m_ShadingStylingStylingFoldout.value)
            {
                EditorGUI.indentLevel += 1;
                DoStylingStylingArea((Styling)shadingStyle.floatValue, StyleAreaMode.Shading, stylingShadingColor, stylingShadingThickness, stylingShadingThicknessControl, stylingShadingThicknessFalloff, stylingShadingOpacity, stylingShadingOpacityFalloff, stylingShadingHardness, stylingShadingHalftonesRoundness, stylingShadingHalftonesRoundnessFalloff, color);
                EditorGUI.indentLevel -= 1;
            }
        }

        private void DoStylingCastShadowsStylingArea(Color color)
        {
            m_CastShadowsStylingStylingFoldout.value = ToonEditorUtils.MakeTheToonShaderSubSectionHeaderWithFoldout(Styles.StylingSettings, m_CastShadowsStylingStylingFoldout.value, color);
            if (m_CastShadowsStylingStylingFoldout.value)
            {
                EditorGUI.indentLevel += 1;
                DoStylingStylingArea((Styling)castShadowsStyle.floatValue, StyleAreaMode.CastShadows, stylingCastShadowsColor, stylingCastShadowsThickness, stylingCastShadowsThicknessControl, stylingCastShadowsThicknessFalloff, stylingCastShadowsOpacity, stylingCastShadowsOpacityFalloff, stylingCastShadowsHardness, stylingCastShadowsHalftonesRoundness, stylingCastShadowsHalftonesRoundnessFalloff, color);
                EditorGUI.indentLevel -= 1;
            }
        }

        private void DoStylingSpecularStylingArea(Color color)
        {
            m_SpecularStylingStylingFoldout.value = ToonEditorUtils.MakeTheToonShaderSubSectionHeaderWithFoldout(Styles.StylingSettings, m_SpecularStylingStylingFoldout.value, color);
            if (m_SpecularStylingStylingFoldout.value)
            {
                EditorGUI.indentLevel += 1;
                DoStylingStylingArea((Styling)specularStyle.floatValue, StyleAreaMode.Specular, stylingSpecularColor, stylingSpecularThickness, stylingSpecularThicknessControl, stylingSpecularThicknessFalloff, stylingSpecularOpacity, stylingSpecularOpacityFalloff, stylingSpecularHardness, stylingSpecularHalftonesRoundness, stylingSpecularHalftonesRoundnessFalloff, color);        
                EditorGUI.indentLevel -= 1;
            }

        }
        private void DoStylingRimStylingArea(Color color)
        {
            m_RimStylingStylingFoldout.value = ToonEditorUtils.MakeTheToonShaderSubSectionHeaderWithFoldout(Styles.StylingSettings, m_RimStylingStylingFoldout.value, color);
            if (m_RimStylingStylingFoldout.value)
            {
                EditorGUI.indentLevel += 1;
                DoStylingStylingArea((Styling)rimStyle.floatValue, StyleAreaMode.Rim, stylingRimColor, stylingRimThickness, stylingRimThicknessControl, stylingRimThicknessFalloff, stylingRimOpacity, stylingRimOpacityFalloff, stylingRimHardness, stylingRimHalftonesRoundness, stylingRimHalftonesRoundnessFalloff, color);
                EditorGUI.indentLevel -= 1;
            }

        }


        private void DoStylingStyleDensityRotationArea(Styling shadingStyle,
                                                        SavedBool foldout,
                                                        MaterialProperty styleDensity,
                                                        MaterialProperty stylingRotation, 
                                                        MaterialProperty stylingRotationBetweenCells,
                                                        MaterialProperty halftoneShadingOffset,
                                                        Color color)
        {

            //EditorGUILayout.LabelField("Hatching Directions");

            GUIContent temp;
            if (shadingStyle == Styling.Hatching)
            {
                temp = Styles.StylingHatchingDensityDirection;
            }
            else if (shadingStyle == Styling.HalftonePatterns)
            {
                temp = Styles.StylingHalftonesDensityRotationOffset;
            }
            else //texture
            {
                temp = Styles.StylingHalftonesDensityRotationOffset;
            }
            foldout.value = ToonEditorUtils.MakeTheToonShaderSubSectionHeaderWithFoldout(temp, foldout.value, color);
            //float oriLabelWidth = EditorGUIUtility.labelWidth;
            EditorGUI.indentLevel += 1;
            //m_SurfaceOptionsFoldout.value = EditorGUILayout.BeginFoldoutHeaderGroup(m_SurfaceOptionsFoldout.value, Styles.SurfaceOptions);
            if (foldout.value)
            {
                //EditorUtils.DrawUILine(lightGreyForLines);
                EditorGUILayout.Space();

                EditorGUI.indentLevel -= 1;
                EditorStyles.label.normal.textColor = textColor;
                EditorGUILayout.LabelField("Density");
                EditorStyles.label.normal.textColor = oriCol;
                EditorGUI.indentLevel += 1;

                //if(styleAreaMode == StyleAreaMode.Shading)
                //{
                //    materialEditor.ShaderProperty(hatchingDensity, "Density");                    
                //}
                //else if(styleAreaMode == StyleAreaMode.Specular)
                //{
                //    materialEditor.ShaderProperty(stylingSpecularDensity, "Density");
                //} 
                //else
                //{

                //}
                materialEditor.ShaderProperty(styleDensity, "Density");
                makeAlwaysPositiv(styleDensity);

                EditorUtils.DrawSubMenuSeparation();

                if (shadingStyle == Styling.Hatching)
                {
                    EditorGUI.indentLevel -= 1;
                    EditorStyles.label.normal.textColor = textColor;
                    EditorGUILayout.LabelField("Direction");
                    EditorStyles.label.normal.textColor = oriCol;
                    EditorGUI.indentLevel += 1;

                    //materialEditor.ShaderProperty(stylingRotation, "Initial Direction");
                    DarkerSliderProperty(stylingRotation, "Initial Direction");
                    if (stylingRotationBetweenCells != null)
                    {
                        //materialEditor.ShaderProperty(stylingRotationBetweenCells, "Rotation Between Cells");
                        DarkerSliderProperty(stylingRotationBetweenCells, "Rotation Between Cells");
                    }
                }
                else if (shadingStyle == Styling.HalftonePatterns)
                {
                    EditorGUI.indentLevel -= 1;
                    EditorStyles.label.normal.textColor = textColor;
                    EditorGUILayout.LabelField("Rotation");
                    EditorStyles.label.normal.textColor = oriCol;
                    EditorGUI.indentLevel += 1;

                    //materialEditor.ShaderProperty(stylingRotation, "Halftone Rotation");
                    DarkerSliderProperty(stylingRotation, "Halftone Rotation");

                    EditorUtils.DrawSubMenuSeparation();


                    EditorGUI.indentLevel -= 1;
                    EditorStyles.label.normal.textColor = textColor;
                    EditorGUILayout.LabelField("Offset");
                    EditorStyles.label.normal.textColor = oriCol;
                    EditorGUI.indentLevel += 1;

                    //materialEditor.ShaderProperty(halftoneShadingOffset, "Offset");
                    DarkerSliderProperty(halftoneShadingOffset, "Offset");

                }
                else //texture
                {

                }
                EditorGUILayout.Space();


            }
            EditorGUI.indentLevel -= 1;

        }

        private void DoDrawSpaceArea(MaterialProperty drawSpace,
                                        MaterialProperty sSCameraDistanceScaled,
                                        MaterialProperty anchorSSToObjectsOrigin,
                                        MaterialProperty coordinateSystem,
                                        MaterialProperty polarCenterMode,
                                        MaterialProperty polarCenter
                                    )
        {
            drawSpace.floatValue = (int)(DrawSpace)EditorGUILayout.EnumPopup("Draw Space", (DrawSpace)drawSpace.floatValue);
            if( drawSpace.floatValue == 0 )
            {
                EditorGUILayout.HelpBox("For \"Object UV Space\", if you use the same material across various objects and want them to all have the same pattern size in styling," +
                    " be aware that different sized objects require different density settings! ", MessageType.Info);
            }
            if (drawSpace.floatValue == (int)DrawSpace.ScreenSpace)
            {
                //if (rp != RenderPipelineOptions.URP2D)
                //{
                    materialEditor.ShaderProperty(sSCameraDistanceScaled, "Scaled by Camera Distance");
                //}
                materialEditor.ShaderProperty(anchorSSToObjectsOrigin, "Anchor to Object's Origin");
                if (anchorSSToObjectsOrigin.floatValue == 1)
                {
                    EditorGUILayout.HelpBox("When the origin is not visible on the screen, it defaults to non-anchor mode!", MessageType.Info);

                }

            }
            coordinateSystem.floatValue = (int)(CoordinateSystem)EditorGUILayout.EnumPopup("Coordinate System", (CoordinateSystem)coordinateSystem.floatValue);
            if (coordinateSystem.floatValue == (int)CoordinateSystem.Polar)
            {
                if (drawSpace.floatValue == (int)(DrawSpace.ScreenSpace))
                {
                    polarCenterMode.floatValue = (int)(PolarCenterMode)EditorGUILayout.EnumPopup("Polar Center Mode", (PolarCenterMode)polarCenterMode.floatValue);
                    if (polarCenterMode.floatValue == (int)PolarCenterMode.ScreenSpace)
                    {
                        Vector2 screenSpacePolarCenter = new Vector2(Mathf.Clamp(polarCenter.vectorValue.x, 0, 1), Mathf.Clamp(polarCenter.vectorValue.y, 0, 1));
                        screenSpacePolarCenter = EditorGUILayout.Vector2Field("Polar Center", screenSpacePolarCenter);

                        polarCenter.vectorValue = screenSpacePolarCenter;
                    }
                    else
                    {
                        Vector3 worldSpacePolarCenter = EditorGUILayout.Vector3Field("Polar Center", new Vector3(polarCenter.vectorValue.x, polarCenter.vectorValue.y, polarCenter.vectorValue.z));
                        polarCenter.vectorValue = worldSpacePolarCenter;
                    }

                }
                else
                {
                    Vector2 screenSpacePolarCenter = new Vector2(Mathf.Clamp(polarCenter.vectorValue.x, 0, 1), Mathf.Clamp(polarCenter.vectorValue.y, 0, 1));
                    screenSpacePolarCenter = EditorGUILayout.Vector2Field("Polar Center", screenSpacePolarCenter);

                    polarCenter.vectorValue = screenSpacePolarCenter;
                }

            }
        }

        private void DoStylingDrawSpaceArea(StyleAreaMode styleAreaMode, SavedBool foldout, Color color)
        {
            foldout.value = ToonEditorUtils.MakeTheToonShaderSubSectionHeaderWithFoldout(Styles.StylingDrawSpace, foldout.value, color);
            if (foldout.value)
            {
                EditorGUILayout.Space();
                DisableGUIIfOptimizedShader();
                if (styleAreaMode == StyleAreaMode.Shading)
                {
                    DoDrawSpaceArea(drawSpace, sSCameraDistanceScaled, anchorSSToObjectsOrigin, coordinateSystem, polarCenterMode, polarCenter);
                }
                else if (styleAreaMode == StyleAreaMode.CastShadows)
                {
                    DoDrawSpaceArea(castShadowsDrawSpace, castShadowsSSCameraDistanceScaled, castShadowsAnchorSSToObjectsOrigin, castShadowsCoordinateSystem, castShadowsPolarCenterMode, castShadowsPolarCenter);
                }
                else if (styleAreaMode == StyleAreaMode.Specular)
                {
                    DoDrawSpaceArea(specularDrawSpace, specularSSCameraDistanceScaled, specularAnchorSSToObjectsOrigin, specularCoordinateSystem, specularPolarCenterMode, specularPolarCenter);
                }
                else
                {
                    DoDrawSpaceArea(rimDrawSpace, rimSSCameraDistanceScaled, rimAnchorSSToObjectsOrigin, rimCoordinateSystem, rimPolarCenterMode, rimPolarCenter);
                }
                GUI.enabled = true;


                EditorGUILayout.Space();
            }
        }

        private void DoOutlineArea(Material material)
        {


            DisableGUIIfOptimizedShader();
            EditorGUI.BeginChangeCheck();
            bool showOutline = ToonEditorUtils.MakeTheToonShaderSectionHeaderWithFoldoutWithToggle(Styles.OutlineHeader, m_OutlineAreaFoldout, enableOutline);
            if (EditorGUI.EndChangeCheck())
            {
                Shader shader = FindCorrectShader(material);
                material.shader = shader;
            }

            GUI.enabled = true;
            outlineAnimBool.target = showOutline == true;
            if (EditorGUILayout.BeginFadeGroup(outlineAnimBool.faded))
            {
                if (rp != RenderPipelineOptions.URP2D && !IsToon2DShader())
                {
                    EditorGUILayout.Space();

                    EditorGUILayout.HelpBox("Currently the outline feature breaks batching and requires additional drawcalls! " +
                        "That means the performance is not the best, but we will work on it in future updates. " +
                        "Please don't use outlines on a massive amount of objects, as then you will experience super bad performance!", MessageType.Warning);


                    EditorGUILayout.HelpBox("This feature currently doesn't work with Depth Priming turned on!", MessageType.Warning);
                }
                //if (alphaClipProp.floatValue == 1)
                //{
                //    EditorGUILayout.HelpBox("When Alpha Clipping is enabled, you can't use the Outline feature!", MessageType.Warning);
                //} else
                //{
                    EditorGUILayout.Space();
                    EditorGUI.indentLevel += 1;
                    EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth - 94;
                    materialEditor.ShaderProperty(outlineColor, "Outline Color");
                    EditorGUIUtility.labelWidth = oriLabelWidth;
                    materialEditor.ShaderProperty(outlineWidth, "Outline Width");

                    
                if (rp != RenderPipelineOptions.URP2D && !IsToon2DShader())
                {
                    EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth - 94;
                    GUI.enabled = false;
                    materialEditor.ShaderProperty(outlineDepthOffset, "Outline Depth Offset");
                    GUI.enabled = false;
                    EditorGUIUtility.labelWidth = oriLabelWidth;
                    materialEditor.ShaderProperty(outlineCameraDistanceImpact, "Outline Camera Distance Impact");
                    GUI.enabled = true;
                }
                else
                {
                    //materialEditor.ShaderProperty(outlineSoftness, "Outline Softness"); //TODO
                    //materialEditor.ShaderProperty(outlineMode, "Outline Mode");
                }


                    EditorGUI.indentLevel -= 1;
                
            }
            EditorGUILayout.EndFadeGroup();

        }



        void makeAlwaysPositiv(MaterialProperty materialProperty)
        {
            materialProperty.floatValue = Mathf.Max(materialProperty.floatValue, 0);
        }

    }
}

//#endif