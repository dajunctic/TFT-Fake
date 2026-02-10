#ifndef ALLIN13DSHADER_BRDF
#define ALLIN13DSHADER_BRDF

/************/
#define MEDIUMP_FLT_MAX    65504.0
#define saturateMediump(x) min(x, MEDIUMP_FLT_MAX)
#define MIN_ROUGHNESS 0.01

struct BDRFPerLightData
{
	float3 H;
	float3 L;

	float3 lightColor;
	float distanceAttenuation;
	float3 shadowColor;
	float3 correctedLightColor;

	float rawNdotL;
	float correctedRawNdotL;
	float NdotL;
	float TdotL;
	float BdotL;

	float NdotH;
	float TdotH;
	float BdotH;

	float VdotH;
	float LdotH;
	float LdotH_2;

	float LdotV;

	float3 F;
	float3 kS;
	float3 kD;
};

struct BDRFCommonData
{
	float3 N;
	float3 T;
	float3 B;
	float3 V;

	float NdotV;
	float TdotV;
	float BdotV;

	float metallic;
	float smoothness;
	float roughness;
	float roughness_2;
	float cubeLod;

	float3 F0;

	float2 mainUV;

	float3 positionWS;
	float2 normalizedScreenSpaceUV;
};

float D_GGX_Anisotropic(float NoH, const float3 h,
        const float3 t, const float3 b, float at, float ab) {
   
	//TODO: Pass TdotH and BdotH through parameters
	float ToH = dot(t, h);
    float BoH = dot(b, h);
    float a2 = at * ab;
    float3 v = float3(ab * ToH, at * BoH, a2 * NoH);
    float v2 = dot(v, v);
    float w2 = a2 / v2;
    return a2 * w2 * w2 * (1.0 / ALLIN13DSHADER_PI);
}

float V_SmithGGXCorrelated_Anisotropic(float at, float ab, float ToV, float BoV,
        float ToL, float BoL, float NoV, float NoL) {
    float lambdaV = NoL * length(float3(at * ToV, ab * BoV, NoV));
    float lambdaL = NoV * length(float3(at * ToL, ab * BoL, NoL));
    float v = 0.5 / (lambdaV + lambdaL);
    return saturateMediump(v);
}
/************/

float DistributionGGX(float a, float NdotH)
{
	float a2     = max(a*a, MIN_ROUGHNESS * MIN_ROUGHNESS);
	float NdotH2 = NdotH*NdotH;
	
	float num   = a2;
	float denom = (NdotH2 * (a2 - 1.0) + 1.0);
	denom = ALLIN13DSHADER_PI * denom * denom;
	
	return num / denom;
}

float GeometrySchlickGGX(float NdotV, float roughness)
{
	float r = (roughness + 1.0);
	float k = (r*r) / 8.0;

	float num   = NdotV;
	float denom = NdotV * (1.0 - k) + k;
	
	return num / denom;
}

float GeometrySmith(float NdotV, float NdotL, float roughness)
{
	float ggx2  = GeometrySchlickGGX(NdotV, roughness);
	float ggx1  = GeometrySchlickGGX(NdotL, roughness);

	return ggx1 * ggx2;
}

float3 fresnelSchlick(float3 F0, float VdotH)
{
	float OneMinusVdotH = 1 - VdotH;
	float OneMinusVdotH_5 = OneMinusVdotH * OneMinusVdotH * OneMinusVdotH * OneMinusVdotH * OneMinusVdotH;
	return F0 + (1.0 - F0) * OneMinusVdotH_5;
}

float3 fresnelSchlickRoughness(float cosTheta, float3 F0, float roughness)
{
	float oneMinusRoughness = 1.0 - roughness;
	float oneMinusCosTheta = 1.0 - cosTheta;

	return F0 + (max(oneMinusRoughness, F0) - F0) * pow(clamp(oneMinusCosTheta, 0.0, 1.0), 5.0);
}

inline float3 FresnelLerp (float3 F0, float3 F90, float cosA)
{
    float t = Pow_5 (1 - cosA);   // ala Schlick interpoliation
    return lerp (F0, F90, t);
}

//Cook Torrance
float3 SpecularTerm(float a, float roughness, float3 F, float NdotH, float NdotV, float NdotL, float VdotH)
{
	float D		= DistributionGGX(a, NdotH);
	float G		= GeometrySmith(NdotV, NdotL, roughness);
	float3 numerator = 4.0 * D * G * F;

	float denominator = 4 * NdotV * NdotL;
	denominator = max(denominator, 0.0001);

	float3 res = numerator / denominator;
	return res;
}


float3 SpecularAnisoTerm(
	float at, float ab, 
	float3 F, 
	float NdotH, float NdotV, float NdotL, 
	float TdotV, float BdotV, 
	float TdotL, float BdotL, 
	float TdotH, float BdotH, 
	float3 H, float3 T, float3 B)
{
	float D		= D_GGX_Anisotropic(NdotH, H, T, B, at, ab);
	float V		= V_SmithGGXCorrelated_Anisotropic(
					at, ab, 
					TdotV, BdotV, 
					TdotL, BdotL,
					NdotV, NdotL); 

	float3 res = D * V * F;

	return res;
}

float3 SpecularIBL(float3 positionWS, float2 normalizedScreenSpaceUV, float3 normalWS, float3 viewDirWS, float cubeLod)
{
	float3 res = 0;
	
#ifdef REFLECTIONS_ON
	res = GetSkyColor(positionWS, normalizedScreenSpaceUV, normalWS, viewDirWS, cubeLod);
#endif
	return res;
}

float3 DiffuseTerm(float LdotH, float LdotV, float NdotL, float roughness, float3 colorDiffuse)
{
	float LdotH_2 = LdotH * LdotH;

	float f0 = 1.0;
	float f90 = 0.5 + 2*(roughness * LdotH_2);

	float3 fDiffuse = lerp(f0, f90, NdotL) * lerp(f0, f90, LdotV);

	float3 res = (colorDiffuse / ALLIN13DSHADER_PI) * fDiffuse;
	return res;
}

float3 DiffuseTerm02(float NdotV, float NdotL, float LdotH, float perceptualRoughness, float3 albedo)
{
    float fd90 = 0.5 + 2 * LdotH * LdotH * perceptualRoughness;
    // Two schlick fresnel term
    float lightScatter   = (1 + (fd90 - 1) * Pow_5(1 - NdotL));
    float viewScatter    = (1 + (fd90 - 1) * Pow_5(1 - NdotV));

	float3 res = (albedo /*/ ALLIN13DSHADER_PI*/) * lightScatter * viewScatter;
	return res;
}

float3 IndirectLighting(float3 albedo, float3 specularColor, float3 lightmap, EffectsData effectsData, BDRFCommonData commonData)
{
	float3 N = commonData.N;
	float3 V = commonData.V;

	float3 ambientColor = GetAmbientColor(effectsData);
	float3 diffuse = (ambientColor + lightmap) * albedo;

	float3 F = fresnelSchlickRoughness(commonData.NdotV, commonData.F0, commonData.roughness);
	float3 kS = F;
	float3 specular = 0;
#ifdef REFLECTIONS_ON
	float3 specularIBL = SpecularIBL(commonData.positionWS, commonData.normalizedScreenSpaceUV, N, V, commonData.cubeLod);
	
	//float reflectionLuminance = GetLuminance(float4(specularIBL, 1.0));
	//specularIBL = lerp(diffuse, specularIBL, smoothstep(0.0, 0.1, reflectionLuminance * (1 - commonData.roughness)));
	
	//TODO: Intermediate _Metallic values looks weird
	float F_factor = 1 - commonData.metallic;
	specular = specularIBL * lerp(1.0, F, F_factor);
#else
	specular = lerp(diffuse * 0.25, diffuse, (1 - commonData.roughness));
#endif

	float3 kD = 1.0 - kS;
	kD *= 1.0 - commonData.metallic;

	float3 indirectDiffuse = kD * diffuse;
	
	//TODO: Some problems with specularColor
	float3 indirectSpecular = specular * specularColor;

	
	float3 res = indirectDiffuse + indirectSpecular;

	return res;
}


float3 DirectDiffuse_PBR(float3 albedo, BDRFCommonData commonData, BDRFPerLightData perLightData, AllIn1LightData lightData)
{	
	float3 diffuseTerm = DiffuseTerm02(commonData.NdotV, perLightData.NdotL, perLightData.LdotH, commonData.roughness, albedo);
	
	float correctedNdotL = saturate(perLightData.correctedRawNdotL);
	float3 directDiffuse = 0;
#if defined(_LIGHTMODEL_CLASSIC)
	directDiffuse = diffuseTerm * perLightData.kD * perLightData.correctedLightColor * correctedNdotL;
#elif defined(_LIGHTMODEL_HALFLAMBERT)
	float NdotL = saturate(perLightData.correctedRawNdotL);
	float halfLambertTerm = (NdotL * ACCESS_PROP_FLOAT(_HalfLambertWrap)) + (1 - ACCESS_PROP_FLOAT(_HalfLambertWrap));
	float halfLambertTerm_2 = halfLambertTerm * halfLambertTerm;
	directDiffuse = diffuseTerm * halfLambertTerm_2 * perLightData.correctedLightColor;
#elif defined(_LIGHTMODEL_FAKEGI)
	float fakeGI = (correctedNdotL * ACCESS_PROP_FLOAT(_HardnessFakeGI)) + 1.0 - ACCESS_PROP_FLOAT(_HardnessFakeGI);
	directDiffuse = diffuseTerm * fakeGI * perLightData.correctedLightColor;
#elif defined(_LIGHTMODEL_TOON)
	directDiffuse = diffuseTerm * perLightData.kD * perLightData.correctedLightColor * correctedNdotL;
	float toonFactor = smoothstep(ACCESS_PROP_FLOAT(_ToonCutoff), ACCESS_PROP_FLOAT(_ToonCutoff) + ACCESS_PROP_FLOAT(_ToonSmoothness), correctedNdotL);
	directDiffuse *= toonFactor;
#elif defined(_LIGHTMODEL_TOONRAMP)
	float3 rampLight = SAMPLE_TEX2D_LOD(_ToonRamp, float4((perLightData.correctedRawNdotL * 0.5) + 0.5, 0, 0, 0)).rgb;
	directDiffuse = diffuseTerm * perLightData.kD * perLightData.correctedLightColor * rampLight;
	//float diffuseTermLuminance = GetLuminance(directDiffuse);
	//directDiffuse = SAMPLE_TEX2D(_ToonRamp, float2(diffuseTermLuminance, diffuseTermLuminance)).rgb;
#endif
	
	return directDiffuse;
}

#ifdef SPECULAR_ON
float3 DirectSpecular_PBR(float3 specularColor, BDRFCommonData commonData, BDRFPerLightData perLightData, AllIn1LightData lightData, float4 specularMap)
{
	float3 specularTerm = 0;

	//float4 specularMap = SAMPLE_TEX2D(_SpecularMap, commonData.mainUV);

	#if defined(_SPECULARMODEL_CLASSIC)
		specularTerm = SpecularTerm(commonData.roughness_2, commonData.roughness, perLightData.F, perLightData.NdotH, commonData.NdotV, perLightData.NdotL, perLightData.VdotH);
	#elif defined(_SPECULARMODEL_TOON)
		specularTerm = SpecularTerm(commonData.roughness_2, commonData.roughness, perLightData.F, perLightData.NdotH, commonData.NdotV, perLightData.NdotL, perLightData.VdotH);
		specularTerm = smoothstep(ACCESS_PROP_FLOAT(_SpecularToonCutoff), ACCESS_PROP_FLOAT(_SpecularToonCutoff) + ACCESS_PROP_FLOAT(_SpecularToonSmoothness), specularTerm);
	#elif defined(_SPECULARMODEL_ANISOTROPIC) || defined(_SPECULARMODEL_ANISOTROPICTOON)
		//https://google.github.io/filament/Filament.html?utm_source=chatgpt.com#materialsystem/anisotropicmodel
		float aniso = ACCESS_PROP_FLOAT(_Anisotropy);
		float at = max((1 - ACCESS_PROP_FLOAT(_AnisoShininess)) * (1.0 + aniso), 0.001);
		float ab = max((1 - ACCESS_PROP_FLOAT(_AnisoShininess)) * (1.0 - aniso), 0.001);

		specularTerm = SpecularAnisoTerm(at, ab, perLightData.F, 
											perLightData.NdotH, commonData.NdotV, perLightData.NdotL, 
											commonData.TdotV, commonData.BdotV, perLightData.TdotL, 
											perLightData.BdotL, perLightData.TdotH, perLightData.BdotH, 
											perLightData.H, commonData.T, commonData.B);
		specularTerm = saturate(specularTerm);

		#if defined(_SPECULARMODEL_ANISOTROPICTOON)
			float specularSmoothness = max(ACCESS_PROP_FLOAT(_SpecularToonSmoothness), 0.001);
			specularTerm = smoothstep(ACCESS_PROP_FLOAT(_SpecularToonCutoff), ACCESS_PROP_FLOAT(_SpecularToonCutoff) + specularSmoothness, specularTerm);
		#endif
	#endif

	specularTerm *= specularMap.r;

	float3 directSpecular = specularTerm * specularColor * ACCESS_PROP_FLOAT(_SpecularAtten) * perLightData.correctedLightColor * perLightData.NdotL;
	return directSpecular;
}
#endif

float3 DirectLighting_PBR(float3 albedo, BDRFCommonData commonData, BDRFPerLightData perLightData, AllIn1LightData lightData, float4 specularMap)
{
	float3 diffuseTerm = DirectDiffuse_PBR(albedo, commonData, perLightData, lightData);

	float3 specularColor = lerp(1.0, albedo, commonData.metallic);

	float3 specularTerm = 0;
#ifdef SPECULAR_ON
	specularTerm = DirectSpecular_PBR(specularColor, commonData, perLightData, lightData, specularMap);
#endif

	float3 directLight = diffuseTerm + specularTerm;
	return directLight;
}

BDRFCommonData CreateCommonBDRFData(float3 albedo, EffectsData effectsData)
{
	BDRFCommonData res;

	res.N = effectsData.normalWS;
	res.T = effectsData.tangentWS;
	res.B = effectsData.bitangentWS;
	res.V =  normalize(_WorldSpaceCameraPos.xyz - effectsData.vertexWS);

	res.metallic = ACCESS_PROP_FLOAT(_Metallic);
	float smoothness = ACCESS_PROP_FLOAT(_Smoothness);
#ifdef _METALLIC_MAP_ON
	float4 metallicMapColor = SAMPLE_TEX2D(_MetallicMap, effectsData.mainUV);

	float metallicMapValue = metallicMapColor.r;
	res.metallic *= metallicMapValue;
	res.metallic = saturate(res.metallic);

	float glossMapValue = metallicMapColor.a;
	smoothness *= glossMapValue;
	smoothness = saturate(smoothness);
#endif

	res.smoothness = lerp(0, 0.95, smoothness);

	res.roughness = (1 - res.smoothness);
	res.roughness_2 = res.roughness * res.roughness;
	res.cubeLod = res.roughness * 8;

	res.NdotV = max(dot(res.N, res.V), 0.0);
	res.TdotV = max(dot(res.T, res.V), 0.0);
	res.BdotV = max(dot(res.B, res.V), 0.0);


	float3 f0 = 0.04;
	f0 = lerp(f0, albedo, res.metallic);

	res.F0 = f0;

	res.mainUV = effectsData.mainUV;

	res.positionWS = effectsData.vertexWS;
	res.normalizedScreenSpaceUV = effectsData.normalizedScreenSpaceUV;

	return res;
}

BDRFPerLightData CreatePerLightData(BDRFCommonData commonData, AllIn1LightData lightData, float isAdditionalLight)
{
	BDRFPerLightData res;

	float oneMinusMetallic = 1 - commonData.metallic;

	res.L = lightData.lightDir;
	
	res.lightColor = lightData.lightColor.rgb;
	res.distanceAttenuation = lightData.distanceAttenuation;
	res.shadowColor = lightData.shadowColor.rgb;
	res.correctedLightColor = lightData.lightColor.rgb * lightData.distanceAttenuation * lightData.shadowColor.rgb;

	res.H = normalize(commonData.V + res.L);

	res.NdotH = max(dot(commonData.N, res.H), 0.0);
	res.rawNdotL = dot(commonData.N, res.L);
	res.correctedRawNdotL = res.rawNdotL;
	#if !defined(FORWARD_ADD_PASS)
		#if defined(_LIGHTMODEL_HALFLAMBERT) || defined(_LIGHTMODEL_FAKEGI) || defined(_LIGHTMODEL_TOONRAMP)
			res.correctedLightColor = isAdditionalLight > 0 ? res.correctedLightColor : res.lightColor;
			res.correctedRawNdotL = isAdditionalLight > 0 ? res.correctedRawNdotL : res.correctedRawNdotL * GetLuminance(lightData.shadowColor.rgb);
		#endif
	#endif

	res.NdotL = max(res.rawNdotL, 0.0);
	res.TdotL = max(dot(commonData.T, res.L), 0.0);
	res.BdotL = max(dot(commonData.B, res.L), 0.0);

	res.VdotH = max(dot(commonData.V, res.H), 0.0);
	res.TdotH = max(dot(commonData.T, res.H), 0.0);
	res.BdotH = max(dot(commonData.B, res.H), 0.0);

	res.LdotV = max(dot(res.L, commonData.V), 0.0);
	res.LdotH = max(dot(res.L, res.H), 0.0);

	res.LdotH_2 = res.LdotH * res.LdotH;

	res.F = fresnelSchlick(commonData.F0, res.VdotH);
	res.kS = res.F;
	res.kD = (1.0 - res.kS) * oneMinusMetallic;

	return res;
}

float3 CalculateLighting_PBR(float3 albedo, float3 lightmap, float alpha, EffectsData effectsData)
{
	BDRFCommonData commonData = CreateCommonBDRFData(albedo, effectsData);

	float3 specularColor = lerp(1.0, albedo, commonData.metallic);

	AllIn1LightData mainLightData = GetMainLightData(effectsData.vertexWS, effectsData);


	float3 directLighting = albedo;
	float4 specularMap = float4(1, 1, 1, 1);


#ifdef ALLIN1_USE_LIGHT_LAYERS
	uint meshRenderingLayers = GetMeshRenderingLayer();
	if (IsMatchingLightLayer(mainLightData.layerMask, meshRenderingLayers))
	{ 
#endif

	BDRFPerLightData perLightData_mainLight = CreatePerLightData(commonData, mainLightData, 0.0);

#ifdef SPECULAR_ON
	specularMap = SAMPLE_TEX2D(_SpecularMap, commonData.mainUV);
#endif

	directLighting = DirectLighting_PBR(albedo, commonData, perLightData_mainLight, mainLightData, specularMap);

#ifdef ALLIN1_USE_LIGHT_LAYERS
	}
#endif

#if defined(ADDITIONAL_LIGHT_LOOP)
	uint numAdditionalLights = NUM_ADDITIONAL_LIGHTS;
	LIGHT_LOOP_BEGIN_ALLIN13D(numAdditionalLights, effectsData)
		AllIn1LightData additionalLightData = GetPointLightData(lightIndex, effectsData.vertexWS, effectsData.normalWS, effectsData);
	#ifdef ALLIN1_USE_LIGHT_LAYERS
		uint meshRenderingLayers = GetMeshRenderingLayer();
		if (IsMatchingLightLayer(additionalLightData.layerMask, meshRenderingLayers))
		{ 
	#endif
		BDRFPerLightData perLightData_additionalLight = CreatePerLightData(commonData, additionalLightData, 1.0);
		directLighting += DirectLighting_PBR(albedo, commonData, perLightData_additionalLight, additionalLightData, specularMap);

	#ifdef ALLIN1_USE_LIGHT_LAYERS
		}
	#endif

	LIGHT_LOOP_END_ALLIN13D
#endif
	
	float2 ssaoFactor = GetSSAO(effectsData.normalizedScreenSpaceUV.xy, alpha);
	
	float3 res = directLighting * ssaoFactor.x;

	
	
	//We add IndirectLighting only once
#ifndef FORWARD_ADD_PASS
	#if defined(_CUSTOM_SHADOW_COLOR_ON)
		float shadowT = saturate(mainLightData.realtimeShadow + 1.0 - global_shadowColor.a);
		res = lerp(global_shadowColor, res, shadowT);
	#endif

	float3 ao = 1.0;
	#ifdef _AOMAP_ON
		ao = GetAOMapTerm(effectsData.mainUV);
	#endif

	float3 indirectLighting = IndirectLighting(albedo, specularColor, lightmap, effectsData, commonData);
	res += (indirectLighting) * ao * ssaoFactor.y;
#endif

	return res;
}

#endif