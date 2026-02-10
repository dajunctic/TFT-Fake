using UnityEngine.Rendering;

namespace AllIn13DShader
{
	[System.Serializable]
	public class ZTestCommand : ShaderCommand
	{
		private const string SHADER_ENTRY_FORMAT = @"ZTest {0}";
		private const string STR_DEFINED_IN_PROPERTIES = @"[_ZTestMode]";

		public CompareFunction zTestMode;

		protected override string GetShaderEntryFormat()
		{
			return SHADER_ENTRY_FORMAT;
		}

		protected override string GetShaderEntryValueDefinedInCode()
		{
			string res = string.Empty;

			switch (zTestMode)
			{
				case CompareFunction.Disabled:
					res = "Disabled";
					break;
				case CompareFunction.Never:
					res = "Never";
					break;
				case CompareFunction.Less:
					res = "Less";
					break;
				case CompareFunction.Equal:
					res = "Equal";
					break;
				case CompareFunction.LessEqual:
					res = "LEqual";
					break;
				case CompareFunction.Greater:
					res = "Greater";
					break;
				case CompareFunction.NotEqual:
					res = "NotEqual";
					break;
				case CompareFunction.GreaterEqual:
					res = "GEqual";
					break;
				case CompareFunction.Always:
					res = "Always";
					break;
			}

			return res;
		}

		protected override string GetShaderEntryValueDefinedInProperties()
		{
			return STR_DEFINED_IN_PROPERTIES;
		}
	}
}