#ifndef ALLIN1SPRITESHADERSRP_PROPERTIES
#define ALLIN1SPRITESHADERSRP_PROPERTIES

CBUFFER_START(UnityPerMaterial)
	half4 /*_MainTex_ST,*/ /*_MainTex_TexelSize,*/ _Color;
	float4 _MainTex_ScaleAndTiling;
				 
	half _Alpha;
				
	half _MinXUV, _MaxXUV, _MinYUV, _MaxYUV;
			
	half _RectSize;
			
	half _OffsetUvX, _OffsetUvY;
			
	half _ClipUvLeft, _ClipUvRight, _ClipUvUp, _ClipUvDown;

	half _RadialStartAngle, _RadialClip, _RadialClip2;
			
	half _TwistUvAmount, _TwistUvPosX, _TwistUvPosY, _TwistUvRadius;

	half _RotateUvAmount;
			
	half _FishEyeUvAmount;
			
	half _PinchUvAmount;
			
	half _HandDrawnAmount, _HandDrawnSpeed;
			
	half _ShakeUvSpeed, _ShakeUvX, _ShakeUvY;
			
	float _WaveAmount, _WaveSpeed, _WaveStrength, _WaveX, _WaveY;
			
	half _RoundWaveStrength, _RoundWaveSpeed;
			
	half _ZoomUvAmount;
			 
	half4 _FadeBurnColor; /*_FadeTex_ST,*/ /*_FadeBurnTex_ST*/
	float4 _FadeTex_ScaleAndTiling, _FadeBurnTex_ScaleAndTiling;
	half _FadeAmount, _FadeBurnWidth, _FadeBurnTransition, _FadeBurnGlow;
			
	half _TextureScrollXSpeed, _TextureScrollYSpeed;
			
	half4 _GlowColor;
	half _Glow, _GlowGlobal;

			
	half4 _OutlineColor;
	half _OutlineAlpha, _OutlineGlow, _OutlineWidth;
	int _OutlinePixelWidth;

	//half4 _OutlineTex_ST;
	float4 _OutlineTex_ScaleAndTiling;
	half _OutlineTexXSpeed, _OutlineTexYSpeed;
			
	//half4 _OutlineDistortTex_ST;
	float4 _OutlineDistortTex_ScaleAndTiling;
	half _OutlineDistortTexXSpeed, _OutlineDistortTexYSpeed, _OutlineDistortAmount;
			
	//half4 _DistortTex_ST;
	float4 _DistortTex_ScaleAndTiling;
	half _DistortTexXSpeed, _DistortTexYSpeed, _DistortAmount;
			
	half _WarpStrength, _WarpSpeed, _WarpScale;
			
	half _GrassSpeed, _GrassWind, _GrassManualAnim, _GrassRadialBend;
			
	half _GradBlend, _GradBoostX, _GradBoostY;
	half4 _GradTopRightCol, _GradTopLeftCol, _GradBotRightCol, _GradBotLeftCol;
			
	half4 _ColorSwapRed, _ColorSwapGreen, _ColorSwapBlue;
	half _ColorSwapRedLuminosity, _ColorSwapGreenLuminosity, _ColorSwapBlueLuminosity, _ColorSwapBlend;
			
	half _HsvShift, _HsvSaturation, _HsvBright;
			
	half4 _HitEffectColor;
	half _HitEffectGlow, _HitEffectBlend;
			
	half _PixelateSize;
			
	half _NegativeAmount;
			
	half _ColorRampLuminosity, _ColorRampBlend;

	half _GreyscaleLuminosity, _GreyscaleBlend;
	half4 _GreyscaleTintColor;
			
	half _PosterizeNumColors, _PosterizeGamma;
			
	half _BlurIntensity;
			
	half _MotionBlurAngle, _MotionBlurDist;

	half _GhostColorBoost, _GhostTransparency, _GhostBlend;
			
	half _AlphaOutlineGlow, _AlphaOutlinePower, _AlphaOutlineMinAlpha, _AlphaOutlineBlend;
	half4 _AlphaOutlineColor;
			
	half _InnerOutlineThickness, _InnerOutlineAlpha, _InnerOutlineGlow;
	half4 _InnerOutlineColor;
			
	half _HologramStripesAmount, _HologramMinAlpha, _HologramUnmodAmount, _HologramStripesSpeed, _HologramMaxAlpha, _HologramBlend;
	half4 _HologramStripeColor;
			
	half _ChromAberrAmount, _ChromAberrAlpha;
			
	half _GlitchAmount, _GlitchSize;
			
	half _FlickerFreq, _FlickerPercent, _FlickerAlpha;
			
	half _ShadowX, _ShadowY, _ShadowAlpha;
	half4 _ShadowColor;
			
	half4 _ShineColor;
	half _ShineLocation, _ShineRotate, _ShineWidth, _ShineGlow;
			
	half _AlphaCutoffValue;
			
	half _AlphaRoundThreshold;
			
	half4 _ColorChangeNewCol, _ColorChangeTarget;
	half _ColorChangeTolerance, _ColorChangeLuminosity;

	half4 _ColorChangeNewCol2, _ColorChangeTarget2;
	half _ColorChangeTolerance2;
			
	half4 _ColorChangeNewCol3, _ColorChangeTarget3;
	half _ColorChangeTolerance3;
			
	half _Contrast, _Brightness;
			
	//half4 _OverlayTex_ST;
	float4 _OverlayTex_ScaleAndTiling;
	float4 _OverlayColor;
	half _OverlayGlow, _OverlayBlend, _OverlayTextureScrollXSpeed, _OverlayTextureScrollYSpeed;

	float _RandomSeed;
CBUFFER_END

			
#define CUSTOM_TRANSFORM_TEX(uv, st) uv * st.xy + st.zw
#define GET_PIXEL(offsetX, offsetY, uv, tex, texelSize) SAMPLE_TEXTURE2D(tex, sampler##tex, uv + half2(offsetX * texelSize.x, offsetY * texelSize.y)).rgb
#define DECLARE_TEX_AND_SAMPLER(texName) \
				TEXTURE2D(texName);\
				SAMPLER(sampler##texName);


DECLARE_TEX_AND_SAMPLER(_MainTex)
            
#if FADE_ON
		DECLARE_TEX_AND_SAMPLER(_FadeTex)
		DECLARE_TEX_AND_SAMPLER(_FadeBurnTex)
#endif

#if GLOW_ON
		DECLARE_TEX_AND_SAMPLER(_GlowTex)
		//sampler2D _GlowTex;
#endif

#if OUTTEX_ON
		DECLARE_TEX_AND_SAMPLER(_OutlineTex)
#endif

#if OUTDIST_ON
		DECLARE_TEX_AND_SAMPLER(_OutlineDistortTex)
		//sampler2D _OutlineDistortTex;
#endif

#if DISTORT_ON
		DECLARE_TEX_AND_SAMPLER(_DistortTex)
		//sampler2D _DistortTex;
#endif

#if COLORSWAP_ON
		DECLARE_TEX_AND_SAMPLER(_ColorSwapTex)
#endif

#if COLORRAMP_ON
		DECLARE_TEX_AND_SAMPLER(_ColorRampTex)
		DECLARE_TEX_AND_SAMPLER(_ColorRampTexGradient)
#endif

#if SHINE_ON
		DECLARE_TEX_AND_SAMPLER(_ShineMask)
#endif
			
#if OVERLAY_ON
		DECLARE_TEX_AND_SAMPLER(_OverlayTex)
#endif

#endif