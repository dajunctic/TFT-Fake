using System.Collections.Generic;

namespace Dajunctic
{

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
