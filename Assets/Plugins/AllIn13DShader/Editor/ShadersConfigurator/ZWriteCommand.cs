namespace AllIn13DShader
{
	[System.Serializable]
	public class ZWriteCommand : ShaderCommand
	{
		private const string SHADER_ENTRY_FORMAT = @"ZWrite {0}";
		private const string STR_DEFINED_IN_PROPERTIES = @"[_ZWrite]";

		public bool zWriteOn;

		protected override string GetShaderEntryFormat()
		{
			return SHADER_ENTRY_FORMAT;
		}

		protected override string GetShaderEntryValueDefinedInCode()
		{
			string res = zWriteOn ? "On" : "Off";
			return res;
		}

		protected override string GetShaderEntryValueDefinedInProperties()
		{
			return STR_DEFINED_IN_PROPERTIES;	
		}
	}
}