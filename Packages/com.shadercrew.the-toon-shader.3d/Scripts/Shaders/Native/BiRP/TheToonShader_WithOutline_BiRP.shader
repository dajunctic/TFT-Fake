// Upgrade NOTE: replaced 'defined FOG_COMBINED_WITH_WORLD_POS' with 'defined (FOG_COMBINED_WITH_WORLD_POS)'

Shader "TheToonShader/BiRP/TheToonShader_WithOutline"
{
   Properties
   {
    _TheToonShaderIdentifier("_TheToonShaderIdentifier", Float) = 1.0
    _ShadingMode("Shading Mode", Float) = 0.0 
    _LightFunction("Light Function", Float) = 1.0 

    [MaterialToggle] _UseAlphaOnlyFromBaseMap("Use Alpha Only From BaseMap", Float) = 0.0
    [MaterialToggle] _EnableToonShading("Enable Toon Shading", Float) = 1.0

    _ShadingFunction("Shading Mode", Float) = 0.0 
    _GradientTex("Gradient Texture", 2D) = "white" {}
    _GradientMode("Gradient Mode", Float) = 0.0 
    _GradientBlending("Gradient Blending", Float) = 2.0 
    _GradientBlendFactor ("Gradient Blend Factor", Range(0,1)) = 1.0    
	_NumberOfCells ("Number Of Cells", int) = 2
	_CellTransitionSmoothness ("Cell Transition Smoothness", Range(0,1)) = 0.2
    [MaterialToggle] _SumLightsBeforePosterization("Sum Lights Before Posterization", Float) = 1.0
    [MaterialToggle] _ShadingUseLightColors("Use Light Colors", Float) = 0.0
        [MaterialToggle] _EnableShadows("Enable Shadows", Float) = 1.0
        _CoreShadowColor("Core Shadow Color", Color) = (0,0,0,1)
        _TerminatorPosition("Terminator Position",  Range(-1,1)) = 0.0
        _TerminatorWidth("Terminator Width",  Range(0,1)) = 0.0
        _TerminatorSmoothness("Terminator Smoothness",  Range(0,1)) = 0.0
        _FormShadowColor("Shadow Color", Color) = (0,0,0,1)
        [MaterialToggle] _ShadingAffectedByNormalMap("Shadows Affect By NormalMap", Float) = 1.0

        [MaterialToggle] _EnableCastShadows("Enable Cast Shadows", Float) = 1.0
        _CastShadowsStrength("Cast Shadows Strength", Range(0,1)) = 0.0
        _CastShadowsSmoothness ("Cast Shadows Smoothness", Range(0,1)) = 0.9
        _CastShadowColorMode("Cast Shadow Color Mode",  Float) = 0.0
        _CastShadowColor("Cast Shadow Color", Color) = (0,0,0,1)
        [MaterialToggle] _EnableSpecular("Enable Specular", Float) = 0.0
        _SpecularBlending("Specular Blending", Float) = 1.0 
        _SpecularColor("Specular Color", Color) = (1,1,0,1)
        _SpecularSize("Specular Size", Range(0,1)) = 0.0
        _SpecularSmoothness ("Specular Smoothness", Range(0,1)) = 0.0
        _SpecularOpacity ("Specular Opacity", Range(0,1)) = 1.0
        [MaterialToggle] _SpecularAffectedByNormalMap ("Specular Affected By NormalMap", Float) = 1.0
        [MaterialToggle] _SpecularUseLightColors ("Use Light Colors", Float) = 1.0
        [MaterialToggle] _EnableRim("Enable Rim", Float) = 0.0
        _RimBlending("Rim Blending", Float) = 0.0 
        _RimColor("Specular Color", Color) = (1,0,0,1)
        _RimSize("Rim Size", Range(0,1)) = 0.4
        _RimSmoothness ("Rim Smoothness", Range(0,1)) = 0.0    
        _RimOpacity ("Rim Opacity", Range(0,1)) = 1.0    
        _RimAffectedArea ("Rim Affected Area", Float) = 1.0 
        [MaterialToggle] _RimAffectedByNormalMap ("Rim Affected By NormalMap", Float) = 1.0
	[MaterialToggle] _EnableStyling ("Enable Shading Styling", float) = 0.0

    [MaterialToggle] _EnableStylingDistanceFade("_EnableStylingDistanceFade", Float) = 0.0 
    _StylingDFStartingDistance("_StylingDFStartingDistance", Float) = 0.0 
    _StylingDFFalloff("_StylingDFFalloff", Float) = 0.0 
    [MaterialToggle] _StylingAdjustDistanceFadeValue("_StylingAdjustDistanceFadeValue", Float) = 0.0 
    _StylingDistanceFadeValue("_StylingDistanceFadeValue", Range(0,1)) = 0.0
    [MaterialToggle] _HatchingAffectedByNormalMap("Affected by NormalMap", Float) = 0.0    
    [MaterialToggle] _EnableAntiAliasing("Enable Anti-Aliasing", Float) = 1.0    
	[MaterialToggle] _EnableShadingStyling ("Enable Shading Styling", float) = 1.0
    _StylingShadingSyncWithOtherStyling("_StylingShadingSyncWithOtherStyling", Float) = 0.0 

    _StylingColor("Styling Color", Color) = (0,0,0,1)    
    _ShadingStyle("Shading Style", Float) = 0.0 
    [MaterialToggle] _SyncWithLightPartitioning("Sync With Light Partitioning", Float) = 0.0 
    _NumberOfCellsHatching ("Number Of Cells", int) = 2
    _StylingTerminatorPosition("Terminator Position",  Range(-1,1)) = 0.0
    _StylingOvermodelingFactor("Overmodeling Factor", Range(0,1)) = 0.0
    _StylingShadingBlending("Shading Styling Blending", Float) = 0.0 
    [MaterialToggle] _StylingShadingIsInverted("Shading Styling is inverted", Float) = 0.0 
    _DrawSpace("Draw Space", Float) = 1.0 
    [MaterialToggle] _SSCameraDistanceScaled("Scale by Camera Distance", Float) = 1.0
    [MaterialToggle] _AnchorSSToObjectsOrigin("Anchor to Object's Origin", Float) = 1.0
    _CoordinateSystem("Coordinate System", Float) = 0.0 
    _PolarCenterMode("Polar Center Mode", Float) = 0.0 
    _PolarCenter ("Polar Center", Vector) = (0, 0, 0, 1)	
    _StylingShadingDensity("Hatching Density", Float) = 30.0    
    _StylingShadingInitialDirection("Hatching Initial Direction",  Range(0,360)) = 45
    [IntRange] _StylingShadingRotationBetweenCells("Hatching Rotation Between Cells",  Range(-180,180)) = 30
    _StylingShadingHalftonesOffset("Styling Shading HalftonesOffset", Range(0,1)) = 1
    _StylingShadingThicknessControl("Hatching Thickness Control", Float) = 0.0 
    _StylingShadingThickness("Hatching Thickness", Range(0,1)) = 0.9
    _StylingShadingOpacity("Hatching Opacity", Range(0,1)) = 1.0
    _StylingShadingOpacityFalloff("Hatching Opacity Falloff", Range(0,1)) = 0.0
    _StylingShadingThicknessFalloff("Hatching Thickness Falloff", Range(0,1)) = 0.5
    _StylingShadingHardness("Hatching Hardness", Range(0,1)) = 0.9
    _StylingShadingHalftonesRoundness("_StylingShadingHalftonesRoundness", Range(0,1)) = 1
    _StylingShadingHalftonesRoundnessFalloff("_StylingShadingHalftonesRoundnessFalloff", Range(0,1)) = 1
    [MaterialToggle] _EnableShadingRandomizer("_EnableShadingRandomizer", Float) = 0.0
    _ShadingNoise1Size ("_ShadingNoise1Size", Range(0,10)) = 1.0
    _ShadingNoise1Seed ("_ShadingNoise1Seed", Float) = 1.0
    _ShadingNoise2Seed ("_ShadingNoise2Seed", Float) = 1.0
    _NoiseIntensity ("_NoiseIntensity", Range(0,5)) = 0.0
    _SpacingRandomMode("SpacingRandomMode", Float) = 0.0 
    _SpacingRandomIntensity ("_SpacingRandomIntensity", Range(0,1)) = 0.0
    _OpacityRandomMode("_OpacityRandomMode", Float) = 0.0 
    _OpacityRandomIntensity ("_OpacityRandomIntensity", Range(0,1)) = 0.0
    _HardnessRandomMode("_HardnessRandomMode", Float) = 0.0 
    _HardnessRandomIntensity ("_HardnessRandomIntensity", Range(0,1)) = 0.0
    _LengthRandomMode("_LengthRandomMode", Float) = 0.0 
    _LengthRandomIntensity ("_LengthRandomIntensity", Range(0,1)) = 0.0
    _ThicknessRandomMode("_ThicknessRandomMode", Float) = 0.0 
    _ThicknesshRandomIntensity ("_HatchingSpacingRandomIntensity", Range(0,1)) = 0.0
    _EnableCastShadowsStyling ("Enable Cast Shadows Styling", Float) = 1.0 
    _StylingCastShadowsSyncWithOtherStyling("_StylingCastShadowsSyncWithOtherStyling", Float) = 1.0 

    _StylingCastShadowsColor("_StylingCastShadowsColor", Color) = (0,0,0,1)    
    _CastShadowsStyle ("Cast Shadows Style", Float) = 0.0 
    _CastShadowsNumberOfCellsHatching ("_CastShadowsNumberOfCellsHatching", int) = 1
    _StylingCastShadowsSmoothness ("_StylingCastShadowsSmoothness", Range(0,1)) = 1.0
    _StylingCastShadowsBlending("_StylingCastShadowsBlending", Float) = 0.0 
    [MaterialToggle] _StylingCastShadowsIsInverted("Cast Shadows Styling is inverted", Float) = 0.0
    _CastShadowsDrawSpace("Cast Shadows Draw Space", Float) = 1.0 
    [MaterialToggle] _CastShadowsSSCameraDistanceScaled("Cast Shadows Scale by Camera Distance", Float) = 1.0
    [MaterialToggle] _CastShadowsAnchorSSToObjectsOrigin("Cast Shadows Anchor to Object's Origin", Float) = 1.0
    _CastShadowsCoordinateSystem("Cast Shadows Coordinate System", Float) = 0.0 
    _CastShadowsPolarCenterMode("Cast Shadows  Polar Center Mode", Float) = 0.0 
    _CastShadowsPolarCenter ("Cast Shadows  Polar Center", Vector) = (0, 0, 0, 1)	
    _StylingCastShadowsDensity("Cast Shadows Hatching Density", Float) = 30.0    
    _StylingCastShadowsInitialDirection("Cast Shadows Hatching Initial Direction",  Range(0,360)) = 45
    [IntRange] _StylingCastShadowsRotationBetweenCells("Hatching Rotation Between Cells",  Range(-180,180)) = 30
    _StylingCastShadowsHalftonesOffset("Styling Cast Shadows HalftonesOffset", Range(0,1)) = 1
    _StylingCastShadowsOpacity("_StylingCastShadowsOpacity", Range(0,1)) = 1.0
    _StylingCastShadowsOpacityFalloff("_StylingCastShadowsOpacityFalloff", Range(0,1)) = 0.0
    _StylingCastShadowsThicknessControl("_StylingCastShadowsThicknessControl", Float) = 0.0 
    _StylingCastShadowsThickness("_StylingCastShadowsThickness", Range(0,1)) = 0.8
    _StylingCastShadowsThicknessFalloff("_StylingCastShadowsThicknessFalloff", Range(0,1)) = 0.1
    _StylingCastShadowsHardness("_StylingCastShadowsHardness", Range(0,1)) = 0.9
    _StylingCastShadowsHalftonesRoundness("_StylingCastShadowsHalftonesRoundness", Range(0,1)) = 1
    _StylingCastShadowsHalftonesRoundnessFalloff("_StylingCastShadowsHalftonesRoundnessFalloff", Range(0,1)) = 1
    [MaterialToggle] _EnableCastShadowsRandomizer("_EnableCastShadowsRandomizer", Float) = 0.0
    _CastShadowsNoise1Size ("_CastShadowsNoise1Size", Range(0,10)) = 1.0
    _CastShadowsNoise1Seed ("_CastShadowsNoise1Seed", Float) = 1.0
    _CastShadowsNoise2Seed ("_CastShadowsNoise2Seed", Float) = 1.0
    _CastShadowsNoiseIntensity ("_CastShadowsNoiseIntensity", Range(0,5)) = 0.0
    _CastShadowsSpacingRandomMode("_CastShadowsSpacingRandomMode", Float) = 0.0 
    _CastShadowsSpacingRandomIntensity ("_CastShadowsSpacingRandomIntensity", Range(0,1)) = 0.0
    _CastShadowsOpacityRandomMode("_CastShadowsOpacityRandomMode", Float) = 0.0 
    _CastShadowsOpacityRandomIntensity ("_CastShadowsOpacityRandomIntensity", Range(0,1)) = 0.0
    _CastShadowsHardnessRandomMode("_CastShadowsHardnessRandomMode", Float) = 0.0 
    _CastShadowsHardnessRandomIntensity ("_CastShadowsHardnessRandomIntensity", Range(0,1)) = 0.0
    _CastShadowsLengthRandomMode("_CastShadowsLengthRandomMode", Float) = 0.0 
    _CastShadowsLengthRandomIntensity ("_CastShadowsLengthRandomIntensity", Range(0,1)) = 0.0
    _CastShadowsThicknessRandomMode("_CastShadowsThicknessRandomMode", Float) = 0.0 
    _CastShadowsThicknesshRandomIntensity ("_CastShadowsThicknesshRandomIntensity", Range(0,1)) = 0.0
	[MaterialToggle] _EnableSpecularStyling ("Enable Specular Styling", float) = 0.0
    _StylingSpecularSyncWithOtherStyling("_StylingSpecularSyncWithOtherStyling", Float) = 0.0
    [MaterialToggle] _SyncWithSpecular("_SyncWithSpecular", Float) = 1.0 
    _StylingSpecularSize("_StylingSpecularSize", Range(0,1)) = 0.8
    _StylingSpecularSmoothness ("_StylingSpecularSmoothness", Range(0,1)) = 0.2

    [MaterialToggle] _StylingSpecularCutOutShading ("_StylingSpecularCutOutShading", float) = 0.0    
    [MaterialToggle] _StylingSpecularUseLightColors ("_StylingSpecularUseLightColors", float) = 0.0    

    _StylingSpecularColor("Styling Color", Color) = (1,1,0,1)    
    _SpecularStyle("Specular Style", Float) = 0.0 
    _StylingSpecularBlending("_SpecularBlending", Float) = 0.0 
    [MaterialToggle] _StylingSpecularIsInverted("Specular Styling is inverted", Float) = 0.0
    _SpecularDrawSpace("Draw Space", Float) = 1.0 
    [MaterialToggle] _SpecularSSCameraDistanceScaled("Scale by Camera Distance", Float) = 1.0
    [MaterialToggle] _SpecularAnchorSSToObjectsOrigin("Anchor to Object's Origin", Float) = 1.0
    _SpecularCoordinateSystem("Coordinate System", Float) = 0.0 
    _SpecularPolarCenterMode("Polar Center Mode", Float) = 0.0 
    _SpecularPolarCenter ("Polar Center", Vector) = (0, 0, 0, 1)	
    _StylingSpecularDensity("_StylingSpecularDensity", Float) = 30.0
    _StylingSpecularRotation("_StylingSpecularRotation",  Range(0,360)) = 0
    _StylingSpecularHalftonesOffset("_StylingSpecularHalftoneOffset", Range(0,1)) = 1
    _StylingSpecularOpacity("_StylingSpeculaOpacity", Range(0,1)) = 1.0
    _StylingSpecularOpacityFalloff("_StylingSpeculaOpacityFalloff", Range(0,1)) = 0.0
    _StylingSpecularThicknessControl("_StylingSpecularThicknessControl", Float) = 0.0 
    _StylingSpecularThickness("_StylingSpecularThickness", Range(0,1)) = 0.8
    _StylingSpecularThicknessFalloff("_StylingSpecularThicknessFalloff", Range(0,1)) = 0.1
    _StylingSpecularHardness("_StylingSpeculaHardness", Range(0,1)) = 0.9
    _StylingSpecularHalftonesRoundness("_SpecularHalftoneRoundness", Range(0,1)) = 1
    _StylingSpecularHalftonesRoundnessFalloff("_SpecularHalftoneRoundnessFalloff", Range(0,1)) = 1
    [MaterialToggle] _EnableSpecularRandomizer("_EnableSpecularRandomizer", Float) = 0.0
    _SpecularNoise1Size ("_SpecularNoise1Size", Range(0,10)) = 1.0
    _SpecularNoise1Seed ("_SpecularNoise1Seed", Float) = 1.0
    _SpecularNoise2Seed ("_SpecularNoise2Seed", Float) = 1.0
    _SpecularNoiseIntensity ("_SpecularNoiseIntensity", Range(0,5)) = 0.0
    _SpecularSpacingRandomMode("_SpecularSpacingRandomMode", Float) = 0.0 
    _SpecularSpacingRandomIntensity ("_SpecularSpacingRandomIntensity", Range(0,1)) = 0.0
    _SpecularOpacityRandomMode("_SpecularOpacityRandomMode", Float) = 0.0 
    _SpecularOpacityRandomIntensity ("_SpecularOpacityRandomIntensity", Range(0,1)) = 0.0
    _SpecularHardnessRandomMode("_SpecularHardnessRandomMode", Float) = 0.0 
    _SpecularHardnessRandomIntensity ("_SpecularHardnessRandomIntensity", Range(0,1)) = 0.0
    _SpecularLengthRandomMode("_SpecularLengthRandomMode", Float) = 0.0 
    _SpecularLengthRandomIntensity ("_SpecularLengthRandomIntensity", Range(0,1)) = 0.0
    _SpecularThicknessRandomMode("_ThicknessRandomMode", Float) = 0.0 
    _SpecularThicknesshRandomIntensity ("_HatchingSpacingRandomIntensity", Range(0,1)) = 0.0
	[MaterialToggle] _EnableRimStyling ("Enable Shading Styling", float) = 0.0
    _StylingRimSyncWithOtherStyling("_StylingRimSyncWithOtherStyling", Float) = 0.0
    [MaterialToggle] _SyncWithRim("_SyncWithRim", Float) = 1.0 
    _StylingRimSize("_StylingRimSize", Range(0,1)) = 0.1
    _StylingRimSmoothness ("_StylingRimSmoothness", Range(0,1)) = 0.3
    _StylingRimAffectedArea ("Rim Affected Area", Float) = 2.0 

    _StylingRimColor("_StylingRimColor", Color) = (0,0,1,1) 
    _RimStyle("Rim Style", Float) = 0.0 
    _StylingRimBlending("_RimBlending", Float) = 0.0 
    [MaterialToggle] _StylingRimIsInverted("Rim Styling is inverted", Float) = 0.0
    _RimDrawSpace("Draw Space", Float) = 0.0 
    [MaterialToggle] _RimSSCameraDistanceScaled("Scale by Camera Distance", Float) = 1.0
    [MaterialToggle] _RimAnchorSSToObjectsOrigin("Anchor to Object's Origin", Float) = 1.0
    _RimCoordinateSystem("Coordinate System", Float) = 0.0 
    _RimPolarCenterMode("Polar Center Mode", Float) = 0.0 
    _RimPolarCenter ("Polar Center", Vector) = (0, 0, 0, 1)	    
    _StylingRimDensity("_StylingRimDensity", Float) = 30.0
    _StylingRimRotation("_StylingRimRotation",  Range(0,360)) = 0
    _StylingRimHalftonesOffset("_StylingRimHalftonesOffset", Range(0,1)) = 1
    _StylingRimThicknessControl("_StylingRimThicknessControl", Float) = 0.0 
    _StylingRimThickness("_StylingRimThickness", Range(0,1)) = 0.8
    _StylingRimThicknessFalloff("_StylingRimThicknessFalloff", Range(0,1)) = 0.1
    _StylingRimOpacity("_StylingRimOpacity", Range(0,1)) = 1.0
    _StylingRimOpacityFalloff("_StylingRimOpacityFalloff", Range(0,1)) = 0.0
    _StylingRimHardness("_StylingRimHardness", Range(0,1)) = 0.9
    _StylingRimHalftonesRoundness("_StylingRimHalftonesRoundness", Range(0,1)) = 1
    _StylingRimHalftonesRoundnessFalloff("_StylingRimHalftonesRoundnessFalloff", Range(0,1)) = 1
    [MaterialToggle] _EnableRimRandomizer("_EnableRimRandomizer", Float) = 0.0
    _RimNoise1Size ("_RimNoise1Size", Range(0,1)) = 1.0
    _RimNoise1Seed ("_RimNoise1Seed", Float) = 1.0
    _RimNoise2Seed ("_RimNoise2Seed", Float) = 1.0
    _RimNoiseIntensity ("_RimNoiseIntensity", Range(0,5)) = 0.0
    _RimSpacingRandomMode("_RimSpacingRandomMode", Float) = 0.0 
    _RimSpacingRandomIntensity ("_RimSpacingRandomIntensity", Range(0,1)) = 0.0
    _RimOpacityRandomMode("_RimOpacityRandomMode", Float) = 0.0 
    _RimOpacityRandomIntensity ("_RimOpacityRandomIntensity", Range(0,1)) = 0.0
    _RimHardnessRandomMode("_RimHardnessRandomMode", Float) = 0.0 
    _RimHardnessRandomIntensity ("_RimHardnessRandomIntensity", Range(0,1)) = 0.0
    _RimLengthRandomMode("_RimLengthRandomMode", Float) = 0.0 
    _RimLengthRandomIntensity ("_RimLengthRandomIntensity", Range(0,1)) = 0.0
    _RimThicknessRandomMode("_RimThicknessRandomMode", Float) = 0.0 
    _RimThicknesshRandomIntensity ("_RimThicknesshRandomIntensity", Range(0,1)) = 0.0
    _NoiseMap1("_NoiseMap1", 2D) = "white" {}
    _NoiseMap2("_NoiseMap2", 2D) = "white" {}

    _NoiseTextureQuality("_NoiseTextureQuality", Float) = 0.0 
    _HatchingCameraDistanceFade("Camera Distance Fade", Float) = 0.0
    _HalftonePatternCameraDistanceFade("Camera Distance Fade", Float) = 0.0
    [MaterialToggle] _EnableOutline ("Enable Outline", Float) = 0.0
    _OutlineColor("Outline Color", Color) = (0,0,0,1)
    _OutlineWidth("Outline Width", Range(1.5,40)) = 6
    _OutlineDepthOffset("Outline Depth Offset", Float) = 0.0 
    _OutlineCameraDistanceImpact ("Outline Camera Distance Impact", Range(0,1)) = 0.0
	[MainColor] _Color("Color", Color) = (1,1,1,1)
    [MainTexture] _MainTex("Albedo (RGB)", 2D) = "white" {}

    _Cutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0.5

    _Glossiness("Smoothness", Range(0,1)) = 0.5
    _GlossMapScale("Smoothness Scale", Range(0.0, 1.0)) = 1.0
    [Enum(Metallic Alpha,0,Albedo Alpha,1)] _SmoothnessTextureChannel ("Smoothness texture channel", Float) = 0
    [Gamma] _Metallic("Metallic", Range(0,1)) = 0.0
    _MetallicGlossMap("Gloss Map", 2D) = "white" {}
    _BumpScale ("Bump Scale", Float) = 1.0
    [Normal] _BumpMap ("Normal Map", 2D) = "bump" {}
    _Parallax ("Height Scale", Range (0.005, 0.08)) = 0.02
    _ParallaxMap ("Height Map", 2D) = "black" {}
    _OcclusionStrength("Occlusion Strength", Range(0, 1)) = 1.0
    _OcclusionMap ("Occlusion", 2D) = "white" {}
    _EmissionMap ("Emission", 2D) = "white" {}
    _EmissionColor ("Color", Color) = (0, 0, 0)
    _DetailMask("Detail Mask", 2D) = "white" {}
    _DetailAlbedoMap("Detail Albedo x2", 2D) = "grey" {}
    _DetailNormalMapScale ("Scale", Float) = 1.0
    [Normal] _DetailNormalMap ("Normal Map", 2D) = "bump" {}
    [Enum(UV0,0,UV1,1)] _UVSec("UV Set for secondary textures", Float) = 0
    [HideInInspector] _Mode ("__mode", Float) = 0.0

    [Enum(UnityEngine.Rendering.CullMode)] _Cull ("Cull", Float) = 2
    [HideInInspector] _AlphaClip("__clip", Float) = 0.0
   }
   SubShader
   {
      Tags { "RenderType" = "Opaque" "Queue" = "Geometry" }
      Pass
      {
		   Name "FORWARD"
		   Tags { "LightMode" = "ForwardBase" }
             Cull [_Cull]
    Cull [_Cull]
         CGPROGRAM
            #pragma vertex Vert
   #pragma fragment Frag

         #pragma target 3.0
         #pragma multi_compile_instancing
         #pragma multi_compile_fog
         #pragma multi_compile_fwdbase
         #include "HLSLSupport.cginc"
         #define UNITY_INSTANCED_LOD_FADE
         #define UNITY_INSTANCED_SH
         #define UNITY_INSTANCED_LIGHTMAPSTS

         #include "UnityShaderVariables.cginc"
         #include "UnityShaderUtilities.cginc"
         #include "UnityCG.cginc"
         #include "Lighting.cginc"
         #include "UnityPBSLighting.cginc"
         #include "AutoLight.cginc"
         #define SHADER_PASS SHADERPASS_FORWARD
         #define _PASSFORWARD 1
     #pragma shader_feature_local_fragment  _SHADING_COLOR _SHADING_BLINNPHONG _SHADING_PBR    
        #pragma shader_feature_local _NORMALMAP
        #pragma shader_feature_local _METALLICGLOSSMAP
        #pragma shader_feature_local _PARALLAXMAP
        #pragma shader_feature_local _DETAIL_MULX2
        #pragma shader_feature_local _EMISSION       
        #pragma shader_feature_local _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
   #define _STANDARD 1
#if defined(SHADER_API_GAMECORE)

	#define ZERO_INITIALIZE(type, name) name = (type)0;
	#define ZERO_INITIALIZE_ARRAY(type, name, arraySize) { for (int arrayIndex = 0; arrayIndex < arraySize; arrayIndex++) { name[arrayIndex] = (type)0; } }
	#define CALCULATE_TEXTURE2D_LOD(textureName, samplerName, coord2) textureName.CalculateLevelOfDetail(samplerName, coord2)
	#define TEXTURE2D(textureName)                Texture2D textureName
	#define TEXTURE2D_ARRAY(textureName)          Texture2DArray textureName
	#define TEXTURECUBE(textureName)              TextureCube textureName
	#define TEXTURECUBE_ARRAY(textureName)        TextureCubeArray textureName
	#define TEXTURE3D(textureName)                Texture3D textureName

	#define TEXTURE2D_FLOAT(textureName)          TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_FLOAT(textureName)    TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_FLOAT(textureName)        TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_FLOAT(textureName)  TEXTURECUBE_ARRAY(textureName)
	#define TEXTURE3D_FLOAT(textureName)          TEXTURE3D(textureName)

	#define TEXTURE2D_HALF(textureName)           TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_HALF(textureName)     TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_HALF(textureName)         TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_HALF(textureName)   TEXTURECUBE_ARRAY(textureName)
	#define TEXTURE3D_HALF(textureName)           TEXTURE3D(textureName)

	#define TEXTURE2D_SHADOW(textureName)         TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_SHADOW(textureName)   TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_SHADOW(textureName)       TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_SHADOW(textureName) TEXTURECUBE_ARRAY(textureName)

	#define RW_TEXTURE2D(type, textureName)       RWTexture2D<type> textureName
	#define RW_TEXTURE2D_ARRAY(type, textureName) RWTexture2DArray<type> textureName
	#define RW_TEXTURE3D(type, textureName)       RWTexture3D<type> textureName

	#define SAMPLER(samplerName)                  SamplerState samplerName
	#define SAMPLER_CMP(samplerName)              SamplerComparisonState samplerName
	#define ASSIGN_SAMPLER(samplerName, samplerValue) samplerName = samplerValue

	#define TEXTURE2D_PARAM(textureName, samplerName)                 TEXTURE2D(textureName),         SAMPLER(samplerName)
	#define TEXTURE2D_ARRAY_PARAM(textureName, samplerName)           TEXTURE2D_ARRAY(textureName),   SAMPLER(samplerName)
	#define TEXTURECUBE_PARAM(textureName, samplerName)               TEXTURECUBE(textureName),       SAMPLER(samplerName)
	#define TEXTURECUBE_ARRAY_PARAM(textureName, samplerName)         TEXTURECUBE_ARRAY(textureName), SAMPLER(samplerName)
	#define TEXTURE3D_PARAM(textureName, samplerName)                 TEXTURE3D(textureName),         SAMPLER(samplerName)

	#define TEXTURE2D_SHADOW_PARAM(textureName, samplerName)          TEXTURE2D(textureName),         SAMPLER_CMP(samplerName)
	#define TEXTURE2D_ARRAY_SHADOW_PARAM(textureName, samplerName)    TEXTURE2D_ARRAY(textureName),   SAMPLER_CMP(samplerName)
	#define TEXTURECUBE_SHADOW_PARAM(textureName, samplerName)        TEXTURECUBE(textureName),       SAMPLER_CMP(samplerName)
	#define TEXTURECUBE_ARRAY_SHADOW_PARAM(textureName, samplerName)  TEXTURECUBE_ARRAY(textureName), SAMPLER_CMP(samplerName)

	#define TEXTURE2D_ARGS(textureName, samplerName)                textureName, samplerName
	#define TEXTURE2D_ARRAY_ARGS(textureName, samplerName)          textureName, samplerName
	#define TEXTURECUBE_ARGS(textureName, samplerName)              textureName, samplerName
	#define TEXTURECUBE_ARRAY_ARGS(textureName, samplerName)        textureName, samplerName
	#define TEXTURE3D_ARGS(textureName, samplerName)                textureName, samplerName

	#define TEXTURE2D_SHADOW_ARGS(textureName, samplerName)         textureName, samplerName
	#define TEXTURE2D_ARRAY_SHADOW_ARGS(textureName, samplerName)   textureName, samplerName
	#define TEXTURECUBE_SHADOW_ARGS(textureName, samplerName)       textureName, samplerName
	#define TEXTURECUBE_ARRAY_SHADOW_ARGS(textureName, samplerName) textureName, samplerName

	#define PLATFORM_SAMPLE_TEXTURE2D(textureName, samplerName, coord2)                               textureName.Sample(samplerName, coord2)
	#define PLATFORM_SAMPLE_TEXTURE2D_LOD(textureName, samplerName, coord2, lod)                      textureName.SampleLevel(samplerName, coord2, lod)
	#define PLATFORM_SAMPLE_TEXTURE2D_BIAS(textureName, samplerName, coord2, bias)                    textureName.SampleBias(samplerName, coord2, bias)
	#define PLATFORM_SAMPLE_TEXTURE2D_GRAD(textureName, samplerName, coord2, dpdx, dpdy)              textureName.SampleGrad(samplerName, coord2, dpdx, dpdy)
	#define PLATFORM_SAMPLE_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)                  textureName.Sample(samplerName, float3(coord2, index))
	#define PLATFORM_SAMPLE_TEXTURE2D_ARRAY_LOD(textureName, samplerName, coord2, index, lod)         textureName.SampleLevel(samplerName, float3(coord2, index), lod)
	#define PLATFORM_SAMPLE_TEXTURE2D_ARRAY_BIAS(textureName, samplerName, coord2, index, bias)       textureName.SampleBias(samplerName, float3(coord2, index), bias)
	#define PLATFORM_SAMPLE_TEXTURE2D_ARRAY_GRAD(textureName, samplerName, coord2, index, dpdx, dpdy) textureName.SampleGrad(samplerName, float3(coord2, index), dpdx, dpdy)
	#define PLATFORM_SAMPLE_TEXTURECUBE(textureName, samplerName, coord3)                             textureName.Sample(samplerName, coord3)
	#define PLATFORM_SAMPLE_TEXTURECUBE_LOD(textureName, samplerName, coord3, lod)                    textureName.SampleLevel(samplerName, coord3, lod)
	#define PLATFORM_SAMPLE_TEXTURECUBE_BIAS(textureName, samplerName, coord3, bias)                  textureName.SampleBias(samplerName, coord3, bias)
	#define PLATFORM_SAMPLE_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)                textureName.Sample(samplerName, float4(coord3, index))
	#define PLATFORM_SAMPLE_TEXTURECUBE_ARRAY_LOD(textureName, samplerName, coord3, index, lod)       textureName.SampleLevel(samplerName, float4(coord3, index), lod)
	#define PLATFORM_SAMPLE_TEXTURECUBE_ARRAY_BIAS(textureName, samplerName, coord3, index, bias)     textureName.SampleBias(samplerName, float4(coord3, index), bias)
	#define PLATFORM_SAMPLE_TEXTURE3D(textureName, samplerName, coord3)                               textureName.Sample(samplerName, coord3)
	#define PLATFORM_SAMPLE_TEXTURE3D_LOD(textureName, samplerName, coord3, lod)                      textureName.SampleLevel(samplerName, coord3, lod)

	#define SAMPLE_TEXTURE2D(textureName, samplerName, coord2)                               PLATFORM_SAMPLE_TEXTURE2D(textureName, samplerName, coord2)
	#define SAMPLE_TEXTURE2D_LOD(textureName, samplerName, coord2, lod)                      PLATFORM_SAMPLE_TEXTURE2D_LOD(textureName, samplerName, coord2, lod)
	#define SAMPLE_TEXTURE2D_BIAS(textureName, samplerName, coord2, bias)                    PLATFORM_SAMPLE_TEXTURE2D_BIAS(textureName, samplerName, coord2, bias)
	#define SAMPLE_TEXTURE2D_GRAD(textureName, samplerName, coord2, dpdx, dpdy)              PLATFORM_SAMPLE_TEXTURE2D_GRAD(textureName, samplerName, coord2, dpdx, dpdy)
	#define SAMPLE_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)                  PLATFORM_SAMPLE_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)
	#define SAMPLE_TEXTURE2D_ARRAY_LOD(textureName, samplerName, coord2, index, lod)         PLATFORM_SAMPLE_TEXTURE2D_ARRAY_LOD(textureName, samplerName, coord2, index, lod)
	#define SAMPLE_TEXTURE2D_ARRAY_BIAS(textureName, samplerName, coord2, index, bias)       PLATFORM_SAMPLE_TEXTURE2D_ARRAY_BIAS(textureName, samplerName, coord2, index, bias)
	#define SAMPLE_TEXTURE2D_ARRAY_GRAD(textureName, samplerName, coord2, index, dpdx, dpdy) PLATFORM_SAMPLE_TEXTURE2D_ARRAY_GRAD(textureName, samplerName, coord2, index, dpdx, dpdy)
	#define SAMPLE_TEXTURECUBE(textureName, samplerName, coord3)                             PLATFORM_SAMPLE_TEXTURECUBE(textureName, samplerName, coord3)
	#define SAMPLE_TEXTURECUBE_LOD(textureName, samplerName, coord3, lod)                    PLATFORM_SAMPLE_TEXTURECUBE_LOD(textureName, samplerName, coord3, lod)
	#define SAMPLE_TEXTURECUBE_BIAS(textureName, samplerName, coord3, bias)                  PLATFORM_SAMPLE_TEXTURECUBE_BIAS(textureName, samplerName, coord3, bias)
	#define SAMPLE_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)                PLATFORM_SAMPLE_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)
	#define SAMPLE_TEXTURECUBE_ARRAY_LOD(textureName, samplerName, coord3, index, lod)       PLATFORM_SAMPLE_TEXTURECUBE_ARRAY_LOD(textureName, samplerName, coord3, index, lod)
	#define SAMPLE_TEXTURECUBE_ARRAY_BIAS(textureName, samplerName, coord3, index, bias)     PLATFORM_SAMPLE_TEXTURECUBE_ARRAY_BIAS(textureName, samplerName, coord3, index, bias)
	#define SAMPLE_TEXTURE3D(textureName, samplerName, coord3)                               PLATFORM_SAMPLE_TEXTURE3D(textureName, samplerName, coord3)
	#define SAMPLE_TEXTURE3D_LOD(textureName, samplerName, coord3, lod)                      PLATFORM_SAMPLE_TEXTURE3D_LOD(textureName, samplerName, coord3, lod)

	#define SAMPLE_TEXTURE2D_SHADOW(textureName, samplerName, coord3)                    textureName.SampleCmpLevelZero(samplerName, (coord3).xy, (coord3).z)
	#define SAMPLE_TEXTURE2D_ARRAY_SHADOW(textureName, samplerName, coord3, index)       textureName.SampleCmpLevelZero(samplerName, float3((coord3).xy, index), (coord3).z)
	#define SAMPLE_TEXTURECUBE_SHADOW(textureName, samplerName, coord4)                  textureName.SampleCmpLevelZero(samplerName, (coord4).xyz, (coord4).w)
	#define SAMPLE_TEXTURECUBE_ARRAY_SHADOW(textureName, samplerName, coord4, index)     textureName.SampleCmpLevelZero(samplerName, float4((coord4).xyz, index), (coord4).w)

	#define SAMPLE_DEPTH_TEXTURE(textureName, samplerName, coord2)          SAMPLE_TEXTURE2D(textureName, samplerName, coord2).r
	#define SAMPLE_DEPTH_TEXTURE_LOD(textureName, samplerName, coord2, lod) SAMPLE_TEXTURE2D_LOD(textureName, samplerName, coord2, lod).r

	#define LOAD_TEXTURE2D(textureName, unCoord2)                                   textureName.Load(int3(unCoord2, 0))
	#define LOAD_TEXTURE2D_LOD(textureName, unCoord2, lod)                          textureName.Load(int3(unCoord2, lod))
	#define LOAD_TEXTURE2D_MSAA(textureName, unCoord2, sampleIndex)                 textureName.Load(unCoord2, sampleIndex)
	#define LOAD_TEXTURE2D_ARRAY(textureName, unCoord2, index)                      textureName.Load(int4(unCoord2, index, 0))
	#define LOAD_TEXTURE2D_ARRAY_MSAA(textureName, unCoord2, index, sampleIndex)    textureName.Load(int3(unCoord2, index), sampleIndex)
	#define LOAD_TEXTURE2D_ARRAY_LOD(textureName, unCoord2, index, lod)             textureName.Load(int4(unCoord2, index, lod))
	#define LOAD_TEXTURE3D(textureName, unCoord3)                                   textureName.Load(int4(unCoord3, 0))
	#define LOAD_TEXTURE3D_LOD(textureName, unCoord3, lod)                          textureName.Load(int4(unCoord3, lod))

	#define PLATFORM_SUPPORT_GATHER
	#define GATHER_TEXTURE2D(textureName, samplerName, coord2)                textureName.Gather(samplerName, coord2)
	#define GATHER_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)   textureName.Gather(samplerName, float3(coord2, index))
	#define GATHER_TEXTURECUBE(textureName, samplerName, coord3)              textureName.Gather(samplerName, coord3)
	#define GATHER_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index) textureName.Gather(samplerName, float4(coord3, index))
	#define GATHER_RED_TEXTURE2D(textureName, samplerName, coord2)            textureName.GatherRed(samplerName, coord2)
	#define GATHER_GREEN_TEXTURE2D(textureName, samplerName, coord2)          textureName.GatherGreen(samplerName, coord2)
	#define GATHER_BLUE_TEXTURE2D(textureName, samplerName, coord2)           textureName.GatherBlue(samplerName, coord2)
	#define GATHER_ALPHA_TEXTURE2D(textureName, samplerName, coord2)          textureName.GatherAlpha(samplerName, coord2)
#elif defined(SHADER_API_XBOXONE)
	#define ZERO_INITIALIZE(type, name) name = (type)0;
	#define ZERO_INITIALIZE_ARRAY(type, name, arraySize) { for (int arrayIndex = 0; arrayIndex < arraySize; arrayIndex++) { name[arrayIndex] = (type)0; } }
	#define CALCULATE_TEXTURE2D_LOD(textureName, samplerName, coord2) textureName.CalculateLevelOfDetail(samplerName, coord2)
	#define TEXTURE2D(textureName)                Texture2D textureName
	#define TEXTURE2D_ARRAY(textureName)          Texture2DArray textureName
	#define TEXTURECUBE(textureName)              TextureCube textureName
	#define TEXTURECUBE_ARRAY(textureName)        TextureCubeArray textureName
	#define TEXTURE3D(textureName)                Texture3D textureName

	#define TEXTURE2D_FLOAT(textureName)          TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_FLOAT(textureName)    TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_FLOAT(textureName)        TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_FLOAT(textureName)  TEXTURECUBE_ARRAY(textureName)
	#define TEXTURE3D_FLOAT(textureName)          TEXTURE3D(textureName)

	#define TEXTURE2D_HALF(textureName)           TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_HALF(textureName)     TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_HALF(textureName)         TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_HALF(textureName)   TEXTURECUBE_ARRAY(textureName)
	#define TEXTURE3D_HALF(textureName)           TEXTURE3D(textureName)

	#define TEXTURE2D_SHADOW(textureName)         TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_SHADOW(textureName)   TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_SHADOW(textureName)       TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_SHADOW(textureName) TEXTURECUBE_ARRAY(textureName)

	#define RW_TEXTURE2D(type, textureName)       RWTexture2D<type> textureName
	#define RW_TEXTURE2D_ARRAY(type, textureName) RWTexture2DArray<type> textureName
	#define RW_TEXTURE3D(type, textureName)       RWTexture3D<type> textureName

	#define SAMPLER(samplerName)                  SamplerState samplerName
	#define SAMPLER_CMP(samplerName)              SamplerComparisonState samplerName

	#define TEXTURE2D_PARAM(textureName, samplerName)                 TEXTURE2D(textureName),         SAMPLER(samplerName)
	#define TEXTURE2D_ARRAY_PARAM(textureName, samplerName)           TEXTURE2D_ARRAY(textureName),   SAMPLER(samplerName)
	#define TEXTURECUBE_PARAM(textureName, samplerName)               TEXTURECUBE(textureName),       SAMPLER(samplerName)
	#define TEXTURECUBE_ARRAY_PARAM(textureName, samplerName)         TEXTURECUBE_ARRAY(textureName), SAMPLER(samplerName)
	#define TEXTURE3D_PARAM(textureName, samplerName)                 TEXTURE3D(textureName),         SAMPLER(samplerName)

	#define TEXTURE2D_SHADOW_PARAM(textureName, samplerName)          TEXTURE2D(textureName),         SAMPLER_CMP(samplerName)
	#define TEXTURE2D_ARRAY_SHADOW_PARAM(textureName, samplerName)    TEXTURE2D_ARRAY(textureName),   SAMPLER_CMP(samplerName)
	#define TEXTURECUBE_SHADOW_PARAM(textureName, samplerName)        TEXTURECUBE(textureName),       SAMPLER_CMP(samplerName)
	#define TEXTURECUBE_ARRAY_SHADOW_PARAM(textureName, samplerName)  TEXTURECUBE_ARRAY(textureName), SAMPLER_CMP(samplerName)

	#define TEXTURE2D_ARGS(textureName, samplerName)                textureName, samplerName
	#define TEXTURE2D_ARRAY_ARGS(textureName, samplerName)          textureName, samplerName
	#define TEXTURECUBE_ARGS(textureName, samplerName)              textureName, samplerName
	#define TEXTURECUBE_ARRAY_ARGS(textureName, samplerName)        textureName, samplerName
	#define TEXTURE3D_ARGS(textureName, samplerName)                textureName, samplerName

	#define TEXTURE2D_SHADOW_ARGS(textureName, samplerName)         textureName, samplerName
	#define TEXTURE2D_ARRAY_SHADOW_ARGS(textureName, samplerName)   textureName, samplerName
	#define TEXTURECUBE_SHADOW_ARGS(textureName, samplerName)       textureName, samplerName
	#define TEXTURECUBE_ARRAY_SHADOW_ARGS(textureName, samplerName) textureName, samplerName

	#define SAMPLE_TEXTURE2D(textureName, samplerName, coord2)                               textureName.Sample(samplerName, coord2)
	#define SAMPLE_TEXTURE2D_LOD(textureName, samplerName, coord2, lod)                      textureName.SampleLevel(samplerName, coord2, lod)
	#define SAMPLE_TEXTURE2D_BIAS(textureName, samplerName, coord2, bias)                    textureName.SampleBias(samplerName, coord2, bias)
	#define SAMPLE_TEXTURE2D_GRAD(textureName, samplerName, coord2, dpdx, dpdy)              textureName.SampleGrad(samplerName, coord2, dpdx, dpdy)
	#define SAMPLE_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)                  textureName.Sample(samplerName, float3(coord2, index))
	#define SAMPLE_TEXTURE2D_ARRAY_LOD(textureName, samplerName, coord2, index, lod)         textureName.SampleLevel(samplerName, float3(coord2, index), lod)
	#define SAMPLE_TEXTURE2D_ARRAY_BIAS(textureName, samplerName, coord2, index, bias)       textureName.SampleBias(samplerName, float3(coord2, index), bias)
	#define SAMPLE_TEXTURE2D_ARRAY_GRAD(textureName, samplerName, coord2, index, dpdx, dpdy) textureName.SampleGrad(samplerName, float3(coord2, index), dpdx, dpdy)
	#define SAMPLE_TEXTURECUBE(textureName, samplerName, coord3)                             textureName.Sample(samplerName, coord3)
	#define SAMPLE_TEXTURECUBE_LOD(textureName, samplerName, coord3, lod)                    textureName.SampleLevel(samplerName, coord3, lod)
	#define SAMPLE_TEXTURECUBE_BIAS(textureName, samplerName, coord3, bias)                  textureName.SampleBias(samplerName, coord3, bias)
	#define SAMPLE_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)                textureName.Sample(samplerName, float4(coord3, index))
	#define SAMPLE_TEXTURECUBE_ARRAY_LOD(textureName, samplerName, coord3, index, lod)       textureName.SampleLevel(samplerName, float4(coord3, index), lod)
	#define SAMPLE_TEXTURECUBE_ARRAY_BIAS(textureName, samplerName, coord3, index, bias)     textureName.SampleBias(samplerName, float4(coord3, index), bias)
	#define SAMPLE_TEXTURE3D(textureName, samplerName, coord3)                               textureName.Sample(samplerName, coord3)
	#define SAMPLE_TEXTURE3D_LOD(textureName, samplerName, coord3, lod)                      textureName.SampleLevel(samplerName, coord3, lod)

	#define SAMPLE_TEXTURE2D_SHADOW(textureName, samplerName, coord3)                    textureName.SampleCmpLevelZero(samplerName, (coord3).xy, (coord3).z)
	#define SAMPLE_TEXTURE2D_ARRAY_SHADOW(textureName, samplerName, coord3, index)       textureName.SampleCmpLevelZero(samplerName, float3((coord3).xy, index), (coord3).z)
	#define SAMPLE_TEXTURECUBE_SHADOW(textureName, samplerName, coord4)                  textureName.SampleCmpLevelZero(samplerName, (coord4).xyz, (coord4).w)
	#define SAMPLE_TEXTURECUBE_ARRAY_SHADOW(textureName, samplerName, coord4, index)     textureName.SampleCmpLevelZero(samplerName, float4((coord4).xyz, index), (coord4).w)

	#define LOAD_TEXTURE2D(textureName, unCoord2)                                   textureName.Load(int3(unCoord2, 0))
	#define LOAD_TEXTURE2D_LOD(textureName, unCoord2, lod)                          textureName.Load(int3(unCoord2, lod))
	#define LOAD_TEXTURE2D_MSAA(textureName, unCoord2, sampleIndex)                 textureName.Load(unCoord2, sampleIndex)
	#define LOAD_TEXTURE2D_ARRAY(textureName, unCoord2, index)                      textureName.Load(int4(unCoord2, index, 0))
	#define LOAD_TEXTURE2D_ARRAY_MSAA(textureName, unCoord2, index, sampleIndex)    textureName.Load(int3(unCoord2, index), sampleIndex)
	#define LOAD_TEXTURE2D_ARRAY_LOD(textureName, unCoord2, index, lod)             textureName.Load(int4(unCoord2, index, lod))
	#define LOAD_TEXTURE3D(textureName, unCoord3)                                   textureName.Load(int4(unCoord3, 0))
	#define LOAD_TEXTURE3D_LOD(textureName, unCoord3, lod)                          textureName.Load(int4(unCoord3, lod))

	#define PLATFORM_SUPPORT_GATHER
	#define GATHER_TEXTURE2D(textureName, samplerName, coord2)                textureName.Gather(samplerName, coord2)
	#define GATHER_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)   textureName.Gather(samplerName, float3(coord2, index))
	#define GATHER_TEXTURECUBE(textureName, samplerName, coord3)              textureName.Gather(samplerName, coord3)
	#define GATHER_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index) textureName.Gather(samplerName, float4(coord3, index))
	#define GATHER_RED_TEXTURE2D(textureName, samplerName, coord2)            textureName.GatherRed(samplerName, coord2)
	#define GATHER_GREEN_TEXTURE2D(textureName, samplerName, coord2)          textureName.GatherGreen(samplerName, coord2)
	#define GATHER_BLUE_TEXTURE2D(textureName, samplerName, coord2)           textureName.GatherBlue(samplerName, coord2)
	#define GATHER_ALPHA_TEXTURE2D(textureName, samplerName, coord2)          textureName.GatherAlpha(samplerName, coord2)
#elif defined(SHADER_API_PSSL)
	#define ZERO_INITIALIZE(type, name) name = (type)0;
	#define ZERO_INITIALIZE_ARRAY(type, name, arraySize) { for (int arrayIndex = 0; arrayIndex < arraySize; arrayIndex++) { name[arrayIndex] = (type)0; } }
	#define CALCULATE_TEXTURE2D_LOD(textureName, samplerName, coord2) textureName.GetLOD(samplerName, coord2)
	#define TEXTURE2D(textureName)                Texture2D textureName
	#define TEXTURE2D_ARRAY(textureName)          Texture2DArray textureName
	#define TEXTURECUBE(textureName)              TextureCube textureName
	#define TEXTURECUBE_ARRAY(textureName)        TextureCubeArray textureName
	#define TEXTURE3D(textureName)                Texture3D textureName

	#define TEXTURE2D_FLOAT(textureName)          TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_FLOAT(textureName)    TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_FLOAT(textureName)        TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_FLOAT(textureName)  TEXTURECUBE_ARRAY(textureName)
	#define TEXTURE3D_FLOAT(textureName)          TEXTURE3D(textureName)

	#define TEXTURE2D_HALF(textureName)           TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_HALF(textureName)     TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_HALF(textureName)         TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_HALF(textureName)   TEXTURECUBE_ARRAY(textureName)
	#define TEXTURE3D_HALF(textureName)           TEXTURE3D(textureName)

	#define TEXTURE2D_SHADOW(textureName)         TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_SHADOW(textureName)   TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_SHADOW(textureName)       TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_SHADOW(textureName) TEXTURECUBE_ARRAY(textureName)

	#define RW_TEXTURE2D(type, textureName)       RW_Texture2D<type> textureName
	#define RW_TEXTURE2D_ARRAY(type, textureName) RW_Texture2D_Array<type> textureName
	#define RW_TEXTURE3D(type, textureName)       RW_Texture3D<type> textureName

	#define SAMPLER(samplerName)                  SamplerState samplerName
	#define SAMPLER_CMP(samplerName)              SamplerComparisonState samplerName

	#define TEXTURE2D_PARAM(textureName, samplerName)                 TEXTURE2D(textureName),         SAMPLER(samplerName)
	#define TEXTURE2D_ARRAY_PARAM(textureName, samplerName)           TEXTURE2D_ARRAY(textureName),   SAMPLER(samplerName)
	#define TEXTURECUBE_PARAM(textureName, samplerName)               TEXTURECUBE(textureName),       SAMPLER(samplerName)
	#define TEXTURECUBE_ARRAY_PARAM(textureName, samplerName)         TEXTURECUBE_ARRAY(textureName), SAMPLER(samplerName)
	#define TEXTURE3D_PARAM(textureName, samplerName)                 TEXTURE3D(textureName),         SAMPLER(samplerName)

	#define TEXTURE2D_SHADOW_PARAM(textureName, samplerName)          TEXTURE2D(textureName),         SAMPLER_CMP(samplerName)
	#define TEXTURE2D_ARRAY_SHADOW_PARAM(textureName, samplerName)    TEXTURE2D_ARRAY(textureName),   SAMPLER_CMP(samplerName)
	#define TEXTURECUBE_SHADOW_PARAM(textureName, samplerName)        TEXTURECUBE(textureName),       SAMPLER_CMP(samplerName)
	#define TEXTURECUBE_ARRAY_SHADOW_PARAM(textureName, samplerName)  TEXTURECUBE_ARRAY(textureName), SAMPLER_CMP(samplerName)

	#define TEXTURE2D_ARGS(textureName, samplerName)                textureName, samplerName
	#define TEXTURE2D_ARRAY_ARGS(textureName, samplerName)          textureName, samplerName
	#define TEXTURECUBE_ARGS(textureName, samplerName)              textureName, samplerName
	#define TEXTURECUBE_ARRAY_ARGS(textureName, samplerName)        textureName, samplerName
	#define TEXTURE3D_ARGS(textureName, samplerName)                textureName, samplerName

	#define TEXTURE2D_SHADOW_ARGS(textureName, samplerName)         textureName, samplerName
	#define TEXTURE2D_ARRAY_SHADOW_ARGS(textureName, samplerName)   textureName, samplerName
	#define TEXTURECUBE_SHADOW_ARGS(textureName, samplerName)       textureName, samplerName
	#define TEXTURECUBE_ARRAY_SHADOW_ARGS(textureName, samplerName) textureName, samplerName

	#define SAMPLE_TEXTURE2D(textureName, samplerName, coord2)                               textureName.Sample(samplerName, coord2)
	#define SAMPLE_TEXTURE2D_LOD(textureName, samplerName, coord2, lod)                      textureName.SampleLevel(samplerName, coord2, lod)
	#define SAMPLE_TEXTURE2D_BIAS(textureName, samplerName, coord2, bias)                    textureName.SampleBias(samplerName, coord2, bias)
	#define SAMPLE_TEXTURE2D_GRAD(textureName, samplerName, coord2, dpdx, dpdy)              textureName.SampleGrad(samplerName, coord2, dpdx, dpdy)
	#define SAMPLE_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)                  textureName.Sample(samplerName, float3(coord2, index))
	#define SAMPLE_TEXTURE2D_ARRAY_LOD(textureName, samplerName, coord2, index, lod)         textureName.SampleLevel(samplerName, float3(coord2, index), lod)
	#define SAMPLE_TEXTURE2D_ARRAY_BIAS(textureName, samplerName, coord2, index, bias)       textureName.SampleBias(samplerName, float3(coord2, index), bias)
	#define SAMPLE_TEXTURE2D_ARRAY_GRAD(textureName, samplerName, coord2, index, dpdx, dpdy) textureName.SampleGrad(samplerName, float3(coord2, index), dpdx, dpdy)
	#define SAMPLE_TEXTURECUBE(textureName, samplerName, coord3)                             textureName.Sample(samplerName, coord3)
	#define SAMPLE_TEXTURECUBE_LOD(textureName, samplerName, coord3, lod)                    textureName.SampleLevel(samplerName, coord3, lod)
	#define SAMPLE_TEXTURECUBE_BIAS(textureName, samplerName, coord3, bias)                  textureName.SampleBias(samplerName, coord3, bias)
	#define SAMPLE_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)                textureName.Sample(samplerName, float4(coord3, index))
	#define SAMPLE_TEXTURECUBE_ARRAY_LOD(textureName, samplerName, coord3, index, lod)       textureName.SampleLevel(samplerName, float4(coord3, index), lod)
	#define SAMPLE_TEXTURECUBE_ARRAY_BIAS(textureName, samplerName, coord3, index, bias)     textureName.SampleBias(samplerName, float4(coord3, index), bias)
	#define SAMPLE_TEXTURE3D(textureName, samplerName, coord3)                               textureName.Sample(samplerName, coord3)
	#define SAMPLE_TEXTURE3D_LOD(textureName, samplerName, coord3, lod)                      textureName.SampleLevel(samplerName, coord3, lod)

	#define SAMPLE_TEXTURE2D_SHADOW(textureName, samplerName, coord3)                    textureName.SampleCmpLevelZero(samplerName, (coord3).xy, (coord3).z)
	#define SAMPLE_TEXTURE2D_ARRAY_SHADOW(textureName, samplerName, coord3, index)       textureName.SampleCmpLevelZero(samplerName, float3((coord3).xy, index), (coord3).z)
	#define SAMPLE_TEXTURECUBE_SHADOW(textureName, samplerName, coord4)                  textureName.SampleCmpLevelZero(samplerName, (coord4).xyz, (coord4).w)
	#define SAMPLE_TEXTURECUBE_ARRAY_SHADOW(textureName, samplerName, coord4, index)     textureName.SampleCmpLevelZero(samplerName, float4((coord4).xyz, index), (coord4).w)

	#define LOAD_TEXTURE2D(textureName, unCoord2)                                   textureName.Load(int3(unCoord2, 0))
	#define LOAD_TEXTURE2D_LOD(textureName, unCoord2, lod)                          textureName.Load(int3(unCoord2, lod))
	#define LOAD_TEXTURE2D_MSAA(textureName, unCoord2, sampleIndex)                 textureName.Load(unCoord2, sampleIndex)
	#define LOAD_TEXTURE2D_ARRAY(textureName, unCoord2, index)                      textureName.Load(int4(unCoord2, index, 0))
	#define LOAD_TEXTURE2D_ARRAY_MSAA(textureName, unCoord2, index, sampleIndex)    textureName.Load(int3(unCoord2, index), sampleIndex)
	#define LOAD_TEXTURE2D_ARRAY_LOD(textureName, unCoord2, index, lod)             textureName.Load(int4(unCoord2, index, lod))
	#define LOAD_TEXTURE3D(textureName, unCoord3)                                   textureName.Load(int4(unCoord3, 0))
	#define LOAD_TEXTURE3D_LOD(textureName, unCoord3, lod)                          textureName.Load(int4(unCoord3, lod))

	#define PLATFORM_SUPPORT_GATHER
	#define GATHER_TEXTURE2D(textureName, samplerName, coord2)                textureName.Gather(samplerName, coord2)
	#define GATHER_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)   textureName.Gather(samplerName, float3(coord2, index))
	#define GATHER_TEXTURECUBE(textureName, samplerName, coord3)              textureName.Gather(samplerName, coord3)
	#define GATHER_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index) textureName.Gather(samplerName, float4(coord3, index))
	#define GATHER_RED_TEXTURE2D(textureName, samplerName, coord2)            textureName.GatherRed(samplerName, coord2)
	#define GATHER_GREEN_TEXTURE2D(textureName, samplerName, coord2)          textureName.GatherGreen(samplerName, coord2)
	#define GATHER_BLUE_TEXTURE2D(textureName, samplerName, coord2)           textureName.GatherBlue(samplerName, coord2)
	#define GATHER_ALPHA_TEXTURE2D(textureName, samplerName, coord2)          textureName.GatherAlpha(samplerName, coord2)
#elif defined(SHADER_API_D3D11)
	#define ZERO_INITIALIZE(type, name) name = (type)0;
	#define ZERO_INITIALIZE_ARRAY(type, name, arraySize) { for (int arrayIndex = 0; arrayIndex < arraySize; arrayIndex++) { name[arrayIndex] = (type)0; } }
	#define CALCULATE_TEXTURE2D_LOD(textureName, samplerName, coord2) textureName.CalculateLevelOfDetail(samplerName, coord2)
	#define TEXTURE2D(textureName)                Texture2D textureName
	#define TEXTURE2D_ARRAY(textureName)          Texture2DArray textureName
	#define TEXTURECUBE(textureName)              TextureCube textureName
	#define TEXTURECUBE_ARRAY(textureName)        TextureCubeArray textureName
	#define TEXTURE3D(textureName)                Texture3D textureName

	#define TEXTURE2D_FLOAT(textureName)          TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_FLOAT(textureName)    TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_FLOAT(textureName)        TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_FLOAT(textureName)  TEXTURECUBE_ARRAY(textureName)
	#define TEXTURE3D_FLOAT(textureName)          TEXTURE3D(textureName)

	#define TEXTURE2D_HALF(textureName)           TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_HALF(textureName)     TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_HALF(textureName)         TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_HALF(textureName)   TEXTURECUBE_ARRAY(textureName)
	#define TEXTURE3D_HALF(textureName)           TEXTURE3D(textureName)

	#define TEXTURE2D_SHADOW(textureName)         TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_SHADOW(textureName)   TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_SHADOW(textureName)       TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_SHADOW(textureName) TEXTURECUBE_ARRAY(textureName)

	#define RW_TEXTURE2D(type, textureName)       RWTexture2D<type> textureName
	#define RW_TEXTURE2D_ARRAY(type, textureName) RWTexture2DArray<type> textureName
	#define RW_TEXTURE3D(type, textureName)       RWTexture3D<type> textureName

	#define SAMPLER(samplerName)                  SamplerState samplerName
	#define SAMPLER_CMP(samplerName)              SamplerComparisonState samplerName

	#define TEXTURE2D_PARAM(textureName, samplerName)                 TEXTURE2D(textureName),         SAMPLER(samplerName)
	#define TEXTURE2D_ARRAY_PARAM(textureName, samplerName)           TEXTURE2D_ARRAY(textureName),   SAMPLER(samplerName)
	#define TEXTURECUBE_PARAM(textureName, samplerName)               TEXTURECUBE(textureName),       SAMPLER(samplerName)
	#define TEXTURECUBE_ARRAY_PARAM(textureName, samplerName)         TEXTURECUBE_ARRAY(textureName), SAMPLER(samplerName)
	#define TEXTURE3D_PARAM(textureName, samplerName)                 TEXTURE3D(textureName),         SAMPLER(samplerName)

	#define TEXTURE2D_SHADOW_PARAM(textureName, samplerName)          TEXTURE2D(textureName),         SAMPLER_CMP(samplerName)
	#define TEXTURE2D_ARRAY_SHADOW_PARAM(textureName, samplerName)    TEXTURE2D_ARRAY(textureName),   SAMPLER_CMP(samplerName)
	#define TEXTURECUBE_SHADOW_PARAM(textureName, samplerName)        TEXTURECUBE(textureName),       SAMPLER_CMP(samplerName)
	#define TEXTURECUBE_ARRAY_SHADOW_PARAM(textureName, samplerName)  TEXTURECUBE_ARRAY(textureName), SAMPLER_CMP(samplerName)

	#define TEXTURE2D_ARGS(textureName, samplerName)                textureName, samplerName
	#define TEXTURE2D_ARRAY_ARGS(textureName, samplerName)          textureName, samplerName
	#define TEXTURECUBE_ARGS(textureName, samplerName)              textureName, samplerName
	#define TEXTURECUBE_ARRAY_ARGS(textureName, samplerName)        textureName, samplerName
	#define TEXTURE3D_ARGS(textureName, samplerName)                textureName, samplerName

	#define TEXTURE2D_SHADOW_ARGS(textureName, samplerName)         textureName, samplerName
	#define TEXTURE2D_ARRAY_SHADOW_ARGS(textureName, samplerName)   textureName, samplerName
	#define TEXTURECUBE_SHADOW_ARGS(textureName, samplerName)       textureName, samplerName
	#define TEXTURECUBE_ARRAY_SHADOW_ARGS(textureName, samplerName) textureName, samplerName

	#define SAMPLE_TEXTURE2D(textureName, samplerName, coord2)                               textureName.Sample(samplerName, coord2)
	#define SAMPLE_TEXTURE2D_LOD(textureName, samplerName, coord2, lod)                      textureName.SampleLevel(samplerName, coord2, lod)
	#define SAMPLE_TEXTURE2D_BIAS(textureName, samplerName, coord2, bias)                    textureName.SampleBias(samplerName, coord2, bias)
	#define SAMPLE_TEXTURE2D_GRAD(textureName, samplerName, coord2, dpdx, dpdy)              textureName.SampleGrad(samplerName, coord2, dpdx, dpdy)
	#define SAMPLE_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)                  textureName.Sample(samplerName, float3(coord2, index))
	#define SAMPLE_TEXTURE2D_ARRAY_LOD(textureName, samplerName, coord2, index, lod)         textureName.SampleLevel(samplerName, float3(coord2, index), lod)
	#define SAMPLE_TEXTURE2D_ARRAY_BIAS(textureName, samplerName, coord2, index, bias)       textureName.SampleBias(samplerName, float3(coord2, index), bias)
	#define SAMPLE_TEXTURE2D_ARRAY_GRAD(textureName, samplerName, coord2, index, dpdx, dpdy) textureName.SampleGrad(samplerName, float3(coord2, index), dpdx, dpdy)
	#define SAMPLE_TEXTURECUBE(textureName, samplerName, coord3)                             textureName.Sample(samplerName, coord3)
	#define SAMPLE_TEXTURECUBE_LOD(textureName, samplerName, coord3, lod)                    textureName.SampleLevel(samplerName, coord3, lod)
	#define SAMPLE_TEXTURECUBE_BIAS(textureName, samplerName, coord3, bias)                  textureName.SampleBias(samplerName, coord3, bias)
	#define SAMPLE_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)                textureName.Sample(samplerName, float4(coord3, index))
	#define SAMPLE_TEXTURECUBE_ARRAY_LOD(textureName, samplerName, coord3, index, lod)       textureName.SampleLevel(samplerName, float4(coord3, index), lod)
	#define SAMPLE_TEXTURECUBE_ARRAY_BIAS(textureName, samplerName, coord3, index, bias)     textureName.SampleBias(samplerName, float4(coord3, index), bias)
	#define SAMPLE_TEXTURE3D(textureName, samplerName, coord3)                               textureName.Sample(samplerName, coord3)
	#define SAMPLE_TEXTURE3D_LOD(textureName, samplerName, coord3, lod)                      textureName.SampleLevel(samplerName, coord3, lod)

	#define SAMPLE_TEXTURE2D_SHADOW(textureName, samplerName, coord3)                    textureName.SampleCmpLevelZero(samplerName, (coord3).xy, (coord3).z)
	#define SAMPLE_TEXTURE2D_ARRAY_SHADOW(textureName, samplerName, coord3, index)       textureName.SampleCmpLevelZero(samplerName, float3((coord3).xy, index), (coord3).z)
	#define SAMPLE_TEXTURECUBE_SHADOW(textureName, samplerName, coord4)                  textureName.SampleCmpLevelZero(samplerName, (coord4).xyz, (coord4).w)
	#define SAMPLE_TEXTURECUBE_ARRAY_SHADOW(textureName, samplerName, coord4, index)     textureName.SampleCmpLevelZero(samplerName, float4((coord4).xyz, index), (coord4).w)

	#define LOAD_TEXTURE2D(textureName, unCoord2)                                   textureName.Load(int3(unCoord2, 0))
	#define LOAD_TEXTURE2D_LOD(textureName, unCoord2, lod)                          textureName.Load(int3(unCoord2, lod))
	#define LOAD_TEXTURE2D_MSAA(textureName, unCoord2, sampleIndex)                 textureName.Load(unCoord2, sampleIndex)
	#define LOAD_TEXTURE2D_ARRAY(textureName, unCoord2, index)                      textureName.Load(int4(unCoord2, index, 0))
	#define LOAD_TEXTURE2D_ARRAY_MSAA(textureName, unCoord2, index, sampleIndex)    textureName.Load(int3(unCoord2, index), sampleIndex)
	#define LOAD_TEXTURE2D_ARRAY_LOD(textureName, unCoord2, index, lod)             textureName.Load(int4(unCoord2, index, lod))
	#define LOAD_TEXTURE3D(textureName, unCoord3)                                   textureName.Load(int4(unCoord3, 0))
	#define LOAD_TEXTURE3D_LOD(textureName, unCoord3, lod)                          textureName.Load(int4(unCoord3, lod))

	#define PLATFORM_SUPPORT_GATHER
	#define GATHER_TEXTURE2D(textureName, samplerName, coord2)                textureName.Gather(samplerName, coord2)
	#define GATHER_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)   textureName.Gather(samplerName, float3(coord2, index))
	#define GATHER_TEXTURECUBE(textureName, samplerName, coord3)              textureName.Gather(samplerName, coord3)
	#define GATHER_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index) textureName.Gather(samplerName, float4(coord3, index))
	#define GATHER_RED_TEXTURE2D(textureName, samplerName, coord2)            textureName.GatherRed(samplerName, coord2)
	#define GATHER_GREEN_TEXTURE2D(textureName, samplerName, coord2)          textureName.GatherGreen(samplerName, coord2)
	#define GATHER_BLUE_TEXTURE2D(textureName, samplerName, coord2)           textureName.GatherBlue(samplerName, coord2)
	#define GATHER_ALPHA_TEXTURE2D(textureName, samplerName, coord2)          textureName.GatherAlpha(samplerName, coord2)

#elif defined(SHADER_API_METAL)
	#define ZERO_INITIALIZE(type, name) name = (type)0;
	#define ZERO_INITIALIZE_ARRAY(type, name, arraySize) { for (int arrayIndex = 0; arrayIndex < arraySize; arrayIndex++) { name[arrayIndex] = (type)0; } }
	#define CALCULATE_TEXTURE2D_LOD(textureName, samplerName, coord2) textureName.CalculateLevelOfDetail(samplerName, coord2)
	#define TEXTURE2D(textureName)                Texture2D textureName
	#define TEXTURE2D_ARRAY(textureName)          Texture2DArray textureName
	#define TEXTURECUBE(textureName)              TextureCube textureName
	#define TEXTURECUBE_ARRAY(textureName)        TextureCubeArray textureName
	#define TEXTURE3D(textureName)                Texture3D textureName

	#define TEXTURE2D_FLOAT(textureName)          Texture2D_float textureName
	#define TEXTURE2D_ARRAY_FLOAT(textureName)    Texture2DArray textureName    
	#define TEXTURECUBE_FLOAT(textureName)        TextureCube_float textureName
	#define TEXTURECUBE_ARRAY_FLOAT(textureName)  TextureCubeArray textureName  
	#define TEXTURE3D_FLOAT(textureName)          Texture3D_float textureName

	#define TEXTURE2D_HALF(textureName)           Texture2D_half textureName
	#define TEXTURE2D_ARRAY_HALF(textureName)     Texture2DArray textureName    
	#define TEXTURECUBE_HALF(textureName)         TextureCube_half textureName
	#define TEXTURECUBE_ARRAY_HALF(textureName)   TextureCubeArray textureName  
	#define TEXTURE3D_HALF(textureName)           Texture3D_half textureName

	#define TEXTURE2D_SHADOW(textureName)         TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_SHADOW(textureName)   TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_SHADOW(textureName)       TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_SHADOW(textureName) TEXTURECUBE_ARRAY(textureName)

	#define RW_TEXTURE2D(type, textureName)       RWTexture2D<type> textureName
	#define RW_TEXTURE2D_ARRAY(type, textureName) RWTexture2DArray<type> textureName
	#define RW_TEXTURE3D(type, textureName)       RWTexture3D<type> textureName

	#define SAMPLER(samplerName)                  SamplerState samplerName
	#define SAMPLER_CMP(samplerName)              SamplerComparisonState samplerName

	#define TEXTURE2D_PARAM(textureName, samplerName)                 TEXTURE2D(textureName),         SAMPLER(samplerName)
	#define TEXTURE2D_ARRAY_PARAM(textureName, samplerName)           TEXTURE2D_ARRAY(textureName),   SAMPLER(samplerName)
	#define TEXTURECUBE_PARAM(textureName, samplerName)               TEXTURECUBE(textureName),       SAMPLER(samplerName)
	#define TEXTURECUBE_ARRAY_PARAM(textureName, samplerName)         TEXTURECUBE_ARRAY(textureName), SAMPLER(samplerName)
	#define TEXTURE3D_PARAM(textureName, samplerName)                 TEXTURE3D(textureName),         SAMPLER(samplerName)

	#define TEXTURE2D_SHADOW_PARAM(textureName, samplerName)          TEXTURE2D(textureName),         SAMPLER_CMP(samplerName)
	#define TEXTURE2D_ARRAY_SHADOW_PARAM(textureName, samplerName)    TEXTURE2D_ARRAY(textureName),   SAMPLER_CMP(samplerName)
	#define TEXTURECUBE_SHADOW_PARAM(textureName, samplerName)        TEXTURECUBE(textureName),       SAMPLER_CMP(samplerName)
	#define TEXTURECUBE_ARRAY_SHADOW_PARAM(textureName, samplerName)  TEXTURECUBE_ARRAY(textureName), SAMPLER_CMP(samplerName)

	#define TEXTURE2D_ARGS(textureName, samplerName)                textureName, samplerName
	#define TEXTURE2D_ARRAY_ARGS(textureName, samplerName)          textureName, samplerName
	#define TEXTURECUBE_ARGS(textureName, samplerName)              textureName, samplerName
	#define TEXTURECUBE_ARRAY_ARGS(textureName, samplerName)        textureName, samplerName
	#define TEXTURE3D_ARGS(textureName, samplerName)                textureName, samplerName

	#define TEXTURE2D_SHADOW_ARGS(textureName, samplerName)         textureName, samplerName
	#define TEXTURE2D_ARRAY_SHADOW_ARGS(textureName, samplerName)   textureName, samplerName
	#define TEXTURECUBE_SHADOW_ARGS(textureName, samplerName)       textureName, samplerName
	#define TEXTURECUBE_ARRAY_SHADOW_ARGS(textureName, samplerName) textureName, samplerName

	#define SAMPLE_TEXTURE2D(textureName, samplerName, coord2)                               textureName.Sample(samplerName, coord2)
	#define SAMPLE_TEXTURE2D_LOD(textureName, samplerName, coord2, lod)                      textureName.SampleLevel(samplerName, coord2, lod)
	#define SAMPLE_TEXTURE2D_BIAS(textureName, samplerName, coord2, bias)                    textureName.SampleBias(samplerName, coord2, bias)
	#define SAMPLE_TEXTURE2D_GRAD(textureName, samplerName, coord2, dpdx, dpdy)              textureName.SampleGrad(samplerName, coord2, dpdx, dpdy)
	#define SAMPLE_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)                  textureName.Sample(samplerName, float3(coord2, index))
	#define SAMPLE_TEXTURE2D_ARRAY_LOD(textureName, samplerName, coord2, index, lod)         textureName.SampleLevel(samplerName, float3(coord2, index), lod)
	#define SAMPLE_TEXTURE2D_ARRAY_BIAS(textureName, samplerName, coord2, index, bias)       textureName.SampleBias(samplerName, float3(coord2, index), bias)
	#define SAMPLE_TEXTURE2D_ARRAY_GRAD(textureName, samplerName, coord2, index, dpdx, dpdy) textureName.SampleGrad(samplerName, float3(coord2, index), dpdx, dpdy)
	#define SAMPLE_TEXTURECUBE(textureName, samplerName, coord3)                             textureName.Sample(samplerName, coord3)
	#define SAMPLE_TEXTURECUBE_LOD(textureName, samplerName, coord3, lod)                    textureName.SampleLevel(samplerName, coord3, lod)
	#define SAMPLE_TEXTURECUBE_BIAS(textureName, samplerName, coord3, bias)                  textureName.SampleBias(samplerName, coord3, bias)
	#define SAMPLE_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)                textureName.Sample(samplerName, float4(coord3, index))
	#define SAMPLE_TEXTURECUBE_ARRAY_LOD(textureName, samplerName, coord3, index, lod)       textureName.SampleLevel(samplerName, float4(coord3, index), lod)
	#define SAMPLE_TEXTURECUBE_ARRAY_BIAS(textureName, samplerName, coord3, index, bias)     textureName.SampleBias(samplerName, float4(coord3, index), bias)
	#define SAMPLE_TEXTURE3D(textureName, samplerName, coord3)                               textureName.Sample(samplerName, coord3)
	#define SAMPLE_TEXTURE3D_LOD(textureName, samplerName, coord3, lod)                      textureName.SampleLevel(samplerName, coord3, lod)

	#define SAMPLE_TEXTURE2D_SHADOW(textureName, samplerName, coord3)                    textureName.SampleCmpLevelZero(samplerName, (coord3).xy, (coord3).z)
	#define SAMPLE_TEXTURE2D_ARRAY_SHADOW(textureName, samplerName, coord3, index)       textureName.SampleCmpLevelZero(samplerName, float3((coord3).xy, index), (coord3).z)
	#define SAMPLE_TEXTURECUBE_SHADOW(textureName, samplerName, coord4)                  textureName.SampleCmpLevelZero(samplerName, (coord4).xyz, (coord4).w)
	#define SAMPLE_TEXTURECUBE_ARRAY_SHADOW(textureName, samplerName, coord4, index)     textureName.SampleCmpLevelZero(samplerName, float4((coord4).xyz, index), (coord4).w)

	#define LOAD_TEXTURE2D(textureName, unCoord2)                                   textureName.Load(int3(unCoord2, 0))
	#define LOAD_TEXTURE2D_LOD(textureName, unCoord2, lod)                          textureName.Load(int3(unCoord2, lod))
	#define LOAD_TEXTURE2D_MSAA(textureName, unCoord2, sampleIndex)                 textureName.Load(unCoord2, sampleIndex)
	#define LOAD_TEXTURE2D_ARRAY(textureName, unCoord2, index)                      textureName.Load(int4(unCoord2, index, 0))
	#define LOAD_TEXTURE2D_ARRAY_MSAA(textureName, unCoord2, index, sampleIndex)    textureName.Load(int3(unCoord2, index), sampleIndex)
	#define LOAD_TEXTURE2D_ARRAY_LOD(textureName, unCoord2, index, lod)             textureName.Load(int4(unCoord2, index, lod))
	#define LOAD_TEXTURE3D(textureName, unCoord3)                                   textureName.Load(int4(unCoord3, 0))
	#define LOAD_TEXTURE3D_LOD(textureName, unCoord3, lod)                          textureName.Load(int4(unCoord3, lod))

	#define PLATFORM_SUPPORT_GATHER
	#define GATHER_TEXTURE2D(textureName, samplerName, coord2)                textureName.Gather(samplerName, coord2)
	#define GATHER_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)   textureName.Gather(samplerName, float3(coord2, index))
	#define GATHER_TEXTURECUBE(textureName, samplerName, coord3)              textureName.Gather(samplerName, coord3)
	#define GATHER_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index) textureName.Gather(samplerName, float4(coord3, index))
	#define GATHER_RED_TEXTURE2D(textureName, samplerName, coord2)            textureName.GatherRed(samplerName, coord2)
	#define GATHER_GREEN_TEXTURE2D(textureName, samplerName, coord2)          textureName.GatherGreen(samplerName, coord2)
	#define GATHER_BLUE_TEXTURE2D(textureName, samplerName, coord2)           textureName.GatherBlue(samplerName, coord2)
	#define GATHER_ALPHA_TEXTURE2D(textureName, samplerName, coord2)          textureName.GatherAlpha(samplerName, coord2)

#elif defined(SHADER_API_VULKAN)
	#define ZERO_INITIALIZE(type, name) name = (type)0;
	#define ZERO_INITIALIZE_ARRAY(type, name, arraySize) { for (int arrayIndex = 0; arrayIndex < arraySize; arrayIndex++) { name[arrayIndex] = (type)0; } }
	#define CALCULATE_TEXTURE2D_LOD(textureName, samplerName, coord2) textureName.CalculateLevelOfDetail(samplerName, coord2)
	#define TEXTURE2D(textureName)                Texture2D textureName
	#define TEXTURE2D_ARRAY(textureName)          Texture2DArray textureName
	#define TEXTURECUBE(textureName)              TextureCube textureName
	#define TEXTURECUBE_ARRAY(textureName)        TextureCubeArray textureName
	#define TEXTURE3D(textureName)                Texture3D textureName

	#define TEXTURE2D_FLOAT(textureName)          Texture2D_float textureName
	#define TEXTURE2D_ARRAY_FLOAT(textureName)    Texture2DArray textureName    
	#define TEXTURECUBE_FLOAT(textureName)        TextureCube_float textureName
	#define TEXTURECUBE_ARRAY_FLOAT(textureName)  TextureCubeArray textureName  
	#define TEXTURE3D_FLOAT(textureName)          Texture3D_float textureName

	#define TEXTURE2D_HALF(textureName)           Texture2D_half textureName
	#define TEXTURE2D_ARRAY_HALF(textureName)     Texture2DArray textureName    
	#define TEXTURECUBE_HALF(textureName)         TextureCube_half textureName
	#define TEXTURECUBE_ARRAY_HALF(textureName)   TextureCubeArray textureName  
	#define TEXTURE3D_HALF(textureName)           Texture3D_half textureName

	#define TEXTURE2D_SHADOW(textureName)         TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_SHADOW(textureName)   TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_SHADOW(textureName)       TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_SHADOW(textureName) TEXTURECUBE_ARRAY(textureName)

	#define RW_TEXTURE2D(type, textureName)       RWTexture2D<type> textureName
	#define RW_TEXTURE2D_ARRAY(type, textureName) RWTexture2DArray<type> textureName
	#define RW_TEXTURE3D(type, textureName)       RWTexture3D<type> textureName

	#define SAMPLER(samplerName)                  SamplerState samplerName
	#define SAMPLER_CMP(samplerName)              SamplerComparisonState samplerName

	#define TEXTURE2D_PARAM(textureName, samplerName)                 TEXTURE2D(textureName),         SAMPLER(samplerName)
	#define TEXTURE2D_ARRAY_PARAM(textureName, samplerName)           TEXTURE2D_ARRAY(textureName),   SAMPLER(samplerName)
	#define TEXTURECUBE_PARAM(textureName, samplerName)               TEXTURECUBE(textureName),       SAMPLER(samplerName)
	#define TEXTURECUBE_ARRAY_PARAM(textureName, samplerName)         TEXTURECUBE_ARRAY(textureName), SAMPLER(samplerName)
	#define TEXTURE3D_PARAM(textureName, samplerName)                 TEXTURE3D(textureName),         SAMPLER(samplerName)

	#define TEXTURE2D_SHADOW_PARAM(textureName, samplerName)          TEXTURE2D(textureName),         SAMPLER_CMP(samplerName)
	#define TEXTURE2D_ARRAY_SHADOW_PARAM(textureName, samplerName)    TEXTURE2D_ARRAY(textureName),   SAMPLER_CMP(samplerName)
	#define TEXTURECUBE_SHADOW_PARAM(textureName, samplerName)        TEXTURECUBE(textureName),       SAMPLER_CMP(samplerName)
	#define TEXTURECUBE_ARRAY_SHADOW_PARAM(textureName, samplerName)  TEXTURECUBE_ARRAY(textureName), SAMPLER_CMP(samplerName)

	#define TEXTURE2D_ARGS(textureName, samplerName)                textureName, samplerName
	#define TEXTURE2D_ARRAY_ARGS(textureName, samplerName)          textureName, samplerName
	#define TEXTURECUBE_ARGS(textureName, samplerName)              textureName, samplerName
	#define TEXTURECUBE_ARRAY_ARGS(textureName, samplerName)        textureName, samplerName
	#define TEXTURE3D_ARGS(textureName, samplerName)                textureName, samplerName

	#define TEXTURE2D_SHADOW_ARGS(textureName, samplerName)         textureName, samplerName
	#define TEXTURE2D_ARRAY_SHADOW_ARGS(textureName, samplerName)   textureName, samplerName
	#define TEXTURECUBE_SHADOW_ARGS(textureName, samplerName)       textureName, samplerName
	#define TEXTURECUBE_ARRAY_SHADOW_ARGS(textureName, samplerName) textureName, samplerName

	#define SAMPLE_TEXTURE2D(textureName, samplerName, coord2)                               textureName.Sample(samplerName, coord2)
	#define SAMPLE_TEXTURE2D_LOD(textureName, samplerName, coord2, lod)                      textureName.SampleLevel(samplerName, coord2, lod)
	#define SAMPLE_TEXTURE2D_BIAS(textureName, samplerName, coord2, bias)                    textureName.SampleBias(samplerName, coord2, bias)
	#define SAMPLE_TEXTURE2D_GRAD(textureName, samplerName, coord2, dpdx, dpdy)              textureName.SampleGrad(samplerName, coord2, dpdx, dpdy)
	#define SAMPLE_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)                  textureName.Sample(samplerName, float3(coord2, index))
	#define SAMPLE_TEXTURE2D_ARRAY_LOD(textureName, samplerName, coord2, index, lod)         textureName.SampleLevel(samplerName, float3(coord2, index), lod)
	#define SAMPLE_TEXTURE2D_ARRAY_BIAS(textureName, samplerName, coord2, index, bias)       textureName.SampleBias(samplerName, float3(coord2, index), bias)
	#define SAMPLE_TEXTURE2D_ARRAY_GRAD(textureName, samplerName, coord2, index, dpdx, dpdy) textureName.SampleGrad(samplerName, float3(coord2, index), dpdx, dpdy)
	#define SAMPLE_TEXTURECUBE(textureName, samplerName, coord3)                             textureName.Sample(samplerName, coord3)
	#define SAMPLE_TEXTURECUBE_LOD(textureName, samplerName, coord3, lod)                    textureName.SampleLevel(samplerName, coord3, lod)
	#define SAMPLE_TEXTURECUBE_BIAS(textureName, samplerName, coord3, bias)                  textureName.SampleBias(samplerName, coord3, bias)
	#define SAMPLE_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)                textureName.Sample(samplerName, float4(coord3, index))
	#define SAMPLE_TEXTURECUBE_ARRAY_LOD(textureName, samplerName, coord3, index, lod)       textureName.SampleLevel(samplerName, float4(coord3, index), lod)
	#define SAMPLE_TEXTURECUBE_ARRAY_BIAS(textureName, samplerName, coord3, index, bias)     textureName.SampleBias(samplerName, float4(coord3, index), bias)
	#define SAMPLE_TEXTURE3D(textureName, samplerName, coord3)                               textureName.Sample(samplerName, coord3)
	#define SAMPLE_TEXTURE3D_LOD(textureName, samplerName, coord3, lod)                      textureName.SampleLevel(samplerName, coord3, lod)

	#define SAMPLE_TEXTURE2D_SHADOW(textureName, samplerName, coord3)                    textureName.SampleCmpLevelZero(samplerName, (coord3).xy, (coord3).z)
	#define SAMPLE_TEXTURE2D_ARRAY_SHADOW(textureName, samplerName, coord3, index)       textureName.SampleCmpLevelZero(samplerName, float3((coord3).xy, index), (coord3).z)
	#define SAMPLE_TEXTURECUBE_SHADOW(textureName, samplerName, coord4)                  textureName.SampleCmpLevelZero(samplerName, (coord4).xyz, (coord4).w)
	#define SAMPLE_TEXTURECUBE_ARRAY_SHADOW(textureName, samplerName, coord4, index)     textureName.SampleCmpLevelZero(samplerName, float4((coord4).xyz, index), (coord4).w)

	#define LOAD_TEXTURE2D(textureName, unCoord2)                                   textureName.Load(int3(unCoord2, 0))
	#define LOAD_TEXTURE2D_LOD(textureName, unCoord2, lod)                          textureName.Load(int3(unCoord2, lod))
	#define LOAD_TEXTURE2D_MSAA(textureName, unCoord2, sampleIndex)                 textureName.Load(unCoord2, sampleIndex)
	#define LOAD_TEXTURE2D_ARRAY(textureName, unCoord2, index)                      textureName.Load(int4(unCoord2, index, 0))
	#define LOAD_TEXTURE2D_ARRAY_MSAA(textureName, unCoord2, index, sampleIndex)    textureName.Load(int3(unCoord2, index), sampleIndex)
	#define LOAD_TEXTURE2D_ARRAY_LOD(textureName, unCoord2, index, lod)             textureName.Load(int4(unCoord2, index, lod))
	#define LOAD_TEXTURE3D(textureName, unCoord3)                                   textureName.Load(int4(unCoord3, 0))
	#define LOAD_TEXTURE3D_LOD(textureName, unCoord3, lod)                          textureName.Load(int4(unCoord3, lod))

	#define PLATFORM_SUPPORT_GATHER
	#define GATHER_TEXTURE2D(textureName, samplerName, coord2)                textureName.Gather(samplerName, coord2)
	#define GATHER_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)   textureName.Gather(samplerName, float3(coord2, index))
	#define GATHER_TEXTURECUBE(textureName, samplerName, coord3)              textureName.Gather(samplerName, coord3)
	#define GATHER_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index) textureName.Gather(samplerName, float4(coord3, index))
	#define GATHER_RED_TEXTURE2D(textureName, samplerName, coord2)            textureName.GatherRed(samplerName, coord2)
	#define GATHER_GREEN_TEXTURE2D(textureName, samplerName, coord2)          textureName.GatherGreen(samplerName, coord2)
	#define GATHER_BLUE_TEXTURE2D(textureName, samplerName, coord2)           textureName.GatherBlue(samplerName, coord2)
	#define GATHER_ALPHA_TEXTURE2D(textureName, samplerName, coord2)          textureName.GatherAlpha(samplerName, coord2)

#elif defined(SHADER_API_SWITCH)
	#define ZERO_INITIALIZE(type, name) name = (type)0;
	#define ZERO_INITIALIZE_ARRAY(type, name, arraySize) { for (int arrayIndex = 0; arrayIndex < arraySize; arrayIndex++) { name[arrayIndex] = (type)0; } }
	#define CALCULATE_TEXTURE2D_LOD(textureName, samplerName, coord2) textureName.CalculateLevelOfDetail(samplerName, coord2)
	#define TEXTURE2D(textureName)                Texture2D textureName
	#define TEXTURE2D_ARRAY(textureName)          Texture2DArray textureName
	#define TEXTURECUBE(textureName)              TextureCube textureName
	#define TEXTURECUBE_ARRAY(textureName)        TextureCubeArray textureName
	#define TEXTURE3D(textureName)                Texture3D textureName

	#define TEXTURE2D_FLOAT(textureName)          Texture2D_float textureName
	#define TEXTURE2D_ARRAY_FLOAT(textureName)    Texture2DArray textureName    
	#define TEXTURECUBE_FLOAT(textureName)        TextureCube_float textureName
	#define TEXTURECUBE_ARRAY_FLOAT(textureName)  TextureCubeArray textureName  
	#define TEXTURE3D_FLOAT(textureName)          Texture3D_float textureName

	#define TEXTURE2D_HALF(textureName)           Texture2D_half textureName
	#define TEXTURE2D_ARRAY_HALF(textureName)     Texture2DArray textureName    
	#define TEXTURECUBE_HALF(textureName)         TextureCube_half textureName
	#define TEXTURECUBE_ARRAY_HALF(textureName)   TextureCubeArray textureName  
	#define TEXTURE3D_HALF(textureName)           Texture3D_half textureName

	#define TEXTURE2D_SHADOW(textureName)         TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_SHADOW(textureName)   TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_SHADOW(textureName)       TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_SHADOW(textureName) TEXTURECUBE_ARRAY(textureName)

	#define RW_TEXTURE2D(type, textureName)       RWTexture2D<type> textureName
	#define RW_TEXTURE2D_ARRAY(type, textureName) RWTexture2DArray<type> textureName
	#define RW_TEXTURE3D(type, textureName)       RWTexture3D<type> textureName

	#define SAMPLER(samplerName)                  SamplerState samplerName
	#define SAMPLER_CMP(samplerName)              SamplerComparisonState samplerName

	#define TEXTURE2D_PARAM(textureName, samplerName)                 TEXTURE2D(textureName),         SAMPLER(samplerName)
	#define TEXTURE2D_ARRAY_PARAM(textureName, samplerName)           TEXTURE2D_ARRAY(textureName),   SAMPLER(samplerName)
	#define TEXTURECUBE_PARAM(textureName, samplerName)               TEXTURECUBE(textureName),       SAMPLER(samplerName)
	#define TEXTURECUBE_ARRAY_PARAM(textureName, samplerName)         TEXTURECUBE_ARRAY(textureName), SAMPLER(samplerName)
	#define TEXTURE3D_PARAM(textureName, samplerName)                 TEXTURE3D(textureName),         SAMPLER(samplerName)

	#define TEXTURE2D_SHADOW_PARAM(textureName, samplerName)          TEXTURE2D(textureName),         SAMPLER_CMP(samplerName)
	#define TEXTURE2D_ARRAY_SHADOW_PARAM(textureName, samplerName)    TEXTURE2D_ARRAY(textureName),   SAMPLER_CMP(samplerName)
	#define TEXTURECUBE_SHADOW_PARAM(textureName, samplerName)        TEXTURECUBE(textureName),       SAMPLER_CMP(samplerName)
	#define TEXTURECUBE_ARRAY_SHADOW_PARAM(textureName, samplerName)  TEXTURECUBE_ARRAY(textureName), SAMPLER_CMP(samplerName)

	#define TEXTURE2D_ARGS(textureName, samplerName)                textureName, samplerName
	#define TEXTURE2D_ARRAY_ARGS(textureName, samplerName)          textureName, samplerName
	#define TEXTURECUBE_ARGS(textureName, samplerName)              textureName, samplerName
	#define TEXTURECUBE_ARRAY_ARGS(textureName, samplerName)        textureName, samplerName
	#define TEXTURE3D_ARGS(textureName, samplerName)                textureName, samplerName

	#define TEXTURE2D_SHADOW_ARGS(textureName, samplerName)         textureName, samplerName
	#define TEXTURE2D_ARRAY_SHADOW_ARGS(textureName, samplerName)   textureName, samplerName
	#define TEXTURECUBE_SHADOW_ARGS(textureName, samplerName)       textureName, samplerName
	#define TEXTURECUBE_ARRAY_SHADOW_ARGS(textureName, samplerName) textureName, samplerName

	#define SAMPLE_TEXTURE2D(textureName, samplerName, coord2)                               textureName.Sample(samplerName, coord2)
	#define SAMPLE_TEXTURE2D_LOD(textureName, samplerName, coord2, lod)                      textureName.SampleLevel(samplerName, coord2, lod)
	#define SAMPLE_TEXTURE2D_BIAS(textureName, samplerName, coord2, bias)                    textureName.SampleBias(samplerName, coord2, bias)
	#define SAMPLE_TEXTURE2D_GRAD(textureName, samplerName, coord2, dpdx, dpdy)              textureName.SampleGrad(samplerName, coord2, dpdx, dpdy)
	#define SAMPLE_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)                  textureName.Sample(samplerName, float3(coord2, index))
	#define SAMPLE_TEXTURE2D_ARRAY_LOD(textureName, samplerName, coord2, index, lod)         textureName.SampleLevel(samplerName, float3(coord2, index), lod)
	#define SAMPLE_TEXTURE2D_ARRAY_BIAS(textureName, samplerName, coord2, index, bias)       textureName.SampleBias(samplerName, float3(coord2, index), bias)
	#define SAMPLE_TEXTURE2D_ARRAY_GRAD(textureName, samplerName, coord2, index, dpdx, dpdy) textureName.SampleGrad(samplerName, float3(coord2, index), dpdx, dpdy)
	#define SAMPLE_TEXTURECUBE(textureName, samplerName, coord3)                             textureName.Sample(samplerName, coord3)
	#define SAMPLE_TEXTURECUBE_LOD(textureName, samplerName, coord3, lod)                    textureName.SampleLevel(samplerName, coord3, lod)
	#define SAMPLE_TEXTURECUBE_BIAS(textureName, samplerName, coord3, bias)                  textureName.SampleBias(samplerName, coord3, bias)
	#define SAMPLE_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)                textureName.Sample(samplerName, float4(coord3, index))
	#define SAMPLE_TEXTURECUBE_ARRAY_LOD(textureName, samplerName, coord3, index, lod)       textureName.SampleLevel(samplerName, float4(coord3, index), lod)
	#define SAMPLE_TEXTURECUBE_ARRAY_BIAS(textureName, samplerName, coord3, index, bias)     textureName.SampleBias(samplerName, float4(coord3, index), bias)
	#define SAMPLE_TEXTURE3D(textureName, samplerName, coord3)                               textureName.Sample(samplerName, coord3)
	#define SAMPLE_TEXTURE3D_LOD(textureName, samplerName, coord3, lod)                      textureName.SampleLevel(samplerName, coord3, lod)

	#define SAMPLE_TEXTURE2D_SHADOW(textureName, samplerName, coord3)                    textureName.SampleCmpLevelZero(samplerName, (coord3).xy, (coord3).z)
	#define SAMPLE_TEXTURE2D_ARRAY_SHADOW(textureName, samplerName, coord3, index)       textureName.SampleCmpLevelZero(samplerName, float3((coord3).xy, index), (coord3).z)
	#define SAMPLE_TEXTURECUBE_SHADOW(textureName, samplerName, coord4)                  textureName.SampleCmpLevelZero(samplerName, (coord4).xyz, (coord4).w)
	#define SAMPLE_TEXTURECUBE_ARRAY_SHADOW(textureName, samplerName, coord4, index)     textureName.SampleCmpLevelZero(samplerName, float4((coord4).xyz, index), (coord4).w)
	#define LOAD_TEXTURE2D(textureName, unCoord2)                       textureName.Load(int3(unCoord2, 0))
	#define LOAD_TEXTURE2D_LOD(textureName, unCoord2, lod)              textureName.Load(int3(unCoord2, lod))
	#define LOAD_TEXTURE2D_MSAA(textureName, unCoord2, sampleIndex)     textureName.Load(unCoord2, sampleIndex)
	#define LOAD_TEXTURE2D_ARRAY(textureName, unCoord2, index)          textureName.Load(int4(unCoord2, index, 0))
	#define LOAD_TEXTURE2D_ARRAY_LOD(textureName, unCoord2, index, lod) textureName.Load(int4(unCoord2, index, lod))
	#define LOAD_TEXTURE3D(textureName, unCoord3)                       textureName.Load(int4(unCoord3, 0))
	#define LOAD_TEXTURE3D_LOD(textureName, unCoord3, lod)              textureName.Load(int4(unCoord3, lod))

	#define PLATFORM_SUPPORT_GATHER
	#define GATHER_TEXTURE2D(textureName, samplerName, coord2)                textureName.Gather(samplerName, coord2)
	#define GATHER_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)   textureName.Gather(samplerName, float3(coord2, index))
	#define GATHER_TEXTURECUBE(textureName, samplerName, coord3)              textureName.Gather(samplerName, coord3)
	#define GATHER_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index) textureName.Gather(samplerName, float4(coord3, index))
	#define GATHER_RED_TEXTURE2D(textureName, samplerName, coord2)            textureName.GatherRed(samplerName, coord2)
	#define GATHER_GREEN_TEXTURE2D(textureName, samplerName, coord2)          textureName.GatherGreen(samplerName, coord2)
	#define GATHER_BLUE_TEXTURE2D(textureName, samplerName, coord2)           textureName.GatherBlue(samplerName, coord2)
	#define GATHER_ALPHA_TEXTURE2D(textureName, samplerName, coord2)          textureName.GatherAlpha(samplerName, coord2)

#elif defined(SHADER_API_GLCORE)
	#if (SHADER_TARGET >= 46)
	#define OPENGL4_1_SM5 1
	#else
	#define OPENGL4_1_SM5 0
	#endif
	#define ZERO_INITIALIZE(type, name) name = (type)0;
	#define ZERO_INITIALIZE_ARRAY(type, name, arraySize) { for (int arrayIndex = 0; arrayIndex < arraySize; arrayIndex++) { name[arrayIndex] = (type)0; } }
	#define CALCULATE_TEXTURE2D_LOD(textureName, samplerName, coord2) textureName.CalculateLevelOfDetail(samplerName, coord2)
	#define TEXTURE2D(textureName)                  Texture2D textureName
	#define TEXTURE2D_ARRAY(textureName)            Texture2DArray textureName
	#define TEXTURECUBE(textureName)                TextureCube textureName
	#define TEXTURECUBE_ARRAY(textureName)          TextureCubeArray textureName
	#define TEXTURE3D(textureName)                  Texture3D textureName

	#define TEXTURE2D_FLOAT(textureName)            TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_FLOAT(textureName)      TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_FLOAT(textureName)          TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_FLOAT(textureName)    TEXTURECUBE_ARRAY(textureName)
	#define TEXTURE3D_FLOAT(textureName)            TEXTURE3D(textureName)

	#define TEXTURE2D_HALF(textureName)             TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_HALF(textureName)       TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_HALF(textureName)           TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_HALF(textureName)     TEXTURECUBE_ARRAY(textureName)
	#define TEXTURE3D_HALF(textureName)             TEXTURE3D(textureName)

	#define TEXTURE2D_SHADOW(textureName)           TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_SHADOW(textureName)     TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_SHADOW(textureName)         TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_SHADOW(textureName)   TEXTURECUBE_ARRAY(textureName)

	#define RW_TEXTURE2D(type, textureName)         RWTexture2D<type> textureName
	#define RW_TEXTURE2D_ARRAY(type, textureName)   RWTexture2DArray<type> textureName
	#define RW_TEXTURE3D(type, textureName)         RWTexture3D<type> textureName

	#define SAMPLER(samplerName)                    SamplerState samplerName
	#define SAMPLER_CMP(samplerName)                SamplerComparisonState samplerName

	#define TEXTURE2D_PARAM(textureName, samplerName)                 TEXTURE2D(textureName),         SAMPLER(samplerName)
	#define TEXTURE2D_ARRAY_PARAM(textureName, samplerName)           TEXTURE2D_ARRAY(textureName),   SAMPLER(samplerName)
	#define TEXTURECUBE_PARAM(textureName, samplerName)               TEXTURECUBE(textureName),       SAMPLER(samplerName)
	#define TEXTURECUBE_ARRAY_PARAM(textureName, samplerName)         TEXTURECUBE_ARRAY(textureName), SAMPLER(samplerName)
	#define TEXTURE3D_PARAM(textureName, samplerName)                 TEXTURE3D(textureName),         SAMPLER(samplerName)

	#define TEXTURE2D_SHADOW_PARAM(textureName, samplerName)          TEXTURE2D(textureName),         SAMPLER_CMP(samplerName)
	#define TEXTURE2D_ARRAY_SHADOW_PARAM(textureName, samplerName)    TEXTURE2D_ARRAY(textureName),   SAMPLER_CMP(samplerName)
	#define TEXTURECUBE_SHADOW_PARAM(textureName, samplerName)        TEXTURECUBE(textureName),       SAMPLER_CMP(samplerName)
	#define TEXTURECUBE_ARRAY_SHADOW_PARAM(textureName, samplerName)  TEXTURECUBE_ARRAY(textureName), SAMPLER_CMP(samplerName)

	#define TEXTURE2D_ARGS(textureName, samplerName)                textureName, samplerName
	#define TEXTURE2D_ARRAY_ARGS(textureName, samplerName)          textureName, samplerName
	#define TEXTURECUBE_ARGS(textureName, samplerName)              textureName, samplerName
	#define TEXTURECUBE_ARRAY_ARGS(textureName, samplerName)        textureName, samplerName
	#define TEXTURE3D_ARGS(textureName, samplerName)                textureName, samplerName

	#define TEXTURE2D_SHADOW_ARGS(textureName, samplerName)         textureName, samplerName
	#define TEXTURE2D_ARRAY_SHADOW_ARGS(textureName, samplerName)   textureName, samplerName
	#define TEXTURECUBE_SHADOW_ARGS(textureName, samplerName)       textureName, samplerName
	#define TEXTURECUBE_ARRAY_SHADOW_ARGS(textureName, samplerName) textureName, samplerName

	#define SAMPLE_TEXTURE2D(textureName, samplerName, coord2)                               textureName.Sample(samplerName, coord2)
	#define SAMPLE_TEXTURE2D_LOD(textureName, samplerName, coord2, lod)                      textureName.SampleLevel(samplerName, coord2, lod)
	#define SAMPLE_TEXTURE2D_BIAS(textureName, samplerName, coord2, bias)                    textureName.SampleBias(samplerName, coord2, bias)
	#define SAMPLE_TEXTURE2D_GRAD(textureName, samplerName, coord2, ddx, ddy)                textureName.SampleGrad(samplerName, coord2, ddx, ddy)
	#define SAMPLE_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)                  textureName.Sample(samplerName, float3(coord2, index))
	#define SAMPLE_TEXTURE2D_ARRAY_LOD(textureName, samplerName, coord2, index, lod)         textureName.SampleLevel(samplerName, float3(coord2, index), lod)
	#define SAMPLE_TEXTURE2D_ARRAY_BIAS(textureName, samplerName, coord2, index, bias)       textureName.SampleBias(samplerName, float3(coord2, index), bias)
	#define SAMPLE_TEXTURE2D_ARRAY_GRAD(textureName, samplerName, coord2, index, dpdx, dpdy) textureName.SampleGrad(samplerName, float3(coord2, index), dpdx, dpdy)
	#define SAMPLE_TEXTURECUBE(textureName, samplerName, coord3)                             textureName.Sample(samplerName, coord3)
	#define SAMPLE_TEXTURECUBE_LOD(textureName, samplerName, coord3, lod)                    textureName.SampleLevel(samplerName, coord3, lod)
	#define SAMPLE_TEXTURECUBE_BIAS(textureName, samplerName, coord3, bias)                  textureName.SampleBias(samplerName, coord3, bias)
	#ifdef UNITY_NO_CUBEMAP_ARRAY
	#define SAMPLE_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)           ERROR_ON_UNSUPPORTED_FUNCTION(SAMPLE_TEXTURECUBE_ARRAY)
	#define SAMPLE_TEXTURECUBE_ARRAY_LOD(textureName, samplerName, coord3, index, lod)  ERROR_ON_UNSUPPORTED_FUNCTION(SAMPLE_TEXTURECUBE_ARRAY_LOD)
	#define SAMPLE_TEXTURECUBE_ARRAY_LOD(textureName, samplerName, coord3, index, bias) ERROR_ON_UNSUPPORTED_FUNCTION(SAMPLE_TEXTURECUBE_ARRAY_LOD)
	#else
	#define SAMPLE_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)           textureName.Sample(samplerName, float4(coord3, index))
	#define SAMPLE_TEXTURECUBE_ARRAY_LOD(textureName, samplerName, coord3, index, lod)  textureName.SampleLevel(samplerName, float4(coord3, index), lod)
	#define SAMPLE_TEXTURECUBE_ARRAY_BIAS(textureName, samplerName, coord3, index, bias)textureName.SampleBias(samplerName, float4(coord3, index), bias)
	#endif
	#define SAMPLE_TEXTURE3D(textureName, samplerName, coord3)                          textureName.Sample(samplerName, coord3)
	#define SAMPLE_TEXTURE3D_LOD(textureName, samplerName, coord3, lod)                 textureName.SampleLevel(samplerName, coord3, lod)

	#define SAMPLE_TEXTURE2D_SHADOW(textureName, samplerName, coord3)                   textureName.SampleCmpLevelZero(samplerName, (coord3).xy, (coord3).z)
	#define SAMPLE_TEXTURE2D_ARRAY_SHADOW(textureName, samplerName, coord3, index)      textureName.SampleCmpLevelZero(samplerName, float3((coord3).xy, index), (coord3).z)
	#define SAMPLE_TEXTURECUBE_SHADOW(textureName, samplerName, coord4)                 textureName.SampleCmpLevelZero(samplerName, (coord4).xyz, (coord4).w)
	#define SAMPLE_TEXTURECUBE_ARRAY_SHADOW(textureName, samplerName, coord4, index)    textureName.SampleCmpLevelZero(samplerName, float4((coord4).xyz, index), (coord4).w)

	#define LOAD_TEXTURE2D(textureName, unCoord2)                                   textureName.Load(int3(unCoord2, 0))
	#define LOAD_TEXTURE2D_LOD(textureName, unCoord2, lod)                          textureName.Load(int3(unCoord2, lod))
	#define LOAD_TEXTURE2D_MSAA(textureName, unCoord2, sampleIndex)                 textureName.Load(unCoord2, sampleIndex)
	#define LOAD_TEXTURE2D_ARRAY(textureName, unCoord2, index)                      textureName.Load(int4(unCoord2, index, 0))
	#define LOAD_TEXTURE2D_ARRAY_MSAA(textureName, unCoord2, index, sampleIndex)    textureName.Load(int3(unCoord2, index), sampleIndex)
	#define LOAD_TEXTURE2D_ARRAY_LOD(textureName, unCoord2, index, lod)             textureName.Load(int4(unCoord2, index, lod))

	#if OPENGL4_1_SM5
	#define PLATFORM_SUPPORT_GATHER
	#define GATHER_TEXTURE2D(textureName, samplerName, coord2)                  textureName.Gather(samplerName, coord2)
	#define GATHER_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)     textureName.Gather(samplerName, float3(coord2, index))
	#define GATHER_TEXTURECUBE(textureName, samplerName, coord3)                textureName.Gather(samplerName, coord3)
	#define GATHER_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)   textureName.Gather(samplerName, float4(coord3, index))
	#else
	#define GATHER_TEXTURE2D(textureName, samplerName, coord2)                  ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_TEXTURE2D)
	#define GATHER_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)     ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_TEXTURE2D_ARRAY)
	#define GATHER_TEXTURECUBE(textureName, samplerName, coord3)                ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_TEXTURECUBE)
	#define GATHER_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)   ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_TEXTURECUBE_ARRAY)
	#endif
	#elif defined(SHADER_API_GLES3)
	#if (SHADER_TARGET >= 40)
	#define GLES3_1_AEP 1
	#else
	#define GLES3_1_AEP 0
	#endif
	#define ZERO_INITIALIZE(type, name) name = (type)0;
	#define ZERO_INITIALIZE_ARRAY(type, name, arraySize) { for (int arrayIndex = 0; arrayIndex < arraySize; arrayIndex++) { name[arrayIndex] = (type)0; } }
	#define CALCULATE_TEXTURE2D_LOD(textureName, samplerName, coord2) textureName.CalculateLevelOfDetail(samplerName, coord2)
	#define TEXTURE2D(textureName)                  Texture2D textureName
	#define TEXTURE2D_ARRAY(textureName)            Texture2DArray textureName
	#define TEXTURECUBE(textureName)                TextureCube textureName
	#define TEXTURECUBE_ARRAY(textureName)          TextureCubeArray textureName
	#define TEXTURE3D(textureName)                  Texture3D textureName

	#define TEXTURE2D_FLOAT(textureName)            Texture2D_float textureName
	#define TEXTURE2D_ARRAY_FLOAT(textureName)      Texture2DArray textureName    
	#define TEXTURECUBE_FLOAT(textureName)          TextureCube_float textureName
	#define TEXTURECUBE_ARRAY_FLOAT(textureName)    TextureCubeArray textureName  
	#define TEXTURE3D_FLOAT(textureName)            Texture3D_float textureName

	#define TEXTURE2D_HALF(textureName)             Texture2D_half textureName
	#define TEXTURE2D_ARRAY_HALF(textureName)       Texture2DArray textureName    
	#define TEXTURECUBE_HALF(textureName)           TextureCube_half textureName
	#define TEXTURECUBE_ARRAY_HALF(textureName)     TextureCubeArray textureName  
	#define TEXTURE3D_HALF(textureName)             Texture3D_half textureName

	#define TEXTURE2D_SHADOW(textureName)           TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_SHADOW(textureName)     TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_SHADOW(textureName)         TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_SHADOW(textureName)   TEXTURECUBE_ARRAY(textureName)

	#if GLES3_1_AEP
	#define RW_TEXTURE2D(type, textureName)         RWTexture2D<type> textureName
	#define RW_TEXTURE2D_ARRAY(type, textureName)   RWTexture2DArray<type> textureName
	#define RW_TEXTURE3D(type, textureName)         RWTexture3D<type> textureName
	#else
	#define RW_TEXTURE2D(type, textureName)         ERROR_ON_UNSUPPORTED_FUNCTION(RWTexture2D)
	#define RW_TEXTURE2D_ARRAY(type, textureName)   ERROR_ON_UNSUPPORTED_FUNCTION(RWTexture2DArray)
	#define RW_TEXTURE3D(type, textureName)         ERROR_ON_UNSUPPORTED_FUNCTION(RWTexture3D)
	#endif

	#define SAMPLER(samplerName)                    SamplerState samplerName
	#define SAMPLER_CMP(samplerName)                SamplerComparisonState samplerName

	#define TEXTURE2D_PARAM(textureName, samplerName)                 TEXTURE2D(textureName),         SAMPLER(samplerName)
	#define TEXTURE2D_ARRAY_PARAM(textureName, samplerName)           TEXTURE2D_ARRAY(textureName),   SAMPLER(samplerName)
	#define TEXTURECUBE_PARAM(textureName, samplerName)               TEXTURECUBE(textureName),       SAMPLER(samplerName)
	#define TEXTURECUBE_ARRAY_PARAM(textureName, samplerName)         TEXTURECUBE_ARRAY(textureName), SAMPLER(samplerName)
	#define TEXTURE3D_PARAM(textureName, samplerName)                 TEXTURE3D(textureName),         SAMPLER(samplerName)

	#define TEXTURE2D_SHADOW_PARAM(textureName, samplerName)          TEXTURE2D(textureName),         SAMPLER_CMP(samplerName)
	#define TEXTURE2D_ARRAY_SHADOW_PARAM(textureName, samplerName)    TEXTURE2D_ARRAY(textureName),   SAMPLER_CMP(samplerName)
	#define TEXTURECUBE_SHADOW_PARAM(textureName, samplerName)        TEXTURECUBE(textureName),       SAMPLER_CMP(samplerName)
	#define TEXTURECUBE_ARRAY_SHADOW_PARAM(textureName, samplerName)  TEXTURECUBE_ARRAY(textureName), SAMPLER_CMP(samplerName)

	#define TEXTURE2D_ARGS(textureName, samplerName)                textureName, samplerName
	#define TEXTURE2D_ARRAY_ARGS(textureName, samplerName)          textureName, samplerName
	#define TEXTURECUBE_ARGS(textureName, samplerName)              textureName, samplerName
	#define TEXTURECUBE_ARRAY_ARGS(textureName, samplerName)        textureName, samplerName
	#define TEXTURE3D_ARGS(textureName, samplerName)                textureName, samplerName

	#define TEXTURE2D_SHADOW_ARGS(textureName, samplerName)         textureName, samplerName
	#define TEXTURE2D_ARRAY_SHADOW_ARGS(textureName, samplerName)   textureName, samplerName
	#define TEXTURECUBE_SHADOW_ARGS(textureName, samplerName)       textureName, samplerName
	#define TEXTURECUBE_ARRAY_SHADOW_ARGS(textureName, samplerName) textureName, samplerName

	#define SAMPLE_TEXTURE2D(textureName, samplerName, coord2)                               textureName.Sample(samplerName, coord2)
	#define SAMPLE_TEXTURE2D_LOD(textureName, samplerName, coord2, lod)                      textureName.SampleLevel(samplerName, coord2, lod)
	#define SAMPLE_TEXTURE2D_BIAS(textureName, samplerName, coord2, bias)                    textureName.SampleBias(samplerName, coord2, bias)
	#define SAMPLE_TEXTURE2D_GRAD(textureName, samplerName, coord2, ddx, ddy)                textureName.SampleGrad(samplerName, coord2, ddx, ddy)
	#define SAMPLE_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)                  textureName.Sample(samplerName, float3(coord2, index))
	#define SAMPLE_TEXTURE2D_ARRAY_LOD(textureName, samplerName, coord2, index, lod)         textureName.SampleLevel(samplerName, float3(coord2, index), lod)
	#define SAMPLE_TEXTURE2D_ARRAY_BIAS(textureName, samplerName, coord2, index, bias)       textureName.SampleBias(samplerName, float3(coord2, index), bias)
	#define SAMPLE_TEXTURE2D_ARRAY_GRAD(textureName, samplerName, coord2, index, dpdx, dpdy) textureName.SampleGrad(samplerName, float3(coord2, index), dpdx, dpdy)
	#define SAMPLE_TEXTURECUBE(textureName, samplerName, coord3)                             textureName.Sample(samplerName, coord3)
	#define SAMPLE_TEXTURECUBE_LOD(textureName, samplerName, coord3, lod)                    textureName.SampleLevel(samplerName, coord3, lod)
	#define SAMPLE_TEXTURECUBE_BIAS(textureName, samplerName, coord3, bias)                  textureName.SampleBias(samplerName, coord3, bias)

	#ifdef UNITY_NO_CUBEMAP_ARRAY
	#define SAMPLE_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)           ERROR_ON_UNSUPPORTED_FUNCTION(SAMPLE_TEXTURECUBE_ARRAY)
	#define SAMPLE_TEXTURECUBE_ARRAY_LOD(textureName, samplerName, coord3, index, lod)  ERROR_ON_UNSUPPORTED_FUNCTION(SAMPLE_TEXTURECUBE_ARRAY_LOD)
	#define SAMPLE_TEXTURECUBE_ARRAY_BIAS(textureName, samplerName, coord3, index, bias)ERROR_ON_UNSUPPORTED_FUNCTION(SAMPLE_TEXTURECUBE_ARRAY_BIAS)
	#else
	#define SAMPLE_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)           textureName.Sample(samplerName, float4(coord3, index))
	#define SAMPLE_TEXTURECUBE_ARRAY_LOD(textureName, samplerName, coord3, index, lod)  textureName.SampleLevel(samplerName, float4(coord3, index), lod)
	#define SAMPLE_TEXTURECUBE_ARRAY_BIAS(textureName, samplerName, coord3, index, bias)textureName.SampleBias(samplerName, float4(coord3, index), bias)
	#endif

	#define SAMPLE_TEXTURE3D(textureName, samplerName, coord3)                          textureName.Sample(samplerName, coord3)
	#define SAMPLE_TEXTURE3D_LOD(textureName, samplerName, coord3, lod)                 textureName.SampleLevel(samplerName, coord3, lod)

	#define SAMPLE_TEXTURE2D_SHADOW(textureName, samplerName, coord3)                   textureName.SampleCmpLevelZero(samplerName, (coord3).xy, (coord3).z)
	#define SAMPLE_TEXTURE2D_ARRAY_SHADOW(textureName, samplerName, coord3, index)      textureName.SampleCmpLevelZero(samplerName, float3((coord3).xy, index), (coord3).z)
	#define SAMPLE_TEXTURECUBE_SHADOW(textureName, samplerName, coord4)                 textureName.SampleCmpLevelZero(samplerName, (coord4).xyz, (coord4).w)
	#define SAMPLE_TEXTURECUBE_ARRAY_SHADOW(textureName, samplerName, coord4, index)    textureName.SampleCmpLevelZero(samplerName, float4((coord4).xyz, index), (coord4).w)
	#define LOAD_TEXTURE2D(textureName, unCoord2)                                       textureName.Load(int3(unCoord2, 0))
	#define LOAD_TEXTURE2D_LOD(textureName, unCoord2, lod)                              textureName.Load(int3(unCoord2, lod))
	#define LOAD_TEXTURE2D_MSAA(textureName, unCoord2, sampleIndex)                     textureName.Load(unCoord2, sampleIndex)
	#define LOAD_TEXTURE2D_ARRAY(textureName, unCoord2, index)                          textureName.Load(int4(unCoord2, index, 0))
	#define LOAD_TEXTURE2D_ARRAY_MSAA(textureName, unCoord2, index, sampleIndex)        textureName.Load(int3(unCoord2, index), sampleIndex)
	#define LOAD_TEXTURE2D_ARRAY_LOD(textureName, unCoord2, index, lod)                 textureName.Load(int4(unCoord2, index, lod))
	#define LOAD_TEXTURE3D(textureName, unCoord3)                                       textureName.Load(int4(unCoord3, 0))
	#define LOAD_TEXTURE3D_LOD(textureName, unCoord3, lod)                              textureName.Load(int4(unCoord3, lod))

	#if GLES3_1_AEP
	#define PLATFORM_SUPPORT_GATHER
	#define GATHER_TEXTURE2D(textureName, samplerName, coord2)                  textureName.Gather(samplerName, coord2)
	#define GATHER_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)     textureName.Gather(samplerName, float3(coord2, index))
	#define GATHER_TEXTURECUBE(textureName, samplerName, coord3)                textureName.Gather(samplerName, coord3)
	#define GATHER_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)   textureName.Gather(samplerName, float4(coord3, index))
	#define GATHER_RED_TEXTURE2D(textureName, samplerName, coord2)              textureName.GatherRed(samplerName, coord2)
	#define GATHER_GREEN_TEXTURE2D(textureName, samplerName, coord2)            textureName.GatherGreen(samplerName, coord2)
	#define GATHER_BLUE_TEXTURE2D(textureName, samplerName, coord2)             textureName.GatherBlue(samplerName, coord2)
	#define GATHER_ALPHA_TEXTURE2D(textureName, samplerName, coord2)            textureName.GatherAlpha(samplerName, coord2)
	#else
	#define GATHER_TEXTURE2D(textureName, samplerName, coord2)                  ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_TEXTURE2D)
	#define GATHER_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)     ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_TEXTURE2D_ARRAY)
	#define GATHER_TEXTURECUBE(textureName, samplerName, coord3)                ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_TEXTURECUBE)
	#define GATHER_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)   ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_TEXTURECUBE_ARRAY)
	#define GATHER_RED_TEXTURE2D(textureName, samplerName, coord2)              ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_RED_TEXTURE2D)
	#define GATHER_GREEN_TEXTURE2D(textureName, samplerName, coord2)            ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_GREEN_TEXTURE2D)
	#define GATHER_BLUE_TEXTURE2D(textureName, samplerName, coord2)             ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_BLUE_TEXTURE2D)
	#define GATHER_ALPHA_TEXTURE2D(textureName, samplerName, coord2)            ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_ALPHA_TEXTURE2D)
	#endif
#elif defined(SHADER_API_GLES)
	#define uint int

	#define rcp(x) 1.0 / (x)
	#define ddx_fine ddx
	#define ddy_fine ddy
	#define asfloat
	#define asuint(x) asint(x)
	#define f32tof16
	#define f16tof32

	#define ERROR_ON_UNSUPPORTED_FUNCTION(funcName) #error #funcName is not supported on GLES 2.0
	#define ZERO_INITIALIZE(type, name) name = (type)0;
	#define ZERO_INITIALIZE_ARRAY(type, name, arraySize) { for (int arrayIndex = 0; arrayIndex < arraySize; arrayIndex++) { name[arrayIndex] = (type)0; } }
	#define CALCULATE_TEXTURE2D_LOD(textureName, samplerName, coord2) #error calculate Level of Detail not supported in GLES2
	#define TEXTURE2D(textureName)                          sampler2D textureName
	#define TEXTURE2D_ARRAY(textureName)                    samplerCUBE textureName 
	#define TEXTURECUBE(textureName)                        samplerCUBE textureName
	#define TEXTURECUBE_ARRAY(textureName)                  samplerCUBE textureName 
	#define TEXTURE3D(textureName)                          sampler3D textureName

	#define TEXTURE2D_FLOAT(textureName)                    sampler2D_float textureName
	#define TEXTURE2D_ARRAY_FLOAT(textureName)              TEXTURECUBE_FLOAT(textureName) 
	#define TEXTURECUBE_FLOAT(textureName)                  samplerCUBE_float textureName
	#define TEXTURECUBE_ARRAY_FLOAT(textureName)            TEXTURECUBE_FLOAT(textureName) 
	#define TEXTURE3D_FLOAT(textureName)                    sampler3D_float textureName

	#define TEXTURE2D_HALF(textureName)                     sampler2D_half textureName
	#define TEXTURE2D_ARRAY_HALF(textureName)               TEXTURECUBE_HALF(textureName) 
	#define TEXTURECUBE_HALF(textureName)                   samplerCUBE_half textureName
	#define TEXTURECUBE_ARRAY_HALF(textureName)             TEXTURECUBE_HALF(textureName) 
	#define TEXTURE3D_HALF(textureName)                     sampler3D_half textureName

	#define TEXTURE2D_SHADOW(textureName)                   SHADOW2D_TEXTURE_AND_SAMPLER textureName
	#define TEXTURE2D_ARRAY_SHADOW(textureName)             TEXTURECUBE_SHADOW(textureName) 
	#define TEXTURECUBE_SHADOW(textureName)                 SHADOWCUBE_TEXTURE_AND_SAMPLER textureName
	#define TEXTURECUBE_ARRAY_SHADOW(textureName)           TEXTURECUBE_SHADOW(textureName) 

	#define RW_TEXTURE2D(type, textureNam)                  ERROR_ON_UNSUPPORTED_FUNCTION(RWTexture2D)
	#define RW_TEXTURE2D_ARRAY(type, textureName)           ERROR_ON_UNSUPPORTED_FUNCTION(RWTexture2DArray)
	#define RW_TEXTURE3D(type, textureNam)                  ERROR_ON_UNSUPPORTED_FUNCTION(RWTexture3D)

	#define SAMPLER(samplerName)
	#define SAMPLER_CMP(samplerName)

	#define TEXTURE2D_PARAM(textureName, samplerName)                sampler2D textureName
	#define TEXTURE2D_ARRAY_PARAM(textureName, samplerName)          samplerCUBE textureName
	#define TEXTURECUBE_PARAM(textureName, samplerName)              samplerCUBE textureName
	#define TEXTURECUBE_ARRAY_PARAM(textureName, samplerName)        samplerCUBE textureName
	#define TEXTURE3D_PARAM(textureName, samplerName)                sampler3D textureName
	#define TEXTURE2D_SHADOW_PARAM(textureName, samplerName)         SHADOW2D_TEXTURE_AND_SAMPLER textureName
	#define TEXTURE2D_ARRAY_SHADOW_PARAM(textureName, samplerName)   SHADOWCUBE_TEXTURE_AND_SAMPLER textureName
	#define TEXTURECUBE_SHADOW_PARAM(textureName, samplerName)       SHADOWCUBE_TEXTURE_AND_SAMPLER textureName

	#define TEXTURE2D_ARGS(textureName, samplerName)               textureName
	#define TEXTURE2D_ARRAY_ARGS(textureName, samplerName)         textureName
	#define TEXTURECUBE_ARGS(textureName, samplerName)             textureName
	#define TEXTURECUBE_ARRAY_ARGS(textureName, samplerName)       textureName
	#define TEXTURE3D_ARGS(textureName, samplerName)               textureName
	#define TEXTURE2D_SHADOW_ARGS(textureName, samplerName)        textureName
	#define TEXTURE2D_ARRAY_SHADOW_ARGS(textureName, samplerName)  textureName
	#define TEXTURECUBE_SHADOW_ARGS(textureName, samplerName)      textureName

	#define SAMPLE_TEXTURE2D(textureName, samplerName, coord2) tex2D(textureName, coord2)

	#if (SHADER_TARGET >= 30)
	    #define SAMPLE_TEXTURE2D_LOD(textureName, samplerName, coord2, lod) tex2Dlod(textureName, float4(coord2, 0, lod))
	#else
	    #define SAMPLE_TEXTURE2D_LOD(textureName, samplerName, coord2, lod) SAMPLE_TEXTURE2D_BIAS(textureName, samplerName, coord2, lod)
	#endif

	#define SAMPLE_TEXTURE2D_BIAS(textureName, samplerName, coord2, bias)                       tex2Dbias(textureName, float4(coord2, 0, bias))
	#define SAMPLE_TEXTURE2D_GRAD(textureName, samplerName, coord2, ddx, ddy)                   SAMPLE_TEXTURE2D(textureName, samplerName, coord2)
	#define SAMPLE_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)                     ERROR_ON_UNSUPPORTED_FUNCTION(SAMPLE_TEXTURE2D_ARRAY)
	#define SAMPLE_TEXTURE2D_ARRAY_LOD(textureName, samplerName, coord2, index, lod)            ERROR_ON_UNSUPPORTED_FUNCTION(SAMPLE_TEXTURE2D_ARRAY_LOD)
	#define SAMPLE_TEXTURE2D_ARRAY_BIAS(textureName, samplerName, coord2, index, bias)          ERROR_ON_UNSUPPORTED_FUNCTION(SAMPLE_TEXTURE2D_ARRAY_BIAS)
	#define SAMPLE_TEXTURE2D_ARRAY_GRAD(textureName, samplerName, coord2, index, dpdx, dpdy)    ERROR_ON_UNSUPPORTED_FUNCTION(SAMPLE_TEXTURE2D_ARRAY_GRAD)
	#define SAMPLE_TEXTURECUBE(textureName, samplerName, coord3)                                texCUBE(textureName, coord3)
	#define SAMPLE_TEXTURECUBE_LOD(textureName, samplerName, coord3, lod)                       SAMPLE_TEXTURECUBE_BIAS(textureName, samplerName, coord3, lod)
	#define SAMPLE_TEXTURECUBE_BIAS(textureName, samplerName, coord3, bias)                     texCUBEbias(textureName, float4(coord3, bias))
	#define SAMPLE_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)                   ERROR_ON_UNSUPPORTED_FUNCTION(SAMPLE_TEXTURECUBE_ARRAY)
	#define SAMPLE_TEXTURECUBE_ARRAY_LOD(textureName, samplerName, coord3, index, lod)          ERROR_ON_UNSUPPORTED_FUNCTION(SAMPLE_TEXTURECUBE_ARRAY_LOD)
	#define SAMPLE_TEXTURECUBE_ARRAY_BIAS(textureName, samplerName, coord3, index, bias)        ERROR_ON_UNSUPPORTED_FUNCTION(SAMPLE_TEXTURECUBE_ARRAY_BIAS)
	#define SAMPLE_TEXTURE3D(textureName, samplerName, coord3)                                  tex3D(textureName, coord3)
	#define SAMPLE_TEXTURE3D_LOD(textureName, samplerName, coord3, lod)                         ERROR_ON_UNSUPPORTED_FUNCTION(SAMPLE_TEXTURE3D_LOD)

	#define SAMPLE_TEXTURE2D_SHADOW(textureName, samplerName, coord3)                           SHADOW2D_SAMPLE(textureName, samplerName, coord3)
	#define SAMPLE_TEXTURE2D_ARRAY_SHADOW(textureName, samplerName, coord3, index)              ERROR_ON_UNSUPPORTED_FUNCTION(SAMPLE_TEXTURE2D_ARRAY_SHADOW)
	#define SAMPLE_TEXTURECUBE_SHADOW(textureName, samplerName, coord4)                         SHADOWCUBE_SAMPLE(textureName, samplerName, coord4)
	#define SAMPLE_TEXTURECUBE_ARRAY_SHADOW(textureName, samplerName, coord4, index)            ERROR_ON_UNSUPPORTED_FUNCTION(SAMPLE_TEXTURECUBE_ARRAY_SHADOW)
	#define LOAD_TEXTURE2D(textureName, unCoord2)                                               half4(0, 0, 0, 0)
	#define LOAD_TEXTURE2D_LOD(textureName, unCoord2, lod)                                      half4(0, 0, 0, 0)
	#define LOAD_TEXTURE2D_MSAA(textureName, unCoord2, sampleIndex)                             half4(0, 0, 0, 0)
	#define LOAD_TEXTURE2D_ARRAY(textureName, unCoord2, index)                                  half4(0, 0, 0, 0)
	#define LOAD_TEXTURE2D_ARRAY_MSAA(textureName, unCoord2, index, sampleIndex)                half4(0, 0, 0, 0)
	#define LOAD_TEXTURE2D_ARRAY_LOD(textureName, unCoord2, index, lod)                         half4(0, 0, 0, 0)
	#define LOAD_TEXTURE3D(textureName, unCoord3)                                               ERROR_ON_UNSUPPORTED_FUNCTION(LOAD_TEXTURE3D)
	#define LOAD_TEXTURE3D_LOD(textureName, unCoord3, lod)                                      ERROR_ON_UNSUPPORTED_FUNCTION(LOAD_TEXTURE3D_LOD)
	#define GATHER_TEXTURE2D(textureName, samplerName, coord2)                  ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_TEXTURE2D)
	#define GATHER_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)     ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_TEXTURE2D_ARRAY)
	#define GATHER_TEXTURECUBE(textureName, samplerName, coord3)                ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_TEXTURECUBE)
	#define GATHER_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)   ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_TEXTURECUBE_ARRAY)
	#define GATHER_RED_TEXTURE2D(textureName, samplerName, coord2)              ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_RED_TEXTURE2D)
	#define GATHER_GREEN_TEXTURE2D(textureName, samplerName, coord2)            ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_GREEN_TEXTURE2D)
	#define GATHER_BLUE_TEXTURE2D(textureName, samplerName, coord2)             ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_BLUE_TEXTURE2D)
	#define GATHER_ALPHA_TEXTURE2D(textureName, samplerName, coord2)            ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_ALPHA_TEXTURE2D)

#else
#error unsupported shader api
#endif
#ifndef UNITY_BRANCH
#   define UNITY_BRANCH
#endif
#ifndef UNITY_FLATTEN
#   define UNITY_FLATTEN
#endif
#ifndef UNITY_UNROLL
#   define UNITY_UNROLL
#endif
#ifndef UNITY_UNROLLX
#   define UNITY_UNROLLX(_x)
#endif
#ifndef UNITY_LOOP
#   define UNITY_LOOP
#endif
#define _USINGTEXCOORD1 1
         struct VertexToPixel
         {
            UNITY_POSITION(pos);
            float3 worldPos : TEXCOORD0;
            float3 worldNormal : TEXCOORD1;
            float4 worldTangent : TEXCOORD2;
             float4 texcoord0 : TEXCOORD3;
             float4 texcoord1 : TEXCOORD4;
            float4 lmap : TEXCOORD8;
            #if UNITY_SHOULD_SAMPLE_SH
               half3 sh : TEXCOORD9; 
            #endif
            #ifdef LIGHTMAP_ON
               UNITY_LIGHTING_COORDS(10,11)
               UNITY_FOG_COORDS(12)
            #else
               UNITY_FOG_COORDS(10)
               UNITY_SHADOW_COORDS(11)
            #endif
             float4 extraV2F0 : TEXCOORD13;
            UNITY_VERTEX_INPUT_INSTANCE_ID
            UNITY_VERTEX_OUTPUT_STEREO
         };
            struct Surface
            {
               half3 Albedo;
               half Height;
               half3 Normal;
               half Smoothness;
               half3 Emission;
               half Metallic;
               half3 Specular;
               half Occlusion;
               half SpecularPower; 
               half Alpha;
               float outputDepth; 
               half SpecularOcclusion;
               half SubsurfaceMask;
               half Thickness;
               half CoatMask;
               half CoatSmoothness;
               half Anisotropy;
               half IridescenceMask;
               half IridescenceThickness;
               int DiffusionProfileHash;
               float SpecularAAThreshold;
               float SpecularAAScreenSpaceVariance;
               float3 DiffuseGI;
               float3 BackDiffuseGI;
               float3 SpecularGI;
               float ior;
               float3 transmittanceColor;
               float atDistance;
               float transmittanceMask;
               float4 ShadowMask;
               float NormalAlpha;
               float MAOSAlpha;
            };
            struct Blackboard
            {
                float blackboardDummyData;
            };
            struct ShaderData
            {
               float4 clipPos; 
               float3 localSpacePosition;
               float3 localSpaceNormal;
               float3 localSpaceTangent;
               float3 worldSpacePosition;
               float3 worldSpaceNormal;
               float3 worldSpaceTangent;
               float tangentSign;

               float3 worldSpaceViewDir;
               float3 tangentSpaceViewDir;

               float4 texcoord0;
               float4 texcoord1;
               float4 texcoord2;
               float4 texcoord3;

               float2 screenUV;
               float4 screenPos;

               float4 vertexColor;
               bool isFrontFace;

               float4 extraV2F0;
               float4 extraV2F1;
               float4 extraV2F2;
               float4 extraV2F3;
               float4 extraV2F4;
               float4 extraV2F5;
               float4 extraV2F6;
               float4 extraV2F7;

               float3x3 TBNMatrix;
               Blackboard blackboard;
            };

            struct VertexData
            {
               #if SHADER_TARGET > 30
               #endif
               float4 vertex : POSITION;
               float3 normal : NORMAL;
               float4 tangent : TANGENT;
               float4 texcoord0 : TEXCOORD0;
               #if _URP && (_USINGTEXCOORD1 || _PASSMETA || _PASSFORWARD || _PASSGBUFFER)
                  float4 texcoord1 : TEXCOORD1;
               #endif

               #if _URP && (_USINGTEXCOORD2 || _PASSMETA || ((_PASSFORWARD || _PASSGBUFFER) && defined(DYNAMICLIGHTMAP_ON)))
                  float4 texcoord2 : TEXCOORD2;
               #endif

               #if _STANDARD && (_USINGTEXCOORD1 || (_PASSMETA || ((_PASSFORWARD || _PASSGBUFFER || _PASSFORWARDADD) && LIGHTMAP_ON)))
                  float4 texcoord1 : TEXCOORD1;
               #endif
               #if _STANDARD && (_USINGTEXCOORD2 || (_PASSMETA || ((_PASSFORWARD || _PASSGBUFFER) && DYNAMICLIGHTMAP_ON)))
                  float4 texcoord2 : TEXCOORD2;
               #endif
               #if _HDRP
                  float4 texcoord1 : TEXCOORD1;
                  float4 texcoord2 : TEXCOORD2;
               #endif
               #if _PASSMOTIONVECTOR || ((_PASSFORWARD || _PASSUNLIT) && defined(_WRITE_TRANSPARENT_MOTION_VECTOR))
                  float3 previousPositionOS : TEXCOORD4; 
                  #if defined (_ADD_PRECOMPUTED_VELOCITY)
                     float3 precomputedVelocity    : TEXCOORD5; 
                  #endif
               #endif

               UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct TessVertex 
            {
               float4 vertex : INTERNALTESSPOS;
               float3 normal : NORMAL;
               float4 tangent : TANGENT;
               float4 texcoord0 : TEXCOORD0;
               float4 texcoord1 : TEXCOORD1;
               float4 texcoord2 : TEXCOORD2;
                float4 extraV2F0 : TEXCOORD5;
               #if _PASSMOTIONVECTOR || ((_PASSFORWARD || _PASSUNLIT) && defined(_WRITE_TRANSPARENT_MOTION_VECTOR))
                  float3 previousPositionOS : TEXCOORD13; 
                  #if defined (_ADD_PRECOMPUTED_VELOCITY)
                     float3 precomputedVelocity : TEXCOORD14;
                  #endif
               #endif

               UNITY_VERTEX_INPUT_INSTANCE_ID
               UNITY_VERTEX_OUTPUT_STEREO
            };

            struct ExtraV2F
            {
               float4 extraV2F0;
               float4 extraV2F1;
               float4 extraV2F2;
               float4 extraV2F3;
               float4 extraV2F4;
               float4 extraV2F5;
               float4 extraV2F6;
               float4 extraV2F7;
               Blackboard blackboard;
               float4 time;
            };
            float3 WorldToTangentSpace(ShaderData d, float3 normal)
            {
               return mul(d.TBNMatrix, normal);
            }

            float3 TangentToWorldSpace(ShaderData d, float3 normal)
            {
               return mul(normal, d.TBNMatrix);
            }
            #if _STANDARD
               float3 TransformWorldToObject(float3 p) { return mul(unity_WorldToObject, float4(p, 1)); };
               float3 TransformObjectToWorld(float3 p) { return mul(unity_ObjectToWorld, float4(p, 1)); };
               float4 TransformWorldToObject(float4 p) { return mul(unity_WorldToObject, p); };
               float4 TransformObjectToWorld(float4 p) { return mul(unity_ObjectToWorld, p); };
               float4x4 GetWorldToObjectMatrix() { return unity_WorldToObject; }
               float4x4 GetObjectToWorldMatrix() { return unity_ObjectToWorld; }
               #if (defined(SHADER_API_D3D11) || defined(SHADER_API_XBOXONE) || defined(UNITY_COMPILER_HLSLCC) || defined(SHADER_API_PSSL) || (SHADER_TARGET_SURFACE_ANALYSIS && !SHADER_TARGET_SURFACE_ANALYSIS_MOJOSHADER))
                 #define UNITY_SAMPLE_TEX2D_LOD(tex,coord, lod) tex.SampleLevel (sampler##tex,coord, lod)
                 #define UNITY_SAMPLE_TEX2D_SAMPLER_LOD(tex,samplertex,coord, lod) tex.SampleLevel (sampler##samplertex,coord, lod)
              #else
                 #define UNITY_SAMPLE_TEX2D_LOD(tex,coord,lod) tex2D (tex,coord,0,lod)
                 #define UNITY_SAMPLE_TEX2D_SAMPLER_LOD(tex,samplertex,coord,lod) tex2D (tex,coord,0,lod)
              #endif

               #undef UNITY_MATRIX_I_M

               #define UNITY_MATRIX_I_M   unity_WorldToObject
            #endif

            float3 GetCameraWorldPosition()
            {
               #if _HDRP
                  return GetCameraRelativePositionWS(_WorldSpaceCameraPos);
               #else
                  return _WorldSpaceCameraPos;
               #endif
            }

            #if _GRABPASSUSED
               #if _STANDARD
                  TEXTURE2D(%GRABTEXTURE%);
                  SAMPLER(sampler_%GRABTEXTURE%);
               #endif

               half3 GetSceneColor(float2 uv)
               {
                  #if _STANDARD
                     return SAMPLE_TEXTURE2D(%GRABTEXTURE%, sampler_%GRABTEXTURE%, uv).rgb;
                  #else
                     return SHADERGRAPH_SAMPLE_SCENE_COLOR(uv);
                  #endif
               }
            #endif
            #if _STANDARD
               UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);
               float GetSceneDepth(float2 uv) { return SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, uv); }
               float GetLinear01Depth(float2 uv) { return Linear01Depth(GetSceneDepth(uv)); }
               float GetLinearEyeDepth(float2 uv) { return LinearEyeDepth(GetSceneDepth(uv)); } 
            #else
               float GetSceneDepth(float2 uv) { return SHADERGRAPH_SAMPLE_SCENE_DEPTH(uv); }
               float GetLinear01Depth(float2 uv) { return Linear01Depth(GetSceneDepth(uv), _ZBufferParams); }
               float GetLinearEyeDepth(float2 uv) { return LinearEyeDepth(GetSceneDepth(uv), _ZBufferParams); } 
            #endif

            float3 GetWorldPositionFromDepthBuffer(float2 uv, float3 worldSpaceViewDir)
            {
               float eye = GetLinearEyeDepth(uv);
               float3 camView = mul((float3x3)UNITY_MATRIX_M, transpose(mul(UNITY_MATRIX_I_M, UNITY_MATRIX_I_V)) [2].xyz);

               float dt = dot(worldSpaceViewDir, camView);
               float3 div = worldSpaceViewDir/dt;
               float3 wpos = (eye * div) + GetCameraWorldPosition();
               return wpos;
            }

            #if _HDRP
            float3 ObjectToWorldSpacePosition(float3 pos)
            {
               return GetAbsolutePositionWS(TransformObjectToWorld(pos));
            }
            #else
            float3 ObjectToWorldSpacePosition(float3 pos)
            {
               return TransformObjectToWorld(pos);
            }
            #endif

            #if _STANDARD
               UNITY_DECLARE_SCREENSPACE_TEXTURE(_CameraDepthNormalsTexture);
               float3 GetSceneNormal(float2 uv, float3 worldSpaceViewDir)
               {
                  float4 depthNorms = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_CameraDepthNormalsTexture, uv);
                  float3 norms = DecodeViewNormalStereo(depthNorms);
                  norms = mul((float3x3)UNITY_MATRIX_V, norms) * 0.5 + 0.5;
                  return norms;
               }
            #elif _HDRP && !_DECALSHADER
               float3 GetSceneNormal(float2 uv, float3 worldSpaceViewDir)
               {
                  NormalData nd;
                  DecodeFromNormalBuffer(_ScreenSize.xy * uv, nd);
                  return nd.normalWS;
               }
            #elif _URP
               #if (SHADER_LIBRARY_VERSION_MAJOR >= 10)
                  #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareNormalsTexture.hlsl"
               #endif

               float3 GetSceneNormal(float2 uv, float3 worldSpaceViewDir)
               {
                  #if (SHADER_LIBRARY_VERSION_MAJOR >= 10)
                     return SampleSceneNormals(uv);
                  #else
                     float3 wpos = GetWorldPositionFromDepthBuffer(uv, worldSpaceViewDir);
                     return normalize(-cross(ddx(wpos), ddy(wpos))) * 0.5 + 0.5;
                  #endif

                }
             #endif

             #if _HDRP

               half3 UnpackNormalmapRGorAG(half4 packednormal)
               {
                  packednormal.x *= packednormal.w;

                  half3 normal;
                  normal.xy = packednormal.xy * 2 - 1;
                  normal.z = sqrt(1 - saturate(dot(normal.xy, normal.xy)));
                  return normal;
               }
               half3 UnpackNormal(half4 packednormal)
               {
                  #if defined(UNITY_NO_DXT5nm)
                     return packednormal.xyz * 2 - 1;
                  #else
                     return UnpackNormalmapRGorAG(packednormal);
                  #endif
               }
            #endif
            #if _HDRP || _URP

               half3 UnpackScaleNormal(half4 packednormal, half scale)
               {
                 #ifndef UNITY_NO_DXT5nm
                   packednormal.x *= packednormal.w;
                 #endif
                   half3 normal;
                   normal.xy = (packednormal.xy * 2 - 1) * scale;
                   normal.z = sqrt(1 - saturate(dot(normal.xy, normal.xy)));
                   return normal;
               }	

             #endif
            void GetSun(out float3 lightDir, out float3 color)
            {
               lightDir = float3(0.5, 0.5, 0);
               color = 1;
               #if _HDRP
                  if (_DirectionalLightCount > 0)
                  {
                     DirectionalLightData light = _DirectionalLightDatas[0];
                     lightDir = -light.forward.xyz;
                     color = light.color;
                  }
               #elif _STANDARD
			         lightDir = normalize(_WorldSpaceLightPos0.xyz);
                  color = _LightColor0.rgb;
               #elif _URP
	               Light light = GetMainLight();
	               lightDir = light.direction;
	               color = light.color;
               #endif
            }
    half _ShadingMode;
    half _LightFunction;
    half _UseAlphaOnlyFromBaseMap;
    half _EnableToonShading;
    half _ShadingFunction;
    int _NumberOfCells;
	float _CellTransitionSmoothness;
	half _SumLightsBeforePosterization;
	half _ShadingUseLightColors;
    half _GradientMode;
    half _GradientBlending;
    float _GradientBlendFactor;

    float4 _GradientTex_TexelSize;
    half _ShadingAffectedByNormalMap;
    half _EnableShadows;
    float4 _CoreShadowColor;
    float _TerminatorPosition;
    float _TerminatorWidth;
    float _TerminatorSmoothness;
    float4 _FormShadowColor;

    half _EnableCastShadows;
    float _CastShadowsStrength;
    float _CastShadowsSmoothness;
    half _CastShadowColorMode;
    float4 _CastShadowColor;
    half _EnableSpecular;
    half _SpecularBlending;
    half4 _SpecularColor;
    float _SpecularSize;
    float _SpecularSmoothness;
    float _SpecularOpacity;
    half _SpecularAffectedByNormalMap;
    half _SpecularUseLightColors;
    half _EnableRim;
    half _RimBlending;
    float4 _RimColor;
    float _RimSize;
    float _RimSmoothness;
    float _RimOpacity;
    half _RimAffectedArea;
    half _RimAffectedByNormalMap;
     half _EnableStyling;

    half _EnableStylingDistanceFade;
    float _StylingDFStartingDistance;
    float _StylingDFFalloff;
    half _StylingAdjustDistanceFadeValue;
    float _StylingDistanceFadeValue;
    half _HatchingAffectedByNormalMap;
    half _EnableAntiAliasing;
    float4 _NoiseTex2_TexelSize;
    half _EnableShadingStyling;
    half _StylingShadingSyncWithOtherStyling;
    half _ShadingStyle;    
    half _SyncWithLightPartitioning;
    half _NumberOfCellsHatching;
    half _StylingTerminatorPosition;    
    float _StylingOvermodelingFactor;
    half _StylingShadingBlending;
    half _StylingShadingIsInverted;
    half _DrawSpace;
    half _SSCameraDistanceScaled;
    half _AnchorSSToObjectsOrigin;
    half _CoordinateSystem;
    half _PolarCenterMode;
    float4 _PolarCenter;
    float _StylingShadingDensity;
    float _StylingShadingInitialDirection;
    float _StylingShadingRotationBetweenCells;
    float _StylingShadingHalftonesOffset;
    float4 _StylingColor;
    float _StylingShadingOpacity;
    float _StylingShadingOpacityFalloff;
    float _StylingShadingThicknessControl;
    float _StylingShadingThickness;
    float _StylingShadingThicknessFalloff;
    float _StylingShadingHardness;
    float _StylingShadingHalftonesRoundness;
    float _StylingShadingHalftonesRoundnessFalloff;
    half _EnableShadingRandomizer;
    float _ShadingNoise1Size;
    float _ShadingNoise1Seed;
    float _ShadingNoise2Seed;
    float _NoiseIntensity;
    half _SpacingRandomMode;
    float _SpacingRandomIntensity;
    half _OpacityRandomMode;
    float _OpacityRandomIntensity;
    half _LengthRandomMode;
    float _LengthRandomIntensity;
    half _HardnessRandomMode;
    float _HardnessRandomIntensity;
    half _ThicknessRandomMode;
    float _ThicknesshRandomIntensity;
    half _EnableCastShadowsStyling;
    half _CastShadowsNumberOfCellsHatching;
    float _StylingCastShadowsSmoothness;
    half _StylingCastShadowsSyncWithOtherStyling;
    half _CastShadowsStyle;
    half _StylingCastShadowsBlending;
    half _StylingCastShadowsIsInverted;
    half _CastShadowsDrawSpace;
    half _CastShadowsSSCameraDistanceScaled;
    half _CastShadowsAnchorSSToObjectsOrigin;
    half _CastShadowsCoordinateSystem;
    half _CastShadowsPolarCenterMode;
    float4 _CastShadowsPolarCenter;
    float _StylingCastShadowsDensity;
    float _StylingCastShadowsInitialDirection;
    float _StylingCastShadowsRotationBetweenCells;
    float _StylingCastShadowsHalftonesOffset;
    float4 _StylingCastShadowsColor;
    float _StylingCastShadowsOpacity;
    float _StylingCastShadowsOpacityFalloff;
    half _StylingCastShadowsThicknessControl;
    float _StylingCastShadowsThickness;
    float _StylingCastShadowsThicknessFalloff;
    float _StylingCastShadowsHardness;
    float _StylingCastShadowsHalftonesRoundness;
    float _StylingCastShadowsHalftonesRoundnessFalloff;
    half _EnableCastShadowsRandomizer;
    float _CastShadowsNoise1Size;
    float _CastShadowsNoise1Seed;
    float _CastShadowsNoise2Seed;
    float _CastShadowsNoiseIntensity;
    half _CastShadowsSpacingRandomMode;
    float _CastShadowsSpacingRandomIntensity;
    half _CastShadowsOpacityRandomMode;
    float _CastShadowsOpacityRandomIntensity;
    half _CastShadowsHardnessRandomMode;
    float _CastShadowsHardnessRandomIntensity;
    half _CastShadowsLengthRandomMode;
    float _CastShadowsLengthRandomIntensity;
    half _CastShadowsThicknessRandomMode;
    float _CastShadowsThicknesshRandomIntensity;
    half _EnableSpecularStyling;
    half _SyncWithSpecular;
    float _StylingSpecularSize;
    float _StylingSpecularSmoothness;
    half _StylingSpecularCutOutShading;
    half _StylingSpecularUseLightColors;
    half _StylingSpecularSyncWithOtherStyling;
    half _SpecularStyle;
    half _StylingSpecularBlending;
    half _StylingSpecularIsInverted;
    half _SpecularDrawSpace;
    half _SpecularSSCameraDistanceScaled;
    half _SpecularAnchorSSToObjectsOrigin;
    half _SpecularCoordinateSystem;
    half _SpecularPolarCenterMode;
    float4 _SpecularPolarCenter;
    float _StylingSpecularDensity;    
    float _StylingSpecularRotation;
    float _StylingSpecularHalftonesOffset;
    float4 _StylingSpecularColor;
    float _StylingSpecularOpacity;
    float _StylingSpecularOpacityFalloff;
    float _StylingSpecularThicknessControl;
    float _StylingSpecularThickness;
    float _StylingSpecularThicknessFalloff;
    float _StylingSpecularHardness;
    float _StylingSpecularHalftonesRoundness;
    float _StylingSpecularHalftonesRoundnessFalloff;
    float _EnableSpecularRandomizer;    
    float _SpecularNoise1Size;
    float _SpecularNoise1Seed;
    float _SpecularNoise2Seed;
    float _SpecularNoiseIntensity;
    half _SpecularSpacingRandomMode;
    float _SpecularSpacingRandomIntensity;
    half _SpecularOpacityRandomMode;
    float _SpecularOpacityRandomIntensity;
    half _SpecularLengthRandomMode;
    float _SpecularLengthRandomIntensity;
    half _SpecularHardnessRandomMode;
    float _SpecularHardnessRandomIntensity;
    half _SpecularThicknessRandomMode;
    float _SpecularThicknesshRandomIntensity;
    half _EnableRimStyling;
    half _StylingRimSyncWithOtherStyling;
    half _SyncWithRim;
    float _StylingRimSize;
    float _StylingRimSmoothness;
    half _StylingRimAffectedArea;
    half _RimStyle;
    half _StylingRimBlending;
    half _StylingRimIsInverted;
    half _RimDrawSpace;
    half _RimSSCameraDistanceScaled;
    half _RimAnchorSSToObjectsOrigin;
    half _RimCoordinateSystem;
    half _RimPolarCenterMode;
    float4 _RimPolarCenter;  
    float _StylingRimDensity;    
    float _StylingRimRotation;
    float _StylingRimHalftonesOffset;
    float4 _StylingRimColor;
    float _StylingRimOpacity;
    float _StylingRimOpacityFalloff;
    float _StylingRimThicknessControl;
    float _StylingRimThickness;
    float _StylingRimThicknessFalloff;
    float _StylingRimHardness;
    float _StylingRimHalftonesRoundness;
    float _StylingRimHalftonesRoundnessFalloff;
    float _EnableRimRandomizer;        
    float _RimNoise1Size;
    float _RimNoise1Seed;
    float _RimNoise2Seed;
    float _RimNoiseIntensity;
    half _RimSpacingRandomMode;
    float _RimSpacingRandomIntensity;
    half _RimOpacityRandomMode; 
    float _RimOpacityRandomIntensity;
    half _RimLengthRandomMode;
    float _RimLengthRandomIntensity;
    half _RimHardnessRandomMode;
    float _RimHardnessRandomIntensity;
    half _RimThicknessRandomMode;
    float _RimThicknesshRandomIntensity;
    half4 _OutlineColor;
    half _OutlineWidth;
    half _EnableOutline;
    half4 _Color;
    half _Cutoff; 
    half _Mode;
    float _Cull;
    half _Metallic;
    half3 _EmissionColor;
    half _UVSec;

    half _BumpScale; 
    half _DetailNormalMapScale;
    float _Glossiness;
    float _GlossMapScale;
    half _OcclusionStrength;
    half _Parallax;

    float4 _MainTex_ST;
    float4 _DetailAlbedoMap_ST;
    sampler2D _NoiseMap1;
    sampler2D _NoiseMap2;

    sampler2D _HatchingMap;
    sampler2D _GradientTex;
    void Ext_ModifyVertex0 (inout VertexData v, inout ExtraV2F d)
    {
        float4x4 UnityObjectToClipPosMatrix = mul(UNITY_MATRIX_VP, GetObjectToWorldMatrix());
        float4 pos = mul(UnityObjectToClipPosMatrix, v.vertex);
        float4 objectOriginClipPos = mul(UnityObjectToClipPosMatrix, float4(0, 0, 0, 1));
        objectOriginClipPos.xy = clamp(objectOriginClipPos.xy, -1.0*objectOriginClipPos.w, 1.0*objectOriginClipPos.w);
        float4 originScreenPos = ComputeScreenPos(objectOriginClipPos);
        float2 originScreenUV = originScreenPos.xy/originScreenPos.w;
        float4 screenPos = ComputeScreenPos(pos);
        float2 uv = ((screenPos.xy) / screenPos.w);
        float4 ssss = ComputeScreenPos(pos);
       d.extraV2F0 = float4(originScreenUV,originScreenUV);
    }
    sampler2D _MainTex;
    sampler2D _DetailAlbedoMap;
    sampler2D _DetailMask;
    sampler2D _BumpMap;
    sampler2D _DetailNormalMap;
    sampler2D _MetallicGlossMap;
    sampler2D _OcclusionMap;
    sampler2D _ParallaxMap;
    sampler2D _EmissionMap;
	void Ext_SurfaceFunction1 (inout Surface o, ShaderData d)
	{
        float4 texcoords;
        texcoords.xy = d.texcoord0.xy * _MainTex_ST.xy + _MainTex_ST.zw; 
        texcoords.zw = (_UVSec == 0) ? d.texcoord0.xy * _DetailAlbedoMap_ST.xy + _DetailAlbedoMap_ST.zw : d.texcoord1.xy * _DetailAlbedoMap_ST.xy + _DetailAlbedoMap_ST.zw;
    #if _SHADING_COLOR
        half4 diffuseAlpha = tex2D(_MainTex, texcoords.xy);
        o.Albedo = diffuseAlpha.rgb * _Color.rgb;
        o.Normal = UnpackScaleNormal(tex2D(_BumpMap, texcoords.xy), _BumpScale);

		half alpha = diffuseAlpha.a;
        o.Alpha = alpha * _Color.a;
        if (_Mode) 
        {
            clip (alpha - _Cutoff);
        }

        #ifdef _EMISSION
            half3 emission = tex2D(_EmissionMap, texcoords.xy).rgb * _EmissionColor.rgb; 
            o.Emission = emission;
        #endif
    #else
        #ifdef _PARALLAXMAP
            half h = tex2D (_ParallaxMap, texcoords.xy).g;
            float2 offset = ParallaxOffset1Step (h, _Parallax, d.tangentSpaceViewDir);
            texcoords = float4(texcoords.xy + offset, texcoords.zw + offset);
            o.Normal = float3(0, 0, 1);
        #endif
        half3 albedo = tex2D(_MainTex, texcoords.xy).rgb * _Color.rgb; 
        #ifdef _DETAIL_MULX2
                half3 detailAlbedo = tex2D (_DetailAlbedoMap, texcoords.zw).rgb;
                albedo *= LerpWhiteTo (detailAlbedo * unity_ColorSpaceDouble.rgb, tex2D(_DetailMask, texcoords.xy).a);
        #endif
        o.Albedo = albedo;
        half2 metallicSmoothness = half2(0,0);
        #ifdef _METALLICGLOSSMAP
            half4 metallicGlossMap = tex2D(_MetallicGlossMap, texcoords.xy);
            metallicSmoothness.r = metallicGlossMap.r;
            #ifdef _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
                metallicSmoothness.g = tex2D(_MainTex, texcoords.xy).a;
            #else
                metallicSmoothness.g = metallicGlossMap.a;
            #endif
            metallicSmoothness.g *= _GlossMapScale;
        #else
            metallicSmoothness.r = _Metallic;
            #ifdef _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
                metallicSmoothness.g  = tex2D(_MainTex, texcoords.xy).a * _GlossMapScale;
            #else
                metallicSmoothness.g = _Glossiness;
            #endif
        #endif
        o.Metallic = metallicSmoothness.r;
        o.Smoothness = metallicSmoothness.g;
        o.Occlusion = lerp(1, tex2D(_OcclusionMap, texcoords.xy).g, _OcclusionStrength);
        half alpha = 0;
        #if defined(_SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A)
            alpha = _Color.a;
        #else
            alpha =  tex2D(_MainTex, texcoords.xy).a * _Color.a;
        #endif
        if (_Mode) 
        {
            clip (alpha - _Cutoff);
        }
        o.Alpha = alpha;
        #ifdef _NORMALMAP
            half3 normal = UnpackScaleNormal(tex2D(_BumpMap, texcoords.xy), _BumpScale);
            #ifdef _DETAIL_MULX2
                half3 detailNormal = UnpackScaleNormal(tex2D(_DetailNormalMap, texcoords.zw), _DetailNormalMapScale);
	            detailNormal = lerp(float3(0, 0, 1), detailNormal, tex2D(_DetailMask, texcoords.xy).a);
	            normal = BlendNormals(normal, detailNormal);
            #endif
            o.Normal = normal;
        #endif
        #ifdef _EMISSION
            o.Emission += tex2D(_EmissionMap, texcoords.xy).rgb * _EmissionColor.rgb; 
        #endif
    #endif
	}
            void ChainSurfaceFunction(inout Surface l, inout ShaderData d)
            {
                  Ext_SurfaceFunction1(l, d);
            }

#if !_DECALSHADER

            void ChainModifyVertex(inout VertexData v, inout VertexToPixel v2p, float4 time)
            {
                 ExtraV2F d;
                 ZERO_INITIALIZE(ExtraV2F, d);
                 ZERO_INITIALIZE(Blackboard, d.blackboard);
                 d.time = time;

                   Ext_ModifyVertex0(v, d);
                  v2p.extraV2F0 = d.extraV2F0;
            }

            void ChainModifyTessellatedVertex(inout VertexData v, inout VertexToPixel v2p)
            {
               ExtraV2F d;
               ZERO_INITIALIZE(ExtraV2F, d);
               ZERO_INITIALIZE(Blackboard, d.blackboard);
                d.extraV2F0 = v2p.extraV2F0;
                v2p.extraV2F0 = d.extraV2F0;
            }

            void ChainFinalColorForward(inout Surface l, inout ShaderData d, inout half4 color)
            {
            }

            void ChainFinalGBufferStandard(inout Surface s, inout ShaderData d, inout half4 GBuffer0, inout half4 GBuffer1, inout half4 GBuffer2, inout half4 outEmission, inout half4 outShadowMask)
            {
            }
#endif
#if _DECALSHADER

        ShaderData CreateShaderData(SurfaceDescriptionInputs IN)
        {
            ShaderData d = (ShaderData)0;
            d.TBNMatrix = float3x3(IN.WorldSpaceTangent, IN.WorldSpaceBiTangent, IN.WorldSpaceNormal);
            d.worldSpaceNormal = IN.WorldSpaceNormal;
            d.worldSpaceTangent = IN.WorldSpaceTangent;

            d.worldSpacePosition = IN.WorldSpacePosition;
            d.texcoord0 = IN.uv0.xyxy;
            d.screenPos = IN.ScreenPosition;

            d.worldSpaceViewDir = normalize(_WorldSpaceCameraPos - d.worldSpacePosition);

            d.tangentSpaceViewDir = mul(d.TBNMatrix, d.worldSpaceViewDir);
            #if _HDRP
            #else
            #endif
            return d;
        }
#else

         ShaderData CreateShaderData(VertexToPixel i
                  #if NEED_FACING
                     , bool facing
                  #endif
         )
         {
            ShaderData d = (ShaderData)0;
            d.clipPos = i.pos;
            d.worldSpacePosition = i.worldPos;

            d.worldSpaceNormal = normalize(i.worldNormal);
            d.worldSpaceTangent.xyz = normalize(i.worldTangent.xyz);

            d.tangentSign = i.worldTangent.w * unity_WorldTransformParams.w;
            float3 bitangent = cross(d.worldSpaceTangent.xyz, d.worldSpaceNormal) * d.tangentSign;
            d.TBNMatrix = float3x3(d.worldSpaceTangent, -bitangent, d.worldSpaceNormal);
            d.worldSpaceViewDir = normalize(_WorldSpaceCameraPos - i.worldPos);

            d.tangentSpaceViewDir = mul(d.TBNMatrix, d.worldSpaceViewDir);
             d.texcoord0 = i.texcoord0;
             d.texcoord1 = i.texcoord1;
            #if _HDRP
            #else
            #endif
             d.extraV2F0 = i.extraV2F0;
            return d;
         }

#endif
         VertexToPixel Vert (VertexData v)
         {
           UNITY_SETUP_INSTANCE_ID(v);
           VertexToPixel o;
           UNITY_INITIALIZE_OUTPUT(VertexToPixel,o);
           UNITY_TRANSFER_INSTANCE_ID(v,o);
           UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

#if !_TESSELLATION_ON
           ChainModifyVertex(v, o, _Time);
#endif

           o.pos = UnityObjectToClipPos(v.vertex);
            o.texcoord0 = v.texcoord0;
            o.texcoord1 = v.texcoord1;
           o.worldPos = mul(GetObjectToWorldMatrix(), v.vertex).xyz;
           o.worldNormal = UnityObjectToWorldNormal(v.normal);
           o.worldTangent = float4(UnityObjectToWorldDir(v.tangent.xyz), v.tangent.w);
           #ifdef DYNAMICLIGHTMAP_ON
           o.lmap.zw = v.texcoord2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
           #endif
           #ifdef LIGHTMAP_ON
           o.lmap.xy = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
           #endif
           #ifndef LIGHTMAP_ON
             #if UNITY_SHOULD_SAMPLE_SH && !UNITY_SAMPLE_FULL_SH_PER_PIXEL
               o.sh = 0;
               #ifdef VERTEXLIGHT_ON
                 o.sh += Shade4PointLights (
                   unity_4LightPosX0, unity_4LightPosY0, unity_4LightPosZ0,
                   unity_LightColor[0].rgb, unity_LightColor[1].rgb, unity_LightColor[2].rgb, unity_LightColor[3].rgb,
                   unity_4LightAtten0, o.worldPos, o.worldNormal);
               #endif
               o.sh = ShadeSHPerVertex (o.worldNormal, o.sh);
             #endif
           #endif 

           UNITY_TRANSFER_LIGHTING(o,v.texcoord1.xy); 
           #ifdef FOG_COMBINED_WITH_TSPACE
             UNITY_TRANSFER_FOG_COMBINED_WITH_TSPACE(o,o.pos); 
           #elif defined (FOG_COMBINED_WITH_WORLD_POS)
             UNITY_TRANSFER_FOG_COMBINED_WITH_WORLD_POS(o,o.pos); 
           #else
             UNITY_TRANSFER_FOG(o,o.pos); 
           #endif

           return o;
         }
         #include "Packages/com.shadercrew.the-toon-shader.core/Scripts/Shaders/xxSharedTTSDependecies/TheToonShaderLightingFunctions.hlsl"

         #include "Packages/com.shadercrew.the-toon-shader.3d/Scripts/Shaders/xxSharedTTSDependecies/ModifiedLightingFunctions/BiRP/UnityPBSLighting_Toon.cginc"
         #include "Packages/com.shadercrew.the-toon-shader.core/Scripts/Shaders/xxSharedTTSDependecies/TheToonShaderFunctions.hlsl"
         fixed4 Frag (VertexToPixel IN
         #ifdef _DEPTHOFFSET_ON
              , out float outputDepth : SV_Depth
         #endif
         #if NEED_FACING
            , bool facing : SV_IsFrontFace
         #endif
         ) : SV_Target
         {
           UNITY_SETUP_INSTANCE_ID(IN);
           #ifdef FOG_COMBINED_WITH_TSPACE
             UNITY_EXTRACT_FOG_FROM_TSPACE(IN);
           #elif defined (FOG_COMBINED_WITH_WORLD_POS)
             UNITY_EXTRACT_FOG_FROM_WORLD_POS(IN);
           #else
             UNITY_EXTRACT_FOG(IN);
           #endif

           ShaderData d = CreateShaderData(IN
              #if NEED_FACING
                 , facing
              #endif
           );
           Surface l = (Surface)0;
           #ifdef _DEPTHOFFSET_ON
              l.outputDepth = outputDepth;
           #endif
           l.Albedo = half3(0.5, 0.5, 0.5);
           l.Normal = float3(0,0,1);
           l.Occlusion = 1;
           l.Alpha = 1;

           ChainSurfaceFunction(l, d);
           #ifdef _DEPTHOFFSET_ON
              outputDepth = l.outputDepth;
           #endif
           #ifndef USING_DIRECTIONAL_LIGHT
             fixed3 lightDir = normalize(UnityWorldSpaceLightDir(d.worldSpacePosition));
           #else
             fixed3 lightDir = _WorldSpaceLightPos0.xyz;
           #endif
           float3 worldViewDir = normalize(UnityWorldSpaceViewDir(d.worldSpacePosition));
           UNITY_LIGHT_ATTENUATION(atten, IN, d.worldSpacePosition)

           #if _USESPECULAR || _USESPECULARWORKFLOW || _SPECULARFROMMETALLIC
              #ifdef UNITY_COMPILER_HLSL
                 SurfaceOutputStandardSpecular o = (SurfaceOutputStandardSpecular)0;
              #else
                 SurfaceOutputStandardSpecular o;
              #endif
              o.Specular = l.Specular;
              o.Occlusion = l.Occlusion;
              o.Smoothness = l.Smoothness;
           #elif _BDRFLAMBERT || _BDRF3 || _SIMPLELIT
              #ifdef UNITY_COMPILER_HLSL
                 SurfaceOutput o = (SurfaceOutput)0;
              #else
                 SurfaceOutput o;
              #endif

              o.Specular = l.Specular;
              o.Gloss = l.Smoothness;
              _SpecColor.rgb = l.Specular; 
           #else
              #ifdef UNITY_COMPILER_HLSL
                 SurfaceOutputStandard o = (SurfaceOutputStandard)0;
              #else
                 SurfaceOutputStandard o;
              #endif
              o.Smoothness = l.Smoothness;
              o.Metallic = l.Metallic;
              o.Occlusion = l.Occlusion;
           #endif

           o.Albedo = l.Albedo;
           o.Emission = l.Emission;
           o.Alpha = l.Alpha;
           #if _WORLDSPACENORMAL
              o.Normal = l.Normal;
           #else
              o.Normal = normalize(TangentToWorldSpace(d, l.Normal));
           #endif

            fixed4 c = 0;
            UnityGI gi;
            UNITY_INITIALIZE_OUTPUT(UnityGI, gi);
            gi.indirect.diffuse = 0;
            gi.indirect.specular = 0;
            gi.light.color = _LightColor0.rgb;
            gi.light.dir = lightDir;
            UnityGIInput giInput;
            UNITY_INITIALIZE_OUTPUT(UnityGIInput, giInput);
            giInput.light = gi.light;
            giInput.worldPos = d.worldSpacePosition;
            giInput.worldViewDir = worldViewDir;
            giInput.atten = atten;
            #if defined(LIGHTMAP_ON) || defined(DYNAMICLIGHTMAP_ON)
               giInput.lightmapUV = IN.lmap;
            #else
               giInput.lightmapUV = 0.0;
            #endif
            #if UNITY_SHOULD_SAMPLE_SH && !UNITY_SAMPLE_FULL_SH_PER_PIXEL
               giInput.ambient = IN.sh;
            #else
               giInput.ambient.rgb = 0.0;
            #endif
            giInput.probeHDR[0] = unity_SpecCube0_HDR;
            giInput.probeHDR[1] = unity_SpecCube1_HDR;
            #if defined(UNITY_SPECCUBE_BLENDING) || defined(UNITY_SPECCUBE_BOX_PROJECTION)
               giInput.boxMin[0] = unity_SpecCube0_BoxMin; 
            #endif
            #ifdef UNITY_SPECCUBE_BOX_PROJECTION
               giInput.boxMax[0] = unity_SpecCube0_BoxMax;
               giInput.probePosition[0] = unity_SpecCube0_ProbePosition;
               giInput.boxMax[1] = unity_SpecCube1_BoxMax;
               giInput.boxMin[1] = unity_SpecCube1_BoxMin;
               giInput.probePosition[1] = unity_SpecCube1_ProbePosition;
            #endif
            #if defined(_OVERRIDE_SHADOWMASK)
               float4 mulColor = saturate(dot(l.ShadowMask, unity_OcclusionMaskSelector));
               gi.light.color *= mulColor;
               giInput.light.color *= mulColor;
            #endif
            #if defined(_OVERRIDE_BAKEDGI)
                gi.indirect.diffuse = l.DiffuseGI;
                gi.indirect.specular = l.SpecularGI;
            #endif
            c = half4(l.Albedo, l.Alpha);
           AddTheToonShader(c,o, gi,giInput,d);
           ChainFinalColorForward(l, d, c);

           #if !DISABLEFOG
            UNITY_APPLY_FOG(_unity_fogCoord, c); 
           #endif

           return c;
         }

         ENDCG

      }
          Cull [_Cull]
    Cull [_Cull]
	   Pass
      {
		   Name "FORWARD"
		   Tags { "LightMode" = "ForwardAdd" }
		   ZWrite Off Blend One One
             Cull [_Cull]
    Cull [_Cull]
         CGPROGRAM

            #pragma vertex Vert
   #pragma fragment Frag
        #pragma target 3.0
         #pragma multi_compile_instancing
         #pragma multi_compile_fog
         #pragma skip_variants INSTANCING_ON
         #pragma multi_compile_fwdadd_fullshadows
         #include "HLSLSupport.cginc"
         #define UNITY_INSTANCED_LOD_FADE
         #define UNITY_INSTANCED_SH
         #define UNITY_INSTANCED_LIGHTMAPSTS
         #include "UnityShaderVariables.cginc"
         #include "UnityShaderUtilities.cginc"
         #include "UnityCG.cginc"
         #include "Lighting.cginc"
         #include "UnityPBSLighting.cginc"
         #include "AutoLight.cginc"
         #define _PASSFORWARD 1
         #define _PASSFORWARDADD 1
     #pragma shader_feature_local_fragment  _SHADING_COLOR _SHADING_BLINNPHONG _SHADING_PBR    
        #pragma shader_feature_local _NORMALMAP
        #pragma shader_feature_local _METALLICGLOSSMAP
        #pragma shader_feature_local _PARALLAXMAP
        #pragma shader_feature_local _DETAIL_MULX2
        #pragma shader_feature_local _EMISSION       
        #pragma shader_feature_local _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
   #define _STANDARD 1
#if defined(SHADER_API_GAMECORE)

	#define ZERO_INITIALIZE(type, name) name = (type)0;
	#define ZERO_INITIALIZE_ARRAY(type, name, arraySize) { for (int arrayIndex = 0; arrayIndex < arraySize; arrayIndex++) { name[arrayIndex] = (type)0; } }
	#define CALCULATE_TEXTURE2D_LOD(textureName, samplerName, coord2) textureName.CalculateLevelOfDetail(samplerName, coord2)
	#define TEXTURE2D(textureName)                Texture2D textureName
	#define TEXTURE2D_ARRAY(textureName)          Texture2DArray textureName
	#define TEXTURECUBE(textureName)              TextureCube textureName
	#define TEXTURECUBE_ARRAY(textureName)        TextureCubeArray textureName
	#define TEXTURE3D(textureName)                Texture3D textureName

	#define TEXTURE2D_FLOAT(textureName)          TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_FLOAT(textureName)    TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_FLOAT(textureName)        TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_FLOAT(textureName)  TEXTURECUBE_ARRAY(textureName)
	#define TEXTURE3D_FLOAT(textureName)          TEXTURE3D(textureName)

	#define TEXTURE2D_HALF(textureName)           TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_HALF(textureName)     TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_HALF(textureName)         TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_HALF(textureName)   TEXTURECUBE_ARRAY(textureName)
	#define TEXTURE3D_HALF(textureName)           TEXTURE3D(textureName)

	#define TEXTURE2D_SHADOW(textureName)         TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_SHADOW(textureName)   TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_SHADOW(textureName)       TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_SHADOW(textureName) TEXTURECUBE_ARRAY(textureName)

	#define RW_TEXTURE2D(type, textureName)       RWTexture2D<type> textureName
	#define RW_TEXTURE2D_ARRAY(type, textureName) RWTexture2DArray<type> textureName
	#define RW_TEXTURE3D(type, textureName)       RWTexture3D<type> textureName

	#define SAMPLER(samplerName)                  SamplerState samplerName
	#define SAMPLER_CMP(samplerName)              SamplerComparisonState samplerName
	#define ASSIGN_SAMPLER(samplerName, samplerValue) samplerName = samplerValue

	#define TEXTURE2D_PARAM(textureName, samplerName)                 TEXTURE2D(textureName),         SAMPLER(samplerName)
	#define TEXTURE2D_ARRAY_PARAM(textureName, samplerName)           TEXTURE2D_ARRAY(textureName),   SAMPLER(samplerName)
	#define TEXTURECUBE_PARAM(textureName, samplerName)               TEXTURECUBE(textureName),       SAMPLER(samplerName)
	#define TEXTURECUBE_ARRAY_PARAM(textureName, samplerName)         TEXTURECUBE_ARRAY(textureName), SAMPLER(samplerName)
	#define TEXTURE3D_PARAM(textureName, samplerName)                 TEXTURE3D(textureName),         SAMPLER(samplerName)

	#define TEXTURE2D_SHADOW_PARAM(textureName, samplerName)          TEXTURE2D(textureName),         SAMPLER_CMP(samplerName)
	#define TEXTURE2D_ARRAY_SHADOW_PARAM(textureName, samplerName)    TEXTURE2D_ARRAY(textureName),   SAMPLER_CMP(samplerName)
	#define TEXTURECUBE_SHADOW_PARAM(textureName, samplerName)        TEXTURECUBE(textureName),       SAMPLER_CMP(samplerName)
	#define TEXTURECUBE_ARRAY_SHADOW_PARAM(textureName, samplerName)  TEXTURECUBE_ARRAY(textureName), SAMPLER_CMP(samplerName)

	#define TEXTURE2D_ARGS(textureName, samplerName)                textureName, samplerName
	#define TEXTURE2D_ARRAY_ARGS(textureName, samplerName)          textureName, samplerName
	#define TEXTURECUBE_ARGS(textureName, samplerName)              textureName, samplerName
	#define TEXTURECUBE_ARRAY_ARGS(textureName, samplerName)        textureName, samplerName
	#define TEXTURE3D_ARGS(textureName, samplerName)                textureName, samplerName

	#define TEXTURE2D_SHADOW_ARGS(textureName, samplerName)         textureName, samplerName
	#define TEXTURE2D_ARRAY_SHADOW_ARGS(textureName, samplerName)   textureName, samplerName
	#define TEXTURECUBE_SHADOW_ARGS(textureName, samplerName)       textureName, samplerName
	#define TEXTURECUBE_ARRAY_SHADOW_ARGS(textureName, samplerName) textureName, samplerName

	#define PLATFORM_SAMPLE_TEXTURE2D(textureName, samplerName, coord2)                               textureName.Sample(samplerName, coord2)
	#define PLATFORM_SAMPLE_TEXTURE2D_LOD(textureName, samplerName, coord2, lod)                      textureName.SampleLevel(samplerName, coord2, lod)
	#define PLATFORM_SAMPLE_TEXTURE2D_BIAS(textureName, samplerName, coord2, bias)                    textureName.SampleBias(samplerName, coord2, bias)
	#define PLATFORM_SAMPLE_TEXTURE2D_GRAD(textureName, samplerName, coord2, dpdx, dpdy)              textureName.SampleGrad(samplerName, coord2, dpdx, dpdy)
	#define PLATFORM_SAMPLE_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)                  textureName.Sample(samplerName, float3(coord2, index))
	#define PLATFORM_SAMPLE_TEXTURE2D_ARRAY_LOD(textureName, samplerName, coord2, index, lod)         textureName.SampleLevel(samplerName, float3(coord2, index), lod)
	#define PLATFORM_SAMPLE_TEXTURE2D_ARRAY_BIAS(textureName, samplerName, coord2, index, bias)       textureName.SampleBias(samplerName, float3(coord2, index), bias)
	#define PLATFORM_SAMPLE_TEXTURE2D_ARRAY_GRAD(textureName, samplerName, coord2, index, dpdx, dpdy) textureName.SampleGrad(samplerName, float3(coord2, index), dpdx, dpdy)
	#define PLATFORM_SAMPLE_TEXTURECUBE(textureName, samplerName, coord3)                             textureName.Sample(samplerName, coord3)
	#define PLATFORM_SAMPLE_TEXTURECUBE_LOD(textureName, samplerName, coord3, lod)                    textureName.SampleLevel(samplerName, coord3, lod)
	#define PLATFORM_SAMPLE_TEXTURECUBE_BIAS(textureName, samplerName, coord3, bias)                  textureName.SampleBias(samplerName, coord3, bias)
	#define PLATFORM_SAMPLE_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)                textureName.Sample(samplerName, float4(coord3, index))
	#define PLATFORM_SAMPLE_TEXTURECUBE_ARRAY_LOD(textureName, samplerName, coord3, index, lod)       textureName.SampleLevel(samplerName, float4(coord3, index), lod)
	#define PLATFORM_SAMPLE_TEXTURECUBE_ARRAY_BIAS(textureName, samplerName, coord3, index, bias)     textureName.SampleBias(samplerName, float4(coord3, index), bias)
	#define PLATFORM_SAMPLE_TEXTURE3D(textureName, samplerName, coord3)                               textureName.Sample(samplerName, coord3)
	#define PLATFORM_SAMPLE_TEXTURE3D_LOD(textureName, samplerName, coord3, lod)                      textureName.SampleLevel(samplerName, coord3, lod)

	#define SAMPLE_TEXTURE2D(textureName, samplerName, coord2)                               PLATFORM_SAMPLE_TEXTURE2D(textureName, samplerName, coord2)
	#define SAMPLE_TEXTURE2D_LOD(textureName, samplerName, coord2, lod)                      PLATFORM_SAMPLE_TEXTURE2D_LOD(textureName, samplerName, coord2, lod)
	#define SAMPLE_TEXTURE2D_BIAS(textureName, samplerName, coord2, bias)                    PLATFORM_SAMPLE_TEXTURE2D_BIAS(textureName, samplerName, coord2, bias)
	#define SAMPLE_TEXTURE2D_GRAD(textureName, samplerName, coord2, dpdx, dpdy)              PLATFORM_SAMPLE_TEXTURE2D_GRAD(textureName, samplerName, coord2, dpdx, dpdy)
	#define SAMPLE_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)                  PLATFORM_SAMPLE_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)
	#define SAMPLE_TEXTURE2D_ARRAY_LOD(textureName, samplerName, coord2, index, lod)         PLATFORM_SAMPLE_TEXTURE2D_ARRAY_LOD(textureName, samplerName, coord2, index, lod)
	#define SAMPLE_TEXTURE2D_ARRAY_BIAS(textureName, samplerName, coord2, index, bias)       PLATFORM_SAMPLE_TEXTURE2D_ARRAY_BIAS(textureName, samplerName, coord2, index, bias)
	#define SAMPLE_TEXTURE2D_ARRAY_GRAD(textureName, samplerName, coord2, index, dpdx, dpdy) PLATFORM_SAMPLE_TEXTURE2D_ARRAY_GRAD(textureName, samplerName, coord2, index, dpdx, dpdy)
	#define SAMPLE_TEXTURECUBE(textureName, samplerName, coord3)                             PLATFORM_SAMPLE_TEXTURECUBE(textureName, samplerName, coord3)
	#define SAMPLE_TEXTURECUBE_LOD(textureName, samplerName, coord3, lod)                    PLATFORM_SAMPLE_TEXTURECUBE_LOD(textureName, samplerName, coord3, lod)
	#define SAMPLE_TEXTURECUBE_BIAS(textureName, samplerName, coord3, bias)                  PLATFORM_SAMPLE_TEXTURECUBE_BIAS(textureName, samplerName, coord3, bias)
	#define SAMPLE_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)                PLATFORM_SAMPLE_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)
	#define SAMPLE_TEXTURECUBE_ARRAY_LOD(textureName, samplerName, coord3, index, lod)       PLATFORM_SAMPLE_TEXTURECUBE_ARRAY_LOD(textureName, samplerName, coord3, index, lod)
	#define SAMPLE_TEXTURECUBE_ARRAY_BIAS(textureName, samplerName, coord3, index, bias)     PLATFORM_SAMPLE_TEXTURECUBE_ARRAY_BIAS(textureName, samplerName, coord3, index, bias)
	#define SAMPLE_TEXTURE3D(textureName, samplerName, coord3)                               PLATFORM_SAMPLE_TEXTURE3D(textureName, samplerName, coord3)
	#define SAMPLE_TEXTURE3D_LOD(textureName, samplerName, coord3, lod)                      PLATFORM_SAMPLE_TEXTURE3D_LOD(textureName, samplerName, coord3, lod)

	#define SAMPLE_TEXTURE2D_SHADOW(textureName, samplerName, coord3)                    textureName.SampleCmpLevelZero(samplerName, (coord3).xy, (coord3).z)
	#define SAMPLE_TEXTURE2D_ARRAY_SHADOW(textureName, samplerName, coord3, index)       textureName.SampleCmpLevelZero(samplerName, float3((coord3).xy, index), (coord3).z)
	#define SAMPLE_TEXTURECUBE_SHADOW(textureName, samplerName, coord4)                  textureName.SampleCmpLevelZero(samplerName, (coord4).xyz, (coord4).w)
	#define SAMPLE_TEXTURECUBE_ARRAY_SHADOW(textureName, samplerName, coord4, index)     textureName.SampleCmpLevelZero(samplerName, float4((coord4).xyz, index), (coord4).w)

	#define SAMPLE_DEPTH_TEXTURE(textureName, samplerName, coord2)          SAMPLE_TEXTURE2D(textureName, samplerName, coord2).r
	#define SAMPLE_DEPTH_TEXTURE_LOD(textureName, samplerName, coord2, lod) SAMPLE_TEXTURE2D_LOD(textureName, samplerName, coord2, lod).r

	#define LOAD_TEXTURE2D(textureName, unCoord2)                                   textureName.Load(int3(unCoord2, 0))
	#define LOAD_TEXTURE2D_LOD(textureName, unCoord2, lod)                          textureName.Load(int3(unCoord2, lod))
	#define LOAD_TEXTURE2D_MSAA(textureName, unCoord2, sampleIndex)                 textureName.Load(unCoord2, sampleIndex)
	#define LOAD_TEXTURE2D_ARRAY(textureName, unCoord2, index)                      textureName.Load(int4(unCoord2, index, 0))
	#define LOAD_TEXTURE2D_ARRAY_MSAA(textureName, unCoord2, index, sampleIndex)    textureName.Load(int3(unCoord2, index), sampleIndex)
	#define LOAD_TEXTURE2D_ARRAY_LOD(textureName, unCoord2, index, lod)             textureName.Load(int4(unCoord2, index, lod))
	#define LOAD_TEXTURE3D(textureName, unCoord3)                                   textureName.Load(int4(unCoord3, 0))
	#define LOAD_TEXTURE3D_LOD(textureName, unCoord3, lod)                          textureName.Load(int4(unCoord3, lod))

	#define PLATFORM_SUPPORT_GATHER
	#define GATHER_TEXTURE2D(textureName, samplerName, coord2)                textureName.Gather(samplerName, coord2)
	#define GATHER_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)   textureName.Gather(samplerName, float3(coord2, index))
	#define GATHER_TEXTURECUBE(textureName, samplerName, coord3)              textureName.Gather(samplerName, coord3)
	#define GATHER_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index) textureName.Gather(samplerName, float4(coord3, index))
	#define GATHER_RED_TEXTURE2D(textureName, samplerName, coord2)            textureName.GatherRed(samplerName, coord2)
	#define GATHER_GREEN_TEXTURE2D(textureName, samplerName, coord2)          textureName.GatherGreen(samplerName, coord2)
	#define GATHER_BLUE_TEXTURE2D(textureName, samplerName, coord2)           textureName.GatherBlue(samplerName, coord2)
	#define GATHER_ALPHA_TEXTURE2D(textureName, samplerName, coord2)          textureName.GatherAlpha(samplerName, coord2)
#elif defined(SHADER_API_XBOXONE)
	#define ZERO_INITIALIZE(type, name) name = (type)0;
	#define ZERO_INITIALIZE_ARRAY(type, name, arraySize) { for (int arrayIndex = 0; arrayIndex < arraySize; arrayIndex++) { name[arrayIndex] = (type)0; } }
	#define CALCULATE_TEXTURE2D_LOD(textureName, samplerName, coord2) textureName.CalculateLevelOfDetail(samplerName, coord2)
	#define TEXTURE2D(textureName)                Texture2D textureName
	#define TEXTURE2D_ARRAY(textureName)          Texture2DArray textureName
	#define TEXTURECUBE(textureName)              TextureCube textureName
	#define TEXTURECUBE_ARRAY(textureName)        TextureCubeArray textureName
	#define TEXTURE3D(textureName)                Texture3D textureName

	#define TEXTURE2D_FLOAT(textureName)          TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_FLOAT(textureName)    TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_FLOAT(textureName)        TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_FLOAT(textureName)  TEXTURECUBE_ARRAY(textureName)
	#define TEXTURE3D_FLOAT(textureName)          TEXTURE3D(textureName)

	#define TEXTURE2D_HALF(textureName)           TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_HALF(textureName)     TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_HALF(textureName)         TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_HALF(textureName)   TEXTURECUBE_ARRAY(textureName)
	#define TEXTURE3D_HALF(textureName)           TEXTURE3D(textureName)

	#define TEXTURE2D_SHADOW(textureName)         TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_SHADOW(textureName)   TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_SHADOW(textureName)       TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_SHADOW(textureName) TEXTURECUBE_ARRAY(textureName)

	#define RW_TEXTURE2D(type, textureName)       RWTexture2D<type> textureName
	#define RW_TEXTURE2D_ARRAY(type, textureName) RWTexture2DArray<type> textureName
	#define RW_TEXTURE3D(type, textureName)       RWTexture3D<type> textureName

	#define SAMPLER(samplerName)                  SamplerState samplerName
	#define SAMPLER_CMP(samplerName)              SamplerComparisonState samplerName

	#define TEXTURE2D_PARAM(textureName, samplerName)                 TEXTURE2D(textureName),         SAMPLER(samplerName)
	#define TEXTURE2D_ARRAY_PARAM(textureName, samplerName)           TEXTURE2D_ARRAY(textureName),   SAMPLER(samplerName)
	#define TEXTURECUBE_PARAM(textureName, samplerName)               TEXTURECUBE(textureName),       SAMPLER(samplerName)
	#define TEXTURECUBE_ARRAY_PARAM(textureName, samplerName)         TEXTURECUBE_ARRAY(textureName), SAMPLER(samplerName)
	#define TEXTURE3D_PARAM(textureName, samplerName)                 TEXTURE3D(textureName),         SAMPLER(samplerName)

	#define TEXTURE2D_SHADOW_PARAM(textureName, samplerName)          TEXTURE2D(textureName),         SAMPLER_CMP(samplerName)
	#define TEXTURE2D_ARRAY_SHADOW_PARAM(textureName, samplerName)    TEXTURE2D_ARRAY(textureName),   SAMPLER_CMP(samplerName)
	#define TEXTURECUBE_SHADOW_PARAM(textureName, samplerName)        TEXTURECUBE(textureName),       SAMPLER_CMP(samplerName)
	#define TEXTURECUBE_ARRAY_SHADOW_PARAM(textureName, samplerName)  TEXTURECUBE_ARRAY(textureName), SAMPLER_CMP(samplerName)

	#define TEXTURE2D_ARGS(textureName, samplerName)                textureName, samplerName
	#define TEXTURE2D_ARRAY_ARGS(textureName, samplerName)          textureName, samplerName
	#define TEXTURECUBE_ARGS(textureName, samplerName)              textureName, samplerName
	#define TEXTURECUBE_ARRAY_ARGS(textureName, samplerName)        textureName, samplerName
	#define TEXTURE3D_ARGS(textureName, samplerName)                textureName, samplerName

	#define TEXTURE2D_SHADOW_ARGS(textureName, samplerName)         textureName, samplerName
	#define TEXTURE2D_ARRAY_SHADOW_ARGS(textureName, samplerName)   textureName, samplerName
	#define TEXTURECUBE_SHADOW_ARGS(textureName, samplerName)       textureName, samplerName
	#define TEXTURECUBE_ARRAY_SHADOW_ARGS(textureName, samplerName) textureName, samplerName

	#define SAMPLE_TEXTURE2D(textureName, samplerName, coord2)                               textureName.Sample(samplerName, coord2)
	#define SAMPLE_TEXTURE2D_LOD(textureName, samplerName, coord2, lod)                      textureName.SampleLevel(samplerName, coord2, lod)
	#define SAMPLE_TEXTURE2D_BIAS(textureName, samplerName, coord2, bias)                    textureName.SampleBias(samplerName, coord2, bias)
	#define SAMPLE_TEXTURE2D_GRAD(textureName, samplerName, coord2, dpdx, dpdy)              textureName.SampleGrad(samplerName, coord2, dpdx, dpdy)
	#define SAMPLE_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)                  textureName.Sample(samplerName, float3(coord2, index))
	#define SAMPLE_TEXTURE2D_ARRAY_LOD(textureName, samplerName, coord2, index, lod)         textureName.SampleLevel(samplerName, float3(coord2, index), lod)
	#define SAMPLE_TEXTURE2D_ARRAY_BIAS(textureName, samplerName, coord2, index, bias)       textureName.SampleBias(samplerName, float3(coord2, index), bias)
	#define SAMPLE_TEXTURE2D_ARRAY_GRAD(textureName, samplerName, coord2, index, dpdx, dpdy) textureName.SampleGrad(samplerName, float3(coord2, index), dpdx, dpdy)
	#define SAMPLE_TEXTURECUBE(textureName, samplerName, coord3)                             textureName.Sample(samplerName, coord3)
	#define SAMPLE_TEXTURECUBE_LOD(textureName, samplerName, coord3, lod)                    textureName.SampleLevel(samplerName, coord3, lod)
	#define SAMPLE_TEXTURECUBE_BIAS(textureName, samplerName, coord3, bias)                  textureName.SampleBias(samplerName, coord3, bias)
	#define SAMPLE_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)                textureName.Sample(samplerName, float4(coord3, index))
	#define SAMPLE_TEXTURECUBE_ARRAY_LOD(textureName, samplerName, coord3, index, lod)       textureName.SampleLevel(samplerName, float4(coord3, index), lod)
	#define SAMPLE_TEXTURECUBE_ARRAY_BIAS(textureName, samplerName, coord3, index, bias)     textureName.SampleBias(samplerName, float4(coord3, index), bias)
	#define SAMPLE_TEXTURE3D(textureName, samplerName, coord3)                               textureName.Sample(samplerName, coord3)
	#define SAMPLE_TEXTURE3D_LOD(textureName, samplerName, coord3, lod)                      textureName.SampleLevel(samplerName, coord3, lod)

	#define SAMPLE_TEXTURE2D_SHADOW(textureName, samplerName, coord3)                    textureName.SampleCmpLevelZero(samplerName, (coord3).xy, (coord3).z)
	#define SAMPLE_TEXTURE2D_ARRAY_SHADOW(textureName, samplerName, coord3, index)       textureName.SampleCmpLevelZero(samplerName, float3((coord3).xy, index), (coord3).z)
	#define SAMPLE_TEXTURECUBE_SHADOW(textureName, samplerName, coord4)                  textureName.SampleCmpLevelZero(samplerName, (coord4).xyz, (coord4).w)
	#define SAMPLE_TEXTURECUBE_ARRAY_SHADOW(textureName, samplerName, coord4, index)     textureName.SampleCmpLevelZero(samplerName, float4((coord4).xyz, index), (coord4).w)

	#define LOAD_TEXTURE2D(textureName, unCoord2)                                   textureName.Load(int3(unCoord2, 0))
	#define LOAD_TEXTURE2D_LOD(textureName, unCoord2, lod)                          textureName.Load(int3(unCoord2, lod))
	#define LOAD_TEXTURE2D_MSAA(textureName, unCoord2, sampleIndex)                 textureName.Load(unCoord2, sampleIndex)
	#define LOAD_TEXTURE2D_ARRAY(textureName, unCoord2, index)                      textureName.Load(int4(unCoord2, index, 0))
	#define LOAD_TEXTURE2D_ARRAY_MSAA(textureName, unCoord2, index, sampleIndex)    textureName.Load(int3(unCoord2, index), sampleIndex)
	#define LOAD_TEXTURE2D_ARRAY_LOD(textureName, unCoord2, index, lod)             textureName.Load(int4(unCoord2, index, lod))
	#define LOAD_TEXTURE3D(textureName, unCoord3)                                   textureName.Load(int4(unCoord3, 0))
	#define LOAD_TEXTURE3D_LOD(textureName, unCoord3, lod)                          textureName.Load(int4(unCoord3, lod))

	#define PLATFORM_SUPPORT_GATHER
	#define GATHER_TEXTURE2D(textureName, samplerName, coord2)                textureName.Gather(samplerName, coord2)
	#define GATHER_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)   textureName.Gather(samplerName, float3(coord2, index))
	#define GATHER_TEXTURECUBE(textureName, samplerName, coord3)              textureName.Gather(samplerName, coord3)
	#define GATHER_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index) textureName.Gather(samplerName, float4(coord3, index))
	#define GATHER_RED_TEXTURE2D(textureName, samplerName, coord2)            textureName.GatherRed(samplerName, coord2)
	#define GATHER_GREEN_TEXTURE2D(textureName, samplerName, coord2)          textureName.GatherGreen(samplerName, coord2)
	#define GATHER_BLUE_TEXTURE2D(textureName, samplerName, coord2)           textureName.GatherBlue(samplerName, coord2)
	#define GATHER_ALPHA_TEXTURE2D(textureName, samplerName, coord2)          textureName.GatherAlpha(samplerName, coord2)
#elif defined(SHADER_API_PSSL)
	#define ZERO_INITIALIZE(type, name) name = (type)0;
	#define ZERO_INITIALIZE_ARRAY(type, name, arraySize) { for (int arrayIndex = 0; arrayIndex < arraySize; arrayIndex++) { name[arrayIndex] = (type)0; } }
	#define CALCULATE_TEXTURE2D_LOD(textureName, samplerName, coord2) textureName.GetLOD(samplerName, coord2)
	#define TEXTURE2D(textureName)                Texture2D textureName
	#define TEXTURE2D_ARRAY(textureName)          Texture2DArray textureName
	#define TEXTURECUBE(textureName)              TextureCube textureName
	#define TEXTURECUBE_ARRAY(textureName)        TextureCubeArray textureName
	#define TEXTURE3D(textureName)                Texture3D textureName

	#define TEXTURE2D_FLOAT(textureName)          TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_FLOAT(textureName)    TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_FLOAT(textureName)        TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_FLOAT(textureName)  TEXTURECUBE_ARRAY(textureName)
	#define TEXTURE3D_FLOAT(textureName)          TEXTURE3D(textureName)

	#define TEXTURE2D_HALF(textureName)           TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_HALF(textureName)     TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_HALF(textureName)         TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_HALF(textureName)   TEXTURECUBE_ARRAY(textureName)
	#define TEXTURE3D_HALF(textureName)           TEXTURE3D(textureName)

	#define TEXTURE2D_SHADOW(textureName)         TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_SHADOW(textureName)   TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_SHADOW(textureName)       TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_SHADOW(textureName) TEXTURECUBE_ARRAY(textureName)

	#define RW_TEXTURE2D(type, textureName)       RW_Texture2D<type> textureName
	#define RW_TEXTURE2D_ARRAY(type, textureName) RW_Texture2D_Array<type> textureName
	#define RW_TEXTURE3D(type, textureName)       RW_Texture3D<type> textureName

	#define SAMPLER(samplerName)                  SamplerState samplerName
	#define SAMPLER_CMP(samplerName)              SamplerComparisonState samplerName

	#define TEXTURE2D_PARAM(textureName, samplerName)                 TEXTURE2D(textureName),         SAMPLER(samplerName)
	#define TEXTURE2D_ARRAY_PARAM(textureName, samplerName)           TEXTURE2D_ARRAY(textureName),   SAMPLER(samplerName)
	#define TEXTURECUBE_PARAM(textureName, samplerName)               TEXTURECUBE(textureName),       SAMPLER(samplerName)
	#define TEXTURECUBE_ARRAY_PARAM(textureName, samplerName)         TEXTURECUBE_ARRAY(textureName), SAMPLER(samplerName)
	#define TEXTURE3D_PARAM(textureName, samplerName)                 TEXTURE3D(textureName),         SAMPLER(samplerName)

	#define TEXTURE2D_SHADOW_PARAM(textureName, samplerName)          TEXTURE2D(textureName),         SAMPLER_CMP(samplerName)
	#define TEXTURE2D_ARRAY_SHADOW_PARAM(textureName, samplerName)    TEXTURE2D_ARRAY(textureName),   SAMPLER_CMP(samplerName)
	#define TEXTURECUBE_SHADOW_PARAM(textureName, samplerName)        TEXTURECUBE(textureName),       SAMPLER_CMP(samplerName)
	#define TEXTURECUBE_ARRAY_SHADOW_PARAM(textureName, samplerName)  TEXTURECUBE_ARRAY(textureName), SAMPLER_CMP(samplerName)

	#define TEXTURE2D_ARGS(textureName, samplerName)                textureName, samplerName
	#define TEXTURE2D_ARRAY_ARGS(textureName, samplerName)          textureName, samplerName
	#define TEXTURECUBE_ARGS(textureName, samplerName)              textureName, samplerName
	#define TEXTURECUBE_ARRAY_ARGS(textureName, samplerName)        textureName, samplerName
	#define TEXTURE3D_ARGS(textureName, samplerName)                textureName, samplerName

	#define TEXTURE2D_SHADOW_ARGS(textureName, samplerName)         textureName, samplerName
	#define TEXTURE2D_ARRAY_SHADOW_ARGS(textureName, samplerName)   textureName, samplerName
	#define TEXTURECUBE_SHADOW_ARGS(textureName, samplerName)       textureName, samplerName
	#define TEXTURECUBE_ARRAY_SHADOW_ARGS(textureName, samplerName) textureName, samplerName

	#define SAMPLE_TEXTURE2D(textureName, samplerName, coord2)                               textureName.Sample(samplerName, coord2)
	#define SAMPLE_TEXTURE2D_LOD(textureName, samplerName, coord2, lod)                      textureName.SampleLevel(samplerName, coord2, lod)
	#define SAMPLE_TEXTURE2D_BIAS(textureName, samplerName, coord2, bias)                    textureName.SampleBias(samplerName, coord2, bias)
	#define SAMPLE_TEXTURE2D_GRAD(textureName, samplerName, coord2, dpdx, dpdy)              textureName.SampleGrad(samplerName, coord2, dpdx, dpdy)
	#define SAMPLE_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)                  textureName.Sample(samplerName, float3(coord2, index))
	#define SAMPLE_TEXTURE2D_ARRAY_LOD(textureName, samplerName, coord2, index, lod)         textureName.SampleLevel(samplerName, float3(coord2, index), lod)
	#define SAMPLE_TEXTURE2D_ARRAY_BIAS(textureName, samplerName, coord2, index, bias)       textureName.SampleBias(samplerName, float3(coord2, index), bias)
	#define SAMPLE_TEXTURE2D_ARRAY_GRAD(textureName, samplerName, coord2, index, dpdx, dpdy) textureName.SampleGrad(samplerName, float3(coord2, index), dpdx, dpdy)
	#define SAMPLE_TEXTURECUBE(textureName, samplerName, coord3)                             textureName.Sample(samplerName, coord3)
	#define SAMPLE_TEXTURECUBE_LOD(textureName, samplerName, coord3, lod)                    textureName.SampleLevel(samplerName, coord3, lod)
	#define SAMPLE_TEXTURECUBE_BIAS(textureName, samplerName, coord3, bias)                  textureName.SampleBias(samplerName, coord3, bias)
	#define SAMPLE_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)                textureName.Sample(samplerName, float4(coord3, index))
	#define SAMPLE_TEXTURECUBE_ARRAY_LOD(textureName, samplerName, coord3, index, lod)       textureName.SampleLevel(samplerName, float4(coord3, index), lod)
	#define SAMPLE_TEXTURECUBE_ARRAY_BIAS(textureName, samplerName, coord3, index, bias)     textureName.SampleBias(samplerName, float4(coord3, index), bias)
	#define SAMPLE_TEXTURE3D(textureName, samplerName, coord3)                               textureName.Sample(samplerName, coord3)
	#define SAMPLE_TEXTURE3D_LOD(textureName, samplerName, coord3, lod)                      textureName.SampleLevel(samplerName, coord3, lod)

	#define SAMPLE_TEXTURE2D_SHADOW(textureName, samplerName, coord3)                    textureName.SampleCmpLevelZero(samplerName, (coord3).xy, (coord3).z)
	#define SAMPLE_TEXTURE2D_ARRAY_SHADOW(textureName, samplerName, coord3, index)       textureName.SampleCmpLevelZero(samplerName, float3((coord3).xy, index), (coord3).z)
	#define SAMPLE_TEXTURECUBE_SHADOW(textureName, samplerName, coord4)                  textureName.SampleCmpLevelZero(samplerName, (coord4).xyz, (coord4).w)
	#define SAMPLE_TEXTURECUBE_ARRAY_SHADOW(textureName, samplerName, coord4, index)     textureName.SampleCmpLevelZero(samplerName, float4((coord4).xyz, index), (coord4).w)

	#define LOAD_TEXTURE2D(textureName, unCoord2)                                   textureName.Load(int3(unCoord2, 0))
	#define LOAD_TEXTURE2D_LOD(textureName, unCoord2, lod)                          textureName.Load(int3(unCoord2, lod))
	#define LOAD_TEXTURE2D_MSAA(textureName, unCoord2, sampleIndex)                 textureName.Load(unCoord2, sampleIndex)
	#define LOAD_TEXTURE2D_ARRAY(textureName, unCoord2, index)                      textureName.Load(int4(unCoord2, index, 0))
	#define LOAD_TEXTURE2D_ARRAY_MSAA(textureName, unCoord2, index, sampleIndex)    textureName.Load(int3(unCoord2, index), sampleIndex)
	#define LOAD_TEXTURE2D_ARRAY_LOD(textureName, unCoord2, index, lod)             textureName.Load(int4(unCoord2, index, lod))
	#define LOAD_TEXTURE3D(textureName, unCoord3)                                   textureName.Load(int4(unCoord3, 0))
	#define LOAD_TEXTURE3D_LOD(textureName, unCoord3, lod)                          textureName.Load(int4(unCoord3, lod))

	#define PLATFORM_SUPPORT_GATHER
	#define GATHER_TEXTURE2D(textureName, samplerName, coord2)                textureName.Gather(samplerName, coord2)
	#define GATHER_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)   textureName.Gather(samplerName, float3(coord2, index))
	#define GATHER_TEXTURECUBE(textureName, samplerName, coord3)              textureName.Gather(samplerName, coord3)
	#define GATHER_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index) textureName.Gather(samplerName, float4(coord3, index))
	#define GATHER_RED_TEXTURE2D(textureName, samplerName, coord2)            textureName.GatherRed(samplerName, coord2)
	#define GATHER_GREEN_TEXTURE2D(textureName, samplerName, coord2)          textureName.GatherGreen(samplerName, coord2)
	#define GATHER_BLUE_TEXTURE2D(textureName, samplerName, coord2)           textureName.GatherBlue(samplerName, coord2)
	#define GATHER_ALPHA_TEXTURE2D(textureName, samplerName, coord2)          textureName.GatherAlpha(samplerName, coord2)
#elif defined(SHADER_API_D3D11)
	#define ZERO_INITIALIZE(type, name) name = (type)0;
	#define ZERO_INITIALIZE_ARRAY(type, name, arraySize) { for (int arrayIndex = 0; arrayIndex < arraySize; arrayIndex++) { name[arrayIndex] = (type)0; } }
	#define CALCULATE_TEXTURE2D_LOD(textureName, samplerName, coord2) textureName.CalculateLevelOfDetail(samplerName, coord2)
	#define TEXTURE2D(textureName)                Texture2D textureName
	#define TEXTURE2D_ARRAY(textureName)          Texture2DArray textureName
	#define TEXTURECUBE(textureName)              TextureCube textureName
	#define TEXTURECUBE_ARRAY(textureName)        TextureCubeArray textureName
	#define TEXTURE3D(textureName)                Texture3D textureName

	#define TEXTURE2D_FLOAT(textureName)          TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_FLOAT(textureName)    TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_FLOAT(textureName)        TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_FLOAT(textureName)  TEXTURECUBE_ARRAY(textureName)
	#define TEXTURE3D_FLOAT(textureName)          TEXTURE3D(textureName)

	#define TEXTURE2D_HALF(textureName)           TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_HALF(textureName)     TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_HALF(textureName)         TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_HALF(textureName)   TEXTURECUBE_ARRAY(textureName)
	#define TEXTURE3D_HALF(textureName)           TEXTURE3D(textureName)

	#define TEXTURE2D_SHADOW(textureName)         TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_SHADOW(textureName)   TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_SHADOW(textureName)       TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_SHADOW(textureName) TEXTURECUBE_ARRAY(textureName)

	#define RW_TEXTURE2D(type, textureName)       RWTexture2D<type> textureName
	#define RW_TEXTURE2D_ARRAY(type, textureName) RWTexture2DArray<type> textureName
	#define RW_TEXTURE3D(type, textureName)       RWTexture3D<type> textureName

	#define SAMPLER(samplerName)                  SamplerState samplerName
	#define SAMPLER_CMP(samplerName)              SamplerComparisonState samplerName

	#define TEXTURE2D_PARAM(textureName, samplerName)                 TEXTURE2D(textureName),         SAMPLER(samplerName)
	#define TEXTURE2D_ARRAY_PARAM(textureName, samplerName)           TEXTURE2D_ARRAY(textureName),   SAMPLER(samplerName)
	#define TEXTURECUBE_PARAM(textureName, samplerName)               TEXTURECUBE(textureName),       SAMPLER(samplerName)
	#define TEXTURECUBE_ARRAY_PARAM(textureName, samplerName)         TEXTURECUBE_ARRAY(textureName), SAMPLER(samplerName)
	#define TEXTURE3D_PARAM(textureName, samplerName)                 TEXTURE3D(textureName),         SAMPLER(samplerName)

	#define TEXTURE2D_SHADOW_PARAM(textureName, samplerName)          TEXTURE2D(textureName),         SAMPLER_CMP(samplerName)
	#define TEXTURE2D_ARRAY_SHADOW_PARAM(textureName, samplerName)    TEXTURE2D_ARRAY(textureName),   SAMPLER_CMP(samplerName)
	#define TEXTURECUBE_SHADOW_PARAM(textureName, samplerName)        TEXTURECUBE(textureName),       SAMPLER_CMP(samplerName)
	#define TEXTURECUBE_ARRAY_SHADOW_PARAM(textureName, samplerName)  TEXTURECUBE_ARRAY(textureName), SAMPLER_CMP(samplerName)

	#define TEXTURE2D_ARGS(textureName, samplerName)                textureName, samplerName
	#define TEXTURE2D_ARRAY_ARGS(textureName, samplerName)          textureName, samplerName
	#define TEXTURECUBE_ARGS(textureName, samplerName)              textureName, samplerName
	#define TEXTURECUBE_ARRAY_ARGS(textureName, samplerName)        textureName, samplerName
	#define TEXTURE3D_ARGS(textureName, samplerName)                textureName, samplerName

	#define TEXTURE2D_SHADOW_ARGS(textureName, samplerName)         textureName, samplerName
	#define TEXTURE2D_ARRAY_SHADOW_ARGS(textureName, samplerName)   textureName, samplerName
	#define TEXTURECUBE_SHADOW_ARGS(textureName, samplerName)       textureName, samplerName
	#define TEXTURECUBE_ARRAY_SHADOW_ARGS(textureName, samplerName) textureName, samplerName

	#define SAMPLE_TEXTURE2D(textureName, samplerName, coord2)                               textureName.Sample(samplerName, coord2)
	#define SAMPLE_TEXTURE2D_LOD(textureName, samplerName, coord2, lod)                      textureName.SampleLevel(samplerName, coord2, lod)
	#define SAMPLE_TEXTURE2D_BIAS(textureName, samplerName, coord2, bias)                    textureName.SampleBias(samplerName, coord2, bias)
	#define SAMPLE_TEXTURE2D_GRAD(textureName, samplerName, coord2, dpdx, dpdy)              textureName.SampleGrad(samplerName, coord2, dpdx, dpdy)
	#define SAMPLE_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)                  textureName.Sample(samplerName, float3(coord2, index))
	#define SAMPLE_TEXTURE2D_ARRAY_LOD(textureName, samplerName, coord2, index, lod)         textureName.SampleLevel(samplerName, float3(coord2, index), lod)
	#define SAMPLE_TEXTURE2D_ARRAY_BIAS(textureName, samplerName, coord2, index, bias)       textureName.SampleBias(samplerName, float3(coord2, index), bias)
	#define SAMPLE_TEXTURE2D_ARRAY_GRAD(textureName, samplerName, coord2, index, dpdx, dpdy) textureName.SampleGrad(samplerName, float3(coord2, index), dpdx, dpdy)
	#define SAMPLE_TEXTURECUBE(textureName, samplerName, coord3)                             textureName.Sample(samplerName, coord3)
	#define SAMPLE_TEXTURECUBE_LOD(textureName, samplerName, coord3, lod)                    textureName.SampleLevel(samplerName, coord3, lod)
	#define SAMPLE_TEXTURECUBE_BIAS(textureName, samplerName, coord3, bias)                  textureName.SampleBias(samplerName, coord3, bias)
	#define SAMPLE_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)                textureName.Sample(samplerName, float4(coord3, index))
	#define SAMPLE_TEXTURECUBE_ARRAY_LOD(textureName, samplerName, coord3, index, lod)       textureName.SampleLevel(samplerName, float4(coord3, index), lod)
	#define SAMPLE_TEXTURECUBE_ARRAY_BIAS(textureName, samplerName, coord3, index, bias)     textureName.SampleBias(samplerName, float4(coord3, index), bias)
	#define SAMPLE_TEXTURE3D(textureName, samplerName, coord3)                               textureName.Sample(samplerName, coord3)
	#define SAMPLE_TEXTURE3D_LOD(textureName, samplerName, coord3, lod)                      textureName.SampleLevel(samplerName, coord3, lod)

	#define SAMPLE_TEXTURE2D_SHADOW(textureName, samplerName, coord3)                    textureName.SampleCmpLevelZero(samplerName, (coord3).xy, (coord3).z)
	#define SAMPLE_TEXTURE2D_ARRAY_SHADOW(textureName, samplerName, coord3, index)       textureName.SampleCmpLevelZero(samplerName, float3((coord3).xy, index), (coord3).z)
	#define SAMPLE_TEXTURECUBE_SHADOW(textureName, samplerName, coord4)                  textureName.SampleCmpLevelZero(samplerName, (coord4).xyz, (coord4).w)
	#define SAMPLE_TEXTURECUBE_ARRAY_SHADOW(textureName, samplerName, coord4, index)     textureName.SampleCmpLevelZero(samplerName, float4((coord4).xyz, index), (coord4).w)

	#define LOAD_TEXTURE2D(textureName, unCoord2)                                   textureName.Load(int3(unCoord2, 0))
	#define LOAD_TEXTURE2D_LOD(textureName, unCoord2, lod)                          textureName.Load(int3(unCoord2, lod))
	#define LOAD_TEXTURE2D_MSAA(textureName, unCoord2, sampleIndex)                 textureName.Load(unCoord2, sampleIndex)
	#define LOAD_TEXTURE2D_ARRAY(textureName, unCoord2, index)                      textureName.Load(int4(unCoord2, index, 0))
	#define LOAD_TEXTURE2D_ARRAY_MSAA(textureName, unCoord2, index, sampleIndex)    textureName.Load(int3(unCoord2, index), sampleIndex)
	#define LOAD_TEXTURE2D_ARRAY_LOD(textureName, unCoord2, index, lod)             textureName.Load(int4(unCoord2, index, lod))
	#define LOAD_TEXTURE3D(textureName, unCoord3)                                   textureName.Load(int4(unCoord3, 0))
	#define LOAD_TEXTURE3D_LOD(textureName, unCoord3, lod)                          textureName.Load(int4(unCoord3, lod))

	#define PLATFORM_SUPPORT_GATHER
	#define GATHER_TEXTURE2D(textureName, samplerName, coord2)                textureName.Gather(samplerName, coord2)
	#define GATHER_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)   textureName.Gather(samplerName, float3(coord2, index))
	#define GATHER_TEXTURECUBE(textureName, samplerName, coord3)              textureName.Gather(samplerName, coord3)
	#define GATHER_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index) textureName.Gather(samplerName, float4(coord3, index))
	#define GATHER_RED_TEXTURE2D(textureName, samplerName, coord2)            textureName.GatherRed(samplerName, coord2)
	#define GATHER_GREEN_TEXTURE2D(textureName, samplerName, coord2)          textureName.GatherGreen(samplerName, coord2)
	#define GATHER_BLUE_TEXTURE2D(textureName, samplerName, coord2)           textureName.GatherBlue(samplerName, coord2)
	#define GATHER_ALPHA_TEXTURE2D(textureName, samplerName, coord2)          textureName.GatherAlpha(samplerName, coord2)

#elif defined(SHADER_API_METAL)
	#define ZERO_INITIALIZE(type, name) name = (type)0;
	#define ZERO_INITIALIZE_ARRAY(type, name, arraySize) { for (int arrayIndex = 0; arrayIndex < arraySize; arrayIndex++) { name[arrayIndex] = (type)0; } }
	#define CALCULATE_TEXTURE2D_LOD(textureName, samplerName, coord2) textureName.CalculateLevelOfDetail(samplerName, coord2)
	#define TEXTURE2D(textureName)                Texture2D textureName
	#define TEXTURE2D_ARRAY(textureName)          Texture2DArray textureName
	#define TEXTURECUBE(textureName)              TextureCube textureName
	#define TEXTURECUBE_ARRAY(textureName)        TextureCubeArray textureName
	#define TEXTURE3D(textureName)                Texture3D textureName

	#define TEXTURE2D_FLOAT(textureName)          Texture2D_float textureName
	#define TEXTURE2D_ARRAY_FLOAT(textureName)    Texture2DArray textureName    
	#define TEXTURECUBE_FLOAT(textureName)        TextureCube_float textureName
	#define TEXTURECUBE_ARRAY_FLOAT(textureName)  TextureCubeArray textureName  
	#define TEXTURE3D_FLOAT(textureName)          Texture3D_float textureName

	#define TEXTURE2D_HALF(textureName)           Texture2D_half textureName
	#define TEXTURE2D_ARRAY_HALF(textureName)     Texture2DArray textureName    
	#define TEXTURECUBE_HALF(textureName)         TextureCube_half textureName
	#define TEXTURECUBE_ARRAY_HALF(textureName)   TextureCubeArray textureName  
	#define TEXTURE3D_HALF(textureName)           Texture3D_half textureName

	#define TEXTURE2D_SHADOW(textureName)         TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_SHADOW(textureName)   TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_SHADOW(textureName)       TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_SHADOW(textureName) TEXTURECUBE_ARRAY(textureName)

	#define RW_TEXTURE2D(type, textureName)       RWTexture2D<type> textureName
	#define RW_TEXTURE2D_ARRAY(type, textureName) RWTexture2DArray<type> textureName
	#define RW_TEXTURE3D(type, textureName)       RWTexture3D<type> textureName

	#define SAMPLER(samplerName)                  SamplerState samplerName
	#define SAMPLER_CMP(samplerName)              SamplerComparisonState samplerName

	#define TEXTURE2D_PARAM(textureName, samplerName)                 TEXTURE2D(textureName),         SAMPLER(samplerName)
	#define TEXTURE2D_ARRAY_PARAM(textureName, samplerName)           TEXTURE2D_ARRAY(textureName),   SAMPLER(samplerName)
	#define TEXTURECUBE_PARAM(textureName, samplerName)               TEXTURECUBE(textureName),       SAMPLER(samplerName)
	#define TEXTURECUBE_ARRAY_PARAM(textureName, samplerName)         TEXTURECUBE_ARRAY(textureName), SAMPLER(samplerName)
	#define TEXTURE3D_PARAM(textureName, samplerName)                 TEXTURE3D(textureName),         SAMPLER(samplerName)

	#define TEXTURE2D_SHADOW_PARAM(textureName, samplerName)          TEXTURE2D(textureName),         SAMPLER_CMP(samplerName)
	#define TEXTURE2D_ARRAY_SHADOW_PARAM(textureName, samplerName)    TEXTURE2D_ARRAY(textureName),   SAMPLER_CMP(samplerName)
	#define TEXTURECUBE_SHADOW_PARAM(textureName, samplerName)        TEXTURECUBE(textureName),       SAMPLER_CMP(samplerName)
	#define TEXTURECUBE_ARRAY_SHADOW_PARAM(textureName, samplerName)  TEXTURECUBE_ARRAY(textureName), SAMPLER_CMP(samplerName)

	#define TEXTURE2D_ARGS(textureName, samplerName)                textureName, samplerName
	#define TEXTURE2D_ARRAY_ARGS(textureName, samplerName)          textureName, samplerName
	#define TEXTURECUBE_ARGS(textureName, samplerName)              textureName, samplerName
	#define TEXTURECUBE_ARRAY_ARGS(textureName, samplerName)        textureName, samplerName
	#define TEXTURE3D_ARGS(textureName, samplerName)                textureName, samplerName

	#define TEXTURE2D_SHADOW_ARGS(textureName, samplerName)         textureName, samplerName
	#define TEXTURE2D_ARRAY_SHADOW_ARGS(textureName, samplerName)   textureName, samplerName
	#define TEXTURECUBE_SHADOW_ARGS(textureName, samplerName)       textureName, samplerName
	#define TEXTURECUBE_ARRAY_SHADOW_ARGS(textureName, samplerName) textureName, samplerName

	#define SAMPLE_TEXTURE2D(textureName, samplerName, coord2)                               textureName.Sample(samplerName, coord2)
	#define SAMPLE_TEXTURE2D_LOD(textureName, samplerName, coord2, lod)                      textureName.SampleLevel(samplerName, coord2, lod)
	#define SAMPLE_TEXTURE2D_BIAS(textureName, samplerName, coord2, bias)                    textureName.SampleBias(samplerName, coord2, bias)
	#define SAMPLE_TEXTURE2D_GRAD(textureName, samplerName, coord2, dpdx, dpdy)              textureName.SampleGrad(samplerName, coord2, dpdx, dpdy)
	#define SAMPLE_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)                  textureName.Sample(samplerName, float3(coord2, index))
	#define SAMPLE_TEXTURE2D_ARRAY_LOD(textureName, samplerName, coord2, index, lod)         textureName.SampleLevel(samplerName, float3(coord2, index), lod)
	#define SAMPLE_TEXTURE2D_ARRAY_BIAS(textureName, samplerName, coord2, index, bias)       textureName.SampleBias(samplerName, float3(coord2, index), bias)
	#define SAMPLE_TEXTURE2D_ARRAY_GRAD(textureName, samplerName, coord2, index, dpdx, dpdy) textureName.SampleGrad(samplerName, float3(coord2, index), dpdx, dpdy)
	#define SAMPLE_TEXTURECUBE(textureName, samplerName, coord3)                             textureName.Sample(samplerName, coord3)
	#define SAMPLE_TEXTURECUBE_LOD(textureName, samplerName, coord3, lod)                    textureName.SampleLevel(samplerName, coord3, lod)
	#define SAMPLE_TEXTURECUBE_BIAS(textureName, samplerName, coord3, bias)                  textureName.SampleBias(samplerName, coord3, bias)
	#define SAMPLE_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)                textureName.Sample(samplerName, float4(coord3, index))
	#define SAMPLE_TEXTURECUBE_ARRAY_LOD(textureName, samplerName, coord3, index, lod)       textureName.SampleLevel(samplerName, float4(coord3, index), lod)
	#define SAMPLE_TEXTURECUBE_ARRAY_BIAS(textureName, samplerName, coord3, index, bias)     textureName.SampleBias(samplerName, float4(coord3, index), bias)
	#define SAMPLE_TEXTURE3D(textureName, samplerName, coord3)                               textureName.Sample(samplerName, coord3)
	#define SAMPLE_TEXTURE3D_LOD(textureName, samplerName, coord3, lod)                      textureName.SampleLevel(samplerName, coord3, lod)

	#define SAMPLE_TEXTURE2D_SHADOW(textureName, samplerName, coord3)                    textureName.SampleCmpLevelZero(samplerName, (coord3).xy, (coord3).z)
	#define SAMPLE_TEXTURE2D_ARRAY_SHADOW(textureName, samplerName, coord3, index)       textureName.SampleCmpLevelZero(samplerName, float3((coord3).xy, index), (coord3).z)
	#define SAMPLE_TEXTURECUBE_SHADOW(textureName, samplerName, coord4)                  textureName.SampleCmpLevelZero(samplerName, (coord4).xyz, (coord4).w)
	#define SAMPLE_TEXTURECUBE_ARRAY_SHADOW(textureName, samplerName, coord4, index)     textureName.SampleCmpLevelZero(samplerName, float4((coord4).xyz, index), (coord4).w)

	#define LOAD_TEXTURE2D(textureName, unCoord2)                                   textureName.Load(int3(unCoord2, 0))
	#define LOAD_TEXTURE2D_LOD(textureName, unCoord2, lod)                          textureName.Load(int3(unCoord2, lod))
	#define LOAD_TEXTURE2D_MSAA(textureName, unCoord2, sampleIndex)                 textureName.Load(unCoord2, sampleIndex)
	#define LOAD_TEXTURE2D_ARRAY(textureName, unCoord2, index)                      textureName.Load(int4(unCoord2, index, 0))
	#define LOAD_TEXTURE2D_ARRAY_MSAA(textureName, unCoord2, index, sampleIndex)    textureName.Load(int3(unCoord2, index), sampleIndex)
	#define LOAD_TEXTURE2D_ARRAY_LOD(textureName, unCoord2, index, lod)             textureName.Load(int4(unCoord2, index, lod))
	#define LOAD_TEXTURE3D(textureName, unCoord3)                                   textureName.Load(int4(unCoord3, 0))
	#define LOAD_TEXTURE3D_LOD(textureName, unCoord3, lod)                          textureName.Load(int4(unCoord3, lod))

	#define PLATFORM_SUPPORT_GATHER
	#define GATHER_TEXTURE2D(textureName, samplerName, coord2)                textureName.Gather(samplerName, coord2)
	#define GATHER_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)   textureName.Gather(samplerName, float3(coord2, index))
	#define GATHER_TEXTURECUBE(textureName, samplerName, coord3)              textureName.Gather(samplerName, coord3)
	#define GATHER_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index) textureName.Gather(samplerName, float4(coord3, index))
	#define GATHER_RED_TEXTURE2D(textureName, samplerName, coord2)            textureName.GatherRed(samplerName, coord2)
	#define GATHER_GREEN_TEXTURE2D(textureName, samplerName, coord2)          textureName.GatherGreen(samplerName, coord2)
	#define GATHER_BLUE_TEXTURE2D(textureName, samplerName, coord2)           textureName.GatherBlue(samplerName, coord2)
	#define GATHER_ALPHA_TEXTURE2D(textureName, samplerName, coord2)          textureName.GatherAlpha(samplerName, coord2)

#elif defined(SHADER_API_VULKAN)
	#define ZERO_INITIALIZE(type, name) name = (type)0;
	#define ZERO_INITIALIZE_ARRAY(type, name, arraySize) { for (int arrayIndex = 0; arrayIndex < arraySize; arrayIndex++) { name[arrayIndex] = (type)0; } }
	#define CALCULATE_TEXTURE2D_LOD(textureName, samplerName, coord2) textureName.CalculateLevelOfDetail(samplerName, coord2)
	#define TEXTURE2D(textureName)                Texture2D textureName
	#define TEXTURE2D_ARRAY(textureName)          Texture2DArray textureName
	#define TEXTURECUBE(textureName)              TextureCube textureName
	#define TEXTURECUBE_ARRAY(textureName)        TextureCubeArray textureName
	#define TEXTURE3D(textureName)                Texture3D textureName

	#define TEXTURE2D_FLOAT(textureName)          Texture2D_float textureName
	#define TEXTURE2D_ARRAY_FLOAT(textureName)    Texture2DArray textureName    
	#define TEXTURECUBE_FLOAT(textureName)        TextureCube_float textureName
	#define TEXTURECUBE_ARRAY_FLOAT(textureName)  TextureCubeArray textureName  
	#define TEXTURE3D_FLOAT(textureName)          Texture3D_float textureName

	#define TEXTURE2D_HALF(textureName)           Texture2D_half textureName
	#define TEXTURE2D_ARRAY_HALF(textureName)     Texture2DArray textureName    
	#define TEXTURECUBE_HALF(textureName)         TextureCube_half textureName
	#define TEXTURECUBE_ARRAY_HALF(textureName)   TextureCubeArray textureName  
	#define TEXTURE3D_HALF(textureName)           Texture3D_half textureName

	#define TEXTURE2D_SHADOW(textureName)         TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_SHADOW(textureName)   TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_SHADOW(textureName)       TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_SHADOW(textureName) TEXTURECUBE_ARRAY(textureName)

	#define RW_TEXTURE2D(type, textureName)       RWTexture2D<type> textureName
	#define RW_TEXTURE2D_ARRAY(type, textureName) RWTexture2DArray<type> textureName
	#define RW_TEXTURE3D(type, textureName)       RWTexture3D<type> textureName

	#define SAMPLER(samplerName)                  SamplerState samplerName
	#define SAMPLER_CMP(samplerName)              SamplerComparisonState samplerName

	#define TEXTURE2D_PARAM(textureName, samplerName)                 TEXTURE2D(textureName),         SAMPLER(samplerName)
	#define TEXTURE2D_ARRAY_PARAM(textureName, samplerName)           TEXTURE2D_ARRAY(textureName),   SAMPLER(samplerName)
	#define TEXTURECUBE_PARAM(textureName, samplerName)               TEXTURECUBE(textureName),       SAMPLER(samplerName)
	#define TEXTURECUBE_ARRAY_PARAM(textureName, samplerName)         TEXTURECUBE_ARRAY(textureName), SAMPLER(samplerName)
	#define TEXTURE3D_PARAM(textureName, samplerName)                 TEXTURE3D(textureName),         SAMPLER(samplerName)

	#define TEXTURE2D_SHADOW_PARAM(textureName, samplerName)          TEXTURE2D(textureName),         SAMPLER_CMP(samplerName)
	#define TEXTURE2D_ARRAY_SHADOW_PARAM(textureName, samplerName)    TEXTURE2D_ARRAY(textureName),   SAMPLER_CMP(samplerName)
	#define TEXTURECUBE_SHADOW_PARAM(textureName, samplerName)        TEXTURECUBE(textureName),       SAMPLER_CMP(samplerName)
	#define TEXTURECUBE_ARRAY_SHADOW_PARAM(textureName, samplerName)  TEXTURECUBE_ARRAY(textureName), SAMPLER_CMP(samplerName)

	#define TEXTURE2D_ARGS(textureName, samplerName)                textureName, samplerName
	#define TEXTURE2D_ARRAY_ARGS(textureName, samplerName)          textureName, samplerName
	#define TEXTURECUBE_ARGS(textureName, samplerName)              textureName, samplerName
	#define TEXTURECUBE_ARRAY_ARGS(textureName, samplerName)        textureName, samplerName
	#define TEXTURE3D_ARGS(textureName, samplerName)                textureName, samplerName

	#define TEXTURE2D_SHADOW_ARGS(textureName, samplerName)         textureName, samplerName
	#define TEXTURE2D_ARRAY_SHADOW_ARGS(textureName, samplerName)   textureName, samplerName
	#define TEXTURECUBE_SHADOW_ARGS(textureName, samplerName)       textureName, samplerName
	#define TEXTURECUBE_ARRAY_SHADOW_ARGS(textureName, samplerName) textureName, samplerName

	#define SAMPLE_TEXTURE2D(textureName, samplerName, coord2)                               textureName.Sample(samplerName, coord2)
	#define SAMPLE_TEXTURE2D_LOD(textureName, samplerName, coord2, lod)                      textureName.SampleLevel(samplerName, coord2, lod)
	#define SAMPLE_TEXTURE2D_BIAS(textureName, samplerName, coord2, bias)                    textureName.SampleBias(samplerName, coord2, bias)
	#define SAMPLE_TEXTURE2D_GRAD(textureName, samplerName, coord2, dpdx, dpdy)              textureName.SampleGrad(samplerName, coord2, dpdx, dpdy)
	#define SAMPLE_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)                  textureName.Sample(samplerName, float3(coord2, index))
	#define SAMPLE_TEXTURE2D_ARRAY_LOD(textureName, samplerName, coord2, index, lod)         textureName.SampleLevel(samplerName, float3(coord2, index), lod)
	#define SAMPLE_TEXTURE2D_ARRAY_BIAS(textureName, samplerName, coord2, index, bias)       textureName.SampleBias(samplerName, float3(coord2, index), bias)
	#define SAMPLE_TEXTURE2D_ARRAY_GRAD(textureName, samplerName, coord2, index, dpdx, dpdy) textureName.SampleGrad(samplerName, float3(coord2, index), dpdx, dpdy)
	#define SAMPLE_TEXTURECUBE(textureName, samplerName, coord3)                             textureName.Sample(samplerName, coord3)
	#define SAMPLE_TEXTURECUBE_LOD(textureName, samplerName, coord3, lod)                    textureName.SampleLevel(samplerName, coord3, lod)
	#define SAMPLE_TEXTURECUBE_BIAS(textureName, samplerName, coord3, bias)                  textureName.SampleBias(samplerName, coord3, bias)
	#define SAMPLE_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)                textureName.Sample(samplerName, float4(coord3, index))
	#define SAMPLE_TEXTURECUBE_ARRAY_LOD(textureName, samplerName, coord3, index, lod)       textureName.SampleLevel(samplerName, float4(coord3, index), lod)
	#define SAMPLE_TEXTURECUBE_ARRAY_BIAS(textureName, samplerName, coord3, index, bias)     textureName.SampleBias(samplerName, float4(coord3, index), bias)
	#define SAMPLE_TEXTURE3D(textureName, samplerName, coord3)                               textureName.Sample(samplerName, coord3)
	#define SAMPLE_TEXTURE3D_LOD(textureName, samplerName, coord3, lod)                      textureName.SampleLevel(samplerName, coord3, lod)

	#define SAMPLE_TEXTURE2D_SHADOW(textureName, samplerName, coord3)                    textureName.SampleCmpLevelZero(samplerName, (coord3).xy, (coord3).z)
	#define SAMPLE_TEXTURE2D_ARRAY_SHADOW(textureName, samplerName, coord3, index)       textureName.SampleCmpLevelZero(samplerName, float3((coord3).xy, index), (coord3).z)
	#define SAMPLE_TEXTURECUBE_SHADOW(textureName, samplerName, coord4)                  textureName.SampleCmpLevelZero(samplerName, (coord4).xyz, (coord4).w)
	#define SAMPLE_TEXTURECUBE_ARRAY_SHADOW(textureName, samplerName, coord4, index)     textureName.SampleCmpLevelZero(samplerName, float4((coord4).xyz, index), (coord4).w)

	#define LOAD_TEXTURE2D(textureName, unCoord2)                                   textureName.Load(int3(unCoord2, 0))
	#define LOAD_TEXTURE2D_LOD(textureName, unCoord2, lod)                          textureName.Load(int3(unCoord2, lod))
	#define LOAD_TEXTURE2D_MSAA(textureName, unCoord2, sampleIndex)                 textureName.Load(unCoord2, sampleIndex)
	#define LOAD_TEXTURE2D_ARRAY(textureName, unCoord2, index)                      textureName.Load(int4(unCoord2, index, 0))
	#define LOAD_TEXTURE2D_ARRAY_MSAA(textureName, unCoord2, index, sampleIndex)    textureName.Load(int3(unCoord2, index), sampleIndex)
	#define LOAD_TEXTURE2D_ARRAY_LOD(textureName, unCoord2, index, lod)             textureName.Load(int4(unCoord2, index, lod))
	#define LOAD_TEXTURE3D(textureName, unCoord3)                                   textureName.Load(int4(unCoord3, 0))
	#define LOAD_TEXTURE3D_LOD(textureName, unCoord3, lod)                          textureName.Load(int4(unCoord3, lod))

	#define PLATFORM_SUPPORT_GATHER
	#define GATHER_TEXTURE2D(textureName, samplerName, coord2)                textureName.Gather(samplerName, coord2)
	#define GATHER_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)   textureName.Gather(samplerName, float3(coord2, index))
	#define GATHER_TEXTURECUBE(textureName, samplerName, coord3)              textureName.Gather(samplerName, coord3)
	#define GATHER_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index) textureName.Gather(samplerName, float4(coord3, index))
	#define GATHER_RED_TEXTURE2D(textureName, samplerName, coord2)            textureName.GatherRed(samplerName, coord2)
	#define GATHER_GREEN_TEXTURE2D(textureName, samplerName, coord2)          textureName.GatherGreen(samplerName, coord2)
	#define GATHER_BLUE_TEXTURE2D(textureName, samplerName, coord2)           textureName.GatherBlue(samplerName, coord2)
	#define GATHER_ALPHA_TEXTURE2D(textureName, samplerName, coord2)          textureName.GatherAlpha(samplerName, coord2)

#elif defined(SHADER_API_SWITCH)
	#define ZERO_INITIALIZE(type, name) name = (type)0;
	#define ZERO_INITIALIZE_ARRAY(type, name, arraySize) { for (int arrayIndex = 0; arrayIndex < arraySize; arrayIndex++) { name[arrayIndex] = (type)0; } }
	#define CALCULATE_TEXTURE2D_LOD(textureName, samplerName, coord2) textureName.CalculateLevelOfDetail(samplerName, coord2)
	#define TEXTURE2D(textureName)                Texture2D textureName
	#define TEXTURE2D_ARRAY(textureName)          Texture2DArray textureName
	#define TEXTURECUBE(textureName)              TextureCube textureName
	#define TEXTURECUBE_ARRAY(textureName)        TextureCubeArray textureName
	#define TEXTURE3D(textureName)                Texture3D textureName

	#define TEXTURE2D_FLOAT(textureName)          Texture2D_float textureName
	#define TEXTURE2D_ARRAY_FLOAT(textureName)    Texture2DArray textureName    
	#define TEXTURECUBE_FLOAT(textureName)        TextureCube_float textureName
	#define TEXTURECUBE_ARRAY_FLOAT(textureName)  TextureCubeArray textureName  
	#define TEXTURE3D_FLOAT(textureName)          Texture3D_float textureName

	#define TEXTURE2D_HALF(textureName)           Texture2D_half textureName
	#define TEXTURE2D_ARRAY_HALF(textureName)     Texture2DArray textureName    
	#define TEXTURECUBE_HALF(textureName)         TextureCube_half textureName
	#define TEXTURECUBE_ARRAY_HALF(textureName)   TextureCubeArray textureName  
	#define TEXTURE3D_HALF(textureName)           Texture3D_half textureName

	#define TEXTURE2D_SHADOW(textureName)         TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_SHADOW(textureName)   TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_SHADOW(textureName)       TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_SHADOW(textureName) TEXTURECUBE_ARRAY(textureName)

	#define RW_TEXTURE2D(type, textureName)       RWTexture2D<type> textureName
	#define RW_TEXTURE2D_ARRAY(type, textureName) RWTexture2DArray<type> textureName
	#define RW_TEXTURE3D(type, textureName)       RWTexture3D<type> textureName

	#define SAMPLER(samplerName)                  SamplerState samplerName
	#define SAMPLER_CMP(samplerName)              SamplerComparisonState samplerName

	#define TEXTURE2D_PARAM(textureName, samplerName)                 TEXTURE2D(textureName),         SAMPLER(samplerName)
	#define TEXTURE2D_ARRAY_PARAM(textureName, samplerName)           TEXTURE2D_ARRAY(textureName),   SAMPLER(samplerName)
	#define TEXTURECUBE_PARAM(textureName, samplerName)               TEXTURECUBE(textureName),       SAMPLER(samplerName)
	#define TEXTURECUBE_ARRAY_PARAM(textureName, samplerName)         TEXTURECUBE_ARRAY(textureName), SAMPLER(samplerName)
	#define TEXTURE3D_PARAM(textureName, samplerName)                 TEXTURE3D(textureName),         SAMPLER(samplerName)

	#define TEXTURE2D_SHADOW_PARAM(textureName, samplerName)          TEXTURE2D(textureName),         SAMPLER_CMP(samplerName)
	#define TEXTURE2D_ARRAY_SHADOW_PARAM(textureName, samplerName)    TEXTURE2D_ARRAY(textureName),   SAMPLER_CMP(samplerName)
	#define TEXTURECUBE_SHADOW_PARAM(textureName, samplerName)        TEXTURECUBE(textureName),       SAMPLER_CMP(samplerName)
	#define TEXTURECUBE_ARRAY_SHADOW_PARAM(textureName, samplerName)  TEXTURECUBE_ARRAY(textureName), SAMPLER_CMP(samplerName)

	#define TEXTURE2D_ARGS(textureName, samplerName)                textureName, samplerName
	#define TEXTURE2D_ARRAY_ARGS(textureName, samplerName)          textureName, samplerName
	#define TEXTURECUBE_ARGS(textureName, samplerName)              textureName, samplerName
	#define TEXTURECUBE_ARRAY_ARGS(textureName, samplerName)        textureName, samplerName
	#define TEXTURE3D_ARGS(textureName, samplerName)                textureName, samplerName

	#define TEXTURE2D_SHADOW_ARGS(textureName, samplerName)         textureName, samplerName
	#define TEXTURE2D_ARRAY_SHADOW_ARGS(textureName, samplerName)   textureName, samplerName
	#define TEXTURECUBE_SHADOW_ARGS(textureName, samplerName)       textureName, samplerName
	#define TEXTURECUBE_ARRAY_SHADOW_ARGS(textureName, samplerName) textureName, samplerName

	#define SAMPLE_TEXTURE2D(textureName, samplerName, coord2)                               textureName.Sample(samplerName, coord2)
	#define SAMPLE_TEXTURE2D_LOD(textureName, samplerName, coord2, lod)                      textureName.SampleLevel(samplerName, coord2, lod)
	#define SAMPLE_TEXTURE2D_BIAS(textureName, samplerName, coord2, bias)                    textureName.SampleBias(samplerName, coord2, bias)
	#define SAMPLE_TEXTURE2D_GRAD(textureName, samplerName, coord2, dpdx, dpdy)              textureName.SampleGrad(samplerName, coord2, dpdx, dpdy)
	#define SAMPLE_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)                  textureName.Sample(samplerName, float3(coord2, index))
	#define SAMPLE_TEXTURE2D_ARRAY_LOD(textureName, samplerName, coord2, index, lod)         textureName.SampleLevel(samplerName, float3(coord2, index), lod)
	#define SAMPLE_TEXTURE2D_ARRAY_BIAS(textureName, samplerName, coord2, index, bias)       textureName.SampleBias(samplerName, float3(coord2, index), bias)
	#define SAMPLE_TEXTURE2D_ARRAY_GRAD(textureName, samplerName, coord2, index, dpdx, dpdy) textureName.SampleGrad(samplerName, float3(coord2, index), dpdx, dpdy)
	#define SAMPLE_TEXTURECUBE(textureName, samplerName, coord3)                             textureName.Sample(samplerName, coord3)
	#define SAMPLE_TEXTURECUBE_LOD(textureName, samplerName, coord3, lod)                    textureName.SampleLevel(samplerName, coord3, lod)
	#define SAMPLE_TEXTURECUBE_BIAS(textureName, samplerName, coord3, bias)                  textureName.SampleBias(samplerName, coord3, bias)
	#define SAMPLE_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)                textureName.Sample(samplerName, float4(coord3, index))
	#define SAMPLE_TEXTURECUBE_ARRAY_LOD(textureName, samplerName, coord3, index, lod)       textureName.SampleLevel(samplerName, float4(coord3, index), lod)
	#define SAMPLE_TEXTURECUBE_ARRAY_BIAS(textureName, samplerName, coord3, index, bias)     textureName.SampleBias(samplerName, float4(coord3, index), bias)
	#define SAMPLE_TEXTURE3D(textureName, samplerName, coord3)                               textureName.Sample(samplerName, coord3)
	#define SAMPLE_TEXTURE3D_LOD(textureName, samplerName, coord3, lod)                      textureName.SampleLevel(samplerName, coord3, lod)

	#define SAMPLE_TEXTURE2D_SHADOW(textureName, samplerName, coord3)                    textureName.SampleCmpLevelZero(samplerName, (coord3).xy, (coord3).z)
	#define SAMPLE_TEXTURE2D_ARRAY_SHADOW(textureName, samplerName, coord3, index)       textureName.SampleCmpLevelZero(samplerName, float3((coord3).xy, index), (coord3).z)
	#define SAMPLE_TEXTURECUBE_SHADOW(textureName, samplerName, coord4)                  textureName.SampleCmpLevelZero(samplerName, (coord4).xyz, (coord4).w)
	#define SAMPLE_TEXTURECUBE_ARRAY_SHADOW(textureName, samplerName, coord4, index)     textureName.SampleCmpLevelZero(samplerName, float4((coord4).xyz, index), (coord4).w)
	#define LOAD_TEXTURE2D(textureName, unCoord2)                       textureName.Load(int3(unCoord2, 0))
	#define LOAD_TEXTURE2D_LOD(textureName, unCoord2, lod)              textureName.Load(int3(unCoord2, lod))
	#define LOAD_TEXTURE2D_MSAA(textureName, unCoord2, sampleIndex)     textureName.Load(unCoord2, sampleIndex)
	#define LOAD_TEXTURE2D_ARRAY(textureName, unCoord2, index)          textureName.Load(int4(unCoord2, index, 0))
	#define LOAD_TEXTURE2D_ARRAY_LOD(textureName, unCoord2, index, lod) textureName.Load(int4(unCoord2, index, lod))
	#define LOAD_TEXTURE3D(textureName, unCoord3)                       textureName.Load(int4(unCoord3, 0))
	#define LOAD_TEXTURE3D_LOD(textureName, unCoord3, lod)              textureName.Load(int4(unCoord3, lod))

	#define PLATFORM_SUPPORT_GATHER
	#define GATHER_TEXTURE2D(textureName, samplerName, coord2)                textureName.Gather(samplerName, coord2)
	#define GATHER_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)   textureName.Gather(samplerName, float3(coord2, index))
	#define GATHER_TEXTURECUBE(textureName, samplerName, coord3)              textureName.Gather(samplerName, coord3)
	#define GATHER_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index) textureName.Gather(samplerName, float4(coord3, index))
	#define GATHER_RED_TEXTURE2D(textureName, samplerName, coord2)            textureName.GatherRed(samplerName, coord2)
	#define GATHER_GREEN_TEXTURE2D(textureName, samplerName, coord2)          textureName.GatherGreen(samplerName, coord2)
	#define GATHER_BLUE_TEXTURE2D(textureName, samplerName, coord2)           textureName.GatherBlue(samplerName, coord2)
	#define GATHER_ALPHA_TEXTURE2D(textureName, samplerName, coord2)          textureName.GatherAlpha(samplerName, coord2)

#elif defined(SHADER_API_GLCORE)
	#if (SHADER_TARGET >= 46)
	#define OPENGL4_1_SM5 1
	#else
	#define OPENGL4_1_SM5 0
	#endif
	#define ZERO_INITIALIZE(type, name) name = (type)0;
	#define ZERO_INITIALIZE_ARRAY(type, name, arraySize) { for (int arrayIndex = 0; arrayIndex < arraySize; arrayIndex++) { name[arrayIndex] = (type)0; } }
	#define CALCULATE_TEXTURE2D_LOD(textureName, samplerName, coord2) textureName.CalculateLevelOfDetail(samplerName, coord2)
	#define TEXTURE2D(textureName)                  Texture2D textureName
	#define TEXTURE2D_ARRAY(textureName)            Texture2DArray textureName
	#define TEXTURECUBE(textureName)                TextureCube textureName
	#define TEXTURECUBE_ARRAY(textureName)          TextureCubeArray textureName
	#define TEXTURE3D(textureName)                  Texture3D textureName

	#define TEXTURE2D_FLOAT(textureName)            TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_FLOAT(textureName)      TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_FLOAT(textureName)          TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_FLOAT(textureName)    TEXTURECUBE_ARRAY(textureName)
	#define TEXTURE3D_FLOAT(textureName)            TEXTURE3D(textureName)

	#define TEXTURE2D_HALF(textureName)             TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_HALF(textureName)       TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_HALF(textureName)           TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_HALF(textureName)     TEXTURECUBE_ARRAY(textureName)
	#define TEXTURE3D_HALF(textureName)             TEXTURE3D(textureName)

	#define TEXTURE2D_SHADOW(textureName)           TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_SHADOW(textureName)     TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_SHADOW(textureName)         TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_SHADOW(textureName)   TEXTURECUBE_ARRAY(textureName)

	#define RW_TEXTURE2D(type, textureName)         RWTexture2D<type> textureName
	#define RW_TEXTURE2D_ARRAY(type, textureName)   RWTexture2DArray<type> textureName
	#define RW_TEXTURE3D(type, textureName)         RWTexture3D<type> textureName

	#define SAMPLER(samplerName)                    SamplerState samplerName
	#define SAMPLER_CMP(samplerName)                SamplerComparisonState samplerName

	#define TEXTURE2D_PARAM(textureName, samplerName)                 TEXTURE2D(textureName),         SAMPLER(samplerName)
	#define TEXTURE2D_ARRAY_PARAM(textureName, samplerName)           TEXTURE2D_ARRAY(textureName),   SAMPLER(samplerName)
	#define TEXTURECUBE_PARAM(textureName, samplerName)               TEXTURECUBE(textureName),       SAMPLER(samplerName)
	#define TEXTURECUBE_ARRAY_PARAM(textureName, samplerName)         TEXTURECUBE_ARRAY(textureName), SAMPLER(samplerName)
	#define TEXTURE3D_PARAM(textureName, samplerName)                 TEXTURE3D(textureName),         SAMPLER(samplerName)

	#define TEXTURE2D_SHADOW_PARAM(textureName, samplerName)          TEXTURE2D(textureName),         SAMPLER_CMP(samplerName)
	#define TEXTURE2D_ARRAY_SHADOW_PARAM(textureName, samplerName)    TEXTURE2D_ARRAY(textureName),   SAMPLER_CMP(samplerName)
	#define TEXTURECUBE_SHADOW_PARAM(textureName, samplerName)        TEXTURECUBE(textureName),       SAMPLER_CMP(samplerName)
	#define TEXTURECUBE_ARRAY_SHADOW_PARAM(textureName, samplerName)  TEXTURECUBE_ARRAY(textureName), SAMPLER_CMP(samplerName)

	#define TEXTURE2D_ARGS(textureName, samplerName)                textureName, samplerName
	#define TEXTURE2D_ARRAY_ARGS(textureName, samplerName)          textureName, samplerName
	#define TEXTURECUBE_ARGS(textureName, samplerName)              textureName, samplerName
	#define TEXTURECUBE_ARRAY_ARGS(textureName, samplerName)        textureName, samplerName
	#define TEXTURE3D_ARGS(textureName, samplerName)                textureName, samplerName

	#define TEXTURE2D_SHADOW_ARGS(textureName, samplerName)         textureName, samplerName
	#define TEXTURE2D_ARRAY_SHADOW_ARGS(textureName, samplerName)   textureName, samplerName
	#define TEXTURECUBE_SHADOW_ARGS(textureName, samplerName)       textureName, samplerName
	#define TEXTURECUBE_ARRAY_SHADOW_ARGS(textureName, samplerName) textureName, samplerName

	#define SAMPLE_TEXTURE2D(textureName, samplerName, coord2)                               textureName.Sample(samplerName, coord2)
	#define SAMPLE_TEXTURE2D_LOD(textureName, samplerName, coord2, lod)                      textureName.SampleLevel(samplerName, coord2, lod)
	#define SAMPLE_TEXTURE2D_BIAS(textureName, samplerName, coord2, bias)                    textureName.SampleBias(samplerName, coord2, bias)
	#define SAMPLE_TEXTURE2D_GRAD(textureName, samplerName, coord2, ddx, ddy)                textureName.SampleGrad(samplerName, coord2, ddx, ddy)
	#define SAMPLE_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)                  textureName.Sample(samplerName, float3(coord2, index))
	#define SAMPLE_TEXTURE2D_ARRAY_LOD(textureName, samplerName, coord2, index, lod)         textureName.SampleLevel(samplerName, float3(coord2, index), lod)
	#define SAMPLE_TEXTURE2D_ARRAY_BIAS(textureName, samplerName, coord2, index, bias)       textureName.SampleBias(samplerName, float3(coord2, index), bias)
	#define SAMPLE_TEXTURE2D_ARRAY_GRAD(textureName, samplerName, coord2, index, dpdx, dpdy) textureName.SampleGrad(samplerName, float3(coord2, index), dpdx, dpdy)
	#define SAMPLE_TEXTURECUBE(textureName, samplerName, coord3)                             textureName.Sample(samplerName, coord3)
	#define SAMPLE_TEXTURECUBE_LOD(textureName, samplerName, coord3, lod)                    textureName.SampleLevel(samplerName, coord3, lod)
	#define SAMPLE_TEXTURECUBE_BIAS(textureName, samplerName, coord3, bias)                  textureName.SampleBias(samplerName, coord3, bias)
	#ifdef UNITY_NO_CUBEMAP_ARRAY
	#define SAMPLE_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)           ERROR_ON_UNSUPPORTED_FUNCTION(SAMPLE_TEXTURECUBE_ARRAY)
	#define SAMPLE_TEXTURECUBE_ARRAY_LOD(textureName, samplerName, coord3, index, lod)  ERROR_ON_UNSUPPORTED_FUNCTION(SAMPLE_TEXTURECUBE_ARRAY_LOD)
	#define SAMPLE_TEXTURECUBE_ARRAY_LOD(textureName, samplerName, coord3, index, bias) ERROR_ON_UNSUPPORTED_FUNCTION(SAMPLE_TEXTURECUBE_ARRAY_LOD)
	#else
	#define SAMPLE_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)           textureName.Sample(samplerName, float4(coord3, index))
	#define SAMPLE_TEXTURECUBE_ARRAY_LOD(textureName, samplerName, coord3, index, lod)  textureName.SampleLevel(samplerName, float4(coord3, index), lod)
	#define SAMPLE_TEXTURECUBE_ARRAY_BIAS(textureName, samplerName, coord3, index, bias)textureName.SampleBias(samplerName, float4(coord3, index), bias)
	#endif
	#define SAMPLE_TEXTURE3D(textureName, samplerName, coord3)                          textureName.Sample(samplerName, coord3)
	#define SAMPLE_TEXTURE3D_LOD(textureName, samplerName, coord3, lod)                 textureName.SampleLevel(samplerName, coord3, lod)

	#define SAMPLE_TEXTURE2D_SHADOW(textureName, samplerName, coord3)                   textureName.SampleCmpLevelZero(samplerName, (coord3).xy, (coord3).z)
	#define SAMPLE_TEXTURE2D_ARRAY_SHADOW(textureName, samplerName, coord3, index)      textureName.SampleCmpLevelZero(samplerName, float3((coord3).xy, index), (coord3).z)
	#define SAMPLE_TEXTURECUBE_SHADOW(textureName, samplerName, coord4)                 textureName.SampleCmpLevelZero(samplerName, (coord4).xyz, (coord4).w)
	#define SAMPLE_TEXTURECUBE_ARRAY_SHADOW(textureName, samplerName, coord4, index)    textureName.SampleCmpLevelZero(samplerName, float4((coord4).xyz, index), (coord4).w)

	#define LOAD_TEXTURE2D(textureName, unCoord2)                                   textureName.Load(int3(unCoord2, 0))
	#define LOAD_TEXTURE2D_LOD(textureName, unCoord2, lod)                          textureName.Load(int3(unCoord2, lod))
	#define LOAD_TEXTURE2D_MSAA(textureName, unCoord2, sampleIndex)                 textureName.Load(unCoord2, sampleIndex)
	#define LOAD_TEXTURE2D_ARRAY(textureName, unCoord2, index)                      textureName.Load(int4(unCoord2, index, 0))
	#define LOAD_TEXTURE2D_ARRAY_MSAA(textureName, unCoord2, index, sampleIndex)    textureName.Load(int3(unCoord2, index), sampleIndex)
	#define LOAD_TEXTURE2D_ARRAY_LOD(textureName, unCoord2, index, lod)             textureName.Load(int4(unCoord2, index, lod))

	#if OPENGL4_1_SM5
	#define PLATFORM_SUPPORT_GATHER
	#define GATHER_TEXTURE2D(textureName, samplerName, coord2)                  textureName.Gather(samplerName, coord2)
	#define GATHER_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)     textureName.Gather(samplerName, float3(coord2, index))
	#define GATHER_TEXTURECUBE(textureName, samplerName, coord3)                textureName.Gather(samplerName, coord3)
	#define GATHER_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)   textureName.Gather(samplerName, float4(coord3, index))
	#else
	#define GATHER_TEXTURE2D(textureName, samplerName, coord2)                  ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_TEXTURE2D)
	#define GATHER_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)     ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_TEXTURE2D_ARRAY)
	#define GATHER_TEXTURECUBE(textureName, samplerName, coord3)                ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_TEXTURECUBE)
	#define GATHER_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)   ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_TEXTURECUBE_ARRAY)
	#endif
	#elif defined(SHADER_API_GLES3)
	#if (SHADER_TARGET >= 40)
	#define GLES3_1_AEP 1
	#else
	#define GLES3_1_AEP 0
	#endif
	#define ZERO_INITIALIZE(type, name) name = (type)0;
	#define ZERO_INITIALIZE_ARRAY(type, name, arraySize) { for (int arrayIndex = 0; arrayIndex < arraySize; arrayIndex++) { name[arrayIndex] = (type)0; } }
	#define CALCULATE_TEXTURE2D_LOD(textureName, samplerName, coord2) textureName.CalculateLevelOfDetail(samplerName, coord2)
	#define TEXTURE2D(textureName)                  Texture2D textureName
	#define TEXTURE2D_ARRAY(textureName)            Texture2DArray textureName
	#define TEXTURECUBE(textureName)                TextureCube textureName
	#define TEXTURECUBE_ARRAY(textureName)          TextureCubeArray textureName
	#define TEXTURE3D(textureName)                  Texture3D textureName

	#define TEXTURE2D_FLOAT(textureName)            Texture2D_float textureName
	#define TEXTURE2D_ARRAY_FLOAT(textureName)      Texture2DArray textureName    
	#define TEXTURECUBE_FLOAT(textureName)          TextureCube_float textureName
	#define TEXTURECUBE_ARRAY_FLOAT(textureName)    TextureCubeArray textureName  
	#define TEXTURE3D_FLOAT(textureName)            Texture3D_float textureName

	#define TEXTURE2D_HALF(textureName)             Texture2D_half textureName
	#define TEXTURE2D_ARRAY_HALF(textureName)       Texture2DArray textureName    
	#define TEXTURECUBE_HALF(textureName)           TextureCube_half textureName
	#define TEXTURECUBE_ARRAY_HALF(textureName)     TextureCubeArray textureName  
	#define TEXTURE3D_HALF(textureName)             Texture3D_half textureName

	#define TEXTURE2D_SHADOW(textureName)           TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_SHADOW(textureName)     TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_SHADOW(textureName)         TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_SHADOW(textureName)   TEXTURECUBE_ARRAY(textureName)

	#if GLES3_1_AEP
	#define RW_TEXTURE2D(type, textureName)         RWTexture2D<type> textureName
	#define RW_TEXTURE2D_ARRAY(type, textureName)   RWTexture2DArray<type> textureName
	#define RW_TEXTURE3D(type, textureName)         RWTexture3D<type> textureName
	#else
	#define RW_TEXTURE2D(type, textureName)         ERROR_ON_UNSUPPORTED_FUNCTION(RWTexture2D)
	#define RW_TEXTURE2D_ARRAY(type, textureName)   ERROR_ON_UNSUPPORTED_FUNCTION(RWTexture2DArray)
	#define RW_TEXTURE3D(type, textureName)         ERROR_ON_UNSUPPORTED_FUNCTION(RWTexture3D)
	#endif

	#define SAMPLER(samplerName)                    SamplerState samplerName
	#define SAMPLER_CMP(samplerName)                SamplerComparisonState samplerName

	#define TEXTURE2D_PARAM(textureName, samplerName)                 TEXTURE2D(textureName),         SAMPLER(samplerName)
	#define TEXTURE2D_ARRAY_PARAM(textureName, samplerName)           TEXTURE2D_ARRAY(textureName),   SAMPLER(samplerName)
	#define TEXTURECUBE_PARAM(textureName, samplerName)               TEXTURECUBE(textureName),       SAMPLER(samplerName)
	#define TEXTURECUBE_ARRAY_PARAM(textureName, samplerName)         TEXTURECUBE_ARRAY(textureName), SAMPLER(samplerName)
	#define TEXTURE3D_PARAM(textureName, samplerName)                 TEXTURE3D(textureName),         SAMPLER(samplerName)

	#define TEXTURE2D_SHADOW_PARAM(textureName, samplerName)          TEXTURE2D(textureName),         SAMPLER_CMP(samplerName)
	#define TEXTURE2D_ARRAY_SHADOW_PARAM(textureName, samplerName)    TEXTURE2D_ARRAY(textureName),   SAMPLER_CMP(samplerName)
	#define TEXTURECUBE_SHADOW_PARAM(textureName, samplerName)        TEXTURECUBE(textureName),       SAMPLER_CMP(samplerName)
	#define TEXTURECUBE_ARRAY_SHADOW_PARAM(textureName, samplerName)  TEXTURECUBE_ARRAY(textureName), SAMPLER_CMP(samplerName)

	#define TEXTURE2D_ARGS(textureName, samplerName)                textureName, samplerName
	#define TEXTURE2D_ARRAY_ARGS(textureName, samplerName)          textureName, samplerName
	#define TEXTURECUBE_ARGS(textureName, samplerName)              textureName, samplerName
	#define TEXTURECUBE_ARRAY_ARGS(textureName, samplerName)        textureName, samplerName
	#define TEXTURE3D_ARGS(textureName, samplerName)                textureName, samplerName

	#define TEXTURE2D_SHADOW_ARGS(textureName, samplerName)         textureName, samplerName
	#define TEXTURE2D_ARRAY_SHADOW_ARGS(textureName, samplerName)   textureName, samplerName
	#define TEXTURECUBE_SHADOW_ARGS(textureName, samplerName)       textureName, samplerName
	#define TEXTURECUBE_ARRAY_SHADOW_ARGS(textureName, samplerName) textureName, samplerName

	#define SAMPLE_TEXTURE2D(textureName, samplerName, coord2)                               textureName.Sample(samplerName, coord2)
	#define SAMPLE_TEXTURE2D_LOD(textureName, samplerName, coord2, lod)                      textureName.SampleLevel(samplerName, coord2, lod)
	#define SAMPLE_TEXTURE2D_BIAS(textureName, samplerName, coord2, bias)                    textureName.SampleBias(samplerName, coord2, bias)
	#define SAMPLE_TEXTURE2D_GRAD(textureName, samplerName, coord2, ddx, ddy)                textureName.SampleGrad(samplerName, coord2, ddx, ddy)
	#define SAMPLE_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)                  textureName.Sample(samplerName, float3(coord2, index))
	#define SAMPLE_TEXTURE2D_ARRAY_LOD(textureName, samplerName, coord2, index, lod)         textureName.SampleLevel(samplerName, float3(coord2, index), lod)
	#define SAMPLE_TEXTURE2D_ARRAY_BIAS(textureName, samplerName, coord2, index, bias)       textureName.SampleBias(samplerName, float3(coord2, index), bias)
	#define SAMPLE_TEXTURE2D_ARRAY_GRAD(textureName, samplerName, coord2, index, dpdx, dpdy) textureName.SampleGrad(samplerName, float3(coord2, index), dpdx, dpdy)
	#define SAMPLE_TEXTURECUBE(textureName, samplerName, coord3)                             textureName.Sample(samplerName, coord3)
	#define SAMPLE_TEXTURECUBE_LOD(textureName, samplerName, coord3, lod)                    textureName.SampleLevel(samplerName, coord3, lod)
	#define SAMPLE_TEXTURECUBE_BIAS(textureName, samplerName, coord3, bias)                  textureName.SampleBias(samplerName, coord3, bias)

	#ifdef UNITY_NO_CUBEMAP_ARRAY
	#define SAMPLE_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)           ERROR_ON_UNSUPPORTED_FUNCTION(SAMPLE_TEXTURECUBE_ARRAY)
	#define SAMPLE_TEXTURECUBE_ARRAY_LOD(textureName, samplerName, coord3, index, lod)  ERROR_ON_UNSUPPORTED_FUNCTION(SAMPLE_TEXTURECUBE_ARRAY_LOD)
	#define SAMPLE_TEXTURECUBE_ARRAY_BIAS(textureName, samplerName, coord3, index, bias)ERROR_ON_UNSUPPORTED_FUNCTION(SAMPLE_TEXTURECUBE_ARRAY_BIAS)
	#else
	#define SAMPLE_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)           textureName.Sample(samplerName, float4(coord3, index))
	#define SAMPLE_TEXTURECUBE_ARRAY_LOD(textureName, samplerName, coord3, index, lod)  textureName.SampleLevel(samplerName, float4(coord3, index), lod)
	#define SAMPLE_TEXTURECUBE_ARRAY_BIAS(textureName, samplerName, coord3, index, bias)textureName.SampleBias(samplerName, float4(coord3, index), bias)
	#endif

	#define SAMPLE_TEXTURE3D(textureName, samplerName, coord3)                          textureName.Sample(samplerName, coord3)
	#define SAMPLE_TEXTURE3D_LOD(textureName, samplerName, coord3, lod)                 textureName.SampleLevel(samplerName, coord3, lod)

	#define SAMPLE_TEXTURE2D_SHADOW(textureName, samplerName, coord3)                   textureName.SampleCmpLevelZero(samplerName, (coord3).xy, (coord3).z)
	#define SAMPLE_TEXTURE2D_ARRAY_SHADOW(textureName, samplerName, coord3, index)      textureName.SampleCmpLevelZero(samplerName, float3((coord3).xy, index), (coord3).z)
	#define SAMPLE_TEXTURECUBE_SHADOW(textureName, samplerName, coord4)                 textureName.SampleCmpLevelZero(samplerName, (coord4).xyz, (coord4).w)
	#define SAMPLE_TEXTURECUBE_ARRAY_SHADOW(textureName, samplerName, coord4, index)    textureName.SampleCmpLevelZero(samplerName, float4((coord4).xyz, index), (coord4).w)
	#define LOAD_TEXTURE2D(textureName, unCoord2)                                       textureName.Load(int3(unCoord2, 0))
	#define LOAD_TEXTURE2D_LOD(textureName, unCoord2, lod)                              textureName.Load(int3(unCoord2, lod))
	#define LOAD_TEXTURE2D_MSAA(textureName, unCoord2, sampleIndex)                     textureName.Load(unCoord2, sampleIndex)
	#define LOAD_TEXTURE2D_ARRAY(textureName, unCoord2, index)                          textureName.Load(int4(unCoord2, index, 0))
	#define LOAD_TEXTURE2D_ARRAY_MSAA(textureName, unCoord2, index, sampleIndex)        textureName.Load(int3(unCoord2, index), sampleIndex)
	#define LOAD_TEXTURE2D_ARRAY_LOD(textureName, unCoord2, index, lod)                 textureName.Load(int4(unCoord2, index, lod))
	#define LOAD_TEXTURE3D(textureName, unCoord3)                                       textureName.Load(int4(unCoord3, 0))
	#define LOAD_TEXTURE3D_LOD(textureName, unCoord3, lod)                              textureName.Load(int4(unCoord3, lod))

	#if GLES3_1_AEP
	#define PLATFORM_SUPPORT_GATHER
	#define GATHER_TEXTURE2D(textureName, samplerName, coord2)                  textureName.Gather(samplerName, coord2)
	#define GATHER_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)     textureName.Gather(samplerName, float3(coord2, index))
	#define GATHER_TEXTURECUBE(textureName, samplerName, coord3)                textureName.Gather(samplerName, coord3)
	#define GATHER_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)   textureName.Gather(samplerName, float4(coord3, index))
	#define GATHER_RED_TEXTURE2D(textureName, samplerName, coord2)              textureName.GatherRed(samplerName, coord2)
	#define GATHER_GREEN_TEXTURE2D(textureName, samplerName, coord2)            textureName.GatherGreen(samplerName, coord2)
	#define GATHER_BLUE_TEXTURE2D(textureName, samplerName, coord2)             textureName.GatherBlue(samplerName, coord2)
	#define GATHER_ALPHA_TEXTURE2D(textureName, samplerName, coord2)            textureName.GatherAlpha(samplerName, coord2)
	#else
	#define GATHER_TEXTURE2D(textureName, samplerName, coord2)                  ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_TEXTURE2D)
	#define GATHER_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)     ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_TEXTURE2D_ARRAY)
	#define GATHER_TEXTURECUBE(textureName, samplerName, coord3)                ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_TEXTURECUBE)
	#define GATHER_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)   ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_TEXTURECUBE_ARRAY)
	#define GATHER_RED_TEXTURE2D(textureName, samplerName, coord2)              ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_RED_TEXTURE2D)
	#define GATHER_GREEN_TEXTURE2D(textureName, samplerName, coord2)            ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_GREEN_TEXTURE2D)
	#define GATHER_BLUE_TEXTURE2D(textureName, samplerName, coord2)             ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_BLUE_TEXTURE2D)
	#define GATHER_ALPHA_TEXTURE2D(textureName, samplerName, coord2)            ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_ALPHA_TEXTURE2D)
	#endif
#elif defined(SHADER_API_GLES)
	#define uint int

	#define rcp(x) 1.0 / (x)
	#define ddx_fine ddx
	#define ddy_fine ddy
	#define asfloat
	#define asuint(x) asint(x)
	#define f32tof16
	#define f16tof32

	#define ERROR_ON_UNSUPPORTED_FUNCTION(funcName) #error #funcName is not supported on GLES 2.0
	#define ZERO_INITIALIZE(type, name) name = (type)0;
	#define ZERO_INITIALIZE_ARRAY(type, name, arraySize) { for (int arrayIndex = 0; arrayIndex < arraySize; arrayIndex++) { name[arrayIndex] = (type)0; } }
	#define CALCULATE_TEXTURE2D_LOD(textureName, samplerName, coord2) #error calculate Level of Detail not supported in GLES2
	#define TEXTURE2D(textureName)                          sampler2D textureName
	#define TEXTURE2D_ARRAY(textureName)                    samplerCUBE textureName 
	#define TEXTURECUBE(textureName)                        samplerCUBE textureName
	#define TEXTURECUBE_ARRAY(textureName)                  samplerCUBE textureName 
	#define TEXTURE3D(textureName)                          sampler3D textureName

	#define TEXTURE2D_FLOAT(textureName)                    sampler2D_float textureName
	#define TEXTURE2D_ARRAY_FLOAT(textureName)              TEXTURECUBE_FLOAT(textureName) 
	#define TEXTURECUBE_FLOAT(textureName)                  samplerCUBE_float textureName
	#define TEXTURECUBE_ARRAY_FLOAT(textureName)            TEXTURECUBE_FLOAT(textureName) 
	#define TEXTURE3D_FLOAT(textureName)                    sampler3D_float textureName

	#define TEXTURE2D_HALF(textureName)                     sampler2D_half textureName
	#define TEXTURE2D_ARRAY_HALF(textureName)               TEXTURECUBE_HALF(textureName) 
	#define TEXTURECUBE_HALF(textureName)                   samplerCUBE_half textureName
	#define TEXTURECUBE_ARRAY_HALF(textureName)             TEXTURECUBE_HALF(textureName) 
	#define TEXTURE3D_HALF(textureName)                     sampler3D_half textureName

	#define TEXTURE2D_SHADOW(textureName)                   SHADOW2D_TEXTURE_AND_SAMPLER textureName
	#define TEXTURE2D_ARRAY_SHADOW(textureName)             TEXTURECUBE_SHADOW(textureName) 
	#define TEXTURECUBE_SHADOW(textureName)                 SHADOWCUBE_TEXTURE_AND_SAMPLER textureName
	#define TEXTURECUBE_ARRAY_SHADOW(textureName)           TEXTURECUBE_SHADOW(textureName) 

	#define RW_TEXTURE2D(type, textureNam)                  ERROR_ON_UNSUPPORTED_FUNCTION(RWTexture2D)
	#define RW_TEXTURE2D_ARRAY(type, textureName)           ERROR_ON_UNSUPPORTED_FUNCTION(RWTexture2DArray)
	#define RW_TEXTURE3D(type, textureNam)                  ERROR_ON_UNSUPPORTED_FUNCTION(RWTexture3D)

	#define SAMPLER(samplerName)
	#define SAMPLER_CMP(samplerName)

	#define TEXTURE2D_PARAM(textureName, samplerName)                sampler2D textureName
	#define TEXTURE2D_ARRAY_PARAM(textureName, samplerName)          samplerCUBE textureName
	#define TEXTURECUBE_PARAM(textureName, samplerName)              samplerCUBE textureName
	#define TEXTURECUBE_ARRAY_PARAM(textureName, samplerName)        samplerCUBE textureName
	#define TEXTURE3D_PARAM(textureName, samplerName)                sampler3D textureName
	#define TEXTURE2D_SHADOW_PARAM(textureName, samplerName)         SHADOW2D_TEXTURE_AND_SAMPLER textureName
	#define TEXTURE2D_ARRAY_SHADOW_PARAM(textureName, samplerName)   SHADOWCUBE_TEXTURE_AND_SAMPLER textureName
	#define TEXTURECUBE_SHADOW_PARAM(textureName, samplerName)       SHADOWCUBE_TEXTURE_AND_SAMPLER textureName

	#define TEXTURE2D_ARGS(textureName, samplerName)               textureName
	#define TEXTURE2D_ARRAY_ARGS(textureName, samplerName)         textureName
	#define TEXTURECUBE_ARGS(textureName, samplerName)             textureName
	#define TEXTURECUBE_ARRAY_ARGS(textureName, samplerName)       textureName
	#define TEXTURE3D_ARGS(textureName, samplerName)               textureName
	#define TEXTURE2D_SHADOW_ARGS(textureName, samplerName)        textureName
	#define TEXTURE2D_ARRAY_SHADOW_ARGS(textureName, samplerName)  textureName
	#define TEXTURECUBE_SHADOW_ARGS(textureName, samplerName)      textureName

	#define SAMPLE_TEXTURE2D(textureName, samplerName, coord2) tex2D(textureName, coord2)

	#if (SHADER_TARGET >= 30)
	    #define SAMPLE_TEXTURE2D_LOD(textureName, samplerName, coord2, lod) tex2Dlod(textureName, float4(coord2, 0, lod))
	#else
	    #define SAMPLE_TEXTURE2D_LOD(textureName, samplerName, coord2, lod) SAMPLE_TEXTURE2D_BIAS(textureName, samplerName, coord2, lod)
	#endif

	#define SAMPLE_TEXTURE2D_BIAS(textureName, samplerName, coord2, bias)                       tex2Dbias(textureName, float4(coord2, 0, bias))
	#define SAMPLE_TEXTURE2D_GRAD(textureName, samplerName, coord2, ddx, ddy)                   SAMPLE_TEXTURE2D(textureName, samplerName, coord2)
	#define SAMPLE_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)                     ERROR_ON_UNSUPPORTED_FUNCTION(SAMPLE_TEXTURE2D_ARRAY)
	#define SAMPLE_TEXTURE2D_ARRAY_LOD(textureName, samplerName, coord2, index, lod)            ERROR_ON_UNSUPPORTED_FUNCTION(SAMPLE_TEXTURE2D_ARRAY_LOD)
	#define SAMPLE_TEXTURE2D_ARRAY_BIAS(textureName, samplerName, coord2, index, bias)          ERROR_ON_UNSUPPORTED_FUNCTION(SAMPLE_TEXTURE2D_ARRAY_BIAS)
	#define SAMPLE_TEXTURE2D_ARRAY_GRAD(textureName, samplerName, coord2, index, dpdx, dpdy)    ERROR_ON_UNSUPPORTED_FUNCTION(SAMPLE_TEXTURE2D_ARRAY_GRAD)
	#define SAMPLE_TEXTURECUBE(textureName, samplerName, coord3)                                texCUBE(textureName, coord3)
	#define SAMPLE_TEXTURECUBE_LOD(textureName, samplerName, coord3, lod)                       SAMPLE_TEXTURECUBE_BIAS(textureName, samplerName, coord3, lod)
	#define SAMPLE_TEXTURECUBE_BIAS(textureName, samplerName, coord3, bias)                     texCUBEbias(textureName, float4(coord3, bias))
	#define SAMPLE_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)                   ERROR_ON_UNSUPPORTED_FUNCTION(SAMPLE_TEXTURECUBE_ARRAY)
	#define SAMPLE_TEXTURECUBE_ARRAY_LOD(textureName, samplerName, coord3, index, lod)          ERROR_ON_UNSUPPORTED_FUNCTION(SAMPLE_TEXTURECUBE_ARRAY_LOD)
	#define SAMPLE_TEXTURECUBE_ARRAY_BIAS(textureName, samplerName, coord3, index, bias)        ERROR_ON_UNSUPPORTED_FUNCTION(SAMPLE_TEXTURECUBE_ARRAY_BIAS)
	#define SAMPLE_TEXTURE3D(textureName, samplerName, coord3)                                  tex3D(textureName, coord3)
	#define SAMPLE_TEXTURE3D_LOD(textureName, samplerName, coord3, lod)                         ERROR_ON_UNSUPPORTED_FUNCTION(SAMPLE_TEXTURE3D_LOD)

	#define SAMPLE_TEXTURE2D_SHADOW(textureName, samplerName, coord3)                           SHADOW2D_SAMPLE(textureName, samplerName, coord3)
	#define SAMPLE_TEXTURE2D_ARRAY_SHADOW(textureName, samplerName, coord3, index)              ERROR_ON_UNSUPPORTED_FUNCTION(SAMPLE_TEXTURE2D_ARRAY_SHADOW)
	#define SAMPLE_TEXTURECUBE_SHADOW(textureName, samplerName, coord4)                         SHADOWCUBE_SAMPLE(textureName, samplerName, coord4)
	#define SAMPLE_TEXTURECUBE_ARRAY_SHADOW(textureName, samplerName, coord4, index)            ERROR_ON_UNSUPPORTED_FUNCTION(SAMPLE_TEXTURECUBE_ARRAY_SHADOW)
	#define LOAD_TEXTURE2D(textureName, unCoord2)                                               half4(0, 0, 0, 0)
	#define LOAD_TEXTURE2D_LOD(textureName, unCoord2, lod)                                      half4(0, 0, 0, 0)
	#define LOAD_TEXTURE2D_MSAA(textureName, unCoord2, sampleIndex)                             half4(0, 0, 0, 0)
	#define LOAD_TEXTURE2D_ARRAY(textureName, unCoord2, index)                                  half4(0, 0, 0, 0)
	#define LOAD_TEXTURE2D_ARRAY_MSAA(textureName, unCoord2, index, sampleIndex)                half4(0, 0, 0, 0)
	#define LOAD_TEXTURE2D_ARRAY_LOD(textureName, unCoord2, index, lod)                         half4(0, 0, 0, 0)
	#define LOAD_TEXTURE3D(textureName, unCoord3)                                               ERROR_ON_UNSUPPORTED_FUNCTION(LOAD_TEXTURE3D)
	#define LOAD_TEXTURE3D_LOD(textureName, unCoord3, lod)                                      ERROR_ON_UNSUPPORTED_FUNCTION(LOAD_TEXTURE3D_LOD)
	#define GATHER_TEXTURE2D(textureName, samplerName, coord2)                  ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_TEXTURE2D)
	#define GATHER_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)     ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_TEXTURE2D_ARRAY)
	#define GATHER_TEXTURECUBE(textureName, samplerName, coord3)                ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_TEXTURECUBE)
	#define GATHER_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)   ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_TEXTURECUBE_ARRAY)
	#define GATHER_RED_TEXTURE2D(textureName, samplerName, coord2)              ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_RED_TEXTURE2D)
	#define GATHER_GREEN_TEXTURE2D(textureName, samplerName, coord2)            ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_GREEN_TEXTURE2D)
	#define GATHER_BLUE_TEXTURE2D(textureName, samplerName, coord2)             ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_BLUE_TEXTURE2D)
	#define GATHER_ALPHA_TEXTURE2D(textureName, samplerName, coord2)            ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_ALPHA_TEXTURE2D)

#else
#error unsupported shader api
#endif
#ifndef UNITY_BRANCH
#   define UNITY_BRANCH
#endif
#ifndef UNITY_FLATTEN
#   define UNITY_FLATTEN
#endif
#ifndef UNITY_UNROLL
#   define UNITY_UNROLL
#endif
#ifndef UNITY_UNROLLX
#   define UNITY_UNROLLX(_x)
#endif
#ifndef UNITY_LOOP
#   define UNITY_LOOP
#endif
#define _USINGTEXCOORD1 1
         struct VertexToPixel
         {
            UNITY_POSITION(pos);       
            float3 worldPos : TEXCOORD0;
            float3 worldNormal : TEXCOORD1;
            float4 worldTangent : TEXCOORD2;
             float4 texcoord0 : TEXCOORD3;
             float4 texcoord1 : TEXCOORD4;
            UNITY_LIGHTING_COORDS(8,9)
            UNITY_FOG_COORDS(10)
             float4 extraV2F0 : TEXCOORD11;
            UNITY_VERTEX_INPUT_INSTANCE_ID
            UNITY_VERTEX_OUTPUT_STEREO

         };
            struct Surface
            {
               half3 Albedo;
               half Height;
               half3 Normal;
               half Smoothness;
               half3 Emission;
               half Metallic;
               half3 Specular;
               half Occlusion;
               half SpecularPower; 
               half Alpha;
               float outputDepth; 
               half SpecularOcclusion;
               half SubsurfaceMask;
               half Thickness;
               half CoatMask;
               half CoatSmoothness;
               half Anisotropy;
               half IridescenceMask;
               half IridescenceThickness;
               int DiffusionProfileHash;
               float SpecularAAThreshold;
               float SpecularAAScreenSpaceVariance;
               float3 DiffuseGI;
               float3 BackDiffuseGI;
               float3 SpecularGI;
               float ior;
               float3 transmittanceColor;
               float atDistance;
               float transmittanceMask;
               float4 ShadowMask;
               float NormalAlpha;
               float MAOSAlpha;
            };
            struct Blackboard
            {
                float blackboardDummyData;
            };
            struct ShaderData
            {
               float4 clipPos; 
               float3 localSpacePosition;
               float3 localSpaceNormal;
               float3 localSpaceTangent;
               float3 worldSpacePosition;
               float3 worldSpaceNormal;
               float3 worldSpaceTangent;
               float tangentSign;

               float3 worldSpaceViewDir;
               float3 tangentSpaceViewDir;

               float4 texcoord0;
               float4 texcoord1;
               float4 texcoord2;
               float4 texcoord3;

               float2 screenUV;
               float4 screenPos;

               float4 vertexColor;
               bool isFrontFace;

               float4 extraV2F0;
               float4 extraV2F1;
               float4 extraV2F2;
               float4 extraV2F3;
               float4 extraV2F4;
               float4 extraV2F5;
               float4 extraV2F6;
               float4 extraV2F7;

               float3x3 TBNMatrix;
               Blackboard blackboard;
            };

            struct VertexData
            {
               #if SHADER_TARGET > 30
               #endif
               float4 vertex : POSITION;
               float3 normal : NORMAL;
               float4 tangent : TANGENT;
               float4 texcoord0 : TEXCOORD0;
               #if _URP && (_USINGTEXCOORD1 || _PASSMETA || _PASSFORWARD || _PASSGBUFFER)
                  float4 texcoord1 : TEXCOORD1;
               #endif

               #if _URP && (_USINGTEXCOORD2 || _PASSMETA || ((_PASSFORWARD || _PASSGBUFFER) && defined(DYNAMICLIGHTMAP_ON)))
                  float4 texcoord2 : TEXCOORD2;
               #endif

               #if _STANDARD && (_USINGTEXCOORD1 || (_PASSMETA || ((_PASSFORWARD || _PASSGBUFFER || _PASSFORWARDADD) && LIGHTMAP_ON)))
                  float4 texcoord1 : TEXCOORD1;
               #endif
               #if _STANDARD && (_USINGTEXCOORD2 || (_PASSMETA || ((_PASSFORWARD || _PASSGBUFFER) && DYNAMICLIGHTMAP_ON)))
                  float4 texcoord2 : TEXCOORD2;
               #endif
               #if _HDRP
                  float4 texcoord1 : TEXCOORD1;
                  float4 texcoord2 : TEXCOORD2;
               #endif
               #if _PASSMOTIONVECTOR || ((_PASSFORWARD || _PASSUNLIT) && defined(_WRITE_TRANSPARENT_MOTION_VECTOR))
                  float3 previousPositionOS : TEXCOORD4; 
                  #if defined (_ADD_PRECOMPUTED_VELOCITY)
                     float3 precomputedVelocity    : TEXCOORD5; 
                  #endif
               #endif

               UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct TessVertex 
            {
               float4 vertex : INTERNALTESSPOS;
               float3 normal : NORMAL;
               float4 tangent : TANGENT;
               float4 texcoord0 : TEXCOORD0;
               float4 texcoord1 : TEXCOORD1;
               float4 texcoord2 : TEXCOORD2;
                float4 extraV2F0 : TEXCOORD5;
               #if _PASSMOTIONVECTOR || ((_PASSFORWARD || _PASSUNLIT) && defined(_WRITE_TRANSPARENT_MOTION_VECTOR))
                  float3 previousPositionOS : TEXCOORD13; 
                  #if defined (_ADD_PRECOMPUTED_VELOCITY)
                     float3 precomputedVelocity : TEXCOORD14;
                  #endif
               #endif

               UNITY_VERTEX_INPUT_INSTANCE_ID
               UNITY_VERTEX_OUTPUT_STEREO
            };

            struct ExtraV2F
            {
               float4 extraV2F0;
               float4 extraV2F1;
               float4 extraV2F2;
               float4 extraV2F3;
               float4 extraV2F4;
               float4 extraV2F5;
               float4 extraV2F6;
               float4 extraV2F7;
               Blackboard blackboard;
               float4 time;
            };
            float3 WorldToTangentSpace(ShaderData d, float3 normal)
            {
               return mul(d.TBNMatrix, normal);
            }

            float3 TangentToWorldSpace(ShaderData d, float3 normal)
            {
               return mul(normal, d.TBNMatrix);
            }
            #if _STANDARD
               float3 TransformWorldToObject(float3 p) { return mul(unity_WorldToObject, float4(p, 1)); };
               float3 TransformObjectToWorld(float3 p) { return mul(unity_ObjectToWorld, float4(p, 1)); };
               float4 TransformWorldToObject(float4 p) { return mul(unity_WorldToObject, p); };
               float4 TransformObjectToWorld(float4 p) { return mul(unity_ObjectToWorld, p); };
               float4x4 GetWorldToObjectMatrix() { return unity_WorldToObject; }
               float4x4 GetObjectToWorldMatrix() { return unity_ObjectToWorld; }
               #if (defined(SHADER_API_D3D11) || defined(SHADER_API_XBOXONE) || defined(UNITY_COMPILER_HLSLCC) || defined(SHADER_API_PSSL) || (SHADER_TARGET_SURFACE_ANALYSIS && !SHADER_TARGET_SURFACE_ANALYSIS_MOJOSHADER))
                 #define UNITY_SAMPLE_TEX2D_LOD(tex,coord, lod) tex.SampleLevel (sampler##tex,coord, lod)
                 #define UNITY_SAMPLE_TEX2D_SAMPLER_LOD(tex,samplertex,coord, lod) tex.SampleLevel (sampler##samplertex,coord, lod)
              #else
                 #define UNITY_SAMPLE_TEX2D_LOD(tex,coord,lod) tex2D (tex,coord,0,lod)
                 #define UNITY_SAMPLE_TEX2D_SAMPLER_LOD(tex,samplertex,coord,lod) tex2D (tex,coord,0,lod)
              #endif

               #undef UNITY_MATRIX_I_M

               #define UNITY_MATRIX_I_M   unity_WorldToObject
            #endif

            float3 GetCameraWorldPosition()
            {
               #if _HDRP
                  return GetCameraRelativePositionWS(_WorldSpaceCameraPos);
               #else
                  return _WorldSpaceCameraPos;
               #endif
            }

            #if _GRABPASSUSED
               #if _STANDARD
                  TEXTURE2D(%GRABTEXTURE%);
                  SAMPLER(sampler_%GRABTEXTURE%);
               #endif

               half3 GetSceneColor(float2 uv)
               {
                  #if _STANDARD
                     return SAMPLE_TEXTURE2D(%GRABTEXTURE%, sampler_%GRABTEXTURE%, uv).rgb;
                  #else
                     return SHADERGRAPH_SAMPLE_SCENE_COLOR(uv);
                  #endif
               }
            #endif
            #if _STANDARD
               UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);
               float GetSceneDepth(float2 uv) { return SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, uv); }
               float GetLinear01Depth(float2 uv) { return Linear01Depth(GetSceneDepth(uv)); }
               float GetLinearEyeDepth(float2 uv) { return LinearEyeDepth(GetSceneDepth(uv)); } 
            #else
               float GetSceneDepth(float2 uv) { return SHADERGRAPH_SAMPLE_SCENE_DEPTH(uv); }
               float GetLinear01Depth(float2 uv) { return Linear01Depth(GetSceneDepth(uv), _ZBufferParams); }
               float GetLinearEyeDepth(float2 uv) { return LinearEyeDepth(GetSceneDepth(uv), _ZBufferParams); } 
            #endif

            float3 GetWorldPositionFromDepthBuffer(float2 uv, float3 worldSpaceViewDir)
            {
               float eye = GetLinearEyeDepth(uv);
               float3 camView = mul((float3x3)UNITY_MATRIX_M, transpose(mul(UNITY_MATRIX_I_M, UNITY_MATRIX_I_V)) [2].xyz);

               float dt = dot(worldSpaceViewDir, camView);
               float3 div = worldSpaceViewDir/dt;
               float3 wpos = (eye * div) + GetCameraWorldPosition();
               return wpos;
            }

            #if _HDRP
            float3 ObjectToWorldSpacePosition(float3 pos)
            {
               return GetAbsolutePositionWS(TransformObjectToWorld(pos));
            }
            #else
            float3 ObjectToWorldSpacePosition(float3 pos)
            {
               return TransformObjectToWorld(pos);
            }
            #endif

            #if _STANDARD
               UNITY_DECLARE_SCREENSPACE_TEXTURE(_CameraDepthNormalsTexture);
               float3 GetSceneNormal(float2 uv, float3 worldSpaceViewDir)
               {
                  float4 depthNorms = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_CameraDepthNormalsTexture, uv);
                  float3 norms = DecodeViewNormalStereo(depthNorms);
                  norms = mul((float3x3)UNITY_MATRIX_V, norms) * 0.5 + 0.5;
                  return norms;
               }
            #elif _HDRP && !_DECALSHADER
               float3 GetSceneNormal(float2 uv, float3 worldSpaceViewDir)
               {
                  NormalData nd;
                  DecodeFromNormalBuffer(_ScreenSize.xy * uv, nd);
                  return nd.normalWS;
               }
            #elif _URP
               #if (SHADER_LIBRARY_VERSION_MAJOR >= 10)
                  #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareNormalsTexture.hlsl"
               #endif

               float3 GetSceneNormal(float2 uv, float3 worldSpaceViewDir)
               {
                  #if (SHADER_LIBRARY_VERSION_MAJOR >= 10)
                     return SampleSceneNormals(uv);
                  #else
                     float3 wpos = GetWorldPositionFromDepthBuffer(uv, worldSpaceViewDir);
                     return normalize(-cross(ddx(wpos), ddy(wpos))) * 0.5 + 0.5;
                  #endif

                }
             #endif

             #if _HDRP

               half3 UnpackNormalmapRGorAG(half4 packednormal)
               {
                  packednormal.x *= packednormal.w;

                  half3 normal;
                  normal.xy = packednormal.xy * 2 - 1;
                  normal.z = sqrt(1 - saturate(dot(normal.xy, normal.xy)));
                  return normal;
               }
               half3 UnpackNormal(half4 packednormal)
               {
                  #if defined(UNITY_NO_DXT5nm)
                     return packednormal.xyz * 2 - 1;
                  #else
                     return UnpackNormalmapRGorAG(packednormal);
                  #endif
               }
            #endif
            #if _HDRP || _URP

               half3 UnpackScaleNormal(half4 packednormal, half scale)
               {
                 #ifndef UNITY_NO_DXT5nm
                   packednormal.x *= packednormal.w;
                 #endif
                   half3 normal;
                   normal.xy = (packednormal.xy * 2 - 1) * scale;
                   normal.z = sqrt(1 - saturate(dot(normal.xy, normal.xy)));
                   return normal;
               }	

             #endif
            void GetSun(out float3 lightDir, out float3 color)
            {
               lightDir = float3(0.5, 0.5, 0);
               color = 1;
               #if _HDRP
                  if (_DirectionalLightCount > 0)
                  {
                     DirectionalLightData light = _DirectionalLightDatas[0];
                     lightDir = -light.forward.xyz;
                     color = light.color;
                  }
               #elif _STANDARD
			         lightDir = normalize(_WorldSpaceLightPos0.xyz);
                  color = _LightColor0.rgb;
               #elif _URP
	               Light light = GetMainLight();
	               lightDir = light.direction;
	               color = light.color;
               #endif
            }
    half _ShadingMode;
    half _LightFunction;
    half _UseAlphaOnlyFromBaseMap;
    half _EnableToonShading;
    half _ShadingFunction;
    int _NumberOfCells;
	float _CellTransitionSmoothness;
	half _SumLightsBeforePosterization;
	half _ShadingUseLightColors;
    half _GradientMode;
    half _GradientBlending;
    float _GradientBlendFactor;

    float4 _GradientTex_TexelSize;
    half _ShadingAffectedByNormalMap;
    half _EnableShadows;
    float4 _CoreShadowColor;
    float _TerminatorPosition;
    float _TerminatorWidth;
    float _TerminatorSmoothness;
    float4 _FormShadowColor;

    half _EnableCastShadows;
    float _CastShadowsStrength;
    float _CastShadowsSmoothness;
    half _CastShadowColorMode;
    float4 _CastShadowColor;
    half _EnableSpecular;
    half _SpecularBlending;
    half4 _SpecularColor;
    float _SpecularSize;
    float _SpecularSmoothness;
    float _SpecularOpacity;
    half _SpecularAffectedByNormalMap;
    half _SpecularUseLightColors;
    half _EnableRim;
    half _RimBlending;
    float4 _RimColor;
    float _RimSize;
    float _RimSmoothness;
    float _RimOpacity;
    half _RimAffectedArea;
    half _RimAffectedByNormalMap;
     half _EnableStyling;

    half _EnableStylingDistanceFade;
    float _StylingDFStartingDistance;
    float _StylingDFFalloff;
    half _StylingAdjustDistanceFadeValue;
    float _StylingDistanceFadeValue;
    half _HatchingAffectedByNormalMap;
    half _EnableAntiAliasing;
    float4 _NoiseTex2_TexelSize;
    half _EnableShadingStyling;
    half _StylingShadingSyncWithOtherStyling;
    half _ShadingStyle;    
    half _SyncWithLightPartitioning;
    half _NumberOfCellsHatching;
    half _StylingTerminatorPosition;    
    float _StylingOvermodelingFactor;
    half _StylingShadingBlending;
    half _StylingShadingIsInverted;
    half _DrawSpace;
    half _SSCameraDistanceScaled;
    half _AnchorSSToObjectsOrigin;
    half _CoordinateSystem;
    half _PolarCenterMode;
    float4 _PolarCenter;
    float _StylingShadingDensity;
    float _StylingShadingInitialDirection;
    float _StylingShadingRotationBetweenCells;
    float _StylingShadingHalftonesOffset;
    float4 _StylingColor;
    float _StylingShadingOpacity;
    float _StylingShadingOpacityFalloff;
    float _StylingShadingThicknessControl;
    float _StylingShadingThickness;
    float _StylingShadingThicknessFalloff;
    float _StylingShadingHardness;
    float _StylingShadingHalftonesRoundness;
    float _StylingShadingHalftonesRoundnessFalloff;
    half _EnableShadingRandomizer;
    float _ShadingNoise1Size;
    float _ShadingNoise1Seed;
    float _ShadingNoise2Seed;
    float _NoiseIntensity;
    half _SpacingRandomMode;
    float _SpacingRandomIntensity;
    half _OpacityRandomMode;
    float _OpacityRandomIntensity;
    half _LengthRandomMode;
    float _LengthRandomIntensity;
    half _HardnessRandomMode;
    float _HardnessRandomIntensity;
    half _ThicknessRandomMode;
    float _ThicknesshRandomIntensity;
    half _EnableCastShadowsStyling;
    half _CastShadowsNumberOfCellsHatching;
    float _StylingCastShadowsSmoothness;
    half _StylingCastShadowsSyncWithOtherStyling;
    half _CastShadowsStyle;
    half _StylingCastShadowsBlending;
    half _StylingCastShadowsIsInverted;
    half _CastShadowsDrawSpace;
    half _CastShadowsSSCameraDistanceScaled;
    half _CastShadowsAnchorSSToObjectsOrigin;
    half _CastShadowsCoordinateSystem;
    half _CastShadowsPolarCenterMode;
    float4 _CastShadowsPolarCenter;
    float _StylingCastShadowsDensity;
    float _StylingCastShadowsInitialDirection;
    float _StylingCastShadowsRotationBetweenCells;
    float _StylingCastShadowsHalftonesOffset;
    float4 _StylingCastShadowsColor;
    float _StylingCastShadowsOpacity;
    float _StylingCastShadowsOpacityFalloff;
    half _StylingCastShadowsThicknessControl;
    float _StylingCastShadowsThickness;
    float _StylingCastShadowsThicknessFalloff;
    float _StylingCastShadowsHardness;
    float _StylingCastShadowsHalftonesRoundness;
    float _StylingCastShadowsHalftonesRoundnessFalloff;
    half _EnableCastShadowsRandomizer;
    float _CastShadowsNoise1Size;
    float _CastShadowsNoise1Seed;
    float _CastShadowsNoise2Seed;
    float _CastShadowsNoiseIntensity;
    half _CastShadowsSpacingRandomMode;
    float _CastShadowsSpacingRandomIntensity;
    half _CastShadowsOpacityRandomMode;
    float _CastShadowsOpacityRandomIntensity;
    half _CastShadowsHardnessRandomMode;
    float _CastShadowsHardnessRandomIntensity;
    half _CastShadowsLengthRandomMode;
    float _CastShadowsLengthRandomIntensity;
    half _CastShadowsThicknessRandomMode;
    float _CastShadowsThicknesshRandomIntensity;
    half _EnableSpecularStyling;
    half _SyncWithSpecular;
    float _StylingSpecularSize;
    float _StylingSpecularSmoothness;
    half _StylingSpecularCutOutShading;
    half _StylingSpecularUseLightColors;
    half _StylingSpecularSyncWithOtherStyling;
    half _SpecularStyle;
    half _StylingSpecularBlending;
    half _StylingSpecularIsInverted;
    half _SpecularDrawSpace;
    half _SpecularSSCameraDistanceScaled;
    half _SpecularAnchorSSToObjectsOrigin;
    half _SpecularCoordinateSystem;
    half _SpecularPolarCenterMode;
    float4 _SpecularPolarCenter;
    float _StylingSpecularDensity;    
    float _StylingSpecularRotation;
    float _StylingSpecularHalftonesOffset;
    float4 _StylingSpecularColor;
    float _StylingSpecularOpacity;
    float _StylingSpecularOpacityFalloff;
    float _StylingSpecularThicknessControl;
    float _StylingSpecularThickness;
    float _StylingSpecularThicknessFalloff;
    float _StylingSpecularHardness;
    float _StylingSpecularHalftonesRoundness;
    float _StylingSpecularHalftonesRoundnessFalloff;
    float _EnableSpecularRandomizer;    
    float _SpecularNoise1Size;
    float _SpecularNoise1Seed;
    float _SpecularNoise2Seed;
    float _SpecularNoiseIntensity;
    half _SpecularSpacingRandomMode;
    float _SpecularSpacingRandomIntensity;
    half _SpecularOpacityRandomMode;
    float _SpecularOpacityRandomIntensity;
    half _SpecularLengthRandomMode;
    float _SpecularLengthRandomIntensity;
    half _SpecularHardnessRandomMode;
    float _SpecularHardnessRandomIntensity;
    half _SpecularThicknessRandomMode;
    float _SpecularThicknesshRandomIntensity;
    half _EnableRimStyling;
    half _StylingRimSyncWithOtherStyling;
    half _SyncWithRim;
    float _StylingRimSize;
    float _StylingRimSmoothness;
    half _StylingRimAffectedArea;
    half _RimStyle;
    half _StylingRimBlending;
    half _StylingRimIsInverted;
    half _RimDrawSpace;
    half _RimSSCameraDistanceScaled;
    half _RimAnchorSSToObjectsOrigin;
    half _RimCoordinateSystem;
    half _RimPolarCenterMode;
    float4 _RimPolarCenter;  
    float _StylingRimDensity;    
    float _StylingRimRotation;
    float _StylingRimHalftonesOffset;
    float4 _StylingRimColor;
    float _StylingRimOpacity;
    float _StylingRimOpacityFalloff;
    float _StylingRimThicknessControl;
    float _StylingRimThickness;
    float _StylingRimThicknessFalloff;
    float _StylingRimHardness;
    float _StylingRimHalftonesRoundness;
    float _StylingRimHalftonesRoundnessFalloff;
    float _EnableRimRandomizer;        
    float _RimNoise1Size;
    float _RimNoise1Seed;
    float _RimNoise2Seed;
    float _RimNoiseIntensity;
    half _RimSpacingRandomMode;
    float _RimSpacingRandomIntensity;
    half _RimOpacityRandomMode; 
    float _RimOpacityRandomIntensity;
    half _RimLengthRandomMode;
    float _RimLengthRandomIntensity;
    half _RimHardnessRandomMode;
    float _RimHardnessRandomIntensity;
    half _RimThicknessRandomMode;
    float _RimThicknesshRandomIntensity;
    half4 _OutlineColor;
    half _OutlineWidth;
    half _EnableOutline;
    half4 _Color;
    half _Cutoff; 
    half _Mode;
    float _Cull;
    half _Metallic;
    half3 _EmissionColor;
    half _UVSec;

    half _BumpScale; 
    half _DetailNormalMapScale;
    float _Glossiness;
    float _GlossMapScale;
    half _OcclusionStrength;
    half _Parallax;

    float4 _MainTex_ST;
    float4 _DetailAlbedoMap_ST;
    sampler2D _NoiseMap1;
    sampler2D _NoiseMap2;

    sampler2D _HatchingMap;
    sampler2D _GradientTex;
    void Ext_ModifyVertex0 (inout VertexData v, inout ExtraV2F d)
    {
        float4x4 UnityObjectToClipPosMatrix = mul(UNITY_MATRIX_VP, GetObjectToWorldMatrix());
        float4 pos = mul(UnityObjectToClipPosMatrix, v.vertex);
        float4 objectOriginClipPos = mul(UnityObjectToClipPosMatrix, float4(0, 0, 0, 1));
        objectOriginClipPos.xy = clamp(objectOriginClipPos.xy, -1.0*objectOriginClipPos.w, 1.0*objectOriginClipPos.w);
        float4 originScreenPos = ComputeScreenPos(objectOriginClipPos);
        float2 originScreenUV = originScreenPos.xy/originScreenPos.w;
        float4 screenPos = ComputeScreenPos(pos);
        float2 uv = ((screenPos.xy) / screenPos.w);
        float4 ssss = ComputeScreenPos(pos);
       d.extraV2F0 = float4(originScreenUV,originScreenUV);
    }
    sampler2D _MainTex;
    sampler2D _DetailAlbedoMap;
    sampler2D _DetailMask;
    sampler2D _BumpMap;
    sampler2D _DetailNormalMap;
    sampler2D _MetallicGlossMap;
    sampler2D _OcclusionMap;
    sampler2D _ParallaxMap;
    sampler2D _EmissionMap;
	void Ext_SurfaceFunction1 (inout Surface o, ShaderData d)
	{
        float4 texcoords;
        texcoords.xy = d.texcoord0.xy * _MainTex_ST.xy + _MainTex_ST.zw; 
        texcoords.zw = (_UVSec == 0) ? d.texcoord0.xy * _DetailAlbedoMap_ST.xy + _DetailAlbedoMap_ST.zw : d.texcoord1.xy * _DetailAlbedoMap_ST.xy + _DetailAlbedoMap_ST.zw;
    #if _SHADING_COLOR
        half4 diffuseAlpha = tex2D(_MainTex, texcoords.xy);
        o.Albedo = diffuseAlpha.rgb * _Color.rgb;
        o.Normal = UnpackScaleNormal(tex2D(_BumpMap, texcoords.xy), _BumpScale);

		half alpha = diffuseAlpha.a;
        o.Alpha = alpha * _Color.a;
        if (_Mode) 
        {
            clip (alpha - _Cutoff);
        }

        #ifdef _EMISSION
            half3 emission = tex2D(_EmissionMap, texcoords.xy).rgb * _EmissionColor.rgb; 
            o.Emission = emission;
        #endif
    #else
        #ifdef _PARALLAXMAP
            half h = tex2D (_ParallaxMap, texcoords.xy).g;
            float2 offset = ParallaxOffset1Step (h, _Parallax, d.tangentSpaceViewDir);
            texcoords = float4(texcoords.xy + offset, texcoords.zw + offset);
            o.Normal = float3(0, 0, 1);
        #endif
        half3 albedo = tex2D(_MainTex, texcoords.xy).rgb * _Color.rgb; 
        #ifdef _DETAIL_MULX2
                half3 detailAlbedo = tex2D (_DetailAlbedoMap, texcoords.zw).rgb;
                albedo *= LerpWhiteTo (detailAlbedo * unity_ColorSpaceDouble.rgb, tex2D(_DetailMask, texcoords.xy).a);
        #endif
        o.Albedo = albedo;
        half2 metallicSmoothness = half2(0,0);
        #ifdef _METALLICGLOSSMAP
            half4 metallicGlossMap = tex2D(_MetallicGlossMap, texcoords.xy);
            metallicSmoothness.r = metallicGlossMap.r;
            #ifdef _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
                metallicSmoothness.g = tex2D(_MainTex, texcoords.xy).a;
            #else
                metallicSmoothness.g = metallicGlossMap.a;
            #endif
            metallicSmoothness.g *= _GlossMapScale;
        #else
            metallicSmoothness.r = _Metallic;
            #ifdef _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
                metallicSmoothness.g  = tex2D(_MainTex, texcoords.xy).a * _GlossMapScale;
            #else
                metallicSmoothness.g = _Glossiness;
            #endif
        #endif
        o.Metallic = metallicSmoothness.r;
        o.Smoothness = metallicSmoothness.g;
        o.Occlusion = lerp(1, tex2D(_OcclusionMap, texcoords.xy).g, _OcclusionStrength);
        half alpha = 0;
        #if defined(_SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A)
            alpha = _Color.a;
        #else
            alpha =  tex2D(_MainTex, texcoords.xy).a * _Color.a;
        #endif
        if (_Mode) 
        {
            clip (alpha - _Cutoff);
        }
        o.Alpha = alpha;
        #ifdef _NORMALMAP
            half3 normal = UnpackScaleNormal(tex2D(_BumpMap, texcoords.xy), _BumpScale);
            #ifdef _DETAIL_MULX2
                half3 detailNormal = UnpackScaleNormal(tex2D(_DetailNormalMap, texcoords.zw), _DetailNormalMapScale);
	            detailNormal = lerp(float3(0, 0, 1), detailNormal, tex2D(_DetailMask, texcoords.xy).a);
	            normal = BlendNormals(normal, detailNormal);
            #endif
            o.Normal = normal;
        #endif
        #ifdef _EMISSION
            o.Emission += tex2D(_EmissionMap, texcoords.xy).rgb * _EmissionColor.rgb; 
        #endif
    #endif
	}
            void ChainSurfaceFunction(inout Surface l, inout ShaderData d)
            {
                  Ext_SurfaceFunction1(l, d);
            }

#if !_DECALSHADER

            void ChainModifyVertex(inout VertexData v, inout VertexToPixel v2p, float4 time)
            {
                 ExtraV2F d;
                 ZERO_INITIALIZE(ExtraV2F, d);
                 ZERO_INITIALIZE(Blackboard, d.blackboard);
                 d.time = time;

                   Ext_ModifyVertex0(v, d);
                  v2p.extraV2F0 = d.extraV2F0;
            }

            void ChainModifyTessellatedVertex(inout VertexData v, inout VertexToPixel v2p)
            {
               ExtraV2F d;
               ZERO_INITIALIZE(ExtraV2F, d);
               ZERO_INITIALIZE(Blackboard, d.blackboard);
                d.extraV2F0 = v2p.extraV2F0;
                v2p.extraV2F0 = d.extraV2F0;
            }

            void ChainFinalColorForward(inout Surface l, inout ShaderData d, inout half4 color)
            {
            }

            void ChainFinalGBufferStandard(inout Surface s, inout ShaderData d, inout half4 GBuffer0, inout half4 GBuffer1, inout half4 GBuffer2, inout half4 outEmission, inout half4 outShadowMask)
            {
            }
#endif
#if _DECALSHADER

        ShaderData CreateShaderData(SurfaceDescriptionInputs IN)
        {
            ShaderData d = (ShaderData)0;
            d.TBNMatrix = float3x3(IN.WorldSpaceTangent, IN.WorldSpaceBiTangent, IN.WorldSpaceNormal);
            d.worldSpaceNormal = IN.WorldSpaceNormal;
            d.worldSpaceTangent = IN.WorldSpaceTangent;

            d.worldSpacePosition = IN.WorldSpacePosition;
            d.texcoord0 = IN.uv0.xyxy;
            d.screenPos = IN.ScreenPosition;

            d.worldSpaceViewDir = normalize(_WorldSpaceCameraPos - d.worldSpacePosition);

            d.tangentSpaceViewDir = mul(d.TBNMatrix, d.worldSpaceViewDir);
            #if _HDRP
            #else
            #endif
            return d;
        }
#else

         ShaderData CreateShaderData(VertexToPixel i
                  #if NEED_FACING
                     , bool facing
                  #endif
         )
         {
            ShaderData d = (ShaderData)0;
            d.clipPos = i.pos;
            d.worldSpacePosition = i.worldPos;

            d.worldSpaceNormal = normalize(i.worldNormal);
            d.worldSpaceTangent.xyz = normalize(i.worldTangent.xyz);

            d.tangentSign = i.worldTangent.w * unity_WorldTransformParams.w;
            float3 bitangent = cross(d.worldSpaceTangent.xyz, d.worldSpaceNormal) * d.tangentSign;
            d.TBNMatrix = float3x3(d.worldSpaceTangent, -bitangent, d.worldSpaceNormal);
            d.worldSpaceViewDir = normalize(_WorldSpaceCameraPos - i.worldPos);

            d.tangentSpaceViewDir = mul(d.TBNMatrix, d.worldSpaceViewDir);
             d.texcoord0 = i.texcoord0;
             d.texcoord1 = i.texcoord1;
            #if _HDRP
            #else
            #endif
             d.extraV2F0 = i.extraV2F0;
            return d;
         }

#endif
         VertexToPixel Vert (VertexData v)
         {
           UNITY_SETUP_INSTANCE_ID(v);
           VertexToPixel o;
           UNITY_INITIALIZE_OUTPUT(VertexToPixel,o);
           UNITY_TRANSFER_INSTANCE_ID(v,o);
           UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

#if !_TESSELLATION_ON
           ChainModifyVertex(v, o, _Time);
#endif

           o.pos = UnityObjectToClipPos(v.vertex);
            o.texcoord0 = v.texcoord0;
            o.texcoord1 = v.texcoord1;
           o.worldPos = mul(GetObjectToWorldMatrix(), v.vertex).xyz;
           o.worldNormal = UnityObjectToWorldNormal(v.normal);
           o.worldTangent = float4(UnityObjectToWorldDir(v.tangent.xyz), v.tangent.w);
           UNITY_TRANSFER_LIGHTING(o, v.texcoord1.xy); 
           UNITY_TRANSFER_FOG(o,o.pos); 

           return o;
         }
         #include "Packages/com.shadercrew.the-toon-shader.core/Scripts/Shaders/xxSharedTTSDependecies/TheToonShaderLightingFunctions.hlsl"

         #include "Packages/com.shadercrew.the-toon-shader.3d/Scripts/Shaders/xxSharedTTSDependecies/ModifiedLightingFunctions/BiRP/UnityPBSLighting_Toon.cginc"
         #include "Packages/com.shadercrew.the-toon-shader.core/Scripts/Shaders/xxSharedTTSDependecies/TheToonShaderFunctions.hlsl"
         fixed4 Frag (VertexToPixel IN
         #ifdef _DEPTHOFFSET_ON
              , out float outputDepth : SV_Depth
         #endif
         #if NEED_FACING
            , bool facing : SV_IsFrontFace
         #endif
         ) : SV_Target
         {
           UNITY_SETUP_INSTANCE_ID(IN);
           #ifdef FOG_COMBINED_WITH_TSPACE
             UNITY_EXTRACT_FOG_FROM_TSPACE(IN);
           #elif defined (FOG_COMBINED_WITH_WORLD_POS)
             UNITY_EXTRACT_FOG_FROM_WORLD_POS(IN);
           #else
             UNITY_EXTRACT_FOG(IN);
           #endif
           ShaderData d = CreateShaderData(IN
              #if NEED_FACING
                 , facing
              #endif
           );
           Surface l = (Surface)0;
           #ifdef _DEPTHOFFSET_ON
              l.outputDepth = outputDepth;
           #endif

           l.Albedo = half3(0.5, 0.5, 0.5);
           l.Normal = float3(0,0,1);
           l.Occlusion = 1;
           l.Alpha = 1;

           ChainSurfaceFunction(l, d);

           #ifdef _DEPTHOFFSET_ON
              outputDepth = l.outputDepth;
           #endif
           #ifndef USING_DIRECTIONAL_LIGHT
             fixed3 lightDir = normalize(UnityWorldSpaceLightDir(d.worldSpacePosition));
           #else
             fixed3 lightDir = _WorldSpaceLightPos0.xyz;
           #endif
           float3 worldViewDir = normalize(UnityWorldSpaceViewDir(d.worldSpacePosition));

           #if _USESPECULAR || _USESPECULARWORKFLOW || _SPECULARFROMMETALLIC
              #ifdef UNITY_COMPILER_HLSL
                 SurfaceOutputStandardSpecular o = (SurfaceOutputStandardSpecular)0;
              #else
                 SurfaceOutputStandardSpecular o;
              #endif
              o.Specular = l.Specular;
              o.Occlusion = l.Occlusion;
              o.Smoothness = l.Smoothness;
           #elif _BDRFLAMBERT || _BDRF3 || _SIMPLELIT
              #ifdef UNITY_COMPILER_HLSL
                 SurfaceOutput o = (SurfaceOutput)0;
              #else
                 SurfaceOutput o;
              #endif

              o.Specular = l.SpecularPower;
              o.Gloss = l.Smoothness;
              _SpecColor.rgb = l.Specular; 
           #else
              #ifdef UNITY_COMPILER_HLSL
                 SurfaceOutputStandard o = (SurfaceOutputStandard)0;
              #else
                 SurfaceOutputStandard o;
              #endif
              o.Smoothness = l.Smoothness;
              o.Metallic = l.Metallic;
              o.Occlusion = l.Occlusion;
           #endif
           o.Albedo = l.Albedo;
           o.Emission = l.Emission;
           o.Alpha = l.Alpha;

           #if _WORLDSPACENORMAL
              o.Normal = l.Normal;
           #else
              o.Normal = normalize(TangentToWorldSpace(d, l.Normal));
           #endif
           UNITY_LIGHT_ATTENUATION(atten, IN, d.worldSpacePosition)
           half4 c = 0;
           UnityGI gi;
           UNITY_INITIALIZE_OUTPUT(UnityGI, gi);
           gi.indirect.diffuse = 0;
           gi.indirect.specular = 0;
           gi.light.color = _LightColor0.rgb;
           gi.light.dir = lightDir;
           gi.light.color *= atten;

           #if defined(_OVERRIDE_SHADOWMASK)
               float4 mulColor = saturate(dot(l.ShadowMask, unity_OcclusionMaskSelector));
               gi.light.color *= mulColor;
            #endif

            c = half4(l.Albedo, l.Alpha);
            #if _SHADING_COLOR
                c = 0;
            #else
                AddTheToonShader(c,o, gi,d);
            #endif

           ChainFinalColorForward(l, d, c);

           #if !DISABLEFOG
            UNITY_APPLY_FOG(_unity_fogCoord, c); 
           #endif
           #if !_ALPHABLEND_ON
              UNITY_OPAQUE_ALPHA(c.a);
           #endif
           return c;
         }

         ENDCG

      }
	   Pass {
		   Name "ShadowCaster"
		   Tags { "LightMode" = "ShadowCaster" }
		   ZWrite On ZTest LEqual

             Cull [_Cull]
    Cull [_Cull]
         CGPROGRAM

            #pragma vertex Vert
   #pragma fragment Frag
         #pragma target 3.0
         #pragma multi_compile_instancing
         #pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
         #pragma multi_compile_shadowcaster
         #include "HLSLSupport.cginc"
         #define UNITY_INSTANCED_LOD_FADE
         #define UNITY_INSTANCED_SH
         #define UNITY_INSTANCED_LIGHTMAPSTS
         #include "UnityShaderVariables.cginc"
         #include "UnityShaderUtilities.cginc"

         #include "UnityCG.cginc"
         #include "Lighting.cginc"
         #include "UnityPBSLighting.cginc"

         #define _PASSSHADOW 1
     #pragma shader_feature_local_fragment  _SHADING_COLOR _SHADING_BLINNPHONG _SHADING_PBR    
        #pragma shader_feature_local _NORMALMAP
        #pragma shader_feature_local _METALLICGLOSSMAP
        #pragma shader_feature_local _PARALLAXMAP
        #pragma shader_feature_local _DETAIL_MULX2
        #pragma shader_feature_local _EMISSION       
        #pragma shader_feature_local _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
   #define _STANDARD 1
#if defined(SHADER_API_GAMECORE)

	#define ZERO_INITIALIZE(type, name) name = (type)0;
	#define ZERO_INITIALIZE_ARRAY(type, name, arraySize) { for (int arrayIndex = 0; arrayIndex < arraySize; arrayIndex++) { name[arrayIndex] = (type)0; } }
	#define CALCULATE_TEXTURE2D_LOD(textureName, samplerName, coord2) textureName.CalculateLevelOfDetail(samplerName, coord2)
	#define TEXTURE2D(textureName)                Texture2D textureName
	#define TEXTURE2D_ARRAY(textureName)          Texture2DArray textureName
	#define TEXTURECUBE(textureName)              TextureCube textureName
	#define TEXTURECUBE_ARRAY(textureName)        TextureCubeArray textureName
	#define TEXTURE3D(textureName)                Texture3D textureName

	#define TEXTURE2D_FLOAT(textureName)          TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_FLOAT(textureName)    TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_FLOAT(textureName)        TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_FLOAT(textureName)  TEXTURECUBE_ARRAY(textureName)
	#define TEXTURE3D_FLOAT(textureName)          TEXTURE3D(textureName)

	#define TEXTURE2D_HALF(textureName)           TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_HALF(textureName)     TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_HALF(textureName)         TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_HALF(textureName)   TEXTURECUBE_ARRAY(textureName)
	#define TEXTURE3D_HALF(textureName)           TEXTURE3D(textureName)

	#define TEXTURE2D_SHADOW(textureName)         TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_SHADOW(textureName)   TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_SHADOW(textureName)       TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_SHADOW(textureName) TEXTURECUBE_ARRAY(textureName)

	#define RW_TEXTURE2D(type, textureName)       RWTexture2D<type> textureName
	#define RW_TEXTURE2D_ARRAY(type, textureName) RWTexture2DArray<type> textureName
	#define RW_TEXTURE3D(type, textureName)       RWTexture3D<type> textureName

	#define SAMPLER(samplerName)                  SamplerState samplerName
	#define SAMPLER_CMP(samplerName)              SamplerComparisonState samplerName
	#define ASSIGN_SAMPLER(samplerName, samplerValue) samplerName = samplerValue

	#define TEXTURE2D_PARAM(textureName, samplerName)                 TEXTURE2D(textureName),         SAMPLER(samplerName)
	#define TEXTURE2D_ARRAY_PARAM(textureName, samplerName)           TEXTURE2D_ARRAY(textureName),   SAMPLER(samplerName)
	#define TEXTURECUBE_PARAM(textureName, samplerName)               TEXTURECUBE(textureName),       SAMPLER(samplerName)
	#define TEXTURECUBE_ARRAY_PARAM(textureName, samplerName)         TEXTURECUBE_ARRAY(textureName), SAMPLER(samplerName)
	#define TEXTURE3D_PARAM(textureName, samplerName)                 TEXTURE3D(textureName),         SAMPLER(samplerName)

	#define TEXTURE2D_SHADOW_PARAM(textureName, samplerName)          TEXTURE2D(textureName),         SAMPLER_CMP(samplerName)
	#define TEXTURE2D_ARRAY_SHADOW_PARAM(textureName, samplerName)    TEXTURE2D_ARRAY(textureName),   SAMPLER_CMP(samplerName)
	#define TEXTURECUBE_SHADOW_PARAM(textureName, samplerName)        TEXTURECUBE(textureName),       SAMPLER_CMP(samplerName)
	#define TEXTURECUBE_ARRAY_SHADOW_PARAM(textureName, samplerName)  TEXTURECUBE_ARRAY(textureName), SAMPLER_CMP(samplerName)

	#define TEXTURE2D_ARGS(textureName, samplerName)                textureName, samplerName
	#define TEXTURE2D_ARRAY_ARGS(textureName, samplerName)          textureName, samplerName
	#define TEXTURECUBE_ARGS(textureName, samplerName)              textureName, samplerName
	#define TEXTURECUBE_ARRAY_ARGS(textureName, samplerName)        textureName, samplerName
	#define TEXTURE3D_ARGS(textureName, samplerName)                textureName, samplerName

	#define TEXTURE2D_SHADOW_ARGS(textureName, samplerName)         textureName, samplerName
	#define TEXTURE2D_ARRAY_SHADOW_ARGS(textureName, samplerName)   textureName, samplerName
	#define TEXTURECUBE_SHADOW_ARGS(textureName, samplerName)       textureName, samplerName
	#define TEXTURECUBE_ARRAY_SHADOW_ARGS(textureName, samplerName) textureName, samplerName

	#define PLATFORM_SAMPLE_TEXTURE2D(textureName, samplerName, coord2)                               textureName.Sample(samplerName, coord2)
	#define PLATFORM_SAMPLE_TEXTURE2D_LOD(textureName, samplerName, coord2, lod)                      textureName.SampleLevel(samplerName, coord2, lod)
	#define PLATFORM_SAMPLE_TEXTURE2D_BIAS(textureName, samplerName, coord2, bias)                    textureName.SampleBias(samplerName, coord2, bias)
	#define PLATFORM_SAMPLE_TEXTURE2D_GRAD(textureName, samplerName, coord2, dpdx, dpdy)              textureName.SampleGrad(samplerName, coord2, dpdx, dpdy)
	#define PLATFORM_SAMPLE_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)                  textureName.Sample(samplerName, float3(coord2, index))
	#define PLATFORM_SAMPLE_TEXTURE2D_ARRAY_LOD(textureName, samplerName, coord2, index, lod)         textureName.SampleLevel(samplerName, float3(coord2, index), lod)
	#define PLATFORM_SAMPLE_TEXTURE2D_ARRAY_BIAS(textureName, samplerName, coord2, index, bias)       textureName.SampleBias(samplerName, float3(coord2, index), bias)
	#define PLATFORM_SAMPLE_TEXTURE2D_ARRAY_GRAD(textureName, samplerName, coord2, index, dpdx, dpdy) textureName.SampleGrad(samplerName, float3(coord2, index), dpdx, dpdy)
	#define PLATFORM_SAMPLE_TEXTURECUBE(textureName, samplerName, coord3)                             textureName.Sample(samplerName, coord3)
	#define PLATFORM_SAMPLE_TEXTURECUBE_LOD(textureName, samplerName, coord3, lod)                    textureName.SampleLevel(samplerName, coord3, lod)
	#define PLATFORM_SAMPLE_TEXTURECUBE_BIAS(textureName, samplerName, coord3, bias)                  textureName.SampleBias(samplerName, coord3, bias)
	#define PLATFORM_SAMPLE_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)                textureName.Sample(samplerName, float4(coord3, index))
	#define PLATFORM_SAMPLE_TEXTURECUBE_ARRAY_LOD(textureName, samplerName, coord3, index, lod)       textureName.SampleLevel(samplerName, float4(coord3, index), lod)
	#define PLATFORM_SAMPLE_TEXTURECUBE_ARRAY_BIAS(textureName, samplerName, coord3, index, bias)     textureName.SampleBias(samplerName, float4(coord3, index), bias)
	#define PLATFORM_SAMPLE_TEXTURE3D(textureName, samplerName, coord3)                               textureName.Sample(samplerName, coord3)
	#define PLATFORM_SAMPLE_TEXTURE3D_LOD(textureName, samplerName, coord3, lod)                      textureName.SampleLevel(samplerName, coord3, lod)

	#define SAMPLE_TEXTURE2D(textureName, samplerName, coord2)                               PLATFORM_SAMPLE_TEXTURE2D(textureName, samplerName, coord2)
	#define SAMPLE_TEXTURE2D_LOD(textureName, samplerName, coord2, lod)                      PLATFORM_SAMPLE_TEXTURE2D_LOD(textureName, samplerName, coord2, lod)
	#define SAMPLE_TEXTURE2D_BIAS(textureName, samplerName, coord2, bias)                    PLATFORM_SAMPLE_TEXTURE2D_BIAS(textureName, samplerName, coord2, bias)
	#define SAMPLE_TEXTURE2D_GRAD(textureName, samplerName, coord2, dpdx, dpdy)              PLATFORM_SAMPLE_TEXTURE2D_GRAD(textureName, samplerName, coord2, dpdx, dpdy)
	#define SAMPLE_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)                  PLATFORM_SAMPLE_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)
	#define SAMPLE_TEXTURE2D_ARRAY_LOD(textureName, samplerName, coord2, index, lod)         PLATFORM_SAMPLE_TEXTURE2D_ARRAY_LOD(textureName, samplerName, coord2, index, lod)
	#define SAMPLE_TEXTURE2D_ARRAY_BIAS(textureName, samplerName, coord2, index, bias)       PLATFORM_SAMPLE_TEXTURE2D_ARRAY_BIAS(textureName, samplerName, coord2, index, bias)
	#define SAMPLE_TEXTURE2D_ARRAY_GRAD(textureName, samplerName, coord2, index, dpdx, dpdy) PLATFORM_SAMPLE_TEXTURE2D_ARRAY_GRAD(textureName, samplerName, coord2, index, dpdx, dpdy)
	#define SAMPLE_TEXTURECUBE(textureName, samplerName, coord3)                             PLATFORM_SAMPLE_TEXTURECUBE(textureName, samplerName, coord3)
	#define SAMPLE_TEXTURECUBE_LOD(textureName, samplerName, coord3, lod)                    PLATFORM_SAMPLE_TEXTURECUBE_LOD(textureName, samplerName, coord3, lod)
	#define SAMPLE_TEXTURECUBE_BIAS(textureName, samplerName, coord3, bias)                  PLATFORM_SAMPLE_TEXTURECUBE_BIAS(textureName, samplerName, coord3, bias)
	#define SAMPLE_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)                PLATFORM_SAMPLE_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)
	#define SAMPLE_TEXTURECUBE_ARRAY_LOD(textureName, samplerName, coord3, index, lod)       PLATFORM_SAMPLE_TEXTURECUBE_ARRAY_LOD(textureName, samplerName, coord3, index, lod)
	#define SAMPLE_TEXTURECUBE_ARRAY_BIAS(textureName, samplerName, coord3, index, bias)     PLATFORM_SAMPLE_TEXTURECUBE_ARRAY_BIAS(textureName, samplerName, coord3, index, bias)
	#define SAMPLE_TEXTURE3D(textureName, samplerName, coord3)                               PLATFORM_SAMPLE_TEXTURE3D(textureName, samplerName, coord3)
	#define SAMPLE_TEXTURE3D_LOD(textureName, samplerName, coord3, lod)                      PLATFORM_SAMPLE_TEXTURE3D_LOD(textureName, samplerName, coord3, lod)

	#define SAMPLE_TEXTURE2D_SHADOW(textureName, samplerName, coord3)                    textureName.SampleCmpLevelZero(samplerName, (coord3).xy, (coord3).z)
	#define SAMPLE_TEXTURE2D_ARRAY_SHADOW(textureName, samplerName, coord3, index)       textureName.SampleCmpLevelZero(samplerName, float3((coord3).xy, index), (coord3).z)
	#define SAMPLE_TEXTURECUBE_SHADOW(textureName, samplerName, coord4)                  textureName.SampleCmpLevelZero(samplerName, (coord4).xyz, (coord4).w)
	#define SAMPLE_TEXTURECUBE_ARRAY_SHADOW(textureName, samplerName, coord4, index)     textureName.SampleCmpLevelZero(samplerName, float4((coord4).xyz, index), (coord4).w)

	#define SAMPLE_DEPTH_TEXTURE(textureName, samplerName, coord2)          SAMPLE_TEXTURE2D(textureName, samplerName, coord2).r
	#define SAMPLE_DEPTH_TEXTURE_LOD(textureName, samplerName, coord2, lod) SAMPLE_TEXTURE2D_LOD(textureName, samplerName, coord2, lod).r

	#define LOAD_TEXTURE2D(textureName, unCoord2)                                   textureName.Load(int3(unCoord2, 0))
	#define LOAD_TEXTURE2D_LOD(textureName, unCoord2, lod)                          textureName.Load(int3(unCoord2, lod))
	#define LOAD_TEXTURE2D_MSAA(textureName, unCoord2, sampleIndex)                 textureName.Load(unCoord2, sampleIndex)
	#define LOAD_TEXTURE2D_ARRAY(textureName, unCoord2, index)                      textureName.Load(int4(unCoord2, index, 0))
	#define LOAD_TEXTURE2D_ARRAY_MSAA(textureName, unCoord2, index, sampleIndex)    textureName.Load(int3(unCoord2, index), sampleIndex)
	#define LOAD_TEXTURE2D_ARRAY_LOD(textureName, unCoord2, index, lod)             textureName.Load(int4(unCoord2, index, lod))
	#define LOAD_TEXTURE3D(textureName, unCoord3)                                   textureName.Load(int4(unCoord3, 0))
	#define LOAD_TEXTURE3D_LOD(textureName, unCoord3, lod)                          textureName.Load(int4(unCoord3, lod))

	#define PLATFORM_SUPPORT_GATHER
	#define GATHER_TEXTURE2D(textureName, samplerName, coord2)                textureName.Gather(samplerName, coord2)
	#define GATHER_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)   textureName.Gather(samplerName, float3(coord2, index))
	#define GATHER_TEXTURECUBE(textureName, samplerName, coord3)              textureName.Gather(samplerName, coord3)
	#define GATHER_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index) textureName.Gather(samplerName, float4(coord3, index))
	#define GATHER_RED_TEXTURE2D(textureName, samplerName, coord2)            textureName.GatherRed(samplerName, coord2)
	#define GATHER_GREEN_TEXTURE2D(textureName, samplerName, coord2)          textureName.GatherGreen(samplerName, coord2)
	#define GATHER_BLUE_TEXTURE2D(textureName, samplerName, coord2)           textureName.GatherBlue(samplerName, coord2)
	#define GATHER_ALPHA_TEXTURE2D(textureName, samplerName, coord2)          textureName.GatherAlpha(samplerName, coord2)
#elif defined(SHADER_API_XBOXONE)
	#define ZERO_INITIALIZE(type, name) name = (type)0;
	#define ZERO_INITIALIZE_ARRAY(type, name, arraySize) { for (int arrayIndex = 0; arrayIndex < arraySize; arrayIndex++) { name[arrayIndex] = (type)0; } }
	#define CALCULATE_TEXTURE2D_LOD(textureName, samplerName, coord2) textureName.CalculateLevelOfDetail(samplerName, coord2)
	#define TEXTURE2D(textureName)                Texture2D textureName
	#define TEXTURE2D_ARRAY(textureName)          Texture2DArray textureName
	#define TEXTURECUBE(textureName)              TextureCube textureName
	#define TEXTURECUBE_ARRAY(textureName)        TextureCubeArray textureName
	#define TEXTURE3D(textureName)                Texture3D textureName

	#define TEXTURE2D_FLOAT(textureName)          TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_FLOAT(textureName)    TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_FLOAT(textureName)        TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_FLOAT(textureName)  TEXTURECUBE_ARRAY(textureName)
	#define TEXTURE3D_FLOAT(textureName)          TEXTURE3D(textureName)

	#define TEXTURE2D_HALF(textureName)           TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_HALF(textureName)     TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_HALF(textureName)         TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_HALF(textureName)   TEXTURECUBE_ARRAY(textureName)
	#define TEXTURE3D_HALF(textureName)           TEXTURE3D(textureName)

	#define TEXTURE2D_SHADOW(textureName)         TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_SHADOW(textureName)   TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_SHADOW(textureName)       TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_SHADOW(textureName) TEXTURECUBE_ARRAY(textureName)

	#define RW_TEXTURE2D(type, textureName)       RWTexture2D<type> textureName
	#define RW_TEXTURE2D_ARRAY(type, textureName) RWTexture2DArray<type> textureName
	#define RW_TEXTURE3D(type, textureName)       RWTexture3D<type> textureName

	#define SAMPLER(samplerName)                  SamplerState samplerName
	#define SAMPLER_CMP(samplerName)              SamplerComparisonState samplerName

	#define TEXTURE2D_PARAM(textureName, samplerName)                 TEXTURE2D(textureName),         SAMPLER(samplerName)
	#define TEXTURE2D_ARRAY_PARAM(textureName, samplerName)           TEXTURE2D_ARRAY(textureName),   SAMPLER(samplerName)
	#define TEXTURECUBE_PARAM(textureName, samplerName)               TEXTURECUBE(textureName),       SAMPLER(samplerName)
	#define TEXTURECUBE_ARRAY_PARAM(textureName, samplerName)         TEXTURECUBE_ARRAY(textureName), SAMPLER(samplerName)
	#define TEXTURE3D_PARAM(textureName, samplerName)                 TEXTURE3D(textureName),         SAMPLER(samplerName)

	#define TEXTURE2D_SHADOW_PARAM(textureName, samplerName)          TEXTURE2D(textureName),         SAMPLER_CMP(samplerName)
	#define TEXTURE2D_ARRAY_SHADOW_PARAM(textureName, samplerName)    TEXTURE2D_ARRAY(textureName),   SAMPLER_CMP(samplerName)
	#define TEXTURECUBE_SHADOW_PARAM(textureName, samplerName)        TEXTURECUBE(textureName),       SAMPLER_CMP(samplerName)
	#define TEXTURECUBE_ARRAY_SHADOW_PARAM(textureName, samplerName)  TEXTURECUBE_ARRAY(textureName), SAMPLER_CMP(samplerName)

	#define TEXTURE2D_ARGS(textureName, samplerName)                textureName, samplerName
	#define TEXTURE2D_ARRAY_ARGS(textureName, samplerName)          textureName, samplerName
	#define TEXTURECUBE_ARGS(textureName, samplerName)              textureName, samplerName
	#define TEXTURECUBE_ARRAY_ARGS(textureName, samplerName)        textureName, samplerName
	#define TEXTURE3D_ARGS(textureName, samplerName)                textureName, samplerName

	#define TEXTURE2D_SHADOW_ARGS(textureName, samplerName)         textureName, samplerName
	#define TEXTURE2D_ARRAY_SHADOW_ARGS(textureName, samplerName)   textureName, samplerName
	#define TEXTURECUBE_SHADOW_ARGS(textureName, samplerName)       textureName, samplerName
	#define TEXTURECUBE_ARRAY_SHADOW_ARGS(textureName, samplerName) textureName, samplerName

	#define SAMPLE_TEXTURE2D(textureName, samplerName, coord2)                               textureName.Sample(samplerName, coord2)
	#define SAMPLE_TEXTURE2D_LOD(textureName, samplerName, coord2, lod)                      textureName.SampleLevel(samplerName, coord2, lod)
	#define SAMPLE_TEXTURE2D_BIAS(textureName, samplerName, coord2, bias)                    textureName.SampleBias(samplerName, coord2, bias)
	#define SAMPLE_TEXTURE2D_GRAD(textureName, samplerName, coord2, dpdx, dpdy)              textureName.SampleGrad(samplerName, coord2, dpdx, dpdy)
	#define SAMPLE_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)                  textureName.Sample(samplerName, float3(coord2, index))
	#define SAMPLE_TEXTURE2D_ARRAY_LOD(textureName, samplerName, coord2, index, lod)         textureName.SampleLevel(samplerName, float3(coord2, index), lod)
	#define SAMPLE_TEXTURE2D_ARRAY_BIAS(textureName, samplerName, coord2, index, bias)       textureName.SampleBias(samplerName, float3(coord2, index), bias)
	#define SAMPLE_TEXTURE2D_ARRAY_GRAD(textureName, samplerName, coord2, index, dpdx, dpdy) textureName.SampleGrad(samplerName, float3(coord2, index), dpdx, dpdy)
	#define SAMPLE_TEXTURECUBE(textureName, samplerName, coord3)                             textureName.Sample(samplerName, coord3)
	#define SAMPLE_TEXTURECUBE_LOD(textureName, samplerName, coord3, lod)                    textureName.SampleLevel(samplerName, coord3, lod)
	#define SAMPLE_TEXTURECUBE_BIAS(textureName, samplerName, coord3, bias)                  textureName.SampleBias(samplerName, coord3, bias)
	#define SAMPLE_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)                textureName.Sample(samplerName, float4(coord3, index))
	#define SAMPLE_TEXTURECUBE_ARRAY_LOD(textureName, samplerName, coord3, index, lod)       textureName.SampleLevel(samplerName, float4(coord3, index), lod)
	#define SAMPLE_TEXTURECUBE_ARRAY_BIAS(textureName, samplerName, coord3, index, bias)     textureName.SampleBias(samplerName, float4(coord3, index), bias)
	#define SAMPLE_TEXTURE3D(textureName, samplerName, coord3)                               textureName.Sample(samplerName, coord3)
	#define SAMPLE_TEXTURE3D_LOD(textureName, samplerName, coord3, lod)                      textureName.SampleLevel(samplerName, coord3, lod)

	#define SAMPLE_TEXTURE2D_SHADOW(textureName, samplerName, coord3)                    textureName.SampleCmpLevelZero(samplerName, (coord3).xy, (coord3).z)
	#define SAMPLE_TEXTURE2D_ARRAY_SHADOW(textureName, samplerName, coord3, index)       textureName.SampleCmpLevelZero(samplerName, float3((coord3).xy, index), (coord3).z)
	#define SAMPLE_TEXTURECUBE_SHADOW(textureName, samplerName, coord4)                  textureName.SampleCmpLevelZero(samplerName, (coord4).xyz, (coord4).w)
	#define SAMPLE_TEXTURECUBE_ARRAY_SHADOW(textureName, samplerName, coord4, index)     textureName.SampleCmpLevelZero(samplerName, float4((coord4).xyz, index), (coord4).w)

	#define LOAD_TEXTURE2D(textureName, unCoord2)                                   textureName.Load(int3(unCoord2, 0))
	#define LOAD_TEXTURE2D_LOD(textureName, unCoord2, lod)                          textureName.Load(int3(unCoord2, lod))
	#define LOAD_TEXTURE2D_MSAA(textureName, unCoord2, sampleIndex)                 textureName.Load(unCoord2, sampleIndex)
	#define LOAD_TEXTURE2D_ARRAY(textureName, unCoord2, index)                      textureName.Load(int4(unCoord2, index, 0))
	#define LOAD_TEXTURE2D_ARRAY_MSAA(textureName, unCoord2, index, sampleIndex)    textureName.Load(int3(unCoord2, index), sampleIndex)
	#define LOAD_TEXTURE2D_ARRAY_LOD(textureName, unCoord2, index, lod)             textureName.Load(int4(unCoord2, index, lod))
	#define LOAD_TEXTURE3D(textureName, unCoord3)                                   textureName.Load(int4(unCoord3, 0))
	#define LOAD_TEXTURE3D_LOD(textureName, unCoord3, lod)                          textureName.Load(int4(unCoord3, lod))

	#define PLATFORM_SUPPORT_GATHER
	#define GATHER_TEXTURE2D(textureName, samplerName, coord2)                textureName.Gather(samplerName, coord2)
	#define GATHER_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)   textureName.Gather(samplerName, float3(coord2, index))
	#define GATHER_TEXTURECUBE(textureName, samplerName, coord3)              textureName.Gather(samplerName, coord3)
	#define GATHER_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index) textureName.Gather(samplerName, float4(coord3, index))
	#define GATHER_RED_TEXTURE2D(textureName, samplerName, coord2)            textureName.GatherRed(samplerName, coord2)
	#define GATHER_GREEN_TEXTURE2D(textureName, samplerName, coord2)          textureName.GatherGreen(samplerName, coord2)
	#define GATHER_BLUE_TEXTURE2D(textureName, samplerName, coord2)           textureName.GatherBlue(samplerName, coord2)
	#define GATHER_ALPHA_TEXTURE2D(textureName, samplerName, coord2)          textureName.GatherAlpha(samplerName, coord2)
#elif defined(SHADER_API_PSSL)
	#define ZERO_INITIALIZE(type, name) name = (type)0;
	#define ZERO_INITIALIZE_ARRAY(type, name, arraySize) { for (int arrayIndex = 0; arrayIndex < arraySize; arrayIndex++) { name[arrayIndex] = (type)0; } }
	#define CALCULATE_TEXTURE2D_LOD(textureName, samplerName, coord2) textureName.GetLOD(samplerName, coord2)
	#define TEXTURE2D(textureName)                Texture2D textureName
	#define TEXTURE2D_ARRAY(textureName)          Texture2DArray textureName
	#define TEXTURECUBE(textureName)              TextureCube textureName
	#define TEXTURECUBE_ARRAY(textureName)        TextureCubeArray textureName
	#define TEXTURE3D(textureName)                Texture3D textureName

	#define TEXTURE2D_FLOAT(textureName)          TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_FLOAT(textureName)    TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_FLOAT(textureName)        TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_FLOAT(textureName)  TEXTURECUBE_ARRAY(textureName)
	#define TEXTURE3D_FLOAT(textureName)          TEXTURE3D(textureName)

	#define TEXTURE2D_HALF(textureName)           TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_HALF(textureName)     TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_HALF(textureName)         TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_HALF(textureName)   TEXTURECUBE_ARRAY(textureName)
	#define TEXTURE3D_HALF(textureName)           TEXTURE3D(textureName)

	#define TEXTURE2D_SHADOW(textureName)         TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_SHADOW(textureName)   TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_SHADOW(textureName)       TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_SHADOW(textureName) TEXTURECUBE_ARRAY(textureName)

	#define RW_TEXTURE2D(type, textureName)       RW_Texture2D<type> textureName
	#define RW_TEXTURE2D_ARRAY(type, textureName) RW_Texture2D_Array<type> textureName
	#define RW_TEXTURE3D(type, textureName)       RW_Texture3D<type> textureName

	#define SAMPLER(samplerName)                  SamplerState samplerName
	#define SAMPLER_CMP(samplerName)              SamplerComparisonState samplerName

	#define TEXTURE2D_PARAM(textureName, samplerName)                 TEXTURE2D(textureName),         SAMPLER(samplerName)
	#define TEXTURE2D_ARRAY_PARAM(textureName, samplerName)           TEXTURE2D_ARRAY(textureName),   SAMPLER(samplerName)
	#define TEXTURECUBE_PARAM(textureName, samplerName)               TEXTURECUBE(textureName),       SAMPLER(samplerName)
	#define TEXTURECUBE_ARRAY_PARAM(textureName, samplerName)         TEXTURECUBE_ARRAY(textureName), SAMPLER(samplerName)
	#define TEXTURE3D_PARAM(textureName, samplerName)                 TEXTURE3D(textureName),         SAMPLER(samplerName)

	#define TEXTURE2D_SHADOW_PARAM(textureName, samplerName)          TEXTURE2D(textureName),         SAMPLER_CMP(samplerName)
	#define TEXTURE2D_ARRAY_SHADOW_PARAM(textureName, samplerName)    TEXTURE2D_ARRAY(textureName),   SAMPLER_CMP(samplerName)
	#define TEXTURECUBE_SHADOW_PARAM(textureName, samplerName)        TEXTURECUBE(textureName),       SAMPLER_CMP(samplerName)
	#define TEXTURECUBE_ARRAY_SHADOW_PARAM(textureName, samplerName)  TEXTURECUBE_ARRAY(textureName), SAMPLER_CMP(samplerName)

	#define TEXTURE2D_ARGS(textureName, samplerName)                textureName, samplerName
	#define TEXTURE2D_ARRAY_ARGS(textureName, samplerName)          textureName, samplerName
	#define TEXTURECUBE_ARGS(textureName, samplerName)              textureName, samplerName
	#define TEXTURECUBE_ARRAY_ARGS(textureName, samplerName)        textureName, samplerName
	#define TEXTURE3D_ARGS(textureName, samplerName)                textureName, samplerName

	#define TEXTURE2D_SHADOW_ARGS(textureName, samplerName)         textureName, samplerName
	#define TEXTURE2D_ARRAY_SHADOW_ARGS(textureName, samplerName)   textureName, samplerName
	#define TEXTURECUBE_SHADOW_ARGS(textureName, samplerName)       textureName, samplerName
	#define TEXTURECUBE_ARRAY_SHADOW_ARGS(textureName, samplerName) textureName, samplerName

	#define SAMPLE_TEXTURE2D(textureName, samplerName, coord2)                               textureName.Sample(samplerName, coord2)
	#define SAMPLE_TEXTURE2D_LOD(textureName, samplerName, coord2, lod)                      textureName.SampleLevel(samplerName, coord2, lod)
	#define SAMPLE_TEXTURE2D_BIAS(textureName, samplerName, coord2, bias)                    textureName.SampleBias(samplerName, coord2, bias)
	#define SAMPLE_TEXTURE2D_GRAD(textureName, samplerName, coord2, dpdx, dpdy)              textureName.SampleGrad(samplerName, coord2, dpdx, dpdy)
	#define SAMPLE_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)                  textureName.Sample(samplerName, float3(coord2, index))
	#define SAMPLE_TEXTURE2D_ARRAY_LOD(textureName, samplerName, coord2, index, lod)         textureName.SampleLevel(samplerName, float3(coord2, index), lod)
	#define SAMPLE_TEXTURE2D_ARRAY_BIAS(textureName, samplerName, coord2, index, bias)       textureName.SampleBias(samplerName, float3(coord2, index), bias)
	#define SAMPLE_TEXTURE2D_ARRAY_GRAD(textureName, samplerName, coord2, index, dpdx, dpdy) textureName.SampleGrad(samplerName, float3(coord2, index), dpdx, dpdy)
	#define SAMPLE_TEXTURECUBE(textureName, samplerName, coord3)                             textureName.Sample(samplerName, coord3)
	#define SAMPLE_TEXTURECUBE_LOD(textureName, samplerName, coord3, lod)                    textureName.SampleLevel(samplerName, coord3, lod)
	#define SAMPLE_TEXTURECUBE_BIAS(textureName, samplerName, coord3, bias)                  textureName.SampleBias(samplerName, coord3, bias)
	#define SAMPLE_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)                textureName.Sample(samplerName, float4(coord3, index))
	#define SAMPLE_TEXTURECUBE_ARRAY_LOD(textureName, samplerName, coord3, index, lod)       textureName.SampleLevel(samplerName, float4(coord3, index), lod)
	#define SAMPLE_TEXTURECUBE_ARRAY_BIAS(textureName, samplerName, coord3, index, bias)     textureName.SampleBias(samplerName, float4(coord3, index), bias)
	#define SAMPLE_TEXTURE3D(textureName, samplerName, coord3)                               textureName.Sample(samplerName, coord3)
	#define SAMPLE_TEXTURE3D_LOD(textureName, samplerName, coord3, lod)                      textureName.SampleLevel(samplerName, coord3, lod)

	#define SAMPLE_TEXTURE2D_SHADOW(textureName, samplerName, coord3)                    textureName.SampleCmpLevelZero(samplerName, (coord3).xy, (coord3).z)
	#define SAMPLE_TEXTURE2D_ARRAY_SHADOW(textureName, samplerName, coord3, index)       textureName.SampleCmpLevelZero(samplerName, float3((coord3).xy, index), (coord3).z)
	#define SAMPLE_TEXTURECUBE_SHADOW(textureName, samplerName, coord4)                  textureName.SampleCmpLevelZero(samplerName, (coord4).xyz, (coord4).w)
	#define SAMPLE_TEXTURECUBE_ARRAY_SHADOW(textureName, samplerName, coord4, index)     textureName.SampleCmpLevelZero(samplerName, float4((coord4).xyz, index), (coord4).w)

	#define LOAD_TEXTURE2D(textureName, unCoord2)                                   textureName.Load(int3(unCoord2, 0))
	#define LOAD_TEXTURE2D_LOD(textureName, unCoord2, lod)                          textureName.Load(int3(unCoord2, lod))
	#define LOAD_TEXTURE2D_MSAA(textureName, unCoord2, sampleIndex)                 textureName.Load(unCoord2, sampleIndex)
	#define LOAD_TEXTURE2D_ARRAY(textureName, unCoord2, index)                      textureName.Load(int4(unCoord2, index, 0))
	#define LOAD_TEXTURE2D_ARRAY_MSAA(textureName, unCoord2, index, sampleIndex)    textureName.Load(int3(unCoord2, index), sampleIndex)
	#define LOAD_TEXTURE2D_ARRAY_LOD(textureName, unCoord2, index, lod)             textureName.Load(int4(unCoord2, index, lod))
	#define LOAD_TEXTURE3D(textureName, unCoord3)                                   textureName.Load(int4(unCoord3, 0))
	#define LOAD_TEXTURE3D_LOD(textureName, unCoord3, lod)                          textureName.Load(int4(unCoord3, lod))

	#define PLATFORM_SUPPORT_GATHER
	#define GATHER_TEXTURE2D(textureName, samplerName, coord2)                textureName.Gather(samplerName, coord2)
	#define GATHER_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)   textureName.Gather(samplerName, float3(coord2, index))
	#define GATHER_TEXTURECUBE(textureName, samplerName, coord3)              textureName.Gather(samplerName, coord3)
	#define GATHER_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index) textureName.Gather(samplerName, float4(coord3, index))
	#define GATHER_RED_TEXTURE2D(textureName, samplerName, coord2)            textureName.GatherRed(samplerName, coord2)
	#define GATHER_GREEN_TEXTURE2D(textureName, samplerName, coord2)          textureName.GatherGreen(samplerName, coord2)
	#define GATHER_BLUE_TEXTURE2D(textureName, samplerName, coord2)           textureName.GatherBlue(samplerName, coord2)
	#define GATHER_ALPHA_TEXTURE2D(textureName, samplerName, coord2)          textureName.GatherAlpha(samplerName, coord2)
#elif defined(SHADER_API_D3D11)
	#define ZERO_INITIALIZE(type, name) name = (type)0;
	#define ZERO_INITIALIZE_ARRAY(type, name, arraySize) { for (int arrayIndex = 0; arrayIndex < arraySize; arrayIndex++) { name[arrayIndex] = (type)0; } }
	#define CALCULATE_TEXTURE2D_LOD(textureName, samplerName, coord2) textureName.CalculateLevelOfDetail(samplerName, coord2)
	#define TEXTURE2D(textureName)                Texture2D textureName
	#define TEXTURE2D_ARRAY(textureName)          Texture2DArray textureName
	#define TEXTURECUBE(textureName)              TextureCube textureName
	#define TEXTURECUBE_ARRAY(textureName)        TextureCubeArray textureName
	#define TEXTURE3D(textureName)                Texture3D textureName

	#define TEXTURE2D_FLOAT(textureName)          TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_FLOAT(textureName)    TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_FLOAT(textureName)        TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_FLOAT(textureName)  TEXTURECUBE_ARRAY(textureName)
	#define TEXTURE3D_FLOAT(textureName)          TEXTURE3D(textureName)

	#define TEXTURE2D_HALF(textureName)           TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_HALF(textureName)     TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_HALF(textureName)         TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_HALF(textureName)   TEXTURECUBE_ARRAY(textureName)
	#define TEXTURE3D_HALF(textureName)           TEXTURE3D(textureName)

	#define TEXTURE2D_SHADOW(textureName)         TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_SHADOW(textureName)   TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_SHADOW(textureName)       TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_SHADOW(textureName) TEXTURECUBE_ARRAY(textureName)

	#define RW_TEXTURE2D(type, textureName)       RWTexture2D<type> textureName
	#define RW_TEXTURE2D_ARRAY(type, textureName) RWTexture2DArray<type> textureName
	#define RW_TEXTURE3D(type, textureName)       RWTexture3D<type> textureName

	#define SAMPLER(samplerName)                  SamplerState samplerName
	#define SAMPLER_CMP(samplerName)              SamplerComparisonState samplerName

	#define TEXTURE2D_PARAM(textureName, samplerName)                 TEXTURE2D(textureName),         SAMPLER(samplerName)
	#define TEXTURE2D_ARRAY_PARAM(textureName, samplerName)           TEXTURE2D_ARRAY(textureName),   SAMPLER(samplerName)
	#define TEXTURECUBE_PARAM(textureName, samplerName)               TEXTURECUBE(textureName),       SAMPLER(samplerName)
	#define TEXTURECUBE_ARRAY_PARAM(textureName, samplerName)         TEXTURECUBE_ARRAY(textureName), SAMPLER(samplerName)
	#define TEXTURE3D_PARAM(textureName, samplerName)                 TEXTURE3D(textureName),         SAMPLER(samplerName)

	#define TEXTURE2D_SHADOW_PARAM(textureName, samplerName)          TEXTURE2D(textureName),         SAMPLER_CMP(samplerName)
	#define TEXTURE2D_ARRAY_SHADOW_PARAM(textureName, samplerName)    TEXTURE2D_ARRAY(textureName),   SAMPLER_CMP(samplerName)
	#define TEXTURECUBE_SHADOW_PARAM(textureName, samplerName)        TEXTURECUBE(textureName),       SAMPLER_CMP(samplerName)
	#define TEXTURECUBE_ARRAY_SHADOW_PARAM(textureName, samplerName)  TEXTURECUBE_ARRAY(textureName), SAMPLER_CMP(samplerName)

	#define TEXTURE2D_ARGS(textureName, samplerName)                textureName, samplerName
	#define TEXTURE2D_ARRAY_ARGS(textureName, samplerName)          textureName, samplerName
	#define TEXTURECUBE_ARGS(textureName, samplerName)              textureName, samplerName
	#define TEXTURECUBE_ARRAY_ARGS(textureName, samplerName)        textureName, samplerName
	#define TEXTURE3D_ARGS(textureName, samplerName)                textureName, samplerName

	#define TEXTURE2D_SHADOW_ARGS(textureName, samplerName)         textureName, samplerName
	#define TEXTURE2D_ARRAY_SHADOW_ARGS(textureName, samplerName)   textureName, samplerName
	#define TEXTURECUBE_SHADOW_ARGS(textureName, samplerName)       textureName, samplerName
	#define TEXTURECUBE_ARRAY_SHADOW_ARGS(textureName, samplerName) textureName, samplerName

	#define SAMPLE_TEXTURE2D(textureName, samplerName, coord2)                               textureName.Sample(samplerName, coord2)
	#define SAMPLE_TEXTURE2D_LOD(textureName, samplerName, coord2, lod)                      textureName.SampleLevel(samplerName, coord2, lod)
	#define SAMPLE_TEXTURE2D_BIAS(textureName, samplerName, coord2, bias)                    textureName.SampleBias(samplerName, coord2, bias)
	#define SAMPLE_TEXTURE2D_GRAD(textureName, samplerName, coord2, dpdx, dpdy)              textureName.SampleGrad(samplerName, coord2, dpdx, dpdy)
	#define SAMPLE_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)                  textureName.Sample(samplerName, float3(coord2, index))
	#define SAMPLE_TEXTURE2D_ARRAY_LOD(textureName, samplerName, coord2, index, lod)         textureName.SampleLevel(samplerName, float3(coord2, index), lod)
	#define SAMPLE_TEXTURE2D_ARRAY_BIAS(textureName, samplerName, coord2, index, bias)       textureName.SampleBias(samplerName, float3(coord2, index), bias)
	#define SAMPLE_TEXTURE2D_ARRAY_GRAD(textureName, samplerName, coord2, index, dpdx, dpdy) textureName.SampleGrad(samplerName, float3(coord2, index), dpdx, dpdy)
	#define SAMPLE_TEXTURECUBE(textureName, samplerName, coord3)                             textureName.Sample(samplerName, coord3)
	#define SAMPLE_TEXTURECUBE_LOD(textureName, samplerName, coord3, lod)                    textureName.SampleLevel(samplerName, coord3, lod)
	#define SAMPLE_TEXTURECUBE_BIAS(textureName, samplerName, coord3, bias)                  textureName.SampleBias(samplerName, coord3, bias)
	#define SAMPLE_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)                textureName.Sample(samplerName, float4(coord3, index))
	#define SAMPLE_TEXTURECUBE_ARRAY_LOD(textureName, samplerName, coord3, index, lod)       textureName.SampleLevel(samplerName, float4(coord3, index), lod)
	#define SAMPLE_TEXTURECUBE_ARRAY_BIAS(textureName, samplerName, coord3, index, bias)     textureName.SampleBias(samplerName, float4(coord3, index), bias)
	#define SAMPLE_TEXTURE3D(textureName, samplerName, coord3)                               textureName.Sample(samplerName, coord3)
	#define SAMPLE_TEXTURE3D_LOD(textureName, samplerName, coord3, lod)                      textureName.SampleLevel(samplerName, coord3, lod)

	#define SAMPLE_TEXTURE2D_SHADOW(textureName, samplerName, coord3)                    textureName.SampleCmpLevelZero(samplerName, (coord3).xy, (coord3).z)
	#define SAMPLE_TEXTURE2D_ARRAY_SHADOW(textureName, samplerName, coord3, index)       textureName.SampleCmpLevelZero(samplerName, float3((coord3).xy, index), (coord3).z)
	#define SAMPLE_TEXTURECUBE_SHADOW(textureName, samplerName, coord4)                  textureName.SampleCmpLevelZero(samplerName, (coord4).xyz, (coord4).w)
	#define SAMPLE_TEXTURECUBE_ARRAY_SHADOW(textureName, samplerName, coord4, index)     textureName.SampleCmpLevelZero(samplerName, float4((coord4).xyz, index), (coord4).w)

	#define LOAD_TEXTURE2D(textureName, unCoord2)                                   textureName.Load(int3(unCoord2, 0))
	#define LOAD_TEXTURE2D_LOD(textureName, unCoord2, lod)                          textureName.Load(int3(unCoord2, lod))
	#define LOAD_TEXTURE2D_MSAA(textureName, unCoord2, sampleIndex)                 textureName.Load(unCoord2, sampleIndex)
	#define LOAD_TEXTURE2D_ARRAY(textureName, unCoord2, index)                      textureName.Load(int4(unCoord2, index, 0))
	#define LOAD_TEXTURE2D_ARRAY_MSAA(textureName, unCoord2, index, sampleIndex)    textureName.Load(int3(unCoord2, index), sampleIndex)
	#define LOAD_TEXTURE2D_ARRAY_LOD(textureName, unCoord2, index, lod)             textureName.Load(int4(unCoord2, index, lod))
	#define LOAD_TEXTURE3D(textureName, unCoord3)                                   textureName.Load(int4(unCoord3, 0))
	#define LOAD_TEXTURE3D_LOD(textureName, unCoord3, lod)                          textureName.Load(int4(unCoord3, lod))

	#define PLATFORM_SUPPORT_GATHER
	#define GATHER_TEXTURE2D(textureName, samplerName, coord2)                textureName.Gather(samplerName, coord2)
	#define GATHER_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)   textureName.Gather(samplerName, float3(coord2, index))
	#define GATHER_TEXTURECUBE(textureName, samplerName, coord3)              textureName.Gather(samplerName, coord3)
	#define GATHER_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index) textureName.Gather(samplerName, float4(coord3, index))
	#define GATHER_RED_TEXTURE2D(textureName, samplerName, coord2)            textureName.GatherRed(samplerName, coord2)
	#define GATHER_GREEN_TEXTURE2D(textureName, samplerName, coord2)          textureName.GatherGreen(samplerName, coord2)
	#define GATHER_BLUE_TEXTURE2D(textureName, samplerName, coord2)           textureName.GatherBlue(samplerName, coord2)
	#define GATHER_ALPHA_TEXTURE2D(textureName, samplerName, coord2)          textureName.GatherAlpha(samplerName, coord2)

#elif defined(SHADER_API_METAL)
	#define ZERO_INITIALIZE(type, name) name = (type)0;
	#define ZERO_INITIALIZE_ARRAY(type, name, arraySize) { for (int arrayIndex = 0; arrayIndex < arraySize; arrayIndex++) { name[arrayIndex] = (type)0; } }
	#define CALCULATE_TEXTURE2D_LOD(textureName, samplerName, coord2) textureName.CalculateLevelOfDetail(samplerName, coord2)
	#define TEXTURE2D(textureName)                Texture2D textureName
	#define TEXTURE2D_ARRAY(textureName)          Texture2DArray textureName
	#define TEXTURECUBE(textureName)              TextureCube textureName
	#define TEXTURECUBE_ARRAY(textureName)        TextureCubeArray textureName
	#define TEXTURE3D(textureName)                Texture3D textureName

	#define TEXTURE2D_FLOAT(textureName)          Texture2D_float textureName
	#define TEXTURE2D_ARRAY_FLOAT(textureName)    Texture2DArray textureName    
	#define TEXTURECUBE_FLOAT(textureName)        TextureCube_float textureName
	#define TEXTURECUBE_ARRAY_FLOAT(textureName)  TextureCubeArray textureName  
	#define TEXTURE3D_FLOAT(textureName)          Texture3D_float textureName

	#define TEXTURE2D_HALF(textureName)           Texture2D_half textureName
	#define TEXTURE2D_ARRAY_HALF(textureName)     Texture2DArray textureName    
	#define TEXTURECUBE_HALF(textureName)         TextureCube_half textureName
	#define TEXTURECUBE_ARRAY_HALF(textureName)   TextureCubeArray textureName  
	#define TEXTURE3D_HALF(textureName)           Texture3D_half textureName

	#define TEXTURE2D_SHADOW(textureName)         TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_SHADOW(textureName)   TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_SHADOW(textureName)       TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_SHADOW(textureName) TEXTURECUBE_ARRAY(textureName)

	#define RW_TEXTURE2D(type, textureName)       RWTexture2D<type> textureName
	#define RW_TEXTURE2D_ARRAY(type, textureName) RWTexture2DArray<type> textureName
	#define RW_TEXTURE3D(type, textureName)       RWTexture3D<type> textureName

	#define SAMPLER(samplerName)                  SamplerState samplerName
	#define SAMPLER_CMP(samplerName)              SamplerComparisonState samplerName

	#define TEXTURE2D_PARAM(textureName, samplerName)                 TEXTURE2D(textureName),         SAMPLER(samplerName)
	#define TEXTURE2D_ARRAY_PARAM(textureName, samplerName)           TEXTURE2D_ARRAY(textureName),   SAMPLER(samplerName)
	#define TEXTURECUBE_PARAM(textureName, samplerName)               TEXTURECUBE(textureName),       SAMPLER(samplerName)
	#define TEXTURECUBE_ARRAY_PARAM(textureName, samplerName)         TEXTURECUBE_ARRAY(textureName), SAMPLER(samplerName)
	#define TEXTURE3D_PARAM(textureName, samplerName)                 TEXTURE3D(textureName),         SAMPLER(samplerName)

	#define TEXTURE2D_SHADOW_PARAM(textureName, samplerName)          TEXTURE2D(textureName),         SAMPLER_CMP(samplerName)
	#define TEXTURE2D_ARRAY_SHADOW_PARAM(textureName, samplerName)    TEXTURE2D_ARRAY(textureName),   SAMPLER_CMP(samplerName)
	#define TEXTURECUBE_SHADOW_PARAM(textureName, samplerName)        TEXTURECUBE(textureName),       SAMPLER_CMP(samplerName)
	#define TEXTURECUBE_ARRAY_SHADOW_PARAM(textureName, samplerName)  TEXTURECUBE_ARRAY(textureName), SAMPLER_CMP(samplerName)

	#define TEXTURE2D_ARGS(textureName, samplerName)                textureName, samplerName
	#define TEXTURE2D_ARRAY_ARGS(textureName, samplerName)          textureName, samplerName
	#define TEXTURECUBE_ARGS(textureName, samplerName)              textureName, samplerName
	#define TEXTURECUBE_ARRAY_ARGS(textureName, samplerName)        textureName, samplerName
	#define TEXTURE3D_ARGS(textureName, samplerName)                textureName, samplerName

	#define TEXTURE2D_SHADOW_ARGS(textureName, samplerName)         textureName, samplerName
	#define TEXTURE2D_ARRAY_SHADOW_ARGS(textureName, samplerName)   textureName, samplerName
	#define TEXTURECUBE_SHADOW_ARGS(textureName, samplerName)       textureName, samplerName
	#define TEXTURECUBE_ARRAY_SHADOW_ARGS(textureName, samplerName) textureName, samplerName

	#define SAMPLE_TEXTURE2D(textureName, samplerName, coord2)                               textureName.Sample(samplerName, coord2)
	#define SAMPLE_TEXTURE2D_LOD(textureName, samplerName, coord2, lod)                      textureName.SampleLevel(samplerName, coord2, lod)
	#define SAMPLE_TEXTURE2D_BIAS(textureName, samplerName, coord2, bias)                    textureName.SampleBias(samplerName, coord2, bias)
	#define SAMPLE_TEXTURE2D_GRAD(textureName, samplerName, coord2, dpdx, dpdy)              textureName.SampleGrad(samplerName, coord2, dpdx, dpdy)
	#define SAMPLE_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)                  textureName.Sample(samplerName, float3(coord2, index))
	#define SAMPLE_TEXTURE2D_ARRAY_LOD(textureName, samplerName, coord2, index, lod)         textureName.SampleLevel(samplerName, float3(coord2, index), lod)
	#define SAMPLE_TEXTURE2D_ARRAY_BIAS(textureName, samplerName, coord2, index, bias)       textureName.SampleBias(samplerName, float3(coord2, index), bias)
	#define SAMPLE_TEXTURE2D_ARRAY_GRAD(textureName, samplerName, coord2, index, dpdx, dpdy) textureName.SampleGrad(samplerName, float3(coord2, index), dpdx, dpdy)
	#define SAMPLE_TEXTURECUBE(textureName, samplerName, coord3)                             textureName.Sample(samplerName, coord3)
	#define SAMPLE_TEXTURECUBE_LOD(textureName, samplerName, coord3, lod)                    textureName.SampleLevel(samplerName, coord3, lod)
	#define SAMPLE_TEXTURECUBE_BIAS(textureName, samplerName, coord3, bias)                  textureName.SampleBias(samplerName, coord3, bias)
	#define SAMPLE_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)                textureName.Sample(samplerName, float4(coord3, index))
	#define SAMPLE_TEXTURECUBE_ARRAY_LOD(textureName, samplerName, coord3, index, lod)       textureName.SampleLevel(samplerName, float4(coord3, index), lod)
	#define SAMPLE_TEXTURECUBE_ARRAY_BIAS(textureName, samplerName, coord3, index, bias)     textureName.SampleBias(samplerName, float4(coord3, index), bias)
	#define SAMPLE_TEXTURE3D(textureName, samplerName, coord3)                               textureName.Sample(samplerName, coord3)
	#define SAMPLE_TEXTURE3D_LOD(textureName, samplerName, coord3, lod)                      textureName.SampleLevel(samplerName, coord3, lod)

	#define SAMPLE_TEXTURE2D_SHADOW(textureName, samplerName, coord3)                    textureName.SampleCmpLevelZero(samplerName, (coord3).xy, (coord3).z)
	#define SAMPLE_TEXTURE2D_ARRAY_SHADOW(textureName, samplerName, coord3, index)       textureName.SampleCmpLevelZero(samplerName, float3((coord3).xy, index), (coord3).z)
	#define SAMPLE_TEXTURECUBE_SHADOW(textureName, samplerName, coord4)                  textureName.SampleCmpLevelZero(samplerName, (coord4).xyz, (coord4).w)
	#define SAMPLE_TEXTURECUBE_ARRAY_SHADOW(textureName, samplerName, coord4, index)     textureName.SampleCmpLevelZero(samplerName, float4((coord4).xyz, index), (coord4).w)

	#define LOAD_TEXTURE2D(textureName, unCoord2)                                   textureName.Load(int3(unCoord2, 0))
	#define LOAD_TEXTURE2D_LOD(textureName, unCoord2, lod)                          textureName.Load(int3(unCoord2, lod))
	#define LOAD_TEXTURE2D_MSAA(textureName, unCoord2, sampleIndex)                 textureName.Load(unCoord2, sampleIndex)
	#define LOAD_TEXTURE2D_ARRAY(textureName, unCoord2, index)                      textureName.Load(int4(unCoord2, index, 0))
	#define LOAD_TEXTURE2D_ARRAY_MSAA(textureName, unCoord2, index, sampleIndex)    textureName.Load(int3(unCoord2, index), sampleIndex)
	#define LOAD_TEXTURE2D_ARRAY_LOD(textureName, unCoord2, index, lod)             textureName.Load(int4(unCoord2, index, lod))
	#define LOAD_TEXTURE3D(textureName, unCoord3)                                   textureName.Load(int4(unCoord3, 0))
	#define LOAD_TEXTURE3D_LOD(textureName, unCoord3, lod)                          textureName.Load(int4(unCoord3, lod))

	#define PLATFORM_SUPPORT_GATHER
	#define GATHER_TEXTURE2D(textureName, samplerName, coord2)                textureName.Gather(samplerName, coord2)
	#define GATHER_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)   textureName.Gather(samplerName, float3(coord2, index))
	#define GATHER_TEXTURECUBE(textureName, samplerName, coord3)              textureName.Gather(samplerName, coord3)
	#define GATHER_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index) textureName.Gather(samplerName, float4(coord3, index))
	#define GATHER_RED_TEXTURE2D(textureName, samplerName, coord2)            textureName.GatherRed(samplerName, coord2)
	#define GATHER_GREEN_TEXTURE2D(textureName, samplerName, coord2)          textureName.GatherGreen(samplerName, coord2)
	#define GATHER_BLUE_TEXTURE2D(textureName, samplerName, coord2)           textureName.GatherBlue(samplerName, coord2)
	#define GATHER_ALPHA_TEXTURE2D(textureName, samplerName, coord2)          textureName.GatherAlpha(samplerName, coord2)

#elif defined(SHADER_API_VULKAN)
	#define ZERO_INITIALIZE(type, name) name = (type)0;
	#define ZERO_INITIALIZE_ARRAY(type, name, arraySize) { for (int arrayIndex = 0; arrayIndex < arraySize; arrayIndex++) { name[arrayIndex] = (type)0; } }
	#define CALCULATE_TEXTURE2D_LOD(textureName, samplerName, coord2) textureName.CalculateLevelOfDetail(samplerName, coord2)
	#define TEXTURE2D(textureName)                Texture2D textureName
	#define TEXTURE2D_ARRAY(textureName)          Texture2DArray textureName
	#define TEXTURECUBE(textureName)              TextureCube textureName
	#define TEXTURECUBE_ARRAY(textureName)        TextureCubeArray textureName
	#define TEXTURE3D(textureName)                Texture3D textureName

	#define TEXTURE2D_FLOAT(textureName)          Texture2D_float textureName
	#define TEXTURE2D_ARRAY_FLOAT(textureName)    Texture2DArray textureName    
	#define TEXTURECUBE_FLOAT(textureName)        TextureCube_float textureName
	#define TEXTURECUBE_ARRAY_FLOAT(textureName)  TextureCubeArray textureName  
	#define TEXTURE3D_FLOAT(textureName)          Texture3D_float textureName

	#define TEXTURE2D_HALF(textureName)           Texture2D_half textureName
	#define TEXTURE2D_ARRAY_HALF(textureName)     Texture2DArray textureName    
	#define TEXTURECUBE_HALF(textureName)         TextureCube_half textureName
	#define TEXTURECUBE_ARRAY_HALF(textureName)   TextureCubeArray textureName  
	#define TEXTURE3D_HALF(textureName)           Texture3D_half textureName

	#define TEXTURE2D_SHADOW(textureName)         TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_SHADOW(textureName)   TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_SHADOW(textureName)       TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_SHADOW(textureName) TEXTURECUBE_ARRAY(textureName)

	#define RW_TEXTURE2D(type, textureName)       RWTexture2D<type> textureName
	#define RW_TEXTURE2D_ARRAY(type, textureName) RWTexture2DArray<type> textureName
	#define RW_TEXTURE3D(type, textureName)       RWTexture3D<type> textureName

	#define SAMPLER(samplerName)                  SamplerState samplerName
	#define SAMPLER_CMP(samplerName)              SamplerComparisonState samplerName

	#define TEXTURE2D_PARAM(textureName, samplerName)                 TEXTURE2D(textureName),         SAMPLER(samplerName)
	#define TEXTURE2D_ARRAY_PARAM(textureName, samplerName)           TEXTURE2D_ARRAY(textureName),   SAMPLER(samplerName)
	#define TEXTURECUBE_PARAM(textureName, samplerName)               TEXTURECUBE(textureName),       SAMPLER(samplerName)
	#define TEXTURECUBE_ARRAY_PARAM(textureName, samplerName)         TEXTURECUBE_ARRAY(textureName), SAMPLER(samplerName)
	#define TEXTURE3D_PARAM(textureName, samplerName)                 TEXTURE3D(textureName),         SAMPLER(samplerName)

	#define TEXTURE2D_SHADOW_PARAM(textureName, samplerName)          TEXTURE2D(textureName),         SAMPLER_CMP(samplerName)
	#define TEXTURE2D_ARRAY_SHADOW_PARAM(textureName, samplerName)    TEXTURE2D_ARRAY(textureName),   SAMPLER_CMP(samplerName)
	#define TEXTURECUBE_SHADOW_PARAM(textureName, samplerName)        TEXTURECUBE(textureName),       SAMPLER_CMP(samplerName)
	#define TEXTURECUBE_ARRAY_SHADOW_PARAM(textureName, samplerName)  TEXTURECUBE_ARRAY(textureName), SAMPLER_CMP(samplerName)

	#define TEXTURE2D_ARGS(textureName, samplerName)                textureName, samplerName
	#define TEXTURE2D_ARRAY_ARGS(textureName, samplerName)          textureName, samplerName
	#define TEXTURECUBE_ARGS(textureName, samplerName)              textureName, samplerName
	#define TEXTURECUBE_ARRAY_ARGS(textureName, samplerName)        textureName, samplerName
	#define TEXTURE3D_ARGS(textureName, samplerName)                textureName, samplerName

	#define TEXTURE2D_SHADOW_ARGS(textureName, samplerName)         textureName, samplerName
	#define TEXTURE2D_ARRAY_SHADOW_ARGS(textureName, samplerName)   textureName, samplerName
	#define TEXTURECUBE_SHADOW_ARGS(textureName, samplerName)       textureName, samplerName
	#define TEXTURECUBE_ARRAY_SHADOW_ARGS(textureName, samplerName) textureName, samplerName

	#define SAMPLE_TEXTURE2D(textureName, samplerName, coord2)                               textureName.Sample(samplerName, coord2)
	#define SAMPLE_TEXTURE2D_LOD(textureName, samplerName, coord2, lod)                      textureName.SampleLevel(samplerName, coord2, lod)
	#define SAMPLE_TEXTURE2D_BIAS(textureName, samplerName, coord2, bias)                    textureName.SampleBias(samplerName, coord2, bias)
	#define SAMPLE_TEXTURE2D_GRAD(textureName, samplerName, coord2, dpdx, dpdy)              textureName.SampleGrad(samplerName, coord2, dpdx, dpdy)
	#define SAMPLE_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)                  textureName.Sample(samplerName, float3(coord2, index))
	#define SAMPLE_TEXTURE2D_ARRAY_LOD(textureName, samplerName, coord2, index, lod)         textureName.SampleLevel(samplerName, float3(coord2, index), lod)
	#define SAMPLE_TEXTURE2D_ARRAY_BIAS(textureName, samplerName, coord2, index, bias)       textureName.SampleBias(samplerName, float3(coord2, index), bias)
	#define SAMPLE_TEXTURE2D_ARRAY_GRAD(textureName, samplerName, coord2, index, dpdx, dpdy) textureName.SampleGrad(samplerName, float3(coord2, index), dpdx, dpdy)
	#define SAMPLE_TEXTURECUBE(textureName, samplerName, coord3)                             textureName.Sample(samplerName, coord3)
	#define SAMPLE_TEXTURECUBE_LOD(textureName, samplerName, coord3, lod)                    textureName.SampleLevel(samplerName, coord3, lod)
	#define SAMPLE_TEXTURECUBE_BIAS(textureName, samplerName, coord3, bias)                  textureName.SampleBias(samplerName, coord3, bias)
	#define SAMPLE_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)                textureName.Sample(samplerName, float4(coord3, index))
	#define SAMPLE_TEXTURECUBE_ARRAY_LOD(textureName, samplerName, coord3, index, lod)       textureName.SampleLevel(samplerName, float4(coord3, index), lod)
	#define SAMPLE_TEXTURECUBE_ARRAY_BIAS(textureName, samplerName, coord3, index, bias)     textureName.SampleBias(samplerName, float4(coord3, index), bias)
	#define SAMPLE_TEXTURE3D(textureName, samplerName, coord3)                               textureName.Sample(samplerName, coord3)
	#define SAMPLE_TEXTURE3D_LOD(textureName, samplerName, coord3, lod)                      textureName.SampleLevel(samplerName, coord3, lod)

	#define SAMPLE_TEXTURE2D_SHADOW(textureName, samplerName, coord3)                    textureName.SampleCmpLevelZero(samplerName, (coord3).xy, (coord3).z)
	#define SAMPLE_TEXTURE2D_ARRAY_SHADOW(textureName, samplerName, coord3, index)       textureName.SampleCmpLevelZero(samplerName, float3((coord3).xy, index), (coord3).z)
	#define SAMPLE_TEXTURECUBE_SHADOW(textureName, samplerName, coord4)                  textureName.SampleCmpLevelZero(samplerName, (coord4).xyz, (coord4).w)
	#define SAMPLE_TEXTURECUBE_ARRAY_SHADOW(textureName, samplerName, coord4, index)     textureName.SampleCmpLevelZero(samplerName, float4((coord4).xyz, index), (coord4).w)

	#define LOAD_TEXTURE2D(textureName, unCoord2)                                   textureName.Load(int3(unCoord2, 0))
	#define LOAD_TEXTURE2D_LOD(textureName, unCoord2, lod)                          textureName.Load(int3(unCoord2, lod))
	#define LOAD_TEXTURE2D_MSAA(textureName, unCoord2, sampleIndex)                 textureName.Load(unCoord2, sampleIndex)
	#define LOAD_TEXTURE2D_ARRAY(textureName, unCoord2, index)                      textureName.Load(int4(unCoord2, index, 0))
	#define LOAD_TEXTURE2D_ARRAY_MSAA(textureName, unCoord2, index, sampleIndex)    textureName.Load(int3(unCoord2, index), sampleIndex)
	#define LOAD_TEXTURE2D_ARRAY_LOD(textureName, unCoord2, index, lod)             textureName.Load(int4(unCoord2, index, lod))
	#define LOAD_TEXTURE3D(textureName, unCoord3)                                   textureName.Load(int4(unCoord3, 0))
	#define LOAD_TEXTURE3D_LOD(textureName, unCoord3, lod)                          textureName.Load(int4(unCoord3, lod))

	#define PLATFORM_SUPPORT_GATHER
	#define GATHER_TEXTURE2D(textureName, samplerName, coord2)                textureName.Gather(samplerName, coord2)
	#define GATHER_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)   textureName.Gather(samplerName, float3(coord2, index))
	#define GATHER_TEXTURECUBE(textureName, samplerName, coord3)              textureName.Gather(samplerName, coord3)
	#define GATHER_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index) textureName.Gather(samplerName, float4(coord3, index))
	#define GATHER_RED_TEXTURE2D(textureName, samplerName, coord2)            textureName.GatherRed(samplerName, coord2)
	#define GATHER_GREEN_TEXTURE2D(textureName, samplerName, coord2)          textureName.GatherGreen(samplerName, coord2)
	#define GATHER_BLUE_TEXTURE2D(textureName, samplerName, coord2)           textureName.GatherBlue(samplerName, coord2)
	#define GATHER_ALPHA_TEXTURE2D(textureName, samplerName, coord2)          textureName.GatherAlpha(samplerName, coord2)

#elif defined(SHADER_API_SWITCH)
	#define ZERO_INITIALIZE(type, name) name = (type)0;
	#define ZERO_INITIALIZE_ARRAY(type, name, arraySize) { for (int arrayIndex = 0; arrayIndex < arraySize; arrayIndex++) { name[arrayIndex] = (type)0; } }
	#define CALCULATE_TEXTURE2D_LOD(textureName, samplerName, coord2) textureName.CalculateLevelOfDetail(samplerName, coord2)
	#define TEXTURE2D(textureName)                Texture2D textureName
	#define TEXTURE2D_ARRAY(textureName)          Texture2DArray textureName
	#define TEXTURECUBE(textureName)              TextureCube textureName
	#define TEXTURECUBE_ARRAY(textureName)        TextureCubeArray textureName
	#define TEXTURE3D(textureName)                Texture3D textureName

	#define TEXTURE2D_FLOAT(textureName)          Texture2D_float textureName
	#define TEXTURE2D_ARRAY_FLOAT(textureName)    Texture2DArray textureName    
	#define TEXTURECUBE_FLOAT(textureName)        TextureCube_float textureName
	#define TEXTURECUBE_ARRAY_FLOAT(textureName)  TextureCubeArray textureName  
	#define TEXTURE3D_FLOAT(textureName)          Texture3D_float textureName

	#define TEXTURE2D_HALF(textureName)           Texture2D_half textureName
	#define TEXTURE2D_ARRAY_HALF(textureName)     Texture2DArray textureName    
	#define TEXTURECUBE_HALF(textureName)         TextureCube_half textureName
	#define TEXTURECUBE_ARRAY_HALF(textureName)   TextureCubeArray textureName  
	#define TEXTURE3D_HALF(textureName)           Texture3D_half textureName

	#define TEXTURE2D_SHADOW(textureName)         TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_SHADOW(textureName)   TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_SHADOW(textureName)       TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_SHADOW(textureName) TEXTURECUBE_ARRAY(textureName)

	#define RW_TEXTURE2D(type, textureName)       RWTexture2D<type> textureName
	#define RW_TEXTURE2D_ARRAY(type, textureName) RWTexture2DArray<type> textureName
	#define RW_TEXTURE3D(type, textureName)       RWTexture3D<type> textureName

	#define SAMPLER(samplerName)                  SamplerState samplerName
	#define SAMPLER_CMP(samplerName)              SamplerComparisonState samplerName

	#define TEXTURE2D_PARAM(textureName, samplerName)                 TEXTURE2D(textureName),         SAMPLER(samplerName)
	#define TEXTURE2D_ARRAY_PARAM(textureName, samplerName)           TEXTURE2D_ARRAY(textureName),   SAMPLER(samplerName)
	#define TEXTURECUBE_PARAM(textureName, samplerName)               TEXTURECUBE(textureName),       SAMPLER(samplerName)
	#define TEXTURECUBE_ARRAY_PARAM(textureName, samplerName)         TEXTURECUBE_ARRAY(textureName), SAMPLER(samplerName)
	#define TEXTURE3D_PARAM(textureName, samplerName)                 TEXTURE3D(textureName),         SAMPLER(samplerName)

	#define TEXTURE2D_SHADOW_PARAM(textureName, samplerName)          TEXTURE2D(textureName),         SAMPLER_CMP(samplerName)
	#define TEXTURE2D_ARRAY_SHADOW_PARAM(textureName, samplerName)    TEXTURE2D_ARRAY(textureName),   SAMPLER_CMP(samplerName)
	#define TEXTURECUBE_SHADOW_PARAM(textureName, samplerName)        TEXTURECUBE(textureName),       SAMPLER_CMP(samplerName)
	#define TEXTURECUBE_ARRAY_SHADOW_PARAM(textureName, samplerName)  TEXTURECUBE_ARRAY(textureName), SAMPLER_CMP(samplerName)

	#define TEXTURE2D_ARGS(textureName, samplerName)                textureName, samplerName
	#define TEXTURE2D_ARRAY_ARGS(textureName, samplerName)          textureName, samplerName
	#define TEXTURECUBE_ARGS(textureName, samplerName)              textureName, samplerName
	#define TEXTURECUBE_ARRAY_ARGS(textureName, samplerName)        textureName, samplerName
	#define TEXTURE3D_ARGS(textureName, samplerName)                textureName, samplerName

	#define TEXTURE2D_SHADOW_ARGS(textureName, samplerName)         textureName, samplerName
	#define TEXTURE2D_ARRAY_SHADOW_ARGS(textureName, samplerName)   textureName, samplerName
	#define TEXTURECUBE_SHADOW_ARGS(textureName, samplerName)       textureName, samplerName
	#define TEXTURECUBE_ARRAY_SHADOW_ARGS(textureName, samplerName) textureName, samplerName

	#define SAMPLE_TEXTURE2D(textureName, samplerName, coord2)                               textureName.Sample(samplerName, coord2)
	#define SAMPLE_TEXTURE2D_LOD(textureName, samplerName, coord2, lod)                      textureName.SampleLevel(samplerName, coord2, lod)
	#define SAMPLE_TEXTURE2D_BIAS(textureName, samplerName, coord2, bias)                    textureName.SampleBias(samplerName, coord2, bias)
	#define SAMPLE_TEXTURE2D_GRAD(textureName, samplerName, coord2, dpdx, dpdy)              textureName.SampleGrad(samplerName, coord2, dpdx, dpdy)
	#define SAMPLE_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)                  textureName.Sample(samplerName, float3(coord2, index))
	#define SAMPLE_TEXTURE2D_ARRAY_LOD(textureName, samplerName, coord2, index, lod)         textureName.SampleLevel(samplerName, float3(coord2, index), lod)
	#define SAMPLE_TEXTURE2D_ARRAY_BIAS(textureName, samplerName, coord2, index, bias)       textureName.SampleBias(samplerName, float3(coord2, index), bias)
	#define SAMPLE_TEXTURE2D_ARRAY_GRAD(textureName, samplerName, coord2, index, dpdx, dpdy) textureName.SampleGrad(samplerName, float3(coord2, index), dpdx, dpdy)
	#define SAMPLE_TEXTURECUBE(textureName, samplerName, coord3)                             textureName.Sample(samplerName, coord3)
	#define SAMPLE_TEXTURECUBE_LOD(textureName, samplerName, coord3, lod)                    textureName.SampleLevel(samplerName, coord3, lod)
	#define SAMPLE_TEXTURECUBE_BIAS(textureName, samplerName, coord3, bias)                  textureName.SampleBias(samplerName, coord3, bias)
	#define SAMPLE_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)                textureName.Sample(samplerName, float4(coord3, index))
	#define SAMPLE_TEXTURECUBE_ARRAY_LOD(textureName, samplerName, coord3, index, lod)       textureName.SampleLevel(samplerName, float4(coord3, index), lod)
	#define SAMPLE_TEXTURECUBE_ARRAY_BIAS(textureName, samplerName, coord3, index, bias)     textureName.SampleBias(samplerName, float4(coord3, index), bias)
	#define SAMPLE_TEXTURE3D(textureName, samplerName, coord3)                               textureName.Sample(samplerName, coord3)
	#define SAMPLE_TEXTURE3D_LOD(textureName, samplerName, coord3, lod)                      textureName.SampleLevel(samplerName, coord3, lod)

	#define SAMPLE_TEXTURE2D_SHADOW(textureName, samplerName, coord3)                    textureName.SampleCmpLevelZero(samplerName, (coord3).xy, (coord3).z)
	#define SAMPLE_TEXTURE2D_ARRAY_SHADOW(textureName, samplerName, coord3, index)       textureName.SampleCmpLevelZero(samplerName, float3((coord3).xy, index), (coord3).z)
	#define SAMPLE_TEXTURECUBE_SHADOW(textureName, samplerName, coord4)                  textureName.SampleCmpLevelZero(samplerName, (coord4).xyz, (coord4).w)
	#define SAMPLE_TEXTURECUBE_ARRAY_SHADOW(textureName, samplerName, coord4, index)     textureName.SampleCmpLevelZero(samplerName, float4((coord4).xyz, index), (coord4).w)
	#define LOAD_TEXTURE2D(textureName, unCoord2)                       textureName.Load(int3(unCoord2, 0))
	#define LOAD_TEXTURE2D_LOD(textureName, unCoord2, lod)              textureName.Load(int3(unCoord2, lod))
	#define LOAD_TEXTURE2D_MSAA(textureName, unCoord2, sampleIndex)     textureName.Load(unCoord2, sampleIndex)
	#define LOAD_TEXTURE2D_ARRAY(textureName, unCoord2, index)          textureName.Load(int4(unCoord2, index, 0))
	#define LOAD_TEXTURE2D_ARRAY_LOD(textureName, unCoord2, index, lod) textureName.Load(int4(unCoord2, index, lod))
	#define LOAD_TEXTURE3D(textureName, unCoord3)                       textureName.Load(int4(unCoord3, 0))
	#define LOAD_TEXTURE3D_LOD(textureName, unCoord3, lod)              textureName.Load(int4(unCoord3, lod))

	#define PLATFORM_SUPPORT_GATHER
	#define GATHER_TEXTURE2D(textureName, samplerName, coord2)                textureName.Gather(samplerName, coord2)
	#define GATHER_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)   textureName.Gather(samplerName, float3(coord2, index))
	#define GATHER_TEXTURECUBE(textureName, samplerName, coord3)              textureName.Gather(samplerName, coord3)
	#define GATHER_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index) textureName.Gather(samplerName, float4(coord3, index))
	#define GATHER_RED_TEXTURE2D(textureName, samplerName, coord2)            textureName.GatherRed(samplerName, coord2)
	#define GATHER_GREEN_TEXTURE2D(textureName, samplerName, coord2)          textureName.GatherGreen(samplerName, coord2)
	#define GATHER_BLUE_TEXTURE2D(textureName, samplerName, coord2)           textureName.GatherBlue(samplerName, coord2)
	#define GATHER_ALPHA_TEXTURE2D(textureName, samplerName, coord2)          textureName.GatherAlpha(samplerName, coord2)

#elif defined(SHADER_API_GLCORE)
	#if (SHADER_TARGET >= 46)
	#define OPENGL4_1_SM5 1
	#else
	#define OPENGL4_1_SM5 0
	#endif
	#define ZERO_INITIALIZE(type, name) name = (type)0;
	#define ZERO_INITIALIZE_ARRAY(type, name, arraySize) { for (int arrayIndex = 0; arrayIndex < arraySize; arrayIndex++) { name[arrayIndex] = (type)0; } }
	#define CALCULATE_TEXTURE2D_LOD(textureName, samplerName, coord2) textureName.CalculateLevelOfDetail(samplerName, coord2)
	#define TEXTURE2D(textureName)                  Texture2D textureName
	#define TEXTURE2D_ARRAY(textureName)            Texture2DArray textureName
	#define TEXTURECUBE(textureName)                TextureCube textureName
	#define TEXTURECUBE_ARRAY(textureName)          TextureCubeArray textureName
	#define TEXTURE3D(textureName)                  Texture3D textureName

	#define TEXTURE2D_FLOAT(textureName)            TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_FLOAT(textureName)      TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_FLOAT(textureName)          TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_FLOAT(textureName)    TEXTURECUBE_ARRAY(textureName)
	#define TEXTURE3D_FLOAT(textureName)            TEXTURE3D(textureName)

	#define TEXTURE2D_HALF(textureName)             TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_HALF(textureName)       TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_HALF(textureName)           TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_HALF(textureName)     TEXTURECUBE_ARRAY(textureName)
	#define TEXTURE3D_HALF(textureName)             TEXTURE3D(textureName)

	#define TEXTURE2D_SHADOW(textureName)           TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_SHADOW(textureName)     TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_SHADOW(textureName)         TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_SHADOW(textureName)   TEXTURECUBE_ARRAY(textureName)

	#define RW_TEXTURE2D(type, textureName)         RWTexture2D<type> textureName
	#define RW_TEXTURE2D_ARRAY(type, textureName)   RWTexture2DArray<type> textureName
	#define RW_TEXTURE3D(type, textureName)         RWTexture3D<type> textureName

	#define SAMPLER(samplerName)                    SamplerState samplerName
	#define SAMPLER_CMP(samplerName)                SamplerComparisonState samplerName

	#define TEXTURE2D_PARAM(textureName, samplerName)                 TEXTURE2D(textureName),         SAMPLER(samplerName)
	#define TEXTURE2D_ARRAY_PARAM(textureName, samplerName)           TEXTURE2D_ARRAY(textureName),   SAMPLER(samplerName)
	#define TEXTURECUBE_PARAM(textureName, samplerName)               TEXTURECUBE(textureName),       SAMPLER(samplerName)
	#define TEXTURECUBE_ARRAY_PARAM(textureName, samplerName)         TEXTURECUBE_ARRAY(textureName), SAMPLER(samplerName)
	#define TEXTURE3D_PARAM(textureName, samplerName)                 TEXTURE3D(textureName),         SAMPLER(samplerName)

	#define TEXTURE2D_SHADOW_PARAM(textureName, samplerName)          TEXTURE2D(textureName),         SAMPLER_CMP(samplerName)
	#define TEXTURE2D_ARRAY_SHADOW_PARAM(textureName, samplerName)    TEXTURE2D_ARRAY(textureName),   SAMPLER_CMP(samplerName)
	#define TEXTURECUBE_SHADOW_PARAM(textureName, samplerName)        TEXTURECUBE(textureName),       SAMPLER_CMP(samplerName)
	#define TEXTURECUBE_ARRAY_SHADOW_PARAM(textureName, samplerName)  TEXTURECUBE_ARRAY(textureName), SAMPLER_CMP(samplerName)

	#define TEXTURE2D_ARGS(textureName, samplerName)                textureName, samplerName
	#define TEXTURE2D_ARRAY_ARGS(textureName, samplerName)          textureName, samplerName
	#define TEXTURECUBE_ARGS(textureName, samplerName)              textureName, samplerName
	#define TEXTURECUBE_ARRAY_ARGS(textureName, samplerName)        textureName, samplerName
	#define TEXTURE3D_ARGS(textureName, samplerName)                textureName, samplerName

	#define TEXTURE2D_SHADOW_ARGS(textureName, samplerName)         textureName, samplerName
	#define TEXTURE2D_ARRAY_SHADOW_ARGS(textureName, samplerName)   textureName, samplerName
	#define TEXTURECUBE_SHADOW_ARGS(textureName, samplerName)       textureName, samplerName
	#define TEXTURECUBE_ARRAY_SHADOW_ARGS(textureName, samplerName) textureName, samplerName

	#define SAMPLE_TEXTURE2D(textureName, samplerName, coord2)                               textureName.Sample(samplerName, coord2)
	#define SAMPLE_TEXTURE2D_LOD(textureName, samplerName, coord2, lod)                      textureName.SampleLevel(samplerName, coord2, lod)
	#define SAMPLE_TEXTURE2D_BIAS(textureName, samplerName, coord2, bias)                    textureName.SampleBias(samplerName, coord2, bias)
	#define SAMPLE_TEXTURE2D_GRAD(textureName, samplerName, coord2, ddx, ddy)                textureName.SampleGrad(samplerName, coord2, ddx, ddy)
	#define SAMPLE_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)                  textureName.Sample(samplerName, float3(coord2, index))
	#define SAMPLE_TEXTURE2D_ARRAY_LOD(textureName, samplerName, coord2, index, lod)         textureName.SampleLevel(samplerName, float3(coord2, index), lod)
	#define SAMPLE_TEXTURE2D_ARRAY_BIAS(textureName, samplerName, coord2, index, bias)       textureName.SampleBias(samplerName, float3(coord2, index), bias)
	#define SAMPLE_TEXTURE2D_ARRAY_GRAD(textureName, samplerName, coord2, index, dpdx, dpdy) textureName.SampleGrad(samplerName, float3(coord2, index), dpdx, dpdy)
	#define SAMPLE_TEXTURECUBE(textureName, samplerName, coord3)                             textureName.Sample(samplerName, coord3)
	#define SAMPLE_TEXTURECUBE_LOD(textureName, samplerName, coord3, lod)                    textureName.SampleLevel(samplerName, coord3, lod)
	#define SAMPLE_TEXTURECUBE_BIAS(textureName, samplerName, coord3, bias)                  textureName.SampleBias(samplerName, coord3, bias)
	#ifdef UNITY_NO_CUBEMAP_ARRAY
	#define SAMPLE_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)           ERROR_ON_UNSUPPORTED_FUNCTION(SAMPLE_TEXTURECUBE_ARRAY)
	#define SAMPLE_TEXTURECUBE_ARRAY_LOD(textureName, samplerName, coord3, index, lod)  ERROR_ON_UNSUPPORTED_FUNCTION(SAMPLE_TEXTURECUBE_ARRAY_LOD)
	#define SAMPLE_TEXTURECUBE_ARRAY_LOD(textureName, samplerName, coord3, index, bias) ERROR_ON_UNSUPPORTED_FUNCTION(SAMPLE_TEXTURECUBE_ARRAY_LOD)
	#else
	#define SAMPLE_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)           textureName.Sample(samplerName, float4(coord3, index))
	#define SAMPLE_TEXTURECUBE_ARRAY_LOD(textureName, samplerName, coord3, index, lod)  textureName.SampleLevel(samplerName, float4(coord3, index), lod)
	#define SAMPLE_TEXTURECUBE_ARRAY_BIAS(textureName, samplerName, coord3, index, bias)textureName.SampleBias(samplerName, float4(coord3, index), bias)
	#endif
	#define SAMPLE_TEXTURE3D(textureName, samplerName, coord3)                          textureName.Sample(samplerName, coord3)
	#define SAMPLE_TEXTURE3D_LOD(textureName, samplerName, coord3, lod)                 textureName.SampleLevel(samplerName, coord3, lod)

	#define SAMPLE_TEXTURE2D_SHADOW(textureName, samplerName, coord3)                   textureName.SampleCmpLevelZero(samplerName, (coord3).xy, (coord3).z)
	#define SAMPLE_TEXTURE2D_ARRAY_SHADOW(textureName, samplerName, coord3, index)      textureName.SampleCmpLevelZero(samplerName, float3((coord3).xy, index), (coord3).z)
	#define SAMPLE_TEXTURECUBE_SHADOW(textureName, samplerName, coord4)                 textureName.SampleCmpLevelZero(samplerName, (coord4).xyz, (coord4).w)
	#define SAMPLE_TEXTURECUBE_ARRAY_SHADOW(textureName, samplerName, coord4, index)    textureName.SampleCmpLevelZero(samplerName, float4((coord4).xyz, index), (coord4).w)

	#define LOAD_TEXTURE2D(textureName, unCoord2)                                   textureName.Load(int3(unCoord2, 0))
	#define LOAD_TEXTURE2D_LOD(textureName, unCoord2, lod)                          textureName.Load(int3(unCoord2, lod))
	#define LOAD_TEXTURE2D_MSAA(textureName, unCoord2, sampleIndex)                 textureName.Load(unCoord2, sampleIndex)
	#define LOAD_TEXTURE2D_ARRAY(textureName, unCoord2, index)                      textureName.Load(int4(unCoord2, index, 0))
	#define LOAD_TEXTURE2D_ARRAY_MSAA(textureName, unCoord2, index, sampleIndex)    textureName.Load(int3(unCoord2, index), sampleIndex)
	#define LOAD_TEXTURE2D_ARRAY_LOD(textureName, unCoord2, index, lod)             textureName.Load(int4(unCoord2, index, lod))

	#if OPENGL4_1_SM5
	#define PLATFORM_SUPPORT_GATHER
	#define GATHER_TEXTURE2D(textureName, samplerName, coord2)                  textureName.Gather(samplerName, coord2)
	#define GATHER_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)     textureName.Gather(samplerName, float3(coord2, index))
	#define GATHER_TEXTURECUBE(textureName, samplerName, coord3)                textureName.Gather(samplerName, coord3)
	#define GATHER_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)   textureName.Gather(samplerName, float4(coord3, index))
	#else
	#define GATHER_TEXTURE2D(textureName, samplerName, coord2)                  ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_TEXTURE2D)
	#define GATHER_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)     ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_TEXTURE2D_ARRAY)
	#define GATHER_TEXTURECUBE(textureName, samplerName, coord3)                ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_TEXTURECUBE)
	#define GATHER_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)   ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_TEXTURECUBE_ARRAY)
	#endif
	#elif defined(SHADER_API_GLES3)
	#if (SHADER_TARGET >= 40)
	#define GLES3_1_AEP 1
	#else
	#define GLES3_1_AEP 0
	#endif
	#define ZERO_INITIALIZE(type, name) name = (type)0;
	#define ZERO_INITIALIZE_ARRAY(type, name, arraySize) { for (int arrayIndex = 0; arrayIndex < arraySize; arrayIndex++) { name[arrayIndex] = (type)0; } }
	#define CALCULATE_TEXTURE2D_LOD(textureName, samplerName, coord2) textureName.CalculateLevelOfDetail(samplerName, coord2)
	#define TEXTURE2D(textureName)                  Texture2D textureName
	#define TEXTURE2D_ARRAY(textureName)            Texture2DArray textureName
	#define TEXTURECUBE(textureName)                TextureCube textureName
	#define TEXTURECUBE_ARRAY(textureName)          TextureCubeArray textureName
	#define TEXTURE3D(textureName)                  Texture3D textureName

	#define TEXTURE2D_FLOAT(textureName)            Texture2D_float textureName
	#define TEXTURE2D_ARRAY_FLOAT(textureName)      Texture2DArray textureName    
	#define TEXTURECUBE_FLOAT(textureName)          TextureCube_float textureName
	#define TEXTURECUBE_ARRAY_FLOAT(textureName)    TextureCubeArray textureName  
	#define TEXTURE3D_FLOAT(textureName)            Texture3D_float textureName

	#define TEXTURE2D_HALF(textureName)             Texture2D_half textureName
	#define TEXTURE2D_ARRAY_HALF(textureName)       Texture2DArray textureName    
	#define TEXTURECUBE_HALF(textureName)           TextureCube_half textureName
	#define TEXTURECUBE_ARRAY_HALF(textureName)     TextureCubeArray textureName  
	#define TEXTURE3D_HALF(textureName)             Texture3D_half textureName

	#define TEXTURE2D_SHADOW(textureName)           TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_SHADOW(textureName)     TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_SHADOW(textureName)         TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_SHADOW(textureName)   TEXTURECUBE_ARRAY(textureName)

	#if GLES3_1_AEP
	#define RW_TEXTURE2D(type, textureName)         RWTexture2D<type> textureName
	#define RW_TEXTURE2D_ARRAY(type, textureName)   RWTexture2DArray<type> textureName
	#define RW_TEXTURE3D(type, textureName)         RWTexture3D<type> textureName
	#else
	#define RW_TEXTURE2D(type, textureName)         ERROR_ON_UNSUPPORTED_FUNCTION(RWTexture2D)
	#define RW_TEXTURE2D_ARRAY(type, textureName)   ERROR_ON_UNSUPPORTED_FUNCTION(RWTexture2DArray)
	#define RW_TEXTURE3D(type, textureName)         ERROR_ON_UNSUPPORTED_FUNCTION(RWTexture3D)
	#endif

	#define SAMPLER(samplerName)                    SamplerState samplerName
	#define SAMPLER_CMP(samplerName)                SamplerComparisonState samplerName

	#define TEXTURE2D_PARAM(textureName, samplerName)                 TEXTURE2D(textureName),         SAMPLER(samplerName)
	#define TEXTURE2D_ARRAY_PARAM(textureName, samplerName)           TEXTURE2D_ARRAY(textureName),   SAMPLER(samplerName)
	#define TEXTURECUBE_PARAM(textureName, samplerName)               TEXTURECUBE(textureName),       SAMPLER(samplerName)
	#define TEXTURECUBE_ARRAY_PARAM(textureName, samplerName)         TEXTURECUBE_ARRAY(textureName), SAMPLER(samplerName)
	#define TEXTURE3D_PARAM(textureName, samplerName)                 TEXTURE3D(textureName),         SAMPLER(samplerName)

	#define TEXTURE2D_SHADOW_PARAM(textureName, samplerName)          TEXTURE2D(textureName),         SAMPLER_CMP(samplerName)
	#define TEXTURE2D_ARRAY_SHADOW_PARAM(textureName, samplerName)    TEXTURE2D_ARRAY(textureName),   SAMPLER_CMP(samplerName)
	#define TEXTURECUBE_SHADOW_PARAM(textureName, samplerName)        TEXTURECUBE(textureName),       SAMPLER_CMP(samplerName)
	#define TEXTURECUBE_ARRAY_SHADOW_PARAM(textureName, samplerName)  TEXTURECUBE_ARRAY(textureName), SAMPLER_CMP(samplerName)

	#define TEXTURE2D_ARGS(textureName, samplerName)                textureName, samplerName
	#define TEXTURE2D_ARRAY_ARGS(textureName, samplerName)          textureName, samplerName
	#define TEXTURECUBE_ARGS(textureName, samplerName)              textureName, samplerName
	#define TEXTURECUBE_ARRAY_ARGS(textureName, samplerName)        textureName, samplerName
	#define TEXTURE3D_ARGS(textureName, samplerName)                textureName, samplerName

	#define TEXTURE2D_SHADOW_ARGS(textureName, samplerName)         textureName, samplerName
	#define TEXTURE2D_ARRAY_SHADOW_ARGS(textureName, samplerName)   textureName, samplerName
	#define TEXTURECUBE_SHADOW_ARGS(textureName, samplerName)       textureName, samplerName
	#define TEXTURECUBE_ARRAY_SHADOW_ARGS(textureName, samplerName) textureName, samplerName

	#define SAMPLE_TEXTURE2D(textureName, samplerName, coord2)                               textureName.Sample(samplerName, coord2)
	#define SAMPLE_TEXTURE2D_LOD(textureName, samplerName, coord2, lod)                      textureName.SampleLevel(samplerName, coord2, lod)
	#define SAMPLE_TEXTURE2D_BIAS(textureName, samplerName, coord2, bias)                    textureName.SampleBias(samplerName, coord2, bias)
	#define SAMPLE_TEXTURE2D_GRAD(textureName, samplerName, coord2, ddx, ddy)                textureName.SampleGrad(samplerName, coord2, ddx, ddy)
	#define SAMPLE_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)                  textureName.Sample(samplerName, float3(coord2, index))
	#define SAMPLE_TEXTURE2D_ARRAY_LOD(textureName, samplerName, coord2, index, lod)         textureName.SampleLevel(samplerName, float3(coord2, index), lod)
	#define SAMPLE_TEXTURE2D_ARRAY_BIAS(textureName, samplerName, coord2, index, bias)       textureName.SampleBias(samplerName, float3(coord2, index), bias)
	#define SAMPLE_TEXTURE2D_ARRAY_GRAD(textureName, samplerName, coord2, index, dpdx, dpdy) textureName.SampleGrad(samplerName, float3(coord2, index), dpdx, dpdy)
	#define SAMPLE_TEXTURECUBE(textureName, samplerName, coord3)                             textureName.Sample(samplerName, coord3)
	#define SAMPLE_TEXTURECUBE_LOD(textureName, samplerName, coord3, lod)                    textureName.SampleLevel(samplerName, coord3, lod)
	#define SAMPLE_TEXTURECUBE_BIAS(textureName, samplerName, coord3, bias)                  textureName.SampleBias(samplerName, coord3, bias)

	#ifdef UNITY_NO_CUBEMAP_ARRAY
	#define SAMPLE_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)           ERROR_ON_UNSUPPORTED_FUNCTION(SAMPLE_TEXTURECUBE_ARRAY)
	#define SAMPLE_TEXTURECUBE_ARRAY_LOD(textureName, samplerName, coord3, index, lod)  ERROR_ON_UNSUPPORTED_FUNCTION(SAMPLE_TEXTURECUBE_ARRAY_LOD)
	#define SAMPLE_TEXTURECUBE_ARRAY_BIAS(textureName, samplerName, coord3, index, bias)ERROR_ON_UNSUPPORTED_FUNCTION(SAMPLE_TEXTURECUBE_ARRAY_BIAS)
	#else
	#define SAMPLE_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)           textureName.Sample(samplerName, float4(coord3, index))
	#define SAMPLE_TEXTURECUBE_ARRAY_LOD(textureName, samplerName, coord3, index, lod)  textureName.SampleLevel(samplerName, float4(coord3, index), lod)
	#define SAMPLE_TEXTURECUBE_ARRAY_BIAS(textureName, samplerName, coord3, index, bias)textureName.SampleBias(samplerName, float4(coord3, index), bias)
	#endif

	#define SAMPLE_TEXTURE3D(textureName, samplerName, coord3)                          textureName.Sample(samplerName, coord3)
	#define SAMPLE_TEXTURE3D_LOD(textureName, samplerName, coord3, lod)                 textureName.SampleLevel(samplerName, coord3, lod)

	#define SAMPLE_TEXTURE2D_SHADOW(textureName, samplerName, coord3)                   textureName.SampleCmpLevelZero(samplerName, (coord3).xy, (coord3).z)
	#define SAMPLE_TEXTURE2D_ARRAY_SHADOW(textureName, samplerName, coord3, index)      textureName.SampleCmpLevelZero(samplerName, float3((coord3).xy, index), (coord3).z)
	#define SAMPLE_TEXTURECUBE_SHADOW(textureName, samplerName, coord4)                 textureName.SampleCmpLevelZero(samplerName, (coord4).xyz, (coord4).w)
	#define SAMPLE_TEXTURECUBE_ARRAY_SHADOW(textureName, samplerName, coord4, index)    textureName.SampleCmpLevelZero(samplerName, float4((coord4).xyz, index), (coord4).w)
	#define LOAD_TEXTURE2D(textureName, unCoord2)                                       textureName.Load(int3(unCoord2, 0))
	#define LOAD_TEXTURE2D_LOD(textureName, unCoord2, lod)                              textureName.Load(int3(unCoord2, lod))
	#define LOAD_TEXTURE2D_MSAA(textureName, unCoord2, sampleIndex)                     textureName.Load(unCoord2, sampleIndex)
	#define LOAD_TEXTURE2D_ARRAY(textureName, unCoord2, index)                          textureName.Load(int4(unCoord2, index, 0))
	#define LOAD_TEXTURE2D_ARRAY_MSAA(textureName, unCoord2, index, sampleIndex)        textureName.Load(int3(unCoord2, index), sampleIndex)
	#define LOAD_TEXTURE2D_ARRAY_LOD(textureName, unCoord2, index, lod)                 textureName.Load(int4(unCoord2, index, lod))
	#define LOAD_TEXTURE3D(textureName, unCoord3)                                       textureName.Load(int4(unCoord3, 0))
	#define LOAD_TEXTURE3D_LOD(textureName, unCoord3, lod)                              textureName.Load(int4(unCoord3, lod))

	#if GLES3_1_AEP
	#define PLATFORM_SUPPORT_GATHER
	#define GATHER_TEXTURE2D(textureName, samplerName, coord2)                  textureName.Gather(samplerName, coord2)
	#define GATHER_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)     textureName.Gather(samplerName, float3(coord2, index))
	#define GATHER_TEXTURECUBE(textureName, samplerName, coord3)                textureName.Gather(samplerName, coord3)
	#define GATHER_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)   textureName.Gather(samplerName, float4(coord3, index))
	#define GATHER_RED_TEXTURE2D(textureName, samplerName, coord2)              textureName.GatherRed(samplerName, coord2)
	#define GATHER_GREEN_TEXTURE2D(textureName, samplerName, coord2)            textureName.GatherGreen(samplerName, coord2)
	#define GATHER_BLUE_TEXTURE2D(textureName, samplerName, coord2)             textureName.GatherBlue(samplerName, coord2)
	#define GATHER_ALPHA_TEXTURE2D(textureName, samplerName, coord2)            textureName.GatherAlpha(samplerName, coord2)
	#else
	#define GATHER_TEXTURE2D(textureName, samplerName, coord2)                  ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_TEXTURE2D)
	#define GATHER_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)     ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_TEXTURE2D_ARRAY)
	#define GATHER_TEXTURECUBE(textureName, samplerName, coord3)                ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_TEXTURECUBE)
	#define GATHER_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)   ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_TEXTURECUBE_ARRAY)
	#define GATHER_RED_TEXTURE2D(textureName, samplerName, coord2)              ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_RED_TEXTURE2D)
	#define GATHER_GREEN_TEXTURE2D(textureName, samplerName, coord2)            ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_GREEN_TEXTURE2D)
	#define GATHER_BLUE_TEXTURE2D(textureName, samplerName, coord2)             ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_BLUE_TEXTURE2D)
	#define GATHER_ALPHA_TEXTURE2D(textureName, samplerName, coord2)            ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_ALPHA_TEXTURE2D)
	#endif
#elif defined(SHADER_API_GLES)
	#define uint int

	#define rcp(x) 1.0 / (x)
	#define ddx_fine ddx
	#define ddy_fine ddy
	#define asfloat
	#define asuint(x) asint(x)
	#define f32tof16
	#define f16tof32

	#define ERROR_ON_UNSUPPORTED_FUNCTION(funcName) #error #funcName is not supported on GLES 2.0
	#define ZERO_INITIALIZE(type, name) name = (type)0;
	#define ZERO_INITIALIZE_ARRAY(type, name, arraySize) { for (int arrayIndex = 0; arrayIndex < arraySize; arrayIndex++) { name[arrayIndex] = (type)0; } }
	#define CALCULATE_TEXTURE2D_LOD(textureName, samplerName, coord2) #error calculate Level of Detail not supported in GLES2
	#define TEXTURE2D(textureName)                          sampler2D textureName
	#define TEXTURE2D_ARRAY(textureName)                    samplerCUBE textureName 
	#define TEXTURECUBE(textureName)                        samplerCUBE textureName
	#define TEXTURECUBE_ARRAY(textureName)                  samplerCUBE textureName 
	#define TEXTURE3D(textureName)                          sampler3D textureName

	#define TEXTURE2D_FLOAT(textureName)                    sampler2D_float textureName
	#define TEXTURE2D_ARRAY_FLOAT(textureName)              TEXTURECUBE_FLOAT(textureName) 
	#define TEXTURECUBE_FLOAT(textureName)                  samplerCUBE_float textureName
	#define TEXTURECUBE_ARRAY_FLOAT(textureName)            TEXTURECUBE_FLOAT(textureName) 
	#define TEXTURE3D_FLOAT(textureName)                    sampler3D_float textureName

	#define TEXTURE2D_HALF(textureName)                     sampler2D_half textureName
	#define TEXTURE2D_ARRAY_HALF(textureName)               TEXTURECUBE_HALF(textureName) 
	#define TEXTURECUBE_HALF(textureName)                   samplerCUBE_half textureName
	#define TEXTURECUBE_ARRAY_HALF(textureName)             TEXTURECUBE_HALF(textureName) 
	#define TEXTURE3D_HALF(textureName)                     sampler3D_half textureName

	#define TEXTURE2D_SHADOW(textureName)                   SHADOW2D_TEXTURE_AND_SAMPLER textureName
	#define TEXTURE2D_ARRAY_SHADOW(textureName)             TEXTURECUBE_SHADOW(textureName) 
	#define TEXTURECUBE_SHADOW(textureName)                 SHADOWCUBE_TEXTURE_AND_SAMPLER textureName
	#define TEXTURECUBE_ARRAY_SHADOW(textureName)           TEXTURECUBE_SHADOW(textureName) 

	#define RW_TEXTURE2D(type, textureNam)                  ERROR_ON_UNSUPPORTED_FUNCTION(RWTexture2D)
	#define RW_TEXTURE2D_ARRAY(type, textureName)           ERROR_ON_UNSUPPORTED_FUNCTION(RWTexture2DArray)
	#define RW_TEXTURE3D(type, textureNam)                  ERROR_ON_UNSUPPORTED_FUNCTION(RWTexture3D)

	#define SAMPLER(samplerName)
	#define SAMPLER_CMP(samplerName)

	#define TEXTURE2D_PARAM(textureName, samplerName)                sampler2D textureName
	#define TEXTURE2D_ARRAY_PARAM(textureName, samplerName)          samplerCUBE textureName
	#define TEXTURECUBE_PARAM(textureName, samplerName)              samplerCUBE textureName
	#define TEXTURECUBE_ARRAY_PARAM(textureName, samplerName)        samplerCUBE textureName
	#define TEXTURE3D_PARAM(textureName, samplerName)                sampler3D textureName
	#define TEXTURE2D_SHADOW_PARAM(textureName, samplerName)         SHADOW2D_TEXTURE_AND_SAMPLER textureName
	#define TEXTURE2D_ARRAY_SHADOW_PARAM(textureName, samplerName)   SHADOWCUBE_TEXTURE_AND_SAMPLER textureName
	#define TEXTURECUBE_SHADOW_PARAM(textureName, samplerName)       SHADOWCUBE_TEXTURE_AND_SAMPLER textureName

	#define TEXTURE2D_ARGS(textureName, samplerName)               textureName
	#define TEXTURE2D_ARRAY_ARGS(textureName, samplerName)         textureName
	#define TEXTURECUBE_ARGS(textureName, samplerName)             textureName
	#define TEXTURECUBE_ARRAY_ARGS(textureName, samplerName)       textureName
	#define TEXTURE3D_ARGS(textureName, samplerName)               textureName
	#define TEXTURE2D_SHADOW_ARGS(textureName, samplerName)        textureName
	#define TEXTURE2D_ARRAY_SHADOW_ARGS(textureName, samplerName)  textureName
	#define TEXTURECUBE_SHADOW_ARGS(textureName, samplerName)      textureName

	#define SAMPLE_TEXTURE2D(textureName, samplerName, coord2) tex2D(textureName, coord2)

	#if (SHADER_TARGET >= 30)
	    #define SAMPLE_TEXTURE2D_LOD(textureName, samplerName, coord2, lod) tex2Dlod(textureName, float4(coord2, 0, lod))
	#else
	    #define SAMPLE_TEXTURE2D_LOD(textureName, samplerName, coord2, lod) SAMPLE_TEXTURE2D_BIAS(textureName, samplerName, coord2, lod)
	#endif

	#define SAMPLE_TEXTURE2D_BIAS(textureName, samplerName, coord2, bias)                       tex2Dbias(textureName, float4(coord2, 0, bias))
	#define SAMPLE_TEXTURE2D_GRAD(textureName, samplerName, coord2, ddx, ddy)                   SAMPLE_TEXTURE2D(textureName, samplerName, coord2)
	#define SAMPLE_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)                     ERROR_ON_UNSUPPORTED_FUNCTION(SAMPLE_TEXTURE2D_ARRAY)
	#define SAMPLE_TEXTURE2D_ARRAY_LOD(textureName, samplerName, coord2, index, lod)            ERROR_ON_UNSUPPORTED_FUNCTION(SAMPLE_TEXTURE2D_ARRAY_LOD)
	#define SAMPLE_TEXTURE2D_ARRAY_BIAS(textureName, samplerName, coord2, index, bias)          ERROR_ON_UNSUPPORTED_FUNCTION(SAMPLE_TEXTURE2D_ARRAY_BIAS)
	#define SAMPLE_TEXTURE2D_ARRAY_GRAD(textureName, samplerName, coord2, index, dpdx, dpdy)    ERROR_ON_UNSUPPORTED_FUNCTION(SAMPLE_TEXTURE2D_ARRAY_GRAD)
	#define SAMPLE_TEXTURECUBE(textureName, samplerName, coord3)                                texCUBE(textureName, coord3)
	#define SAMPLE_TEXTURECUBE_LOD(textureName, samplerName, coord3, lod)                       SAMPLE_TEXTURECUBE_BIAS(textureName, samplerName, coord3, lod)
	#define SAMPLE_TEXTURECUBE_BIAS(textureName, samplerName, coord3, bias)                     texCUBEbias(textureName, float4(coord3, bias))
	#define SAMPLE_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)                   ERROR_ON_UNSUPPORTED_FUNCTION(SAMPLE_TEXTURECUBE_ARRAY)
	#define SAMPLE_TEXTURECUBE_ARRAY_LOD(textureName, samplerName, coord3, index, lod)          ERROR_ON_UNSUPPORTED_FUNCTION(SAMPLE_TEXTURECUBE_ARRAY_LOD)
	#define SAMPLE_TEXTURECUBE_ARRAY_BIAS(textureName, samplerName, coord3, index, bias)        ERROR_ON_UNSUPPORTED_FUNCTION(SAMPLE_TEXTURECUBE_ARRAY_BIAS)
	#define SAMPLE_TEXTURE3D(textureName, samplerName, coord3)                                  tex3D(textureName, coord3)
	#define SAMPLE_TEXTURE3D_LOD(textureName, samplerName, coord3, lod)                         ERROR_ON_UNSUPPORTED_FUNCTION(SAMPLE_TEXTURE3D_LOD)

	#define SAMPLE_TEXTURE2D_SHADOW(textureName, samplerName, coord3)                           SHADOW2D_SAMPLE(textureName, samplerName, coord3)
	#define SAMPLE_TEXTURE2D_ARRAY_SHADOW(textureName, samplerName, coord3, index)              ERROR_ON_UNSUPPORTED_FUNCTION(SAMPLE_TEXTURE2D_ARRAY_SHADOW)
	#define SAMPLE_TEXTURECUBE_SHADOW(textureName, samplerName, coord4)                         SHADOWCUBE_SAMPLE(textureName, samplerName, coord4)
	#define SAMPLE_TEXTURECUBE_ARRAY_SHADOW(textureName, samplerName, coord4, index)            ERROR_ON_UNSUPPORTED_FUNCTION(SAMPLE_TEXTURECUBE_ARRAY_SHADOW)
	#define LOAD_TEXTURE2D(textureName, unCoord2)                                               half4(0, 0, 0, 0)
	#define LOAD_TEXTURE2D_LOD(textureName, unCoord2, lod)                                      half4(0, 0, 0, 0)
	#define LOAD_TEXTURE2D_MSAA(textureName, unCoord2, sampleIndex)                             half4(0, 0, 0, 0)
	#define LOAD_TEXTURE2D_ARRAY(textureName, unCoord2, index)                                  half4(0, 0, 0, 0)
	#define LOAD_TEXTURE2D_ARRAY_MSAA(textureName, unCoord2, index, sampleIndex)                half4(0, 0, 0, 0)
	#define LOAD_TEXTURE2D_ARRAY_LOD(textureName, unCoord2, index, lod)                         half4(0, 0, 0, 0)
	#define LOAD_TEXTURE3D(textureName, unCoord3)                                               ERROR_ON_UNSUPPORTED_FUNCTION(LOAD_TEXTURE3D)
	#define LOAD_TEXTURE3D_LOD(textureName, unCoord3, lod)                                      ERROR_ON_UNSUPPORTED_FUNCTION(LOAD_TEXTURE3D_LOD)
	#define GATHER_TEXTURE2D(textureName, samplerName, coord2)                  ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_TEXTURE2D)
	#define GATHER_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)     ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_TEXTURE2D_ARRAY)
	#define GATHER_TEXTURECUBE(textureName, samplerName, coord3)                ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_TEXTURECUBE)
	#define GATHER_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)   ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_TEXTURECUBE_ARRAY)
	#define GATHER_RED_TEXTURE2D(textureName, samplerName, coord2)              ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_RED_TEXTURE2D)
	#define GATHER_GREEN_TEXTURE2D(textureName, samplerName, coord2)            ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_GREEN_TEXTURE2D)
	#define GATHER_BLUE_TEXTURE2D(textureName, samplerName, coord2)             ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_BLUE_TEXTURE2D)
	#define GATHER_ALPHA_TEXTURE2D(textureName, samplerName, coord2)            ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_ALPHA_TEXTURE2D)

#else
#error unsupported shader api
#endif
#ifndef UNITY_BRANCH
#   define UNITY_BRANCH
#endif
#ifndef UNITY_FLATTEN
#   define UNITY_FLATTEN
#endif
#ifndef UNITY_UNROLL
#   define UNITY_UNROLL
#endif
#ifndef UNITY_UNROLLX
#   define UNITY_UNROLLX(_x)
#endif
#ifndef UNITY_LOOP
#   define UNITY_LOOP
#endif
#define _USINGTEXCOORD1 1
         struct VertexToPixel
         {
            V2F_SHADOW_CASTER; 
            float3 worldPos : TEXCOORD1;
            float3 worldNormal : TEXCOORD2;
            float4 worldTangent : TEXCOORD3;
             float4 texcoord0 : TEXCOORD4;
             float4 texcoord1 : TEXCOORD5;
             float4 extraV2F0 : TEXCOORD9;
            UNITY_VERTEX_INPUT_INSTANCE_ID
            UNITY_VERTEX_OUTPUT_STEREO
         };
            struct Surface
            {
               half3 Albedo;
               half Height;
               half3 Normal;
               half Smoothness;
               half3 Emission;
               half Metallic;
               half3 Specular;
               half Occlusion;
               half SpecularPower; 
               half Alpha;
               float outputDepth; 
               half SpecularOcclusion;
               half SubsurfaceMask;
               half Thickness;
               half CoatMask;
               half CoatSmoothness;
               half Anisotropy;
               half IridescenceMask;
               half IridescenceThickness;
               int DiffusionProfileHash;
               float SpecularAAThreshold;
               float SpecularAAScreenSpaceVariance;
               float3 DiffuseGI;
               float3 BackDiffuseGI;
               float3 SpecularGI;
               float ior;
               float3 transmittanceColor;
               float atDistance;
               float transmittanceMask;
               float4 ShadowMask;
               float NormalAlpha;
               float MAOSAlpha;
            };
            struct Blackboard
            {
                float blackboardDummyData;
            };
            struct ShaderData
            {
               float4 clipPos; 
               float3 localSpacePosition;
               float3 localSpaceNormal;
               float3 localSpaceTangent;
               float3 worldSpacePosition;
               float3 worldSpaceNormal;
               float3 worldSpaceTangent;
               float tangentSign;

               float3 worldSpaceViewDir;
               float3 tangentSpaceViewDir;

               float4 texcoord0;
               float4 texcoord1;
               float4 texcoord2;
               float4 texcoord3;

               float2 screenUV;
               float4 screenPos;

               float4 vertexColor;
               bool isFrontFace;

               float4 extraV2F0;
               float4 extraV2F1;
               float4 extraV2F2;
               float4 extraV2F3;
               float4 extraV2F4;
               float4 extraV2F5;
               float4 extraV2F6;
               float4 extraV2F7;

               float3x3 TBNMatrix;
               Blackboard blackboard;
            };

            struct VertexData
            {
               #if SHADER_TARGET > 30
               #endif
               float4 vertex : POSITION;
               float3 normal : NORMAL;
               float4 tangent : TANGENT;
               float4 texcoord0 : TEXCOORD0;
               #if _URP && (_USINGTEXCOORD1 || _PASSMETA || _PASSFORWARD || _PASSGBUFFER)
                  float4 texcoord1 : TEXCOORD1;
               #endif

               #if _URP && (_USINGTEXCOORD2 || _PASSMETA || ((_PASSFORWARD || _PASSGBUFFER) && defined(DYNAMICLIGHTMAP_ON)))
                  float4 texcoord2 : TEXCOORD2;
               #endif

               #if _STANDARD && (_USINGTEXCOORD1 || (_PASSMETA || ((_PASSFORWARD || _PASSGBUFFER || _PASSFORWARDADD) && LIGHTMAP_ON)))
                  float4 texcoord1 : TEXCOORD1;
               #endif
               #if _STANDARD && (_USINGTEXCOORD2 || (_PASSMETA || ((_PASSFORWARD || _PASSGBUFFER) && DYNAMICLIGHTMAP_ON)))
                  float4 texcoord2 : TEXCOORD2;
               #endif
               #if _HDRP
                  float4 texcoord1 : TEXCOORD1;
                  float4 texcoord2 : TEXCOORD2;
               #endif
               #if _PASSMOTIONVECTOR || ((_PASSFORWARD || _PASSUNLIT) && defined(_WRITE_TRANSPARENT_MOTION_VECTOR))
                  float3 previousPositionOS : TEXCOORD4; 
                  #if defined (_ADD_PRECOMPUTED_VELOCITY)
                     float3 precomputedVelocity    : TEXCOORD5; 
                  #endif
               #endif

               UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct TessVertex 
            {
               float4 vertex : INTERNALTESSPOS;
               float3 normal : NORMAL;
               float4 tangent : TANGENT;
               float4 texcoord0 : TEXCOORD0;
               float4 texcoord1 : TEXCOORD1;
               float4 texcoord2 : TEXCOORD2;
                float4 extraV2F0 : TEXCOORD5;
               #if _PASSMOTIONVECTOR || ((_PASSFORWARD || _PASSUNLIT) && defined(_WRITE_TRANSPARENT_MOTION_VECTOR))
                  float3 previousPositionOS : TEXCOORD13; 
                  #if defined (_ADD_PRECOMPUTED_VELOCITY)
                     float3 precomputedVelocity : TEXCOORD14;
                  #endif
               #endif

               UNITY_VERTEX_INPUT_INSTANCE_ID
               UNITY_VERTEX_OUTPUT_STEREO
            };

            struct ExtraV2F
            {
               float4 extraV2F0;
               float4 extraV2F1;
               float4 extraV2F2;
               float4 extraV2F3;
               float4 extraV2F4;
               float4 extraV2F5;
               float4 extraV2F6;
               float4 extraV2F7;
               Blackboard blackboard;
               float4 time;
            };
            float3 WorldToTangentSpace(ShaderData d, float3 normal)
            {
               return mul(d.TBNMatrix, normal);
            }

            float3 TangentToWorldSpace(ShaderData d, float3 normal)
            {
               return mul(normal, d.TBNMatrix);
            }
            #if _STANDARD
               float3 TransformWorldToObject(float3 p) { return mul(unity_WorldToObject, float4(p, 1)); };
               float3 TransformObjectToWorld(float3 p) { return mul(unity_ObjectToWorld, float4(p, 1)); };
               float4 TransformWorldToObject(float4 p) { return mul(unity_WorldToObject, p); };
               float4 TransformObjectToWorld(float4 p) { return mul(unity_ObjectToWorld, p); };
               float4x4 GetWorldToObjectMatrix() { return unity_WorldToObject; }
               float4x4 GetObjectToWorldMatrix() { return unity_ObjectToWorld; }
               #if (defined(SHADER_API_D3D11) || defined(SHADER_API_XBOXONE) || defined(UNITY_COMPILER_HLSLCC) || defined(SHADER_API_PSSL) || (SHADER_TARGET_SURFACE_ANALYSIS && !SHADER_TARGET_SURFACE_ANALYSIS_MOJOSHADER))
                 #define UNITY_SAMPLE_TEX2D_LOD(tex,coord, lod) tex.SampleLevel (sampler##tex,coord, lod)
                 #define UNITY_SAMPLE_TEX2D_SAMPLER_LOD(tex,samplertex,coord, lod) tex.SampleLevel (sampler##samplertex,coord, lod)
              #else
                 #define UNITY_SAMPLE_TEX2D_LOD(tex,coord,lod) tex2D (tex,coord,0,lod)
                 #define UNITY_SAMPLE_TEX2D_SAMPLER_LOD(tex,samplertex,coord,lod) tex2D (tex,coord,0,lod)
              #endif

               #undef UNITY_MATRIX_I_M

               #define UNITY_MATRIX_I_M   unity_WorldToObject
            #endif

            float3 GetCameraWorldPosition()
            {
               #if _HDRP
                  return GetCameraRelativePositionWS(_WorldSpaceCameraPos);
               #else
                  return _WorldSpaceCameraPos;
               #endif
            }

            #if _GRABPASSUSED
               #if _STANDARD
                  TEXTURE2D(%GRABTEXTURE%);
                  SAMPLER(sampler_%GRABTEXTURE%);
               #endif

               half3 GetSceneColor(float2 uv)
               {
                  #if _STANDARD
                     return SAMPLE_TEXTURE2D(%GRABTEXTURE%, sampler_%GRABTEXTURE%, uv).rgb;
                  #else
                     return SHADERGRAPH_SAMPLE_SCENE_COLOR(uv);
                  #endif
               }
            #endif
            #if _STANDARD
               UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);
               float GetSceneDepth(float2 uv) { return SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, uv); }
               float GetLinear01Depth(float2 uv) { return Linear01Depth(GetSceneDepth(uv)); }
               float GetLinearEyeDepth(float2 uv) { return LinearEyeDepth(GetSceneDepth(uv)); } 
            #else
               float GetSceneDepth(float2 uv) { return SHADERGRAPH_SAMPLE_SCENE_DEPTH(uv); }
               float GetLinear01Depth(float2 uv) { return Linear01Depth(GetSceneDepth(uv), _ZBufferParams); }
               float GetLinearEyeDepth(float2 uv) { return LinearEyeDepth(GetSceneDepth(uv), _ZBufferParams); } 
            #endif

            float3 GetWorldPositionFromDepthBuffer(float2 uv, float3 worldSpaceViewDir)
            {
               float eye = GetLinearEyeDepth(uv);
               float3 camView = mul((float3x3)UNITY_MATRIX_M, transpose(mul(UNITY_MATRIX_I_M, UNITY_MATRIX_I_V)) [2].xyz);

               float dt = dot(worldSpaceViewDir, camView);
               float3 div = worldSpaceViewDir/dt;
               float3 wpos = (eye * div) + GetCameraWorldPosition();
               return wpos;
            }

            #if _HDRP
            float3 ObjectToWorldSpacePosition(float3 pos)
            {
               return GetAbsolutePositionWS(TransformObjectToWorld(pos));
            }
            #else
            float3 ObjectToWorldSpacePosition(float3 pos)
            {
               return TransformObjectToWorld(pos);
            }
            #endif

            #if _STANDARD
               UNITY_DECLARE_SCREENSPACE_TEXTURE(_CameraDepthNormalsTexture);
               float3 GetSceneNormal(float2 uv, float3 worldSpaceViewDir)
               {
                  float4 depthNorms = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_CameraDepthNormalsTexture, uv);
                  float3 norms = DecodeViewNormalStereo(depthNorms);
                  norms = mul((float3x3)UNITY_MATRIX_V, norms) * 0.5 + 0.5;
                  return norms;
               }
            #elif _HDRP && !_DECALSHADER
               float3 GetSceneNormal(float2 uv, float3 worldSpaceViewDir)
               {
                  NormalData nd;
                  DecodeFromNormalBuffer(_ScreenSize.xy * uv, nd);
                  return nd.normalWS;
               }
            #elif _URP
               #if (SHADER_LIBRARY_VERSION_MAJOR >= 10)
                  #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareNormalsTexture.hlsl"
               #endif

               float3 GetSceneNormal(float2 uv, float3 worldSpaceViewDir)
               {
                  #if (SHADER_LIBRARY_VERSION_MAJOR >= 10)
                     return SampleSceneNormals(uv);
                  #else
                     float3 wpos = GetWorldPositionFromDepthBuffer(uv, worldSpaceViewDir);
                     return normalize(-cross(ddx(wpos), ddy(wpos))) * 0.5 + 0.5;
                  #endif

                }
             #endif

             #if _HDRP

               half3 UnpackNormalmapRGorAG(half4 packednormal)
               {
                  packednormal.x *= packednormal.w;

                  half3 normal;
                  normal.xy = packednormal.xy * 2 - 1;
                  normal.z = sqrt(1 - saturate(dot(normal.xy, normal.xy)));
                  return normal;
               }
               half3 UnpackNormal(half4 packednormal)
               {
                  #if defined(UNITY_NO_DXT5nm)
                     return packednormal.xyz * 2 - 1;
                  #else
                     return UnpackNormalmapRGorAG(packednormal);
                  #endif
               }
            #endif
            #if _HDRP || _URP

               half3 UnpackScaleNormal(half4 packednormal, half scale)
               {
                 #ifndef UNITY_NO_DXT5nm
                   packednormal.x *= packednormal.w;
                 #endif
                   half3 normal;
                   normal.xy = (packednormal.xy * 2 - 1) * scale;
                   normal.z = sqrt(1 - saturate(dot(normal.xy, normal.xy)));
                   return normal;
               }	

             #endif
            void GetSun(out float3 lightDir, out float3 color)
            {
               lightDir = float3(0.5, 0.5, 0);
               color = 1;
               #if _HDRP
                  if (_DirectionalLightCount > 0)
                  {
                     DirectionalLightData light = _DirectionalLightDatas[0];
                     lightDir = -light.forward.xyz;
                     color = light.color;
                  }
               #elif _STANDARD
			         lightDir = normalize(_WorldSpaceLightPos0.xyz);
                  color = _LightColor0.rgb;
               #elif _URP
	               Light light = GetMainLight();
	               lightDir = light.direction;
	               color = light.color;
               #endif
            }
    half _ShadingMode;
    half _LightFunction;
    half _UseAlphaOnlyFromBaseMap;
    half _EnableToonShading;
    half _ShadingFunction;
    int _NumberOfCells;
	float _CellTransitionSmoothness;
	half _SumLightsBeforePosterization;
	half _ShadingUseLightColors;
    half _GradientMode;
    half _GradientBlending;
    float _GradientBlendFactor;

    float4 _GradientTex_TexelSize;
    half _ShadingAffectedByNormalMap;
    half _EnableShadows;
    float4 _CoreShadowColor;
    float _TerminatorPosition;
    float _TerminatorWidth;
    float _TerminatorSmoothness;
    float4 _FormShadowColor;

    half _EnableCastShadows;
    float _CastShadowsStrength;
    float _CastShadowsSmoothness;
    half _CastShadowColorMode;
    float4 _CastShadowColor;
    half _EnableSpecular;
    half _SpecularBlending;
    half4 _SpecularColor;
    float _SpecularSize;
    float _SpecularSmoothness;
    float _SpecularOpacity;
    half _SpecularAffectedByNormalMap;
    half _SpecularUseLightColors;
    half _EnableRim;
    half _RimBlending;
    float4 _RimColor;
    float _RimSize;
    float _RimSmoothness;
    float _RimOpacity;
    half _RimAffectedArea;
    half _RimAffectedByNormalMap;
     half _EnableStyling;

    half _EnableStylingDistanceFade;
    float _StylingDFStartingDistance;
    float _StylingDFFalloff;
    half _StylingAdjustDistanceFadeValue;
    float _StylingDistanceFadeValue;
    half _HatchingAffectedByNormalMap;
    half _EnableAntiAliasing;
    float4 _NoiseTex2_TexelSize;
    half _EnableShadingStyling;
    half _StylingShadingSyncWithOtherStyling;
    half _ShadingStyle;    
    half _SyncWithLightPartitioning;
    half _NumberOfCellsHatching;
    half _StylingTerminatorPosition;    
    float _StylingOvermodelingFactor;
    half _StylingShadingBlending;
    half _StylingShadingIsInverted;
    half _DrawSpace;
    half _SSCameraDistanceScaled;
    half _AnchorSSToObjectsOrigin;
    half _CoordinateSystem;
    half _PolarCenterMode;
    float4 _PolarCenter;
    float _StylingShadingDensity;
    float _StylingShadingInitialDirection;
    float _StylingShadingRotationBetweenCells;
    float _StylingShadingHalftonesOffset;
    float4 _StylingColor;
    float _StylingShadingOpacity;
    float _StylingShadingOpacityFalloff;
    float _StylingShadingThicknessControl;
    float _StylingShadingThickness;
    float _StylingShadingThicknessFalloff;
    float _StylingShadingHardness;
    float _StylingShadingHalftonesRoundness;
    float _StylingShadingHalftonesRoundnessFalloff;
    half _EnableShadingRandomizer;
    float _ShadingNoise1Size;
    float _ShadingNoise1Seed;
    float _ShadingNoise2Seed;
    float _NoiseIntensity;
    half _SpacingRandomMode;
    float _SpacingRandomIntensity;
    half _OpacityRandomMode;
    float _OpacityRandomIntensity;
    half _LengthRandomMode;
    float _LengthRandomIntensity;
    half _HardnessRandomMode;
    float _HardnessRandomIntensity;
    half _ThicknessRandomMode;
    float _ThicknesshRandomIntensity;
    half _EnableCastShadowsStyling;
    half _CastShadowsNumberOfCellsHatching;
    float _StylingCastShadowsSmoothness;
    half _StylingCastShadowsSyncWithOtherStyling;
    half _CastShadowsStyle;
    half _StylingCastShadowsBlending;
    half _StylingCastShadowsIsInverted;
    half _CastShadowsDrawSpace;
    half _CastShadowsSSCameraDistanceScaled;
    half _CastShadowsAnchorSSToObjectsOrigin;
    half _CastShadowsCoordinateSystem;
    half _CastShadowsPolarCenterMode;
    float4 _CastShadowsPolarCenter;
    float _StylingCastShadowsDensity;
    float _StylingCastShadowsInitialDirection;
    float _StylingCastShadowsRotationBetweenCells;
    float _StylingCastShadowsHalftonesOffset;
    float4 _StylingCastShadowsColor;
    float _StylingCastShadowsOpacity;
    float _StylingCastShadowsOpacityFalloff;
    half _StylingCastShadowsThicknessControl;
    float _StylingCastShadowsThickness;
    float _StylingCastShadowsThicknessFalloff;
    float _StylingCastShadowsHardness;
    float _StylingCastShadowsHalftonesRoundness;
    float _StylingCastShadowsHalftonesRoundnessFalloff;
    half _EnableCastShadowsRandomizer;
    float _CastShadowsNoise1Size;
    float _CastShadowsNoise1Seed;
    float _CastShadowsNoise2Seed;
    float _CastShadowsNoiseIntensity;
    half _CastShadowsSpacingRandomMode;
    float _CastShadowsSpacingRandomIntensity;
    half _CastShadowsOpacityRandomMode;
    float _CastShadowsOpacityRandomIntensity;
    half _CastShadowsHardnessRandomMode;
    float _CastShadowsHardnessRandomIntensity;
    half _CastShadowsLengthRandomMode;
    float _CastShadowsLengthRandomIntensity;
    half _CastShadowsThicknessRandomMode;
    float _CastShadowsThicknesshRandomIntensity;
    half _EnableSpecularStyling;
    half _SyncWithSpecular;
    float _StylingSpecularSize;
    float _StylingSpecularSmoothness;
    half _StylingSpecularCutOutShading;
    half _StylingSpecularUseLightColors;
    half _StylingSpecularSyncWithOtherStyling;
    half _SpecularStyle;
    half _StylingSpecularBlending;
    half _StylingSpecularIsInverted;
    half _SpecularDrawSpace;
    half _SpecularSSCameraDistanceScaled;
    half _SpecularAnchorSSToObjectsOrigin;
    half _SpecularCoordinateSystem;
    half _SpecularPolarCenterMode;
    float4 _SpecularPolarCenter;
    float _StylingSpecularDensity;    
    float _StylingSpecularRotation;
    float _StylingSpecularHalftonesOffset;
    float4 _StylingSpecularColor;
    float _StylingSpecularOpacity;
    float _StylingSpecularOpacityFalloff;
    float _StylingSpecularThicknessControl;
    float _StylingSpecularThickness;
    float _StylingSpecularThicknessFalloff;
    float _StylingSpecularHardness;
    float _StylingSpecularHalftonesRoundness;
    float _StylingSpecularHalftonesRoundnessFalloff;
    float _EnableSpecularRandomizer;    
    float _SpecularNoise1Size;
    float _SpecularNoise1Seed;
    float _SpecularNoise2Seed;
    float _SpecularNoiseIntensity;
    half _SpecularSpacingRandomMode;
    float _SpecularSpacingRandomIntensity;
    half _SpecularOpacityRandomMode;
    float _SpecularOpacityRandomIntensity;
    half _SpecularLengthRandomMode;
    float _SpecularLengthRandomIntensity;
    half _SpecularHardnessRandomMode;
    float _SpecularHardnessRandomIntensity;
    half _SpecularThicknessRandomMode;
    float _SpecularThicknesshRandomIntensity;
    half _EnableRimStyling;
    half _StylingRimSyncWithOtherStyling;
    half _SyncWithRim;
    float _StylingRimSize;
    float _StylingRimSmoothness;
    half _StylingRimAffectedArea;
    half _RimStyle;
    half _StylingRimBlending;
    half _StylingRimIsInverted;
    half _RimDrawSpace;
    half _RimSSCameraDistanceScaled;
    half _RimAnchorSSToObjectsOrigin;
    half _RimCoordinateSystem;
    half _RimPolarCenterMode;
    float4 _RimPolarCenter;  
    float _StylingRimDensity;    
    float _StylingRimRotation;
    float _StylingRimHalftonesOffset;
    float4 _StylingRimColor;
    float _StylingRimOpacity;
    float _StylingRimOpacityFalloff;
    float _StylingRimThicknessControl;
    float _StylingRimThickness;
    float _StylingRimThicknessFalloff;
    float _StylingRimHardness;
    float _StylingRimHalftonesRoundness;
    float _StylingRimHalftonesRoundnessFalloff;
    float _EnableRimRandomizer;        
    float _RimNoise1Size;
    float _RimNoise1Seed;
    float _RimNoise2Seed;
    float _RimNoiseIntensity;
    half _RimSpacingRandomMode;
    float _RimSpacingRandomIntensity;
    half _RimOpacityRandomMode; 
    float _RimOpacityRandomIntensity;
    half _RimLengthRandomMode;
    float _RimLengthRandomIntensity;
    half _RimHardnessRandomMode;
    float _RimHardnessRandomIntensity;
    half _RimThicknessRandomMode;
    float _RimThicknesshRandomIntensity;
    half4 _OutlineColor;
    half _OutlineWidth;
    half _EnableOutline;
    half4 _Color;
    half _Cutoff; 
    half _Mode;
    float _Cull;
    half _Metallic;
    half3 _EmissionColor;
    half _UVSec;

    half _BumpScale; 
    half _DetailNormalMapScale;
    float _Glossiness;
    float _GlossMapScale;
    half _OcclusionStrength;
    half _Parallax;

    float4 _MainTex_ST;
    float4 _DetailAlbedoMap_ST;
    sampler2D _NoiseMap1;
    sampler2D _NoiseMap2;

    sampler2D _HatchingMap;
    sampler2D _GradientTex;
    void Ext_ModifyVertex0 (inout VertexData v, inout ExtraV2F d)
    {
        float4x4 UnityObjectToClipPosMatrix = mul(UNITY_MATRIX_VP, GetObjectToWorldMatrix());
        float4 pos = mul(UnityObjectToClipPosMatrix, v.vertex);
        float4 objectOriginClipPos = mul(UnityObjectToClipPosMatrix, float4(0, 0, 0, 1));
        objectOriginClipPos.xy = clamp(objectOriginClipPos.xy, -1.0*objectOriginClipPos.w, 1.0*objectOriginClipPos.w);
        float4 originScreenPos = ComputeScreenPos(objectOriginClipPos);
        float2 originScreenUV = originScreenPos.xy/originScreenPos.w;
        float4 screenPos = ComputeScreenPos(pos);
        float2 uv = ((screenPos.xy) / screenPos.w);
        float4 ssss = ComputeScreenPos(pos);
       d.extraV2F0 = float4(originScreenUV,originScreenUV);
    }
    sampler2D _MainTex;
    sampler2D _DetailAlbedoMap;
    sampler2D _DetailMask;
    sampler2D _BumpMap;
    sampler2D _DetailNormalMap;
    sampler2D _MetallicGlossMap;
    sampler2D _OcclusionMap;
    sampler2D _ParallaxMap;
    sampler2D _EmissionMap;
	void Ext_SurfaceFunction1 (inout Surface o, ShaderData d)
	{
        float4 texcoords;
        texcoords.xy = d.texcoord0.xy * _MainTex_ST.xy + _MainTex_ST.zw; 
        texcoords.zw = (_UVSec == 0) ? d.texcoord0.xy * _DetailAlbedoMap_ST.xy + _DetailAlbedoMap_ST.zw : d.texcoord1.xy * _DetailAlbedoMap_ST.xy + _DetailAlbedoMap_ST.zw;
    #if _SHADING_COLOR
        half4 diffuseAlpha = tex2D(_MainTex, texcoords.xy);
        o.Albedo = diffuseAlpha.rgb * _Color.rgb;
        o.Normal = UnpackScaleNormal(tex2D(_BumpMap, texcoords.xy), _BumpScale);

		half alpha = diffuseAlpha.a;
        o.Alpha = alpha * _Color.a;
        if (_Mode) 
        {
            clip (alpha - _Cutoff);
        }

        #ifdef _EMISSION
            half3 emission = tex2D(_EmissionMap, texcoords.xy).rgb * _EmissionColor.rgb; 
            o.Emission = emission;
        #endif
    #else
        #ifdef _PARALLAXMAP
            half h = tex2D (_ParallaxMap, texcoords.xy).g;
            float2 offset = ParallaxOffset1Step (h, _Parallax, d.tangentSpaceViewDir);
            texcoords = float4(texcoords.xy + offset, texcoords.zw + offset);
            o.Normal = float3(0, 0, 1);
        #endif
        half3 albedo = tex2D(_MainTex, texcoords.xy).rgb * _Color.rgb; 
        #ifdef _DETAIL_MULX2
                half3 detailAlbedo = tex2D (_DetailAlbedoMap, texcoords.zw).rgb;
                albedo *= LerpWhiteTo (detailAlbedo * unity_ColorSpaceDouble.rgb, tex2D(_DetailMask, texcoords.xy).a);
        #endif
        o.Albedo = albedo;
        half2 metallicSmoothness = half2(0,0);
        #ifdef _METALLICGLOSSMAP
            half4 metallicGlossMap = tex2D(_MetallicGlossMap, texcoords.xy);
            metallicSmoothness.r = metallicGlossMap.r;
            #ifdef _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
                metallicSmoothness.g = tex2D(_MainTex, texcoords.xy).a;
            #else
                metallicSmoothness.g = metallicGlossMap.a;
            #endif
            metallicSmoothness.g *= _GlossMapScale;
        #else
            metallicSmoothness.r = _Metallic;
            #ifdef _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
                metallicSmoothness.g  = tex2D(_MainTex, texcoords.xy).a * _GlossMapScale;
            #else
                metallicSmoothness.g = _Glossiness;
            #endif
        #endif
        o.Metallic = metallicSmoothness.r;
        o.Smoothness = metallicSmoothness.g;
        o.Occlusion = lerp(1, tex2D(_OcclusionMap, texcoords.xy).g, _OcclusionStrength);
        half alpha = 0;
        #if defined(_SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A)
            alpha = _Color.a;
        #else
            alpha =  tex2D(_MainTex, texcoords.xy).a * _Color.a;
        #endif
        if (_Mode) 
        {
            clip (alpha - _Cutoff);
        }
        o.Alpha = alpha;
        #ifdef _NORMALMAP
            half3 normal = UnpackScaleNormal(tex2D(_BumpMap, texcoords.xy), _BumpScale);
            #ifdef _DETAIL_MULX2
                half3 detailNormal = UnpackScaleNormal(tex2D(_DetailNormalMap, texcoords.zw), _DetailNormalMapScale);
	            detailNormal = lerp(float3(0, 0, 1), detailNormal, tex2D(_DetailMask, texcoords.xy).a);
	            normal = BlendNormals(normal, detailNormal);
            #endif
            o.Normal = normal;
        #endif
        #ifdef _EMISSION
            o.Emission += tex2D(_EmissionMap, texcoords.xy).rgb * _EmissionColor.rgb; 
        #endif
    #endif
	}
            void ChainSurfaceFunction(inout Surface l, inout ShaderData d)
            {
                  Ext_SurfaceFunction1(l, d);
            }

#if !_DECALSHADER

            void ChainModifyVertex(inout VertexData v, inout VertexToPixel v2p, float4 time)
            {
                 ExtraV2F d;
                 ZERO_INITIALIZE(ExtraV2F, d);
                 ZERO_INITIALIZE(Blackboard, d.blackboard);
                 d.time = time;

                   Ext_ModifyVertex0(v, d);
                  v2p.extraV2F0 = d.extraV2F0;
            }

            void ChainModifyTessellatedVertex(inout VertexData v, inout VertexToPixel v2p)
            {
               ExtraV2F d;
               ZERO_INITIALIZE(ExtraV2F, d);
               ZERO_INITIALIZE(Blackboard, d.blackboard);
                d.extraV2F0 = v2p.extraV2F0;
                v2p.extraV2F0 = d.extraV2F0;
            }

            void ChainFinalColorForward(inout Surface l, inout ShaderData d, inout half4 color)
            {
            }

            void ChainFinalGBufferStandard(inout Surface s, inout ShaderData d, inout half4 GBuffer0, inout half4 GBuffer1, inout half4 GBuffer2, inout half4 outEmission, inout half4 outShadowMask)
            {
            }
#endif
#if _DECALSHADER

        ShaderData CreateShaderData(SurfaceDescriptionInputs IN)
        {
            ShaderData d = (ShaderData)0;
            d.TBNMatrix = float3x3(IN.WorldSpaceTangent, IN.WorldSpaceBiTangent, IN.WorldSpaceNormal);
            d.worldSpaceNormal = IN.WorldSpaceNormal;
            d.worldSpaceTangent = IN.WorldSpaceTangent;

            d.worldSpacePosition = IN.WorldSpacePosition;
            d.texcoord0 = IN.uv0.xyxy;
            d.screenPos = IN.ScreenPosition;

            d.worldSpaceViewDir = normalize(_WorldSpaceCameraPos - d.worldSpacePosition);

            d.tangentSpaceViewDir = mul(d.TBNMatrix, d.worldSpaceViewDir);
            #if _HDRP
            #else
            #endif
            return d;
        }
#else

         ShaderData CreateShaderData(VertexToPixel i
                  #if NEED_FACING
                     , bool facing
                  #endif
         )
         {
            ShaderData d = (ShaderData)0;
            d.clipPos = i.pos;
            d.worldSpacePosition = i.worldPos;

            d.worldSpaceNormal = normalize(i.worldNormal);
            d.worldSpaceTangent.xyz = normalize(i.worldTangent.xyz);

            d.tangentSign = i.worldTangent.w * unity_WorldTransformParams.w;
            float3 bitangent = cross(d.worldSpaceTangent.xyz, d.worldSpaceNormal) * d.tangentSign;
            d.TBNMatrix = float3x3(d.worldSpaceTangent, -bitangent, d.worldSpaceNormal);
            d.worldSpaceViewDir = normalize(_WorldSpaceCameraPos - i.worldPos);

            d.tangentSpaceViewDir = mul(d.TBNMatrix, d.worldSpaceViewDir);
             d.texcoord0 = i.texcoord0;
             d.texcoord1 = i.texcoord1;
            #if _HDRP
            #else
            #endif
             d.extraV2F0 = i.extraV2F0;
            return d;
         }

#endif
         VertexToPixel Vert (VertexData v)
         {
            UNITY_SETUP_INSTANCE_ID(v);
            VertexToPixel o;
            UNITY_INITIALIZE_OUTPUT(VertexToPixel,o);
            UNITY_TRANSFER_INSTANCE_ID(v,o);
            UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

#if !_TESSELLATION_ON
           ChainModifyVertex(v, o, _Time);
#endif

             o.texcoord0 = v.texcoord0;
             o.texcoord1 = v.texcoord1;
            o.worldPos = mul(GetObjectToWorldMatrix(), v.vertex).xyz;
            o.worldNormal = UnityObjectToWorldNormal(v.normal);
            o.worldTangent = float4(UnityObjectToWorldDir(v.tangent.xyz), v.tangent.w);
            TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
            return o;
         }
         fixed4 Frag (VertexToPixel IN
         #ifdef _DEPTHOFFSET_ON
              , out float outputDepth : SV_Depth
         #endif
         #if NEED_FACING
            , bool facing : SV_IsFrontFace
         #endif
         ) : SV_Target
         {
           UNITY_SETUP_INSTANCE_ID(IN);
           #ifdef FOG_COMBINED_WITH_TSPACE
             UNITY_EXTRACT_FOG_FROM_TSPACE(IN);
           #elif defined (FOG_COMBINED_WITH_WORLD_POS)
             UNITY_EXTRACT_FOG_FROM_WORLD_POS(IN);
           #else
             UNITY_EXTRACT_FOG(IN);
           #endif

           #ifndef USING_DIRECTIONAL_LIGHT
             fixed3 lightDir = normalize(UnityWorldSpaceLightDir(IN.worldPos));
           #else
             fixed3 lightDir = _WorldSpaceLightPos0.xyz;
           #endif
           ShaderData d = CreateShaderData(IN
              #if NEED_FACING
                 , facing
              #endif
           );

           Surface l = (Surface)0;

           #ifdef _DEPTHOFFSET_ON
              l.outputDepth = outputDepth;
           #endif

           l.Albedo = half3(0.5, 0.5, 0.5);
           l.Normal = float3(0,0,1);
           l.Occlusion = 1;
           l.Alpha = 1;

           ChainSurfaceFunction(l, d);

           #ifdef _DEPTHOFFSET_ON
              outputDepth = l.outputDepth;
           #endif

           SHADOW_CASTER_FRAGMENT(IN)
         }
         ENDCG

      }
	   Pass
      {
		   Name "Meta"
		   Tags { "LightMode" = "Meta" }
		   Cull Off

             Cull [_Cull]
    Cull [_Cull]
         CGPROGRAM

            #pragma vertex Vert
   #pragma fragment Frag
         #pragma target 3.0
         #pragma multi_compile_instancing
         #pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
         #pragma shader_feature EDITOR_VISUALIZATION

         #include "HLSLSupport.cginc"
         #define UNITY_INSTANCED_LOD_FADE
         #define UNITY_INSTANCED_SH
         #define UNITY_INSTANCED_LIGHTMAPSTS
         #include "UnityShaderVariables.cginc"
         #include "UnityShaderUtilities.cginc"

         #include "UnityCG.cginc"
         #include "Lighting.cginc"
         #include "UnityPBSLighting.cginc"
         #include "UnityMetaPass.cginc"

         #define _PASSMETA 1
     #pragma shader_feature_local_fragment  _SHADING_COLOR _SHADING_BLINNPHONG _SHADING_PBR    
        #pragma shader_feature_local _NORMALMAP
        #pragma shader_feature_local _METALLICGLOSSMAP
        #pragma shader_feature_local _PARALLAXMAP
        #pragma shader_feature_local _DETAIL_MULX2
        #pragma shader_feature_local _EMISSION       
        #pragma shader_feature_local _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
   #define _STANDARD 1
#if defined(SHADER_API_GAMECORE)

	#define ZERO_INITIALIZE(type, name) name = (type)0;
	#define ZERO_INITIALIZE_ARRAY(type, name, arraySize) { for (int arrayIndex = 0; arrayIndex < arraySize; arrayIndex++) { name[arrayIndex] = (type)0; } }
	#define CALCULATE_TEXTURE2D_LOD(textureName, samplerName, coord2) textureName.CalculateLevelOfDetail(samplerName, coord2)
	#define TEXTURE2D(textureName)                Texture2D textureName
	#define TEXTURE2D_ARRAY(textureName)          Texture2DArray textureName
	#define TEXTURECUBE(textureName)              TextureCube textureName
	#define TEXTURECUBE_ARRAY(textureName)        TextureCubeArray textureName
	#define TEXTURE3D(textureName)                Texture3D textureName

	#define TEXTURE2D_FLOAT(textureName)          TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_FLOAT(textureName)    TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_FLOAT(textureName)        TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_FLOAT(textureName)  TEXTURECUBE_ARRAY(textureName)
	#define TEXTURE3D_FLOAT(textureName)          TEXTURE3D(textureName)

	#define TEXTURE2D_HALF(textureName)           TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_HALF(textureName)     TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_HALF(textureName)         TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_HALF(textureName)   TEXTURECUBE_ARRAY(textureName)
	#define TEXTURE3D_HALF(textureName)           TEXTURE3D(textureName)

	#define TEXTURE2D_SHADOW(textureName)         TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_SHADOW(textureName)   TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_SHADOW(textureName)       TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_SHADOW(textureName) TEXTURECUBE_ARRAY(textureName)

	#define RW_TEXTURE2D(type, textureName)       RWTexture2D<type> textureName
	#define RW_TEXTURE2D_ARRAY(type, textureName) RWTexture2DArray<type> textureName
	#define RW_TEXTURE3D(type, textureName)       RWTexture3D<type> textureName

	#define SAMPLER(samplerName)                  SamplerState samplerName
	#define SAMPLER_CMP(samplerName)              SamplerComparisonState samplerName
	#define ASSIGN_SAMPLER(samplerName, samplerValue) samplerName = samplerValue

	#define TEXTURE2D_PARAM(textureName, samplerName)                 TEXTURE2D(textureName),         SAMPLER(samplerName)
	#define TEXTURE2D_ARRAY_PARAM(textureName, samplerName)           TEXTURE2D_ARRAY(textureName),   SAMPLER(samplerName)
	#define TEXTURECUBE_PARAM(textureName, samplerName)               TEXTURECUBE(textureName),       SAMPLER(samplerName)
	#define TEXTURECUBE_ARRAY_PARAM(textureName, samplerName)         TEXTURECUBE_ARRAY(textureName), SAMPLER(samplerName)
	#define TEXTURE3D_PARAM(textureName, samplerName)                 TEXTURE3D(textureName),         SAMPLER(samplerName)

	#define TEXTURE2D_SHADOW_PARAM(textureName, samplerName)          TEXTURE2D(textureName),         SAMPLER_CMP(samplerName)
	#define TEXTURE2D_ARRAY_SHADOW_PARAM(textureName, samplerName)    TEXTURE2D_ARRAY(textureName),   SAMPLER_CMP(samplerName)
	#define TEXTURECUBE_SHADOW_PARAM(textureName, samplerName)        TEXTURECUBE(textureName),       SAMPLER_CMP(samplerName)
	#define TEXTURECUBE_ARRAY_SHADOW_PARAM(textureName, samplerName)  TEXTURECUBE_ARRAY(textureName), SAMPLER_CMP(samplerName)

	#define TEXTURE2D_ARGS(textureName, samplerName)                textureName, samplerName
	#define TEXTURE2D_ARRAY_ARGS(textureName, samplerName)          textureName, samplerName
	#define TEXTURECUBE_ARGS(textureName, samplerName)              textureName, samplerName
	#define TEXTURECUBE_ARRAY_ARGS(textureName, samplerName)        textureName, samplerName
	#define TEXTURE3D_ARGS(textureName, samplerName)                textureName, samplerName

	#define TEXTURE2D_SHADOW_ARGS(textureName, samplerName)         textureName, samplerName
	#define TEXTURE2D_ARRAY_SHADOW_ARGS(textureName, samplerName)   textureName, samplerName
	#define TEXTURECUBE_SHADOW_ARGS(textureName, samplerName)       textureName, samplerName
	#define TEXTURECUBE_ARRAY_SHADOW_ARGS(textureName, samplerName) textureName, samplerName

	#define PLATFORM_SAMPLE_TEXTURE2D(textureName, samplerName, coord2)                               textureName.Sample(samplerName, coord2)
	#define PLATFORM_SAMPLE_TEXTURE2D_LOD(textureName, samplerName, coord2, lod)                      textureName.SampleLevel(samplerName, coord2, lod)
	#define PLATFORM_SAMPLE_TEXTURE2D_BIAS(textureName, samplerName, coord2, bias)                    textureName.SampleBias(samplerName, coord2, bias)
	#define PLATFORM_SAMPLE_TEXTURE2D_GRAD(textureName, samplerName, coord2, dpdx, dpdy)              textureName.SampleGrad(samplerName, coord2, dpdx, dpdy)
	#define PLATFORM_SAMPLE_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)                  textureName.Sample(samplerName, float3(coord2, index))
	#define PLATFORM_SAMPLE_TEXTURE2D_ARRAY_LOD(textureName, samplerName, coord2, index, lod)         textureName.SampleLevel(samplerName, float3(coord2, index), lod)
	#define PLATFORM_SAMPLE_TEXTURE2D_ARRAY_BIAS(textureName, samplerName, coord2, index, bias)       textureName.SampleBias(samplerName, float3(coord2, index), bias)
	#define PLATFORM_SAMPLE_TEXTURE2D_ARRAY_GRAD(textureName, samplerName, coord2, index, dpdx, dpdy) textureName.SampleGrad(samplerName, float3(coord2, index), dpdx, dpdy)
	#define PLATFORM_SAMPLE_TEXTURECUBE(textureName, samplerName, coord3)                             textureName.Sample(samplerName, coord3)
	#define PLATFORM_SAMPLE_TEXTURECUBE_LOD(textureName, samplerName, coord3, lod)                    textureName.SampleLevel(samplerName, coord3, lod)
	#define PLATFORM_SAMPLE_TEXTURECUBE_BIAS(textureName, samplerName, coord3, bias)                  textureName.SampleBias(samplerName, coord3, bias)
	#define PLATFORM_SAMPLE_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)                textureName.Sample(samplerName, float4(coord3, index))
	#define PLATFORM_SAMPLE_TEXTURECUBE_ARRAY_LOD(textureName, samplerName, coord3, index, lod)       textureName.SampleLevel(samplerName, float4(coord3, index), lod)
	#define PLATFORM_SAMPLE_TEXTURECUBE_ARRAY_BIAS(textureName, samplerName, coord3, index, bias)     textureName.SampleBias(samplerName, float4(coord3, index), bias)
	#define PLATFORM_SAMPLE_TEXTURE3D(textureName, samplerName, coord3)                               textureName.Sample(samplerName, coord3)
	#define PLATFORM_SAMPLE_TEXTURE3D_LOD(textureName, samplerName, coord3, lod)                      textureName.SampleLevel(samplerName, coord3, lod)

	#define SAMPLE_TEXTURE2D(textureName, samplerName, coord2)                               PLATFORM_SAMPLE_TEXTURE2D(textureName, samplerName, coord2)
	#define SAMPLE_TEXTURE2D_LOD(textureName, samplerName, coord2, lod)                      PLATFORM_SAMPLE_TEXTURE2D_LOD(textureName, samplerName, coord2, lod)
	#define SAMPLE_TEXTURE2D_BIAS(textureName, samplerName, coord2, bias)                    PLATFORM_SAMPLE_TEXTURE2D_BIAS(textureName, samplerName, coord2, bias)
	#define SAMPLE_TEXTURE2D_GRAD(textureName, samplerName, coord2, dpdx, dpdy)              PLATFORM_SAMPLE_TEXTURE2D_GRAD(textureName, samplerName, coord2, dpdx, dpdy)
	#define SAMPLE_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)                  PLATFORM_SAMPLE_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)
	#define SAMPLE_TEXTURE2D_ARRAY_LOD(textureName, samplerName, coord2, index, lod)         PLATFORM_SAMPLE_TEXTURE2D_ARRAY_LOD(textureName, samplerName, coord2, index, lod)
	#define SAMPLE_TEXTURE2D_ARRAY_BIAS(textureName, samplerName, coord2, index, bias)       PLATFORM_SAMPLE_TEXTURE2D_ARRAY_BIAS(textureName, samplerName, coord2, index, bias)
	#define SAMPLE_TEXTURE2D_ARRAY_GRAD(textureName, samplerName, coord2, index, dpdx, dpdy) PLATFORM_SAMPLE_TEXTURE2D_ARRAY_GRAD(textureName, samplerName, coord2, index, dpdx, dpdy)
	#define SAMPLE_TEXTURECUBE(textureName, samplerName, coord3)                             PLATFORM_SAMPLE_TEXTURECUBE(textureName, samplerName, coord3)
	#define SAMPLE_TEXTURECUBE_LOD(textureName, samplerName, coord3, lod)                    PLATFORM_SAMPLE_TEXTURECUBE_LOD(textureName, samplerName, coord3, lod)
	#define SAMPLE_TEXTURECUBE_BIAS(textureName, samplerName, coord3, bias)                  PLATFORM_SAMPLE_TEXTURECUBE_BIAS(textureName, samplerName, coord3, bias)
	#define SAMPLE_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)                PLATFORM_SAMPLE_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)
	#define SAMPLE_TEXTURECUBE_ARRAY_LOD(textureName, samplerName, coord3, index, lod)       PLATFORM_SAMPLE_TEXTURECUBE_ARRAY_LOD(textureName, samplerName, coord3, index, lod)
	#define SAMPLE_TEXTURECUBE_ARRAY_BIAS(textureName, samplerName, coord3, index, bias)     PLATFORM_SAMPLE_TEXTURECUBE_ARRAY_BIAS(textureName, samplerName, coord3, index, bias)
	#define SAMPLE_TEXTURE3D(textureName, samplerName, coord3)                               PLATFORM_SAMPLE_TEXTURE3D(textureName, samplerName, coord3)
	#define SAMPLE_TEXTURE3D_LOD(textureName, samplerName, coord3, lod)                      PLATFORM_SAMPLE_TEXTURE3D_LOD(textureName, samplerName, coord3, lod)

	#define SAMPLE_TEXTURE2D_SHADOW(textureName, samplerName, coord3)                    textureName.SampleCmpLevelZero(samplerName, (coord3).xy, (coord3).z)
	#define SAMPLE_TEXTURE2D_ARRAY_SHADOW(textureName, samplerName, coord3, index)       textureName.SampleCmpLevelZero(samplerName, float3((coord3).xy, index), (coord3).z)
	#define SAMPLE_TEXTURECUBE_SHADOW(textureName, samplerName, coord4)                  textureName.SampleCmpLevelZero(samplerName, (coord4).xyz, (coord4).w)
	#define SAMPLE_TEXTURECUBE_ARRAY_SHADOW(textureName, samplerName, coord4, index)     textureName.SampleCmpLevelZero(samplerName, float4((coord4).xyz, index), (coord4).w)

	#define SAMPLE_DEPTH_TEXTURE(textureName, samplerName, coord2)          SAMPLE_TEXTURE2D(textureName, samplerName, coord2).r
	#define SAMPLE_DEPTH_TEXTURE_LOD(textureName, samplerName, coord2, lod) SAMPLE_TEXTURE2D_LOD(textureName, samplerName, coord2, lod).r

	#define LOAD_TEXTURE2D(textureName, unCoord2)                                   textureName.Load(int3(unCoord2, 0))
	#define LOAD_TEXTURE2D_LOD(textureName, unCoord2, lod)                          textureName.Load(int3(unCoord2, lod))
	#define LOAD_TEXTURE2D_MSAA(textureName, unCoord2, sampleIndex)                 textureName.Load(unCoord2, sampleIndex)
	#define LOAD_TEXTURE2D_ARRAY(textureName, unCoord2, index)                      textureName.Load(int4(unCoord2, index, 0))
	#define LOAD_TEXTURE2D_ARRAY_MSAA(textureName, unCoord2, index, sampleIndex)    textureName.Load(int3(unCoord2, index), sampleIndex)
	#define LOAD_TEXTURE2D_ARRAY_LOD(textureName, unCoord2, index, lod)             textureName.Load(int4(unCoord2, index, lod))
	#define LOAD_TEXTURE3D(textureName, unCoord3)                                   textureName.Load(int4(unCoord3, 0))
	#define LOAD_TEXTURE3D_LOD(textureName, unCoord3, lod)                          textureName.Load(int4(unCoord3, lod))

	#define PLATFORM_SUPPORT_GATHER
	#define GATHER_TEXTURE2D(textureName, samplerName, coord2)                textureName.Gather(samplerName, coord2)
	#define GATHER_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)   textureName.Gather(samplerName, float3(coord2, index))
	#define GATHER_TEXTURECUBE(textureName, samplerName, coord3)              textureName.Gather(samplerName, coord3)
	#define GATHER_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index) textureName.Gather(samplerName, float4(coord3, index))
	#define GATHER_RED_TEXTURE2D(textureName, samplerName, coord2)            textureName.GatherRed(samplerName, coord2)
	#define GATHER_GREEN_TEXTURE2D(textureName, samplerName, coord2)          textureName.GatherGreen(samplerName, coord2)
	#define GATHER_BLUE_TEXTURE2D(textureName, samplerName, coord2)           textureName.GatherBlue(samplerName, coord2)
	#define GATHER_ALPHA_TEXTURE2D(textureName, samplerName, coord2)          textureName.GatherAlpha(samplerName, coord2)
#elif defined(SHADER_API_XBOXONE)
	#define ZERO_INITIALIZE(type, name) name = (type)0;
	#define ZERO_INITIALIZE_ARRAY(type, name, arraySize) { for (int arrayIndex = 0; arrayIndex < arraySize; arrayIndex++) { name[arrayIndex] = (type)0; } }
	#define CALCULATE_TEXTURE2D_LOD(textureName, samplerName, coord2) textureName.CalculateLevelOfDetail(samplerName, coord2)
	#define TEXTURE2D(textureName)                Texture2D textureName
	#define TEXTURE2D_ARRAY(textureName)          Texture2DArray textureName
	#define TEXTURECUBE(textureName)              TextureCube textureName
	#define TEXTURECUBE_ARRAY(textureName)        TextureCubeArray textureName
	#define TEXTURE3D(textureName)                Texture3D textureName

	#define TEXTURE2D_FLOAT(textureName)          TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_FLOAT(textureName)    TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_FLOAT(textureName)        TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_FLOAT(textureName)  TEXTURECUBE_ARRAY(textureName)
	#define TEXTURE3D_FLOAT(textureName)          TEXTURE3D(textureName)

	#define TEXTURE2D_HALF(textureName)           TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_HALF(textureName)     TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_HALF(textureName)         TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_HALF(textureName)   TEXTURECUBE_ARRAY(textureName)
	#define TEXTURE3D_HALF(textureName)           TEXTURE3D(textureName)

	#define TEXTURE2D_SHADOW(textureName)         TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_SHADOW(textureName)   TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_SHADOW(textureName)       TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_SHADOW(textureName) TEXTURECUBE_ARRAY(textureName)

	#define RW_TEXTURE2D(type, textureName)       RWTexture2D<type> textureName
	#define RW_TEXTURE2D_ARRAY(type, textureName) RWTexture2DArray<type> textureName
	#define RW_TEXTURE3D(type, textureName)       RWTexture3D<type> textureName

	#define SAMPLER(samplerName)                  SamplerState samplerName
	#define SAMPLER_CMP(samplerName)              SamplerComparisonState samplerName

	#define TEXTURE2D_PARAM(textureName, samplerName)                 TEXTURE2D(textureName),         SAMPLER(samplerName)
	#define TEXTURE2D_ARRAY_PARAM(textureName, samplerName)           TEXTURE2D_ARRAY(textureName),   SAMPLER(samplerName)
	#define TEXTURECUBE_PARAM(textureName, samplerName)               TEXTURECUBE(textureName),       SAMPLER(samplerName)
	#define TEXTURECUBE_ARRAY_PARAM(textureName, samplerName)         TEXTURECUBE_ARRAY(textureName), SAMPLER(samplerName)
	#define TEXTURE3D_PARAM(textureName, samplerName)                 TEXTURE3D(textureName),         SAMPLER(samplerName)

	#define TEXTURE2D_SHADOW_PARAM(textureName, samplerName)          TEXTURE2D(textureName),         SAMPLER_CMP(samplerName)
	#define TEXTURE2D_ARRAY_SHADOW_PARAM(textureName, samplerName)    TEXTURE2D_ARRAY(textureName),   SAMPLER_CMP(samplerName)
	#define TEXTURECUBE_SHADOW_PARAM(textureName, samplerName)        TEXTURECUBE(textureName),       SAMPLER_CMP(samplerName)
	#define TEXTURECUBE_ARRAY_SHADOW_PARAM(textureName, samplerName)  TEXTURECUBE_ARRAY(textureName), SAMPLER_CMP(samplerName)

	#define TEXTURE2D_ARGS(textureName, samplerName)                textureName, samplerName
	#define TEXTURE2D_ARRAY_ARGS(textureName, samplerName)          textureName, samplerName
	#define TEXTURECUBE_ARGS(textureName, samplerName)              textureName, samplerName
	#define TEXTURECUBE_ARRAY_ARGS(textureName, samplerName)        textureName, samplerName
	#define TEXTURE3D_ARGS(textureName, samplerName)                textureName, samplerName

	#define TEXTURE2D_SHADOW_ARGS(textureName, samplerName)         textureName, samplerName
	#define TEXTURE2D_ARRAY_SHADOW_ARGS(textureName, samplerName)   textureName, samplerName
	#define TEXTURECUBE_SHADOW_ARGS(textureName, samplerName)       textureName, samplerName
	#define TEXTURECUBE_ARRAY_SHADOW_ARGS(textureName, samplerName) textureName, samplerName

	#define SAMPLE_TEXTURE2D(textureName, samplerName, coord2)                               textureName.Sample(samplerName, coord2)
	#define SAMPLE_TEXTURE2D_LOD(textureName, samplerName, coord2, lod)                      textureName.SampleLevel(samplerName, coord2, lod)
	#define SAMPLE_TEXTURE2D_BIAS(textureName, samplerName, coord2, bias)                    textureName.SampleBias(samplerName, coord2, bias)
	#define SAMPLE_TEXTURE2D_GRAD(textureName, samplerName, coord2, dpdx, dpdy)              textureName.SampleGrad(samplerName, coord2, dpdx, dpdy)
	#define SAMPLE_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)                  textureName.Sample(samplerName, float3(coord2, index))
	#define SAMPLE_TEXTURE2D_ARRAY_LOD(textureName, samplerName, coord2, index, lod)         textureName.SampleLevel(samplerName, float3(coord2, index), lod)
	#define SAMPLE_TEXTURE2D_ARRAY_BIAS(textureName, samplerName, coord2, index, bias)       textureName.SampleBias(samplerName, float3(coord2, index), bias)
	#define SAMPLE_TEXTURE2D_ARRAY_GRAD(textureName, samplerName, coord2, index, dpdx, dpdy) textureName.SampleGrad(samplerName, float3(coord2, index), dpdx, dpdy)
	#define SAMPLE_TEXTURECUBE(textureName, samplerName, coord3)                             textureName.Sample(samplerName, coord3)
	#define SAMPLE_TEXTURECUBE_LOD(textureName, samplerName, coord3, lod)                    textureName.SampleLevel(samplerName, coord3, lod)
	#define SAMPLE_TEXTURECUBE_BIAS(textureName, samplerName, coord3, bias)                  textureName.SampleBias(samplerName, coord3, bias)
	#define SAMPLE_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)                textureName.Sample(samplerName, float4(coord3, index))
	#define SAMPLE_TEXTURECUBE_ARRAY_LOD(textureName, samplerName, coord3, index, lod)       textureName.SampleLevel(samplerName, float4(coord3, index), lod)
	#define SAMPLE_TEXTURECUBE_ARRAY_BIAS(textureName, samplerName, coord3, index, bias)     textureName.SampleBias(samplerName, float4(coord3, index), bias)
	#define SAMPLE_TEXTURE3D(textureName, samplerName, coord3)                               textureName.Sample(samplerName, coord3)
	#define SAMPLE_TEXTURE3D_LOD(textureName, samplerName, coord3, lod)                      textureName.SampleLevel(samplerName, coord3, lod)

	#define SAMPLE_TEXTURE2D_SHADOW(textureName, samplerName, coord3)                    textureName.SampleCmpLevelZero(samplerName, (coord3).xy, (coord3).z)
	#define SAMPLE_TEXTURE2D_ARRAY_SHADOW(textureName, samplerName, coord3, index)       textureName.SampleCmpLevelZero(samplerName, float3((coord3).xy, index), (coord3).z)
	#define SAMPLE_TEXTURECUBE_SHADOW(textureName, samplerName, coord4)                  textureName.SampleCmpLevelZero(samplerName, (coord4).xyz, (coord4).w)
	#define SAMPLE_TEXTURECUBE_ARRAY_SHADOW(textureName, samplerName, coord4, index)     textureName.SampleCmpLevelZero(samplerName, float4((coord4).xyz, index), (coord4).w)

	#define LOAD_TEXTURE2D(textureName, unCoord2)                                   textureName.Load(int3(unCoord2, 0))
	#define LOAD_TEXTURE2D_LOD(textureName, unCoord2, lod)                          textureName.Load(int3(unCoord2, lod))
	#define LOAD_TEXTURE2D_MSAA(textureName, unCoord2, sampleIndex)                 textureName.Load(unCoord2, sampleIndex)
	#define LOAD_TEXTURE2D_ARRAY(textureName, unCoord2, index)                      textureName.Load(int4(unCoord2, index, 0))
	#define LOAD_TEXTURE2D_ARRAY_MSAA(textureName, unCoord2, index, sampleIndex)    textureName.Load(int3(unCoord2, index), sampleIndex)
	#define LOAD_TEXTURE2D_ARRAY_LOD(textureName, unCoord2, index, lod)             textureName.Load(int4(unCoord2, index, lod))
	#define LOAD_TEXTURE3D(textureName, unCoord3)                                   textureName.Load(int4(unCoord3, 0))
	#define LOAD_TEXTURE3D_LOD(textureName, unCoord3, lod)                          textureName.Load(int4(unCoord3, lod))

	#define PLATFORM_SUPPORT_GATHER
	#define GATHER_TEXTURE2D(textureName, samplerName, coord2)                textureName.Gather(samplerName, coord2)
	#define GATHER_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)   textureName.Gather(samplerName, float3(coord2, index))
	#define GATHER_TEXTURECUBE(textureName, samplerName, coord3)              textureName.Gather(samplerName, coord3)
	#define GATHER_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index) textureName.Gather(samplerName, float4(coord3, index))
	#define GATHER_RED_TEXTURE2D(textureName, samplerName, coord2)            textureName.GatherRed(samplerName, coord2)
	#define GATHER_GREEN_TEXTURE2D(textureName, samplerName, coord2)          textureName.GatherGreen(samplerName, coord2)
	#define GATHER_BLUE_TEXTURE2D(textureName, samplerName, coord2)           textureName.GatherBlue(samplerName, coord2)
	#define GATHER_ALPHA_TEXTURE2D(textureName, samplerName, coord2)          textureName.GatherAlpha(samplerName, coord2)
#elif defined(SHADER_API_PSSL)
	#define ZERO_INITIALIZE(type, name) name = (type)0;
	#define ZERO_INITIALIZE_ARRAY(type, name, arraySize) { for (int arrayIndex = 0; arrayIndex < arraySize; arrayIndex++) { name[arrayIndex] = (type)0; } }
	#define CALCULATE_TEXTURE2D_LOD(textureName, samplerName, coord2) textureName.GetLOD(samplerName, coord2)
	#define TEXTURE2D(textureName)                Texture2D textureName
	#define TEXTURE2D_ARRAY(textureName)          Texture2DArray textureName
	#define TEXTURECUBE(textureName)              TextureCube textureName
	#define TEXTURECUBE_ARRAY(textureName)        TextureCubeArray textureName
	#define TEXTURE3D(textureName)                Texture3D textureName

	#define TEXTURE2D_FLOAT(textureName)          TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_FLOAT(textureName)    TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_FLOAT(textureName)        TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_FLOAT(textureName)  TEXTURECUBE_ARRAY(textureName)
	#define TEXTURE3D_FLOAT(textureName)          TEXTURE3D(textureName)

	#define TEXTURE2D_HALF(textureName)           TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_HALF(textureName)     TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_HALF(textureName)         TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_HALF(textureName)   TEXTURECUBE_ARRAY(textureName)
	#define TEXTURE3D_HALF(textureName)           TEXTURE3D(textureName)

	#define TEXTURE2D_SHADOW(textureName)         TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_SHADOW(textureName)   TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_SHADOW(textureName)       TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_SHADOW(textureName) TEXTURECUBE_ARRAY(textureName)

	#define RW_TEXTURE2D(type, textureName)       RW_Texture2D<type> textureName
	#define RW_TEXTURE2D_ARRAY(type, textureName) RW_Texture2D_Array<type> textureName
	#define RW_TEXTURE3D(type, textureName)       RW_Texture3D<type> textureName

	#define SAMPLER(samplerName)                  SamplerState samplerName
	#define SAMPLER_CMP(samplerName)              SamplerComparisonState samplerName

	#define TEXTURE2D_PARAM(textureName, samplerName)                 TEXTURE2D(textureName),         SAMPLER(samplerName)
	#define TEXTURE2D_ARRAY_PARAM(textureName, samplerName)           TEXTURE2D_ARRAY(textureName),   SAMPLER(samplerName)
	#define TEXTURECUBE_PARAM(textureName, samplerName)               TEXTURECUBE(textureName),       SAMPLER(samplerName)
	#define TEXTURECUBE_ARRAY_PARAM(textureName, samplerName)         TEXTURECUBE_ARRAY(textureName), SAMPLER(samplerName)
	#define TEXTURE3D_PARAM(textureName, samplerName)                 TEXTURE3D(textureName),         SAMPLER(samplerName)

	#define TEXTURE2D_SHADOW_PARAM(textureName, samplerName)          TEXTURE2D(textureName),         SAMPLER_CMP(samplerName)
	#define TEXTURE2D_ARRAY_SHADOW_PARAM(textureName, samplerName)    TEXTURE2D_ARRAY(textureName),   SAMPLER_CMP(samplerName)
	#define TEXTURECUBE_SHADOW_PARAM(textureName, samplerName)        TEXTURECUBE(textureName),       SAMPLER_CMP(samplerName)
	#define TEXTURECUBE_ARRAY_SHADOW_PARAM(textureName, samplerName)  TEXTURECUBE_ARRAY(textureName), SAMPLER_CMP(samplerName)

	#define TEXTURE2D_ARGS(textureName, samplerName)                textureName, samplerName
	#define TEXTURE2D_ARRAY_ARGS(textureName, samplerName)          textureName, samplerName
	#define TEXTURECUBE_ARGS(textureName, samplerName)              textureName, samplerName
	#define TEXTURECUBE_ARRAY_ARGS(textureName, samplerName)        textureName, samplerName
	#define TEXTURE3D_ARGS(textureName, samplerName)                textureName, samplerName

	#define TEXTURE2D_SHADOW_ARGS(textureName, samplerName)         textureName, samplerName
	#define TEXTURE2D_ARRAY_SHADOW_ARGS(textureName, samplerName)   textureName, samplerName
	#define TEXTURECUBE_SHADOW_ARGS(textureName, samplerName)       textureName, samplerName
	#define TEXTURECUBE_ARRAY_SHADOW_ARGS(textureName, samplerName) textureName, samplerName

	#define SAMPLE_TEXTURE2D(textureName, samplerName, coord2)                               textureName.Sample(samplerName, coord2)
	#define SAMPLE_TEXTURE2D_LOD(textureName, samplerName, coord2, lod)                      textureName.SampleLevel(samplerName, coord2, lod)
	#define SAMPLE_TEXTURE2D_BIAS(textureName, samplerName, coord2, bias)                    textureName.SampleBias(samplerName, coord2, bias)
	#define SAMPLE_TEXTURE2D_GRAD(textureName, samplerName, coord2, dpdx, dpdy)              textureName.SampleGrad(samplerName, coord2, dpdx, dpdy)
	#define SAMPLE_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)                  textureName.Sample(samplerName, float3(coord2, index))
	#define SAMPLE_TEXTURE2D_ARRAY_LOD(textureName, samplerName, coord2, index, lod)         textureName.SampleLevel(samplerName, float3(coord2, index), lod)
	#define SAMPLE_TEXTURE2D_ARRAY_BIAS(textureName, samplerName, coord2, index, bias)       textureName.SampleBias(samplerName, float3(coord2, index), bias)
	#define SAMPLE_TEXTURE2D_ARRAY_GRAD(textureName, samplerName, coord2, index, dpdx, dpdy) textureName.SampleGrad(samplerName, float3(coord2, index), dpdx, dpdy)
	#define SAMPLE_TEXTURECUBE(textureName, samplerName, coord3)                             textureName.Sample(samplerName, coord3)
	#define SAMPLE_TEXTURECUBE_LOD(textureName, samplerName, coord3, lod)                    textureName.SampleLevel(samplerName, coord3, lod)
	#define SAMPLE_TEXTURECUBE_BIAS(textureName, samplerName, coord3, bias)                  textureName.SampleBias(samplerName, coord3, bias)
	#define SAMPLE_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)                textureName.Sample(samplerName, float4(coord3, index))
	#define SAMPLE_TEXTURECUBE_ARRAY_LOD(textureName, samplerName, coord3, index, lod)       textureName.SampleLevel(samplerName, float4(coord3, index), lod)
	#define SAMPLE_TEXTURECUBE_ARRAY_BIAS(textureName, samplerName, coord3, index, bias)     textureName.SampleBias(samplerName, float4(coord3, index), bias)
	#define SAMPLE_TEXTURE3D(textureName, samplerName, coord3)                               textureName.Sample(samplerName, coord3)
	#define SAMPLE_TEXTURE3D_LOD(textureName, samplerName, coord3, lod)                      textureName.SampleLevel(samplerName, coord3, lod)

	#define SAMPLE_TEXTURE2D_SHADOW(textureName, samplerName, coord3)                    textureName.SampleCmpLevelZero(samplerName, (coord3).xy, (coord3).z)
	#define SAMPLE_TEXTURE2D_ARRAY_SHADOW(textureName, samplerName, coord3, index)       textureName.SampleCmpLevelZero(samplerName, float3((coord3).xy, index), (coord3).z)
	#define SAMPLE_TEXTURECUBE_SHADOW(textureName, samplerName, coord4)                  textureName.SampleCmpLevelZero(samplerName, (coord4).xyz, (coord4).w)
	#define SAMPLE_TEXTURECUBE_ARRAY_SHADOW(textureName, samplerName, coord4, index)     textureName.SampleCmpLevelZero(samplerName, float4((coord4).xyz, index), (coord4).w)

	#define LOAD_TEXTURE2D(textureName, unCoord2)                                   textureName.Load(int3(unCoord2, 0))
	#define LOAD_TEXTURE2D_LOD(textureName, unCoord2, lod)                          textureName.Load(int3(unCoord2, lod))
	#define LOAD_TEXTURE2D_MSAA(textureName, unCoord2, sampleIndex)                 textureName.Load(unCoord2, sampleIndex)
	#define LOAD_TEXTURE2D_ARRAY(textureName, unCoord2, index)                      textureName.Load(int4(unCoord2, index, 0))
	#define LOAD_TEXTURE2D_ARRAY_MSAA(textureName, unCoord2, index, sampleIndex)    textureName.Load(int3(unCoord2, index), sampleIndex)
	#define LOAD_TEXTURE2D_ARRAY_LOD(textureName, unCoord2, index, lod)             textureName.Load(int4(unCoord2, index, lod))
	#define LOAD_TEXTURE3D(textureName, unCoord3)                                   textureName.Load(int4(unCoord3, 0))
	#define LOAD_TEXTURE3D_LOD(textureName, unCoord3, lod)                          textureName.Load(int4(unCoord3, lod))

	#define PLATFORM_SUPPORT_GATHER
	#define GATHER_TEXTURE2D(textureName, samplerName, coord2)                textureName.Gather(samplerName, coord2)
	#define GATHER_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)   textureName.Gather(samplerName, float3(coord2, index))
	#define GATHER_TEXTURECUBE(textureName, samplerName, coord3)              textureName.Gather(samplerName, coord3)
	#define GATHER_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index) textureName.Gather(samplerName, float4(coord3, index))
	#define GATHER_RED_TEXTURE2D(textureName, samplerName, coord2)            textureName.GatherRed(samplerName, coord2)
	#define GATHER_GREEN_TEXTURE2D(textureName, samplerName, coord2)          textureName.GatherGreen(samplerName, coord2)
	#define GATHER_BLUE_TEXTURE2D(textureName, samplerName, coord2)           textureName.GatherBlue(samplerName, coord2)
	#define GATHER_ALPHA_TEXTURE2D(textureName, samplerName, coord2)          textureName.GatherAlpha(samplerName, coord2)
#elif defined(SHADER_API_D3D11)
	#define ZERO_INITIALIZE(type, name) name = (type)0;
	#define ZERO_INITIALIZE_ARRAY(type, name, arraySize) { for (int arrayIndex = 0; arrayIndex < arraySize; arrayIndex++) { name[arrayIndex] = (type)0; } }
	#define CALCULATE_TEXTURE2D_LOD(textureName, samplerName, coord2) textureName.CalculateLevelOfDetail(samplerName, coord2)
	#define TEXTURE2D(textureName)                Texture2D textureName
	#define TEXTURE2D_ARRAY(textureName)          Texture2DArray textureName
	#define TEXTURECUBE(textureName)              TextureCube textureName
	#define TEXTURECUBE_ARRAY(textureName)        TextureCubeArray textureName
	#define TEXTURE3D(textureName)                Texture3D textureName

	#define TEXTURE2D_FLOAT(textureName)          TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_FLOAT(textureName)    TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_FLOAT(textureName)        TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_FLOAT(textureName)  TEXTURECUBE_ARRAY(textureName)
	#define TEXTURE3D_FLOAT(textureName)          TEXTURE3D(textureName)

	#define TEXTURE2D_HALF(textureName)           TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_HALF(textureName)     TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_HALF(textureName)         TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_HALF(textureName)   TEXTURECUBE_ARRAY(textureName)
	#define TEXTURE3D_HALF(textureName)           TEXTURE3D(textureName)

	#define TEXTURE2D_SHADOW(textureName)         TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_SHADOW(textureName)   TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_SHADOW(textureName)       TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_SHADOW(textureName) TEXTURECUBE_ARRAY(textureName)

	#define RW_TEXTURE2D(type, textureName)       RWTexture2D<type> textureName
	#define RW_TEXTURE2D_ARRAY(type, textureName) RWTexture2DArray<type> textureName
	#define RW_TEXTURE3D(type, textureName)       RWTexture3D<type> textureName

	#define SAMPLER(samplerName)                  SamplerState samplerName
	#define SAMPLER_CMP(samplerName)              SamplerComparisonState samplerName

	#define TEXTURE2D_PARAM(textureName, samplerName)                 TEXTURE2D(textureName),         SAMPLER(samplerName)
	#define TEXTURE2D_ARRAY_PARAM(textureName, samplerName)           TEXTURE2D_ARRAY(textureName),   SAMPLER(samplerName)
	#define TEXTURECUBE_PARAM(textureName, samplerName)               TEXTURECUBE(textureName),       SAMPLER(samplerName)
	#define TEXTURECUBE_ARRAY_PARAM(textureName, samplerName)         TEXTURECUBE_ARRAY(textureName), SAMPLER(samplerName)
	#define TEXTURE3D_PARAM(textureName, samplerName)                 TEXTURE3D(textureName),         SAMPLER(samplerName)

	#define TEXTURE2D_SHADOW_PARAM(textureName, samplerName)          TEXTURE2D(textureName),         SAMPLER_CMP(samplerName)
	#define TEXTURE2D_ARRAY_SHADOW_PARAM(textureName, samplerName)    TEXTURE2D_ARRAY(textureName),   SAMPLER_CMP(samplerName)
	#define TEXTURECUBE_SHADOW_PARAM(textureName, samplerName)        TEXTURECUBE(textureName),       SAMPLER_CMP(samplerName)
	#define TEXTURECUBE_ARRAY_SHADOW_PARAM(textureName, samplerName)  TEXTURECUBE_ARRAY(textureName), SAMPLER_CMP(samplerName)

	#define TEXTURE2D_ARGS(textureName, samplerName)                textureName, samplerName
	#define TEXTURE2D_ARRAY_ARGS(textureName, samplerName)          textureName, samplerName
	#define TEXTURECUBE_ARGS(textureName, samplerName)              textureName, samplerName
	#define TEXTURECUBE_ARRAY_ARGS(textureName, samplerName)        textureName, samplerName
	#define TEXTURE3D_ARGS(textureName, samplerName)                textureName, samplerName

	#define TEXTURE2D_SHADOW_ARGS(textureName, samplerName)         textureName, samplerName
	#define TEXTURE2D_ARRAY_SHADOW_ARGS(textureName, samplerName)   textureName, samplerName
	#define TEXTURECUBE_SHADOW_ARGS(textureName, samplerName)       textureName, samplerName
	#define TEXTURECUBE_ARRAY_SHADOW_ARGS(textureName, samplerName) textureName, samplerName

	#define SAMPLE_TEXTURE2D(textureName, samplerName, coord2)                               textureName.Sample(samplerName, coord2)
	#define SAMPLE_TEXTURE2D_LOD(textureName, samplerName, coord2, lod)                      textureName.SampleLevel(samplerName, coord2, lod)
	#define SAMPLE_TEXTURE2D_BIAS(textureName, samplerName, coord2, bias)                    textureName.SampleBias(samplerName, coord2, bias)
	#define SAMPLE_TEXTURE2D_GRAD(textureName, samplerName, coord2, dpdx, dpdy)              textureName.SampleGrad(samplerName, coord2, dpdx, dpdy)
	#define SAMPLE_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)                  textureName.Sample(samplerName, float3(coord2, index))
	#define SAMPLE_TEXTURE2D_ARRAY_LOD(textureName, samplerName, coord2, index, lod)         textureName.SampleLevel(samplerName, float3(coord2, index), lod)
	#define SAMPLE_TEXTURE2D_ARRAY_BIAS(textureName, samplerName, coord2, index, bias)       textureName.SampleBias(samplerName, float3(coord2, index), bias)
	#define SAMPLE_TEXTURE2D_ARRAY_GRAD(textureName, samplerName, coord2, index, dpdx, dpdy) textureName.SampleGrad(samplerName, float3(coord2, index), dpdx, dpdy)
	#define SAMPLE_TEXTURECUBE(textureName, samplerName, coord3)                             textureName.Sample(samplerName, coord3)
	#define SAMPLE_TEXTURECUBE_LOD(textureName, samplerName, coord3, lod)                    textureName.SampleLevel(samplerName, coord3, lod)
	#define SAMPLE_TEXTURECUBE_BIAS(textureName, samplerName, coord3, bias)                  textureName.SampleBias(samplerName, coord3, bias)
	#define SAMPLE_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)                textureName.Sample(samplerName, float4(coord3, index))
	#define SAMPLE_TEXTURECUBE_ARRAY_LOD(textureName, samplerName, coord3, index, lod)       textureName.SampleLevel(samplerName, float4(coord3, index), lod)
	#define SAMPLE_TEXTURECUBE_ARRAY_BIAS(textureName, samplerName, coord3, index, bias)     textureName.SampleBias(samplerName, float4(coord3, index), bias)
	#define SAMPLE_TEXTURE3D(textureName, samplerName, coord3)                               textureName.Sample(samplerName, coord3)
	#define SAMPLE_TEXTURE3D_LOD(textureName, samplerName, coord3, lod)                      textureName.SampleLevel(samplerName, coord3, lod)

	#define SAMPLE_TEXTURE2D_SHADOW(textureName, samplerName, coord3)                    textureName.SampleCmpLevelZero(samplerName, (coord3).xy, (coord3).z)
	#define SAMPLE_TEXTURE2D_ARRAY_SHADOW(textureName, samplerName, coord3, index)       textureName.SampleCmpLevelZero(samplerName, float3((coord3).xy, index), (coord3).z)
	#define SAMPLE_TEXTURECUBE_SHADOW(textureName, samplerName, coord4)                  textureName.SampleCmpLevelZero(samplerName, (coord4).xyz, (coord4).w)
	#define SAMPLE_TEXTURECUBE_ARRAY_SHADOW(textureName, samplerName, coord4, index)     textureName.SampleCmpLevelZero(samplerName, float4((coord4).xyz, index), (coord4).w)

	#define LOAD_TEXTURE2D(textureName, unCoord2)                                   textureName.Load(int3(unCoord2, 0))
	#define LOAD_TEXTURE2D_LOD(textureName, unCoord2, lod)                          textureName.Load(int3(unCoord2, lod))
	#define LOAD_TEXTURE2D_MSAA(textureName, unCoord2, sampleIndex)                 textureName.Load(unCoord2, sampleIndex)
	#define LOAD_TEXTURE2D_ARRAY(textureName, unCoord2, index)                      textureName.Load(int4(unCoord2, index, 0))
	#define LOAD_TEXTURE2D_ARRAY_MSAA(textureName, unCoord2, index, sampleIndex)    textureName.Load(int3(unCoord2, index), sampleIndex)
	#define LOAD_TEXTURE2D_ARRAY_LOD(textureName, unCoord2, index, lod)             textureName.Load(int4(unCoord2, index, lod))
	#define LOAD_TEXTURE3D(textureName, unCoord3)                                   textureName.Load(int4(unCoord3, 0))
	#define LOAD_TEXTURE3D_LOD(textureName, unCoord3, lod)                          textureName.Load(int4(unCoord3, lod))

	#define PLATFORM_SUPPORT_GATHER
	#define GATHER_TEXTURE2D(textureName, samplerName, coord2)                textureName.Gather(samplerName, coord2)
	#define GATHER_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)   textureName.Gather(samplerName, float3(coord2, index))
	#define GATHER_TEXTURECUBE(textureName, samplerName, coord3)              textureName.Gather(samplerName, coord3)
	#define GATHER_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index) textureName.Gather(samplerName, float4(coord3, index))
	#define GATHER_RED_TEXTURE2D(textureName, samplerName, coord2)            textureName.GatherRed(samplerName, coord2)
	#define GATHER_GREEN_TEXTURE2D(textureName, samplerName, coord2)          textureName.GatherGreen(samplerName, coord2)
	#define GATHER_BLUE_TEXTURE2D(textureName, samplerName, coord2)           textureName.GatherBlue(samplerName, coord2)
	#define GATHER_ALPHA_TEXTURE2D(textureName, samplerName, coord2)          textureName.GatherAlpha(samplerName, coord2)

#elif defined(SHADER_API_METAL)
	#define ZERO_INITIALIZE(type, name) name = (type)0;
	#define ZERO_INITIALIZE_ARRAY(type, name, arraySize) { for (int arrayIndex = 0; arrayIndex < arraySize; arrayIndex++) { name[arrayIndex] = (type)0; } }
	#define CALCULATE_TEXTURE2D_LOD(textureName, samplerName, coord2) textureName.CalculateLevelOfDetail(samplerName, coord2)
	#define TEXTURE2D(textureName)                Texture2D textureName
	#define TEXTURE2D_ARRAY(textureName)          Texture2DArray textureName
	#define TEXTURECUBE(textureName)              TextureCube textureName
	#define TEXTURECUBE_ARRAY(textureName)        TextureCubeArray textureName
	#define TEXTURE3D(textureName)                Texture3D textureName

	#define TEXTURE2D_FLOAT(textureName)          Texture2D_float textureName
	#define TEXTURE2D_ARRAY_FLOAT(textureName)    Texture2DArray textureName    
	#define TEXTURECUBE_FLOAT(textureName)        TextureCube_float textureName
	#define TEXTURECUBE_ARRAY_FLOAT(textureName)  TextureCubeArray textureName  
	#define TEXTURE3D_FLOAT(textureName)          Texture3D_float textureName

	#define TEXTURE2D_HALF(textureName)           Texture2D_half textureName
	#define TEXTURE2D_ARRAY_HALF(textureName)     Texture2DArray textureName    
	#define TEXTURECUBE_HALF(textureName)         TextureCube_half textureName
	#define TEXTURECUBE_ARRAY_HALF(textureName)   TextureCubeArray textureName  
	#define TEXTURE3D_HALF(textureName)           Texture3D_half textureName

	#define TEXTURE2D_SHADOW(textureName)         TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_SHADOW(textureName)   TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_SHADOW(textureName)       TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_SHADOW(textureName) TEXTURECUBE_ARRAY(textureName)

	#define RW_TEXTURE2D(type, textureName)       RWTexture2D<type> textureName
	#define RW_TEXTURE2D_ARRAY(type, textureName) RWTexture2DArray<type> textureName
	#define RW_TEXTURE3D(type, textureName)       RWTexture3D<type> textureName

	#define SAMPLER(samplerName)                  SamplerState samplerName
	#define SAMPLER_CMP(samplerName)              SamplerComparisonState samplerName

	#define TEXTURE2D_PARAM(textureName, samplerName)                 TEXTURE2D(textureName),         SAMPLER(samplerName)
	#define TEXTURE2D_ARRAY_PARAM(textureName, samplerName)           TEXTURE2D_ARRAY(textureName),   SAMPLER(samplerName)
	#define TEXTURECUBE_PARAM(textureName, samplerName)               TEXTURECUBE(textureName),       SAMPLER(samplerName)
	#define TEXTURECUBE_ARRAY_PARAM(textureName, samplerName)         TEXTURECUBE_ARRAY(textureName), SAMPLER(samplerName)
	#define TEXTURE3D_PARAM(textureName, samplerName)                 TEXTURE3D(textureName),         SAMPLER(samplerName)

	#define TEXTURE2D_SHADOW_PARAM(textureName, samplerName)          TEXTURE2D(textureName),         SAMPLER_CMP(samplerName)
	#define TEXTURE2D_ARRAY_SHADOW_PARAM(textureName, samplerName)    TEXTURE2D_ARRAY(textureName),   SAMPLER_CMP(samplerName)
	#define TEXTURECUBE_SHADOW_PARAM(textureName, samplerName)        TEXTURECUBE(textureName),       SAMPLER_CMP(samplerName)
	#define TEXTURECUBE_ARRAY_SHADOW_PARAM(textureName, samplerName)  TEXTURECUBE_ARRAY(textureName), SAMPLER_CMP(samplerName)

	#define TEXTURE2D_ARGS(textureName, samplerName)                textureName, samplerName
	#define TEXTURE2D_ARRAY_ARGS(textureName, samplerName)          textureName, samplerName
	#define TEXTURECUBE_ARGS(textureName, samplerName)              textureName, samplerName
	#define TEXTURECUBE_ARRAY_ARGS(textureName, samplerName)        textureName, samplerName
	#define TEXTURE3D_ARGS(textureName, samplerName)                textureName, samplerName

	#define TEXTURE2D_SHADOW_ARGS(textureName, samplerName)         textureName, samplerName
	#define TEXTURE2D_ARRAY_SHADOW_ARGS(textureName, samplerName)   textureName, samplerName
	#define TEXTURECUBE_SHADOW_ARGS(textureName, samplerName)       textureName, samplerName
	#define TEXTURECUBE_ARRAY_SHADOW_ARGS(textureName, samplerName) textureName, samplerName

	#define SAMPLE_TEXTURE2D(textureName, samplerName, coord2)                               textureName.Sample(samplerName, coord2)
	#define SAMPLE_TEXTURE2D_LOD(textureName, samplerName, coord2, lod)                      textureName.SampleLevel(samplerName, coord2, lod)
	#define SAMPLE_TEXTURE2D_BIAS(textureName, samplerName, coord2, bias)                    textureName.SampleBias(samplerName, coord2, bias)
	#define SAMPLE_TEXTURE2D_GRAD(textureName, samplerName, coord2, dpdx, dpdy)              textureName.SampleGrad(samplerName, coord2, dpdx, dpdy)
	#define SAMPLE_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)                  textureName.Sample(samplerName, float3(coord2, index))
	#define SAMPLE_TEXTURE2D_ARRAY_LOD(textureName, samplerName, coord2, index, lod)         textureName.SampleLevel(samplerName, float3(coord2, index), lod)
	#define SAMPLE_TEXTURE2D_ARRAY_BIAS(textureName, samplerName, coord2, index, bias)       textureName.SampleBias(samplerName, float3(coord2, index), bias)
	#define SAMPLE_TEXTURE2D_ARRAY_GRAD(textureName, samplerName, coord2, index, dpdx, dpdy) textureName.SampleGrad(samplerName, float3(coord2, index), dpdx, dpdy)
	#define SAMPLE_TEXTURECUBE(textureName, samplerName, coord3)                             textureName.Sample(samplerName, coord3)
	#define SAMPLE_TEXTURECUBE_LOD(textureName, samplerName, coord3, lod)                    textureName.SampleLevel(samplerName, coord3, lod)
	#define SAMPLE_TEXTURECUBE_BIAS(textureName, samplerName, coord3, bias)                  textureName.SampleBias(samplerName, coord3, bias)
	#define SAMPLE_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)                textureName.Sample(samplerName, float4(coord3, index))
	#define SAMPLE_TEXTURECUBE_ARRAY_LOD(textureName, samplerName, coord3, index, lod)       textureName.SampleLevel(samplerName, float4(coord3, index), lod)
	#define SAMPLE_TEXTURECUBE_ARRAY_BIAS(textureName, samplerName, coord3, index, bias)     textureName.SampleBias(samplerName, float4(coord3, index), bias)
	#define SAMPLE_TEXTURE3D(textureName, samplerName, coord3)                               textureName.Sample(samplerName, coord3)
	#define SAMPLE_TEXTURE3D_LOD(textureName, samplerName, coord3, lod)                      textureName.SampleLevel(samplerName, coord3, lod)

	#define SAMPLE_TEXTURE2D_SHADOW(textureName, samplerName, coord3)                    textureName.SampleCmpLevelZero(samplerName, (coord3).xy, (coord3).z)
	#define SAMPLE_TEXTURE2D_ARRAY_SHADOW(textureName, samplerName, coord3, index)       textureName.SampleCmpLevelZero(samplerName, float3((coord3).xy, index), (coord3).z)
	#define SAMPLE_TEXTURECUBE_SHADOW(textureName, samplerName, coord4)                  textureName.SampleCmpLevelZero(samplerName, (coord4).xyz, (coord4).w)
	#define SAMPLE_TEXTURECUBE_ARRAY_SHADOW(textureName, samplerName, coord4, index)     textureName.SampleCmpLevelZero(samplerName, float4((coord4).xyz, index), (coord4).w)

	#define LOAD_TEXTURE2D(textureName, unCoord2)                                   textureName.Load(int3(unCoord2, 0))
	#define LOAD_TEXTURE2D_LOD(textureName, unCoord2, lod)                          textureName.Load(int3(unCoord2, lod))
	#define LOAD_TEXTURE2D_MSAA(textureName, unCoord2, sampleIndex)                 textureName.Load(unCoord2, sampleIndex)
	#define LOAD_TEXTURE2D_ARRAY(textureName, unCoord2, index)                      textureName.Load(int4(unCoord2, index, 0))
	#define LOAD_TEXTURE2D_ARRAY_MSAA(textureName, unCoord2, index, sampleIndex)    textureName.Load(int3(unCoord2, index), sampleIndex)
	#define LOAD_TEXTURE2D_ARRAY_LOD(textureName, unCoord2, index, lod)             textureName.Load(int4(unCoord2, index, lod))
	#define LOAD_TEXTURE3D(textureName, unCoord3)                                   textureName.Load(int4(unCoord3, 0))
	#define LOAD_TEXTURE3D_LOD(textureName, unCoord3, lod)                          textureName.Load(int4(unCoord3, lod))

	#define PLATFORM_SUPPORT_GATHER
	#define GATHER_TEXTURE2D(textureName, samplerName, coord2)                textureName.Gather(samplerName, coord2)
	#define GATHER_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)   textureName.Gather(samplerName, float3(coord2, index))
	#define GATHER_TEXTURECUBE(textureName, samplerName, coord3)              textureName.Gather(samplerName, coord3)
	#define GATHER_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index) textureName.Gather(samplerName, float4(coord3, index))
	#define GATHER_RED_TEXTURE2D(textureName, samplerName, coord2)            textureName.GatherRed(samplerName, coord2)
	#define GATHER_GREEN_TEXTURE2D(textureName, samplerName, coord2)          textureName.GatherGreen(samplerName, coord2)
	#define GATHER_BLUE_TEXTURE2D(textureName, samplerName, coord2)           textureName.GatherBlue(samplerName, coord2)
	#define GATHER_ALPHA_TEXTURE2D(textureName, samplerName, coord2)          textureName.GatherAlpha(samplerName, coord2)

#elif defined(SHADER_API_VULKAN)
	#define ZERO_INITIALIZE(type, name) name = (type)0;
	#define ZERO_INITIALIZE_ARRAY(type, name, arraySize) { for (int arrayIndex = 0; arrayIndex < arraySize; arrayIndex++) { name[arrayIndex] = (type)0; } }
	#define CALCULATE_TEXTURE2D_LOD(textureName, samplerName, coord2) textureName.CalculateLevelOfDetail(samplerName, coord2)
	#define TEXTURE2D(textureName)                Texture2D textureName
	#define TEXTURE2D_ARRAY(textureName)          Texture2DArray textureName
	#define TEXTURECUBE(textureName)              TextureCube textureName
	#define TEXTURECUBE_ARRAY(textureName)        TextureCubeArray textureName
	#define TEXTURE3D(textureName)                Texture3D textureName

	#define TEXTURE2D_FLOAT(textureName)          Texture2D_float textureName
	#define TEXTURE2D_ARRAY_FLOAT(textureName)    Texture2DArray textureName    
	#define TEXTURECUBE_FLOAT(textureName)        TextureCube_float textureName
	#define TEXTURECUBE_ARRAY_FLOAT(textureName)  TextureCubeArray textureName  
	#define TEXTURE3D_FLOAT(textureName)          Texture3D_float textureName

	#define TEXTURE2D_HALF(textureName)           Texture2D_half textureName
	#define TEXTURE2D_ARRAY_HALF(textureName)     Texture2DArray textureName    
	#define TEXTURECUBE_HALF(textureName)         TextureCube_half textureName
	#define TEXTURECUBE_ARRAY_HALF(textureName)   TextureCubeArray textureName  
	#define TEXTURE3D_HALF(textureName)           Texture3D_half textureName

	#define TEXTURE2D_SHADOW(textureName)         TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_SHADOW(textureName)   TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_SHADOW(textureName)       TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_SHADOW(textureName) TEXTURECUBE_ARRAY(textureName)

	#define RW_TEXTURE2D(type, textureName)       RWTexture2D<type> textureName
	#define RW_TEXTURE2D_ARRAY(type, textureName) RWTexture2DArray<type> textureName
	#define RW_TEXTURE3D(type, textureName)       RWTexture3D<type> textureName

	#define SAMPLER(samplerName)                  SamplerState samplerName
	#define SAMPLER_CMP(samplerName)              SamplerComparisonState samplerName

	#define TEXTURE2D_PARAM(textureName, samplerName)                 TEXTURE2D(textureName),         SAMPLER(samplerName)
	#define TEXTURE2D_ARRAY_PARAM(textureName, samplerName)           TEXTURE2D_ARRAY(textureName),   SAMPLER(samplerName)
	#define TEXTURECUBE_PARAM(textureName, samplerName)               TEXTURECUBE(textureName),       SAMPLER(samplerName)
	#define TEXTURECUBE_ARRAY_PARAM(textureName, samplerName)         TEXTURECUBE_ARRAY(textureName), SAMPLER(samplerName)
	#define TEXTURE3D_PARAM(textureName, samplerName)                 TEXTURE3D(textureName),         SAMPLER(samplerName)

	#define TEXTURE2D_SHADOW_PARAM(textureName, samplerName)          TEXTURE2D(textureName),         SAMPLER_CMP(samplerName)
	#define TEXTURE2D_ARRAY_SHADOW_PARAM(textureName, samplerName)    TEXTURE2D_ARRAY(textureName),   SAMPLER_CMP(samplerName)
	#define TEXTURECUBE_SHADOW_PARAM(textureName, samplerName)        TEXTURECUBE(textureName),       SAMPLER_CMP(samplerName)
	#define TEXTURECUBE_ARRAY_SHADOW_PARAM(textureName, samplerName)  TEXTURECUBE_ARRAY(textureName), SAMPLER_CMP(samplerName)

	#define TEXTURE2D_ARGS(textureName, samplerName)                textureName, samplerName
	#define TEXTURE2D_ARRAY_ARGS(textureName, samplerName)          textureName, samplerName
	#define TEXTURECUBE_ARGS(textureName, samplerName)              textureName, samplerName
	#define TEXTURECUBE_ARRAY_ARGS(textureName, samplerName)        textureName, samplerName
	#define TEXTURE3D_ARGS(textureName, samplerName)                textureName, samplerName

	#define TEXTURE2D_SHADOW_ARGS(textureName, samplerName)         textureName, samplerName
	#define TEXTURE2D_ARRAY_SHADOW_ARGS(textureName, samplerName)   textureName, samplerName
	#define TEXTURECUBE_SHADOW_ARGS(textureName, samplerName)       textureName, samplerName
	#define TEXTURECUBE_ARRAY_SHADOW_ARGS(textureName, samplerName) textureName, samplerName

	#define SAMPLE_TEXTURE2D(textureName, samplerName, coord2)                               textureName.Sample(samplerName, coord2)
	#define SAMPLE_TEXTURE2D_LOD(textureName, samplerName, coord2, lod)                      textureName.SampleLevel(samplerName, coord2, lod)
	#define SAMPLE_TEXTURE2D_BIAS(textureName, samplerName, coord2, bias)                    textureName.SampleBias(samplerName, coord2, bias)
	#define SAMPLE_TEXTURE2D_GRAD(textureName, samplerName, coord2, dpdx, dpdy)              textureName.SampleGrad(samplerName, coord2, dpdx, dpdy)
	#define SAMPLE_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)                  textureName.Sample(samplerName, float3(coord2, index))
	#define SAMPLE_TEXTURE2D_ARRAY_LOD(textureName, samplerName, coord2, index, lod)         textureName.SampleLevel(samplerName, float3(coord2, index), lod)
	#define SAMPLE_TEXTURE2D_ARRAY_BIAS(textureName, samplerName, coord2, index, bias)       textureName.SampleBias(samplerName, float3(coord2, index), bias)
	#define SAMPLE_TEXTURE2D_ARRAY_GRAD(textureName, samplerName, coord2, index, dpdx, dpdy) textureName.SampleGrad(samplerName, float3(coord2, index), dpdx, dpdy)
	#define SAMPLE_TEXTURECUBE(textureName, samplerName, coord3)                             textureName.Sample(samplerName, coord3)
	#define SAMPLE_TEXTURECUBE_LOD(textureName, samplerName, coord3, lod)                    textureName.SampleLevel(samplerName, coord3, lod)
	#define SAMPLE_TEXTURECUBE_BIAS(textureName, samplerName, coord3, bias)                  textureName.SampleBias(samplerName, coord3, bias)
	#define SAMPLE_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)                textureName.Sample(samplerName, float4(coord3, index))
	#define SAMPLE_TEXTURECUBE_ARRAY_LOD(textureName, samplerName, coord3, index, lod)       textureName.SampleLevel(samplerName, float4(coord3, index), lod)
	#define SAMPLE_TEXTURECUBE_ARRAY_BIAS(textureName, samplerName, coord3, index, bias)     textureName.SampleBias(samplerName, float4(coord3, index), bias)
	#define SAMPLE_TEXTURE3D(textureName, samplerName, coord3)                               textureName.Sample(samplerName, coord3)
	#define SAMPLE_TEXTURE3D_LOD(textureName, samplerName, coord3, lod)                      textureName.SampleLevel(samplerName, coord3, lod)

	#define SAMPLE_TEXTURE2D_SHADOW(textureName, samplerName, coord3)                    textureName.SampleCmpLevelZero(samplerName, (coord3).xy, (coord3).z)
	#define SAMPLE_TEXTURE2D_ARRAY_SHADOW(textureName, samplerName, coord3, index)       textureName.SampleCmpLevelZero(samplerName, float3((coord3).xy, index), (coord3).z)
	#define SAMPLE_TEXTURECUBE_SHADOW(textureName, samplerName, coord4)                  textureName.SampleCmpLevelZero(samplerName, (coord4).xyz, (coord4).w)
	#define SAMPLE_TEXTURECUBE_ARRAY_SHADOW(textureName, samplerName, coord4, index)     textureName.SampleCmpLevelZero(samplerName, float4((coord4).xyz, index), (coord4).w)

	#define LOAD_TEXTURE2D(textureName, unCoord2)                                   textureName.Load(int3(unCoord2, 0))
	#define LOAD_TEXTURE2D_LOD(textureName, unCoord2, lod)                          textureName.Load(int3(unCoord2, lod))
	#define LOAD_TEXTURE2D_MSAA(textureName, unCoord2, sampleIndex)                 textureName.Load(unCoord2, sampleIndex)
	#define LOAD_TEXTURE2D_ARRAY(textureName, unCoord2, index)                      textureName.Load(int4(unCoord2, index, 0))
	#define LOAD_TEXTURE2D_ARRAY_MSAA(textureName, unCoord2, index, sampleIndex)    textureName.Load(int3(unCoord2, index), sampleIndex)
	#define LOAD_TEXTURE2D_ARRAY_LOD(textureName, unCoord2, index, lod)             textureName.Load(int4(unCoord2, index, lod))
	#define LOAD_TEXTURE3D(textureName, unCoord3)                                   textureName.Load(int4(unCoord3, 0))
	#define LOAD_TEXTURE3D_LOD(textureName, unCoord3, lod)                          textureName.Load(int4(unCoord3, lod))

	#define PLATFORM_SUPPORT_GATHER
	#define GATHER_TEXTURE2D(textureName, samplerName, coord2)                textureName.Gather(samplerName, coord2)
	#define GATHER_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)   textureName.Gather(samplerName, float3(coord2, index))
	#define GATHER_TEXTURECUBE(textureName, samplerName, coord3)              textureName.Gather(samplerName, coord3)
	#define GATHER_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index) textureName.Gather(samplerName, float4(coord3, index))
	#define GATHER_RED_TEXTURE2D(textureName, samplerName, coord2)            textureName.GatherRed(samplerName, coord2)
	#define GATHER_GREEN_TEXTURE2D(textureName, samplerName, coord2)          textureName.GatherGreen(samplerName, coord2)
	#define GATHER_BLUE_TEXTURE2D(textureName, samplerName, coord2)           textureName.GatherBlue(samplerName, coord2)
	#define GATHER_ALPHA_TEXTURE2D(textureName, samplerName, coord2)          textureName.GatherAlpha(samplerName, coord2)

#elif defined(SHADER_API_SWITCH)
	#define ZERO_INITIALIZE(type, name) name = (type)0;
	#define ZERO_INITIALIZE_ARRAY(type, name, arraySize) { for (int arrayIndex = 0; arrayIndex < arraySize; arrayIndex++) { name[arrayIndex] = (type)0; } }
	#define CALCULATE_TEXTURE2D_LOD(textureName, samplerName, coord2) textureName.CalculateLevelOfDetail(samplerName, coord2)
	#define TEXTURE2D(textureName)                Texture2D textureName
	#define TEXTURE2D_ARRAY(textureName)          Texture2DArray textureName
	#define TEXTURECUBE(textureName)              TextureCube textureName
	#define TEXTURECUBE_ARRAY(textureName)        TextureCubeArray textureName
	#define TEXTURE3D(textureName)                Texture3D textureName

	#define TEXTURE2D_FLOAT(textureName)          Texture2D_float textureName
	#define TEXTURE2D_ARRAY_FLOAT(textureName)    Texture2DArray textureName    
	#define TEXTURECUBE_FLOAT(textureName)        TextureCube_float textureName
	#define TEXTURECUBE_ARRAY_FLOAT(textureName)  TextureCubeArray textureName  
	#define TEXTURE3D_FLOAT(textureName)          Texture3D_float textureName

	#define TEXTURE2D_HALF(textureName)           Texture2D_half textureName
	#define TEXTURE2D_ARRAY_HALF(textureName)     Texture2DArray textureName    
	#define TEXTURECUBE_HALF(textureName)         TextureCube_half textureName
	#define TEXTURECUBE_ARRAY_HALF(textureName)   TextureCubeArray textureName  
	#define TEXTURE3D_HALF(textureName)           Texture3D_half textureName

	#define TEXTURE2D_SHADOW(textureName)         TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_SHADOW(textureName)   TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_SHADOW(textureName)       TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_SHADOW(textureName) TEXTURECUBE_ARRAY(textureName)

	#define RW_TEXTURE2D(type, textureName)       RWTexture2D<type> textureName
	#define RW_TEXTURE2D_ARRAY(type, textureName) RWTexture2DArray<type> textureName
	#define RW_TEXTURE3D(type, textureName)       RWTexture3D<type> textureName

	#define SAMPLER(samplerName)                  SamplerState samplerName
	#define SAMPLER_CMP(samplerName)              SamplerComparisonState samplerName

	#define TEXTURE2D_PARAM(textureName, samplerName)                 TEXTURE2D(textureName),         SAMPLER(samplerName)
	#define TEXTURE2D_ARRAY_PARAM(textureName, samplerName)           TEXTURE2D_ARRAY(textureName),   SAMPLER(samplerName)
	#define TEXTURECUBE_PARAM(textureName, samplerName)               TEXTURECUBE(textureName),       SAMPLER(samplerName)
	#define TEXTURECUBE_ARRAY_PARAM(textureName, samplerName)         TEXTURECUBE_ARRAY(textureName), SAMPLER(samplerName)
	#define TEXTURE3D_PARAM(textureName, samplerName)                 TEXTURE3D(textureName),         SAMPLER(samplerName)

	#define TEXTURE2D_SHADOW_PARAM(textureName, samplerName)          TEXTURE2D(textureName),         SAMPLER_CMP(samplerName)
	#define TEXTURE2D_ARRAY_SHADOW_PARAM(textureName, samplerName)    TEXTURE2D_ARRAY(textureName),   SAMPLER_CMP(samplerName)
	#define TEXTURECUBE_SHADOW_PARAM(textureName, samplerName)        TEXTURECUBE(textureName),       SAMPLER_CMP(samplerName)
	#define TEXTURECUBE_ARRAY_SHADOW_PARAM(textureName, samplerName)  TEXTURECUBE_ARRAY(textureName), SAMPLER_CMP(samplerName)

	#define TEXTURE2D_ARGS(textureName, samplerName)                textureName, samplerName
	#define TEXTURE2D_ARRAY_ARGS(textureName, samplerName)          textureName, samplerName
	#define TEXTURECUBE_ARGS(textureName, samplerName)              textureName, samplerName
	#define TEXTURECUBE_ARRAY_ARGS(textureName, samplerName)        textureName, samplerName
	#define TEXTURE3D_ARGS(textureName, samplerName)                textureName, samplerName

	#define TEXTURE2D_SHADOW_ARGS(textureName, samplerName)         textureName, samplerName
	#define TEXTURE2D_ARRAY_SHADOW_ARGS(textureName, samplerName)   textureName, samplerName
	#define TEXTURECUBE_SHADOW_ARGS(textureName, samplerName)       textureName, samplerName
	#define TEXTURECUBE_ARRAY_SHADOW_ARGS(textureName, samplerName) textureName, samplerName

	#define SAMPLE_TEXTURE2D(textureName, samplerName, coord2)                               textureName.Sample(samplerName, coord2)
	#define SAMPLE_TEXTURE2D_LOD(textureName, samplerName, coord2, lod)                      textureName.SampleLevel(samplerName, coord2, lod)
	#define SAMPLE_TEXTURE2D_BIAS(textureName, samplerName, coord2, bias)                    textureName.SampleBias(samplerName, coord2, bias)
	#define SAMPLE_TEXTURE2D_GRAD(textureName, samplerName, coord2, dpdx, dpdy)              textureName.SampleGrad(samplerName, coord2, dpdx, dpdy)
	#define SAMPLE_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)                  textureName.Sample(samplerName, float3(coord2, index))
	#define SAMPLE_TEXTURE2D_ARRAY_LOD(textureName, samplerName, coord2, index, lod)         textureName.SampleLevel(samplerName, float3(coord2, index), lod)
	#define SAMPLE_TEXTURE2D_ARRAY_BIAS(textureName, samplerName, coord2, index, bias)       textureName.SampleBias(samplerName, float3(coord2, index), bias)
	#define SAMPLE_TEXTURE2D_ARRAY_GRAD(textureName, samplerName, coord2, index, dpdx, dpdy) textureName.SampleGrad(samplerName, float3(coord2, index), dpdx, dpdy)
	#define SAMPLE_TEXTURECUBE(textureName, samplerName, coord3)                             textureName.Sample(samplerName, coord3)
	#define SAMPLE_TEXTURECUBE_LOD(textureName, samplerName, coord3, lod)                    textureName.SampleLevel(samplerName, coord3, lod)
	#define SAMPLE_TEXTURECUBE_BIAS(textureName, samplerName, coord3, bias)                  textureName.SampleBias(samplerName, coord3, bias)
	#define SAMPLE_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)                textureName.Sample(samplerName, float4(coord3, index))
	#define SAMPLE_TEXTURECUBE_ARRAY_LOD(textureName, samplerName, coord3, index, lod)       textureName.SampleLevel(samplerName, float4(coord3, index), lod)
	#define SAMPLE_TEXTURECUBE_ARRAY_BIAS(textureName, samplerName, coord3, index, bias)     textureName.SampleBias(samplerName, float4(coord3, index), bias)
	#define SAMPLE_TEXTURE3D(textureName, samplerName, coord3)                               textureName.Sample(samplerName, coord3)
	#define SAMPLE_TEXTURE3D_LOD(textureName, samplerName, coord3, lod)                      textureName.SampleLevel(samplerName, coord3, lod)

	#define SAMPLE_TEXTURE2D_SHADOW(textureName, samplerName, coord3)                    textureName.SampleCmpLevelZero(samplerName, (coord3).xy, (coord3).z)
	#define SAMPLE_TEXTURE2D_ARRAY_SHADOW(textureName, samplerName, coord3, index)       textureName.SampleCmpLevelZero(samplerName, float3((coord3).xy, index), (coord3).z)
	#define SAMPLE_TEXTURECUBE_SHADOW(textureName, samplerName, coord4)                  textureName.SampleCmpLevelZero(samplerName, (coord4).xyz, (coord4).w)
	#define SAMPLE_TEXTURECUBE_ARRAY_SHADOW(textureName, samplerName, coord4, index)     textureName.SampleCmpLevelZero(samplerName, float4((coord4).xyz, index), (coord4).w)
	#define LOAD_TEXTURE2D(textureName, unCoord2)                       textureName.Load(int3(unCoord2, 0))
	#define LOAD_TEXTURE2D_LOD(textureName, unCoord2, lod)              textureName.Load(int3(unCoord2, lod))
	#define LOAD_TEXTURE2D_MSAA(textureName, unCoord2, sampleIndex)     textureName.Load(unCoord2, sampleIndex)
	#define LOAD_TEXTURE2D_ARRAY(textureName, unCoord2, index)          textureName.Load(int4(unCoord2, index, 0))
	#define LOAD_TEXTURE2D_ARRAY_LOD(textureName, unCoord2, index, lod) textureName.Load(int4(unCoord2, index, lod))
	#define LOAD_TEXTURE3D(textureName, unCoord3)                       textureName.Load(int4(unCoord3, 0))
	#define LOAD_TEXTURE3D_LOD(textureName, unCoord3, lod)              textureName.Load(int4(unCoord3, lod))

	#define PLATFORM_SUPPORT_GATHER
	#define GATHER_TEXTURE2D(textureName, samplerName, coord2)                textureName.Gather(samplerName, coord2)
	#define GATHER_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)   textureName.Gather(samplerName, float3(coord2, index))
	#define GATHER_TEXTURECUBE(textureName, samplerName, coord3)              textureName.Gather(samplerName, coord3)
	#define GATHER_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index) textureName.Gather(samplerName, float4(coord3, index))
	#define GATHER_RED_TEXTURE2D(textureName, samplerName, coord2)            textureName.GatherRed(samplerName, coord2)
	#define GATHER_GREEN_TEXTURE2D(textureName, samplerName, coord2)          textureName.GatherGreen(samplerName, coord2)
	#define GATHER_BLUE_TEXTURE2D(textureName, samplerName, coord2)           textureName.GatherBlue(samplerName, coord2)
	#define GATHER_ALPHA_TEXTURE2D(textureName, samplerName, coord2)          textureName.GatherAlpha(samplerName, coord2)

#elif defined(SHADER_API_GLCORE)
	#if (SHADER_TARGET >= 46)
	#define OPENGL4_1_SM5 1
	#else
	#define OPENGL4_1_SM5 0
	#endif
	#define ZERO_INITIALIZE(type, name) name = (type)0;
	#define ZERO_INITIALIZE_ARRAY(type, name, arraySize) { for (int arrayIndex = 0; arrayIndex < arraySize; arrayIndex++) { name[arrayIndex] = (type)0; } }
	#define CALCULATE_TEXTURE2D_LOD(textureName, samplerName, coord2) textureName.CalculateLevelOfDetail(samplerName, coord2)
	#define TEXTURE2D(textureName)                  Texture2D textureName
	#define TEXTURE2D_ARRAY(textureName)            Texture2DArray textureName
	#define TEXTURECUBE(textureName)                TextureCube textureName
	#define TEXTURECUBE_ARRAY(textureName)          TextureCubeArray textureName
	#define TEXTURE3D(textureName)                  Texture3D textureName

	#define TEXTURE2D_FLOAT(textureName)            TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_FLOAT(textureName)      TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_FLOAT(textureName)          TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_FLOAT(textureName)    TEXTURECUBE_ARRAY(textureName)
	#define TEXTURE3D_FLOAT(textureName)            TEXTURE3D(textureName)

	#define TEXTURE2D_HALF(textureName)             TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_HALF(textureName)       TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_HALF(textureName)           TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_HALF(textureName)     TEXTURECUBE_ARRAY(textureName)
	#define TEXTURE3D_HALF(textureName)             TEXTURE3D(textureName)

	#define TEXTURE2D_SHADOW(textureName)           TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_SHADOW(textureName)     TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_SHADOW(textureName)         TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_SHADOW(textureName)   TEXTURECUBE_ARRAY(textureName)

	#define RW_TEXTURE2D(type, textureName)         RWTexture2D<type> textureName
	#define RW_TEXTURE2D_ARRAY(type, textureName)   RWTexture2DArray<type> textureName
	#define RW_TEXTURE3D(type, textureName)         RWTexture3D<type> textureName

	#define SAMPLER(samplerName)                    SamplerState samplerName
	#define SAMPLER_CMP(samplerName)                SamplerComparisonState samplerName

	#define TEXTURE2D_PARAM(textureName, samplerName)                 TEXTURE2D(textureName),         SAMPLER(samplerName)
	#define TEXTURE2D_ARRAY_PARAM(textureName, samplerName)           TEXTURE2D_ARRAY(textureName),   SAMPLER(samplerName)
	#define TEXTURECUBE_PARAM(textureName, samplerName)               TEXTURECUBE(textureName),       SAMPLER(samplerName)
	#define TEXTURECUBE_ARRAY_PARAM(textureName, samplerName)         TEXTURECUBE_ARRAY(textureName), SAMPLER(samplerName)
	#define TEXTURE3D_PARAM(textureName, samplerName)                 TEXTURE3D(textureName),         SAMPLER(samplerName)

	#define TEXTURE2D_SHADOW_PARAM(textureName, samplerName)          TEXTURE2D(textureName),         SAMPLER_CMP(samplerName)
	#define TEXTURE2D_ARRAY_SHADOW_PARAM(textureName, samplerName)    TEXTURE2D_ARRAY(textureName),   SAMPLER_CMP(samplerName)
	#define TEXTURECUBE_SHADOW_PARAM(textureName, samplerName)        TEXTURECUBE(textureName),       SAMPLER_CMP(samplerName)
	#define TEXTURECUBE_ARRAY_SHADOW_PARAM(textureName, samplerName)  TEXTURECUBE_ARRAY(textureName), SAMPLER_CMP(samplerName)

	#define TEXTURE2D_ARGS(textureName, samplerName)                textureName, samplerName
	#define TEXTURE2D_ARRAY_ARGS(textureName, samplerName)          textureName, samplerName
	#define TEXTURECUBE_ARGS(textureName, samplerName)              textureName, samplerName
	#define TEXTURECUBE_ARRAY_ARGS(textureName, samplerName)        textureName, samplerName
	#define TEXTURE3D_ARGS(textureName, samplerName)                textureName, samplerName

	#define TEXTURE2D_SHADOW_ARGS(textureName, samplerName)         textureName, samplerName
	#define TEXTURE2D_ARRAY_SHADOW_ARGS(textureName, samplerName)   textureName, samplerName
	#define TEXTURECUBE_SHADOW_ARGS(textureName, samplerName)       textureName, samplerName
	#define TEXTURECUBE_ARRAY_SHADOW_ARGS(textureName, samplerName) textureName, samplerName

	#define SAMPLE_TEXTURE2D(textureName, samplerName, coord2)                               textureName.Sample(samplerName, coord2)
	#define SAMPLE_TEXTURE2D_LOD(textureName, samplerName, coord2, lod)                      textureName.SampleLevel(samplerName, coord2, lod)
	#define SAMPLE_TEXTURE2D_BIAS(textureName, samplerName, coord2, bias)                    textureName.SampleBias(samplerName, coord2, bias)
	#define SAMPLE_TEXTURE2D_GRAD(textureName, samplerName, coord2, ddx, ddy)                textureName.SampleGrad(samplerName, coord2, ddx, ddy)
	#define SAMPLE_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)                  textureName.Sample(samplerName, float3(coord2, index))
	#define SAMPLE_TEXTURE2D_ARRAY_LOD(textureName, samplerName, coord2, index, lod)         textureName.SampleLevel(samplerName, float3(coord2, index), lod)
	#define SAMPLE_TEXTURE2D_ARRAY_BIAS(textureName, samplerName, coord2, index, bias)       textureName.SampleBias(samplerName, float3(coord2, index), bias)
	#define SAMPLE_TEXTURE2D_ARRAY_GRAD(textureName, samplerName, coord2, index, dpdx, dpdy) textureName.SampleGrad(samplerName, float3(coord2, index), dpdx, dpdy)
	#define SAMPLE_TEXTURECUBE(textureName, samplerName, coord3)                             textureName.Sample(samplerName, coord3)
	#define SAMPLE_TEXTURECUBE_LOD(textureName, samplerName, coord3, lod)                    textureName.SampleLevel(samplerName, coord3, lod)
	#define SAMPLE_TEXTURECUBE_BIAS(textureName, samplerName, coord3, bias)                  textureName.SampleBias(samplerName, coord3, bias)
	#ifdef UNITY_NO_CUBEMAP_ARRAY
	#define SAMPLE_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)           ERROR_ON_UNSUPPORTED_FUNCTION(SAMPLE_TEXTURECUBE_ARRAY)
	#define SAMPLE_TEXTURECUBE_ARRAY_LOD(textureName, samplerName, coord3, index, lod)  ERROR_ON_UNSUPPORTED_FUNCTION(SAMPLE_TEXTURECUBE_ARRAY_LOD)
	#define SAMPLE_TEXTURECUBE_ARRAY_LOD(textureName, samplerName, coord3, index, bias) ERROR_ON_UNSUPPORTED_FUNCTION(SAMPLE_TEXTURECUBE_ARRAY_LOD)
	#else
	#define SAMPLE_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)           textureName.Sample(samplerName, float4(coord3, index))
	#define SAMPLE_TEXTURECUBE_ARRAY_LOD(textureName, samplerName, coord3, index, lod)  textureName.SampleLevel(samplerName, float4(coord3, index), lod)
	#define SAMPLE_TEXTURECUBE_ARRAY_BIAS(textureName, samplerName, coord3, index, bias)textureName.SampleBias(samplerName, float4(coord3, index), bias)
	#endif
	#define SAMPLE_TEXTURE3D(textureName, samplerName, coord3)                          textureName.Sample(samplerName, coord3)
	#define SAMPLE_TEXTURE3D_LOD(textureName, samplerName, coord3, lod)                 textureName.SampleLevel(samplerName, coord3, lod)

	#define SAMPLE_TEXTURE2D_SHADOW(textureName, samplerName, coord3)                   textureName.SampleCmpLevelZero(samplerName, (coord3).xy, (coord3).z)
	#define SAMPLE_TEXTURE2D_ARRAY_SHADOW(textureName, samplerName, coord3, index)      textureName.SampleCmpLevelZero(samplerName, float3((coord3).xy, index), (coord3).z)
	#define SAMPLE_TEXTURECUBE_SHADOW(textureName, samplerName, coord4)                 textureName.SampleCmpLevelZero(samplerName, (coord4).xyz, (coord4).w)
	#define SAMPLE_TEXTURECUBE_ARRAY_SHADOW(textureName, samplerName, coord4, index)    textureName.SampleCmpLevelZero(samplerName, float4((coord4).xyz, index), (coord4).w)

	#define LOAD_TEXTURE2D(textureName, unCoord2)                                   textureName.Load(int3(unCoord2, 0))
	#define LOAD_TEXTURE2D_LOD(textureName, unCoord2, lod)                          textureName.Load(int3(unCoord2, lod))
	#define LOAD_TEXTURE2D_MSAA(textureName, unCoord2, sampleIndex)                 textureName.Load(unCoord2, sampleIndex)
	#define LOAD_TEXTURE2D_ARRAY(textureName, unCoord2, index)                      textureName.Load(int4(unCoord2, index, 0))
	#define LOAD_TEXTURE2D_ARRAY_MSAA(textureName, unCoord2, index, sampleIndex)    textureName.Load(int3(unCoord2, index), sampleIndex)
	#define LOAD_TEXTURE2D_ARRAY_LOD(textureName, unCoord2, index, lod)             textureName.Load(int4(unCoord2, index, lod))

	#if OPENGL4_1_SM5
	#define PLATFORM_SUPPORT_GATHER
	#define GATHER_TEXTURE2D(textureName, samplerName, coord2)                  textureName.Gather(samplerName, coord2)
	#define GATHER_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)     textureName.Gather(samplerName, float3(coord2, index))
	#define GATHER_TEXTURECUBE(textureName, samplerName, coord3)                textureName.Gather(samplerName, coord3)
	#define GATHER_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)   textureName.Gather(samplerName, float4(coord3, index))
	#else
	#define GATHER_TEXTURE2D(textureName, samplerName, coord2)                  ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_TEXTURE2D)
	#define GATHER_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)     ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_TEXTURE2D_ARRAY)
	#define GATHER_TEXTURECUBE(textureName, samplerName, coord3)                ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_TEXTURECUBE)
	#define GATHER_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)   ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_TEXTURECUBE_ARRAY)
	#endif
	#elif defined(SHADER_API_GLES3)
	#if (SHADER_TARGET >= 40)
	#define GLES3_1_AEP 1
	#else
	#define GLES3_1_AEP 0
	#endif
	#define ZERO_INITIALIZE(type, name) name = (type)0;
	#define ZERO_INITIALIZE_ARRAY(type, name, arraySize) { for (int arrayIndex = 0; arrayIndex < arraySize; arrayIndex++) { name[arrayIndex] = (type)0; } }
	#define CALCULATE_TEXTURE2D_LOD(textureName, samplerName, coord2) textureName.CalculateLevelOfDetail(samplerName, coord2)
	#define TEXTURE2D(textureName)                  Texture2D textureName
	#define TEXTURE2D_ARRAY(textureName)            Texture2DArray textureName
	#define TEXTURECUBE(textureName)                TextureCube textureName
	#define TEXTURECUBE_ARRAY(textureName)          TextureCubeArray textureName
	#define TEXTURE3D(textureName)                  Texture3D textureName

	#define TEXTURE2D_FLOAT(textureName)            Texture2D_float textureName
	#define TEXTURE2D_ARRAY_FLOAT(textureName)      Texture2DArray textureName    
	#define TEXTURECUBE_FLOAT(textureName)          TextureCube_float textureName
	#define TEXTURECUBE_ARRAY_FLOAT(textureName)    TextureCubeArray textureName  
	#define TEXTURE3D_FLOAT(textureName)            Texture3D_float textureName

	#define TEXTURE2D_HALF(textureName)             Texture2D_half textureName
	#define TEXTURE2D_ARRAY_HALF(textureName)       Texture2DArray textureName    
	#define TEXTURECUBE_HALF(textureName)           TextureCube_half textureName
	#define TEXTURECUBE_ARRAY_HALF(textureName)     TextureCubeArray textureName  
	#define TEXTURE3D_HALF(textureName)             Texture3D_half textureName

	#define TEXTURE2D_SHADOW(textureName)           TEXTURE2D(textureName)
	#define TEXTURE2D_ARRAY_SHADOW(textureName)     TEXTURE2D_ARRAY(textureName)
	#define TEXTURECUBE_SHADOW(textureName)         TEXTURECUBE(textureName)
	#define TEXTURECUBE_ARRAY_SHADOW(textureName)   TEXTURECUBE_ARRAY(textureName)

	#if GLES3_1_AEP
	#define RW_TEXTURE2D(type, textureName)         RWTexture2D<type> textureName
	#define RW_TEXTURE2D_ARRAY(type, textureName)   RWTexture2DArray<type> textureName
	#define RW_TEXTURE3D(type, textureName)         RWTexture3D<type> textureName
	#else
	#define RW_TEXTURE2D(type, textureName)         ERROR_ON_UNSUPPORTED_FUNCTION(RWTexture2D)
	#define RW_TEXTURE2D_ARRAY(type, textureName)   ERROR_ON_UNSUPPORTED_FUNCTION(RWTexture2DArray)
	#define RW_TEXTURE3D(type, textureName)         ERROR_ON_UNSUPPORTED_FUNCTION(RWTexture3D)
	#endif

	#define SAMPLER(samplerName)                    SamplerState samplerName
	#define SAMPLER_CMP(samplerName)                SamplerComparisonState samplerName

	#define TEXTURE2D_PARAM(textureName, samplerName)                 TEXTURE2D(textureName),         SAMPLER(samplerName)
	#define TEXTURE2D_ARRAY_PARAM(textureName, samplerName)           TEXTURE2D_ARRAY(textureName),   SAMPLER(samplerName)
	#define TEXTURECUBE_PARAM(textureName, samplerName)               TEXTURECUBE(textureName),       SAMPLER(samplerName)
	#define TEXTURECUBE_ARRAY_PARAM(textureName, samplerName)         TEXTURECUBE_ARRAY(textureName), SAMPLER(samplerName)
	#define TEXTURE3D_PARAM(textureName, samplerName)                 TEXTURE3D(textureName),         SAMPLER(samplerName)

	#define TEXTURE2D_SHADOW_PARAM(textureName, samplerName)          TEXTURE2D(textureName),         SAMPLER_CMP(samplerName)
	#define TEXTURE2D_ARRAY_SHADOW_PARAM(textureName, samplerName)    TEXTURE2D_ARRAY(textureName),   SAMPLER_CMP(samplerName)
	#define TEXTURECUBE_SHADOW_PARAM(textureName, samplerName)        TEXTURECUBE(textureName),       SAMPLER_CMP(samplerName)
	#define TEXTURECUBE_ARRAY_SHADOW_PARAM(textureName, samplerName)  TEXTURECUBE_ARRAY(textureName), SAMPLER_CMP(samplerName)

	#define TEXTURE2D_ARGS(textureName, samplerName)                textureName, samplerName
	#define TEXTURE2D_ARRAY_ARGS(textureName, samplerName)          textureName, samplerName
	#define TEXTURECUBE_ARGS(textureName, samplerName)              textureName, samplerName
	#define TEXTURECUBE_ARRAY_ARGS(textureName, samplerName)        textureName, samplerName
	#define TEXTURE3D_ARGS(textureName, samplerName)                textureName, samplerName

	#define TEXTURE2D_SHADOW_ARGS(textureName, samplerName)         textureName, samplerName
	#define TEXTURE2D_ARRAY_SHADOW_ARGS(textureName, samplerName)   textureName, samplerName
	#define TEXTURECUBE_SHADOW_ARGS(textureName, samplerName)       textureName, samplerName
	#define TEXTURECUBE_ARRAY_SHADOW_ARGS(textureName, samplerName) textureName, samplerName

	#define SAMPLE_TEXTURE2D(textureName, samplerName, coord2)                               textureName.Sample(samplerName, coord2)
	#define SAMPLE_TEXTURE2D_LOD(textureName, samplerName, coord2, lod)                      textureName.SampleLevel(samplerName, coord2, lod)
	#define SAMPLE_TEXTURE2D_BIAS(textureName, samplerName, coord2, bias)                    textureName.SampleBias(samplerName, coord2, bias)
	#define SAMPLE_TEXTURE2D_GRAD(textureName, samplerName, coord2, ddx, ddy)                textureName.SampleGrad(samplerName, coord2, ddx, ddy)
	#define SAMPLE_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)                  textureName.Sample(samplerName, float3(coord2, index))
	#define SAMPLE_TEXTURE2D_ARRAY_LOD(textureName, samplerName, coord2, index, lod)         textureName.SampleLevel(samplerName, float3(coord2, index), lod)
	#define SAMPLE_TEXTURE2D_ARRAY_BIAS(textureName, samplerName, coord2, index, bias)       textureName.SampleBias(samplerName, float3(coord2, index), bias)
	#define SAMPLE_TEXTURE2D_ARRAY_GRAD(textureName, samplerName, coord2, index, dpdx, dpdy) textureName.SampleGrad(samplerName, float3(coord2, index), dpdx, dpdy)
	#define SAMPLE_TEXTURECUBE(textureName, samplerName, coord3)                             textureName.Sample(samplerName, coord3)
	#define SAMPLE_TEXTURECUBE_LOD(textureName, samplerName, coord3, lod)                    textureName.SampleLevel(samplerName, coord3, lod)
	#define SAMPLE_TEXTURECUBE_BIAS(textureName, samplerName, coord3, bias)                  textureName.SampleBias(samplerName, coord3, bias)

	#ifdef UNITY_NO_CUBEMAP_ARRAY
	#define SAMPLE_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)           ERROR_ON_UNSUPPORTED_FUNCTION(SAMPLE_TEXTURECUBE_ARRAY)
	#define SAMPLE_TEXTURECUBE_ARRAY_LOD(textureName, samplerName, coord3, index, lod)  ERROR_ON_UNSUPPORTED_FUNCTION(SAMPLE_TEXTURECUBE_ARRAY_LOD)
	#define SAMPLE_TEXTURECUBE_ARRAY_BIAS(textureName, samplerName, coord3, index, bias)ERROR_ON_UNSUPPORTED_FUNCTION(SAMPLE_TEXTURECUBE_ARRAY_BIAS)
	#else
	#define SAMPLE_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)           textureName.Sample(samplerName, float4(coord3, index))
	#define SAMPLE_TEXTURECUBE_ARRAY_LOD(textureName, samplerName, coord3, index, lod)  textureName.SampleLevel(samplerName, float4(coord3, index), lod)
	#define SAMPLE_TEXTURECUBE_ARRAY_BIAS(textureName, samplerName, coord3, index, bias)textureName.SampleBias(samplerName, float4(coord3, index), bias)
	#endif

	#define SAMPLE_TEXTURE3D(textureName, samplerName, coord3)                          textureName.Sample(samplerName, coord3)
	#define SAMPLE_TEXTURE3D_LOD(textureName, samplerName, coord3, lod)                 textureName.SampleLevel(samplerName, coord3, lod)

	#define SAMPLE_TEXTURE2D_SHADOW(textureName, samplerName, coord3)                   textureName.SampleCmpLevelZero(samplerName, (coord3).xy, (coord3).z)
	#define SAMPLE_TEXTURE2D_ARRAY_SHADOW(textureName, samplerName, coord3, index)      textureName.SampleCmpLevelZero(samplerName, float3((coord3).xy, index), (coord3).z)
	#define SAMPLE_TEXTURECUBE_SHADOW(textureName, samplerName, coord4)                 textureName.SampleCmpLevelZero(samplerName, (coord4).xyz, (coord4).w)
	#define SAMPLE_TEXTURECUBE_ARRAY_SHADOW(textureName, samplerName, coord4, index)    textureName.SampleCmpLevelZero(samplerName, float4((coord4).xyz, index), (coord4).w)
	#define LOAD_TEXTURE2D(textureName, unCoord2)                                       textureName.Load(int3(unCoord2, 0))
	#define LOAD_TEXTURE2D_LOD(textureName, unCoord2, lod)                              textureName.Load(int3(unCoord2, lod))
	#define LOAD_TEXTURE2D_MSAA(textureName, unCoord2, sampleIndex)                     textureName.Load(unCoord2, sampleIndex)
	#define LOAD_TEXTURE2D_ARRAY(textureName, unCoord2, index)                          textureName.Load(int4(unCoord2, index, 0))
	#define LOAD_TEXTURE2D_ARRAY_MSAA(textureName, unCoord2, index, sampleIndex)        textureName.Load(int3(unCoord2, index), sampleIndex)
	#define LOAD_TEXTURE2D_ARRAY_LOD(textureName, unCoord2, index, lod)                 textureName.Load(int4(unCoord2, index, lod))
	#define LOAD_TEXTURE3D(textureName, unCoord3)                                       textureName.Load(int4(unCoord3, 0))
	#define LOAD_TEXTURE3D_LOD(textureName, unCoord3, lod)                              textureName.Load(int4(unCoord3, lod))

	#if GLES3_1_AEP
	#define PLATFORM_SUPPORT_GATHER
	#define GATHER_TEXTURE2D(textureName, samplerName, coord2)                  textureName.Gather(samplerName, coord2)
	#define GATHER_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)     textureName.Gather(samplerName, float3(coord2, index))
	#define GATHER_TEXTURECUBE(textureName, samplerName, coord3)                textureName.Gather(samplerName, coord3)
	#define GATHER_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)   textureName.Gather(samplerName, float4(coord3, index))
	#define GATHER_RED_TEXTURE2D(textureName, samplerName, coord2)              textureName.GatherRed(samplerName, coord2)
	#define GATHER_GREEN_TEXTURE2D(textureName, samplerName, coord2)            textureName.GatherGreen(samplerName, coord2)
	#define GATHER_BLUE_TEXTURE2D(textureName, samplerName, coord2)             textureName.GatherBlue(samplerName, coord2)
	#define GATHER_ALPHA_TEXTURE2D(textureName, samplerName, coord2)            textureName.GatherAlpha(samplerName, coord2)
	#else
	#define GATHER_TEXTURE2D(textureName, samplerName, coord2)                  ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_TEXTURE2D)
	#define GATHER_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)     ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_TEXTURE2D_ARRAY)
	#define GATHER_TEXTURECUBE(textureName, samplerName, coord3)                ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_TEXTURECUBE)
	#define GATHER_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)   ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_TEXTURECUBE_ARRAY)
	#define GATHER_RED_TEXTURE2D(textureName, samplerName, coord2)              ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_RED_TEXTURE2D)
	#define GATHER_GREEN_TEXTURE2D(textureName, samplerName, coord2)            ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_GREEN_TEXTURE2D)
	#define GATHER_BLUE_TEXTURE2D(textureName, samplerName, coord2)             ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_BLUE_TEXTURE2D)
	#define GATHER_ALPHA_TEXTURE2D(textureName, samplerName, coord2)            ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_ALPHA_TEXTURE2D)
	#endif
#elif defined(SHADER_API_GLES)
	#define uint int

	#define rcp(x) 1.0 / (x)
	#define ddx_fine ddx
	#define ddy_fine ddy
	#define asfloat
	#define asuint(x) asint(x)
	#define f32tof16
	#define f16tof32

	#define ERROR_ON_UNSUPPORTED_FUNCTION(funcName) #error #funcName is not supported on GLES 2.0
	#define ZERO_INITIALIZE(type, name) name = (type)0;
	#define ZERO_INITIALIZE_ARRAY(type, name, arraySize) { for (int arrayIndex = 0; arrayIndex < arraySize; arrayIndex++) { name[arrayIndex] = (type)0; } }
	#define CALCULATE_TEXTURE2D_LOD(textureName, samplerName, coord2) #error calculate Level of Detail not supported in GLES2
	#define TEXTURE2D(textureName)                          sampler2D textureName
	#define TEXTURE2D_ARRAY(textureName)                    samplerCUBE textureName 
	#define TEXTURECUBE(textureName)                        samplerCUBE textureName
	#define TEXTURECUBE_ARRAY(textureName)                  samplerCUBE textureName 
	#define TEXTURE3D(textureName)                          sampler3D textureName

	#define TEXTURE2D_FLOAT(textureName)                    sampler2D_float textureName
	#define TEXTURE2D_ARRAY_FLOAT(textureName)              TEXTURECUBE_FLOAT(textureName) 
	#define TEXTURECUBE_FLOAT(textureName)                  samplerCUBE_float textureName
	#define TEXTURECUBE_ARRAY_FLOAT(textureName)            TEXTURECUBE_FLOAT(textureName) 
	#define TEXTURE3D_FLOAT(textureName)                    sampler3D_float textureName

	#define TEXTURE2D_HALF(textureName)                     sampler2D_half textureName
	#define TEXTURE2D_ARRAY_HALF(textureName)               TEXTURECUBE_HALF(textureName) 
	#define TEXTURECUBE_HALF(textureName)                   samplerCUBE_half textureName
	#define TEXTURECUBE_ARRAY_HALF(textureName)             TEXTURECUBE_HALF(textureName) 
	#define TEXTURE3D_HALF(textureName)                     sampler3D_half textureName

	#define TEXTURE2D_SHADOW(textureName)                   SHADOW2D_TEXTURE_AND_SAMPLER textureName
	#define TEXTURE2D_ARRAY_SHADOW(textureName)             TEXTURECUBE_SHADOW(textureName) 
	#define TEXTURECUBE_SHADOW(textureName)                 SHADOWCUBE_TEXTURE_AND_SAMPLER textureName
	#define TEXTURECUBE_ARRAY_SHADOW(textureName)           TEXTURECUBE_SHADOW(textureName) 

	#define RW_TEXTURE2D(type, textureNam)                  ERROR_ON_UNSUPPORTED_FUNCTION(RWTexture2D)
	#define RW_TEXTURE2D_ARRAY(type, textureName)           ERROR_ON_UNSUPPORTED_FUNCTION(RWTexture2DArray)
	#define RW_TEXTURE3D(type, textureNam)                  ERROR_ON_UNSUPPORTED_FUNCTION(RWTexture3D)

	#define SAMPLER(samplerName)
	#define SAMPLER_CMP(samplerName)

	#define TEXTURE2D_PARAM(textureName, samplerName)                sampler2D textureName
	#define TEXTURE2D_ARRAY_PARAM(textureName, samplerName)          samplerCUBE textureName
	#define TEXTURECUBE_PARAM(textureName, samplerName)              samplerCUBE textureName
	#define TEXTURECUBE_ARRAY_PARAM(textureName, samplerName)        samplerCUBE textureName
	#define TEXTURE3D_PARAM(textureName, samplerName)                sampler3D textureName
	#define TEXTURE2D_SHADOW_PARAM(textureName, samplerName)         SHADOW2D_TEXTURE_AND_SAMPLER textureName
	#define TEXTURE2D_ARRAY_SHADOW_PARAM(textureName, samplerName)   SHADOWCUBE_TEXTURE_AND_SAMPLER textureName
	#define TEXTURECUBE_SHADOW_PARAM(textureName, samplerName)       SHADOWCUBE_TEXTURE_AND_SAMPLER textureName

	#define TEXTURE2D_ARGS(textureName, samplerName)               textureName
	#define TEXTURE2D_ARRAY_ARGS(textureName, samplerName)         textureName
	#define TEXTURECUBE_ARGS(textureName, samplerName)             textureName
	#define TEXTURECUBE_ARRAY_ARGS(textureName, samplerName)       textureName
	#define TEXTURE3D_ARGS(textureName, samplerName)               textureName
	#define TEXTURE2D_SHADOW_ARGS(textureName, samplerName)        textureName
	#define TEXTURE2D_ARRAY_SHADOW_ARGS(textureName, samplerName)  textureName
	#define TEXTURECUBE_SHADOW_ARGS(textureName, samplerName)      textureName

	#define SAMPLE_TEXTURE2D(textureName, samplerName, coord2) tex2D(textureName, coord2)

	#if (SHADER_TARGET >= 30)
	    #define SAMPLE_TEXTURE2D_LOD(textureName, samplerName, coord2, lod) tex2Dlod(textureName, float4(coord2, 0, lod))
	#else
	    #define SAMPLE_TEXTURE2D_LOD(textureName, samplerName, coord2, lod) SAMPLE_TEXTURE2D_BIAS(textureName, samplerName, coord2, lod)
	#endif

	#define SAMPLE_TEXTURE2D_BIAS(textureName, samplerName, coord2, bias)                       tex2Dbias(textureName, float4(coord2, 0, bias))
	#define SAMPLE_TEXTURE2D_GRAD(textureName, samplerName, coord2, ddx, ddy)                   SAMPLE_TEXTURE2D(textureName, samplerName, coord2)
	#define SAMPLE_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)                     ERROR_ON_UNSUPPORTED_FUNCTION(SAMPLE_TEXTURE2D_ARRAY)
	#define SAMPLE_TEXTURE2D_ARRAY_LOD(textureName, samplerName, coord2, index, lod)            ERROR_ON_UNSUPPORTED_FUNCTION(SAMPLE_TEXTURE2D_ARRAY_LOD)
	#define SAMPLE_TEXTURE2D_ARRAY_BIAS(textureName, samplerName, coord2, index, bias)          ERROR_ON_UNSUPPORTED_FUNCTION(SAMPLE_TEXTURE2D_ARRAY_BIAS)
	#define SAMPLE_TEXTURE2D_ARRAY_GRAD(textureName, samplerName, coord2, index, dpdx, dpdy)    ERROR_ON_UNSUPPORTED_FUNCTION(SAMPLE_TEXTURE2D_ARRAY_GRAD)
	#define SAMPLE_TEXTURECUBE(textureName, samplerName, coord3)                                texCUBE(textureName, coord3)
	#define SAMPLE_TEXTURECUBE_LOD(textureName, samplerName, coord3, lod)                       SAMPLE_TEXTURECUBE_BIAS(textureName, samplerName, coord3, lod)
	#define SAMPLE_TEXTURECUBE_BIAS(textureName, samplerName, coord3, bias)                     texCUBEbias(textureName, float4(coord3, bias))
	#define SAMPLE_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)                   ERROR_ON_UNSUPPORTED_FUNCTION(SAMPLE_TEXTURECUBE_ARRAY)
	#define SAMPLE_TEXTURECUBE_ARRAY_LOD(textureName, samplerName, coord3, index, lod)          ERROR_ON_UNSUPPORTED_FUNCTION(SAMPLE_TEXTURECUBE_ARRAY_LOD)
	#define SAMPLE_TEXTURECUBE_ARRAY_BIAS(textureName, samplerName, coord3, index, bias)        ERROR_ON_UNSUPPORTED_FUNCTION(SAMPLE_TEXTURECUBE_ARRAY_BIAS)
	#define SAMPLE_TEXTURE3D(textureName, samplerName, coord3)                                  tex3D(textureName, coord3)
	#define SAMPLE_TEXTURE3D_LOD(textureName, samplerName, coord3, lod)                         ERROR_ON_UNSUPPORTED_FUNCTION(SAMPLE_TEXTURE3D_LOD)

	#define SAMPLE_TEXTURE2D_SHADOW(textureName, samplerName, coord3)                           SHADOW2D_SAMPLE(textureName, samplerName, coord3)
	#define SAMPLE_TEXTURE2D_ARRAY_SHADOW(textureName, samplerName, coord3, index)              ERROR_ON_UNSUPPORTED_FUNCTION(SAMPLE_TEXTURE2D_ARRAY_SHADOW)
	#define SAMPLE_TEXTURECUBE_SHADOW(textureName, samplerName, coord4)                         SHADOWCUBE_SAMPLE(textureName, samplerName, coord4)
	#define SAMPLE_TEXTURECUBE_ARRAY_SHADOW(textureName, samplerName, coord4, index)            ERROR_ON_UNSUPPORTED_FUNCTION(SAMPLE_TEXTURECUBE_ARRAY_SHADOW)
	#define LOAD_TEXTURE2D(textureName, unCoord2)                                               half4(0, 0, 0, 0)
	#define LOAD_TEXTURE2D_LOD(textureName, unCoord2, lod)                                      half4(0, 0, 0, 0)
	#define LOAD_TEXTURE2D_MSAA(textureName, unCoord2, sampleIndex)                             half4(0, 0, 0, 0)
	#define LOAD_TEXTURE2D_ARRAY(textureName, unCoord2, index)                                  half4(0, 0, 0, 0)
	#define LOAD_TEXTURE2D_ARRAY_MSAA(textureName, unCoord2, index, sampleIndex)                half4(0, 0, 0, 0)
	#define LOAD_TEXTURE2D_ARRAY_LOD(textureName, unCoord2, index, lod)                         half4(0, 0, 0, 0)
	#define LOAD_TEXTURE3D(textureName, unCoord3)                                               ERROR_ON_UNSUPPORTED_FUNCTION(LOAD_TEXTURE3D)
	#define LOAD_TEXTURE3D_LOD(textureName, unCoord3, lod)                                      ERROR_ON_UNSUPPORTED_FUNCTION(LOAD_TEXTURE3D_LOD)
	#define GATHER_TEXTURE2D(textureName, samplerName, coord2)                  ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_TEXTURE2D)
	#define GATHER_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)     ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_TEXTURE2D_ARRAY)
	#define GATHER_TEXTURECUBE(textureName, samplerName, coord3)                ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_TEXTURECUBE)
	#define GATHER_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)   ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_TEXTURECUBE_ARRAY)
	#define GATHER_RED_TEXTURE2D(textureName, samplerName, coord2)              ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_RED_TEXTURE2D)
	#define GATHER_GREEN_TEXTURE2D(textureName, samplerName, coord2)            ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_GREEN_TEXTURE2D)
	#define GATHER_BLUE_TEXTURE2D(textureName, samplerName, coord2)             ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_BLUE_TEXTURE2D)
	#define GATHER_ALPHA_TEXTURE2D(textureName, samplerName, coord2)            ERROR_ON_UNSUPPORTED_FUNCTION(GATHER_ALPHA_TEXTURE2D)

#else
#error unsupported shader api
#endif
#ifndef UNITY_BRANCH
#   define UNITY_BRANCH
#endif
#ifndef UNITY_FLATTEN
#   define UNITY_FLATTEN
#endif
#ifndef UNITY_UNROLL
#   define UNITY_UNROLL
#endif
#ifndef UNITY_UNROLLX
#   define UNITY_UNROLLX(_x)
#endif
#ifndef UNITY_LOOP
#   define UNITY_LOOP
#endif
#define _USINGTEXCOORD1 1
         struct VertexToPixel
         {
            UNITY_POSITION(pos);
            float3 worldPos : TEXCOORD0;
            float3 worldNormal : TEXCOORD1;
            float4 worldTangent : TEXCOORD2;
             float4 texcoord0 : TEXCOORD3;
             float4 texcoord1 : TEXCOORD4;
            #ifdef EDITOR_VISUALIZATION
              float2 vizUV : TEXCOORD8;
              float4 lightCoord : TEXCOORD9;
            #endif
             float4 extraV2F0 : TEXCOORD10;
            UNITY_VERTEX_INPUT_INSTANCE_ID
            UNITY_VERTEX_OUTPUT_STEREO
         };
            struct Surface
            {
               half3 Albedo;
               half Height;
               half3 Normal;
               half Smoothness;
               half3 Emission;
               half Metallic;
               half3 Specular;
               half Occlusion;
               half SpecularPower; 
               half Alpha;
               float outputDepth; 
               half SpecularOcclusion;
               half SubsurfaceMask;
               half Thickness;
               half CoatMask;
               half CoatSmoothness;
               half Anisotropy;
               half IridescenceMask;
               half IridescenceThickness;
               int DiffusionProfileHash;
               float SpecularAAThreshold;
               float SpecularAAScreenSpaceVariance;
               float3 DiffuseGI;
               float3 BackDiffuseGI;
               float3 SpecularGI;
               float ior;
               float3 transmittanceColor;
               float atDistance;
               float transmittanceMask;
               float4 ShadowMask;
               float NormalAlpha;
               float MAOSAlpha;
            };
            struct Blackboard
            {
                float blackboardDummyData;
            };
            struct ShaderData
            {
               float4 clipPos; 
               float3 localSpacePosition;
               float3 localSpaceNormal;
               float3 localSpaceTangent;
               float3 worldSpacePosition;
               float3 worldSpaceNormal;
               float3 worldSpaceTangent;
               float tangentSign;

               float3 worldSpaceViewDir;
               float3 tangentSpaceViewDir;

               float4 texcoord0;
               float4 texcoord1;
               float4 texcoord2;
               float4 texcoord3;

               float2 screenUV;
               float4 screenPos;

               float4 vertexColor;
               bool isFrontFace;

               float4 extraV2F0;
               float4 extraV2F1;
               float4 extraV2F2;
               float4 extraV2F3;
               float4 extraV2F4;
               float4 extraV2F5;
               float4 extraV2F6;
               float4 extraV2F7;

               float3x3 TBNMatrix;
               Blackboard blackboard;
            };

            struct VertexData
            {
               #if SHADER_TARGET > 30
               #endif
               float4 vertex : POSITION;
               float3 normal : NORMAL;
               float4 tangent : TANGENT;
               float4 texcoord0 : TEXCOORD0;
               #if _URP && (_USINGTEXCOORD1 || _PASSMETA || _PASSFORWARD || _PASSGBUFFER)
                  float4 texcoord1 : TEXCOORD1;
               #endif

               #if _URP && (_USINGTEXCOORD2 || _PASSMETA || ((_PASSFORWARD || _PASSGBUFFER) && defined(DYNAMICLIGHTMAP_ON)))
                  float4 texcoord2 : TEXCOORD2;
               #endif

               #if _STANDARD && (_USINGTEXCOORD1 || (_PASSMETA || ((_PASSFORWARD || _PASSGBUFFER || _PASSFORWARDADD) && LIGHTMAP_ON)))
                  float4 texcoord1 : TEXCOORD1;
               #endif
               #if _STANDARD && (_USINGTEXCOORD2 || (_PASSMETA || ((_PASSFORWARD || _PASSGBUFFER) && DYNAMICLIGHTMAP_ON)))
                  float4 texcoord2 : TEXCOORD2;
               #endif
               #if _HDRP
                  float4 texcoord1 : TEXCOORD1;
                  float4 texcoord2 : TEXCOORD2;
               #endif
               #if _PASSMOTIONVECTOR || ((_PASSFORWARD || _PASSUNLIT) && defined(_WRITE_TRANSPARENT_MOTION_VECTOR))
                  float3 previousPositionOS : TEXCOORD4; 
                  #if defined (_ADD_PRECOMPUTED_VELOCITY)
                     float3 precomputedVelocity    : TEXCOORD5; 
                  #endif
               #endif

               UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct TessVertex 
            {
               float4 vertex : INTERNALTESSPOS;
               float3 normal : NORMAL;
               float4 tangent : TANGENT;
               float4 texcoord0 : TEXCOORD0;
               float4 texcoord1 : TEXCOORD1;
               float4 texcoord2 : TEXCOORD2;
                float4 extraV2F0 : TEXCOORD5;
               #if _PASSMOTIONVECTOR || ((_PASSFORWARD || _PASSUNLIT) && defined(_WRITE_TRANSPARENT_MOTION_VECTOR))
                  float3 previousPositionOS : TEXCOORD13; 
                  #if defined (_ADD_PRECOMPUTED_VELOCITY)
                     float3 precomputedVelocity : TEXCOORD14;
                  #endif
               #endif

               UNITY_VERTEX_INPUT_INSTANCE_ID
               UNITY_VERTEX_OUTPUT_STEREO
            };

            struct ExtraV2F
            {
               float4 extraV2F0;
               float4 extraV2F1;
               float4 extraV2F2;
               float4 extraV2F3;
               float4 extraV2F4;
               float4 extraV2F5;
               float4 extraV2F6;
               float4 extraV2F7;
               Blackboard blackboard;
               float4 time;
            };
            float3 WorldToTangentSpace(ShaderData d, float3 normal)
            {
               return mul(d.TBNMatrix, normal);
            }

            float3 TangentToWorldSpace(ShaderData d, float3 normal)
            {
               return mul(normal, d.TBNMatrix);
            }
            #if _STANDARD
               float3 TransformWorldToObject(float3 p) { return mul(unity_WorldToObject, float4(p, 1)); };
               float3 TransformObjectToWorld(float3 p) { return mul(unity_ObjectToWorld, float4(p, 1)); };
               float4 TransformWorldToObject(float4 p) { return mul(unity_WorldToObject, p); };
               float4 TransformObjectToWorld(float4 p) { return mul(unity_ObjectToWorld, p); };
               float4x4 GetWorldToObjectMatrix() { return unity_WorldToObject; }
               float4x4 GetObjectToWorldMatrix() { return unity_ObjectToWorld; }
               #if (defined(SHADER_API_D3D11) || defined(SHADER_API_XBOXONE) || defined(UNITY_COMPILER_HLSLCC) || defined(SHADER_API_PSSL) || (SHADER_TARGET_SURFACE_ANALYSIS && !SHADER_TARGET_SURFACE_ANALYSIS_MOJOSHADER))
                 #define UNITY_SAMPLE_TEX2D_LOD(tex,coord, lod) tex.SampleLevel (sampler##tex,coord, lod)
                 #define UNITY_SAMPLE_TEX2D_SAMPLER_LOD(tex,samplertex,coord, lod) tex.SampleLevel (sampler##samplertex,coord, lod)
              #else
                 #define UNITY_SAMPLE_TEX2D_LOD(tex,coord,lod) tex2D (tex,coord,0,lod)
                 #define UNITY_SAMPLE_TEX2D_SAMPLER_LOD(tex,samplertex,coord,lod) tex2D (tex,coord,0,lod)
              #endif

               #undef UNITY_MATRIX_I_M

               #define UNITY_MATRIX_I_M   unity_WorldToObject
            #endif

            float3 GetCameraWorldPosition()
            {
               #if _HDRP
                  return GetCameraRelativePositionWS(_WorldSpaceCameraPos);
               #else
                  return _WorldSpaceCameraPos;
               #endif
            }

            #if _GRABPASSUSED
               #if _STANDARD
                  TEXTURE2D(%GRABTEXTURE%);
                  SAMPLER(sampler_%GRABTEXTURE%);
               #endif

               half3 GetSceneColor(float2 uv)
               {
                  #if _STANDARD
                     return SAMPLE_TEXTURE2D(%GRABTEXTURE%, sampler_%GRABTEXTURE%, uv).rgb;
                  #else
                     return SHADERGRAPH_SAMPLE_SCENE_COLOR(uv);
                  #endif
               }
            #endif
            #if _STANDARD
               UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);
               float GetSceneDepth(float2 uv) { return SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, uv); }
               float GetLinear01Depth(float2 uv) { return Linear01Depth(GetSceneDepth(uv)); }
               float GetLinearEyeDepth(float2 uv) { return LinearEyeDepth(GetSceneDepth(uv)); } 
            #else
               float GetSceneDepth(float2 uv) { return SHADERGRAPH_SAMPLE_SCENE_DEPTH(uv); }
               float GetLinear01Depth(float2 uv) { return Linear01Depth(GetSceneDepth(uv), _ZBufferParams); }
               float GetLinearEyeDepth(float2 uv) { return LinearEyeDepth(GetSceneDepth(uv), _ZBufferParams); } 
            #endif

            float3 GetWorldPositionFromDepthBuffer(float2 uv, float3 worldSpaceViewDir)
            {
               float eye = GetLinearEyeDepth(uv);
               float3 camView = mul((float3x3)UNITY_MATRIX_M, transpose(mul(UNITY_MATRIX_I_M, UNITY_MATRIX_I_V)) [2].xyz);

               float dt = dot(worldSpaceViewDir, camView);
               float3 div = worldSpaceViewDir/dt;
               float3 wpos = (eye * div) + GetCameraWorldPosition();
               return wpos;
            }

            #if _HDRP
            float3 ObjectToWorldSpacePosition(float3 pos)
            {
               return GetAbsolutePositionWS(TransformObjectToWorld(pos));
            }
            #else
            float3 ObjectToWorldSpacePosition(float3 pos)
            {
               return TransformObjectToWorld(pos);
            }
            #endif

            #if _STANDARD
               UNITY_DECLARE_SCREENSPACE_TEXTURE(_CameraDepthNormalsTexture);
               float3 GetSceneNormal(float2 uv, float3 worldSpaceViewDir)
               {
                  float4 depthNorms = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_CameraDepthNormalsTexture, uv);
                  float3 norms = DecodeViewNormalStereo(depthNorms);
                  norms = mul((float3x3)UNITY_MATRIX_V, norms) * 0.5 + 0.5;
                  return norms;
               }
            #elif _HDRP && !_DECALSHADER
               float3 GetSceneNormal(float2 uv, float3 worldSpaceViewDir)
               {
                  NormalData nd;
                  DecodeFromNormalBuffer(_ScreenSize.xy * uv, nd);
                  return nd.normalWS;
               }
            #elif _URP
               #if (SHADER_LIBRARY_VERSION_MAJOR >= 10)
                  #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareNormalsTexture.hlsl"
               #endif

               float3 GetSceneNormal(float2 uv, float3 worldSpaceViewDir)
               {
                  #if (SHADER_LIBRARY_VERSION_MAJOR >= 10)
                     return SampleSceneNormals(uv);
                  #else
                     float3 wpos = GetWorldPositionFromDepthBuffer(uv, worldSpaceViewDir);
                     return normalize(-cross(ddx(wpos), ddy(wpos))) * 0.5 + 0.5;
                  #endif

                }
             #endif

             #if _HDRP

               half3 UnpackNormalmapRGorAG(half4 packednormal)
               {
                  packednormal.x *= packednormal.w;

                  half3 normal;
                  normal.xy = packednormal.xy * 2 - 1;
                  normal.z = sqrt(1 - saturate(dot(normal.xy, normal.xy)));
                  return normal;
               }
               half3 UnpackNormal(half4 packednormal)
               {
                  #if defined(UNITY_NO_DXT5nm)
                     return packednormal.xyz * 2 - 1;
                  #else
                     return UnpackNormalmapRGorAG(packednormal);
                  #endif
               }
            #endif
            #if _HDRP || _URP

               half3 UnpackScaleNormal(half4 packednormal, half scale)
               {
                 #ifndef UNITY_NO_DXT5nm
                   packednormal.x *= packednormal.w;
                 #endif
                   half3 normal;
                   normal.xy = (packednormal.xy * 2 - 1) * scale;
                   normal.z = sqrt(1 - saturate(dot(normal.xy, normal.xy)));
                   return normal;
               }	

             #endif
            void GetSun(out float3 lightDir, out float3 color)
            {
               lightDir = float3(0.5, 0.5, 0);
               color = 1;
               #if _HDRP
                  if (_DirectionalLightCount > 0)
                  {
                     DirectionalLightData light = _DirectionalLightDatas[0];
                     lightDir = -light.forward.xyz;
                     color = light.color;
                  }
               #elif _STANDARD
			         lightDir = normalize(_WorldSpaceLightPos0.xyz);
                  color = _LightColor0.rgb;
               #elif _URP
	               Light light = GetMainLight();
	               lightDir = light.direction;
	               color = light.color;
               #endif
            }
    half _ShadingMode;
    half _LightFunction;
    half _UseAlphaOnlyFromBaseMap;
    half _EnableToonShading;
    half _ShadingFunction;
    int _NumberOfCells;
	float _CellTransitionSmoothness;
	half _SumLightsBeforePosterization;
	half _ShadingUseLightColors;
    half _GradientMode;
    half _GradientBlending;
    float _GradientBlendFactor;

    float4 _GradientTex_TexelSize;
    half _ShadingAffectedByNormalMap;
    half _EnableShadows;
    float4 _CoreShadowColor;
    float _TerminatorPosition;
    float _TerminatorWidth;
    float _TerminatorSmoothness;
    float4 _FormShadowColor;

    half _EnableCastShadows;
    float _CastShadowsStrength;
    float _CastShadowsSmoothness;
    half _CastShadowColorMode;
    float4 _CastShadowColor;
    half _EnableSpecular;
    half _SpecularBlending;
    half4 _SpecularColor;
    float _SpecularSize;
    float _SpecularSmoothness;
    float _SpecularOpacity;
    half _SpecularAffectedByNormalMap;
    half _SpecularUseLightColors;
    half _EnableRim;
    half _RimBlending;
    float4 _RimColor;
    float _RimSize;
    float _RimSmoothness;
    float _RimOpacity;
    half _RimAffectedArea;
    half _RimAffectedByNormalMap;
     half _EnableStyling;

    half _EnableStylingDistanceFade;
    float _StylingDFStartingDistance;
    float _StylingDFFalloff;
    half _StylingAdjustDistanceFadeValue;
    float _StylingDistanceFadeValue;
    half _HatchingAffectedByNormalMap;
    half _EnableAntiAliasing;
    float4 _NoiseTex2_TexelSize;
    half _EnableShadingStyling;
    half _StylingShadingSyncWithOtherStyling;
    half _ShadingStyle;    
    half _SyncWithLightPartitioning;
    half _NumberOfCellsHatching;
    half _StylingTerminatorPosition;    
    float _StylingOvermodelingFactor;
    half _StylingShadingBlending;
    half _StylingShadingIsInverted;
    half _DrawSpace;
    half _SSCameraDistanceScaled;
    half _AnchorSSToObjectsOrigin;
    half _CoordinateSystem;
    half _PolarCenterMode;
    float4 _PolarCenter;
    float _StylingShadingDensity;
    float _StylingShadingInitialDirection;
    float _StylingShadingRotationBetweenCells;
    float _StylingShadingHalftonesOffset;
    float4 _StylingColor;
    float _StylingShadingOpacity;
    float _StylingShadingOpacityFalloff;
    float _StylingShadingThicknessControl;
    float _StylingShadingThickness;
    float _StylingShadingThicknessFalloff;
    float _StylingShadingHardness;
    float _StylingShadingHalftonesRoundness;
    float _StylingShadingHalftonesRoundnessFalloff;
    half _EnableShadingRandomizer;
    float _ShadingNoise1Size;
    float _ShadingNoise1Seed;
    float _ShadingNoise2Seed;
    float _NoiseIntensity;
    half _SpacingRandomMode;
    float _SpacingRandomIntensity;
    half _OpacityRandomMode;
    float _OpacityRandomIntensity;
    half _LengthRandomMode;
    float _LengthRandomIntensity;
    half _HardnessRandomMode;
    float _HardnessRandomIntensity;
    half _ThicknessRandomMode;
    float _ThicknesshRandomIntensity;
    half _EnableCastShadowsStyling;
    half _CastShadowsNumberOfCellsHatching;
    float _StylingCastShadowsSmoothness;
    half _StylingCastShadowsSyncWithOtherStyling;
    half _CastShadowsStyle;
    half _StylingCastShadowsBlending;
    half _StylingCastShadowsIsInverted;
    half _CastShadowsDrawSpace;
    half _CastShadowsSSCameraDistanceScaled;
    half _CastShadowsAnchorSSToObjectsOrigin;
    half _CastShadowsCoordinateSystem;
    half _CastShadowsPolarCenterMode;
    float4 _CastShadowsPolarCenter;
    float _StylingCastShadowsDensity;
    float _StylingCastShadowsInitialDirection;
    float _StylingCastShadowsRotationBetweenCells;
    float _StylingCastShadowsHalftonesOffset;
    float4 _StylingCastShadowsColor;
    float _StylingCastShadowsOpacity;
    float _StylingCastShadowsOpacityFalloff;
    half _StylingCastShadowsThicknessControl;
    float _StylingCastShadowsThickness;
    float _StylingCastShadowsThicknessFalloff;
    float _StylingCastShadowsHardness;
    float _StylingCastShadowsHalftonesRoundness;
    float _StylingCastShadowsHalftonesRoundnessFalloff;
    half _EnableCastShadowsRandomizer;
    float _CastShadowsNoise1Size;
    float _CastShadowsNoise1Seed;
    float _CastShadowsNoise2Seed;
    float _CastShadowsNoiseIntensity;
    half _CastShadowsSpacingRandomMode;
    float _CastShadowsSpacingRandomIntensity;
    half _CastShadowsOpacityRandomMode;
    float _CastShadowsOpacityRandomIntensity;
    half _CastShadowsHardnessRandomMode;
    float _CastShadowsHardnessRandomIntensity;
    half _CastShadowsLengthRandomMode;
    float _CastShadowsLengthRandomIntensity;
    half _CastShadowsThicknessRandomMode;
    float _CastShadowsThicknesshRandomIntensity;
    half _EnableSpecularStyling;
    half _SyncWithSpecular;
    float _StylingSpecularSize;
    float _StylingSpecularSmoothness;
    half _StylingSpecularCutOutShading;
    half _StylingSpecularUseLightColors;
    half _StylingSpecularSyncWithOtherStyling;
    half _SpecularStyle;
    half _StylingSpecularBlending;
    half _StylingSpecularIsInverted;
    half _SpecularDrawSpace;
    half _SpecularSSCameraDistanceScaled;
    half _SpecularAnchorSSToObjectsOrigin;
    half _SpecularCoordinateSystem;
    half _SpecularPolarCenterMode;
    float4 _SpecularPolarCenter;
    float _StylingSpecularDensity;    
    float _StylingSpecularRotation;
    float _StylingSpecularHalftonesOffset;
    float4 _StylingSpecularColor;
    float _StylingSpecularOpacity;
    float _StylingSpecularOpacityFalloff;
    float _StylingSpecularThicknessControl;
    float _StylingSpecularThickness;
    float _StylingSpecularThicknessFalloff;
    float _StylingSpecularHardness;
    float _StylingSpecularHalftonesRoundness;
    float _StylingSpecularHalftonesRoundnessFalloff;
    float _EnableSpecularRandomizer;    
    float _SpecularNoise1Size;
    float _SpecularNoise1Seed;
    float _SpecularNoise2Seed;
    float _SpecularNoiseIntensity;
    half _SpecularSpacingRandomMode;
    float _SpecularSpacingRandomIntensity;
    half _SpecularOpacityRandomMode;
    float _SpecularOpacityRandomIntensity;
    half _SpecularLengthRandomMode;
    float _SpecularLengthRandomIntensity;
    half _SpecularHardnessRandomMode;
    float _SpecularHardnessRandomIntensity;
    half _SpecularThicknessRandomMode;
    float _SpecularThicknesshRandomIntensity;
    half _EnableRimStyling;
    half _StylingRimSyncWithOtherStyling;
    half _SyncWithRim;
    float _StylingRimSize;
    float _StylingRimSmoothness;
    half _StylingRimAffectedArea;
    half _RimStyle;
    half _StylingRimBlending;
    half _StylingRimIsInverted;
    half _RimDrawSpace;
    half _RimSSCameraDistanceScaled;
    half _RimAnchorSSToObjectsOrigin;
    half _RimCoordinateSystem;
    half _RimPolarCenterMode;
    float4 _RimPolarCenter;  
    float _StylingRimDensity;    
    float _StylingRimRotation;
    float _StylingRimHalftonesOffset;
    float4 _StylingRimColor;
    float _StylingRimOpacity;
    float _StylingRimOpacityFalloff;
    float _StylingRimThicknessControl;
    float _StylingRimThickness;
    float _StylingRimThicknessFalloff;
    float _StylingRimHardness;
    float _StylingRimHalftonesRoundness;
    float _StylingRimHalftonesRoundnessFalloff;
    float _EnableRimRandomizer;        
    float _RimNoise1Size;
    float _RimNoise1Seed;
    float _RimNoise2Seed;
    float _RimNoiseIntensity;
    half _RimSpacingRandomMode;
    float _RimSpacingRandomIntensity;
    half _RimOpacityRandomMode; 
    float _RimOpacityRandomIntensity;
    half _RimLengthRandomMode;
    float _RimLengthRandomIntensity;
    half _RimHardnessRandomMode;
    float _RimHardnessRandomIntensity;
    half _RimThicknessRandomMode;
    float _RimThicknesshRandomIntensity;
    half4 _OutlineColor;
    half _OutlineWidth;
    half _EnableOutline;
    half4 _Color;
    half _Cutoff; 
    half _Mode;
    float _Cull;
    half _Metallic;
    half3 _EmissionColor;
    half _UVSec;

    half _BumpScale; 
    half _DetailNormalMapScale;
    float _Glossiness;
    float _GlossMapScale;
    half _OcclusionStrength;
    half _Parallax;

    float4 _MainTex_ST;
    float4 _DetailAlbedoMap_ST;
    sampler2D _NoiseMap1;
    sampler2D _NoiseMap2;

    sampler2D _HatchingMap;
    sampler2D _GradientTex;
    void Ext_ModifyVertex0 (inout VertexData v, inout ExtraV2F d)
    {
        float4x4 UnityObjectToClipPosMatrix = mul(UNITY_MATRIX_VP, GetObjectToWorldMatrix());
        float4 pos = mul(UnityObjectToClipPosMatrix, v.vertex);
        float4 objectOriginClipPos = mul(UnityObjectToClipPosMatrix, float4(0, 0, 0, 1));
        objectOriginClipPos.xy = clamp(objectOriginClipPos.xy, -1.0*objectOriginClipPos.w, 1.0*objectOriginClipPos.w);
        float4 originScreenPos = ComputeScreenPos(objectOriginClipPos);
        float2 originScreenUV = originScreenPos.xy/originScreenPos.w;
        float4 screenPos = ComputeScreenPos(pos);
        float2 uv = ((screenPos.xy) / screenPos.w);
        float4 ssss = ComputeScreenPos(pos);
       d.extraV2F0 = float4(originScreenUV,originScreenUV);
    }
    sampler2D _MainTex;
    sampler2D _DetailAlbedoMap;
    sampler2D _DetailMask;
    sampler2D _BumpMap;
    sampler2D _DetailNormalMap;
    sampler2D _MetallicGlossMap;
    sampler2D _OcclusionMap;
    sampler2D _ParallaxMap;
    sampler2D _EmissionMap;
	void Ext_SurfaceFunction1 (inout Surface o, ShaderData d)
	{
        float4 texcoords;
        texcoords.xy = d.texcoord0.xy * _MainTex_ST.xy + _MainTex_ST.zw; 
        texcoords.zw = (_UVSec == 0) ? d.texcoord0.xy * _DetailAlbedoMap_ST.xy + _DetailAlbedoMap_ST.zw : d.texcoord1.xy * _DetailAlbedoMap_ST.xy + _DetailAlbedoMap_ST.zw;
    #if _SHADING_COLOR
        half4 diffuseAlpha = tex2D(_MainTex, texcoords.xy);
        o.Albedo = diffuseAlpha.rgb * _Color.rgb;
        o.Normal = UnpackScaleNormal(tex2D(_BumpMap, texcoords.xy), _BumpScale);

		half alpha = diffuseAlpha.a;
        o.Alpha = alpha * _Color.a;
        if (_Mode) 
        {
            clip (alpha - _Cutoff);
        }

        #ifdef _EMISSION
            half3 emission = tex2D(_EmissionMap, texcoords.xy).rgb * _EmissionColor.rgb; 
            o.Emission = emission;
        #endif
    #else
        #ifdef _PARALLAXMAP
            half h = tex2D (_ParallaxMap, texcoords.xy).g;
            float2 offset = ParallaxOffset1Step (h, _Parallax, d.tangentSpaceViewDir);
            texcoords = float4(texcoords.xy + offset, texcoords.zw + offset);
            o.Normal = float3(0, 0, 1);
        #endif
        half3 albedo = tex2D(_MainTex, texcoords.xy).rgb * _Color.rgb; 
        #ifdef _DETAIL_MULX2
                half3 detailAlbedo = tex2D (_DetailAlbedoMap, texcoords.zw).rgb;
                albedo *= LerpWhiteTo (detailAlbedo * unity_ColorSpaceDouble.rgb, tex2D(_DetailMask, texcoords.xy).a);
        #endif
        o.Albedo = albedo;
        half2 metallicSmoothness = half2(0,0);
        #ifdef _METALLICGLOSSMAP
            half4 metallicGlossMap = tex2D(_MetallicGlossMap, texcoords.xy);
            metallicSmoothness.r = metallicGlossMap.r;
            #ifdef _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
                metallicSmoothness.g = tex2D(_MainTex, texcoords.xy).a;
            #else
                metallicSmoothness.g = metallicGlossMap.a;
            #endif
            metallicSmoothness.g *= _GlossMapScale;
        #else
            metallicSmoothness.r = _Metallic;
            #ifdef _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
                metallicSmoothness.g  = tex2D(_MainTex, texcoords.xy).a * _GlossMapScale;
            #else
                metallicSmoothness.g = _Glossiness;
            #endif
        #endif
        o.Metallic = metallicSmoothness.r;
        o.Smoothness = metallicSmoothness.g;
        o.Occlusion = lerp(1, tex2D(_OcclusionMap, texcoords.xy).g, _OcclusionStrength);
        half alpha = 0;
        #if defined(_SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A)
            alpha = _Color.a;
        #else
            alpha =  tex2D(_MainTex, texcoords.xy).a * _Color.a;
        #endif
        if (_Mode) 
        {
            clip (alpha - _Cutoff);
        }
        o.Alpha = alpha;
        #ifdef _NORMALMAP
            half3 normal = UnpackScaleNormal(tex2D(_BumpMap, texcoords.xy), _BumpScale);
            #ifdef _DETAIL_MULX2
                half3 detailNormal = UnpackScaleNormal(tex2D(_DetailNormalMap, texcoords.zw), _DetailNormalMapScale);
	            detailNormal = lerp(float3(0, 0, 1), detailNormal, tex2D(_DetailMask, texcoords.xy).a);
	            normal = BlendNormals(normal, detailNormal);
            #endif
            o.Normal = normal;
        #endif
        #ifdef _EMISSION
            o.Emission += tex2D(_EmissionMap, texcoords.xy).rgb * _EmissionColor.rgb; 
        #endif
    #endif
	}
            void ChainSurfaceFunction(inout Surface l, inout ShaderData d)
            {
                  Ext_SurfaceFunction1(l, d);
            }

#if !_DECALSHADER

            void ChainModifyVertex(inout VertexData v, inout VertexToPixel v2p, float4 time)
            {
                 ExtraV2F d;
                 ZERO_INITIALIZE(ExtraV2F, d);
                 ZERO_INITIALIZE(Blackboard, d.blackboard);
                 d.time = time;

                   Ext_ModifyVertex0(v, d);
                  v2p.extraV2F0 = d.extraV2F0;
            }

            void ChainModifyTessellatedVertex(inout VertexData v, inout VertexToPixel v2p)
            {
               ExtraV2F d;
               ZERO_INITIALIZE(ExtraV2F, d);
               ZERO_INITIALIZE(Blackboard, d.blackboard);
                d.extraV2F0 = v2p.extraV2F0;
                v2p.extraV2F0 = d.extraV2F0;
            }

            void ChainFinalColorForward(inout Surface l, inout ShaderData d, inout half4 color)
            {
            }

            void ChainFinalGBufferStandard(inout Surface s, inout ShaderData d, inout half4 GBuffer0, inout half4 GBuffer1, inout half4 GBuffer2, inout half4 outEmission, inout half4 outShadowMask)
            {
            }
#endif
#if _DECALSHADER

        ShaderData CreateShaderData(SurfaceDescriptionInputs IN)
        {
            ShaderData d = (ShaderData)0;
            d.TBNMatrix = float3x3(IN.WorldSpaceTangent, IN.WorldSpaceBiTangent, IN.WorldSpaceNormal);
            d.worldSpaceNormal = IN.WorldSpaceNormal;
            d.worldSpaceTangent = IN.WorldSpaceTangent;

            d.worldSpacePosition = IN.WorldSpacePosition;
            d.texcoord0 = IN.uv0.xyxy;
            d.screenPos = IN.ScreenPosition;

            d.worldSpaceViewDir = normalize(_WorldSpaceCameraPos - d.worldSpacePosition);

            d.tangentSpaceViewDir = mul(d.TBNMatrix, d.worldSpaceViewDir);
            #if _HDRP
            #else
            #endif
            return d;
        }
#else

         ShaderData CreateShaderData(VertexToPixel i
                  #if NEED_FACING
                     , bool facing
                  #endif
         )
         {
            ShaderData d = (ShaderData)0;
            d.clipPos = i.pos;
            d.worldSpacePosition = i.worldPos;

            d.worldSpaceNormal = normalize(i.worldNormal);
            d.worldSpaceTangent.xyz = normalize(i.worldTangent.xyz);

            d.tangentSign = i.worldTangent.w * unity_WorldTransformParams.w;
            float3 bitangent = cross(d.worldSpaceTangent.xyz, d.worldSpaceNormal) * d.tangentSign;
            d.TBNMatrix = float3x3(d.worldSpaceTangent, -bitangent, d.worldSpaceNormal);
            d.worldSpaceViewDir = normalize(_WorldSpaceCameraPos - i.worldPos);

            d.tangentSpaceViewDir = mul(d.TBNMatrix, d.worldSpaceViewDir);
             d.texcoord0 = i.texcoord0;
             d.texcoord1 = i.texcoord1;
            #if _HDRP
            #else
            #endif
             d.extraV2F0 = i.extraV2F0;
            return d;
         }

#endif
         VertexToPixel Vert (VertexData v)
         {
            UNITY_SETUP_INSTANCE_ID(v);
            VertexToPixel o;
            UNITY_INITIALIZE_OUTPUT(VertexToPixel,o);
            UNITY_TRANSFER_INSTANCE_ID(v,o);
            UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

#if !_TESSELLATION_ON
           ChainModifyVertex(v, o, _Time);
#endif
            o.pos = UnityMetaVertexPosition(v.vertex, v.texcoord1.xy, v.texcoord2.xy, unity_LightmapST, unity_DynamicLightmapST);
            #ifdef EDITOR_VISUALIZATION
               o.vizUV = 0;
               o.lightCoord = 0;
               if (unity_VisualizationMode == EDITORVIZ_TEXTURE)
                  o.vizUV = UnityMetaVizUV(unity_EditorViz_UVIndex, v.texcoord0.xy, v.texcoord1.xy, v.texcoord2.xy, unity_EditorViz_Texture_ST);
               else if (unity_VisualizationMode == EDITORVIZ_SHOWLIGHTMASK)
               {
                  o.vizUV = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
                  o.lightCoord = mul(unity_EditorViz_WorldToLight, mul(GetObjectToWorldMatrix(), float4(v.vertex.xyz, 1)));
               }
            #endif
             o.texcoord0 = v.texcoord0;
             o.texcoord1 = v.texcoord1;
            o.worldPos = mul(GetObjectToWorldMatrix(), v.vertex).xyz;
            o.worldNormal = UnityObjectToWorldNormal(v.normal);
            o.worldTangent = float4(UnityObjectToWorldDir(v.tangent.xyz), v.tangent.w);

            return o;
         }
         fixed4 Frag (VertexToPixel IN
         #if NEED_FACING
            , bool facing : SV_IsFrontFace
         #endif
         ) : SV_Target
         {
            UNITY_SETUP_INSTANCE_ID(IN);

            #ifdef FOG_COMBINED_WITH_TSPACE
               UNITY_EXTRACT_FOG_FROM_TSPACE(IN);
            #elif defined (FOG_COMBINED_WITH_WORLD_POS)
               UNITY_EXTRACT_FOG_FROM_WORLD_POS(IN);
            #else
               UNITY_EXTRACT_FOG(IN);
            #endif

            ShaderData d = CreateShaderData(IN
               #if NEED_FACING
                 , facing
              #endif
            );

            Surface l = (Surface)0;

            l.Albedo = half3(0.5, 0.5, 0.5);
            l.Normal = float3(0,0,1);
            l.Occlusion = 1;
            l.Alpha = 1;
            ChainSurfaceFunction(l, d);

            UnityMetaInput metaIN;
            UNITY_INITIALIZE_OUTPUT(UnityMetaInput, metaIN);
            metaIN.Albedo = l.Albedo;
            metaIN.Emission = l.Emission;
            #if _USESPECULAR
               metaIN.SpecularColor = l.Specular;
            #endif

            #ifdef EDITOR_VISUALIZATION
              metaIN.VizUV = IN.vizUV;
              metaIN.LightCoord = IN.lightCoord;
            #endif
            return UnityMetaFragment(metaIN);
         }
         ENDCG

      }
Pass
{
    Name"Outline"
    Cull Front

    CGPROGRAM

    #pragma vertex vert
    #pragma fragment frag

    #include "UnityCG.cginc"

CBUFFER_START(UnityPerMaterial)
    half4 _Color;
    half _Cutoff; 
    half _Mode;
    float _Cull;
    half _Metallic;
    half3 _EmissionColor;
    half _UVSec;

    half _BumpScale; 
    half _DetailNormalMapScale;
    float _Glossiness;
    float _GlossMapScale;
    half _OcclusionStrength;
    half _Parallax;

    float4 _MainTex_ST;
    float4 _DetailAlbedoMap_ST;
    half _ShadingMode;
    half _LightFunction;

    half _UseAlphaOnlyFromBaseMap;
    half _EnableToonShading;
    half _ShadingFunction;
    int _NumberOfCells;
	float _CellTransitionSmoothness;
    half _SumLightsBeforePosterization;
	half _ShadingUseLightColors;
    half _GradientMode;
    half _GradientBlending;
    float _GradientBlendFactor;

    float4 _GradientTex_TexelSize;
    half _ShadingAffectedByNormalMap;
    half _EnableShadows;
    float4 _CoreShadowColor;
    float _TerminatorWidth;
    float _TerminatorSmoothness;
    float4 _FormShadowColor;

    half _EnableCastShadows;
    float _CastShadowsStrength;
    float _CastShadowsSmoothness;
    half _CastShadowColorMode;
    float4 _CastShadowColor;
    half _EnableSpecular;
    half _SpecularBlending;
    half4 _SpecularColor;
    float _SpecularSize;
    float _SpecularSmoothness;
    float _SpecularOpacity;
    half _SpecularAffectedByNormalMap;
    half _SpecularUseLightColors;
    half _EnableRim;
    half _RimBlending;
    float4 _RimColor;
    float _RimSize;
    float _RimSmoothness;
    float _RimOpacity;
    half _RimAffectedArea;
    half _RimAffectedByNormalMap;
     half _EnableStyling;

    half _EnableStylingDistanceFade;
    float _StylingDFStartingDistance;
    float _StylingDFFalloff;
    half _StylingAdjustDistanceFadeValue;
    float _StylingDistanceFadeValue;
    half _HatchingAffectedByNormalMap;
    half _EnableAntiAliasing;
    float4 _NoiseTex2_TexelSize;
    half _EnableShadingStyling;
    half _StylingShadingSyncWithOtherStyling;
    half _ShadingStyle;    
    half _SyncWithLightPartitioning;
    half _NumberOfCellsHatching;
    float _StylingOvermodelingFactor;
    half _StylingShadingBlending;
    half _StylingShadingIsInverted;
    half _DrawSpace;
    half _SSCameraDistanceScaled;
    half _AnchorSSToObjectsOrigin;
    half _CoordinateSystem;
    half _PolarCenterMode;
    float4 _PolarCenter;
    float _StylingShadingDensity;
    float _StylingShadingInitialDirection;
    float _StylingShadingRotationBetweenCells;
    float _StylingShadingHalftonesOffset;
    float4 _StylingColor;
    float _StylingShadingOpacity;
    float _StylingShadingOpacityFalloff;
    float _StylingShadingThicknessControl;
    float _StylingShadingThickness;
    float _StylingShadingThicknessFalloff;
    float _StylingShadingHardness;
    float _StylingShadingHalftonesRoundness;
    float _StylingShadingHalftonesRoundnessFalloff;
    half _EnableShadingRandomizer;
    float _ShadingNoise1Size;
    float _ShadingNoise1Seed;
    float _ShadingNoise2Seed;
    float _NoiseIntensity;
    half _SpacingRandomMode;
    float _SpacingRandomIntensity;
    half _OpacityRandomMode;
    float _OpacityRandomIntensity;
    half _LengthRandomMode;
    float _LengthRandomIntensity;
    half _HardnessRandomMode;
    float _HardnessRandomIntensity;
    half _ThicknessRandomMode;
    float _ThicknesshRandomIntensity;
    half _EnableCastShadowsStyling;
    half _CastShadowsNumberOfCellsHatching;
    float _StylingCastShadowsSmoothness;
    half _StylingCastShadowsSyncWithOtherStyling;
    half _CastShadowsStyle;
    half _StylingCastShadowsBlending;
    half _StylingCastShadowsIsInverted;
    half _CastShadowsDrawSpace;
    half _CastShadowsSSCameraDistanceScaled;
    half _CastShadowsAnchorSSToObjectsOrigin;
    half _CastShadowsCoordinateSystem;
    half _CastShadowsPolarCenterMode;
    float4 _CastShadowsPolarCenter;
    float _StylingCastShadowsDensity;
    float _StylingCastShadowsInitialDirection;
    float _StylingCastShadowsRotationBetweenCells;
    float _StylingCastShadowsHalftonesOffset;
    float4 _StylingCastShadowsColor;
    float _StylingCastShadowsOpacity;
    float _StylingCastShadowsOpacityFalloff;
    half _StylingCastShadowsThicknessControl;
    float _StylingCastShadowsThickness;
    float _StylingCastShadowsThicknessFalloff;
    float _StylingCastShadowsHardness;
    float _StylingCastShadowsHalftonesRoundness;
    float _StylingCastShadowsHalftonesRoundnessFalloff;
    half _EnableCastShadowsRandomizer;
    float _CastShadowsNoise1Size;
    float _CastShadowsNoise1Seed;
    float _CastShadowsNoise2Seed;
    float _CastShadowsNoiseIntensity;
    half _CastShadowsSpacingRandomMode;
    float _CastShadowsSpacingRandomIntensity;
    half _CastShadowsOpacityRandomMode;
    float _CastShadowsOpacityRandomIntensity;
    half _CastShadowsHardnessRandomMode;
    float _CastShadowsHardnessRandomIntensity;
    half _CastShadowsLengthRandomMode;
    float _CastShadowsLengthRandomIntensity;
    half _CastShadowsThicknessRandomMode;
    float _CastShadowsThicknesshRandomIntensity;
    half _EnableSpecularStyling;
    half _SyncWithSpecular;
    float _StylingSpecularSize;
    float _StylingSpecularSmoothness;
    half _StylingSpecularCutOutShading;
    half _StylingSpecularUseLightColors;
    half _StylingSpecularSyncWithOtherStyling;
    half _SpecularStyle;
    half _StylingSpecularBlending;
    half _StylingSpecularIsInverted;
    half _SpecularDrawSpace;
    half _SpecularSSCameraDistanceScaled;
    half _SpecularAnchorSSToObjectsOrigin;
    half _SpecularCoordinateSystem;
    half _SpecularPolarCenterMode;
    float4 _SpecularPolarCenter;
    float _StylingSpecularDensity;    
    float _StylingSpecularRotation;
    float _StylingSpecularHalftonesOffset;
    float4 _StylingSpecularColor;
    float _StylingSpecularOpacity;
    float _StylingSpecularOpacityFalloff;
    float _StylingSpecularThicknessControl;
    float _StylingSpecularThickness;
    float _StylingSpecularThicknessFalloff;
    float _StylingSpecularHardness;
    float _StylingSpecularHalftonesRoundness;
    float _StylingSpecularHalftonesRoundnessFalloff;
    float _EnableSpecularRandomizer;    
    float _SpecularNoise1Size;
    float _SpecularNoise1Seed;
    float _SpecularNoise2Seed;
    float _SpecularNoiseIntensity;
    half _SpecularSpacingRandomMode;
    float _SpecularSpacingRandomIntensity;
    half _SpecularOpacityRandomMode;
    float _SpecularOpacityRandomIntensity;
    half _SpecularLengthRandomMode;
    float _SpecularLengthRandomIntensity;
    half _SpecularHardnessRandomMode;
    float _SpecularHardnessRandomIntensity;
    half _SpecularThicknessRandomMode;
    float _SpecularThicknesshRandomIntensity;
    half _EnableRimStyling;
    half _StylingRimSyncWithOtherStyling;
    half _SyncWithRim;
    float _StylingRimSize;
    float _StylingRimSmoothness;
    half _StylingRimAffectedArea;
    half _RimStyle;
    half _StylingRimBlending;
    half _StylingRimIsInverted;
    half _RimDrawSpace;
    half _RimSSCameraDistanceScaled;
    half _RimAnchorSSToObjectsOrigin;
    half _RimCoordinateSystem;
    half _RimPolarCenterMode;
    float4 _RimPolarCenter;  
    float _StylingRimDensity;    
    float _StylingRimRotation;
    float _StylingRimHalftonesOffset;
    float4 _StylingRimColor;
    float _StylingRimOpacity;
    float _StylingRimOpacityFalloff;
    float _StylingRimThicknessControl;
    float _StylingRimThickness;
    float _StylingRimThicknessFalloff;
    float _StylingRimHardness;
    float _StylingRimHalftonesRoundness;
    float _StylingRimHalftonesRoundnessFalloff;
    float _EnableRimRandomizer;        
    float _RimNoise1Size;
    float _RimNoise1Seed;
    float _RimNoise2Seed;
    float _RimNoiseIntensity;
    half _RimSpacingRandomMode;
    float _RimSpacingRandomIntensity;
    half _RimOpacityRandomMode; 
    float _RimOpacityRandomIntensity;
    half _RimLengthRandomMode;
    float _RimLengthRandomIntensity;
    half _RimHardnessRandomMode;
    float _RimHardnessRandomIntensity;
    half _RimThicknessRandomMode;
    float _RimThicknesshRandomIntensity;
    half4 _OutlineColor;
    half _OutlineWidth;
    half _EnableOutline;
CBUFFER_END
struct v2f
{
    float4 pos : POSITION;
};

struct vdata
{
    float4 vertex : POSITION;
    float3 normal : NORMAL;
};
v2f vert(vdata v)
{
    v2f o;
    float4 clipPos = UnityObjectToClipPos(v.vertex);
    if (_EnableOutline == 1)
    {

        float3 clipNormal = mul((float3x3) UNITY_MATRIX_VP, mul((float3x3) UNITY_MATRIX_M, v.normal));
        float2 offset = normalize(clipNormal.xy) / _ScreenParams.xy * _OutlineWidth * clipPos.w * 2;
        clipPos.xy += offset;

        o.pos = clipPos;
    }
    else
    {
        o.pos = clipPos;

    }
    return o;
}

half4 frag(v2f i) : COLOR
{
    if (_EnableOutline == 0)
    {
        clip(-1);
        return 1;
    }
    else
    {
        return _OutlineColor;

    }
}
    ENDCG        

}
   }
   CustomEditor "ShaderCrew.TheToonShader.TheToonShaderGUIEditor"
}
