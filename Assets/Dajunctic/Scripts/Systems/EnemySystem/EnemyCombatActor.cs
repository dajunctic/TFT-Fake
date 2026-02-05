using System.Collections.Generic;
using Dajunctic;
using UnityEngine;

namespace Dajunctic
{
    public class EnemyCombatActor: CombatActor
    {
        [Header("Enemy")]
        [SerializeField] public Area area;
        public HexAreaView hexAreaView;

        public override string DataId => name;

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void SetupTree()
        {
            root = new Selector(new List<Node>()
            {
                CreateCombatBranch(),
                new PatrolNode(this, hexAreaView)
            });
        }
    }
}