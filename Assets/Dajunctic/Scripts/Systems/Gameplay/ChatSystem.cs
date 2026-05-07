using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Dajunctic
{
    /// <summary>
    /// Local message store and broadcaster.
    /// Does NOT listen to RequestSendMessageEvent — that is handled by ChatNetworkBridge
    /// which routes the message through the server so all clients receive it.
    /// </summary>
    public class ChatSystem : MonoBehaviour, IGameSystem
    {
        private List<ChatMessage> _messageHistory = new List<ChatMessage>();

        public Task LoadDataAsync() => Task.CompletedTask;

        public void Initialize(GameSystemManager manager)
        {
            // Welcome message visible to everyone once the chat is open
            AddSystemMessage("Chào mừng đến với TFT Fake! Nhấn Enter để chat.");
        }

        public void Shutdown()
        {
            _messageHistory.Clear();
        }

        /// <summary>
        /// Called by ChatNetworkBridge.RpcReceiveMessage (on ALL clients) to display a message.
        /// Also used for system messages.
        /// </summary>
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

            // Keep history bounded
            if (_messageHistory.Count > 100)
                _messageHistory.RemoveAt(0);

            // ChatUI listens to this event
            this.Raise(new ChatMessageEvent { Message = message });
        }

        public void AddSystemMessage(string content)
            => SendMessage("System", content, ChatMessageType.System);

        public List<ChatMessage> GetHistory() => _messageHistory;
    }
}
