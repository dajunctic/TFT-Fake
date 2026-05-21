using System.Linq;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using GraphProcessor;

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
            var graph = AssetUtils.FindAssetAtFolder<BaseGraph>(new[] { "Assets" })
                .FirstOrDefault(g => g.name == name + "_graph");
            Data.LevelData.SetGraph(graph);
            EditorUtility.SetDirty(this);

            if (graph == null)
            {
                Debug.LogError($"Cannot find graph for {name}", this);
            }
        }
#endif
    }
    

}


