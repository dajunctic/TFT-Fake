using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AllIn13DShader
{
	public class AdvancedPropertiesDrawer
	{
		private List<int> advancedPropertiesIndices;
		private int blendSrcIndex;
		private int blendDstIndex;

		private AllIn13DShaderInspectorReferences references;

		private MaterialProperty advancedPropertiesEnabledMatProperty;

		private bool AdvancedPropertiesEnabled
		{
			get
			{
				bool res = advancedPropertiesEnabledMatProperty.floatValue > 0;
				return res;
			}
			set
			{
				float floatValue = value ? 1.0f : 0f;
				advancedPropertiesEnabledMatProperty.floatValue = floatValue;
			}
		}

		public AdvancedPropertiesDrawer(List<int> advancedPropertiesIndices, int blendSrcIndex, int blendDstIndex, AllIn13DShaderInspectorReferences references)
		{
			this.advancedPropertiesIndices = advancedPropertiesIndices;
			this.blendSrcIndex = blendSrcIndex;
			this.blendDstIndex = blendDstIndex;

			this.references = references;

			advancedPropertiesEnabledMatProperty = references.matProperties[advancedPropertiesIndices[0]];
		}

		public void Draw()
		{
			AdvancedPropertiesEnabled = GUILayout.Toggle(AdvancedPropertiesEnabled, new GUIContent("Show Advanced Configuration"), references.toggleButtonStyle);
			if (AdvancedPropertiesEnabled)
			{
				EditorGUILayout.BeginVertical(references.propertiesStyle);

				MaterialProperty matPropertyBlendSrc = references.matProperties[blendSrcIndex];
				MaterialProperty matProeprtyBlendDst = references.matProperties[blendDstIndex];

				EffectPropertyDrawer.DrawProperty(
					materialProperty: matPropertyBlendSrc, 
					labelPrefix: string.Empty, 
					displayName: matPropertyBlendSrc.displayName,
					customValue: string.Empty,
					allowReset: false, 
					isKeywordProperty: false,
					propertyType: EffectProperty.PropertyType.BASIC,
					references);

				EffectPropertyDrawer.DrawProperty(
					materialProperty: matProeprtyBlendDst,
					labelPrefix: string.Empty,
					displayName: matProeprtyBlendDst.displayName,
					customValue: string.Empty,
					allowReset: false,
					isKeywordProperty: false,
					propertyType: EffectProperty.PropertyType.BASIC,
					references);
				
				EditorUtils.DrawThinLine();

				for (int i = 1; i < advancedPropertiesIndices.Count; i++)
				{
					//if(i == blendSrcIndex || i == blendDstIndex) { continue; }
					MaterialProperty matProperty = references.matProperties[advancedPropertiesIndices[i]];
					EffectPropertyDrawer.DrawProperty(
						materialProperty: matProperty,
						labelPrefix: string.Empty,
						displayName: matProperty.displayName,
						customValue: string.Empty,
						allowReset: false,
						isKeywordProperty: false,
						propertyType: EffectProperty.PropertyType.BASIC,
						references: references);

					EditorUtils.DrawThinLine();
				}

				references.editorMat.EnableInstancingField();

				Rect rect = EditorGUILayout.GetControlRect();
				rect.x += 3f;
				rect.width -= 3f;
				references.editorMat.RenderQueueField(rect);

				EditorGUILayout.EndVertical();
			}
		}
	}
}