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

        /// <summary>true = fully open (user explicitly opened, input active); false = fully closed.</summary>
        private bool _isOpen;

        /// <summary>true = temporarily visible due to incoming message; will auto-hide.</summary>
        private bool _isAutoShowing;

        private Coroutine _autoHideCoroutine;

        public override void Initialize()
        {
            base.Initialize();
            
            inputField.onSelect.AddListener(_ => OnInputFocus(true));
            inputField.onDeselect.AddListener(_ => OnInputFocus(false));
            inputField.onSubmit.AddListener(OnSubmit);

            // Start closed
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
                    // Hoàn toàn ẩn → mở ra
                    OpenManually();
                }
                else if (string.IsNullOrWhiteSpace(inputField.text))
                {
                    // Đang hiển thị (bất kỳ dạng nào) + Enter + không có text → ẩn đi
                    CloseChat();
                }
                else if (!_isOpen)
                {
                    // Auto-showing và có text → mở để gõ tiếp
                    OpenManually();
                }
                // else: đang mở + focused + có text → OnSubmit xử lý
            }
        }

        private void FocusChat()
        {
            inputField.ActivateInputField();
            inputField.Select();
        }

        // ── Message received ─────────────────────────────────────────────────

        private void OnMessageReceived(ChatMessageEvent evt)
        {
            AddMessage(evt.Message);

            // Auto-show when closed and the message is from another player
            // (skip System messages so the welcome message doesn't force-open the UI)
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

        // ── Auto-show (read-only, timed) ─────────────────────────────────────

        private void AutoShow()
        {
            _isAutoShowing = true;
            UpdateLayout();

            // Reset timer on each new incoming message
            if (_autoHideCoroutine != null) StopCoroutine(_autoHideCoroutine);
            _autoHideCoroutine = StartCoroutine(AutoHideCoroutine());
        }

        private IEnumerator AutoHideCoroutine()
        {
            yield return new WaitForSeconds(autoHideDelay);

            // Only hide if the user hasn't manually opened the chat
            if (!_isOpen)
            {
                _isAutoShowing = false;
                UpdateLayout();
            }
            _autoHideCoroutine = null;
        }

        // ── Manual open ──────────────────────────────────────────────────────

        private void OpenManually()
        {
            // Cancel auto-hide — user is taking control
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

        // ── Submit (Enter key while focused) ─────────────────────────────────

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

        // ── Layout ───────────────────────────────────────────────────────────

        private void UpdateLayout()
        {
            bool visible = _isOpen || _isAutoShowing;

            chatGroup.alpha = visible ? 1f : 0f;

            // Input is only interactive when the user explicitly opened the chat
            chatGroup.interactable = _isOpen;
            chatGroup.blocksRaycasts = _isOpen;
        }
    }
}
