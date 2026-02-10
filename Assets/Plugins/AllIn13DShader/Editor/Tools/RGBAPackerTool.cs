using UnityEngine;

namespace AllIn13DShader
{
	public class RGBAPackerTool
	{
		public RGBAPackerValues values;

		public RGBAPackerTool()
		{
			values = ScriptableObject.CreateInstance<RGBAPackerValues>();
		}

		public void CreateRGBATexture()
		{
			int textureSize = (int)values.textureSizes;
			Texture2D rgbaTexture = new Texture2D(textureSize, textureSize, TextureFormat.RGBA32, false);

			// Get readable copies of input textures
			Texture2D readableR = GetReadableTexture(values.rChannelTexture, textureSize);
			Texture2D readableG = GetReadableTexture(values.gChannelTexture, textureSize);
			Texture2D readableB = GetReadableTexture(values.bChannelTexture, textureSize);
			Texture2D readableA = GetReadableTexture(values.aChannelTexture, textureSize);

			// Pack channels
			for(int x = 0; x < textureSize; x++)
			{
				for(int y = 0; y < textureSize; y++)
				{
					float r = readableR != null ? readableR.GetPixel(x, y).r : (values.rChannelDefaultWhite ? 1f : 0f);
					float g = readableG != null ? readableG.GetPixel(x, y).r : (values.gChannelDefaultWhite ? 1f : 0f);
					float b = readableB != null ? readableB.GetPixel(x, y).r : (values.bChannelDefaultWhite ? 1f : 0f);
					float a = readableA != null ? readableA.GetPixel(x, y).r : (values.aChannelDefaultWhite ? 1f : 0f);

					rgbaTexture.SetPixel(x, y, new Color(r, g, b, a));
				}
			}

			rgbaTexture.Apply();
			values.createdRGBATexture = rgbaTexture;

			// Clean up temporary textures
			if(readableR != values.rChannelTexture && readableR != null) Object.DestroyImmediate(readableR);
			if(readableG != values.gChannelTexture && readableG != null) Object.DestroyImmediate(readableG);
			if(readableB != values.bChannelTexture && readableB != null) Object.DestroyImmediate(readableB);
			if(readableA != values.aChannelTexture && readableA != null) Object.DestroyImmediate(readableA);
		}

		private Texture2D GetReadableTexture(Texture2D source, int targetSize)
		{
			if(source == null) return null;

			// Check if texture is already readable
			try
			{
				source.GetPixel(0, 0);
				// If we get here, texture is readable, but we might need to resize
				if(source.width == targetSize && source.height == targetSize)
					return source;
			}
			catch
			{
				// Texture is not readable, need to make a copy
			}

			// Create readable copy
			RenderTexture renderTexture = RenderTexture.GetTemporary(targetSize, targetSize, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
			Graphics.Blit(source, renderTexture);

			RenderTexture previous = RenderTexture.active;
			RenderTexture.active = renderTexture;

			Texture2D readableTexture = new Texture2D(targetSize, targetSize, TextureFormat.RGBA32, false);
			readableTexture.ReadPixels(new Rect(0, 0, targetSize, targetSize), 0, 0);
			readableTexture.Apply();

			RenderTexture.active = previous;
			RenderTexture.ReleaseTemporary(renderTexture);

			return readableTexture;
		}
	}
}