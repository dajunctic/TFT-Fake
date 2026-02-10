using UnityEditor;
using UnityEngine;

namespace AllIn13DShader
{
	public class PropertiesConfigCollection : ScriptableObject
	{
		public PropertiesConfig propertiesConfig;
		
		public void AddConfig(PropertiesConfig config)
		{
			this.propertiesConfig = config;
		}

		public PropertiesConfig FindPropertiesConfigByShader(Shader shader)
		{
			PropertiesConfig res = null;

			res = propertiesConfig;

			return res;
		}

		public bool IsAllIn3DShaderMaterial(Material mat)
		{
			bool res = false;

			res = mat.shader.name.Contains("AllIn13DShader");

			return res;
		}
	}
}