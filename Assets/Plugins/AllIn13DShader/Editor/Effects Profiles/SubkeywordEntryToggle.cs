namespace AllIn13DShader
{
	[System.Serializable]
	public class SubkeywordEntryToggle
	{
		public string propertyName;

		public bool isEnabled;
		public string keyword;
		public string displayName;

		public SubkeywordEntryToggle(bool isEnabled, string keyword, string displayName)
		{
			this.isEnabled = isEnabled;
			this.keyword = keyword;
			this.displayName = displayName;
		}

		public SubkeywordEntryToggle(SubkeywordEntryToggle copyFrom)
		{
			this.isEnabled = copyFrom.isEnabled;
			this.keyword = copyFrom.keyword;
			this.displayName = copyFrom.displayName;
		}
	}
}