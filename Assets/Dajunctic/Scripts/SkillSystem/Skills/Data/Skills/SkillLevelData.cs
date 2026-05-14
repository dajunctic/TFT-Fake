using System;
using System.Linq;
using UnityEngine;

namespace Dajunctic.SkillSystem.Data
{
    [Serializable]
    public class SkillLevelData : AbilityLevelData
    {
        [SerializeField] CooldownType cooldownType;        
        [SerializeField] float[] cooldown = { -1f };
        [SerializeField] float[] initialCooldown = { 0 };
        [SerializeField] float initialEnergy = 200;
        [SerializeField] int maxPlayCount = -1;

        public CooldownType CooldownType => cooldownType;
        public float InitialEnergy => initialEnergy;
        public int MaxPlayCount => maxPlayCount;

        public override T CreateCopy<T>()
        {
            var copy = base.CreateCopy<SkillLevelData>();
            
            copy.cooldownType = cooldownType;
            copy.cooldown = cooldown.ToArray();
            copy.initialCooldown = initialCooldown.ToArray();
            copy.initialEnergy = initialEnergy;
            copy.maxPlayCount = maxPlayCount;

            return copy as T;
        }
        
        public float GetCooldown(int level)
        {
            if (cooldown.Length == 0)
            {
                return -1;
            }
            return cooldown[Mathf.Clamp(level, 0, cooldown.Length - 1)];
        }

        public float GetInitialCooldown(int level)
        {
            if (initialCooldown.Length == 0)
            {
                return 0;
            }

            return initialCooldown[Mathf.Clamp(level, 0, initialCooldown.Length - 1)];
        }
    }
}
