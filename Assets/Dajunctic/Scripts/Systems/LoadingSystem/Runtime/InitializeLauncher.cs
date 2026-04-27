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
        // [SerializeField] AssetReference fadingScene;

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
            // Load DummyScene trước để đảm bảo Unity luôn có ít nhất 2 scene (tránh lỗi Unloading last loaded scene)
            SceneManager.LoadSceneAsync("Dummy", LoadSceneMode.Additive).completed += (op) =>
            {
                AddressableUtils.LoadScene(launcherScene);
            };
        }
    }
}