using UnityEngine;

namespace AllIn13DShader
{
	public class EffectProfileMaterialInfo : AbstractMaterialInfo
	{
		public EffectsProfile effectProfile;
		
		public EffectProfileMaterialInfo(EffectsProfile effectProfile, Material mat) : base(mat)
		{
			this.effectProfile = effectProfile;

			RefreshKeywords();
		}

		public override void DisableKeyword(string keyword)
		{}

		public override void EnableKeyword(string keyword) 
		{}

		public override bool IsShaderVariant()
		{
			return true;
		}

		public override void RefreshKeywords()
		{
			this.enabledKeywords = effectProfile.GetKeywordsEnabled().ToArray();
		}
	}
}