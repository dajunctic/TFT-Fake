using UnityEngine;

namespace AllIn13DShader
{
	public class CommonMaterialInfo : AbstractMaterialInfo
	{
		public CommonMaterialInfo(Material mat) : base(mat)
		{
			RefreshKeywords();
		}

		public EffectsProfile effectProfile;

		public override void RefreshKeywords()
		{
			this.enabledKeywords = new string[mat.enabledKeywords.Length];
			for (int i = 0; i < enabledKeywords.Length; i++)
			{
				this.enabledKeywords[i] = mat.enabledKeywords[i].name;
			}
		}
		
		public override void EnableKeyword(string keyword)
		{
			mat.EnableKeyword(keyword);

			RefreshKeywords();
		}

		public override void DisableKeyword(string keyword)
		{
			mat.DisableKeyword(keyword);

			RefreshKeywords();
		}

		public override bool IsShaderVariant()
		{
			return false;
		}
	}
}