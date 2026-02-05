using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Dajunctic
{
    public class Launcher: BaseView
    {
        [SerializeField] AssetReference homeScene;

        void Start()
        {
            AddressableUtils.LoadScene(homeScene);
        }
    }
}