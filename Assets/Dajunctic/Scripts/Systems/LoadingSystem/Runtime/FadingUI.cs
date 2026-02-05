using Dajunctic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Dajunctic{
    public class FadingUI: BaseView
    {
        public GameObject loadingUI;

        public override void ListenEvents()
        {
            base.ListenEvents();
            this.RegisterListener<ShowFadingUIEvent>(param => OnShow());
            this.RegisterListener<HideFadingUIEvent>(param => OnHide());
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        public override void StopListenEvents()
        {
            base.StopListenEvents();
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }

        void OnShow()
        {
            loadingUI.SetActive(true);
        }

        void OnHide()
        {
            loadingUI.SetActive(false);
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene != gameObject.scene) OnHide();
        }

        void OnSceneUnloaded(Scene scene)
        {
            if (scene != gameObject.scene) OnShow();
        }

    }

    public class ShowFadingUIEvent: IEvent
    {
        
    }

    public class HideFadingUIEvent: IEvent
    {
        
    }
}