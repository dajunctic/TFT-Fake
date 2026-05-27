using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using GraphProcessor;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Dajunctic.SkillSystem.Data
{
    [CreateAssetMenu(menuName = "Dajunctic.SkillSystem/Ability/Passive")]
    [GuidReferenceable(typeof(IAbilityEntityData<>))]
    public class PassiveEntityDataAsset
        : BaseScriptableObjectDataAsset<IPassiveEntityData, PassiveEntityData>,
            IPassiveEntityDataAsset<IPassiveEntityData>
    {
#if UNITY_EDITOR
        [ContextMenu("Auto Set Graph"), Button]
        public void AutoSet()
        {
            var graph = AssetUtils
                .FindAssetAtFolder<BaseGraph>(new[] { "Assets" })
                .FirstOrDefault(g => g.name == name + "_graph");
            Data.LevelData.SetGraph(graph);
            if (graph == null)
            {
                Debug.LogError($"Cannot find graph for {name}", this);
            }
        }
#endif
    }

}
