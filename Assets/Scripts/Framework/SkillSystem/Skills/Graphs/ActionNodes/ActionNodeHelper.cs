namespace Dajunctic.SkillSystem.Logic
{
    public static class ActionNodeHelper
    {
        public static void Play(this IActionNode[] arr, object source)
        {
            foreach (var node in arr)
            {
                node.Play(source);
            }
        }

        public static void OnMissileDespawn(this IActionNode[] arr, object source)
        {
            foreach (var action in arr)
            {
                if (action is IMissileActionNode missileAction)
                {
                    missileAction.OnMissileDespawn(source);
                }
            }
        }
    }
}
