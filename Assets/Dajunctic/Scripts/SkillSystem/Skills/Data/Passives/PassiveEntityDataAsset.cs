using System.Linq;
using Dajunctic.SkillSystem.Panthera;
using Dajunctic.SkillSystem.Panthera.Data;
using Dajunctic.SkillSystem.Panthera.Utils;
using Dajunctic.SkillSystem.Logic;
using Sirenix.OdinInspector;
using UnityEngine;
using XNode;
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
                .FindAssetAtFolder<NodeGraph>(new[] { "Assets" })
                .FirstOrDefault(g => g.name == name + "_graph");
            Data.LevelData.SetGraph(graph);
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

        [Button, ContextMenu("Auto Fix Captain Passive Desc")]
        public void AutoFixCaptainPassiveDesc()
        {
            data.LevelData.FixCaptainPassiveDesc();
            EditorUtility.SetDirty(this);
        }
#endif
    }


}



