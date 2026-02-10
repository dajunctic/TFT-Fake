using UnityEngine;

namespace AllIn13DShader
{
	[CreateAssetMenu(menuName = "AllIn13DShader/Shader Pass Collection")]
	public class ShaderPassCollection : ScriptableObject
	{
		public ShaderPassConfig mainPass;
		public ShaderPassConfig outlinePass;
		public ShaderPassConfig shadowCasterPass;
		public ShaderPassConfig depthOnlyPass;
		public ShaderPassConfig depthNormalsPass;
		public ShaderPassConfig metaPass;
		public ShaderPassConfig forwardAdd;

		public ShaderPassConfig GetShaderPassConfig(AllIn13DPassType passType)
		{
			ShaderPassConfig res = null;

			switch (passType)
			{
				case AllIn13DPassType.MAIN:
					res = mainPass;
					break;
				case AllIn13DPassType.OUTLINE:
					res = outlinePass;
					break;
				case AllIn13DPassType.SHADOW_CASTER:
					res = shadowCasterPass;
					break;
				case AllIn13DPassType.DEPTH_ONLY:
					res = depthOnlyPass;
					break;
				case AllIn13DPassType.DEPTH_NORMALS:
					res = depthNormalsPass;
					break;
				case AllIn13DPassType.META:
					res = metaPass;
					break;
				case AllIn13DPassType.FORWARD_ADD:
					res = forwardAdd;
					break;
			}

			return res;
		}
	}
}