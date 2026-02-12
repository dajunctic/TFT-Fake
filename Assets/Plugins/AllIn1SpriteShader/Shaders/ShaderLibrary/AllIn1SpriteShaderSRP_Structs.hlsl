#ifndef ALLIN1SPRITESHADERSRP_STRUCTS
#define ALLIN1SPRITESHADERSRP_STRUCTS

struct appdata
{
	float4 vertex : POSITION;
	float2 uv : TEXCOORD0;
	half4 color : COLOR;
	UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct v2f
{
	float2 uv : TEXCOORD0;
	float4 vertex : SV_POSITION;
	half4 color : COLOR;

#if OUTTEX_ON
	half2 uvOutTex : TEXCOORD1;
#endif

#if OUTDIST_ON
	half2 uvOutDistTex : TEXCOORD2;
#endif

#if DISTORT_ON
	half2 uvDistTex : TEXCOORD3;
#endif

#if FOG_ON
	UNITY_FOG_COORDS(4)
#endif
	UNITY_VERTEX_INPUT_INSTANCE_ID
	UNITY_VERTEX_OUTPUT_STEREO
};

#endif