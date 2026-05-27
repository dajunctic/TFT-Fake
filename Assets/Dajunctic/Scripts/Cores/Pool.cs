using System;
using UnityEngine;

namespace Dajunctic
{
    [CreateAssetMenu(fileName = "Pool", menuName = "Dajunctic/Core/Pool")]
    public class Pool: ScriptableObject
    {
        [SerializeField] private PoolView poolMonoPrefab;

        public void Initialize() => Validate();

        private PoolView poolMono;

        private void Validate(Action onValidateCallback = null)
        {
            if (poolMono == null)
            {
                poolMono = Instantiate(poolMonoPrefab);
                poolMono.name = "[Pool]";
            }
            onValidateCallback?.Invoke();
        }
    }
}
