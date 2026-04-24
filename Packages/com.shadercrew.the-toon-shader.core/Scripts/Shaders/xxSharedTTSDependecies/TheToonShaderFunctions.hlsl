#ifndef THETOONSHADER_FUNCTION
#define THETOONSHADER_FUNCTION








        































struct GeneralStylingData
{
    half enableDistanceFade;
    float distanceFadeStartDistance;
    float distanceFadeFalloff;
    half adjustDistanceFadeValue;
    float distanceFadeValue;
};


struct StylingData
{
    half isEnabled;
    half style;
    half type;
    float4 color;
    float rotation;
    float rotationBetweenCells;
    float density;
    float offset;
    float size;
    float sizeControl;
    float sizeFalloff;
    float roundness;
    float roundnessFalloff;
    float hardness;
    float opacity;
    float opacityFalloff;
};

struct StylingRandomData
{
    float enableRandomizer;
    float perlinNoiseSize;
    float perlinNoiseSeed;
    float whiteNoiseSeed;
    
    float noiseIntensity;
    
    half spacingRandomMode;
    float spacingRandomIntensity;

    half opacityRandomMode; 
    float opacityRandomIntensity;

    half lengthRandomMode;
    float lengthRandomIntensity;

    half hardnessRandomMode;
    float hardnessRandomIntensity;

    half thicknessRandomMode; 
    float thicknesshRandomIntensity;
    
   
   

};

struct AdditionalStylingSpecularData
{
    
};

struct AdditionalStylingRimData
{
    
};

struct PositionAndBlendingData
{
    half position;
    half blending;
    half isInverted;
};

struct UVSpaceData
{
    half drawSpace;
    half coordinateSystem;
    half polarCenterMode;
    float4 polarCenter;
    half sSCameraDistanceScaled;
    half anchorSSToObjectsOrigin;
};


struct NoiseSampleData
{
    float perlinNoise;
    float perlinNoiseFloored;
    float whiteNoise;
    float whiteNoiseFloored;
};

struct RequiredNoiseData
{
    bool perlinNoise;
    bool perlinNoiseFloored;
    bool whiteNoise;
    bool whiteNoiseFloored;
};


#define UNITY_TWO_PI        6.28318530718f

#ifdef _URP2D



struct Light2DData
{
    float4 lightMultiply;
    float4 lightAdditive;
    float4 lightMulitplyWithMask;
    float4 lightAdditveWithMask;
    float normalMapLighting;
};

#if _USE_OPTIMIZATION_DEFINES
    #define T2D_MAX_LIGHTS _MAX_LIGHT_COUNT
#else
    #define T2D_MAX_LIGHTS 12
#endif


int _T2D_LightCount;
float4 _T2D_LightPosRadius[T2D_MAX_LIGHTS];
float4 _T2D_LightColorInt[T2D_MAX_LIGHTS];
float _T2D_LightTargetSortingLayer[T2D_MAX_LIGHTS];
#endif
float shiftLinear(
float ll0, float lll0
)
{
    float llll0 = (ll0 - lll0) / max(lll0 + 1.0, 1e-6);
    float lllll0 = (ll0 - lll0) / max(1.0 - lll0, 1e-6);
    return lerp(llll0, lllll0, step(lll0, ll0)); 
}
float sum(
float3 lllllll0
)
{
   return dot(lllllll0, float3(1, 1, 1));
}
float invLerp(
float lllllllll0, float llllllllll0, float ll0
)
{
    return (ll0 - lllllllll0) / (llllllllll0 - lllllllll0);
}
float4 invLerp(
float4 lllllllll0, float4 llllllllll0, float4 ll0
)
{
    return (ll0 - lllllllll0) / (llllllllll0 - lllllllll0);
}
float remap(
float lllllllllllllllll0, float llllllllllllllllll0, float lllllllllllllllllll0, float llllllllllllllllllll0, float ll0
)
{
    float llllllllllllllllllllll0 = invLerp(lllllllllllllllll0, llllllllllllllllll0, ll0);
    return lerp(lllllllllllllllllll0, llllllllllllllllllll0, llllllllllllllllllllll0);
}
#if !_URP2D
float2 GetScreenUV(
float2 llllllllllllllllllllllll0, float lllllllllllllllllllllllll0
)
{
#if _URP
    float4 llllllllllllllllllllllllll0 = TransformObjectToHClip(float3(0, 0, 0));
#else
    float4 llllllllllllllllllllllllll0 = UnityObjectToClipPos(float3(0, 0, 0));
#endif
    float2 llllllllllllllllllllllllllll0 = float2(llllllllllllllllllllllll0.x, llllllllllllllllllllllll0.y);
    float lllllllllllllllllllllllllllll0 = _ScreenParams.y / _ScreenParams.x;
    llllllllllllllllllllllllllll0.x -= llllllllllllllllllllllllll0.x / (llllllllllllllllllllllllll0.w);
    llllllllllllllllllllllllllll0.y -= llllllllllllllllllllllllll0.y / (llllllllllllllllllllllllll0.w);
    llllllllllllllllllllllllllll0.y *= lllllllllllllllllllllllllllll0;
    llllllllllllllllllllllllllll0 *= 1 / lllllllllllllllllllllllll0;
    llllllllllllllllllllllllllll0 *= llllllllllllllllllllllllll0.z;
    return llllllllllllllllllllllllllll0;
};
#endif
float2 toPolar(
float2 lllllllllllllllllllllllllllllll0
)
{
    float l1 = length(lllllllllllllllllllllllllllllll0);
    float ll1 = atan2(lllllllllllllllllllllllllllllll0.y, lllllllllllllllllllllllllllllll0.x);
    return float2(ll1 / UNITY_TWO_PI, l1);
}
float2 ConvertToDrawSpace(
#ifndef _URP2D
#if _URP
    InputData inputData, 
#else
    float3 llll1,
    float3 lllll1,
#endif
#else
    float3 llll1,
    float3 lllll1,
    float2 llllllll1,
#endif
float2 lllllllll1, UVSpaceData uvSpaceData , float4 llllllllllllllllllllllllllll0
)
{
    #if _URP
        float3 llll1 = inputData.positionWS;
        float3 lllll1 = inputData.normalWS;
    #endif      
    if (uvSpaceData.drawSpace == 0)    
    {
    }
    else if (uvSpaceData.drawSpace == 1)    
    {            
        float4 llllllllllllllllllllllll0 = mul(UNITY_MATRIX_VP, float4(llll1, 1.0));
        float4 llllllllllllll1 = ComputeScreenPos(llllllllllllllllllllllll0);
        lllllllll1 = ((llllllllllllll1.xy) / llllllllllllll1.w); 
        if (uvSpaceData.anchorSSToObjectsOrigin)
        {
            float4 lllllllllllllll1 = mul(UNITY_MATRIX_VP, float4(_WorldSpaceCameraPos, 1.0));
            float2 llllllllllllllll1 = lllllllllllllll1.xy / lllllllllllllll1.w;
        #ifndef _URP2D
            float2 lllllllllllllllll1 = llllllllllllllllllllllllllll0.xy;
        #else
            float2 lllllllllllllllll1 = llllllll1;
        #endif
            lllllllll1 = lllllllll1 - lllllllllllllllll1; 
        }
    }
    else if (uvSpaceData.drawSpace == 2)    
    {
        float3 lllllllllllllllllll1 = abs(lllll1);
        if (lllllllllllllllllll1.x > lllllllllllllllllll1.y && lllllllllllllllllll1.x > lllllllllllllllllll1.z)
        {
            lllllllll1 = llll1.yz;
        }
        else if (lllllllllllllllllll1.y > lllllllllllllllllll1.z)
        {
            lllllllll1 = llll1.xz;
        }
        else
        {
            lllllllll1 = llll1.xy;
        }
    }
    if (uvSpaceData.coordinateSystem == 1) 
    {
        if (uvSpaceData.drawSpace == 1)
        {
            if (uvSpaceData.polarCenterMode == 0) 
            {
                lllllllll1.xy -= uvSpaceData.polarCenter.xy;
            }
            else 
            {
                uvSpaceData.polarCenter.a = 1;
                float4 llllllllllllllllllll1 = mul(UNITY_MATRIX_VP, uvSpaceData.polarCenter);
                float4 lllllllllllllllllllll1 = ComputeScreenPos(llllllllllllllllllll1);
                float2 llllllllllllllllllllll1 = lllllllllllllllllllll1.xy / lllllllllllllllllllll1.w;
                lllllllll1.xy -= llllllllllllllllllllll1;
            }
        }
        else
        {
            lllllllll1.xy -= uvSpaceData.polarCenter.xy;
        }
    }
    if (uvSpaceData.coordinateSystem == 1) 
    {
        lllllllll1 = toPolar(lllllllll1);
    }
    if (uvSpaceData.drawSpace == 1)
    {
        if (uvSpaceData.sSCameraDistanceScaled == 1)
        {
            float3 lllllllllllllllllllllll1 = mul(UNITY_MATRIX_M, float4(0, 0, 0, 1.0)).xyz;
            lllllllll1.xy *= distance(_WorldSpaceCameraPos, lllllllllllllllllllllll1);
        }
        float llllllllllllllllllllllll1 = _ScreenParams.x / _ScreenParams.y;
        lllllllll1.x *= llllllllllllllllllllllll1;
    }
    return lllllllll1;
}
float CalculateSpecularMaskSkipDot(
float llllllllllllllllllllllllll1, float3 lllllllllllllllllllllllllll1, float llllllllllllllllllllllllllll1, float lllllllllllllllllllllllllllll1, float llllllllllllllllllllllllllllll1
)
{
    float lllllllllllllllllllllllllllllll1 = 0;
    float l2 = (1 - (llllllllllllllllllllllllllll1)) * 10; 
    llllllllllllllllllllllllll1 = max(llllllllllllllllllllllllll1, 0); 
    float ll2 = pow(llllllllllllllllllllllllll1, l2 * l2);
    float lll2 = smoothstep(0.8, 0.8 + lllllllllllllllllllllllllllll1 / 1, ll2);
    lllllllllllllllllllllllllllllll1 = lll2 * llllllllllllllllllllllllllllll1 * 5;
    return lllllllllllllllllllllllllllllll1;
}
float CalculateSpecularMask(
float3 lllll2, float3 llllll2, float3 lllllllllllllllllllllllllll1, float llllllllllllllllllllllllllll1, float lllllllllllllllllllllllllllll1, float llllllllllllllllllllllllllllll1
)
{
    float lllllllllllllllllllllllllllllll1 = 0;
    float3 llllllllllll2 = normalize(llllll2 + lllllllllllllllllllllllllll1);
    float llllllllllllllllllllllllll1 = dot(lllll2, llllllllllll2);
    lllllllllllllllllllllllllllllll1 = CalculateSpecularMaskSkipDot(llllllllllllllllllllllllll1, lllllllllllllllllllllllllll1, llllllllllllllllllllllllllll1, lllllllllllllllllllllllllllll1, llllllllllllllllllllllllllllll1);
    return lllllllllllllllllllllllllllllll1;
}
#ifdef _URP2D
inline bool LightTargetsRenderer(
int lllllllllllllll2, int llllllllllllllll2
)
{
    uint mask = (uint)_T2D_LightTargetSortingLayer[lllllllllllllll2];
    uint bit  = 1u << (uint) llllllllllllllll2;
    return (mask & bit) != 0u;
}
void CalculateSpecularMaskFromMultipleLights2DGlobal( 
float3 lllllllllllllllll2, float llllllllllllllllllllllllllllll1, float3 lllll2, float3 lllllllllllllllllllllllllll1, float llllllllllllllllllllllllllll1, float lllllllllllllllllllllllllllll1, float lllllllllllllllllllllll2,
                                                inout float lllllllllllllllllllllllllllllll1, inout float3 lllllllllllllllllllllllll2
)
{
    lllllllllllllllllllllllll2 *= 0;
    lllllllllllllllllllllllllllllll1 = 0;
    for (int idx = 0; idx < _T2D_LightCount; idx++)
    {
        if (_T2D_LightPosRadius[idx].w > 0 && _T2D_LightColorInt[idx].w >0 && LightTargetsRenderer(idx,_SortingLayerIndex))
        {
            float3 llllllllllllllllllllllllll2 = _T2D_LightPosRadius[idx].xyz;
            float3 lllllllllllllllllllllllllll2 = _T2D_LightColorInt[idx].xyz;
            float llllllllllllllllllllllllllll2 = _T2D_LightPosRadius[idx].w;
            float lllllllllllllllllllllllllllll2 = _T2D_LightColorInt[idx].w;
            float llllllllllllllllllllllllllllll2 = distance(lllllllllllllllll2.xy,llllllllllllllllllllllllll2.xy);
            float3 lllllllllllllllllllllllllllllll2 = normalize(lllllllllllllllll2 - llllllllllllllllllllllllll2.xyz);
            float l3 = CalculateSpecularMask(lllll2, lllllllllllllllllllllllllllllll2, lllllllllllllllllllllllllll1, llllllllllllllllllllllllllll1, lllllllllllllllllllllllllllll1, llllllllllllllllllllllllllllll1);
            l3 *= saturate(llllllllllllllllllllllllllll2*0.9 - llllllllllllllllllllllllllllll2);
            lllllllllllllllllllllllll2 += lllllllllllllllllllllllllll2.rgb * l3; 
            lllllllllllllllllllllllllllllll1 += saturate(l3  * saturate(lllllllllllllllllllllllllllll2));
        }
    }
    lllllllllllllllllllllllllllllll1 *= lllllllllllllllllllllll2;
    lllllllllllllllllllllllllllllll1 *= llllllllllllllllllllllllllllll1;
}
#endif
float CalculateRimMask(
float3 lll3, float3 lllllllllllllllllllllllllll1, float lllll3, float llllll3, float llllllllllllllllllllllllllllll1,
                        half llllllll3, half lllllllll3, half llllllllll3, float lllllllllll3
)
{
    float llllllllllll3 = 0;         
    float lllllllllllll3 = saturate(1 - dot(lllllllllllllllllllllllllll1, lll3));
    lllll3 = 1 - lllll3;
    float llllllllllllll3 = smoothstep(saturate(lllll3 - llllll3), lllll3, lllllllllllll3);
    if ((llllllll3 == 0 && llllllllllllllllllllllllllllll1 > 0.0 && ((lllllllllll3 >= 0 || lllllllll3 == 0) || llllllllll3 == 0))
    || (llllllll3 == 1 && (llllllllllllllllllllllllllllll1 <= 0.0 || (lllllllllll3 <= 2 && lllllllll3 == 1)))
    || llllllll3 == 2 )
    {
        if (llllllll3 == 1)
        {
            #ifndef _URP2D
            float lllllllllllllll3 = llllllllllllllllllllllllllllll1;
            if (lllllllll3)
            {
                if (llllllllllllllllllllllllllllll1 > 0)
                {
                    llllllllllllllllllllllllllllll1 *= lllllllllll3;
                }
            }
            {
                float llllllllllllllll3 = 1 - abs(min(llllllllllllllllllllllllllllll1 * 2 , 0)); 
                if (lllllllllllllll3 > 0)
                {
                    llllllllllllllll3 = lllllllllll3;
                }
                llllllllllll3 = llllllllllllll3 * (1 - llllllllllllllll3);
            }
            #else
            llllllllllll3 = llllllllllllll3;
#endif
        }
        else if (llllllll3 == 0)
        {
            llllllllllll3 = llllllllllllll3 * (llllllllllllllllllllllllllllll1 * 2) * (lllllllllll3);
        }
        else if (llllllll3 == 2)
        {
            llllllllllll3 = llllllllllllll3;
        }
    }
    return llllllllllll3;
}
float CalculateRimMask2(
float3 lll3, float3 lllllllllllllllllllllllllll1, float lllll3, float llllll3, float llllllllllllllllllllllllllllll1,
                        half llllllll3, half lllllllll3, half llllllllll3, float lllllllllll3
)
{
    float llllllllllll3 = 0;        
    float lllllllllllll3 = saturate(1 - dot(lllllllllllllllllllllllllll1, lll3));
    lllll3 = 1 - lllll3;
    float llllllllllllll3 = smoothstep(saturate(lllll3 - llllll3), lllll3, lllllllllllll3);
    if ((llllllll3 == 0 && llllllllllllllllllllllllllllll1 > 0.0 && ((lllllllllll3 >= 0 || lllllllll3 == 0) || llllllllll3 == 0))
    || (llllllll3 == 1 && (llllllllllllllllllllllllllllll1 <= 0.0 || (lllllllllll3 <= 2 && lllllllll3 == 1)))
    || llllllll3 == 2)
    {
        if (llllllll3 == 1)
        {
            if (lllllllll3)
            {
                llllllllllll3 = llllllllllllll3 * (1 - lllllllllll3);
            }
            else
            {
                float llllllllllllllll3 = 1 - abs(min(llllllllllllllllllllllllllllll1 * 2, 0)); 
                float lllllll0 = lerp(0, llllllllllllllll3 * 4, llllll3);
                llllllllllll3 = llllllllllllll3 * (1 - llllllllllllllll3);
            }
        }
        else if (llllllll3 == 2)
        {
            llllllllllll3 = llllllllllllll3; 
        }
        else
        {
            llllllllllll3 = llllllllllllll3 * (llllllllllllllllllllllllllllll1 * 2) * (lllllllllll3);
        }
    }
    return llllllllllll3;
}
float2 RotateUV(
float2 lllllllll1, float ll1
)
{
    float llll4 = radians(ll1);
    float lllll4= cos(llll4);
    float llllll4= sin(llll4);
    float2 lllllll4;
    lllllll4.x = lllllllll1.x * lllll4 - lllllllll1.y * llllll4;
    lllllll4.y = lllllllll1.x * llllll4 + lllllllll1.y * lllll4;
    return lllllll4;
}
float2 RotateUVRadians(
float2 lllllllll1, float llllllllll4
)
{
    float llll4 = llllllllll4;                
    float lllll4 = cos(llll4);
    float llllll4 = sin(llll4);
    float2 lllllll4;
    lllllll4.x = lllllllll1.x * lllll4 - lllllllll1.y * llllll4;
    lllllll4.y = lllllllll1.x * llllll4 + lllllllll1.y * lllll4;
    return lllllll4;
}
NoiseSampleData SampleNoiseData(
float2 lllllllll1, StylingData stylingData, StylingRandomData stylingRandomData, RequiredNoiseData requiredNoiseData, 
#ifdef USE_UNITY_TEXTURE_2D_TYPE
    UnityTexture2D llllllllllllllll4, UnityTexture2D lllllllllllllllll4
#else
    sampler2D llllllllllllllll4, sampler2D lllllllllllllllll4
#endif
)
{
    NoiseSampleData noiseSampleData;
    if (stylingRandomData.enableRandomizer == 1)
    {
        if (stylingData.style == 1)
        {
            if (fmod(floor(lllllllll1.y * stylingData.density), 2) == 0)
            {
                lllllllll1.x += stylingData.offset / stylingData.density;
            }
        }
        float llllllllllllllllll4 = 0;
        if (requiredNoiseData.perlinNoiseFloored == 1)
        {
            float2 lllllllllllllllllll4 = lllllllll1;
            lllllllllllllllllll4.x = floor(lllllllll1.x * stylingData.density) / stylingData.density;
            if (stylingData.style == 0)
            {
            }
            else if (stylingData.style == 1)
            {
                lllllllllllllllllll4.y = floor(lllllllll1.y * stylingData.density) / stylingData.density;
            }
            lllllllllllllllllll4 *= stylingRandomData.perlinNoiseSize;
            llllllllllllllllll4 = tex2Dlod(llllllllllllllll4, float4(lllllllllllllllllll4, 0.0, 0.0)).x; 
        }
        float llllllllllllllllllll4 = 0;
        if (requiredNoiseData.perlinNoise == 1)
        {
            float2 lllllllllllllllllllll4 = lllllllll1 * stylingRandomData.perlinNoiseSize;
            llllllllllllllllllll4 = tex2Dlod(llllllllllllllll4, float4(lllllllllllllllllllll4, 0.0, 0.0)).x; 
        }
        float llllllllllllllllllllll4 = 0;
        if (requiredNoiseData.whiteNoise == 1)
        {
            float2 lllllllllllllllllllllll4 = lllllllll1;
            lllllllllllllllllllllll4.x = floor(lllllllll1.x * stylingData.density) / stylingData.density;
            if (stylingData.style == 0)
            {
                lllllllllllllllllllllll4.y = 0.1;
            }
            else
            if (stylingData.style == 1)
            {
                lllllllllllllllllllllll4.y = floor(lllllllll1.y * stylingData.density) / stylingData.density;
            }
            llllllllllllllllllllll4 = tex2Dlod(lllllllllllllllll4, float4(lllllllllllllllllllllll4, 0.0, 0.0)).x; 
        }
        float llllllllllllllllllllllll4 = 0;
        if (requiredNoiseData.whiteNoiseFloored == 1)
        {
            float2 lllllllllllllllllllllllll4 = lllllllll1;
            lllllllllllllllllllllllll4.x = floor(lllllllll1.x * stylingData.density) / stylingData.density;
            if (stylingData.style == 1)
            {
                lllllllllllllllllllllllll4.y = 0.1;
            }
            llllllllllllllllllllllll4 = tex2Dlod(lllllllllllllllll4, float4(lllllllllllllllllllllllll4, 0.0, 0.0)).x; 
        }
        noiseSampleData.perlinNoise = llllllllllllllllllll4;
        noiseSampleData.perlinNoiseFloored = llllllllllllllllll4;
        noiseSampleData.whiteNoise = llllllllllllllllllllll4;
        noiseSampleData.whiteNoiseFloored = llllllllllllllllllllllll4;
    }
    else
    {
        noiseSampleData.perlinNoise = 0;
        noiseSampleData.perlinNoiseFloored = 0;
        noiseSampleData.whiteNoise = 0;
        noiseSampleData.whiteNoiseFloored = 0;
    }
    return noiseSampleData;
}
float Hatching(
float ll0, float2 lllllllll1, StylingData hatchingData, StylingRandomData stylingRandomData, NoiseSampleData noiseSampleData, half lllllllllllllllllllllllllllll4
)
{
    ll0 = 1 - ll0;   
    float2 llllllllllllllllllllllllllllll4 = lllllllll1;      
    float lllllllllllllllllllllllllllllll4 = hatchingData.size / 2;    
    float l5 = llllllllllllllllllllllllllllll4.x;            
    l5 *= hatchingData.density;
    if (stylingRandomData.enableRandomizer == 1)
    {
        l5 += noiseSampleData.perlinNoise * stylingRandomData.noiseIntensity;
        float ll5 = 0;
        if (stylingRandomData.thicknessRandomMode == 0)
        {
            ll5 = noiseSampleData.whiteNoise;
        }
        else if (stylingRandomData.thicknessRandomMode == 1) 
        {
            ll5 = noiseSampleData.perlinNoiseFloored;
        }
        else 
        {
            ll5 = ((noiseSampleData.perlinNoiseFloored) + noiseSampleData.whiteNoise) / 2;
        }
        ll5 *= stylingRandomData.thicknesshRandomIntensity;
        float lll5 = remap(0, 1, 0.0, lllllllllllllllllllllllllllllll4, ll5);
        lllllllllllllllllllllllllllllll4 -= lll5;
        float llll5 = 0;
        if (stylingRandomData.spacingRandomMode == 0)
        {
            llll5 = noiseSampleData.whiteNoise;
        }
        else if (stylingRandomData.spacingRandomMode == 1) 
        {
            llll5 = noiseSampleData.perlinNoiseFloored;
        }
        else 
        {
            llll5 = ((noiseSampleData.perlinNoiseFloored) + noiseSampleData.whiteNoise) / 2;
        }
        float lllll5 = remap(0, 1, -0.5 + lllllllllllllllllllllllllllllll4, 0.5 - lllllllllllllllllllllllllllllll4, llll5);
        l5 += lllll5 * stylingRandomData.spacingRandomIntensity * saturate(1 - stylingRandomData.noiseIntensity); 
    }
    l5 = abs(frac(l5) - 0.5);
    float llllll5 = 0;
    if (stylingRandomData.enableRandomizer == 1)
    {
        float lllllll5 = 0;
        if (stylingRandomData.lengthRandomMode == 0)
        {
            lllllll5 = noiseSampleData.whiteNoise * saturate(1 - stylingRandomData.noiseIntensity); 
        }
        else if (stylingRandomData.lengthRandomMode == 1)
        {
            lllllll5 = noiseSampleData.perlinNoiseFloored; 
        }
        else
        {
            lllllll5 = ((noiseSampleData.perlinNoiseFloored + (noiseSampleData.whiteNoise * saturate(1 - stylingRandomData.noiseIntensity))) / 2); 
        }
        float llllllll5 = lllllll5 * stylingRandomData.lengthRandomIntensity;
        llllll5 = remap(0, 1 - llllllll5, 0, 1, ll0);    
    }
    else
    {
        llllll5 = remap(0, 1, 0, 1, ll0);;
    }    
    float lllllllll5 = smoothstep(min(1 - hatchingData.sizeFalloff, 0.99), 1, llllll5);
    lllllllll5 = max(lllllllllllllllllllllllllllllll4 - lllllllll5, 0);
    float llllllllll5 = 0;
    if (stylingRandomData.enableRandomizer == 1)
    {
        float lllllllllll5 = 0;
        if (stylingRandomData.hardnessRandomMode == 0) 
        {
            lllllllllll5 = noiseSampleData.whiteNoise;
        }
        else if (stylingRandomData.hardnessRandomMode == 1) 
        {
            lllllllllll5 = noiseSampleData.perlinNoiseFloored * 5;
        }
        else
        {
            lllllllllll5 = ((noiseSampleData.perlinNoiseFloored + noiseSampleData.whiteNoise) / 2) * 5;
        }
        llllllllll5 = remap(0, 1, 0, lllllllll5, min(saturate(hatchingData.hardness - lllllllllll5 * stylingRandomData.hardnessRandomIntensity), hatchingData.hardness));
    }
    else
    {
        llllllllll5 = remap(0, 1, 0, lllllllll5, hatchingData.hardness);
    }
    if (lllllllll5 != 0 )
    {
        float llllllllllll5 = 0;
        if (lllllllllllllllllllllllllllll4)
        {
            llllllllllll5 = fwidth(l5); 
        }
        if (lllllllll5 == lllllllllllllllllllllllllllllll4 && hatchingData.size == 1)
        {
            llllllllllll5 = 0;
        }                        
        if (llllllllll5 - llllllllllll5 < 0) 
        {
            llllllllllll5 = 0;
        }
        l5 = smoothstep(llllllllll5 - llllllllllll5, lllllllll5 + llllllllllll5, l5);
    }
    else
    {
        l5 = 1; 
    }
    l5 = 1 - l5;
    if (stylingRandomData.enableRandomizer == 1)
    {
        float lllllllllllll5;
        if (stylingRandomData.opacityRandomMode == 0) 
        {
            lllllllllllll5 = noiseSampleData.whiteNoise;
        }
        else if (stylingRandomData.opacityRandomMode == 1) 
        {
            lllllllllllll5 = noiseSampleData.perlinNoiseFloored * 5;
        }
        else 
        {
            lllllllllllll5 = ((noiseSampleData.perlinNoiseFloored * 5) + noiseSampleData.whiteNoise) / 2;
            lllllllllllll5 = ((noiseSampleData.perlinNoiseFloored + noiseSampleData.whiteNoise) / 2) * 5;
        }
        l5 = saturate(l5 - (lllllllllllll5 * stylingRandomData.opacityRandomIntensity));
    }
    float llllllllllllll5 = smoothstep(min(1-hatchingData.opacityFalloff, 0.99), 1, llllll5);
    l5 *= 1 - llllllllllllll5;
    l5 *= hatchingData.opacity;
    return l5;
}
float Halftones(
float ll0, float2 lllllllll1, StylingData halftonesData, StylingRandomData stylingRandomData, NoiseSampleData noiseSampleData
)
{            
    float2 llllllllllllllllll5 = lllllllll1;               
    llllllllllllllllll5 *= halftonesData.density;
    if (stylingRandomData.enableRandomizer == 1)
    {
        llllllllllllllllll5 += noiseSampleData.perlinNoise * stylingRandomData.noiseIntensity;
    }
    if (fmod(floor(llllllllllllllllll5.y), 2) == 0)
    {
        llllllllllllllllll5.x += halftonesData.offset;
    }
    if (stylingRandomData.enableRandomizer == 1)
    {
        float lllllll5 = 0;
        if (stylingRandomData.lengthRandomMode == 0)
        {
            lllllll5 = noiseSampleData.whiteNoiseFloored * saturate(1 - stylingRandomData.noiseIntensity); 
        }
        else if (stylingRandomData.lengthRandomMode == 1)
        {
            lllllll5 = noiseSampleData.perlinNoiseFloored; 
        }
        else
        {
            lllllll5 = ((noiseSampleData.perlinNoiseFloored + (noiseSampleData.whiteNoise * saturate(1 - stylingRandomData.noiseIntensity))) / 2); 
        }
        float llllllll5 = lllllll5 * stylingRandomData.lengthRandomIntensity;
        ll0 -= llllllll5;
    }
    float lllllllllllllllllllll5 = halftonesData.size;
    if (halftonesData.sizeControl == 1)  
    {
        lllllllllllllllllllll5 *= ll0;
    }
    else
    {
        float llllllllllllllllllllll5 = smoothstep(min(1 - halftonesData.sizeFalloff, 1), 1, (1 - ll0)); 
        lllllllllllllllllllll5 = max(lllllllllllllllllllll5 - llllllllllllllllllllll5, 0);
    }
    lllllllllllllllllllll5 /= 2;
    if (stylingRandomData.enableRandomizer == 1)
    {
        float ll5 = 0;
        if (stylingRandomData.thicknessRandomMode == 0)
        {
            ll5 = noiseSampleData.whiteNoise;
        }
        else if (stylingRandomData.thicknessRandomMode == 1) 
        {
            ll5 = noiseSampleData.perlinNoiseFloored;
        }
        else 
        {
            ll5 = ((noiseSampleData.perlinNoiseFloored) + noiseSampleData.whiteNoise) / 2;
        }
        float llllllllllllllllllllllll5 = remap(0, 1, 0.0, lllllllllllllllllllll5, ll5 * stylingRandomData.thicknesshRandomIntensity);
        lllllllllllllllllllll5 -= llllllllllllllllllllllll5;
    }
    float lllllllllllllllllllllllll5 = 1 - halftonesData.roundness;
    float llllllllllllllllllllllllll5 = smoothstep(halftonesData.roundnessFalloff, 1, 1 - ll0);
    lllllllllllllllllllllllll5 = max(lllllllllllllllllllllllll5 - llllllllllllllllllllllllll5 * 4, 0);
    lllllllllllllllllllllllll5 /= 2;
    if (stylingRandomData.enableRandomizer == 1)
    {
        float llll5 = 0;
        if (stylingRandomData.spacingRandomMode == 0)
        {
            llll5 = noiseSampleData.whiteNoise;
        }
        else if (stylingRandomData.spacingRandomMode == 1) 
        {
            llll5 = noiseSampleData.perlinNoiseFloored;
        }
        else 
        {
            llll5 = ((noiseSampleData.perlinNoiseFloored) + noiseSampleData.whiteNoise) / 2;
        }
        float lllll5 = remap(0, 1, -0.5 + lllllllllllllllllllll5, 0.5 - lllllllllllllllllllll5, llll5);
        llllllllllllllllll5 += lllll5 * stylingRandomData.spacingRandomIntensity * saturate(1 - stylingRandomData.noiseIntensity); 
    }
    float lllllllllllllllllllllllllllll5 = halftonesData.hardness;
    if (stylingRandomData.enableRandomizer == 1)
    {
        float lllllllllll5 = 0;
        if (stylingRandomData.hardnessRandomMode == 0) 
        {
            lllllllllll5 = noiseSampleData.whiteNoise;
        }
        else if (stylingRandomData.hardnessRandomMode == 1) 
        {
            lllllllllll5 = noiseSampleData.perlinNoiseFloored * 5;
        }
        else
        {
            lllllllllll5 = ((noiseSampleData.perlinNoiseFloored + noiseSampleData.whiteNoise) / 2) * 5;
        }
        lllllllllllllllllllllllllllll5 = min(saturate(halftonesData.hardness - lllllllllll5 * stylingRandomData.hardnessRandomIntensity), halftonesData.hardness);
    }
    float lllllllllllllllllllllllllllllll5 = remap(0, 1, 0, lllllllllllllllllllll5, lllllllllllllllllllllllllllll5);
    float l1 = length(max(abs(frac(llllllllllllllllll5) - 0.5) - lllllllllllllllllllllllll5 * lllllllllllllllllllllllllllllll5 * 2, 0.0)) + lllllllllllllllllllllllll5 * lllllllllllllllllllllllllllllll5 * 2;
    float ll6 = smoothstep(lllllllllllllllllllllllllllllll5, lllllllllllllllllllll5, l1);
    ll6 = 1 - ll6;
    if (stylingRandomData.enableRandomizer == 1)
    {
        float lllllllllllll5;
        if (stylingRandomData.opacityRandomMode == 0) 
        {
            lllllllllllll5 = noiseSampleData.whiteNoise;
        }
        else if (stylingRandomData.opacityRandomMode == 1) 
        {
            lllllllllllll5 = noiseSampleData.perlinNoiseFloored * 5;
        }
        else 
        {
            lllllllllllll5 = ((noiseSampleData.perlinNoiseFloored * 5) + noiseSampleData.whiteNoise) / 2;
            lllllllllllll5 = ((noiseSampleData.perlinNoiseFloored + noiseSampleData.whiteNoise) / 2) * 5;
        }
        ll6 = saturate(ll6 - (lllllllllllll5 * stylingRandomData.opacityRandomIntensity));
    }
    float llll6 = smoothstep(min(1-halftonesData.opacityFalloff, 0.99), 1, 1 - ll0);
    if (halftonesData.type == 1 || halftonesData.opacityFalloff != 0)
    {
        ll6 *= 1 - llll6;
    }
    ll6 *= halftonesData.opacity;
    ll6 = 1 - ll6;
    return ll6;
}
void DoBlending(
inout float4 lllll6, float ll0, float lllllll6, float4 llllllll6
)
{
    if (lllllll6 == 0) 
    {
        lllll6 = lerp(lllll6, llllllll6, ll0);
    }
    else if (lllllll6 == 1) 
    {        
        lllll6 += (llllllll6 * ll0);
    }
    else if (lllllll6 == 2) 
    {
        lllll6 *= 1-ll0 + (llllllll6 * ll0); 
    }
    else if (lllllll6 == 3) 
    {
        lllll6 -= (llllllll6 * ll0);
    }
    else if (lllllll6 == 4) 
    {
        lllll6 = lerp(lllll6, llllllll6, ll0);
    }
}
#ifdef _URP2D
float4 SpriteOutline(float2 lllllllll1, Texture2D lllllllllllllllllllllllllll6)
{
    float llllllllll6 = SAMPLE_TEXTURE2D(lllllllllllllllllllllllllll6, sampler_MainTex, lllllllll1).a;
    float lllllllllll6 = 0.1;
    {
        float2 offsets[8] =
        {
            float2(_OutlineWidth, 0.0),
            float2(-_OutlineWidth, 0.0),
            float2(0.0, _OutlineWidth),
            float2(0.0, -_OutlineWidth),
            float2(_OutlineWidth, _OutlineWidth),
            float2(-_OutlineWidth, _OutlineWidth),
            float2(_OutlineWidth, -_OutlineWidth),
            float2(-_OutlineWidth, -_OutlineWidth)
        };
        bool lllllllllllll6 = false;
        if (llllllllll6 > lllllllllll6)
        {
            [unroll(8)]
            for (int i = 0; i < 8; i++)
            {
                float llllllllllllll6 = SAMPLE_TEXTURE2D(lllllllllllllllllllllllllll6, sampler_MainTex, lllllllll1 + offsets[i]).a;
                if (llllllllllllll6 < lllllllllll6)
                    lllllllllllll6 = true;
            }
        }
        if (lllllllllllll6)
            return _OutlineColor;
        return float4(0, 0, 0, 0);
    }
}
#endif
void DoToonShading(
#ifndef _URP2D
#if _URP
    InputData inputData, 
    SurfaceData surface,
#else
#if _USESPECULAR || _USESPECULARWORKFLOW || _SPECULARFROMMETALLIC
                 SurfaceOutputStandardSpecular o,
#elif _BDRFLAMBERT || _BDRF3 || _SIMPLELIT
                 SurfaceOutput o,
#else
                 SurfaceOutputStandard o,
#endif
    UnityGI gi,
#if !_PASSFORWARDADD
    UnityGIInput giInput,
#endif
#endif
    ShaderData d,
#if _URP
    #if UNITY_VERSION >= 202120
    float3 lllllllllllllll6,
    #endif
#endif
#else 
    float3 lllllllllllllllll2,
    Light2DData light2DData,
    float3 lllllllllllllllll6,
    float2 llllllll1,
#endif
    inout float4 lllll6, 
#ifdef _URP2D
    int llllllllllllllllllll6,
#endif
    int lllllllllllllllllllll6, float llllllllllllllllllllll6, 
#ifndef _URP2D
    half lllllllllllllllllllllll6,
#endif
    half llllllllllllllllllllllll6,
    float2 lllllllll1, float4 llllllllllllllllllllllllllll0, 
#ifndef _URP2D
    sampler2D lllllllllllllllllllllllllll6,
#else
    Texture2D lllllllllllllllllllllllllll6,
#endif
    half llllllllllllllllllllllllllll6,
#ifndef _URP2D
    half lllllllllllllllllllllllllllll6, 
#endif
    half llllllllllllllllllllllllllllll6, half lllllllllllllllllllllllllllllll6,
#ifdef _URP2D
    half l7,
#endif
#ifdef USE_UNITY_TEXTURE_2D_TYPE
    UnityTexture2D ll7,
#else
    sampler2D ll7,
    float4 lll7,
#endif
#ifndef _URP2D
    half llll7, 
#endif
    half lllll7, float llllll7,
    half lllllll7, float4 llllllll7,
#ifndef _URP2D
    float lllllllll7, float llllllllll7, float lllllllllll7, float4 llllllllllll7,
    float lllllllllllll7, float llllllllllllll7, float lllllllllllllll7, half llllllllllllllll7, float4 lllllllllllllllll7,
#endif
    half llllllllllllllllll7,
    half lllllllllllllllllll7, half llllllllllllllllllll7, float4 lllllllllllllllllllll7, float llllllllllllllllllllll7, float lllllllllllllllllllllll7, float llllllllllllllllllllllll7, half lllllllllllllllllllllllll7, half llllllllllllllllllllllllll7,
    half lllllllllllllllllllllllllll7, half llllllllllllllllllllllllllll7, float4 lllllllllllllllllllllllllllll7, float llllllllllllllllllllllllllllll7, float lllllllllllllllllllllllllllllll7, float l8, half ll8, half lll8,
    half llll8, 
#ifndef _URP2D
    GeneralStylingData generalStylingData,
#endif
    half lllll8, half lllllllllllllllllllllllllllll4,
    half lllllll8,
    half llllllll8,
    float lllllllll8, float llllllllll8,
#ifndef _URP2D
    half lllllllllll8,
#endif    
    half llllllllllll8,
    PositionAndBlendingData positionAndBlendingDataShading, UVSpaceData uvSpaceDataShading, StylingData stylingDataShading, StylingRandomData stylingRandomDataShading,
#ifndef _URP2D
    half lllllllllllll8, 
    half llllllllllllll8,
    half lllllllllllllll8, float llllllllllllllll8,
    PositionAndBlendingData positionAndBlendingDataCastShadows, UVSpaceData uvSpaceDataCastShadows, StylingData stylingDataCastShadows, StylingRandomData stylingRandomDataCastShadows,
#endif
    half lllllllllllllllll8,
    half llllllllllllllllll8, float lllllllllllllllllll8, float llllllllllllllllllll8, half lllllllllllllllllllll8, half llllllllllllllllllllll8,
    half lllllllllllllllllllllll8,
    PositionAndBlendingData positionAndBlendingDataSpecular, UVSpaceData uvSpaceDataSpecular, StylingData stylingDataSpecular, StylingRandomData stylingRandomDataSpecular,
    half llllllllllllllllllllllll8, 
    half lllllllllllllllllllllllll8, float llllllllllllllllllllllllll8, float lllllllllllllllllllllllllll8, half llllllllllllllllllllllllllll8,
    half lllllllllllllllllllllllllllll8,
    PositionAndBlendingData positionAndBlendingDataRim, UVSpaceData uvSpaceDataRim, StylingData stylingDataRim, StylingRandomData stylingRandomDataRim,
#ifdef USE_UNITY_TEXTURE_2D_TYPE
    UnityTexture2D llllllllllllllll4, UnityTexture2D lllllllllllllllll4, 
#else
    sampler2D llllllllllllllll4, sampler2D lllllllllllllllll4,
    float4 l9,
#endif
    float3 ll9
)
{
#ifdef _URP2D
    float4 lll9 = light2DData.lightMultiply + light2DData.lightAdditive;
#endif
#ifndef _URP2D
    float4 llll9 = float4(0, 0, 0, 0);
#ifdef USE_UNITY_TEXTURE_2D_TYPE
    llll9 = ll7.texelSize;
#else
    llll9 = lll7;
#endif
#endif      
    #if _URP
        AlphaDiscard(surface.alpha, 0.5);
    #else
    #endif
    float lllll9 = 0;
    float4 llllll9 = lllll6;
    int lllllll9 = lllllllllllllllllllll6;
#if _USE_OPTIMIZATION_DEFINES
#if _ENABLE_TOON_SHADING
    llllllllllllllllllllllllllllll6 = 1;
#else
    llllllllllllllllllllllllllllll6 = 0;
#endif
#ifndef _URP2D
    #if _SHADING_COLOR
    llllllllllllllllllllllllllll6 = 0;
    #else
    llllllllllllllllllllllllllll6 = 1;
    #endif 
#else
    llllllllllllllllllllllllllll6 = 0;
#endif
#if _ENABLE_STYLING
    llll8 = 1;
#else
    llll8 = 0;
#endif
#if _ENABLE_SHADING_STYLING
    lllllll8 = 1;
#else
    lllllll8 = 0;
#endif
#ifndef _URP2D
    #if _ENABLE_CASTSHADOWS_STYLING
        lllllllllllll8 = 1;
    #else
        lllllllllllll8 = 0;
    #endif
    #ifdef _STYLING_CASTSHADOWS_SYNC_WITH_OTHER_STYLING
        llllllllllllll8 = _STYLING_CASTSHADOWS_SYNC_WITH_OTHER_STYLING;
    #endif  
    #if _SHADING_TERMINATORPOSITION
        lllllllll7 = lllllllll7;
    #else
        lllllllll7 = 0;
    #endif
    #if _SHADING_STYLING_TERMINATORPOSITION
        lllllllllll8 = lllllllllll8;
    #else
        lllllllllll8 = 0;
    #endif
#endif
#if _ENABLE_SPECULAR_STYLING
    lllllllllllllllll8 = 1;
#else
    lllllllllllllllll8 = 0;
#endif
#if _ENABLE_SPECULAR
    lllllllllllllllllll7 = 1;
#else
    lllllllllllllllllll7 = 0;
#endif
#ifndef _URP2D
#if _SUM_LIGHTS_BEFORE_POSTERIZATION
    lllllllllllllllllllllll6 = 1;
#else
    lllllllllllllllllllllll6 = 0;
#endif
#endif
#if _SHADING_USE_LIGHT_COLORS
    llllllllllllllllllllllll6 = 1;
#else
    llllllllllllllllllllllll6 = 0;
#endif
#if _SPECULAR_USE_LIGHT_COLORS
    llllllllllllllllllllllllll7 = 1;
#else
    llllllllllllllllllllllllll7 = 0;
#endif
#if _STYLING_SPECULAR_USE_LIGHT_COLORS
    llllllllllllllllllllll8 = 1;
#else
    llllllllllllllllllllll8 = 0;
#endif  
#ifdef _URP2D
    #if _ENABLE_OUTLINE
        _EnableOutline = 1;
    #else
        _EnableOutline = 0;
    #endif    
    #ifdef _CELL_METHOD
        llllllllllllllllllll6 = _CELL_METHOD;
    #endif  
    #if _ENABLE_MAINTEX_POSTERIZATION
        _EnableMainTexPosterization = 1;
    #else
        _EnableMainTexPosterization = 0;
    #endif   
    #ifdef _CONVERT_NORMAL_TO_ALBEDO
        _ConvertNormalToAlbedo = 1;
    #else
        _ConvertNormalToAlbedo = 0;
    #endif 
    #ifdef _USE_CORE_SHADOW_COLOR
        _UseCoreShadowColor = 1;
    #else
        _UseCoreShadowColor = 0;
    #endif 
    #ifdef _ENABLE_LIGHT_PARTITIONING
        _EnableLightPartitioning = 1;
    #else
        _EnableLightPartitioning = 0;
    #endif 
    #ifdef _ROUND_METHOD
        _RoundingMethod = _ROUND_METHOD;
    #endif 
#endif  
#endif
#ifdef _URP2D
    if (_ConvertNormalToAlbedo == 1)
    {   
        float3 llllllll9 = ll9;
        float lllllllll9 = (1 - llllllll9.y * _NormalToAlbedoConversionVector.y * _NormalToAlbedoConversionVector.w) * (1 - llllllll9.x * _NormalToAlbedoConversionVector.x * _NormalToAlbedoConversionVector.w) * (1 - llllllll9.z * _NormalToAlbedoConversionVector.z * _NormalToAlbedoConversionVector.w);
        light2DData.normalMapLighting = lllllllll9;       
    }
    else
    {
        light2DData.normalMapLighting = 1;
    }
    if (llllllllllllllllllllllllllllll6 == 0)
    {
        lllll6 *= light2DData.normalMapLighting;
    }
#endif
    float3 llllllllll9;
    if (llllllllllllllllll7 == 0)
    {
        llllllllll9 = ll9;
    }
    else
    {
#ifndef _URP2D
    #if _URP 
        llllllllll9 = inputData.normalWS;
    #else
        llllllllll9 = o.Normal;
    #endif
#else
        llllllllll9 = ll9;
#endif
    }
    float3 lllll2;
#ifndef _URP2D
    if (lllllllllllllllllllllllll7 == 0)
    {
        lllll2 = ll9;
    }
    else
    {
    #if _URP 
        lllll2 = inputData.normalWS;
    #else
        lllll2 = o.Normal;
    #endif
    }
#else
        lllll2 = ll9;
#endif
    float3 llllllllllll9;
    if (lllll8 == 0)
    {
        llllllllllll9 = ll9;
    }
    else
    {
#ifndef _URP2D
    #if _URP 
        llllllllllll9 = inputData.normalWS;
    #else
        llllllllllll9 = o.Normal;
    #endif        
#else
        llllllllllll9 = ll9;
#endif
    }
#ifndef _URP2D
    float3 lllllllllllllllllllllllllll1 = normalize(d.worldSpaceViewDir);
#else
    float3 llllllllllllll9 = normalize(_WorldSpaceCameraPos - lllllllllllllllll2); 
    float3 lllllllllllllllllllllllllll1 = float3(0.0,0.0,1.0);
    float3 llllllllllllllll9 = float3(0.0,0.0,1.0);
    float3 lllllllllllllllll9 = llllllllllllllllllllllllllll0 * 2.0 - 1.0;
    lllllllllllllllllllllllllll1 = lllllllllllllllll9;
    lllllllllllllllllllllllllll1 = float3(0.0,lllllllllllllllll9.y,lllllllllllllllll9.x);
#endif
    float4 llllllllllllllllll9 = 0;
    float llllllllllllllllllllllllllllll1 = -1;
    float llllllllllllllllllll9 = -1;
    half3 lllllllllllllllllllll9 = 0;
    float lllllllllll3 = 0; 
    float lllllllllllllllllllllll9 = 0; 
    float lllllllllllllllllllllllllllllll1 = 0;
    half3 lllllllllllllllllllllllll2 = 0;
    float llllllllllllllllllllllllll9 = 0;
    half3 lllllllllllllllllllllllllll9 = 0;
    ToonShadingData toonShadingData;
    toonShadingData.enableToonShading = llllllllllllllllllllllllllllll6;
#if _URP
    toonShadingData.normalWS = inputData.normalWS;
#endif
    toonShadingData.normalWSNoMap = ll9;
    toonShadingData.cellTransitionSmoothness = llllllllllllllllllllll6;
    toonShadingData.numberOfCells = lllllll9;
    toonShadingData.specularEdgeSmoothness = lllllllllllllllllllllll7;
    toonShadingData.shadingAffectByNormalMap = llllllllllllllllll7;
    toonShadingData.specularAffectedByNormalMap = lllllllllllllllllllllllll7;
#if _URP && !_URP2D
    if ((llllllllllllllllllllllllllll6 == 0 && llllllllllllllllllllllllllllll6 == 1 && (lllllll7 == 1 || lllllllllllllllllll7 == 1 || lllllllllllll7 == 1)) || (llll8 == 1 && (lllllll8 == 1 || lllllllllllll8 == 1 || lllllllllllllllll8 == 1)))
    {
        bool llllllllllllllllllllllllllll9 = llllllllllllllllllllllllllll6 == 0 && llllllllllllllllllllllllllllll6 == 1;
        bool lllllllllllllllllllllllllllll9 = llll8 == 1 && (lllllll8 == 1 || lllllllllllll8 == 1 || lllllllllllllllll8 == 1);
        bool llllllllllllllllllllllllllllll9 = llllllllllllllllll7 == lllll8; 
        bool lllllllllllllllllllllllllllllll9 = lllllllllllllllllllllllll7 == lllll8; 
        float l10 = 1;
        float ll10 = 1;
        Light mainLight = GetMainLight(inputData.shadowCoord, inputData.positionWS, inputData.shadowMask);
        float lll10 = max(mainLight.color.x, mainLight.color.y); 
        lll10 = max(lll10, mainLight.color.z);
        float3 llllllll9 = llllllllll9;
        float llllllllllllllllllllllllllll1 = llllllllllllllllllllll7;
        float lllllllllllllllllllllllllllll1 = lllllllllllllllllllllll7;
        float lllllllllllllllllllllll2 = llllllllllllllllllllllll7;
        float llllllll10 = llllllllllllllllllllllllll7;
        half lllllllll10 = lllllllllllllllllll7;
        half llllllllll10 = lllllll7;
        if (!llllllllllllllllllllllllllll9)
        {
            llllllll9 = llllllllllll9;
            lllll2 = llllllllllll9;            
            llllllllllllllllllllllllllll1 = lllllllllllllllllll8;
            lllllllllllllllllllllllllllll1 = llllllllllllllllllll8;
            lllllllllllllllllllllll2 = _StylingSpecularOpacity;
            llllllll10 = llllllllllllllllllllll8;
            lllllllll10 = lllllllllllllllll8;
            llllllllll10 = lllllll8;
            lllllllll7 = lllllllllll8;
        } 
        else 
        {
            if(lllllll7 == 0) 
            {
                llllllll9 = llllllllllll9;
                llllllllll10 = lllllll8;
            }
            if(lllllllllllllllllll7 == 0) 
            {
                lllll2 = llllllllllll9;           
                lllllllll10 = lllllllllllllllll8;
            }
            else 
            {
                if(lllllllllllllllllllllllllllll9 && lllllllllllllllll8 == 1 && llllllllllllllllll8 == 1) 
                {
                    lllllllllllllllllll8 = llllllllllllllllllllll7;
                    llllllllllllllllllll8 = lllllllllllllllllllllll7;
                }
            }
        }
        float lllllllllll10 = 1;
        if (mainLight.color.r > 0.0 || mainLight.color.g > 0.0 || mainLight.color.b > 0.0)
        {
            lllllllllll10 = (mainLight.shadowAttenuation * mainLight.distanceAttenuation);
            half llllllllllll10 = mainLight.distanceAttenuation * lll10;
            llllllllllllllllllllllllllllll1 = dot(mainLight.direction, llllllll9);
            if (lllllllll7 != 0.0)
            {
                llllllllllllllllllllllllllllll1 = shiftLinear(llllllllllllllllllllllllllllll1, max(-0.9999, lllllllll7));
            }            
            if (llllllllllllllllllllllllllllll1 > 0)
            {
                llllllllllllllllllllllllllllll1 *= llllllllllll10;
            }
            if (lllllllll10 || (!llllllllllllllllllllllllllll9 && lllllllllllllllll8))
            {
                lllllllllllllllllllllllllllllll1 = CalculateSpecularMask(lllll2, mainLight.direction, lllllllllllllllllllllllllll1, llllllllllllllllllllllllllll1, lllllllllllllllllllllllllllll1, llllllllllllllllllllllllllllll1); 
                lllllllllllllllllllllllllllllll1 *= lllllllllllllllllllllll2;
                if( (llllllllllllllllllllllllllll9 && lllllllllllll7) || (llll8 && lllllllllllll8))
                {
                    lllllllllllllllllllllllllllllll1 = min(lllllllllllllllllllllllllllllll1, mainLight.shadowAttenuation);
                }
                if (llllllll10 == 1)
                {
                    lllllllllllllllllllllllll2 = lllllllllllllllllllllllllllllll1 * mainLight.color;
                }
            }
            if (!llllllllllllllllllllllllllll9)
            {
                llllllllllllllllllll9 = llllllllllllllllllllllllllllll1;
                llllllllllllllllllllllllll9 = lllllllllllllllllllllllllllllll1;
                lllllllllllllllllllllllllll9 = lllllllllllllllllllllllll2;
                lllllllllllllllllllllllllllllll1 = 0;
                lllllllllllllllllllllllll2 = 0;
            } 
            else
            {
                if(lllllll7 == 0) 
                {
                    llllllllllllllllllll9 = llllllllllllllllllllllllllllll1;
                }
                if(lllllllllllllllllll7 == 0) 
                {
                    llllllllllllllllllllllllll9 = lllllllllllllllllllllllllllllll1;
                    lllllllllllllllllllllllllll9 = lllllllllllllllllllllllll2;
                    lllllllllllllllllllllllllllllll1 = 0;
                    lllllllllllllllllllllllll2 = 0;
                }
            }
            if (lllllllllllllllllllllllllllll9 && llllllllllllllllllllllllllll9) 
            {
                if (llllllllllllllllllllllllllllll9 && lllllllll8)
                {
                    llllllllllllllllllll9 = llllllllllllllllllllllllllllll1;
                }
                else
                {
                    if (lllllllll8)
                    {
                        lllllllllll8 = lllllllll7;
                    }
                    llllllllllllllllllll9 = dot(mainLight.direction, llllllllllll9);
                    if (lllllllllll8 != 0.0)
                    {
                        llllllllllllllllllll9 = shiftLinear(llllllllllllllllllll9, max(-0.9999, lllllllllll8));
                    }
                    if (llllllllllllllllllll9 > 0)
                    {
                        llllllllllllllllllll9 *= llllllllllll10; 
                    }
                }
                if (lllllllllllllllll8 == 1)
                {
                    if (llllllllllllllllllllllllllllll9 && lllllllllllllllllllllllllllllll9 && llllllllllllllllll8 == 1)
                    {
                        llllllllllllllllllllllllll9 = lllllllllllllllllllllllllllllll1;
                        lllllllllllllllllllllllllll9 = lllllllllllllllllllllllll2;
                    }
                    else
                    {
                        llllllllllllllllllllllllll9 = CalculateSpecularMask(llllllllllll9, mainLight.direction, lllllllllllllllllllllllllll1, lllllllllllllllllll8, llllllllllllllllllll8, llllllllllllllllllll9);
                        if(lllllllllllll7 || lllllllllllll8)
                        {
                            llllllllllllllllllllllllll9 = min(llllllllllllllllllllllllll9, mainLight.shadowAttenuation);
                        }
                        if (llllllllllllllllllllll8 == 1)
                        {
                            lllllllllllllllllllllllllll9 = llllllllllllllllllllllllll9 * mainLight.color;
                        }
                    }
                }
            }
            {
                l10 = lllllllllll10;
            }
        }
        else
        {
            l10 = 1;
            lllllllllll10 = 1;
            llllllllllllllllllllllllllllll1 = -1;
            llllllllllllllllllll9 = -1;
        }
        float lllllllllllll10 = 0;
        float llllllllllllll10 = 0;
        float lllllllllllllll10 = 0;
        float llllllllllllllll10 = 0;
        float lllllllllllllllll10 = 2;
        float llllllllllllllllll10 = 2;
        float lllllllllllllllllll10 = 0;
        float llllllllllllllllllll10 = 1;
#if defined(_ADDITIONAL_LIGHTS)  
    #if UNITY_VERSION >= 202200
        uint meshRenderingLayers = GetMeshRenderingLayer();
    #else
        uint meshRenderingLayers = GetMeshRenderingLightLayer();
    #endif
#if USE_CLUSTER_LIGHT_LOOP
        [loop] for (uint lightIndex = 0; lightIndex < min(URP_FP_DIRECTIONAL_LIGHTS_COUNT, MAX_VISIBLE_LIGHTS); lightIndex++)
        {
            Light addLight = GetAdditionalLight(lightIndex, inputData.positionWS, half4(1,1,1,1));       
    #ifdef _LIGHT_LAYERS
            if (IsMatchingLightLayer(addLight.layerMask, meshRenderingLayers))
    #endif
            {
                float lllllllllllllllllllllllllllll2 = max(addLight.color.x, addLight.color.y);
                lllllllllllllllllllllllllllll2 = max(lllllllllllllllllllllllllllll2, addLight.color.z);
                half llllllllllllllllllllll10 = addLight.distanceAttenuation * lllllllllllllllllllllllllllll2;
                float lllllllllllllllllllllll10 = smoothstep(0, 0.1 / (addLight.distanceAttenuation * lllllllllllllllllllllllllllll2), addLight.distanceAttenuation * lllllllllllllllllllllllllllll2);
                float llllllllllllllllllllllll10 = smoothstep(0, 0.01, addLight.distanceAttenuation * lllllllllllllllllllllllllllll2);
                lllllllllllllllllll10 += addLight.shadowAttenuation * llllllllllllllllllllll10;
                float lllllllllllllllllllllllll10 = dot(addLight.direction, llllllll9);
                if (lllllllll7 != 0)
                {
                    lllllllllllllllllllllllll10 = shiftLinear(lllllllllllllllllllllllll10, max(-0.9999, lllllllll7));
                }
                float llllllllllllllllllllllllll10 = lerp(-1, lllllllllllllllllllllllll10, lllllllllllllllllllllll10);
            {
                    llllllllllllllllllll10 = min(llllllllllllllllllll10, lerp(1, addLight.shadowAttenuation, llllllllllllllllllllllll10));
                }
                float lllllllllllllllllllllllllll10 = saturate(llllllllllllllllllllllllll10) * llllllllllllllllllllll10; 
                lllllllllllllll10 += lllllllllllllllllllllllllll10;
                if (llllllllllllllllllllllllllll9 || (lllllll8 == 1 && lllllllllllll8 == 1 && llllllllllllll8 == 1))
                {
                    if (lllllllllllll7 == 1 || (!llllllllllllllllllllllllllll9))
                    {
                        lllllllllllllllllllllllllll10 *= addLight.shadowAttenuation;
                    }
                    lllllllllllll10 += lllllllllllllllllllllllllll10;
                }
                if (llllllllllllllllllllllllllll9)
                {
                    if (llllllllllllllllllllllll6 == 1)
                    {
                        lllllllllllllllllllll9 += saturate(lllllllllllllllllllllllllll10 * (addLight.color));
                    }
                }
                if (sign(llllllllllllllllllllllllll10) == -1 && lllllllllllllll10 == 0)
                {
                    float llllllllllllllllllllllllllll10 = abs(llllllllllllllllllllllllll10);
                    lllllllllllllllll10 = min(lllllllllllllllll10, llllllllllllllllllllllllllll10);
                }
                float lllllllllllllllllllllllllllll10 = 0;
                if (lllllllllllllllllll7 || (!llllllllllllllllllllllllllll9 && lllllllllllllllll8))
                {
                    lllllllllllllllllllllllllllll10 = CalculateSpecularMask(lllll2, addLight.direction, lllllllllllllllllllllllllll1, llllllllllllllllllllllllllll1, lllllllllllllllllllllllllllll1, lllllllllllllllllllllllll10);
                    lllllllllllllllllllllllllllll10 *= lllllllllllllllllllllll2;
                    if (lllllllllllll7 || lllllllllllll8)
                    {
                        lllllllllllllllllllllllllllll10 *= addLight.shadowAttenuation;
                    }
                    lllllllllllllllllllllllllllllll1 += lllllllllllllllllllllllllllll10;
                    if (llllllll10 == 1)
                    {
                        lllllllllllllllllllllllll2 += addLight.color * lllllllllllllllllllllllllllll10;
                    }
                }
                if (lllllllllllllllllllllllllllll9 && llllllllllllllllllllllllllll9) 
                {
                    float llllllllllllllllllllllllllllll10 = 0;
                    if (llllllllllllllllllllllllllllll9 && (lllllllll8 || (lllllllll7 == lllllllllll8)))
                    {
                        llllllllllllllll10 = lllllllllllllll10;
                        llllllllllllllllll10 = lllllllllllllllll10;
                        llllllllllllll10 = lllllllllllll10;
                    }
                    else
                    {
                        llllllllllllllllllllllllllllll10 = dot(addLight.direction, llllllllllll9);
                        if (lllllllll8)
                        {
                            lllllllllll8 = lllllllll7;
                        }
                        if (lllllllllll8 != 0)
                        {
                            llllllllllllllllllllllllllllll10 = shiftLinear(llllllllllllllllllllllllllllll10, max(-0.9999, lllllllllll8));
                        }
                        float lllllllllllllllllllllllllllllll10 = lerp(-1, llllllllllllllllllllllllllllll10, lllllllllllllllllllllll10);
                        float l11 = saturate(lllllllllllllllllllllllllllllll10) * llllllllllllllllllllll10;
                        llllllllllllllll10 += l11;
                        if (lllllllllllll8 == 1 && lllllll8 == 1 && llllllllllllll8 == 1)
                        {
                            l11 *= addLight.shadowAttenuation;
                            llllllllllllll10 += l11;
                        }
                        if (sign(lllllllllllllllllllllllllllllll10) == -1 && llllllllllllllll10 == 0 && llllllllllllll8 == 1)
                        {
                            float ll11 = abs(lllllllllllllllllllllllllllllll10);
                            llllllllllllllllll10 = min(llllllllllllllllll10, ll11);
                        }
                    }
                    if (lllllllllllllllll8 == 1)
                    {
                        float lll11 = 0;
                        if (llllllllllllllllllllllllllllll9 && lllllllllllllllllllllllllllllll9 && llllllllllllllllll8 == 1)
                        {
                            llllllllllllllllllllllllll9 = lllllllllllllllllllllllllllllll1;
                            lll11 = lllllllllllllllllllllllllllll10;
                        }
                        else
                        {
                            lll11 = CalculateSpecularMask(lllll2, addLight.direction, lllllllllllllllllllllllllll1, lllllllllllllllllll8, llllllllllllllllllll8, lllllllllllllllllllllllll10);
                            lll11 = lll11;
                            if (lllllllllllll8)
                            {
                                lll11 *= addLight.shadowAttenuation;
                            }
                            llllllllllllllllllllllllll9 += lll11;
                        }
                        if (llllllllllllllllllllll8 == 1)
                        {
                            lllllllllllllllllllllllllll9 += addLight.color * lll11;
                        }
                    }
                }
            }
        }
#endif
        uint pixelLightCount = GetAdditionalLightsCount();
        LIGHT_LOOP_BEGIN(pixelLightCount)
        Light addLight = GetAdditionalLight(lightIndex, inputData.positionWS, half4(1, 1, 1, 1));
#ifdef _LIGHT_LAYERS
        if (IsMatchingLightLayer(addLight.layerMask, meshRenderingLayers))
#endif
        {  
            float lllllllllllllllllllllllllllll2 = max(addLight.color.x, addLight.color.y);
            lllllllllllllllllllllllllllll2 = max(lllllllllllllllllllllllllllll2, addLight.color.z);
            half llllllllllllllllllllll10 = addLight.distanceAttenuation * lllllllllllllllllllllllllllll2;
            float lllllllllllllllllllllll10 = smoothstep(0, 0.1/(addLight.distanceAttenuation * lllllllllllllllllllllllllllll2), addLight.distanceAttenuation * lllllllllllllllllllllllllllll2);
            float llllllllllllllllllllllll10 = smoothstep(0, 0.01, addLight.distanceAttenuation * lllllllllllllllllllllllllllll2);
            lllllllllllllllllll10 += addLight.shadowAttenuation * llllllllllllllllllllll10;
            float lllllllllllllllllllllllll10 = dot(addLight.direction, llllllll9);   
            if (lllllllll7 != 0)
            {
                lllllllllllllllllllllllll10 = shiftLinear(lllllllllllllllllllllllll10, max(-0.9999, lllllllll7));
            }
            float llllllllllllllllllllllllll10 = lerp(-1, lllllllllllllllllllllllll10, lllllllllllllllllllllll10);
            {
                llllllllllllllllllll10 = min(llllllllllllllllllll10, lerp(1, addLight.shadowAttenuation, llllllllllllllllllllllll10));
            }
            float lllllllllllllllllllllllllll10 = saturate(llllllllllllllllllllllllll10) * llllllllllllllllllllll10; 
            lllllllllllllll10 += lllllllllllllllllllllllllll10;
            if (llllllllllllllllllllllllllll9 || (lllllll8 == 1 && lllllllllllll8 == 1 && llllllllllllll8 == 1))
            {
                if (lllllllllllll7 == 1 || (!llllllllllllllllllllllllllll9))
                {
                    lllllllllllllllllllllllllll10 *= addLight.shadowAttenuation;
                }
                lllllllllllll10 += lllllllllllllllllllllllllll10;
            }
            if (llllllllllllllllllllllllllll9)
            {
                if (llllllllllllllllllllllll6 == 1)
                {
                    lllllllllllllllllllll9 += saturate(lllllllllllllllllllllllllll10 * (addLight.color));
                }
            }
            if (sign(llllllllllllllllllllllllll10) == -1 && lllllllllllllll10 == 0)
            {
                float llllllllllllllllllllllllllll10 = abs(llllllllllllllllllllllllll10);
                lllllllllllllllll10 = min(lllllllllllllllll10, llllllllllllllllllllllllllll10);
            }
            float lllllllllllllllllllllllllllll10 = 0;
            if (lllllllllllllllllll7 ||  (!llllllllllllllllllllllllllll9 && lllllllllllllllll8))
            {
                lllllllllllllllllllllllllllll10 = CalculateSpecularMask(lllll2, addLight.direction, lllllllllllllllllllllllllll1, llllllllllllllllllllllllllll1, lllllllllllllllllllllllllllll1, lllllllllllllllllllllllll10);
                lllllllllllllllllllllllllllll10 *= lllllllllllllllllllllll2;
                if(lllllllllllll7 || lllllllllllll8)
                {
                    lllllllllllllllllllllllllllll10 *= addLight.shadowAttenuation;
                }
                lllllllllllllllllllllllllllllll1 += lllllllllllllllllllllllllllll10;
                if (llllllll10 == 1)
                {
                    lllllllllllllllllllllllll2 += addLight.color * lllllllllllllllllllllllllllll10;
                }
            }
            if (lllllllllllllllllllllllllllll9 && llllllllllllllllllllllllllll9) 
            {
                float llllllllllllllllllllllllllllll10 = 0;
                if (llllllllllllllllllllllllllllll9 && (lllllllll8 || (lllllllll7 == lllllllllll8) ) )
                {
                    llllllllllllllll10 = lllllllllllllll10;
                    llllllllllllllllll10 = lllllllllllllllll10;
                    llllllllllllll10 = lllllllllllll10;
                }
                else
                {
                    llllllllllllllllllllllllllllll10 = dot(addLight.direction, llllllllllll9);
                    if (lllllllll8)
                    {
                        lllllllllll8 = lllllllll7;
                    }
                    if (lllllllllll8 != 0)
                    {
                        llllllllllllllllllllllllllllll10 = shiftLinear(llllllllllllllllllllllllllllll10, max(-0.9999, lllllllllll8));
                    }
                    float lllllllllllllllllllllllllllllll10 = lerp(-1, llllllllllllllllllllllllllllll10, lllllllllllllllllllllll10);
                    float l11 = saturate(lllllllllllllllllllllllllllllll10) * llllllllllllllllllllll10;
                    llllllllllllllll10 += l11;
                    if (lllllllllllll8 == 1 && lllllll8 == 1 && llllllllllllll8 == 1)
                    {
                        l11 *= addLight.shadowAttenuation;
                        llllllllllllll10 += l11;
                    }
                    if (sign(lllllllllllllllllllllllllllllll10) == -1 && llllllllllllllll10 == 0 && llllllllllllll8 == 1)
                    {
                        float ll11 = abs(lllllllllllllllllllllllllllllll10);
                        llllllllllllllllll10 = min(llllllllllllllllll10, ll11);
                    }
                }
                if (lllllllllllllllll8 == 1)
                {
                    float lll11 = 0;
                    if (llllllllllllllllllllllllllllll9 && lllllllllllllllllllllllllllllll9 && llllllllllllllllll8 == 1)
                    {
                        llllllllllllllllllllllllll9 = lllllllllllllllllllllllllllllll1;
                        lll11 = lllllllllllllllllllllllllllll10;
                    }
                    else
                    {
                        lll11 = CalculateSpecularMask(lllll2, addLight.direction, lllllllllllllllllllllllllll1, lllllllllllllllllll8, llllllllllllllllllll8, lllllllllllllllllllllllll10);
                        lll11 = lll11;
                        if( lllllllllllll8)
                        {
                            lll11 *= addLight.shadowAttenuation;
                        }
                        llllllllllllllllllllllllll9 += lll11;
                    }
                    if (llllllllllllllllllllll8 == 1)
                    {
                        lllllllllllllllllllllllllll9 += addLight.color * lll11;
                    }
                }
            }
        }
        LIGHT_LOOP_END
#endif
        if (llllllllllllllllllllllllllllll6 == 1 && lllllll7 == 1 && llllllllllllllllllllllll6 == 1)
        {
            float3 llllllllllllllllll11 = saturate(saturate(llllllllllllllllllllllllllllll1) * (mainLight.color));
            if(lllllllllllll7 == 1)
            {
                llllllllllllllllll11 *= lllllllllll10;
            }
            lllllllllllllllllllll9 += saturate(llllllllllllllllll11);
            lllllllllllllllllllll9 = saturate(lllllllllllllllllllll9);
            const float3 lllllllllllllllllll11 = float3(0.2126, 0.7152, 0.0722);
            float llllllllllllllllllll11   = dot(lllllllllllllllllllll9, lllllllllllllllllll11);  
            float lllllllllllllllllllll11 = Posterize(saturate(llllllllllllllllllll11), toonShadingData);       
            const float llllllllllllllllllllll11 = 1e-6;        
            float lllllllllllllllllllllll11   = (llllllllllllllllllll11 > llllllllllllllllllllll11) ? (lllllllllllllllllllll11 / llllllllllllllllllll11) : 0.0;
            lllllllllllllllllllll9 = lllllllllllllllllllll9 * lllllllllllllllllllllll11;
        }
        if (!llllllllllllllllllllllllllll9)
        {
            llllllllllllllll10 = lllllllllllllll10;
            llllllllllllll10 = lllllllllllll10;
            llllllllllllllllll10 = lllllllllllllllll10;
            llllllllllllllllllllllllll9 = lllllllllllllllllllllllllllllll1 + llllllllllllllllllllllllll9; 
            lllllllllllllllllllllllllll9 = lllllllllllllllllllllllll2;
            lllllllllllllllllllllllllllllll1 = 0;
            lllllllllllllllllllllllll2 = 0;
        }
        float llllllllllllllllllllllll11 = saturate(llllllllllllllllllllllllllllll1);
        float lllllllllllllllllllllllll11 = saturate(lllllllllllll10);
        if (lllllllllllllllllllllllllllllll6 == 0)
        {
            if (lllllllllllllllllllllll6 == 0)
            {
                llllllllllllllllllllllll11 = Posterize(llllllllllllllllllllllll11, toonShadingData);
                lllllllllllllllllllllllll11 = Posterize(lllllllllllllllllllllllll11, toonShadingData);
            }
        }
        if (llllllllllllllllllllllllllllll6 == 1 && lllllllllllll7 == 1 && (lllllll7 == 0 || (lllllllllllllllllllllllllll7 && ll8==1) ) )
        {
            float llllllllllllllllllllllllll11 = saturate(min(l10, llllllllllllllllllll10));
            float lllllllllllllllllllllllllll11 = lllllllllll10 * saturate(llllllllllllllllllllllllllllll1) + saturate(lllllllllllllll10) * lllllllllllllllllll10;
            float llllllllllllllllllllllllllll11 = saturate((1 - llllllllllllllllllllllllll11) * saturate(lllllllllllllllllllllllllll11)) + llllllllllllllllllllllllll11; 
            lllllllllll3 = llllllllllllllllllllllllllll11;
        }
        if (llll8 == 1)
        {
            if (lllllllllllll8 == 1)
            {
                if (llllllllllllll8 == 1)
                {
                    if (llllllllllllllllllll9 > 0)
                    {
                        llllllllllllllllllll9 = saturate(llllllllllllllllllll9);
                        llllllllllllllllllll9 *= lllllllllll10;                        
                    }
                    if (llllllllllllllll10 > 0)
                    {
                        llllllllllllllllllll9 = saturate(llllllllllllllllllll9);
                        llllllllllllllllllll9 += saturate(llllllllllllll10);
                    }
                    else
                    {
                        if (llllllllllllllllll10 > 0)
                        {
                            llllllllllllllllllll9 = max(llllllllllllllllllll9, -1 * llllllllllllllllll10);
                        }
                    }
                }
                else
                {
                    float llllllllllllllllllllllllll11 = min(l10, llllllllllllllllllll10);               
                    float lllllllllllllllllllllllllll11 = lllllllllll10 * saturate(llllllllllllllllllll9) + saturate(llllllllllllllll10) * lllllllllllllllllll10;
                    float llllllllllllllllllllllllllll11 = ((1 - llllllllllllllllllllllllll11) * (lllllllllllllllllllllllllll11)) + llllllllllllllllllllllllll11; 
                    lllllllllllllllllllllll9 = llllllllllllllllllllllllllll11;
                }
            }         
            if (lllllllllllll8 == 0 || llllllllllllll8 != 1) 
            {
                float l12 = llllllllllllllllllll9;
                llllllllllllllllllll9 = saturate(llllllllllllllllllll9) + saturate(llllllllllllllll10);
                if (llllllllllllllllllll9 == 0)
                {
                    llllllllllllllllllll9 = max(l12, -1 * llllllllllllllllll10);
                }
            }
        }
        if (llllllllllllllllllllllllllllll1 > 0)
        {
            llllllllllllllllllllllllllllll1 = saturate(llllllllllllllllllllllll11);
            if(lllllllllllll7 == 1)
            {
                llllllllllllllllllllllllllllll1 *= lllllllllll10;
            }
        }
        if (lllllllllllllll10 > 0)
        {
            llllllllllllllllllllllllllllll1 = saturate(llllllllllllllllllllllllllllll1);
            llllllllllllllllllllllllllllll1 += saturate(lllllllllllllllllllllllll11);
        }
        else
        {
            if (lllllllllllllllll10 > 0)
            {
                llllllllllllllllllllllllllllll1 = max(llllllllllllllllllllllllllllll1, -1 * lllllllllllllllll10);
            }
        }
        if (llllllllllllllllllllllllllllll1 < 0)
        {
        }
        else
        {
            if (lllllllllllllllllllllllllllllll6 == 0 && lllllllllllllllllllllll6 == 1)
            {
                llllllllllllllllllllllllllllll1 = Posterize(saturate(llllllllllllllllllllllllllllll1), toonShadingData);
            }
        }
    }
#else 
#if _URP2D
    float3 ll12 = saturate(lll9.xyz);
    float3 lll12 = normalize(lll9.xyz);
    float llll12 = max(max(ll12.r,ll12.b),ll12.g);
    llllllllllllllllllllllllllllll1 = llll12;
    llllllllllllllllllll9 = llll12;
    if (_ConvertNormalToAlbedo == 1)
    {
        llllllllllllllllllll9 *= light2DData.normalMapLighting;
    }
#else
    UnityLight lll9 = gi.light;
    llllllllllllllllllllllllllllll1 = dot(lll9.dir, llllllllll9);
    if (lllllllll7 != 0)
    {
        llllllllllllllllllllllllllllll1 = shiftLinear(llllllllllllllllllllllllllllll1, max(-0.9999, lllllllll7));
    }
    if (lllllllll8 == 0)
    {
        llllllllllllllllllll9 = dot(lll9.dir, llllllllllll9);
        if (lllllllllll8 != 0)
        {
            llllllllllllllllllll9 = shiftLinear(llllllllllllllllllll9, max(-0.9999, lllllllllll8));
        }
    }
    else
    {
        llllllllllllllllllll9 = llllllllllllllllllllllllllllll1;
    }
#if !_PASSFORWARDADD    
    if (llllllllllllllllllllllllllllll1 > 0)
    {
        lllllllllll3 = giInput.atten;
    }
    else
    {
        lllllllllll3 = 1;
    }
    if (lllllllllllll8 == 1 && lllllll8 == 1 && llllllllllllll8 == 1)
    {
        llllllllllllllllllll9 *= lllllllllll3;
    }
    lllllllllllllllllllllll9 = lllllllllll3;
        #else    
    lllllllllll3 = 0;    
    lllllllllllllllllllllllllll7 = 0;    
    llll8 = 0;    
    llllllllllllllllllllllll8 = 0;
    lllllll8 = 0;
    lllllllllllll8 = 0;
    stylingDataShading.color = 0;
    stylingDataSpecular.color = half4(gi.light.color,1);
#endif
#endif
#endif
    float lllll12 = lllllllllll3;
    float llllll12 = 0;
    float4 lllllll12 = 0;
    float3 lll3;
    if (lll8 == 0)
    {
        lll3 = ll9;
    }
    else
    {
#ifndef _URP2D
    #if _URP 
        lll3 = inputData.normalWS;
    #else
        lll3 = o.Normal;
    #endif
#else
        lll3 = ll9;
#endif
    }
    float llllllllllllllllllllllllll11 = 0;      
    if (llllllllllllllllllllllllllll6 == 0) 
    {
        lllll12 = lllllllllll3;
        if (llllllllllllllllllllllllllllll6 == 1)
        {
            if (lllllllllllllllllllllllllllllll6 == 0)
            {
                if (lllllll7 == 1)
                {
                    float4 llllll12 = saturate(llllllllllllllllllllllllllllll1);
                #ifndef _URP2D
                    #if _URP
                        if (llllllllllllllllllllllll6 == 1)
                        {
                            lllll6 *= float4(lllllllllllllllllllll9, 1); 
                        }
                    #else
                        llllll12 = Posterize(llllll12.r, toonShadingData);
                    #endif
                    lllll6 = lerp(lerp(llllllll7, lllll6, 1 - llllllll7.a), lllll6, llllll12);
                #else
                    if (_ConvertNormalToAlbedo == 1)
                    {   
                        lllll6 *= light2DData.normalMapLighting;
                    }
                    if(_UseCoreShadowColor == 1 ) 
                    {
                        light2DData.lightMultiply = 1 - ((1-light2DData.lightMultiply) * llllllll7.a);
                        light2DData.lightAdditive = 1 - ((1-light2DData.lightAdditive) * llllllll7.a);
                    }
                    if (_EnableLightPartitioning == 1)                     
                    {
                        const float llllllllllllllllllllll11 = 1e-6;        
                        toonShadingData.cellTransitionSmoothness = max(0.01, toonShadingData.cellTransitionSmoothness);
                        if (llllllllllllllllllll6 == 0) 
                        {                
                            if(_EnableMainTexPosterization) 
                            {
                                lllll6 = float4(PosterizeMulti(lllll6.rgb, toonShadingData, _RoundingMethod),1);
                            }  
                            if(_RoundingMethod == 1) 
                            {
                                toonShadingData.numberOfCells += 1;
                            }
                            float3 llllllllllll12 = saturate(light2DData.lightMultiply);
                            float lllllllllllll12 = (max(max(llllllllllll12.r,llllllllllll12.b),llllllllllll12.g));
                            float4 llllllllllllll12 = max(light2DData.lightMultiply, 0.0); 
                            llllllllllllll12 =  llllllllllllll12 / (1.0 + llllllllllllll12); 
                            float4 lllllllllllllll12 = float4(PosterizeMulti(llllllllllllll12.rgb, toonShadingData, _RoundingMethod),1);   
                            if(_UseCoreShadowColor == 1 ) 
                            {
                                lllllllllllllll12 = lerp(llllllll7, lllllllllllllll12, lllllllllllll12);        
                            }
                            lllllllllllllll12 = saturate(lllllllllllllll12);                    
                            lllllllllllllll12 = min(lllllllllllllll12, 1.0 - llllllllllllllllllllll11);
                            lllllllllllllll12 = lllllllllllllll12 / (1.0 - lllllllllllllll12); 
                            lllll6 *= lllllllllllllll12;    
                            float3 llllllllllllllll12 = saturate(light2DData.lightAdditive);
                            float lllllllllllllllll12 = (max(max(llllllllllllllll12.r,llllllllllllllll12.b),llllllllllllllll12.g));
                            float4 llllllllllllllllll12 = max(light2DData.lightAdditive, 0.0); 
                            llllllllllllllllll12 =  llllllllllllllllll12 / (1.0 + llllllllllllllllll12); 
                            float4 lllllllllllllllllll12 = float4(PosterizeMulti(llllllllllllllllll12.rgb, toonShadingData, _RoundingMethod),1);   
                            if(_UseCoreShadowColor == 1 ) 
                            {
                                lllllllllllllllllll12 = lerp(llllllll7, lllllllllllllllllll12, lllllllllllllllll12);        
                            }                 
                            lllllllllllllllllll12 = saturate(lllllllllllllllllll12);                                    
                            lllllllllllllllllll12 = min(lllllllllllllllllll12, 1.0 - llllllllllllllllllllll11);
                            lllllllllllllllllll12 = lllllllllllllllllll12 / (1.0 - lllllllllllllllllll12); 
                            lllll6 += lllllllllllllllllll12;                  
                        } 
                        else if (llllllllllllllllllll6 == 1) 
                        {
                            float4 llllllllllllllllllll12 = light2DData.lightMultiply + light2DData.lightAdditive;
                            lllll6 *= light2DData.lightMultiply;   
                            lllll6 += light2DData.lightAdditive;      
                            float4 lllllllllllllllllllll12 = lllll6;
                            if(_UseCoreShadowColor == 1 ) 
                            {
                                float llllllllllllllllllllll12 =  llllllllllllllllllll12 / (1.0 + llllllllllllllllllll12); 
                                lllll6 = lerp(llllllll7, lllll6, llllllllllllllllllllll12);
                            }
                            lllllllllllllllllllll12 = lllllllllllllllllllll12 / (1 + lllllllllllllllllllll12);
                            float lllllllllllllllllllllll12 = ((lllllllllllllllllllll12.r + lllllllllllllllllllll12.b + lllllllllllllllllllll12.g)/3) / ((lllll6.r+lllll6.b+lllll6.g)/3);
                            lllll6 *= lllllllllllllllllllllll12;
                            lllll6 = lllll6 / (1.0 - lllll6); 
                            lllll6 = float4(PosterizeMulti(lllll6.rgb, toonShadingData, _RoundingMethod),1);   
                        }
                        else 
                        {
                            const float3 lllllllllllllllllll11 = float3(0.2126, 0.7152, 0.0722);
                            float4 llllllllllllll12 = light2DData.lightMultiply;
                            float4 llllllllllllllllll12 = light2DData.lightAdditive;
                            float4 lllllllllllllllllllllllllll12 = lllll6 * llllllllllllll12 + llllllllllllllllll12;
                            float llllllllllllllllllll11   = dot(lllllllllllllllllllllllllll12, lllllllllllllllllll11);  
                            float lllllllllllllllllllll11 = PosterizeMulti(saturate(llllllllllllllllllll11), toonShadingData, _RoundingMethod);       
                            float lllllllllllllllllllllll11   = (llllllllllllllllllll11 > llllllllllllllllllllll11) ? (lllllllllllllllllllll11 / llllllllllllllllllll11) : 0.0;
                            lllll6 = lllllllllllllllllllllllllll12 * lllllllllllllllllllllll11;
                            if(_UseCoreShadowColor == 1 ) 
                            {
                                lllll6 = lerp(llllllll7, lllll6, saturate(lllllllllllllllllllll11));
                            }
                        }
                    }                    
                    else
                    {
                        lllll6 = (lllll6 * light2DData.lightMultiply) + light2DData.lightAdditive;
                        if(_UseCoreShadowColor == 1 ) 
                        {                     
                            float4 llllllllllllllllllll12 = light2DData.lightMultiply + light2DData.lightAdditive;
                            lllll6 = lerp(llllllll7, lllll6, saturate(llllllllllllllllllll12));
                        } 
                    }
                #endif
#ifndef _URP2D
    #if !_URP
                    if (lllllllllllll7 == 1)
                    {
                        float3 l13 = lerp(llllllll7.rgb, llllll9.rgb, 1 - llllllll7.a);
                        lllll6 = float4(lerp(l13, lllll6.rgb, saturate(lllllllllll3)), llllll9.a);
                    }
    #endif
#endif
                }
            }
            else
            {
#ifndef _URP2D         
                float ll13 = min(0.95, llllllllllllllllllllllllllllll1); 
                if (llll7 == 1 && lllllll7 == 0 && llllllllllllllllllllllllllllll1 < 0)
                {
                    ll13 = 0;
                }
                ll13 = (ll13 + 1) / 2;
                float4 lll13 = float4(0, 0, 0, 0);
                float llll13 = llll9.z;
                float lllll13 = ll13 * (llll13 - 1);
                float2 llllll13 = (lllll13 + 0.5) * llll9.xy;
                lll13 = tex2D(ll7, llllll13);
#else
                #define GRADIENT_WIDTH 512.0 
                float ll13 = saturate(llllllllllllllllllllllllllllll1); 
                float lllll13 = min(GRADIENT_WIDTH - 1.0, ll13 * (GRADIENT_WIDTH - 1.0));
                float2 llllll13 = float2((lllll13 + 0.5) / GRADIENT_WIDTH, 0.5);
                float4 lll13 = tex2D(ll7, llllll13);
#endif
                DoBlending(lllll6, llllll7, lllll7, lll13);
            }
#ifndef _URP2D
            if (lllllllllllll7 == 0 && (llll8 == 0 || lllllllllllll8 == 0))
            {
                lllllllllll3 = 1;
            }
#endif
            if (lllllll7 == 1 && lllllllllllllllllllllllllllllll6 == 0)
            {
#ifndef _URP2D
                if (llllllllllllllllllllllllllllll1 < 0.0)
                {
                    lllll6 = llllllllllll7;
                    lllllllllll7 = 1 - lllllllllll7;
                    float lllllllllll13 = lllllllllll7 * llllllllll7;
                    float llllllllllll13 = smoothstep(-lllllllllll13 + 0.01, -llllllllll7, llllllllllllllllllllllllllllll1);
                    float3 l13 = lerp(llllllll7.rgb, llllll9.rgb, 1 - llllllll7.a);
                    float3 llllllllllllll13 = lerp(llllllllllll7.rgb, llllll9.rgb, 1 - llllllllllll7.a);
                    lllll6 = float4(lerp(l13, llllllllllllll13, llllllllllll13), llllll9.a);
                }
#else
#endif
            }
        #ifndef _URP2D   
            if (lllllll7 == 0 && lllllllllllllllllllllllllllllll6 == 0 && lllllllllllll7 == 1)
            {
                float3 l13 = lerp(llllllll7.rgb, llllll9.rgb, 1 - llllllll7.a);
                lllll6 = float4(lerp(l13, lllll6.rgb, saturate(lllllllllll3)), llllll9.a);
            }
        #endif
        }
#if _ENABLE_SPECULAR || !_USE_OPTIMIZATION_DEFINES
        if (lllllllllllllllllll7 == 1)
        {
#ifndef _URP2D       
    #if _URP
    #else
            lllllllllllllllllllllllllllllll1 = CalculateSpecularMask(lllll2, lll9.dir, lllllllllllllllllllllllllll1, llllllllllllllllllllll7, lllllllllllllllllllllll7, llllllllllllllllllllllllllllll1);
            lllllllllllllllllllllllllllllll1 *= llllllllllllllllllllllll7;
            if (lllllllllllll7 == 1)
            {
                lllllllllllllllllllllllllllllll1 *= lllllllllll3;
            }
    #endif
#else
            CalculateSpecularMaskFromMultipleLights2DGlobal(lllllllllllllllll2, llllllllllllllllllllllllllllll1, lllll2, lllllllllllllllllllllllllll1, llllllllllllllllllllll7, lllllllllllllllllllllll7, llllllllllllllllllllllll7,
                                                lllllllllllllllllllllllllllllll1, lllllllllllllllllllllllll2);  
#endif
#if _USE_OPTIMIZATION_DEFINES
#ifdef _SPECULAR_BLENDING
            llllllllllllllllllll7 = _SPECULAR_BLENDING;
#endif
#endif
            half4 llllllllllllllll13;
        #if _URP || _URP2D
            if (llllllllllllllllllllllllll7 == 1)
            {
                llllllllllllllll13 = (half4(lllllllllllllllllllllllll2, 1));
            #if _URP2D
                llllllllllllllll13 = (half4(lllllllllllllllllllllllll2, 1));
            #endif
            }
            else
        #endif
            {
                llllllllllllllll13 = lllllllllllllllllllll7;
            }
            DoBlending(lllll6, lllllllllllllllllllllllllllllll1, llllllllllllllllllll7, llllllllllllllll13);
        }
#endif
#ifndef _URP2D
    #if _URP
    lllll6 += half4(surface.emission, 0);
    #else
    lllll6 += half4(o.Emission, 0);
    #endif
#else
    lllll6 += half4(lllllllllllllllll6, 0);
#endif
    }
#ifndef _URP2D
    else 
    {
        ToonShadingData toonShadingData;
        toonShadingData.enableToonShading = llllllllllllllllllllllllllllll6;
#if _URP
        toonShadingData.normalWS = inputData.normalWS;
#endif
        toonShadingData.normalWSNoMap = ll9;
        toonShadingData.cellTransitionSmoothness = llllllllllllllllllllll6;
        toonShadingData.numberOfCells = lllllll9;
        toonShadingData.specularEdgeSmoothness = lllllllllllllllllllllll7;
        toonShadingData.shadingAffectByNormalMap = llllllllllllllllll7;
        toonShadingData.specularAffectedByNormalMap = lllllllllllllllllllllllll7;
#if _USE_OPTIMIZATION_DEFINES
#if _ENABLE_TOON_SHADING 
                toonShadingData.enableToonShading = 1;
#else
                toonShadingData.enableToonShading = 0;
#endif
#endif
#if _SHADING_BLINNPHONG       
        if (lllllllllllllllllllllllllllll6 == 0) 
        {
#if _URP
        #if UNITY_VERSION >= 202120
            lllll6 = UniversalFragmentBlinnPhong(inputData, surface.albedo, half4(surface.specular, surface.smoothness), surface.smoothness, surface.emission, surface.alpha,lllllllllllllll6, toonShadingData);
        #else
            lllll6 = UniversalFragmentBlinnPhong(inputData, surface.albedo, half4(surface.specular, surface.smoothness), surface.smoothness, surface.emission, surface.alpha, toonShadingData);
        #endif
#else
#endif
        }
#endif        
#if _SHADING_PBR
        if (lllllllllllllllllllllllllllll6 == 1) 
        {      
#if _URP
            lllll6 = UniversalFragmentPBR(inputData, surface, toonShadingData);
#else
#if !_PASSFORWARDADD
    #if _USESPECULAR || _USESPECULARWORKFLOW || _SPECULARFROMMETALLIC
    #else
        LightingStandard_GI_Toon(o, giInput, gi, toonShadingData);
        #if defined(_OVERRIDE_BAKEDGI)
            gi.indirect.diffuse = l.DiffuseGI;
            gi.indirect.specular = l.SpecularGI;
        #endif
        lllll6 = LightingStandard_Toon (o, d.worldSpaceViewDir, gi, toonShadingData);
        lllll6 += half4(o.Emission, 0);
    #endif     
#else
    #if _USESPECULAR
#elif _BDRF3 || _SIMPLELIT
#else
                  lllll6 = LightingStandard_Toon (o, d.worldSpaceViewDir, gi, toonShadingData);
#endif
#endif
#endif
        }
#endif
    }
#endif
    float llllllllllll3 = 0;
    if (llllllllllllllllllllllllllllll6 == 1)
    {
#ifndef _URP2D
    #if _URP
        Light mainLight = GetMainLight(inputData.shadowCoord, inputData.positionWS, inputData.shadowMask);
        float llllllllllllllllll13 = dot(mainLight.direction, lll3);
        float lllllllllllllllllll13 = mainLight.shadowAttenuation;
    #else
        float llllllllllllllllllllllllllllll1 = dot(lll9.dir, lll3);
    #endif
#else 
#endif
    #if _ENABLE_RIM || !_USE_OPTIMIZATION_DEFINES
        #if !_USE_OPTIMIZATION_DEFINES
        if (lllllllllllllllllllllllllll7 == 1)
        #endif
        {
    #ifndef _URP2D            
        #if _URP         
            llllllllllll3 = CalculateRimMask(lll3, lllllllllllllllllllllllllll1, llllllllllllllllllllllllllllll7, lllllllllllllllllllllllllllllll7, llllllllllllllllll13, ll8, lllllllllllll7, lllllll7, lllllllllllllllllll13);
        #else
            llllllllllll3 = CalculateRimMask(lll3, lllllllllllllllllllllllllll1, llllllllllllllllllllllllllllll7, lllllllllllllllllllllllllllllll7, llllllllllllllllllllllllllllll1, ll8, lllllllllllll7, lllllll7, lllllllllll3);
        #endif   
    #else
            llllllllllll3 = CalculateRimMask(lll3, llllllllllllllll9, llllllllllllllllllllllllllllll7, lllllllllllllllllllllllllllllll7, llllllllllllllllllllllllllllll1, ll8, 0, lllllll7, 1);
    #endif
            llllllllllll3 *= l8;
        #if _USE_OPTIMIZATION_DEFINES
            #ifdef _RIM_BLENDING
                        llllllllllllllllllllllllllll7 = _RIM_BLENDING;
            #endif
        #endif   
            llllllllllll3 = saturate(llllllllllll3);
            DoBlending(lllll6, llllllllllll3, llllllllllllllllllllllllllll7, lllllllllllllllllllllllllllll7);
        }
    #endif
    }
#if _ENABLE_STYLING || !_USE_OPTIMIZATION_DEFINES   
    #if !_USE_OPTIMIZATION_DEFINES
    if (llll8 == 1)
    #endif
    {
#ifdef _EMISSION 
#ifndef _URP2D    
    #if _URP
        float3 lllllllllllllllllllll13 = surface.emission;
    #else
        float3 lllllllllllllllllllll13 = o.Emission;
    #endif
#else
        float3 lllllllllllllllllllll13 = lllllllllllllllll6;
#endif
        float llllllllllllllllllllllll13 = max(max(lllllllllllllllllllll13.r, lllllllllllllllllllll13.g), lllllllllllllllllllll13.b);
#endif
#if !_URP
        if (lllllllllllllllll8 == 1)
        {
            if (lllllllllllllllllll7 == 0 || llllllllllllllllll8 == 0) 
            {
                float lllllllllllllllllllllllll13 = saturate(llllllllllllllllllllllllllllll1);
            #ifndef _URP2D
                llllllllllllllllllllllllll9 = CalculateSpecularMask(llllllllllll9, lll9.dir, lllllllllllllllllllllllllll1, lllllllllllllllllll8, llllllllllllllllllll8, lllllllllllllllllllllllll13);
                llllllllllllllllllllllllll9 = saturate(llllllllllllllllllllllllll9);
                llllllllllllllllllllllllll9 *= lllll12;
            #else
            CalculateSpecularMaskFromMultipleLights2DGlobal(lllllllllllllllll2, lllllllllllllllllllllllll13, llllllllllll9, lllllllllllllllllllllllllll1, lllllllllllllllllll8, llllllllllllllllllll8, 1,
                                                        llllllllllllllllllllllllll9, lllllllllllllllllllllllllll9);
#endif
            }
            else
            {
                llllllllllllllllllllllllll9 = saturate(lllllllllllllllllllllllllllllll1);
            }
        }
#endif
#ifndef _URP2D        
        if (lllllllllllllllllllll8 == 1)
        {
            llllllllllllllllllll9 = 1 - llllllllllllllllllll9 - llllllllllllllllllllllllll9 * 10;
            llllllllllllllllllll9 = 1 - llllllllllllllllllll9;
            lllll12 = 1 - ((1 - lllll12) - llllllllllllllllllllllllll9 * 10);
        }
#else
            llllllllllllllllllll9 = 1 - llllllllllllllllllll9 - lllllllllllllllllllllllllllllll1 * 10;
            llllllllllllllllllll9 = 1 - llllllllllllllllllll9;
#endif
        #if _USE_OPTIMIZATION_DEFINES
            #ifdef _SHADING_STYLING_DRAWSPACE
        uvSpaceDataShading.drawSpace = _SHADING_STYLING_DRAWSPACE;
            #endif
            #ifdef _SHADING_STYLING_COORDINATESYSTEM
        uvSpaceDataShading.coordinateSystem = _SHADING_STYLING_COORDINATESYSTEM;
            #endif
        #endif
#ifndef _URP2D
    #if _URP
        float2 llllllllllllllllllllllllll13 = ConvertToDrawSpace(inputData, lllllllll1, uvSpaceDataShading, llllllllllllllllllllllllllll0);
    #else
        float2 llllllllllllllllllllllllll13 = ConvertToDrawSpace(d.worldSpacePosition, d.worldSpaceNormal, lllllllll1, uvSpaceDataShading, llllllllllllllllllllllllllll0);
    #endif
#else 
        float2 llllllllllllllllllllllllll13 = ConvertToDrawSpace(lllllllllllllllll2, ll9,llllllll1, lllllllll1, uvSpaceDataShading, llllllllllllllllllllllllllll0);
#endif
        float lllllllllllllllllllllllllllll13 = stylingDataShading.density;
        float lllllllllllllllllllllllllllllll4 = stylingDataShading.size;
        float lllllllllllllllllllllllllllllll13 = 1;
#if _ENABLE_SHADING_STYLING || !_USE_OPTIMIZATION_DEFINES   
    #if !_USE_OPTIMIZATION_DEFINES
        if (lllllll8 != 0)
    #endif        
        {
            float l14 = 0;            
        #if _USE_OPTIMIZATION_DEFINES
            #ifdef _SHADING_STYLING_BLENDING
                    positionAndBlendingDataShading.blending = _SHADING_STYLING_BLENDING;
            #endif                   
            #ifdef _SHADING_STYLE
                stylingDataShading.style = _SHADING_STYLE;
            #endif
            #if _SHADING_STYLING_RANDOMIZER
                stylingRandomDataShading.enableRandomizer = 1;
            #else
                stylingRandomDataShading.enableRandomizer = 0;
            #endif
        #endif
            RequiredNoiseData requiredNoiseDataShading;
    #if _USE_OPTIMIZATION_DEFINES
        #ifdef _SHADING_STYLING_RANDOMIZER_PERLIN
            requiredNoiseDataShading.perlinNoise = 1;
        #else
            requiredNoiseDataShading.perlinNoise = 0;
        #endif
        #ifdef _SHADING_STYLING_RANDOMIZER_PERLIN_FLOORED
            requiredNoiseDataShading.perlinNoiseFloored = 1;
        #else
            requiredNoiseDataShading.perlinNoiseFloored = 0;
        #endif         
        #ifdef _SHADING_STYLING_RANDOMIZER_WHITE
            requiredNoiseDataShading.whiteNoise = 1;
        #else
            requiredNoiseDataShading.whiteNoise = 0;
        #endif
        #ifdef _SHADING_STYLING_RANDOMIZER_WHITE_FLOORED
            requiredNoiseDataShading.whiteNoiseFloored = 1;
        #else
            requiredNoiseDataShading.whiteNoiseFloored = 0;
        #endif            
    #else            
            requiredNoiseDataShading.perlinNoise = 1;
            requiredNoiseDataShading.perlinNoiseFloored = 1;
            requiredNoiseDataShading.whiteNoise = 1;
            requiredNoiseDataShading.whiteNoiseFloored = 1;
    #endif
            float ll14 = (llllllllllllllllllll9);
            if (positionAndBlendingDataShading.isInverted == 1)
            {
                ll14 = 1 - saturate(ll14);
            }
            if (stylingDataShading.style == 0) 
            {                             
                float lllllllllllllllllllllllllllll13 = stylingDataShading.density;
                float lllllllllllllllllllllllllllllll4 = stylingDataShading.size;
                lllllllllllllllllllllllllllllll4 = stylingDataShading.size / 2;
                if (lllllllll8 == 0)
                {
                    lllllll9 = llllllllll8;
                }
                else
                {
                    lllllll9 = lllllllllllllllllllll6;
                }
            #if _USE_OPTIMIZATION_DEFINES            
                #ifdef _SHADING_STYLING_NUMBER_OF_CELLS_HATCHING
                        lllllll9 = _SHADING_STYLING_NUMBER_OF_CELLS_HATCHING;
                #endif                            
            #endif
                float lllll14 = (1. / lllllll9) * llllllllllll8;
                int llllll14 = ceil((max(ll14 - lllll14, 0)) * lllllll9);
                llllll14 = lllllll9 - llllll14;
                float lllllll14 = stylingDataShading.rotation;
                float llllllll14 = radians(lllllll14);
                float lllllllll14 = stylingDataShading.rotationBetweenCells;
                float llllllllll14 = radians(lllllllll14);
                float2 lllllllllll14; 
                NoiseSampleData noiseSampleData; 
                lllllllllllllllllllllllllllllll13 = 1;
                float lllllllllllllllll1 = 0;
    #if _USE_OPTIMIZATION_DEFINES            
                [unroll(lllllll9)]
    #else
                [unroll(15)]
    #endif
                    for (int i = 1; i <= llllll14; i++)
                    {
                        lllllllllllllllllllllllllllllll4 = stylingDataShading.size / 2;
                        float lllllllllllll14 = i - 1;
                        float llllllllll4 = llllllll14 + llllllllll14 * lllllllllllll14;
                        llllllllllllllllllllllllll13 += lllllllllllllllll1; 
                        lllllllllll14 = RotateUVRadians(llllllllllllllllllllllllll13, llllllllll4);
                        noiseSampleData = SampleNoiseData(lllllllllll14, stylingDataShading, stylingRandomDataShading, requiredNoiseDataShading, llllllllllllllll4, lllllllllllllllll4);
                        lllllllllllllllll1 += (float) stylingDataShading.density;
                        float lllllllllllllll14 = lllllllllll14.x;
                        lllllllllllllll14 *= stylingDataShading.density;
                        if (stylingRandomDataShading.enableRandomizer == 1)
                        {
                            lllllllllllllll14 += noiseSampleData.perlinNoise * stylingRandomDataShading.noiseIntensity;
                            float ll5 = 0;
                            if (stylingRandomDataShading.thicknessRandomMode == 0)
                            {
                                ll5 = noiseSampleData.whiteNoise;
                            }
                            else if (stylingRandomDataShading.thicknessRandomMode == 1) 
                            {
                                ll5 = noiseSampleData.perlinNoiseFloored;
                            }
                            else 
                            {
                                ll5 = ((noiseSampleData.perlinNoiseFloored) + noiseSampleData.whiteNoise) / 2;
                            }
                            float lll5 = remap(0, 1, 0.0, lllllllllllllllllllllllllllllll4, ll5 * stylingRandomDataShading.thicknesshRandomIntensity);
                            lllllllllllllllllllllllllllllll4 -= lll5;
                            float llll5 = 0;
                            if (stylingRandomDataShading.spacingRandomMode == 0)
                            {
                                llll5 = noiseSampleData.whiteNoise;
                            }
                            else if (stylingRandomDataShading.spacingRandomMode == 1) 
                            {
                                llll5 = noiseSampleData.perlinNoiseFloored;
                            }
                            else 
                            {
                                llll5 = ((noiseSampleData.perlinNoiseFloored) + noiseSampleData.whiteNoise) / 2;
                            }
                            float lllll5 = remap(0, 1, -0.5 + lllllllllllllllllllllllllllllll4, 0.5 - lllllllllllllllllllllllllllllll4, llll5);
                            lllllllllllllll14 += lllll5 * stylingRandomDataShading.spacingRandomIntensity * saturate(1 - stylingRandomDataShading.noiseIntensity); 
                        }
                        lllllllllllllll14 = abs(frac(lllllllllllllll14) - 0.5);
                        float llllllllllllllllllll14 = (float) (lllllll9 - i) / lllllll9;
                        float lllllllllllllllllllll14 = remap(0, 1, 0, lllll14, llllllllllll8);
                        float llllll5;
                        float llllllll5;
                        float llllllllllllllllllllllll14 = 0;
                        if (stylingRandomDataShading.enableRandomizer == 1)
                        {
                            float lllllll5 = 0;
                            if (stylingRandomDataShading.lengthRandomMode == 0)
                            {
                                lllllll5 = noiseSampleData.whiteNoise * saturate(1 - stylingRandomDataShading.noiseIntensity);
                            }
                            else if (stylingRandomDataShading.lengthRandomMode == 1)
                            {
                                lllllll5 = noiseSampleData.perlinNoiseFloored; 
                            }
                            else
                            {
                                lllllll5 = ((noiseSampleData.perlinNoiseFloored + (noiseSampleData.whiteNoise * saturate(1 - stylingRandomDataShading.noiseIntensity))) / 2); 
                            }
                            llllllll5 = lllllll5 * stylingRandomDataShading.lengthRandomIntensity;
                            llllllllllllllllllllllll14 = remap(0, 1, 0, llllllllllllllllllll14 + lllllllllllllllllllll14, llllllll5);
                        }
                        llllll5 = remap(0, llllllllllllllllllll14 + lllllllllllllllllllll14 - llllllllllllllllllllllll14, 0, 1, ll14);
                        if (i == lllllll9 && sign(ll14) == 1)
                        {
                            float llllllllllllllllllllllll14 = 0;
                            if (stylingRandomDataShading.enableRandomizer == 1)
                            {
                                llllllllllllllllllllllll14 = remap(0, 1, 0, 1 - lllll14, llllllll5);
                            }
                            llllll5 = remap(0, lllll14, 1 - lllll14 + llllllllllllllllllllllll14, 1 + llllllllllllllllllllllll14, ll14);
                        }
                        if (i == lllllll9 && sign(ll14) == -1)
                        {
                            float lllllllllllllllllllllllllll14 = (float) 1. / lllllll9;
                            lllllllllllllllllllll14 = remap(0, 1, 0, lllllllllllllllllllllllllll14, llllllllllll8);
                            float llllllllllllllllllllllll14 = 0;
                            if (stylingRandomDataShading.enableRandomizer == 1)
                            {
                                llllllllllllllllllllllll14 = remap(0, 1, 0, 1 - lllllllllllllllllllll14, llllllll5);
                            }
                            llllll5 = remap(0, -1, 1 - lllllllllllllllllllll14 + llllllllllllllllllllllll14, 0, ll14);
                        }
                        float lllllllll5 = smoothstep(1 - stylingDataShading.sizeFalloff, 1, llllll5);
                        if (lllll12 <= 0 && ll14 > 0)
                        {
                        }
                        lllllllll5 = max(lllllllllllllllllllllllllllllll4 - lllllllll5, 0);
                        float llllllllll5;
                        if (stylingRandomDataShading.enableRandomizer == 1)
                        {
                            float lllllllllll5 = 0;
                            if (stylingRandomDataShading.hardnessRandomMode == 0) 
                            {
                                lllllllllll5 = noiseSampleData.whiteNoise;
                            }
                            else if (stylingRandomDataShading.hardnessRandomMode == 1) 
                            {
                                lllllllllll5 = noiseSampleData.perlinNoiseFloored * 5;
                            }
                            else
                            {
                                lllllllllll5 = ((noiseSampleData.perlinNoiseFloored + noiseSampleData.whiteNoise) / 2) * 5;
                            }
                            llllllllll5 = remap(0, 1, 0, lllllllll5, min(saturate(stylingDataShading.hardness - lllllllllll5 * stylingRandomDataShading.hardnessRandomIntensity), stylingDataShading.hardness));
                        }
                        else
                        {
                            llllllllll5 = remap(0, 1, 0, lllllllll5, stylingDataShading.hardness);
                        }
                        if (lllllllll5 != 0)
                        {
                            float llllllllllll5 = 0;
                            if (lllllllllllllllllllllllllllll4)
                            {
                                llllllllllll5 = fwidth(lllllllllllllll14); 
                            }
                            if (lllllllll5 == lllllllllllllllllllllllllllllll4 && stylingDataShading.size == 1)
                            {
                                llllllllllll5 = 0;
                            }
                            if (llllllllll5 - llllllllllll5 < 0)
                            {
                                llllllllllll5 = 0;
                            }
                            lllllllllllllll14 = smoothstep(llllllllll5 - llllllllllll5, lllllllll5 + llllllllllll5, lllllllllllllll14);
                        }
                        else
                        {
                            lllllllllllllll14 = 1; 
                        }
                        lllllllllllllll14 = 1 - lllllllllllllll14;
                        if (stylingRandomDataShading.enableRandomizer == 1)
                        {
                            float lllllllllllll5;
                            if (stylingRandomDataShading.opacityRandomMode == 0) 
                            {
                                lllllllllllll5 = noiseSampleData.whiteNoise;
                            }
                            else if (stylingRandomDataShading.opacityRandomMode == 1) 
                            {
                                lllllllllllll5 = noiseSampleData.perlinNoiseFloored * 5;
                            }
                            else 
                            {
                                lllllllllllll5 = ((noiseSampleData.perlinNoiseFloored + noiseSampleData.whiteNoise) / 2) * 5;
                            }
                            lllllllllllllll14 = saturate(lllllllllllllll14 - (lllllllllllll5 * stylingRandomDataShading.opacityRandomIntensity));
                        }
                        float llllllllllllll5 = smoothstep(saturate(min(1 - stylingDataShading.opacityFalloff, 1)), 1, llllll5);
                        lllllllllllllll14 *= 1 - llllllllllllll5;
                        lllllllllllllll14 = 1 - lllllllllllllll14;
                        lllllllllllllllllllllllllllllll13 = min(lllllllllllllll14, lllllllllllllllllllllllllllllll13);
                    }
                lllllllllllllllllllllllllllllll13 = 1 - lllllllllllllllllllllllllllllll13;
                lllllllllllllllllllllllllllllll13 *= stylingDataShading.opacity;
                lllllllllllllllllllllllllllllll13 = 1 - lllllllllllllllllllllllllllllll13;
                l14 = lllllllllllllllllllllllllllllll13;             
            }
            else if (stylingDataShading.style == 1) 
            {               
                float2 llllllllllllllllll5 = llllllllllllllllllllllllll13;
                float2 lllllll4 = RotateUV(llllllllllllllllll5, stylingDataShading.rotation);
                NoiseSampleData noiseSampleData = SampleNoiseData(lllllll4, stylingDataShading, stylingRandomDataShading, requiredNoiseDataShading, llllllllllllllll4, lllllllllllllllll4);
                if (false)
                {
                } 
                float llllll15 = 1 - ll14;
                float ll6 = Halftones(llllll15, lllllll4, stylingDataShading, stylingRandomDataShading, noiseSampleData);
                l14 = ll6;
            }
            if (false)
            {
            }
        #ifdef _EMISSION
            l14 = 1 - l14;
            l14 = saturate(l14 - llllllllllllllllllllllll13);
            l14 = 1 - l14;
        #endif
    #ifndef _URP2D    
            #if _USE_OPTIMIZATION_DEFINES
                #if _ENABLE_STYLING_DISTANCEFADE
                     generalStylingData.enableDistanceFade = 1;
                #else
                    generalStylingData.enableDistanceFade = 0;
                #endif
            #endif
            if (generalStylingData.enableDistanceFade == 1)
            {
                float llllllll15 = ll14;
                if (stylingDataShading.style == 0)
                {
                    int lllllll9;
                    if (lllllllll8 == 0)
                    {
                        lllllll9 = llllllllll8;
                    }
                    else
                    {
                        lllllll9 = lllllllllllllllllllll6;
                    }
                    float lllll14 = (1. / lllllll9) * llllllllllll8;
                    float lllllllllllllllllllll14 = remap(0, 1, 0, lllll14, llllllllllll8);
                    llllllll15 -= -1 + ((lllllll9 - 1.) / lllllll9) + lllllllllllllllllllll14;
                }
                float llllllllllll15 = distance(_WorldSpaceCameraPos, d.worldSpacePosition);
                float lllllllllllll15 = max(llllllll15, 1 - stylingDataShading.opacityFalloff);
                lllllllllllll15 = remap(1 - stylingDataShading.opacityFalloff, 1, 0, 1, lllllllllllll15);
                float llllllllllllll15 = max(llllllll15, 1 - stylingDataShading.sizeFalloff);
                llllllllllllll15 = remap(1 - stylingDataShading.sizeFalloff, 1, 0, 1, llllllllllllll15);
                float lllllllllllllll15 = lerp(0.0, 1, saturate(1 - stylingDataShading.size)); 
                if (generalStylingData.adjustDistanceFadeValue == 1)
                {
                    lllllllllllllll15 = generalStylingData.distanceFadeValue;
                }
                llllllllllllll15 = max(lllllllllllllll15, llllllllllllll15 * 2);
                lllllllllllll15 = max(lllllllllllllll15, lllllllllllll15);
                float llllllllllllllll15 = max(llllllllllllll15, lllllllllllll15);
                llllllllllllllll15 = saturate(llllllllllllllll15);
                l14 = lerp(l14, llllllllllllllll15, saturate(((llllllllllll15 - generalStylingData.distanceFadeStartDistance) / generalStylingData.distanceFadeFalloff)));
            }
    #endif     
            if (positionAndBlendingDataShading.isInverted == 1)
            {
                l14 = 1 - l14;
            }
            DoBlending(lllll6, 1 - l14, positionAndBlendingDataShading.blending, stylingDataShading.color);
            if (false)
            {                
            }
            if (false)
            {
            }
        }
#endif
#ifndef _URP2D    
#if (_ENABLE_CASTSHADOWS_STYLING && _STYLING_CASTSHADOWS_SYNC_WITH_OTHER_STYLING != 1) || !_USE_OPTIMIZATION_DEFINES   
    #if !_USE_OPTIMIZATION_DEFINES
        if (lllllllllllll8 && llllllllllllll8 != 1)   
    #endif
        {
        #if _USE_OPTIMIZATION_DEFINES
            #ifdef _CASTSHADOWS_STYLING_BLENDING
                positionAndBlendingDataCastShadows.blending = _CASTSHADOWS_STYLING_BLENDING;
            #endif
            #ifdef _CASTSHADOWS_STYLING_DRAWSPACE
                uvSpaceDataCastShadows.drawSpace = _CASTSHADOWS_STYLING_DRAWSPACE;
            #endif
            #ifdef _CASTSHADOWS_STYLING_COORDINATESYSTEM
                uvSpaceDataCastShadows.coordinateSystem = _CASTSHADOWS_STYLING_COORDINATESYSTEM;
            #endif            
            #ifdef _CASTSHADOWS_STYLE
                stylingDataCastShadows.style = _CASTSHADOWS_STYLE;
            #endif
            #if _CASTSHADOWS_STYLING_RANDOMIZER
                stylingRandomDataCastShadows.enableRandomizer = 1;
            #else
                stylingRandomDataCastShadows.enableRandomizer = 0;
            #endif
        #endif
            RequiredNoiseData requiredNoiseDataCastShadows;
    #if _USE_OPTIMIZATION_DEFINES
        #ifdef _CASTSHADOWS_STYLING_RANDOMIZER_PERLIN
            requiredNoiseDataCastShadows.perlinNoise = 1;
        #else
            requiredNoiseDataCastShadows.perlinNoise = 0;
        #endif
        #ifdef _CASTSHADOWS_STYLING_RANDOMIZER_PERLIN_FLOORED
            requiredNoiseDataCastShadows.perlinNoiseFloored = 1;
        #else
            requiredNoiseDataCastShadows.perlinNoiseFloored = 0;
        #endif         
        #ifdef _CASTSHADOWS_STYLING_RANDOMIZER_WHITE
            requiredNoiseDataCastShadows.whiteNoise = 1;
        #else
            requiredNoiseDataCastShadows.whiteNoise = 0;
        #endif
        #ifdef _CASTSHADOWS_STYLING_RANDOMIZER_WHITE_FLOORED
            requiredNoiseDataCastShadows.whiteNoiseFloored = 1;
        #else
            requiredNoiseDataCastShadows.whiteNoiseFloored = 0;
        #endif            
        #else            
            requiredNoiseDataCastShadows.perlinNoise = 1;
            requiredNoiseDataCastShadows.perlinNoiseFloored = 1;
            requiredNoiseDataCastShadows.whiteNoise = 1;
            requiredNoiseDataCastShadows.whiteNoiseFloored = 1;
        #endif
    #if _URP
            float2 lllllllllllllllll15 = ConvertToDrawSpace(inputData, lllllllll1, uvSpaceDataCastShadows, llllllllllllllllllllllllllll0);           
    #else
            float2 lllllllllllllllll15 = ConvertToDrawSpace(d.worldSpacePosition, d.worldSpaceNormal, lllllllll1, uvSpaceDataCastShadows, llllllllllllllllllllllllllll0);
    #endif
        #ifdef _EMISSION
            lllllllllllllllllllllll9 = 1 - lllllllllllllllllllllll9;
            lllllllllllllllllllllll9 = saturate(lllllllllllllllllllllll9 - llllllllllllllllllllllll13);
            lllllllllllllllllllllll9 = 1 - lllllllllllllllllllllll9;
        #endif
            lllll12 = lllllllllllllllllllllll9;
            float l14 = 0;
            if (stylingDataCastShadows.style == 0) 
            {
                float llllllllllllllllllll15 = stylingDataCastShadows.rotation;
                float lllllllllllllllllllll15 = radians(llllllllllllllllllll15);
                float llllllllllllllllllllll15 = stylingDataCastShadows.rotationBetweenCells;
                float lllllllllllllllllllllll15 = radians(llllllllllllllllllllll15);
                float llllllllllllllllllllllll15 = llllllllllllllll8;
                llllllllllllllllllllllll15 = min(llllllllllllllllllllllll15, 0.99);
                float lllllllllllllllllllllllll15 = 1;
                float lllllll9 = lllllllllllllll8;
            #if _USE_OPTIMIZATION_DEFINES            
                #ifdef _CASTSHADOWS_STYLING_NUMBER_OF_CELLS_HATCHING
                        lllllll9 = _CASTSHADOWS_STYLING_NUMBER_OF_CELLS_HATCHING;
                #endif                           
                [unroll(lllllll9)]
            #else
                [unroll(15)]
            #endif
                for (int j = 1; j <= lllllll9; j++)
                {
                    lllllllllllllllllllllll9 = min(j / lllllll9, lllll12);
                    if (lllllll9 != 1)
                    {
                        float lllllllllll6 = 0;
                        if (lllllll9 <= 1)
                        {
                            lllllllllll6 = 0.0;
                        }
                        else
                        {
                            float llllllllllllllllllllllllllll15 = (float) j - 1;
                            float lllllllllllllllllllllllllllll15 = (float) (lllllll9 - 1);
                            float llllllllllllllllllllllllllllll15 = llllllllllllllllllllllllllll15 / lllllllllllllllllllllllllllll15;
                            lllllllllll6 = lerp(1.0, llllllllllllllllllllllllllllll15, llllllllllllllllllllllll15);
                        }
                        float lllllllllllllllllllllllllllllll15 = min(lllllllllll6, lllll12); 
                        lllllllllllllllllllllllllllllll15 = remap(0, lllllllllll6, 0, 1, lllll12);
                        lllllllllllllllllllllll9 = lllllllllllllllllllllllllllllll15;
                        lllllllllllllllllllllll9 = max(lllllllllll3, lllll12);
                    }
                    else
                    {
                        lllllllllllllllllllllll9 = lllll12;
                    }
                    float lllllllllllll14 = j - 1;
                    float llllllllll4 = lllllllllllllllllllll15 + lllllllllllllllllllllll15 * lllllllllllll14;
                    float2 lllllllllll14 = RotateUVRadians(lllllllllllllllll15, llllllllll4);
                    lllllllllll14.x += (j - 1) / (float) lllllll9 * stylingDataCastShadows.density; 
                    NoiseSampleData noiseSampleData = SampleNoiseData(lllllllllll14, stylingDataCastShadows, stylingRandomDataCastShadows, requiredNoiseDataCastShadows, llllllllllllllll4, lllllllllllllllll4);
                    float llll16 = Hatching(1 - lllllllllllllllllllllll9, lllllllllll14, stylingDataCastShadows, stylingRandomDataCastShadows, noiseSampleData, lllllllllllllllllllllllllllll4);
                    llll16 = 1 - llll16;
                    {
                        lllllllllllllllllllllllll15 = min(llll16, lllllllllllllllllllllllll15);
                    }
                }
                l14 = lllllllllllllllllllllllll15;
            }
            else if (stylingDataCastShadows.style == 1) 
            {                        
                float2 lllllll4 = RotateUV(lllllllllllllllll15, stylingDataCastShadows.rotation);
                NoiseSampleData noiseSampleData = SampleNoiseData(lllllll4, stylingDataCastShadows, stylingRandomDataCastShadows, requiredNoiseDataCastShadows, llllllllllllllll4, lllllllllllllllll4);
                float ll6 = Halftones(1 - lllllllllllllllllllllll9, lllllll4, stylingDataCastShadows, stylingRandomDataCastShadows, noiseSampleData);
                l14 = ll6;            
            }
            DoBlending(lllll6, 1 - l14, positionAndBlendingDataCastShadows.blending, stylingDataCastShadows.color);                    
        }
#endif        
#endif   
#if _ENABLE_SPECULAR_STYLING || !_USE_OPTIMIZATION_DEFINES   
    #if !_USE_OPTIMIZATION_DEFINES
        if (lllllllllllllllll8)   
    #endif
        {
        #if _USE_OPTIMIZATION_DEFINES
            #ifdef _SPECULAR_STYLING_BLENDING
                positionAndBlendingDataSpecular.blending = _SPECULAR_STYLING_BLENDING;
            #endif
            #ifdef _SPECULAR_STYLING_DRAWSPACE
                uvSpaceDataSpecular.drawSpace = _SPECULAR_STYLING_DRAWSPACE;
            #endif
            #ifdef _SPECULAR_STYLING_COORDINATESYSTEM
                uvSpaceDataSpecular.coordinateSystem = _SPECULAR_STYLING_COORDINATESYSTEM;
            #endif            
            #ifdef _SPECULAR_STYLE
                stylingDataSpecular.style = _SPECULAR_STYLE;
            #endif
            #if _SPECULAR_STYLING_RANDOMIZER
                stylingRandomDataSpecular.enableRandomizer = 1;
            #else
                stylingRandomDataSpecular.enableRandomizer = 0;
            #endif
        #endif
            RequiredNoiseData requiredNoiseDataSpecular;
#if _USE_OPTIMIZATION_DEFINES            
#ifdef _SPECULAR_STYLING_RANDOMIZER_PERLIN
                    requiredNoiseDataSpecular.perlinNoise = 1;
#else
                    requiredNoiseDataSpecular.perlinNoise = 0;
#endif
#ifdef _SPECULAR_STYLING_RANDOMIZER_PERLIN_FLOORED
                    requiredNoiseDataSpecular.perlinNoiseFloored = 1;
#else
                    requiredNoiseDataSpecular.perlinNoiseFloored = 0;
#endif         
#ifdef _SPECULAR_STYLING_RANDOMIZER_WHITE
                    requiredNoiseDataSpecular.whiteNoise = 1;
#else
                    requiredNoiseDataSpecular.whiteNoise = 0;
#endif
#ifdef _SPECULAR_STYLING_RANDOMIZER_WHITE_FLOORED
                    requiredNoiseDataSpecular.whiteNoiseFloored = 1;
#else
                    requiredNoiseDataSpecular.whiteNoiseFloored = 0;
#endif      
#else            
            requiredNoiseDataSpecular.perlinNoise = 1;
            requiredNoiseDataSpecular.perlinNoiseFloored = 1;
            requiredNoiseDataSpecular.whiteNoise = 1;
            requiredNoiseDataSpecular.whiteNoiseFloored = 1;
#endif
    #ifndef _URP2D     
        #if _URP
                float2 lllllll16 = ConvertToDrawSpace(inputData, lllllllll1, uvSpaceDataSpecular, llllllllllllllllllllllllllll0);
        #else
            float2 lllllll16 = ConvertToDrawSpace(d.worldSpacePosition, d.worldSpaceNormal, lllllllll1, uvSpaceDataSpecular, llllllllllllllllllllllllllll0);
            #endif
    #else
            float2 lllllll16 = ConvertToDrawSpace(lllllllllllllllll2, ll9,llllllll1, lllllllll1, uvSpaceDataSpecular, llllllllllllllllllllllllllll0);
    #endif
                float2 lllllll4 = RotateUV(lllllll16, stylingDataSpecular.rotation);
                lllllll16 = lllllll4;
            NoiseSampleData noiseSampleData = SampleNoiseData(lllllll16, stylingDataSpecular, stylingRandomDataSpecular, requiredNoiseDataSpecular, llllllllllllllll4, lllllllllllllllll4);
    #if _USE_OPTIMIZATION_DEFINES 
        #ifdef _SPECULAR_STYLE
            stylingDataSpecular.style = _SPECULAR_STYLE;
        #endif
    #endif
            float l14 = 0;     
            if (stylingDataSpecular.style == 0) 
            {               
                l14 = Hatching(llllllllllllllllllllllllll9, lllllll16, stylingDataSpecular, stylingRandomDataSpecular, noiseSampleData, lllllllllllllllllllllllllllll4);
                l14 = 1 - l14;
            }
            else if (stylingDataSpecular.style == 1) 
            {
                float ll6 = Halftones(llllllllllllllllllllllllll9, lllllll16, stylingDataSpecular, stylingRandomDataSpecular, noiseSampleData);
                l14 = ll6;              
            }
            #if _USE_OPTIMIZATION_DEFINES
                #ifdef _SPECULAR_STYLING_BLENDING
                     positionAndBlendingDataSpecular.blending = _SPECULAR_STYLING_BLENDING;
                #endif
            #endif
            half4 llllllllllllllll13;
            if (llllllllllllllllllllll8 == 1)
            {
                llllllllllllllll13 = half4(lllllllllllllllllllllllllll9, 1);
                #if _URP2D
                llllllllllllllll13 *= _StylingSpecularColorBoost;
                #endif
            }
            else
            {
                llllllllllllllll13 = stylingDataSpecular.color;
            }
            DoBlending(lllll6, 1 - l14, positionAndBlendingDataSpecular.blending, llllllllllllllll13);
        }
#endif
#if _ENABLE_RIM_STYLING || !_USE_OPTIMIZATION_DEFINES   
        #if !_USE_OPTIMIZATION_DEFINES
        if (llllllllllllllllllllllll8)
        #endif
        {
        #if _USE_OPTIMIZATION_DEFINES
            #ifdef _RIM_STYLING_BLENDING
                    positionAndBlendingDataRim.blending = _RIM_STYLING_BLENDING;
            #endif
            #ifdef _RIM_STYLING_DRAWSPACE
                uvSpaceDataRim.drawSpace = _RIM_STYLING_DRAWSPACE;
            #endif
            #ifdef _RIM_STYLING_COORDINATESYSTEM
                uvSpaceDataRim.coordinateSystem = _RIM_STYLING_COORDINATESYSTEM;
            #endif        
            #ifdef _RIM_STYLE
                stylingDataRim.style = _RIM_STYLE;
            #endif
            #if _RIM_STYLING_RANDOMIZER
                stylingRandomDataRim.enableRandomizer = 1;
            #else
                stylingRandomDataRim.enableRandomizer = 0;
            #endif
        #endif
            RequiredNoiseData requiredNoiseDataRim;
        #if _USE_OPTIMIZATION_DEFINES
            #ifdef _RIM_STYLING_RANDOMIZER_PERLIN
                requiredNoiseDataRim.perlinNoise = 1;
            #else
                requiredNoiseDataRim.perlinNoise = 0;
            #endif
            #ifdef _RIM_STYLING_RANDOMIZER_PERLIN_FLOORED
                requiredNoiseDataRim.perlinNoiseFloored = 1;
            #else
                requiredNoiseDataRim.perlinNoiseFloored = 0;
            #endif         
            #ifdef _RIM_STYLING_RANDOMIZER_WHITE
                requiredNoiseDataRim.whiteNoise = 1;
            #else
                requiredNoiseDataRim.whiteNoise = 0;
            #endif
            #ifdef _RIM_STYLING_RANDOMIZER_WHITE_FLOORED
                requiredNoiseDataRim.whiteNoiseFloored = 1;
            #else
                requiredNoiseDataRim.whiteNoiseFloored = 0;
            #endif      
        #else            
            requiredNoiseDataRim.perlinNoise = 1;
            requiredNoiseDataRim.perlinNoiseFloored = 1;
            requiredNoiseDataRim.whiteNoise = 1;
            requiredNoiseDataRim.whiteNoiseFloored = 1;
        #endif
    #ifndef _URP2D             
        #if _URP
            float2 llllllllllllll16 = ConvertToDrawSpace(inputData, lllllllll1, uvSpaceDataRim, llllllllllllllllllllllllllll0);
        #else
            float2 llllllllllllll16 = ConvertToDrawSpace(d.worldSpacePosition, d.worldSpaceNormal, lllllllll1, uvSpaceDataRim, llllllllllllllllllllllllllll0);
        #endif
    #else
            float2 llllllllllllll16 = ConvertToDrawSpace(lllllllllllllllll2, ll9,llllllll1, lllllllll1, uvSpaceDataRim, llllllllllllllllllllllllllll0);
    #endif
            float2 lllllll4 = RotateUV(llllllllllllll16, stylingDataRim.rotation);
            NoiseSampleData noiseSampleData = SampleNoiseData(lllllll4, stylingDataRim, stylingRandomDataRim, requiredNoiseDataRim, llllllllllllllll4, lllllllllllllllll4);
            if (lllllllllllllllllllllllllll7 == 0 || lllllllllllllllllllllllll8 == 0) 
            {
        #ifndef _URP2D     
            #if _URP
                Light mainLight = GetMainLight(inputData.shadowCoord, inputData.positionWS, inputData.shadowMask);
                float llllllllllllllllll13 = dot(mainLight.direction, lll3);
                float lllllllllllllllllll13 = mainLight.shadowAttenuation;
                llllllllllll3 = CalculateRimMask(llllllllllll9, lllllllllllllllllllllllllll1, llllllllllllllllllllllllll8, lllllllllllllllllllllllllll8, llllllllllllllllll13, llllllllllllllllllllllllllll8, lllllllllllll7, lllllll7, lllllllllllllllllll13);
            #else
                llllllllllll3 = CalculateRimMask(llllllllllll9, lllllllllllllllllllllllllll1, llllllllllllllllllllllllll8, lllllllllllllllllllllllllll8, llllllllllllllllllllllllllllll1, llllllllllllllllllllllllllll8, lllllllllllll7, lllllll7, lllllllllll3);
            #endif
        #else
              llllllllllll3 = CalculateRimMask(llllllllllll9, llllllllllllllll9, llllllllllllllllllllllllll8, lllllllllllllllllllllllllll8, llllllllllllllllllllllllllllll1, llllllllllllllllllllllllllll8, 0, lllllll7, 1);
        #endif              
            }
            llllllllllll3 = saturate(llllllllllll3 - lllllllllllllllllllllllllllllll1 * 10);
            float l14 = 0;
            if (stylingDataRim.style == 0) 
            {
                l14 = Hatching(llllllllllll3, lllllll4, stylingDataRim, stylingRandomDataRim, noiseSampleData, lllllllllllllllllllllllllllll4);
                l14 = 1 - l14;
            }
            else if (stylingDataRim.style == 1) 
            {
                float ll6 = Halftones(llllllllllll3, lllllll4, stylingDataRim, stylingRandomDataRim, noiseSampleData);
                l14 = ll6;
            }
            DoBlending(lllll6, 1-l14, positionAndBlendingDataRim.blending, stylingDataRim.color);
        }
    #endif
    }
#endif
#ifdef _URP2D
    if (_EnableOutline)
    {
        float4 llllllllllllllllllllll16 = SpriteOutline(lllllllll1, lllllllllllllllllllllllllll6);
        lllll6 = (llllllllllllllllllllll16.a > 0.001) ? llllllllllllllllllllll16 : lllll6;
    }
#endif


}

    
    
    
    
    
    
    
    
    
    
    
    
    
    

        


void AddTheToonShader(inout float4 albedo,
#ifndef _URP2D

#if _URP
    InputData inputData, 
    SurfaceData surface,
#else
    #if _USESPECULAR || _USESPECULARWORKFLOW || _SPECULARFROMMETALLIC
                 SurfaceOutputStandardSpecular o,
    #elif _BDRFLAMBERT || _BDRF3 || _SIMPLELIT

                 SurfaceOutput o,
    #else
                 SurfaceOutputStandard o,
    #endif

    UnityGI gi,
    #if !_PASSFORWARDADD
    UnityGIInput giInput,
    #endif
#endif

 ShaderData d
#if _URP
    #if UNITY_VERSION >= 202120
, float3 normalTS
    #endif
#endif

#else
    UnityTexture2D _GradientTex,
    UnityTexture2D _NoiseTex1, 
    UnityTexture2D _NoiseTex2,
    float3 worldPosition,
    Light2DData light2DData,
    float2 uv,
    float4 screenUV,
    float3 normal,
    float3 emission,
    float2 originScreenPos
#endif
)
{
    
#ifndef _URP2D
    float2 uv = d.texcoord0.xy;
        
    

    
    float3 pureNormal = d.worldSpaceNormal;

    float4 screenUV = d.extraV2F0;
#else
    float3 pureNormal = normal;
#endif

    

    
    UVSpaceData uvSpaceDataShading;
    uvSpaceDataShading.drawSpace = _DrawSpace;
    uvSpaceDataShading.coordinateSystem = _CoordinateSystem;
    uvSpaceDataShading.polarCenterMode = _PolarCenterMode;
    uvSpaceDataShading.polarCenter = _PolarCenter;
    uvSpaceDataShading.sSCameraDistanceScaled = _SSCameraDistanceScaled;
    uvSpaceDataShading.anchorSSToObjectsOrigin = _AnchorSSToObjectsOrigin;
    
     
    
#ifndef _URP2D
    UVSpaceData uvSpaceDataCastShadows;
    uvSpaceDataCastShadows.drawSpace = _CastShadowsDrawSpace;
    uvSpaceDataCastShadows.coordinateSystem = _CastShadowsCoordinateSystem;
    uvSpaceDataCastShadows.polarCenterMode = _CastShadowsPolarCenterMode;
    uvSpaceDataCastShadows.polarCenter = _CastShadowsPolarCenter;
    uvSpaceDataCastShadows.sSCameraDistanceScaled = _CastShadowsSSCameraDistanceScaled;
    uvSpaceDataCastShadows.anchorSSToObjectsOrigin = _CastShadowsAnchorSSToObjectsOrigin;
#endif
    
    UVSpaceData uvSpaceDataSpecular;
    uvSpaceDataSpecular.drawSpace = _SpecularDrawSpace;
    uvSpaceDataSpecular.coordinateSystem = _SpecularCoordinateSystem;
    uvSpaceDataSpecular.polarCenterMode = _SpecularPolarCenterMode;
    uvSpaceDataSpecular.polarCenter = _SpecularPolarCenter;
    uvSpaceDataSpecular.sSCameraDistanceScaled = _SpecularSSCameraDistanceScaled;
    uvSpaceDataSpecular.anchorSSToObjectsOrigin = _SpecularAnchorSSToObjectsOrigin;
    
    UVSpaceData uvSpaceDataRim;
    uvSpaceDataRim.drawSpace = _RimDrawSpace;
    uvSpaceDataRim.coordinateSystem = _RimCoordinateSystem;
    uvSpaceDataRim.polarCenterMode = _RimPolarCenterMode;
    uvSpaceDataRim.polarCenter = _RimPolarCenter;
    uvSpaceDataRim.sSCameraDistanceScaled = _RimSSCameraDistanceScaled;
    uvSpaceDataRim.anchorSSToObjectsOrigin = _RimAnchorSSToObjectsOrigin;

#ifndef _URP2D
    GeneralStylingData generalStylingData;
    generalStylingData.enableDistanceFade = _EnableStylingDistanceFade;
    generalStylingData.distanceFadeStartDistance = _StylingDFStartingDistance;
    generalStylingData.distanceFadeFalloff = _StylingDFFalloff;
    generalStylingData.adjustDistanceFadeValue = _StylingAdjustDistanceFadeValue;
    generalStylingData.distanceFadeValue = _StylingDistanceFadeValue;
#endif
    StylingData stylingDataShading;
    stylingDataShading.style = _ShadingStyle;
    stylingDataShading.type = 0;
    stylingDataShading.color = _StylingColor;
    stylingDataShading.rotation = _StylingShadingInitialDirection;
    stylingDataShading.rotationBetweenCells = _StylingShadingRotationBetweenCells;
    stylingDataShading.density = _StylingShadingDensity;
    stylingDataShading.offset = _StylingShadingHalftonesOffset;
    stylingDataShading.size = _StylingShadingThickness;
    stylingDataShading.sizeControl = _StylingShadingThicknessControl;
    stylingDataShading.sizeFalloff = _StylingShadingThicknessFalloff;
    stylingDataShading.roundness = _StylingShadingHalftonesRoundness;
    stylingDataShading.roundnessFalloff = _StylingShadingHalftonesRoundnessFalloff;
    stylingDataShading.hardness = _StylingShadingHardness;
    stylingDataShading.opacity = _StylingShadingOpacity;
    stylingDataShading.opacityFalloff = _StylingShadingOpacityFalloff;

    StylingData stylingDataSpecular;
    stylingDataSpecular.style = _SpecularStyle;
    stylingDataSpecular.type = 1;
    stylingDataSpecular.color = _StylingSpecularColor;
    stylingDataSpecular.rotation = _StylingSpecularRotation;
    stylingDataSpecular.density = _StylingSpecularDensity;
    stylingDataSpecular.offset = _StylingSpecularHalftonesOffset;
    stylingDataSpecular.size = _StylingSpecularThickness;
    stylingDataSpecular.sizeControl = _StylingSpecularThicknessControl;
    stylingDataSpecular.sizeFalloff = _StylingSpecularThicknessFalloff;
    stylingDataSpecular.roundness = _StylingSpecularHalftonesRoundness;
    stylingDataSpecular.roundnessFalloff = _StylingSpecularHalftonesRoundnessFalloff;
    stylingDataSpecular.hardness = _StylingSpecularHardness;
    stylingDataSpecular.opacity = _StylingSpecularOpacity;
    stylingDataSpecular.opacityFalloff = _StylingSpecularOpacityFalloff;

    
#ifndef _URP2D 
    StylingData stylingDataCastShadows;    
    
    stylingDataCastShadows.style = _CastShadowsStyle;
    stylingDataCastShadows.type = 1;
    stylingDataCastShadows.color = _StylingCastShadowsColor;
    stylingDataCastShadows.rotation = _StylingCastShadowsInitialDirection;
    stylingDataCastShadows.rotationBetweenCells = _StylingCastShadowsRotationBetweenCells;
    stylingDataCastShadows.density = _StylingCastShadowsDensity;
    stylingDataCastShadows.offset = _StylingCastShadowsHalftonesOffset;
    stylingDataCastShadows.size = _StylingCastShadowsThickness;
    stylingDataCastShadows.sizeControl = _StylingCastShadowsThicknessControl;
    stylingDataCastShadows.sizeFalloff = _StylingCastShadowsThicknessFalloff;
    stylingDataCastShadows.roundness = _StylingCastShadowsHalftonesRoundness;
    stylingDataCastShadows.roundnessFalloff = _StylingCastShadowsHalftonesRoundnessFalloff;
    stylingDataCastShadows.hardness = _StylingCastShadowsHardness;
    stylingDataCastShadows.opacity = _StylingCastShadowsOpacity;
    stylingDataCastShadows.opacityFalloff = _StylingCastShadowsOpacityFalloff;
#endif

    StylingData stylingDataRim;
    stylingDataRim.style = _RimStyle;
    stylingDataRim.type = 1;
    stylingDataRim.color = _StylingRimColor;
    stylingDataRim.rotation = _StylingRimRotation;
    stylingDataRim.density = _StylingRimDensity;
    stylingDataRim.offset = _StylingRimHalftonesOffset;
    stylingDataRim.size = _StylingRimThickness;
    stylingDataRim.sizeControl = _StylingRimThicknessControl;
    stylingDataRim.sizeFalloff = _StylingRimThicknessFalloff;
    stylingDataRim.roundness = _StylingRimHalftonesRoundness;
    stylingDataRim.roundnessFalloff = _StylingRimHalftonesRoundnessFalloff;
    stylingDataRim.hardness = _StylingRimHardness;
    stylingDataRim.opacity = _StylingRimOpacity;
    stylingDataRim.opacityFalloff = _StylingRimOpacityFalloff;

    
 
    
    PositionAndBlendingData positionAndBlendingDataShading;
            
    positionAndBlendingDataShading.blending = _StylingShadingBlending;
    positionAndBlendingDataShading.isInverted = _StylingShadingIsInverted;

    PositionAndBlendingData positionAndBlendingDataSpecular;
            
    positionAndBlendingDataSpecular.blending = _StylingSpecularBlending;
    positionAndBlendingDataSpecular.isInverted = _StylingSpecularIsInverted;
    
#ifndef _URP2D     
    PositionAndBlendingData positionAndBlendingDataCastShadows;
    positionAndBlendingDataCastShadows.blending = _StylingCastShadowsBlending;
    positionAndBlendingDataCastShadows.isInverted = _StylingCastShadowsIsInverted;   
#endif
    
    PositionAndBlendingData positionAndBlendingDataRim;
            
    positionAndBlendingDataRim.blending = _StylingRimBlending;
    positionAndBlendingDataRim.isInverted = _StylingRimIsInverted;



    StylingRandomData stylingRandomDataShading;
    stylingRandomDataShading.enableRandomizer = _EnableShadingRandomizer;
    stylingRandomDataShading.perlinNoiseSize = _ShadingNoise1Size;
    stylingRandomDataShading.perlinNoiseSeed = _ShadingNoise1Seed;
    stylingRandomDataShading.whiteNoiseSeed = _ShadingNoise2Seed;
    stylingRandomDataShading.noiseIntensity = _NoiseIntensity;
    stylingRandomDataShading.spacingRandomMode = _SpacingRandomMode;
    stylingRandomDataShading.spacingRandomIntensity = _SpacingRandomIntensity;
    stylingRandomDataShading.opacityRandomMode = _OpacityRandomMode;
    stylingRandomDataShading.opacityRandomIntensity = _OpacityRandomIntensity;
    stylingRandomDataShading.lengthRandomMode = _LengthRandomMode;
    stylingRandomDataShading.lengthRandomIntensity = _LengthRandomIntensity;
    stylingRandomDataShading.hardnessRandomMode = _HardnessRandomMode;
    stylingRandomDataShading.hardnessRandomIntensity = _HardnessRandomIntensity;
    stylingRandomDataShading.thicknessRandomMode = _ThicknessRandomMode;
    stylingRandomDataShading.thicknesshRandomIntensity = _ThicknesshRandomIntensity;
    
    
    
    StylingRandomData stylingRandomDataSpecular;
    stylingRandomDataSpecular.enableRandomizer = _EnableSpecularRandomizer;
    stylingRandomDataSpecular.perlinNoiseSize = _SpecularNoise1Size;
    stylingRandomDataSpecular.perlinNoiseSeed = _SpecularNoise1Seed;
    stylingRandomDataSpecular.whiteNoiseSeed = _SpecularNoise2Seed;
    stylingRandomDataSpecular.noiseIntensity = _SpecularNoiseIntensity;
    stylingRandomDataSpecular.spacingRandomMode = _SpecularSpacingRandomMode;
    stylingRandomDataSpecular.spacingRandomIntensity = _SpecularSpacingRandomIntensity;
    stylingRandomDataSpecular.opacityRandomMode = _SpecularOpacityRandomMode;
    stylingRandomDataSpecular.opacityRandomIntensity = _SpecularOpacityRandomIntensity;
    stylingRandomDataSpecular.lengthRandomMode = _SpecularLengthRandomMode;
    stylingRandomDataSpecular.lengthRandomIntensity = _SpecularLengthRandomIntensity;
    stylingRandomDataSpecular.hardnessRandomMode = _SpecularHardnessRandomMode;
    stylingRandomDataSpecular.hardnessRandomIntensity = _SpecularHardnessRandomIntensity;
    stylingRandomDataSpecular.thicknessRandomMode = _SpecularThicknessRandomMode;
    stylingRandomDataSpecular.thicknesshRandomIntensity = _SpecularThicknesshRandomIntensity;
    
#ifndef _URP2D 
    StylingRandomData stylingRandomDataCastShadows;
    stylingRandomDataCastShadows.enableRandomizer = _EnableCastShadowsRandomizer;
    stylingRandomDataCastShadows.perlinNoiseSize = _CastShadowsNoise1Size;
    stylingRandomDataCastShadows.perlinNoiseSeed = _CastShadowsNoise1Seed;
    stylingRandomDataCastShadows.whiteNoiseSeed = _CastShadowsNoise2Seed;
    stylingRandomDataCastShadows.noiseIntensity = _CastShadowsNoiseIntensity;
    stylingRandomDataCastShadows.spacingRandomMode = _CastShadowsSpacingRandomMode;
    stylingRandomDataCastShadows.spacingRandomIntensity = _CastShadowsSpacingRandomIntensity;
    stylingRandomDataCastShadows.opacityRandomMode = _CastShadowsOpacityRandomMode;
    stylingRandomDataCastShadows.opacityRandomIntensity = _CastShadowsOpacityRandomIntensity;
    stylingRandomDataCastShadows.lengthRandomMode = _CastShadowsLengthRandomMode;
    stylingRandomDataCastShadows.lengthRandomIntensity = _CastShadowsLengthRandomIntensity;
    stylingRandomDataCastShadows.hardnessRandomMode = _CastShadowsHardnessRandomMode;
    stylingRandomDataCastShadows.hardnessRandomIntensity = _CastShadowsHardnessRandomIntensity;
    stylingRandomDataCastShadows.thicknessRandomMode = _CastShadowsThicknessRandomMode;
    stylingRandomDataCastShadows.thicknesshRandomIntensity = _CastShadowsThicknesshRandomIntensity;
#endif

    StylingRandomData stylingRandomDataRim;
    stylingRandomDataRim.enableRandomizer = _EnableRimRandomizer;
    stylingRandomDataRim.perlinNoiseSize = _RimNoise1Size;
    stylingRandomDataRim.perlinNoiseSeed = _RimNoise1Seed;
    stylingRandomDataRim.whiteNoiseSeed = _RimNoise2Seed;
    stylingRandomDataRim.noiseIntensity = _RimNoiseIntensity;
    stylingRandomDataRim.spacingRandomMode = _RimSpacingRandomMode;
    stylingRandomDataRim.spacingRandomIntensity = _RimSpacingRandomIntensity;
    stylingRandomDataRim.opacityRandomMode = _RimOpacityRandomMode;
    stylingRandomDataRim.opacityRandomIntensity = _RimOpacityRandomIntensity;
    stylingRandomDataRim.lengthRandomMode = _RimLengthRandomMode;
    stylingRandomDataRim.lengthRandomIntensity = _RimLengthRandomIntensity;
    stylingRandomDataRim.hardnessRandomMode = _RimHardnessRandomMode;
    stylingRandomDataRim.hardnessRandomIntensity = _RimHardnessRandomIntensity;
    stylingRandomDataRim.thicknessRandomMode = _RimThicknessRandomMode;
    stylingRandomDataRim.thicknesshRandomIntensity = _RimThicknesshRandomIntensity;


#ifndef _URP2D   
    DoToonShading(
        #if _URP
            inputData,
            surface,
        #else
            o,
            gi,
            #if !_PASSFORWARDADD
            giInput,
            #endif
        #endif
            d,
        #if _URP
            #if UNITY_VERSION >= 202120
            normalTS,
            #endif
        #endif    
            albedo, _NumberOfCells, _CellTransitionSmoothness, _SumLightsBeforePosterization, _ShadingUseLightColors,
    
            uv, screenUV, _HatchingMap,
            
            _ShadingMode, _LightFunction,

            _EnableToonShading, _ShadingFunction,

            _GradientTex, _GradientTex_TexelSize, _GradientMode, _GradientBlending, _GradientBlendFactor,

            _EnableShadows, _CoreShadowColor, 
    
            _TerminatorPosition,
    
            _TerminatorWidth, _TerminatorSmoothness, _FormShadowColor,
            _EnableCastShadows, _CastShadowsStrength, _CastShadowsSmoothness, _CastShadowColorMode, _CastShadowColor,
            _ShadingAffectedByNormalMap,
    
            _EnableSpecular, _SpecularBlending, _SpecularColor, _SpecularSize, _SpecularSmoothness, _SpecularOpacity, _SpecularAffectedByNormalMap, _SpecularUseLightColors,
            
            _EnableRim, _RimBlending, _RimColor, _RimSize, _RimSmoothness, _RimOpacity, _RimAffectedArea, _RimAffectedByNormalMap,
            
    
            _EnableStyling, 
    
            generalStylingData, _HatchingAffectedByNormalMap, _EnableAntiAliasing,
    
            _EnableShadingStyling, 
            _StylingShadingSyncWithOtherStyling,
            _SyncWithLightPartitioning, _NumberOfCellsHatching, 
            _StylingTerminatorPosition,
            _StylingOvermodelingFactor,
            positionAndBlendingDataShading, uvSpaceDataShading, stylingDataShading, stylingRandomDataShading,
    
            _EnableCastShadowsStyling,
            _StylingCastShadowsSyncWithOtherStyling,
            _CastShadowsNumberOfCellsHatching, _StylingCastShadowsSmoothness, 
            positionAndBlendingDataCastShadows, uvSpaceDataCastShadows, stylingDataCastShadows, stylingRandomDataCastShadows,
    
            _EnableSpecularStyling,
            _SyncWithSpecular, _StylingSpecularSize, _StylingSpecularSmoothness, _StylingSpecularCutOutShading, _StylingSpecularUseLightColors,
            _StylingSpecularSyncWithOtherStyling,
            positionAndBlendingDataSpecular, uvSpaceDataSpecular, stylingDataSpecular, stylingRandomDataSpecular,
    
            _EnableRimStyling,
            _SyncWithRim, _StylingRimSize, _StylingRimSmoothness, _StylingRimAffectedArea, 
            _StylingRimSyncWithOtherStyling,
            positionAndBlendingDataRim, uvSpaceDataRim, stylingDataRim, stylingRandomDataRim,


            _NoiseMap1, _NoiseMap2, _NoiseTex2_TexelSize,   
            
            pureNormal);
    
#else 
    
    DoToonShading( 
   
            worldPosition, light2DData, emission, originScreenPos,
    
            albedo, _CellMethod, _NumberOfCells, _CellTransitionSmoothness, _ShadingUseLightColors,
    
            uv, screenUV,
            _MainTex,
            
            _ShadingMode, 

            _EnableToonShading, _ShadingFunction,
    
            _EnableMainTexPosterization,

            _GradientTex, _GradientBlending, _GradientBlendFactor,

            _EnableShadows, _CoreShadowColor, 

            _ShadingAffectedByNormalMap,
        
    
            _EnableSpecular, _SpecularBlending, _SpecularColor, _SpecularSize, _SpecularSmoothness, _SpecularOpacity, _SpecularAffectedByNormalMap, _SpecularUseLightColors,
            
            _EnableRim, _RimBlending, _RimColor, _RimSize, _RimSmoothness, _RimOpacity, _RimAffectedArea, _RimAffectedByNormalMap,
            
    
            _EnableStyling,
    
            _HatchingAffectedByNormalMap, _EnableAntiAliasing,
    
            _EnableShadingStyling,
            _StylingShadingSyncWithOtherStyling,
            _SyncWithLightPartitioning, _NumberOfCellsHatching, _StylingOvermodelingFactor,
            positionAndBlendingDataShading, uvSpaceDataShading, stylingDataShading, stylingRandomDataShading,
    
            _EnableSpecularStyling,
            _SyncWithSpecular, _StylingSpecularSize, _StylingSpecularSmoothness, _StylingSpecularCutOutShading, _StylingSpecularUseLightColors,
            _StylingSpecularSyncWithOtherStyling,
            positionAndBlendingDataSpecular, uvSpaceDataSpecular, stylingDataSpecular, stylingRandomDataSpecular,
    
            _EnableRimStyling,
            _SyncWithRim, _StylingRimSize, _StylingRimSmoothness, _StylingRimAffectedArea,
            _StylingRimSyncWithOtherStyling,
            positionAndBlendingDataRim, uvSpaceDataRim, stylingDataRim, stylingRandomDataRim,


            _NoiseTex1, _NoiseTex2,
            
            pureNormal);
#endif
}










#endif
