
namespace Dajunctic
{
    public class DashDirectionSetting
    {
        
    }

    public class DashDirectionBehaviour: SkillBehaviour<DashDirectionSetting>
    {
        public override void Execute(CombatActor combatActor, SkillTrackContext context)
        {
            base.Execute(combatActor, context);
            
        }
    
    }
}