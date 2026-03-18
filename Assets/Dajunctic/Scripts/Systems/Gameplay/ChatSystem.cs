using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Dajunctic
{
    public class ChatSystem : MonoBehaviour, IGameSystem
    {
        private List<ChatMessage> _messageHistory = new List<ChatMessage>();
        private GameSystemManager _manager;

        public Task LoadDataAsync()
        {
            // No SO data needed yet for chat, but we can load config here if needed
            return Task.CompletedTask;
        }

        public void Initialize(GameSystemManager manager)
        {
            _manager = manager;
            this.RegisterListener<RequestSendMessageEvent>(OnRequestSendMessage);
            
            // Welcome message
            AddSystemMessage("Chào mừng bạn đến với TFT Fake! Chúc bạn chơi game vui vẻ.");
        }

        public void Shutdown()
        {
            this.RemoveListener<RequestSendMessageEvent>(OnRequestSendMessage);
        }

        private void OnRequestSendMessage(RequestSendMessageEvent evt)
        {
            if (string.IsNullOrWhiteSpace(evt.Content)) return;

            // In a real game, Sender would come from the PlayerSystem
            string sender = _manager.Player != null ? "Me" : "Player";
            
            SendMessage(sender, evt.Content);
        }

        public void SendMessage(string sender, string content, ChatMessageType type = ChatMessageType.Global)
        {
            var message = new ChatMessage
            {
                Sender = sender,
                Content = content,
                Timestamp = DateTime.Now,
                Type = type
            };

            _messageHistory.Add(message);
            
            // Limit history
            if (_messageHistory.Count > 100)
                _messageHistory.RemoveAt(0);

            this.Raise(new ChatMessageEvent { Message = message });
        }

        public void AddSystemMessage(string content)
        {
            SendMessage("Hệ thống", content, ChatMessageType.System);
        }

        public List<ChatMessage> GetHistory() => _messageHistory;
    }
}
