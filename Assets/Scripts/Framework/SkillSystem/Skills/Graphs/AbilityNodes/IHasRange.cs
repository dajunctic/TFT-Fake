namespace Dajunctic.SkillSystem.Logic
{
    /// <summary>
    /// Interface for ability nodes that define a targeting range.
    /// Used by UseSkillGambitAction to determine how close the actor
    /// needs to move before casting the skill.
    /// </summary>
    public interface IHasRange
    {
        float GetRange();
    }
}
