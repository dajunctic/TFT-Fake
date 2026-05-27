using System;

namespace Dajunctic
{
    public struct ChatMessage
    {
        public string Sender;
        public string Content;
        public DateTime Timestamp;
        public ChatMessageType Type;
    }

    public enum ChatMessageType
    {
        Global,
        System,
        Whisper
    }

    public struct ChatMessageEvent : IEvent
    {
        public ChatMessage Message;
    }

    public struct RequestSendMessageEvent : IEvent
    {
        public string Content;
    }
}
