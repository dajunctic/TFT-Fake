#ifndef ALLIN13DSHADER_VERTEX_EFFECTS
#define ALLIN13DSHADER_VERTEX_EFFECTS

#ifdef _VERTEX_SHAKE_ON
float3 VertexShake(float3 vertexPos, float3 shaderTime)
{
	float3 res = vertexPos;
    
	float3 speedOffset = float3(1.0f, 1.13f, 1.07f) * ACCESS_PROP_FLOAT(_ShakeSpeedMult);
	float3 displacement = sin(shaderTime.y * ACCESS_PROP_FLOAT4(_ShakeSpeed).xyz * speedOffset) * ACCESS_PROP_FLOAT4(_ShakeMaxDisplacement).xyz;
	displacement *= ACCESS_PROP_FLOAT(_ShakeBlend);
    
	res += displacement;

	return res;
}
#endif

#ifdef _VERTEX_INFLATE_ON
float3 VertexInflate(float3 vertexPos, float3 normalOS, float3 shaderTime)
{
	float3 res = vertexPos;

	float inflateValue = lerp(ACCESS_PROP_FLOAT(_MinInflate), ACCESS_PROP_FLOAT(_MaxInflate), ACCESS_PROP_FLOAT(_InflateBlend));
	res += normalOS * inflateValue;
	
	return res;
}
#endif

#ifdef _VERTEX_DISTORTION_ON
float3 VertexDistortion(float3 vertexPos, float3 normalOS, float3 shaderTime)
{
	float3 res = vertexPos;
	
	float noisePower = 1.0;

	float2 noiseUV = SIMPLE_CUSTOM_TRANSFORM_TEX(vertexPos.xy, _VertexDistortionNoiseTex);
	float4 correctedNoiseUV = float4(noiseUV.x, noiseUV.y, 0, 0);
	
	correctedNoiseUV.x += frac(shaderTime.x * ACCESS_PROP_FLOAT2(_VertexDistortionNoiseSpeed).x);
	correctedNoiseUV.y += frac(shaderTime.x * ACCESS_PROP_FLOAT2(_VertexDistortionNoiseSpeed).y);

	noisePower = SAMPLE_TEX2D_LOD(_VertexDistortionNoiseTex, correctedNoiseUV).r;

	res += normalOS * noisePower * ACCESS_PROP_FLOAT(_VertexDistortionAmount);
	
	return res;
}
#endif

#ifdef _VOXELIZE_ON
float3 VertexVoxel(float3 vertexPos)
{
	float3 voxelizedPosition = round(vertexPos * ACCESS_PROP_FLOAT(_VoxelSize)) / ACCESS_PROP_FLOAT(_VoxelSize);
	return lerp(vertexPos, voxelizedPosition, ACCESS_PROP_FLOAT(_VoxelBlend));
}
#endif

#ifdef _GLITCH_ON
float3 Glitch(float3 vertexPos, float3 shaderTime)
{
	float3 res = vertexPos;

	float3 glitchDir = GetDirOSFloat3(ACCESS_PROP_FLOAT3(_GlitchOffset));

	float3 scale = float3(length(OBJECT_TO_WORLD_MATRIX[0].xyz),
					length(OBJECT_TO_WORLD_MATRIX[1].xyz),
					length(OBJECT_TO_WORLD_MATRIX[2].xyz));
	float pos = ACCESS_PROP_FLOAT(_GlitchWorldSpace) ? mul(OBJECT_TO_WORLD_MATRIX, float4(vertexPos, 1.0)).y : vertexPos.y;
	float time = shaderTime.y * ACCESS_PROP_FLOAT(_GlitchSpeed);
	
	// Add high frequency noise to the main UV
    float2 glitchUV = float2(pos * ACCESS_PROP_FLOAT(_GlitchTiling) + time, time * 0.89);
    float mainNoise = noise2D(glitchUV);
    float fastNoise = noise2D(glitchUV * 2.5 + float2(time * 3.7, 0));
    mainNoise = mainNoise * 0.6 + fastNoise * 0.4;

	float2 periodicUV = float2(time * 0.5, time * 0.14);
    float periodicNoise = saturate(noise2D(periodicUV) + 0.1);
                
    float detailNoise = noise2D(float2(20.0 * glitchUV.x, glitchUV.y));
                
    float glitchValue = (2.0 * mainNoise - 1.0) * periodicNoise;
    glitchValue += glitchValue * lerp(0, saturate(2.0 * detailNoise - 1.0), 2.0);

	res += (glitchDir / scale) * glitchValue * ACCESS_PROP_FLOAT(_GlitchAmount);

	return res;
}
#endif

#ifdef _RECALCULATE_NORMALS_ON
float3 RecalculateNormal(float3 originalVertex, float3 normal, float3 tangent)
{
	return normal;
}
#endif

#ifdef _WIND_ON
float3 Wind(float3 vertexPos, float3 shaderTime)
{
	float3 res = vertexPos;

	float windMask = 1.0;
	#ifdef _USE_WIND_VERTICAL_MASK
	windMask = RemapFloat(vertexPos.y, ACCESS_PROP_FLOAT(_WindVerticalMaskMinY), ACCESS_PROP_FLOAT(_WindVerticalMaskMaxY), 0, 1);
	#endif

	float3 vertexWS = GetPositionWS(float4(vertexPos, 1.0));
	
	float2 windNoiseUV = float2(vertexWS.x, vertexWS.z) / global_windWorldSize;
	windNoiseUV += global_noiseSpeed * shaderTime.x; 
	windNoiseUV = frac(windNoiseUV);

	float3 windNoise = SAMPLE_TEX2D_LOD(global_windNoiseTex, float4(windNoiseUV.x, windNoiseUV.y, 0, 0));
	windNoise = RemapFloat3(windNoise, 0, 1, global_minWindValue, global_maxWindValue);
	
	if(global_useWindDir)
	{
		windNoise = (windNoise.x + windNoise.y + windNoise.z) / 3;
	}

	float3 windDisplacement = windNoise * global_windForce * windMask * global_windDir;
	windDisplacement = lerp(0, windDisplacement, ACCESS_PROP_FLOAT(_WindAttenuation));

	windDisplacement.y = 0;
	
	
	
	res += windDisplacement;

	return res;
}
#endif

#endif