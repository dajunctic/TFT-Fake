using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Dajunctic.SkillSystem.Commands
{
    [CreateAssetMenu(fileName = "New Skill Timeline", menuName = "Dajunctic/Skill/Skill Timeline")]
    public class SkillTimelineSO : ScriptableObject
    {
        [HideLabel]
        [Title("Skill Timeline Actions", "Executed in sequence", TitleAlignments.Centered)]
        [SerializeReference]
        public List<SkillAction> Actions = new List<SkillAction>();
    }
}
