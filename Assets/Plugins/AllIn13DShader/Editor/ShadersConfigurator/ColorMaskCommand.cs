using UnityEngine.Rendering;

namespace AllIn13DShader
{
	[System.Serializable]
	public class ColorMaskCommand : ShaderCommand
	{
		private const string SHADER_ENTRY_FORMAT = @"ColorMask {0}";
		private const string STR_DEFINED_IN_PROPERTIES = @"[_ColorMask]";

		public ColorWriteMask colorWriteMask;

		protected override string GetShaderEntryFormat()
		{
			return SHADER_ENTRY_FORMAT;
		}

		protected override string GetShaderEntryValueDefinedInCode()
		{
			string res = string.Empty;
			if (colorWriteMask.HasFlag(ColorWriteMask.Red))
			{
				res += "R";
			}

			if (colorWriteMask.HasFlag(ColorWriteMask.Green))
			{
				res += "G";
			}

			if (colorWriteMask.HasFlag(ColorWriteMask.Blue))
			{
				res += "B";
			}

			if (colorWriteMask.HasFlag(ColorWriteMask.Alpha))
			{
				res += "A";
			}

			if (string.IsNullOrEmpty(res))
			{
				res = "0";
			}

			return res;
		}

		protected override string GetShaderEntryValueDefinedInProperties()
		{
			return STR_DEFINED_IN_PROPERTIES;
		}
	}
}