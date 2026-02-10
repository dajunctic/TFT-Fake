#ifndef ALLIN13DSHADER_FEATURES
#define ALLIN13DSHADER_FEATURES

#if defined(INSTANCING_ON) && !defined(PROCEDURAL_INSTANCING_ON)
	#define ALLIN13D_GPU_INSTANCING
#endif

#if defined(UNITY_DOTS_INSTANCING_ENABLED) || defined(URP_PASS)
	#define ALWAYS_DECLARE_PROPERTY
#endif

#if defined(ALLIN13D_GPU_INSTANCING)
	#define BATCHING_BUFFER_START	UNITY_INSTANCING_BUFFER_START(UnityPerMaterial)
	#define BATCHING_BUFFER_END		UNITY_INSTANCING_BUFFER_END(UnityPerMaterial)

#else
	#if defined(URP_PASS) && !defined(PROCEDURAL_INSTANCING_ON)
		#define BATCHING_BUFFER_START	CBUFFER_START(UnityPerMaterial)
		#define BATCHING_BUFFER_END		CBUFFER_END
	#else
		#define BATCHING_BUFFER_START
		#define BATCHING_BUFFER_END	
	#endif
#endif

#if defined(URP_PASS) && defined(_LIGHT_LAYERS)
	#define ALLIN1_USE_LIGHT_LAYERS
#endif

#ifdef URP_PASS
	#define DECLARE_TEX2D(texName) \
		TEXTURE2D(texName); \
		SAMPLER(sampler_##texName);

	#define SAMPLE_TEX2D(texName, uv)		SAMPLE_TEXTURE2D(texName, sampler_##texName, uv)
	#define SAMPLE_TEX2D_DERIVATIVES(texName, uv, ddx, ddy) SAMPLE_TEXTURE2D_GRAD(texName, sampler_##texName, uv, ddx, ddy)
	#define SAMPLE_TEX2D_LOD(texName, uv)	SAMPLE_TEXTURE2D_LOD(texName, sampler_##texName, uv.xy, 0)
	#define SAMPLE_TEX2D_PROJ(texName, uv)	SAMPLE_TEXTURE2D(texName, sampler##texName, uv.xy/uv.w)
#else
	#define DECLARE_TEX2D(texName)	sampler2D texName;

	#define SAMPLE_TEX2D(texName, uv)						tex2D(texName, uv)
	#define SAMPLE_TEX2D_DERIVATIVES(texName, uv, ddx, ddy) tex2D(texName, uv, ddx, ddy)
	#define SAMPLE_TEX2D_LOD(texName, uv)					tex2Dlod(texName, uv)
	#define SAMPLE_TEX2D_PROJ(texName, uv)					tex2Dproj(texName, uv)
#endif

#define STOCHASTIC_SAMPLING_NO_DEF_DD(texName, uv, stochasticOffset, res) \
	dx = ddx(uv);\
	dy = ddy(uv);\
	res = mul(SAMPLE_TEX2D_DERIVATIVES(texName, uv + hash2D2D(stochasticOffset[0].xy), dx, dy), stochasticOffset[3].x) + \
		mul(SAMPLE_TEX2D_DERIVATIVES(texName, uv + hash2D2D(stochasticOffset[1].xy), dx, dy), stochasticOffset[3].y) + \
		mul(SAMPLE_TEX2D_DERIVATIVES(texName, uv + hash2D2D(stochasticOffset[2].xy), dx, dy), stochasticOffset[3].z);
	
#define STOCHASTIC_SAMPLING(texName, uv, stochasticOffset, res) \
	float2 dx = 0;\
	float2 dy = 0;\
	STOCHASTIC_SAMPLING_NO_DEF_DD(texName, uv, stochasticOffset, res)
			
#define STOCHASTIC_SAMPLING_COMPLETE_NO_DEF_DD(texName, uv, stochasticOffsetName, res) \
	stochasticOffset = getStochasticOffsets(uv, ACCESS_PROP_FLOAT(_StochasticScale), ACCESS_PROP_FLOAT(_StochasticSkew));\
	STOCHASTIC_SAMPLING_NO_DEF_DD(texName, uv, stochasticOffsetName, res)
	
#define STOCHASTIC_SAMPLING_COMPLETE(texName, uv, stochasticOffset, res) \
	stochasticOffset = getStochasticOffsets(uv, ACCESS_PROP_FLOAT(_StochasticScale), ACCESS_PROP_FLOAT(_StochasticSkew));\
	STOCHASTIC_SAMPLING(texName, uv, stochasticOffsetName, res)


#if defined(PROCEDURAL_INSTANCING_ON)
	#define DECLARE_PROPERTY_FLOAT(name)	float name;
	#define DECLARE_PROPERTY_FLOAT2(name)	float2 name;
	#define DECLARE_PROPERTY_FLOAT3(name)	float3 name;
	#define DECLARE_PROPERTY_FLOAT4(name)	float4 name;
#else
	#if (defined(INSTANCING_ON) || defined(PROCEDURAL_INSTANCING_ON)) && !defined(URP_PASS)
		#define DECLARE_PROPERTY_FLOAT(name)	UNITY_DEFINE_INSTANCED_PROP(float, name)
		#define DECLARE_PROPERTY_FLOAT2(name)	UNITY_DEFINE_INSTANCED_PROP(float2, name)
		#define DECLARE_PROPERTY_FLOAT3(name)	UNITY_DEFINE_INSTANCED_PROP(float3, name)
		#define DECLARE_PROPERTY_FLOAT4(name)	UNITY_DEFINE_INSTANCED_PROP(float4, name)
	#else
		#define DECLARE_PROPERTY_FLOAT(name)	float name;
		#define DECLARE_PROPERTY_FLOAT2(name)	float2 name;
		#define DECLARE_PROPERTY_FLOAT3(name)	float3 name;
		#define DECLARE_PROPERTY_FLOAT4(name)	float4 name;
	#endif
#endif


//------ Shader features --------
#pragma shader_feature_local_fragment _ALLIN13D_SURFACE_TRANSPARENT
#if !defined(ALLIN1_SHADER_VARIANT)
	#include_with_pragmas "../ShaderLibrary/AllIn13DShader_ShaderFeatures.hlsl"
#endif
//------------------------------

#if defined(_SPECULARMODEL_CLASSIC) || defined(_SPECULARMODEL_TOON) || defined(_SPECULARMODEL_ANISOTROPIC) || defined(_SPECULARMODEL_ANISOTROPICTOON)
	#define SPECULAR_ON
#else
	#if !defined(_SPECULARMODEL_NONE)
		#define _SPECULARMODEL_NONE
	#endif
#endif

#if defined(_LIGHTMODEL_CLASSIC) || defined(_LIGHTMODEL_TOON) || defined(_LIGHTMODEL_TOONRAMP) || defined(_LIGHTMODEL_HALFLAMBERT) || defined(_LIGHTMODEL_FAKEGI) || defined(_LIGHTMODEL_FASTLIGHTING)
	#define LIGHT_ON
#else
	#if !defined(_LIGHTMODEL_NONE)
		#define _LIGHTMODEL_NONE
	#endif
#endif

#if (!defined(FORWARD_ADD_PASS) && defined(BIRP_PASS)) || defined(URP_PASS)
	#define ADDITIONAL_LIGHT_LOOP
#endif

//#pragma shader_feature_local _REFLECTIONS_NONE _REFLECTIONS_CLASSIC _REFLECTIONS_TOON

#if defined(_REFLECTIONS_CLASSIC) || defined(_REFLECTIONS_TOON)
	#define REFLECTIONS_ON
#else
	#if !defined(_REFLECTIONS_NONE)
		#define _REFLECTIONS_NONE
	#endif
#endif

#if !defined(_OUTLINETYPE_NONE)
	#define OUTLINE_ON
#endif

#if defined(_INTERSECTION_GLOW_ON) || defined(_SCREEN_SPACE_UV_ON) || defined(_INTERSECTION_FADE_ON) || defined(_DEPTH_COLORING_ON) || defined(_DITHER_ON)
	#define REQUIRE_SCENE_DEPTH
#endif

#if defined(_SCREEN_SPACE_UV_ON) || defined(REQUIRE_SCENE_DEPTH) || defined(_FADE_BY_CAM_DISTANCE_ON)
	#define REQUIRE_CAM_DISTANCE
#endif

#if defined(_SPECULARMODEL_ANISOTROPIC) || defined(_SPECULARMODEL_ANISOTROPICTOON) || defined(_NORMAL_MAP_ON)
	#define REQUIRE_TANGENT_WS
#endif

#if defined(URP_PASS)
	#if defined(LIGHTMAP_ON) && defined(LIGHTMAP_SHADOW_MIXING)
		#define SUBTRACTIVE_LIGHTING
	#endif
#else
	#if defined(LIGHTMAP_ON) && defined(SHADOWS_SCREEN) && defined(LIGHTMAP_SHADOW_MIXING) && !defined(SHADOWS_SHADOWMASK)
		#define SUBTRACTIVE_LIGHTING
	#endif
#endif

//Dependencies
#if !defined(LIGHT_ON)
	#undef REFLECTIONS_ON
	#undef _NORMAL_MAP_ON
	#undef SPECULAR_ON
#endif

#include "../ShaderLibrary/AllIn13DShader_GlobalPropertiesAndTexturesDeclaration.hlsl"


//-------- Properties Buffer -----------

BATCHING_BUFFER_START
	#include "../ShaderLibrary/AllIn13DShader_BufferPropertiesDeclaration.hlsl"
BATCHING_BUFFER_END


#ifdef UNITY_DOTS_INSTANCING_ENABLED
	#undef DECLARE_PROPERTY_FLOAT	
	#undef DECLARE_PROPERTY_FLOAT2
	#undef DECLARE_PROPERTY_FLOAT3
	#undef DECLARE_PROPERTY_FLOAT4
	#undef ALLIN13DSHADER_BUFFERPROPERTIESDECLARATION

	#define DECLARE_PROPERTY_FLOAT(name)	UNITY_DOTS_INSTANCED_PROP_OVERRIDE_SUPPORTED(float, name)
	#define DECLARE_PROPERTY_FLOAT2(name)	UNITY_DOTS_INSTANCED_PROP_OVERRIDE_SUPPORTED(float2, name)
	#define DECLARE_PROPERTY_FLOAT3(name)	UNITY_DOTS_INSTANCED_PROP_OVERRIDE_SUPPORTED(float3, name)
	#define DECLARE_PROPERTY_FLOAT4(name)	UNITY_DOTS_INSTANCED_PROP_OVERRIDE_SUPPORTED(float4, name) 

	UNITY_DOTS_INSTANCING_START(MaterialPropertyMetadata) 
	#include "../ShaderLibrary/AllIn13DShader_BufferPropertiesDeclaration.hlsl"
	UNITY_DOTS_INSTANCING_END(MaterialPropertyMetadata)
#endif

//----------------------------------

#if defined(UNITY_DOTS_INSTANCING_ENABLED)
	#define ACCESS_PROP_FLOAT(name)				UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, name)
	#define ACCESS_PROP_FLOAT2(name)			UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float2, name)
	#define ACCESS_PROP_FLOAT3(name)			UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float3, name)
	#define ACCESS_PROP_FLOAT4(name)			UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4, name)
	#define ACCESS_PROP_TILING_AND_OFFSET(name) UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_CUSTOM_DEFAULT(float4, name, float4(1, 1, 0, 0))

#elif defined(ALLIN13D_GPU_INSTANCING)
	#define ACCESS_PROP_FLOAT(name)		UNITY_ACCESS_INSTANCED_PROP(UnityPerMaterial, name)
	#define ACCESS_PROP_FLOAT2(name)	UNITY_ACCESS_INSTANCED_PROP(UnityPerMaterial, name)
	#define ACCESS_PROP_FLOAT3(name)	UNITY_ACCESS_INSTANCED_PROP(UnityPerMaterial, name)
	#define ACCESS_PROP_FLOAT4(name)	UNITY_ACCESS_INSTANCED_PROP(UnityPerMaterial, name)
	#define ACCESS_PROP_TILING_AND_OFFSET(name) UNITY_ACCESS_INSTANCED_PROP(UnityPerMaterial, name)

#else
	#define ACCESS_PROP_FLOAT(name)			name
	#define ACCESS_PROP_FLOAT2(name)		name
	#define ACCESS_PROP_FLOAT3(name)		name
	#define ACCESS_PROP_FLOAT4(name)		name
	#define ACCESS_PROP_TILING_AND_OFFSET(name)	name

#endif

#endif