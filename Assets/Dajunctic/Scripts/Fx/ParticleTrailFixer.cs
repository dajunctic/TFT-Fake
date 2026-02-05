using Dajunctic.Utils;
using UnityEngine;

namespace Dajunctic.Common
{
    public class ParticleTrailFixer : MonoBehaviour
    {
        ParticleSystem _particle;
        
        void OnEnable()
        {
            _particle = GetComponent<ParticleSystem>();
            if (_particle)
            {
                _particle.Clear();
                _particle.Stop();
                this.DelayFrame(1, () =>
                {
                    _particle.Play();
                });
            }
        }

        void OnDisable()
        {
            if (_particle)
            {
                _particle.Clear();
                _particle.Stop();
            }
        }
    }
}