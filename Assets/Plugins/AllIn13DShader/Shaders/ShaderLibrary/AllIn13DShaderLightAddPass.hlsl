#ifndef ALLIN13DSHADER_LIGHT_ADD_PASS_INCLUDED
#define ALLIN13DSHADER_LIGHT_ADD_PASS_INCLUDED

#include "../ShaderLibrary/AllIn13DShader_BasePass.hlsl"

float4 CalculateLightingAdd(float3 vertexWS, float3 normalWS, float3 viewDirWS, 
	float4 objectColor, float shadows, float2 mainUV, 
	FragmentData fragmentData, EffectsData effectsData)
{
	float4 col = float4(0, 0, 0, objectColor.a);
	col.rgb = CalculateLighting(vertexWS, normalWS, 0, 0, objectColor.rgb, objectColor.a,
		0, 0, 0, viewDirWS, mainUV, 0, 1.0, fragmentData, 1.0, effectsData);

	return col;
}

FragmentData BasicVertexAdd(VertexData v)
{
	FragmentData o;

	UNITY_SETUP_INSTANCE_ID(v);
    UNITY_TRANSFER_INSTANCE_ID(v, o);
	UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o); 
	
	v.vertex = ApplyVertexEffects(v.vertex, v.normal, 0);

	
	POSITION_WS(o) = GetPositionWS(v.vertex);

	o.normalWS = GetNormalWS(v.normal);

	VIEWDIR_WS(o) = GetViewDirWS(POSITION_WS(o));
	
	SCALED_MAIN_UV(o) = SIMPLE_CUSTOM_TRANSFORM_TEX(v.uv, _MainTex);
	RAW_MAIN_UV(o) = v.uv;
	
	o.pos = OBJECT_TO_CLIP_SPACE(v);

	ShadowCoordStruct shadowCoordStruct = GetShadowCoords(v, o.pos, POSITION_WS(o));
	FOGCOORD(o) = GetFogFactor(o.pos);

#ifdef REQUIRE_TANGENT_WS
	float3 tangentWS = GetDirWS(float4(v.tangent.xyz, 0));
	float3 bitangentWS = GetBitangentWS(v.tangent, tangentWS, o.normalWS);
	INIT_T_SPACE(o.normalWS)
#endif

	return o;
}

float4 BasicFragmentAdd(FragmentData i) : SV_Target
{
	UNITY_SETUP_INSTANCE_ID(i);
	UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);

#ifdef _LIGHTMODEL_FASTLIGHTING
	float4 additiveRes = 0;
#else

	AllIn1DecalData decalData;
	INIT_DECAL_DATA(decalData)

#ifdef ALLIN1_DECALS_READY_TO_USE
	ConfigureDecalData(decalData, i.pos);
#endif

	EffectsData data = CalculateEffectsData(i, decalData);

	data = ApplyUVEffects_FragmentStage(data);
	data.normalWS = GetNormalWS(data, i, decalData);

	float4 objectColor = GetBaseColor(data);

	float3 normalOS = data.normalOS;
	float3 normalWS = data.normalWS;
	float3 viewDirWS = data.viewDirWS;
	
	objectColor *= ACCESS_PROP_FLOAT4(_Color);
	objectColor = ApplyColorEffectsBeforeLighting(objectColor, data);

	float4 col = objectColor;

	col = CalculateLightingAdd(POSITION_WS(i), normalWS, VIEWDIR_WS(i), objectColor, 1.0, i.mainUV, i, data);
	
	col = ApplyAlphaEffects(col, i.mainUV, 0, data.camDistance, data.projPos);

#ifdef _ALPHA_CUTOFF_ON
	clip((col.a - ACCESS_PROP_FLOAT(_AlphaCutoffValue)) - 0.001);
#endif	

	col = ApplyColorEffectsAfterLighting(col, data);
	col.a *= ACCESS_PROP_FLOAT(_GeneralAlpha);

#ifdef _FOG_ON
	col = CustomMixFog(FOGCOORD(i), col); 
#endif

	float4 additiveRes = col * col.a;
#endif

	return additiveRes;
}

#endif