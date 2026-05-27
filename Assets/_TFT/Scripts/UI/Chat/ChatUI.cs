using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;

namespace Dajunctic
{
    public class ChatUI : BaseView
    {
        [Header("UI References")]
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private RectTransform messageContainer;
        [SerializeField] private GameObject messagePrefab;
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private CanvasGroup chatGroup;

        [Header("Settings")]
        [SerializeField] private int maxMessages = 50;

        [Header("Auto-Show Settings")]
        [Tooltip("Seconds the chat stays visible after receiving a message (when chat was closed and user hasn't opened it manually).")]
        [SerializeField] private float autoHideDelay = 5f;

        private List<ChatMessageUI> _activeMessages = new List<ChatMessageUI>();
        private bool _isFocused;

        private bool _isOpen;

        private bool _isAutoShowing;

        private Coroutine _autoHideCoroutine;

        public override void Initialize()
        {
            base.Initialize();
            
            inputField.onSelect.AddListener(_ => OnInputFocus(true));
            inputField.onDeselect.AddListener(_ => OnInputFocus(false));
            inputField.onSubmit.AddListener(OnSubmit);

            _isOpen = false;
            _isAutoShowing = false;
            UpdateLayout();
        }

        public override void ListenEvents()
        {
            base.ListenEvents();
            this.RegisterListener<ChatMessageEvent>(OnMessageReceived);
        }

        public override void StopListenEvents()
        {
            base.StopListenEvents();
            this.RemoveListener<ChatMessageEvent>(OnMessageReceived);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                bool isVisible = _isOpen || _isAutoShowing;

                if (!isVisible)
                {
                    
                    OpenManually();
                }
                else if (string.IsNullOrWhiteSpace(inputField.text))
                {
                    
                    CloseChat();
                }
                else if (!_isOpen)
                {
                    
                    OpenManually();
                }
                
            }
        }

        private void FocusChat()
        {
            inputField.ActivateInputField();
            inputField.Select();
        }

        private void OnMessageReceived(ChatMessageEvent evt)
        {
            AddMessage(evt.Message);

            if (!_isOpen && evt.Message.Type != ChatMessageType.System)
            {
                AutoShow();
            }
        }

        private void AddMessage(ChatMessage message)
        {
            var go = Instantiate(messagePrefab, messageContainer);
            var ui = go.GetComponent<ChatMessageUI>();
            ui.Setup(message);
            
            _activeMessages.Add(ui);

            if (_activeMessages.Count > maxMessages)
            {
                Destroy(_activeMessages[0].gameObject);
                _activeMessages.RemoveAt(0);
            }

            StartCoroutine(ScrollToBottomCoroutine());
        }

        private IEnumerator ScrollToBottomCoroutine()
        {
            yield return new WaitForEndOfFrame();
            
            if (scrollRect != null)
            {
                Canvas.ForceUpdateCanvases();
                scrollRect.verticalNormalizedPosition = 0f;
            }
        }

        private void AutoShow()
        {
            _isAutoShowing = true;
            UpdateLayout();

            if (_autoHideCoroutine != null) StopCoroutine(_autoHideCoroutine);
            _autoHideCoroutine = StartCoroutine(AutoHideCoroutine());
        }

        private IEnumerator AutoHideCoroutine()
        {
            yield return new WaitForSeconds(autoHideDelay);

            if (!_isOpen)
            {
                _isAutoShowing = false;
                UpdateLayout();
            }
            _autoHideCoroutine = null;
        }

        private void OpenManually()
        {
            
            if (_autoHideCoroutine != null)
            {
                StopCoroutine(_autoHideCoroutine);
                _autoHideCoroutine = null;
            }

            _isAutoShowing = false;
            _isOpen = true;
            UpdateLayout();
            FocusChat();
        }

        private void OnSubmit(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                CloseChat();
            }
            else
            {
                this.Raise(new RequestSendMessageEvent { Content = text });
                inputField.text = "";
                FocusChat();
            }
        }

        private void CloseChat()
        {
            if (_autoHideCoroutine != null)
            {
                StopCoroutine(_autoHideCoroutine);
                _autoHideCoroutine = null;
            }
            _isOpen = false;
            _isAutoShowing = false;
            inputField.text = "";
            inputField.DeactivateInputField();
            UpdateLayout();
        }

        private void OnInputFocus(bool focused)
        {
            _isFocused = focused;
        }

        private void UpdateLayout()
        {
            bool visible = _isOpen || _isAutoShowing;

            chatGroup.alpha = visible ? 1f : 0f;

            chatGroup.interactable = _isOpen;
            chatGroup.blocksRaycasts = _isOpen;
        }
    }
}
