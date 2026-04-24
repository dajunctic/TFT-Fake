Shader "TheToonShader/URP/2022/TheToonShader_WithSTS"
{
   Properties
   {
      [HideInInspector]_QueueOffset("_QueueOffset", Float) = 0
      [HideInInspector]_QueueControl("_QueueControl", Float) = -1
      [HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
      [HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
      [HideInInspector][NoScaleOffset]unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
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
    _ShadingMode("Shading Mode", Float) = 0.0 
    _LightFunction("Light Function", Float) = 1.0 
    [MainTexture] _BaseMap("Base Map", 2D) = "white" {}
    [MainColor] _BaseColor("Base Color", Color) = (1,1,1,1)

    [MaterialToggle] _UseAlphaOnlyFromBaseMap("Use Alpha Only From BaseMap", Float) = 0.0
     _Cutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0.5

    _BumpScale("Normal Scale", Float) = 1.0
    _BumpMap("Normal Map", 2D) = "bump" {}
    [HDR] _EmissionColor("Emission Color", Color) = (0,0,0)
    _EmissionMap("Emission Map", 2D) = "white" {}
    _Surface("__surface", Float) = 0.0
    _Blend("__blend", Float) = 0.0 
    _Cull("__cull", Float) = 2.0
    [ToggleUI] _AlphaClip("__clip", Float) = 0.0

    [ToggleUI] _ReceiveShadows("Receive Shadows", Float) = 1.0
    [ToggleOff] _SpecularHighlights("Specular Highlights", Float) = 1.0
    _SpecColor("Specular Color", Color) = (0.5, 0.5, 0.5, 0.5)
    _SpecGlossMap("Specular Map", 2D) = "white" {}
    _Smoothness("Smoothness", Range(0.0, 1.0)) = 0.5
    [Enum(Specular Alpha,0,Albedo Alpha,1)] _SmoothnessSource("Smoothness Source", Float) = 0.0
    _WorkflowMode("WorkflowMode", Float) = 1.0
    _SmoothnessTextureChannel("Smoothness texture channel", Float) = 0

    _Metallic("Metallic", Range(0.0, 1.0)) = 0.0
    _MetallicGlossMap("Metallic", 2D) = "white" {}

    [ToggleOff] _EnvironmentReflections("Environment Reflections", Float) = 1.0

    _Parallax("Height Scale", Range(0.005, 0.08)) = 0.005
    _ParallaxMap("Height Map", 2D) = "black" {}

    _OcclusionStrength("Strength", Range(0.0, 1.0)) = 1.0
    _OcclusionMap("Occlusion", 2D) = "white" {}

    _DetailMask("Detail Mask", 2D) = "white" {}
    _DetailAlbedoMapScale("Detail Albedo Scale", Range(0.0, 2.0)) = 1.0
    _DetailAlbedoMap("Detail Albedo Map", 2D) = "linearGrey" {}
    _DetailNormalMapScale("Detail Normal Scale", Range(0.0, 2.0)) = 1.0
    [Normal] _DetailNormalMap("Detail Normal Map", 2D) = "bump" {}
	_DissolveColor("Dissolve Color", Color) = (1,1,1,1)
	_DissolveColorSaturation("Dissolve Color Saturation", Range(0,1)) = 1.0
	_DissolveEmission("Dissolve Emission", Range(0,1)) = 1.0
	[AbsoluteValue()] _DissolveEmissionBooster("Dissolve Emission Booster", float) = 1
	_DissolveTex("Dissolve Effect Texture", 2D) = "white" {}

	_DissolveMethod ("Dissolve Method", Float) = 0
	_DissolveTexSpace ("Dissolve Tex Space", Float) = 0
    [MaterialToggle] _CrossSectionEnabled("Cross-Section Enabled", float) = 0.0
    _CrossSectionColor("Cross-Section Color", Color) = (1,0,0,1)

    [MaterialToggle] _CrossSectionTextureEnabled("Cross-Section Texture Enabled", float) = 0.0
    _CrossSectionTexture("Cross-Section Texture", 2D) = "white" {}
    _CrossSectionTextureScale ("Cross-Section Texture Scale", Float) = 1.0
    [MaterialToggle] _CrossSectionUVScaledByDistance("Scale UV by Camera Distance", Float) = 1.0
	[Enum(STSInteractionMode)] _InteractionMode ("Interaction Mode", Float) = 0
	[Enum(ObstructionMode)] _Obstruction ("Obstruction Mode", Float) = 0

	_ObstructionPlayerOffset("Player Offset", Float) = 0.0
	_AngleStrength("Angle Obstruction Strength", Range(0,1)) = 1.0
	_ConeStrength ("Cone Obstruction Strength", Range(0,1)) = 1.0
	_ConeObstructionDestroyRadius ("Cone Obstruction Destroy Radius", float) = 10.0
	_CylinderStrength ("Cylinder Obstruction Strength", Range(0,1)) = 1.0
	_CylinderObstructionDestroyRadius ("Cylinder Obstruction Destroy Radius", float) = 10.0

	_CircleStrength ("Circle Obstruction Strength", Range(0,1)) = 1.0
	_CircleObstructionDestroyRadius ("Circle Obstruction Destroy Radius", float) = 10.0

	_CurveStrength ("Curve Obstruction Strength", Range(0,1)) = 1.0
	_CurveObstructionDestroyRadius ("Curve Obstruction Destroy Radius", float) = 10.0
	[HideInInspector] _ObstructionCurve("Obstruction Curve", 2D) = "white" {}

	_DissolveFallOff("Dissolve FallOff", Range(0,1)) = 0.0

	_DissolveMask("Dissolve Mask", 2D) = "white" {}
	_DissolveMaskEnabled("Use DissolveMask", float) = 0.0

    _AffectedAreaPlayerBasedObstruction("_AffectedAreaPlayerBasedObstruction",  float) = 0.0

	_IntrinsicDissolveStrength("Intrinsic Dissolve Strength", Range(0,1)) = 0.0

	[MaterialToggle] _PreviewMode("Preview Mode", float) = 0.0
	_PreviewIndicatorLineThickness("Indicator Line Thickness",  Range(0.01,0.5)) = 0.04        

	[AbsoluteValue()] _UVs ("Dissolve Texture Scale", float) = 1.0
	[MaterialToggle] _hasClippedShadows("Has Clipped Shadows", Float) = 0
	[MaterialToggle] _Floor ("Floor", float) = 0.0
	[Enum(FloorMode)] _FloorMode ("Floor Mode", Float) = 0
	_FloorY ("FloorY",  float) = 1.0
	_PlayerPosYOffset ("PlayerPos Y Offset", float) = 1.0  
    _AffectedAreaFloor("_AffectedAreaFloor",  float) = 0.0

	[AbsoluteValue()] _FloorYTextureGradientLength ("FloorY Texture Gradient Length", float) = 0.1  

	[MaterialToggle] _AnimationEnabled("Animation Enabled", Float) = 1
	_AnimationSpeed("Animation Speed", Range(0,2)) = 1

	[MaterialToggle] _IsReplacementShader ("hidden: _IsReplacementShader", Float) = 0

	[HideInInspector] _RaycastMode ("hidden: _RaycastMode", Float) = 0
	[HideInInspector] _TriggerMode ("hidden: _TriggerMode", Float) = 0

	[HideInInspector] _IsExempt ("_IsExempt", Float) = 0

	[AbsoluteValue()] _TransitionDuration ("Transition Duration In Seconds", Float) = 2

	[AbsoluteValue()] _DefaultEffectRadius ("Default Effect Radius",Float) = 25    
    [MaterialToggle] _EnableDefaultEffectRadius("Enable Default Effect Radius", float) = 0.0

	[HideInInspector] _numOfPlayersInside ("hidden: _numOfPlayersInside", Float) = 0
	[HideInInspector] _tValue ("hidden: _tValue", Float) = 0
	[HideInInspector] _tDirection ("hidden: _tDirection", Float) = 0
	[HideInInspector] _id ("hidden: _id", Float) = 0

	[MaterialToggle] _TexturedEmissionEdge("Textured Emission Edge", float) = 1.0
	_TexturedEmissionEdgeStrength("Textured Emission Edge Strength", Range(0,1)) = 0.3

	[MaterialToggle] _IsometricExclusion("Isometric Exclusion", float) = 0.0
	_IsometricExclusionDistance("Isometric Exclusion Distance", float) = 0.0
	_IsometricExclusionGradientLength("Isometric Exclusion Gradient Length", float) = 0.1

	[MaterialToggle] _Ceiling ("Ceiling", float) = 0.0

	[Enum(CeilingMode)] _CeilingMode ("Ceiling Mode", Float) = 0
	[Enum(CeilingBlendMode)] _CeilingBlendMode ("Blending Mode", Float) = 1.0
	_CeilingY ("CeilingY",  float) = 1.0
	_CeilingPlayerYOffset ("PlayerPos Y Offset", float) = 1.0  
	_CeilingYGradientLength ("CeilingY Gradient Length", float) = 0.1

	[MaterialToggle] _Zoning("Zoning", float) = 0.0
	[Enum(ZoningMode)] _ZoningMode("Zoning Mode", Float) = 0.0
	_ZoningEdgeGradientLength ("Edge Gradient Length", float) = 0.1
	[MaterialToggle] _IsZoningRevealable ("Is Zoning Revealable", float) = 0.0
	[MaterialToggle] _SyncZonesWithFloorY ("Sync Zones With FloorY", float) = 0.0
	_SyncZonesFloorYOffset ("Sync Zones Floor YOffset", float) = 0.0

    [MaterialToggle] _UseCustomTime ("_UseCustomTime", float) = 0.0
	[MaterialToggle] _isReferenceMaterial("Is Reference Material", float) = 0.0
    [HideInInspector] _ShowContentDissolveArea ("hidden: _ShowContentDissolveArea", Float) = 1
    [HideInInspector] _ShowContentInteractionOptionsArea ("hidden: _ShowContentInteractionOptionsArea", Float) = 1
    [HideInInspector] _ShowContentObstructionOptionsArea ("hidden: _ShowContentObstructionOptionsArea", Float) = 1
    [HideInInspector] _ShowContentAnimationArea ("hidden: _ShowContentAnimationArea", Float) = 1
    [HideInInspector] _ShowContentZoningArea ("hidden: _ShowContentZoningArea", Float) = 1
    [HideInInspector] _ShowContentReplacementOptionsArea ("hidden: _ShowContentReplacementOptionsArea", Float) = 1
    [HideInInspector] _ShowContentDebugArea ("hidden: _ShowContentDebugArea", Float) = 1
    [MaterialToggle] _SyncCullMode ("_SyncCullMode", Float) = 0
   }
   SubShader
   {
      Tags { "RenderPipeline"="UniversalPipeline" "RenderType" = "Opaque" "UniversalMaterialType" = "Lit" "Queue" = "Geometry" }
        Pass
        {
            Name "Universal Forward"
            Tags 
            { 
                "LightMode" = "UniversalForwardOnly"
            }
            Cull Back
            Blend One Zero
            ZTest LEqual
            ZWrite On

            Blend One Zero, One Zero
Cull Back
ZTest LEqual
ZWrite On

                Cull [_Cull]
            HLSLPROGRAM

               #pragma vertex Vert
   #pragma fragment Frag

            #pragma target 3.0

            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma multi_compile_fog
            #pragma multi_compile_instancing
            #pragma instancing_options renderinglayer
            #pragma multi_compile _ DOTS_INSTANCING_ON
            #pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION
            #pragma multi_compile _ LIGHTMAP_ON
            #pragma multi_compile _ DYNAMICLIGHTMAP_ON
            #pragma multi_compile _ DIRLIGHTMAP_COMBINED
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
            #pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma multi_compile_fragment _ _REFLECTION_PROBE_BLENDING
            #pragma multi_compile_fragment _ _REFLECTION_PROBE_BOX_PROJECTION
            #pragma multi_compile_fragment _ _SHADOWS_SOFT
            #pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
            #pragma multi_compile _ SHADOWS_SHADOWMASK
            #pragma multi_compile_fragment _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
            #pragma multi_compile_fragment _ _LIGHT_LAYERS
            #pragma multi_compile_fragment _ DEBUG_DISPLAY
            #pragma multi_compile_fragment _ _LIGHT_COOKIES
            #pragma multi_compile_fragment _ _WRITE_RENDERING_LAYERS
            #pragma multi_compile _ _FORWARD_PLUS
            #pragma multi_compile_fragment _ LOD_FADE_CROSSFADE
            #define SHADER_PASS SHADERPASS_FORWARD
            #define VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
            #define _PASSFORWARD 1
            #define _FOG_FRAGMENT 1
     #pragma shader_feature_local_fragment  _SHADING_COLOR _SHADING_BLINNPHONG _SHADING_PBR    
    #pragma shader_feature_local _NORMALMAP
    #pragma shader_feature_local_fragment _EMISSION

    #pragma shader_feature_local_fragment _ALPHATEST_ON
    #pragma shader_feature_local _RECEIVE_SHADOWS_OFF
    #pragma shader_feature_local_fragment _ _SPECGLOSSMAP _SPECULAR_COLOR 
    #pragma shader_feature_local_fragment _GLOSSINESS_FROM_BASE_ALPHA

    #pragma shader_feature_local_fragment _OCCLUSIONMAP
    #pragma shader_feature_local _PARALLAXMAP
    #pragma shader_feature_local _ _DETAIL_MULX2 _DETAIL_SCALED
    #pragma shader_feature_local_fragment _SURFACE_TYPE_TRANSPARENT

    #pragma shader_feature_local_fragment _ _ALPHAPREMULTIPLY_ON _ALPHAMODULATE_ON

    #pragma shader_feature_local_fragment _METALLICSPECGLOSSMAP
    #pragma shader_feature_local_fragment _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
    #pragma shader_feature_local_fragment _SPECULARHIGHLIGHTS_OFF
    #pragma shader_feature_local_fragment _ENVIRONMENTREFLECTIONS_OFF
    #pragma shader_feature_local_fragment _SPECULAR_SETUP
        #pragma shader_feature_local_fragment _OBSTRUCTION_CURVE
        #pragma shader_feature_local_fragment _DISSOLVEMASK
        #pragma shader_feature_local_fragment _ZONING
        #pragma shader_feature_local_fragment _REPLACEMENT
        #pragma shader_feature_local_fragment _PLAYERINDEPENDENT
   #define _URP 1
#define NEED_FACING 1
            #if _SIMPLELIT
               #define _SPECULAR_COLOR
            #endif
            #include "Packages/com.shadercrew.the-toon-shader.core/Scripts/Shaders/xxSharedTTSDependecies/TheToonShaderLightingFunctions.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.shadercrew.the-toon-shader.3d/Scripts/Shaders/xxSharedTTSDependecies/ModifiedLightingFunctions/URP/LightingModifiedForToon2022.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DBuffer.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/LODCrossFade.hlsl"
               #undef WorldNormalVector
      #define WorldNormalVector(data, normal) mul(normal, data.TBNMatrix)
      #define UnityObjectToWorldNormal(normal) mul(GetObjectToWorldMatrix(), normal)

      #define _WorldSpaceLightPos0 _MainLightPosition
      #define UNITY_DECLARE_TEX2D(name) TEXTURE2D(name); SAMPLER(sampler##name);
      #define UNITY_DECLARE_TEX2D_NOSAMPLER(name) TEXTURE2D(name);
      #define UNITY_DECLARE_TEX2DARRAY(name) TEXTURE2D_ARRAY(name); SAMPLER(sampler##name);
      #define UNITY_DECLARE_TEX2DARRAY_NOSAMPLER(name) TEXTURE2D_ARRAY(name);

      #define UNITY_SAMPLE_TEX2DARRAY(tex,coord)            SAMPLE_TEXTURE2D_ARRAY(tex, sampler##tex, coord.xy, coord.z)
      #define UNITY_SAMPLE_TEX2DARRAY_LOD(tex,coord,lod)    SAMPLE_TEXTURE2D_ARRAY_LOD(tex, sampler##tex, coord.xy, coord.z, lod)
      #define UNITY_SAMPLE_TEX2D(tex, coord)                SAMPLE_TEXTURE2D(tex, sampler##tex, coord)
      #define UNITY_SAMPLE_TEX2D_SAMPLER(tex, samp, coord)  SAMPLE_TEXTURE2D(tex, sampler##samp, coord)

      #define UNITY_SAMPLE_TEX2D_LOD(tex,coord, lod)   SAMPLE_TEXTURE2D_LOD(tex, sampler_##tex, coord, lod)
      #define UNITY_SAMPLE_TEX2D_SAMPLER_LOD(tex,samplertex,coord, lod) SAMPLE_TEXTURE2D_LOD (tex, sampler##samplertex,coord, lod)
      #if defined(UNITY_COMPILER_HLSL)
         #define UNITY_INITIALIZE_OUTPUT(type,name) name = (type)0;
      #else
         #define UNITY_INITIALIZE_OUTPUT(type,name)
      #endif

      #define sampler2D_float sampler2D
      #define sampler2D_half sampler2D
      struct VertexToPixel
      {
         float4 pos : SV_POSITION;
         float3 worldPos : TEXCOORD0;
         float3 worldNormal : TEXCOORD1;
         float4 worldTangent : TEXCOORD2;
          float4 texcoord0 : TEXCOORD3;
          float4 screenPos : TEXCOORD7;
         #if defined(LIGHTMAP_ON)
            float2 lightmapUV : TEXCOORD8;
         #endif
         #if defined(DYNAMICLIGHTMAP_ON)
            float2 dynamicLightmapUV : TEXCOORD9;
         #endif
         #if !defined(LIGHTMAP_ON)
            float3 sh : TEXCOORD10;
         #endif

         #if defined(VARYINGS_NEED_FOG_AND_VERTEX_LIGHT)
            float4 fogFactorAndVertexLight : TEXCOORD11;
         #endif

         #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
           float4 shadowCoord : TEXCOORD12;
         #endif
          float4 extraV2F0 : TEXCOORD13;
         #if UNITY_ANY_INSTANCING_ENABLED
         uint instanceID : CUSTOM_INSTANCE_ID;
         #endif
         #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
         uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
         #endif
         #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
         uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
         #endif
         #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
         FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
         #endif
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

               #undef GetWorldToObjectMatrix()

               #define GetWorldToObjectMatrix()   unity_WorldToObject
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
               float3 camView = mul((float3x3)GetObjectToWorldMatrix(), transpose(mul(GetWorldToObjectMatrix(), UNITY_MATRIX_I_V)) [2].xyz);

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
                  norms = mul((float3x3)GetWorldToViewMatrix(), norms) * 0.5 + 0.5;
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
         CBUFFER_START(UnityPerMaterial)
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
	half4 _BaseColor;
    float _BumpScale;
    float4 _BaseMap_ST;
    half _Cutoff;
    half4 _EmissionColor;
     half4 _SpecColor;
     float _Smoothness;
	float4 _DetailAlbedoMap_ST;
	half _Metallic;
	half _Parallax;
	half _OcclusionStrength;
	half _DetailAlbedoMapScale;
	half _DetailNormalMapScale;
#if UNITY_VERSION < 202200
	float _AlphaToMaskAvailable;
#endif
    float _SyncCullMode;

    float _IsReplacementShader;
    float _TriggerMode;
    float _RaycastMode;
    float _IsExempt;
    float _isReferenceMaterial;
    float _InteractionMode;

    float _tDirection = 0;
    float _numOfPlayersInside = 0;
    float _tValue = 0;
    float _id = 0;
    half _TextureVisibility;
    half _AngleStrength;
    float _Obstruction;
    half _ObstructionPlayerOffset;

    float _UVs;
    float4 _ObstructionCurve_TexelSize;      
    float _DissolveMaskEnabled;
    float4 _DissolveMask_TexelSize;
    float4 _DissolveTex_TexelSize;
    half4 _DissolveColor;
    float _DissolveColorSaturation;
    float _DissolveEmission;
    float _DissolveEmissionBooster;
    float _hasClippedShadows;
    float _ConeStrength;
    float _ConeObstructionDestroyRadius;
    float _CylinderStrength;
    float _CylinderObstructionDestroyRadius;
    float _CircleStrength;
    float _CircleObstructionDestroyRadius;
    float _CurveStrength;
    float _CurveObstructionDestroyRadius;
    float _IntrinsicDissolveStrength;
    float _DissolveFallOff;
    float _AffectedAreaPlayerBasedObstruction;
    float _PreviewMode;
    float _PreviewIndicatorLineThickness;
    float _AnimationEnabled;
    float _AnimationSpeed;
    float _DefaultEffectRadius;
    float _EnableDefaultEffectRadius;
    float _TransitionDuration;
    float _TexturedEmissionEdge;
    float _TexturedEmissionEdgeStrength;
    float _IsometricExclusion;
    float _IsometricExclusionDistance;
    float _IsometricExclusionGradientLength;
    float _Floor;
    float _FloorMode;
    float _FloorY;
    float _FloorYTextureGradientLength;
    float _PlayerPosYOffset;
    float _AffectedAreaFloor;
    float _Ceiling;
    float _CeilingMode;
    float _CeilingBlendMode;
    float _CeilingY;
    float _CeilingPlayerYOffset;
    float _CeilingYGradientLength;
    float _Zoning;
    float _ZoningMode;
    float _ZoningEdgeGradientLength;
    float _IsZoningRevealable;
    float _SyncZonesWithFloorY;
    float _SyncZonesFloorYOffset;

    half _UseCustomTime;

    half _CrossSectionEnabled;
    half4 _CrossSectionColor;
    half _CrossSectionTextureEnabled;
    float _CrossSectionTextureScale;
    half _CrossSectionUVScaledByDistance;
    half _DissolveMethod;
    half _DissolveTexSpace;
         CBUFFER_END
    sampler2D _NoiseMap1;
    sampler2D _NoiseMap2;

    sampler2D _HatchingMap;
    sampler2D _GradientTex;
    void Ext_ModifyVertex0 (inout VertexData v, inout ExtraV2F d)
    {
        float4x4 UnityObjectToClipPosMatrix = mul(GetWorldToHClipMatrix(), GetObjectToWorldMatrix());
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
    TEXTURE2D(_BaseMap);        SAMPLER(sampler_BaseMap);
    TEXTURE2D(_BumpMap);        SAMPLER(sampler_BumpMap);
    TEXTURE2D(_EmissionMap);    SAMPLER(sampler_EmissionMap);
    TEXTURE2D(_SpecGlossMap);       SAMPLER(sampler_SpecGlossMap);  
	TEXTURE2D(_ParallaxMap);        SAMPLER(sampler_ParallaxMap);
	TEXTURE2D(_OcclusionMap);       SAMPLER(sampler_OcclusionMap);
	TEXTURE2D(_DetailMask);         SAMPLER(sampler_DetailMask);
	TEXTURE2D(_DetailAlbedoMap);    SAMPLER(sampler_DetailAlbedoMap);
	TEXTURE2D(_DetailNormalMap);    SAMPLER(sampler_DetailNormalMap);
	TEXTURE2D(_MetallicGlossMap);   SAMPLER(sampler_MetallicGlossMap);
	TEXTURE2D(_ClearCoatMap);       SAMPLER(sampler_ClearCoatMap);
    #if defined(_DETAIL_MULX2) || defined(_DETAIL_SCALED)
		#define _DETAIL
	#endif

	#if _SPECULAR_SETUP
		#define _USESPECULAR 1
	#else
		#undef _USESPECULAR
	#endif
	float SharpenAlphaMy(float alpha, float alphaClipTreshold)
	{
	    return saturate((alpha - alphaClipTreshold) / max(fwidth(alpha), 0.0001) + 0.5);
	}
#if UNITY_VERSION < 202200
	half3 AlphaModulate(half3 albedo, half alpha)
	{
		#if defined(_ALPHAMODULATE_ON)
		    return lerp(half3(1.0, 1.0, 1.0), albedo, alpha);
		#else
		    return albedo;
		#endif
	}

	#if defined(_ALPHATEST_ON)
		bool IsAlphaToMaskAvailable()
		{
		    return (_AlphaToMaskAvailable != 0.0);
		}
		half AlphaClip(half alpha, half cutoff)
		{
		    half clippedAlpha = (alpha >= cutoff) ? float(alpha) : 0.0;

		    half alphaToCoverageAlpha = SharpenAlphaMy(alpha, cutoff);

		    alpha = IsAlphaToMaskAvailable() ? alphaToCoverageAlpha : clippedAlpha;
		    clip(alpha - 0.0001);

		    return alpha;
		}
	#endif
#endif
    int _DebugSceneOverrideMode;
    bool IsAlphaDiscardEnabledMy()
    {
        #if defined(DEBUG_DISPLAY)
        return (_DebugSceneOverrideMode == DEBUGSCENEOVERRIDEMODE_NONE);
        #else
        return true;
        #endif
    }
	half Alpha(half albedoAlpha, half4 color, half cutoff)
	{
		#if !defined(_SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A) && !defined(_GLOSSINESS_FROM_BASE_ALPHA)
		    half alpha = albedoAlpha * color.a;
		#else
		    half alpha = color.a;
		#endif
			#ifdef _ALPHATEST_ON
			    if (IsAlphaDiscardEnabledMy())
			        alpha = AlphaClip(alpha, cutoff);
			#endif

		    return alpha;
	}
 	half4 SampleAlbedoAlpha(float2 uv, TEXTURE2D_PARAM(albedoAlphaMap, sampler_albedoAlphaMap))
	{
	    return half4(SAMPLE_TEXTURE2D(albedoAlphaMap, sampler_albedoAlphaMap, uv));
	}
 	half3 SampleNormal(float2 uv, TEXTURE2D_PARAM(bumpMap, sampler_bumpMap), half scale = half(1.0))
	{
	#ifdef _NORMALMAP
	    half4 n = SAMPLE_TEXTURE2D(bumpMap, sampler_bumpMap, uv);
	    #if BUMP_SCALE_NOT_SUPPORTED
	        return UnpackNormal(n);
	    #else
	        return UnpackNormalScale(n, scale);
	    #endif
	#else
	    return half3(0.0h, 0.0h, 1.0h);
	#endif
	}
#if UNITY_VERSION < 202100
	half4 SampleSpecularSmoothnessMy(half2 uv, half alpha, half4 specColor, TEXTURE2D_PARAM( specMap, sampler_specMap))
	{
		half4 specularSmoothness = half4(0.0h, 0.0h, 0.0h, 1.0h);
		#ifdef _SPECGLOSSMAP
			specularSmoothness = SAMPLE_TEXTURE2D(specMap, sampler_specMap, uv) * specColor;
		#elif defined(_SPECULAR_COLOR)
			specularSmoothness = specColor;
		#endif

		#ifdef _GLOSSINESS_FROM_BASE_ALPHA
			specularSmoothness.a = exp2(10 * alpha + 1);
		#else
			specularSmoothness.a = exp2(10 * specularSmoothness.a + 1);
		#endif
		return specularSmoothness;
	}
#else

	half4 SampleSpecularSmoothnessMy(float2 uv, half alpha, half4 specColor, TEXTURE2D_PARAM(specMap, sampler_specMap))
	{
		half4 specularSmoothness = half4(0, 0, 0, 1);
	#ifdef _SPECGLOSSMAP
		specularSmoothness = SAMPLE_TEXTURE2D(specMap, sampler_specMap, uv) * specColor;
	#elif defined(_SPECULAR_COLOR)
		specularSmoothness = specColor;
	#endif

	#ifdef _GLOSSINESS_FROM_BASE_ALPHA
		specularSmoothness.a = alpha;
	#endif

		return specularSmoothness;
	}
#endif
    half3 SampleEmissionMy(float2 uv, half3 emissionColor, TEXTURE2D_PARAM(emissionMap, sampler_emissionMap))
    {
    #ifndef _EMISSION
        return 0;
    #else
        return SAMPLE_TEXTURE2D(emissionMap, sampler_emissionMap, uv).rgb * emissionColor;
    #endif
    }
	#ifndef BUILTIN_TARGET_API
		half2 ParallaxOffset1Step(half height, half amplitude, half3 viewDirTS)
		{
		    height = height * amplitude - amplitude / 2.0;
		    half3 v = normalize(viewDirTS);
		    v.z += 0.42;
		    return height * (v.xy / v.z);
		}
	#endif

	float2 ParallaxMapping(TEXTURE2D_PARAM(heightMap, sampler_heightMap), half3 viewDirTS, half scale, float2 uv)
	{
	    half h = SAMPLE_TEXTURE2D(heightMap, sampler_heightMap, uv).g;
	    float2 offset = ParallaxOffset1Step(h, scale, viewDirTS);
	    return offset;
	}
	#ifdef _SPECULAR_SETUP
	    #define SAMPLE_METALLICSPECULAR(uv) SAMPLE_TEXTURE2D(_SpecGlossMap, sampler_SpecGlossMap, uv)
	#else
	    #define SAMPLE_METALLICSPECULAR(uv) SAMPLE_TEXTURE2D(_MetallicGlossMap, sampler_MetallicGlossMap, uv)
	#endif

	half4 SampleMetallicSpecGloss(float2 uv, half albedoAlpha)
	{
	    half4 specGloss;

		#ifdef _METALLICSPECGLOSSMAP
		    specGloss = half4(SAMPLE_METALLICSPECULAR(uv));
		    #ifdef _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
		        specGloss.a = albedoAlpha * _Smoothness;
		    #else
		        specGloss.a *= _Smoothness;
		    #endif
		#else 
		    #if _SPECULAR_SETUP
		        specGloss.rgb = _SpecColor.rgb;
		    #else
		        specGloss.rgb = _Metallic.rrr;
		    #endif

		    #ifdef _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
		        specGloss.a = albedoAlpha * _Smoothness;
		    #else
		        specGloss.a = _Smoothness;
		    #endif
		#endif

	    return specGloss;
	}

	half SampleOcclusion(float2 uv)
	{
	    #ifdef _OCCLUSIONMAP
	        half occ = SAMPLE_TEXTURE2D(_OcclusionMap, sampler_OcclusionMap, uv).g;
	        return LerpWhiteTo(occ, _OcclusionStrength);
	    #else
	        return half(1.0);
	    #endif
	}
    half SampleOcclusionMy(float2 uv)
    {
    #ifdef _OCCLUSIONMAP
    #if defined(SHADER_API_GLES)
        return SAMPLE_TEXTURE2D(_OcclusionMap, sampler_OcclusionMap, uv).g;
    #else
        half occ = SAMPLE_TEXTURE2D(_OcclusionMap, sampler_OcclusionMap, uv).g;
        return LerpWhiteTo(occ, _OcclusionStrength);
    #endif
    #else
        return 1.0;
    #endif
    }
	half2 SampleClearCoat(float2 uv)
	{
		#if defined(_CLEARCOAT) || defined(_CLEARCOATMAP)
		    half2 clearCoatMaskSmoothness = half2(_ClearCoatMask, _ClearCoatSmoothness);

		#if defined(_CLEARCOATMAP)
		    clearCoatMaskSmoothness *= SAMPLE_TEXTURE2D(_ClearCoatMap, sampler_ClearCoatMap, uv).rg;
		#endif

		    return clearCoatMaskSmoothness;
		#else
		    return half2(0.0, 1.0);
		#endif  
	}

	void ApplyPerPixelDisplacement(half3 viewDirTS, inout float2 uv)
	{
		#if defined(_PARALLAXMAP)
		    uv += ParallaxMapping(TEXTURE2D_ARGS(_ParallaxMap, sampler_ParallaxMap), viewDirTS, _Parallax, uv);
		#endif
	}

	half3 ScaleDetailAlbedo(half3 detailAlbedo, half scale)
	{
	    return half(2.0) * detailAlbedo * scale - scale + half(1.0);
	}

	half3 ApplyDetailAlbedo(float2 detailUv, half3 albedo, half detailMask)
	{
		#if defined(_DETAIL)
		    half3 detailAlbedo = SAMPLE_TEXTURE2D(_DetailAlbedoMap, sampler_DetailAlbedoMap, detailUv).rgb;
		#if defined(_DETAIL_SCALED)
		    detailAlbedo = ScaleDetailAlbedo(detailAlbedo, _DetailAlbedoMapScale);
		#else
		    detailAlbedo = half(2.0) * detailAlbedo;
		#endif

		    return albedo * LerpWhiteTo(detailAlbedo, detailMask);
		#else
		    return albedo;
		#endif
	}

	half3 ApplyDetailNormal(float2 detailUv, half3 normalTS, half detailMask)
	{
		#if defined(_DETAIL)
		#if BUMP_SCALE_NOT_SUPPORTED
		    half3 detailNormalTS = UnpackNormal(SAMPLE_TEXTURE2D(_DetailNormalMap, sampler_DetailNormalMap, detailUv));
		#else
		    half3 detailNormalTS = UnpackNormalScale(SAMPLE_TEXTURE2D(_DetailNormalMap, sampler_DetailNormalMap, detailUv), _DetailNormalMapScale);
		#endif
		    detailNormalTS = normalize(detailNormalTS);

		    return lerp(normalTS, BlendNormalRNM(normalTS, detailNormalTS), detailMask); 
		#else
		    return normalTS;
		#endif
	}
	void Ext_SurfaceFunction1 (inout Surface o, ShaderData d)
	{

        float4 texcoords;
        texcoords.xy = d.texcoord0.xy * _BaseMap_ST.xy + _BaseMap_ST.zw; 
        float2 uv = texcoords.xy;

#if _SHADING_BLINNPHONG || _SHADING_COLOR
        half4 diffuseAlpha = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap,  texcoords.xy);
        o.Albedo = diffuseAlpha.rgb * _BaseColor.rgb;
	#if !_PASSFORWARD
		if(_EnableToonShading == 0 || _ShadingAffectedByNormalMap == 1) 
		{
			o.Normal = SampleNormal(texcoords.xy, TEXTURE2D_ARGS(_BumpMap, sampler_BumpMap), _BumpScale);
		}
	#else
        o.Normal = SampleNormal(texcoords.xy, TEXTURE2D_ARGS(_BumpMap, sampler_BumpMap), _BumpScale);
	#endif
		o.Alpha = diffuseAlpha.a;
        o.Alpha = o.Alpha * _BaseColor.a;
        AlphaDiscard(o.Alpha, _Cutoff);
        half3 emission = SampleEmissionMy(uv, _EmissionColor.rgb, TEXTURE2D_ARGS(_EmissionMap, sampler_EmissionMap));
        o.Emission = emission;
#endif

      if(_ShadingMode == 1 && _LightFunction == 0 ) 
      {
#if _SHADING_BLINNPHONG
        half4 specular = SampleSpecularSmoothnessMy(uv, o.Alpha, _SpecColor, TEXTURE2D_ARGS(_SpecGlossMap, sampler_SpecGlossMap));
        half smoothness =  specular.a;

        o.Specular = specular;
        o.Smoothness = smoothness;
#endif

      } 
      else if (_ShadingMode == 1 && _LightFunction == 1) 
      {
#if _SHADING_PBR
		#if defined(_PARALLAXMAP)
		    ApplyPerPixelDisplacement(d.tangentSpaceViewDir, uv);
		#endif
    	half4 albedoAlpha =  SampleAlbedoAlpha(uv, TEXTURE2D_ARGS(_BaseMap, sampler_BaseMap));
	    o.Alpha = Alpha(albedoAlpha.a, _BaseColor, _Cutoff);

	    half4 specGloss = SampleMetallicSpecGloss(uv, albedoAlpha.a);
	    o.Albedo = albedoAlpha.rgb * _BaseColor.rgb;
	    o.Albedo = AlphaModulate(o.Albedo, o.Alpha);
		#if _SPECULAR_SETUP
		    o.Metallic = half(1.0);
		    o.Specular = specGloss.rgb;
		#else
		    o.Metallic = specGloss.r;
		    o.Specular = half3(0.0, 0.0, 0.0);
		#endif
        o.Smoothness = specGloss.a;
	#if !_PASSFORWARD
		if(_EnableToonShading == 0 || _ShadingAffectedByNormalMap == 1) 
		{
			o.Normal = SampleNormal(uv, TEXTURE2D_ARGS(_BumpMap, sampler_BumpMap), _BumpScale);
		}
	#else
		o.Normal = SampleNormal(uv, TEXTURE2D_ARGS(_BumpMap, sampler_BumpMap), _BumpScale);
	#endif
	    o.Occlusion = SampleOcclusion(uv);
	    o.Emission = SampleEmissionMy(uv, _EmissionColor.rgb, TEXTURE2D_ARGS(_EmissionMap, sampler_EmissionMap));

		#if defined(_DETAIL)
		    half detailMask = SAMPLE_TEXTURE2D(_DetailMask, sampler_DetailMask, uv).a;
		    float2 detailUv = uv * _DetailAlbedoMap_ST.xy + _DetailAlbedoMap_ST.zw;
		    o.Albedo = ApplyDetailAlbedo(detailUv, o.Albedo, detailMask);
		    o.Normal = ApplyDetailNormal(detailUv, o.Normal, detailMask);
		#endif

#endif
      }

	}
    float _ArrayLength = 0;
    #if (defined(SHADER_API_GLES) || defined(SHADER_API_GLES3)) 
        float4 _PlayersPosVectorArray[20];
        float _PlayersDataFloatArray[150];     
    #else
        float4 _PlayersPosVectorArray[100];
        float _PlayersDataFloatArray[500];  
    #endif
    #if _ZONING
        #if (defined(SHADER_API_GLES) || defined(SHADER_API_GLES3)) 
            float _ZDFA[500];
        #else
            float _ZDFA[1000];
        #endif
        float _ZonesDataCount;
    #endif

    float _STSCustomTime = 0;
    #if _REPLACEMENT        
        half4 _DissolveColorGlobal;
        float _DissolveColorSaturationGlobal;
        float _DissolveEmissionGlobal;
        float _DissolveEmissionBoosterGlobal;
        float _TextureVisibilityGlobal;
        float _ObstructionGlobal;
        float _AngleStrengthGlobal;
        float _ConeStrengthGlobal;
        float _ConeObstructionDestroyRadiusGlobal;
        float _CylinderStrengthGlobal;
        float _CylinderObstructionDestroyRadiusGlobal;
        float _CircleStrengthGlobal;
        float _CircleObstructionDestroyRadiusGlobal;
        float _CurveStrengthGlobal;
        float _CurveObstructionDestroyRadiusGlobal;
        float _DissolveFallOffGlobal;
        float _AffectedAreaPlayerBasedObstructionGlobal;
        float _IntrinsicDissolveStrengthGlobal;
        float _PreviewModeGlobal;
        float _UVsGlobal;
        float _hasClippedShadowsGlobal;
        float _FloorGlobal;
        float _FloorModeGlobal;
        float _FloorYGlobal;
        float _PlayerPosYOffsetGlobal;
        float _FloorYTextureGradientLengthGlobal;
        float _AffectedAreaFloorGlobal;
        float _AnimationEnabledGlobal;
        float _AnimationSpeedGlobal;
        float _DefaultEffectRadiusGlobal;
        float _EnableDefaultEffectRadiusGlobal;
        float _TransitionDurationGlobal;        
        float _TexturedEmissionEdgeGlobal;
        float _TexturedEmissionEdgeStrengthGlobal;
        float _IsometricExclusionGlobal;
        float _IsometricExclusionDistanceGlobal;
        float _IsometricExclusionGradientLengthGlobal;
        float _CeilingGlobal;
        float _CeilingModeGlobal;
        float _CeilingBlendModeGlobal;
        float _CeilingYGlobal;
        float _CeilingPlayerYOffsetGlobal;
        float _CeilingYGradientLengthGlobal;
        float _ZoningGlobal;
        float _ZoningModeGlobal;
        float _ZoningEdgeGradientLengthGlobal;
        float _IsZoningRevealableGlobal;
        float _SyncZonesWithFloorYGlobal;
        float _SyncZonesFloorYOffsetGlobal;
        float4 _ObstructionCurveGlobal_TexelSize;
        float4 _DissolveMaskGlobal_TexelSize;
        float _DissolveMaskEnabledGlobal;
        float _PreviewIndicatorLineThicknessGlobal;
        half _UseCustomTimeGlobal;

        half _CrossSectionEnabledGlobal;
        half4 _CrossSectionColorGlobal;
        half _CrossSectionTextureEnabledGlobal;
        float _CrossSectionTextureScaleGlobal;
        half _CrossSectionUVScaledByDistanceGlobal;

        half _DissolveMethodGlobal;
        half _DissolveTexSpaceGlobal;
        half _ObstructionPlayerOffsetGlobal;

        float4 _DissolveTexGlobal_TexelSize;
    #endif
    #if _REPLACEMENT
        sampler2D _DissolveTexGlobal;
    #else
        sampler2D _DissolveTex;
    #endif
    #if _REPLACEMENT
        sampler2D _DissolveMaskGlobal;
    #else
        sampler2D _DissolveMask;
    #endif
    #if _REPLACEMENT
        sampler2D _ObstructionCurveGlobal;
    #else
        sampler2D _ObstructionCurve;
    #endif

    #if _REPLACEMENT
        sampler2D _CrossSectionTextureGlobal;
    #else
        sampler2D _CrossSectionTexture;
    #endif
#ifdef USE_UNITY_TEXTURE_2D_TYPE
#undef USE_UNITY_TEXTURE_2D_TYPE
#endif

#include "Packages/com.shadercrew.seethroughshader.core/Scripts/Shaders/xxSharedSTSDependencies/SeeThroughShaderFunction.hlsl"
	void Ext_SurfaceFunction2 (inout Surface o, ShaderData d)
	{
        half3 albedo;
        half3 emission;
        float alphaForClipping;
#ifdef USE_UNITY_TEXTURE_2D_TYPE
#undef USE_UNITY_TEXTURE_2D_TYPE
#endif
#if _REPLACEMENT
    DoSeeThroughShading( o.Albedo, o.Normal, d.worldSpacePosition, d.worldSpaceNormal, d.screenPos,
                        _numOfPlayersInside, _tDirection, _tValue, _id,
                        _TriggerMode, _RaycastMode,
                        _IsExempt,

                        _DissolveMethodGlobal, _DissolveTexSpaceGlobal,

                        _DissolveColorGlobal, _DissolveColorSaturationGlobal, _UVsGlobal,
                        _DissolveEmissionGlobal, _DissolveEmissionBoosterGlobal, _TexturedEmissionEdgeGlobal, _TexturedEmissionEdgeStrengthGlobal,
                        _hasClippedShadowsGlobal,

                        _ObstructionGlobal,
                        _ObstructionPlayerOffsetGlobal,
                        _AngleStrengthGlobal,
                        _ConeStrengthGlobal, _ConeObstructionDestroyRadiusGlobal,
                        _CylinderStrengthGlobal, _CylinderObstructionDestroyRadiusGlobal,
                        _CircleStrengthGlobal, _CircleObstructionDestroyRadiusGlobal,
                        _CurveStrengthGlobal, _CurveObstructionDestroyRadiusGlobal,
                        _DissolveFallOffGlobal,
                        _DissolveMaskEnabledGlobal,
                        _AffectedAreaPlayerBasedObstructionGlobal,
                        _IntrinsicDissolveStrengthGlobal,
                        _DefaultEffectRadiusGlobal, _EnableDefaultEffectRadiusGlobal,

                        _IsometricExclusionGlobal, _IsometricExclusionDistanceGlobal, _IsometricExclusionGradientLengthGlobal,
                        _CeilingGlobal, _CeilingModeGlobal, _CeilingBlendModeGlobal, _CeilingYGlobal, _CeilingPlayerYOffsetGlobal, _CeilingYGradientLengthGlobal,
                        _FloorGlobal, _FloorModeGlobal, _FloorYGlobal, _PlayerPosYOffsetGlobal, _FloorYTextureGradientLengthGlobal, _AffectedAreaFloorGlobal,

                        _AnimationEnabledGlobal, _AnimationSpeedGlobal,
                        _TransitionDurationGlobal,

                        _UseCustomTimeGlobal,

                        _ZoningGlobal, _ZoningModeGlobal, _IsZoningRevealableGlobal, _ZoningEdgeGradientLengthGlobal,
                        _SyncZonesWithFloorYGlobal, _SyncZonesFloorYOffsetGlobal,

                        _PreviewModeGlobal,
                        _PreviewIndicatorLineThicknessGlobal,

                        _DissolveTexGlobal,
                        _DissolveMaskGlobal,
                        _ObstructionCurveGlobal,

                        _DissolveTexGlobal_TexelSize,
                        _DissolveMaskGlobal_TexelSize,
                        _ObstructionCurveGlobal_TexelSize,

                        albedo, emission, alphaForClipping);
#else 
    DoSeeThroughShading( o.Albedo, o.Normal, d.worldSpacePosition, d.worldSpaceNormal, d.screenPos,

                        _numOfPlayersInside, _tDirection, _tValue, _id,
                        _TriggerMode, _RaycastMode,
                        _IsExempt,

                        _DissolveMethod, _DissolveTexSpace,

                        _DissolveColor, _DissolveColorSaturation, _UVs,
                        _DissolveEmission, _DissolveEmissionBooster, _TexturedEmissionEdge, _TexturedEmissionEdgeStrength,
                        _hasClippedShadows,

                        _Obstruction,
                        _ObstructionPlayerOffset,
                        _AngleStrength,
                        _ConeStrength, _ConeObstructionDestroyRadius,
                        _CylinderStrength, _CylinderObstructionDestroyRadius,
                        _CircleStrength, _CircleObstructionDestroyRadius,
                        _CurveStrength, _CurveObstructionDestroyRadius,
                        _DissolveFallOff,
                        _DissolveMaskEnabled,
                        _AffectedAreaPlayerBasedObstruction,
                        _IntrinsicDissolveStrength,
                        _DefaultEffectRadius, _EnableDefaultEffectRadius,

                        _IsometricExclusion, _IsometricExclusionDistance, _IsometricExclusionGradientLength,
                        _Ceiling, _CeilingMode, _CeilingBlendMode, _CeilingY, _CeilingPlayerYOffset, _CeilingYGradientLength,
                        _Floor, _FloorMode, _FloorY, _PlayerPosYOffset, _FloorYTextureGradientLength, _AffectedAreaFloor,

                        _AnimationEnabled, _AnimationSpeed,
                        _TransitionDuration,

                        _UseCustomTime,

                        _Zoning, _ZoningMode , _IsZoningRevealable, _ZoningEdgeGradientLength,
                        _SyncZonesWithFloorY, _SyncZonesFloorYOffset,

                        _PreviewMode,
                        _PreviewIndicatorLineThickness,                            

                        _DissolveTex,
                        _DissolveMask,
                        _ObstructionCurve,

                        _DissolveTex_TexelSize,
                        _DissolveMask_TexelSize,
                        _ObstructionCurve_TexelSize,

                        albedo, emission, alphaForClipping);
#endif 
        o.Albedo = albedo;
        o.Emission += emission;   

	}
    void Ext_FinalColorForward2 (Surface o, ShaderData d, inout half4 color)
    {
        #if _REPLACEMENT   
            DoCrossSection(_CrossSectionEnabledGlobal,
                        _CrossSectionColorGlobal,
                        _CrossSectionTextureEnabledGlobal,
                        _CrossSectionTextureGlobal,
                        _CrossSectionTextureScaleGlobal,
                        _CrossSectionUVScaledByDistanceGlobal,
                        d.isFrontFace,
                        d.screenPos,
                        color);
        #else 
            DoCrossSection(_CrossSectionEnabled,
                        _CrossSectionColor,
                        _CrossSectionTextureEnabled,
                        _CrossSectionTexture,
                        _CrossSectionTextureScale,
                        _CrossSectionUVScaledByDistance,
                        d.isFrontFace,
                        d.screenPos,
                        color);
        #endif
    }
            void ChainSurfaceFunction(inout Surface l, inout ShaderData d)
            {
                  Ext_SurfaceFunction1(l, d);
                  Ext_SurfaceFunction2(l, d);
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
                  Ext_FinalColorForward2(l, d, color);
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
             d.screenUV = (IN.ScreenPosition.xy / max(0.01, IN.ScreenPosition.w));
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
             d.isFrontFace = facing;
            #if _HDRP
            #else
            #endif
             d.screenPos = i.screenPos;
             d.screenUV = (i.screenPos.xy / i.screenPos.w);
             d.extraV2F0 = i.extraV2F0;
            return d;
         }

#endif
         #if defined(_PASSSHADOW)
            float3 _LightDirection;
            float3 _LightPosition;
         #endif
         VertexToPixel Vert (VertexData v)
         {
           VertexToPixel o = (VertexToPixel)0;

           UNITY_SETUP_INSTANCE_ID(v);
           UNITY_TRANSFER_INSTANCE_ID(v, o);
           UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
#if !_TESSELLATION_ON
           ChainModifyVertex(v, o, _Time);
#endif

            o.texcoord0 = v.texcoord0;
           VertexPositionInputs vertexInput = GetVertexPositionInputs(v.vertex.xyz);
           o.worldPos = TransformObjectToWorld(v.vertex.xyz);
           o.worldNormal = TransformObjectToWorldNormal(v.normal);
           o.worldTangent = float4(TransformObjectToWorldDir(v.tangent.xyz), v.tangent.w);
          #if _PASSSHADOW
              #if _CASTING_PUNCTUAL_LIGHT_SHADOW
                 float3 lightDirectionWS = normalize(_LightPosition - o.worldPos);
              #else
                 float3 lightDirectionWS = _LightDirection;
              #endif
              o.pos = TransformWorldToHClip(ApplyShadowBias(o.worldPos, o.worldNormal, lightDirectionWS));
              #if UNITY_REVERSED_Z
                  o.pos.z = min(o.pos.z, UNITY_NEAR_CLIP_VALUE);
              #else
                  o.pos.z = max(o.pos.z, UNITY_NEAR_CLIP_VALUE);
              #endif
          #elif _PASSMETA
              o.pos = MetaVertexPosition(float4(v.vertex.xyz, 0), v.texcoord1.xy, v.texcoord2.xy, unity_LightmapST, unity_DynamicLightmapST);
          #else
              o.pos = TransformWorldToHClip(o.worldPos);
          #endif
           o.screenPos = ComputeScreenPos(o.pos, _ProjectionParams.x);
          #if _PASSFORWARD || _PASSGBUFFER
              float2 uv1 = v.texcoord1.xy;
              OUTPUT_LIGHTMAP_UV(uv1, unity_LightmapST, o.lightmapUV);
              OUTPUT_SH(o.worldNormal, o.sh);
              #if defined(DYNAMICLIGHTMAP_ON)
                   o.dynamicLightmapUV.xy = v.texcoord2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
              #endif
          #endif

          #ifdef VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
              half fogFactor = 0;
              #if defined(_FOG_FRAGMENT)
                fogFactor = ComputeFogFactor(o.pos.z);
              #endif
              #if _BAKEDLIT
                 o.fogFactorAndVertexLight = half4(fogFactor, 0, 0, 0);
              #else
                 half3 vertexLight = VertexLighting(o.worldPos, o.worldNormal);
                 o.fogFactorAndVertexLight = half4(fogFactor, vertexLight);
              #endif
          #endif

          #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
             o.shadowCoord = GetShadowCoord(vertexInput);
          #endif

           return o;
         }
         #include "Packages/com.shadercrew.the-toon-shader.core/Scripts/Shaders/xxSharedTTSDependecies/TheToonShaderFunctions.hlsl"
         void Frag (VertexToPixel IN
              , out half4 outColor : SV_Target0
            #ifdef _WRITE_RENDERING_LAYERS
              , out float4 outRenderingLayers : SV_Target1
            #endif
            #ifdef _DEPTHOFFSET_ON
              , out float outputDepth : SV_Depth
            #endif
            #if NEED_FACING
               , bool facing : SV_IsFrontFace
            #endif
         )
         {
           UNITY_SETUP_INSTANCE_ID(IN);
           UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);

           #if defined(LOD_FADE_CROSSFADE)
              LODFadeCrossFade(IN.pos);
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
            float3 specular = l.Specular;
            float metallic = l.Metallic;
            InputData inputData = (InputData)0;

            inputData.positionWS = IN.worldPos;
            #if _WORLDSPACENORMAL
              inputData.normalWS = l.Normal;
            #else
              inputData.normalWS = normalize(TangentToWorldSpace(d, l.Normal));
            #endif

            inputData.viewDirectionWS = SafeNormalize(d.worldSpaceViewDir);
            #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
                  inputData.shadowCoord = IN.shadowCoord;
            #elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
                  inputData.shadowCoord = TransformWorldToShadowCoord(IN.worldPos);
            #else
                  inputData.shadowCoord = float4(0, 0, 0, 0);
            #endif
#if _BAKEDLIT
            inputData.fogCoord = IN.fogFactorAndVertexLight.x;
            inputData.vertexLighting = 0;
#else
            inputData.fogCoord = InitializeInputDataFog(float4(IN.worldPos, 1.0), IN.fogFactorAndVertexLight.x);
            inputData.vertexLighting = IN.fogFactorAndVertexLight.yzw;
#endif    
            #if defined(_OVERRIDE_BAKEDGI)
               inputData.bakedGI = l.DiffuseGI;
               l.Emission += l.SpecularGI;
            #elif _BAKEDLIT
               inputData.bakedGI = SAMPLE_GI(IN.lightmapUV, IN.sh, inputData.normalWS);
            #else
               #if defined(DYNAMICLIGHTMAP_ON)
                  inputData.bakedGI = SAMPLE_GI(IN.lightmapUV, IN.dynamicLightmapUV.xy, IN.sh, inputData.normalWS);
               #else
                  inputData.bakedGI = SAMPLE_GI(IN.lightmapUV, IN.sh, inputData.normalWS);
               #endif
            #endif

            inputData.normalizedScreenSpaceUV = GetNormalizedScreenSpaceUV(IN.pos);
            #if !_BAKEDLIT
               inputData.shadowMask = SAMPLE_SHADOWMASK(IN.lightmapUV);
               #if defined(_OVERRIDE_SHADOWMASK)
                  float4 mulColor = saturate(dot(l.ShadowMask, _MainLightOcclusionProbes)); 
                  inputData.shadowMask = mulColor;
               #endif
            #else
               inputData.shadowMask = float4(1,1,1,1);
            #endif

            #if defined(DEBUG_DISPLAY)
                #if defined(DYNAMICLIGHTMAP_ON)
                  inputData.dynamicLightmapUV = IN.dynamicLightmapUV.xy;
                #endif
                #if defined(LIGHTMAP_ON)
                  inputData.staticLightmapUV = IN.lightmapUV;
                #else
                  inputData.vertexSH = IN.sh;
                #endif
            #endif
            #if _WORLDSPACENORMAL
              float3 normalTS = WorldToTangentSpace(d, l.Normal);
            #else
              float3 normalTS = l.Normal;
            #endif

            SurfaceData surface         = (SurfaceData)0;
            surface.albedo              = l.Albedo;
            surface.metallic            = saturate(metallic);
            surface.specular            = specular;
            surface.smoothness          = saturate(l.Smoothness),
            surface.occlusion           = l.Occlusion,
            surface.emission            = l.Emission,
            surface.alpha               = saturate(l.Alpha);
            surface.clearCoatMask       = 0;
            surface.clearCoatSmoothness = 1;

            #ifdef _CLEARCOAT
                  surface.clearCoatMask       = saturate(l.CoatMask);
                  surface.clearCoatSmoothness = saturate(l.CoatSmoothness);
            #endif
            half4 color = half4(l.Albedo, l.Alpha);

            #ifdef _DBUFFER
                ApplyDecalToSurfaceData(IN.pos, surface, inputData);
            #endif
            AddTheToonShader(color, inputData, surface, d, normalTS);

            #if !DISABLEFOG
                color.rgb = MixFog(color.rgb, inputData.fogCoord);
            #endif

            ChainFinalColorForward(l, d, color);
            outColor = color;

            #ifdef _WRITE_RENDERING_LAYERS
                uint renderingLayers = GetMeshRenderingLayer();
                outRenderingLayers = float4(EncodeMeshRenderingLayer(renderingLayers), 0, 0, 0);
            #endif

         }

         ENDHLSL

      }
        Pass
        {
            Name "GBuffer"
            Tags
            {
               "LightMode" = "UniversalGBuffer"
            }
             Blend One Zero
             ZTest LEqual
             ZWrite On

                Cull [_Cull]
            HLSLPROGRAM

               #pragma vertex Vert
   #pragma fragment Frag

            #pragma target 3.0

            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma multi_compile_instancing
            #pragma multi_compile_fog
            #pragma instancing_options renderinglayer
            #pragma multi_compile _ DOTS_INSTANCING_ON
            #pragma multi_compile _ LIGHTMAP_ON
            #pragma multi_compile _ DYNAMICLIGHTMAP_ON
            #pragma multi_compile _ DIRLIGHTMAP_COMBINED
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
            #pragma multi_compile_fragment _ _REFLECTION_PROBE_BLENDING
            #pragma multi_compile_fragment _ _REFLECTION_PROBE_BOX_PROJECTION
            #pragma multi_compile_fragment _ _SHADOWS_SOFT
            #pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
            #pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE
            #pragma multi_compile_fragment _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
            #pragma multi_compile_fragment _ _GBUFFER_NORMALS_OCT
            #pragma multi_compile_fragment _ _WRITE_RENDERING_LAYERS
            #pragma multi_compile_fragment _ _RENDER_PASS_ENABLED
            #pragma multi_compile_fragment _ DEBUG_DISPLAY
            #pragma multi_compile _ SHADOWS_SHADOWMASK
            #pragma multi_compile_fragment _ LOD_FADE_CROSSFADE
            #define _FOG_FRAGMENT 1

            #define VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
            #define SHADERPASS SHADERPASS_GBUFFER
            #define _PASSGBUFFER 1
     #pragma shader_feature_local_fragment  _SHADING_COLOR _SHADING_BLINNPHONG _SHADING_PBR    
    #pragma shader_feature_local _NORMALMAP
    #pragma shader_feature_local_fragment _EMISSION

    #pragma shader_feature_local_fragment _ALPHATEST_ON
    #pragma shader_feature_local _RECEIVE_SHADOWS_OFF
    #pragma shader_feature_local_fragment _ _SPECGLOSSMAP _SPECULAR_COLOR 
    #pragma shader_feature_local_fragment _GLOSSINESS_FROM_BASE_ALPHA

    #pragma shader_feature_local_fragment _OCCLUSIONMAP
    #pragma shader_feature_local _PARALLAXMAP
    #pragma shader_feature_local _ _DETAIL_MULX2 _DETAIL_SCALED
    #pragma shader_feature_local_fragment _SURFACE_TYPE_TRANSPARENT

    #pragma shader_feature_local_fragment _ _ALPHAPREMULTIPLY_ON _ALPHAMODULATE_ON

    #pragma shader_feature_local_fragment _METALLICSPECGLOSSMAP
    #pragma shader_feature_local_fragment _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
    #pragma shader_feature_local_fragment _SPECULARHIGHLIGHTS_OFF
    #pragma shader_feature_local_fragment _ENVIRONMENTREFLECTIONS_OFF
    #pragma shader_feature_local_fragment _SPECULAR_SETUP
        #pragma shader_feature_local_fragment _OBSTRUCTION_CURVE
        #pragma shader_feature_local_fragment _DISSOLVEMASK
        #pragma shader_feature_local_fragment _ZONING
        #pragma shader_feature_local_fragment _REPLACEMENT
        #pragma shader_feature_local_fragment _PLAYERINDEPENDENT
   #define _URP 1
#define NEED_FACING 1
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DBuffer.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/LODCrossFade.hlsl"
                  #undef WorldNormalVector
      #define WorldNormalVector(data, normal) mul(normal, data.TBNMatrix)
      #define UnityObjectToWorldNormal(normal) mul(GetObjectToWorldMatrix(), normal)

      #define _WorldSpaceLightPos0 _MainLightPosition
      #define UNITY_DECLARE_TEX2D(name) TEXTURE2D(name); SAMPLER(sampler##name);
      #define UNITY_DECLARE_TEX2D_NOSAMPLER(name) TEXTURE2D(name);
      #define UNITY_DECLARE_TEX2DARRAY(name) TEXTURE2D_ARRAY(name); SAMPLER(sampler##name);
      #define UNITY_DECLARE_TEX2DARRAY_NOSAMPLER(name) TEXTURE2D_ARRAY(name);

      #define UNITY_SAMPLE_TEX2DARRAY(tex,coord)            SAMPLE_TEXTURE2D_ARRAY(tex, sampler##tex, coord.xy, coord.z)
      #define UNITY_SAMPLE_TEX2DARRAY_LOD(tex,coord,lod)    SAMPLE_TEXTURE2D_ARRAY_LOD(tex, sampler##tex, coord.xy, coord.z, lod)
      #define UNITY_SAMPLE_TEX2D(tex, coord)                SAMPLE_TEXTURE2D(tex, sampler##tex, coord)
      #define UNITY_SAMPLE_TEX2D_SAMPLER(tex, samp, coord)  SAMPLE_TEXTURE2D(tex, sampler##samp, coord)

      #define UNITY_SAMPLE_TEX2D_LOD(tex,coord, lod)   SAMPLE_TEXTURE2D_LOD(tex, sampler_##tex, coord, lod)
      #define UNITY_SAMPLE_TEX2D_SAMPLER_LOD(tex,samplertex,coord, lod) SAMPLE_TEXTURE2D_LOD (tex, sampler##samplertex,coord, lod)
      #if defined(UNITY_COMPILER_HLSL)
         #define UNITY_INITIALIZE_OUTPUT(type,name) name = (type)0;
      #else
         #define UNITY_INITIALIZE_OUTPUT(type,name)
      #endif

      #define sampler2D_float sampler2D
      #define sampler2D_half sampler2D
      struct VertexToPixel
      {
         float4 pos : SV_POSITION;
         float3 worldPos : TEXCOORD0;
         float3 worldNormal : TEXCOORD1;
         float4 worldTangent : TEXCOORD2;
          float4 texcoord0 : TEXCOORD3;
          float4 screenPos : TEXCOORD7;
         #if defined(LIGHTMAP_ON)
            float2 lightmapUV : TEXCOORD8;
         #endif
         #if defined(DYNAMICLIGHTMAP_ON)
            float2 dynamicLightmapUV : TEXCOORD9;
         #endif
         #if !defined(LIGHTMAP_ON)
            float3 sh : TEXCOORD10;
         #endif

         #if defined(VARYINGS_NEED_FOG_AND_VERTEX_LIGHT)
            float4 fogFactorAndVertexLight : TEXCOORD11;
         #endif

         #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
           float4 shadowCoord : TEXCOORD12;
         #endif
          float4 extraV2F0 : TEXCOORD13;
         #if UNITY_ANY_INSTANCING_ENABLED
         uint instanceID : CUSTOM_INSTANCE_ID;
         #endif
         #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
         uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
         #endif
         #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
         uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
         #endif
         #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
         FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
         #endif
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

               #undef GetWorldToObjectMatrix()

               #define GetWorldToObjectMatrix()   unity_WorldToObject
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
               float3 camView = mul((float3x3)GetObjectToWorldMatrix(), transpose(mul(GetWorldToObjectMatrix(), UNITY_MATRIX_I_V)) [2].xyz);

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
                  norms = mul((float3x3)GetWorldToViewMatrix(), norms) * 0.5 + 0.5;
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
            CBUFFER_START(UnityPerMaterial)
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
	half4 _BaseColor;
    float _BumpScale;
    float4 _BaseMap_ST;
    half _Cutoff;
    half4 _EmissionColor;
     half4 _SpecColor;
     float _Smoothness;
	float4 _DetailAlbedoMap_ST;
	half _Metallic;
	half _Parallax;
	half _OcclusionStrength;
	half _DetailAlbedoMapScale;
	half _DetailNormalMapScale;
#if UNITY_VERSION < 202200
	float _AlphaToMaskAvailable;
#endif
    float _SyncCullMode;

    float _IsReplacementShader;
    float _TriggerMode;
    float _RaycastMode;
    float _IsExempt;
    float _isReferenceMaterial;
    float _InteractionMode;

    float _tDirection = 0;
    float _numOfPlayersInside = 0;
    float _tValue = 0;
    float _id = 0;
    half _TextureVisibility;
    half _AngleStrength;
    float _Obstruction;
    half _ObstructionPlayerOffset;

    float _UVs;
    float4 _ObstructionCurve_TexelSize;      
    float _DissolveMaskEnabled;
    float4 _DissolveMask_TexelSize;
    float4 _DissolveTex_TexelSize;
    half4 _DissolveColor;
    float _DissolveColorSaturation;
    float _DissolveEmission;
    float _DissolveEmissionBooster;
    float _hasClippedShadows;
    float _ConeStrength;
    float _ConeObstructionDestroyRadius;
    float _CylinderStrength;
    float _CylinderObstructionDestroyRadius;
    float _CircleStrength;
    float _CircleObstructionDestroyRadius;
    float _CurveStrength;
    float _CurveObstructionDestroyRadius;
    float _IntrinsicDissolveStrength;
    float _DissolveFallOff;
    float _AffectedAreaPlayerBasedObstruction;
    float _PreviewMode;
    float _PreviewIndicatorLineThickness;
    float _AnimationEnabled;
    float _AnimationSpeed;
    float _DefaultEffectRadius;
    float _EnableDefaultEffectRadius;
    float _TransitionDuration;
    float _TexturedEmissionEdge;
    float _TexturedEmissionEdgeStrength;
    float _IsometricExclusion;
    float _IsometricExclusionDistance;
    float _IsometricExclusionGradientLength;
    float _Floor;
    float _FloorMode;
    float _FloorY;
    float _FloorYTextureGradientLength;
    float _PlayerPosYOffset;
    float _AffectedAreaFloor;
    float _Ceiling;
    float _CeilingMode;
    float _CeilingBlendMode;
    float _CeilingY;
    float _CeilingPlayerYOffset;
    float _CeilingYGradientLength;
    float _Zoning;
    float _ZoningMode;
    float _ZoningEdgeGradientLength;
    float _IsZoningRevealable;
    float _SyncZonesWithFloorY;
    float _SyncZonesFloorYOffset;

    half _UseCustomTime;

    half _CrossSectionEnabled;
    half4 _CrossSectionColor;
    half _CrossSectionTextureEnabled;
    float _CrossSectionTextureScale;
    half _CrossSectionUVScaledByDistance;
    half _DissolveMethod;
    half _DissolveTexSpace;
            CBUFFER_END
    sampler2D _NoiseMap1;
    sampler2D _NoiseMap2;

    sampler2D _HatchingMap;
    sampler2D _GradientTex;
    void Ext_ModifyVertex0 (inout VertexData v, inout ExtraV2F d)
    {
        float4x4 UnityObjectToClipPosMatrix = mul(GetWorldToHClipMatrix(), GetObjectToWorldMatrix());
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
    TEXTURE2D(_BaseMap);        SAMPLER(sampler_BaseMap);
    TEXTURE2D(_BumpMap);        SAMPLER(sampler_BumpMap);
    TEXTURE2D(_EmissionMap);    SAMPLER(sampler_EmissionMap);
    TEXTURE2D(_SpecGlossMap);       SAMPLER(sampler_SpecGlossMap);  
	TEXTURE2D(_ParallaxMap);        SAMPLER(sampler_ParallaxMap);
	TEXTURE2D(_OcclusionMap);       SAMPLER(sampler_OcclusionMap);
	TEXTURE2D(_DetailMask);         SAMPLER(sampler_DetailMask);
	TEXTURE2D(_DetailAlbedoMap);    SAMPLER(sampler_DetailAlbedoMap);
	TEXTURE2D(_DetailNormalMap);    SAMPLER(sampler_DetailNormalMap);
	TEXTURE2D(_MetallicGlossMap);   SAMPLER(sampler_MetallicGlossMap);
	TEXTURE2D(_ClearCoatMap);       SAMPLER(sampler_ClearCoatMap);
    #if defined(_DETAIL_MULX2) || defined(_DETAIL_SCALED)
		#define _DETAIL
	#endif

	#if _SPECULAR_SETUP
		#define _USESPECULAR 1
	#else
		#undef _USESPECULAR
	#endif
	float SharpenAlphaMy(float alpha, float alphaClipTreshold)
	{
	    return saturate((alpha - alphaClipTreshold) / max(fwidth(alpha), 0.0001) + 0.5);
	}
#if UNITY_VERSION < 202200
	half3 AlphaModulate(half3 albedo, half alpha)
	{
		#if defined(_ALPHAMODULATE_ON)
		    return lerp(half3(1.0, 1.0, 1.0), albedo, alpha);
		#else
		    return albedo;
		#endif
	}

	#if defined(_ALPHATEST_ON)
		bool IsAlphaToMaskAvailable()
		{
		    return (_AlphaToMaskAvailable != 0.0);
		}
		half AlphaClip(half alpha, half cutoff)
		{
		    half clippedAlpha = (alpha >= cutoff) ? float(alpha) : 0.0;

		    half alphaToCoverageAlpha = SharpenAlphaMy(alpha, cutoff);

		    alpha = IsAlphaToMaskAvailable() ? alphaToCoverageAlpha : clippedAlpha;
		    clip(alpha - 0.0001);

		    return alpha;
		}
	#endif
#endif
    int _DebugSceneOverrideMode;
    bool IsAlphaDiscardEnabledMy()
    {
        #if defined(DEBUG_DISPLAY)
        return (_DebugSceneOverrideMode == DEBUGSCENEOVERRIDEMODE_NONE);
        #else
        return true;
        #endif
    }
	half Alpha(half albedoAlpha, half4 color, half cutoff)
	{
		#if !defined(_SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A) && !defined(_GLOSSINESS_FROM_BASE_ALPHA)
		    half alpha = albedoAlpha * color.a;
		#else
		    half alpha = color.a;
		#endif
			#ifdef _ALPHATEST_ON
			    if (IsAlphaDiscardEnabledMy())
			        alpha = AlphaClip(alpha, cutoff);
			#endif

		    return alpha;
	}
 	half4 SampleAlbedoAlpha(float2 uv, TEXTURE2D_PARAM(albedoAlphaMap, sampler_albedoAlphaMap))
	{
	    return half4(SAMPLE_TEXTURE2D(albedoAlphaMap, sampler_albedoAlphaMap, uv));
	}
 	half3 SampleNormal(float2 uv, TEXTURE2D_PARAM(bumpMap, sampler_bumpMap), half scale = half(1.0))
	{
	#ifdef _NORMALMAP
	    half4 n = SAMPLE_TEXTURE2D(bumpMap, sampler_bumpMap, uv);
	    #if BUMP_SCALE_NOT_SUPPORTED
	        return UnpackNormal(n);
	    #else
	        return UnpackNormalScale(n, scale);
	    #endif
	#else
	    return half3(0.0h, 0.0h, 1.0h);
	#endif
	}
#if UNITY_VERSION < 202100
	half4 SampleSpecularSmoothnessMy(half2 uv, half alpha, half4 specColor, TEXTURE2D_PARAM( specMap, sampler_specMap))
	{
		half4 specularSmoothness = half4(0.0h, 0.0h, 0.0h, 1.0h);
		#ifdef _SPECGLOSSMAP
			specularSmoothness = SAMPLE_TEXTURE2D(specMap, sampler_specMap, uv) * specColor;
		#elif defined(_SPECULAR_COLOR)
			specularSmoothness = specColor;
		#endif

		#ifdef _GLOSSINESS_FROM_BASE_ALPHA
			specularSmoothness.a = exp2(10 * alpha + 1);
		#else
			specularSmoothness.a = exp2(10 * specularSmoothness.a + 1);
		#endif
		return specularSmoothness;
	}
#else

	half4 SampleSpecularSmoothnessMy(float2 uv, half alpha, half4 specColor, TEXTURE2D_PARAM(specMap, sampler_specMap))
	{
		half4 specularSmoothness = half4(0, 0, 0, 1);
	#ifdef _SPECGLOSSMAP
		specularSmoothness = SAMPLE_TEXTURE2D(specMap, sampler_specMap, uv) * specColor;
	#elif defined(_SPECULAR_COLOR)
		specularSmoothness = specColor;
	#endif

	#ifdef _GLOSSINESS_FROM_BASE_ALPHA
		specularSmoothness.a = alpha;
	#endif

		return specularSmoothness;
	}
#endif
    half3 SampleEmissionMy(float2 uv, half3 emissionColor, TEXTURE2D_PARAM(emissionMap, sampler_emissionMap))
    {
    #ifndef _EMISSION
        return 0;
    #else
        return SAMPLE_TEXTURE2D(emissionMap, sampler_emissionMap, uv).rgb * emissionColor;
    #endif
    }
	#ifndef BUILTIN_TARGET_API
		half2 ParallaxOffset1Step(half height, half amplitude, half3 viewDirTS)
		{
		    height = height * amplitude - amplitude / 2.0;
		    half3 v = normalize(viewDirTS);
		    v.z += 0.42;
		    return height * (v.xy / v.z);
		}
	#endif

	float2 ParallaxMapping(TEXTURE2D_PARAM(heightMap, sampler_heightMap), half3 viewDirTS, half scale, float2 uv)
	{
	    half h = SAMPLE_TEXTURE2D(heightMap, sampler_heightMap, uv).g;
	    float2 offset = ParallaxOffset1Step(h, scale, viewDirTS);
	    return offset;
	}
	#ifdef _SPECULAR_SETUP
	    #define SAMPLE_METALLICSPECULAR(uv) SAMPLE_TEXTURE2D(_SpecGlossMap, sampler_SpecGlossMap, uv)
	#else
	    #define SAMPLE_METALLICSPECULAR(uv) SAMPLE_TEXTURE2D(_MetallicGlossMap, sampler_MetallicGlossMap, uv)
	#endif

	half4 SampleMetallicSpecGloss(float2 uv, half albedoAlpha)
	{
	    half4 specGloss;

		#ifdef _METALLICSPECGLOSSMAP
		    specGloss = half4(SAMPLE_METALLICSPECULAR(uv));
		    #ifdef _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
		        specGloss.a = albedoAlpha * _Smoothness;
		    #else
		        specGloss.a *= _Smoothness;
		    #endif
		#else 
		    #if _SPECULAR_SETUP
		        specGloss.rgb = _SpecColor.rgb;
		    #else
		        specGloss.rgb = _Metallic.rrr;
		    #endif

		    #ifdef _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
		        specGloss.a = albedoAlpha * _Smoothness;
		    #else
		        specGloss.a = _Smoothness;
		    #endif
		#endif

	    return specGloss;
	}

	half SampleOcclusion(float2 uv)
	{
	    #ifdef _OCCLUSIONMAP
	        half occ = SAMPLE_TEXTURE2D(_OcclusionMap, sampler_OcclusionMap, uv).g;
	        return LerpWhiteTo(occ, _OcclusionStrength);
	    #else
	        return half(1.0);
	    #endif
	}
    half SampleOcclusionMy(float2 uv)
    {
    #ifdef _OCCLUSIONMAP
    #if defined(SHADER_API_GLES)
        return SAMPLE_TEXTURE2D(_OcclusionMap, sampler_OcclusionMap, uv).g;
    #else
        half occ = SAMPLE_TEXTURE2D(_OcclusionMap, sampler_OcclusionMap, uv).g;
        return LerpWhiteTo(occ, _OcclusionStrength);
    #endif
    #else
        return 1.0;
    #endif
    }
	half2 SampleClearCoat(float2 uv)
	{
		#if defined(_CLEARCOAT) || defined(_CLEARCOATMAP)
		    half2 clearCoatMaskSmoothness = half2(_ClearCoatMask, _ClearCoatSmoothness);

		#if defined(_CLEARCOATMAP)
		    clearCoatMaskSmoothness *= SAMPLE_TEXTURE2D(_ClearCoatMap, sampler_ClearCoatMap, uv).rg;
		#endif

		    return clearCoatMaskSmoothness;
		#else
		    return half2(0.0, 1.0);
		#endif  
	}

	void ApplyPerPixelDisplacement(half3 viewDirTS, inout float2 uv)
	{
		#if defined(_PARALLAXMAP)
		    uv += ParallaxMapping(TEXTURE2D_ARGS(_ParallaxMap, sampler_ParallaxMap), viewDirTS, _Parallax, uv);
		#endif
	}

	half3 ScaleDetailAlbedo(half3 detailAlbedo, half scale)
	{
	    return half(2.0) * detailAlbedo * scale - scale + half(1.0);
	}

	half3 ApplyDetailAlbedo(float2 detailUv, half3 albedo, half detailMask)
	{
		#if defined(_DETAIL)
		    half3 detailAlbedo = SAMPLE_TEXTURE2D(_DetailAlbedoMap, sampler_DetailAlbedoMap, detailUv).rgb;
		#if defined(_DETAIL_SCALED)
		    detailAlbedo = ScaleDetailAlbedo(detailAlbedo, _DetailAlbedoMapScale);
		#else
		    detailAlbedo = half(2.0) * detailAlbedo;
		#endif

		    return albedo * LerpWhiteTo(detailAlbedo, detailMask);
		#else
		    return albedo;
		#endif
	}

	half3 ApplyDetailNormal(float2 detailUv, half3 normalTS, half detailMask)
	{
		#if defined(_DETAIL)
		#if BUMP_SCALE_NOT_SUPPORTED
		    half3 detailNormalTS = UnpackNormal(SAMPLE_TEXTURE2D(_DetailNormalMap, sampler_DetailNormalMap, detailUv));
		#else
		    half3 detailNormalTS = UnpackNormalScale(SAMPLE_TEXTURE2D(_DetailNormalMap, sampler_DetailNormalMap, detailUv), _DetailNormalMapScale);
		#endif
		    detailNormalTS = normalize(detailNormalTS);

		    return lerp(normalTS, BlendNormalRNM(normalTS, detailNormalTS), detailMask); 
		#else
		    return normalTS;
		#endif
	}
	void Ext_SurfaceFunction1 (inout Surface o, ShaderData d)
	{

        float4 texcoords;
        texcoords.xy = d.texcoord0.xy * _BaseMap_ST.xy + _BaseMap_ST.zw; 
        float2 uv = texcoords.xy;

#if _SHADING_BLINNPHONG || _SHADING_COLOR
        half4 diffuseAlpha = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap,  texcoords.xy);
        o.Albedo = diffuseAlpha.rgb * _BaseColor.rgb;
	#if !_PASSFORWARD
		if(_EnableToonShading == 0 || _ShadingAffectedByNormalMap == 1) 
		{
			o.Normal = SampleNormal(texcoords.xy, TEXTURE2D_ARGS(_BumpMap, sampler_BumpMap), _BumpScale);
		}
	#else
        o.Normal = SampleNormal(texcoords.xy, TEXTURE2D_ARGS(_BumpMap, sampler_BumpMap), _BumpScale);
	#endif
		o.Alpha = diffuseAlpha.a;
        o.Alpha = o.Alpha * _BaseColor.a;
        AlphaDiscard(o.Alpha, _Cutoff);
        half3 emission = SampleEmissionMy(uv, _EmissionColor.rgb, TEXTURE2D_ARGS(_EmissionMap, sampler_EmissionMap));
        o.Emission = emission;
#endif

      if(_ShadingMode == 1 && _LightFunction == 0 ) 
      {
#if _SHADING_BLINNPHONG
        half4 specular = SampleSpecularSmoothnessMy(uv, o.Alpha, _SpecColor, TEXTURE2D_ARGS(_SpecGlossMap, sampler_SpecGlossMap));
        half smoothness =  specular.a;

        o.Specular = specular;
        o.Smoothness = smoothness;
#endif

      } 
      else if (_ShadingMode == 1 && _LightFunction == 1) 
      {
#if _SHADING_PBR
		#if defined(_PARALLAXMAP)
		    ApplyPerPixelDisplacement(d.tangentSpaceViewDir, uv);
		#endif
    	half4 albedoAlpha =  SampleAlbedoAlpha(uv, TEXTURE2D_ARGS(_BaseMap, sampler_BaseMap));
	    o.Alpha = Alpha(albedoAlpha.a, _BaseColor, _Cutoff);

	    half4 specGloss = SampleMetallicSpecGloss(uv, albedoAlpha.a);
	    o.Albedo = albedoAlpha.rgb * _BaseColor.rgb;
	    o.Albedo = AlphaModulate(o.Albedo, o.Alpha);
		#if _SPECULAR_SETUP
		    o.Metallic = half(1.0);
		    o.Specular = specGloss.rgb;
		#else
		    o.Metallic = specGloss.r;
		    o.Specular = half3(0.0, 0.0, 0.0);
		#endif
        o.Smoothness = specGloss.a;
	#if !_PASSFORWARD
		if(_EnableToonShading == 0 || _ShadingAffectedByNormalMap == 1) 
		{
			o.Normal = SampleNormal(uv, TEXTURE2D_ARGS(_BumpMap, sampler_BumpMap), _BumpScale);
		}
	#else
		o.Normal = SampleNormal(uv, TEXTURE2D_ARGS(_BumpMap, sampler_BumpMap), _BumpScale);
	#endif
	    o.Occlusion = SampleOcclusion(uv);
	    o.Emission = SampleEmissionMy(uv, _EmissionColor.rgb, TEXTURE2D_ARGS(_EmissionMap, sampler_EmissionMap));

		#if defined(_DETAIL)
		    half detailMask = SAMPLE_TEXTURE2D(_DetailMask, sampler_DetailMask, uv).a;
		    float2 detailUv = uv * _DetailAlbedoMap_ST.xy + _DetailAlbedoMap_ST.zw;
		    o.Albedo = ApplyDetailAlbedo(detailUv, o.Albedo, detailMask);
		    o.Normal = ApplyDetailNormal(detailUv, o.Normal, detailMask);
		#endif

#endif
      }

	}
    float _ArrayLength = 0;
    #if (defined(SHADER_API_GLES) || defined(SHADER_API_GLES3)) 
        float4 _PlayersPosVectorArray[20];
        float _PlayersDataFloatArray[150];     
    #else
        float4 _PlayersPosVectorArray[100];
        float _PlayersDataFloatArray[500];  
    #endif
    #if _ZONING
        #if (defined(SHADER_API_GLES) || defined(SHADER_API_GLES3)) 
            float _ZDFA[500];
        #else
            float _ZDFA[1000];
        #endif
        float _ZonesDataCount;
    #endif

    float _STSCustomTime = 0;
    #if _REPLACEMENT        
        half4 _DissolveColorGlobal;
        float _DissolveColorSaturationGlobal;
        float _DissolveEmissionGlobal;
        float _DissolveEmissionBoosterGlobal;
        float _TextureVisibilityGlobal;
        float _ObstructionGlobal;
        float _AngleStrengthGlobal;
        float _ConeStrengthGlobal;
        float _ConeObstructionDestroyRadiusGlobal;
        float _CylinderStrengthGlobal;
        float _CylinderObstructionDestroyRadiusGlobal;
        float _CircleStrengthGlobal;
        float _CircleObstructionDestroyRadiusGlobal;
        float _CurveStrengthGlobal;
        float _CurveObstructionDestroyRadiusGlobal;
        float _DissolveFallOffGlobal;
        float _AffectedAreaPlayerBasedObstructionGlobal;
        float _IntrinsicDissolveStrengthGlobal;
        float _PreviewModeGlobal;
        float _UVsGlobal;
        float _hasClippedShadowsGlobal;
        float _FloorGlobal;
        float _FloorModeGlobal;
        float _FloorYGlobal;
        float _PlayerPosYOffsetGlobal;
        float _FloorYTextureGradientLengthGlobal;
        float _AffectedAreaFloorGlobal;
        float _AnimationEnabledGlobal;
        float _AnimationSpeedGlobal;
        float _DefaultEffectRadiusGlobal;
        float _EnableDefaultEffectRadiusGlobal;
        float _TransitionDurationGlobal;        
        float _TexturedEmissionEdgeGlobal;
        float _TexturedEmissionEdgeStrengthGlobal;
        float _IsometricExclusionGlobal;
        float _IsometricExclusionDistanceGlobal;
        float _IsometricExclusionGradientLengthGlobal;
        float _CeilingGlobal;
        float _CeilingModeGlobal;
        float _CeilingBlendModeGlobal;
        float _CeilingYGlobal;
        float _CeilingPlayerYOffsetGlobal;
        float _CeilingYGradientLengthGlobal;
        float _ZoningGlobal;
        float _ZoningModeGlobal;
        float _ZoningEdgeGradientLengthGlobal;
        float _IsZoningRevealableGlobal;
        float _SyncZonesWithFloorYGlobal;
        float _SyncZonesFloorYOffsetGlobal;
        float4 _ObstructionCurveGlobal_TexelSize;
        float4 _DissolveMaskGlobal_TexelSize;
        float _DissolveMaskEnabledGlobal;
        float _PreviewIndicatorLineThicknessGlobal;
        half _UseCustomTimeGlobal;

        half _CrossSectionEnabledGlobal;
        half4 _CrossSectionColorGlobal;
        half _CrossSectionTextureEnabledGlobal;
        float _CrossSectionTextureScaleGlobal;
        half _CrossSectionUVScaledByDistanceGlobal;

        half _DissolveMethodGlobal;
        half _DissolveTexSpaceGlobal;
        half _ObstructionPlayerOffsetGlobal;

        float4 _DissolveTexGlobal_TexelSize;
    #endif
    #if _REPLACEMENT
        sampler2D _DissolveTexGlobal;
    #else
        sampler2D _DissolveTex;
    #endif
    #if _REPLACEMENT
        sampler2D _DissolveMaskGlobal;
    #else
        sampler2D _DissolveMask;
    #endif
    #if _REPLACEMENT
        sampler2D _ObstructionCurveGlobal;
    #else
        sampler2D _ObstructionCurve;
    #endif

    #if _REPLACEMENT
        sampler2D _CrossSectionTextureGlobal;
    #else
        sampler2D _CrossSectionTexture;
    #endif
#ifdef USE_UNITY_TEXTURE_2D_TYPE
#undef USE_UNITY_TEXTURE_2D_TYPE
#endif

#include "Packages/com.shadercrew.seethroughshader.core/Scripts/Shaders/xxSharedSTSDependencies/SeeThroughShaderFunction.hlsl"
	void Ext_SurfaceFunction2 (inout Surface o, ShaderData d)
	{
        half3 albedo;
        half3 emission;
        float alphaForClipping;
#ifdef USE_UNITY_TEXTURE_2D_TYPE
#undef USE_UNITY_TEXTURE_2D_TYPE
#endif
#if _REPLACEMENT
    DoSeeThroughShading( o.Albedo, o.Normal, d.worldSpacePosition, d.worldSpaceNormal, d.screenPos,
                        _numOfPlayersInside, _tDirection, _tValue, _id,
                        _TriggerMode, _RaycastMode,
                        _IsExempt,

                        _DissolveMethodGlobal, _DissolveTexSpaceGlobal,

                        _DissolveColorGlobal, _DissolveColorSaturationGlobal, _UVsGlobal,
                        _DissolveEmissionGlobal, _DissolveEmissionBoosterGlobal, _TexturedEmissionEdgeGlobal, _TexturedEmissionEdgeStrengthGlobal,
                        _hasClippedShadowsGlobal,

                        _ObstructionGlobal,
                        _ObstructionPlayerOffsetGlobal,
                        _AngleStrengthGlobal,
                        _ConeStrengthGlobal, _ConeObstructionDestroyRadiusGlobal,
                        _CylinderStrengthGlobal, _CylinderObstructionDestroyRadiusGlobal,
                        _CircleStrengthGlobal, _CircleObstructionDestroyRadiusGlobal,
                        _CurveStrengthGlobal, _CurveObstructionDestroyRadiusGlobal,
                        _DissolveFallOffGlobal,
                        _DissolveMaskEnabledGlobal,
                        _AffectedAreaPlayerBasedObstructionGlobal,
                        _IntrinsicDissolveStrengthGlobal,
                        _DefaultEffectRadiusGlobal, _EnableDefaultEffectRadiusGlobal,

                        _IsometricExclusionGlobal, _IsometricExclusionDistanceGlobal, _IsometricExclusionGradientLengthGlobal,
                        _CeilingGlobal, _CeilingModeGlobal, _CeilingBlendModeGlobal, _CeilingYGlobal, _CeilingPlayerYOffsetGlobal, _CeilingYGradientLengthGlobal,
                        _FloorGlobal, _FloorModeGlobal, _FloorYGlobal, _PlayerPosYOffsetGlobal, _FloorYTextureGradientLengthGlobal, _AffectedAreaFloorGlobal,

                        _AnimationEnabledGlobal, _AnimationSpeedGlobal,
                        _TransitionDurationGlobal,

                        _UseCustomTimeGlobal,

                        _ZoningGlobal, _ZoningModeGlobal, _IsZoningRevealableGlobal, _ZoningEdgeGradientLengthGlobal,
                        _SyncZonesWithFloorYGlobal, _SyncZonesFloorYOffsetGlobal,

                        _PreviewModeGlobal,
                        _PreviewIndicatorLineThicknessGlobal,

                        _DissolveTexGlobal,
                        _DissolveMaskGlobal,
                        _ObstructionCurveGlobal,

                        _DissolveTexGlobal_TexelSize,
                        _DissolveMaskGlobal_TexelSize,
                        _ObstructionCurveGlobal_TexelSize,

                        albedo, emission, alphaForClipping);
#else 
    DoSeeThroughShading( o.Albedo, o.Normal, d.worldSpacePosition, d.worldSpaceNormal, d.screenPos,

                        _numOfPlayersInside, _tDirection, _tValue, _id,
                        _TriggerMode, _RaycastMode,
                        _IsExempt,

                        _DissolveMethod, _DissolveTexSpace,

                        _DissolveColor, _DissolveColorSaturation, _UVs,
                        _DissolveEmission, _DissolveEmissionBooster, _TexturedEmissionEdge, _TexturedEmissionEdgeStrength,
                        _hasClippedShadows,

                        _Obstruction,
                        _ObstructionPlayerOffset,
                        _AngleStrength,
                        _ConeStrength, _ConeObstructionDestroyRadius,
                        _CylinderStrength, _CylinderObstructionDestroyRadius,
                        _CircleStrength, _CircleObstructionDestroyRadius,
                        _CurveStrength, _CurveObstructionDestroyRadius,
                        _DissolveFallOff,
                        _DissolveMaskEnabled,
                        _AffectedAreaPlayerBasedObstruction,
                        _IntrinsicDissolveStrength,
                        _DefaultEffectRadius, _EnableDefaultEffectRadius,

                        _IsometricExclusion, _IsometricExclusionDistance, _IsometricExclusionGradientLength,
                        _Ceiling, _CeilingMode, _CeilingBlendMode, _CeilingY, _CeilingPlayerYOffset, _CeilingYGradientLength,
                        _Floor, _FloorMode, _FloorY, _PlayerPosYOffset, _FloorYTextureGradientLength, _AffectedAreaFloor,

                        _AnimationEnabled, _AnimationSpeed,
                        _TransitionDuration,

                        _UseCustomTime,

                        _Zoning, _ZoningMode , _IsZoningRevealable, _ZoningEdgeGradientLength,
                        _SyncZonesWithFloorY, _SyncZonesFloorYOffset,

                        _PreviewMode,
                        _PreviewIndicatorLineThickness,                            

                        _DissolveTex,
                        _DissolveMask,
                        _ObstructionCurve,

                        _DissolveTex_TexelSize,
                        _DissolveMask_TexelSize,
                        _ObstructionCurve_TexelSize,

                        albedo, emission, alphaForClipping);
#endif 
        o.Albedo = albedo;
        o.Emission += emission;   

	}
    void Ext_FinalColorForward2 (Surface o, ShaderData d, inout half4 color)
    {
        #if _REPLACEMENT   
            DoCrossSection(_CrossSectionEnabledGlobal,
                        _CrossSectionColorGlobal,
                        _CrossSectionTextureEnabledGlobal,
                        _CrossSectionTextureGlobal,
                        _CrossSectionTextureScaleGlobal,
                        _CrossSectionUVScaledByDistanceGlobal,
                        d.isFrontFace,
                        d.screenPos,
                        color);
        #else 
            DoCrossSection(_CrossSectionEnabled,
                        _CrossSectionColor,
                        _CrossSectionTextureEnabled,
                        _CrossSectionTexture,
                        _CrossSectionTextureScale,
                        _CrossSectionUVScaledByDistance,
                        d.isFrontFace,
                        d.screenPos,
                        color);
        #endif
    }
            void ChainSurfaceFunction(inout Surface l, inout ShaderData d)
            {
                  Ext_SurfaceFunction1(l, d);
                  Ext_SurfaceFunction2(l, d);
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
                  Ext_FinalColorForward2(l, d, color);
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
             d.screenUV = (IN.ScreenPosition.xy / max(0.01, IN.ScreenPosition.w));
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
             d.isFrontFace = facing;
            #if _HDRP
            #else
            #endif
             d.screenPos = i.screenPos;
             d.screenUV = (i.screenPos.xy / i.screenPos.w);
             d.extraV2F0 = i.extraV2F0;
            return d;
         }

#endif
         #if defined(_PASSSHADOW)
            float3 _LightDirection;
            float3 _LightPosition;
         #endif
         VertexToPixel Vert (VertexData v)
         {
           VertexToPixel o = (VertexToPixel)0;

           UNITY_SETUP_INSTANCE_ID(v);
           UNITY_TRANSFER_INSTANCE_ID(v, o);
           UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
#if !_TESSELLATION_ON
           ChainModifyVertex(v, o, _Time);
#endif

            o.texcoord0 = v.texcoord0;
           VertexPositionInputs vertexInput = GetVertexPositionInputs(v.vertex.xyz);
           o.worldPos = TransformObjectToWorld(v.vertex.xyz);
           o.worldNormal = TransformObjectToWorldNormal(v.normal);
           o.worldTangent = float4(TransformObjectToWorldDir(v.tangent.xyz), v.tangent.w);
          #if _PASSSHADOW
              #if _CASTING_PUNCTUAL_LIGHT_SHADOW
                 float3 lightDirectionWS = normalize(_LightPosition - o.worldPos);
              #else
                 float3 lightDirectionWS = _LightDirection;
              #endif
              o.pos = TransformWorldToHClip(ApplyShadowBias(o.worldPos, o.worldNormal, lightDirectionWS));
              #if UNITY_REVERSED_Z
                  o.pos.z = min(o.pos.z, UNITY_NEAR_CLIP_VALUE);
              #else
                  o.pos.z = max(o.pos.z, UNITY_NEAR_CLIP_VALUE);
              #endif
          #elif _PASSMETA
              o.pos = MetaVertexPosition(float4(v.vertex.xyz, 0), v.texcoord1.xy, v.texcoord2.xy, unity_LightmapST, unity_DynamicLightmapST);
          #else
              o.pos = TransformWorldToHClip(o.worldPos);
          #endif
           o.screenPos = ComputeScreenPos(o.pos, _ProjectionParams.x);
          #if _PASSFORWARD || _PASSGBUFFER
              float2 uv1 = v.texcoord1.xy;
              OUTPUT_LIGHTMAP_UV(uv1, unity_LightmapST, o.lightmapUV);
              OUTPUT_SH(o.worldNormal, o.sh);
              #if defined(DYNAMICLIGHTMAP_ON)
                   o.dynamicLightmapUV.xy = v.texcoord2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
              #endif
          #endif

          #ifdef VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
              half fogFactor = 0;
              #if defined(_FOG_FRAGMENT)
                fogFactor = ComputeFogFactor(o.pos.z);
              #endif
              #if _BAKEDLIT
                 o.fogFactorAndVertexLight = half4(fogFactor, 0, 0, 0);
              #else
                 half3 vertexLight = VertexLighting(o.worldPos, o.worldNormal);
                 o.fogFactorAndVertexLight = half4(fogFactor, vertexLight);
              #endif
          #endif

          #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
             o.shadowCoord = GetShadowCoord(vertexInput);
          #endif

           return o;
         }
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/UnityGBuffer.hlsl"
            FragmentOutput Frag (VertexToPixel IN
            #ifdef _DEPTHOFFSET_ON
              , out float outputDepth : SV_Depth
            #endif
            #if NEED_FACING
               , bool facing : SV_IsFrontFace
            #endif
            ) 
            {
               UNITY_SETUP_INSTANCE_ID(IN);
               UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);

               #if defined(LOD_FADE_CROSSFADE)
                  LODFadeCrossFade(IN.pos);
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

               #if _USESPECULAR || _SIMPLELIT
                  float3 specular = l.Specular;
                  float metallic = 0;
               #else   
                  float3 specular = 0;
                  float metallic = l.Metallic;
               #endif

               InputData inputData = (InputData)0;

               inputData.positionWS = IN.worldPos;
               #if _WORLDSPACENORMAL
                  inputData.normalWS = l.Normal;
               #else
                  inputData.normalWS = normalize(TangentToWorldSpace(d, l.Normal));
               #endif

               inputData.viewDirectionWS = SafeNormalize(d.worldSpaceViewDir);
               #if defined(MAIN_LIGHT_CALCULATE_SHADOWS)
                   inputData.shadowCoord = TransformWorldToShadowCoord(inputData.positionWS);
               #else
                   inputData.shadowCoord = float4(0, 0, 0, 0);
               #endif
               InitializeInputDataFog(float4(IN.worldPos, 1.0), IN.fogFactorAndVertexLight.x);
               inputData.vertexLighting = IN.fogFactorAndVertexLight.yzw;
               #if defined(_OVERRIDE_BAKEDGI)
                  inputData.bakedGI = l.DiffuseGI;
                  l.Emission += l.SpecularGI;
               #else
                  #if defined(DYNAMICLIGHTMAP_ON)
                   inputData.bakedGI = SAMPLE_GI(IN.lightmapUV, IN.dynamicLightmapUV.xy, IN.sh, inputData.normalWS);
                  #else
                      inputData.bakedGI = SAMPLE_GI(IN.lightmapUV, IN.sh, inputData.normalWS);
                  #endif
               #endif

               inputData.normalizedScreenSpaceUV = GetNormalizedScreenSpaceUV(IN.pos);
               inputData.shadowMask = SAMPLE_SHADOWMASK(IN.lightmapUV);

               #if defined(DEBUG_DISPLAY)
                   #if defined(DYNAMICLIGHTMAP_ON)
                     inputData.dynamicLightmapUV = IN.dynamicLightmapUV.xy;
                   #endif
                   #if defined(LIGHTMAP_ON)
                     inputData.staticLightmapUV = IN.lightmapUV;
                   #else
                     inputData.vertexSH = IN.sh;
                   #endif
               #endif

               #ifdef _DBUFFER
                   ApplyDecal(IN.pos,
                       l.Albedo,
                       specular,
                       inputData.normalWS,
                       metallic,
                       l.Occlusion,
                       l.Smoothness);
               #endif

               BRDFData brdfData;
               InitializeBRDFData(l.Albedo, metallic, specular, l.Smoothness, l.Alpha, brdfData);
               Light mainLight = GetMainLight(inputData.shadowCoord, inputData.positionWS, inputData.shadowMask);
               MixRealtimeAndBakedGI(mainLight, inputData.normalWS, inputData.bakedGI, inputData.shadowMask);
               half3 color = GlobalIllumination(brdfData, inputData.bakedGI, l.Occlusion, inputData.positionWS, inputData.normalWS, inputData.viewDirectionWS);

               return BRDFDataToGbuffer(brdfData, inputData, l.Smoothness, l.Emission + color, l.Occlusion);
            }

         ENDHLSL

      }
        Pass
        {
            Name "ShadowCaster"
            Tags 
            { 
                "LightMode" = "ShadowCaster"
            }
            Blend One Zero, One Zero
            Cull Back
            ZTest LEqual
            ZWrite On
                Cull [_Cull]
            HLSLPROGRAM

               #pragma vertex Vert
   #pragma fragment Frag

            #pragma target 3.0

            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma multi_compile_instancing
            #pragma multi_compile_vertex _ _CASTING_PUNCTUAL_LIGHT_SHADOW
            #pragma multi_compile_fragment _ LOD_FADE_CROSSFADE
            #define _NORMAL_DROPOFF_TS 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define _PASSSHADOW 1
     #pragma shader_feature_local_fragment  _SHADING_COLOR _SHADING_BLINNPHONG _SHADING_PBR    
    #pragma shader_feature_local _NORMALMAP
    #pragma shader_feature_local_fragment _EMISSION

    #pragma shader_feature_local_fragment _ALPHATEST_ON
    #pragma shader_feature_local _RECEIVE_SHADOWS_OFF
    #pragma shader_feature_local_fragment _ _SPECGLOSSMAP _SPECULAR_COLOR 
    #pragma shader_feature_local_fragment _GLOSSINESS_FROM_BASE_ALPHA

    #pragma shader_feature_local_fragment _OCCLUSIONMAP
    #pragma shader_feature_local _PARALLAXMAP
    #pragma shader_feature_local _ _DETAIL_MULX2 _DETAIL_SCALED
    #pragma shader_feature_local_fragment _SURFACE_TYPE_TRANSPARENT

    #pragma shader_feature_local_fragment _ _ALPHAPREMULTIPLY_ON _ALPHAMODULATE_ON

    #pragma shader_feature_local_fragment _METALLICSPECGLOSSMAP
    #pragma shader_feature_local_fragment _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
    #pragma shader_feature_local_fragment _SPECULARHIGHLIGHTS_OFF
    #pragma shader_feature_local_fragment _ENVIRONMENTREFLECTIONS_OFF
    #pragma shader_feature_local_fragment _SPECULAR_SETUP
        #pragma shader_feature_local_fragment _OBSTRUCTION_CURVE
        #pragma shader_feature_local_fragment _DISSOLVEMASK
        #pragma shader_feature_local_fragment _ZONING
        #pragma shader_feature_local_fragment _REPLACEMENT
        #pragma shader_feature_local_fragment _PLAYERINDEPENDENT
   #define _URP 1
#define NEED_FACING 1
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/LODCrossFade.hlsl"
                  #undef WorldNormalVector
      #define WorldNormalVector(data, normal) mul(normal, data.TBNMatrix)
      #define UnityObjectToWorldNormal(normal) mul(GetObjectToWorldMatrix(), normal)

      #define _WorldSpaceLightPos0 _MainLightPosition
      #define UNITY_DECLARE_TEX2D(name) TEXTURE2D(name); SAMPLER(sampler##name);
      #define UNITY_DECLARE_TEX2D_NOSAMPLER(name) TEXTURE2D(name);
      #define UNITY_DECLARE_TEX2DARRAY(name) TEXTURE2D_ARRAY(name); SAMPLER(sampler##name);
      #define UNITY_DECLARE_TEX2DARRAY_NOSAMPLER(name) TEXTURE2D_ARRAY(name);

      #define UNITY_SAMPLE_TEX2DARRAY(tex,coord)            SAMPLE_TEXTURE2D_ARRAY(tex, sampler##tex, coord.xy, coord.z)
      #define UNITY_SAMPLE_TEX2DARRAY_LOD(tex,coord,lod)    SAMPLE_TEXTURE2D_ARRAY_LOD(tex, sampler##tex, coord.xy, coord.z, lod)
      #define UNITY_SAMPLE_TEX2D(tex, coord)                SAMPLE_TEXTURE2D(tex, sampler##tex, coord)
      #define UNITY_SAMPLE_TEX2D_SAMPLER(tex, samp, coord)  SAMPLE_TEXTURE2D(tex, sampler##samp, coord)

      #define UNITY_SAMPLE_TEX2D_LOD(tex,coord, lod)   SAMPLE_TEXTURE2D_LOD(tex, sampler_##tex, coord, lod)
      #define UNITY_SAMPLE_TEX2D_SAMPLER_LOD(tex,samplertex,coord, lod) SAMPLE_TEXTURE2D_LOD (tex, sampler##samplertex,coord, lod)
      #if defined(UNITY_COMPILER_HLSL)
         #define UNITY_INITIALIZE_OUTPUT(type,name) name = (type)0;
      #else
         #define UNITY_INITIALIZE_OUTPUT(type,name)
      #endif

      #define sampler2D_float sampler2D
      #define sampler2D_half sampler2D
      struct VertexToPixel
      {
         float4 pos : SV_POSITION;
         float3 worldPos : TEXCOORD0;
         float3 worldNormal : TEXCOORD1;
         float4 worldTangent : TEXCOORD2;
          float4 texcoord0 : TEXCOORD3;
          float4 screenPos : TEXCOORD7;
         #if defined(LIGHTMAP_ON)
            float2 lightmapUV : TEXCOORD8;
         #endif
         #if defined(DYNAMICLIGHTMAP_ON)
            float2 dynamicLightmapUV : TEXCOORD9;
         #endif
         #if !defined(LIGHTMAP_ON)
            float3 sh : TEXCOORD10;
         #endif

         #if defined(VARYINGS_NEED_FOG_AND_VERTEX_LIGHT)
            float4 fogFactorAndVertexLight : TEXCOORD11;
         #endif

         #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
           float4 shadowCoord : TEXCOORD12;
         #endif
          float4 extraV2F0 : TEXCOORD13;
         #if UNITY_ANY_INSTANCING_ENABLED
         uint instanceID : CUSTOM_INSTANCE_ID;
         #endif
         #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
         uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
         #endif
         #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
         uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
         #endif
         #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
         FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
         #endif
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

               #undef GetWorldToObjectMatrix()

               #define GetWorldToObjectMatrix()   unity_WorldToObject
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
               float3 camView = mul((float3x3)GetObjectToWorldMatrix(), transpose(mul(GetWorldToObjectMatrix(), UNITY_MATRIX_I_V)) [2].xyz);

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
                  norms = mul((float3x3)GetWorldToViewMatrix(), norms) * 0.5 + 0.5;
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
            CBUFFER_START(UnityPerMaterial)
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
	half4 _BaseColor;
    float _BumpScale;
    float4 _BaseMap_ST;
    half _Cutoff;
    half4 _EmissionColor;
     half4 _SpecColor;
     float _Smoothness;
	float4 _DetailAlbedoMap_ST;
	half _Metallic;
	half _Parallax;
	half _OcclusionStrength;
	half _DetailAlbedoMapScale;
	half _DetailNormalMapScale;
#if UNITY_VERSION < 202200
	float _AlphaToMaskAvailable;
#endif
    float _SyncCullMode;

    float _IsReplacementShader;
    float _TriggerMode;
    float _RaycastMode;
    float _IsExempt;
    float _isReferenceMaterial;
    float _InteractionMode;

    float _tDirection = 0;
    float _numOfPlayersInside = 0;
    float _tValue = 0;
    float _id = 0;
    half _TextureVisibility;
    half _AngleStrength;
    float _Obstruction;
    half _ObstructionPlayerOffset;

    float _UVs;
    float4 _ObstructionCurve_TexelSize;      
    float _DissolveMaskEnabled;
    float4 _DissolveMask_TexelSize;
    float4 _DissolveTex_TexelSize;
    half4 _DissolveColor;
    float _DissolveColorSaturation;
    float _DissolveEmission;
    float _DissolveEmissionBooster;
    float _hasClippedShadows;
    float _ConeStrength;
    float _ConeObstructionDestroyRadius;
    float _CylinderStrength;
    float _CylinderObstructionDestroyRadius;
    float _CircleStrength;
    float _CircleObstructionDestroyRadius;
    float _CurveStrength;
    float _CurveObstructionDestroyRadius;
    float _IntrinsicDissolveStrength;
    float _DissolveFallOff;
    float _AffectedAreaPlayerBasedObstruction;
    float _PreviewMode;
    float _PreviewIndicatorLineThickness;
    float _AnimationEnabled;
    float _AnimationSpeed;
    float _DefaultEffectRadius;
    float _EnableDefaultEffectRadius;
    float _TransitionDuration;
    float _TexturedEmissionEdge;
    float _TexturedEmissionEdgeStrength;
    float _IsometricExclusion;
    float _IsometricExclusionDistance;
    float _IsometricExclusionGradientLength;
    float _Floor;
    float _FloorMode;
    float _FloorY;
    float _FloorYTextureGradientLength;
    float _PlayerPosYOffset;
    float _AffectedAreaFloor;
    float _Ceiling;
    float _CeilingMode;
    float _CeilingBlendMode;
    float _CeilingY;
    float _CeilingPlayerYOffset;
    float _CeilingYGradientLength;
    float _Zoning;
    float _ZoningMode;
    float _ZoningEdgeGradientLength;
    float _IsZoningRevealable;
    float _SyncZonesWithFloorY;
    float _SyncZonesFloorYOffset;

    half _UseCustomTime;

    half _CrossSectionEnabled;
    half4 _CrossSectionColor;
    half _CrossSectionTextureEnabled;
    float _CrossSectionTextureScale;
    half _CrossSectionUVScaledByDistance;
    half _DissolveMethod;
    half _DissolveTexSpace;
            CBUFFER_END
    sampler2D _NoiseMap1;
    sampler2D _NoiseMap2;

    sampler2D _HatchingMap;
    sampler2D _GradientTex;
    void Ext_ModifyVertex0 (inout VertexData v, inout ExtraV2F d)
    {
        float4x4 UnityObjectToClipPosMatrix = mul(GetWorldToHClipMatrix(), GetObjectToWorldMatrix());
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
    TEXTURE2D(_BaseMap);        SAMPLER(sampler_BaseMap);
    TEXTURE2D(_BumpMap);        SAMPLER(sampler_BumpMap);
    TEXTURE2D(_EmissionMap);    SAMPLER(sampler_EmissionMap);
    TEXTURE2D(_SpecGlossMap);       SAMPLER(sampler_SpecGlossMap);  
	TEXTURE2D(_ParallaxMap);        SAMPLER(sampler_ParallaxMap);
	TEXTURE2D(_OcclusionMap);       SAMPLER(sampler_OcclusionMap);
	TEXTURE2D(_DetailMask);         SAMPLER(sampler_DetailMask);
	TEXTURE2D(_DetailAlbedoMap);    SAMPLER(sampler_DetailAlbedoMap);
	TEXTURE2D(_DetailNormalMap);    SAMPLER(sampler_DetailNormalMap);
	TEXTURE2D(_MetallicGlossMap);   SAMPLER(sampler_MetallicGlossMap);
	TEXTURE2D(_ClearCoatMap);       SAMPLER(sampler_ClearCoatMap);
    #if defined(_DETAIL_MULX2) || defined(_DETAIL_SCALED)
		#define _DETAIL
	#endif

	#if _SPECULAR_SETUP
		#define _USESPECULAR 1
	#else
		#undef _USESPECULAR
	#endif
	float SharpenAlphaMy(float alpha, float alphaClipTreshold)
	{
	    return saturate((alpha - alphaClipTreshold) / max(fwidth(alpha), 0.0001) + 0.5);
	}
#if UNITY_VERSION < 202200
	half3 AlphaModulate(half3 albedo, half alpha)
	{
		#if defined(_ALPHAMODULATE_ON)
		    return lerp(half3(1.0, 1.0, 1.0), albedo, alpha);
		#else
		    return albedo;
		#endif
	}

	#if defined(_ALPHATEST_ON)
		bool IsAlphaToMaskAvailable()
		{
		    return (_AlphaToMaskAvailable != 0.0);
		}
		half AlphaClip(half alpha, half cutoff)
		{
		    half clippedAlpha = (alpha >= cutoff) ? float(alpha) : 0.0;

		    half alphaToCoverageAlpha = SharpenAlphaMy(alpha, cutoff);

		    alpha = IsAlphaToMaskAvailable() ? alphaToCoverageAlpha : clippedAlpha;
		    clip(alpha - 0.0001);

		    return alpha;
		}
	#endif
#endif
    int _DebugSceneOverrideMode;
    bool IsAlphaDiscardEnabledMy()
    {
        #if defined(DEBUG_DISPLAY)
        return (_DebugSceneOverrideMode == DEBUGSCENEOVERRIDEMODE_NONE);
        #else
        return true;
        #endif
    }
	half Alpha(half albedoAlpha, half4 color, half cutoff)
	{
		#if !defined(_SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A) && !defined(_GLOSSINESS_FROM_BASE_ALPHA)
		    half alpha = albedoAlpha * color.a;
		#else
		    half alpha = color.a;
		#endif
			#ifdef _ALPHATEST_ON
			    if (IsAlphaDiscardEnabledMy())
			        alpha = AlphaClip(alpha, cutoff);
			#endif

		    return alpha;
	}
 	half4 SampleAlbedoAlpha(float2 uv, TEXTURE2D_PARAM(albedoAlphaMap, sampler_albedoAlphaMap))
	{
	    return half4(SAMPLE_TEXTURE2D(albedoAlphaMap, sampler_albedoAlphaMap, uv));
	}
 	half3 SampleNormal(float2 uv, TEXTURE2D_PARAM(bumpMap, sampler_bumpMap), half scale = half(1.0))
	{
	#ifdef _NORMALMAP
	    half4 n = SAMPLE_TEXTURE2D(bumpMap, sampler_bumpMap, uv);
	    #if BUMP_SCALE_NOT_SUPPORTED
	        return UnpackNormal(n);
	    #else
	        return UnpackNormalScale(n, scale);
	    #endif
	#else
	    return half3(0.0h, 0.0h, 1.0h);
	#endif
	}
#if UNITY_VERSION < 202100
	half4 SampleSpecularSmoothnessMy(half2 uv, half alpha, half4 specColor, TEXTURE2D_PARAM( specMap, sampler_specMap))
	{
		half4 specularSmoothness = half4(0.0h, 0.0h, 0.0h, 1.0h);
		#ifdef _SPECGLOSSMAP
			specularSmoothness = SAMPLE_TEXTURE2D(specMap, sampler_specMap, uv) * specColor;
		#elif defined(_SPECULAR_COLOR)
			specularSmoothness = specColor;
		#endif

		#ifdef _GLOSSINESS_FROM_BASE_ALPHA
			specularSmoothness.a = exp2(10 * alpha + 1);
		#else
			specularSmoothness.a = exp2(10 * specularSmoothness.a + 1);
		#endif
		return specularSmoothness;
	}
#else

	half4 SampleSpecularSmoothnessMy(float2 uv, half alpha, half4 specColor, TEXTURE2D_PARAM(specMap, sampler_specMap))
	{
		half4 specularSmoothness = half4(0, 0, 0, 1);
	#ifdef _SPECGLOSSMAP
		specularSmoothness = SAMPLE_TEXTURE2D(specMap, sampler_specMap, uv) * specColor;
	#elif defined(_SPECULAR_COLOR)
		specularSmoothness = specColor;
	#endif

	#ifdef _GLOSSINESS_FROM_BASE_ALPHA
		specularSmoothness.a = alpha;
	#endif

		return specularSmoothness;
	}
#endif
    half3 SampleEmissionMy(float2 uv, half3 emissionColor, TEXTURE2D_PARAM(emissionMap, sampler_emissionMap))
    {
    #ifndef _EMISSION
        return 0;
    #else
        return SAMPLE_TEXTURE2D(emissionMap, sampler_emissionMap, uv).rgb * emissionColor;
    #endif
    }
	#ifndef BUILTIN_TARGET_API
		half2 ParallaxOffset1Step(half height, half amplitude, half3 viewDirTS)
		{
		    height = height * amplitude - amplitude / 2.0;
		    half3 v = normalize(viewDirTS);
		    v.z += 0.42;
		    return height * (v.xy / v.z);
		}
	#endif

	float2 ParallaxMapping(TEXTURE2D_PARAM(heightMap, sampler_heightMap), half3 viewDirTS, half scale, float2 uv)
	{
	    half h = SAMPLE_TEXTURE2D(heightMap, sampler_heightMap, uv).g;
	    float2 offset = ParallaxOffset1Step(h, scale, viewDirTS);
	    return offset;
	}
	#ifdef _SPECULAR_SETUP
	    #define SAMPLE_METALLICSPECULAR(uv) SAMPLE_TEXTURE2D(_SpecGlossMap, sampler_SpecGlossMap, uv)
	#else
	    #define SAMPLE_METALLICSPECULAR(uv) SAMPLE_TEXTURE2D(_MetallicGlossMap, sampler_MetallicGlossMap, uv)
	#endif

	half4 SampleMetallicSpecGloss(float2 uv, half albedoAlpha)
	{
	    half4 specGloss;

		#ifdef _METALLICSPECGLOSSMAP
		    specGloss = half4(SAMPLE_METALLICSPECULAR(uv));
		    #ifdef _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
		        specGloss.a = albedoAlpha * _Smoothness;
		    #else
		        specGloss.a *= _Smoothness;
		    #endif
		#else 
		    #if _SPECULAR_SETUP
		        specGloss.rgb = _SpecColor.rgb;
		    #else
		        specGloss.rgb = _Metallic.rrr;
		    #endif

		    #ifdef _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
		        specGloss.a = albedoAlpha * _Smoothness;
		    #else
		        specGloss.a = _Smoothness;
		    #endif
		#endif

	    return specGloss;
	}

	half SampleOcclusion(float2 uv)
	{
	    #ifdef _OCCLUSIONMAP
	        half occ = SAMPLE_TEXTURE2D(_OcclusionMap, sampler_OcclusionMap, uv).g;
	        return LerpWhiteTo(occ, _OcclusionStrength);
	    #else
	        return half(1.0);
	    #endif
	}
    half SampleOcclusionMy(float2 uv)
    {
    #ifdef _OCCLUSIONMAP
    #if defined(SHADER_API_GLES)
        return SAMPLE_TEXTURE2D(_OcclusionMap, sampler_OcclusionMap, uv).g;
    #else
        half occ = SAMPLE_TEXTURE2D(_OcclusionMap, sampler_OcclusionMap, uv).g;
        return LerpWhiteTo(occ, _OcclusionStrength);
    #endif
    #else
        return 1.0;
    #endif
    }
	half2 SampleClearCoat(float2 uv)
	{
		#if defined(_CLEARCOAT) || defined(_CLEARCOATMAP)
		    half2 clearCoatMaskSmoothness = half2(_ClearCoatMask, _ClearCoatSmoothness);

		#if defined(_CLEARCOATMAP)
		    clearCoatMaskSmoothness *= SAMPLE_TEXTURE2D(_ClearCoatMap, sampler_ClearCoatMap, uv).rg;
		#endif

		    return clearCoatMaskSmoothness;
		#else
		    return half2(0.0, 1.0);
		#endif  
	}

	void ApplyPerPixelDisplacement(half3 viewDirTS, inout float2 uv)
	{
		#if defined(_PARALLAXMAP)
		    uv += ParallaxMapping(TEXTURE2D_ARGS(_ParallaxMap, sampler_ParallaxMap), viewDirTS, _Parallax, uv);
		#endif
	}

	half3 ScaleDetailAlbedo(half3 detailAlbedo, half scale)
	{
	    return half(2.0) * detailAlbedo * scale - scale + half(1.0);
	}

	half3 ApplyDetailAlbedo(float2 detailUv, half3 albedo, half detailMask)
	{
		#if defined(_DETAIL)
		    half3 detailAlbedo = SAMPLE_TEXTURE2D(_DetailAlbedoMap, sampler_DetailAlbedoMap, detailUv).rgb;
		#if defined(_DETAIL_SCALED)
		    detailAlbedo = ScaleDetailAlbedo(detailAlbedo, _DetailAlbedoMapScale);
		#else
		    detailAlbedo = half(2.0) * detailAlbedo;
		#endif

		    return albedo * LerpWhiteTo(detailAlbedo, detailMask);
		#else
		    return albedo;
		#endif
	}

	half3 ApplyDetailNormal(float2 detailUv, half3 normalTS, half detailMask)
	{
		#if defined(_DETAIL)
		#if BUMP_SCALE_NOT_SUPPORTED
		    half3 detailNormalTS = UnpackNormal(SAMPLE_TEXTURE2D(_DetailNormalMap, sampler_DetailNormalMap, detailUv));
		#else
		    half3 detailNormalTS = UnpackNormalScale(SAMPLE_TEXTURE2D(_DetailNormalMap, sampler_DetailNormalMap, detailUv), _DetailNormalMapScale);
		#endif
		    detailNormalTS = normalize(detailNormalTS);

		    return lerp(normalTS, BlendNormalRNM(normalTS, detailNormalTS), detailMask); 
		#else
		    return normalTS;
		#endif
	}
	void Ext_SurfaceFunction1 (inout Surface o, ShaderData d)
	{

        float4 texcoords;
        texcoords.xy = d.texcoord0.xy * _BaseMap_ST.xy + _BaseMap_ST.zw; 
        float2 uv = texcoords.xy;

#if _SHADING_BLINNPHONG || _SHADING_COLOR
        half4 diffuseAlpha = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap,  texcoords.xy);
        o.Albedo = diffuseAlpha.rgb * _BaseColor.rgb;
	#if !_PASSFORWARD
		if(_EnableToonShading == 0 || _ShadingAffectedByNormalMap == 1) 
		{
			o.Normal = SampleNormal(texcoords.xy, TEXTURE2D_ARGS(_BumpMap, sampler_BumpMap), _BumpScale);
		}
	#else
        o.Normal = SampleNormal(texcoords.xy, TEXTURE2D_ARGS(_BumpMap, sampler_BumpMap), _BumpScale);
	#endif
		o.Alpha = diffuseAlpha.a;
        o.Alpha = o.Alpha * _BaseColor.a;
        AlphaDiscard(o.Alpha, _Cutoff);
        half3 emission = SampleEmissionMy(uv, _EmissionColor.rgb, TEXTURE2D_ARGS(_EmissionMap, sampler_EmissionMap));
        o.Emission = emission;
#endif

      if(_ShadingMode == 1 && _LightFunction == 0 ) 
      {
#if _SHADING_BLINNPHONG
        half4 specular = SampleSpecularSmoothnessMy(uv, o.Alpha, _SpecColor, TEXTURE2D_ARGS(_SpecGlossMap, sampler_SpecGlossMap));
        half smoothness =  specular.a;

        o.Specular = specular;
        o.Smoothness = smoothness;
#endif

      } 
      else if (_ShadingMode == 1 && _LightFunction == 1) 
      {
#if _SHADING_PBR
		#if defined(_PARALLAXMAP)
		    ApplyPerPixelDisplacement(d.tangentSpaceViewDir, uv);
		#endif
    	half4 albedoAlpha =  SampleAlbedoAlpha(uv, TEXTURE2D_ARGS(_BaseMap, sampler_BaseMap));
	    o.Alpha = Alpha(albedoAlpha.a, _BaseColor, _Cutoff);

	    half4 specGloss = SampleMetallicSpecGloss(uv, albedoAlpha.a);
	    o.Albedo = albedoAlpha.rgb * _BaseColor.rgb;
	    o.Albedo = AlphaModulate(o.Albedo, o.Alpha);
		#if _SPECULAR_SETUP
		    o.Metallic = half(1.0);
		    o.Specular = specGloss.rgb;
		#else
		    o.Metallic = specGloss.r;
		    o.Specular = half3(0.0, 0.0, 0.0);
		#endif
        o.Smoothness = specGloss.a;
	#if !_PASSFORWARD
		if(_EnableToonShading == 0 || _ShadingAffectedByNormalMap == 1) 
		{
			o.Normal = SampleNormal(uv, TEXTURE2D_ARGS(_BumpMap, sampler_BumpMap), _BumpScale);
		}
	#else
		o.Normal = SampleNormal(uv, TEXTURE2D_ARGS(_BumpMap, sampler_BumpMap), _BumpScale);
	#endif
	    o.Occlusion = SampleOcclusion(uv);
	    o.Emission = SampleEmissionMy(uv, _EmissionColor.rgb, TEXTURE2D_ARGS(_EmissionMap, sampler_EmissionMap));

		#if defined(_DETAIL)
		    half detailMask = SAMPLE_TEXTURE2D(_DetailMask, sampler_DetailMask, uv).a;
		    float2 detailUv = uv * _DetailAlbedoMap_ST.xy + _DetailAlbedoMap_ST.zw;
		    o.Albedo = ApplyDetailAlbedo(detailUv, o.Albedo, detailMask);
		    o.Normal = ApplyDetailNormal(detailUv, o.Normal, detailMask);
		#endif

#endif
      }

	}
    float _ArrayLength = 0;
    #if (defined(SHADER_API_GLES) || defined(SHADER_API_GLES3)) 
        float4 _PlayersPosVectorArray[20];
        float _PlayersDataFloatArray[150];     
    #else
        float4 _PlayersPosVectorArray[100];
        float _PlayersDataFloatArray[500];  
    #endif
    #if _ZONING
        #if (defined(SHADER_API_GLES) || defined(SHADER_API_GLES3)) 
            float _ZDFA[500];
        #else
            float _ZDFA[1000];
        #endif
        float _ZonesDataCount;
    #endif

    float _STSCustomTime = 0;
    #if _REPLACEMENT        
        half4 _DissolveColorGlobal;
        float _DissolveColorSaturationGlobal;
        float _DissolveEmissionGlobal;
        float _DissolveEmissionBoosterGlobal;
        float _TextureVisibilityGlobal;
        float _ObstructionGlobal;
        float _AngleStrengthGlobal;
        float _ConeStrengthGlobal;
        float _ConeObstructionDestroyRadiusGlobal;
        float _CylinderStrengthGlobal;
        float _CylinderObstructionDestroyRadiusGlobal;
        float _CircleStrengthGlobal;
        float _CircleObstructionDestroyRadiusGlobal;
        float _CurveStrengthGlobal;
        float _CurveObstructionDestroyRadiusGlobal;
        float _DissolveFallOffGlobal;
        float _AffectedAreaPlayerBasedObstructionGlobal;
        float _IntrinsicDissolveStrengthGlobal;
        float _PreviewModeGlobal;
        float _UVsGlobal;
        float _hasClippedShadowsGlobal;
        float _FloorGlobal;
        float _FloorModeGlobal;
        float _FloorYGlobal;
        float _PlayerPosYOffsetGlobal;
        float _FloorYTextureGradientLengthGlobal;
        float _AffectedAreaFloorGlobal;
        float _AnimationEnabledGlobal;
        float _AnimationSpeedGlobal;
        float _DefaultEffectRadiusGlobal;
        float _EnableDefaultEffectRadiusGlobal;
        float _TransitionDurationGlobal;        
        float _TexturedEmissionEdgeGlobal;
        float _TexturedEmissionEdgeStrengthGlobal;
        float _IsometricExclusionGlobal;
        float _IsometricExclusionDistanceGlobal;
        float _IsometricExclusionGradientLengthGlobal;
        float _CeilingGlobal;
        float _CeilingModeGlobal;
        float _CeilingBlendModeGlobal;
        float _CeilingYGlobal;
        float _CeilingPlayerYOffsetGlobal;
        float _CeilingYGradientLengthGlobal;
        float _ZoningGlobal;
        float _ZoningModeGlobal;
        float _ZoningEdgeGradientLengthGlobal;
        float _IsZoningRevealableGlobal;
        float _SyncZonesWithFloorYGlobal;
        float _SyncZonesFloorYOffsetGlobal;
        float4 _ObstructionCurveGlobal_TexelSize;
        float4 _DissolveMaskGlobal_TexelSize;
        float _DissolveMaskEnabledGlobal;
        float _PreviewIndicatorLineThicknessGlobal;
        half _UseCustomTimeGlobal;

        half _CrossSectionEnabledGlobal;
        half4 _CrossSectionColorGlobal;
        half _CrossSectionTextureEnabledGlobal;
        float _CrossSectionTextureScaleGlobal;
        half _CrossSectionUVScaledByDistanceGlobal;

        half _DissolveMethodGlobal;
        half _DissolveTexSpaceGlobal;
        half _ObstructionPlayerOffsetGlobal;

        float4 _DissolveTexGlobal_TexelSize;
    #endif
    #if _REPLACEMENT
        sampler2D _DissolveTexGlobal;
    #else
        sampler2D _DissolveTex;
    #endif
    #if _REPLACEMENT
        sampler2D _DissolveMaskGlobal;
    #else
        sampler2D _DissolveMask;
    #endif
    #if _REPLACEMENT
        sampler2D _ObstructionCurveGlobal;
    #else
        sampler2D _ObstructionCurve;
    #endif

    #if _REPLACEMENT
        sampler2D _CrossSectionTextureGlobal;
    #else
        sampler2D _CrossSectionTexture;
    #endif
#ifdef USE_UNITY_TEXTURE_2D_TYPE
#undef USE_UNITY_TEXTURE_2D_TYPE
#endif

#include "Packages/com.shadercrew.seethroughshader.core/Scripts/Shaders/xxSharedSTSDependencies/SeeThroughShaderFunction.hlsl"
	void Ext_SurfaceFunction2 (inout Surface o, ShaderData d)
	{
        half3 albedo;
        half3 emission;
        float alphaForClipping;
#ifdef USE_UNITY_TEXTURE_2D_TYPE
#undef USE_UNITY_TEXTURE_2D_TYPE
#endif
#if _REPLACEMENT
    DoSeeThroughShading( o.Albedo, o.Normal, d.worldSpacePosition, d.worldSpaceNormal, d.screenPos,
                        _numOfPlayersInside, _tDirection, _tValue, _id,
                        _TriggerMode, _RaycastMode,
                        _IsExempt,

                        _DissolveMethodGlobal, _DissolveTexSpaceGlobal,

                        _DissolveColorGlobal, _DissolveColorSaturationGlobal, _UVsGlobal,
                        _DissolveEmissionGlobal, _DissolveEmissionBoosterGlobal, _TexturedEmissionEdgeGlobal, _TexturedEmissionEdgeStrengthGlobal,
                        _hasClippedShadowsGlobal,

                        _ObstructionGlobal,
                        _ObstructionPlayerOffsetGlobal,
                        _AngleStrengthGlobal,
                        _ConeStrengthGlobal, _ConeObstructionDestroyRadiusGlobal,
                        _CylinderStrengthGlobal, _CylinderObstructionDestroyRadiusGlobal,
                        _CircleStrengthGlobal, _CircleObstructionDestroyRadiusGlobal,
                        _CurveStrengthGlobal, _CurveObstructionDestroyRadiusGlobal,
                        _DissolveFallOffGlobal,
                        _DissolveMaskEnabledGlobal,
                        _AffectedAreaPlayerBasedObstructionGlobal,
                        _IntrinsicDissolveStrengthGlobal,
                        _DefaultEffectRadiusGlobal, _EnableDefaultEffectRadiusGlobal,

                        _IsometricExclusionGlobal, _IsometricExclusionDistanceGlobal, _IsometricExclusionGradientLengthGlobal,
                        _CeilingGlobal, _CeilingModeGlobal, _CeilingBlendModeGlobal, _CeilingYGlobal, _CeilingPlayerYOffsetGlobal, _CeilingYGradientLengthGlobal,
                        _FloorGlobal, _FloorModeGlobal, _FloorYGlobal, _PlayerPosYOffsetGlobal, _FloorYTextureGradientLengthGlobal, _AffectedAreaFloorGlobal,

                        _AnimationEnabledGlobal, _AnimationSpeedGlobal,
                        _TransitionDurationGlobal,

                        _UseCustomTimeGlobal,

                        _ZoningGlobal, _ZoningModeGlobal, _IsZoningRevealableGlobal, _ZoningEdgeGradientLengthGlobal,
                        _SyncZonesWithFloorYGlobal, _SyncZonesFloorYOffsetGlobal,

                        _PreviewModeGlobal,
                        _PreviewIndicatorLineThicknessGlobal,

                        _DissolveTexGlobal,
                        _DissolveMaskGlobal,
                        _ObstructionCurveGlobal,

                        _DissolveTexGlobal_TexelSize,
                        _DissolveMaskGlobal_TexelSize,
                        _ObstructionCurveGlobal_TexelSize,

                        albedo, emission, alphaForClipping);
#else 
    DoSeeThroughShading( o.Albedo, o.Normal, d.worldSpacePosition, d.worldSpaceNormal, d.screenPos,

                        _numOfPlayersInside, _tDirection, _tValue, _id,
                        _TriggerMode, _RaycastMode,
                        _IsExempt,

                        _DissolveMethod, _DissolveTexSpace,

                        _DissolveColor, _DissolveColorSaturation, _UVs,
                        _DissolveEmission, _DissolveEmissionBooster, _TexturedEmissionEdge, _TexturedEmissionEdgeStrength,
                        _hasClippedShadows,

                        _Obstruction,
                        _ObstructionPlayerOffset,
                        _AngleStrength,
                        _ConeStrength, _ConeObstructionDestroyRadius,
                        _CylinderStrength, _CylinderObstructionDestroyRadius,
                        _CircleStrength, _CircleObstructionDestroyRadius,
                        _CurveStrength, _CurveObstructionDestroyRadius,
                        _DissolveFallOff,
                        _DissolveMaskEnabled,
                        _AffectedAreaPlayerBasedObstruction,
                        _IntrinsicDissolveStrength,
                        _DefaultEffectRadius, _EnableDefaultEffectRadius,

                        _IsometricExclusion, _IsometricExclusionDistance, _IsometricExclusionGradientLength,
                        _Ceiling, _CeilingMode, _CeilingBlendMode, _CeilingY, _CeilingPlayerYOffset, _CeilingYGradientLength,
                        _Floor, _FloorMode, _FloorY, _PlayerPosYOffset, _FloorYTextureGradientLength, _AffectedAreaFloor,

                        _AnimationEnabled, _AnimationSpeed,
                        _TransitionDuration,

                        _UseCustomTime,

                        _Zoning, _ZoningMode , _IsZoningRevealable, _ZoningEdgeGradientLength,
                        _SyncZonesWithFloorY, _SyncZonesFloorYOffset,

                        _PreviewMode,
                        _PreviewIndicatorLineThickness,                            

                        _DissolveTex,
                        _DissolveMask,
                        _ObstructionCurve,

                        _DissolveTex_TexelSize,
                        _DissolveMask_TexelSize,
                        _ObstructionCurve_TexelSize,

                        albedo, emission, alphaForClipping);
#endif 
        o.Albedo = albedo;
        o.Emission += emission;   

	}
    void Ext_FinalColorForward2 (Surface o, ShaderData d, inout half4 color)
    {
        #if _REPLACEMENT   
            DoCrossSection(_CrossSectionEnabledGlobal,
                        _CrossSectionColorGlobal,
                        _CrossSectionTextureEnabledGlobal,
                        _CrossSectionTextureGlobal,
                        _CrossSectionTextureScaleGlobal,
                        _CrossSectionUVScaledByDistanceGlobal,
                        d.isFrontFace,
                        d.screenPos,
                        color);
        #else 
            DoCrossSection(_CrossSectionEnabled,
                        _CrossSectionColor,
                        _CrossSectionTextureEnabled,
                        _CrossSectionTexture,
                        _CrossSectionTextureScale,
                        _CrossSectionUVScaledByDistance,
                        d.isFrontFace,
                        d.screenPos,
                        color);
        #endif
    }
            void ChainSurfaceFunction(inout Surface l, inout ShaderData d)
            {
                  Ext_SurfaceFunction1(l, d);
                  Ext_SurfaceFunction2(l, d);
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
                  Ext_FinalColorForward2(l, d, color);
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
             d.screenUV = (IN.ScreenPosition.xy / max(0.01, IN.ScreenPosition.w));
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
             d.isFrontFace = facing;
            #if _HDRP
            #else
            #endif
             d.screenPos = i.screenPos;
             d.screenUV = (i.screenPos.xy / i.screenPos.w);
             d.extraV2F0 = i.extraV2F0;
            return d;
         }

#endif
         #if defined(_PASSSHADOW)
            float3 _LightDirection;
            float3 _LightPosition;
         #endif
         VertexToPixel Vert (VertexData v)
         {
           VertexToPixel o = (VertexToPixel)0;

           UNITY_SETUP_INSTANCE_ID(v);
           UNITY_TRANSFER_INSTANCE_ID(v, o);
           UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
#if !_TESSELLATION_ON
           ChainModifyVertex(v, o, _Time);
#endif

            o.texcoord0 = v.texcoord0;
           VertexPositionInputs vertexInput = GetVertexPositionInputs(v.vertex.xyz);
           o.worldPos = TransformObjectToWorld(v.vertex.xyz);
           o.worldNormal = TransformObjectToWorldNormal(v.normal);
           o.worldTangent = float4(TransformObjectToWorldDir(v.tangent.xyz), v.tangent.w);
          #if _PASSSHADOW
              #if _CASTING_PUNCTUAL_LIGHT_SHADOW
                 float3 lightDirectionWS = normalize(_LightPosition - o.worldPos);
              #else
                 float3 lightDirectionWS = _LightDirection;
              #endif
              o.pos = TransformWorldToHClip(ApplyShadowBias(o.worldPos, o.worldNormal, lightDirectionWS));
              #if UNITY_REVERSED_Z
                  o.pos.z = min(o.pos.z, UNITY_NEAR_CLIP_VALUE);
              #else
                  o.pos.z = max(o.pos.z, UNITY_NEAR_CLIP_VALUE);
              #endif
          #elif _PASSMETA
              o.pos = MetaVertexPosition(float4(v.vertex.xyz, 0), v.texcoord1.xy, v.texcoord2.xy, unity_LightmapST, unity_DynamicLightmapST);
          #else
              o.pos = TransformWorldToHClip(o.worldPos);
          #endif
           o.screenPos = ComputeScreenPos(o.pos, _ProjectionParams.x);
          #if _PASSFORWARD || _PASSGBUFFER
              float2 uv1 = v.texcoord1.xy;
              OUTPUT_LIGHTMAP_UV(uv1, unity_LightmapST, o.lightmapUV);
              OUTPUT_SH(o.worldNormal, o.sh);
              #if defined(DYNAMICLIGHTMAP_ON)
                   o.dynamicLightmapUV.xy = v.texcoord2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
              #endif
          #endif

          #ifdef VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
              half fogFactor = 0;
              #if defined(_FOG_FRAGMENT)
                fogFactor = ComputeFogFactor(o.pos.z);
              #endif
              #if _BAKEDLIT
                 o.fogFactorAndVertexLight = half4(fogFactor, 0, 0, 0);
              #else
                 half3 vertexLight = VertexLighting(o.worldPos, o.worldNormal);
                 o.fogFactorAndVertexLight = half4(fogFactor, vertexLight);
              #endif
          #endif

          #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
             o.shadowCoord = GetShadowCoord(vertexInput);
          #endif

           return o;
         }
            half4 Frag (VertexToPixel IN
            #ifdef _DEPTHOFFSET_ON
              , out float outputDepth : SV_Depth
            #endif
            #if NEED_FACING
               , bool facing : SV_IsFrontFace
            #endif
            ) : SV_Target
            {
               UNITY_SETUP_INSTANCE_ID(IN);

               #if defined(LOD_FADE_CROSSFADE)
                  LODFadeCrossFade(IN.pos);
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

             return 0;

            }

         ENDHLSL

      }
        Pass
        {
            Name "DepthOnly"
            Tags 
            { 
                "LightMode" = "DepthOnly"
            }
            Blend One Zero, One Zero
            Cull Back
            ZTest LEqual
            ZWrite On
            ColorMask 0
                Cull [_Cull]
            HLSLPROGRAM

               #pragma vertex Vert
   #pragma fragment Frag
            #define _PASSDEPTH 1

            #pragma target 3.0
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma multi_compile_instancing
            #pragma multi_compile _ DOTS_INSTANCING_ON
     #pragma shader_feature_local_fragment  _SHADING_COLOR _SHADING_BLINNPHONG _SHADING_PBR    
    #pragma shader_feature_local _NORMALMAP
    #pragma shader_feature_local_fragment _EMISSION

    #pragma shader_feature_local_fragment _ALPHATEST_ON
    #pragma shader_feature_local _RECEIVE_SHADOWS_OFF
    #pragma shader_feature_local_fragment _ _SPECGLOSSMAP _SPECULAR_COLOR 
    #pragma shader_feature_local_fragment _GLOSSINESS_FROM_BASE_ALPHA

    #pragma shader_feature_local_fragment _OCCLUSIONMAP
    #pragma shader_feature_local _PARALLAXMAP
    #pragma shader_feature_local _ _DETAIL_MULX2 _DETAIL_SCALED
    #pragma shader_feature_local_fragment _SURFACE_TYPE_TRANSPARENT

    #pragma shader_feature_local_fragment _ _ALPHAPREMULTIPLY_ON _ALPHAMODULATE_ON

    #pragma shader_feature_local_fragment _METALLICSPECGLOSSMAP
    #pragma shader_feature_local_fragment _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
    #pragma shader_feature_local_fragment _SPECULARHIGHLIGHTS_OFF
    #pragma shader_feature_local_fragment _ENVIRONMENTREFLECTIONS_OFF
    #pragma shader_feature_local_fragment _SPECULAR_SETUP
        #pragma shader_feature_local_fragment _OBSTRUCTION_CURVE
        #pragma shader_feature_local_fragment _DISSOLVEMASK
        #pragma shader_feature_local_fragment _ZONING
        #pragma shader_feature_local_fragment _REPLACEMENT
        #pragma shader_feature_local_fragment _PLAYERINDEPENDENT
   #define _URP 1
#define NEED_FACING 1
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Version.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"
                  #undef WorldNormalVector
      #define WorldNormalVector(data, normal) mul(normal, data.TBNMatrix)
      #define UnityObjectToWorldNormal(normal) mul(GetObjectToWorldMatrix(), normal)

      #define _WorldSpaceLightPos0 _MainLightPosition
      #define UNITY_DECLARE_TEX2D(name) TEXTURE2D(name); SAMPLER(sampler##name);
      #define UNITY_DECLARE_TEX2D_NOSAMPLER(name) TEXTURE2D(name);
      #define UNITY_DECLARE_TEX2DARRAY(name) TEXTURE2D_ARRAY(name); SAMPLER(sampler##name);
      #define UNITY_DECLARE_TEX2DARRAY_NOSAMPLER(name) TEXTURE2D_ARRAY(name);

      #define UNITY_SAMPLE_TEX2DARRAY(tex,coord)            SAMPLE_TEXTURE2D_ARRAY(tex, sampler##tex, coord.xy, coord.z)
      #define UNITY_SAMPLE_TEX2DARRAY_LOD(tex,coord,lod)    SAMPLE_TEXTURE2D_ARRAY_LOD(tex, sampler##tex, coord.xy, coord.z, lod)
      #define UNITY_SAMPLE_TEX2D(tex, coord)                SAMPLE_TEXTURE2D(tex, sampler##tex, coord)
      #define UNITY_SAMPLE_TEX2D_SAMPLER(tex, samp, coord)  SAMPLE_TEXTURE2D(tex, sampler##samp, coord)

      #define UNITY_SAMPLE_TEX2D_LOD(tex,coord, lod)   SAMPLE_TEXTURE2D_LOD(tex, sampler_##tex, coord, lod)
      #define UNITY_SAMPLE_TEX2D_SAMPLER_LOD(tex,samplertex,coord, lod) SAMPLE_TEXTURE2D_LOD (tex, sampler##samplertex,coord, lod)
      #if defined(UNITY_COMPILER_HLSL)
         #define UNITY_INITIALIZE_OUTPUT(type,name) name = (type)0;
      #else
         #define UNITY_INITIALIZE_OUTPUT(type,name)
      #endif

      #define sampler2D_float sampler2D
      #define sampler2D_half sampler2D
      struct VertexToPixel
      {
         float4 pos : SV_POSITION;
         float3 worldPos : TEXCOORD0;
         float3 worldNormal : TEXCOORD1;
         float4 worldTangent : TEXCOORD2;
          float4 texcoord0 : TEXCOORD3;
          float4 screenPos : TEXCOORD7;
         #if defined(LIGHTMAP_ON)
            float2 lightmapUV : TEXCOORD8;
         #endif
         #if defined(DYNAMICLIGHTMAP_ON)
            float2 dynamicLightmapUV : TEXCOORD9;
         #endif
         #if !defined(LIGHTMAP_ON)
            float3 sh : TEXCOORD10;
         #endif

         #if defined(VARYINGS_NEED_FOG_AND_VERTEX_LIGHT)
            float4 fogFactorAndVertexLight : TEXCOORD11;
         #endif

         #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
           float4 shadowCoord : TEXCOORD12;
         #endif
          float4 extraV2F0 : TEXCOORD13;
         #if UNITY_ANY_INSTANCING_ENABLED
         uint instanceID : CUSTOM_INSTANCE_ID;
         #endif
         #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
         uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
         #endif
         #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
         uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
         #endif
         #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
         FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
         #endif
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

               #undef GetWorldToObjectMatrix()

               #define GetWorldToObjectMatrix()   unity_WorldToObject
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
               float3 camView = mul((float3x3)GetObjectToWorldMatrix(), transpose(mul(GetWorldToObjectMatrix(), UNITY_MATRIX_I_V)) [2].xyz);

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
                  norms = mul((float3x3)GetWorldToViewMatrix(), norms) * 0.5 + 0.5;
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
            CBUFFER_START(UnityPerMaterial)
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
	half4 _BaseColor;
    float _BumpScale;
    float4 _BaseMap_ST;
    half _Cutoff;
    half4 _EmissionColor;
     half4 _SpecColor;
     float _Smoothness;
	float4 _DetailAlbedoMap_ST;
	half _Metallic;
	half _Parallax;
	half _OcclusionStrength;
	half _DetailAlbedoMapScale;
	half _DetailNormalMapScale;
#if UNITY_VERSION < 202200
	float _AlphaToMaskAvailable;
#endif
    float _SyncCullMode;

    float _IsReplacementShader;
    float _TriggerMode;
    float _RaycastMode;
    float _IsExempt;
    float _isReferenceMaterial;
    float _InteractionMode;

    float _tDirection = 0;
    float _numOfPlayersInside = 0;
    float _tValue = 0;
    float _id = 0;
    half _TextureVisibility;
    half _AngleStrength;
    float _Obstruction;
    half _ObstructionPlayerOffset;

    float _UVs;
    float4 _ObstructionCurve_TexelSize;      
    float _DissolveMaskEnabled;
    float4 _DissolveMask_TexelSize;
    float4 _DissolveTex_TexelSize;
    half4 _DissolveColor;
    float _DissolveColorSaturation;
    float _DissolveEmission;
    float _DissolveEmissionBooster;
    float _hasClippedShadows;
    float _ConeStrength;
    float _ConeObstructionDestroyRadius;
    float _CylinderStrength;
    float _CylinderObstructionDestroyRadius;
    float _CircleStrength;
    float _CircleObstructionDestroyRadius;
    float _CurveStrength;
    float _CurveObstructionDestroyRadius;
    float _IntrinsicDissolveStrength;
    float _DissolveFallOff;
    float _AffectedAreaPlayerBasedObstruction;
    float _PreviewMode;
    float _PreviewIndicatorLineThickness;
    float _AnimationEnabled;
    float _AnimationSpeed;
    float _DefaultEffectRadius;
    float _EnableDefaultEffectRadius;
    float _TransitionDuration;
    float _TexturedEmissionEdge;
    float _TexturedEmissionEdgeStrength;
    float _IsometricExclusion;
    float _IsometricExclusionDistance;
    float _IsometricExclusionGradientLength;
    float _Floor;
    float _FloorMode;
    float _FloorY;
    float _FloorYTextureGradientLength;
    float _PlayerPosYOffset;
    float _AffectedAreaFloor;
    float _Ceiling;
    float _CeilingMode;
    float _CeilingBlendMode;
    float _CeilingY;
    float _CeilingPlayerYOffset;
    float _CeilingYGradientLength;
    float _Zoning;
    float _ZoningMode;
    float _ZoningEdgeGradientLength;
    float _IsZoningRevealable;
    float _SyncZonesWithFloorY;
    float _SyncZonesFloorYOffset;

    half _UseCustomTime;

    half _CrossSectionEnabled;
    half4 _CrossSectionColor;
    half _CrossSectionTextureEnabled;
    float _CrossSectionTextureScale;
    half _CrossSectionUVScaledByDistance;
    half _DissolveMethod;
    half _DissolveTexSpace;
            CBUFFER_END
    sampler2D _NoiseMap1;
    sampler2D _NoiseMap2;

    sampler2D _HatchingMap;
    sampler2D _GradientTex;
    void Ext_ModifyVertex0 (inout VertexData v, inout ExtraV2F d)
    {
        float4x4 UnityObjectToClipPosMatrix = mul(GetWorldToHClipMatrix(), GetObjectToWorldMatrix());
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
    TEXTURE2D(_BaseMap);        SAMPLER(sampler_BaseMap);
    TEXTURE2D(_BumpMap);        SAMPLER(sampler_BumpMap);
    TEXTURE2D(_EmissionMap);    SAMPLER(sampler_EmissionMap);
    TEXTURE2D(_SpecGlossMap);       SAMPLER(sampler_SpecGlossMap);  
	TEXTURE2D(_ParallaxMap);        SAMPLER(sampler_ParallaxMap);
	TEXTURE2D(_OcclusionMap);       SAMPLER(sampler_OcclusionMap);
	TEXTURE2D(_DetailMask);         SAMPLER(sampler_DetailMask);
	TEXTURE2D(_DetailAlbedoMap);    SAMPLER(sampler_DetailAlbedoMap);
	TEXTURE2D(_DetailNormalMap);    SAMPLER(sampler_DetailNormalMap);
	TEXTURE2D(_MetallicGlossMap);   SAMPLER(sampler_MetallicGlossMap);
	TEXTURE2D(_ClearCoatMap);       SAMPLER(sampler_ClearCoatMap);
    #if defined(_DETAIL_MULX2) || defined(_DETAIL_SCALED)
		#define _DETAIL
	#endif

	#if _SPECULAR_SETUP
		#define _USESPECULAR 1
	#else
		#undef _USESPECULAR
	#endif
	float SharpenAlphaMy(float alpha, float alphaClipTreshold)
	{
	    return saturate((alpha - alphaClipTreshold) / max(fwidth(alpha), 0.0001) + 0.5);
	}
#if UNITY_VERSION < 202200
	half3 AlphaModulate(half3 albedo, half alpha)
	{
		#if defined(_ALPHAMODULATE_ON)
		    return lerp(half3(1.0, 1.0, 1.0), albedo, alpha);
		#else
		    return albedo;
		#endif
	}

	#if defined(_ALPHATEST_ON)
		bool IsAlphaToMaskAvailable()
		{
		    return (_AlphaToMaskAvailable != 0.0);
		}
		half AlphaClip(half alpha, half cutoff)
		{
		    half clippedAlpha = (alpha >= cutoff) ? float(alpha) : 0.0;

		    half alphaToCoverageAlpha = SharpenAlphaMy(alpha, cutoff);

		    alpha = IsAlphaToMaskAvailable() ? alphaToCoverageAlpha : clippedAlpha;
		    clip(alpha - 0.0001);

		    return alpha;
		}
	#endif
#endif
    int _DebugSceneOverrideMode;
    bool IsAlphaDiscardEnabledMy()
    {
        #if defined(DEBUG_DISPLAY)
        return (_DebugSceneOverrideMode == DEBUGSCENEOVERRIDEMODE_NONE);
        #else
        return true;
        #endif
    }
	half Alpha(half albedoAlpha, half4 color, half cutoff)
	{
		#if !defined(_SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A) && !defined(_GLOSSINESS_FROM_BASE_ALPHA)
		    half alpha = albedoAlpha * color.a;
		#else
		    half alpha = color.a;
		#endif
			#ifdef _ALPHATEST_ON
			    if (IsAlphaDiscardEnabledMy())
			        alpha = AlphaClip(alpha, cutoff);
			#endif

		    return alpha;
	}
 	half4 SampleAlbedoAlpha(float2 uv, TEXTURE2D_PARAM(albedoAlphaMap, sampler_albedoAlphaMap))
	{
	    return half4(SAMPLE_TEXTURE2D(albedoAlphaMap, sampler_albedoAlphaMap, uv));
	}
 	half3 SampleNormal(float2 uv, TEXTURE2D_PARAM(bumpMap, sampler_bumpMap), half scale = half(1.0))
	{
	#ifdef _NORMALMAP
	    half4 n = SAMPLE_TEXTURE2D(bumpMap, sampler_bumpMap, uv);
	    #if BUMP_SCALE_NOT_SUPPORTED
	        return UnpackNormal(n);
	    #else
	        return UnpackNormalScale(n, scale);
	    #endif
	#else
	    return half3(0.0h, 0.0h, 1.0h);
	#endif
	}
#if UNITY_VERSION < 202100
	half4 SampleSpecularSmoothnessMy(half2 uv, half alpha, half4 specColor, TEXTURE2D_PARAM( specMap, sampler_specMap))
	{
		half4 specularSmoothness = half4(0.0h, 0.0h, 0.0h, 1.0h);
		#ifdef _SPECGLOSSMAP
			specularSmoothness = SAMPLE_TEXTURE2D(specMap, sampler_specMap, uv) * specColor;
		#elif defined(_SPECULAR_COLOR)
			specularSmoothness = specColor;
		#endif

		#ifdef _GLOSSINESS_FROM_BASE_ALPHA
			specularSmoothness.a = exp2(10 * alpha + 1);
		#else
			specularSmoothness.a = exp2(10 * specularSmoothness.a + 1);
		#endif
		return specularSmoothness;
	}
#else

	half4 SampleSpecularSmoothnessMy(float2 uv, half alpha, half4 specColor, TEXTURE2D_PARAM(specMap, sampler_specMap))
	{
		half4 specularSmoothness = half4(0, 0, 0, 1);
	#ifdef _SPECGLOSSMAP
		specularSmoothness = SAMPLE_TEXTURE2D(specMap, sampler_specMap, uv) * specColor;
	#elif defined(_SPECULAR_COLOR)
		specularSmoothness = specColor;
	#endif

	#ifdef _GLOSSINESS_FROM_BASE_ALPHA
		specularSmoothness.a = alpha;
	#endif

		return specularSmoothness;
	}
#endif
    half3 SampleEmissionMy(float2 uv, half3 emissionColor, TEXTURE2D_PARAM(emissionMap, sampler_emissionMap))
    {
    #ifndef _EMISSION
        return 0;
    #else
        return SAMPLE_TEXTURE2D(emissionMap, sampler_emissionMap, uv).rgb * emissionColor;
    #endif
    }
	#ifndef BUILTIN_TARGET_API
		half2 ParallaxOffset1Step(half height, half amplitude, half3 viewDirTS)
		{
		    height = height * amplitude - amplitude / 2.0;
		    half3 v = normalize(viewDirTS);
		    v.z += 0.42;
		    return height * (v.xy / v.z);
		}
	#endif

	float2 ParallaxMapping(TEXTURE2D_PARAM(heightMap, sampler_heightMap), half3 viewDirTS, half scale, float2 uv)
	{
	    half h = SAMPLE_TEXTURE2D(heightMap, sampler_heightMap, uv).g;
	    float2 offset = ParallaxOffset1Step(h, scale, viewDirTS);
	    return offset;
	}
	#ifdef _SPECULAR_SETUP
	    #define SAMPLE_METALLICSPECULAR(uv) SAMPLE_TEXTURE2D(_SpecGlossMap, sampler_SpecGlossMap, uv)
	#else
	    #define SAMPLE_METALLICSPECULAR(uv) SAMPLE_TEXTURE2D(_MetallicGlossMap, sampler_MetallicGlossMap, uv)
	#endif

	half4 SampleMetallicSpecGloss(float2 uv, half albedoAlpha)
	{
	    half4 specGloss;

		#ifdef _METALLICSPECGLOSSMAP
		    specGloss = half4(SAMPLE_METALLICSPECULAR(uv));
		    #ifdef _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
		        specGloss.a = albedoAlpha * _Smoothness;
		    #else
		        specGloss.a *= _Smoothness;
		    #endif
		#else 
		    #if _SPECULAR_SETUP
		        specGloss.rgb = _SpecColor.rgb;
		    #else
		        specGloss.rgb = _Metallic.rrr;
		    #endif

		    #ifdef _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
		        specGloss.a = albedoAlpha * _Smoothness;
		    #else
		        specGloss.a = _Smoothness;
		    #endif
		#endif

	    return specGloss;
	}

	half SampleOcclusion(float2 uv)
	{
	    #ifdef _OCCLUSIONMAP
	        half occ = SAMPLE_TEXTURE2D(_OcclusionMap, sampler_OcclusionMap, uv).g;
	        return LerpWhiteTo(occ, _OcclusionStrength);
	    #else
	        return half(1.0);
	    #endif
	}
    half SampleOcclusionMy(float2 uv)
    {
    #ifdef _OCCLUSIONMAP
    #if defined(SHADER_API_GLES)
        return SAMPLE_TEXTURE2D(_OcclusionMap, sampler_OcclusionMap, uv).g;
    #else
        half occ = SAMPLE_TEXTURE2D(_OcclusionMap, sampler_OcclusionMap, uv).g;
        return LerpWhiteTo(occ, _OcclusionStrength);
    #endif
    #else
        return 1.0;
    #endif
    }
	half2 SampleClearCoat(float2 uv)
	{
		#if defined(_CLEARCOAT) || defined(_CLEARCOATMAP)
		    half2 clearCoatMaskSmoothness = half2(_ClearCoatMask, _ClearCoatSmoothness);

		#if defined(_CLEARCOATMAP)
		    clearCoatMaskSmoothness *= SAMPLE_TEXTURE2D(_ClearCoatMap, sampler_ClearCoatMap, uv).rg;
		#endif

		    return clearCoatMaskSmoothness;
		#else
		    return half2(0.0, 1.0);
		#endif  
	}

	void ApplyPerPixelDisplacement(half3 viewDirTS, inout float2 uv)
	{
		#if defined(_PARALLAXMAP)
		    uv += ParallaxMapping(TEXTURE2D_ARGS(_ParallaxMap, sampler_ParallaxMap), viewDirTS, _Parallax, uv);
		#endif
	}

	half3 ScaleDetailAlbedo(half3 detailAlbedo, half scale)
	{
	    return half(2.0) * detailAlbedo * scale - scale + half(1.0);
	}

	half3 ApplyDetailAlbedo(float2 detailUv, half3 albedo, half detailMask)
	{
		#if defined(_DETAIL)
		    half3 detailAlbedo = SAMPLE_TEXTURE2D(_DetailAlbedoMap, sampler_DetailAlbedoMap, detailUv).rgb;
		#if defined(_DETAIL_SCALED)
		    detailAlbedo = ScaleDetailAlbedo(detailAlbedo, _DetailAlbedoMapScale);
		#else
		    detailAlbedo = half(2.0) * detailAlbedo;
		#endif

		    return albedo * LerpWhiteTo(detailAlbedo, detailMask);
		#else
		    return albedo;
		#endif
	}

	half3 ApplyDetailNormal(float2 detailUv, half3 normalTS, half detailMask)
	{
		#if defined(_DETAIL)
		#if BUMP_SCALE_NOT_SUPPORTED
		    half3 detailNormalTS = UnpackNormal(SAMPLE_TEXTURE2D(_DetailNormalMap, sampler_DetailNormalMap, detailUv));
		#else
		    half3 detailNormalTS = UnpackNormalScale(SAMPLE_TEXTURE2D(_DetailNormalMap, sampler_DetailNormalMap, detailUv), _DetailNormalMapScale);
		#endif
		    detailNormalTS = normalize(detailNormalTS);

		    return lerp(normalTS, BlendNormalRNM(normalTS, detailNormalTS), detailMask); 
		#else
		    return normalTS;
		#endif
	}
	void Ext_SurfaceFunction1 (inout Surface o, ShaderData d)
	{

        float4 texcoords;
        texcoords.xy = d.texcoord0.xy * _BaseMap_ST.xy + _BaseMap_ST.zw; 
        float2 uv = texcoords.xy;

#if _SHADING_BLINNPHONG || _SHADING_COLOR
        half4 diffuseAlpha = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap,  texcoords.xy);
        o.Albedo = diffuseAlpha.rgb * _BaseColor.rgb;
	#if !_PASSFORWARD
		if(_EnableToonShading == 0 || _ShadingAffectedByNormalMap == 1) 
		{
			o.Normal = SampleNormal(texcoords.xy, TEXTURE2D_ARGS(_BumpMap, sampler_BumpMap), _BumpScale);
		}
	#else
        o.Normal = SampleNormal(texcoords.xy, TEXTURE2D_ARGS(_BumpMap, sampler_BumpMap), _BumpScale);
	#endif
		o.Alpha = diffuseAlpha.a;
        o.Alpha = o.Alpha * _BaseColor.a;
        AlphaDiscard(o.Alpha, _Cutoff);
        half3 emission = SampleEmissionMy(uv, _EmissionColor.rgb, TEXTURE2D_ARGS(_EmissionMap, sampler_EmissionMap));
        o.Emission = emission;
#endif

      if(_ShadingMode == 1 && _LightFunction == 0 ) 
      {
#if _SHADING_BLINNPHONG
        half4 specular = SampleSpecularSmoothnessMy(uv, o.Alpha, _SpecColor, TEXTURE2D_ARGS(_SpecGlossMap, sampler_SpecGlossMap));
        half smoothness =  specular.a;

        o.Specular = specular;
        o.Smoothness = smoothness;
#endif

      } 
      else if (_ShadingMode == 1 && _LightFunction == 1) 
      {
#if _SHADING_PBR
		#if defined(_PARALLAXMAP)
		    ApplyPerPixelDisplacement(d.tangentSpaceViewDir, uv);
		#endif
    	half4 albedoAlpha =  SampleAlbedoAlpha(uv, TEXTURE2D_ARGS(_BaseMap, sampler_BaseMap));
	    o.Alpha = Alpha(albedoAlpha.a, _BaseColor, _Cutoff);

	    half4 specGloss = SampleMetallicSpecGloss(uv, albedoAlpha.a);
	    o.Albedo = albedoAlpha.rgb * _BaseColor.rgb;
	    o.Albedo = AlphaModulate(o.Albedo, o.Alpha);
		#if _SPECULAR_SETUP
		    o.Metallic = half(1.0);
		    o.Specular = specGloss.rgb;
		#else
		    o.Metallic = specGloss.r;
		    o.Specular = half3(0.0, 0.0, 0.0);
		#endif
        o.Smoothness = specGloss.a;
	#if !_PASSFORWARD
		if(_EnableToonShading == 0 || _ShadingAffectedByNormalMap == 1) 
		{
			o.Normal = SampleNormal(uv, TEXTURE2D_ARGS(_BumpMap, sampler_BumpMap), _BumpScale);
		}
	#else
		o.Normal = SampleNormal(uv, TEXTURE2D_ARGS(_BumpMap, sampler_BumpMap), _BumpScale);
	#endif
	    o.Occlusion = SampleOcclusion(uv);
	    o.Emission = SampleEmissionMy(uv, _EmissionColor.rgb, TEXTURE2D_ARGS(_EmissionMap, sampler_EmissionMap));

		#if defined(_DETAIL)
		    half detailMask = SAMPLE_TEXTURE2D(_DetailMask, sampler_DetailMask, uv).a;
		    float2 detailUv = uv * _DetailAlbedoMap_ST.xy + _DetailAlbedoMap_ST.zw;
		    o.Albedo = ApplyDetailAlbedo(detailUv, o.Albedo, detailMask);
		    o.Normal = ApplyDetailNormal(detailUv, o.Normal, detailMask);
		#endif

#endif
      }

	}
    float _ArrayLength = 0;
    #if (defined(SHADER_API_GLES) || defined(SHADER_API_GLES3)) 
        float4 _PlayersPosVectorArray[20];
        float _PlayersDataFloatArray[150];     
    #else
        float4 _PlayersPosVectorArray[100];
        float _PlayersDataFloatArray[500];  
    #endif
    #if _ZONING
        #if (defined(SHADER_API_GLES) || defined(SHADER_API_GLES3)) 
            float _ZDFA[500];
        #else
            float _ZDFA[1000];
        #endif
        float _ZonesDataCount;
    #endif

    float _STSCustomTime = 0;
    #if _REPLACEMENT        
        half4 _DissolveColorGlobal;
        float _DissolveColorSaturationGlobal;
        float _DissolveEmissionGlobal;
        float _DissolveEmissionBoosterGlobal;
        float _TextureVisibilityGlobal;
        float _ObstructionGlobal;
        float _AngleStrengthGlobal;
        float _ConeStrengthGlobal;
        float _ConeObstructionDestroyRadiusGlobal;
        float _CylinderStrengthGlobal;
        float _CylinderObstructionDestroyRadiusGlobal;
        float _CircleStrengthGlobal;
        float _CircleObstructionDestroyRadiusGlobal;
        float _CurveStrengthGlobal;
        float _CurveObstructionDestroyRadiusGlobal;
        float _DissolveFallOffGlobal;
        float _AffectedAreaPlayerBasedObstructionGlobal;
        float _IntrinsicDissolveStrengthGlobal;
        float _PreviewModeGlobal;
        float _UVsGlobal;
        float _hasClippedShadowsGlobal;
        float _FloorGlobal;
        float _FloorModeGlobal;
        float _FloorYGlobal;
        float _PlayerPosYOffsetGlobal;
        float _FloorYTextureGradientLengthGlobal;
        float _AffectedAreaFloorGlobal;
        float _AnimationEnabledGlobal;
        float _AnimationSpeedGlobal;
        float _DefaultEffectRadiusGlobal;
        float _EnableDefaultEffectRadiusGlobal;
        float _TransitionDurationGlobal;        
        float _TexturedEmissionEdgeGlobal;
        float _TexturedEmissionEdgeStrengthGlobal;
        float _IsometricExclusionGlobal;
        float _IsometricExclusionDistanceGlobal;
        float _IsometricExclusionGradientLengthGlobal;
        float _CeilingGlobal;
        float _CeilingModeGlobal;
        float _CeilingBlendModeGlobal;
        float _CeilingYGlobal;
        float _CeilingPlayerYOffsetGlobal;
        float _CeilingYGradientLengthGlobal;
        float _ZoningGlobal;
        float _ZoningModeGlobal;
        float _ZoningEdgeGradientLengthGlobal;
        float _IsZoningRevealableGlobal;
        float _SyncZonesWithFloorYGlobal;
        float _SyncZonesFloorYOffsetGlobal;
        float4 _ObstructionCurveGlobal_TexelSize;
        float4 _DissolveMaskGlobal_TexelSize;
        float _DissolveMaskEnabledGlobal;
        float _PreviewIndicatorLineThicknessGlobal;
        half _UseCustomTimeGlobal;

        half _CrossSectionEnabledGlobal;
        half4 _CrossSectionColorGlobal;
        half _CrossSectionTextureEnabledGlobal;
        float _CrossSectionTextureScaleGlobal;
        half _CrossSectionUVScaledByDistanceGlobal;

        half _DissolveMethodGlobal;
        half _DissolveTexSpaceGlobal;
        half _ObstructionPlayerOffsetGlobal;

        float4 _DissolveTexGlobal_TexelSize;
    #endif
    #if _REPLACEMENT
        sampler2D _DissolveTexGlobal;
    #else
        sampler2D _DissolveTex;
    #endif
    #if _REPLACEMENT
        sampler2D _DissolveMaskGlobal;
    #else
        sampler2D _DissolveMask;
    #endif
    #if _REPLACEMENT
        sampler2D _ObstructionCurveGlobal;
    #else
        sampler2D _ObstructionCurve;
    #endif

    #if _REPLACEMENT
        sampler2D _CrossSectionTextureGlobal;
    #else
        sampler2D _CrossSectionTexture;
    #endif
#ifdef USE_UNITY_TEXTURE_2D_TYPE
#undef USE_UNITY_TEXTURE_2D_TYPE
#endif

#include "Packages/com.shadercrew.seethroughshader.core/Scripts/Shaders/xxSharedSTSDependencies/SeeThroughShaderFunction.hlsl"
	void Ext_SurfaceFunction2 (inout Surface o, ShaderData d)
	{
        half3 albedo;
        half3 emission;
        float alphaForClipping;
#ifdef USE_UNITY_TEXTURE_2D_TYPE
#undef USE_UNITY_TEXTURE_2D_TYPE
#endif
#if _REPLACEMENT
    DoSeeThroughShading( o.Albedo, o.Normal, d.worldSpacePosition, d.worldSpaceNormal, d.screenPos,
                        _numOfPlayersInside, _tDirection, _tValue, _id,
                        _TriggerMode, _RaycastMode,
                        _IsExempt,

                        _DissolveMethodGlobal, _DissolveTexSpaceGlobal,

                        _DissolveColorGlobal, _DissolveColorSaturationGlobal, _UVsGlobal,
                        _DissolveEmissionGlobal, _DissolveEmissionBoosterGlobal, _TexturedEmissionEdgeGlobal, _TexturedEmissionEdgeStrengthGlobal,
                        _hasClippedShadowsGlobal,

                        _ObstructionGlobal,
                        _ObstructionPlayerOffsetGlobal,
                        _AngleStrengthGlobal,
                        _ConeStrengthGlobal, _ConeObstructionDestroyRadiusGlobal,
                        _CylinderStrengthGlobal, _CylinderObstructionDestroyRadiusGlobal,
                        _CircleStrengthGlobal, _CircleObstructionDestroyRadiusGlobal,
                        _CurveStrengthGlobal, _CurveObstructionDestroyRadiusGlobal,
                        _DissolveFallOffGlobal,
                        _DissolveMaskEnabledGlobal,
                        _AffectedAreaPlayerBasedObstructionGlobal,
                        _IntrinsicDissolveStrengthGlobal,
                        _DefaultEffectRadiusGlobal, _EnableDefaultEffectRadiusGlobal,

                        _IsometricExclusionGlobal, _IsometricExclusionDistanceGlobal, _IsometricExclusionGradientLengthGlobal,
                        _CeilingGlobal, _CeilingModeGlobal, _CeilingBlendModeGlobal, _CeilingYGlobal, _CeilingPlayerYOffsetGlobal, _CeilingYGradientLengthGlobal,
                        _FloorGlobal, _FloorModeGlobal, _FloorYGlobal, _PlayerPosYOffsetGlobal, _FloorYTextureGradientLengthGlobal, _AffectedAreaFloorGlobal,

                        _AnimationEnabledGlobal, _AnimationSpeedGlobal,
                        _TransitionDurationGlobal,

                        _UseCustomTimeGlobal,

                        _ZoningGlobal, _ZoningModeGlobal, _IsZoningRevealableGlobal, _ZoningEdgeGradientLengthGlobal,
                        _SyncZonesWithFloorYGlobal, _SyncZonesFloorYOffsetGlobal,

                        _PreviewModeGlobal,
                        _PreviewIndicatorLineThicknessGlobal,

                        _DissolveTexGlobal,
                        _DissolveMaskGlobal,
                        _ObstructionCurveGlobal,

                        _DissolveTexGlobal_TexelSize,
                        _DissolveMaskGlobal_TexelSize,
                        _ObstructionCurveGlobal_TexelSize,

                        albedo, emission, alphaForClipping);
#else 
    DoSeeThroughShading( o.Albedo, o.Normal, d.worldSpacePosition, d.worldSpaceNormal, d.screenPos,

                        _numOfPlayersInside, _tDirection, _tValue, _id,
                        _TriggerMode, _RaycastMode,
                        _IsExempt,

                        _DissolveMethod, _DissolveTexSpace,

                        _DissolveColor, _DissolveColorSaturation, _UVs,
                        _DissolveEmission, _DissolveEmissionBooster, _TexturedEmissionEdge, _TexturedEmissionEdgeStrength,
                        _hasClippedShadows,

                        _Obstruction,
                        _ObstructionPlayerOffset,
                        _AngleStrength,
                        _ConeStrength, _ConeObstructionDestroyRadius,
                        _CylinderStrength, _CylinderObstructionDestroyRadius,
                        _CircleStrength, _CircleObstructionDestroyRadius,
                        _CurveStrength, _CurveObstructionDestroyRadius,
                        _DissolveFallOff,
                        _DissolveMaskEnabled,
                        _AffectedAreaPlayerBasedObstruction,
                        _IntrinsicDissolveStrength,
                        _DefaultEffectRadius, _EnableDefaultEffectRadius,

                        _IsometricExclusion, _IsometricExclusionDistance, _IsometricExclusionGradientLength,
                        _Ceiling, _CeilingMode, _CeilingBlendMode, _CeilingY, _CeilingPlayerYOffset, _CeilingYGradientLength,
                        _Floor, _FloorMode, _FloorY, _PlayerPosYOffset, _FloorYTextureGradientLength, _AffectedAreaFloor,

                        _AnimationEnabled, _AnimationSpeed,
                        _TransitionDuration,

                        _UseCustomTime,

                        _Zoning, _ZoningMode , _IsZoningRevealable, _ZoningEdgeGradientLength,
                        _SyncZonesWithFloorY, _SyncZonesFloorYOffset,

                        _PreviewMode,
                        _PreviewIndicatorLineThickness,                            

                        _DissolveTex,
                        _DissolveMask,
                        _ObstructionCurve,

                        _DissolveTex_TexelSize,
                        _DissolveMask_TexelSize,
                        _ObstructionCurve_TexelSize,

                        albedo, emission, alphaForClipping);
#endif 
        o.Albedo = albedo;
        o.Emission += emission;   

	}
    void Ext_FinalColorForward2 (Surface o, ShaderData d, inout half4 color)
    {
        #if _REPLACEMENT   
            DoCrossSection(_CrossSectionEnabledGlobal,
                        _CrossSectionColorGlobal,
                        _CrossSectionTextureEnabledGlobal,
                        _CrossSectionTextureGlobal,
                        _CrossSectionTextureScaleGlobal,
                        _CrossSectionUVScaledByDistanceGlobal,
                        d.isFrontFace,
                        d.screenPos,
                        color);
        #else 
            DoCrossSection(_CrossSectionEnabled,
                        _CrossSectionColor,
                        _CrossSectionTextureEnabled,
                        _CrossSectionTexture,
                        _CrossSectionTextureScale,
                        _CrossSectionUVScaledByDistance,
                        d.isFrontFace,
                        d.screenPos,
                        color);
        #endif
    }
            void ChainSurfaceFunction(inout Surface l, inout ShaderData d)
            {
                  Ext_SurfaceFunction1(l, d);
                  Ext_SurfaceFunction2(l, d);
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
                  Ext_FinalColorForward2(l, d, color);
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
             d.screenUV = (IN.ScreenPosition.xy / max(0.01, IN.ScreenPosition.w));
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
             d.isFrontFace = facing;
            #if _HDRP
            #else
            #endif
             d.screenPos = i.screenPos;
             d.screenUV = (i.screenPos.xy / i.screenPos.w);
             d.extraV2F0 = i.extraV2F0;
            return d;
         }

#endif
         #if defined(_PASSSHADOW)
            float3 _LightDirection;
            float3 _LightPosition;
         #endif
         VertexToPixel Vert (VertexData v)
         {
           VertexToPixel o = (VertexToPixel)0;

           UNITY_SETUP_INSTANCE_ID(v);
           UNITY_TRANSFER_INSTANCE_ID(v, o);
           UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
#if !_TESSELLATION_ON
           ChainModifyVertex(v, o, _Time);
#endif

            o.texcoord0 = v.texcoord0;
           VertexPositionInputs vertexInput = GetVertexPositionInputs(v.vertex.xyz);
           o.worldPos = TransformObjectToWorld(v.vertex.xyz);
           o.worldNormal = TransformObjectToWorldNormal(v.normal);
           o.worldTangent = float4(TransformObjectToWorldDir(v.tangent.xyz), v.tangent.w);
          #if _PASSSHADOW
              #if _CASTING_PUNCTUAL_LIGHT_SHADOW
                 float3 lightDirectionWS = normalize(_LightPosition - o.worldPos);
              #else
                 float3 lightDirectionWS = _LightDirection;
              #endif
              o.pos = TransformWorldToHClip(ApplyShadowBias(o.worldPos, o.worldNormal, lightDirectionWS));
              #if UNITY_REVERSED_Z
                  o.pos.z = min(o.pos.z, UNITY_NEAR_CLIP_VALUE);
              #else
                  o.pos.z = max(o.pos.z, UNITY_NEAR_CLIP_VALUE);
              #endif
          #elif _PASSMETA
              o.pos = MetaVertexPosition(float4(v.vertex.xyz, 0), v.texcoord1.xy, v.texcoord2.xy, unity_LightmapST, unity_DynamicLightmapST);
          #else
              o.pos = TransformWorldToHClip(o.worldPos);
          #endif
           o.screenPos = ComputeScreenPos(o.pos, _ProjectionParams.x);
          #if _PASSFORWARD || _PASSGBUFFER
              float2 uv1 = v.texcoord1.xy;
              OUTPUT_LIGHTMAP_UV(uv1, unity_LightmapST, o.lightmapUV);
              OUTPUT_SH(o.worldNormal, o.sh);
              #if defined(DYNAMICLIGHTMAP_ON)
                   o.dynamicLightmapUV.xy = v.texcoord2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
              #endif
          #endif

          #ifdef VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
              half fogFactor = 0;
              #if defined(_FOG_FRAGMENT)
                fogFactor = ComputeFogFactor(o.pos.z);
              #endif
              #if _BAKEDLIT
                 o.fogFactorAndVertexLight = half4(fogFactor, 0, 0, 0);
              #else
                 half3 vertexLight = VertexLighting(o.worldPos, o.worldNormal);
                 o.fogFactorAndVertexLight = half4(fogFactor, vertexLight);
              #endif
          #endif

          #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
             o.shadowCoord = GetShadowCoord(vertexInput);
          #endif

           return o;
         }
            half4 Frag (VertexToPixel IN
            #ifdef _DEPTHOFFSET_ON
              , out float outputDepth : SV_Depth
            #endif
            #if NEED_FACING
               , bool facing : SV_IsFrontFace
            #endif
            ) : SV_Target
            {
               UNITY_SETUP_INSTANCE_ID(IN);
               UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);

                #if defined(LOD_FADE_CROSSFADE) && USE_UNITY_CROSSFADE
                    LODFadeCrossFade(IN.pos);
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

               return 0;

            }

         ENDHLSL

      }
        Pass
        {
            Name "Meta"
            Tags 
            { 
                "LightMode" = "Meta"
            }

            Cull Off
                Cull [_Cull]
            HLSLPROGRAM

               #pragma vertex Vert
   #pragma fragment Frag

            #pragma target 3.0

            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #define SHADERPASS SHADERPASS_META
            #define _PASSMETA 1
     #pragma shader_feature_local_fragment  _SHADING_COLOR _SHADING_BLINNPHONG _SHADING_PBR    
    #pragma shader_feature_local _NORMALMAP
    #pragma shader_feature_local_fragment _EMISSION

    #pragma shader_feature_local_fragment _ALPHATEST_ON
    #pragma shader_feature_local _RECEIVE_SHADOWS_OFF
    #pragma shader_feature_local_fragment _ _SPECGLOSSMAP _SPECULAR_COLOR 
    #pragma shader_feature_local_fragment _GLOSSINESS_FROM_BASE_ALPHA

    #pragma shader_feature_local_fragment _OCCLUSIONMAP
    #pragma shader_feature_local _PARALLAXMAP
    #pragma shader_feature_local _ _DETAIL_MULX2 _DETAIL_SCALED
    #pragma shader_feature_local_fragment _SURFACE_TYPE_TRANSPARENT

    #pragma shader_feature_local_fragment _ _ALPHAPREMULTIPLY_ON _ALPHAMODULATE_ON

    #pragma shader_feature_local_fragment _METALLICSPECGLOSSMAP
    #pragma shader_feature_local_fragment _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
    #pragma shader_feature_local_fragment _SPECULARHIGHLIGHTS_OFF
    #pragma shader_feature_local_fragment _ENVIRONMENTREFLECTIONS_OFF
    #pragma shader_feature_local_fragment _SPECULAR_SETUP
        #pragma shader_feature_local_fragment _OBSTRUCTION_CURVE
        #pragma shader_feature_local_fragment _DISSOLVEMASK
        #pragma shader_feature_local_fragment _ZONING
        #pragma shader_feature_local_fragment _REPLACEMENT
        #pragma shader_feature_local_fragment _PLAYERINDEPENDENT
   #define _URP 1
#define NEED_FACING 1
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Version.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/MetaInput.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

                  #undef WorldNormalVector
      #define WorldNormalVector(data, normal) mul(normal, data.TBNMatrix)
      #define UnityObjectToWorldNormal(normal) mul(GetObjectToWorldMatrix(), normal)

      #define _WorldSpaceLightPos0 _MainLightPosition
      #define UNITY_DECLARE_TEX2D(name) TEXTURE2D(name); SAMPLER(sampler##name);
      #define UNITY_DECLARE_TEX2D_NOSAMPLER(name) TEXTURE2D(name);
      #define UNITY_DECLARE_TEX2DARRAY(name) TEXTURE2D_ARRAY(name); SAMPLER(sampler##name);
      #define UNITY_DECLARE_TEX2DARRAY_NOSAMPLER(name) TEXTURE2D_ARRAY(name);

      #define UNITY_SAMPLE_TEX2DARRAY(tex,coord)            SAMPLE_TEXTURE2D_ARRAY(tex, sampler##tex, coord.xy, coord.z)
      #define UNITY_SAMPLE_TEX2DARRAY_LOD(tex,coord,lod)    SAMPLE_TEXTURE2D_ARRAY_LOD(tex, sampler##tex, coord.xy, coord.z, lod)
      #define UNITY_SAMPLE_TEX2D(tex, coord)                SAMPLE_TEXTURE2D(tex, sampler##tex, coord)
      #define UNITY_SAMPLE_TEX2D_SAMPLER(tex, samp, coord)  SAMPLE_TEXTURE2D(tex, sampler##samp, coord)

      #define UNITY_SAMPLE_TEX2D_LOD(tex,coord, lod)   SAMPLE_TEXTURE2D_LOD(tex, sampler_##tex, coord, lod)
      #define UNITY_SAMPLE_TEX2D_SAMPLER_LOD(tex,samplertex,coord, lod) SAMPLE_TEXTURE2D_LOD (tex, sampler##samplertex,coord, lod)
      #if defined(UNITY_COMPILER_HLSL)
         #define UNITY_INITIALIZE_OUTPUT(type,name) name = (type)0;
      #else
         #define UNITY_INITIALIZE_OUTPUT(type,name)
      #endif

      #define sampler2D_float sampler2D
      #define sampler2D_half sampler2D
      struct VertexToPixel
      {
         float4 pos : SV_POSITION;
         float3 worldPos : TEXCOORD0;
         float3 worldNormal : TEXCOORD1;
         float4 worldTangent : TEXCOORD2;
          float4 texcoord0 : TEXCOORD3;
          float4 screenPos : TEXCOORD7;
         #if defined(LIGHTMAP_ON)
            float2 lightmapUV : TEXCOORD8;
         #endif
         #if defined(DYNAMICLIGHTMAP_ON)
            float2 dynamicLightmapUV : TEXCOORD9;
         #endif
         #if !defined(LIGHTMAP_ON)
            float3 sh : TEXCOORD10;
         #endif

         #if defined(VARYINGS_NEED_FOG_AND_VERTEX_LIGHT)
            float4 fogFactorAndVertexLight : TEXCOORD11;
         #endif

         #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
           float4 shadowCoord : TEXCOORD12;
         #endif
          float4 extraV2F0 : TEXCOORD13;
         #if UNITY_ANY_INSTANCING_ENABLED
         uint instanceID : CUSTOM_INSTANCE_ID;
         #endif
         #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
         uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
         #endif
         #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
         uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
         #endif
         #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
         FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
         #endif
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

               #undef GetWorldToObjectMatrix()

               #define GetWorldToObjectMatrix()   unity_WorldToObject
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
               float3 camView = mul((float3x3)GetObjectToWorldMatrix(), transpose(mul(GetWorldToObjectMatrix(), UNITY_MATRIX_I_V)) [2].xyz);

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
                  norms = mul((float3x3)GetWorldToViewMatrix(), norms) * 0.5 + 0.5;
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
            CBUFFER_START(UnityPerMaterial)
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
	half4 _BaseColor;
    float _BumpScale;
    float4 _BaseMap_ST;
    half _Cutoff;
    half4 _EmissionColor;
     half4 _SpecColor;
     float _Smoothness;
	float4 _DetailAlbedoMap_ST;
	half _Metallic;
	half _Parallax;
	half _OcclusionStrength;
	half _DetailAlbedoMapScale;
	half _DetailNormalMapScale;
#if UNITY_VERSION < 202200
	float _AlphaToMaskAvailable;
#endif
    float _SyncCullMode;

    float _IsReplacementShader;
    float _TriggerMode;
    float _RaycastMode;
    float _IsExempt;
    float _isReferenceMaterial;
    float _InteractionMode;

    float _tDirection = 0;
    float _numOfPlayersInside = 0;
    float _tValue = 0;
    float _id = 0;
    half _TextureVisibility;
    half _AngleStrength;
    float _Obstruction;
    half _ObstructionPlayerOffset;

    float _UVs;
    float4 _ObstructionCurve_TexelSize;      
    float _DissolveMaskEnabled;
    float4 _DissolveMask_TexelSize;
    float4 _DissolveTex_TexelSize;
    half4 _DissolveColor;
    float _DissolveColorSaturation;
    float _DissolveEmission;
    float _DissolveEmissionBooster;
    float _hasClippedShadows;
    float _ConeStrength;
    float _ConeObstructionDestroyRadius;
    float _CylinderStrength;
    float _CylinderObstructionDestroyRadius;
    float _CircleStrength;
    float _CircleObstructionDestroyRadius;
    float _CurveStrength;
    float _CurveObstructionDestroyRadius;
    float _IntrinsicDissolveStrength;
    float _DissolveFallOff;
    float _AffectedAreaPlayerBasedObstruction;
    float _PreviewMode;
    float _PreviewIndicatorLineThickness;
    float _AnimationEnabled;
    float _AnimationSpeed;
    float _DefaultEffectRadius;
    float _EnableDefaultEffectRadius;
    float _TransitionDuration;
    float _TexturedEmissionEdge;
    float _TexturedEmissionEdgeStrength;
    float _IsometricExclusion;
    float _IsometricExclusionDistance;
    float _IsometricExclusionGradientLength;
    float _Floor;
    float _FloorMode;
    float _FloorY;
    float _FloorYTextureGradientLength;
    float _PlayerPosYOffset;
    float _AffectedAreaFloor;
    float _Ceiling;
    float _CeilingMode;
    float _CeilingBlendMode;
    float _CeilingY;
    float _CeilingPlayerYOffset;
    float _CeilingYGradientLength;
    float _Zoning;
    float _ZoningMode;
    float _ZoningEdgeGradientLength;
    float _IsZoningRevealable;
    float _SyncZonesWithFloorY;
    float _SyncZonesFloorYOffset;

    half _UseCustomTime;

    half _CrossSectionEnabled;
    half4 _CrossSectionColor;
    half _CrossSectionTextureEnabled;
    float _CrossSectionTextureScale;
    half _CrossSectionUVScaledByDistance;
    half _DissolveMethod;
    half _DissolveTexSpace;
            CBUFFER_END
    sampler2D _NoiseMap1;
    sampler2D _NoiseMap2;

    sampler2D _HatchingMap;
    sampler2D _GradientTex;
    void Ext_ModifyVertex0 (inout VertexData v, inout ExtraV2F d)
    {
        float4x4 UnityObjectToClipPosMatrix = mul(GetWorldToHClipMatrix(), GetObjectToWorldMatrix());
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
    TEXTURE2D(_BaseMap);        SAMPLER(sampler_BaseMap);
    TEXTURE2D(_BumpMap);        SAMPLER(sampler_BumpMap);
    TEXTURE2D(_EmissionMap);    SAMPLER(sampler_EmissionMap);
    TEXTURE2D(_SpecGlossMap);       SAMPLER(sampler_SpecGlossMap);  
	TEXTURE2D(_ParallaxMap);        SAMPLER(sampler_ParallaxMap);
	TEXTURE2D(_OcclusionMap);       SAMPLER(sampler_OcclusionMap);
	TEXTURE2D(_DetailMask);         SAMPLER(sampler_DetailMask);
	TEXTURE2D(_DetailAlbedoMap);    SAMPLER(sampler_DetailAlbedoMap);
	TEXTURE2D(_DetailNormalMap);    SAMPLER(sampler_DetailNormalMap);
	TEXTURE2D(_MetallicGlossMap);   SAMPLER(sampler_MetallicGlossMap);
	TEXTURE2D(_ClearCoatMap);       SAMPLER(sampler_ClearCoatMap);
    #if defined(_DETAIL_MULX2) || defined(_DETAIL_SCALED)
		#define _DETAIL
	#endif

	#if _SPECULAR_SETUP
		#define _USESPECULAR 1
	#else
		#undef _USESPECULAR
	#endif
	float SharpenAlphaMy(float alpha, float alphaClipTreshold)
	{
	    return saturate((alpha - alphaClipTreshold) / max(fwidth(alpha), 0.0001) + 0.5);
	}
#if UNITY_VERSION < 202200
	half3 AlphaModulate(half3 albedo, half alpha)
	{
		#if defined(_ALPHAMODULATE_ON)
		    return lerp(half3(1.0, 1.0, 1.0), albedo, alpha);
		#else
		    return albedo;
		#endif
	}

	#if defined(_ALPHATEST_ON)
		bool IsAlphaToMaskAvailable()
		{
		    return (_AlphaToMaskAvailable != 0.0);
		}
		half AlphaClip(half alpha, half cutoff)
		{
		    half clippedAlpha = (alpha >= cutoff) ? float(alpha) : 0.0;

		    half alphaToCoverageAlpha = SharpenAlphaMy(alpha, cutoff);

		    alpha = IsAlphaToMaskAvailable() ? alphaToCoverageAlpha : clippedAlpha;
		    clip(alpha - 0.0001);

		    return alpha;
		}
	#endif
#endif
    int _DebugSceneOverrideMode;
    bool IsAlphaDiscardEnabledMy()
    {
        #if defined(DEBUG_DISPLAY)
        return (_DebugSceneOverrideMode == DEBUGSCENEOVERRIDEMODE_NONE);
        #else
        return true;
        #endif
    }
	half Alpha(half albedoAlpha, half4 color, half cutoff)
	{
		#if !defined(_SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A) && !defined(_GLOSSINESS_FROM_BASE_ALPHA)
		    half alpha = albedoAlpha * color.a;
		#else
		    half alpha = color.a;
		#endif
			#ifdef _ALPHATEST_ON
			    if (IsAlphaDiscardEnabledMy())
			        alpha = AlphaClip(alpha, cutoff);
			#endif

		    return alpha;
	}
 	half4 SampleAlbedoAlpha(float2 uv, TEXTURE2D_PARAM(albedoAlphaMap, sampler_albedoAlphaMap))
	{
	    return half4(SAMPLE_TEXTURE2D(albedoAlphaMap, sampler_albedoAlphaMap, uv));
	}
 	half3 SampleNormal(float2 uv, TEXTURE2D_PARAM(bumpMap, sampler_bumpMap), half scale = half(1.0))
	{
	#ifdef _NORMALMAP
	    half4 n = SAMPLE_TEXTURE2D(bumpMap, sampler_bumpMap, uv);
	    #if BUMP_SCALE_NOT_SUPPORTED
	        return UnpackNormal(n);
	    #else
	        return UnpackNormalScale(n, scale);
	    #endif
	#else
	    return half3(0.0h, 0.0h, 1.0h);
	#endif
	}
#if UNITY_VERSION < 202100
	half4 SampleSpecularSmoothnessMy(half2 uv, half alpha, half4 specColor, TEXTURE2D_PARAM( specMap, sampler_specMap))
	{
		half4 specularSmoothness = half4(0.0h, 0.0h, 0.0h, 1.0h);
		#ifdef _SPECGLOSSMAP
			specularSmoothness = SAMPLE_TEXTURE2D(specMap, sampler_specMap, uv) * specColor;
		#elif defined(_SPECULAR_COLOR)
			specularSmoothness = specColor;
		#endif

		#ifdef _GLOSSINESS_FROM_BASE_ALPHA
			specularSmoothness.a = exp2(10 * alpha + 1);
		#else
			specularSmoothness.a = exp2(10 * specularSmoothness.a + 1);
		#endif
		return specularSmoothness;
	}
#else

	half4 SampleSpecularSmoothnessMy(float2 uv, half alpha, half4 specColor, TEXTURE2D_PARAM(specMap, sampler_specMap))
	{
		half4 specularSmoothness = half4(0, 0, 0, 1);
	#ifdef _SPECGLOSSMAP
		specularSmoothness = SAMPLE_TEXTURE2D(specMap, sampler_specMap, uv) * specColor;
	#elif defined(_SPECULAR_COLOR)
		specularSmoothness = specColor;
	#endif

	#ifdef _GLOSSINESS_FROM_BASE_ALPHA
		specularSmoothness.a = alpha;
	#endif

		return specularSmoothness;
	}
#endif
    half3 SampleEmissionMy(float2 uv, half3 emissionColor, TEXTURE2D_PARAM(emissionMap, sampler_emissionMap))
    {
    #ifndef _EMISSION
        return 0;
    #else
        return SAMPLE_TEXTURE2D(emissionMap, sampler_emissionMap, uv).rgb * emissionColor;
    #endif
    }
	#ifndef BUILTIN_TARGET_API
		half2 ParallaxOffset1Step(half height, half amplitude, half3 viewDirTS)
		{
		    height = height * amplitude - amplitude / 2.0;
		    half3 v = normalize(viewDirTS);
		    v.z += 0.42;
		    return height * (v.xy / v.z);
		}
	#endif

	float2 ParallaxMapping(TEXTURE2D_PARAM(heightMap, sampler_heightMap), half3 viewDirTS, half scale, float2 uv)
	{
	    half h = SAMPLE_TEXTURE2D(heightMap, sampler_heightMap, uv).g;
	    float2 offset = ParallaxOffset1Step(h, scale, viewDirTS);
	    return offset;
	}
	#ifdef _SPECULAR_SETUP
	    #define SAMPLE_METALLICSPECULAR(uv) SAMPLE_TEXTURE2D(_SpecGlossMap, sampler_SpecGlossMap, uv)
	#else
	    #define SAMPLE_METALLICSPECULAR(uv) SAMPLE_TEXTURE2D(_MetallicGlossMap, sampler_MetallicGlossMap, uv)
	#endif

	half4 SampleMetallicSpecGloss(float2 uv, half albedoAlpha)
	{
	    half4 specGloss;

		#ifdef _METALLICSPECGLOSSMAP
		    specGloss = half4(SAMPLE_METALLICSPECULAR(uv));
		    #ifdef _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
		        specGloss.a = albedoAlpha * _Smoothness;
		    #else
		        specGloss.a *= _Smoothness;
		    #endif
		#else 
		    #if _SPECULAR_SETUP
		        specGloss.rgb = _SpecColor.rgb;
		    #else
		        specGloss.rgb = _Metallic.rrr;
		    #endif

		    #ifdef _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
		        specGloss.a = albedoAlpha * _Smoothness;
		    #else
		        specGloss.a = _Smoothness;
		    #endif
		#endif

	    return specGloss;
	}

	half SampleOcclusion(float2 uv)
	{
	    #ifdef _OCCLUSIONMAP
	        half occ = SAMPLE_TEXTURE2D(_OcclusionMap, sampler_OcclusionMap, uv).g;
	        return LerpWhiteTo(occ, _OcclusionStrength);
	    #else
	        return half(1.0);
	    #endif
	}
    half SampleOcclusionMy(float2 uv)
    {
    #ifdef _OCCLUSIONMAP
    #if defined(SHADER_API_GLES)
        return SAMPLE_TEXTURE2D(_OcclusionMap, sampler_OcclusionMap, uv).g;
    #else
        half occ = SAMPLE_TEXTURE2D(_OcclusionMap, sampler_OcclusionMap, uv).g;
        return LerpWhiteTo(occ, _OcclusionStrength);
    #endif
    #else
        return 1.0;
    #endif
    }
	half2 SampleClearCoat(float2 uv)
	{
		#if defined(_CLEARCOAT) || defined(_CLEARCOATMAP)
		    half2 clearCoatMaskSmoothness = half2(_ClearCoatMask, _ClearCoatSmoothness);

		#if defined(_CLEARCOATMAP)
		    clearCoatMaskSmoothness *= SAMPLE_TEXTURE2D(_ClearCoatMap, sampler_ClearCoatMap, uv).rg;
		#endif

		    return clearCoatMaskSmoothness;
		#else
		    return half2(0.0, 1.0);
		#endif  
	}

	void ApplyPerPixelDisplacement(half3 viewDirTS, inout float2 uv)
	{
		#if defined(_PARALLAXMAP)
		    uv += ParallaxMapping(TEXTURE2D_ARGS(_ParallaxMap, sampler_ParallaxMap), viewDirTS, _Parallax, uv);
		#endif
	}

	half3 ScaleDetailAlbedo(half3 detailAlbedo, half scale)
	{
	    return half(2.0) * detailAlbedo * scale - scale + half(1.0);
	}

	half3 ApplyDetailAlbedo(float2 detailUv, half3 albedo, half detailMask)
	{
		#if defined(_DETAIL)
		    half3 detailAlbedo = SAMPLE_TEXTURE2D(_DetailAlbedoMap, sampler_DetailAlbedoMap, detailUv).rgb;
		#if defined(_DETAIL_SCALED)
		    detailAlbedo = ScaleDetailAlbedo(detailAlbedo, _DetailAlbedoMapScale);
		#else
		    detailAlbedo = half(2.0) * detailAlbedo;
		#endif

		    return albedo * LerpWhiteTo(detailAlbedo, detailMask);
		#else
		    return albedo;
		#endif
	}

	half3 ApplyDetailNormal(float2 detailUv, half3 normalTS, half detailMask)
	{
		#if defined(_DETAIL)
		#if BUMP_SCALE_NOT_SUPPORTED
		    half3 detailNormalTS = UnpackNormal(SAMPLE_TEXTURE2D(_DetailNormalMap, sampler_DetailNormalMap, detailUv));
		#else
		    half3 detailNormalTS = UnpackNormalScale(SAMPLE_TEXTURE2D(_DetailNormalMap, sampler_DetailNormalMap, detailUv), _DetailNormalMapScale);
		#endif
		    detailNormalTS = normalize(detailNormalTS);

		    return lerp(normalTS, BlendNormalRNM(normalTS, detailNormalTS), detailMask); 
		#else
		    return normalTS;
		#endif
	}
	void Ext_SurfaceFunction1 (inout Surface o, ShaderData d)
	{

        float4 texcoords;
        texcoords.xy = d.texcoord0.xy * _BaseMap_ST.xy + _BaseMap_ST.zw; 
        float2 uv = texcoords.xy;

#if _SHADING_BLINNPHONG || _SHADING_COLOR
        half4 diffuseAlpha = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap,  texcoords.xy);
        o.Albedo = diffuseAlpha.rgb * _BaseColor.rgb;
	#if !_PASSFORWARD
		if(_EnableToonShading == 0 || _ShadingAffectedByNormalMap == 1) 
		{
			o.Normal = SampleNormal(texcoords.xy, TEXTURE2D_ARGS(_BumpMap, sampler_BumpMap), _BumpScale);
		}
	#else
        o.Normal = SampleNormal(texcoords.xy, TEXTURE2D_ARGS(_BumpMap, sampler_BumpMap), _BumpScale);
	#endif
		o.Alpha = diffuseAlpha.a;
        o.Alpha = o.Alpha * _BaseColor.a;
        AlphaDiscard(o.Alpha, _Cutoff);
        half3 emission = SampleEmissionMy(uv, _EmissionColor.rgb, TEXTURE2D_ARGS(_EmissionMap, sampler_EmissionMap));
        o.Emission = emission;
#endif

      if(_ShadingMode == 1 && _LightFunction == 0 ) 
      {
#if _SHADING_BLINNPHONG
        half4 specular = SampleSpecularSmoothnessMy(uv, o.Alpha, _SpecColor, TEXTURE2D_ARGS(_SpecGlossMap, sampler_SpecGlossMap));
        half smoothness =  specular.a;

        o.Specular = specular;
        o.Smoothness = smoothness;
#endif

      } 
      else if (_ShadingMode == 1 && _LightFunction == 1) 
      {
#if _SHADING_PBR
		#if defined(_PARALLAXMAP)
		    ApplyPerPixelDisplacement(d.tangentSpaceViewDir, uv);
		#endif
    	half4 albedoAlpha =  SampleAlbedoAlpha(uv, TEXTURE2D_ARGS(_BaseMap, sampler_BaseMap));
	    o.Alpha = Alpha(albedoAlpha.a, _BaseColor, _Cutoff);

	    half4 specGloss = SampleMetallicSpecGloss(uv, albedoAlpha.a);
	    o.Albedo = albedoAlpha.rgb * _BaseColor.rgb;
	    o.Albedo = AlphaModulate(o.Albedo, o.Alpha);
		#if _SPECULAR_SETUP
		    o.Metallic = half(1.0);
		    o.Specular = specGloss.rgb;
		#else
		    o.Metallic = specGloss.r;
		    o.Specular = half3(0.0, 0.0, 0.0);
		#endif
        o.Smoothness = specGloss.a;
	#if !_PASSFORWARD
		if(_EnableToonShading == 0 || _ShadingAffectedByNormalMap == 1) 
		{
			o.Normal = SampleNormal(uv, TEXTURE2D_ARGS(_BumpMap, sampler_BumpMap), _BumpScale);
		}
	#else
		o.Normal = SampleNormal(uv, TEXTURE2D_ARGS(_BumpMap, sampler_BumpMap), _BumpScale);
	#endif
	    o.Occlusion = SampleOcclusion(uv);
	    o.Emission = SampleEmissionMy(uv, _EmissionColor.rgb, TEXTURE2D_ARGS(_EmissionMap, sampler_EmissionMap));

		#if defined(_DETAIL)
		    half detailMask = SAMPLE_TEXTURE2D(_DetailMask, sampler_DetailMask, uv).a;
		    float2 detailUv = uv * _DetailAlbedoMap_ST.xy + _DetailAlbedoMap_ST.zw;
		    o.Albedo = ApplyDetailAlbedo(detailUv, o.Albedo, detailMask);
		    o.Normal = ApplyDetailNormal(detailUv, o.Normal, detailMask);
		#endif

#endif
      }

	}
    float _ArrayLength = 0;
    #if (defined(SHADER_API_GLES) || defined(SHADER_API_GLES3)) 
        float4 _PlayersPosVectorArray[20];
        float _PlayersDataFloatArray[150];     
    #else
        float4 _PlayersPosVectorArray[100];
        float _PlayersDataFloatArray[500];  
    #endif
    #if _ZONING
        #if (defined(SHADER_API_GLES) || defined(SHADER_API_GLES3)) 
            float _ZDFA[500];
        #else
            float _ZDFA[1000];
        #endif
        float _ZonesDataCount;
    #endif

    float _STSCustomTime = 0;
    #if _REPLACEMENT        
        half4 _DissolveColorGlobal;
        float _DissolveColorSaturationGlobal;
        float _DissolveEmissionGlobal;
        float _DissolveEmissionBoosterGlobal;
        float _TextureVisibilityGlobal;
        float _ObstructionGlobal;
        float _AngleStrengthGlobal;
        float _ConeStrengthGlobal;
        float _ConeObstructionDestroyRadiusGlobal;
        float _CylinderStrengthGlobal;
        float _CylinderObstructionDestroyRadiusGlobal;
        float _CircleStrengthGlobal;
        float _CircleObstructionDestroyRadiusGlobal;
        float _CurveStrengthGlobal;
        float _CurveObstructionDestroyRadiusGlobal;
        float _DissolveFallOffGlobal;
        float _AffectedAreaPlayerBasedObstructionGlobal;
        float _IntrinsicDissolveStrengthGlobal;
        float _PreviewModeGlobal;
        float _UVsGlobal;
        float _hasClippedShadowsGlobal;
        float _FloorGlobal;
        float _FloorModeGlobal;
        float _FloorYGlobal;
        float _PlayerPosYOffsetGlobal;
        float _FloorYTextureGradientLengthGlobal;
        float _AffectedAreaFloorGlobal;
        float _AnimationEnabledGlobal;
        float _AnimationSpeedGlobal;
        float _DefaultEffectRadiusGlobal;
        float _EnableDefaultEffectRadiusGlobal;
        float _TransitionDurationGlobal;        
        float _TexturedEmissionEdgeGlobal;
        float _TexturedEmissionEdgeStrengthGlobal;
        float _IsometricExclusionGlobal;
        float _IsometricExclusionDistanceGlobal;
        float _IsometricExclusionGradientLengthGlobal;
        float _CeilingGlobal;
        float _CeilingModeGlobal;
        float _CeilingBlendModeGlobal;
        float _CeilingYGlobal;
        float _CeilingPlayerYOffsetGlobal;
        float _CeilingYGradientLengthGlobal;
        float _ZoningGlobal;
        float _ZoningModeGlobal;
        float _ZoningEdgeGradientLengthGlobal;
        float _IsZoningRevealableGlobal;
        float _SyncZonesWithFloorYGlobal;
        float _SyncZonesFloorYOffsetGlobal;
        float4 _ObstructionCurveGlobal_TexelSize;
        float4 _DissolveMaskGlobal_TexelSize;
        float _DissolveMaskEnabledGlobal;
        float _PreviewIndicatorLineThicknessGlobal;
        half _UseCustomTimeGlobal;

        half _CrossSectionEnabledGlobal;
        half4 _CrossSectionColorGlobal;
        half _CrossSectionTextureEnabledGlobal;
        float _CrossSectionTextureScaleGlobal;
        half _CrossSectionUVScaledByDistanceGlobal;

        half _DissolveMethodGlobal;
        half _DissolveTexSpaceGlobal;
        half _ObstructionPlayerOffsetGlobal;

        float4 _DissolveTexGlobal_TexelSize;
    #endif
    #if _REPLACEMENT
        sampler2D _DissolveTexGlobal;
    #else
        sampler2D _DissolveTex;
    #endif
    #if _REPLACEMENT
        sampler2D _DissolveMaskGlobal;
    #else
        sampler2D _DissolveMask;
    #endif
    #if _REPLACEMENT
        sampler2D _ObstructionCurveGlobal;
    #else
        sampler2D _ObstructionCurve;
    #endif

    #if _REPLACEMENT
        sampler2D _CrossSectionTextureGlobal;
    #else
        sampler2D _CrossSectionTexture;
    #endif
#ifdef USE_UNITY_TEXTURE_2D_TYPE
#undef USE_UNITY_TEXTURE_2D_TYPE
#endif

#include "Packages/com.shadercrew.seethroughshader.core/Scripts/Shaders/xxSharedSTSDependencies/SeeThroughShaderFunction.hlsl"
	void Ext_SurfaceFunction2 (inout Surface o, ShaderData d)
	{
        half3 albedo;
        half3 emission;
        float alphaForClipping;
#ifdef USE_UNITY_TEXTURE_2D_TYPE
#undef USE_UNITY_TEXTURE_2D_TYPE
#endif
#if _REPLACEMENT
    DoSeeThroughShading( o.Albedo, o.Normal, d.worldSpacePosition, d.worldSpaceNormal, d.screenPos,
                        _numOfPlayersInside, _tDirection, _tValue, _id,
                        _TriggerMode, _RaycastMode,
                        _IsExempt,

                        _DissolveMethodGlobal, _DissolveTexSpaceGlobal,

                        _DissolveColorGlobal, _DissolveColorSaturationGlobal, _UVsGlobal,
                        _DissolveEmissionGlobal, _DissolveEmissionBoosterGlobal, _TexturedEmissionEdgeGlobal, _TexturedEmissionEdgeStrengthGlobal,
                        _hasClippedShadowsGlobal,

                        _ObstructionGlobal,
                        _ObstructionPlayerOffsetGlobal,
                        _AngleStrengthGlobal,
                        _ConeStrengthGlobal, _ConeObstructionDestroyRadiusGlobal,
                        _CylinderStrengthGlobal, _CylinderObstructionDestroyRadiusGlobal,
                        _CircleStrengthGlobal, _CircleObstructionDestroyRadiusGlobal,
                        _CurveStrengthGlobal, _CurveObstructionDestroyRadiusGlobal,
                        _DissolveFallOffGlobal,
                        _DissolveMaskEnabledGlobal,
                        _AffectedAreaPlayerBasedObstructionGlobal,
                        _IntrinsicDissolveStrengthGlobal,
                        _DefaultEffectRadiusGlobal, _EnableDefaultEffectRadiusGlobal,

                        _IsometricExclusionGlobal, _IsometricExclusionDistanceGlobal, _IsometricExclusionGradientLengthGlobal,
                        _CeilingGlobal, _CeilingModeGlobal, _CeilingBlendModeGlobal, _CeilingYGlobal, _CeilingPlayerYOffsetGlobal, _CeilingYGradientLengthGlobal,
                        _FloorGlobal, _FloorModeGlobal, _FloorYGlobal, _PlayerPosYOffsetGlobal, _FloorYTextureGradientLengthGlobal, _AffectedAreaFloorGlobal,

                        _AnimationEnabledGlobal, _AnimationSpeedGlobal,
                        _TransitionDurationGlobal,

                        _UseCustomTimeGlobal,

                        _ZoningGlobal, _ZoningModeGlobal, _IsZoningRevealableGlobal, _ZoningEdgeGradientLengthGlobal,
                        _SyncZonesWithFloorYGlobal, _SyncZonesFloorYOffsetGlobal,

                        _PreviewModeGlobal,
                        _PreviewIndicatorLineThicknessGlobal,

                        _DissolveTexGlobal,
                        _DissolveMaskGlobal,
                        _ObstructionCurveGlobal,

                        _DissolveTexGlobal_TexelSize,
                        _DissolveMaskGlobal_TexelSize,
                        _ObstructionCurveGlobal_TexelSize,

                        albedo, emission, alphaForClipping);
#else 
    DoSeeThroughShading( o.Albedo, o.Normal, d.worldSpacePosition, d.worldSpaceNormal, d.screenPos,

                        _numOfPlayersInside, _tDirection, _tValue, _id,
                        _TriggerMode, _RaycastMode,
                        _IsExempt,

                        _DissolveMethod, _DissolveTexSpace,

                        _DissolveColor, _DissolveColorSaturation, _UVs,
                        _DissolveEmission, _DissolveEmissionBooster, _TexturedEmissionEdge, _TexturedEmissionEdgeStrength,
                        _hasClippedShadows,

                        _Obstruction,
                        _ObstructionPlayerOffset,
                        _AngleStrength,
                        _ConeStrength, _ConeObstructionDestroyRadius,
                        _CylinderStrength, _CylinderObstructionDestroyRadius,
                        _CircleStrength, _CircleObstructionDestroyRadius,
                        _CurveStrength, _CurveObstructionDestroyRadius,
                        _DissolveFallOff,
                        _DissolveMaskEnabled,
                        _AffectedAreaPlayerBasedObstruction,
                        _IntrinsicDissolveStrength,
                        _DefaultEffectRadius, _EnableDefaultEffectRadius,

                        _IsometricExclusion, _IsometricExclusionDistance, _IsometricExclusionGradientLength,
                        _Ceiling, _CeilingMode, _CeilingBlendMode, _CeilingY, _CeilingPlayerYOffset, _CeilingYGradientLength,
                        _Floor, _FloorMode, _FloorY, _PlayerPosYOffset, _FloorYTextureGradientLength, _AffectedAreaFloor,

                        _AnimationEnabled, _AnimationSpeed,
                        _TransitionDuration,

                        _UseCustomTime,

                        _Zoning, _ZoningMode , _IsZoningRevealable, _ZoningEdgeGradientLength,
                        _SyncZonesWithFloorY, _SyncZonesFloorYOffset,

                        _PreviewMode,
                        _PreviewIndicatorLineThickness,                            

                        _DissolveTex,
                        _DissolveMask,
                        _ObstructionCurve,

                        _DissolveTex_TexelSize,
                        _DissolveMask_TexelSize,
                        _ObstructionCurve_TexelSize,

                        albedo, emission, alphaForClipping);
#endif 
        o.Albedo = albedo;
        o.Emission += emission;   

	}
    void Ext_FinalColorForward2 (Surface o, ShaderData d, inout half4 color)
    {
        #if _REPLACEMENT   
            DoCrossSection(_CrossSectionEnabledGlobal,
                        _CrossSectionColorGlobal,
                        _CrossSectionTextureEnabledGlobal,
                        _CrossSectionTextureGlobal,
                        _CrossSectionTextureScaleGlobal,
                        _CrossSectionUVScaledByDistanceGlobal,
                        d.isFrontFace,
                        d.screenPos,
                        color);
        #else 
            DoCrossSection(_CrossSectionEnabled,
                        _CrossSectionColor,
                        _CrossSectionTextureEnabled,
                        _CrossSectionTexture,
                        _CrossSectionTextureScale,
                        _CrossSectionUVScaledByDistance,
                        d.isFrontFace,
                        d.screenPos,
                        color);
        #endif
    }
            void ChainSurfaceFunction(inout Surface l, inout ShaderData d)
            {
                  Ext_SurfaceFunction1(l, d);
                  Ext_SurfaceFunction2(l, d);
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
                  Ext_FinalColorForward2(l, d, color);
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
             d.screenUV = (IN.ScreenPosition.xy / max(0.01, IN.ScreenPosition.w));
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
             d.isFrontFace = facing;
            #if _HDRP
            #else
            #endif
             d.screenPos = i.screenPos;
             d.screenUV = (i.screenPos.xy / i.screenPos.w);
             d.extraV2F0 = i.extraV2F0;
            return d;
         }

#endif
         #if defined(_PASSSHADOW)
            float3 _LightDirection;
            float3 _LightPosition;
         #endif
         VertexToPixel Vert (VertexData v)
         {
           VertexToPixel o = (VertexToPixel)0;

           UNITY_SETUP_INSTANCE_ID(v);
           UNITY_TRANSFER_INSTANCE_ID(v, o);
           UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
#if !_TESSELLATION_ON
           ChainModifyVertex(v, o, _Time);
#endif

            o.texcoord0 = v.texcoord0;
           VertexPositionInputs vertexInput = GetVertexPositionInputs(v.vertex.xyz);
           o.worldPos = TransformObjectToWorld(v.vertex.xyz);
           o.worldNormal = TransformObjectToWorldNormal(v.normal);
           o.worldTangent = float4(TransformObjectToWorldDir(v.tangent.xyz), v.tangent.w);
          #if _PASSSHADOW
              #if _CASTING_PUNCTUAL_LIGHT_SHADOW
                 float3 lightDirectionWS = normalize(_LightPosition - o.worldPos);
              #else
                 float3 lightDirectionWS = _LightDirection;
              #endif
              o.pos = TransformWorldToHClip(ApplyShadowBias(o.worldPos, o.worldNormal, lightDirectionWS));
              #if UNITY_REVERSED_Z
                  o.pos.z = min(o.pos.z, UNITY_NEAR_CLIP_VALUE);
              #else
                  o.pos.z = max(o.pos.z, UNITY_NEAR_CLIP_VALUE);
              #endif
          #elif _PASSMETA
              o.pos = MetaVertexPosition(float4(v.vertex.xyz, 0), v.texcoord1.xy, v.texcoord2.xy, unity_LightmapST, unity_DynamicLightmapST);
          #else
              o.pos = TransformWorldToHClip(o.worldPos);
          #endif
           o.screenPos = ComputeScreenPos(o.pos, _ProjectionParams.x);
          #if _PASSFORWARD || _PASSGBUFFER
              float2 uv1 = v.texcoord1.xy;
              OUTPUT_LIGHTMAP_UV(uv1, unity_LightmapST, o.lightmapUV);
              OUTPUT_SH(o.worldNormal, o.sh);
              #if defined(DYNAMICLIGHTMAP_ON)
                   o.dynamicLightmapUV.xy = v.texcoord2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
              #endif
          #endif

          #ifdef VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
              half fogFactor = 0;
              #if defined(_FOG_FRAGMENT)
                fogFactor = ComputeFogFactor(o.pos.z);
              #endif
              #if _BAKEDLIT
                 o.fogFactorAndVertexLight = half4(fogFactor, 0, 0, 0);
              #else
                 half3 vertexLight = VertexLighting(o.worldPos, o.worldNormal);
                 o.fogFactorAndVertexLight = half4(fogFactor, vertexLight);
              #endif
          #endif

          #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
             o.shadowCoord = GetShadowCoord(vertexInput);
          #endif

           return o;
         }
            half4 Frag (VertexToPixel IN
               #if NEED_FACING
                  , bool facing : SV_IsFrontFace
               #endif
            ) : SV_Target
            {
               UNITY_SETUP_INSTANCE_ID(IN);

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

               MetaInput metaInput = (MetaInput)0;
               metaInput.Albedo = l.Albedo;
               metaInput.Emission = l.Emission;

               return MetaFragment(metaInput);

            }

         ENDHLSL

      }
        Pass
        {
            Name "DepthNormals"
            Tags
            {
               "LightMode" = "DepthNormals"
            }
             Cull Back
                ZTest LEqual
                ZWrite On

                Cull [_Cull]
            HLSLPROGRAM

               #pragma vertex Vert
   #pragma fragment Frag

            #pragma target 3.0

            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma multi_compile_fog
            #pragma multi_compile_instancing
            #pragma multi_compile _ DOTS_INSTANCING_ON
            #pragma multi_compile_fragment _ LOD_FADE_CROSSFADE
            #pragma multi_compile_fragment _ _WRITE_RENDERING_LAYERS
            #define SHADERPASS SHADERPASS_DEPTHNORMALSONLY
            #define _PASSDEPTH 1
            #define _PASSDEPTHNORMALS 1
     #pragma shader_feature_local_fragment  _SHADING_COLOR _SHADING_BLINNPHONG _SHADING_PBR    
    #pragma shader_feature_local _NORMALMAP
    #pragma shader_feature_local_fragment _EMISSION

    #pragma shader_feature_local_fragment _ALPHATEST_ON
    #pragma shader_feature_local _RECEIVE_SHADOWS_OFF
    #pragma shader_feature_local_fragment _ _SPECGLOSSMAP _SPECULAR_COLOR 
    #pragma shader_feature_local_fragment _GLOSSINESS_FROM_BASE_ALPHA

    #pragma shader_feature_local_fragment _OCCLUSIONMAP
    #pragma shader_feature_local _PARALLAXMAP
    #pragma shader_feature_local _ _DETAIL_MULX2 _DETAIL_SCALED
    #pragma shader_feature_local_fragment _SURFACE_TYPE_TRANSPARENT

    #pragma shader_feature_local_fragment _ _ALPHAPREMULTIPLY_ON _ALPHAMODULATE_ON

    #pragma shader_feature_local_fragment _METALLICSPECGLOSSMAP
    #pragma shader_feature_local_fragment _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
    #pragma shader_feature_local_fragment _SPECULARHIGHLIGHTS_OFF
    #pragma shader_feature_local_fragment _ENVIRONMENTREFLECTIONS_OFF
    #pragma shader_feature_local_fragment _SPECULAR_SETUP
        #pragma shader_feature_local_fragment _OBSTRUCTION_CURVE
        #pragma shader_feature_local_fragment _DISSOLVEMASK
        #pragma shader_feature_local_fragment _ZONING
        #pragma shader_feature_local_fragment _REPLACEMENT
        #pragma shader_feature_local_fragment _PLAYERINDEPENDENT
   #define _URP 1
#define NEED_FACING 1
            #if _SIMPLELIT
               #define _SPECULAR_COLOR
            #endif
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Version.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/LODCrossFade.hlsl"
               #undef WorldNormalVector
      #define WorldNormalVector(data, normal) mul(normal, data.TBNMatrix)
      #define UnityObjectToWorldNormal(normal) mul(GetObjectToWorldMatrix(), normal)

      #define _WorldSpaceLightPos0 _MainLightPosition
      #define UNITY_DECLARE_TEX2D(name) TEXTURE2D(name); SAMPLER(sampler##name);
      #define UNITY_DECLARE_TEX2D_NOSAMPLER(name) TEXTURE2D(name);
      #define UNITY_DECLARE_TEX2DARRAY(name) TEXTURE2D_ARRAY(name); SAMPLER(sampler##name);
      #define UNITY_DECLARE_TEX2DARRAY_NOSAMPLER(name) TEXTURE2D_ARRAY(name);

      #define UNITY_SAMPLE_TEX2DARRAY(tex,coord)            SAMPLE_TEXTURE2D_ARRAY(tex, sampler##tex, coord.xy, coord.z)
      #define UNITY_SAMPLE_TEX2DARRAY_LOD(tex,coord,lod)    SAMPLE_TEXTURE2D_ARRAY_LOD(tex, sampler##tex, coord.xy, coord.z, lod)
      #define UNITY_SAMPLE_TEX2D(tex, coord)                SAMPLE_TEXTURE2D(tex, sampler##tex, coord)
      #define UNITY_SAMPLE_TEX2D_SAMPLER(tex, samp, coord)  SAMPLE_TEXTURE2D(tex, sampler##samp, coord)

      #define UNITY_SAMPLE_TEX2D_LOD(tex,coord, lod)   SAMPLE_TEXTURE2D_LOD(tex, sampler_##tex, coord, lod)
      #define UNITY_SAMPLE_TEX2D_SAMPLER_LOD(tex,samplertex,coord, lod) SAMPLE_TEXTURE2D_LOD (tex, sampler##samplertex,coord, lod)
      #if defined(UNITY_COMPILER_HLSL)
         #define UNITY_INITIALIZE_OUTPUT(type,name) name = (type)0;
      #else
         #define UNITY_INITIALIZE_OUTPUT(type,name)
      #endif

      #define sampler2D_float sampler2D
      #define sampler2D_half sampler2D
      struct VertexToPixel
      {
         float4 pos : SV_POSITION;
         float3 worldPos : TEXCOORD0;
         float3 worldNormal : TEXCOORD1;
         float4 worldTangent : TEXCOORD2;
          float4 texcoord0 : TEXCOORD3;
          float4 screenPos : TEXCOORD7;
         #if defined(LIGHTMAP_ON)
            float2 lightmapUV : TEXCOORD8;
         #endif
         #if defined(DYNAMICLIGHTMAP_ON)
            float2 dynamicLightmapUV : TEXCOORD9;
         #endif
         #if !defined(LIGHTMAP_ON)
            float3 sh : TEXCOORD10;
         #endif

         #if defined(VARYINGS_NEED_FOG_AND_VERTEX_LIGHT)
            float4 fogFactorAndVertexLight : TEXCOORD11;
         #endif

         #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
           float4 shadowCoord : TEXCOORD12;
         #endif
          float4 extraV2F0 : TEXCOORD13;
         #if UNITY_ANY_INSTANCING_ENABLED
         uint instanceID : CUSTOM_INSTANCE_ID;
         #endif
         #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
         uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
         #endif
         #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
         uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
         #endif
         #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
         FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
         #endif
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

               #undef GetWorldToObjectMatrix()

               #define GetWorldToObjectMatrix()   unity_WorldToObject
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
               float3 camView = mul((float3x3)GetObjectToWorldMatrix(), transpose(mul(GetWorldToObjectMatrix(), UNITY_MATRIX_I_V)) [2].xyz);

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
                  norms = mul((float3x3)GetWorldToViewMatrix(), norms) * 0.5 + 0.5;
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
         CBUFFER_START(UnityPerMaterial)
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
	half4 _BaseColor;
    float _BumpScale;
    float4 _BaseMap_ST;
    half _Cutoff;
    half4 _EmissionColor;
     half4 _SpecColor;
     float _Smoothness;
	float4 _DetailAlbedoMap_ST;
	half _Metallic;
	half _Parallax;
	half _OcclusionStrength;
	half _DetailAlbedoMapScale;
	half _DetailNormalMapScale;
#if UNITY_VERSION < 202200
	float _AlphaToMaskAvailable;
#endif
    float _SyncCullMode;

    float _IsReplacementShader;
    float _TriggerMode;
    float _RaycastMode;
    float _IsExempt;
    float _isReferenceMaterial;
    float _InteractionMode;

    float _tDirection = 0;
    float _numOfPlayersInside = 0;
    float _tValue = 0;
    float _id = 0;
    half _TextureVisibility;
    half _AngleStrength;
    float _Obstruction;
    half _ObstructionPlayerOffset;

    float _UVs;
    float4 _ObstructionCurve_TexelSize;      
    float _DissolveMaskEnabled;
    float4 _DissolveMask_TexelSize;
    float4 _DissolveTex_TexelSize;
    half4 _DissolveColor;
    float _DissolveColorSaturation;
    float _DissolveEmission;
    float _DissolveEmissionBooster;
    float _hasClippedShadows;
    float _ConeStrength;
    float _ConeObstructionDestroyRadius;
    float _CylinderStrength;
    float _CylinderObstructionDestroyRadius;
    float _CircleStrength;
    float _CircleObstructionDestroyRadius;
    float _CurveStrength;
    float _CurveObstructionDestroyRadius;
    float _IntrinsicDissolveStrength;
    float _DissolveFallOff;
    float _AffectedAreaPlayerBasedObstruction;
    float _PreviewMode;
    float _PreviewIndicatorLineThickness;
    float _AnimationEnabled;
    float _AnimationSpeed;
    float _DefaultEffectRadius;
    float _EnableDefaultEffectRadius;
    float _TransitionDuration;
    float _TexturedEmissionEdge;
    float _TexturedEmissionEdgeStrength;
    float _IsometricExclusion;
    float _IsometricExclusionDistance;
    float _IsometricExclusionGradientLength;
    float _Floor;
    float _FloorMode;
    float _FloorY;
    float _FloorYTextureGradientLength;
    float _PlayerPosYOffset;
    float _AffectedAreaFloor;
    float _Ceiling;
    float _CeilingMode;
    float _CeilingBlendMode;
    float _CeilingY;
    float _CeilingPlayerYOffset;
    float _CeilingYGradientLength;
    float _Zoning;
    float _ZoningMode;
    float _ZoningEdgeGradientLength;
    float _IsZoningRevealable;
    float _SyncZonesWithFloorY;
    float _SyncZonesFloorYOffset;

    half _UseCustomTime;

    half _CrossSectionEnabled;
    half4 _CrossSectionColor;
    half _CrossSectionTextureEnabled;
    float _CrossSectionTextureScale;
    half _CrossSectionUVScaledByDistance;
    half _DissolveMethod;
    half _DissolveTexSpace;
         CBUFFER_END
    sampler2D _NoiseMap1;
    sampler2D _NoiseMap2;

    sampler2D _HatchingMap;
    sampler2D _GradientTex;
    void Ext_ModifyVertex0 (inout VertexData v, inout ExtraV2F d)
    {
        float4x4 UnityObjectToClipPosMatrix = mul(GetWorldToHClipMatrix(), GetObjectToWorldMatrix());
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
    TEXTURE2D(_BaseMap);        SAMPLER(sampler_BaseMap);
    TEXTURE2D(_BumpMap);        SAMPLER(sampler_BumpMap);
    TEXTURE2D(_EmissionMap);    SAMPLER(sampler_EmissionMap);
    TEXTURE2D(_SpecGlossMap);       SAMPLER(sampler_SpecGlossMap);  
	TEXTURE2D(_ParallaxMap);        SAMPLER(sampler_ParallaxMap);
	TEXTURE2D(_OcclusionMap);       SAMPLER(sampler_OcclusionMap);
	TEXTURE2D(_DetailMask);         SAMPLER(sampler_DetailMask);
	TEXTURE2D(_DetailAlbedoMap);    SAMPLER(sampler_DetailAlbedoMap);
	TEXTURE2D(_DetailNormalMap);    SAMPLER(sampler_DetailNormalMap);
	TEXTURE2D(_MetallicGlossMap);   SAMPLER(sampler_MetallicGlossMap);
	TEXTURE2D(_ClearCoatMap);       SAMPLER(sampler_ClearCoatMap);
    #if defined(_DETAIL_MULX2) || defined(_DETAIL_SCALED)
		#define _DETAIL
	#endif

	#if _SPECULAR_SETUP
		#define _USESPECULAR 1
	#else
		#undef _USESPECULAR
	#endif
	float SharpenAlphaMy(float alpha, float alphaClipTreshold)
	{
	    return saturate((alpha - alphaClipTreshold) / max(fwidth(alpha), 0.0001) + 0.5);
	}
#if UNITY_VERSION < 202200
	half3 AlphaModulate(half3 albedo, half alpha)
	{
		#if defined(_ALPHAMODULATE_ON)
		    return lerp(half3(1.0, 1.0, 1.0), albedo, alpha);
		#else
		    return albedo;
		#endif
	}

	#if defined(_ALPHATEST_ON)
		bool IsAlphaToMaskAvailable()
		{
		    return (_AlphaToMaskAvailable != 0.0);
		}
		half AlphaClip(half alpha, half cutoff)
		{
		    half clippedAlpha = (alpha >= cutoff) ? float(alpha) : 0.0;

		    half alphaToCoverageAlpha = SharpenAlphaMy(alpha, cutoff);

		    alpha = IsAlphaToMaskAvailable() ? alphaToCoverageAlpha : clippedAlpha;
		    clip(alpha - 0.0001);

		    return alpha;
		}
	#endif
#endif
    int _DebugSceneOverrideMode;
    bool IsAlphaDiscardEnabledMy()
    {
        #if defined(DEBUG_DISPLAY)
        return (_DebugSceneOverrideMode == DEBUGSCENEOVERRIDEMODE_NONE);
        #else
        return true;
        #endif
    }
	half Alpha(half albedoAlpha, half4 color, half cutoff)
	{
		#if !defined(_SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A) && !defined(_GLOSSINESS_FROM_BASE_ALPHA)
		    half alpha = albedoAlpha * color.a;
		#else
		    half alpha = color.a;
		#endif
			#ifdef _ALPHATEST_ON
			    if (IsAlphaDiscardEnabledMy())
			        alpha = AlphaClip(alpha, cutoff);
			#endif

		    return alpha;
	}
 	half4 SampleAlbedoAlpha(float2 uv, TEXTURE2D_PARAM(albedoAlphaMap, sampler_albedoAlphaMap))
	{
	    return half4(SAMPLE_TEXTURE2D(albedoAlphaMap, sampler_albedoAlphaMap, uv));
	}
 	half3 SampleNormal(float2 uv, TEXTURE2D_PARAM(bumpMap, sampler_bumpMap), half scale = half(1.0))
	{
	#ifdef _NORMALMAP
	    half4 n = SAMPLE_TEXTURE2D(bumpMap, sampler_bumpMap, uv);
	    #if BUMP_SCALE_NOT_SUPPORTED
	        return UnpackNormal(n);
	    #else
	        return UnpackNormalScale(n, scale);
	    #endif
	#else
	    return half3(0.0h, 0.0h, 1.0h);
	#endif
	}
#if UNITY_VERSION < 202100
	half4 SampleSpecularSmoothnessMy(half2 uv, half alpha, half4 specColor, TEXTURE2D_PARAM( specMap, sampler_specMap))
	{
		half4 specularSmoothness = half4(0.0h, 0.0h, 0.0h, 1.0h);
		#ifdef _SPECGLOSSMAP
			specularSmoothness = SAMPLE_TEXTURE2D(specMap, sampler_specMap, uv) * specColor;
		#elif defined(_SPECULAR_COLOR)
			specularSmoothness = specColor;
		#endif

		#ifdef _GLOSSINESS_FROM_BASE_ALPHA
			specularSmoothness.a = exp2(10 * alpha + 1);
		#else
			specularSmoothness.a = exp2(10 * specularSmoothness.a + 1);
		#endif
		return specularSmoothness;
	}
#else

	half4 SampleSpecularSmoothnessMy(float2 uv, half alpha, half4 specColor, TEXTURE2D_PARAM(specMap, sampler_specMap))
	{
		half4 specularSmoothness = half4(0, 0, 0, 1);
	#ifdef _SPECGLOSSMAP
		specularSmoothness = SAMPLE_TEXTURE2D(specMap, sampler_specMap, uv) * specColor;
	#elif defined(_SPECULAR_COLOR)
		specularSmoothness = specColor;
	#endif

	#ifdef _GLOSSINESS_FROM_BASE_ALPHA
		specularSmoothness.a = alpha;
	#endif

		return specularSmoothness;
	}
#endif
    half3 SampleEmissionMy(float2 uv, half3 emissionColor, TEXTURE2D_PARAM(emissionMap, sampler_emissionMap))
    {
    #ifndef _EMISSION
        return 0;
    #else
        return SAMPLE_TEXTURE2D(emissionMap, sampler_emissionMap, uv).rgb * emissionColor;
    #endif
    }
	#ifndef BUILTIN_TARGET_API
		half2 ParallaxOffset1Step(half height, half amplitude, half3 viewDirTS)
		{
		    height = height * amplitude - amplitude / 2.0;
		    half3 v = normalize(viewDirTS);
		    v.z += 0.42;
		    return height * (v.xy / v.z);
		}
	#endif

	float2 ParallaxMapping(TEXTURE2D_PARAM(heightMap, sampler_heightMap), half3 viewDirTS, half scale, float2 uv)
	{
	    half h = SAMPLE_TEXTURE2D(heightMap, sampler_heightMap, uv).g;
	    float2 offset = ParallaxOffset1Step(h, scale, viewDirTS);
	    return offset;
	}
	#ifdef _SPECULAR_SETUP
	    #define SAMPLE_METALLICSPECULAR(uv) SAMPLE_TEXTURE2D(_SpecGlossMap, sampler_SpecGlossMap, uv)
	#else
	    #define SAMPLE_METALLICSPECULAR(uv) SAMPLE_TEXTURE2D(_MetallicGlossMap, sampler_MetallicGlossMap, uv)
	#endif

	half4 SampleMetallicSpecGloss(float2 uv, half albedoAlpha)
	{
	    half4 specGloss;

		#ifdef _METALLICSPECGLOSSMAP
		    specGloss = half4(SAMPLE_METALLICSPECULAR(uv));
		    #ifdef _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
		        specGloss.a = albedoAlpha * _Smoothness;
		    #else
		        specGloss.a *= _Smoothness;
		    #endif
		#else 
		    #if _SPECULAR_SETUP
		        specGloss.rgb = _SpecColor.rgb;
		    #else
		        specGloss.rgb = _Metallic.rrr;
		    #endif

		    #ifdef _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
		        specGloss.a = albedoAlpha * _Smoothness;
		    #else
		        specGloss.a = _Smoothness;
		    #endif
		#endif

	    return specGloss;
	}

	half SampleOcclusion(float2 uv)
	{
	    #ifdef _OCCLUSIONMAP
	        half occ = SAMPLE_TEXTURE2D(_OcclusionMap, sampler_OcclusionMap, uv).g;
	        return LerpWhiteTo(occ, _OcclusionStrength);
	    #else
	        return half(1.0);
	    #endif
	}
    half SampleOcclusionMy(float2 uv)
    {
    #ifdef _OCCLUSIONMAP
    #if defined(SHADER_API_GLES)
        return SAMPLE_TEXTURE2D(_OcclusionMap, sampler_OcclusionMap, uv).g;
    #else
        half occ = SAMPLE_TEXTURE2D(_OcclusionMap, sampler_OcclusionMap, uv).g;
        return LerpWhiteTo(occ, _OcclusionStrength);
    #endif
    #else
        return 1.0;
    #endif
    }
	half2 SampleClearCoat(float2 uv)
	{
		#if defined(_CLEARCOAT) || defined(_CLEARCOATMAP)
		    half2 clearCoatMaskSmoothness = half2(_ClearCoatMask, _ClearCoatSmoothness);

		#if defined(_CLEARCOATMAP)
		    clearCoatMaskSmoothness *= SAMPLE_TEXTURE2D(_ClearCoatMap, sampler_ClearCoatMap, uv).rg;
		#endif

		    return clearCoatMaskSmoothness;
		#else
		    return half2(0.0, 1.0);
		#endif  
	}

	void ApplyPerPixelDisplacement(half3 viewDirTS, inout float2 uv)
	{
		#if defined(_PARALLAXMAP)
		    uv += ParallaxMapping(TEXTURE2D_ARGS(_ParallaxMap, sampler_ParallaxMap), viewDirTS, _Parallax, uv);
		#endif
	}

	half3 ScaleDetailAlbedo(half3 detailAlbedo, half scale)
	{
	    return half(2.0) * detailAlbedo * scale - scale + half(1.0);
	}

	half3 ApplyDetailAlbedo(float2 detailUv, half3 albedo, half detailMask)
	{
		#if defined(_DETAIL)
		    half3 detailAlbedo = SAMPLE_TEXTURE2D(_DetailAlbedoMap, sampler_DetailAlbedoMap, detailUv).rgb;
		#if defined(_DETAIL_SCALED)
		    detailAlbedo = ScaleDetailAlbedo(detailAlbedo, _DetailAlbedoMapScale);
		#else
		    detailAlbedo = half(2.0) * detailAlbedo;
		#endif

		    return albedo * LerpWhiteTo(detailAlbedo, detailMask);
		#else
		    return albedo;
		#endif
	}

	half3 ApplyDetailNormal(float2 detailUv, half3 normalTS, half detailMask)
	{
		#if defined(_DETAIL)
		#if BUMP_SCALE_NOT_SUPPORTED
		    half3 detailNormalTS = UnpackNormal(SAMPLE_TEXTURE2D(_DetailNormalMap, sampler_DetailNormalMap, detailUv));
		#else
		    half3 detailNormalTS = UnpackNormalScale(SAMPLE_TEXTURE2D(_DetailNormalMap, sampler_DetailNormalMap, detailUv), _DetailNormalMapScale);
		#endif
		    detailNormalTS = normalize(detailNormalTS);

		    return lerp(normalTS, BlendNormalRNM(normalTS, detailNormalTS), detailMask); 
		#else
		    return normalTS;
		#endif
	}
	void Ext_SurfaceFunction1 (inout Surface o, ShaderData d)
	{

        float4 texcoords;
        texcoords.xy = d.texcoord0.xy * _BaseMap_ST.xy + _BaseMap_ST.zw; 
        float2 uv = texcoords.xy;

#if _SHADING_BLINNPHONG || _SHADING_COLOR
        half4 diffuseAlpha = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap,  texcoords.xy);
        o.Albedo = diffuseAlpha.rgb * _BaseColor.rgb;
	#if !_PASSFORWARD
		if(_EnableToonShading == 0 || _ShadingAffectedByNormalMap == 1) 
		{
			o.Normal = SampleNormal(texcoords.xy, TEXTURE2D_ARGS(_BumpMap, sampler_BumpMap), _BumpScale);
		}
	#else
        o.Normal = SampleNormal(texcoords.xy, TEXTURE2D_ARGS(_BumpMap, sampler_BumpMap), _BumpScale);
	#endif
		o.Alpha = diffuseAlpha.a;
        o.Alpha = o.Alpha * _BaseColor.a;
        AlphaDiscard(o.Alpha, _Cutoff);
        half3 emission = SampleEmissionMy(uv, _EmissionColor.rgb, TEXTURE2D_ARGS(_EmissionMap, sampler_EmissionMap));
        o.Emission = emission;
#endif

      if(_ShadingMode == 1 && _LightFunction == 0 ) 
      {
#if _SHADING_BLINNPHONG
        half4 specular = SampleSpecularSmoothnessMy(uv, o.Alpha, _SpecColor, TEXTURE2D_ARGS(_SpecGlossMap, sampler_SpecGlossMap));
        half smoothness =  specular.a;

        o.Specular = specular;
        o.Smoothness = smoothness;
#endif

      } 
      else if (_ShadingMode == 1 && _LightFunction == 1) 
      {
#if _SHADING_PBR
		#if defined(_PARALLAXMAP)
		    ApplyPerPixelDisplacement(d.tangentSpaceViewDir, uv);
		#endif
    	half4 albedoAlpha =  SampleAlbedoAlpha(uv, TEXTURE2D_ARGS(_BaseMap, sampler_BaseMap));
	    o.Alpha = Alpha(albedoAlpha.a, _BaseColor, _Cutoff);

	    half4 specGloss = SampleMetallicSpecGloss(uv, albedoAlpha.a);
	    o.Albedo = albedoAlpha.rgb * _BaseColor.rgb;
	    o.Albedo = AlphaModulate(o.Albedo, o.Alpha);
		#if _SPECULAR_SETUP
		    o.Metallic = half(1.0);
		    o.Specular = specGloss.rgb;
		#else
		    o.Metallic = specGloss.r;
		    o.Specular = half3(0.0, 0.0, 0.0);
		#endif
        o.Smoothness = specGloss.a;
	#if !_PASSFORWARD
		if(_EnableToonShading == 0 || _ShadingAffectedByNormalMap == 1) 
		{
			o.Normal = SampleNormal(uv, TEXTURE2D_ARGS(_BumpMap, sampler_BumpMap), _BumpScale);
		}
	#else
		o.Normal = SampleNormal(uv, TEXTURE2D_ARGS(_BumpMap, sampler_BumpMap), _BumpScale);
	#endif
	    o.Occlusion = SampleOcclusion(uv);
	    o.Emission = SampleEmissionMy(uv, _EmissionColor.rgb, TEXTURE2D_ARGS(_EmissionMap, sampler_EmissionMap));

		#if defined(_DETAIL)
		    half detailMask = SAMPLE_TEXTURE2D(_DetailMask, sampler_DetailMask, uv).a;
		    float2 detailUv = uv * _DetailAlbedoMap_ST.xy + _DetailAlbedoMap_ST.zw;
		    o.Albedo = ApplyDetailAlbedo(detailUv, o.Albedo, detailMask);
		    o.Normal = ApplyDetailNormal(detailUv, o.Normal, detailMask);
		#endif

#endif
      }

	}
    float _ArrayLength = 0;
    #if (defined(SHADER_API_GLES) || defined(SHADER_API_GLES3)) 
        float4 _PlayersPosVectorArray[20];
        float _PlayersDataFloatArray[150];     
    #else
        float4 _PlayersPosVectorArray[100];
        float _PlayersDataFloatArray[500];  
    #endif
    #if _ZONING
        #if (defined(SHADER_API_GLES) || defined(SHADER_API_GLES3)) 
            float _ZDFA[500];
        #else
            float _ZDFA[1000];
        #endif
        float _ZonesDataCount;
    #endif

    float _STSCustomTime = 0;
    #if _REPLACEMENT        
        half4 _DissolveColorGlobal;
        float _DissolveColorSaturationGlobal;
        float _DissolveEmissionGlobal;
        float _DissolveEmissionBoosterGlobal;
        float _TextureVisibilityGlobal;
        float _ObstructionGlobal;
        float _AngleStrengthGlobal;
        float _ConeStrengthGlobal;
        float _ConeObstructionDestroyRadiusGlobal;
        float _CylinderStrengthGlobal;
        float _CylinderObstructionDestroyRadiusGlobal;
        float _CircleStrengthGlobal;
        float _CircleObstructionDestroyRadiusGlobal;
        float _CurveStrengthGlobal;
        float _CurveObstructionDestroyRadiusGlobal;
        float _DissolveFallOffGlobal;
        float _AffectedAreaPlayerBasedObstructionGlobal;
        float _IntrinsicDissolveStrengthGlobal;
        float _PreviewModeGlobal;
        float _UVsGlobal;
        float _hasClippedShadowsGlobal;
        float _FloorGlobal;
        float _FloorModeGlobal;
        float _FloorYGlobal;
        float _PlayerPosYOffsetGlobal;
        float _FloorYTextureGradientLengthGlobal;
        float _AffectedAreaFloorGlobal;
        float _AnimationEnabledGlobal;
        float _AnimationSpeedGlobal;
        float _DefaultEffectRadiusGlobal;
        float _EnableDefaultEffectRadiusGlobal;
        float _TransitionDurationGlobal;        
        float _TexturedEmissionEdgeGlobal;
        float _TexturedEmissionEdgeStrengthGlobal;
        float _IsometricExclusionGlobal;
        float _IsometricExclusionDistanceGlobal;
        float _IsometricExclusionGradientLengthGlobal;
        float _CeilingGlobal;
        float _CeilingModeGlobal;
        float _CeilingBlendModeGlobal;
        float _CeilingYGlobal;
        float _CeilingPlayerYOffsetGlobal;
        float _CeilingYGradientLengthGlobal;
        float _ZoningGlobal;
        float _ZoningModeGlobal;
        float _ZoningEdgeGradientLengthGlobal;
        float _IsZoningRevealableGlobal;
        float _SyncZonesWithFloorYGlobal;
        float _SyncZonesFloorYOffsetGlobal;
        float4 _ObstructionCurveGlobal_TexelSize;
        float4 _DissolveMaskGlobal_TexelSize;
        float _DissolveMaskEnabledGlobal;
        float _PreviewIndicatorLineThicknessGlobal;
        half _UseCustomTimeGlobal;

        half _CrossSectionEnabledGlobal;
        half4 _CrossSectionColorGlobal;
        half _CrossSectionTextureEnabledGlobal;
        float _CrossSectionTextureScaleGlobal;
        half _CrossSectionUVScaledByDistanceGlobal;

        half _DissolveMethodGlobal;
        half _DissolveTexSpaceGlobal;
        half _ObstructionPlayerOffsetGlobal;

        float4 _DissolveTexGlobal_TexelSize;
    #endif
    #if _REPLACEMENT
        sampler2D _DissolveTexGlobal;
    #else
        sampler2D _DissolveTex;
    #endif
    #if _REPLACEMENT
        sampler2D _DissolveMaskGlobal;
    #else
        sampler2D _DissolveMask;
    #endif
    #if _REPLACEMENT
        sampler2D _ObstructionCurveGlobal;
    #else
        sampler2D _ObstructionCurve;
    #endif

    #if _REPLACEMENT
        sampler2D _CrossSectionTextureGlobal;
    #else
        sampler2D _CrossSectionTexture;
    #endif
#ifdef USE_UNITY_TEXTURE_2D_TYPE
#undef USE_UNITY_TEXTURE_2D_TYPE
#endif

#include "Packages/com.shadercrew.seethroughshader.core/Scripts/Shaders/xxSharedSTSDependencies/SeeThroughShaderFunction.hlsl"
	void Ext_SurfaceFunction2 (inout Surface o, ShaderData d)
	{
        half3 albedo;
        half3 emission;
        float alphaForClipping;
#ifdef USE_UNITY_TEXTURE_2D_TYPE
#undef USE_UNITY_TEXTURE_2D_TYPE
#endif
#if _REPLACEMENT
    DoSeeThroughShading( o.Albedo, o.Normal, d.worldSpacePosition, d.worldSpaceNormal, d.screenPos,
                        _numOfPlayersInside, _tDirection, _tValue, _id,
                        _TriggerMode, _RaycastMode,
                        _IsExempt,

                        _DissolveMethodGlobal, _DissolveTexSpaceGlobal,

                        _DissolveColorGlobal, _DissolveColorSaturationGlobal, _UVsGlobal,
                        _DissolveEmissionGlobal, _DissolveEmissionBoosterGlobal, _TexturedEmissionEdgeGlobal, _TexturedEmissionEdgeStrengthGlobal,
                        _hasClippedShadowsGlobal,

                        _ObstructionGlobal,
                        _ObstructionPlayerOffsetGlobal,
                        _AngleStrengthGlobal,
                        _ConeStrengthGlobal, _ConeObstructionDestroyRadiusGlobal,
                        _CylinderStrengthGlobal, _CylinderObstructionDestroyRadiusGlobal,
                        _CircleStrengthGlobal, _CircleObstructionDestroyRadiusGlobal,
                        _CurveStrengthGlobal, _CurveObstructionDestroyRadiusGlobal,
                        _DissolveFallOffGlobal,
                        _DissolveMaskEnabledGlobal,
                        _AffectedAreaPlayerBasedObstructionGlobal,
                        _IntrinsicDissolveStrengthGlobal,
                        _DefaultEffectRadiusGlobal, _EnableDefaultEffectRadiusGlobal,

                        _IsometricExclusionGlobal, _IsometricExclusionDistanceGlobal, _IsometricExclusionGradientLengthGlobal,
                        _CeilingGlobal, _CeilingModeGlobal, _CeilingBlendModeGlobal, _CeilingYGlobal, _CeilingPlayerYOffsetGlobal, _CeilingYGradientLengthGlobal,
                        _FloorGlobal, _FloorModeGlobal, _FloorYGlobal, _PlayerPosYOffsetGlobal, _FloorYTextureGradientLengthGlobal, _AffectedAreaFloorGlobal,

                        _AnimationEnabledGlobal, _AnimationSpeedGlobal,
                        _TransitionDurationGlobal,

                        _UseCustomTimeGlobal,

                        _ZoningGlobal, _ZoningModeGlobal, _IsZoningRevealableGlobal, _ZoningEdgeGradientLengthGlobal,
                        _SyncZonesWithFloorYGlobal, _SyncZonesFloorYOffsetGlobal,

                        _PreviewModeGlobal,
                        _PreviewIndicatorLineThicknessGlobal,

                        _DissolveTexGlobal,
                        _DissolveMaskGlobal,
                        _ObstructionCurveGlobal,

                        _DissolveTexGlobal_TexelSize,
                        _DissolveMaskGlobal_TexelSize,
                        _ObstructionCurveGlobal_TexelSize,

                        albedo, emission, alphaForClipping);
#else 
    DoSeeThroughShading( o.Albedo, o.Normal, d.worldSpacePosition, d.worldSpaceNormal, d.screenPos,

                        _numOfPlayersInside, _tDirection, _tValue, _id,
                        _TriggerMode, _RaycastMode,
                        _IsExempt,

                        _DissolveMethod, _DissolveTexSpace,

                        _DissolveColor, _DissolveColorSaturation, _UVs,
                        _DissolveEmission, _DissolveEmissionBooster, _TexturedEmissionEdge, _TexturedEmissionEdgeStrength,
                        _hasClippedShadows,

                        _Obstruction,
                        _ObstructionPlayerOffset,
                        _AngleStrength,
                        _ConeStrength, _ConeObstructionDestroyRadius,
                        _CylinderStrength, _CylinderObstructionDestroyRadius,
                        _CircleStrength, _CircleObstructionDestroyRadius,
                        _CurveStrength, _CurveObstructionDestroyRadius,
                        _DissolveFallOff,
                        _DissolveMaskEnabled,
                        _AffectedAreaPlayerBasedObstruction,
                        _IntrinsicDissolveStrength,
                        _DefaultEffectRadius, _EnableDefaultEffectRadius,

                        _IsometricExclusion, _IsometricExclusionDistance, _IsometricExclusionGradientLength,
                        _Ceiling, _CeilingMode, _CeilingBlendMode, _CeilingY, _CeilingPlayerYOffset, _CeilingYGradientLength,
                        _Floor, _FloorMode, _FloorY, _PlayerPosYOffset, _FloorYTextureGradientLength, _AffectedAreaFloor,

                        _AnimationEnabled, _AnimationSpeed,
                        _TransitionDuration,

                        _UseCustomTime,

                        _Zoning, _ZoningMode , _IsZoningRevealable, _ZoningEdgeGradientLength,
                        _SyncZonesWithFloorY, _SyncZonesFloorYOffset,

                        _PreviewMode,
                        _PreviewIndicatorLineThickness,                            

                        _DissolveTex,
                        _DissolveMask,
                        _ObstructionCurve,

                        _DissolveTex_TexelSize,
                        _DissolveMask_TexelSize,
                        _ObstructionCurve_TexelSize,

                        albedo, emission, alphaForClipping);
#endif 
        o.Albedo = albedo;
        o.Emission += emission;   

	}
    void Ext_FinalColorForward2 (Surface o, ShaderData d, inout half4 color)
    {
        #if _REPLACEMENT   
            DoCrossSection(_CrossSectionEnabledGlobal,
                        _CrossSectionColorGlobal,
                        _CrossSectionTextureEnabledGlobal,
                        _CrossSectionTextureGlobal,
                        _CrossSectionTextureScaleGlobal,
                        _CrossSectionUVScaledByDistanceGlobal,
                        d.isFrontFace,
                        d.screenPos,
                        color);
        #else 
            DoCrossSection(_CrossSectionEnabled,
                        _CrossSectionColor,
                        _CrossSectionTextureEnabled,
                        _CrossSectionTexture,
                        _CrossSectionTextureScale,
                        _CrossSectionUVScaledByDistance,
                        d.isFrontFace,
                        d.screenPos,
                        color);
        #endif
    }
            void ChainSurfaceFunction(inout Surface l, inout ShaderData d)
            {
                  Ext_SurfaceFunction1(l, d);
                  Ext_SurfaceFunction2(l, d);
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
                  Ext_FinalColorForward2(l, d, color);
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
             d.screenUV = (IN.ScreenPosition.xy / max(0.01, IN.ScreenPosition.w));
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
             d.isFrontFace = facing;
            #if _HDRP
            #else
            #endif
             d.screenPos = i.screenPos;
             d.screenUV = (i.screenPos.xy / i.screenPos.w);
             d.extraV2F0 = i.extraV2F0;
            return d;
         }

#endif
         #if defined(_PASSSHADOW)
            float3 _LightDirection;
            float3 _LightPosition;
         #endif
         VertexToPixel Vert (VertexData v)
         {
           VertexToPixel o = (VertexToPixel)0;

           UNITY_SETUP_INSTANCE_ID(v);
           UNITY_TRANSFER_INSTANCE_ID(v, o);
           UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
#if !_TESSELLATION_ON
           ChainModifyVertex(v, o, _Time);
#endif

            o.texcoord0 = v.texcoord0;
           VertexPositionInputs vertexInput = GetVertexPositionInputs(v.vertex.xyz);
           o.worldPos = TransformObjectToWorld(v.vertex.xyz);
           o.worldNormal = TransformObjectToWorldNormal(v.normal);
           o.worldTangent = float4(TransformObjectToWorldDir(v.tangent.xyz), v.tangent.w);
          #if _PASSSHADOW
              #if _CASTING_PUNCTUAL_LIGHT_SHADOW
                 float3 lightDirectionWS = normalize(_LightPosition - o.worldPos);
              #else
                 float3 lightDirectionWS = _LightDirection;
              #endif
              o.pos = TransformWorldToHClip(ApplyShadowBias(o.worldPos, o.worldNormal, lightDirectionWS));
              #if UNITY_REVERSED_Z
                  o.pos.z = min(o.pos.z, UNITY_NEAR_CLIP_VALUE);
              #else
                  o.pos.z = max(o.pos.z, UNITY_NEAR_CLIP_VALUE);
              #endif
          #elif _PASSMETA
              o.pos = MetaVertexPosition(float4(v.vertex.xyz, 0), v.texcoord1.xy, v.texcoord2.xy, unity_LightmapST, unity_DynamicLightmapST);
          #else
              o.pos = TransformWorldToHClip(o.worldPos);
          #endif
           o.screenPos = ComputeScreenPos(o.pos, _ProjectionParams.x);
          #if _PASSFORWARD || _PASSGBUFFER
              float2 uv1 = v.texcoord1.xy;
              OUTPUT_LIGHTMAP_UV(uv1, unity_LightmapST, o.lightmapUV);
              OUTPUT_SH(o.worldNormal, o.sh);
              #if defined(DYNAMICLIGHTMAP_ON)
                   o.dynamicLightmapUV.xy = v.texcoord2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
              #endif
          #endif

          #ifdef VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
              half fogFactor = 0;
              #if defined(_FOG_FRAGMENT)
                fogFactor = ComputeFogFactor(o.pos.z);
              #endif
              #if _BAKEDLIT
                 o.fogFactorAndVertexLight = half4(fogFactor, 0, 0, 0);
              #else
                 half3 vertexLight = VertexLighting(o.worldPos, o.worldNormal);
                 o.fogFactorAndVertexLight = half4(fogFactor, vertexLight);
              #endif
          #endif

          #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
             o.shadowCoord = GetShadowCoord(vertexInput);
          #endif

           return o;
         }
         void Frag (VertexToPixel IN
            , out half4 outNormalWS : SV_Target0
         #ifdef _WRITE_RENDERING_LAYERS
            , out float4 outRenderingLayers : SV_Target1
         #endif
            #ifdef _DEPTHOFFSET_ON
              , out float outputDepth : SV_Depth
            #endif
            #if NEED_FACING
               , bool facing : SV_IsFrontFace
            #endif
         )
         {
           UNITY_SETUP_INSTANCE_ID(IN);
           UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);

           #if defined(LOD_FADE_CROSSFADE)
              LODFadeCrossFade(IN.pos);
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

          #if defined(_GBUFFER_NORMALS_OCT)
              float3 normalWS = d.worldSpaceNormal;
              float2 octNormalWS = PackNormalOctQuadEncode(normalWS);           
              float2 remappedOctNormalWS = saturate(octNormalWS * 0.5 + 0.5);   
              half3 packedNormalWS = PackFloat2To888(remappedOctNormalWS);      
              outNormalWS = half4(packedNormalWS, 0.0);
          #else
              float3 wsn = l.Normal;
              #if !_WORLDSPACENORMAL
                wsn = TangentToWorldSpace(d, l.Normal);
              #endif
              outNormalWS = half4(NormalizeNormalPerPixel(wsn), 0.0);
          #endif

          #ifdef _WRITE_RENDERING_LAYERS
            uint renderingLayers = GetMeshRenderingLayer();
            outRenderingLayers = float4(EncodeMeshRenderingLayer(renderingLayers), 0, 0, 0);
          #endif
         }

         ENDHLSL

      }
   }
   CustomEditor "ShaderCrew.TheToonShader.TheToonShaderGUIEditor"
}
