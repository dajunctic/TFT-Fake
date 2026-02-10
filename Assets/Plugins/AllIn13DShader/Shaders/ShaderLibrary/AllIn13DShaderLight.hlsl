#ifndef ALLIN13DSHADER_LIGHT_INCLUDED
#define ALLIN13DSHADER_LIGHT_INCLUDED

#if USE_FORWARD_PLUS
    #define LIGHT_LOOP_BEGIN_ALLIN13D(lightCount, data) { \
    uint lightIndex; \
    ClusterIterator _urp_internal_clusterIterator = ClusterInit(data.normalizedScreenSpaceUV, data.vertexWS, 0); \
    [loop] while (ClusterNext(_urp_internal_clusterIterator, lightIndex)) { \
        lightIndex += URP_FP_DIRECTIONAL_LIGHTS_COUNT; \
        FORWARD_PLUS_SUBTRACTIVE_LIGHT_CHECK
    #define LIGHT_LOOP_END_ALLIN13D } }
#else
    #define LIGHT_LOOP_BEGIN_ALLIN13D(lightCount, data) \
	if(lightCount > 0) { \
    for (uint lightIndex = 0u; lightIndex < lightCount; ++lightIndex) {
    #define LIGHT_LOOP_END_ALLIN13D } }
#endif

float GetMainLightIntensity()
{
	return length(GetMainLightColorRGB());
}

#if defined(_SHADINGMODEL_PBR) || defined(_SPECULARMODEL_ANISOTROPIC) || defined(_SPECULARMODEL_ANISOTROPICTOON)
	#include "../ShaderLibrary/AllIn13DShader_BRDF.hlsl"
#endif

float3 GetBitangentWS(float4 tangentOS, float3 tangentWS, float3 normalWS)
{
    float tangentSign = tangentOS.w * unity_WorldTransformParams.w;
    float3 res = cross(normalWS, tangentWS) * tangentSign;
    return res;
}

float3 DiffuseTerm(AllIn1LightData lightData, float3 normal, float isAdditionalLight)
{
	float rawNdotL = dot(normal, lightData.lightDir);
	float3 lightModel = 0;
	float3 lightColor = lightData.lightColor.rgb * lightData.distanceAttenuation * lightData.shadowColor.rgb;

#if !defined(FORWARD_ADD_PASS)
	#if defined(_LIGHTMODEL_HALFLAMBERT) || defined(_LIGHTMODEL_FAKEGI) || defined(_LIGHTMODEL_TOONRAMP)
		lightColor = isAdditionalLight > 0 ? lightColor : lightData.lightColor.rgb;
		rawNdotL = isAdditionalLight > 0 ? rawNdotL : rawNdotL * GetLuminance(lightData.shadowColor.rgb);
	#endif
#endif

#if defined(_LIGHTMODEL_CLASSIC)
	float NdotL = saturate(rawNdotL);
	lightModel = NdotL;

#elif defined(_LIGHTMODEL_TOON)
	float NdotL = saturate(rawNdotL);
	lightModel = smoothstep(ACCESS_PROP_FLOAT(_ToonCutoff), ACCESS_PROP_FLOAT(_ToonCutoff) + ACCESS_PROP_FLOAT(_ToonSmoothness), NdotL); 

#elif defined(_LIGHTMODEL_TOONRAMP)
	float NdotL = (rawNdotL * 0.5) + 0.5;
	lightModel = SAMPLE_TEX2D_LOD(_ToonRamp, float4(NdotL, 0, 0, 0)).rgb;

#elif defined(_LIGHTMODEL_HALFLAMBERT)
	float NdotL = saturate(rawNdotL);
	float halfLambertTerm = (NdotL * ACCESS_PROP_FLOAT(_HalfLambertWrap)) + (1 - ACCESS_PROP_FLOAT(_HalfLambertWrap));
	lightModel = halfLambertTerm * halfLambertTerm;

#elif defined(_LIGHTMODEL_FAKEGI)
	float NdotL = saturate(rawNdotL);
	lightModel = (NdotL * ACCESS_PROP_FLOAT(_HardnessFakeGI)) + 1.0 - ACCESS_PROP_FLOAT(_HardnessFakeGI);

#elif defined(_LIGHTMODEL_FASTLIGHTING)
	lightModel = saturate(rawNdotL);

#endif
	
	float3 res = lightModel * lightColor;

    return res;
}

#ifdef SPECULAR_ON

float3 SpecularTerm(float3 objectColor, AllIn1LightData lightData, 
		float3 normalWS, float3 tangentWS, float3 bitangentWS,
		float3 viewDirWS, float glossiness, float2 mainUV, float4 specularTex)
{
	float3 lightColor = lightData.lightColor.rgb * lightData.distanceAttenuation * lightData.shadowColor.rgb;
    float3 reflectionDir = reflect(-lightData.lightDir, normalWS);
    float rawVdotL = dot(viewDirWS, reflectionDir);
	
    float3 specularModel = 0;
	

#if defined(_SPECULARMODEL_CLASSIC) || defined(_SPECULARMODEL_TOON)
	float3 halfVector = normalize(viewDirWS + lightData.lightDir);
	float NdotH = saturate(dot(normalWS, halfVector));
	float specularIntensity = pow(NdotH, glossiness);
	
#elif defined(_SPECULARMODEL_ANISOTROPIC) || defined(_SPECULARMODEL_ANISOTROPICTOON)
	float3 H = normalize(viewDirWS + lightData.lightDir);

	float TdotH = saturate(dot(tangentWS, H));
	float BdotH = saturate(dot(bitangentWS, H));

	float TdotV = saturate(dot(tangentWS, viewDirWS));
	float BdotV = saturate(dot(bitangentWS, viewDirWS));

	float TdotL = saturate(dot(tangentWS, lightData.lightDir));
	float BdotL = saturate(dot(bitangentWS, lightData.lightDir));

	float NdotH = saturate(dot(normalWS, H));
	float NdotL = saturate(dot(normalWS, lightData.lightDir));
	float NdotV = saturate(dot(normalWS, viewDirWS));

	float VdotH = saturate(dot(viewDirWS, H));

	float anisotropy = ACCESS_PROP_FLOAT(_Anisotropy);
	float anisoShininess = ACCESS_PROP_FLOAT(_AnisoShininess);
	float at = max((1 - anisoShininess) * (1.0 + anisotropy), 0.001);
	float ab = max((1 - anisoShininess) * (1.0 - anisotropy), 0.001);

	float3 specularIntensity = SpecularAnisoTerm(
		at, ab, 
		float3(1.0, 1.0, 1.0), 
		NdotH, NdotV, NdotL,
		TdotV, BdotV, 
		TdotL, BdotL, 
		TdotH, BdotH,
		H, tangentWS.xyz, bitangentWS.xyz) * NdotL;

	specularIntensity = saturate(specularIntensity);
#endif

#if defined(_SPECULARMODEL_ANISOTROPICTOON) || defined(_SPECULARMODEL_TOON)
	float specularSmoothness = max(ACCESS_PROP_FLOAT(_SpecularToonSmoothness), 0.001);
	specularIntensity = smoothstep(ACCESS_PROP_FLOAT(_SpecularToonCutoff), ACCESS_PROP_FLOAT(_SpecularToonCutoff) + specularSmoothness, specularIntensity);
#endif

	specularModel = specularIntensity * lightColor;
	float3 res = specularModel * ACCESS_PROP_FLOAT(_SpecularAtten) * specularTex.r;
	
	return res;
}

float3 SpecularTerm_Basic(float3 objectColor, AllIn1LightData lightData, 
		float3 normalWS, float3 tangentWS, float3 bitangentWS, float3 viewDirWS, float2 mainUV, float4 specularTex)
{
	float3 res = SpecularTerm(objectColor, lightData,
		normalWS, tangentWS, bitangentWS,
		viewDirWS, ACCESS_PROP_FLOAT(_Shininess) * ACCESS_PROP_FLOAT(_Shininess), mainUV, specularTex);
	return res;
}

#endif

float3 RimTermOperation(float3 normalWS, float3 viewDirWS, float minRim, float maxRim, float rimAttenuation, float3 rimColor)
{
	float NdotV = saturate(dot(normalWS, viewDirWS));
	float rimIntensity = (1 - NdotV);
	rimIntensity = smoothstep(minRim, maxRim, rimIntensity) * rimAttenuation;

	float3 res = rimIntensity * rimColor;
	return res;
}

float3 DirectLighting(float3 objectColor, EffectsData effectsData, AllIn1LightData lightData, float4 specularTex, float isAdditionalLight)
{
	float3 res = 0;

	float3 diffuseTerm = DiffuseTerm(lightData, effectsData.normalWS, isAdditionalLight) * objectColor;
	
#if defined(_CUSTOM_SHADOW_COLOR_ON) && !defined(FORWARD_ADD_PASS)
	float shadowT = saturate(lightData.realtimeShadow + 1.0 - global_shadowColor.a);
	diffuseTerm = lerp(global_shadowColor, diffuseTerm, shadowT);
#endif
	
	float3 specularTerm = 0;
#ifdef SPECULAR_ON
	
	float NdotL = saturate(dot(effectsData.normalWS, lightData.lightDir));

    specularTerm = SpecularTerm_Basic(objectColor, lightData,
		effectsData.normalWS, effectsData.tangentWS, effectsData.bitangentWS, 
		effectsData.viewDirWS, effectsData.mainUV, specularTex) * NdotL;
#endif
	
	res = diffuseTerm + specularTerm;

	return res;
}

float3 DirectLighting(float3 objectColor, float2 ssaoFactor, EffectsData effectsData)
{
	AllIn1LightData mainLightData = GetMainLightData(effectsData.vertexWS, effectsData);


	float3 res = objectColor;
	float4 specularTex = float4(1, 1, 1, 1);

#ifdef ALLIN1_USE_LIGHT_LAYERS
	uint meshRenderingLayers = GetMeshRenderingLayer();
	if (IsMatchingLightLayer(mainLightData.layerMask, meshRenderingLayers))
	{ 
#endif

#ifdef SPECULAR_ON
	specularTex = SAMPLE_TEX2D(_SpecularMap, effectsData.mainUV);
#endif

	res = DirectLighting(objectColor, effectsData, mainLightData, specularTex, 0);

#ifdef ALLIN1_USE_LIGHT_LAYERS
		}
#endif

#if defined(ADDITIONAL_LIGHT_LOOP) && !defined(_LIGHTMODEL_FASTLIGHTING)
	uint numAdditionalLights = NUM_ADDITIONAL_LIGHTS;
	LIGHT_LOOP_BEGIN_ALLIN13D(numAdditionalLights, effectsData)
		AllIn1LightData additionalLightData = GetPointLightData(lightIndex, effectsData.vertexWS, effectsData.normalWS, effectsData);
	#ifdef ALLIN1_USE_LIGHT_LAYERS
		if (IsMatchingLightLayer(additionalLightData.layerMask, meshRenderingLayers))
		{
	#endif
		res += DirectLighting(objectColor, effectsData, additionalLightData, specularTex, 1);
	#ifdef ALLIN1_USE_LIGHT_LAYERS
		}
	#endif

	LIGHT_LOOP_END_ALLIN13D
#endif
	
	res *= ssaoFactor.x;

	return res;
}

float3 IndirectLighting_Basic(float3 objectColor, float3 lightmap, float2 ssaoFactor, EffectsData effectsData)
{
	float3 ambientColor = GetAmbientColor(effectsData);

	float3 ao = 1.0;

	float3 reflections = 0;
#ifdef REFLECTIONS_ON
	reflections = GetSkyColor(effectsData.vertexWS, effectsData.normalizedScreenSpaceUV, effectsData.normalWS, effectsData.viewDirWS, 1.0);
#endif
	
	float3 indirectLighting = (ambientColor + lightmap) * objectColor;
	indirectLighting += reflections;

	indirectLighting *= ssaoFactor.y;

	return indirectLighting;
}

float3 CalculateLighting_Basic(float3 objectColor, float3 lightmap, float alpha, EffectsData effectsData)
{
	float2 ssaoFactor = GetSSAO(effectsData.normalizedScreenSpaceUV.xy, alpha);

	float3 directLighting = DirectLighting(objectColor, ssaoFactor, effectsData);
	
	
	float3 indirectLighting = 0;	
	//We add IndirectLighting only once
#ifndef FORWARD_ADD_PASS
	indirectLighting = IndirectLighting_Basic(objectColor, lightmap, ssaoFactor, effectsData);
	
	#ifdef _AOMAP_ON
		float3 aoMapValue = GetAOMapTerm(effectsData.mainUV);
		indirectLighting *= aoMapValue;
	#endif
#endif

	float3 res = directLighting + indirectLighting;

	return res;
}

#ifdef _SHADINGMODEL_PBR
float3 CalculateLighting_MetallicWorkflow(
	float3 vertexWS, 
	float3 normalWS, float3 tangentWS, float3 bitangentWS, 
	float3 objectColor, float alpha,
	float3 shadows, float3 lightmap, float3 ambientCol, float3 viewDirWS, 
	float2 mainUV, FragmentData fragmentData, EffectsData effectsData)
{
	float3 res = CalculateLighting_PBR(objectColor, lightmap, alpha, effectsData);

	return res;
}

#endif

float3 CalculateLighting(float3 vertexWS, 
	float3 normalWS, float3 tangentWS, float3 bitangentWS, 
	float3 objectColor, float alpha,
	float shadows, float3 lightmap, float3 ambientCol, float3 viewDirWS, 
	float2 mainUV,
	float3 lightColor, float3 lightDir,
	FragmentData fragmentData, float lightAtten, EffectsData effectsData)
{
#ifdef _CUSTOM_SHADOW_COLOR_ON
	float3 shadowColor = lerp(1.0, global_shadowColor, 1 - shadows);
#else
	float3 shadowColor = shadows;
#endif
	
	float3 res = 0;
#ifdef _LIGHTMODEL_FASTLIGHTING
	res = CalculateLighting_Basic(objectColor, lightmap, alpha, effectsData);
#else
	#ifdef _SHADINGMODEL_PBR
		res = CalculateLighting_MetallicWorkflow(vertexWS, normalWS, tangentWS, 
			bitangentWS, objectColor, alpha, shadowColor, 
			lightmap, ambientCol, 
			viewDirWS, mainUV, 
			fragmentData, effectsData);
	#else
		res = CalculateLighting_Basic(objectColor, lightmap, alpha, effectsData);
	#endif
#endif
	
	return res;
}

#endif