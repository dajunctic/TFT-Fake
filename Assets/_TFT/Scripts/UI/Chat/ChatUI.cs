using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

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
        [SerializeField] private float fadeOutTime = 5f;

        private List<ChatMessageUI> _activeMessages = new List<ChatMessageUI>();
        private float _lastMessageTime;
        private bool _isFocused;

        public override void Initialize()
        {
            base.Initialize();
            
            inputField.onSelect.AddListener(_ => OnInputFocus(true));
            inputField.onDeselect.AddListener(_ => OnInputFocus(false));
            inputField.onSubmit.AddListener(OnSubmit);

            // Initially hide or fade
            UpdateVisibility();
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
            // Enter key to focus chat
            if (Input.GetKeyDown(KeyCode.Return) && !_isFocused)
            {
                inputField.ActivateInputField();
            }

            // Auto fade out if not focused and no recent activity
            if (!_isFocused && Time.time - _lastMessageTime > fadeOutTime)
            {
                chatGroup.alpha = Mathf.Lerp(chatGroup.alpha, 0.2f, Time.deltaTime);
            }
            else
            {
                chatGroup.alpha = Mathf.Lerp(chatGroup.alpha, 1.0f, Time.deltaTime * 5f);
            }
        }

        private void OnMessageReceived(ChatMessageEvent evt)
        {
            AddMessage(evt.Message);
            _lastMessageTime = Time.time;
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

            // Scroll to bottom
            Canvas.ForceUpdateCanvases();
            scrollRect.verticalNormalizedPosition = 0f;
        }

        private void OnSubmit(string text)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                this.Raise(new RequestSendMessageEvent { Content = text });
            }

            inputField.text = "";
            
            // Re-focus unless escape was pressed (handled by Unity's InputField usually)
            if (Input.GetKey(KeyCode.Return))
            {
                inputField.ActivateInputField();
            }
        }

        private void OnInputFocus(bool focused)
        {
            _isFocused = focused;
            UpdateVisibility();
        }

        private void UpdateVisibility()
        {
            // When focused, background should be more visible
            // Implement background alpha change here if desired
        }
    }
}
