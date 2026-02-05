using KBCore.Refs;
using UnityEngine;

namespace Dajunctic
{
    public class FxView : BaseView
    {
        [SerializeField, Child] ParticleSystem particle;
        private float _timer = 0;
        private float _maxDuration = -1;
        private bool _isInitialized = false;

        public void Play(PlayFxData data)
        {
            transform.position = data.Position;
            _maxDuration = data.duration;
            _timer = 0;
            _isInitialized = true;
            
            particle.Play();
        }

        public override void Tick()
        {
            base.Tick();

            if (!_isInitialized) return;

            if (_maxDuration > 0)
            {
                _timer += Time.deltaTime;
                if (_timer >= _maxDuration)
                {
                    DestroyFx();
                }
            }
            else if (_maxDuration == -1)
            {
                if (!particle.IsAlive(true))
                {
                    DestroyFx();
                }
            }
        }

        private void DestroyFx()
        {
            _isInitialized = false;
            Destroy(gameObject); 
        }
    }

    public class PlayFxData
    {
        public Vector3 Position;
        public float duration = -1;
    }

}