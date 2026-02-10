#ifndef ALLIN13DSHADER_URP_METAPASS
#define ALLIN13DSHADER_URP_METAPASS

struct Attributes
{
    float4 positionOS   : POSITION;
    float3 normalOS     : NORMAL;
    float2 uv0          : TEXCOORD0;
    float2 uv1          : TEXCOORD1;
    float2 uv2          : TEXCOORD2;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct Varyings
{
    float4 positionCS   : SV_POSITION;
    float2 uv           : TEXCOORD0;
#ifdef EDITOR_VISUALIZATION
    float2 VizUV        : TEXCOORD1;
    float4 LightCoord   : TEXCOORD2;
#endif
};

//#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/UniversalMetaPass.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/MetaInput.hlsl"


Varyings AllIn1VertexMeta(Attributes input)
{
    Varyings output = (Varyings)0;
    output.positionCS = UnityMetaVertexPosition(input.positionOS.xyz, input.uv1, input.uv2);
    //output.uv = TRANSFORM_TEX(input.uv0, _BaseMap);
	output.uv = input.uv0;
#ifdef EDITOR_VISUALIZATION
    UnityEditorVizData(input.positionOS.xyz, input.uv0, input.uv1, input.uv2, output.VizUV, output.LightCoord);
#endif
    return output;
}

half4 AllIn1FragmentMeta(Varyings input) : SV_Target 
{
    float2 uv = input.uv;

    MetaInput metaInput;
	metaInput.Albedo = ACCESS_PROP_FLOAT4(_Color).rgb * SAMPLE_TEXTURE2D(_MainTex, sampler__MainTex, uv).rgb;

	metaInput.Emission = float3(0, 0, 0);
#ifdef _EMISSION_ON
	metaInput.Emission = SAMPLE_TEX2D(_EmissionMap, uv) * ACCESS_PROP_FLOAT4(_EmissionColor) * ACCESS_PROP_FLOAT(_EmissionSelfGlow);
#endif

#ifdef EDITOR_VISUALIZATION
    metaInput.VizUV = input.VizUV;
    metaInput.LightCoord = input.LightCoord;
#endif

	float4 res = UnityMetaFragment(metaInput);
	return res;
}

#endif //Directional Lightmaps