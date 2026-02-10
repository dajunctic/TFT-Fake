using UnityEngine.Rendering;

namespace AllIn13DShader
{
	public class ConverterStandardBIRP : ConverterStandard
	{
		protected override void ConvertBlending()
		{
			if (from.IsKeywordEnabled("_ALPHAPREMULTIPLY_ON"))
			{
				SetBlendSrc(UnityEngine.Rendering.BlendMode.One);
				SetBlendDst(UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
				SetAlphaPreset();
			}
			else if (from.IsKeywordEnabled("_ALPHABLEND_ON"))
			{
				SetBlendSrc(UnityEngine.Rendering.BlendMode.SrcAlpha);
				SetBlendDst(UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
				SetAlphaPreset();
			}
			else
			{
				SetBlendSrc(UnityEngine.Rendering.BlendMode.One);
				SetBlendDst(UnityEngine.Rendering.BlendMode.Zero);
				SetOpaquePreset();
			}
		}
	}
}