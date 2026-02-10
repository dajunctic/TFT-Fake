namespace AllIn13DShader
{
	public struct EffectAttributeData
	{
		public string effectID;
		public string groupID;
		public string drawerID;
		public string incompatibleWithEffectID;
		public string dependentEffectID;
		public bool docEnabled;

		public string[] extraPasses;

		public void Init()
		{
			this.effectID = string.Empty;
			this.groupID = string.Empty;
			this.drawerID = string.Empty;
			this.incompatibleWithEffectID = string.Empty;
			this.dependentEffectID = string.Empty;
			this.docEnabled = false;
			this.extraPasses = new string[0];
		}
	}
}