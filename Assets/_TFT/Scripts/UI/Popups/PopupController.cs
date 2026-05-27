using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Dajunctic
{
    public class PopupController : BaseView
    {
        private static PopupController _instance;
        public static PopupController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<PopupController>();
                }
                return _instance;
            }
        }

        [SerializeField] private PopupControllerData data;
        [SerializeField] private RectTransform popupParent;

        private Dictionary<Type, BasePopup> popupDictionary = new Dictionary<Type, BasePopup>();
        private Stack<BasePopup> activePopups = new Stack<BasePopup>();

        protected override void Awake()
        {
            base.Awake();
            if (_instance == null)
            {
                _instance = this;
                ClearChildren();
                InitializeDictionary();
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }

        }

        private void ClearChildren()
        {
            if (popupParent == null) return;
            foreach (Transform child in popupParent)
            {
                Destroy(child.gameObject);
            }
        }

        private void InitializeDictionary()
        {
            if (data == null)
            {
                Debug.LogError("[PopupController] PopupControllerData is NULL! Please assign it in the Inspector.");
                return;
            }
            if (data.Prefabs == null || data.Prefabs.Count == 0)
            {
                Debug.LogError("[PopupController] PopupControllerData.Prefabs list is empty! Add popup prefabs to it in the Inspector.");
                return;
            }

            foreach (var prefab in data.Prefabs)
            {
                if (prefab != null)
                {
                    popupDictionary[prefab.GetType()] = prefab;
                    Debug.Log($"[PopupController] Registered popup: {prefab.GetType().Name}");
                }
            }
        }

        public override void ListenEvents()
        {
            base.ListenEvents();
            this.RegisterListener<ShowPopupEvent>(OnShowPopupRequested);
        }

        public override void StopListenEvents()
        {
            base.StopListenEvents();
            this.RemoveListener<ShowPopupEvent>(OnShowPopupRequested);
        }

        private void OnShowPopupRequested(ShowPopupEvent evt)
        {
            ShowPopup(evt.PopupType, evt.ShowMode, evt.Data);
        }

        public T ShowPopup<T>(PopupShowMode showMode = PopupShowMode.DoNothing, object data = null) where T : BasePopup
        {
            return ShowPopup(typeof(T), showMode, data) as T;
        }

        public BasePopup ShowPopup(Type type, PopupShowMode showMode = PopupShowMode.DoNothing, object data = null)
        {
            
            foreach (var active in activePopups)
            {
                if (active != null && active.GetType() == type)
                {
                    
                    active.BeforeShow(data);
                    active.AfterShow();
                    return active;
                }
            }

            if (!popupDictionary.TryGetValue(type, out var prefab))
            {
                string registered = popupDictionary.Count > 0
                    ? string.Join(", ", popupDictionary.Keys.Select(k => k.Name))
                    : "(none)";
                Debug.LogError($"[PopupController] Popup '{type.Name}' not found in PopupControllerData!\n" +
                               $"→ FIX: Open the PopupController GameObject in the Inspector, find the 'Data' (PopupControllerData) asset, " +
                               $"and add the '{type.Name}' prefab to the 'Prefabs' list.\n" +
                               $"Currently registered: {registered}");
                return null;
            }

            switch (showMode)
            {
                case PopupShowMode.DismissCurrent:
                    DismissPopup();
                    break;
                case PopupShowMode.DismissAll:
                    DismissAll();
                    break;
                case PopupShowMode.PauseCurrent:
                    if (activePopups.Count > 0)
                    {
                        var top = activePopups.Peek();
                        top.BeforePause();
                        top.gameObject.SetActive(false);
                        top.AfterPause();
                    }
                    break;
                case PopupShowMode.DoNothing:
                default:
                    
                    break;
            }

            return InstantiatePopup(prefab, data);
        }

        private BasePopup InstantiatePopup(BasePopup prefab, object data)
        {
            BasePopup instance = Instantiate(prefab, popupParent);
            instance.gameObject.SetActive(false);

            instance.BeforeShow(data);
            
            instance.gameObject.SetActive(true);
            activePopups.Push(instance);
            
            instance.AfterShow();

            return instance;
        }

        public void DismissPopup()
        {
            if (activePopups.Count == 0) return;

            BasePopup popup = activePopups.Pop();
            
            popup.BeforeDismiss();
            popup.gameObject.SetActive(false);
            popup.AfterDismiss();
            
            Destroy(popup.gameObject);

            if (activePopups.Count > 0)
            {
                var top = activePopups.Peek();
                if (!top.gameObject.activeSelf)
                {
                    top.BeforeResume();
                    top.gameObject.SetActive(true);
                    top.AfterResume();
                }
            }
        }

        public void DismissAll()
        {
            while (activePopups.Count > 0)
            {
                DismissPopup();
            }
        }
    }
}
