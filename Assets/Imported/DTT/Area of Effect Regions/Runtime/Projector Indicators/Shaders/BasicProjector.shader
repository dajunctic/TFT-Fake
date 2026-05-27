Shader "DTT/Projector/Basic Projector"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Base Color", Color) = (1,1,1,1)
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
                fixed4 tex = tex2Dproj(_MainTex, UNITY_PROJ_COORD(i.uv)) * _Color;
                fixed4 color = tex;
                UNITY_APPLY_FOG_COLOR(i.fogCoord, color, fixed4(0,0,0,0));
                return color;
            }
            ENDCG
        }
    }
}