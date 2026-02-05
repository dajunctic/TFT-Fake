using System;
using UnityEngine;

namespace Dajunctic
{
    public class AnimatorEventListener: MonoBehaviour
    {
        private CombatActor _actor;

        private void Awake()
        {
            _actor = GetComponentInParent<CombatActor>();
        }

        public void OnAnimTrigger(string param)
        {
        }
    }
}