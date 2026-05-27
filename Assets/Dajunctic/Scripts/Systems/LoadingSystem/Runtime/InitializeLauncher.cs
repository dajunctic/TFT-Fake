using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Dajunctic
{
    public class InitializeLauncher: MonoBehaviour
    {
        [SerializeField] Ticker ticker;
        [SerializeField] Pool pool;
        [SerializeField] EventDispatcher eventDispatcher;
        [SerializeField] AssetReference launcherScene;

        void Awake()
        {
            ticker.Initialize();
            eventDispatcher.Initialize();
            pool.Initialize();

            ServiceLocator.Register(ticker);
            ServiceLocator.Register(eventDispatcher);
            ServiceLocator.Register(pool);
            
        }

        void Start()
        {
            
            SceneManager.LoadSceneAsync("Dummy", LoadSceneMode.Additive).completed += (op) =>
            {
                AddressableUtils.LoadScene(launcherScene);
            };
        }
    }
}
