Shader "DTT/Projector/Arc Projector"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Base Color", Color) = (1,1,1,1)
        // The normalized arc angle.
        _FillColor("Secondary Color", Color) = (1, 1 ,1 ,0.5)
        _FillProgress("Progress", Range(0, 1)) = 0.5
        _Arc("Arc", Range(0, 1)) = 0
        // The normalized angle offset.
        _Angle("Angle", Range(0, 1)) = 0
        _Origin ("Fill Origin", int) = 0
    }
    
    SubShader
    {
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		LOD 100    	 
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
        AlphaTest Greater 0

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            #pragma multi_compile_instancing
            #include "UnityCG.cginc" 

            // Fields
            sampler2D _MainTex;
            fixed4 _Color;
            fixed4 _FillColor;
            float _FillProgress;
            float _Arc;
            float _Angle;
            int _Origin;
            float4x4 unity_Projector;

            // Vertex Data
            struct vertex
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
            };

            // Vertex to Fragment shader data
            struct v2f
            {
                float4 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 pos : SV_POSITION;
            };  
     
            // Vertex shader
            v2f vert (vertex v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.pos);
                o.uv = mul(unity_Projector, v.pos);
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }

            // Fragment shader
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 baseTex = tex2Dproj(_MainTex, UNITY_PROJ_COORD(i.uv)) * _Color;
                fixed4 fillTex = tex2Dproj(_MainTex, UNITY_PROJ_COORD(i.uv)) * _FillColor;

                float dist = length(i.uv - float2(0.5, 0.5)) * 2;
                
                // Offset so origin is centre.
                float2 offset = i.uv.xy - 0.5f;

                // The angle of the current uv.
                const float arcAlpha = offset.x == 0 || offset.y == 0 ? 0 : atan2(offset.y, offset.x);

                // The angle in 0..1 range.
                const float normalizedAngle = fmod((arcAlpha / UNITY_PI + 1) / 2 + _Angle + _Arc / 2, 1);

                float cutoff = 0;
                switch (_Origin)
                {
                case 0:
                    cutoff = dist > _FillProgress  ? 1 : 0;
                    break;
                case 1:
                    cutoff = 1-dist > _FillProgress ? 1 : 0;
                    break;
                case 2:
                    cutoff = normalizedAngle > 1- (1-_FillProgress) * (1-_Arc) ? 1 : 0;
                    break;
                case 3:
                    cutoff = normalizedAngle < 1- _FillProgress * (1-_Arc) ? 1 : 0;
                    break;
                default:
                    cutoff = dist > _FillProgress ? 1 : 0;
                    break;
                }
                
                fixed4 color = normalizedAngle > _Arc ? cutoff == 0 ? fillTex : baseTex : 0;

                UNITY_APPLY_FOG_COLOR(i.fogCoord, color, fixed4(0,0,0,0));
                return color;
            }
            ENDCG
        }
    }
}
