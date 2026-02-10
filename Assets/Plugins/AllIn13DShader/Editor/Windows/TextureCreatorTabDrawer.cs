namespace AllIn13DShader
{
	public class TextureCreatorTabDrawer : AssetWindowTabDrawer
	{
		private const string TAB_NAME = "Texture Creators";

		private NormalMapCreatorTool normalMapCreatorTool;
		private NormalMapCreatorDrawer normalMapCreatorDrawer;

		private GradientCreatorTool gradientCreatorTool;
		private GradientCreatorDrawer gradientCreatorDrawer;

		private AtlasPackerTool atlasPackerTool;
		private AtlasPackerDrawer atlasPackerDrawer;

		private NoiseCreatorTool noiseCreatorTool;
		private NoiseCreatorDrawer noiseCreatorDrawer;

		private RGBAPackerTool rgbaPackerTool;
		private RGBAPackerDrawer rgbaPackerDrawer;

		public TextureCreatorTabDrawer(CommonStyles commonStyles, AllIn13DShaderWindow parentWindow) : base(commonStyles, parentWindow)
		{
			Initialize();
		}

		private void Initialize()
		{
			normalMapCreatorTool = new NormalMapCreatorTool();
			normalMapCreatorDrawer = new NormalMapCreatorDrawer(normalMapCreatorTool, commonStyles, Repaint);

			gradientCreatorTool = new GradientCreatorTool();
			gradientCreatorDrawer = new GradientCreatorDrawer(gradientCreatorTool, commonStyles);

			atlasPackerTool = new AtlasPackerTool();
			atlasPackerDrawer = new AtlasPackerDrawer(atlasPackerTool, commonStyles);

			noiseCreatorTool = new NoiseCreatorTool();
			noiseCreatorDrawer = new NoiseCreatorDrawer(noiseCreatorTool, commonStyles);

			rgbaPackerTool = new RGBAPackerTool();
			rgbaPackerDrawer = new RGBAPackerDrawer(rgbaPackerTool, commonStyles);
		}

		public override void OnEnable()
		{
		
		}

		public override void OnDisable()
		{
		
		}

		public override void Show()
		{
			Initialize();
		}

		public override void Hide()
		{
			
		}

		public override void EnteredPlayMode()
		{

		}

		public override void Draw()
		{
			normalMapCreatorDrawer.Draw();

			EditorUtils.DrawThinLine();

			gradientCreatorDrawer.Draw();

			EditorUtils.DrawThinLine();

			atlasPackerDrawer.Draw();

			EditorUtils.DrawThinLine();

			noiseCreatorDrawer.Draw();

			EditorUtils.DrawThinLine();

			rgbaPackerDrawer.Draw();
		}

		private void Repaint()
		{
			parentWindow.Repaint();
		}

		public override string GetTabName()
		{
			return TAB_NAME;
		}
	}
}