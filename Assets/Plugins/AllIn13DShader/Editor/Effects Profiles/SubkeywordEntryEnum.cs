namespace AllIn13DShader
{
	[System.Serializable]
	public class SubkeywordEntryEnum
	{
		public string propertyName;

		public int kwIndexEnabled;
		public string[] displayNames;
		public string[] keywords;

		public SubkeywordEntryEnum(int kwIndexEnabled, string[] displayNames, string[] keywords, string propertyName) 
		{
			this.propertyName = propertyName;

			this.displayNames = new string[displayNames.Length];
			this.keywords = new string[keywords.Length];
			
			for (int i = 0; i < displayNames.Length; i++)
			{
				this.displayNames[i] = displayNames[i];
				this.keywords[i] = keywords[i];
			}
		}

		public SubkeywordEntryEnum(SubkeywordEntryEnum copyFrom)
		{
			this.kwIndexEnabled = copyFrom.kwIndexEnabled;

			this.displayNames = new string[copyFrom.displayNames.Length];
			for(int i = 0; i < copyFrom.displayNames.Length; i++)
			{
				this.displayNames[i] = copyFrom.displayNames[i];
			}

			this.keywords = new string[copyFrom.keywords.Length];
			for(int i = 0; i < copyFrom.keywords.Length; i++)
			{
				this.keywords[i] = copyFrom.keywords[i];
			}

			this.propertyName = copyFrom.propertyName;
		}

		public string GetKeywordEnabled()
		{
			string res = keywords[kwIndexEnabled];
			return res;
		}
	}
}