namespace AllIn13DShader
{
	public abstract class ShaderCommand
	{
		public ShaderCommandValue shaderCommandValue;

		public string GetShaderEntry()
		{
			string res = string.Empty;
			string shaderEntryValue = string.Empty;
			
			switch (shaderCommandValue)
			{
				case ShaderCommandValue.DEFINED_IN_CODE:
					shaderEntryValue = GetShaderEntryValueDefinedInCode();
					break;
				case ShaderCommandValue.DEFINED_IN_PROPERTIES:
					shaderEntryValue = GetShaderEntryValueDefinedInProperties();
					break;
			}

			if (!string.IsNullOrEmpty(shaderEntryValue))
			{
				string format = GetShaderEntryFormat();
				res = string.Format(format, shaderEntryValue);
			}
			
			return res;
		}

		protected abstract string GetShaderEntryFormat();

		protected abstract string GetShaderEntryValueDefinedInProperties();

		protected abstract string GetShaderEntryValueDefinedInCode();

	}
}