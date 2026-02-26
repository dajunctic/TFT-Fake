using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Dajunctic
{
    public class Launcher: BaseView
    {
        [SerializeField] AssetReference homeScene;
        [SerializeField] BaseApplication application;

        async void Start()
        {
            await Task.Run(() => SpinWait.SpinUntil(() => application.Initialized));
            AddressableUtils.LoadScene(homeScene);
        }
    }
}