using System.Linq;
using Sirenix.OdinInspector;
using Dajunctic.SkillSystem.Panthera;
using Dajunctic.SkillSystem.Panthera.Data;
using Dajunctic.SkillSystem.Panthera.Utils;
using Dajunctic.SkillSystem.Logic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using XNode;

namespace Dajunctic.SkillSystem.Data
{
    [CreateAssetMenu(menuName = "Dajunctic.SkillSystem/Ability/Skill")]
    [GuidReferenceable(typeof(IAbilityEntityData<>))]
    public class SkillEntityDataAsset : BaseScriptableObjectDataAsset<ISkillEntityData, SkillEntityData>,
        ISkillEntityDataAsset<ISkillEntityData>
    {
#if UNITY_EDITOR
        [ContextMenu("Auto Set Graph"), Button]
        public void AutoSet()
        {
            var graph = AssetUtils.FindAssetAtFolder<NodeGraph>(new[] { "Assets" })
                .FirstOrDefault(g => g.name == name + "_graph");
            Data.LevelData.SetGraph(graph);
            EditorUtility.SetDirty(this);

            if (graph == null)
            {
                Debug.LogError($"Cannot find graph for {name}", this);
            }
        }

        
        
        [Button, ContextMenu("Auto Set Localized String")]
        public void AutoSetLocalizedString()
        {
            data.StaticData.SetLocalizeString(name);
            data.LevelData.SetLocalizeString(name);
            EditorUtility.SetDirty(this);
        }

        [Button, ContextMenu("Auto Set Smart String")]
        public void AutoSetSmartString()
        {
            data.LevelData.SetSmartString();
            EditorUtility.SetDirty(this);
        }
#endif
    }
    

}


