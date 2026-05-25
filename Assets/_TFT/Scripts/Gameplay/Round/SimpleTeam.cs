using System.Collections.Generic;

namespace Dajunctic
{
    /// <summary>
    /// ICombatTeam wrapper đơn giản dùng cho PvE wave.
    /// Champion nhận team này làm EnemyTeam để BT tìm được DummyActor.
    /// </summary>
    public class SimpleTeam : ICombatTeam
    {
        private readonly List<IDamageTaker> _members = new List<IDamageTaker>();

        public bool IsInitialized => _members.Count > 0;
        public List<IDamageTaker> Members => _members;

        public void Add(IDamageTaker member)
        {
            if (!_members.Contains(member))
                _members.Add(member);
        }

        public void Remove(IDamageTaker member) => _members.Remove(member);

        public void Clear() => _members.Clear();
    }
}
