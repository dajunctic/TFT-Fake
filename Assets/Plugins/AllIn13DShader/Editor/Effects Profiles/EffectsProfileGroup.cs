using System.Collections.Generic;

namespace AllIn13DShader
{
	[System.Serializable]
	public class EffectsProfileGroup
	{
		public EffectGroupGlobalConfig effectGroupConfig;
		public List<EffectsProfileEntry> entries;

		public EffectsProfileGroup(EffectGroup effectGroup)
		{
			this.effectGroupConfig = effectGroup.effectGroupConfig;

			this.entries = new List<EffectsProfileEntry>();

			for(int i = 0; i < effectGroup.effects.Length; i++)
			{
				AllIn13DEffectConfig effectConfig = effectGroup.effects[i];
				EffectsProfileEntry entry = new EffectsProfileEntry(effectConfig.effectName, effectConfig.displayName, true);

				for (int j = 0; j < effectConfig.effectProperties.Count; j++)
				{
					EffectProperty effectProperty = effectConfig.effectProperties[j];

					if (effectProperty.IsPropertyWithKeywords())
					{
						entry.AddSubkeywordEntries(effectProperty);
					}
				}

				entries.Add(entry);
			}
		}

		public EffectsProfileGroup(EffectsProfileGroup copyFrom)
		{
			this.effectGroupConfig = copyFrom.effectGroupConfig;
			this.entries = new List<EffectsProfileEntry>(copyFrom.entries.Count);
			for(int i = 0; i < copyFrom.entries.Count; i++)
			{
				this.entries.Add(new EffectsProfileEntry(copyFrom.entries[i]));
			}
		}

		public void CollectKeywords(List<string> res)
		{
			for(int i = 0; i < entries.Count; i++)
			{
				if (entries[i].isEnabled)
				{
					entries[i].CollectKeywords(res);
				}
			}
		}

		public bool HasEffectsEnabled()
		{
			bool res = false;

			for(int i = 0; i < entries.Count; i++)
			{
				if (entries[i].isEnabled)
				{
					res = true;
					break;
				}
			}

			return res;
		}
	}
}