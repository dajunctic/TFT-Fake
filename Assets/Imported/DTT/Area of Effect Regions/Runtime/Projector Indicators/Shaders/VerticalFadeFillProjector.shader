Shader "DTT/Projector/Vertical Fill And Fade Projector"
{
    
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Base Color", Color) = (1,1,1,1)
        _FillColor ("Fill Color", Color) = (1,1,1,1)
        _FadeAmount ("Fade Amount", Range(0,1)) = 0
        _FillProgress ("Fill Progress", Range(0, 1)) = 1
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
            float _FadeAmount;
            float _FillProgress;
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

            float InverseLerp(float from, float to, float value)
            {
                return (value - from) / (to - from);
            }

            float Remap(float from, float fromMin, float fromMax, float toMin,  float toMax)
            {
                float fromAbs  =  from - fromMin;
                float fromMaxAbs = fromMax - fromMin;      
       
                float normal = fromAbs / fromMaxAbs;
 
                float toMaxAbs = toMax - toMin;
                float toAbs = toMaxAbs * normal;
 
                float to = toAbs + toMin;
       
                return to;
            }
     
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
                fixed4 mainTex = tex2Dproj(_MainTex, UNITY_PROJ_COORD(i.uv)) * _Color;
                fixed4 fillTex = tex2Dproj(_MainTex, UNITY_PROJ_COORD(i.uv)) * _FillColor;

                float lowerAlphaOffset = -1 + _FadeAmount * 2;
                float upperAlphaOffset = lowerAlphaOffset + 1;

                float alpha = Remap(i.uv.y, lowerAlphaOffset, upperAlphaOffset, 0, 1);

                fixed4 color = {1,1,1,1};
                switch (_Origin)
                {
                case 0 :
                    color = i.uv.y < _FillProgress ? fillTex : mainTex;
                    color.a = i.uv.y < _FillProgress ? alpha * fillTex.a : alpha * mainTex.a;
                    break;
                case 1:
                    color = i.uv.y > _FillProgress ? fillTex : mainTex;
                    color.a = i.uv.y > _FillProgress ? alpha * fillTex.a : alpha * mainTex.a;
                    break;
                case 2:
                    color = i.uv.x < _FillProgress ? fillTex : mainTex;
                    color.a = i.uv.x < _FillProgress ? alpha * fillTex.a : alpha * mainTex.a;
                    break;
                case 3:
                    color = i.uv.x > _FillProgress ? fillTex : mainTex;
                    color.a = i.uv.x > _FillProgress ? alpha * fillTex.a : alpha * mainTex.a;
                    break;
                case 4:
                    color = i.uv.y < _FillProgress ? fillTex : mainTex;
                    color.a = i.uv.y < _FillProgress ? alpha * fillTex.a : alpha * mainTex.a;
                    break;
                default:
                    color = i.uv.y > _FillProgress ? fillTex : mainTex;
                    color.a = i.uv.y > _FillProgress ? alpha * fillTex.a : alpha * mainTex.a;
                    break;
                }
                UNITY_APPLY_FOG_COLOR(i.fogCoord, color, fixed4(0,0,0,0));
                return color;
            }
            ENDCG
        }
    }
}
