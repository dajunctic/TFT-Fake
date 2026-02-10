#ifndef ALLIN13DSHADER_URP_DEPTHONLYPASS
#define ALLIN13DSHADER_URP_DEPTHONLYPASS

struct DepthOnlyVertexData
{
    float4 positionOS     : POSITION;
	float2 uv : TEXCOORD0;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct DepthOnlyFragmentData
{
    float4 positionCS   : SV_POSITION;
	float3 positionWS : TEXCOORD1;
	float4 mainUV	: TEXCOORD2;
	float4 interpolator_01 : TEXCOORD3;

    UNITY_VERTEX_INPUT_INSTANCE_ID
    UNITY_VERTEX_OUTPUT_STEREO
};

DepthOnlyFragmentData DepthOnlyVertex(DepthOnlyVertexData input)
{
	DepthOnlyFragmentData res;

	UNITY_SETUP_INSTANCE_ID(input);
	UNITY_TRANSFER_INSTANCE_ID(input, res);
	UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(res); 

	res.interpolator_01 = float4(0, 0, 0, 0);
	res.mainUV = float4(0, 0, 0, 0);
	SCALED_MAIN_UV(res) = CUSTOM_TRANSFORM_TEX(input.uv, UV_DIFF(res), _MainTex);

	res.positionWS = GetPositionWS(input.positionOS);
	res.positionCS = TransformObjectToHClip(input.positionOS.xyz);

	return res;
}

float DepthOnlyFragment(DepthOnlyFragmentData input) : SV_TARGET
{
	UNITY_SETUP_INSTANCE_ID(input);
    UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
	
	float4 screenPos = ComputeScreenPos(input.positionCS);
	float camDistance = distance(input.positionWS, _WorldSpaceCameraPos);
	float4 baseColor = SAMPLE_TEX2D(_MainTex, input.mainUV.xy);
	baseColor = ApplyAlphaEffects(baseColor, SCALED_MAIN_UV(input), 1.0, camDistance, screenPos);

	float alphaClip = saturate(baseColor.a);
	#ifdef _ALPHA_CUTOFF_ON
		clip((alphaClip - ACCESS_PROP_FLOAT(_AlphaCutoffValue)) - 0.01);
	#endif

	
	
	
	return input.positionCS.z;
}

#endif