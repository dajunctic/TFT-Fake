#ifndef ALLIN13DSHADER_BASE_PASS_INCLUDED
#define ALLIN13DSHADER_BASE_PASS_INCLUDED

FragmentData BasicVertex(VertexData v)
{
	FragmentData o;
	
	UNITY_SETUP_INSTANCE_ID(v);
    UNITY_TRANSFER_INSTANCE_ID(v, o);
	UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o); 

#ifdef _SPHERIZE_NORMALS_ON
	float3 normalOS = normalize(v.vertex);
#else
	float3 normalOS = v.normal;
#endif

	o.interpolator_01 = 0; 

#ifdef _USE_CUSTOM_TIME
	SHADER_TIME(o) = allIn13DShader_globalTime.xyz + ACCESS_PROP_FLOAT(_TimingSeed);
#else
	SHADER_TIME(o) = _Time.xyz + ACCESS_PROP_FLOAT(_TimingSeed);
#endif

	float3 originalVertex = v.vertex.xyz;
	
	v.vertex = ApplyVertexEffects(v.vertex, normalOS, SHADER_TIME(o));
#ifdef _RECALCULATE_NORMALS_ON
	float3 tangentNeighbour = originalVertex + normalize(v.tangent.xyz) * RECALCULATE_NORMAL_OFFSET;
	float3 bitangent = normalize(cross(v.normal, v.tangent.xyz));
	float3 bitangentNeightbour = originalVertex + bitangent * RECALCULATE_NORMAL_OFFSET;

	tangentNeighbour = ApplyVertexEffects(float4(tangentNeighbour, 1.0), normalOS, SHADER_TIME(o)).xyz;
	bitangentNeightbour = ApplyVertexEffects(float4(bitangentNeightbour, 1.0), normalOS, SHADER_TIME(o)).xyz;

	float3 correctedTangent = normalize(tangentNeighbour - v.vertex.xyz);
	float3 correctedBitangent = normalize(bitangentNeightbour - v.vertex.xyz);

	v.tangent = float4(correctedTangent, v.tangent.w);
	normalOS = normalize(cross(correctedTangent, correctedBitangent));
#endif


	POSITION_OS(o) = v.vertex.xyz;
	NORMAL_OS(o) = normalOS;
	POSITION_WS(o) = GetPositionWS(v.vertex);

	o.normalWS = GetNormalWS(normalOS);

	o.pos = OBJECT_TO_CLIP_SPACE(v);

	
	
	VIEWDIR_WS(o) = GetViewDirWS(POSITION_WS(o));
	
	float4 projPos = 0;
#ifdef REQUIRE_SCENE_DEPTH
	o.projPos = ComputeScreenPos(o.pos);
	o.projPos.z = ComputeEyeDepth(POSITION_WS(o));
	
	projPos = o.projPos;
#endif

	float2 uv = v.uv;
	uv = ApplyUVEffects_VertexStage(uv, POSITION_WS(o), projPos, SHADER_TIME(o));
	
	UV_DIFF(o) = uv - v.uv;

	SCALED_MAIN_UV(o) = CUSTOM_TRANSFORM_TEX(v.uv, UV_DIFF(o), _MainTex);
	RAW_MAIN_UV(o) = uv;
	
	o.interpolator_02 = 0;


#if defined(REQUIRE_TANGENT_WS)
	float3 tangentWS = GetDirWS(float4(v.tangent.xyz, 0));
	float3 bitangentWS = GetBitangentWS(v.tangent, tangentWS, o.normalWS);
	INIT_T_SPACE(o.normalWS)

	UV_NORMAL_MAP(o) = CUSTOM_TRANSFORM_TEX(v.uv, UV_DIFF(o), _NormalMap);
#endif

	ShadowCoordStruct shadowCoordStruct = GetShadowCoords(v, o.pos, POSITION_WS(o));
	o._ShadowCoord = shadowCoordStruct._ShadowCoord;

#ifdef LIGHTMAP_ON
	UV_LIGHTMAP(o) = v.uvLightmap * unity_LightmapST.xy + unity_LightmapST.zw;
#else
	UV_LIGHTMAP(o) = 0;
#endif
	FOGCOORD(o) = GetFogFactor(o.pos);

#ifdef _EMISSION_ON
	UV_EMISSION_MAP(o) = SIMPLE_CUSTOM_TRANSFORM_TEX(v.uv, _EmissionMap); 
#endif


	//Vertex Color initialization
	VERTEX_COLOR_R(o) = v.vertexColor.r;	
	VERTEX_COLOR_G(o) = v.vertexColor.g;
	VERTEX_COLOR_B(o) = v.vertexColor.b;
	VERTEX_COLOR_A(o) = v.vertexColor.a;

	return o;
}

float4 BasicFragment(
	FragmentData i
	#ifdef _WRITE_RENDERING_LAYERS
		#if UNITY_VERSION >= 60020000
			, out uint outRenderingLayers : SV_Target1
		#else
			, out float4 outRenderingLayers : SV_Target1
		#endif
	#endif
	) : SV_Target
{
	UNITY_SETUP_INSTANCE_ID(i);
	UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);


	AllIn1DecalData decalData;
	INIT_DECAL_DATA(decalData);

#ifdef ALLIN1_DECALS_READY_TO_USE
	ConfigureDecalData(decalData, i.pos);
#endif

	EffectsData data = CalculateEffectsData(i, decalData);
	
	data = ApplyUVEffects_FragmentStage(data);


	data.normalWS = GetNormalWS(data, i, decalData);



#ifdef _NORMAL_MAP_ON
	data.bitangentWS = normalize(cross(data.normalWS, data.tangentWS));
#endif

	float4 objectColor = GetBaseColor(data);

	float3 normalOS = data.normalOS;
	float3 normalWS = data.normalWS;
	float3 viewDirWS = data.viewDirWS;

	float sceneDepthDiff = 1.0;
	float normalizedDepth = 0;
#ifdef REQUIRE_SCENE_DEPTH
	sceneDepthDiff = GetSceneDepthDiff(i.projPos);
	normalizedDepth = GetNormalizedDepth(i.projPos);
#endif

	float camDistance = 0;
#ifdef REQUIRE_CAM_DISTANCE
	camDistance = distance(POSITION_WS(i), _WorldSpaceCameraPos);
#endif
	
	objectColor *= ACCESS_PROP_FLOAT4(_Color);
	objectColor = ApplyColorEffectsBeforeLighting(objectColor, data);

#ifdef ALLIN1_DECALS_READY_TO_USE
	objectColor.rgb = objectColor.rgb * decalData.baseColor.a + decalData.baseColor.rgb;
#endif

	float4 col = objectColor;
	
	float3 lightmap = GetLightmap(UV_LIGHTMAP(i), data);
#if defined(_AFFECTED_BY_LIGHTMAPS_ON) && defined(_LIGHTMAP_COLOR_CORRECTION_ON)
	lightmap = LightmapColorCorrection(lightmap);
#endif


	float3 ambientColor = GetAmbientColor(data);

#ifdef LIGHT_ON
	float3 mainLightColor = GetMainLightColorRGB();
	float3 mainLightDir = GetMainLightDir(POSITION_WS(i));

	col.rgb = CalculateLighting(
		POSITION_WS(i),
		normalWS, data.tangentWS, data.bitangentWS,
		objectColor.rgb, objectColor.a,
		1.0, lightmap, ambientColor, viewDirWS, 
		SCALED_MAIN_UV(i), mainLightColor, mainLightDir, i, 1.0, data);
#else
	float2 ssaoFactor = GetSSAO(data.normalizedScreenSpaceUV.xy, objectColor.a);
	col.rgb = IndirectLighting_Basic(objectColor.rgb, lightmap, ssaoFactor, data);
#endif
	
#ifdef _EMISSION_ON
	float2 emissionUV = SIMPLE_CUSTOM_TRANSFORM_TEX(MAIN_UV(data), _EmissionMap);
	float4 emissionMapCol = SAMPLE_TEX2D(_EmissionMap, emissionUV);
	float4 emissionCol = emissionMapCol * ACCESS_PROP_FLOAT4(_EmissionColor) * ACCESS_PROP_FLOAT(_EmissionSelfGlow);

	col.rgb += emissionCol.rgb;
#endif
	
	col = ApplyAlphaEffects(col, SCALED_MAIN_UV(i), sceneDepthDiff, data.camDistance, data.projPos);

#ifdef _ALPHA_CUTOFF_ON
	clip((col.a - ACCESS_PROP_FLOAT(_AlphaCutoffValue)) - 0.001);
#endif	

	col = ApplyColorEffectsAfterLighting(col, data);
	col.a *= ACCESS_PROP_FLOAT(_GeneralAlpha);
	
#ifdef _FOG_ON
	col = CustomMixFog(FOGCOORD(i), col);
#endif

#if defined(_WRITE_RENDERING_LAYERS)
	#if UNITY_VERSION >= 60020000
		outRenderingLayers = EncodeMeshRenderingLayer();
	#else
		uint renderingLayers = GetMeshRenderingLayer();
		outRenderingLayers = float4(EncodeMeshRenderingLayer(renderingLayers), 0, 0, 0);
	#endif
#endif

	return col;
}

#endif /*ALLIN13DSHADER_BASE_PASS_INCLUDED*/