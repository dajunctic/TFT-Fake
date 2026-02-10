using UnityEngine.Rendering;

namespace AllIn13DShader
{
	[System.Serializable]
	public class CullCommand : ShaderCommand
	{
		private const string SHADER_ENTRY_FORMAT = @"Cull {0}";
		private const string STR_DEFINED_IN_PROPERTIES = @"[_CullingMode]";

		public CullMode cullMode;

		protected override string GetShaderEntryFormat()
		{
			return SHADER_ENTRY_FORMAT;
		}

		protected override string GetShaderEntryValueDefinedInCode()
		{
			string res = cullMode.ToString();
			return res;
		}

		protected override string GetShaderEntryValueDefinedInProperties()
		{
			return STR_DEFINED_IN_PROPERTIES;
		}
	}
}