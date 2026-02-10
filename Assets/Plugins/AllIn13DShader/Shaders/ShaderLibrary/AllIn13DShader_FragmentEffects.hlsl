#ifndef ALLIN13DSHADER_FRAGMENT_EFFECTS
#define ALLIN13DSHADER_FRAGMENT_EFFECTS

//<EffectsCode>
float3 HueShift(float3 inputColor, float hueShift, float hueSaturation, float hueBrightness)
{
	float3 res = inputColor;

	float cosHsv = hueBrightness * hueSaturation * cos(hueShift * 3.14159265 / 180);
	float sinHsv = hueBrightness * hueSaturation * sin(hueShift * 3.14159265 / 180);
	res.r = (.299 * hueBrightness + .701 * cosHsv + .168 * sinHsv) * inputColor.x
		+ (.587 * hueBrightness - .587 * cosHsv + .330 * sinHsv) * inputColor.y
		+ (.114 * hueBrightness - .114 * cosHsv - .497 * sinHsv) * inputColor.z;
	res.g = (.299 * hueBrightness - .299 * cosHsv - .328 * sinHsv) *inputColor.x
		+ (.587 * hueBrightness + .413 * cosHsv + .035 * sinHsv) * inputColor.y
		+ (.114 * hueBrightness - .114 * cosHsv + .292 * sinHsv) * inputColor.z;
	res.b = (.299 * hueBrightness - .3 * cosHsv + 1.25 * sinHsv) * inputColor.x
		+ (.587 * hueBrightness - .588 * cosHsv - 1.05 * sinHsv) * inputColor.y
		+ (.114 * hueBrightness + .886 * cosHsv - .203 * sinHsv) * inputColor.z;

	return res;
}

float3 ContrastBrightness(float3 inputColor, float contrast, float brightness)
{
	float3 res = max(0, (inputColor - float3(0.5, 0.5, 0.5)) * contrast + float3(0.5, 0.5, 0.5) + brightness);
	return res;
}


#if defined(_AFFECTED_BY_LIGHTMAPS_ON) && defined(_LIGHTMAP_COLOR_CORRECTION_ON)
float3 LightmapColorCorrection(float3 lightmap)
{
	float3 res = lightmap;

	res = HueShift(res, ACCESS_PROP_FLOAT(_HueShiftLM), ACCESS_PROP_FLOAT(_HueSaturationLM), ACCESS_PROP_FLOAT(_HueBrightnessLM));
	res = ContrastBrightness(res, ACCESS_PROP_FLOAT(_ContrastLM), ACCESS_PROP_FLOAT(_BrightnessLM));

	return res;
}
#endif
//</EffectsCode>

#ifdef _HUE_SHIFT_ON
float3 HueShift(float3 inputColor)
{
	float3 res = HueShift(inputColor, ACCESS_PROP_FLOAT(_HueShift), ACCESS_PROP_FLOAT(_HueSaturation), ACCESS_PROP_FLOAT(_HueBrightness));
	return res;
}
#endif


#ifdef _ALBEDO_VERTEX_COLOR_ON
float4 AlbedoVertexColor(float4 inputColor, EffectsData data)
{
	float4 res = inputColor;

#ifdef _ALBEDOVERTEXCOLORMODE_MULTIPLY
	float3 multipliedColor = res.rgb * data.vertexColor.rgb;
	res.rgb = lerp(res.rgb, multipliedColor, ACCESS_PROP_FLOAT(_VertexColorBlending));
#else
	res.rgb = lerp(inputColor.rgb, data.vertexColor.rgb, ACCESS_PROP_FLOAT(_VertexColorBlending));
#endif

	return res;
}
#endif

#if defined(_TEXTURE_BLENDING_ON)
float4 TextureBlending(float4 inputColor, EffectsData data)
{
	float4 res = inputColor;

	#if defined(_TEXTUREBLENDINGSOURCE_TEXTURE)
		float2 blendingMaskUV = SIMPLE_CUSTOM_TRANSFORM_TEX(data.rawMainUV, _TexBlendingMask);
		float3 maskColor = SAMPLE_TEX2D(_TexBlendingMask, blendingMaskUV).rgb;
		#if defined(_TEXTUREBLENDINGMODE_RGB)
			float maskG = smoothstep(ACCESS_PROP_FLOAT(_BlendingMaskCutoffG), ACCESS_PROP_FLOAT(_BlendingMaskCutoffG) + ACCESS_PROP_FLOAT(_BlendingMaskSmoothnessG), maskColor.g);
			float maskB = smoothstep(ACCESS_PROP_FLOAT(_BlendingMaskCutoffB), ACCESS_PROP_FLOAT(_BlendingMaskCutoffB) + ACCESS_PROP_FLOAT(_BlendingMaskSmoothnessB), maskColor.b);
			maskColor = float3(1.0, maskG, maskB);
		#else 
			float maskBlackAndWhite = smoothstep(ACCESS_PROP_FLOAT(_BlendingMaskCutoffWhite), ACCESS_PROP_FLOAT(_BlendingMaskCutoffWhite) + ACCESS_PROP_FLOAT(_BlendingMaskSmoothnessWhite), maskColor.r);
			maskColor = float3(maskBlackAndWhite, maskBlackAndWhite, maskBlackAndWhite);
		#endif
	#else
		float3 maskColor = data.vertexColor.rgb;
		float maskG = smoothstep(ACCESS_PROP_FLOAT(_BlendingMaskCutoffG), ACCESS_PROP_FLOAT(_BlendingMaskCutoffG) + ACCESS_PROP_FLOAT(_BlendingMaskSmoothnessG), maskColor.g);
		float maskB = smoothstep(ACCESS_PROP_FLOAT(_BlendingMaskCutoffB), ACCESS_PROP_FLOAT(_BlendingMaskCutoffB) + ACCESS_PROP_FLOAT(_BlendingMaskSmoothnessB), maskColor.b);
		
		maskColor.g = maskG;
		maskColor.b = maskB;
	#endif

	#ifdef _STOCHASTIC_SAMPLING_ON
		float2 dx = 0;
		float2 dy = 0;
			
		float stochasticScale	= ACCESS_PROP_FLOAT(_StochasticScale);
		float stochasticSkew	= ACCESS_PROP_FLOAT(_StochasticSkew);
	#endif
	
	#ifdef _TEXTUREBLENDINGMODE_RGB
		float2 texGUV = SIMPLE_CUSTOM_TRANSFORM_TEX(data.rawMainUV, _BlendingTextureG);
		float2 texBUV = SIMPLE_CUSTOM_TRANSFORM_TEX(data.rawMainUV, _BlendingTextureB);

		float4 texG = 0;
		float4 texB = 0;
		#ifdef _STOCHASTIC_SAMPLING_ON		
			float4x3 stochasticOffsets_G = getStochasticOffsets(texGUV, stochasticScale, stochasticSkew);
			STOCHASTIC_SAMPLING_NO_DEF_DD(_BlendingTextureG, texGUV, stochasticOffsets_G, texG)
		
			float4x3 stochasticOffsets_B = getStochasticOffsets(texBUV, stochasticScale, stochasticSkew);
			STOCHASTIC_SAMPLING_NO_DEF_DD(_BlendingTextureB, texGUV, stochasticOffsets_B, texB)
		#else
			texG = SAMPLE_TEX2D(_BlendingTextureG, texGUV);
			texB = SAMPLE_TEX2D(_BlendingTextureB, texBUV);
		#endif
	
		res = lerp(res, texG, maskColor.g);
		res = lerp(res, texB, maskColor.b);
	#else
		float2 texWhiteUV = SIMPLE_CUSTOM_TRANSFORM_TEX(data.rawMainUV, _BlendingTextureWhite);
		
		float4 texWhite = 0;
		#ifdef _STOCHASTIC_SAMPLING_ON
			float4x3 stochasticOffsets_W = getStochasticOffsets(texWhiteUV, stochasticScale, stochasticSkew);
			STOCHASTIC_SAMPLING_NO_DEF_DD(_BlendingTextureWhite, texWhiteUV, stochasticOffsets_W, texWhite)
		#else
			texWhite = SAMPLE_TEX2D(_BlendingTextureWhite, texWhiteUV);
		#endif
		
		res = lerp(res, texWhite, maskColor.r); 
	#endif

	return res;
}
#endif


#ifdef _HOLOGRAM_ON
float4 Hologram(float4 inputColor, EffectsData data)
{
	float4 res = inputColor;

	float3 dir = normalize(ACCESS_PROP_FLOAT3(_HologramLineDirection).xyz);
                
    // Calculate primary hologram pattern using direction projection
    float3 scrollPos1 = data.vertexWS * ACCESS_PROP_FLOAT(_HologramFrequency) + (data.shaderTime.y * ACCESS_PROP_FLOAT(_HologramScrollSpeed));
    float3 scrollUV1 = frac(scrollPos1);

	float projectedValue1 = dot(scrollUV1, normalize(dir));
    float distance1 = abs(projectedValue1 - ACCESS_PROP_FLOAT(_HologramLineCenter)) * ACCESS_PROP_FLOAT(_HologramLineSpacing);
    float gradientMask1 = 1 - distance1;
    gradientMask1 = pow(saturate(gradientMask1), ACCESS_PROP_FLOAT(_HologramLineSmoothness));
    gradientMask1 = max(gradientMask1, ACCESS_PROP_FLOAT(_HologramBaseAlpha));
	
	// Calculate accent line pattern using direction projection
    float3 scrollPos2 = data.vertexWS * ACCESS_PROP_FLOAT(_HologramAccentFrequency) + (data.shaderTime.y * ACCESS_PROP_FLOAT(_HologramAccentSpeed));
    float3 scrollUV2 = frac(scrollPos2);

	float projectedValue2 = dot(scrollUV2, normalize(dir));
    float distance2 = abs(projectedValue2 - ACCESS_PROP_FLOAT(_HologramLineCenter)) * ACCESS_PROP_FLOAT(_HologramLineSpacing);
    float gradientMask2 = 1 - distance2;
    gradientMask2 = pow(saturate(gradientMask2), ACCESS_PROP_FLOAT(_HologramLineSmoothness));

	// Combine both patterns
    float combinedMask = saturate(gradientMask1 * ACCESS_PROP_FLOAT(_HologramAlpha) + gradientMask2 * ACCESS_PROP_FLOAT(_HologramAccentAlpha));
                
    float4 finalColor = ACCESS_PROP_FLOAT4(_HologramColor);
    finalColor.a = combinedMask * ACCESS_PROP_FLOAT4(_HologramColor).a;

	res *= finalColor;

	return res;
}
#endif

#ifdef _MATCAP_ON

float3 Matcap(EffectsData data)
{
#ifdef _NORMAL_MAP_ON
	float3 normalMapOS = normalize(mul(UNITY_MATRIX_IT_MV, float4(data.normalWS, 0.0)).xyz);
	float3 normalVS = normalize(mul(UNITY_MATRIX_MV, float4(normalMapOS, 0.0)).xyz);
#else
	float3 normalVS = normalize(mul(UNITY_MATRIX_MV, float4(data.normalOS, 0.0)).xyz);
#endif

	float3 positionVSDir = normalize(GetPositionVS(data.vertexOS));

	float3 normalCrossPosition = cross(positionVSDir, normalVS);

	float u = (-normalCrossPosition.y * 0.5) + 0.5;
	float v = (normalCrossPosition.x * 0.5) + 0.5;

	float2 matcapUV = float2(u, v);
	float3 res = SAMPLE_TEX2D(_MatcapTex, matcapUV).rgb * ACCESS_PROP_FLOAT(_MatcapIntensity);
	return res;
}
#endif


#ifdef _COLOR_RAMP_ON
float4 ColorRamp(float4 inputColor, EffectsData data)
{
	float luminance = GetLuminance(inputColor);
	luminance = saturate(luminance + ACCESS_PROP_FLOAT(_ColorRampLuminosity));

	float2 rampUV = float2(luminance, 0);
	rampUV.x *= ACCESS_PROP_FLOAT(_ColorRampTiling);
	rampUV.x += frac(data.shaderTime.x * ACCESS_PROP_FLOAT(_ColorRampScrollSpeed));

	float3 colorRamp = SAMPLE_TEX2D(_ColorRampTex, rampUV).rgb;
	float3 resRGB = lerp(inputColor.rgb, colorRamp, ACCESS_PROP_FLOAT(_ColorRampBlend));

	float4 res = float4(resRGB, inputColor.a);

	return res;	
}
#endif

#ifdef _HIT_ON

float4 Hit(float4 inputColor)
{
	float3 resRGB = lerp(inputColor.rgb, ACCESS_PROP_FLOAT4(_HitColor).rgb * ACCESS_PROP_FLOAT(_HitGlow), ACCESS_PROP_FLOAT(_HitBlend));
	float4 res = float4(resRGB, inputColor.a);
	return res;
}

#endif
#ifdef _RIM_LIGHTING_ON
float3 Rim(float3 inputColorRGB, EffectsData data)
{
	float3 rimOffset = ACCESS_PROP_FLOAT3(_RimOffset);
	float rimIntensity = 0;
    
	UNITY_BRANCH
	if(dot(rimOffset, rimOffset) > 0.001f) //If we have an offset we calculate it
	{
		float3 viewSpaceOffset = mul((float3x3)UNITY_MATRIX_V, -rimOffset);
		float3 normalizedViewDir = normalize(data.viewDirWS);
		float3 biasedViewDir = normalize(normalizedViewDir + viewSpaceOffset);
		float NdotV = saturate(dot(data.normalWS, biasedViewDir));
		rimIntensity = smoothstep(ACCESS_PROP_FLOAT(_MinRim), ACCESS_PROP_FLOAT(_MaxRim), 1 - NdotV) * ACCESS_PROP_FLOAT(_RimAttenuation);
	}
	else //Otherwise we take quick path
	{
		float3 normalizedViewDir = normalize(data.viewDirWS);
		float NdotV = saturate(dot(data.normalWS, normalizedViewDir));
		rimIntensity = smoothstep(ACCESS_PROP_FLOAT(_MinRim), ACCESS_PROP_FLOAT(_MaxRim), 1 - NdotV) * ACCESS_PROP_FLOAT(_RimAttenuation);
	}
    
	return lerp(inputColorRGB, ACCESS_PROP_FLOAT4(_RimColor).rgb, rimIntensity);
}
#endif

#ifdef _GREYSCALE_ON
float3 Greyscale(float3 inputColorRGB)
{
	float3 res = inputColorRGB;
	float luminance = GetLuminance(res);
	luminance = saturate(luminance + ACCESS_PROP_FLOAT(_GreyscaleLuminosity));
	res = lerp(res, luminance * ACCESS_PROP_FLOAT4(_GreyscaleTintColor).rgb, ACCESS_PROP_FLOAT(_GreyscaleBlend));
	
	return res;
}
#endif

#ifdef _POSTERIZE_ON
float3 Posterize(float3 inputColorRGB)
{
	float3 res		= inputColorRGB;
	float gamma		= ACCESS_PROP_FLOAT(_PosterizeGamma);
	float numColors = ACCESS_PROP_FLOAT(_PosterizeNumColors);
	res.rgb = pow(res.rgb, gamma) * numColors;
	res.rgb = floor(res.rgb) / numColors;
	res.rgb = pow(res.rgb, 1.0 / gamma);
	
	return res;
}
#endif

#ifdef _HIGHLIGHTS_ON
float3 Highlights(float3 inputColorRGB, EffectsData data)
{
	float3 normalizedLightDir = normalize(data.lightDir);
	float3 offsetLightDir = normalize(normalizedLightDir + ACCESS_PROP_FLOAT3(_HighlightOffset));
	float3 normalizedViewDir = normalize(data.viewDirWS);

	float NdotL = saturate(dot(data.normalWS, offsetLightDir));
	float NdotV = saturate(dot(data.normalWS, normalizedViewDir));
	float rimFactor = 1.0 - NdotV;
    
	float highlightCutoff		= ACCESS_PROP_FLOAT(_HighlightCutoff);
	float highlightSmoothness	= ACCESS_PROP_FLOAT(_HighlightSmoothness);
	float highlightsIntensity	= smoothstep(highlightCutoff, highlightCutoff + highlightSmoothness, rimFactor);
    
	float3 highlights = highlightsIntensity * NdotL * ACCESS_PROP_FLOAT4(_HighlightsColor).rgb * ACCESS_PROP_FLOAT(_HighlightsStrength);

	return inputColorRGB * (highlights + 1);
}
#endif

#ifdef _CONTRAST_BRIGHTNESS_ON
float3 ContrastBrightness(float3 inputColorRGB)
{
	float3 res = ContrastBrightness(inputColorRGB, ACCESS_PROP_FLOAT(_Contrast), ACCESS_PROP_FLOAT(_Brightness));
	return res;
}
#endif

#ifdef _HEIGHT_GRADIENT_ON
float4 HeightGradient(float4 inputColor, EffectsData data)
{
	float4 res = inputColor;

	
	#ifdef _HEIGHTGRADIENTPOSITIONSPACE_WORLD
		float3 selectedPos = data.vertexWS;
	#else
		float3 selectedPos = data.vertexOS;
	#endif

	float gradient = RemapFloat(selectedPos.y, ACCESS_PROP_FLOAT(_MinGradientHeight), ACCESS_PROP_FLOAT(_MaxGradientHeight), 0.0, 1.0);
	gradient = saturate(gradient);

	float4 gradientColor = lerp(ACCESS_PROP_FLOAT4(_GradientHeightColor01), ACCESS_PROP_FLOAT4(_GradientHeightColor02), gradient);

	res *= gradientColor;

	return res;
}
#endif

#ifdef _INTERSECTION_GLOW_ON
float4 IntersectionGlow(float4 inputColor, EffectsData data)
{
	float4 res = inputColor;

	float depthGlowMask = saturate(ACCESS_PROP_FLOAT(_DepthGlowDist) * pow(max(0, 1 - data.sceneDepthDiff), ACCESS_PROP_FLOAT(_DepthGlowPower)));

	res.rgb = lerp(res.rgb, ACCESS_PROP_FLOAT(_DepthGlowGlobalIntensity) * res.rgb, depthGlowMask);
	res.rgb += ACCESS_PROP_FLOAT4(_DepthGlowColor).rgb * ACCESS_PROP_FLOAT(_DepthGlowColorIntensity) * depthGlowMask * res.a;

	return res;
}
#endif

#ifdef _DEPTH_COLORING_ON

float4 DepthColoring(float4 inputColor, EffectsData data)
{
	float4 res = inputColor;
	
	float eyeDepth = GetEyeDepth(data.vertexVS);
	float eyeDepth01 = smoothstep(global_MinDepth, global_MinDepth + global_DepthZoneLength, eyeDepth);
	eyeDepth01 = pow(eyeDepth01, global_DepthGradientFallOff);

#ifdef FORWARD_ADD_PASS
	res *= (1 - eyeDepth01);
#else
	float4 depthCol = SAMPLE_TEX2D(global_DepthGradient, float2(eyeDepth01, 0));
	res = lerp(res, depthCol, depthCol.a);
#endif

	

	return res;
}

#endif

#ifdef _SUBSURFACE_SCATTERING_ON
float4 SubsurfaceScattering(float4 inputColor, EffectsData data)
{
	float4 res = inputColor;

	float4 sssMapColor = SAMPLE_TEX2D(_SSSMap, data.mainUV);

	//Why do we need to add 1000 for this to look good?
	float3 lightColor = GetMainLightColorRGB() * 5;
	float lightIntensity = GetMainLightIntensity();

	float3 normalizedViewDir = normalize(data.viewDirWS);
	float3 scatterNormal = (data.normalWS * ACCESS_PROP_FLOAT(_NormalInfluence)) + data.lightDir;
	scatterNormal = -normalize(scatterNormal);
    
	float VdotScatterNormal = saturate(dot(scatterNormal, normalizedViewDir));

	float sssPower = max(1.0, ACCESS_PROP_FLOAT(_SSSPower));
	float backScatterFactor = pow(VdotScatterNormal, sssPower);

	float frontScatter = saturate(dot(data.normalWS, data.lightDir)); // How directly light hits surface
	float frontVdotN = 1.0 - saturate(dot(normalizedViewDir, data.normalWS)); // Fresnel-like front falloff
	frontVdotN = pow(frontVdotN, ACCESS_PROP_FLOAT(_SSSFrontPower));
	float frontScatterFactor = frontScatter * frontVdotN * ACCESS_PROP_FLOAT(_SSSFrontAtten);

	float scatterIntensity = max(backScatterFactor, frontScatterFactor) * ACCESS_PROP_FLOAT(_SSSAtten) * sssMapColor.r;
    
	// Simulate wavelength-dependent scattering with original scatter calculation
	float3 deepScatterColor = float3(1.0, 0.3, 0.2);
	float3 scatterColor = scatterIntensity * lightColor.rgb * lightIntensity * ACCESS_PROP_FLOAT4(_SSSColor).rgb;
	scatterColor *= lerp(float3(1,1,1), deepScatterColor, VdotScatterNormal);
    
	res.rgb += scatterColor * inputColor.rgb;
	return res;
}
#endif

#endif