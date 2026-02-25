using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Dajunctic
{
    public class InitializeLauncher: MonoBehaviour
    {
        [SerializeField] Ticker ticker;
        [SerializeField] EventDispatcher eventDispatcher;
        [SerializeField] AssetReference launcherScene;
        // [SerializeField] AssetReference fadingScene;

        void Awake()
        {
            ticker.Initialize();
            eventDispatcher.Initialize();

            ServiceLocator.Register(ticker);
            ServiceLocator.Register(eventDispatcher);
            
        }

        async void Start()
        {
            // if (fadingScene != null)
            // {
            //     await Addressables.LoadSceneAsync(fadingScene, LoadSceneMode.Single).Task;
            //     this.Raise(new ShowFadingUIEvent());
            // }
            
            AddressableUtils.LoadScene(launcherScene);
           
        }
    }
}