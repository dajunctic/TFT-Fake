using System.Collections;
using UnityEngine;

namespace Dajunctic
{
    public class BaseApplication : MonoBehaviour, IApplication, ILifeCycle
    {
        public bool Initialized => _initialized;
        private bool _initialized;

        void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            IApplication.Instance = this;
            DontDestroyOnLoad(gameObject);

            _initialized = true;
        }

        public void Cleanup()
        {
            
        }
    }
}