using System;

namespace Dajunctic.SkillSystem.Data
{
    [Serializable]
    public class PassiveLevelData : AbilityLevelData
    {
        public override T CreateCopy<T>()
        {
            var copy = base.CreateCopy<PassiveLevelData>();
            return copy as T;
        }
    }
}
