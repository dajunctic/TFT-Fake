#if UNITY_EDITOR
using UnityEditor;

namespace AllIn1SpriteShader
{
	public static class EditorUtils
	{
		public static AllIn1ShaderPropertyType GetShaderTypeByMaterialProperty(MaterialProperty matProperty)
		{
			AllIn1ShaderPropertyType res;

#if UNITY_6000_2_OR_NEWER
			switch (matProperty.propertyType)
			{
				case UnityEngine.Rendering.ShaderPropertyType.Color:
					res = AllIn1ShaderPropertyType.Color;
					break;
				case UnityEngine.Rendering.ShaderPropertyType.Float:
					res = AllIn1ShaderPropertyType.Float;
					break;
				case UnityEngine.Rendering.ShaderPropertyType.Int:
					res = AllIn1ShaderPropertyType.Int;
					break;
				case UnityEngine.Rendering.ShaderPropertyType.Range:
					res = AllIn1ShaderPropertyType.Range;
					break;
				case UnityEngine.Rendering.ShaderPropertyType.Texture:
					res = AllIn1ShaderPropertyType.Texture;
					break;
				case UnityEngine.Rendering.ShaderPropertyType.Vector:
					res = AllIn1ShaderPropertyType.Vector;
					break;
				default:
					res = AllIn1ShaderPropertyType.Vector;
					break;
			}
#else
			switch (matProperty.type)
			{
				case MaterialProperty.PropType.Color:
					res = AllIn1ShaderPropertyType.Color;
					break;
				case MaterialProperty.PropType.Float:
					res = AllIn1ShaderPropertyType.Float;
					break;
				case MaterialProperty.PropType.Int:
					res = AllIn1ShaderPropertyType.Int;
					break;
				case MaterialProperty.PropType.Range:
					res = AllIn1ShaderPropertyType.Range;
					break;
				case MaterialProperty.PropType.Texture:
					res = AllIn1ShaderPropertyType.Texture;
					break;
				case MaterialProperty.PropType.Vector:
					res = AllIn1ShaderPropertyType.Vector;
					break;
				default:
					res = AllIn1ShaderPropertyType.Vector;
					break;
			}
#endif
			return res;

		}
	}
}
#endif