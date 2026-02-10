using UnityEngine.Rendering;

namespace AllIn13DShader
{
	[System.Serializable]
	public class BlendCommand : ShaderCommand
	{
		private const string SHADER_ENTRY_FORMAT = @"Blend {0}";
		public const string SRC_DEFINED_IN_PROPERTIES = @"[_BlendSrc]";
		public const string DST_DEFINED_IN_PROPERTIES = @"[_BlendDst]";

		public BlendMode srcBlending = BlendMode.Zero;
		public BlendMode dstBlending = BlendMode.One;

		protected override string GetShaderEntryFormat()
		{
			return SHADER_ENTRY_FORMAT;
		}

		protected override string GetShaderEntryValueDefinedInProperties()
		{
			string res = $"{SRC_DEFINED_IN_PROPERTIES} {DST_DEFINED_IN_PROPERTIES}";
			return res;
		}

		protected override string GetShaderEntryValueDefinedInCode()
		{
			string res = $"{srcBlending.ToString()} {dstBlending.ToString()}";
			return res;
		}
	}
}