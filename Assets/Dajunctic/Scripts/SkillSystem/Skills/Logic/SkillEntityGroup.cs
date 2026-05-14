using System.Collections.Generic;
using System.Linq;
using Dajunctic.SkillSystem.Data;
using UnityEngine;

namespace Dajunctic.SkillSystem.Logic
{
    public class SkillEntityGroup
    {
        public List<ISkillEntity> Skills;

        ISkillEntity _cachedSkill;
        List<ISkillEntity> _results = new();

        public ISkillEntity First()
        {
            return Skills.FirstOrDefault();
        }

        public ISkillEntity GetUsableSkill()
        {
            if (_cachedSkill != null && _cachedSkill.IsUsable)
            {
                return _cachedSkill;
            }

            _cachedSkill = null;

            _results.Clear();
            foreach (var skill in Skills)
            {
                if (skill.IsUsable)
                {
                    _results.Add(skill);
                }
            }

            if (_results.Count > 0)
            {
                _cachedSkill = _results[Random.Range(0, _results.Count)];
            }

            return _cachedSkill;
        }                

        public void ResetCache()
        {
            _cachedSkill = null;
        }

        public void UpdateCooldown(CooldownType type, float deltaTime)
        {
            foreach (var skill in Skills)
            {
                if (skill.LevelData.CooldownType == type)
                {
                    skill.UpdateCooldown(deltaTime);
                }
            }
        }
        
        public void SetLevelProvider(IAbilityLevelProvider levelProvider)
        {
            foreach (var skill in Skills)
            {
                skill.SetLevelProvider(levelProvider);
            }   
        }
        
        public void SetOwner(ISkillOwner owner)
        {
            foreach (var skill in Skills)
            {
                skill.SetOwner(owner);
            }   
        }

        public void ResetFields()
        {
            foreach (var skill in Skills)
            {
                skill.ResetFields();
            }
        }

        public void Stop()
        {
            foreach (var skill in Skills)
            {
                skill.Stop();
            }
        }

        public void Cleanup()
        {
            foreach (var skill in Skills)
            {
                skill.Cleanup();
            }
            _cachedSkill = null;
            _results.Clear();
        }

        public void ClearTarget()
        {
            foreach (var skill in Skills)
            {
                skill.ClearTarget();
            }
        }

        internal void ResetCooldown(bool isBeginCombat)
        {
            foreach (var skill in Skills)
            {
                skill.ResetCooldown(isBeginCombat);
            }
        }
    }
}
