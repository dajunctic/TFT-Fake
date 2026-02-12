#ifndef ALLIN1SPRITESHADERSRP_VERTEXPASS
#define ALLIN1SPRITESHADERSRP_VERTEXPASS

v2f vert(appdata v)
{
	#if RECTSIZE_ON
		v.vertex.xyz += (v.vertex.xyz * (_RectSize - 1.0));
	#endif

		v2f o;
		UNITY_SETUP_INSTANCE_ID(v);
		UNITY_TRANSFER_INSTANCE_ID(v, o);
		UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
            	
	#if BILBOARD_ON
		half3 camRight = mul((half3x3)unity_CameraToWorld, half3(1,0,0));
		half3 camUp = half3(0,1,0);
	#if BILBOARDY_ON
		camUp = mul((half3x3)unity_CameraToWorld, half3(0,1,0));
	#endif
		half3 localPos = v.vertex.x * camRight + v.vertex.y * camUp;
		o.vertex = TransformObjectToHClip(half4(localPos, 1).xyz);
	#else
		o.vertex = TransformObjectToHClip(v.vertex.xyz);
	#endif
		o.uv = /*TRANSFORM_TEX(v.uv, _MainTex)*/v.uv * _MainTex_ScaleAndTiling.xy + _MainTex_ScaleAndTiling.zw;
		o.color = v.color;

		half2 center = half2(0.5, 0.5);
	#if ATLAS_ON
		center = half2((_MaxXUV + _MinXUV) / 2.0, (_MaxYUV + _MinYUV) / 2.0);
	#endif

	#if POLARUV_ON
		o.uv = v.uv - center;
	#endif

	#if ROTATEUV_ON
		half2 uvC = v.uv;
		half cosAngle = cos(_RotateUvAmount);
		half sinAngle = sin(_RotateUvAmount);
		half2x2 rot = half2x2(cosAngle, -sinAngle, sinAngle, cosAngle);
		uvC -= center;
		o.uv = mul(rot, uvC);
		o.uv += center;
	#endif

	#if OUTTEX_ON
		o.uvOutTex = CUSTOM_TRANSFORM_TEX(v.uv, _OutlineTex_ScaleAndTiling);
	#endif

	#if OUTDIST_ON
		o.uvOutDistTex = CUSTOM_TRANSFORM_TEX(v.uv, _OutlineDistortTex_ScaleAndTiling);
	#endif

	#if DISTORT_ON
		o.uvDistTex = CUSTOM_TRANSFORM_TEX(v.uv, _DistortTex_ScaleAndTiling);
	#endif

	#if FOG_ON
		UNITY_TRANSFER_FOG(o,o.vertex);
	#endif

	return o;
}

#endif