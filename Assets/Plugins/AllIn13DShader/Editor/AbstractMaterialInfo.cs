using UnityEngine;

namespace AllIn13DShader
{
	public abstract class AbstractMaterialInfo
	{
		public string[] enabledKeywords;
		public Material mat;

		public AbstractMaterialInfo(Material mat)
		{
			this.enabledKeywords = new string[0];
			this.mat = mat;
		}

		public abstract void RefreshKeywords();

		public abstract void EnableKeyword(string keyword);

		public abstract void DisableKeyword(string keyword);

		public abstract bool IsShaderVariant();

		public bool IsKeywordEnabled(string keyword)
		{
			bool res = false;

			for (int i = 0; i < enabledKeywords.Length; i++)
			{
				if (enabledKeywords[i] == keyword)
				{
					res = true;
					break;
				}
			}

			return res;
		}

		public int GetEnabledKeywordIndexByEffect(AllIn13DEffectConfig effectConfig)
		{
			int res = -1;
			for (int i = 0; i < enabledKeywords.Length; i++)
			{
				res = effectConfig.GetKeywordIndex(enabledKeywords[i]);
				if(res >= 0)
				{
					break;
				}
			}

			return res;
		}

		public int GetEnabledKeywordIndexByEffectProperty(EffectProperty effectProperty)
		{
			int res = -1;
			
			if(effectProperty.IsEnumProperty() || effectProperty.IsToggleProperty())
			{
				for (int i = 0; i < enabledKeywords.Length; i++)
				{
					for (int j = 0; j < effectProperty.fullKeywordNames.Length; j++)
					{
						if (effectProperty.fullKeywordNames[j] == enabledKeywords[i])
						{
							res = j;
							break;
						}
					}

					if(res >= 0)
					{
						break;
					}
				}
			}

			return res;
		}

		public static AbstractMaterialInfo CreateInstance(Material mat)
		{
			EffectsProfileCollection effectsProfileCollection = GlobalConfiguration.instance.effectsProfileCollection;
			EffectsProfile effectsProfile = effectsProfileCollection.FindEffectProfileByShader(mat.shader);

			AbstractMaterialInfo res = null;
			if (effectsProfile == null)
			{
				res = new CommonMaterialInfo(mat);
			}
			else
			{
				res = new EffectProfileMaterialInfo(effectsProfile, mat);
			}

			return res;
		}
	}
}