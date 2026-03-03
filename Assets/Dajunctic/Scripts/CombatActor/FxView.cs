using KBCore.Refs;
using UnityEngine;

namespace Dajunctic
{
    public class FxView : BaseView
    {
        [SerializeField, Child] ParticleSystem particle;
        private float _timer = 0;
        private float _maxDuration = -1;
        private bool _isPlayed = false;

        public void Play(SpawnFxEvent data)
        {
            transform.position = data.position;
            _maxDuration = data.duration;
            _timer = 0;
            _isPlayed = true;

            particle.Play();
        }

        public override void Tick()
        {
            base.Tick();

            if (!_isPlayed) return;

            if (_maxDuration > 0)
            {
                _timer += DeltaTime;
                if (_timer >= _maxDuration)
                {
                    DestroyFx();
                }
            }
            else if (_maxDuration == -1)
            {
                if (particle == null || !particle.IsAlive(true))
                {
                    DestroyFx();
                }
            }
        }

        private void DestroyFx()
        {
            _isPlayed = false;

            if (Application.isPlaying)
            {
                Destroy(gameObject);
            }
            else
            {
                DestroyImmediate(gameObject);
            }
        }
    }

    public class SpawnFxEvent : IEvent
    {
        public string id;
        public Vector3 position;
        public Quaternion rotation;
        public float duration = -1;
    }

}