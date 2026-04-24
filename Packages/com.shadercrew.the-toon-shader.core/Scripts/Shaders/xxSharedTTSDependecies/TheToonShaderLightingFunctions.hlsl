#ifndef THETOONSHADER_LIGHTING_FUNCTION
#define THETOONSHADER_LIGHTING_FUNCTION


struct ToonShadingData
{
    half enableToonShading;
    float3 normalWS;
    float3 normalWSNoMap;
    float cellTransitionSmoothness;
    half numberOfCells;
    float specularEdgeSmoothness;
    half shadingAffectByNormalMap;
    half specularAffectedByNormalMap;
    
};
float Posterize(float llllllllllllll0, ToonShadingData toonShadingData)
{
    if (toonShadingData.enableToonShading == 1)
    {
        float ll0 = (1.0 / toonShadingData.numberOfCells) * toonShadingData.cellTransitionSmoothness; 
        half lll0 = ceil(llllllllllllll0 * toonShadingData.numberOfCells);
        half llll0 = lll0 / toonShadingData.numberOfCells;
        half lllll0 = max(0, lll0 - 1) / toonShadingData.numberOfCells;
        half llllll0 = max(llll0 * smoothstep(lllll0, lllll0 + ll0, llllllllllllll0), lllll0);
        return lerp(llllll0, llllllllllllll0, toonShadingData.cellTransitionSmoothness);
    }
    else
    {
        return llllllllllllll0;
    }
}
float3 Posterize(float3 llllllllllllll0, ToonShadingData toonShadingData)
{
    if (toonShadingData.enableToonShading == 1)
    {
        half ll0 = (1.0 / toonShadingData.numberOfCells) * toonShadingData.cellTransitionSmoothness; 
        half3 lll0 = ceil(llllllllllllll0 * toonShadingData.numberOfCells);
        half3 llll0 = lll0  / toonShadingData.numberOfCells;
        half3 lllll0 = max(0, lll0 - 1.0) / toonShadingData.numberOfCells;
        half3 llllll0 = max(llll0 * smoothstep(lllll0, lllll0 + ll0, llllllllllllll0), lllll0);
        return lerp(llllll0, llllllllllllll0, toonShadingData.cellTransitionSmoothness);
    }
    else
    {
        return llllllllllllll0;
    }
}
float3 PosterizeMulti(
float3 llllllllllllll0, ToonShadingData toonShadingData, float lllllllllllllll0
)
{
    if (toonShadingData.enableToonShading == 1)
    {
        half ll0 = (1.0 / toonShadingData.numberOfCells) * toonShadingData.cellTransitionSmoothness; 
        half3 lllllllllllllllll0 = llllllllllllll0 * toonShadingData.numberOfCells;
        half3 lll0 = 0;
        if (lllllllllllllll0 == 0)
        {
            lll0 = ceil(lllllllllllllllll0);           
        }
        else
        {
            lll0 = floor(lllllllllllllllll0);
        }
        half3 llll0 = lll0 / toonShadingData.numberOfCells;
        half3 lllll0 = max(0, lll0 - 1.0) / toonShadingData.numberOfCells;
        half3 llllll0 = max(llll0 * smoothstep(lllll0, lllll0 + ll0, llllllllllllll0), lllll0);
        return lerp(llllll0, llllllllllllll0, toonShadingData.cellTransitionSmoothness);
    }
    else
    {
        return llllllllllllll0;
    }
}
float CalculateCellShadingPartitioning(half3 direction, ToonShadingData toonShadingData)
{
    if (toonShadingData.enableToonShading == 1)
    {
        half3 lllllllllllllllllllllll0;
        if (toonShadingData.shadingAffectByNormalMap == 0)
        {
            lllllllllllllllllllllll0 = toonShadingData.normalWSNoMap;
        }
        else
        {
            lllllllllllllllllllllll0 = toonShadingData.normalWS;
        }
        float llllllllllllllllllllllll0 = saturate(dot(lllllllllllllllllllllll0, direction));
        return Posterize(llllllllllllllllllllllll0, toonShadingData);
    }
    else
    {
        return saturate(dot(toonShadingData.normalWS, direction));
    }
}
half3 PosterizeShifted(half3 llllllllllllll0, ToonShadingData toonShadingData)
{
    if (toonShadingData.enableToonShading == 1)
    {
        half3 llllllllllllllllllllllllll0 = llllllllllllll0;
        float ll0 = (1.0 / toonShadingData.numberOfCells) * toonShadingData.cellTransitionSmoothness; 
        float llllllllllllllllllllllllllll0 = (1 / (toonShadingData.numberOfCells + 1)) + ll0 * ((0.25 / toonShadingData.numberOfCells) - (1 / (toonShadingData.numberOfCells + 1)));
        float lllllllllllllllllllllllllllll0 = 1 + (1 / toonShadingData.numberOfCells);
        llllllllllllll0 = (llllllllllllll0 - llllllllllllllllllllllllllll0) * lllllllllllllllllllllllllllll0;
        half3 llll0 = ceil(llllllllllllll0 * toonShadingData.numberOfCells) / toonShadingData.numberOfCells;
        half3 lllll0 = max(0, ceil(llllllllllllll0 * toonShadingData.numberOfCells) - 1) / toonShadingData.numberOfCells;
        llllllllllllll0 = max(llll0 * smoothstep(lllll0, lllll0 + ll0, llllllllllllll0), lllll0);
        llllllllllllll0 = lerp(llllllllllllll0, llllllllllllllllllllllllll0, toonShadingData.cellTransitionSmoothness);
    }
    return llllllllllllll0;
}


#endif
