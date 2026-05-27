namespace Dajunctic
{
    public interface IActionNodeSystem
    { 
        void Despawn(SkillSystem.Logic.IActionNode node);
        SkillSystem.Logic.IActionNode[] CreateActionNodes(object graph, object nodes = null);
    }
}
