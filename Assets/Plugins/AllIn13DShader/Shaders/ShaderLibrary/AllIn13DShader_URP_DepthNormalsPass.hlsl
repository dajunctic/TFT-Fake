#ifndef ALLIN13DSHADER_URP_DEPTH_NORMALS_PASS_INCLUDED
#define ALLIN13DSHADER_URP_DEPTH_NORMALS_PASS_INCLUDED

	struct DepthNormalsVertexData
	{
		float4 positionOS	: POSITION;
		float3 normal		: NORMAL;
		float2 uv			: TEXCOORD0;
		float4 tangent		: TANGENT;
		UNITY_VERTEX_INPUT_INSTANCE_ID
	};

	struct DepthNormalsFragmentData
	{
		float4 positionCS		: SV_POSITION;
		float3 normalWS			: TEXCOORD1;
		float4 mainUV			: TEXCOORD2;
		float4 interpolator_01	: TEXCOORD3;
		float4 interpolator_02	: TEXCOORD4;

#ifdef REQUIRE_SCENE_DEPTH
		float4 projPos : TEXCOORD5;
#endif

		float3 positionWS : TEXCOORD6;

#ifdef REQUIRE_TANGENT_WS
		T_SPACE_PROPERTIES(7, 8, 9)
#endif

		UNITY_VERTEX_INPUT_INSTANCE_ID
		UNITY_VERTEX_OUTPUT_STEREO
	};

	DepthNormalsFragmentData DepthNormalsVertex(DepthNormalsVertexData input)
	{
		DepthNormalsFragmentData o;

		UNITY_SETUP_INSTANCE_ID(input);
		UNITY_TRANSFER_INSTANCE_ID(input, o);
		UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o); 

		
		o.interpolator_01 = float4(0, 0, 0, 0);
		o.interpolator_02 = float4(0, 0, 0, 0);
		o.mainUV = float4(0, 0, 0, 0);
		SCALED_MAIN_UV(o) = CUSTOM_TRANSFORM_TEX(input.uv, UV_DIFF(o), _MainTex); 

#ifdef _SPHERIZE_NORMALS_ON
		float3 normalOS = normalize(input.positionOS);
#else
		float3 normalOS = input.normal;
#endif
		
#ifdef _USE_CUSTOM_TIME
		float3 shaderTime = allIn13DShader_globalTime.xyz + ACCESS_PROP_FLOAT(_TimingSeed);
#else
		float3 shaderTime = _Time.xyz + ACCESS_PROP_FLOAT(_TimingSeed);
#endif
		
		input.positionOS = ApplyVertexEffects(input.positionOS, normalOS, shaderTime);
		
		o.positionCS = TransformObjectToHClip(input.positionOS.xyz);
		o.normalWS = GetNormalWS(input.normal);
		o.positionWS = GetPositionWS(input.positionOS);

#if defined(REQUIRE_TANGENT_WS)
		float3 tangentWS = GetDirWS(float4(input.tangent.xyz, 0));
		float3 bitangentWS = GetBitangentWS(input.tangent, tangentWS, o.normalWS);
		INIT_T_SPACE(o.normalWS)

		UV_NORMAL_MAP(o) = CUSTOM_TRANSFORM_TEX(input.uv, UV_DIFF(o), _NormalMap);
#endif


	
		float4 projPos = 0;
#ifdef REQUIRE_SCENE_DEPTH
		o.projPos = ComputeScreenPos(o.positionCS);

		float3 positionWS = GetPositionWS(input.positionOS);
		o.projPos.z = ComputeEyeDepth(positionWS);
		projPos = o.projPos;
#endif

		return o;
	}

	float4 DepthNormalsFragment(
		DepthNormalsFragmentData input
		#ifdef _WRITE_RENDERING_LAYERS
			#if UNITY_VERSION >= 60020000
				, out uint outRenderingLayers : SV_Target1
			#else
				, out float4 outRenderingLayers : SV_Target1
			#endif
		#endif
	) : SV_TARGET
	{
		UNITY_SETUP_INSTANCE_ID(input);
		UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

		float4 baseColor = SAMPLE_TEX2D(_MainTex, input.mainUV.xy);

		float3 normalWS = normalize(input.normalWS);
		float4 res = float4(normalWS, 1.0);

		#ifdef _NORMAL_MAP_ON
			float3 tangentWS = normalize(TANGENT_WS(input));
			float3 bitangentWS = normalize(cross(normalWS, tangentWS));

			float4 sampledNormal = SAMPLE_TEX2D(_NormalMap, input.mainUV);
		
			float3 tnormal = UnpackNormal(sampledNormal);
			tnormal.xy *= ACCESS_PROP_FLOAT(_NormalStrength); 

			res.x = dot(input.tspace0, tnormal);
			res.y = dot(input.tspace1, tnormal);
			res.z = dot(input.tspace2, tnormal);
    
			res = normalize(res);
			res.w = 0;
		#endif

		float camDistance = distance(input.positionWS, _WorldSpaceCameraPos); 
		float4 screenPos = ComputeScreenPos(input.positionCS); 
		baseColor = ApplyAlphaEffects(baseColor, SCALED_MAIN_UV(input), 1.0, camDistance, screenPos);

		float alphaClip = saturate(baseColor.a);
		#ifdef _ALPHA_CUTOFF_ON
			clip((alphaClip - ACCESS_PROP_FLOAT(_AlphaCutoffValue)) - 0.01);
		#endif

		#if defined(_WRITE_RENDERING_LAYERS)
			#if UNITY_VERSION >= 60020000
				outRenderingLayers = EncodeMeshRenderingLayer();
			#else
				uint renderingLayers = GetMeshRenderingLayer();
				outRenderingLayers = float4(EncodeMeshRenderingLayer(renderingLayers), 0, 0, 0);
			#endif
		#endif

		return res;
	}

#endif