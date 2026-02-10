#ifndef ALLIN13DSHADER_ALPHA_EFFECTS
#define ALLIN13DSHADER_ALPHA_EFFECTS

#ifdef _FADE_ON

float4 Fade(float4 inputColor, float2 uv)
{
	float4 res = inputColor;

	float2 fadeUV = SIMPLE_CUSTOM_TRANSFORM_TEX(uv, _FadeTex);
	float fadeSample = SAMPLE_TEX2D(_FadeTex, fadeUV).r;
	
	fadeSample = pow(saturate(fadeSample), ACCESS_PROP_FLOAT(_FadePower));

#ifdef _FADE_BURN_ON
	float fadeAmount = lerp(ACCESS_PROP_FLOAT(_FadeAmount) - ACCESS_PROP_FLOAT(_FadeTransition) - ACCESS_PROP_FLOAT(_FadeBurnWidth), 1.0, ACCESS_PROP_FLOAT(_FadeAmount));
	float fade = smoothstep(fadeAmount, fadeAmount + ACCESS_PROP_FLOAT(_FadeTransition), fadeSample);

	float fadePlusBurn = smoothstep(fadeAmount + ACCESS_PROP_FLOAT(_FadeBurnWidth), fadeAmount + ACCESS_PROP_FLOAT(_FadeBurnWidth) + ACCESS_PROP_FLOAT(_FadeTransition), fadeSample);
	
	float diff = saturate(fade - fadePlusBurn);
	
	float3 burnColor = diff * ACCESS_PROP_FLOAT4(_FadeBurnColor).rgb;

	res.rgb += burnColor;
#else
	float fadeAmount = lerp(ACCESS_PROP_FLOAT(_FadeAmount) - ACCESS_PROP_FLOAT(_FadeTransition), 1.0, ACCESS_PROP_FLOAT(_FadeAmount));
	float fade = smoothstep(fadeAmount, fadeAmount + ACCESS_PROP_FLOAT(_FadeTransition), fadeSample);
#endif
	
	res.a *= fade;

	return res;
}

#endif

#ifdef _INTERSECTION_FADE_ON
float4 IntersectionFade(float4 inputColor, float sceneDepthDiff)
{
	float4 res = inputColor;

	res.a *= saturate(ACCESS_PROP_FLOAT(_IntersectionFadeFactor) * sceneDepthDiff);

	return res;
}
#endif

#ifdef _FADE_BY_CAM_DISTANCE_ON
float4 FadeByCamDistance(float4 inputColor, float camDistance, out float camFadeDistanceNormalized)
{
	float4 res = inputColor;

	float t = 0;
#ifdef _FADE_BY_CAM_DISTANCE_NEAR_FADE
	t = 1 - smoothstep(ACCESS_PROP_FLOAT(_MinDistanceToFade), ACCESS_PROP_FLOAT(_MaxDistanceToFade), camDistance);
#else
	t = smoothstep(ACCESS_PROP_FLOAT(_MinDistanceToFade), ACCESS_PROP_FLOAT(_MaxDistanceToFade), camDistance);
#endif

	
	res.a = lerp(res.a, 0, t);

	camFadeDistanceNormalized = t;

	return res;
}
#endif

#endif