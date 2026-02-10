namespace AllIn13DShader
{
	public abstract class AssetWindowTabDrawer
	{
		protected CommonStyles commonStyles;
		protected AllIn13DShaderWindow parentWindow;

		public AssetWindowTabDrawer(CommonStyles commonStyles, AllIn13DShaderWindow parentWindow)
		{
			this.commonStyles = commonStyles;
			this.parentWindow = parentWindow;
		}

		public abstract void Hide();

		public abstract void Show();
		
		public abstract void OnDisable();
		
		public abstract void OnEnable();

		public abstract void Draw();

		public abstract void EnteredPlayMode();

		public virtual void ExitingEditMode()
		{
		
		}

		public abstract string GetTabName();
	}
}