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
        // [SerializeField] private float fadeOutTime = 5f;

        private List<ChatMessageUI> _activeMessages = new List<ChatMessageUI>();
        private bool _isFocused;
        private bool _isOpen; // Trạng thái đóng/mở tuyệt đối của Chat

        public override void Initialize()
        {
            base.Initialize();
            
            inputField.onSelect.AddListener(_ => OnInputFocus(true));
            inputField.onDeselect.AddListener(_ => OnInputFocus(false));
            inputField.onSubmit.AddListener(OnSubmit);

            // Khởi tạo ở trạng thái đóng
            _isOpen = false;
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
            // Chỉ xử lý phím Enter trong Update khi Chat đang ĐÓNG hoặc mất FOCUS
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (!_isOpen)
                {
                    // Đang đóng hoàn toàn -> Mở ra
                    _isOpen = true;
                    UpdateLayout();
                    FocusChat();
                }
                else if (!_isFocused)
                {
                    // Đang mở nhưng click ra ngoài -> Focus lại để gõ tiếp
                    FocusChat();
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

            // Đợi đến cuối frame để Layout Groups cập nhật kích thước rồi mới cuộn
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

        private void OnSubmit(string text)
        {
            // OnSubmit được gọi khi phím Enter được nhấn trong lúc đang focus
            if (string.IsNullOrWhiteSpace(text))
            {
                // Nếu ô text trống và nhấn Enter -> ĐÓNG CHAT
                _isOpen = false;
                inputField.text = "";
                inputField.DeactivateInputField();
                UpdateLayout();
            }
            else
            {
                // Nếu có text -> Gửi tin nhắn
                this.Raise(new RequestSendMessageEvent { Content = text });
                
                // Xóa text và giữ nguyên trạng thái mở, focus lại để gõ tiếp
                inputField.text = "";
                FocusChat();
            }
        }

        private void OnInputFocus(bool focused)
        {
            _isFocused = focused;
        }

        private void UpdateLayout()
        {
            if (_isOpen)
            {
                chatGroup.alpha = 1f;
                chatGroup.interactable = true;
                chatGroup.blocksRaycasts = true;
            }
            else
            {
                chatGroup.alpha = 0f;
                chatGroup.interactable = false;
                chatGroup.blocksRaycasts = false;
            }
        }
    }
}
