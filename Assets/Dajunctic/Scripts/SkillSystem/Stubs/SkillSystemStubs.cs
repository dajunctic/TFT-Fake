using System;

namespace Dajunctic.SkillSystem.Data
{
    public class StaticData
    {
        public void SetLocalizeString(string s) {}
    }

    public class LevelData
    {
        public GraphProcessor.BaseGraph Graph { get; set; }
        public void SetGraph(object graph) {}
        public void SetProperties(object props) {}
        public void SetLocalizeString(string s) {}
        public void SetSmartString() {}
    }
}

namespace Dajunctic.SkillSystem.Constants
{
    public static class PhysicsConstants
    {
        public const float Gravity = 9.8f;
    }
}
