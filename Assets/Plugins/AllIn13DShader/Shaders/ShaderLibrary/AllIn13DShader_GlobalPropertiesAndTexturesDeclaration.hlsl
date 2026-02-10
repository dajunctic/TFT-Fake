#ifndef ALLIN13DSHADER_GLOBALPROPERTIESANDTEXTURESDECLARATION
#define ALLIN13DSHADER_GLOBALPROPERTIESANDTEXTURESDECLARATION

DECLARE_TEX2D(_MainTex)

#if defined(_TEXTURE_BLENDING_ON)
	#if defined(_TEXTUREBLENDINGMODE_RGB)
		DECLARE_TEX2D(_BlendingTextureG)
		DECLARE_TEX2D(_BlendingTextureB)
	#else
		DECLARE_TEX2D(_BlendingTextureWhite)
	#endif

	#if defined(_TEXTUREBLENDINGSOURCE_TEXTURE)
		DECLARE_TEX2D(_TexBlendingMask)
	#endif

	#if defined(_NORMAL_MAP_ON)
		#if defined(_TEXTUREBLENDINGMODE_RGB)
			DECLARE_TEX2D(_BlendingNormalMapG)
			DECLARE_TEX2D(_BlendingNormalMapB)
		#else
			DECLARE_TEX2D(_BlendingNormalMapWhite)
		#endif
	#endif
#endif

#if defined(REQUIRE_TANGENT_WS)
	DECLARE_TEX2D(_NormalMap)
#endif

#if defined(_COLOR_RAMP_ON)
	DECLARE_TEX2D(_ColorRampTex)
#endif

// ----- Global Properties
float global_MinDepth;
float global_DepthZoneLength;
float global_DepthGradientFallOff;

#if defined(REQUIRE_SCENE_DEPTH)
	DECLARE_TEX2D(global_DepthGradient)
#endif

#if defined(_CUSTOM_SHADOW_COLOR_ON)
	float4 global_shadowColor;
#endif

float4 allIn13DShader_globalTime;

#if defined(_LIGHTMODEL_FASTLIGHTING)
	float3 global_lightDirection;
	float4 global_lightColor;
#endif

#if defined(_WIND_ON)
	DECLARE_TEX2D(global_windNoiseTex)
	float global_windForce;
	float2 global_noiseSpeed;
	float global_useWindDir;
	float3 global_windDir;
	float global_minWindValue;
	float global_maxWindValue;
	float global_windWorldSize;
#endif
//------------------------

#if defined(_SHADINGMODEL_PBR)
	#if defined(_METALLIC_MAP_ON)
		DECLARE_TEX2D(_MetallicMap)
	#endif
#endif

#if defined(SPECULAR_ON)
	DECLARE_TEX2D(_SpecularMap)
#endif

#if defined(_LIGHTMODEL_TOONRAMP)
	DECLARE_TEX2D(_ToonRamp)
#endif

#if defined(_AOMAP_ON)
	DECLARE_TEX2D(_AOMap)
#endif

#if defined(_SUBSURFACE_SCATTERING_ON)
	DECLARE_TEX2D(_SSSMap)
#endif

#if defined(_FADE_ON)
	DECLARE_TEX2D(_FadeTex)
#endif

#if defined(_VERTEX_DISTORTION_ON) || defined(URP_PASS)
	DECLARE_TEX2D(_VertexDistortionNoiseTex)
#endif

#if defined(_MATCAP_ON)
	DECLARE_TEX2D(_MatcapTex)
#endif

#if defined(_UV_DISTORTION_ON)
	DECLARE_TEX2D(_DistortTex)
#endif

#if defined(_EMISSION_ON)
	DECLARE_TEX2D(_EmissionMap)
#endif

#if defined(_TRIPLANAR_MAPPING_ON)
	DECLARE_TEX2D(_TriplanarTopTex)
	DECLARE_TEX2D(_TriplanarTopNormalMap)
#endif

#endif //ALLIN13DSHADER_GLOBALPROPERTIESANDTEXTURESDECLARATION