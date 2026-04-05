Shader "DTT/Unlit/ArcShader"
{
    Properties
    {
        // The main texture used to display.
        _MainTex ("Texture", 2D) = "white" {}
        // The color that can be applied to be blended with the texture.
        _Color ("Color", Color) = (1, 1, 1, 1)
        // The normalized arc angle.
        _FillColor("Secondary Color", Color) = (1, 1 ,1 ,0.5)
        _Progress("Progress", Range(0, 1)) = 0.5
        _Arc("Arc", Range(0, 1)) = 0
        // The normalized angle offset.
        _Angle("Angle", Range(0, 1)) = 0
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
            float _Arc;
            float _Angle;
            float _Progress;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o, o.vertex);
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                // Sample texture and apply colour.
                fixed4 tex = tex2D(_MainTex, i.uv);
                fixed4 col = tex * _Color;

                float dist = length(i.uv - float2(0.5, 0.5));
                float cutoff = dist > _Progress / 2 ? 1 : 0;

                // Offset so origin is centre.
                float2 offset = i.uv.xy - 0.5f;

                // The angle of the current uv.
                const float arcAlpha = offset.x == 0 || offset.y == 0 ? 0 : atan2(offset.y, offset.x);

                // The angle in 0..1 range.
                const float normalizedAngle = fmod((arcAlpha / UNITY_PI + 1) / 2 + _Angle + _Arc / 2, 1);

                // Apply fog.
                UNITY_APPLY_FOG(i.fogCoord, col);

                // Only return the colour if it's not masked by the arc.
                return normalizedAngle > _Arc ? cutoff == 0 ? col : tex * _FillColor : 0;
            }
            ENDCG
        }
    }
        
    CustomEditor "DTT.AreaOfEffectRegions.Editor.ArcShaderEditor"
}