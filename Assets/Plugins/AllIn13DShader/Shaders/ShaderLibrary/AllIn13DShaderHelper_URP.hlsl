#ifndef ALLIN13DSHADER_HELPER_URP_INCLUDED
#define ALLIN13DSHADER_HELPER_URP_INCLUDED

#define NUM_ADDITIONAL_LIGHTS GetNumAdditionalLights();

#define OBJECT_TO_WORLD_MATRIX GetObjectToWorldMatrix()

#if defined(ALLIN1_DECALS_SUPPORT) && defined(_DBUFFER)
	#define ALLIN1_DECALS_READY_TO_USE
#endif

#ifdef REQUIRE_SCENE_DEPTH

	#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
	
	float GetRawDepth(float4 projPos)
	{
		float res = SampleSceneDepth(projPos.xy / projPos.w);
		return res;
	}

	float GetLinearDepth01(float4 projPos)
	{
		float rawDepth = GetRawDepth(projPos);
		float res = Linear01Depth (rawDepth, _ZBufferParams);

		return res;
	}
	
	float GetSceneDepthDiff(float4 projPos)
	{
		float rawDepth = GetRawDepth(projPos);
		float res = LinearEyeDepth(rawDepth, _ZBufferParams) - projPos.z;

		return res;
	}

#endif

float3 GetNormalWS(float3 normalOS)
{
	float3 normalWS = TransformObjectToWorldNormal(normalOS);
	return normalWS;
}

float3 GetViewDirWS(float3 vertexWS)
{
	float3 res = GetWorldSpaceViewDir(vertexWS);
	return res;
}

float3 GetPositionVS(float3 positionOS)
{
	float3 res = TransformWorldToView(positionOS);
	return res;
}

float3 GetPositionWS(float4 positionOS)
{
	return TransformObjectToWorld(positionOS.xyz);
}

float3 GetDirWS(float4 dirOS)
{
	return TransformObjectToWorldDir(dirOS.xyz);
}

float3 GetDirOSFloat3(float3 dirOS)
{
	return TransformObjectToWorldDir(dirOS.xyz);
}

float3 GetDirOS(float4 dirOS)
{
	return GetDirOSFloat3(dirOS.xyz);
}

float3 GetMainLightDir(float3 vertexWS)
{
#ifdef _LIGHTMODEL_FASTLIGHTING
	float3 res = global_lightDirection;
#else
	float3 res = GetMainLight().direction;
#endif
	
	return res;
}

float3 GetMainLightColorRGB()
{
#ifdef _LIGHTMODEL_FASTLIGHTING
	float3 res = global_lightColor.rgb;
#else
	float3 res = GetMainLight().color;
#endif

	return res;
}

float2 GetSSAO(float2 normalizedScreenSpaceUV, float alpha)
{
	AmbientOcclusionFactor aoFactorURP = GetScreenSpaceAmbientOcclusion(normalizedScreenSpaceUV);
		
#if defined(_ALLIN13D_SURFACE_TRANSPARENT)
	float2 res = float2(1, 1);
#else
	float2 res = float2(aoFactorURP.directAmbientOcclusion, aoFactorURP.indirectAmbientOcclusion);
#endif
	
	return res;
}

//normalWS needed for the equivalent method in BIRP
//effectsData needed for the equivalent method in BIRP
AllIn1LightData GetPointLightData(int index, float3 vertexWS, float3 normalWS, EffectsData effectsData)
{
	AllIn1LightData lightData;

	Light additionalLight = GetAdditionalLight(index, vertexWS);
	lightData.lightColor = additionalLight.color;
	lightData.lightDir = additionalLight.direction;

#ifdef _CLUSTER_LIGHT_LOOP 
	int lightIndex = index;
#else
	int lightIndex = GetPerObjectLightIndex(index);
#endif

#if defined(_LIGHT_COOKIES)
    float3 cookieColor = SampleAdditionalLightCookie(lightIndex, effectsData.vertexWS);
    lightData.lightColor *= cookieColor;
#endif


#ifdef _RECEIVE_SHADOWS_ON
	//lightData.realtimeShadow = AdditionalLightRealtimeShadow(perObjectIndex, vertexWS, additionalLight.direction);
	lightData.realtimeShadow = AdditionalLightShadow(lightIndex, vertexWS, additionalLight.direction, 1.0, 0);
#else
	lightData.realtimeShadow = 1.0;
#endif
	
	lightData.distanceAttenuation = additionalLight.distanceAttenuation;
	lightData.shadowColor = lightData.realtimeShadow;
	lightData.layerMask = additionalLight.layerMask;
	
	return lightData;
}

AllIn1LightData GetMainLightData(float3 vertexWS, EffectsData effectsData)
{
	AllIn1LightData lightData;
	
#ifdef _LIGHTMODEL_NONE
	lightData.lightColor = float3(1.0, 1.0, 1.0);
	lightData.lightDir = float3(0.0, 1.0, 0.0);
	lightData.distanceAttenuation = 1.0;
	lightData.shadowColor = 1.0;
	lightData.realtimeShadow = 1.0;
	lightData.layerMask = 1;
#elif _LIGHTMODEL_FASTLIGHTING
	lightData.lightColor = global_lightColor;
	lightData.lightDir = global_lightDirection;
	lightData.distanceAttenuation = 1.0;
	lightData.shadowColor = 1.0;
	lightData.realtimeShadow = 1.0;
	lightData.layerMask = 1;
#else
	Light mainLight = GetMainLight(); 
	lightData.lightColor = mainLight.color;
	lightData.lightDir = mainLight.direction;

	#if defined(_AFFECTED_BY_LIGHTMAPS_ON)
		lightData.distanceAttenuation = mainLight.distanceAttenuation;
	#else
		lightData.distanceAttenuation = 1.0;
	#endif
	
	float4 shadowCoords = TransformWorldToShadowCoord(vertexWS);
	#ifdef _RECEIVE_SHADOWS_ON
		lightData.realtimeShadow = MainLightRealtimeShadow(shadowCoords);
		#if defined(_RECEIVEDSHADOWSTYPE_STYLIZED)
			lightData.realtimeShadow = AntiAliasing(lightData.realtimeShadow, ACCESS_PROP_FLOAT(_ShadowCutoff));
		#endif
	#else
		lightData.realtimeShadow = 1.0;
	#endif

	lightData.shadowColor = lightData.realtimeShadow;
	lightData.layerMask = mainLight.layerMask;

	#if defined(_LIGHT_COOKIES)
        float3 cookieColor = SampleMainLightCookie(effectsData.vertexWS);
        lightData.lightColor *= cookieColor;
    #endif
#endif
	
	return lightData;
}

int GetNumAdditionalLights()
{
	return GetAdditionalLightsCount();
}

/*Needed for URP shadow caster pass*/
float3 _LightDirection;
float3 _LightPosition;
/************/
FragmentDataShadowCaster GetClipPosShadowCaster(VertexData v, FragmentDataShadowCaster input)
{
    float3 positionWS = TransformObjectToWorld(v.vertex.xyz);
    float3 normalWS = TransformObjectToWorldNormal(v.normal);

#if _CASTING_PUNCTUAL_LIGHT_SHADOW
    float3 lightDirectionWS = normalize(_LightPosition - positionWS);
#else
    float3 lightDirectionWS = _LightDirection;
#endif

    float4 positionCS = TransformWorldToHClip(ApplyShadowBias(positionWS, normalWS, lightDirectionWS));

#if UNITY_REVERSED_Z
    positionCS.z = min(positionCS.z, UNITY_NEAR_CLIP_VALUE);
#else
    positionCS.z = max(positionCS.z, UNITY_NEAR_CLIP_VALUE);
#endif

	input.pos = positionCS;
    return input;
}

ShadowCoordStruct GetShadowCoords(VertexData v, float4 clipPos, float3 vertexWS)
{
	ShadowCoordStruct res;

	res._ShadowCoord = 0;
	res.pos = clipPos;
	
	return res;
}

float GetShadowAttenuation(EffectsData data)
{
	float res = 1.0;

#if !defined(_LIGHTMODEL_FASTLIGHTING)

	float4 shadowCoords = TransformWorldToShadowCoord(data.vertexWS);

	float mainLightShadow = MainLightRealtimeShadow(shadowCoords);
	
	//int numAdditionalLights = NUM_ADDITIONAL_LIGHTS;
	//float additionalLightsShadows = 1.0;
	res = mainLightShadow;
	//for(int lightIndex = 0; lightIndex < numAdditionalLights; lightIndex++)
	//{
	//	Light additionalLight = GetAdditionalLight(lightIndex, vertexWS);
	//	float additionalLightShadow = AdditionalLightRealtimeShadow(lightIndex, vertexWS/*, additionalLight.direction*/) * additionalLight.distanceAttenuation;
		
	//	res *= additionalLightShadow;
	//}	
#endif

	return res;
}

//float GetShadowAttenuation(float3 vertexWS)
//{
//	float4 shadowCoords = TransformWorldToShadowCoord(vertexWS);
//	float mainLightShadow = MainLightRealtimeShadow(shadowCoords);

//	float res = mainLightShadow;
//	int numAdditionalLights = NUM_ADDITIONAL_LIGHTS;
//	for(int lightIndex = 0; lightIndex < numAdditionalLights; lightIndex++)
//	{
//		Light additionalLight = GetAdditionalLight(lightIndex, vertexWS);
//		float additionalLightShadow = AdditionalLightRealtimeShadow(lightIndex, vertexWS) * additionalLight.distanceAttenuation;
		
//		res += additionalLightShadow;
//	}
	
//	res = 1.0;
//	return res;
//}

float3 GetLightmap(float2 uvLightmap, EffectsData data)
{
	float3 res = 0.0;

#if defined(_AFFECTED_BY_LIGHTMAPS_ON) && defined(LIGHTMAP_ON)
	res = SampleLightmap(uvLightmap, data.normalWS);
	#ifdef SUBTRACTIVE_LIGHTING
		AllIn1LightData mainLight = GetMainLightData(data.vertexWS, data);
		float attenuation = mainLight.realtimeShadow;
		float ndotl = saturate(dot(data.normalWS, mainLight.lightDir));
		float3 shadowedLightEstimate =
				ndotl * (1 - attenuation) * mainLight.lightColor.rgb;
		float3 subtractedLight = res - shadowedLightEstimate;
		subtractedLight = max(subtractedLight, _SubtractiveShadowColor.xyz);
		subtractedLight = lerp(subtractedLight, res, attenuation);
	
		res = subtractedLight;
	#endif
#endif

	return res;
}

float3 GetAmbientColor(EffectsData data)
{
	float3 res = float3(0, 0, 0);
#if !defined(LIGHTMAP_ON)
	#if defined(_CUSTOM_AMBIENT_LIGHT_ON)
		res = ACCESS_PROP_FLOAT4(_CustomAmbientColor).rgb;
	#else
		#if defined(PROBE_VOLUMES_L1) || defined(PROBE_VOLUMES_L2)
			half3 bakedGI;
			float4 probeOcclusion;
			EvaluateAdaptiveProbeVolume(data.vertexWS.xyz, data.normalWS.xyz, data.viewDirWS.xyz,
									  data.projPos.xy, GetMeshRenderingLayer(), 
									  bakedGI, probeOcclusion);
			res = bakedGI;
		#else
			res = SampleSH(data.normalWS);
		#endif
	#endif

#endif

	return res;
}

float GetFogFactor(float4 clipPos)
{
	float res = 0;

#if defined(FOG_LINEAR) || defined(FOG_EXP) || defined(FOG_EXP2)
	res = ComputeFogFactor(clipPos.z);
#endif

	return res;
}

float4 CustomMixFog(float fogFactor, float4 col)
{
	float4 res = col;
	res.rgb = MixFog(res.rgb, fogFactor);
	return res;
}

void ConfigureDecalData(inout AllIn1DecalData allIn1Decal, float4 positionCS)
{
#if defined(_DBUFFER)
	FETCH_DBUFFER(DBuffer, _DBufferTexture, int2(positionCS.xy));

    DecalSurfaceData decalSurfaceData;
    DECODE_FROM_DBUFFER(DBuffer, decalSurfaceData);

	allIn1Decal.baseColor = decalSurfaceData.baseColor;
	allIn1Decal.mask = step(0.1, allIn1Decal.baseColor.a);
	allIn1Decal.mask = saturate(allIn1Decal.mask + decalSurfaceData.normalWS.w);

	float4 normalTS = decalSurfaceData.normalWS; //Even if it's called normalWS, the content is a sampled normal map in tangent space

	allIn1Decal.normalTS = lerp(normalTS, DEFAULT_NORMAL_MAP_VALUE, allIn1Decal.mask);
	allIn1Decal.unpackedNormal = UnpackNormal(allIn1Decal.normalTS);
#endif
}

#ifdef REFLECTIONS_ON

float3 BoxProjection (
	float3 direction, float3 position,
	float4 cubemapPosition, float3 boxMin, float3 boxMax
) {
	#if UNITY_SPECCUBE_BOX_PROJECTION
		UNITY_BRANCH
		if (cubemapPosition.w > 0) {
			float3 factors =
				((direction > 0 ? boxMax : boxMin) - position) / direction;
			float scalar = min(min(factors.x, factors.y), factors.z);
			direction = direction * scalar + (position - cubemapPosition.xyz);
		}
	#endif
	return direction;
}

float3 GetReflectionsSimple(float3 worldRefl, float cubeLod, float3 positionWS)
{
	half probe0Volume = CalculateProbeVolumeSqrMagnitude(unity_SpecCube0_BoxMin, unity_SpecCube0_BoxMax);
    half probe1Volume = CalculateProbeVolumeSqrMagnitude(unity_SpecCube1_BoxMin, unity_SpecCube1_BoxMax);

    half volumeDiff = probe0Volume - probe1Volume;
    float importanceSign = unity_SpecCube1_BoxMin.w;

	float desiredWeightProbe0 = CalculateProbeWeight(positionWS, unity_SpecCube0_BoxMin, unity_SpecCube0_BoxMax);
    float desiredWeightProbe1 = CalculateProbeWeight(positionWS, unity_SpecCube1_BoxMin, unity_SpecCube1_BoxMax);

	// A probe is dominant if its importance is higher
    // Or have equal importance but smaller volume
    bool probe0Dominant = importanceSign > 0.0f || (importanceSign == 0.0f && volumeDiff < -0.0001h);
    bool probe1Dominant = importanceSign < 0.0f || (importanceSign == 0.0f && volumeDiff > 0.0001h);

    float weightProbe0 = probe1Dominant ? min(desiredWeightProbe0, 1.0f - desiredWeightProbe1) : desiredWeightProbe0;
    float weightProbe1 = probe0Dominant ? min(desiredWeightProbe1, 1.0f - desiredWeightProbe0) : desiredWeightProbe1;

    float totalWeight = weightProbe0 + weightProbe1;

    // If either probe 0 or probe 1 is dominant the sum of weights is guaranteed to be 1.
    // If neither is dominant this is not guaranteed - only normalize weights if totalweight exceeds 1.
    weightProbe0 /= max(totalWeight, 1.0f);
    weightProbe1 /= max(totalWeight, 1.0f);

	float3 res = 0;
	if(weightProbe0 > 0.01)
	{
		float3 reflUV0 = BoxProjection(worldRefl, positionWS, unity_SpecCube0_ProbePosition, unity_SpecCube0_BoxMin.xyz, unity_SpecCube0_BoxMax.xyz);
		float4 probe0HDR = SAMPLE_TEXTURECUBE_LOD(unity_SpecCube0, samplerunity_SpecCube0, reflUV0, cubeLod);
		float3 probe0 = DecodeHDREnvironment(probe0HDR, unity_SpecCube0_HDR);

		res += weightProbe0 * probe0;
	}
	if(weightProbe1 > 0.01f) 
	{
		float3 reflUV1 = BoxProjection(worldRefl, positionWS, unity_SpecCube1_ProbePosition, unity_SpecCube1_BoxMin.xyz, unity_SpecCube1_BoxMax.xyz);
		float4 probe1HDR = SAMPLE_TEXTURECUBE_LOD(unity_SpecCube1, samplerunity_SpecCube0, reflUV1, cubeLod);
		float3 probe1 = DecodeHDREnvironment(probe1HDR, unity_SpecCube1_HDR);

		res += weightProbe1 * probe1;
	}
	if(totalWeight < 0.99)
	{
		float4 glossyEnvironmentHDR = SAMPLE_TEXTURECUBE_LOD(_GlossyEnvironmentCubeMap, sampler_GlossyEnvironmentCubeMap, worldRefl, cubeLod);
		float3 glossyEnvironment = DecodeHDREnvironment(glossyEnvironmentHDR, _GlossyEnvironmentCubeMap_HDR);

		res += (1.0f - totalWeight) * glossyEnvironment;
	}

	return res;
}

float3 GetReflectionsCluster(float3 positionWS, float2 normalizedScreenSpaceUV, float3 worldRefl, float cubeLod)
{
	float3 res = 0;
	float3 irradiance = float3(0, 0, 0);
	float mip = cubeLod;
#if USE_CLUSTER_LIGHT_LOOP && CLUSTER_HAS_REFLECTION_PROBES
	float totalWeight = 0.0f;
	uint probeIndex;
	ClusterIterator it = ClusterInit(normalizedScreenSpaceUV, positionWS, 1);
	[loop] while (ClusterNext(it, probeIndex) && totalWeight < 0.99f)
	{
		probeIndex -= URP_FP_PROBES_BEGIN;

		float weight = CalculateProbeWeight(positionWS, urp_ReflProbes_BoxMin[probeIndex], urp_ReflProbes_BoxMax[probeIndex]);
		weight = min(weight, 1.0f - totalWeight);

		half3 sampleVector = worldRefl;
	#ifdef _REFLECTION_PROBE_BOX_PROJECTION
			sampleVector = BoxProjectedCubemapDirection(worldRefl, positionWS, urp_ReflProbes_ProbePosition[probeIndex], urp_ReflProbes_BoxMin[probeIndex], urp_ReflProbes_BoxMax[probeIndex]);
	#endif // _REFLECTION_PROBE_BOX_PROJECTION

		uint maxMip = (uint)abs(urp_ReflProbes_ProbePosition[probeIndex].w) - 1;
		half probeMip = min(mip, maxMip);
		float2 uv = saturate(PackNormalOctQuadEncode(sampleVector) * 0.5 + 0.5);

		float mip0 = floor(probeMip);
		float mip1 = mip0 + 1;
		float mipBlend = probeMip - mip0;
		float4 scaleOffset0 = urp_ReflProbes_MipScaleOffset[probeIndex * 7 + (uint)mip0];
		float4 scaleOffset1 = urp_ReflProbes_MipScaleOffset[probeIndex * 7 + (uint)mip1];

		half3 irradiance0 = half4(SAMPLE_TEXTURE2D_LOD(urp_ReflProbes_Atlas, sampler_LinearClamp, uv * scaleOffset0.xy + scaleOffset0.zw, 0.0)).rgb;
		half3 irradiance1 = half4(SAMPLE_TEXTURE2D_LOD(urp_ReflProbes_Atlas, sampler_LinearClamp, uv * scaleOffset1.xy + scaleOffset1.zw, 0.0)).rgb;
		res += weight * lerp(irradiance0, irradiance1, mipBlend);
		totalWeight += weight;
	}
#else
	half probe0Volume = CalculateProbeVolumeSqrMagnitude(unity_SpecCube0_BoxMin, unity_SpecCube0_BoxMax);
    half probe1Volume = CalculateProbeVolumeSqrMagnitude(unity_SpecCube1_BoxMin, unity_SpecCube1_BoxMax);

    half volumeDiff = probe0Volume - probe1Volume;
    float importanceSign = unity_SpecCube1_BoxMin.w;

    // A probe is dominant if its importance is higher
    // Or have equal importance but smaller volume
    bool probe0Dominant = importanceSign > 0.0f || (importanceSign == 0.0f && volumeDiff < -0.0001h);
    bool probe1Dominant = importanceSign < 0.0f || (importanceSign == 0.0f && volumeDiff > 0.0001h);

    float desiredWeightProbe0 = CalculateProbeWeight(positionWS, unity_SpecCube0_BoxMin, unity_SpecCube0_BoxMax);
    float desiredWeightProbe1 = CalculateProbeWeight(positionWS, unity_SpecCube1_BoxMin, unity_SpecCube1_BoxMax);

    // Subject the probes weight if the other probe is dominant
    float weightProbe0 = probe1Dominant ? min(desiredWeightProbe0, 1.0f - desiredWeightProbe1) : desiredWeightProbe0;
    float weightProbe1 = probe0Dominant ? min(desiredWeightProbe1, 1.0f - desiredWeightProbe0) : desiredWeightProbe1;

    float totalWeight = weightProbe0 + weightProbe1;

    // If either probe 0 or probe 1 is dominant the sum of weights is guaranteed to be 1.
    // If neither is dominant this is not guaranteed - only normalize weights if totalweight exceeds 1.
    weightProbe0 /= max(totalWeight, 1.0f);
    weightProbe1 /= max(totalWeight, 1.0f);

    // Sample the first reflection probe
    if (weightProbe0 > 0.01f)
    {
        half3 reflectVector0 = worldRefl;
#ifdef _REFLECTION_PROBE_BOX_PROJECTION
        reflectVector0 = BoxProjectedCubemapDirection(worldRefl, positionWS, unity_SpecCube0_ProbePosition, unity_SpecCube0_BoxMin, unity_SpecCube0_BoxMax);
#endif // _REFLECTION_PROBE_BOX_PROJECTION

        half4 encodedIrradiance = half4(SAMPLE_TEXTURECUBE_LOD(unity_SpecCube0, samplerunity_SpecCube0, reflectVector0, mip));

        irradiance += weightProbe0 * DecodeHDREnvironment(encodedIrradiance, unity_SpecCube0_HDR);
    }

    // Sample the second reflection probe
    if (weightProbe1 > 0.01f)
    {
        half3 reflectVector1 = worldRefl;
#ifdef _REFLECTION_PROBE_BOX_PROJECTION
        worldRefl = BoxProjectedCubemapDirection(worldRefl, positionWS, unity_SpecCube1_ProbePosition, unity_SpecCube1_BoxMin, unity_SpecCube1_BoxMax);
#endif // _REFLECTION_PROBE_BOX_PROJECTION
        half4 encodedIrradiance = half4(SAMPLE_TEXTURECUBE_LOD(unity_SpecCube1, samplerunity_SpecCube1, worldRefl, mip));

        irradiance += weightProbe1 * DecodeHDREnvironment(encodedIrradiance, unity_SpecCube1_HDR);
    }

	res = irradiance;
#endif

    // Use any remaining weight to blend to environment reflection cube map
    if (totalWeight < 0.99f)
    {
        half4 encodedIrradiance = half4(SAMPLE_TEXTURECUBE_LOD(_GlossyEnvironmentCubeMap, sampler_GlossyEnvironmentCubeMap, worldRefl, mip));

        res += (1.0f - totalWeight) * DecodeHDREnvironment(encodedIrradiance, _GlossyEnvironmentCubeMap_HDR);
    }
	
	return res;
}

float3 GetSkyColor(float3 positionWS, float2 normalizedScreenSpaceUV, float3 normalWS, float3 viewDirWS, float cubeLod)
{
	float3 worldRefl = normalize(reflect(-viewDirWS, normalWS));
	float4 skyData = 0;
	float3 res = 0;

#ifdef _REFLECTION_PROBE_BLENDING
	#if USE_CLUSTER_LIGHT_LOOP && CLUSTER_HAS_REFLECTION_PROBES
		res = GetReflectionsCluster(positionWS, normalizedScreenSpaceUV, worldRefl, cubeLod);
	#else
		res = GetReflectionsSimple(worldRefl, cubeLod, positionWS);
	#endif
#else
	res = GetReflectionsSimple(worldRefl, cubeLod, positionWS);
#endif

#ifdef _REFLECTIONS_TOON
	float posterizationLevel = lerp(2, 20, ACCESS_PROP_FLOAT(_ToonFactor));
	res = floor(res * posterizationLevel) / posterizationLevel;
#endif

	res *= ACCESS_PROP_FLOAT(_ReflectionsAtten);

	return res;
}
#endif

float3 ShadeSH(float4 normalWS)
{
	float3 res = SampleSH(normalWS.xyz);
	return res;
}

#define OBJECT_TO_CLIP_SPACE(v) TransformObjectToHClip(v.vertex.xyz)
#define OBJECT_TO_CLIP_SPACE_FLOAT4(pos) TransformObjectToHClip(pos.xyz)

#endif