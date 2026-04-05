Shader "DTT/Unlit/VerticalFade"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1, 1, 1, 1)
        _FillColor ("Color", Color) = (1, 1, 1, 0.5)
        _FadeAmount ("Fade Amount", Range(0, 1)) = 0
        _FillProgress ("Fill Progress", Range(0, 1)) = 1
    }
    SubShader
    {     
        Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
        LOD 100
          
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha 

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            
            #pragma multi_compile_instancing
            
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            fixed4 _FillColor;
            float _FadeAmount;
            float _FillProgress;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            float InverseLerp(float from, float to, float value)
            {
                return (value - from) / (to - from);
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                // Sample texture and apply colour.
                fixed4 tex = tex2D(_MainTex, i.uv);

                // Determine fade amount on y axis.
                float fade = saturate(InverseLerp(0, _FadeAmount, i.uv.y));

                // Apply fade to alpha channel.
                tex.a = tex.a * fade;

                // Apply fog.
                UNITY_APPLY_FOG(i.fogCoord, col);
                return i.uv.y < _FillProgress ? tex * _Color : tex * _FillColor;
            }
            ENDCG
        }
    }
        
    CustomEditor "DTT.AreaOfEffectRegions.Editor.VerticalFadeEditor"
}