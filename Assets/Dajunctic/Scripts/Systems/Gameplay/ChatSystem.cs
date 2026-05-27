using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Dajunctic
{

    public class ChatSystem : MonoBehaviour, IGameSystem
    {
        private List<ChatMessage> _messageHistory = new List<ChatMessage>();

        public Task LoadDataAsync() => Task.CompletedTask;

        public void Initialize(GameSystemManager manager)
        {
            
            AddSystemMessage("Chào mừng đến với TFT Fake! Nhấn Enter để chat.");
        }

        public void Shutdown()
        {
            _messageHistory.Clear();
        }

        public void SendMessage(string sender, string content, ChatMessageType type = ChatMessageType.Global)
        {
            if (string.IsNullOrWhiteSpace(content)) return;

            var message = new ChatMessage
            {
                Sender = sender,
                Content = content,
                Timestamp = DateTime.Now,
                Type = type
            };

            _messageHistory.Add(message);

            if (_messageHistory.Count > 100)
                _messageHistory.RemoveAt(0);

            this.Raise(new ChatMessageEvent { Message = message });
        }

        public void AddSystemMessage(string content)
            => SendMessage("System", content, ChatMessageType.System);

        public List<ChatMessage> GetHistory() => _messageHistory;
    }
}
