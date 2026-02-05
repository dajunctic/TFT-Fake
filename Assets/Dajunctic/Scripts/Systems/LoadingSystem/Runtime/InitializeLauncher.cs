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
        [SerializeField] AssetReference fadingScene;

        void Awake()
        {
            ticker.Initialize();
            var dispatcher = Instantiate(eventDispatcher);
            dispatcher.name = "EventDispatcher";

            ServiceLocator.Register(ticker);
            ServiceLocator.Register(dispatcher);
            
        }

        async void Start()
        {
            await Addressables.LoadSceneAsync(fadingScene, LoadSceneMode.Single).Task;
            this.Raise(new ShowFadingUIEvent());
            
            AddressableUtils.LoadScene(launcherScene);
           
        }
    }
}